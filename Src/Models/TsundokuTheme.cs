using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Tsundoku.Models
{
    public class TsundokuTheme : ReactiveObject, ICloneable, IComparable, IDisposable
    {
        [JsonIgnore] private bool disposedValue;
        [Reactive] public string ThemeName { get; set; }
        [Reactive] public string MenuBGColor { get; set; }
        [Reactive] public string UsernameColor { get; set; }
        [Reactive] public string MenuTextColor { get; set; }
        [Reactive] public string SearchBarBGColor { get; set; }
        [Reactive] public string SearchBarBorderColor { get; set; }
        [Reactive] public string SearchBarTextColor { get; set; }
        [Reactive] public string DividerColor { get; set; }
        [Reactive] public string MenuButtonBGColor { get; set; } 
        [Reactive] public string MenuButtonBGHoverColor { get; set; }
        [Reactive] public string MenuButtonBorderColor { get; set; }
        [Reactive] public string MenuButtonBorderHoverColor { get; set; }
        [Reactive] public string MenuButtonTextAndIconColor { get; set; }
        [Reactive] public string MenuButtonTextAndIconHoverColor { get; set; }
        [Reactive] public string CollectionBGColor { get; set; }
        [Reactive] public string StatusAndBookTypeBGColor { get; set; }
        [Reactive] public string StatusAndBookTypeBGHoverColor { get; set; }
        [Reactive] public string StatusAndBookTypeTextColor { get; set; }
        [Reactive] public string StatusAndBookTypeTextHoverColor { get; set; }
        [Reactive] public string SeriesCardBGColor { get; set; }
        [Reactive] public string SeriesCardTitleColor { get; set; }
        [Reactive] public string SeriesCardStaffColor { get; set; }
        [Reactive] public string SeriesCardDescColor { get; set; }
        [Reactive] public string SeriesProgressBGColor { get; set; }
        [Reactive] public string SeriesProgressBarColor { get; set; }
        [Reactive] public string SeriesProgressBarBGColor { get; set; }
        [Reactive] public string SeriesProgressBarBorderColor { get; set; }
        [Reactive] public string SeriesProgressTextColor { get; set; }
        [Reactive] public string SeriesProgressButtonsHoverColor { get; set; }
        [Reactive] public string SeriesButtonIconColor { get; set; }
        [Reactive] public string SeriesButtonIconHoverColor { get; set; }
        [Reactive] public string SeriesEditPaneBGColor { get; set; }
        [Reactive] public string SeriesNotesBGColor  { get; set; }
        [Reactive] public string SeriesNotesBorderColor { get; set; }
        [Reactive] public string SeriesNotesTextColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsBGColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsBGHoverColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsBorderColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsBorderHoverColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsIconColor { get; set; }
        [Reactive] public string SeriesEditPaneButtonsIconHoverColor { get; set; }

        public static readonly TsundokuTheme DEFAULT_THEME = new TsundokuTheme(
            "Default", //ThemeName
            "#ff20232d",
            "#ffb4bddb",
            "#ffb4bddb",
            "#ff626460",
            "#ffdfd59e",
            "#ffb4bddb",
            "#ffdfd59e",
            "#ff626460",
            "#ff2c2d42",
            "#ffdfd59e",
            "#ffdfd59e",
            "#ffb4bddb",
            "#ff626460",
            "#ff2c2d42",
            "#ff626460",
            "#ffdfd59e",
            "#ffececec",
            "#ff626460",
            "#ff20232d",
            "#ffdfd59e",
            "#ffb4bddb",
            "#ffececec",
            "#ff626460",
            "#ffdfd59e",
            "#ff20232d",
            "#ffececec",
            "#ffececec",
            "#ff20232d",
            "#ffececec",
            "#ffdfd59e",
            "#ff20232d",
            "#ff626460",
            "#ffdfd59e",
            "#ffb4bddb",
            "#ff2c2d42",
            "#ff626460",
            "#ffdfd59e",
            "#ffdfd59e",
            "#ff626460",
            "#ffb4bddb"
        );

        public TsundokuTheme()
        {

        }

        public TsundokuTheme(string themeName)
        {
            ThemeName = themeName;
        }

        [JsonConstructor]
        public TsundokuTheme(string themeName, string menuBGColor, string usernameColor, string menuTextColor, string searchBarBGColor, string searchBarBorderColor, string searchBarTextColor, string dividerColor, string menuButtonBGColor, string menuButtonBGHoverColor, string menuButtonBorderColor, string menuButtonBorderHoverColor, string menuButtonTextAndIconColor, string menuButtonTextAndIconHoverColor, string collectionBGColor, string statusAndBookTypeBGColor, string statusAndBookTypeBGHoverColor, string statusAndBookTypeTextColor, string statusAndBookTypeTextHoverColor, string seriesCardBGColor, string seriesCardTitleColor, string seriesCardStaffColor, string seriesCardDescColor, string seriesProgressBGColor, string seriesProgressBarColor, string seriesProgressBarBGColor, string seriesProgressBarBorderColor, string seriesProgressTextColor, string seriesProgressButtonsHoverColor, string seriesButtonIconColor, string seriesButtonIconHoverColor, string seriesEditPaneBGColor, string seriesNotesBGColor, string seriesNotesBorderColor, string seriesNotesTextColor, string seriesEditPaneButtonsBGColor, string seriesEditPaneButtonsBGHoverColor, string seriesEditPaneButtonsBorderColor, string seriesEditPaneButtonsBorderHoverColor, string seriesEditPaneButtonsIconColor, string seriesEditPaneButtonsIconHoverColor) : this(themeName)
        {
            MenuBGColor = menuBGColor;
            UsernameColor = usernameColor;
            MenuTextColor = menuTextColor;
            SearchBarBGColor = searchBarBGColor;
            SearchBarBorderColor = searchBarBorderColor;
            SearchBarTextColor = searchBarTextColor;
            DividerColor = dividerColor;
            MenuButtonBGColor = menuButtonBGColor;
            MenuButtonBGHoverColor = menuButtonBGHoverColor;
            MenuButtonBorderColor = menuButtonBorderColor;
            MenuButtonBorderHoverColor = menuButtonBorderHoverColor;
            MenuButtonTextAndIconColor = menuButtonTextAndIconColor;
            MenuButtonTextAndIconHoverColor = menuButtonTextAndIconHoverColor;
            CollectionBGColor = collectionBGColor;
            StatusAndBookTypeBGColor = statusAndBookTypeBGColor;
            StatusAndBookTypeBGHoverColor = statusAndBookTypeBGHoverColor;
            StatusAndBookTypeTextColor = statusAndBookTypeTextColor;
            StatusAndBookTypeTextHoverColor = statusAndBookTypeTextHoverColor;
            SeriesCardBGColor = seriesCardBGColor;
            SeriesCardTitleColor = seriesCardTitleColor;
            SeriesCardStaffColor = seriesCardStaffColor;
            SeriesCardDescColor = seriesCardDescColor;
            SeriesProgressBGColor = seriesProgressBGColor;
            SeriesProgressBarColor = seriesProgressBarColor;
            SeriesProgressBarBGColor = seriesProgressBarBGColor;
            SeriesProgressBarBorderColor = seriesProgressBarBorderColor;
            SeriesProgressTextColor = seriesProgressTextColor;
            SeriesProgressButtonsHoverColor = seriesProgressButtonsHoverColor;
            SeriesButtonIconColor = seriesButtonIconColor;
            SeriesButtonIconHoverColor = seriesButtonIconHoverColor;
            SeriesEditPaneBGColor = seriesEditPaneBGColor;
            SeriesNotesBGColor = seriesNotesBGColor;
            SeriesNotesBorderColor = seriesNotesBorderColor;
            SeriesNotesTextColor = seriesNotesTextColor;
            SeriesEditPaneButtonsBGColor = seriesEditPaneButtonsBGColor;
            SeriesEditPaneButtonsBGHoverColor = seriesEditPaneButtonsBGHoverColor;
            SeriesEditPaneButtonsBorderColor = seriesEditPaneButtonsBorderColor;
            SeriesEditPaneButtonsBorderHoverColor = seriesEditPaneButtonsBorderHoverColor;
            SeriesEditPaneButtonsIconColor = seriesEditPaneButtonsIconColor;
            SeriesEditPaneButtonsIconHoverColor = seriesEditPaneButtonsIconHoverColor;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ThemeName);
            hash.Add(MenuBGColor);
            hash.Add(UsernameColor);
            hash.Add(MenuTextColor);
            hash.Add(SearchBarBGColor);
            hash.Add(SearchBarBorderColor);
            hash.Add(SearchBarTextColor);
            hash.Add(MenuButtonBGColor);
            hash.Add(MenuButtonBGHoverColor);
            hash.Add(MenuButtonBorderColor);
            hash.Add(MenuButtonBorderHoverColor);
            hash.Add(MenuButtonTextAndIconColor);
            hash.Add(MenuButtonTextAndIconHoverColor);
            hash.Add(DividerColor);
            hash.Add(CollectionBGColor);
            hash.Add(StatusAndBookTypeBGColor);
            hash.Add(StatusAndBookTypeBGHoverColor);
            hash.Add(StatusAndBookTypeTextColor);
            hash.Add(StatusAndBookTypeTextHoverColor);
            hash.Add(SeriesCardBGColor);
            hash.Add(SeriesCardTitleColor);
            hash.Add(SeriesCardStaffColor);
            hash.Add(SeriesCardDescColor);
            hash.Add(SeriesProgressBGColor);
            hash.Add(SeriesProgressBarColor);
            hash.Add(SeriesProgressBarBGColor);
            hash.Add(SeriesProgressBarBorderColor);
            hash.Add(SeriesProgressTextColor);
            hash.Add(SeriesProgressButtonsHoverColor);
            hash.Add(SeriesButtonIconColor);
            hash.Add(SeriesButtonIconHoverColor);
            hash.Add(SeriesEditPaneBGColor);
            hash.Add(SeriesNotesBGColor);
            hash.Add(SeriesNotesBorderColor);
            hash.Add(SeriesNotesTextColor);
            hash.Add(SeriesEditPaneButtonsBGColor);
            hash.Add(SeriesEditPaneButtonsBGHoverColor);
            hash.Add(SeriesEditPaneButtonsBorderColor);
            hash.Add(SeriesEditPaneButtonsBorderHoverColor);
            hash.Add(SeriesEditPaneButtonsIconColor);
            hash.Add(SeriesEditPaneButtonsIconHoverColor);
            return hash.ToHashCode();
        }

        public int CompareTo(object? obj)
        {
            return this.ThemeName.CompareTo((obj as TsundokuTheme).ThemeName);
        }

        public virtual TsundokuTheme Cloning()
        {
            return new TsundokuTheme(ThemeName, MenuBGColor, UsernameColor, MenuTextColor,SearchBarBGColor,SearchBarBorderColor,SearchBarTextColor, DividerColor, MenuButtonBGColor, MenuButtonBGHoverColor, MenuButtonBorderColor, MenuButtonBorderHoverColor, MenuButtonTextAndIconColor, MenuButtonTextAndIconHoverColor, CollectionBGColor,StatusAndBookTypeBGColor,StatusAndBookTypeBGHoverColor,StatusAndBookTypeTextColor,StatusAndBookTypeTextHoverColor,SeriesCardBGColor,SeriesCardTitleColor,SeriesCardStaffColor,SeriesCardDescColor,SeriesProgressBGColor,SeriesProgressBarColor,SeriesProgressBarBGColor,SeriesProgressBarBorderColor,SeriesProgressTextColor,SeriesProgressButtonsHoverColor,SeriesButtonIconColor,SeriesButtonIconHoverColor,SeriesEditPaneBGColor,SeriesNotesBGColor,SeriesNotesBorderColor,SeriesNotesTextColor,SeriesEditPaneButtonsBGColor,SeriesEditPaneButtonsBGHoverColor,SeriesEditPaneButtonsBorderColor,SeriesEditPaneButtonsBorderHoverColor,SeriesEditPaneButtonsIconColor,SeriesEditPaneButtonsIconHoverColor);
        }

        object ICloneable.Clone()
        {
            return Cloning();
        }

        protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// dispose managed state (managed objects)
					
				}

				// free unmanaged resources (unmanaged objects) and override finalizer
				// set large fields to null
				disposedValue = true;
			}
		}

		// Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~Series()
		// {
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
    }
}