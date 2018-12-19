using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using System.Data;

namespace MowerHelper.Models.BLL.Admin
{
    public class AdminAddress
    {
        public string Full_Name { get; set; }
        public string address { get; set; }
        public int selectId { get; set; }
        public string orderByItem { get; set; }
        public string orderBy { get; set; }
        public string noofrecords { get; set; }
        public string pageno { get; set; }
        public static int Totalnoofrecords { get; set; }
        public int loginId { get; set; }
        public int roleId { get; set; }

        public List<AdminAddress> getAddressGrid(AdminAddress objAdmin)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getAddressGrid(objAdmin);
        }
        public DataSet getcontactinfo(int roleid, int loginid)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getcontactinfo(roleid, loginid);
        }
    }
}