using CsvHelper.Configuration;
using System.Globalization;
using Task2.Models;

namespace Task2.Services
{
    internal class EmployeeCsvMap : ClassMap<Employee>
    {
        public EmployeeCsvMap()
        {
            Map(e => e.Id);
            Map(e => e.FullName);
            Map(e => e.Age);
            Map(e => e.IsMarried);
            Map(e => e.Position);
            Map(e => e.HireDate).TypeConverterOption.Format("yyyy-MM-dd");
            Map(e => e.Salary).TypeConverterOption.NumberStyles(NumberStyles.Any);
        }
    }
}
