using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Task2.Models;

namespace Task2.Services
{
    internal class EmployeeCsvService
    {
        private const char sep = ';';
        public static List<Employee> ImportFromCsv(string path)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<EmployeeCsvMap>();
                var records = csv.GetRecords<Employee>().ToList();
                return records;
            }
        }

        public static void ExportToCsv(IEnumerable<Employee> employees, string path)
        {
            using(var writer = new StreamWriter(path, false, Encoding.UTF8))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<EmployeeCsvMap>();
                csv.WriteRecords(employees);
            }
        }
    }
}
