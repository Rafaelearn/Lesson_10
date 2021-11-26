using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework
{
    class Lotery:ILotery
    {
        private static Random random = new Random();
        public string Name { get; private set; }
        public List<Student> Winners { get; private set; } = new List<Student>();
        public DateTime Date { get; private set; }
        public int CountParticipants { get; private set; }
        public Lotery(string nameTicket, DateTime date, int count)
        {
            Name = nameTicket;
            Date = date;
            CountParticipants = count;
        }
        public void Start(List<Student> students)
        {
            List<Student> sortedStudents = new List<Student>(from u in students orderby u.NumWins select u);
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
}
