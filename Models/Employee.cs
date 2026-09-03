using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

enum Position
{
    Intern,
    Junior,
    Middle,
    Senior,
    Teamlead,
    CEO
}

namespace Task2.Models
{
    internal class Employee : INotifyPropertyChanged
    {
        private uint id;
        private string fullName;
        private uint age;
        private bool isMarried;
        private Position position;
        private DateTime hireDate;
        private double salary;

        public uint Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }
        public uint Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }
        public bool IsMarried
        {
            get { return isMarried; }
            set
            {
                isMarried = value;
                OnPropertyChanged("IsMarried");
            }
        }
        public Position Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        public DateTime HireDate
        {
            get { return hireDate; }
            set
            {
                hireDate = value;
                OnPropertyChanged("HireDate");
            }
        }
        public double Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                OnPropertyChanged("Salary");
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, FullName: {FullName}, Age: {Age}," +
                $"IsMarried: {IsMarried}, Position: {Position}," +
                $"HireDate: {HireDate:yyyy-MM-dd}, Salary: {Salary:F2}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public Employee() { }
        public Employee(Employee e)
        {
            this.id = e.Id;
            this.fullName = e.FullName;
            this.age = e.Age;
            this.isMarried = e.IsMarried;
            this.position = e.Position;
            this.hireDate = e.HireDate;
            this.salary = e.Salary;
        }
    }
}
