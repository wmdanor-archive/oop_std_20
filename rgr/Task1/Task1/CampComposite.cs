using System;
using System.Collections.Generic;

namespace Task1
{
    public class CampComposite : CampComponent
    {
        private List<CampComponent> _children = new List<CampComponent>();

        public CampComposite(string name) : base(name) { }

        public override void Add(CampComponent campComponent)
        {
            _children.Add(campComponent ?? throw new ArgumentNullException("campComponent"));
        }

        public override void Remove(CampComponent campComponent)
        {
            _children.Remove(campComponent ?? throw new ArgumentNullException("campComponent"));
        }

        public override void Display(int level = 0)
        {
            Console.WriteLine(new string('\t', level) + Name);

            foreach (var child in _children)
                child.Display(level + 1);
        }

        public override void LightsOut()
        {
            foreach (var child in _children)
                child.LightsOut();
        }

        public override void Rise()
        {
            foreach (var child in _children)
                child.Rise();
        }
    }
}
