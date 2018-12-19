using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.LISTER
{
    public class ContactCog
    {
        private int _Contact_ID;
        private string _ContactFirstName;
        private string _ContactLastName;
        private string _ContactMail;
        private string _ContactPhone;
        private string _ContactSubject;
        private string _ContactMessage;
        private string _ContactFax;
        private string _CreatedBy;
        private string _Siteurl;
        private string _verificationcode;

        public ContactCog()
        {
        }

        public ContactCog(string ContactFirstname, string ContactLastName, string ContactMail, string Contactphone, string ContactSubject, string ContactMessage, string ContactFax, string CreatedBy)
        {
            _ContactFirstName = ContactFirstname;
            _ContactLastName = ContactLastName;
            _ContactMail = ContactMail;
            _ContactPhone = Contactphone;
            _ContactSubject = ContactSubject;
            _ContactMessage = ContactMessage;
            _ContactFax = ContactFax;
            _CreatedBy = CreatedBy;
        }
        public string key { get; set; }
        public string verificationcode
        {
            get { return _verificationcode; }
            set { _verificationcode = value; }
        }
        public string siteurl
        {
            get { return _Siteurl; }
            set { _Siteurl = value; }
        }
        public int Contact_ID
        {
            get { return _Contact_ID; }

            set { _Contact_ID = value; }
        }
        public string ContactFirstName
        {
            get { return _ContactFirstName; }

            set { _ContactFirstName = value; }
        }
        public string ContactLastName
        {
            get { return _ContactLastName; }

            set { _ContactLastName = value; }
        }
        public string ContactMail
        {
            get { return _ContactMail; }

            set { _ContactMail = value; }
        }
        public string ContactPhone
        {
            get { return _ContactPhone; }

            set { _ContactPhone = value; }
        }

        public string ContactSubject
        {
            get { return _ContactSubject; }

            set { _ContactSubject = value; }
        }

        public string ContactMessage
        {
            get { return _ContactMessage; }

            set { _ContactMessage = value; }
        }

        public string ContactFax
        {
            get { return _ContactFax; }
            set { _ContactFax = value; }
        }
        public string CreatedBy
        {
            get { return _CreatedBy; }

            set { _CreatedBy = value; }
        }
        public static bool Contact_Us(ContactCog objcont)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Contact_Us(objcont);

        }


    }
}