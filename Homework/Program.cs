using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Homework
{
    class Program
    {
        static Random random = new Random();
        private const string PathToStudentFile = @"..\..\Resources\student.txt";

        class Lotery
        {
            private static Random random = new Random();
            public string Name { get; private set; }
            public List<Student> Winners { get; private set; } = new List<Student>();
            public DateTime Date = new DateTime();
            public int CountParticipants { get; private set; }
            public Lotery(string nameTicket, DateTime date, int count)
            {
                Name = nameTicket;
                Date = date;
                CountParticipants = count;
            }
            public void AddWinner(Student student)
            {
                if (Winners.Count < CountParticipants)
                {
                    Winners.Add(student);
                }
            }
            public void Start(List<Student> students)
            {
                List<Student> sortedStudents =  new List<Student>(from u in students orderby u.NumWins select u);
                //Так  у кого меньше выйгрешей есть больше шансов выйграть в лотерею.....
                if (sortedStudents.Count <= CountParticipants)
                {
                    for (int i = 0; i < sortedStudents.Count; i++)
                    {
                        sortedStudents[i].WinLotery();
                        Winners.Add(sortedStudents[i]);
                    }
                }
                else
                {
                    int i = 0;
                    while (Winners.Count < CountParticipants)
                    {
                        i = i % sortedStudents.Count;
                        if (random.Next(100) < 79)
                        {
                            sortedStudents[i].WinLotery();
                            Winners.Add(sortedStudents[i]);
                        }
                        i++;
                    }
                }
            }
            public void Display()
            {
                Console.WriteLine($"Name: {Name}");
                Console.WriteLine($"Date: {Date.ToShortDateString()}");
                Console.WriteLine("Winners: ");
                foreach (var item in Winners)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
        class Student
        {
            private static int count = 0;
            public int ID { get; private set; }
            public byte NumWins { get; private set; }
            public string Name { get; private set; }
            public int NumGroup { get; private set; }

            public Student(string name, int numberGroup)
            {
                Name = name;
                NumGroup = numberGroup;
                ID = count;
                count++;
            }
            public void WinLotery()
            {
                NumWins++;
            }
            public override string ToString()
            {
                return $"{ID} {Name} {NumGroup} Количество выйгрешей: {NumWins}";
            }

        }
        static void Main(string[] args)
        {
            DoTaskLottery();
            //DoTaskExcel();

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
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open($"{Environment.CurrentDirectory}\\input.xlsx");
            Worksheet ws = wb.Worksheets[1];
            Console.WriteLine("Opened input");
            object[,] readRange = ws.Range["A2", "B10"].Value2;
            Dictionary<string, string> illcure = new Dictionary<string, string>();
            for (int i = 1; i <= readRange.GetLength(0); i++)
            {
                illcure.Add(readRange[i, 1].ToString().ToLower(), readRange[i, 2].ToString());
            }
            Console.WriteLine("Reading result:");
            foreach (var k in illcure)
            {
                Console.WriteLine($"{k.Key}->{k.Value}");
            }
            wb.Close();
            Console.WriteLine("Closed input");
            wb = excel.Workbooks.Open($"{Environment.CurrentDirectory}\\output.xlsx");
            ws = wb.Worksheets[1];
            Console.WriteLine("Opened output");
            readRange = ws.Range["G2", "G35"].Value2;
            for (int i = 1; i <= readRange.Length; i++)
            {
                string readString = readRange[i, 1].ToString().ToLower();
                foreach (var pair in illcure)
                {
                    if (readString.Contains(pair.Key))
                    {
                        readRange[i, 1] = pair.Value;
                        break;
                    }
                }
            }
            ws.Range["G2", "G35"].Value2 = readRange;
            Console.WriteLine("Written output");
            wb.Save();
            Console.WriteLine("Saved output");
            wb.Close();
            excel.Quit();
            Console.WriteLine("Closed output");
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
