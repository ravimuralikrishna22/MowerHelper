using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.LISTER
{
    public class ListContactInfo
    {
        public int Contact_ID { get; set; }
        public string ContactMail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactSubject { get; set; }
        public string DateOfSubmit { get; set; }
        public string ContactMessage { get; set; }
        public string ContactName { get; set; }
        public string ContactLastName { get; set; }
        public int NoOfRecords { get; set; }
        public string OrderByitem { get; set; }
        public int PageNo { get; set; }
        public string ReplyStatus_Ind { get; set; }
        public static int TotalRecords { get; set; }
        public string OrderBy { get; set; }
        public string ReplyStatus { get; set; }
        public string ContactFirstName { get; set; }
        public static List<ListContactInfo> List_ContactUs(ListContactInfo objlstCont)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.List_ContactUs(objlstCont);

        }
        public static ListContactInfo View_Contacts(int Contact_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.View_Contacts(Contact_ID);

        }
        public static ListContactInfo UPD_ReplyStatus(int Contact_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.UPD_ReplyStatus(Contact_ID);
        }
        public static bool DEL_Contacts(int Contact_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.DEL_Contacts(Contact_ID);
        }


    }


}