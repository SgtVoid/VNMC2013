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
    class Activity
    {
        private static Activity[] all;
        public static Activity[] All
        {
            get
            {
                if (all != null) return all;

                string json = string.Empty;
                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                StreamResourceInfo resource = Application.GetResourceStream(new Uri("/VNMC2013;component/programs.json", UriKind.Relative));

                using (StreamReader reader = new StreamReader(resource.Stream))
                {
                    json += reader.ReadToEnd();
                }
                return all = JsonConvert.DeserializeObject<Activity[]>(json);
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }

        private Person[] people;
        public Person[] People
        {
            get
            {
                if (people != null) return people;
                return people = Person.All.Where(x => x.PrimaryActivity == this.Id).ToArray();
            }
        }

        public DateTime AlarmTime { get; set; }
    }
}
