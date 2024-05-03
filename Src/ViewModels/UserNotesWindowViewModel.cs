using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Tsundoku.ViewModels
{
    public class UserNotesWindowViewModel : ViewModelBase
    {
        [Reactive] public string Notes { get; set; }

        public UserNotesWindowViewModel()
        {
            // this.WhenAnyValue(x => x.CurrentTheme).ObserveOn(RxApp.MainThreadScheduler).Subscribe(x => MainUser.MainTheme = x.ThemeName);
            Notes = MainUser.Notes;
            this.CurrentTheme = MainUser.SavedThemes.First(theme => theme.ThemeName.Equals(MainUser.MainTheme));
            this.WhenAnyValue(x => x.Notes).Throttle(TimeSpan.FromMilliseconds(1000)).Subscribe(x => MainUser.Notes = x);
        }
    }
}
