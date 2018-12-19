using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class WebConfigSettings
    {
        public int PageNo
        {
            get;
            set;
        }
        public int? Parameter_ID
        {
            get;
            set;
        }
        public int NoofRecords
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public string OrderByItem
        {
            get;
            set;
        }
        public string Parameter_Key
        {
            get;
            set;
        }
        public int Project_ID
        {
            get;
            set;
        }
        public int ProjectStatus_ID
        {
            get;
            set;
        }
        public string Parameter_Value
        {
            get;
            set;
        }
        public string InUse_Ind
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int UpdatedBy
        {
            get;
            set;
        }
        public int Loginhistory_ID
        {
            get;
            set;
        }
        public int CreatedBy
        {
            get;
            set;
        }
public WebConfigSettings(int? _In_Parameter_ID, int _NoofRecords, int _PageNo, string _OrderBy, string _OrderByItem, string _In_Parameter_Key)
{
    Parameter_ID = _In_Parameter_ID;
    NoofRecords = _NoofRecords;
    PageNo = _PageNo;
    OrderBy = _OrderBy;
    OrderByItem = _OrderByItem;
    Parameter_Key = _In_Parameter_Key;
}
//for list
public WebConfigSettings(int _Parameter_ID, string _Parameter_Key, string _Parameter_Value, string _InUse_Ind, string _Description, int _Project_ID, int _ProjectStatus_ID)
{
	Parameter_ID = _Parameter_ID;
	Parameter_Key = _Parameter_Key;
	Parameter_Value = _Parameter_Value;
	InUse_Ind = _InUse_Ind;
	Description = _Description;
	Project_ID = _Project_ID;
	ProjectStatus_ID = _ProjectStatus_ID;
}


public WebConfigSettings(int _In_Project_ID, int _In_ProjectStatus_ID, string _In_Parameter_Key, string _In_Parameter_Value, string _In_Inuse_Ind, string _in_Description, int _in_CreatedBy, int _Loginhistory_ID)
{
	Project_ID = _In_Project_ID;
	ProjectStatus_ID = _In_ProjectStatus_ID;
	Parameter_Key = _In_Parameter_Key;
	Parameter_Value = _In_Parameter_Value;
	InUse_Ind = _In_Inuse_Ind;
	Description = _in_Description;
	CreatedBy = _in_CreatedBy;
	Loginhistory_ID = _Loginhistory_ID;


}
public WebConfigSettings(int _In_Project_ID, int _In_Parameter_ID, string _In_Parameter_Key, string _In_Parameter_Value, string _In_Inuse_Ind, string _in_Description, int _In_ProjectStatus_ID, int _in_UpdatedBy, int _Loginhistory_ID)
{
	Project_ID = _In_Project_ID;
	Parameter_ID = _In_Parameter_ID;
	Parameter_Key = _In_Parameter_Key;
	Parameter_Value = _In_Parameter_Value;
	InUse_Ind = _In_Inuse_Ind;
	Description = _in_Description;
	ProjectStatus_ID = _In_ProjectStatus_ID;
	UpdatedBy = _in_UpdatedBy;
	Loginhistory_ID = _Loginhistory_ID;

}
        public static List<WebConfigSettings> Admin_List_WebConfigSettings(WebConfigSettings objWebConfigSettings, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Admin_List_WebConfigSettings(objWebConfigSettings,ref Total_Records);
        }
        public static void Ad_UPD_WebConfigSettings(WebConfigSettings objWebConfigSettings)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.Ad_UPD_WebConfigSettings(objWebConfigSettings);
        }

        public static void Ad_INS_WebConfigSettings(WebConfigSettings objWebConfigSettings)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.Ad_INS_WebConfigSettings(objWebConfigSettings);
        }
    }
}