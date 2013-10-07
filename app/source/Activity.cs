using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace VNMC2013
{
    [DataContract]
    public class Activity
    {
        public static Activity[] All
        {
            get
            {
                return GlobalData.Instance.Activities;
            }
        }

        [DataMember(Name = "Id")]
        public int Id { get; set; }
        [DataMember(Name = "Title")]
        public string Name { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }

        private Person[] people;
        public Person[] People
        {
            get
            {
                if (people != null) return people;
                return people = Person.All.Where(x => x.PrimaryActivity == this.Id).ToArray();
            }
        }

        public DateTime AlarmTime
        {
            get
            {
                switch (Id)
                {
                    case 21: // Dubai City Tour (1/2 dag) en Dubai Mall
                        return new DateTime(2013, 11, 10, 8, 30, 0);
                    case 22: // Abu Dhabi Tour & Ferrari World
                        return new DateTime(2013, 11, 10, 7, 30, 0);
                    case 23: // Diepzee vissen (1/2 dag)
                        return new DateTime(2013, 11, 10, 4, 30, 0);
                    case 25: // Desert Trip (1/2 dag)
                        return new DateTime(2013, 11, 10, 8, 30, 0);
                    case 26: // Aquaventure Park
                        return new DateTime(2013, 11, 10, 8, 30, 0);
                    case 29: // Musandam Dhow Cruise
                        return new DateTime(2013, 11, 10, 7, 30, 0);
                    case 30: // Shop till you drop tour (1/2 dag)
                        return new DateTime(2013, 11, 10, 8, 0, 0);
                    case 31: // Hatta Mountain Safari   string
                        return new DateTime(2013, 11, 10, 7, 30, 0);
                    case 33: // Introductie duik
                        return new DateTime(2013, 11, 10, 4, 0, 0);
                    case 34: // Duiken
                        return new DateTime(2013, 11, 10, 4, 0, 0);
                    case 35: // Abu Dhabi Tour (zonder Ferrari World)
                        return new DateTime(2013, 11, 10, 8, 30, 0);
                    default:
                        return new DateTime();
                }
            }

        }
    }
}
