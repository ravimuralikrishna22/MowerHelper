using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.CDAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class SitePage
    {
        public string Loginhistory_ID { get; set; }
        public int? SiteCategory_ID { get; set; }
        public string Subject_Text { get; set; }
        public string Subject_Title { get; set; }
        public int? SitePage_ID { get; set; }
        public int NoOfRecords { get; set; }
        public int pageNo { get; set; }
        public int? Application_ID { get; set; }
        public string OrderBy { get; set; }
        public string OrderByItem { get; set; }
        public string Category { get; set; }
        public string ApplicationName { get; set; }
        
        
        public static bool Save(SitePage obj)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_Insert_SitePage(obj);
        }

        public static List<SitePage> ADMIN_List_SitePages(SitePage obj, ref int Total_records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_List_SitePages(obj,ref Total_records);
        }

        public static SitePage ADMIN_GET_SitePage(int SitePage_id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_GET_SitePage(SitePage_id);
        }
        public static bool Update(SitePage obj)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_Update_SitePage(obj);
        }

    }
}