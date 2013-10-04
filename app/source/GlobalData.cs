using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.UserData;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using VNMC2013;
using VNMC2013.Data;


namespace VNMC2013
{
    class GlobalData
    {
        Person[] _people;
        Room[] _rooms;
        Activity[] _activities;

        public delegate void ContactsLoadedHandler();
        public event ContactsLoadedHandler OnContactsLoaded;

        private static GlobalData _instance = null;
        public static GlobalData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalData();

                return _instance;
            }
        }

        bool _isloaded = false;
        public bool IsLoaded
        {
            get
            {
                return _isloaded;
            }
        }

        public ReadOnlyCollection<Microsoft.Phone.UserData.Contact> Contacts
        {
            get;
            private set;
        }

        private GlobalData()
        {
            Contacts = null;
        }

        public void SetContacts(ReadOnlyCollection<Microsoft.Phone.UserData.Contact> contacts)
        {
            if (Contacts == null)
            {
                Contacts = contacts;

                if (OnContactsLoaded != null)
                    OnContactsLoaded();
            }
        }


        public bool Load()
        {
            try
            {
                ///Get POIs
                XmlSerializer serializer = new XmlSerializer(typeof(POI[]));
                Stream stream = System.Windows.Application.GetResourceStream(new Uri("VNMC2013;component/Assets/POI.xml", UriKind.Relative)).Stream;
                _PointsOfInterest = (POI[])serializer.Deserialize(stream);


                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                /// Get Activities
                serializer = new XmlSerializer(typeof(Activity[]));
                stream = storage.OpenFile("Activities.xml", FileMode.Open);
                _activities = (Activity[])serializer.Deserialize(stream);

                /// Get Rooms
                serializer = new XmlSerializer(typeof(Room[]));
                stream = storage.OpenFile("Rooms.xml", FileMode.Open);
                _rooms = (Room[])serializer.Deserialize(stream);

                /// Get People
                serializer = new XmlSerializer(typeof(Person[]));
                stream = storage.OpenFile("People.xml", FileMode.Open);
                _people = (Person[])serializer.Deserialize(stream);




                _isloaded = true;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Sync()
        {
            try
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                XmlSerializer serializer;
                FileStream stream;

                VNMCData.Service1Client service = new VNMCData.Service1Client();


                service.GetActivitiesCompleted += (object o, VNMC2013.VNMCData.GetActivitiesCompletedEventArgs args) =>
                {
                    var activities = args.Result;

                    var appactivities = (from x in activities
                                         select new Activity()
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Description = System.Text.RegularExpressions.Regex.Replace(x.Description, "<[^>]*>", "")
                                         }).ToArray();


                    serializer = new XmlSerializer(typeof(Activity[]));
                    stream = storage.OpenFile("Activities.xml", FileMode.OpenOrCreate);
                    serializer.Serialize(stream, appactivities);
                    _activities = appactivities;
                    stream.Close();
                };
                service.GetActivitiesAsync();

                service.GetPeopleCompleted += (object o, VNMC2013.VNMCData.GetPeopleCompletedEventArgs args) =>
                {
                    var peeps = args.Result.ToArray();

                    var applicationPeople = (from x in peeps
                                             select new Person()
                                             {
                                                 Id = x.Id,
                                                 FirstName = x.FirstName.titelize(),
                                                 LastName = x.LastName.titelize(),
                                                 PrimaryActivity = x.PrimaryActivity
                                             }).ToArray();

                    serializer = new XmlSerializer(typeof(Person[]));
                    stream = storage.OpenFile("People.xml", FileMode.OpenOrCreate);
                    serializer.Serialize(stream, applicationPeople);
                    _people = applicationPeople;
                    stream.Close();

                    GetRoomies();
                    foreach (var p in GlobalData.Instance.People) { p.LoadPhoto(); }
                };
                service.GetPeopleAsync();


                _isloaded = true;
                return true;
            }
            catch
            {

                return false;
            }
        }


        private void GetRoomies()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            XmlSerializer serializer;
            FileStream stream;

            VNMCData.Service1Client service = new VNMCData.Service1Client();

             service.GetRoomiesCompleted += (object o, VNMC2013.VNMCData.GetRoomiesCompletedEventArgs args) =>
                {
                    var roomies = args.Result.ToArray();

                    var approomies = (from x in roomies
                                      select new Room()
                                      {
                                          Id = x.Id,
                                          Person1Id = (int)(x.Person1Id == null ? -1 : x.Person1Id),
                                          Person2Id = (int)(x.Person2Id == null ? -1 : x.Person2Id),
                                      }).ToArray();

                    serializer = new XmlSerializer(typeof(Room[]));
                    stream = storage.OpenFile("Rooms.xml", FileMode.OpenOrCreate);
                    serializer.Serialize(stream, approomies);
                    _rooms = approomies;
                    stream.Close();
                };
                service.GetRoomiesAsync();
        }


        public Person[] People
        {
            get
            {
                return _people;
            }
        }

        public Activity[] Activities
        {
            get
            {
                return _activities;
            }
        }

        public Room[] Rooms
        {
            get
            {
                return _rooms;
            }
        }




        ///TEST!
        ///
        private POI[] _PointsOfInterest = null;
        public POI[] PointsOfInterest
        {
            get
            {
                return _PointsOfInterest;
            }
        }


    }
}
