using System;

namespace Task1
{
    public abstract class CampComponent
    {
        private string _name;

        public CampComponent(string name)
        {
            this.Name = name;
        }

        public string Name {
            get
            {
                return _name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Is null, empty or consists only of white-space characters", "name");

                _name = value;
            }
        }

        public abstract void Add(CampComponent campComponent);
        public abstract void Remove(CampComponent campComponent);

        public abstract void Display(int level = 0);
        public abstract void LightsOut();
        public abstract void Rise();
    }
}
