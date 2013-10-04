using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace VNMC2013
{
    public class Room
    {
        public string Id { get; set; }

        public int Person1Id { get; set; }

        public int Person2Id { get; set; }

        public Person Person1
        {
            get
            {
                return Person.All.FirstOrDefault(x => x.Id == Person1Id);
            }
        }

        public Person Person2
        {
            get
            {
                return Person.All.FirstOrDefault(x => x.Id == Person2Id);
            }
        }

        public static Room[] All
        {
            get { return GlobalData.Instance.Rooms; }
        }
    }
}
