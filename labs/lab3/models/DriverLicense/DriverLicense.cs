using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lab1.models.Other;
using lab1.models.Drivers;

namespace lab1.models.DriverLicense
{
    [Serializable]
    public class DriverLicense
    {
        private readonly Driver owner;
        private bool is_active = true;
        private SortedDictionary<Categories, LicenseInfo> lcategories;

        public class LicenseInfo
        {
            public LicenseInfo(DateTime issued, int duration_years)
            {
                Issued = issued;
                Expires = issued.AddYears(duration_years);
            }

            public readonly DateTime Issued, Expires;
        }


        #region Constructor

        public static DriverLicense CreateDriverLicense(Driver owner)
        {
            if (owner.Age < 14) return null;
            else return new DriverLicense(owner);
        }

        private DriverLicense(Driver owner)
        {
            this.owner = owner;
            owner.DriverDeathEvent += OnOwnerDeath;
            lcategories = new SortedDictionary<Categories, LicenseInfo>();
        }

        #endregion


        #region Categories

        public bool AddCategory(Categories category, int duration, DateTime issued)
        {
            if (!is_active || duration < 2 || duration > 30)
            {
                return false;
            }

            bool unable = true;

            switch (category)
            {
                case Categories.M:
                case Categories.A1:
                case Categories.A:
                    unable = owner.Age < 16;
                    break;
                case Categories.B1:
                case Categories.B:
                case Categories.C1:
                case Categories.C:
                    unable = owner.Age < 18;
                    break;
                case Categories.D1:
                case Categories.D:
                case Categories.T:
                    unable = owner.Age < 21 || (
                        owner.GetCategoryTimeExperience(Categories.B) < 3 &&
                        owner.GetCategoryTimeExperience(Categories.C) < 3 &&
                        owner.GetCategoryTimeExperience(Categories.C1) < 3);
                    break;
                case Categories.BE:
                    unable = owner.Age < 19 ||
                        owner.GetCategoryTimeExperience(Categories.B) < 1;
                    break;
                case Categories.C1E:
                    unable = owner.Age < 19 || (
                        owner.GetCategoryTimeExperience(Categories.C) < 1 &&
                        owner.GetCategoryTimeExperience(Categories.C1) < 1);
                    break;
                case Categories.CE:
                    unable = owner.Age < 19 ||
                        owner.GetCategoryTimeExperience(Categories.C) < 1;
                    break;
                case Categories.D1E:
                    unable = owner.Age < 21 || (
                        owner.GetCategoryTimeExperience(Categories.D) < 1 &&
                        owner.GetCategoryTimeExperience(Categories.D1) < 1);
                    break;
                case Categories.DE:
                    unable = owner.Age < 21 ||
                        owner.GetCategoryTimeExperience(Categories.D) < 1;
                    break;
            }
            if (unable)
            {
                return false;
            }
            else
            {
                lcategories[category] = new LicenseInfo(issued, duration);
                return true;
            }
        }

        public void RemoveCategory(Categories category)
        {
            if (!is_active) return;
            lcategories.Remove(category);
        }

        public LicenseInfo GetCategoryInfo(Categories category)
        {
            LicenseInfo info;
            if (!lcategories.TryGetValue(category, out info)) return null;
            else return info;
        }

        public bool HasCategory(Categories category, DateTime now)
        {
            LicenseInfo info;
            if (!lcategories.TryGetValue(category, out info)) return false;
            if (info.Issued < now || info.Expires > now) return false;
            return true;
        }

        public SortedDictionary<Categories, LicenseInfo> LCategories { get => lcategories; }

        #endregion


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

        public bool IsActive { get => is_active; }
    }
}
