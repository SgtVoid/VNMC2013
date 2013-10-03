using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        void Update(string user, string password);

        [OperationContract]
        Activity[] GetActivities();

        [OperationContract]
        Person[] GetPeople();

        [OperationContract]
        Room[] GetRoomies();
        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Person
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public int PrimaryActivity { get; set; }
    }

    [DataContract]
    public class Activity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
    }

    [DataContract]
    public class Room
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int? Person1Id { get; set; }
        [DataMember]
        public int? Person2Id { get; set; }
    }
}
