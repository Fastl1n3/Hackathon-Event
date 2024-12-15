using System;
using System.Collections.Generic;
using System.IO;

namespace Hackathon {
    public static class CsvEmployeeReader {
        public static List<Employee> ReadCsv(string filePath) {
            var entries = new List<Employee>();
            try {
                var reader = new StreamReader(filePath);
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var parts = line.Split(';');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int id)) {
                        entries.Add(new Employee(id, parts[1]));
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
            return entries;
        }
    }
}