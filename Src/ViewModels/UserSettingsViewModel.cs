using System;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Text;
using Tsundoku.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace Tsundoku.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {
        [Reactive] public string UsernameText { get; set; }
        [Reactive] public bool IsChangeUsernameButtonEnabled { get; set; }
        public ICommand ExportToSpreadsheetCommand { get; }

        public UserSettingsViewModel()
        {
            CurCurrency = MainUser.Currency;
            this.WhenAnyValue(x => x.CurCurrency).ObserveOn(RxApp.MainThreadScheduler).Subscribe(x => MainUser.Currency = x);
            this.WhenAnyValue(x => x.UsernameText, x => !string.IsNullOrWhiteSpace(x)).Subscribe(x => IsChangeUsernameButtonEnabled = x);
            ExportToSpreadsheetCommand = ReactiveCommand.CreateFromTask(async () =>{
                await ExportToSpreadsheetAsync();
            });
        }

        private async Task ExportToSpreadsheetAsync()
        {
            await Task.Run(() =>
            {
                string file = @"TsundokuCollection.csv";
                StringBuilder output = new StringBuilder();
                string[] headers = new string[] { "Title", "Staff", "Format", "Status", "Cur Volumes", "Max Volumes", "Demographic", "Cost", "Score", "Volumes Read", "Notes" };
                output.AppendLine(string.Join(",", headers));

                foreach (Models.Series curSeries in MainWindowViewModel.Collection)
                {
                    output.AppendLine(string.Join(",", new string[] { 
                        curSeries.Titles.ContainsKey(MainWindowViewModel.MainUser.CurLanguage) ? curSeries.Titles[MainWindowViewModel.MainUser.CurLanguage] : curSeries.Titles["Romaji"], 
                        curSeries.Staff.ContainsKey(MainWindowViewModel.MainUser.CurLanguage) ? curSeries.Staff[MainWindowViewModel.MainUser.CurLanguage] : curSeries.Staff["Romaji"], 
                        curSeries.Format, 
                        curSeries.Status, 
                        curSeries.CurVolumeCount.ToString(), 
                        curSeries.MaxVolumeCount.ToString(), 
                        curSeries.Demographic, 
                        $"{MainWindowViewModel.MainUser.Currency}{curSeries.Cost.ToString()}", 
                        curSeries.Score.ToString(), 
                        $"{curSeries.VolumesRead.ToString()}", 
                        curSeries.SeriesNotes }));
                }

                try
                {
                    System.IO.File.WriteAllTextAsync(file, output.ToString(), Encoding.UTF8);
                    Constants.Logger.Info($"Exported {MainWindowViewModel.MainUser.UserName}'s Data To -> TsundokuCollection.csv");
                }
                catch (Exception ex)
                {
                    Constants.Logger.Warn($"Could not Export {MainWindowViewModel.MainUser.UserName}'s Data To -> TsundokuCollection.csv \n{ex}");
                }
            });
        }
    }
}
