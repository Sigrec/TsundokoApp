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

        public CollectionStatsWindow()
        {
            InitializeComponent();
            DataContext = new CollectionStatsViewModel();
            Opened += (s, e) =>
            {
                var sub = UnknownRectangle.Bind(Avalonia.Controls.Shapes.Shape.FillProperty, UnknownRectangleColorSource);
                UnknownRectangleColorSource.OnNext(SolidColorBrush.Parse(ViewModel.CurrentTheme.SeriesCardDescColor == ViewModel.CurrentTheme.MenuTextColor ? ViewModel.CurrentTheme.SeriesCardTitleColor : ViewModel.CurrentTheme.SeriesCardDescColor));
                if (CanUpdate) { UpdateChartColors(); }
                CanUpdate = false;
                IsOpen ^= true;

                if (Screens.Primary.WorkingArea.Height < 1025)
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
            OngoingObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));
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

            Color behindBarColor = Color.Parse(ViewModel.CurrentTheme.MenuButtonBGColor);
            ColumnSeries<ObservableValue> BarBehindObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[0];
            BarBehindObject.Fill = new SolidColorPaint(new SKColor(behindBarColor.R, behindBarColor.G, behindBarColor.B, 120));

            ColumnSeries<ObservableValue> BarObject = (ColumnSeries<ObservableValue>)ViewModel.RatingDistribution[1];
            BarObject.Fill = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuBGColor));
            BarObject.DataLabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            BarObject.Stroke = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));

            Axis XAxisObject = ViewModel.RatingXAxes[0];
            XAxisObject.LabelsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));
            XAxisObject.TicksPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.MenuTextColor));

            ViewModel.RatingYAxes[0].SeparatorsPaint = new SolidColorPaint(SKColor.Parse(ViewModel.CurrentTheme.DividerColor));
        }

        private async void CopyTextAsync(object sender, PointerPressedEventArgs args)
        {
            string curText = $"{(sender as Controls.ValueStat).Text} {(sender as Controls.ValueStat).Title}";
            LOGGER.Info($"Copying {curText} to Clipboard");
            await TextCopy.ClipboardService.SetTextAsync($"{curText}");
        }
    }
}
