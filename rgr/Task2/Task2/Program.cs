using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public interface IGroupPrinter  // Component
    {
        void Print(IEnumerable<Student> grouplist);
    }

    public class DeanService : IGroupPrinter    // ConcreteComponent
    {
        public void Print(IEnumerable<Student> grouplist)
        {
            int i = 1;
            foreach (var student in grouplist)
            {
                Console.WriteLine($"{i}: {student.Name}");
                i++;
            }
        }
    }

    public interface IGroupPrinterNew : IGroupPrinter   // Decorator
    {
        void Print(string groupCypher);
    }

    public class DeanServiceNew : IGroupPrinterNew  // ConcreteDecorator
    {
        private readonly IGroupPrinter _groupPrinter;
        public Dictionary<string, IList<Student>> Groups { get; } = new Dictionary<string, IList<Student>>();

        public DeanServiceNew(IGroupPrinter groupPrinter)
        {
            _groupPrinter = groupPrinter;
        }

        public void Print(IEnumerable<Student> grouplist)
        {
            _groupPrinter.Print(grouplist);
        }

        public void Print(string groupCypher)
        {
            var group = Groups[groupCypher];
            Console.WriteLine(groupCypher + ":");
            Print(group);
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public Student(string name)
        {
            Name = name;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var rk02 = new List<Student>();
            var tp93 = new List<Student>();

            rk02.Add(new Student("Pol"));
            rk02.Add(new Student("Jim"));

            tp93.Add(new Student("Anna"));
            tp93.Add(new Student("Willy"));
            tp93.Add(new Student("John"));

            var oldDeanService = new DeanService();
            oldDeanService.Print(tp93);

            var deanService = new DeanServiceNew(oldDeanService);

            deanService.Groups.Add("rk02", rk02);
            deanService.Groups.Add("tp93", tp93);

            deanService.Print("rk02");
            deanService.Print("tp93");

            Console.ReadKey();
        }
    }
}
