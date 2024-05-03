using Avalonia.ReactiveUI;
using Tsundoku.ViewModels;

namespace Tsundoku.Views;

public partial class UserNotesWindow : ReactiveWindow<UserNotesWindowViewModel>
{
    public bool IsOpen = false;

    public UserNotesWindow()
    {
        InitializeComponent();
        
        Opened += (s, e) =>
        {
            IsOpen ^= true;
        };

        Closing += (s, e) =>
        {
            if (IsOpen) 
            { 
                ((UserNotesWindow)s).Hide();
                IsOpen ^= true;
            }
            e.Cancel = true;
        };
    }
}