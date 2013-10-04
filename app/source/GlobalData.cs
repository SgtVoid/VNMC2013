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
using VNMC2013.JSON;
using RestSharp;
using System.Runtime.Serialization.Json;


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
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

                /// Get Activities
                XmlSerializer serializer = new XmlSerializer(typeof(Activity[]));
                FileStream stream = storage.OpenFile("Activities.xml", FileMode.Open);
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


        public bool Sync(string Username, string Password)
        {
            try
            {
                var client = new RestClient
                {
                    BaseUrl = "https://insite.macaw.nl/sites/Events/VNMC2013/_vti_bin/listdata.svc",
                    Authenticator = new HttpBasicAuthenticator(Username, Password)
                };

                var request = new RestRequest();
                request.Resource = "/Activities";
                
                var ActivityAsyncHandler = client.ExecuteAsync(request, r =>
                {
                    if (r.ResponseStatus == ResponseStatus.Completed)
                    {
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                        {
                            var serializer = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Activities.RootObject));
                            var res = (VNMC2013.JSON.Activities.RootObject)serializer.ReadObject(ms);

                            foreach(var a in res.d.results) 
                            {
                                a.Description = System.Text.RegularExpressions.Regex.Replace(a.Description, "<[^>]*>", "");
                            }

                            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                            XmlSerializer ser = new XmlSerializer(typeof(Activity[]));
                            FileStream stream = storage.OpenFile("Activities.xml", FileMode.OpenOrCreate);
                            ser.Serialize(stream, res.d.results.ToArray());
                            _activities = res.d.results.ToArray();
                            stream.Close();
                        }
                    }
                });
                
                request.Resource = "/UserInformationList";
                var PeopleAsyncHandler = client.ExecuteAsync(request, r =>
                {
                    if (r.ResponseStatus == ResponseStatus.Completed)
                    {
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                        {
                            var serializer = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Peoples.RootObject));
                            var users = (VNMC2013.JSON.Peoples.RootObject)serializer.ReadObject(ms);

                            _people = users.d.results.ToArray();

                            request.Resource = "/Registration";
                            var RegistrationAsyncHandler = client.ExecuteAsync(request, s =>
                            {
                                if (s.ResponseStatus == ResponseStatus.Completed)
                                {
                                    using (var mems = new MemoryStream(Encoding.Unicode.GetBytes(s.Content)))
                                    {
                                        var serializer2 = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Registration.RootObject));
                                        var registrations = (VNMC2013.JSON.Registration.RootObject)serializer2.ReadObject(mems);

                                        _people = (from reg in registrations.d.results
                                                    join usr in _people on reg.CreatedById equals usr.Id
                                                    where reg.IkGaMeeOpHetVNMCValue == "Ja"
                                                    select new Person
                                                    {
                                                        Id = usr.Id,
                                                        FirstName = usr.FirstName,
                                                        LastName = usr.LastName,
                                                        PrimaryActivity = (int)(reg.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId == null ? -1 : reg.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId),
                                                    }).ToArray();

                                        IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                                        XmlSerializer ser = new XmlSerializer(typeof(Person[]));
                                        FileStream stream = storage.OpenFile("People.xml", FileMode.OpenOrCreate);
                                        ser.Serialize(stream, _people);
                                        stream.Close();

                                        request.Resource = "/Roomies";
                                        var RoomiesAsyncHandler = client.ExecuteAsync(request, t =>
                                        {
                                            if (t.ResponseStatus == ResponseStatus.Completed)
                                            {
                                                using (var mems2 = new MemoryStream(Encoding.Unicode.GetBytes(t.Content)))
                                                {
                                                    var serializer3 = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Roomies.RootObject));
                                                    var roomies = (VNMC2013.JSON.Roomies.RootObject)serializer3.ReadObject(mems2);

                                                    Dictionary<string, Room> Roomieslist = new Dictionary<string, Room>();

                                                    foreach (var d in roomies.d.results)
                                                    {
                                                        if (d.WieId != null)
                                                        {
                                                            if (!Roomieslist.Keys.Contains(d.Kamer)) //new room
                                                            {
                                                                Roomieslist.Add(d.Kamer, new Room() { Id = d.Kamer, Person1Id = (int)d.WieId });
                                                            }
                                                            else //existing room
                                                            {
                                                                Roomieslist[d.Kamer].Person2Id = (int)d.WieId;
                                                            }
                                                        }
                                                    }

                                                    _rooms = Roomieslist.Values.ToArray();

                                                    IsolatedStorageFile storage2 = IsolatedStorageFile.GetUserStoreForApplication();
                                                    XmlSerializer ser2 = new XmlSerializer(typeof(Room[]));
                                                    FileStream stream2 = storage.OpenFile("Rooms.xml", FileMode.OpenOrCreate);
                                                    ser.Serialize(stream, _rooms);
                                                    stream.Close();
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                });

                _isloaded = true;
                return true;
            }
            catch
            {

                return false;
            }
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
                if (_PointsOfInterest == null)
                {
                    _PointsOfInterest = new POI[] { 
                                            new POI() 
                                            { 
                                                Name = "Premier Inn Dubai Investments Park",
                                                AddressLine1 = "Green Community Village",
                                                AddressLine2 = "Dubai",
                                                AddressLine3 = "United Arab Emirates",
                                                ImagePath = "/Assets/POIs/hotel.PNG",
                                                Website = "",
                                                GeoLat = 25.008165,
                                                GeoLong = 55.156748,
                                                Phone = "+971 4 885 0999"

                                            }, 
                                            new POI() 
                                            { 
                                                Name = "Meydan Beach Club",
                                                AddressLine1 = "Meydan Beach Club",
                                                AddressLine2 = "Dubai",
                                                AddressLine3 = "United Arab Emirates",
                                                ImagePath = "/Assets/POIs/beachclub.PNG",
                                                Website = "",
                                                GeoLat = 25.081153,
                                                GeoLong = 55.136013,
                                                Phone = "+971 4 433 3777"
                                            }, 
                                            new POI() 
                                            { 
                                                Name = "Aquaventure Waterpark",
                                                AddressLine1 = "Atlantis The Palm",
                                                AddressLine2 = "Crescent Road",
                                                AddressLine3 = "Palm Island - Dubai",

                                                ImagePath = "http://www.bestourism.com/img/items/big/104/United-Arab-Emirates_Aquaventure-Dubai_338.jpg67",
                                                Website = "",

                                                GeoLat = 25.13346,
                                                GeoLong = 55.120012,
                                                Phone = "+971 4 426 0000"
                                            } , 
                                            new POI() 
                                            { 
                                                Name = "Montgomerie Golf Club",
                                                AddressLine1 = "Emirates Hills 3",
                                                AddressLine2 = "Dubai",
                                                AddressLine3 = "United Arab Emirates",

                                                ImagePath = "https://lh4.googleusercontent.com/-CYz-RSoXP_w/UUDk9W9eq8I/AAAAAAAAAA8/UagXiJSEJfk/w816-h612-no/Montgomerie+Golf+Club",
                                                Website = "http://www.themontgomerie.com",

                                                GeoLat = 25.008165,
                                                GeoLong = 55.156748,
                                                Phone = "+971 4 390 5600"
                                            } , 
                                            new POI() 
                                            { 
                                                Name = "Burj Al Arab",
                                                AddressLine1 = "Burj Al Arab",
                                                AddressLine2 = "Dubai",
                                                AddressLine3 = "United Arab Emirates",

                                                ImagePath = "/Assets/POIs/Burj-Al-Arab.jpg",
                                                Website = "http://www.burjalarab.com‎",

                                                GeoLat = 25.140963,
                                                GeoLong = 55.185018,
                                                Phone = "+971 4 301 7777"
                                            } 
                                        };
                }
                return _PointsOfInterest;
            }
        }


    }
}
