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
    class Room
    {
        public int Id { get; set; }
        public Person[] People = new Person[2];

        public int Person1Id
        {
            set
            {
                People[0] = Person.All.First(x => x.Id == value);
            }
        }

        public int Person2Id
        {
            set
            {
                People[1] = Person.All.First(x => x.Id == value);
            }
        }

        public static Room[] All()
        {
            string json = string.Empty;
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            StreamResourceInfo resource = Application.GetResourceStream(new Uri("/VNMC2013;component/rooms.json", UriKind.Relative));

            using (StreamReader reader = new StreamReader(resource.Stream))
            {
                json += reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Room[]>(json);
        }
    }
}
