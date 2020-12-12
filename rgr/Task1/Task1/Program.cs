using System;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            MainTest();
            Console.WriteLine('\n' + new string('-', 64) + '\n');
            ExceptionTest();

            Console.ReadKey();
        }

        static void MainTest()
        {
            var camp = new CampComposite("Camp name");

            var detachment1 = new CampComposite("Detachment #1");
            var detachment2 = new CampComposite("Detachment #2");

            var room1 = new CampComposite("Room #1");
            var room2 = new CampComposite("Room #2");
            var room3 = new CampComposite("Room #3");

            var child1 = new Child("Tom");
            var child2 = new Child("Mike");
            var child3 = new Child("Anna");
            var child4 = new Child("Kim Jong Un");
            var child5 = new Child("Poll");
            var child6 = new Child("Julie");
            var child7 = new Child("de Bill");

            room1.Add(child1);
            room1.Add(child2);
            room2.Add(child3);
            room2.Add(child4);
            room3.Add(child5);
            room3.Add(child6);
            room3.Add(child7);

            detachment1.Add(room1);
            detachment1.Add(room2);
            detachment2.Add(room3);

            camp.Add(detachment1);
            camp.Add(detachment2);

            Console.WriteLine("Main test:\n");
            Console.WriteLine("Initial state:");
            camp.Display();
            camp.LightsOut();
            Console.WriteLine("\nLights out:");
            camp.Display();
            camp.Rise();
            Console.WriteLine("\nRise:");
            camp.Display();

            detachment1.Remove(room1);

            Console.WriteLine("\nRemoved room #1:");
            camp.Display();
        }

        static void ExceptionTest()
        {
            Console.WriteLine("Exception test:\n");

            Console.Write("Component null name test: ");
            try
            {
                var v = new CampComposite(null);
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Component empty name test: ");
            try
            {
                var v = new CampComposite("");
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Component white-space name test: ");
            try
            {
                var v = new CampComposite("        \t");
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Composite null add test: ");
            try
            {
                var v = new CampComposite("test");
                v.Add(null);
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Composite null remove test: ");
            try
            {
                var v = new CampComposite("test");
                v.Remove(null);
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Child add test: ");
            try
            {
                var v = new Child("test");
                v.Add(new Child("c"));
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }

            Console.Write("Child remove test: ");
            try
            {
                var v = new Child("test");
                v.Remove(new Child("c"));
                Console.WriteLine("Failure");
            }
            catch { Console.WriteLine("Success"); }


        }
    }
}
