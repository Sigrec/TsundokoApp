using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Tsundoku.ViewModels;
using Avalonia.ReactiveUI;
using System.Reactive.Subjects;
using Avalonia.Media;
using SkiaSharp;

namespace Tsundoku.Views
{
    public partial class CollectionStatsWindow : ReactiveWindow<CollectionStatsViewModel>
    {
        public bool IsOpen = false;
        public bool CanUpdate = true; // On First Update
        Subject<SolidColorBrush> UnknownRectangleColorSource = new Subject<SolidColorBrush>();
        Subject<SolidColorBrush> ManfraRectangleColorSource = new Subject<SolidColorBrush>();
        Subject<SolidColorBrush> ComicRectangleColorSource = new Subject<SolidColorBrush>();

        public CollectionStatsWindow()
        {
            InitializeComponent();
            DataContext = new CollectionStatsViewModel();
            Opened += (s, e) =>
            {
                var unknownSub = UnknownRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, UnknownRectangleColorSource);
                UnknownRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));

                var comicSub = ComicRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, ComicRectangleColorSource);
                ComicRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));

                var manfraSub = ManfraRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, ManfraRectangleColorSource);
                ManfraRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardStaffColor == ViewModel.CurrentTheme.SeriesCardTitleColor ? ViewModel.CurrentTheme.SeriesEditPaneButtonsIconColor : ViewModel.CurrentTheme.SeriesCardStaffColor));

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
                    ((CollectionStatsWindow)s).Hide();
                    IsOpen ^= true;
                    Topmost = false;
                }
                e.Cancel = true;
            };
        }

        public void UpdateChartColors()
        {
            SolidColorPaint MenuBGColor = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));
            SolidColorPaint MenuTextColor = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            SolidColorPaint DividerColor = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
            SolidColorPaint MenuButtonBGColor = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuButtonBGColor));

            PieSeries<ObservableValue> ShounenObject = (PieSeries<ObservableValue>)ViewModel.Demographics[0];
            ShounenObject.Fill = MenuBGColor;
            ShounenObject.Stroke = DividerColor;

            PieSeries<ObservableValue> SeinenObject = (PieSeries<ObservableValue>)ViewModel.Demographics[1];
            SeinenObject.Fill = MenuButtonBGColor;
            SeinenObject.Stroke = DividerColor;

            PieSeries<ObservableValue> ShoujoObject = (PieSeries<ObservableValue>)ViewModel.Demographics[2];
            ShoujoObject.Fill = MenuTextColor;
            ShoujoObject.Stroke = DividerColor;

            PieSeries<ObservableValue> JoseiObject = (PieSeries<ObservableValue>)ViewModel.Demographics[3];
            JoseiObject.Fill = DividerColor;
            JoseiObject.Stroke = DividerColor;

            PieSeries<ObservableValue> UnknownObject = (PieSeries<ObservableValue>)ViewModel.Demographics[4];
            UnknownObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            UnknownObject.Stroke = DividerColor;

            PieSeries<ObservableValue> OngoingObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[0];
            OngoingObject.Fill = MenuBGColor;
            OngoingObject.Stroke = DividerColor;
            
            PieSeries<ObservableValue> FinishedObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[1];
            FinishedObject.Fill = MenuButtonBGColor;
            FinishedObject.Stroke = DividerColor;

            PieSeries<ObservableValue> CancelledObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[2];
            CancelledObject.Fill = DividerColor;
            CancelledObject.Stroke = DividerColor;

            PieSeries<ObservableValue> HiatusObject = (PieSeries<ObservableValue>)ViewModel.StatusDistribution[3];
            HiatusObject.Fill = MenuTextColor;
            HiatusObject.Stroke = DividerColor;

            // Format pie chart themeing
            PieSeries<ObservableValue> MangaObject = (PieSeries<ObservableValue>)ViewModel.Formats[0];
            MangaObject.Fill = MenuBGColor;
            MangaObject.Stroke = DividerColor;

            PieSeries<ObservableValue> ManhwaObject = (PieSeries<ObservableValue>)ViewModel.Formats[1];
            ManhwaObject.Fill = MenuButtonBGColor;
            ManhwaObject.Stroke = DividerColor;

            PieSeries<ObservableValue> Manhuabject = (PieSeries<ObservableValue>)ViewModel.Formats[2];
            Manhuabject.Fill = MenuTextColor;
            Manhuabject.Stroke = DividerColor;

            PieSeries<ObservableValue> NovelObject = (PieSeries<ObservableValue>)ViewModel.Formats[3];
            NovelObject.Fill = DividerColor;
            NovelObject.Stroke = DividerColor;

            PieSeries<ObservableValue> ComicObject = (PieSeries<ObservableValue>)ViewModel.Formats[4];
            ComicObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            ComicObject.Stroke = DividerColor;

            PieSeries<ObservableValue> ManfraObject = (PieSeries<ObservableValue>)ViewModel.Formats[5];
            ManfraObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
            ManfraObject.Stroke = DividerColor;

            // Distribution themeing
            Color behindBarColor = Color.Parse(ViewModel.CurrentTheme.MenuButtonBGColor);
            SolidColorPaint BehindBarColor = new SolidColorPaint(new SKColor(behindBarColor.R, behindBarColor.G, behindBarColor.B, 120));

            // Rating Bar Chart themeing
            ColumnSeries<ObservableValue> RatingBarBehindObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[0];
            RatingBarBehindObject.Fill = BehindBarColor;

            ColumnSeries<ObservableValue> RatingBarObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[1];
            RatingBarObject.Fill = MenuBGColor;
            RatingBarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;
            RatingBarObject.Stroke = DividerColor;

            Axis RatingXAxisObject = ViewModel.RatingXAxes[0];
            RatingXAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;
            RatingXAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;

            ViewModel.RatingYAxes[0].SeparatorsPaint = DividerColor;

            // Volume Count Bar Chart themeing
            ColumnSeries<ObservableValue> VolumeCountBarBehindObject = (ColumnSeries<ObservableValue>)ViewModel.VolumeCountDistribution[0];
            VolumeCountBarBehindObject.Fill = BehindBarColor;

            ColumnSeries<ObservableValue> VolumeCountBarObject = (ColumnSeries<ObservableValue>)ViewModel.VolumeCountDistribution[1];
            VolumeCountBarObject.Fill = MenuBGColor;
            VolumeCountBarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;
            VolumeCountBarObject.Stroke = DividerColor;

            Axis VolumeCountXAxisObject = ViewModel.VolumeCountXAxes[0];
            VolumeCountXAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;
            VolumeCountXAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));;

            ViewModel.VolumeCountYAxes[0].SeparatorsPaint = DividerColor;
        }

        private async void CopyTextAsync(object sender, PointerPressedEventArgs args)
        {
            string curText = $"{(sender as Controls.ValueStat).Text} {(sender as Controls.ValueStat).Title}";
            LOGGER.Info($"Copying {curText} to Clipboard");
            await TextCopy.ClipboardService.SetTextAsync($"{curText}");
        }
    }
}
