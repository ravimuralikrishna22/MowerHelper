using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class pagenumber
    {
        public int? Section_ID { get; set; }
        public int? Page_ID { get; set; }
        public int? Application_ID { get; set; }
        public int NoOfRecords { get; set; }
        public int PageNo { get; set; }
        public string OrderBy { get; set; }
        public string OrderBYitem { get; set; }
        public string Notes { get; set; }
        public string PagePath { get; set; }
        public int slno { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string DisplayLevel { get; set; }
        public string DisplayName { get; set; }
        public string Sectionname { get; set; }
        public pagenumber()
        {
        }

        public pagenumber(int _Section_ID, string _Sectionname, string _SectionPath, string _SectionTitle, string _SectionMenuTitle, string _SectionDescription, bool _Section_isEnabled, int _Section_ParentID, bool _Section_isParent, bool _Section_IsSystem,
bool _service_Ind, string _DisplayName, string _Level_No, string _DisplayLevel, bool _OpenInNewWindow_Ind, string _ParentName)
{
	if (_Section_ID != 0) {
		Section_ID = _Section_ID;
	}
	Sectionname = _Sectionname;
   
}
        public pagenumber(int _Section_ID, string _Sectionname)
        {
            if (_Section_ID != 0)
            {
                Section_ID = _Section_ID;
            }
            Sectionname = _Sectionname;
        }

        public pagenumber(int? in_Section_ID, int? in_Page_ID, int in_Application_ID, int in_PageNo, int in_NoOfRecords, string in_OrderBy, string in_OrderBYitem)
{
	if (in_Section_ID != 0) {
		Section_ID = in_Section_ID;
	}
	if (in_Page_ID != 0) {
		Page_ID = in_Page_ID;
	}
	Application_ID = in_Application_ID;
	NoOfRecords = in_NoOfRecords;
	PageNo = in_PageNo;
	OrderBy = in_OrderBy;
	OrderBYitem = in_OrderBYitem;
}
        public pagenumber(int? in_sectionid, string _pagepath, string _notes, int _applicationid, int _createdby)
{
	Section_ID = in_sectionid;
	PagePath = _pagepath;
	Notes = _notes;
	Application_ID = _applicationid;
	CreatedBy = _createdby;
}
        public pagenumber(int? in_sectionid, int? in_pageid, int in_Application_ID)
{
	Section_ID = in_sectionid;
	Page_ID = in_pageid;
    Application_ID = in_Application_ID;
}
        public pagenumber(int in_Pageid, Nullable<int> in_Section_ID, string in_pagepath, string in_notes, int in_Application_ID, int in_UpdatedBy)
{
	Page_ID = Convert.ToInt32(in_Pageid);
	Section_ID = Convert.ToInt32(in_Section_ID);
	PagePath = in_pagepath;
	Notes = in_notes;
	Application_ID = in_Application_ID;
	UpdatedBy = in_UpdatedBy;
}
        
        public static List<pagenumber> admin_listpagenumbers(pagenumber objlistpages, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.admin_listpagenumbers(objlistpages,ref Total_Records);
        }

        public static void updatepagelist(pagenumber objview, ref string outmsg)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.updatepagelist(objview,ref outmsg);
        }

        public static void inserpagenumber(pagenumber objview, ref string outmsg)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.inserpagenumber(objview,ref outmsg);
        }

        public static void del_listpagenumbers(int Page_ID, int Application_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.del_listpagenumbers(Page_ID, Application_ID);
        }

        public static List<pagenumber> viewpagelist(pagenumber objview)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.viewpagelist(objview);
        }

        public static List<pagenumber> dropdownlist(Nullable<int> Section_ID, string SearchBy)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.dropdownlist(Section_ID, SearchBy);
        }
    }
}