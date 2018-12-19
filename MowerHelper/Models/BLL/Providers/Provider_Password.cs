using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_Password
    {
        public string Password
        {
            get;
            set;
        }
        public int? Login_ID
        {
            get;
            set;
        }
        public static bool InsertPassword(string Password, int? UpdatedBy, int? Login_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.InsertPassword(Password, UpdatedBy, Login_ID);
        }
        public static bool Temp_Ins_Password(Provider_Password obj)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Temp_Ins_Password(obj);
        }
    }
}