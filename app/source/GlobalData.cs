﻿using System;
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
        public delegate void ContactsLoadedHandler();
        public event ContactsLoadedHandler OnContactsLoaded;

        public delegate void SyncCompletedHandler();
        public event SyncCompletedHandler OnSyncCompleted;

        public delegate void UpdateProgressHandler(int percentage);
        public event UpdateProgressHandler OnUpdateProgress;

        public delegate void SyncErrorHandler();
        public event SyncErrorHandler OnSyncError;

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

        private bool _isloaded = false;
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

        private Person[] _people;
        public Person[] People
        {
            get
            {
                return _people;
            }
        }

        private Activity[] _activities;
        public Activity[] Activities
        {
            get
            {
                return _activities;
            }
        }

        private Room[] _rooms;
        public Room[] Rooms
        {
            get
            {
                return _rooms;
            }
        }

        private POI[] _PointsOfInterest = null;
        public POI[] PointsOfInterest
        {
            get
            {
                return _PointsOfInterest;
            }
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
                // Get Messages
                var t = MessageCollection.Instance;

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

                return _isloaded = true;
            }
            catch
            {
                return false;
            }
        }

        public async void Sync(string Username, string Password)
        {
            try
            {
                RestClient client = new RestClient
                {
                    BaseUrl = "https://insite.macaw.nl/sites/Events/VNMC2013/_vti_bin/listdata.svc",
                    Authenticator = new HttpBasicAuthenticator(Username, Password)
                };

                RestRequest request = new RestRequest();

                request.Resource = "/Activities";
                OnUpdateProgress(5);
                ActivityAsyncHandler(await client.ExecuteTaskAsync(request));
                OnUpdateProgress(15);
                 
                request.Resource = "/UserInformationList";
                UserInformationAsyncHandler(await client.ExecuteTaskAsync(request));
                OnUpdateProgress(30);

                request.Resource = "/Registration";
                PeopleAsyncHandler(await client.ExecuteTaskAsync(request));
                OnUpdateProgress(30);

                request.Resource = "/Roomies";
                RoomiesAsyncHandler(await client.ExecuteTaskAsync(request));

                OnSyncCompleted();

                 foreach (var p in GlobalData.Instance.People) { p.LoadPhoto(); }
                _isloaded = true;
            }
            catch
            {
                OnSyncError();
            }
        }

        private void ActivityAsyncHandler(IRestResponse r)
        {
            if (r.ResponseStatus == ResponseStatus.Completed)
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Activities.RootObject));
                    var res = (VNMC2013.JSON.Activities.RootObject)serializer.ReadObject(ms);

                    if (res == null) { throw new Exception(); }
                    foreach (var a in res.d.results)
                    {
                        a.Description = System.Text.RegularExpressions.Regex.Replace(a.Description, "<[^>]*>", "");
                    }

                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    XmlSerializer ser = new XmlSerializer(typeof(Activity[]));
                    FileStream stream = storage.OpenFile("Activities.xml", FileMode.Create);

                    ser.Serialize(stream, res.d.results.ToArray());
                    _activities = res.d.results.ToArray();

                    stream.Close();
                }
            }
        }

        private void UserInformationAsyncHandler(IRestResponse r)
        {
            if (r.ResponseStatus == ResponseStatus.Completed)
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Peoples.RootObject));
                    var users = (VNMC2013.JSON.Peoples.RootObject)serializer.ReadObject(ms);

                    _people = users.d.results.ToArray();
                }
            }
        }

        private void PeopleAsyncHandler(IRestResponse r)
        {
            if (r.ResponseStatus == ResponseStatus.Completed)
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Registration.RootObject));
                    var registrations = (VNMC2013.JSON.Registration.RootObject)serializer.ReadObject(ms);

                    _people = (from reg in registrations.d.results
                               join usr in _people on reg.CreatedById equals usr.Id
                               where reg.IkGaMeeOpHetVNMCValue == "Ja"
                               select new Person
                               {
                                   Id = usr.Id,
                                   AccountName = usr.AccountName,
                                   FirstName = usr.FirstName,
                                   LastName = usr.LastName,
                                   PrimaryActivity = (int)(reg.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId == null ? -1 : reg.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId),
                               }).ToArray();

                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    XmlSerializer ser = new XmlSerializer(typeof(Person[]));
                    FileStream stream = storage.OpenFile("People.xml", FileMode.Create);
                    ser.Serialize(stream, _people);
                    stream.Close();
                }
            }
        }

        private void RoomiesAsyncHandler(IRestResponse r)
        {
            if (r.ResponseStatus == ResponseStatus.Completed)
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(r.Content)))
                {
                    var serializer3 = new DataContractJsonSerializer(typeof(VNMC2013.JSON.Roomies.RootObject));
                    var roomies = (VNMC2013.JSON.Roomies.RootObject)serializer3.ReadObject(ms);

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

                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    XmlSerializer ser = new XmlSerializer(typeof(Room[]));
                    FileStream stream = storage.OpenFile("Rooms.xml", FileMode.Create);
                    ser.Serialize(stream, _rooms);
                    stream.Close();
                }
            }
        }
    }
}