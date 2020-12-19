using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Kiosk
    {
        private readonly Dispenser _alcoholicDispenser = new Dispenser(), _nonAlcoholicDispenser = new Dispenser();

        public void AddCocktail(string name, CocktailRecipe alcoholicRecipe, CocktailRecipe nonAlcoholicRecipe)
        {
            _alcoholicDispenser.AddCocktail(new Cocktail(name, true, alcoholicRecipe));
            _nonAlcoholicDispenser.AddCocktail(new Cocktail(name, false, nonAlcoholicRecipe));
        }

        public bool RemoveCocktail(string name)
        {
            if (!_alcoholicDispenser.RemoveCocktail(name)) return false;
            _nonAlcoholicDispenser.RemoveCocktail(name);
            return true;
        }

        public Cocktail GetCocktail(Person person, string cocktailName)
        {
            if (person.Age < 18) return _nonAlcoholicDispenser.Dispence(cocktailName);
            else return _alcoholicDispenser.Dispence(cocktailName);
        }
    }

    public class Dispenser
    {
        private readonly Dictionary<string, Cocktail> _cocktails = new Dictionary<string, Cocktail>();

        public void AddCocktail(Cocktail cocktail)
        {
            _cocktails.Add(cocktail.Name, cocktail);
        }

        public bool RemoveCocktail(string cocktailName)
        {
            return _cocktails.Remove(cocktailName);
        }

        public Cocktail Dispence(string cocktailName)
        {
            return _cocktails[cocktailName];
        }
    }

    public class Cocktail
    {
        public string Name { get; set; }
        public bool Alcoholic { get; set; }
        public CocktailRecipe Recipe { get; set; }

        public Cocktail(string name, bool alcoholic, CocktailRecipe recipe)
        {
            Name = name;
            Alcoholic = alcoholic;
            Recipe = recipe;
        }

        public override string ToString()
        {
            string res = "Name:" + Name + "(" + (Alcoholic ? "alcoholic" : "non-alcoholic") + ")";
            return res;
        }
    }

    public class CocktailRecipe
    {
        public List<Ingredient> Ingredients { get; set; }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Volume { get; set; }

        public Ingredient(string name, double volume)
        {
            Name = name;
            Volume = volume;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var dispencer = new Dispencer();

            dispencer.AddCocktail("Mojito", new List<Ingredient>(2), new List<Ingredient>(2));

            var person1 = new Person("Tom", 20);
            var person2 = new Person("Jimmy", 17);

            Console.WriteLine(dispencer.Dispence(person1, "Mojito"));
            Console.WriteLine(dispencer.Dispence(person2, "Mojito"));

            Method(person1);

            Console.ReadKey();
        }

        static void Method(Person person1)
        {
            //
        }
    }
}
