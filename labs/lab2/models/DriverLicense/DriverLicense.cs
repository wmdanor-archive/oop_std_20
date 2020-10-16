using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;
using lab1.models.Drivers;

namespace lab1.models.DriverLicense
{
    class DriverLicense
    {
        public class LicenseInfo
        {
            public LicenseInfo(DateTime issued, int duration_years)
            {
                Issued = issued;
                Expires = issued.AddYears(duration_years);
            }

            public readonly DateTime Issued, Expires;
        }

        private DriverLicense(Driver owner)
        {
            this.owner = owner;
            is_active = true;
            owner.DriverDeathEvent += OnOwnerDeath;
            lcategories = new SortedDictionary<Categories, LicenseInfo>();
        }

        public static DriverLicense CreateDriverLicense(Driver owner)
        {
            if (owner.Age < 14) return null;
            else return new DriverLicense(owner);
        }

        private bool is_active;

        public bool IsActive { get => is_active; }

        private SortedDictionary<Categories, LicenseInfo> lcategories;

        public SortedDictionary<Categories, LicenseInfo> LCategories { get => lcategories; }

        public void OnOwnerDeath()
        {
            this.is_active = false;
        }

        public void RevokeLicense()
        {
            this.is_active = false;
        }

        public void ReestablishLicense()
        {
            this.is_active = true;
        }

        public bool AddCategory(Categories category, int duration, DateTime issued)
        {
            if (!is_active) return false;
            
            if (duration < 2 || duration > 30) return false;
            switch (category)
            {
                case Categories.M:
                case Categories.A1:
                case Categories.A:
                    if (owner.Age < 16) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.B1:
                case Categories.B:
                case Categories.C1:
                case Categories.C:
                    if (owner.Age < 18) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.D1:
                case Categories.D:
                case Categories.T:
                    if (owner.Age < 21) return false;
                    bool check = owner.GetCategoryTimeExperience(Categories.B) < 3 &&
                                 owner.GetCategoryTimeExperience(Categories.C) < 3 &&
                                 owner.GetCategoryTimeExperience(Categories.C1) < 3;
                    if (check) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.BE:
                    if (owner.Age < 19) return false;
                    if (owner.GetCategoryTimeExperience(Categories.B) < 1) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.C1E:
                    if (owner.Age < 19) return false;
                    if (owner.GetCategoryTimeExperience(Categories.C) < 1 &&
                        owner.GetCategoryTimeExperience(Categories.C1) < 1) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.CE:
                    if (owner.Age < 19) return false;
                    if (owner.GetCategoryTimeExperience(Categories.C) < 1) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.D1E:
                    if (owner.Age < 21) return false;
                    if (owner.GetCategoryTimeExperience(Categories.D) < 1 &&
                        owner.GetCategoryTimeExperience(Categories.D1) < 1) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
                case Categories.DE:
                    if (owner.Age < 21) return false;
                    if (owner.GetCategoryTimeExperience(Categories.D) < 1) return false;
                    lcategories[category] = new LicenseInfo(issued, duration);
                    break;
            }
            return true;
        }

        public void RemoveCategory(Categories category)
        {
            if (!is_active) return;
            lcategories.Remove(category);
        }

        public bool HasCategory(Categories category, DateTime now)
        {
            LicenseInfo info;
            if (!lcategories.TryGetValue(category, out info)) return false;
            if (info.Issued < now || info.Expires > now) return false;
            return true;
        }

        public LicenseInfo GetCategoryInfo(Categories category)
        {
            LicenseInfo info;
            if (!lcategories.TryGetValue(category, out info)) return null;
            else return info;
        }

        private readonly Driver owner;
    }
}
