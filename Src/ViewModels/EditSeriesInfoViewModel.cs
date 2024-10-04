using System.Reactive.Linq;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Tsundoku.Models;

namespace Tsundoku.ViewModels
{
    public class EditSeriesInfoViewModel : ViewModelBase
    {
        public Series Series { get; set; }
        public Button Button { get; set; }
        [Reactive] public int DemographicIndex { get; set; }
        [Reactive] public string CoverImageUrl { get; set; }

        public EditSeriesInfoViewModel(Series Series, Button Button)
        {
            this.Series = Series;
            this.Button = Button;
            this.CurCurrency = MainUser.Currency;
            this.CurrentTheme = MainUser.SavedThemes.First(theme => theme.ThemeName.Equals(MainUser.MainTheme));

            this.WhenAnyValue(x => x.Series.Demographic).ObserveOn(RxApp.TaskpoolScheduler).Subscribe(x => DemographicIndex = Array.IndexOf(DEMOGRAPHICS, x));
        }
    }
}
