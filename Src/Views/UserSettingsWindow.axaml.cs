using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Tsundoku.ViewModels;
using Avalonia.Controls;
using System.Diagnostics;
using ReactiveUI;
using Avalonia.Platform.Storage;
using System.Diagnostics.CodeAnalysis;
using Avalonia.ReactiveUI;

namespace Tsundoku.Views
{
    public partial class SettingsWindow : ReactiveWindow<UserSettingsViewModel>
    {
        public bool IsOpen = false;
        public int currencyLength = 0;
        private static readonly FilePickerFileType fileOptions = new FilePickerFileType("JSON File")
        {
            Patterns = [ "*.json" ]
        };
        private static readonly FilePickerOpenOptions filePickerOptions =  new FilePickerOpenOptions {
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>() { fileOptions }
        };
        MainWindow CollectionWindow;
        
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new UserSettingsViewModel();
            Opened += (s, e) =>
            {
                CollectionWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
                IsOpen ^= true;

                if (Screens.Primary.WorkingArea.Height < 955)
                {
                    this.Height = 550;
                }
            };

            Closing += (s, e) =>
            {
                if (IsOpen)
                {
                    ((SettingsWindow)s).Hide();
                    Topmost = false;
                    IsOpen ^= true;
                }
                e.Cancel = true;
            };

            this.WhenAnyValue(x => x.IndigoButton.IsChecked, (member) => member != null && member == true).Subscribe(x => ViewModel.IndigoMember = x);
            this.WhenAnyValue(x => x.BooksAMillionButton.IsChecked, (member) => member != null && member == true).Subscribe(x => ViewModel.BooksAMillionMember = x);
            this.WhenAnyValue(x => x.KinokuniyaUSAButton.IsChecked, (member) => member != null && member == true).Subscribe(x => ViewModel.KinokuniyaUSAMember = x);
        }

        public void CurrencyChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).IsDropDownOpen)
            {
                string newCurrency = (CurrencySelector.SelectedItem as ComboBoxItem).Content.ToString();
                currencyLength = CollectionWindow.ViewModel.CurCurrency.Length;
                CollectionWindow.ViewModel.CurCurrency = newCurrency;
                MainWindowViewModel.newSeriesWindow.ViewModel.CurCurrency = newCurrency;
                MainWindowViewModel.collectionStatsWindow.ViewModel.CollectionPrice = $"{newCurrency}{ MainWindowViewModel.collectionStatsWindow.ViewModel.CollectionPrice[currencyLength..]}";
                ViewModelBase.MainUser.Currency = newCurrency;
                LOGGER.Info($"Currency Changed To {newCurrency}");
            }
        }


        /// <summary>
        /// Allows user to upload a new Json file to be used as their new data, it additionall creates a backup file of the users last save
        /// </summary>
        [RequiresUnreferencedCode("Calls Tsundoku.ViewModels.MainWindowViewModel.VersionUpdate(JsonNode)")]
        private async void UploadUserData(object sender, RoutedEventArgs args)
        {
            var file = await this.StorageProvider.OpenFilePickerAsync(filePickerOptions);
            if (file.Count > 0)
            {
                UserSettingsViewModel.ImportUserData(file);
                ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).TryShutdown();
                Process.Start(@$"{AppDomain.CurrentDomain.BaseDirectory}\Tsundoku.exe");
            }
        }

        private async void OpenReleasesPage(object sender, PointerPressedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://github.com/Sigrec/Tsundoku/releases");
        }

        public async void OpenAniListLink(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://anilist.co/");
        }

        public async void OpenMangadexLink(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://mangadex.org/");
        }
        
        public async void OpenApplicationFolder(object sender, RoutedEventArgs args)
        {
            await Task.Run(() =>
            {
                Process.Start("explorer.exe", @$"{Path.GetDirectoryName(Path.GetFullPath(@"Covers"))}");
            });
        }

        public async void OpenCoversFolder(object sender, RoutedEventArgs args)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(@"Covers"))
                {
                    Directory.CreateDirectory(@"Covers");
                }
                Process.Start("explorer.exe", @"Covers");
            });
        }

        public async void OpenScreenshotsFolder(object sender, RoutedEventArgs args)
        {
            await Task.Run(() =>{
                if (!Directory.Exists(@"Screenshots"))
                {
                    Directory.CreateDirectory(@"Screenshots");
                }
                Process.Start("explorer.exe", @"Screenshots");
            });
        }

        public async void OpenThemesFolder(object sender, RoutedEventArgs args)
        {
            await Task.Run(() =>{
                if (!Directory.Exists(@"Themes"))
                {
                    Directory.CreateDirectory(@"Themes");
                }
                Process.Start("explorer.exe", @"Themes");
            });
        }

        public void ChangeUsername(object sender, RoutedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(UsernameChange.Text))
            {
                CollectionWindow.ViewModel.UserName = UsernameChange.Text;
                LOGGER.Info($"Username Changed To -> {UsernameChange.Text}");
            }
            else
            {
                LOGGER.Warn("Change Username Field is Missing Input");
            }
        }

        public async void OpenYoutuberSite(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@$"https://www.youtube.com/@{(sender as Button).Name}");
        }

        public async void OpenCoolorsSite(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://coolors.co/generate");
        }

        public async void JoinDiscord(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://discord.gg/QcZ5jcFPeU");
        }

        public async void ReportBug(object sender, RoutedEventArgs args)
        {
            await ViewModelBase.OpenSiteLink(@"https://github.com/Sigrec/Tsundoku/issues/new/choose");
        }
    }
}
