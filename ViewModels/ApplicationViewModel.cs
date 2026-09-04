using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Task2.Helpers;
using Task2.Models;
using Task2.Services;
using Task2.Views;
using System.Collections.Generic;

namespace Task2.ViewModels
{
    internal class ApplicationViewModel : ViewModelBase
    {
        public ObservableCollection<Employee> Employees { get; set; }
        private Employee selectedEmployee;

        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                return createCommand ??
                    (createCommand = new RelayCommand(obj =>
                    {
                        Employee emp = new Employee
                        {
                            Id = Employees.Any() ? Employees.Max(e => e.Id) + 1 : 1,
                            Age = 18, FullName = "",
                            HireDate = DateTime.Today,
                            IsMarried = false,
                            Position = Position.Junior,
                            Salary = 1000
                        };

                        var dialog = new EmployeeDialog(emp);
                        if (dialog.ShowDialog() == true) Employees.Add(emp);
                    },
                    (obj) =>
                    {
                        if (Employees.Count >= Properties.Settings.Default.MaxRecordsCount) return false;
                        return true;
                    }));
            }
        }
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                    (updateCommand = new RelayCommand(obj =>
                    {
                        var emp = new Employee(selectedEmployee);
                        var dialog = new EmployeeDialog(emp);
                        if(dialog.ShowDialog() == true)
                        {
                            Employees[Employees.IndexOf(selectedEmployee)] = emp;
                        }
                    },
                    (obj) => selectedEmployee != null));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand(obj =>
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Are you sure you want to delete '{SelectedEmployee.FullName}' (id = {SelectedEmployee.Id})?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                            Employees.Remove(SelectedEmployee);
                    },
                    (obj) => selectedEmployee != null));
            }
        }

        private RelayCommand exportConnamd;
        public RelayCommand ExportCommand
        {
            get
            {
                return exportConnamd ?? (exportConnamd = new RelayCommand(obj =>
                {
                    var dialog = new Microsoft.Win32.SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" };
                    if (dialog.ShowDialog() != true) return;
                    try
                    {
                        EmployeeCsvService.ExportToCsv(Employees, dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        private RelayCommand importCommand;
        public RelayCommand ImportCommand
        {
            get
            {
                return importCommand ?? (importCommand = new RelayCommand(obj =>
                {
                    var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };
                    if(dialog.ShowDialog() != true) return;

                    List<Employee> imported;
                    try
                    {
                        imported = EmployeeCsvService.ImportFromCsv(dialog.FileName);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Import failed : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var duplicateGroups = imported
                        .GroupBy(e => e.Id)
                        .Where(g => g.Count() > 1)
                        .ToList();

                    if (duplicateGroups.Any())
                    {
                        var details = string.Join("\n", duplicateGroups
                            .Select(g => $"  Id {g.Key}: {string.Join(", ", g.Select(e => e.FullName))}"));

                        var choice = MessageBox.Show(
                            $"This file contains the following Id duplicates:\n{details}\n\n" +
                            "Keep the first occurrence of each duplicate and ignore the rest?",
                            "Duplicate IDs Found",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (choice != MessageBoxResult.Yes)
                        {
                            MessageBox.Show("Import cancelled. Please, fix the file and try again",
                                "Import cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        imported = imported
                            .GroupBy(e => e.Id)
                            .Select(g => g.First())
                            .ToList();
                    }

                    if(imported.Count > Properties.Settings.Default.MaxRecordsCount)
                    {
                        MessageBox.Show("This file contains too many records (check out settings)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Employees.Clear();
                    foreach (var e in imported) Employees.Add(e);
                }));
            }
        }

        private RelayCommand settingsCommand;
        public RelayCommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new RelayCommand(obj =>
                {
                    var dialog = new SettingsDialog();
                    var vm = (SettingsDialogViewModel)dialog.DataContext;

                    if(dialog.ShowDialog() == true)
                    {
                        if(vm.MaxRecordsCount < Employees.Count)
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show($"You have set a record limit lower than the number of records currently loaded. Do you want to trim the list?", "Shrink records", System.Windows.MessageBoxButton.YesNo);
                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                for (int i = Employees.Count - 1; i >= vm.MaxRecordsCount; --i)
                                {
                                    Employees.RemoveAt(i);
                                }
                            }
                        }
                        Properties.Settings.Default.MaxRecordsCount = vm.MaxRecordsCount;
                        Properties.Settings.Default.IsDarkTheme = vm.SelectedTheme == "Dark";
                        Properties.Settings.Default.Save();

                        //ThemeManager.ApplyTheme(Properties.Settings.Default.IsDarkTheme);
                    }
                }));
            }
        }

        public ApplicationViewModel()
        {
            Employees = new ObservableCollection<Employee>
                {
                    new Employee { Id = 1, FullName = "John Doe", Age = 33, IsMarried = true,  HireDate = new DateTime(2015, 7, 20), Position = Position.Middle,   Salary = 1500 },
                    new Employee { Id = 2, FullName = "Mary Sue", Age = 23, IsMarried = false, HireDate = DateTime.Today,                Position = Position.Junior,    Salary = 1000 },
                    new Employee { Id = 3, FullName = "Alex Gordon", Age = 41, IsMarried = true,  HireDate = new DateTime(2018, 3, 15), Position = Position.Teamlead,  Salary = 2500 },
                    new Employee { Id = 4, FullName = "Shale Mamboo", Age = 27, IsMarried = false, HireDate = new DateTime(2020, 11, 1), Position = Position.Middle,   Salary = 1500 },
                    new Employee { Id = 5, FullName = "Sean Lee", Age = 36, IsMarried = true,  HireDate = new DateTime(2019, 6, 10), Position = Position.Senior,    Salary = 2500 },
                    new Employee { Id = 6, FullName = "Emma Watson", Age = 29, IsMarried = false, HireDate = new DateTime(2021, 2, 28), Position = Position.Junior,    Salary = 1100 },
                    new Employee { Id = 7, FullName = "Michael Brown", Age = 45, IsMarried = true,  HireDate = new DateTime(2010, 5, 5), Position = Position.Teamlead,  Salary = 2800 },
                    new Employee { Id = 8, FullName = "Olivia Smith", Age = 31, IsMarried = false, HireDate = new DateTime(2016, 12, 12), Position = Position.Middle,   Salary = 1700 },
                    new Employee { Id = 9, FullName = "David Johnson", Age = 38, IsMarried = true,  HireDate = new DateTime(2017, 9, 9), Position = Position.Senior,    Salary = 2600 },
                    new Employee { Id = 10, FullName = "Sophia Williams", Age = 24, IsMarried = false, HireDate = DateTime.Today.AddDays(-30), Position = Position.Junior, Salary = 950 },
                };
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmplyee");
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
