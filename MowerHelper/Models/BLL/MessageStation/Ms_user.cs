using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class Ms_user
    {
        public int Login_ID
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public Ms_user(int _Login_ID, string _Username)
{
	Login_ID = _Login_ID;
    Username = _Username;
}
        public static Ms_UserCollection GetMs_usersByRoleid(int intSearchType, int RoleID, int PageNo, ref string DisplayName, ref int Total_Records, string lastname = null)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetUsersByRoleid(intSearchType, RoleID, PageNo,ref DisplayName,ref Total_Records, lastname);
        }
    }
}