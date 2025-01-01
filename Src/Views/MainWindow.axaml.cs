using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.ComponentModel;
using System.Diagnostics;
using Tsundoku.Models;
using Tsundoku.ViewModels;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Avalonia.Platform.Storage;
using Avalonia.Media.Imaging;
using FileWatcherEx;
using Avalonia.Media;
using Tsundoku.Helpers;
using Avalonia.Controls.Primitives;

namespace Tsundoku.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        WindowState previousWindowState;
        private static readonly List<Series> CoverChangedSeriesList = [];
        private static readonly FileSystemWatcherEx CoverFolderWatcher = new FileSystemWatcherEx(@"Covers")
        {
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.FileName,
        };
        private static StringBuilder newSearchText = new StringBuilder();
        private static string itemString;
        private static readonly TimeSpan AdvancedSearchPopulateDelay = new TimeSpan(TimeSpan.TicksPerSecond / 4);

        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(action => 
                action(ViewModel!.EditSeriesInfoDialog.RegisterHandler(DoShowEditSeriesInfoDialogAsync)));

            SetupAdvancedSearchBar(" & ");

            CoverFolderWatcher.OnCreated += (s, e) =>
            {
                string path = e.FullPath.Replace(".crdownload", "");
                LOGGER.Debug("{} | {} | {} | {}", path, !ViewModelBase.newCoverCheck, path.EndsWith("jpg", StringComparison.OrdinalIgnoreCase), path.EndsWith("png", StringComparison.OrdinalIgnoreCase));
                if (!ViewModelBase.newCoverCheck && (path.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || path.EndsWith("png", StringComparison.OrdinalIgnoreCase)))
                {
                    string pathFileExtension = path[^3..];
                    Series series = MainWindowViewModel.SearchedCollection.AsParallel().SingleOrDefault(series => series.Cover.Equals(path)) ?? MainWindowViewModel.UserCollection.AsParallel().SingleOrDefault(series => series.Cover.Equals(path));
                    
                    if (series == null)
                    { 
                        string newPath = path[..^3] + (pathFileExtension.Equals("jpg") ? "png" : "jpg");
                        series = MainWindowViewModel.SearchedCollection.AsParallel().SingleOrDefault(series => series.Cover.Equals(newPath)) ?? MainWindowViewModel.UserCollection.AsParallel().SingleOrDefault(series => series.Cover.Equals(newPath));
                    }

                    if (!series.Cover[^3..].Equals(pathFileExtension))
                    {
                        series.Cover = series.Cover[..^3] + pathFileExtension;
                        File.Delete(series.Cover[..^3] + (pathFileExtension.Equals("jpg") ? "png" : "jpg"));
                        LOGGER.Info("Changed File Extention for {} to {}", series.Titles["Romaji"], pathFileExtension);
                    }

                    if (!CoverChangedSeriesList.Contains(series))
                    {
                        CoverChangedSeriesList.Add(series);
                        LOGGER.Info($"Added \"{series.Titles["Romaji"]}\" to Cover Change List");
                    }
                    else
                    {
                        LOGGER.Info($"\"{series.Titles["Romaji"]}\" Cover Changed Again");
                    }
                }
                ViewModelBase.newCoverCheck = false;
            };
            CoverFolderWatcher.Start();

            KeyDown += async (s, e) => 
            {
                if (e.Key == Key.F11) 
                {
                    if (WindowState != WindowState.FullScreen)
                    {
                        previousWindowState = WindowState;
                        WindowState = WindowState.FullScreen;
                        await ToggleNotificationPopup("To Exit Fullscreen Press F11");
                    }
                    else if (WindowState == WindowState.FullScreen)
                    {
                        WindowState = previousWindowState;
                    }
                }
                else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.P)
                {
                    LOGGER.Info($"Saving Screenshot of Collection for \"{ViewModel.CurrentTheme.ThemeName} Theme\"");
                    ScreenCaptureWindows();
                    await ToggleNotificationPopup($"Saved Screenshot for \"{ViewModel.CurrentTheme.ThemeName}\" Theme");
                }
                else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.S)
                {
                    ViewModel.SearchText = "";
                    ViewModelBase.MainUser.SaveUserData();
                    await ToggleNotificationPopup($"Saved \"{ViewModel.UserName}'s\" Data");
                }
                else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.R)
                {
                    LOGGER.Info("Reloading Covers");
                    if (CoverChangedSeriesList.Count != 0)
                    {
                        ReloadCoverBitmaps();
                    }
                    else
                    {
                        LOGGER.Debug("No Covers Reloaded");
                    }
                    await ToggleNotificationPopup($"Reloaded Covers");
                }
                else if (e.KeyModifiers == KeyModifiers.Shift && e.Key == Key.R)
                {
                    LOGGER.Info("Reloading Filter/Sort on Collection");
                    ViewModel.FilterCollection(ViewModel.CurFilter);
                    await ToggleNotificationPopup($"Reloaded \"{ViewModel.CurFilter}\" Filter/Sort on Collection");
                }
                else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.F)
                {
                    if (AdvancedSearchPopup.IsVisible)
                    {
                        AdvancedSearchBar.MinimumPopulateDelay = TimeSpan.Zero;
                        AdvancedSearchBar.Text = string.Empty;
                        AdvancedSearchBar.MinimumPopulateDelay = AdvancedSearchPopulateDelay;
                        AdvancedSearchPopup.IsVisible ^= true;
                    }
                    else
                    {
                        AdvancedSearchPopup.IsVisible ^= true;
                        await ToggleNotificationPopup($"To Exit Advanced Search Press CTRL+F");
                    }
                }
            };

            Closing += (s, e) => { SaveOnClose(ViewModelBase.isReloading); };
        }

        private async Task DoShowEditSeriesInfoDialogAsync(IInteractionContext<EditSeriesInfoViewModel, MainWindowViewModel?> interaction)
        {
            var dialog = new EditSeriesInfoWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<MainWindowViewModel?>(this);
            interaction.SetOutput(result);
        }

        private async Task ToggleNotificationPopup(string notiText)
        {
            ViewModel.NotificationText = notiText;
            NotificationPopup.IsVisible = true;
            await Task.Delay(TimeSpan.FromSeconds(3));
            NotificationPopup.IsVisible = false;
        }

        private void ToggleSeriesFavorite(object sender, RoutedEventArgs args)
        {
            if (ViewModel.CurFilter == TsundokuFilter.Favorites && !((Series)((Button)sender).DataContext).IsFavorite)
            {
                ViewModel.FilterCollection(TsundokuFilter.Favorites);
            }
        }

        public static void ResetMenuButton(ToggleButton menuButton)
        {
            menuButton.IsChecked = false;
        }

        private void CloseNotificationPopup(object sender, RoutedEventArgs args) => NotificationPopup.IsVisible = false;

        private void OpenAddSeriesDialog(object sender, RoutedEventArgs args)
        {
            AddNewSeriesButton.IsChecked = true;
            if (!MainWindowViewModel.newSeriesWindow.IsOpen)
            {
                MainWindowViewModel.newSeriesWindow.Show();
            }
            else
            {
                MainWindowViewModel.newSeriesWindow.WindowState = WindowState.Normal;
                MainWindowViewModel.newSeriesWindow.Activate();
            }
        }

        private void OpenSettingsDialog(object sender, RoutedEventArgs args)
        {
            SettingsButton.IsChecked = true;
            if (!MainWindowViewModel.settingsWindow.IsOpen)
            {
                MainWindowViewModel.settingsWindow.Show();
            }
            else
            {
                MainWindowViewModel.settingsWindow.WindowState = WindowState.Normal;
                MainWindowViewModel.settingsWindow.Activate();
            }
        }

        private void OpenCollectionStatsDialog(object sender, RoutedEventArgs args)
        {
            StatsButton.IsChecked = true;
            if (!MainWindowViewModel.collectionStatsWindow.IsOpen)
            {
                MainWindowViewModel.collectionStatsWindow.Show();
            }
            else
            {
                MainWindowViewModel.collectionStatsWindow.WindowState = WindowState.Normal;
                MainWindowViewModel.collectionStatsWindow.Activate();
            }
        }

        private void OpenPriceAnalysisDialog(object sender, RoutedEventArgs args)
        {
            AnalysisButton.IsChecked = true;
            if (!MainWindowViewModel.priceAnalysisWindow.IsOpen)
            {
                MainWindowViewModel.priceAnalysisWindow.Show();
            }
            else
            {
                MainWindowViewModel.priceAnalysisWindow.WindowState = WindowState.Normal;
                MainWindowViewModel.priceAnalysisWindow.Activate();
            }
        }

        private void OpenThemeSettingsDialog(object sender, RoutedEventArgs args)
        {
            ThemeButton.IsChecked = true;
            MainWindowViewModel.themeSettingsWindow.WindowState = WindowState.Normal;
            if (!MainWindowViewModel.themeSettingsWindow.IsOpen)
            {
                MainWindowViewModel.themeSettingsWindow.Show();
            }
            else
            {
                MainWindowViewModel.themeSettingsWindow.Activate();
            }
        }

        private void OpenUserNotesDialog(object sender, RoutedEventArgs args)
        {
            UserNotesButton.IsChecked = true;
            if (!MainWindowViewModel.userNotesWindow.IsOpen)
            {
                MainWindowViewModel.userNotesWindow.Show();
            }
            else
            {
                MainWindowViewModel.userNotesWindow.WindowState = WindowState.Normal;
                MainWindowViewModel.userNotesWindow.Activate();
            }
        }

        public void SetupAdvancedSearchBar(string delimiter)
        {
            AdvancedSearchBar.ItemsSource = ADVANCED_SEARCH_FILTERS;
            AdvancedSearchBar.FilterMode = AutoCompleteFilterMode.Custom;
            AdvancedSearchBar.MinimumPopulateDelay = AdvancedSearchPopulateDelay; // Might just remove this delay in the end
            AdvancedSearchBar.ItemSelector = (query, item) =>
            {
                newSearchText.Clear();
                if (MainWindowViewModel.AdvancedQueryRegex().IsMatch(query))
                {
                    newSearchText.Append(query[..query.LastIndexOf(delimiter)]).Append(delimiter);
                }
                return !item.Equals("Notes==") ? newSearchText.Append(item).ToString() : newSearchText.Append(item).ToString();
            };
            AdvancedSearchBar.ItemFilter = (query, item) =>
            {
                itemString = item as string;
                if (!MainWindowViewModel.AdvancedQueryRegex().IsMatch(query))
                {
                    return itemString.StartsWith(query, StringComparison.OrdinalIgnoreCase);
                }
                else if (query.Contains(itemString) || itemString.Last() != '=' && query.Contains(itemString[..itemString.IndexOf("==")]))
                {
                    return false;
                }
                
                string filterName = itemString[..^2];
                if (itemString.Contains("==") && (query.Contains($"{filterName}<=") || query.Contains($"{filterName}>=")))
                {
                    return false;
                }
                else if ((itemString.Contains('>') || itemString.Contains('<')) && query.Contains($"{filterName}=="))
                {
                    return false;
                }
 
                return AdvancedSearchBar.IsVisible && itemString.StartsWith(query[(query.LastIndexOf(delimiter) + delimiter.Length)..], StringComparison.OrdinalIgnoreCase);
            };
        }
        
        private void UnShowAdvancedSearchPopup(object sender, PointerPressedEventArgs args)
        {
            AdvancedSearchPopup.IsVisible = false;
        }

        private void RemoveErrorMessage(object sender, KeyEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.AdvancedSearchQueryErrorMessage))
            {
                ViewModel.AdvancedSearchQueryErrorMessage = string.Empty;
            }
        }   

        /// <summary>
        /// Reloads Covers for Series where the cover was changed
        /// </summary>
        private static void ReloadCoverBitmaps()
        {
            LOGGER.Debug("Reloading Cover Bitmaps");
            uint cache = 0;
            for (int x = 0; x < MainWindowViewModel.UserCollection.Count; x++)
            {
                // If the image does not exist in the covers folder then don't create a bitmap for it
                if (CoverChangedSeriesList.Contains(MainWindowViewModel.UserCollection[x]))
                {
                    MainWindowViewModel.ChangeCover(MainWindowViewModel.UserCollection[x], MainWindowViewModel.UserCollection[x].Cover);
                    cache++;
                }
                else if (cache == CoverChangedSeriesList.Count)
                {
                    break;
                }
            }
            CoverChangedSeriesList.Clear();
        }

        /// <summary>
        /// Takes a screenshot of the current collection window
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private void ScreenCaptureWindows()
        {
            Directory.CreateDirectory(@"Screenshots");
            using (System.Drawing.Bitmap bitmap = new((int)this.Width, (int)this.Height))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new System.Drawing.Point((int)this.Bounds.Left, (int)this.Bounds.Top), System.Drawing.Point.Empty, new System.Drawing.Size((int)this.Width, (int)this.Height));
                }
                bitmap.Save(@$"Screenshots\{ViewModel.UserName}-Collection-Screenshot-{ViewModel.CurrentTheme.ThemeName}-{ViewModel.CurLanguage}{(ViewModel.CurFilter != TsundokuFilter.None && ViewModel.CurFilter != TsundokuFilter.Query ? $"-{ViewModel.CurFilter.GetStringValue()}" : string.Empty)}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void SearchCollection(object sender, KeyEventArgs args) => MainWindowViewModel.UserIsSearching(true);

        private async void OpenEditSeriesInfoWindow(object sender, RoutedEventArgs args)
        {
            _ = await ViewModel!.EditSeriesInfoDialog.Handle(new EditSeriesInfoViewModel((Series)(sender as Button).DataContext, (Button)sender));
        }

        /// <summary>
        /// Changes the language for the users collection
        /// </summary>
        private void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).IsDropDownOpen)
            {
                if (!string.IsNullOrWhiteSpace(ViewModel.SearchText)) // Checks if the user is searching, filtering, or sorting
                {
                    // ViewModel.SearchIsBusy = false;
                    ViewModel.SearchText = string.Empty;
                }
                
                if (ViewModel.CurFilter != TsundokuFilter.None)
                {
                    ViewModel.CurFilter = TsundokuFilter.None;
                }
                
                // AddNewSeriesWindow.PreviousLanguage = ViewModel.CurLanguage;
                ViewModel.CurLanguage = (LanguageSelector.SelectedItem as ComboBoxItem).Content.ToString();
                LOGGER.Info($"Changed Langauge to \"{ViewModel.CurLanguage}\"");
                MainWindowViewModel.SortCollection();
            }
        }

        /// <summary>
        /// Changes the filter on the users collection
        /// </summary>
        private void CollectionFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).IsDropDownOpen)
            {
                ViewModel.UpdateCurFilter(CollectionFilterSelector.SelectedItem as ComboBoxItem);
            }
        }

        public void SaveOnClose(bool isReloading)
        {
            LOGGER.Info("Closing Tsundoku");
            ViewModel.SearchText = "";
            if (!isReloading) { ViewModelBase.MainUser.SaveUserData(); }
            Helpers.DiscordRP.Deinitialize();
            CoverFolderWatcher.Dispose();

            if (MainWindowViewModel.newSeriesWindow != null)
            {
                MainWindowViewModel.newSeriesWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.newSeriesWindow.Close();
            }

            if (MainWindowViewModel.settingsWindow != null)
            {
                MainWindowViewModel.settingsWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.settingsWindow.Close();
            }

            if (MainWindowViewModel.themeSettingsWindow != null)
            {
                MainWindowViewModel.themeSettingsWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.themeSettingsWindow.Close();
            }

            if (MainWindowViewModel.priceAnalysisWindow != null)
            {
                MainWindowViewModel.priceAnalysisWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.priceAnalysisWindow.Close();
            }

            if (MainWindowViewModel.collectionStatsWindow != null)
            {
                MainWindowViewModel.collectionStatsWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.collectionStatsWindow.Close();
            }

            if (MainWindowViewModel.userNotesWindow != null)
            {
                MainWindowViewModel.userNotesWindow.Closing += (s, e) => { e.Cancel = false; };
                MainWindowViewModel.userNotesWindow.Close();
            }

            NLog.LogManager.Shutdown();
            App.DisposeMutex();
        }

        /// <summary>
        /// Opens the AniList or MangaDex website link in the users browser when users clicks on the left side of the series card
        /// </summary>
        private async void OpenSiteLink(object sender, PointerPressedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(((sender as Canvas).DataContext as Series).Link.ToString());
        }
        
        private async void ChangeUserIcon(object sender, PointerPressedEventArgs args)
        {
            var file = await this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>() { new FilePickerFileType("Images") { Patterns = ["*.png", "*.jpg", "*.jpeg", "*.crdownload"] } }
            });
            if (file.Count > 0)
            {
                ViewModel.UserIcon = new Bitmap(file[0].Path.LocalPath).CreateScaledBitmap(new PixelSize(USER_ICON_WIDTH, USER_ICON_HEIGHT), BitmapInterpolationMode.HighQuality);
                LOGGER.Info($"Changed Users Icon to {file[0].Path.LocalPath}");
            }
        }

        /// <summary>
        /// Opens my PayPal for users to donate if they want to
        /// </summary>
        private void OpenDonationLink(object sender, RoutedEventArgs args)
        {
            LOGGER.Info($"Opening PayPal Donation Lionk");
            try
            {
                Process.Start(new ProcessStartInfo("https://www.paypal.com/donate/?business=JAYCVEJGDF4GY&no_recurring=0&item_name=Help+keep+Tsundoku+Supported+into+the+Future%21&currency_code=USD") { UseShellExecute = true });
            }
            catch (Win32Exception noBrowser)
            {
                LOGGER.Error(noBrowser.Message);
            }
            catch (Exception other)
            {
                LOGGER.Error(other.Message);
            }
        }

        /// <summary>
        /// Copies the title of the series when clicked om
        /// </summary>
        private async void CopySeriesTitleAsync(object sender, PointerPressedEventArgs args)
        {
            string title = ((TextBlock)sender).Text;
            LOGGER.Info("Copying {} to Clipboard", title);
            await TextCopy.ClipboardService.SetTextAsync(title);
        }

        /// <summary>
        /// Increments the series current volume count
        /// </summary>
        public void AddVolume(object sender, RoutedEventArgs args)
        {
            Series curSeries = (Series)((Button)sender).DataContext;
            if (curSeries.CurVolumeCount < curSeries.MaxVolumeCount)
            {
                MainWindowViewModel.collectionStatsWindow.ViewModel.UsersNumVolumesCollected++;
                MainWindowViewModel.collectionStatsWindow.ViewModel.UsersNumVolumesToBeCollected--;
                curSeries.CurVolumeCount += 1;
                TextBlock volumeDisplay = (TextBlock)((Button)sender).GetLogicalSiblings().ElementAt(1);
                string log = $"Adding 1 Volume To \"{curSeries.Titles["Romaji"]}\" : {volumeDisplay.Text} -> ";
                volumeDisplay.Text = curSeries.CurVolumeCount + "/" + curSeries.MaxVolumeCount;
                LOGGER.Info(log + volumeDisplay.Text);

                (sender as Button).FindLogicalAncestorOfType<Grid>(false).FindLogicalDescendantOfType<ProgressBar>(false).Value = curSeries.CurVolumeCount;
            }
        }

        /// <summary>
        /// Deccrements the series current volume count
        /// </summary>
        public void SubtractVolume(object sender, RoutedEventArgs args)
        {
            Series curSeries = (Series)((Button)sender).DataContext; //Get the current series data
            if (curSeries.CurVolumeCount >= 1) //Only decrement if the user currently has 1 or more volumes
            {
                MainWindowViewModel.collectionStatsWindow.ViewModel.UsersNumVolumesCollected--;
                MainWindowViewModel.collectionStatsWindow.ViewModel.UsersNumVolumesToBeCollected++;
                curSeries.CurVolumeCount -= 1;
                TextBlock volumeDisplay = (TextBlock)((Button)sender).GetLogicalSiblings().ElementAt(1);
                string log = $"Removing 1 Volume From \"{curSeries.Titles["Romaji"]}\" : {volumeDisplay.Text} -> ";
                volumeDisplay.Text = curSeries.CurVolumeCount + "/" + curSeries.MaxVolumeCount;
                LOGGER.Info(log + volumeDisplay.Text);

                (sender as Button).FindLogicalAncestorOfType<Grid>(false).FindLogicalDescendantOfType<ProgressBar>(false).Value = curSeries.CurVolumeCount;
            }
        }
    }
}
