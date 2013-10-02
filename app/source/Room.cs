using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace VNMC2013
{
    public class Room
    {
        public string Id { get; set; }
        public Person[] People = new Person[2];

        public int Person1Id
        {
            set
            {
                People[0] = Person.All.FirstOrDefault(x => x.Id == value);
            }
        }

        public int Person2Id
        {
            set
            {
                People[1] = Person.All.FirstOrDefault(x => x.Id == value);
            }
        }

        public static Room[] All()
        {
            return GlobalData.Instance.Rooms;
        }
    }
}
