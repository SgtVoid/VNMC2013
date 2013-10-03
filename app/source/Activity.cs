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
    public class Activity
    {
        private static Activity[] all;
        public static Activity[] All
        {
            get
            {
                return GlobalData.Instance.Activities;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
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

        public DateTime AlarmTime { get; set; }
    }
}
