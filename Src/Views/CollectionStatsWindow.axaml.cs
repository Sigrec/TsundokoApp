using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Tsundoku.ViewModels;
using Avalonia.ReactiveUI;
using System.Reactive.Subjects;
using Avalonia.Media;
using SkiaSharp;
using Avalonia.Controls.ApplicationLifetimes;

namespace Tsundoku.Views
{
    public partial class CollectionStatsWindow : ReactiveWindow<CollectionStatsViewModel>
    {
        public bool IsOpen = false;
        public bool CanUpdate = true; // On First Update
        private MainWindow CollectionWindow;
        Subject<SolidColorBrush> UnknownRectangleColorSource = new Subject<SolidColorBrush>();
        Subject<SolidColorBrush> ManfraRectangleColorSource = new Subject<SolidColorBrush>();
        Subject<SolidColorBrush> ComicRectangleColorSource = new Subject<SolidColorBrush>();
        Subject<SolidColorBrush> DataBGColorSource = new Subject<SolidColorBrush>();

        public CollectionStatsWindow()
        {
            InitializeComponent();
            DataContext = new CollectionStatsViewModel();
            Opened += (s, e) =>
            {
                CollectionWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;
                
                _ = UnknownRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, UnknownRectangleColorSource);
                UnknownRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));

                _ = ComicRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, ComicRectangleColorSource);
                ComicRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));

                _ = ManfraRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, ManfraRectangleColorSource);
                ManfraRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardStaffColor == ViewModel.CurrentTheme.SeriesCardTitleColor ? ViewModel.CurrentTheme.SeriesEditPaneButtonsIconColor : ViewModel.CurrentTheme.SeriesCardStaffColor));

                _ = StatsBG.Bind(Avalonia.Controls.Border.BackgroundProperty, DataBGColorSource);
                DataBGColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardBGColor == ViewModel.CurrentTheme.MenuBGColor? ViewModel.CurrentTheme.MenuButtonBGColor : ViewModel.CurrentTheme.SeriesCardBGColor));

                _ = DistBGOne.Bind(Avalonia.Controls.Border.BackgroundProperty, DataBGColorSource);
                DataBGColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardBGColor == ViewModel.CurrentTheme.MenuBGColor? ViewModel.CurrentTheme.MenuButtonBGColor : ViewModel.CurrentTheme.SeriesCardBGColor));

                _ = DistBGTwo.Bind(Avalonia.Controls.Border.BackgroundProperty, DataBGColorSource);
                DataBGColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardBGColor == ViewModel.CurrentTheme.MenuBGColor? ViewModel.CurrentTheme.MenuButtonBGColor : ViewModel.CurrentTheme.SeriesCardBGColor));

                if (CanUpdate) { UpdateChartColors(); }
                CanUpdate = false;
                IsOpen ^= true;

                if (Screens.Primary.WorkingArea.Height < 1250)
                {
                    this.Height = 550;
                }
            };

            Closing += (s, e) =>
            {
                if (IsOpen)
                {
                    MainWindow.ResetMenuButton(CollectionWindow.StatsButton);
                    ((CollectionStatsWindow)s).Hide();
                    IsOpen ^= true;
                    Topmost = false;
                }
                e.Cancel = true;
            };
        }

        public void UpdateChartColors()
        {
            PieSeries<ObservableValue> ShounenObject = (PieSeries<ObservableValue>)ViewModel.Demographics[0];
            ShounenObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));
            ShounenObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> SeinenObject = (PieSeries<ObservableValue>)ViewModel.Demographics[1];
            SeinenObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuButtonBGColor));
            SeinenObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> ShoujoObject = (PieSeries<ObservableValue>)ViewModel.Demographics[2];
            ShoujoObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            ShoujoObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> JoseiObject = (PieSeries<ObservableValue>)ViewModel.Demographics[3];
            JoseiObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
            JoseiObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> UnknownObject = (PieSeries<ObservableValue>)ViewModel.Demographics[4];
            UnknownObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            UnknownObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> OngoingObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[0];
            OngoingObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));;
            OngoingObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
            
            PieSeries<ObservableValue> FinishedObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[1];
            FinishedObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuButtonBGColor));
            FinishedObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> CancelledObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[2];
            CancelledObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
            CancelledObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> HiatusObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[3];
            HiatusObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            HiatusObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            // Format pie chart themeing
            PieSeries<ObservableValue> MangaObject = (PieSeries<ObservableValue>)ViewModel.Formats[0];
            MangaObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));;
            MangaObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> ManhwaObject = (PieSeries<ObservableValue>)ViewModel.Formats[1];
            ManhwaObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuButtonBGColor));
            ManhwaObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> Manhuabject = (PieSeries<ObservableValue>)ViewModel.Formats[2];
            Manhuabject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            Manhuabject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> NovelObject = (PieSeries<ObservableValue>)ViewModel.Formats[3];
            NovelObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
            NovelObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> ComicObject = (PieSeries<ObservableValue>)ViewModel.Formats[4];
            ComicObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            ComicObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            PieSeries<ObservableValue> ManfraObject = (PieSeries<ObservableValue>)ViewModel.Formats[5];
            ManfraObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            ManfraObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            // Distribution themeing
            Color behindBarColor = Color.Parse(ViewModel.CurrentTheme.MenuButtonBGColor);
            SolidColorPaint BehindBarColor = new SolidColorPaint(new SKColor(behindBarColor.R, behindBarColor.G, behindBarColor.B, 120));

            // Rating Bar Chart themeing
            ColumnSeries<ObservableValue> RatingBarBehindObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[0];
            RatingBarBehindObject.Fill = BehindBarColor;

            ColumnSeries<ObservableValue> RatingBarObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[1];
            RatingBarObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));;
            RatingBarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            RatingBarObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            Axis RatingXAxisObject = ViewModel.RatingXAxes[0];
            RatingXAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            RatingXAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));

            ViewModel.RatingYAxes[0].SeparatorsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            // Volume Count Bar Chart themeing
            ColumnSeries<ObservableValue> VolumeCountBarBehindObject = (ColumnSeries<ObservableValue>)ViewModel.VolumeCountDistribution[0];
            VolumeCountBarBehindObject.Fill = BehindBarColor;

            ColumnSeries<ObservableValue> VolumeCountBarObject = (ColumnSeries<ObservableValue>)ViewModel.VolumeCountDistribution[1];
            VolumeCountBarObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));;
            VolumeCountBarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            VolumeCountBarObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            Axis VolumeCountXAxisObject = ViewModel.VolumeCountXAxes[0];
            VolumeCountXAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            VolumeCountXAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));

            ViewModel.VolumeCountYAxes[0].SeparatorsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            // Genre Bar Chart themeing
            var GenreBarObject = (RowSeries<KeyValuePair<string, int>>)ViewModel.GenreDistribution[0];
            GenreBarObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));;
            GenreBarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            GenreBarObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            Axis GenreYAxisObject = ViewModel.GenreYAxes[0];
            GenreYAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            GenreYAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));

            Axis GenreXAxisObject = ViewModel.GenreXAxes[0];
            GenreXAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            GenreXAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            GenreXAxisObject.SeparatorsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
        }

        private async void CopyTextAsync(object sender, PointerPressedEventArgs args)
        {
            string curText = $"{(sender as Controls.ValueStat).Text} {(sender as Controls.ValueStat).Title}";
            LOGGER.Info($"Copying {curText} to Clipboard");
            await TextCopy.ClipboardService.SetTextAsync($"{curText}");
        }
    }
}
