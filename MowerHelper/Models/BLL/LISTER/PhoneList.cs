using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.LISTER
{
    public class PhoneList
    {

        public static bool InslicenseRenewalsMailStatus(string ToreferenceId, string FacilityIds = null)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.InslicenseRenewalsMailStatus(ToreferenceId, FacilityIds);
        }
        public static string UPDATE_Email_Message_Status(string EmailMessage_Transaction_IDs)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.UPDATE_Email_Message_Status(EmailMessage_Transaction_IDs);
        }
    }
}