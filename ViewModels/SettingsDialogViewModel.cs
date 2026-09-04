using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Task2.Helpers;

namespace Task2.ViewModels
{
    internal class SettingsDialogViewModel : ViewModelBase
    {
        public IEnumerable<string> Themes => new[] { "Light", "Dark" };

        private int maxRecordsCount;
        public int MaxRecordsCount
        {
            get => maxRecordsCount;
            set
            {
                maxRecordsCount = value;
                OnPropertyChanged("MaxRecordsCount");
            }
        }

        private string selectedTheme;
        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                OnPropertyChanged($"{nameof(SelectedTheme)}");
            }
        }

        public event Action<bool> RequestClose;

        public SettingsDialogViewModel()
        {
            MaxRecordsCount = Properties.Settings.Default.MaxRecordsCount;
            SelectedTheme = Properties.Settings.Default.IsDarkTheme ? "Dark" : "Light";
        }

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new RelayCommand(obj =>
                {
                    if(MaxRecordsCount <= 0)
                    {
                        MessageBox.Show("Maximum record count must be greater than 0",
                            "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    RequestClose?.Invoke(true);
                }));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new RelayCommand(obj =>
                {
                    RequestClose?.Invoke(false);
                }));
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
