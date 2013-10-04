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
        private static Activity[] all;
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

        public DateTime AlarmTime { get; set; }
    }
}
