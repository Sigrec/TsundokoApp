using ReactiveUI.Fody.Helpers;

namespace Tsundoku.ViewModels
{
    public class PopupWindowViewModel : ViewModelBase
    {
        [Reactive] public string Title { get; set; }
        [Reactive] public string InfoText { get; set; }
        [Reactive] public string Icon { get; set; }

        public PopupWindowViewModel() 
        {
            this.CurrentTheme = MainUser.SavedThemes.First(theme => theme.ThemeName.Equals(MainUser.MainTheme));
        }

        public void SetPopupInfo(string title, string icon, string infoText)
        {
            Title = title;
            Icon = icon;
            InfoText = infoText;
        }

        public void ResetPopupInfo()
        {
            Title = string.Empty;
            Icon = string.Empty;
            InfoText = string.Empty;
        }
    }
}