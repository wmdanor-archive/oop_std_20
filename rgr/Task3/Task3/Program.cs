using System;
using System.Collections.Generic;

namespace Task3
{
    public class Kiosk
    {
        private readonly Dictionary<string, Cocktail> _alcoholic = new Dictionary<string, Cocktail>(),
                                                      _nonAlcoholic = new Dictionary<string, Cocktail>();
        private readonly CocktailDispenserStrategy _dispenser = new CocktailDispenserStrategy();

        public void AddCocktail(string name, CocktailRecipe alcoholicRecipe, CocktailRecipe nonAlcoholicRecipe)
        {
            _alcoholic.Add(name, new Cocktail(name, true, alcoholicRecipe));
            _nonAlcoholic.Add(name, new Cocktail(name, false, nonAlcoholicRecipe));
        }

        public bool RemoveCocktail(string name)
        {
            if (!_alcoholic.Remove(name)) return false;
            _nonAlcoholic.Remove(name);
            return true;
        }

        public Cocktail GetCocktail(Person person, string cocktailName)
        {
            SetDispenser(person);

            return _dispenser.GetCocktail(cocktailName);
        }

        private void SetDispenser(Person person)
        {
            if (person.Age < 18) _dispenser.SetStrategy(new AlcoholicDispenser(_alcoholic));
            else _dispenser.SetStrategy(new NonAlcoholicDispenser(_nonAlcoholic));
        }
    }

    public class CocktailDispenserStrategy
    {
        private ICocktailDispenser _cocktailDispenser;

        public Cocktail GetCocktail(string name)
        {
            if (_cocktailDispenser == null) throw new InvalidOperationException("Cocktail dispenser is not defined");

            return _cocktailDispenser.DispenseCocktail(name);
        }

        public void SetStrategy(ICocktailDispenser cocktailDispenser)
        {
            _cocktailDispenser = cocktailDispenser;
        }
    }

    public class AlcoholicDispenser : ICocktailDispenser
    {
        private readonly IDictionary<string, Cocktail> _cocktails = new Dictionary<string, Cocktail>();

        public AlcoholicDispenser(IDictionary<string, Cocktail> cocktails)
        {
            foreach (var pair in cocktails)
            {
                if (!pair.Value.Alcoholic) throw new ArgumentException("Cocktails must be alcoholic");
            }

            _cocktails = cocktails;
        }

        public Cocktail DispenseCocktail(string cocktailName)
        {
            return _cocktails[cocktailName];
        }
    }

    public class NonAlcoholicDispenser : ICocktailDispenser
    {
        private readonly IDictionary<string, Cocktail> _cocktails = new Dictionary<string, Cocktail>();

        public NonAlcoholicDispenser(IDictionary<string, Cocktail> cocktails)
        {
            foreach (var pair in cocktails)
            {
                if (pair.Value.Alcoholic) throw new ArgumentException("Cocktails must be non-alcoholic");
            }

            _cocktails = cocktails;
        }

        public Cocktail DispenseCocktail(string cocktailName)
        {
            return _cocktails[cocktailName];
        }
    }

    public interface ICocktailDispenser
    {
        Cocktail DispenseCocktail(string cocktailName);
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
            var kiosk = new Kiosk();

            kiosk.AddCocktail("Mojito", new CocktailRecipe(), new CocktailRecipe());

            var majorPerson = new Person("Tom", 20);
            var minorPerson = new Person("Jimmy", 17);

            Console.WriteLine(kiosk.GetCocktail(majorPerson, "Mojito"));
            Console.WriteLine(kiosk.GetCocktail(minorPerson, "Mojito"));

            Console.ReadKey();
        }
    }
}
