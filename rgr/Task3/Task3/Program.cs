using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Dispencer
    {
        private CocktailDict _cocktails;

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
        public List<Ingridient> Ingridients { get; set; }

        public Cocktail(string name, bool alcoholic, params Ingridient[] ingridients)
        {
            Name = name;
            Alcoholic = alcoholic;
            Ingridients = new List<Ingridient>(ingridients);
        }
    }

    public class Ingridient
    {
        public string Name { get; set; }
        public double Volume { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
