using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace Homework
{
    class Program
    {
        static Random random = new Random();
        private const string PathToStudentFile = @"..\..\Resources\student.txt";


        static void Main(string[] args)
        {
            DoTaskLottery();
            DoTaskExcel();
            Console.ReadKey();
        }
        static void DoTaskLottery()
        {
            SetListStudentsFromFile(out List<Student> students);
            List<Lotery> loteries = new List<Lotery>()
            {
              new Lotery("Билеты в Театр", DateTime.Today.AddDays(7), 6),
              new Lotery("Билеты в Кино", DateTime.Today.AddDays(14), 10),
              new Lotery("Билеты на Концерт", DateTime.Today.AddDays(21), 4),
              new Lotery("Билеты на Доп. Сессию", DateTime.Today.AddDays(28), 12)
            };
            for (int i = 0; i < loteries.Count; i++)
            {
                loteries[i].Start(GetListPossibleParticipants(students));
            }
            foreach (var item in loteries)
            {
                item.Display();
            }
        }
        static void DoTaskExcel()
        {
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Open($"{Environment.CurrentDirectory}\\input.xlsx");
            Worksheet workSheet = workBook.Worksheets[1];
            object[,] range = workSheet.Range["A2", "B10"].Value2;
            Dictionary<string, string> illcure = new Dictionary<string, string>();
            for (int i = 1; i <= range.GetLength(0); i++)
            {
                illcure.Add(range[i, 1].ToString().ToLower(), range[i, 2].ToString());
            }
            workBook.Close();
            workBook = excelApp.Workbooks.Open($"{Environment.CurrentDirectory}\\output.xlsx");
            workSheet = workBook.Worksheets[1];
            range = workSheet.Range["G2", "G35"].Value2;
            for (int i = 1; i <= range.Length; i++)
            {
                //Изменяем массив 
                string stringExcel = range[i, 1].ToString().ToLower();
                foreach (var pair in illcure)
                {
                    if (stringExcel.Contains(pair.Key))
                    {
                        range[i, 1] = pair.Value;
                        break;
                    }
                }
            }
            workSheet.Range["H2", "H35"].Value2 = range;
            workBook.Save();
            workBook.Close();
            excelApp.Quit();
        }
        static void SetListStudentsFromFile(out List<Student> students)
        {
            students = new List<Student>();
            using (StreamReader reader = new StreamReader(PathToStudentFile))
            {
                string stringfromfile;
                while ((stringfromfile = reader.ReadLine()) != null)
                {

                    string[] dateStudent = stringfromfile.Split();
                    string nameStudent = dateStudent[0];
                    if (!int.TryParse(dateStudent[1], out int numberStudent))
                    {
                        throw new FormatException("Неправильный номер студента");
                    }
                    students.Add(new Student(nameStudent, numberStudent));
                }
            }
        }
        static List<Student> GetListPossibleParticipants(List<Student> students)
        {
            return new List<Student>(from u in students where random.Next(100) < 50 select u);
        }
    }
}
