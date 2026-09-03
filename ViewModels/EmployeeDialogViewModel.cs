using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task2.Helpers;
using Task2.Models;

namespace Task2.ViewModels
{
    internal class EmployeeDialogViewModel : ViewModelBase
    {
        public EmployeeDialogViewModel(Employee employee)
        {
            this.employee = employee;
        }
        public EmployeeDialogViewModel() { }

        public IEnumerable<Position> Positions => Enum.GetValues(typeof(Position)).Cast<Position>();
        private Employee employee;
        public Employee Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }

        public event Action<bool> RequestClose;

        private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new RelayCommand
                    ((obj) =>
                    {
                        RequestClose?.Invoke(true);
                    }));
            }
        }
        private RelayCommand cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new RelayCommand
                    ((obj) =>
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
