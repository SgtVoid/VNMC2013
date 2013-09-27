﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.UserData;

namespace VNMC2013
{
    class GlobalData
    {
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
            }
        }
    }
}