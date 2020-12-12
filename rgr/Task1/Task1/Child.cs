using System;

namespace Task1
{
    public class Child : CampComponent
    {
        public bool Sleeping { get; set; }

        public Child(string name) : base(name)
        {
            Sleeping = false;
        }

        public override void Add(CampComponent campComponent)
        {
            throw new InvalidOperationException("Operation does not exist for Child");
        }

        public override void Remove(CampComponent campComponent)
        {
            throw new InvalidOperationException("Operation does not exist for Child");
        }

        public override void Display(int level = 0)
        {
            Console.WriteLine(new string('\t', level) + Name + ", sleeping: " + Sleeping.ToString());
        }

        public override void LightsOut()
        {
            Sleeping = true;
        }

        public override void Rise()
        {
            Sleeping = false;
        }
    }
}
