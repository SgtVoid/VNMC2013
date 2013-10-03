using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;


namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        const string azureLocalResourceNameFromServiceDefinition = "DataStore";
        public void Update(string user, string password)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("vnmc2013blob");
            

            
            var appRoot = RoleEnvironment.GetLocalResource(azureLocalResourceNameFromServiceDefinition).RootPath;




            XmlSerializer serializer;
            Stream stream;




            Data.VNMC2013DataContext dc =
               new Data.VNMC2013DataContext(new Uri("https://insite.macaw.nl/sites/Events/VNMC2013/_vti_bin/listdata.svc"));




            

            var cred = new System.Net.NetworkCredential(user, password);
            dc.Credentials = cred;

            var userlist = (from x in dc.UserInformationList where x.ContentType != "SharePointGroup" select x).ToList();
            var reglist = dc.Registration.ToList();

            var PeopleResult = from d in reglist
                               join usr in userlist on d.CreatedById.Value equals usr.CreatedById.Value
                               where d.IkGaMeeOpHetVNMCValue == "Ja"
                               select new Person
                               {
                                   Id = d.CreatedById.Value,
                                   FirstName = usr.FirstName,
                                   LastName = usr.LastName,
                                   PrimaryActivity = (int)(d.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId == null ? -1 : d.EersteKeuzeActiviteitOpZondagWijzigenVanJeUiteindelijkeKeuzeIsMogelijkTotBeginOktoberId),
                               };

            serializer = new XmlSerializer(typeof(Person[]));

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("People");
            stream = blockBlob.OpenWrite();

            serializer.Serialize(stream, PeopleResult.ToArray());
            stream.Close();

            var RoomiesResult = from d in dc.Roomies
                                select new
                                {
                                    Id = d.Kamer,
                                    Pid = d.WieId,
                                };

            Dictionary<string, Room> Roomieslist = new Dictionary<string, Room>();

            foreach (var d in RoomiesResult)
            {
                if (d.Pid != null)
                {
                    if (!Roomieslist.Keys.Contains(d.Id)) //new room
                    {
                        Roomieslist.Add(d.Id, new Room() { Id = d.Id, Person1Id = d.Pid });
                    }
                    else //existing room
                    {
                        Roomieslist[d.Id].Person2Id = d.Pid;
                    }
                }
            }

            serializer = new XmlSerializer(typeof(Room[]));

            blockBlob = container.GetBlockBlobReference("Rooms");
            stream = blockBlob.OpenWrite();

            serializer.Serialize(stream, Roomieslist.Values.ToArray());
            stream.Close();

            var ActivityResult = (from d in dc.Activities
                                 select new Activity
                                 {
                                     Id = d.Id,
                                     Name = d.Title,
                                     Description = d.Description
                                 }).ToArray();

            serializer = new XmlSerializer(typeof(Activity[]));

            blockBlob = container.GetBlockBlobReference("Activities");
            stream = blockBlob.OpenWrite();

            serializer.Serialize(stream, ActivityResult);
            stream.Close();

        }


        public Activity[] GetActivities()
        {
            try
            {
                var appRoot = RoleEnvironment.GetLocalResource(azureLocalResourceNameFromServiceDefinition).RootPath;
                XmlSerializer serializer = new XmlSerializer(typeof(Activity[]));

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("vnmc2013blob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("Activities");
                Stream stream = blockBlob.OpenRead();

                Activity[] Activities = (Activity[])serializer.Deserialize(stream);

                return Activities;
            }
            catch
            {
                return new Activity[] { };
            }
        }


        public Person[] GetPeople()
        {
            try
            {
                var appRoot = RoleEnvironment.GetLocalResource(azureLocalResourceNameFromServiceDefinition).RootPath;
                XmlSerializer serializer = new XmlSerializer(typeof(Person[]));



                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("vnmc2013blob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("People");
                Stream stream = blockBlob.OpenRead();


                Person[] People = (Person[])serializer.Deserialize(stream);

                return People;
            }
            catch
            {
                return new Person[] { };
            }
        }


        public Room[] GetRoomies()
        {
            try
            {
                var appRoot = RoleEnvironment.GetLocalResource(azureLocalResourceNameFromServiceDefinition).RootPath;
                XmlSerializer serializer = new XmlSerializer(typeof(Room[]));

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("vnmc2013blob");
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("Rooms");
                Stream stream = blockBlob.OpenRead();


                Room[] rooms = (Room[])serializer.Deserialize(stream);

                return rooms;
            }
            catch
            {
                return new Room[] { };
            }
        }
    }
}
