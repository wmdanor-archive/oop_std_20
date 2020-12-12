﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Dispencer
    {
        private readonly CocktailDict _cocktails = new CocktailDict();

        public void AddCocktail(Cocktail alcoholic, Cocktail nonAlcoholic)
        {
            _cocktails.Add(alcoholic, nonAlcoholic);
        }

        public bool RemoveCocktail(string cocktailName)
        {
            return _cocktails.Remove(cocktailName);
        }

        public Cocktail Dispence(Person person, string cocktailName)
        {
            return _cocktails[cocktailName, person.Age >= 18];
        }
    }

    public class CocktailDict
    {
        private readonly Dictionary<string, CocktailPair> _dict = new Dictionary<string, CocktailPair>();

        public void Add(Cocktail alcoholic, Cocktail nonAlcoholic)
        {
            if (alcoholic.Name != nonAlcoholic.Name) return;

            _dict.Add(alcoholic.Name, new CocktailPair { alcoholic = alcoholic, nonAlcoholic = nonAlcoholic });
        }

        public bool Remove(string name)
        {
            return _dict.Remove(name);
        }

        public Cocktail this[string name, bool alcoholic]
        {
            get
            {
                CocktailPair pair;
                if (_dict.TryGetValue(name, out pair))
                    if (alcoholic) return pair.alcoholic;
                    else return pair.nonAlcoholic;
                else return null;
            }
        }

        public struct CocktailPair
        {
            public Cocktail alcoholic, nonAlcoholic;
        }
    }

    public class Cocktail
    {
        public string Name { get; set; }
        public bool Alcoholic { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Cocktail(string name, bool alcoholic, params Ingredient[] ingredients)
        {
            Name = name;
            Alcoholic = alcoholic;
            Ingredients = new List<Ingredient>(ingredients);
        }

        public override string ToString()
        {
            string res = "Name:" + Name + "(" + (Alcoholic ? "alcoholic" : "non-alcoholic") + ")";
            return res;
        }
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

            dispencer.AddCocktail(new Cocktail("Mojito", true, new Ingredient[2]), new Cocktail("Mojito", false, new Ingredient[2]));

            var person1 = new Person("Tom", 20);
            var person2 = new Person("Jimmy", 17);

            Console.WriteLine(dispencer.Dispence(person1, "Mojito"));
            Console.WriteLine(dispencer.Dispence(person2, "Mojito"));

            Console.ReadKey();
        }
    }
}