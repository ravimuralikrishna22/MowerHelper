using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.BLL.ElectricianServices;
using MowerHelper.Models.BLL.Forums;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Practice;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.BLL.SERVICESMENU;
using MowerHelper.Models.BLL.Tasks;
using MowerHelper.Models.BLL.Technicians;
using MowerHelper.Models.Classes;
using MowerHelper.Models.DAL.DataAccessLayer;
namespace MowerHelper.Models.DAL
{

    public class SQLDataAccessLayer : DataAccessLayerBaseClass
    {
        string ClassName = "SQLDataAccessLayer";
        delegate CollectionBase GenerateCollectionFromReader(IDataReader returnData);
        delegate CollectionBase GenerateCollectionFromReaderTR(IDataReader returnData, ref int Total_Records);
        public List<StatesList> GetStatesBasedOnDirectoryID()
        {
            var objDataList = new List<StatesList>();
            try
            {
                var objcommon = new clsCommonFunctions();
                var ds = objcommon.GetDataSet("Help_dbo.st_Provider_LIST_DirectoryStates");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new StatesList { State_ID = Convert.ToInt32(dr["State_ID"]), State_Name = Convert.ToString(dr["State_Name"]), ind = "Statelist", count = Convert.ToInt32(ds.Tables[0].Rows.Count) });
                }
                else
                {
                    objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new StatesList { ind = "Nolist is available" });
                }

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetStatesBasedOnDirectoryID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            var obj1 = new StatesList();
            return objDataList;
        }
        public override DataSet GetZipBasedOnLatLon(string latitude, string longitude)
        {
            DataSet ds = null;
             try
            {
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objInparamList = {
	                                     new SqlParameter("@in_latitude", latitude),
	                                     new SqlParameter("@in_longitude", longitude)
                                               };
            objcommon.AddInParameters(objInparamList);
             ds = objcommon.GetDataSet("Help_dbo.St_get_zip_with_latlong");
             return ds;
            }
             catch (Exception ex)
             {
                 var obj = new clsExceptionLog();
                 obj.LogException(ex, ClassName, "GetZipBasedOnLatLon", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
             }
             return ds;
        }
        public override DataSet list_Featuredtherapists(Provider_AdvancedSearch obj)
        {
            try
            {
                var clscommon = new clsCommonFunctions();

                IDataParameter[] inParameter = {new SqlParameter("@In_Zip", (obj.Zip ?? string.Empty)), 
                                                        new SqlParameter("@In_IsSearch", (obj.IsSearch ?? string.Empty)), 
                                                        new SqlParameter("@In_city_id", (obj.City_ID == 0 ? null: obj.City_ID)), 
                                                        new SqlParameter("@In_State_id", (obj.State_ID == 0 ? null: obj.State_ID)), 
                                                        new SqlParameter("@In_StateName", (obj.StateName ?? string.Empty)), 
                                                        new SqlParameter("@In_CityName", (obj.CityName ?? string.Empty)), 
                                                        new SqlParameter("@In_country_id", (obj.Country_ID == 0?null: obj.Country_ID)), 
                                                        new SqlParameter("@In_Distance", (obj.Distance == 0 ? null: obj.Distance)), 
                                                        new SqlParameter("@In_latitude", (obj.Lattitude == string.Empty? "null": obj.Lattitude)), 
                                                        new SqlParameter("@In_Longitude", (obj.Longitude ==string.Empty? "null": obj.Longitude))};


                clscommon.AddInParameters(inParameter);
                var ds = clscommon.GetDataSet("Help_dbo.st_provider_list_Featuredtherapists");
                return ds;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "list_Featuredtherapists", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override Provider_ContactInfo Get_ContactInfo(int Reference_ID, int ReferenceType_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamList = {
	                                     new SqlParameter("@in_Reference_ID", Reference_ID),
	                                     new SqlParameter("@in_ReferenceType_ID", ReferenceType_ID)
                                               };
                objcommon.AddInParameters(objInparamList);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Admin_GET_ContactInfo");
                if (objread.Read())
                {
                    var objContactInfo = new Provider_ContactInfo(objread["Address1"].ToString() != null ? objread["Address1"].ToString() : null,
                                                                                    objread["Address2"].ToString() != null ? objread["Address2"].ToString() : null,
                                                                                    objread["Zip"].ToString() != null ? objread["Zip"].ToString() : null,
                                                                                    Convert.ToInt32(objread["State_ID"]) != 0 ? Convert.ToInt32(objread["State_ID"]) : -1,
                                                                                    objread["State"].ToString() != null ? objread["State"].ToString() : null,
                                                                                    Convert.ToInt32(objread["City_ID"]) != 0 ? Convert.ToInt32(objread["City_ID"]) : -1,
                                                                                    objread["City"].ToString() != null ? objread["City"].ToString() : null,
                                                                                    objread["CellPhone"].ToString() != null ? objread["CellPhone"].ToString() : null,
                                                                                    objread["WorkPhone"].ToString() != null ? objread["WorkPhone"].ToString() : null,
                                                                                    objread["Fax"].ToString() != null ? objread["Fax"].ToString() : null,
                                                                                    objread["Email"].ToString() != null ? objread["Email"].ToString() : null,
                                                                                    objread["Website"].ToString() != null ? objread["Website"].ToString() : null,
                                                                                    objread["facebookurl"].ToString() != null ? objread["facebookurl"].ToString() : null,
                                                                                    objread["twitterurl"].ToString() != null ? objread["twitterurl"].ToString() : null);

                    objContactInfo.LicenseNo = objread["LicenseNo"].ToString() ?? null;
                    objContactInfo.IsLicenseVerified = objread["IsLicenseVerified"].ToString() ?? null;
                    objContactInfo.licenseexpirydate = objread["LicenseExpirydate"].ToString() ?? null;
                    objContactInfo.ProviderImage = objread["ProviderImage"].ToString() ?? null;
                    objContactInfo.PracticeName = objread["practicename"].ToString() ?? null;
                    objContactInfo.Firstname = objread["FirstName"].ToString() ?? null;
                    objContactInfo.MiddleInitial = objread["MI"].ToString() ?? null;
                    objContactInfo.LastName = objread["LastName"].ToString() ?? null;
                    objContactInfo.Fullname = objread["Fullname"].ToString() ?? null;
                    objContactInfo.fbcomments = objread["Showcomments"].ToString() ?? null;
                    objContactInfo.Random_number = objread["Random_number"].ToString() ?? null;
                    objContactInfo.ElectricianImage = objread["Electrician_license"].ToString() ?? null;
                    objContactInfo.DriverImage = objread["Driver_license"].ToString() ?? null;
                    objContactInfo.ElctricianCustmiseImage = objread["Electrician_license_customized"].ToString() ?? null;
                    objContactInfo.DriverCustomiseImage = objread["Driver_license_customized"].ToString() ?? null;
                    objContactInfo.Referralcode = objread["Refferalcode"].ToString() ?? null;
                    objContactInfo.Refferal_Indicator = objread["Refferal_Indicator"].ToString() ?? null;
                    return objContactInfo;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_ContactInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
            return null;

        }
        public override DataSet List_field_description(string id)
        {
            try
            {
                var cls = new clsCommonFunctions();
                IDataParameter[] inpara = { new SqlParameter("@in_fieldids", (id ?? null)) };
                cls.AddInParameters(inpara);
                DataSet ds = cls.GetDataSet("Help_dbo.st_list_module_fielddescription");

                return ds;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "List_field_description", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet GetRenewChargeDetails(string Login_id)
        {
            try
            {
                var cls = new clsCommonFunctions();
                IDataParameter[] inpara = { new SqlParameter("@in_Login_id", (Login_id ?? null)) };
                cls.AddInParameters(inpara);
                DataSet ds = cls.GetDataSet("Help_dbo.st_get_billing_service_details");

                return ds;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetRenewChargeDetails", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet GetBillingChargeDetails(string Login_id)
        {
            try
            {
                var cls = new clsCommonFunctions();
                IDataParameter[] inpara = { new SqlParameter("@in_Login_id", (Login_id ?? null)) };
                cls.AddInParameters(inpara);
                DataSet ds = cls.GetDataSet("Help_dbo.st_get_billing_service_details_techcountrelated");

                return ds;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetRenewChargeDetails", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override Provider_ProfilesInfo Get_PrividerProfilesInfo(int Provider_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamList = {
	                           new SqlParameter("@IN_Provider_id", Provider_ID)
                                         };
                objcommon.AddInParameters(objInparamList);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.St_provider_GET_ProviderTherapistProfile");
                if (objread.Read())
                {
                    var objProfilesInfo = new Provider_ProfilesInfo(objread["l_yearsinpractice"] != null ? Convert.ToString(objread["l_yearsinpractice"]) : null,
                                                             objread["l_paragraph"].ToString() != null ? objread["l_paragraph"].ToString() : null,
                                                            objread["URL"].ToString() != null ? objread["URL"].ToString() : null,
                                                            objread["Payments"].ToString() != null ? objread["Payments"].ToString() : null
                                                          );
                    objProfilesInfo.FeeRange_From = objread["FeeRange_From"].ToString() ?? null;
                    objProfilesInfo.FeeRange_to = objread["FeeRange_to"].ToString() ?? null;
                    objProfilesInfo.DisplayFee_ind = objread["DisplayFee_ind"].ToString() ?? null;
                    if (objread["Firstfreesession"].ToString() != null)
                    {
                        objProfilesInfo.freesession = objread["Firstfreesession"].ToString();
                    }
                    //if (objread["Showcomments"].ToString() != null)
                    //{
                    //    objProfilesInfo.fbcomments = objread["Showcomments"].ToString();
                    //}
                    objProfilesInfo.Is_Contact_Info = objread["Is_Contact_Info"].ToString() != null ? objread["Is_Contact_Info"].ToString() : null;
                    objProfilesInfo.showschedule = objread["Show_Schedule"].ToString() != null ? objread["Show_Schedule"].ToString() : "Y";
                    objProfilesInfo.Random_number = objread["Random_number"].ToString() ?? null;
                    objProfilesInfo.State = objread["state"].ToString() ?? null;
                    objProfilesInfo.City = objread["city"].ToString() ?? null;
                    objProfilesInfo.Zip = objread["ZIP"].ToString() ?? null;
                    objProfilesInfo.Website = objread["l_webaddress"].ToString() != null ? objread["l_webaddress"].ToString() : null;
                    objProfilesInfo.facebookurl = objread["Facebookurl"].ToString() != null ? objread["Facebookurl"].ToString() : null;
                    objProfilesInfo.twitterurl = objread["Twitterurl"].ToString() != null ? objread["Twitterurl"].ToString() : null;
                    objProfilesInfo.fbcomments = objread["Showcomments"].ToString() != null ? objread["Showcomments"].ToString() : null;
                    return objProfilesInfo;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_PrividerProfilesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override Provider_ProfilesInfo Get_Prividerlicensedetails(int Provider_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamList = {
	                           new SqlParameter("@In_Providerid", Provider_ID)
                                         };
                objcommon.AddInParameters(objInparamList);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_provider_get_license_details");
                if (objread.Read())
                {
                    var objProfilesInfo = new Provider_ProfilesInfo();
                    objProfilesInfo.LicenseNo = objread["LicenseNo"].ToString() ?? null;
                    objProfilesInfo.IsLicenseVerified = objread["IsLicenseVerified"].ToString() ?? null;
                    objProfilesInfo.licenseexpirydate = objread["licenseexpirydate"].ToString() ?? null;
                    return objProfilesInfo;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_Prividerlicensedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string Insert_AdvancedSearchInfo(Provider_AdvancedSearch ObjInsAdvancedSearch)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_Provider_ID", (ObjInsAdvancedSearch.Provider_ID != 0 ? ObjInsAdvancedSearch.Provider_ID : null)),
		new SqlParameter("@In_l_paragraph", (ObjInsAdvancedSearch.Paragraph ?? null)),
		new SqlParameter("@In_Payments", (ObjInsAdvancedSearch.Payments ?? null)),
		new SqlParameter("@In_FeeRange_From", (ObjInsAdvancedSearch.FeeRange_From ?? null)),
		new SqlParameter("@In_FeeRange_to", (ObjInsAdvancedSearch.FeeRange_To ?? null)),
		new SqlParameter("@In_DisplayFee_ind", (ObjInsAdvancedSearch.DisplayFee_ind ?? null)),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"].ToString()),
		new SqlParameter("@in_Firstfreesession", ObjInsAdvancedSearch.freesession),
        new SqlParameter("@In_l_yearsinpractice", ObjInsAdvancedSearch.Yearinpractice),
        new SqlParameter("@In_updatedby", ObjInsAdvancedSearch.UpdatedBy),
        new SqlParameter("@in_Is_Contact_Info", ObjInsAdvancedSearch.Is_Contact_Info),
            new SqlParameter("@in_Show_schedule", ObjInsAdvancedSearch.showschedule),
            new SqlParameter("@In_l_webaddress",ObjInsAdvancedSearch.Website),
              new SqlParameter("@In_facebookurl",ObjInsAdvancedSearch.facebookurl),
                new SqlParameter("@In_twitterurl",ObjInsAdvancedSearch.twitterurl),
                  new SqlParameter("@in_Showcomments",ObjInsAdvancedSearch.fbcomments)
    	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Provider_INS_AdvancesearchOptions");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_AdvancedSearchInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override string Upd_SimpleProviderDetailsinDirectory(SimpleProvider_DetailsInfo objUpd, ref string Out_Msg)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
		new SqlParameter("@in_Provider_ID", (objUpd.Provider_ID == 0 ? null : objUpd.Provider_ID)),
		new SqlParameter("@in_PracticeName", objUpd.PracticeName),
		new SqlParameter("@in_FirstName", objUpd.FirstName),
		new SqlParameter("@in_MI", objUpd.MI),
		new SqlParameter("@in_LastName", objUpd.LastName),
		new SqlParameter("@in_UpdatedBy", objUpd.UpdatedBy),
		new SqlParameter("@in_Email", objUpd.Email),
		new SqlParameter("@in_Address1", objUpd.Address1),
		new SqlParameter("@in_Address2", objUpd.Address2),
		new SqlParameter("@in_City_ID", objUpd.City_ID),
		new SqlParameter("@in_state_ID", objUpd.State_ID),
		new SqlParameter("@in_Country_ID", "1"),
		new SqlParameter("@in_Zip", objUpd.Zip),
		new SqlParameter("@in_CellPhone", objUpd.CellPhone),
		new SqlParameter("@in_WorkPhone", objUpd.WorkPhone),
		new SqlParameter("@in_FAX", objUpd.FAX),
		new SqlParameter("@In_Website", objUpd.Website),
		new SqlParameter("@In_facebookurl", objUpd.facebookurl),
		new SqlParameter("@In_twitterurl", objUpd.twitterurl),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
		new SqlParameter("@in_licenseno", objUpd.LicenseNo),
		new SqlParameter("@in_ProviderImage", objUpd.ProviderImage),
		new SqlParameter("@in_licenseexpirydate", objUpd.licenseexpirydate),
		new SqlParameter("@In_IsLicenseVerified", objUpd.IsLicenseVerified),
         new SqlParameter("@in_showcomments", objUpd.Fbcomments),
         new SqlParameter("@In_Driver_license",objUpd.DriverLicsenseImage),
         new SqlParameter("@In_Electrician_license",objUpd.ElctricianLicenseImage),
         new SqlParameter("@In_Driver_license_customized",objUpd.DriverCustomiseImage),
         new SqlParameter("@In_Electrician_license_customized",objUpd.ElctricianCustmiseImage)
	};
                IDataParameter[] OutParam = { new SqlParameter("@out_Msg", SqlDbType.VarChar, 500) };

                objcommon.AddInParameters(objparm);
                objcommon.AddOutParameters(OutParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_SimplePracticeProviderInfoinDirectoryListing");
                if (objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value != null)
                {
                    Out_Msg = objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Upd_SimpleProviderDetailsinDirectory", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<clsCountry> GetStatesByCountryId(int? CountryId)
        {
            try
            {
                var statesList = new List<clsCountry>();
                DataSet dsStates;
                ICacheManager ConfigCacheManager = CacheFactory.GetCacheManager();
                if (ConfigCacheManager.Contains("States"))
                {
                    dsStates = (DataSet)ConfigCacheManager["States"];
                    if ((dsStates != null))
                    {
                        if (dsStates.Tables.Count > 0)
                        {
                            if (dsStates.Tables[0].Rows.Count > 0)
                            {
                                statesList.AddRange(from DataRow drStates in dsStates.Tables[0].Select("Country_ID=" + CountryId) select new clsCountry { StateId = Convert.ToInt32(drStates["State_Id"]), StateName = Convert.ToString(drStates["State"]), StateFullName = Convert.ToString(drStates["StateFullName"]) });
                            }
                        }
                    }
                }
                else
                {
                    clsCommonFunctions objcommon = new clsCommonFunctions();

                    IDataParameter[] InObjParam = { new SqlParameter("@in_Country_ID", CountryId!=null?CountryId:null) };
                    objcommon.AddInParameters(InObjParam);
                    dsStates = objcommon.GetDataSet("Help_dbo.st_General_List_States");
                    if ((dsStates != null))
                    {
                        if (dsStates.Tables.Count > 0)
                        {
                            if (dsStates.Tables[0].Rows.Count > 0)
                            {
                                ConfigCacheManager.Add("States", dsStates);
                                statesList.AddRange(from DataRow drStates in dsStates.Tables[0].Select("Country_ID=" + CountryId) select new clsCountry { StateId = Convert.ToInt32(drStates["State_Id"]), StateName = Convert.ToString(drStates["State"]), StateFullName = Convert.ToString(drStates["StateFullName"]) });
                            }
                        }
                    }
                }
                return statesList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetStatesByCountryId", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;

        }
        public override List<clsCountry> GetCityByStateId(int? stateid)
        {
            try
            {
                var cityList = new List<clsCountry>();
                DataSet dsCities;
                ICacheManager ConfigCacheManager = CacheFactory.GetCacheManager();
                if (ConfigCacheManager.Contains("Cities"))
                {
                    dsCities = (DataSet)ConfigCacheManager["Cities"];
                    if ((dsCities != null))
                    {
                        if (dsCities.Tables.Count > 0)
                        {
                            if (dsCities.Tables[0].Rows.Count > 0)
                            {
                                cityList.AddRange(from DataRow drCities in dsCities.Tables[0].Select("State_ID=" + stateid) select new clsCountry { CityName = Convert.ToString(drCities["City"]), CityId = Convert.ToInt32(drCities["City_Id"]) });
                            }
                        }
                    }
                }
                else
                {
                    var objcommon = new clsCommonFunctions();

                    IDataParameter[] InObjParam = { new SqlParameter("@in_State_ID", null) };
                    objcommon.AddInParameters(InObjParam);
                    dsCities = objcommon.GetDataSet("Help_dbo.st_General_List_Cities");
                    if ((dsCities != null))
                    {
                        if (dsCities.Tables.Count > 0)
                        {
                            if (dsCities.Tables[0].Rows.Count > 0)
                            {
                                ConfigCacheManager.Add("Cities", dsCities);
                                cityList.AddRange(from DataRow drCities in dsCities.Tables[0].Select("State_ID=" + stateid) select new clsCountry { CityName = Convert.ToString(drCities["City"]), CityId = Convert.ToInt32(drCities["City_Id"]) });
                            }
                        }
                    }
                }
                return cityList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetCityByStateId", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<clsCountry> Reg_GetStatesByZipCode(string ZipCode)
        {
            try
            {
                var statesList = new List<clsCountry>();
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InObjParam = { new SqlParameter("@in_ZIP_CODE", ZipCode) };
                objcommon.AddInParameters(InObjParam);
                DataSet dsStates = objcommon.GetDataSet("Help_dbo.St_Reg_get_ZipState");
                if ((dsStates != null))
                {
                    if (dsStates.Tables.Count > 0)
                    {
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            statesList.AddRange(from DataRow drStates in dsStates.Tables[0].Rows select new clsCountry { StateId = Convert.ToInt32(drStates["State_Id"]), StateName = Convert.ToString(drStates["State"]), StateFullName = Convert.ToString(drStates["StateFullName"]) });
                        }
                    }
                }
                return statesList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Reg_GetStatesByZipCode", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<clsCountry> Reg_GetCityByZipCode(string ZipCode, int? State_ID)
        {
            try
            {
                var cityList = new List<clsCountry>();
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InObjParam = {
		new SqlParameter("@in_ZIP_CODE", ZipCode),
		new SqlParameter("@in_State_id", (State_ID == 0 ? null : State_ID))
	};
                objcommon.AddInParameters(InObjParam);
                DataSet dsCities = objcommon.GetDataSet("Help_dbo.St_Reg_get_ZipCity");
                if ((dsCities != null))
                {
                    if (dsCities.Tables.Count > 0)
                    {
                        if (dsCities.Tables[0].Rows.Count > 0)
                        {
                            cityList.AddRange(from DataRow drCities in dsCities.Tables[0].Rows select new clsCountry { CityName = Convert.ToString(drCities["City"]), CityId = Convert.ToInt32(drCities["City_Id"]) });
                        }
                    }
                }
                return cityList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Reg_GetCityByZipCode", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;

        }
        private void SetCommandType(SqlCommand sqlCmd, CommandType cmdType, string cmdText)
        {
            sqlCmd.CommandType = cmdType;
            sqlCmd.CommandText = cmdText;
        }
        private CollectionBase ExecuteReaderCmd(SqlCommand sqlCmd, GenerateCollectionFromReader gcfr)
        {
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            SqlConnection cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                CollectionBase temp = gcfr(sqlCmd.ExecuteReader());
                cn.Close();
                return temp;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "ExecuteReaderCmd", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Admin_postajob> ChildSections(string prov_id, string practice_id, string login_id, string section_id, string Role_id)
        {
            try
            {
                var objDataList = new List<Admin_postajob>();
                IDataParameter[] objInparameters = {
		new SqlParameter("@in_Login_ID", login_id),
		new SqlParameter("@in_Section_ID", section_id),
		new SqlParameter("@in_RoleId", Role_id),
		new SqlParameter("@in_Provider_ID", prov_id == null ? null : prov_id),
		new SqlParameter("@in_Practice_ID", practice_id == null ? null : practice_id),
		new SqlParameter("@In_IsBasicACPrv", "Y"),
		new SqlParameter("@In_IsClerk_ind", null),
		new SqlParameter("@In_IsRecept_Ind", null)
	};
                var objDbWrapper = new clsDBWrapper();

                objDbWrapper.AddInParameters(objInparameters);
                DataSet dsSections = objDbWrapper.GetDataSet(section_id == "2291" ? "Help_dbo.st_Security_Get_HomeChildSections" : "Help_dbo.St_Security_Get_ChildSections");

                if (dsSections.Tables.Count > 0)
                {
                    if (dsSections.Tables[0].Rows.Count > 0)
                    {

                        objDataList.AddRange(from DataRow dr in dsSections.Tables[0].Rows
                                             select new Admin_postajob
                                             {
                                                 SectionName = dr["SectionName"] != null ? dr["SectionName"].ToString() : null,
                                                 ImageFileName_1 = dr["ImageFileName_1"] != null ? dr["ImageFileName_1"].ToString() : null,
                                                 SectionDescription =
                                                     dr["SectionDescription"] != null ? dr["SectionDescription"].ToString() : null,
                                                 SectionPath = dr["SectionPath"] != null ? dr["SectionPath"].ToString() : null
                                             });
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ChildSections", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }

        }
       public override List<PracticeblHome> Get_PracticeHomePagePatientsList(PracticeblHome objgethome)
        {
            var HomeList = new List<PracticeblHome>();
            try
            {
                var objcommon = new clsCommonFunctions();

                IDataParameter[] ObjParam = {
            new SqlParameter("@in_Patient_Status_ID", objgethome.Patient_Status_ID),
            new SqlParameter("@in_Provider_ID", objgethome.Provider_ID),
            new SqlParameter("@in_NoofRecords", objgethome.NoofRecords),
            new SqlParameter("@in_PageNo", objgethome.PageNo),
            new SqlParameter("@in_fullname", objgethome.fullname),
            new SqlParameter("@In_OrderByItem", objgethome.Orderbyitem),
            new SqlParameter("@in_OrderBy", objgethome.OrderBy),
             new SqlParameter("@in_lastname",objgethome.LastName)
        };

                var _with1 = objcommon;
                _with1.AddInParameters(ObjParam);

                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Patient_Search_Global_rdPaging");
                
                while (objread.Read())
                {
                    var objhome = new PracticeblHome(Convert.ToString(objread["PatientName"]),
                        Convert.ToInt32(objread["Patient_ID"]),

                        (objread["NextAppt"] == null ? null : objread["NextAppt"].ToString()),
                        (objread["ApptCount"] == null ? null : objread["ApptCount"].ToString()),
                   Convert.ToDouble(objread["Total"] == null ? null : objread["Total"]),
                       (objread["PatientLogin_ID"] == null ? null : objread["PatientLogin_ID"].ToString()),
                    (objread["Practice_ID"].ToString() == null ? null : objread["Practice_ID"].ToString()),
                    (objread["AuthorizedProvider_ID"].ToString() == null ? null : objread["AuthorizedProvider_ID"].ToString()),
                    (objread["ActualProvider_ID"].ToString() == null ? null : objread["ActualProvider_ID"].ToString()),
                    (objread["DefaultCourtLocation_ID"] == null ? null : objread["DefaultCourtLocation_ID"].ToString()));
                    objhome.HomePhone = (objread["HomePhone"].ToString() == "" ? "-" : clsCommonFunctions.UsPhoneFormat(objread["HomePhone"].ToString()));
                    objhome.WorkPhone = (objread["WorkPhone"].ToString() == "" ? "-" : clsCommonFunctions.UsPhoneFormat(objread["WorkPhone"].ToString()));
                    objhome.CellPhone = objread["CellPhone"].ToString() == "" ? null : clsCommonFunctions.UsPhoneFormat(objread["CellPhone"].ToString());
                    objhome.Status_Ind = (objread["status"].ToString() ?? null);
                    objhome.Login_ID = objread["AuthorizedProviderLogin_ID"].ToString() ?? null;
                    objhome.Address = objread["Address"].ToString() ?? null;
                    objhome.email = objread["PatientEmail"].ToString() ?? null;
                    objhome.Transactionexist = objread["Transactionexist"].ToString() ?? null;
                    objhome.DefaultCourtLocation_ID = Convert.ToString(objread["DefaultCourtLocation_ID"]);
                    HomeList.Add(objhome);

                }

                objread.NextResult();

                if (objread.HasRows == true)
                {
                    if (objread.Read())
                    {
                        PracticeblHome.Totalnoofrecords = (int)objread[0];
                    }

                }

                
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_PracticeHomePagePatientsList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                //return null;
            }
            return HomeList;
        }
        public override List<patients> Get_AppointmentHistory(patients objappHistory)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
			new SqlParameter("@in_Patient_ID", objappHistory.Patient_ID),
			new SqlParameter("@in_Provider_ID", objappHistory.Provider_ID),
			new SqlParameter("@in_OrderbyItem", objappHistory.OrderByItem),
			new SqlParameter("@in_Order", objappHistory.Order),
			new SqlParameter("@in_NoofRecords", objappHistory.NoofRecords),
			new SqlParameter("@in_PageNo", objappHistory.page)
		};
                objcommon.AddInParameters(objInparam);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Patients_LIST_Appointment_Paging");

                var AppHistoryList = new List<patients>();

                int SLno = objappHistory.NoofRecords * (objappHistory.page - 1);
                while (objread.Read())
                {
                    SLno += 1;
                    var appHistory = new patients
                    {
                        SLno = SLno,
                        Appointment_ID =
                            (objread["Appointment_Id"] != null ? Convert.ToInt32(objread["Appointment_Id"]) : -1),
                        Appointment_Date =
                            (objread["Appointment_Date"] != null ? objread["Appointment_Date"].ToString() : null),
                        Appointment_Time =
                            (objread["Appointment_Time"] != null ? objread["Appointment_Time"].ToString() : null),
                        Length = (objread["Length"] != null ? Convert.ToInt32(objread["Length"]) : -1),
                        ApptStatus = (objread["ApptStatus"] != null ? objread["ApptStatus"].ToString() : null),
                        Notes = (objread["Notes"] != null ? objread["Notes"].ToString() : null)
                    };
                    AppHistoryList.Add(appHistory);
                }

                objread.NextResult();

                while (objread.Read())
                {
                    patients.TotalRecords = (int)objread[0];
                }

                return AppHistoryList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_AppointmentHistory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }

        }
        public List<ProviderAdvertising> GetTopcitisList()
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objDataList = new List<ProviderAdvertising>();
                DataSet ds = objcommon.GetDataSet("Help_dbo.st_Public_LIST_TopCities");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                             select new ProviderAdvertising
                                             {
                                                 CityNm = Convert.ToString(dr["City"]),
                                                 City_ID = Convert.ToInt32(dr["City_id"]),
                                                 State_Name = Convert.ToString(dr["state"]),
                                                 State_ID = Convert.ToInt32(dr["State_ID"]),
                                                 State = Convert.ToString(dr["stateabr"])
                                             });
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetTopcitisList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }

        }
        public List<ProviderAdvertising> GetCountriesBasedOnDirectoryID(int? Directory_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objDataList = new List<ProviderAdvertising>();
                IDataParameter[] InParam = { new SqlParameter("@In_Directory_ID", (Directory_ID == 0 ? null : Directory_ID)) };
                objcommon.AddInParameters(InParam);
                DataSet ds = objcommon.GetDataSet("Help_dbo.st_Provider_LIST_DirectoryCountries");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new ProviderAdvertising { Country = Convert.ToString(dr["Country_Name"]), Country_ID = Convert.ToInt32(dr["Country_ID"]) });
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetCountriesBasedOnDirectoryID", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }

        }
        public List<FeaturesList> GetProviderBasedGreetings(int? ProviderID, int NoOfRecords, int Pageno)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objDataList = new List<FeaturesList>();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Provider_ID", (ProviderID != 0 ? ProviderID : null)),
			new SqlParameter("@In_Public", "N"),
			new SqlParameter("@In_NoOfRecords", (NoOfRecords != 0 ? NoOfRecords : 0)),
			new SqlParameter("@In_Pageno", (Pageno != 0 ? Pageno : 0))
		};
                objcommon.AddInParameters(InParamList);
                DataSet ds = objcommon.GetDataSet("Help_dbo.st_Provider_List_Greetings");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var objData = new FeaturesList();
                            if (dr["Provider_Greeting_ID"] != null)
                            {
                                objData.Provider_Greeting_ID = Convert.ToInt32(dr["Provider_Greeting_ID"]);
                            }
                            else
                            {
                                objData.Provider_Greeting_ID = null;
                            }
                            if (dr["Provider_ID"] != null)
                            {
                                objData.Provider_ID = Convert.ToInt32(dr["Provider_ID"]);
                            }
                            else
                            {
                                objData.Provider_ID = null;
                            }
                            objData.Title = dr["Title"].ToString() ?? null;
                            objData.Description = dr["Description"].ToString() ?? null;
                            if (dr["Status_Ind"].ToString() != null)
                            {
                                if (dr["Status_Ind"].ToString() == "A")
                                {
                                    objData.Status_Ind = "Active";
                                }
                                else if (dr["Status_Ind"].ToString() == "I")
                                {
                                    objData.Status_Ind = "InActive";
                                }
                            }
                            else
                            {
                                objData.Status_Ind = null;
                            }
                            if (dr["ViewedCount"].ToString() != string.Empty)
                            {
                                objData.ViewedCount = Convert.ToInt32(dr["ViewedCount"].ToString());
                            }
                            else
                            {
                                objData.ViewedCount = null;
                            }

                            objDataList.Add(objData);
                        }
                    }
                    FeaturesList.TotalNoRecords = ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0][0].ToString() : null;
                }

                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetProviderBasedGreetings", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }

        }
        public bool Insert_Provider_Greetings(FeaturesList objData)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Provider_ID", (objData.Provider_ID == 0 ? null : objData.Provider_ID)),
			new SqlParameter("@In_ProviderLogin_ID", (objData.ProviderLogin_ID == 0 ? null : objData.ProviderLogin_ID)),
			new SqlParameter("@In_Practice_ID", (objData.Practice_ID == 0 ? null : objData.Practice_ID)),
			new SqlParameter("@In_Title", (objData.Title ?? null)),
			new SqlParameter("@In_Description", (objData.Description ??null)),
			new SqlParameter("@In_Embed_Text", (string.IsNullOrEmpty(objData.Embed_Text) ? null : objData.Embed_Text)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                objcommon.AddInParameters(InParamList);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_INS_Greetings");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Insert_Provider_Greetings", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public void ApplyStatus(int? ProviderGreetingID, int? ProviderID, string Status)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Greeting_ID", (ProviderGreetingID != 0 ? ProviderGreetingID : null)),
			new SqlParameter("@In_Provider_ID", (ProviderID != 0 ? ProviderID : null)),
			new SqlParameter("@In_Status_Ind", (Status ?? null)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                objcommon.AddInParameters(InParamList);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_DML_GreetingStatus");
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "ApplyStatus", HttpContext.Current.Request, HttpContext.Current.Session);
            }

        }
        public override List<Provider_DocumentInfo> Get_DocumentInfo(Provider_DocumentInfo ObjDocument)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
		new SqlParameter("@in_Provider_Id", (ObjDocument.Provider_ID == 0 ? null : ObjDocument.Provider_ID)),
		new SqlParameter("@In_OrderByItem", ObjDocument.OrderBYItem),
		new SqlParameter("@In_OrderBy", ObjDocument.OrderBy),
		new SqlParameter("@in_PageNo", ObjDocument.PageNo),
		new SqlParameter("@in_NoofRecords", ObjDocument.NoOfRecords),
		new SqlParameter("@in_verifieddocuments", ObjDocument.verifieddocument)
	};
                objcommon.AddInParameters(objInparam);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Provider_GET_Provider_DocumentInfo_rdPaging");
                var objDocumentList = new List<Provider_DocumentInfo>();
                int SLno = 0;
                while (objread.Read())
                {
                    SLno += 1;
                    var Document = new Provider_DocumentInfo(SLno,
                        Convert.ToInt32(objread["ProviderDocument_ID"] ?? null),
                        null,
                        Convert.ToString(objread["Path"] ?? null),
                        Convert.ToString(objread["Title"] ?? null),
                        Convert.ToString(objread["DocDescription"] ?? null),
                        Convert.ToString(objread["CreatedOn"] ?? null),
                        Convert.ToString(objread["ImagePathSuffix"] ?? null),
                        Convert.ToString(objread["FileName"] ?? null),
                        Convert.ToInt32(objread["ViewedCount"].ToString() != string.Empty ? objread["ViewedCount"] : 0),
                        Convert.ToString(objread["Displayinpublic"] ?? null));
                    objDocumentList.Add(Document);
                }
                objread.NextResult();


                while (objread.Read())
                {
                    Provider_DocumentInfo.TotalRecords = Convert.ToInt32(objread[0]);
                }

                return objDocumentList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Get_DocumentInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Del_DocumentInfo(int ProviderDocument_ID, int DeletedBy)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@in_ProviderDocument_ID", ProviderDocument_ID),
		new SqlParameter("@in_DeletedBy", DeletedBy),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Provider_DEL_ProviderDocument");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Del_DocumentInfo", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }

        }
        public override string Insert_Attachment(int? Provider_ID, string Title, string DocDescription, string Path, string PathEncrypted, string FileName, string IsDisplayPublic, int CreatedBy, string verifieddocument, ref string Out_Suffix, double? Document_Size = null)
        {
            try
            {
                var objCommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
		new SqlParameter("@in_Provider_ID", (Provider_ID != 0 ? Provider_ID : null)),
		new SqlParameter("@in_Title", Title),
		new SqlParameter("@in_DocDescription", DocDescription),
		new SqlParameter("@in_Path", Path),
		new SqlParameter("@in_Path_Encrypted", PathEncrypted),
		new SqlParameter("@in_FileName", FileName),
		new SqlParameter("@in_CreatedBy", CreatedBy),
		new SqlParameter("@in_verifieddocuments", verifieddocument),
		new SqlParameter("@in_Displayinpublic", IsDisplayPublic),
		new SqlParameter("@In_Size", Document_Size)
	};

                IDataParameter[] outparam = { new SqlParameter("@Out_ImagePathSuffix", SqlDbType.VarChar, 100) };

                objCommon.AddInParameters(InParamList);
                objCommon.AddOutParameters(outparam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Provider_INS_Attachment");
                if (objCommon.objdbCommandWrapper.Parameters["@Out_ImagePathSuffix"].Value.ToString() != null)
                {
                    Out_Suffix = objCommon.objdbCommandWrapper.Parameters["@Out_ImagePathSuffix"].Value.ToString();
                }
                return Out_Suffix;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_Attachment", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Update_Attachment(int? ProviderDocument_ID, string Title, string DocDescription, string Path, string PathEncrypted, string FileName, int UpdatedBy, string verifieddocument, double? Document_Size = null, string Displayinpublic = null)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
		new SqlParameter("@in_ProviderDocument_ID", (ProviderDocument_ID != 0 ? ProviderDocument_ID : null)),
		new SqlParameter("@in_Title", Title),
		new SqlParameter("@in_DocDescription", DocDescription),
		new SqlParameter("@in_Path", Path),
		new SqlParameter("@in_Path_Encrypted", PathEncrypted),
		new SqlParameter("@in_FileName", FileName),
		new SqlParameter("@in_UpdatedBy", UpdatedBy),
		new SqlParameter("@in_verifieddocuments", verifieddocument),
		new SqlParameter("@In_Size", (Document_Size != 0 ? Document_Size : null)),
		new SqlParameter("@in_Displayinpublic", Displayinpublic),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
	};

                objCommon.AddInParameters(InParamList);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_Attachment");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Update_Attachment", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;

        }
        public override Provider_DocumentInfo GetAttachmentList(int? Provider_ID, int? ProviderDocument_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamList = {
		new SqlParameter("@in_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
		new SqlParameter("@in_ProviderDocument_ID", (ProviderDocument_ID == 0 ? null : ProviderDocument_ID))
	};
                objcommon.AddInParameters(objInparamList);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Provider_GET_Provider_DocumentInfo");

                if (objread.Read() == true)
                {
                    var objAttachmentList = new Provider_DocumentInfo(null,
                        objread["Title"].ToString() != null ? objread["Title"].ToString() : null,
                        objread["DocDescription"].ToString() != null ? objread["DocDescription"].ToString() : null,
                        objread["Path"].ToString() != null ? objread["Path"].ToString() : null,
                        objread["FileName"].ToString() != null ? objread["FileName"].ToString() : null,
                        Convert.ToString(objread["Path_Suffix"] ?? null),
                        null);
                    if (objread["Displayinpublic"].ToString() != null)
                    {
                        objAttachmentList.Displayinpublic = objread["Displayinpublic"].ToString();
                    }


                    return objAttachmentList;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetAttachmentList", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;

        }
        public override bool Contact_Us(ContactCog objcont)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_ContactFirstName", objcont.ContactFirstName),
			new SqlParameter("@in_ContactLastName", objcont.ContactLastName),
			new SqlParameter("@in_ContactMail", objcont.ContactMail),
			new SqlParameter("@in_ContactPhone", objcont.ContactPhone),
			new SqlParameter("@in_ContactSubject", objcont.ContactSubject),
			new SqlParameter("@in_ContactMessage", objcont.ContactMessage),
			new SqlParameter("@in_ContactFax", null),
			new SqlParameter("@in_CreatedBy", objcont.CreatedBy)
		};
                bool blnEmp = false;
                objcommon.AddInParameters(paramlist);
                blnEmp = Convert.ToBoolean(objcommon.ExecuteNonQuerySP("Help_dbo.st_Cognode_ADD_ContactUs"));
                return blnEmp;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Contact_Us", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }


        }
        //    public override List<Provider_TimaTable> GetFacilitiesName(int? Provider_ID, int? Practice_ID)
        //    {
        //        try
        //        {
        //            var objcommon = new clsCommonFunctions();
        //            var ObjDataList = new List<Provider_TimaTable>();
        //            var objData = new Provider_TimaTable();
        //            IDataParameter[] InParam = {
        //    new SqlParameter("@In_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
        //    new SqlParameter("@In_Practice_ID", (Practice_ID == 0 ? null : Practice_ID))
        //};
        //            objcommon.AddInParameters(InParam);
        //            SqlDataReader dr = objcommon.GetDataReader("Help_dbo.st_Provider_GET_RegProviderFacilities");
        //            if (dr.Read())
        //            {
        //                if (dr["PlaceOfService_ID"] != null)
        //                {
        //                    objData.PlaceOfService_ID = Convert.ToInt32(dr["PlaceOfService_ID"]);
        //                }
        //                if (dr["FacilityName"].ToString() != null)
        //                {
        //                    objData.FacilityName = dr["FacilityName"].ToString();
        //                }
        //                ObjDataList.Add(objData);
        //            }
        //            return ObjDataList;
        //        }
        //        catch (Exception ex)
        //        {
        //            clsExceptionLog clsCustomEx = new clsExceptionLog();
        //            clsCustomEx.LogException(ex, ClassName, "GetFacilitiesName", HttpContext.Current.Request, HttpContext.Current.Session);
        //        }
        //        return null;
        //    }
        public override List<Provider_Sitestastics> GET_Statistics(Provider_Sitestastics objstatastics)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                SqlDataReader objread;
                IDataParameter[] objparm = {
		new SqlParameter("@In_provider_id", objstatastics.Provider_ID),
		new SqlParameter("@in_Year", (objstatastics.Year ?? null))
	};
                objcommon.AddInParameters(objparm);
                if (objstatastics.Year == "2008")
                {
                    objread = objcommon.GetDataReader("Help_dbo.st_Provider_GET_StatisticS_Year2008");
                }
                else if (objstatastics.Year == "2009")
                {
                    objread = objcommon.GetDataReader("Help_dbo.st_Provider_GET_StatisticS_Year2009");
                }
                else
                {
                    objread = objcommon.GetDataReader("Help_dbo.st_Provider_GET_Statistics");
                }

                var objGETStatistics = new List<Provider_Sitestastics>();
                while (objread.Read())
                {
                    var objdata = new Provider_Sitestastics();
                    if (objread["statistics_name"].ToString() != null)
                    {
                        objdata.statistics_name = objread["statistics_name"].ToString();
                    }
                    if (objread["January"].ToString() != null & objread["January"].ToString() != "")
                    {
                        objdata.January = objread["January"].ToString();
                    }
                    else
                    {
                        objdata.January = "0";
                    }
                    if (objread["February"].ToString() != null & objread["February"].ToString() != "")
                    {
                        objdata.February = objread["February"].ToString();
                    }
                    else
                    {
                        objdata.February = "0";
                    }
                    if (objread["March"].ToString() != null & objread["March"].ToString() != "")
                    {
                        objdata.March = objread["March"].ToString();
                    }
                    else
                    {
                        objdata.March = "0";
                    }
                    if (objread["April"].ToString() != null & objread["April"].ToString() != "")
                    {
                        objdata.April = objread["April"].ToString();
                    }
                    else
                    {
                        objdata.April = "0";
                    }
                    if (objread["May"].ToString() != null & objread["May"].ToString() != "")
                    {
                        objdata.May = objread["May"].ToString();
                    }
                    else
                    {
                        objdata.May = "0";
                    }
                    if (objread["June"].ToString() != null & objread["June"].ToString() != "")
                    {
                        objdata.June = objread["June"].ToString();
                    }
                    else
                    {
                        objdata.June = "0";
                    }
                    if (objread["July"].ToString() != null & objread["July"].ToString() != "")
                    {
                        objdata.July = objread["July"].ToString();

                    }
                    else
                    {
                        objdata.July = "0";
                    }
                    if (objread["August"].ToString() != null & objread["August"].ToString() != "")
                    {
                        objdata.August = objread["August"].ToString();

                    }
                    else
                    {
                        objdata.August = "0";
                    }
                    if (objread["September"].ToString() != null & objread["September"].ToString() != "")
                    {
                        objdata.September = objread["September"].ToString();

                    }
                    else
                    {
                        objdata.September = "0";
                    }
                    if (objread["October"].ToString() != null & objread["October"].ToString() != "")
                    {
                        objdata.October = objread["October"].ToString();

                    }
                    else
                    {
                        objdata.October = "0";
                    }
                    if (objread["November"].ToString() != null & objread["November"].ToString() != "")
                    {
                        objdata.November = objread["November"].ToString();

                    }
                    else
                    {
                        objdata.November = "0";
                    }
                    if (objread["December"].ToString() != null & objread["December"].ToString() != "")
                    {
                        objdata.December = objread["December"].ToString();

                    }
                    else
                    {
                        objdata.December = "0";
                    }

                    objGETStatistics.Add(objdata);
                }
                return objGETStatistics;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GET_Statistics", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }

        }
        public override Provider_Sitestastics Get_StartYearOfProvider(int? Provider_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objParam = { new SqlParameter("@In_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)) };
                objcommon.AddInParameters(objParam);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_PRovider_GET_StatsYearRange");
                var objdata = new Provider_Sitestastics();
                while (objread.Read())
                {
                    if (objread["CreatedOn"].ToString() != null)
                    {
                        objdata.CreatedOn = objread["CreatedOn"].ToString();
                    }
                }

                return objdata;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Get_StartYearOfProvider", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Category> GetMs_GetCategorieslist(int LoginID, int RoleID, int Prov_id)
        {
            try
            {
                var sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@in_Login_ID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                AddParamToSQLCmd(sqlCmd, "@in_Role_ID", SqlDbType.Int, 0, ParameterDirection.Input, RoleID);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_GetMsgCategoriesWithCount");
                var stsCollection = (DataSet)ExecuteDatasetCmd(sqlCmd);
                var objDataList = new List<Category>();
                if (stsCollection.Tables.Count > 0)
                {

                    if (stsCollection.Tables[0].Rows.Count > 0)
                    {
                        var dv = new DataView(stsCollection.Tables[0])
                        {
                            RowFilter = "MessageCategory_ID not in(132,142,147,1,4)"
                        };
                        if (RoleID == 15 | RoleID == 4 | RoleID == 6 | RoleID == 27)
                        {
                            dv.RowFilter = "MessageCategory_ID not in(1,4,7,66,132,142,147)";

                        }
                        objDataList.AddRange(from DataRowView dr in dv
                                             select new Category
                                             {
                                                 MessageCategory_ID =
                                                     dr["MessageCategory_ID"] != null ? (int)dr["MessageCategory_ID"] : 0,
                                                 MessageCategory =
                                                     dr["MessageCategory"] != null ? dr["MessageCategory"].ToString() : null,
                                                 NewMsgsCount = dr["NewMsgsCount"] != null ? Convert.ToInt32(dr["NewMsgsCount"]) : 0,
                                                 FilePath = dr["FilePath"] != null ? dr["FilePath"].ToString() : null
                                             });
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetMs_GetCategorieslist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        private void AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
        {
            try
            {
                if (sqlCmd == null)
                {
                    throw new ArgumentNullException("sqlCmd");
                }
                if (paramId == string.Empty)
                {
                    throw new ArgumentOutOfRangeException("paramId");
                }
                var newSqlParam = new SqlParameter();
                newSqlParam.ParameterName = paramId;
                newSqlParam.SqlDbType = sqlType;
                newSqlParam.Direction = paramDirection;

                if (paramSize > 0)
                {
                    newSqlParam.Size = paramSize;
                }
                if ((paramvalue != null))
                {
                    newSqlParam.Value = paramvalue;
                }
                sqlCmd.Parameters.Add(newSqlParam);
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "AddParamToSQLCmd", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private DataSet ExecuteDatasetCmd(SqlCommand sqlCmd)
        {
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            var cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                var temp = new DataSet();
                var Da = new SqlDataAdapter(sqlCmd);
                Da.Fill(temp);
                cn.Close();
                return temp;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ExecuteDatasetCmd", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public string CountingProviderVisitToHisProfile(ProviderAdvertising objStatics)
        //{
        //    try
        //    {
        //        IDataParameter[] InParamList = {
        //    new SqlParameter("@In_Provider_ID", objStatics.Provider_ID),
        //    new SqlParameter("@In_StateName", (!string.IsNullOrEmpty(objStatics.State_Name) ? objStatics.State_Name : null)),
        //    new SqlParameter("@In_CityName", (!string.IsNullOrEmpty(objStatics.CityNm) ? objStatics.CityNm : null)),
        //    new SqlParameter("@In_SiteStatic_ID", objStatics.SiteStatic_ID)
        //};
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        objcommon.AddInParameters(InParamList);
        //        return Convert.ToString(objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_ListingStatistic"));
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, ClassName, "CountingProviderVisitToHisProfile", HttpContext.Current.Request, HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public long InsertVisitorTracking(ProviderAdvertising obj)
        {
            try
            {
                long intTrackID = 0;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
			new SqlParameter("@In_State", (!string.IsNullOrEmpty(obj.State_Name) ? obj.State_Name : null)),
			new SqlParameter("@In_City", (!string.IsNullOrEmpty(obj.CityNm) ? obj.CityNm : null)),
			new SqlParameter("@In_Country", (!string.IsNullOrEmpty(obj.Country) ? obj.Country : null)),
			new SqlParameter("@In_Country_Code", (!string.IsNullOrEmpty(obj.CountryCode) ? obj.CountryCode : null)),
			new SqlParameter("@In_Latitude", (!string.IsNullOrEmpty(obj.Latitude) ? obj.Latitude : null)),
			new SqlParameter("@In_Longitude", (!string.IsNullOrEmpty(obj.Longitude) ? obj.Longitude : null)),
			new SqlParameter("@In_IP_Address", (!string.IsNullOrEmpty(obj.IPAddress) ? obj.IPAddress : null)),
			new SqlParameter("@in_UserAgent", (!string.IsNullOrEmpty(obj.UserAgent) ? obj.UserAgent : null)),
			new SqlParameter("@in_Siteurl", (!string.IsNullOrEmpty(obj.Siteurl) ? obj.Siteurl : null))
		};
                IDataParameter[] objoutparam = { new SqlParameter("@Out_ID", SqlDbType.BigInt) };
                objcommon.AddInParameters(objInparam);
                objcommon.AddOutParameters(objoutparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_User_INS_VisitTracking");
                if (objcommon.objdbCommandWrapper.Parameters["@Out_ID"].Value != null)
                {
                    intTrackID = Convert.ToInt64(objcommon.objdbCommandWrapper.Parameters["@Out_ID"].Value);
                }
                return intTrackID;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "InsertVisitorTracking", HttpContext.Current.Request, HttpContext.Current.Session);
                return 0;
            }
        }
        public List<ProviderAdvertising> GetProvidersListForAdvanceSearch(ProviderAdvertising obj, ref int TotalNoofRecords)
        {
            try
            {
                var objDataList = new List<ProviderAdvertising>();
                var objCommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_CityName", (obj.CityNm ?? null)),
			new SqlParameter("@In_State_ID", (obj.State_ID == 0 ? null : obj.State_ID)),
			new SqlParameter("@In_Zip", (obj.ZIPValue ?? null)),
			new SqlParameter("@In_Distance", (obj.Radius == 0 ? null : obj.Radius)),
			new SqlParameter("@In_PageNo", (obj.PageNo == 0 ? null : obj.PageNo)),
			new SqlParameter("@In_NoOfRecords", (obj.NoOfRecords == 0 ? null : obj.NoOfRecords)),
			new SqlParameter("@in_Lastname ", (obj.LastName ?? null)),
			new SqlParameter("@in_Firstfreesession ", (obj.firstfee ?? null))
		};
                objCommon.AddInParameters(InParamList);
                DataSet ds = objCommon.GetDataSet("Help_dbo.st_Provider_LIST_AdvancedSearchProviders");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                             select new ProviderAdvertising
                                             {
                                                 ProviderID = Convert.ToInt32(dr["ID"]),
                                                 ProviderFullName = Convert.ToString(dr["FullName"]),
                                                 Businessname = Convert.ToString(dr["BusinessName"]),
                                                 Address = Convert.ToString(dr["l_address"]),
                                                 CityNm = Convert.ToString(dr["l_city"]),
                                                 Country = Convert.ToString(dr["l_county"]),
                                                 State_Name = Convert.ToString(dr["l_state"]),
                                                 ZIPValue = Convert.ToString(dr["l_zipcode"]),
                                                 Vmoffice = Convert.ToString(dr["l_vmoffice"]),
                                                 Fax = Convert.ToString(dr["l_fax"]),
                                                 Paragraph = Convert.ToString(dr["l_Paragraph"]),
                                                 Picture = Convert.ToString(dr["Picture"]),
                                                 PublicURL = Convert.ToString(dr["PublicURL"]),
                                                 Random_number = Convert.ToString(dr["Random_number"]),
                                                 distance = (dr["distance"] != DBNull.Value ? Convert.ToInt32(dr["distance"]) : 0)
                                             });
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        TotalNoofRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetProvidersListForAdvanceSearch", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public int CountingStateswiseSearch(string ProviderIDS, char? SiteStats_ind, char? listingstats_ind, string State_Name = null, string CityNm = null)
        {
            try
            {
                IDataParameter[] InparamList = {
			new SqlParameter("@In_ProviderIDs", (ProviderIDS ?? null)),
			new SqlParameter("@In_SiteStats_ind", (SiteStats_ind ?? null)),
			new SqlParameter("@In_listingstats_ind", (listingstats_ind ?? null)),
			new SqlParameter("@In_State_Name", State_Name),
			new SqlParameter("@In_City_Name", CityNm)
		};
                var objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InparamList);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_ListingStatistics");
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "CountingStateswiseSearch", HttpContext.Current.Request, HttpContext.Current.Session);
                return 0;
            }

        }
        public List<ProviderAdvertising> GetProviders_ListWithOutRadius_ForAdvanceSearch(ProviderAdvertising obj, ref int TotalNoofRecords)
        {
            try
            {
                var objDataList = new List<ProviderAdvertising>();
                var objCommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_CityName", (obj.CityNm ?? null)),
			new SqlParameter("@In_State_ID", (obj.State_ID == 0 ? null : obj.State_ID)),
			new SqlParameter("@In_City_ID", (obj.City_ID == 0 ? null : obj.City_ID)),
			new SqlParameter("@In_Zip", (obj.ZIPValue ?? null )),
			new SqlParameter("@In_Distance", (obj.Radius == 0 ? null : obj.Radius)),
			new SqlParameter("@In_PageNo", (obj.PageNo == 0 ? null : obj.PageNo)),
			new SqlParameter("@In_NoOfRecords", (obj.NoOfRecords == 0 ? null : obj.NoOfRecords)),
			new SqlParameter("@in_Lastname ", (obj.LastName ?? null )),
			new SqlParameter("@in_Firstfreesession ", (obj.firstfee ?? null ))
		};

                objCommon.AddInParameters(InParamList);
                DataSet ds = objCommon.GetDataSet("Help_dbo.st_Provider_LIST_AdvancedSearchProvidersziponly");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                             select new ProviderAdvertising
                                             {
                                                 ProviderID = Convert.ToInt32(dr["ID"]),
                                                 ProviderFullName = Convert.ToString(dr["FullName"]),
                                                 Businessname = Convert.ToString(dr["BusinessName"]),
                                                 Address = Convert.ToString(dr["l_address"]),
                                                 CityNm = Convert.ToString(dr["l_city"]),
                                                 Country = Convert.ToString(dr["l_county"]),
                                                 State_Name = Convert.ToString(dr["l_state"]),
                                                 ZIPValue = Convert.ToString(dr["l_zipcode"]),
                                                 Vmoffice = Convert.ToString(dr["l_vmoffice"]),
                                                 Fax = Convert.ToString(dr["l_fax"]),
                                                 Paragraph = Convert.ToString(dr["l_Paragraph"]),
                                                 Picture = Convert.ToString(dr["Picture"]),
                                                 PublicURL = Convert.ToString(dr["PublicURL"]),
                                                 Random_number = Convert.ToString(dr["Random_number"]),
                                                 distance = 0
                                             });
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        TotalNoofRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ProviderAdvertising.fulladdress = (ds.Tables[2].Rows[0][0]).ToString();
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetProviders_ListWithOutRadius_ForAdvanceSearch", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public List<ProviderAdvertising> GeneralListProviders(string ZipValue, int? Distance, string LastName, int? CityID, int? PageNo, ref int TotalNoofRecords, ref string Outmsg)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objDataList = new List<ProviderAdvertising>();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Zip", (ZipValue ?? null)),
			new SqlParameter("@In_Distance", (Distance == 0 ? null : Distance)),
			new SqlParameter("@In_LastName", (LastName ?? null)),
			new SqlParameter("@In_City_ID", (CityID == 0 ? null : CityID)),
			new SqlParameter("@In_NoOfRecords", 10),
			new SqlParameter("@In_PageNo", (PageNo == 0 ? null : PageNo))
		};
                IDataParameter[] outparam = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 1000) };
                objcommon.AddInParameters(InParamList);
                objcommon.AddOutParameters(outparam);
                DataSet ds = objcommon.GetDataSet("Help_dbo.St_Public_List_ZipsearchProviders");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                             select new ProviderAdvertising
                                             {
                                                 ProviderID = Convert.ToInt32(dr["ID"]),
                                                 ProviderFullName = Convert.ToString(dr["FullName"]),
                                                 Businessname = Convert.ToString(dr["BusinessName"]),
                                                 Address = Convert.ToString(dr["l_address"]),
                                                 CityNm = Convert.ToString(dr["l_city"]),
                                                 Country = Convert.ToString(dr["l_county"]),
                                                 State_Name = Convert.ToString(dr["l_state"]),
                                                 ZIPValue = Convert.ToString(dr["l_zipcode"]),
                                                 Vmoffice = Convert.ToString(dr["l_vmoffice"]),
                                                 Fax = Convert.ToString(dr["l_fax"]),
                                                 Paragraph = Convert.ToString(dr["l_Paragraph"]),
                                                 Picture = Convert.ToString(dr["Picture"]),
                                                 PublicURL = Convert.ToString(dr["PublicURL"]),
                                                 Random_number = Convert.ToString(dr["Random_number"]),
                                                 distance = (dr["distance"] != DBNull.Value ? Convert.ToInt32(dr["distance"]) : 0)
                                             });
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        TotalNoofRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ProviderAdvertising.fulladdress = (ds.Tables[2].Rows[0][0]).ToString();
                    }
                }
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    Outmsg = objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GeneralListProviders", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public List<ProviderAdvertising> BindStatesList(string city)
        {
            try
            {
                var objDataList = new List<ProviderAdvertising>();
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = { new SqlParameter("@in_City", city) };
                objcommon.AddInParameters(objparam);
                DataSet dsStateslist = objcommon.GetDataSet("Help_dbo.st_Admin_Get_StateNameFromCity");
                if ((dsStateslist != null))
                {
                    if (dsStateslist.Tables.Count > 0)
                    {
                        if (dsStateslist.Tables[0].Rows.Count > 1)
                        {

                            objDataList.AddRange(from DataRow dr in dsStateslist.Tables[0].Rows
                                                 select new ProviderAdvertising
                                                 {
                                                     State_ID = Convert.ToInt32(dr["State_ID"]),
                                                     StateNme1 = Convert.ToString(dr["Statename"]),
                                                     State = Convert.ToString(dr["State"])

                                                 });

                        }
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "BindStatesList", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public List<ProviderAdvertising> ListProvidersBasedOnStateOrCountryOrCityIDs(string CityNm, string StateID, int? CountryID, int? PageNo, ref int TotalNoofRecords, ref string StrOutmsg)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objDataList = new List<ProviderAdvertising>();
                //
                IDataParameter[] InParamList = {
			new SqlParameter("@In_City", (CityNm ?? null)),
			new SqlParameter("@In_State_id", (StateID == null ? null : StateID)),
			new SqlParameter("@In_country_id", (CountryID == 0 ? null : CountryID)),
			new SqlParameter("@In_NoOfRecords", 10),
			new SqlParameter("@In_PageNo", (PageNo == 0 ? null : PageNo))
		};
                objcommon.AddInParameters(InParamList);
                IDataParameter[] OutParamList = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 500) };
                objcommon.AddOutParameters(OutParamList);
                DataSet ds = objcommon.GetDataSet("Help_dbo.St_Public_List_StateProviders_RDPAGING");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                             select new ProviderAdvertising
                                             {
                                                 ProviderID = Convert.ToInt32(dr["ID"]),
                                                 ProviderFullName = Convert.ToString(dr["FullName"]),
                                                 Businessname = Convert.ToString(dr["BusinessName"]),
                                                 Address = Convert.ToString(dr["l_address"]),
                                                 CityNm = Convert.ToString(dr["l_city"]),
                                                 Country = Convert.ToString(dr["l_county"]),
                                                 State_Name = Convert.ToString(dr["l_state"]),
                                                 ZIPValue = Convert.ToString(dr["l_zipcode"]),
                                                 Vmoffice = Convert.ToString(dr["l_vmoffice"]),
                                                 Fax = Convert.ToString(dr["l_fax"]),
                                                 Paragraph = Convert.ToString(dr["l_Paragraph"]),
                                                 Picture = Convert.ToString(dr["Picture"]),
                                                 PublicURL = Convert.ToString(dr["PublicURL"]),
                                                 Random_number = Convert.ToString(dr["Random_number"]),
                                                 distance = 0
                                             });
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        TotalNoofRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ProviderAdvertising.fulladdress = (ds.Tables[2].Rows[0][0]).ToString();
                    }
                }
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    StrOutmsg = objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "ListProvidersBasedOnStateOrCountryOrCityIDs", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public clsCountry GetStatecitywithZip(string strzip)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = { new SqlParameter("@In_zipcode", strzip) };
                var objData = new clsCountry();
                objcommon.AddInParameters(InParamList);
                SqlDataReader drread = objcommon.GetDataReader("Help_dbo.st_Public_GET_ZipCodeCityState");
                if (drread.Read())
                {
                    objData.CityName = drread["CITY"] != null ? drread["CITY"].ToString() : null;
                    objData.StateName = drread["STATE"] != null ? drread["STATE"].ToString() : null;
                }
                return objData;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetStatecitywithZip", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ProviderListPaging> GetPagingforDataList(ref int TotalNoofRecords)
        {
            try
            {
                var objDataList = new List<ProviderListPaging>();
                int noofPages = 0;
                if (TotalNoofRecords > 10)
                {
                    if ((TotalNoofRecords % 10) != 0)
                    {
                        int reNumber = 0;
                        noofPages = Math.DivRem(TotalNoofRecords, 10, out reNumber) + 1;
                    }
                    else
                    {
                        noofPages = (TotalNoofRecords / 10);
                    }
                }
                else
                {
                    noofPages = 1;
                }
                int cnt = 0;
                for (cnt = 1; cnt <= noofPages; cnt++)
                {
                    var objData = new ProviderListPaging { PageNo = cnt };
                    objDataList.Add(objData);
                    objData = null;
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetPagingforDataList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object Insert_NewRegProvidersDetails(Reg_ProvidersDetailInfo ObjInsertDetails, ref string Out_Msg, ref string Out_Provider_ID, ref int OutPractice_ID, ref int OutLogin_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@in_UserName", ObjInsertDetails.UserName),
			new SqlParameter("@in_Password", ObjInsertDetails.Password),
			new SqlParameter("@in_FirstName", ObjInsertDetails.FirstName),
			new SqlParameter("@In_MI", ObjInsertDetails.MI),
			new SqlParameter("@in_LastName", ObjInsertDetails.LastName),
			new SqlParameter("@in_Email", ObjInsertDetails.Email),
			new SqlParameter("@in_Address1", ObjInsertDetails.Address1),
			new SqlParameter("@in_Address2", ObjInsertDetails.Address2),
			new SqlParameter("@in_City_ID", ObjInsertDetails.City_ID),
			new SqlParameter("@in_State_ID", ObjInsertDetails.State_ID),
			new SqlParameter("@in_Country_ID", ObjInsertDetails.Country_ID),
			new SqlParameter("@in_ZIP", ObjInsertDetails.Zip),
			new SqlParameter("@in_WorkPhone", ObjInsertDetails.WorkPhone),
           new SqlParameter("@in_cellphone", ObjInsertDetails.CellPhone), 
			new SqlParameter("@in_PrimaryAddress_Ind", ObjInsertDetails.PrimaryAddress_Ind),
			new SqlParameter("@In_Businessname", ObjInsertDetails.Businessname),
			new SqlParameter("@In_IsSimplePractice", ObjInsertDetails.IsSimplePractice),
           new SqlParameter("@in_licenseno", (ObjInsertDetails.Licensenumber == "" ? null : ObjInsertDetails.Licensenumber.ToString())),
           new SqlParameter("@In_Registeredin", "W")
		};
                IDataParameter[] OutParam = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 500),
			new SqlParameter("@Out_Provider_ID", SqlDbType.VarChar, 500),
			new SqlParameter("@Out_Practice_ID", SqlDbType.VarChar, 500),
			new SqlParameter("@Out_Login_ID", SqlDbType.VarChar, 500)
		};

                objcommon.AddInParameters(objparm);
                objcommon.AddOutParameters(OutParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_provider_Ins_NEWFTProviderdetails");
                if ((objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value).ToString() != "")
                {
                    Out_Msg = (objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value).ToString();
                }
                else
                {
                    Out_Provider_ID = (objcommon.GetCurrentCommand.Parameters["@Out_Provider_ID"].Value).ToString();
                    if (objcommon.GetCurrentCommand.Parameters["@Out_Practice_ID"].Value != DBNull.Value)
                    {
                        OutPractice_ID = Convert.ToInt32(objcommon.GetCurrentCommand.Parameters["@Out_Practice_ID"].Value);
                    }
                    if ((objcommon.GetCurrentCommand.Parameters["@Out_Login_ID"].Value) != null)
                    {
                        OutLogin_ID = Convert.ToInt32(objcommon.GetCurrentCommand.Parameters["@Out_Login_ID"].Value);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Insert_NewRegProvidersDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object check_validRefferalCode(string CRM_Promocode, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@IN_CRM_Promocode", CRM_Promocode)
		};
                IDataParameter[] OutParam = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 500)
		};

                objcommon.AddInParameters(objparm);
                objcommon.AddOutParameters(OutParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_check_valid_refferal_code");
                if ((objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value).ToString() != "")
                {
                    Out_Msg = (objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value).ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "check_validRefferalCode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override void INS_providerRefferalCode(string Promocode, int provider_id)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@IN_CRM_Promocode", Promocode),
            new SqlParameter("@in_provider_id", provider_id)
		};

                objcommon.AddInParameters(objparm);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_INS_provider_Refferal_code");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "check_validRefferalCode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public List<clsCountry> GetZipcodesbasedonStatecities(int? StateID, int? CityID, string Directory_ID, string ZIP, int? Distance, string Cityname)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_State_ID", (StateID == 0 ? null : StateID)),
			new SqlParameter("@In_City_ID", (CityID == 0 ? null : CityID)),
			new SqlParameter("@In_City", (string.IsNullOrEmpty(Cityname) ? null : Cityname)),
			new SqlParameter("@In_Directory_ID", (string.IsNullOrEmpty(Directory_ID) ? null : Directory_ID)),
			new SqlParameter("@In_zipcode", (string.IsNullOrEmpty(ZIP) ? null : ZIP)),
			new SqlParameter("@In_Distance", (Distance == 0 ? null : Distance))
		};
                var objDataList = new List<clsCountry>();
                objcommon.AddInParameters(InParamList);
                DataSet ds = objcommon.GetDataSet("Help_dbo.st_Public_LIST_ProviderZipcodeLinks");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new clsCountry { Zipcode = Convert.ToString(dr["l_zipcode"]) });
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetZipcodesbasedonStatecities", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override int PatientInsertCCTransactionDetails(BLL.Billing.CCProcess objcc, int? loginhistoryid)
        {
            try
            {
                int LocTransactionID;
                var objcommon = new clsCommonFunctions();
                IDataParameter[] paramlist = {
		new SqlParameter("@in_FromReference_ID", objcc.StrPaidRefID),
		new SqlParameter("@in_FromReferenceType_ID", objcc.StrPaidRefTypeID),
		new SqlParameter("@in_ToReference_ID", objcc.StrChargebleRefID),
		new SqlParameter("@in_ToReferenceType_ID", objcc.StrChargebleRefTypeID),
		new SqlParameter("@in_Posted_Data", objcc.strpost),
		new SqlParameter("@in_InvoiceNum", null),
		new SqlParameter("@in_TransactionNo", null),
		new SqlParameter("@in_ManualTransaction_Ind", 0),
		new SqlParameter("@in_Amount", Convert.ToDouble(objcc.dblx_amount)),
		new SqlParameter("@in_CreditCardNo", null),
		new SqlParameter("@in_CreditCardType_Code", objcc.strx_card_code),
		new SqlParameter("@in_CVV2", null),
		new SqlParameter("@in_CardHolderName", null),
		new SqlParameter("@In_Firstname",objcc.FirstName),
		new SqlParameter("@In_Lastname", objcc.LastName),
		new SqlParameter("@In_IsPayPalProcess_Ind", objcc.IsPaypalInd),
		new SqlParameter("@in_CardExpiryMonth", objcc.StrExpMon),
		new SqlParameter("@in_CardExpiryYear", objcc.StrExpYear),
		new SqlParameter("@in_BillingAddress_1", objcc.strBillAddress1),
		new SqlParameter("@in_BillingAddress_2", objcc.strBillAddress2),
		new SqlParameter("@in_BillingCity_ID", objcc.strCityID),
		new SqlParameter("@in_BillingState_ID", objcc.strStateID),
		new SqlParameter("@in_BillingCountry_ID", 1),
		new SqlParameter("@in_BillingZIP", objcc.strZipCode),
		new SqlParameter("@in_ParentTransaction_ID", null),
		new SqlParameter("@in_IsRefundable_Ind", "0"),
		new SqlParameter("@in_CreatedBy", null),
		new SqlParameter("@in_Practice_ID", objcc.strPracticeID ?? null),
		new SqlParameter("@In_RefpatientLogin_ID", objcc.RefLoginID),
		new SqlParameter("@in_provider_id", (objcc.Provider_ID != "0" ? objcc.Provider_ID : null)),
		new SqlParameter("@in_loginhistory_id", (loginhistoryid > 0 ? loginhistoryid : null)),
        new SqlParameter("@in_paypaltransactionid", (objcc.paypaltransactionid ?? null)),
        new SqlParameter("@in_paypalsaletransactionid", (objcc.paypalsaletransactionid ?? null))
	};
                objcommon.AddInParameters(paramlist);
                DataSet dset = objcommon.GetDataSet("Help_dbo.st_CreditCard_INS_Transaction");
                if (dset != null)
                {
                    LocTransactionID = dset.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dset.Tables[0].Rows[0]["TransactionID"]) : 0;
                    return LocTransactionID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "PatientInsertCCTransactionDetails", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return 0;
        }
        public override bool ProviderConfirmation(Reg_ProviderConfirmation ObjConfirmation)
        {
            try
            {
                var common = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
		new SqlParameter("@in_Provider_ID", (ObjConfirmation.Provider_ID ?? null)),
		new SqlParameter("@in_CreatedBy", (ObjConfirmation.CreatedBy ?? null))
	};


                common.AddInParameters(InParamlist);
                common.ExecuteNonQuerySP("Help_dbo.st_Provider_Registration_Confirm");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "ProviderConfirmation", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;

        }
        public override Reg_ProviderConfirmation Reg_GetUserNameAndPassword(int Provider_ID)
        {
            try
            {
                SqlDataReader drSimpleReg = null;
                var objSimpleReg = new Reg_ProviderConfirmation();
                var clscomm = new clsCommonFunctions();
                if (HttpContext.Current.Session["RoleID"] != null)
                {
                    if (HttpContext.Current.Session["RoleID"].ToString() == "31" || HttpContext.Current.Session["RoleID"].ToString() == "38")
                    {
                        IDataParameter[] Inparam =
                        {
                            new SqlParameter("@In_login_ID",
                                (Provider_ID.ToString() == "0" ? null : Provider_ID.ToString()))  //here Provider_ID nothing but loginid
                        };
                        clscomm.AddInParameters(Inparam);
                        drSimpleReg = clscomm.GetDataReader("Help_dbo.st_verificationuser_Get_UserNameAndPassword");
                    }
                    else
                    {
                        IDataParameter[] Inparam =
                    {
                        new SqlParameter("@In_Provider_ID",
                            (Provider_ID.ToString() == "0" ? null : Provider_ID.ToString()))
                    };
                        clscomm.AddInParameters(Inparam);
                        drSimpleReg = clscomm.GetDataReader("Help_dbo.st_SimpleProvider_Get_UserNameAndPassword");
                    }
                }
                else
                {
                    IDataParameter[] Inparam =
                    {
                        new SqlParameter("@In_Provider_ID",
                            (Provider_ID.ToString() == "0" ? null : Provider_ID.ToString()))
                    };
                    clscomm.AddInParameters(Inparam);
                    drSimpleReg = clscomm.GetDataReader("Help_dbo.st_SimpleProvider_Get_UserNameAndPassword");
                }

                if (drSimpleReg.Read())
                {
                    objSimpleReg.ProviderLogin_ID = drSimpleReg["Login_id"] != null
                        ? (int?)Convert.ToInt32(drSimpleReg["Login_id"])
                        : null;
                    objSimpleReg.UserName = (drSimpleReg["UserName"] != null) ? drSimpleReg["UserName"].ToString() : null;
                    objSimpleReg.Password = (drSimpleReg["Password"] != null) ? drSimpleReg["Password"].ToString() : null;
                    objSimpleReg.Email = (drSimpleReg["Email"] != null) ? drSimpleReg["Email"].ToString() : null;
                    objSimpleReg.providername = (drSimpleReg["providername"] != null) ? drSimpleReg["providername"].ToString() : null;
                }
                return objSimpleReg;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Reg_GetUserNameAndPassword", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override EmailMessageOption GET_EmailMessage_OptionBasedonID(int id, int? EmailMessage_Option_ID = 0)
        {
            try
            {
                var objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@In_EmailMsgOptionBody_ID", (id.ToString() == "0" ? null : id.ToString())),
			new SqlParameter("@In_EmailMessage_Option_ID", (EmailMessage_Option_ID.ToString() == "0" ||EmailMessage_Option_ID.ToString() == "" ? null : EmailMessage_Option_ID.ToString()))
		};
                objclsdbwraper.AddInParameters(objparm);
                SqlDataReader ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_EmailMessage_Options");
                var EMO = new EmailMessageOption();
                if (ReturnData.Read())
                {
                    EMO = new EmailMessageOption(Convert.ToString(ReturnData["Message_Title"]), Convert.ToString(ReturnData["Msg_Subject"]), Convert.ToString(ReturnData["Msg_Body"]), ((ReturnData["Msg_Footer"].ToString() != null ? ReturnData["Msg_Footer"].ToString() : null)), Convert.ToInt32(ReturnData["Email_OptionType_ID"]));
                    EMO.ServiceIDs = (ReturnData["BillingService_IDs"].ToString() != null ? ReturnData["BillingService_IDs"].ToString() : null);
                    EMO.EmailMessage_Option_ID = Convert.ToInt32(ReturnData["EmailMessage_Option_ID"].ToString() != null ? ReturnData["EmailMessage_Option_ID"].ToString() : null);
                    return EMO;

                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GET_EmailMessage_OptionBasedonID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Reg_Upd_Status(int? ProviderID, string Status_Ind, int? UpdatedBy, string PromocodeID, string vaultid, string customer_id)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
		new SqlParameter("@In_Practice_ID", null),
		new SqlParameter("@in_Status_Ind", Status_Ind),
		new SqlParameter("@in_UpdatedBy", (UpdatedBy == 0 ? null : UpdatedBy)),
		new SqlParameter("@In_Promocode_ID", (PromocodeID != null ? PromocodeID : null)),
        new SqlParameter("@In_IsUpgrade_Ind", null),
        new SqlParameter("@In_ServiceSt", null),
        new SqlParameter("@in_paypal_vaultid", (vaultid ?? null)),
           new SqlParameter("@in_provider_id", ProviderID == 0 ? null : ProviderID),
           new SqlParameter("@in_customer_id", customer_id)

	};


                objcommon.AddInParameters(InParamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_New_Practice_UPD_ChangeStatus");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Reg_Upd_Status", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override void Ins_trialpackage_users(int Login_ID)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
		new SqlParameter("@in_login_id", Login_ID)

	};


                objcommon.AddInParameters(InParamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.ST_INS_trialpackage_users");
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Ins_trialpackage_users", HttpContext.Current.Request, HttpContext.Current.Session);
            }
        }
        public override string Reg_Insert_EmailMessage(Reg_ProviderConfirmation ObjEmailIns)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
			new SqlParameter("@In_ProviderLogin_ID", (ObjEmailIns.ProviderLogin_ID == 0 ? null : ObjEmailIns.ProviderLogin_ID)),
			new SqlParameter("@In_Practice_ID", null),
			new SqlParameter("@In_Email", ObjEmailIns.Email)
		};
                IDataParameter[] outparam = { 
                                            new SqlParameter("@Out_emailtransaction_id",SqlDbType.Int)
                                            };

                var _with1 = common;
                _with1.AddInParameters(InParamlist);
                common.AddOutParameters(outparam);
                _with1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_Ins_SimpleproviderRegMessage");
                string strEmaiId = null;
                if (!DBNull.Value.Equals(common.objdbCommandWrapper.Parameters["@Out_emailtransaction_id"].Value))
                {
                    strEmaiId = common.objdbCommandWrapper.Parameters["@Out_emailtransaction_id"].Value.ToString();
                }
                return strEmaiId;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Reg_Insert_EmailMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            //return strEmaiId;
            return null;
        }
        public string PasswordManage_ins_EmailMessage(Reg_ProviderConfirmation ObjEmailIns)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
			new SqlParameter("@In_ProviderLogin_ID", (ObjEmailIns.ProviderLogin_ID == 0 ? null : ObjEmailIns.ProviderLogin_ID)),
			new SqlParameter("@In_Practice_ID", null),
			new SqlParameter("@In_Email", ObjEmailIns.Email)
		};

                IDataParameter[] outparam = { 
                                            new SqlParameter("@Out_emailtransaction_id",SqlDbType.Int)
                                            };

                var _with1 = common;
                _with1.AddInParameters(InParamlist);
                common.AddOutParameters(outparam);
                _with1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_Ins_passwordmanagement");
                string strEmaiId = null;
                if (!DBNull.Value.Equals(common.objdbCommandWrapper.Parameters["@Out_emailtransaction_id"].Value))
                {
                    strEmaiId = common.objdbCommandWrapper.Parameters["@Out_emailtransaction_id"].Value.ToString();
                }
                return strEmaiId;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Reg_Insert_EmailMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            //return strEmaiId;
            return null;
        }
        public override Reg_ProvidersDetailInfo Get_ProviderAddressDetails(int prov_id)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] inparamlist = { new SqlParameter("@in_provider_id", (prov_id.ToString() == "0" ? null : prov_id.ToString())) };

                objcommon.AddInParameters(inparamlist);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.St_Get_Provider_address");
                var objGet = new Reg_ProvidersDetailInfo();
                if (objread.Read() == true)
                {
                    objGet.FirstName = (objread["FirstName"].ToString() == null ? null : objread["FirstName"].ToString());
                    objGet.LastName = (objread["LastName"].ToString() == null ? null : objread["LastName"].ToString());
                    objGet.Address1 = (objread["Address1"].ToString() == null ? null : objread["Address1"].ToString());
                    objGet.Address2 = (objread["Address2"].ToString() == null ? null : objread["Address2"].ToString());
                    objGet.City_ID = Convert.ToInt32(objread["City_ID"].ToString() == null ? null : objread["City_ID"].ToString());
                    objGet.State_ID = Convert.ToInt32(objread["State_ID"].ToString() == null ? null : objread["State_ID"].ToString());
                    objGet.Zip = (objread["Zip"].ToString() == null ? null : objread["Zip"].ToString());
                    objGet.Statename = (objread["State_name"].ToString() == null ? null : objread["State_name"].ToString());
                    objGet.Cityname = (objread["City_name"].ToString() == null ? null : objread["City_name"].ToString());
                }
                return objGet;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_ProviderAddressDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool InsertPassword(string Password, int? UpdatedBy, int? Login_ID)
        {
            try
            {
                var objCommon = new clsCommonFunctions();
                IDataParameter[] objParam = {
		new SqlParameter("@in_Password", (Password ?? null)),
		new SqlParameter("@in_UpdatedBy", (UpdatedBy != 0 ? UpdatedBy : null)),
		new SqlParameter("@in_Login_ID", (Login_ID != 0 ? Login_ID : null)),
		new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
	};
                objCommon.AddInParameters(objParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Security_UPD_Password");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "InsertPassword", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override List<ListMessage> ListMessages(int Reciever_ID, int RoleID, int MsgCategory_ID, string ListingMsgsType, int NoofRecords, int PageNo, string OrderBy, string OrderBYitem, ref int Total_Records)
        {
            try
            {
                IDataParameter[] objParm = {
            new SqlParameter("@in_Reciever_ID", Reciever_ID),
            new SqlParameter("@in_Role_ID", RoleID),
            new SqlParameter("@in_MsgCategory_ID", MsgCategory_ID),
            new SqlParameter("@in_ListingMsgsType", ListingMsgsType),
            new SqlParameter("@in_NoofRecords", NoofRecords),
            new SqlParameter("@in_PageNo", PageNo),
            new SqlParameter("@In_OrderBy", OrderBy),
            new SqlParameter("@In_OrderBYitem", OrderBYitem)
        };

                var objclsDBWrapper = new clsDBWrapper();
                objclsDBWrapper.AddInParameters(objParm);
                SqlDataReader returnData = objclsDBWrapper.GetDataReader((RoleID == 1 ? "Help_dbo.st_Admin_MessageStation_List_Messages_RDPaging" : "Help_dbo.st_MessageStation_List_Messages_RDPaging"));
                var userCollection = new List<ListMessage>();
                int _SlNo = ((PageNo - 1) * NoofRecords) + 1;
                while (returnData.Read())
                {
                    var newMs_ListMessage = new ListMessage();
                    if (returnData["Message_ID"] != null)
                    {
                        newMs_ListMessage.Message_ID = Convert.ToInt32(returnData["Message_ID"]);
                    }
                    if (returnData["Subject"] != null)
                    {
                        if (returnData["Reply_ID"].ToString() != null && returnData["Reply_ID"].ToString() != "")
                        {
                            newMs_ListMessage.Subject = "Re:" + returnData["Subject"].ToString();
                        }
                        else
                        {
                            newMs_ListMessage.Subject = returnData["Subject"].ToString();
                        }

                    }
                    if (returnData["Messagebody"] != null)
                    {
                        newMs_ListMessage.Messagebody = returnData["Messagebody"].ToString();
                    }
                    if (returnData["SendingDate"] != null)
                    {
                        newMs_ListMessage.SendingDate = returnData["SendingDate"].ToString();
                    }
                    if (returnData["MsgCategory_ID"] != null)
                    {
                        newMs_ListMessage.MsgCategory_ID = Convert.ToInt32(returnData["MsgCategory_ID"]);
                    }
                    if (returnData["Category"] != null)
                    {
                        newMs_ListMessage.Category = returnData["Category"].ToString();
                    }
                    newMs_ListMessage.Sender_ID = returnData["Sender_ID"] != null ? Convert.ToInt32(returnData["Sender_ID"]) : 0;
                    if (returnData["Sender"] != null)
                    {
                        newMs_ListMessage.Sender = returnData["Sender"].ToString();
                    }
                    if (returnData["Time"] != null)
                    {
                        newMs_ListMessage.Time = returnData["Time"].ToString();
                    }
                    if (returnData["Duration"] != null)
                    {
                        newMs_ListMessage.Duration = returnData["Duration"].ToString();
                    }
                    if (returnData["Archive_Ind"] != null)
                    {
                        newMs_ListMessage.ArchiveInd = returnData["Archive_Ind"].ToString();
                    }
                    if (returnData["Read_Ind"] != null)
                    {
                        newMs_ListMessage.Read_IndWrite = returnData["Read_Ind"].ToString();
                    }
                    if (returnData["Reply_ID"].ToString() != null && returnData["Reply_ID"].ToString() != "")
                    {
                        newMs_ListMessage.Reply_ID = Convert.ToInt32(returnData["Reply_ID"]);
                    }
                    newMs_ListMessage.SlNo = _SlNo;
                    _SlNo += 1;
                    userCollection.Add(newMs_ListMessage);
                }
                if (returnData.NextResult() == true)
                {
                    if (returnData.Read())
                    {
                        Total_Records = Convert.ToInt32(returnData["Total_Records"]);
                    }
                }
                return userCollection;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ListMessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override List<ListMessage> GetAptrequest_ListMessages(int Login_Id, int NoofRecords, int PageNo, ref int Total_Records)
        {
            try
            {
                IDataParameter[] objParm = {
            new SqlParameter("@in_provider_id", Login_Id),
            new SqlParameter("@in_noofrowsperpage", NoofRecords),
            new SqlParameter("@in_pageno", PageNo)
        };

                var objclsDBWrapper = new clsDBWrapper();
                objclsDBWrapper.AddInParameters(objParm);
                SqlDataReader returnData = objclsDBWrapper.GetDataReader("Help_dbo.St_Get_pending_appointmentlist");
                var userCollection = new List<ListMessage>();
                while (returnData.Read())
                {
                    var newMs_ListMessage = new ListMessage();
                    if (returnData["Appointment_ID"] != null)
                    {
                        newMs_ListMessage.Appointment_ID = Convert.ToInt32(returnData["Appointment_ID"]);
                    }
                    if (returnData["customername"] != null)
                    {
                        newMs_ListMessage.customername = returnData["customername"].ToString();
                    }
                    if (returnData["Appointment_Date_Time"] != null)
                    {
                        newMs_ListMessage.MsgDatetime = returnData["Appointment_Date_Time"].ToString();
                    }
                    //if (returnData["AppointmentDate"] != null)
                    //{
                    //    newMs_ListMessage.MsgDate = returnData["AppointmentDate"].ToString();
                    //}
                    if (returnData["Notes"] != null)
                    {
                        newMs_ListMessage.Subject = returnData["Notes"].ToString();
                    }
                    if (returnData["Customerphonenumber"] != null)
                    {
                        newMs_ListMessage.custphonenumber = returnData["Customerphonenumber"].ToString();
                    }
                    if (returnData["Customeraddress"] != null)
                    {
                        newMs_ListMessage.Customeraddress = returnData["Customeraddress"].ToString();
                    }
                    if (returnData["patient_id"] != null)
                    {
                        newMs_ListMessage.Patient_Id = returnData["patient_id"].ToString();
                    }
                    userCollection.Add(newMs_ListMessage);
                }
                if (returnData.NextResult() == true)
                {
                    if (returnData.Read())
                    {
                        Total_Records = Convert.ToInt32(returnData["Totalrecords"]);
                    }
                }
                return userCollection;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetAptrequest_ListMessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override ListMessage LoadMessageDetails(int Message_ID, Int32 Role_ID)
        {
            var sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@in_Message_ID", SqlDbType.Int, 0, ParameterDirection.Input, Message_ID);
            AddParamToSQLCmd(sqlCmd, "@in_Role_ID", SqlDbType.Int, 0, ParameterDirection.Input, Role_ID);
            AddParamToSQLCmd(sqlCmd, "@in_Reciever_ID", SqlDbType.Int, 0, ParameterDirection.Input, HttpContext.Current.Session["userid"]);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_GET_Message");
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            var cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                SqlDataReader dtr = sqlCmd.ExecuteReader();
                ListMessage objMessage = default(ListMessage);
                if (dtr.Read())
                {
                    objMessage = new ListMessage(Convert.ToInt32(dtr["Message_ID"]), (dtr["SenderName"] != null ? dtr["SenderName"].ToString() : ""), (dtr["Subject"] != null ? dtr["Subject"].ToString() : ""), (dtr["Messagebody"] != null ? dtr["Messagebody"].ToString() : ""), (dtr["MsgDate"] != null ? dtr["MsgDate"].ToString() : ""), Convert.ToInt32(dtr["MsgLevel"] != null ? dtr["MsgLevel"] : 0), (dtr["OpenedBy"] != null ? dtr["OpenedBy"].ToString() : ""), (dtr["OpenedByName"] != null ? dtr["OpenedByName"].ToString() : ""), (dtr["OpenedOn"] != null ? dtr["OpenedOn"].ToString() : ""), (dtr["Read_Ind"] != null ? dtr["Read_Ind"].ToString() : ""),
                    (dtr["Archive_Ind"] != null ? dtr["Archive_Ind"].ToString() : ""), (dtr["MsgCategory"] != null ? dtr["MsgCategory"].ToString() : ""), (dtr["ArchivedBy"] != null ? dtr["ArchivedBy"].ToString() : ""), (dtr["ArchivedByName"] != null ? dtr["ArchivedByName"].ToString() : ""), (dtr["ArchivedOn"].ToString() != null ? dtr["ArchivedOn"].ToString() : ""), Convert.ToInt32(dtr["MsgCategory_ID"] != null ? dtr["MsgCategory_ID"] : 0), Convert.ToInt32(dtr["Sender_ID"] != null ? dtr["Sender_ID"] : 0));
                    objMessage.Practice_ID = (dtr["Practice_ID"] ?? 0);
                    objMessage.BillingServicesComapny_ID = (dtr["BillingServicesComapny_ID"] ?? 0);

                    return objMessage;
                }
                cn.Close();
                return null;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "LoadMessageDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override MessageAttachment GetMessageAttachmentById(int Message_ID)
        {
            MessageAttachment attachment = null;
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@in_Message_ID", SqlDbType.Int, 0, ParameterDirection.Input, Message_ID);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_GET_Attachment");
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            SqlConnection cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                SqlDataReader dtr = sqlCmd.ExecuteReader();
                if (dtr.Read())
                {
                    attachment = new MessageAttachment(Convert.ToInt32(dtr["Attach_ID"]), Convert.ToString(dtr["AttachedFileName"]), Convert.ToString(dtr["AttachedFileName_Encrypted"]));
                }
                cn.Close();
                return attachment;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetMessageAttachmentById", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override ListMessage ReplyLoadMessageDetails(int Message_ID, Int32 Role_ID)
        {
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@in_Message_ID", SqlDbType.Int, 0, ParameterDirection.Input, Message_ID);
            AddParamToSQLCmd(sqlCmd, "@in_Role_ID", SqlDbType.Int, 0, ParameterDirection.Input, Role_ID);
            AddParamToSQLCmd(sqlCmd, "@in_Reciever_ID", SqlDbType.Int, 0, ParameterDirection.Input, HttpContext.Current.Session["userid"]);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_GET_Message");
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            SqlConnection cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                SqlDataReader dtr = sqlCmd.ExecuteReader();
                ListMessage objMessage = null;
                if (dtr.Read())
                {
                    objMessage = new ListMessage(Convert.ToInt32(dtr["Message_ID"]), Convert.ToInt32(dtr["ReplyToRoleID"].ToString() != null ? dtr["ReplyToRoleID"].ToString() : "0"), Convert.ToInt32(dtr["Sender_ID"] != null ? dtr["Sender_ID"] : 0), Convert.ToString(dtr["subject"].ToString() != null ? dtr["subject"].ToString() : ""), Convert.ToString(dtr["Messagebody"] != null ? dtr["Messagebody"].ToString() : ""), Convert.ToString((dtr["Read_Ind"] != null ? dtr["Read_Ind"] : "")), Convert.ToString(dtr["Archive_Ind"] != null ? dtr["Archive_Ind"] : ""), Convert.ToString(dtr["MsgCategory"] != null ? dtr["MsgCategory"] : ""), Convert.ToString(dtr["SenderName"] != null ? dtr["SenderName"] : ""), Convert.ToInt32(dtr["MsgCategory_ID"] != null ? dtr["MsgCategory_ID"] : 0));
                    cn.Close();
                    return objMessage;
                }
                return null;
                
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ReplyLoadMessageDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override void ChangeStatusToArchive(int Message_id, Int32 Role_id, string sentArchive_ind)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@in_Message_ID", SqlDbType.Int, 0, ParameterDirection.Input, Message_id);
                AddParamToSQLCmd(sqlCmd, "@in_Reciever_ID", SqlDbType.Int, 0, ParameterDirection.Input, Role_id);
                AddParamToSQLCmd(sqlCmd, "@in_sentarchive_ind", SqlDbType.Char, 0, ParameterDirection.Input, sentArchive_ind);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_Archive_Messages");
                ExecuteScalarCmd(sqlCmd);
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ChangeStatusToArchive", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private Object ExecuteScalarCmd(SqlCommand sqlCmd)
        {
            try
            {
                if (ConnectionString == string.Empty)
                {
                    throw new ArgumentOutOfRangeException("ConnectionString");
                }
                if (sqlCmd == null)
                {
                    throw new ArgumentNullException("sqlCmd");
                }
                Object result = null;

                SqlConnection cn = new SqlConnection(this.ConnectionString);
                try
                {
                    sqlCmd.Connection = cn;
                    cn.Open();
                    result = sqlCmd.ExecuteScalar();
                }
                finally
                {
                    cn.Dispose();
                }

                return result;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ExecuteScalarCmd", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override int InsertMessage(MowerHelper.Models.BLL.MessageStation.Message newMessage)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = {
			new SqlParameter("@in_Sender_ID", (newMessage.Sender_ID != 0 ? newMessage.Sender_ID.ToString() : null)),
			new SqlParameter("@in_Subject", newMessage.Subject),
			new SqlParameter("@in_Messagebody", newMessage.Messagebody),
			new SqlParameter("@in_MsgCategory_ID", (newMessage.MsgCategory_ID != 0 ? newMessage.MsgCategory_ID.ToString() : null)),
			new SqlParameter("@in_AddSignature", null),
			new SqlParameter("@in_Reply_ID", (newMessage.Reply_ID != 0 ? newMessage.Reply_ID.ToString() : null)),
			new SqlParameter("@in_Patient_ID", (newMessage.Patient_ID == 0 ? null : newMessage.Patient_ID.ToString())),
			new SqlParameter("@in_CreatedBy", newMessage.Sender_ID),
			new SqlParameter("@in_From_RoleID", HttpContext.Current.Session["Roleid"]),
			new SqlParameter("@in_Reciever_RoleID", newMessage.Reciever_RoleID),
			new SqlParameter("@in_Reciever_IDs", (!string.IsNullOrEmpty(newMessage.Reciever_IDs) ? newMessage.Reciever_IDs : null)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                IDataParameter objretparam = new SqlParameter("@Loc_Message_ID", null);
                objcommon.AddInParameters(objparam);
                objcommon.AddReturnParameters(objretparam);
                objcommon.ExecuteFunction("Help_dbo.st_MessageStation_INS_Message");
                if (objcommon.GetCurrentCommand.Parameters["@Loc_Message_ID"].Value.ToString() != null)
                {
                    return (int)objcommon.GetCurrentCommand.Parameters["@Loc_Message_ID"].Value;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "InsertMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public override int InsertGroupMessage(MowerHelper.Models.BLL.MessageStation.Message newMessage)
        {
            try
            {
                if (newMessage == null)
                {
                    throw new ArgumentNullException("newMessage");
                }
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@Ret_Message_ID", SqlDbType.Int, 0, ParameterDirection.ReturnValue, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_Sender_ID", SqlDbType.Int, 0, ParameterDirection.Input, newMessage.Sender_ID);
                AddParamToSQLCmd(sqlCmd, "@in_Subject", SqlDbType.VarChar, 200, ParameterDirection.Input, newMessage.Subject);
                AddParamToSQLCmd(sqlCmd, "@in_Messagebody", SqlDbType.VarChar, 300, ParameterDirection.Input, newMessage.Messagebody);
                AddParamToSQLCmd(sqlCmd, "@in_MsgCategory_ID", SqlDbType.Int, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_AddSignature", SqlDbType.VarChar, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_Reply_ID", SqlDbType.Int, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, newMessage.Sender_ID);
                AddParamToSQLCmd(sqlCmd, "@in_From_RoleID", SqlDbType.NVarChar, 100, ParameterDirection.Input, HttpContext.Current.Session["Roleid"]);
                AddParamToSQLCmd(sqlCmd, "@in_Reciever_RoleID", SqlDbType.NVarChar, 100, ParameterDirection.Input, newMessage.Reciever_RoleID);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_INS_GroupMessage");
                ExecuteScalarCmd(sqlCmd);
                return Convert.ToInt32(sqlCmd.Parameters["@Ret_Message_ID"].Value);
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "InsertGroupMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public override int CreateNewMessageAttachment(MessageAttachment newAttachment)
        {
            try
            {
                if (newAttachment == null)
                {
                    throw new ArgumentNullException("newAttachment");
                }
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@Ret_Message_ID", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@in_Message_ID", SqlDbType.Int, 0, ParameterDirection.Input, newAttachment.Attach_ID);
                AddParamToSQLCmd(sqlCmd, "@in_Title", SqlDbType.NChar, 256, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_AttachedFileName", SqlDbType.NText, 256, ParameterDirection.Input, newAttachment.AttachedFileName);
                AddParamToSQLCmd(sqlCmd, "@in_AttachedFileName_Encrypted", SqlDbType.NText, 250, ParameterDirection.Input, newAttachment.AttachedFileName_Encrypted);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_INS_Attachment");
                ExecuteScalarCmd(sqlCmd);
                return Convert.ToInt32(sqlCmd.Parameters["@Ret_Message_ID"].Value);
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "CreateNewMessageAttachment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public override bool Insert_NewRegProviderCreditcard(BLL.Billing.CCProcess objCreditCard, ref int? Out_CCID,string CardId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();

                IDataParameter[] CParamList = {
		new SqlParameter("@in_ReferenceType_ID", objCreditCard.StrChargebleRefTypeID),
		new SqlParameter("@in_Reference_ID", objCreditCard.StrChargebleRefID),
		new SqlParameter("@in_CreditCardNo",objCreditCard.LastFourdigitCCNo),
		new SqlParameter("@in_CreditCardType_Code", objCreditCard.strx_card_code),
		new SqlParameter("@in_CVV2", null),
		new SqlParameter("@in_CardHolderName", null),
		new SqlParameter("@in_CardExpiryMonth", objCreditCard.StrExpMon),
		new SqlParameter("@in_CardExpiryYear", objCreditCard.StrExpYear),
		new SqlParameter("@in_BillingAddress_1", objCreditCard.strBillAddress1),
		new SqlParameter("@in_BillingAddress_2", objCreditCard.strBillAddress2),
		new SqlParameter("@in_BillingCity_ID", objCreditCard.strCityID),
		new SqlParameter("@in_BillingState_ID", objCreditCard.strStateID),
		new SqlParameter("@in_BillingCountry_ID", "1"),
		new SqlParameter("@in_BillingZIP", objCreditCard.strZipCode),
		new SqlParameter("@in_CreatedBy", objCreditCard.CreatedBy),
		new SqlParameter("@in_AllowCCCharges", (objCreditCard.ALLowCCCharges ?? null)),
		new SqlParameter("@In_Firstname",objCreditCard.FirstName),
		new SqlParameter("@in_practice_ind", (!string.IsNullOrEmpty(objCreditCard.practice_ind) ? objCreditCard.practice_ind : null)),
		new SqlParameter("@In_Lastname", objCreditCard.LastName),
        new SqlParameter("@In_creditcardinfo_id", CardId),
        new SqlParameter("@In_IssuingBank",(!string.IsNullOrEmpty(objCreditCard.IssuingBank) ? objCreditCard.IssuingBank : null))
	};
                IDataParameter[] OutParam = {
			new SqlParameter("@Out_CCID", SqlDbType.BigInt)
                                            };
                objcommon.AddOutParameters(OutParam);
                objcommon.AddInParameters(CParamList);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_NewReg_Provider_CreditCard_INS_Card");
                if (!DBNull.Value.Equals(objcommon.GetCurrentCommand.Parameters["@Out_CCID"].Value))
                {
                    Out_CCID = Convert.ToInt32(objcommon.GetCurrentCommand.Parameters["@Out_CCID"].Value);
                }
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_NewRegProviderCreditcard", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }
        }
        public ProviderAdvertising GetTheProviderProfile(string randomnumber)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                var objData = new ProviderAdvertising();
                IDataParameter[] InParamList = {
			new SqlParameter("@in_random_number", (randomnumber!=null ? randomnumber : null))
			
		};
                objcommon.AddInParameters(InParamList);
                DataSet ds = objcommon.GetDataSet("Help_dbo.St_Provider_get_ProviderPublicProfile");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["Provider_id"] != null)
                            {
                                objData.ProviderID = (int)dr["Provider_id"];
                            }
                            else
                            {
                                objData.ProviderID = 0;
                            }
                            if (dr["FullName"] != null)
                            {
                                objData.ProviderFullName = (string)dr["FullName"];
                            }
                            else
                            {
                                objData.ProviderFullName = null;
                            }

                            if (dr["LastName"] != null)
                            {
                                objData.LastName = (string)dr["LastName"];
                            }
                            else
                            {
                                objData.LastName = null;
                            }
                            if (dr["FirstName"] != null)
                            {
                                objData.FirstName = (string)dr["FirstName"];
                            }
                            else
                            {
                                objData.FirstName = null;
                            }
                            if (dr["ZipLatitue"] != DBNull.Value)
                            {
                                if ((dr["ZipLatitue"] != null))
                                {
                                    string[] LatLog = dr["ZipLatitue"].ToString().Split(',');
                                    if (LatLog.Length > 1)
                                    {
                                        objData.LatitudeValue = LatLog[0];
                                        objData.LongitudeValue = LatLog[1];
                                    }
                                    else
                                    {
                                        objData.LatitudeValue = null;
                                        objData.LongitudeValue = null;
                                    }
                                }
                                else
                                {
                                    objData.LatitudeValue = null;
                                    objData.LongitudeValue = null;
                                }
                            }
                            else
                            {
                                objData.LatitudeValue = null;
                                objData.LongitudeValue = null;
                            }

                            if (dr["BusinessName"] != null)
                            {
                                objData.Businessname = (string)dr["BusinessName"];
                            }
                            else
                            {
                                objData.Businessname = null;
                            }
                            if (dr["l_GoogleMapPath"] != null)
                            {
                                objData.l_GoogleMapPath = (string)dr["l_GoogleMapPath"];
                            }
                            else
                            {
                                objData.l_GoogleMapPath = null;
                            }

                            if (dr["IsLicenseVerified"].ToString() != "")
                            {
                                objData.IsLicenseVerified = (string)dr["IsLicenseVerified"];
                            }
                            else
                            {
                                objData.IsLicenseVerified = null;
                            }

                            if (dr["Description"].ToString() != "")
                            {
                                objData.Description = (string)dr["Description"];
                            }
                            else
                            {
                                objData.Description = null;
                            }
                            if (dr["Payments"].ToString() != "")
                            {
                                objData.Payments = (string)dr["Payments"];
                            }
                            else
                            {
                                objData.Payments = null;
                            }
                            if (dr["Is_Contact_Info"].ToString() != "")
                            {
                                objData.Is_Contact_Info = (string)dr["Is_Contact_Info"];
                            }
                            else
                            {
                                objData.Is_Contact_Info = null;
                            }

                            if (dr["LicenseNo"].ToString() != "")
                            {
                                objData.License = (string)dr["LicenseNo"];
                            }
                            else
                            {
                                objData.License = null;
                            }

                            if (Convert.ToInt32(dr["VideoCount"]) != 0)
                            {
                                objData.VideoCount = (int)dr["VideoCount"];
                            }
                            else
                            {
                                objData.VideoCount = 0;
                            }
                            objData.Yearsinpractice = (dr["YearsInPractice"]) != DBNull.Value ? Convert.ToInt32(dr["YearsInPractice"]) : 0;
                            objData.Address = dr["l_address"] != DBNull.Value ? (string)dr["l_address"] : null;
                            if (dr["l_city"] != DBNull.Value)
                            {
                                objData.CityNm = (string)dr["l_city"];
                            }
                            else
                            {
                                objData.CityNm = null;
                            }
                            if (dr["l_county"] != DBNull.Value)
                            {
                                objData.Country = (string)dr["l_county"];
                            }
                            else
                            {
                                objData.Country = null;
                            }
                            if (dr["l_state"] != DBNull.Value)
                            {
                                objData.State_Name = (string)dr["l_state"];
                            }
                            else
                            {
                                objData.State_Name = null;
                            }
                            if (dr["Country_id"] != DBNull.Value)
                            {
                                objData.Country_ID = (int)dr["Country_id"];
                            }
                            else
                            {
                                objData.Country_ID = 0;
                            }
                            if (dr["State_id"] != DBNull.Value)
                            {
                                objData.State_ID = Convert.ToInt32(dr["State_id"]);
                            }
                            else
                            {
                                objData.State_ID = null;
                            }
                            if (dr["city_id"] != DBNull.Value)
                            {
                                objData.City_ID = (int)dr["city_id"];
                            }
                            else
                            {
                                objData.City_ID = null;
                            }

                            if (dr["l_zipcode"] != null)
                            {
                                objData.ZIPValue = (string)dr["l_zipcode"];
                            }
                            else
                            {
                                objData.ZIPValue = null;
                            }
                            if (dr["l_vmoffice"] != DBNull.Value)
                            {
                                objData.Vmoffice = (string)dr["l_vmoffice"];
                            }
                            else
                            {
                                objData.Vmoffice = null;
                            }
                            if (dr["CellPhone"] != DBNull.Value)
                            {
                                if (dr["CellPhone"] != null)
                                {
                                    objData.cellphone = (string)dr["CellPhone"];
                                }
                                else
                                {
                                    objData.cellphone = null;
                                }
                            }
                            else
                            {
                                objData.cellphone = null;
                            }
                            if (dr["l_fax"] != DBNull.Value)
                            {
                                objData.Fax = (string)dr["l_fax"];
                            }
                            else
                            {
                                objData.Fax = null;
                            }
                            if (dr["Website"] != DBNull.Value)
                            {
                                objData.Webaddress = (string)dr["Website"];
                            }
                            else
                            {
                                objData.Webaddress = null;
                            }
                            if (dr["Picture"] != DBNull.Value)
                            {
                                objData.Picture = (string)dr["Picture"];
                            }
                            else
                            {
                                objData.Picture = null;
                            }
                            if (dr["Email"] != null)
                            {
                                objData.Email = (string)dr["Email"];
                            }
                            else
                            {
                                objData.Email = null;
                            }
                            if (dr["PublicURL"] != DBNull.Value)
                            {
                                string str = dr["PublicURL"].ToString();
                                objData.PublicURL = str;
                            }
                            else
                            {
                                objData.PublicURL = null;
                            }
                            if (dr["FeeRange_From"] != DBNull.Value)
                            {
                                objData.Avgcostepersessionfrom = Convert.ToInt32(dr["FeeRange_From"]) != 0 ? Convert.ToString(dr["FeeRange_From"]) : null;

                            }
                            else
                            {
                                objData.Avgcostepersessionfrom = null;
                            }
                            if (dr["FeeRange_to"] != DBNull.Value)
                            {
                                objData.Avgcosepersessionto = Convert.ToInt32(dr["FeeRange_From"]) != 0 ? Convert.ToString(dr["FeeRange_to"]) : null;
                            }
                            else
                            {
                                objData.Avgcosepersessionto = null;
                            }
                            if (dr["facebookurl"] != DBNull.Value)
                            {
                                objData.facebookurl = (string)dr["facebookurl"];
                            }
                            else
                            {
                                objData.facebookurl = null;
                            }
                            if (dr["twitterurl"] != DBNull.Value)
                            {
                                objData.twitterurl = (string)dr["twitterurl"];
                            }
                            else
                            {
                                objData.twitterurl = null;
                            }
                            if (dr["Showcomments"] != DBNull.Value)
                            {
                                objData.fbcomments = (string)dr["Showcomments"];
                            }
                            else
                            {
                                objData.fbcomments = null;
                            }
                            if (dr["show_schedule"].ToString() != "" && dr["show_schedule"].ToString() != null)
                            {
                                objData.show_schedule = dr["show_schedule"].ToString();
                            }
                            else
                            {
                                objData.show_schedule = null;
                            }
                            objData.Random_number = randomnumber;
                        }
                    }
                }
                return objData;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetTheProviderProfile", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<MsgPtRequests> GetMsgPatientRequests(MsgPtRequests ObjMsgPtReuests)
        {
            clsCommonFunctions objcommon1 = new clsCommonFunctions();
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			new SqlParameter("@in_Login_ID", ObjMsgPtReuests.LoginID),
			new SqlParameter("@in_Role_ID", ObjMsgPtReuests.RoleID),
			new SqlParameter("@in_Order ", ObjMsgPtReuests.OrderBy),
			new SqlParameter("@in_OrderbyItem ", ObjMsgPtReuests.OrderByItem),
			new SqlParameter("@in_Category_ID", ObjMsgPtReuests.CategoryId)
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_MessageStation_LIST_PatientRequests");
                List<MsgPtRequests> MsgPatientRequests = new List<MsgPtRequests>();

                int SlNo = 0;

                while (objread.Read())
                {
                    SlNo += 1;

                    MsgPtRequests objMsgPtRequests = new MsgPtRequests();
                    objMsgPtRequests.SlNo = Convert.ToInt32(SlNo);
                    objMsgPtRequests.Request_ID = (objread["Request_ID"].ToString() == null ? 0 : Convert.ToInt32(objread["Request_ID"]));
                    objMsgPtRequests.RequestedBy = (objread["RequestedBy"].ToString() == null ? null : objread["RequestedBy"].ToString());
                    objMsgPtRequests.Patient_ID = (objread["Patient_ID"].ToString() == null ? 0 : Convert.ToInt32(objread["Patient_ID"]));
                    objMsgPtRequests.PatientLoginID = (objread["PatientLoginID"] == null ? 0 : Convert.ToInt32(objread["PatientLoginID"]));
                    objMsgPtRequests.Provider_ID = (objread["Provider_ID"] == null ? 0 : Convert.ToInt32(objread["Provider_ID"]));
                    objMsgPtRequests.ProviderName = (objread["ProviderName"].ToString() == "" ? null : objread["ProviderName"].ToString());
                    objMsgPtRequests.ChangeRelatedTo = (objread["ChangeRelatedTo"].ToString() == "" ? null : objread["ChangeRelatedTo"].ToString());
                    objMsgPtRequests.RequestedDate = (objread["RequestedDate"].ToString() == "" ? null : objread["RequestedDate"].ToString());
                    objMsgPtRequests.EffectiveDate = (objread["EffectiveDate"].ToString() == "" ? null : objread["EffectiveDate"].ToString());
                    objMsgPtRequests.PatientNotes = (objread["PatientNotes"].ToString() == "" ? null : objread["PatientNotes"].ToString());
                    objMsgPtRequests.ProviderNotes = (objread["ProviderNotes"].ToString() == "" ? null : objread["ProviderNotes"].ToString());
                    MsgPatientRequests.Add(objMsgPtRequests);
                }
                return MsgPatientRequests;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetMsgPatientRequests", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetPreviousNextRecords(int? State_ID, int? City_Id)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet ds = new DataSet();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@In_City_Id", (City_Id > 0 ? City_Id : null)),
			new SqlParameter("@In_State_id", (State_ID > 0 ? State_ID : null))
		};
                objcommon.AddInParameters(objInparamlist);
                ds = objcommon.GetDataSet("Help_dbo.St_public_List_City_State_RelatedProviders");
                return ds;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetPreviousNextRecords", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Tasks> Tasklist(Tasks objTasks)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_Task_ID", (objTasks.Task_ID != 0 ? objTasks.Task_ID.ToString() : null)),
			new SqlParameter("@in_Reference_ID", (objTasks.Reference_ID != null ? objTasks.Reference_ID.ToString() : null)),
			new SqlParameter("@In_referencetype_id", (objTasks.ReferenceType_ID != null ? objTasks.ReferenceType_ID.ToString() : null)),
			new SqlParameter("@In_Pracetice_id", (objTasks.Practice_ID != 0 ? objTasks.Practice_ID.ToString() : null)),
			new SqlParameter("@In_OrderBYitem", objTasks.sortField),
			new SqlParameter("@In_OrderBy", objTasks.sortDirection),
			new SqlParameter("@In_RoleInd", objTasks.RoleInd),
			new SqlParameter("@In_user_ID", HttpContext.Current.Session["userid"]),
			new SqlParameter("@In_Status_ind", (objTasks.Status_Ind != null ? objTasks.Status_Ind.ToString() : null)),
			new SqlParameter("@In_tasktitle", objTasks.TaskTitle),
			new SqlParameter("@in_Notes", objTasks.Notes),
			new SqlParameter("@In_Actionitemtitle", objTasks.Actionitemtitle),
			new SqlParameter("@in_Startdate", objTasks.StartDate),
			new SqlParameter("@in_Enddate", objTasks.EndDate),
           new SqlParameter("@In_Createdby", objTasks.CreatedBy)
		};

                objcommon.AddInParameters(paramlist);

                SqlDataReader objread = objcommon.GetDataReader(HttpContext.Current.Session["Roleid"].ToString() == "1" ? "Help_dbo.St_Admin_List_Task_List" : "Help_dbo.St_Task_List_Otherusers");


                var objTasklist = new List<Tasks>();
                while (objread.Read())
                {
                    var objTask = new Tasks();

                    objTask.Task_ID = objread["Task_ID"] != null ? (int)objread["Task_ID"] : 0;

                    objTask.IsDone = objread["IsDone"] != null ? objread["IsDone"].ToString() : null;
                    objTask.Status_Ind = objread["Status_Ind"] != null ? objread["Status_Ind"].ToString() : null;
                    if (objread["DisplayInshedule_Ind"] != null)
                    {

                        objTask.ChkStatus_Ind = Convert.ToString(objread["DisplayInshedule_Ind"]);
                    }
                    objTask.Taskfor = objread["Taskfor"] != null ? objread["Taskfor"].ToString() : null;
                    objTask.Practicename = objread["Practicename"] != null ? objread["Practicename"].ToString() : null;
                    objTask.TaskTitle = objread["TaskTitle"] != null ? objread["TaskTitle"].ToString() : null;
                    if (objread["DueDate"].ToString() != "")
                    {
                        string DueDate = Convert.ToString(objread["DueDate"]);
                        objTask.DueDate = DueDate.Remove(DueDate.Length - 12);
                    }
                    else
                    {
                        objTask.DueDate = null;
                    }
                    objTask.LastAction = Convert.ToDateTime(objread["LastAction"].ToString() == "" ? null : objread["LastAction"].ToString());
                    objTask.TaskDate = objread["TaskDate"] != null ? objread["TaskDate"].ToString() : null;
                    objTask.Notes = objread["Notes"] != null ? objread["Notes"].ToString() : null;
                    objTask.Assignedusers = objread["Assignedusers"] != null ? objread["Assignedusers"].ToString() : null;
                    objTask.CreatedBy = objread["createdby"] != null ? objread["createdby"].ToString() : null;
                    objTask.CreatedOn = objread["CreatedOn"] != null ? objread["CreatedOn"].ToString() : null;
                    objTasklist.Add(objTask);
                }
                objread.NextResult();
                if (objread.HasRows == true)
                {
                    if (objread.Read())
                    {
                        Tasks.Totalrecords = (int)objread[0];
                    }

                }


                return objTasklist;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Tasklist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool AddNewTask(Tasks objTask)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparamlist = {
			new SqlParameter("@in_ReferenceType_ID", (objTask.ReferenceType_ID != 0 ? objTask.ReferenceType_ID.ToString() : null)),
			new SqlParameter("@in_Reference_ID", (objTask.Reference_ID != 0 ? objTask.Reference_ID.ToString() : null)),
			new SqlParameter("@In_Practice_id", (objTask.Practice_ID != 0 ? objTask.Practice_ID.ToString() : null)),
			new SqlParameter("@in_TaskTitle", objTask.TaskTitle),
			new SqlParameter("@in_DueDate", objTask.DueDate),
			new SqlParameter("@in_Notes", objTask.Notes),
			new SqlParameter("@In_Isdone", (objTask.IsDone != null ? objTask.IsDone : null)),
			new SqlParameter("@In_Is_PracticeTask", (objTask.IsPracticeTask != null ? objTask.IsPracticeTask : null)),
			new SqlParameter("@In_Assignedusers", objTask.Assignedusers),
			new SqlParameter("@in_CreatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_DisplayInshedule_Ind", objTask.ChkStatus_Ind),
			new SqlParameter("@In_LoginHistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};

                objcommon.AddInParameters(inparamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Admin_Ins_task");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "AddNewTask", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override bool AddNewActionItem(Tasks objAction)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparamlist = {
			new SqlParameter("@In_ActionItem_id", objAction.Actionitem_ID),
			new SqlParameter("@In_task_id", objAction.Task_ID),
			new SqlParameter("@in_ReferenceType_ID", (objAction.ReferenceType_ID != 0 ? objAction.ReferenceType_ID.ToString() : null)),
			new SqlParameter("@in_Reference_ID", (objAction.Reference_ID != 0 ? objAction.Reference_ID.ToString() : null)),
			new SqlParameter("@In_Status_ind", (objAction.Status_Ind != null ? objAction.Status_Ind : null)),
			new SqlParameter("@In_ActionItemtitle", objAction.Actionitemtitle),
			new SqlParameter("@In_ActionitemDescription", objAction.ActiontitleDescription),
			new SqlParameter("@in_CreatedBy", HttpContext.Current.Session["userid"])
		};
                objcommon.AddInParameters(inparamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Task_Ins_taskActionitem");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "AddNewActionItem", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override bool UpdateTask(Tasks objTask)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparamlist = {
            new SqlParameter("@in_Task_ID", (objTask.Task_ID != 0 ? objTask.Task_ID.ToString() : null)),
            new SqlParameter("@in_TaskTitle", objTask.TaskTitle),
            new SqlParameter("@In_Reference_Id", (objTask.Reference_ID != 0 ? objTask.Reference_ID.ToString() : null)),
            new SqlParameter("@In_Referencetype_id", (objTask.ReferenceType_ID != 0 ? objTask.ReferenceType_ID.ToString() : null)),
            new SqlParameter("@in_DueDate", objTask.DueDate),
            new SqlParameter("@in_Notes", objTask.Notes),
            new SqlParameter("@In_Isdone", (objTask.IsDone != null ? objTask.IsDone : null)),
            new SqlParameter("@In_Status_ind", (objTask.Status_Ind != null ? objTask.Status_Ind : null)),
            new SqlParameter("@In_ModifiedByLogin_id", null),
            new SqlParameter("@in_DisplayInshedule_Ind", objTask.ChkStatus_Ind),
            new SqlParameter("@In_LoginHistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
        };
                objcommon.AddInParameters(inparamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Admin_upd_task");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UpdateTask", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override List<Tasks> ActionItemsList(Tasks objTasks)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] paramlist = {
            new SqlParameter("@in_Task_ID", (objTasks.Task_ID != 0 ? objTasks.Task_ID.ToString() : null)),
			new SqlParameter("@In_ActionItem", (objTasks.Actionitemtitle != null ? objTasks.Actionitemtitle : null)),
			new SqlParameter("@In_OrderBYitem", objTasks.sortField),
			new SqlParameter("@In_OrderBy", objTasks.sortDirection)
		};

                objcommon.AddInParameters(paramlist);
                objread = objcommon.GetDataReader("Help_dbo.St_Task_List_ActionItems");
                List<Tasks> objTasklist = new List<Tasks>();
                while (objread.Read())
                {
                    Tasks objTask = new Tasks();
                    if (objread["Task_ID"] != null)
                    {
                        objTask.Task_ID = Convert.ToInt32(objread["Task_ID"]);
                    }
                    else
                    {
                        objTask.Task_ID = 0;
                    }
                    if (objread["Actionitem_id"] != null)
                    {
                        objTask.Actionitem_ID = Convert.ToInt32(objread["Actionitem_id"]);
                    }
                    else
                    {
                        objTask.Actionitem_ID = 0;
                    }
                    if (objread["Actionitemtitle"] != null)
                    {
                        objTask.Actionitemtitle = objread["Actionitemtitle"].ToString();
                    }
                    else
                    {
                        objTask.Actionitemtitle = null;
                    }
                    objTask.LastAction = Convert.ToDateTime(objread["LastAction"].ToString() == "" ? null : objread["LastAction"].ToString());
                    if (objread["ActionBy"] != null)
                    {
                        objTask.ActionBy = objread["ActionBy"].ToString();
                    }
                    else
                    {
                        objTask.ActionBy = null;
                    }
                    objTasklist.Add(objTask);
                }
                return objTasklist;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "ActionItemsList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<Tasks> GetActionItemsList(Tasks objAction)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] paramlist = { new SqlParameter("@In_Actionitem_id", (objAction.Actionitem_ID != 0 ? objAction.Actionitem_ID : 0)) };


                objcommon.AddInParameters(paramlist);
                objread = objcommon.GetDataReader("Help_dbo.St_task_get_ActionItemDetails");
                List<Tasks> objTasklist = new List<Tasks>();
                while (objread.Read())
                {
                    Tasks objTask = new Tasks();

                    if (objread["Task_ID"] != null)
                    {
                        objTask.Task_ID = Convert.ToInt32(objread["Task_ID"]);
                    }
                    else
                    {
                        objTask.Task_ID = 0;
                    }

                    if (objread["Actionitem_id"] != null)
                    {
                        objTask.Actionitem_ID = Convert.ToInt32(objread["Actionitem_id"]);
                    }
                    else
                    {
                        objTask.Actionitem_ID = 0;
                    }
                    if (objread["Actionitemtitle"] != null)
                    {
                        objTask.Actionitemtitle = objread["Actionitemtitle"].ToString();
                    }
                    else
                    {
                        objTask.Actionitemtitle = null;
                    }

                    objTasklist.Add(objTask);
                }
                return objTasklist;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetActionItemsList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet Providerpublicmessages(MowerHelper.Models.BLL.Providers.Provider_Common obj, string Status)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dset = new DataSet();
                IDataParameter[] parameters = {
			new SqlParameter("@In_PRovider_ID", (obj.ProviderID.ToString() == "0" ? null : obj.ProviderID.ToString())),
			new SqlParameter("@In_Fromdate", obj.FromDate),
			new SqlParameter("@In_Todate", obj.ToDate),
			new SqlParameter("@In_Status", (!string.IsNullOrEmpty(Status) ? Status : null)),
			new SqlParameter("@In_NoOfRecords", obj.NoOfRecords),
			new SqlParameter("@In_PageNo", obj.PageNO),
			new SqlParameter("@IN_OrderByItem", obj.OrderBYItem),
			new SqlParameter("@In_OrderBy", obj.OrderBy),
			new SqlParameter("@In_lOgin_id", (obj.Loginid.ToString() == "0" ? null : obj.Loginid.ToString()))
		};
                objcommon.AddInParameters(parameters);
                dset = objcommon.GetDataSet("Help_dbo.st_Provider_List_ProviderMessages");
                return dset;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Providerpublicmessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet GetProviderpublicmessage(int Msg_id)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dset = new DataSet();
                IDataParameter[] parameters = {
			new SqlParameter("@in_Provider_PublicMessages_ID", Msg_id)
		};
                objcommon.AddInParameters(parameters);
                dset = objcommon.GetDataSet("Help_dbo.St_get_provider_publicmessages");
                return dset;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetProviderpublicmessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override Responseparty Get_ResponsibleParty_Info(int Patient_ID, int Practice_ID, int Provider_ID)
        {
            try
            {
                SqlDataReader objread = null;
                clsCommonFunctions objCommon = new clsCommonFunctions();

                IDataParameter[] ObjInParam = {
            new SqlParameter("@in_Patient_ID", (Patient_ID.ToString() == "0" ? null : Patient_ID.ToString())),
            new SqlParameter("@in_Practice_ID", (Practice_ID.ToString() == "0" ? null : Practice_ID.ToString())),
            new SqlParameter("@in_Provider_ID", (Provider_ID.ToString() == "0" ? null : Provider_ID.ToString()))
        };
                objCommon.AddInParameters(ObjInParam);
                objread = objCommon.GetDataReader("Help_dbo.st_Patient_Edit_GET_PersonalInfo");


                if (objread.Read() == true)
                {
                    Responseparty objRPInfo = new Responseparty();
                    objRPInfo.Patient_ID = Convert.ToInt32(objread["Patient_ID"].ToString() != null ? objread["Patient_ID"].ToString() : "-1");
                    objRPInfo.Login_ID = Convert.ToInt32(objread["Login_ID"].ToString() != "" ? objread["Login_ID"] : -1);
                    objRPInfo.ProviderPatient_ID = Convert.ToInt32(objread["ProviderPatient_ID"].ToString() != "" ? objread["ProviderPatient_ID"] : -1);
                    objRPInfo.AuthorizedProvider_ID = Convert.ToInt32(objread["AuthorizedProvider_ID"].ToString() != "" ? objread["AuthorizedProvider_ID"].ToString() : "-1");
                    objRPInfo.ActualProviderLogin_ID = Convert.ToInt32(objread["ActualProviderLogin_ID"].ToString() != "" ? objread["ActualProviderLogin_ID"] : -1);
                    objRPInfo.ProviderName = (objread["ProviderName"] != null ? objread["ProviderName"].ToString() : null);
                    objRPInfo.PatientName = (objread["PatientName"] != null ? objread["PatientName"].ToString() : null);
                    // objRPInfo.Practice_ID = Convert.ToInt32(objread["Practice_ID"].ToString() != "" ? objread["Practice_ID"] : -1);
                    //  objRPInfo.PlaceOfService_ID = Convert.ToInt32(objread["PlaceOfService_ID"].ToString() != "" ? objread["PlaceOfService_ID"] : -1);
                    //  objRPInfo.PlaceOfService = (objread["PlaceOfService"] != null ? objread["PlaceOfService"].ToString() : null);
                    objRPInfo.Prefix = (objread["Prefix"].ToString() != null ? objread["Prefix"].ToString() : null);
                    objRPInfo.FirstName = (objread["FirstName"] != null ? objread["FirstName"].ToString() : null);
                    objRPInfo.LastName = (objread["LastName"] != null ? objread["LastName"].ToString() : null);
                    objRPInfo.MI = (objread["Mi"] != null ? objread["Mi"].ToString() : null);
                    objRPInfo.Suffix = (objread["Suffix"] != null ? objread["Suffix"].ToString() : null);
                    objRPInfo.Email = (objread["PatientEmail"] != null ? objread["PatientEmail"].ToString() : null);
                    objRPInfo.HomePhone = (objread["HomePhone"] != null ? objread["HomePhone"].ToString() : null);
                    objRPInfo.WPhone = (objread["WorkPhone"] != null ? objread["WorkPhone"].ToString() : null);
                    objRPInfo.MPhone = (objread["CellPhone"] != null ? objread["CellPhone"].ToString() : null);
                    objRPInfo.Address1 = (objread["Address1"] != null ? objread["Address1"].ToString() : null);
                    objRPInfo.Address2 = (objread["Address2"] != null ? objread["Address2"].ToString() : null);
                    objRPInfo.City_ID = Convert.ToInt32(objread["City_ID"].ToString() != "" ? objread["City_ID"].ToString() : "0");
                    objRPInfo.City = (objread["DCity_Name"] != null ? objread["DCity_Name"].ToString() : null);
                    objRPInfo.State_ID = Convert.ToInt32(objread["State_ID"].ToString() != "" ? objread["State_ID"].ToString() : "0");
                    objRPInfo.State = (objread["DState_Name"] != null ? objread["DState_Name"].ToString() : null);
                    objRPInfo.Country_ID = (objread["Country_ID"] != null ? objread["Country_ID"].ToString() : "0");
                    objRPInfo.Zip = (objread["Zip"] != null ? objread["Zip"].ToString() : null);
                    objRPInfo.DrivingLicenceNo = (objread["DrivingLicenceNo"] != null ? objread["DrivingLicenceNo"].ToString() : null);
                    objRPInfo.Patient_Status_ID = Convert.ToInt32(objread["Patient_Status_ID"].ToString() != "" ? objread["Patient_Status_ID"] : -1);

                    return objRPInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Get_ResponsibleParty_Info", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override void Patient_Archive_Reject_Activate(int? Patient_ID, int Patient_Status_ID, int UpdatedBy, int Provider_ID)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();

                IDataParameter[] InObjParam = {
			new SqlParameter("@in_Patient_ID", Patient_ID),
			new SqlParameter("@in_Patient_Status_ID", Patient_Status_ID),
			new SqlParameter("@in_UpdatedBy", (UpdatedBy.ToString() == "0" ? null : UpdatedBy.ToString())),
			new SqlParameter("@in_Practice_Id", null),
			new SqlParameter("@in_Provider_ID", (Provider_ID.ToString() == "0" ? null : Provider_ID.ToString()))
		};
                objCommon.AddInParameters(InObjParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Patient_ArchiveOrRejectOrActivatePatient");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Patient_Archive_Reject_Activate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            //return null;
        }
        public override DataSet GetMs_GetCategoriesDS(int LoginID, int RoleID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@in_Login_ID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                AddParamToSQLCmd(sqlCmd, "@in_Role_ID", SqlDbType.Int, 0, ParameterDirection.Input, RoleID);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, "Help_dbo.st_MessageStation_GetMsgCategoriesWithCount");
                DataSet stsCollection = (DataSet)ExecuteDatasetCmd(sqlCmd);
                return stsCollection;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetMs_GetCategoriesDS", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool MessageStation_INS_ArchivedMessages(int _id, int Reference_Id)
        {
            try
            {
                IDataParameter[] objParm = {
			new SqlParameter("@in_Patient_ID", _id),
			new SqlParameter("@in_Reference_ID", Reference_Id),
			new SqlParameter("@in_MessageCategory_ID", HttpContext.Current.Session["int_Category_ID"]),
			new SqlParameter("@in_ArchivedBy", HttpContext.Current.Session["userID"]),
			new SqlParameter("@in_Role_ID", HttpContext.Current.Session["roleid"])
		};


                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                objclsDBWrapper.AddInParameters(objParm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_MessageStation_INS_ArchivedMessages");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "MessageStation_INS_ArchivedMessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public clsCountry GetProviderStateID(string StateNm)
        {
            SqlDataReader dr = default(SqlDataReader);
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] InObjParam = {
			new SqlParameter("@In_flg", "S"),
			new SqlParameter("@In_State_name", (StateNm == null ? null : StateNm))
		};
                objCommon.AddInParameters(InObjParam);
                dr = objCommon.GetDataReader("Help_dbo.st_GetDirectoryID_StateID_Provider_ID");
                if (dr.Read())
                {
                    return new clsCountry { StateId = (int)dr["State_ID"], StateName = (string)dr["Abbrevation"], StateFullName = (string)dr["State_name"] };
                }
            }
            catch (SqlException ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetProviderStateID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public string GetProviderStateab(string StateNm)
        {
            string stateab = null;
            SqlDataReader dr = default(SqlDataReader);
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] InObjParam = {
			new SqlParameter("@In_flg", "S"),
			new SqlParameter("@In_State_name", (StateNm == null ? null : StateNm))
		};
                objCommon.AddInParameters(InObjParam);
                dr = objCommon.GetDataReader("Help_dbo.st_GetDirectoryID_StateID_Provider_ID");
                if (dr.Read())
                {
                    if (dr["Abbrevation"] != null)
                    {
                        stateab = (string)dr["Abbrevation"];
                    }
                    else
                    {
                        stateab = null;
                    }
                }
            }
            catch (SqlException ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetProviderStateab", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return stateab;

        }
        public object GetProviderCityID(string cityname, int? stateid)
        {
            int CityID = 0;
            SqlDataReader dr = default(SqlDataReader);
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] InObjParam = {
			new SqlParameter("@In_flg", "C"),
			new SqlParameter("@In_City_name", (cityname == null ? null : cityname)),
                new SqlParameter("@In_State_ID", (stateid == null ? null : stateid))
		};
                objCommon.AddInParameters(InObjParam);
                dr = objCommon.GetDataReader("Help_dbo.st_GetDirectoryID_StateID_Provider_ID");
                if (dr.Read())
                {
                    if (dr["City_ID"] != null)
                    {
                        CityID = (int)dr["City_ID"];
                    }
                    else
                    {
                        CityID = 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetProviderCityID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return CityID;

        }

        public string UpdArticlesStatistics(int Article_ID, int SiteStatistic_ID, string statename, string cityname)
        {
            try
            {
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Article_ID", Article_ID),
			new SqlParameter("@In_SiteStatistic_ID", SiteStatistic_ID),
			new SqlParameter("@In_State_Name", statename),
			new SqlParameter("@In_City_Name", cityname)
		};

                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParamList);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_Public_INS_ArticleStatistic").ToString();
            }
            catch (SqlException ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UpdArticlesStatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }


        public List<ProviderWebSites> GetProviderDocumentsForDifferentWebSites(int? ProviderID, int? NoofRecords, int? PageNo)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Provider_ID", (ProviderID == 0 ? null : ProviderID)),
			new SqlParameter("@in_PageNo", (PageNo == 0 ? null : PageNo)),
			new SqlParameter("@in_NoofRecords", (NoofRecords == 0 ? null : NoofRecords)),
			new SqlParameter("@in_Displayinpublic", "Y"),
			new SqlParameter("@in_verifieddocuments", "N")
		};
                DataSet ds = new DataSet();
                List<ProviderWebSites> objDataList = new List<ProviderWebSites>();
                objcommon.AddInParameters(InParamList);
                ds = objcommon.GetDataSet("Help_dbo.st_Provider_GET_Provider_DocumentInfo_rdPaging");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProviderWebSites objData = new ProviderWebSites();
                            if (dr["Title"] != null)
                            {
                                objData.Title = (string)dr["Title"];
                            }
                            else
                            {
                                objData.Title = null;
                            }
                            if (dr["Path"] != null)
                            {
                                objData.File_Path = (string)dr["Path"];
                            }
                            else
                            {
                                objData.File_Path = null;
                            }
                            if (dr["ImagePathSuffix"] != null)
                            {
                                objData.ImagePathSuffix = (string)dr["ImagePathSuffix"];
                            }
                            else
                            {
                                objData.ImagePathSuffix = null;
                            }
                            if (dr["DocDescription"] != DBNull.Value)
                            {
                                if (dr["DocDescription"] != null)
                                {
                                    objData.DocDescription = (string)dr["DocDescription"];
                                }
                                else
                                {
                                    objData.DocDescription = null;
                                }
                            }
                            else
                            {
                                objData.DocDescription = null;
                            }
                            if (dr["CreatedOn"] != null)
                            {
                                objData.CreatedOn = (string)dr["CreatedOn"];
                            }
                            else
                            {
                                objData.CreatedOn = null;
                            }
                            if (dr["ProviderDocument_ID"] != null)
                            {
                                objData.Document_ID = (int)dr["ProviderDocument_ID"];
                            }
                            else
                            {
                                objData.Document_ID = 0;
                            }
                            if (dr["FileName"] != null)
                            {
                                objData.FileName = (string)dr["FileName"];
                            }
                            else
                            {
                                objData.FileName = null;
                            }
                            objDataList.Add(objData);
                            objData = null;
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ProviderWebSites.TotalNoRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetProviderDocumentsForDifferentWebSites", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public void GetNearestZIPCodesandNearestCities(string ZIPCode, ref List<clsNearestCities> objCitiesList, ref List<clsNearestZIPCodes> objZIPCodesList)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet ds = new DataSet();
                IDataParameter[] objInParam = { new SqlParameter("@In_ZipCode", (ZIPCode == null ? null : ZIPCode)) };
                objcommon.AddInParameters(objInParam);
                ds = objcommon.GetDataSet("Help_dbo.st_Provider_GET_ProviderNearbyArea");
                if (ds.Tables.Count == 2)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            clsNearestCities objData = new clsNearestCities();
                            if (dr["City"] != null)
                            {
                                objData.CityName = dr["City"].ToString();
                            }
                            if (dr["State_name"] != null)
                            {
                                objData.StateName = dr["State_name"].ToString();
                            }
                            if (dr["CITY_ID"] != null)
                            {
                                objData.CITY_ID = dr["CITY_ID"].ToString();
                            }
                            objData.count = ds.Tables[0].Rows.Count;
                            objData.ind = "Citylist";
                            objCitiesList.Add(objData);
                            objData = null;
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            clsNearestZIPCodes objData = new clsNearestZIPCodes();
                            if (dr["l_zipcode"] != null)
                            {
                                objData.ZIPCode = dr["l_zipcode"].ToString();
                            }
                            objZIPCodesList.Add(objData);
                            objData = null;
                        }
                    }
                }
                if (ds.Tables.Count == 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            clsNearestCities objData = new clsNearestCities();
                            if (dr["City"] != null)
                            {
                                objData.CityName = dr["City"].ToString();
                            }
                            if (dr["state"] != null)
                            {
                                objData.CityName = dr["City"].ToString();
                            }
                            objCitiesList.Add(objData);
                            objData = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetNearestZIPCodesandNearestCities", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public string UpdAudioVideoStatistics(int userReference_ID, int ReferenceTypeId, int ReferenceID, int SiteStatic_ID, string State_Name = null, string CityNm = null)
        {
            try
            {
                IDataParameter[] InParamList = {
			new SqlParameter("@In_userReference_ID", userReference_ID),
			new SqlParameter("@In_ReferenceType_ID", ReferenceTypeId),
			new SqlParameter("@In_Reference_ID", ReferenceID),
			new SqlParameter("@In_SiteStatic_ID", SiteStatic_ID),
			new SqlParameter("@In_State_Name", State_Name),
			new SqlParameter("@In_City_Name", CityNm)
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParamList);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_user_UPD_OtherStatistic").ToString();
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "UpdAudioVideoStatistics", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public string CountingProviderVisitToHisProfileDocument(int? Provider_ID, int Sitestatistics_ID, int Reference_ID, string State_Name = null, string CityNm = null)
        {
            try
            {
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Provider_ID", Provider_ID),
			new SqlParameter("@In_SiteStatic_ID", Sitestatistics_ID),
			new SqlParameter("@In_StateName", (!string.IsNullOrEmpty(State_Name) ? State_Name : null)),
			new SqlParameter("@In_CityName", (!string.IsNullOrEmpty(CityNm) ? CityNm : null)),
			new SqlParameter("@In_Reference_ID", Reference_ID)
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParamList);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_ListingStatistic").ToString();
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "CountingProviderVisitToHisProfileDocument", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public string GetproviderWebsiteCount(int? ProviderID, string State_Name = null, string CityNm = null)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamList = {
		new SqlParameter("@In_userReference_ID", (ProviderID == 0 ? null : ProviderID)),
		new SqlParameter("@In_ReferenceType_ID", 2),
		new SqlParameter("@In_Reference_ID", null),
		new SqlParameter("@In_SiteStatic_ID", 7),
		new SqlParameter("@In_State_Name", State_Name),
		new SqlParameter("@In_City_Name", CityNm)
	};

                objcommon.AddInParameters(objInparamList);
                objcommon.GetDataSet("Help_dbo.st_user_UPD_OtherStatistic");
                return null;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetproviderWebsiteCount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string Insert_Patinet_FT(PatientRegistration objIns, ref string Out_Msg, ref string Out_login_id)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();

                IDataParameter[] objInparameters = {
			new SqlParameter("@in_Provider_ID", (objIns.Provider_ID == 0 ? null : objIns.Provider_ID)),
            new SqlParameter("@in_Title", objIns.Prefix),
			new SqlParameter("@in_FirstName", objIns.FirstName),
			new SqlParameter("@in_MI", objIns.MI),
			new SqlParameter("@in_LastName", objIns.LastName),
			new SqlParameter("@in_Suffix", objIns.Suffix),
			new SqlParameter("@in_PatientEmail", objIns.PatientEmail),
			new SqlParameter("@in_DrivingLicenceNO", objIns.DrivingLicenceNo),
			new SqlParameter("@in_HomePhone", objIns.HomePhone),
            new SqlParameter("@in_WorkPhone", objIns.WPhone),
            new SqlParameter("@in_CellPhone", objIns.MPhone),
			new SqlParameter("@in_Address1", objIns.Address1),
			new SqlParameter("@in_Address2", objIns.Address2),
			new SqlParameter("@in_City_ID", (objIns.City_ID == 0 ? null : objIns.City_ID)),
			new SqlParameter("@in_State_ID", (objIns.State_ID == 0 ? null : objIns.State_ID)),
			new SqlParameter("@in_Country_ID", (objIns.Country_ID == 0 ? null : objIns.Country_ID)),
			new SqlParameter("@in_ZIP", objIns.ZIP),
           new SqlParameter("@in_CreatedBy", (objIns.UserId == 0 ? null : objIns.UserId)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
            new SqlParameter("@In_Latitude",objIns.GmapLatitude),
            new SqlParameter("@In_Longitude",objIns.GmapLongitude),
            new SqlParameter("@In_Ind",objIns.Ind)
		};
                IDataParameter[] objOutparameters = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100, null),
			new SqlParameter("@Out_login_id", SqlDbType.VarChar, 100,null)
		};
                objCommon.AddInParameters(objInparameters);
                objCommon.AddOutParameters(objOutparameters);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Patient_DML_FTRegPatientPersonalInfo");

                if (objCommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    Out_Msg = objCommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                else
                {
                    Out_Msg = null;
                }
                if (objCommon.objdbCommandWrapper.Parameters["@Out_login_id"].Value != null)
                {
                    Out_login_id = objCommon.objdbCommandWrapper.Parameters["@Out_login_id"].Value.ToString();
                }
                else
                {
                    Out_login_id = null;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_Patinet_FT", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public List<ExpenseLedgerList> GetExpLedgerDataList(int? Provider_ID, int? NoofRecords, int? PageNo, string OrderBy, string OrderByItem, string strStartDate, string strEndDate, string strCategory)
        {
            try
            {

                DataSet ds = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<ExpenseLedgerList> objExpLedgerList = new List<ExpenseLedgerList>();
                int SLNo = 0;
                IDataParameter[] InParm = {
			new SqlParameter("@in_Practice_ID", null),
			new SqlParameter("@in_PlaceOfService_ID", null),
			new SqlParameter("@in_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
			new SqlParameter("@in_NoofRecords", (NoofRecords == 0 ? null : NoofRecords)),
			new SqlParameter("@in_PageNo", (PageNo == 0 ? null : PageNo)),
			new SqlParameter("@In_OrderBy", (OrderBy == null ? null : OrderBy)),
			new SqlParameter("@In_OrderByitem", (OrderByItem == null ? null : OrderByItem)),
			new SqlParameter("@in_Startdate", (strStartDate == null ? null : strStartDate)),
			new SqlParameter("@in_Enddate", (strEndDate == null ? null : strEndDate)),
            new SqlParameter("@in_createdby", HttpContext.Current.Session["UserID"]),
			new SqlParameter("@In_Category", (strCategory == null ? null : strCategory))
		};
                objcommon.AddInParameters(InParm);
                SLNo = Convert.ToInt32(NoofRecords * (PageNo - 1) + 1);
                ds = objcommon.GetDataSet("Help_dbo.st_PlaceOfService_GET_ExpensesLedger_RDPaging");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ExpenseLedgerList objExpList = new ExpenseLedgerList(SLNo, (dr["Practice_ID"] != DBNull.Value ? Convert.ToInt32(dr["Practice_ID"]) : 0), Convert.ToInt32(dr["Exp_Ledger_ID"] != null ? dr["Exp_Ledger_ID"] : null), Convert.ToInt32(dr["Provider_ID"].ToString() != "" ? dr["Provider_ID"] : null), Convert.ToDateTime(dr["Exp_Date"] != null ? dr["Exp_Date"] : null), Convert.ToString(dr["Item"] != null ? dr["Item"] : null), Convert.ToString(dr["Quantity"] != null ? dr["Quantity"].ToString() : null), Convert.ToString(dr["TotalCost"] != null ? dr["TotalCost"] : null), Convert.ToString(dr["CheckNumber"] != null ? dr["CheckNumber"].ToString() : null), Convert.ToString(dr["Vendor"] != null ? dr["Vendor"] : null),
                            Convert.ToString(dr["Deductible"] != null ? dr["Deductible"] : null), Convert.ToString(dr["Category"] != null ? dr["Category"] : null), Convert.ToString(dr["Notes"] != null ? dr["Notes"] : null), Convert.ToString(dr["Image_file"] != null ? dr["Image_file"] : null));
                            objExpLedgerList.Add(objExpList);
                            SLNo = SLNo + 1;
                        }
                        ExpenseLedgerList.NoofRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    }
                    else
                    {
                        ExpenseLedgerList.NoofRecords = 0;
                    }
                }
                else
                {
                    ExpenseLedgerList.NoofRecords = 0;
                }

                return objExpLedgerList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetExpLedgerDataList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ExpenseLedgerList> GetAllCatagories()
        {
            try
            {
                DataSet ds = new DataSet();
                List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                ds = objcommon.GetDataSet("Help_dbo.st_PlaceOfService_LIST_ExpensesCategories");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ExpenseLedgerList obj = new ExpenseLedgerList(1, 1, Convert.ToInt32(dr["Exp_Category_ID"] != null ? dr["Exp_Category_ID"] : null), Convert.ToString(dr["Exp_Category"] != null ? dr["Exp_Category"] : null));
                            objList.Add(obj);
                        }
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetAllCatagories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public object InsertExpense_Ledger(int? Reference_ID, string Exp_Category, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_Reference_ID", (Reference_ID == 0 ? null : Reference_ID)),
			new SqlParameter("@In_Exp_Category", (Exp_Category == null ? null : Exp_Category))
		};
                IDataParameter[] outparam = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 500) };
                var _with1 = objcommon;
                _with1.AddInParameters(InParamList);
                _with1.AddOutParameters(outparam);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Practice_INS_ExpenceCategory");
                if ((objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value) != null)
                {
                    Out_Msg = Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value);
                }
                return Out_Msg;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "InsertExpense_Ledger", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public PatientRegistration GET_RegPatientPersonalInformation(int Patient_ID, int Practice_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparameters = {
			new SqlParameter("@in_Patient_ID", Patient_ID),
			new SqlParameter("@Int_Practice_ID", Practice_ID)
		};

                objcommon.AddInParameters(objInparameters);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Patient_GET_RegPatientPersonalInfo");

                if (objread.Read())
                {
                    PatientRegistration objPatInfo = new PatientRegistration();
                    objPatInfo.Prefix = (objread["Prefix"] != DBNull.Value ? (string)objread["Prefix"] : null);
                    objPatInfo.FirstName = ((string)objread["FirstName"] != null ? (string)objread["FirstName"] : null);
                    objPatInfo.MI = (objread["MI"] != DBNull.Value ? (string)objread["MI"] : null);
                    objPatInfo.LastName = ((string)objread["LastName"] != null ? (string)objread["LastName"] : null);
                    objPatInfo.Suffix = (objread["Suffix"] != DBNull.Value ? (string)objread["Suffix"] : null);
                    objPatInfo.Address1 = (objread["Address1"] != DBNull.Value ? (string)objread["Address1"] : null);
                    objPatInfo.Address2 = (objread["Address2"] != DBNull.Value ? (string)objread["Address2"] : null);
                    objPatInfo.City_ID = ((int)objread["City_ID"] != 0 ? (int)objread["City_ID"] : 0);
                    objPatInfo.City = ((string)objread["City"] != null ? (string)objread["City"] : null);
                    objPatInfo.State_ID = ((int)objread["State_ID"] != 0 ? (int)objread["State_ID"] : 0);
                    objPatInfo.State = ((string)objread["State"] != null ? (string)objread["State"] : null);
                    objPatInfo.ZIP = ((string)objread["ZIP"] != null ? (string)objread["ZIP"] : null);
                    return objPatInfo;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GET_RegPatientPersonalInformation", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;

        }
        public override PatientRegistration Get_Random_UserCredentials()
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();

                IDataParameter[] ObjInParam = { new SqlParameter("@in_Digits", 6) };

                IDataParameter[] ObjOutParams = {
			new SqlParameter("@out_username", SqlDbType.VarChar, 100),
			new SqlParameter("@Out_pwrd", SqlDbType.VarChar, 100)
		};
                objCommon.AddInParameters(ObjInParam);
                objCommon.AddOutParameters(ObjOutParams);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Practice_Generate_UserCredentials");

                PatientRegistration obj = new PatientRegistration();

                if (objCommon.objdbCommandWrapper.Parameters["@out_username"].Value != null)
                {
                    obj.Username = objCommon.objdbCommandWrapper.Parameters["@out_username"].Value.ToString();
                }

                if (objCommon.objdbCommandWrapper.Parameters["@Out_pwrd"].Value != null)
                {
                    obj.Password = objCommon.objdbCommandWrapper.Parameters["@Out_pwrd"].Value.ToString();
                }

                return obj;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Get_Random_UserCredentials", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override string Insert_Security_INS_FTUser(PatientRegistration objUser)
        {
            clsCommonFunctions objCommon = new clsCommonFunctions();
            try
            {
                IDataParameter[] objInparameters = {
			new SqlParameter("@in_USERNAME", objUser.Username),
			new SqlParameter("@In_Reference_ID", (objUser.Reference_ID == 0 ? null : objUser.Reference_ID)),
			new SqlParameter("@In_ReferenceType_ID", objUser.ReferenceType_ID),
			new SqlParameter("@In_Practice_ID", (objUser.Practice_ID == 0 ? null : objUser.Practice_ID)),
			new SqlParameter("@in_PASSWORD", objUser.Password),
			new SqlParameter("@In_Role_ID", objUser.Role_ID),
			new SqlParameter("@In_RoleName", objUser.RoleName),
			new SqlParameter("@in_CreatedBy", HttpContext.Current.Session["userID"]),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};

                IDataParameter[] objOutparameters = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100, null) };
                objCommon.AddInParameters(objInparameters);
                objCommon.AddOutParameters(objOutparameters);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Security_INS_FTUser");

                if (objCommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    return objCommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_Security_INS_FTUser", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public int Insert_Expense_Ledger(int? Provider_ID, int? Exp_Category_ID, string Exp_Date, string Item, string Quantity, string TotalCost, string CheckNumber, string Vendor,
        string Deductible, string Notes, int? CreatedBy, int? RoleID, string Imagepath)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strParamList = {
			new SqlParameter("@in_Practice_ID", null),
			new SqlParameter("@in_PlaceOfService_ID", null),
			new SqlParameter("@in_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
			new SqlParameter("@in_Exp_Category_ID", (Exp_Category_ID == 0 ? null : Exp_Category_ID)),
			new SqlParameter("@in_Exp_Date", (Exp_Date == null ? null : Exp_Date)),
			new SqlParameter("@in_Item", (Item == null ? null : Item)),
			new SqlParameter("@in_Quantity", (string.IsNullOrEmpty(Quantity) ? null : Quantity)),
			new SqlParameter("@in_TotalCost", Convert.ToDecimal((TotalCost == null ? null : TotalCost))),
			new SqlParameter("@in_CheckNumber", (string.IsNullOrEmpty(CheckNumber) ? null : CheckNumber)),
			new SqlParameter("@in_Vendor", (Vendor == null ? null : Vendor)),
			new SqlParameter("@in_Deductible", (Deductible == null ? null : Deductible)),
			new SqlParameter("@in_Notes", (Notes == null ? null : Notes)),
			new SqlParameter("@in_CreatedBy", (CreatedBy == 0 ? null : CreatedBy)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
            new SqlParameter("@in_Image_file", Imagepath)
		};
                objcommon.AddInParameters(strParamList);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_PlaceOfService_INS_ExpensesLedger");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Insert_Expense_Ledger", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public int UPT_ExpenseLedger(int? Exp_Ledger_ID, int? Provider_ID, int? PlaceOfService_ID, int? Exp_Category_ID, string Exp_Date, string Item, string Quantity, string TotalCost, string CheckNumber, string Vendor,
        string Deductible, string Notes, string UpdatedBy, int? RoleID, string Imagepath)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_Exp_Ledger_ID", (Exp_Ledger_ID == 0 ? null : Exp_Ledger_ID)),
			new SqlParameter("@in_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
			new SqlParameter("@in_PlaceOfService_ID", (PlaceOfService_ID == 0 ? null : PlaceOfService_ID)),
			new SqlParameter("@in_Exp_Category_ID", (Exp_Category_ID == 0 ? null : Exp_Category_ID)),
			new SqlParameter("@in_Exp_Date", (Exp_Date == null ? null : Exp_Date)),
			new SqlParameter("@in_Item", (Item == null ? null : Item)),
			new SqlParameter("@in_Quantity", (string.IsNullOrEmpty(Quantity) ? null : Quantity)),
			new SqlParameter("@in_TotalCost", Convert.ToDecimal((TotalCost == null ? null : TotalCost))),
			new SqlParameter("@in_CheckNumber", (CheckNumber == "0" ? null : CheckNumber)),
			new SqlParameter("@in_Vendor", (Vendor == null ? null : Vendor)),
			new SqlParameter("@in_Deductible", (Deductible == null ? null : Deductible)),
			new SqlParameter("@in_Notes", (Notes == null ? null : Notes)),
			new SqlParameter("@in_UpdatedBy", (UpdatedBy == "0" ? null : UpdatedBy)),
			new SqlParameter("@in_Practice_ID", null),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
            new SqlParameter("@in_Image_file", Imagepath)
            
		};
                objcommon.AddInParameters(paramlist);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_PlaceOfService_UPD_ExpensesLedger");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UPT_ExpenseLedger", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public List<ExpenseLedgerList> GetExpenseLedgerDetailsBasedOnID(int? Exp_Ledger_ID)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                DataSet ds = new DataSet();
                IDataParameter[] Inparam = { new SqlParameter("@in_Exp_Ledger_ID", (Exp_Ledger_ID == 0 ? null : Exp_Ledger_ID)) };
                objCommon.AddInParameters(Inparam);
                List<ExpenseLedgerList> OBJExpenses = new List<ExpenseLedgerList>();
                ds = objCommon.GetDataSet("Help_dbo.st_PlaceOfService_GET_ExpensesLedgerDetails");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ExpenseLedgerList objData = new ExpenseLedgerList(Convert.ToInt32(dr["Provider_ID"] != null ? dr["Provider_ID"] : null), Convert.ToString(dr["ProviderName"] != null ? dr["ProviderName"] : null), Convert.ToInt32(dr["Exp_Ledger_ID"] != null ? dr["Exp_Ledger_ID"] : null), Convert.ToInt32(dr["Exp_Category_ID"] != null ? dr["Exp_Category_ID"] : null), Convert.ToString(dr["Category"] != null ? dr["Category"] : null), Convert.ToString(dr["Exp_Date"] != null ? dr["Exp_Date"] : null),
                            Convert.ToString(dr["Item"] != null ? dr["Item"] : null), Convert.ToString(dr["Quantity"] != null ? dr["Quantity"] : null), Convert.ToString(dr["TotalCost"] != null ? dr["TotalCost"] : null), Convert.ToString(dr["CheckNumber"] != null ? dr["CheckNumber"] : null), Convert.ToString(dr["Vendor"] != null ? dr["Vendor"] : null), Convert.ToString(dr["Deductible"] != null ? dr["Deductible"] : null), Convert.ToString(dr["Notes"] != null ? dr["Notes"] : null), Convert.ToString(dr["Image_file"] != null ? dr["Image_file"] : null));
                            OBJExpenses.Add(objData);
                        }
                    }
                }

                return OBJExpenses;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetExpenseLedgerDetailsBasedOnID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public int DeleteExpense_Ledger(int? Exp_Ledger_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParam = {
			new SqlParameter("@in_Exp_Ledger_ID", (Exp_Ledger_ID == 0 ? null : Exp_Ledger_ID)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                objcommon.AddInParameters(InParam);
                return objcommon.ExecuteNonQuerySP("Help_dbo.st_ExpenseLedger_Del_Expenses");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "DeleteExpense_Ledger", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public override System.Collections.Generic.List<NotesInfo> GetCustomerNotesInfo(NotesInfo objNotesInfo)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			
			new SqlParameter("@In_NoOfRecords", objNotesInfo.NoOfrecords),
			new SqlParameter("@In_PageNo", objNotesInfo.PageNO),
			new SqlParameter("@In_OrderBy", objNotesInfo.OrderBy),
			new SqlParameter("@In_OrderBYitem", objNotesInfo.OrderByItem),
			new SqlParameter("@In_Practice_ID", (objNotesInfo.PracticeID == 0 ? null : objNotesInfo.PracticeID)),
			new SqlParameter("@In_Fromreference_ID", (objNotesInfo.FromReference_ID == 0 ? null : objNotesInfo.FromReference_ID)),
			new SqlParameter("@In_ToReference_ID", (objNotesInfo.ToReference_ID == 0 ? null : objNotesInfo.ToReference_ID)),
            new SqlParameter("@In_ispatientNote_ind", objNotesInfo.IsPatientNote_Ind),
			new SqlParameter("@In_NotesFromdate", (objNotesInfo.from_date!=null? objNotesInfo.from_date :null )),
			new SqlParameter("@In_NotesTodate", (objNotesInfo.to_date != null? objNotesInfo.to_date : null)),
			new SqlParameter("@In_NotesKeyword", (objNotesInfo.notes_keyword != null? objNotesInfo.notes_keyword :null ))
		
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Notes_list_Generalnotes_RDPAGING");
                List<NotesInfo> NotesInfo = new List<NotesInfo>();
                int SINo = objNotesInfo.NoOfrecords * (objNotesInfo.PageNO - 1);
                while (objread.Read())
                {
                    SINo += 1;
                    NotesInfo objNoteInfo = new NotesInfo();
                    objNoteInfo.SINo = SINo;
                    objNoteInfo.GeneralNote_ID = Convert.ToInt32(objread["GeneralNote_ID"] != null ? objread["GeneralNote_ID"] : null);
                    objNoteInfo.Notes = !string.IsNullOrEmpty(objread["Notes"].ToString()) ? Convert.ToString(objread["Notes"]) : null;
                    objNoteInfo.Notes_Date = Convert.ToDateTime(objread["Notes_Date"].ToString() != "" ? objread["Notes_Date"] : "01 / 01 / 0001");
                    objNoteInfo.IsPatientNote_Ind = Convert.ToString(objread["IsPatientNote_Ind"] != null ? objread["IsPatientNote_Ind"] : null);
                    if (objNoteInfo.IsPatientNote_Ind != "Y")
                    {
                        objNoteInfo.PatientName = "Non-Customer";
                    }
                    else
                    {

                        objNoteInfo.PatientName = Convert.ToString(objread["PatientName"] != null ? objread["PatientName"] : null);
                    }
                    objNoteInfo.notetype = Convert.ToString(objread["notetype"] != null ? objread["notetype"] : null);
                    objNoteInfo.status_ind = Convert.ToString(objread["status_ind"] != null ? objread["status_ind"] : null);
                    objNoteInfo.CreatedBy = Convert.ToString(objread["CreatedBy"] != null ? objread["CreatedBy"] : null);
                    objNoteInfo.Transaction_id = Convert.ToString(objread["Transaction_id"] != null ? objread["Transaction_id"] : null);
                    NotesInfo.Add(objNoteInfo);
                }
                objread.NextResult();
                while (objread.Read())
                {
                    objNotesInfo.TotalRecords = Convert.ToInt32(objread["TotalRecords"]);
                }
                return NotesInfo;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetNotesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<NotesInfo> GetNotesInfo(NotesInfo objNotesInfo)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			
			new SqlParameter("@In_NoOfRecords", objNotesInfo.NoOfrecords),
			new SqlParameter("@In_PageNo", objNotesInfo.PageNO),
			new SqlParameter("@In_OrderBy", objNotesInfo.OrderBy),
			new SqlParameter("@In_OrderBYitem", objNotesInfo.OrderByItem),
			new SqlParameter("@In_Practice_ID", (objNotesInfo.PracticeID == 0 ? null : objNotesInfo.PracticeID)),
			new SqlParameter("@In_Fromreference_ID", (objNotesInfo.FromReference_ID == 0 ? null : objNotesInfo.FromReference_ID)),
			new SqlParameter("@In_ToReference_ID", (objNotesInfo.ToReference_ID == 0 ? null : objNotesInfo.ToReference_ID)),
            new SqlParameter("@In_ispatientNote_ind", objNotesInfo.IsPatientNote_Ind),
			new SqlParameter("@In_NotesFromdate", (objNotesInfo.from_date!=null? objNotesInfo.from_date :null )),
			new SqlParameter("@In_NotesTodate", (objNotesInfo.to_date != null? objNotesInfo.to_date : null)),
			new SqlParameter("@In_NotesKeyword", (objNotesInfo.notes_keyword != null? objNotesInfo.notes_keyword :null )),
            new SqlParameter("@In_status_ind",(objNotesInfo.status_ind!=null?objNotesInfo.status_ind:null))				
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Notes_list_Generalnotes_RDPAGING_VER2");
                List<NotesInfo> NotesInfo = new List<NotesInfo>();
                int SINo = objNotesInfo.NoOfrecords * (objNotesInfo.PageNO - 1);
                while (objread.Read())
                {
                    SINo += 1;
                    NotesInfo objNoteInfo = new NotesInfo();
                    objNoteInfo.SINo = Convert.ToInt32(SINo);
                    objNoteInfo.GeneralNote_ID = Convert.ToInt32(objread["GeneralNote_ID"] != null ? objread["GeneralNote_ID"] : null);
                    objNoteInfo.Notes = !string.IsNullOrEmpty(objread["Notes"].ToString()) ? Convert.ToString(objread["Notes"]) : null;
                    objNoteInfo.Notes_Date = Convert.ToDateTime(objread["Notes_Date"].ToString() != "" ? objread["Notes_Date"] : "01 / 01 / 0001");
                    objNoteInfo.IsPatientNote_Ind = Convert.ToString(objread["IsPatientNote_Ind"] != null ? objread["IsPatientNote_Ind"] : null);
                    if (objNoteInfo.IsPatientNote_Ind != "Y")
                    {
                        objNoteInfo.PatientName = "Non-Client";
                    }
                    else
                    {

                        objNoteInfo.PatientName = Convert.ToString(objread["PatientName"] != null ? objread["PatientName"] : null);
                    }
                    objNoteInfo.notetype = Convert.ToString(objread["notetype"] != null ? objread["notetype"] : null);
                    objNoteInfo.status_ind = Convert.ToString(objread["status_ind"] != null ? objread["status_ind"] : null);
                    objNoteInfo.CreatedBy = Convert.ToString(objread["CreatedBy"] != null ? objread["CreatedBy"] : null);
                    objNoteInfo.Transaction_id = Convert.ToString(objread["Transaction_id"] != null ? objread["Transaction_id"] : null);
                    objNoteInfo.Appointment_id = Convert.ToString(objread["Appointment_id"] != null ? objread["Appointment_id"] : null);
                    NotesInfo.Add(objNoteInfo);
                }
                objread.NextResult();
                while (objread.Read())
                {
                    objNotesInfo.TotalRecords = Convert.ToInt32(objread["TotalRecords"]);
                }
                return NotesInfo;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetNotesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool InsNotesInfo(NotesInfo objInsNotesInfo)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@In_FromReference_id", objInsNotesInfo.FromReference_ID),
			new SqlParameter("@in_Fromreferencetype_id ", objInsNotesInfo.FromReferenceType_ID),
			new SqlParameter("@in_Toreference_id", (objInsNotesInfo.ToReference_ID != 0 ? objInsNotesInfo.ToReference_ID : null)),
			new SqlParameter("@In_Toreferencetype_id", (objInsNotesInfo.ToReferenceType_ID != 0 ? objInsNotesInfo.ToReferenceType_ID : null)),
			new SqlParameter("@In_ispatientNote_ind", objInsNotesInfo.IsPatientNote_Ind),
			new SqlParameter("@In_Notes", objInsNotesInfo.Notes),
			new SqlParameter("@In_Notes_Date", objInsNotesInfo.Notes_Date),
			new SqlParameter("@In_CreatedBy", objInsNotesInfo.CreatedBy),
			new SqlParameter("@In_Practice_ID", (objInsNotesInfo.PracticeID == 0 ? null : objInsNotesInfo.PracticeID)),
			new SqlParameter("@In_LoginHistory_ID", HttpContext.Current.Session["LoginHistory_ID"])
		};
                objclscommon.AddInParameters(objparm);
                objclscommon.ExecuteNonQuerySP("Help_dbo.st_Notes_Ins_GenenalNotes");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "InsNotesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override NotesInfo GetupdNotesInfo(int GeneralNote_ID)
        {

            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                IDataParameter[] objparm = { new SqlParameter("@Notes_id", GeneralNote_ID) };
                objcommon1.AddInParameters(objparm);
                SqlDataReader objread = objcommon1.GetDataReader("Help_dbo.st_Notes_GET_Generalnotes");
                NotesInfo objNoteInfo = new NotesInfo();
                if (objread.Read())
                {
                    objNoteInfo.GeneralNote_ID = Convert.ToInt32(objread["GeneralNote_ID"].ToString() == "" ? null : objread["GeneralNote_ID"]);
                    objNoteInfo.FromReference_ID = Convert.ToInt32(objread["FromReference_ID"].ToString() == "" ? null : objread["FromReference_ID"]);
                    objNoteInfo.FromReferenceType_ID = Convert.ToInt32(objread["FromReferenceType_ID"].ToString() == "" ? null : objread["FromReferenceType_ID"]);
                    objNoteInfo.ToReference_ID = Convert.ToInt32(objread["ToReference_ID"].ToString() == "" ? null : objread["ToReference_ID"]);
                    objNoteInfo.ToReferenceType_ID = Convert.ToInt32(objread["ToReferenceType_ID"].ToString() == "" ? null : objread["ToReferenceType_ID"]);
                    objNoteInfo.Notes = Convert.ToString(objread["Notes"] == null ? null : objread["Notes"]);
                    objNoteInfo.Notes_Date = Convert.ToDateTime(objread["Notes_Date"].ToString() == "" ? null : objread["Notes_Date"]);
                    objNoteInfo.PatientName = Convert.ToString(objread["PatientName"].ToString() == "" ? null : objread["PatientName"]);
                    objNoteInfo.IsPatientNote_Ind = Convert.ToString(objread["IsPatientNote_Ind"] == null ? null : objread["IsPatientNote_Ind"]);
                    objNoteInfo.status_ind = Convert.ToString(objread["status_ind"] == null ? null : objread["status_ind"]);
                    objNoteInfo.CreatedBy = Convert.ToString(objread["CreatedBy"] == null ? null : objread["CreatedBy"]);
                }

                return objNoteInfo;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetupdNotesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool UpdNotesInfo(NotesInfo objUpdNotesInfo)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();

                IDataParameter[] objparm = {
			new SqlParameter("@Notes_id", objUpdNotesInfo.GeneralNote_ID),
		new SqlParameter("@in_Toreference_id", objUpdNotesInfo.ToReference_ID==0?null:objUpdNotesInfo.ToReference_ID),
			new SqlParameter("@In_Toreferencetype_id", objUpdNotesInfo.ToReferenceType_ID==0?null:objUpdNotesInfo.ToReferenceType_ID),
			new SqlParameter("@in_Status_ind", objUpdNotesInfo.status_ind),
			new SqlParameter("@In_Notes", objUpdNotesInfo.Notes),
			new SqlParameter("@In_Notes_Date", (objUpdNotesInfo.Notes_Date == null ? null : objUpdNotesInfo.Notes_Date)),
			new SqlParameter("@In_IsPatientNote_Ind ", objUpdNotesInfo.IsPatientNote_Ind),
			new SqlParameter("@In_updatedBy",HttpContext.Current.Session["UserID"]),
			new SqlParameter("@In_NoteType", objUpdNotesInfo.notetype),
			new SqlParameter("@In_LoginHistory_ID", objUpdNotesInfo.LoginHistory_ID)
		};
                objclscommon.AddInParameters(objparm);
                objclscommon.ExecuteNonQuerySP("Help_dbo.st_Notes_Upd_GenenalNotes");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UpdNotesInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override System.Data.DataSet Getgeneralnotesinfo(int? generalnoteid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objinparamlist = { new SqlParameter("@in_Note_ID", (generalnoteid == 0 ? null : generalnoteid)) };
                objcommon.AddInParameters(objinparamlist);
                return objcommon.GetDataSet("Help_dbo.st_Notes_GET_Generalnotesinfo");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "Getgeneralnotesinfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override int GetProviderLoginID(int PatientProviderID)
        {
            try
            {
                MowerHelper.Models.Classes.clsDataConfigure cls_data = new MowerHelper.Models.Classes.clsDataConfigure();
                IDataParameter[] paramlist = { new SqlParameter("@in_Provider_ID", PatientProviderID) };
                IDataParameter ReturnParam = new SqlParameter("@Loc_Login_ID", null);
                return cls_data.CommandDB("Help_dbo.st_Provider_GET_Provider_UserID", paramlist, ReturnParam);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetProviderLoginID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return 0;
        }
        public override List<Paymentmethods> CreditCard_list_paymentinfo(Paymentmethods objpayments)
        {
            try
            {
                clsCommonFunctions clscommon = new clsCommonFunctions();
                SqlDataReader ObjDR = default(SqlDataReader);
                int refTypeid = 0;
                int referenceid = 0;
                int IntPracticeID = 0;
                //if (HttpContext.Current.Session["Practice_ID"] != null)
                //{
                //    IntPracticeID = Convert.ToInt32(HttpContext.Current.Session["Practice_ID"].ToString());
                //}
                if (HttpContext.Current.Session["Roleid"].ToString() == "5")
                {
                    refTypeid = 3;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["PatientID"].ToString());
                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "32")
                {
                    refTypeid = 8;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["RespID"].ToString());
                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "33")
                {
                    refTypeid = 18;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["InsID"].ToString());
                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "35")
                {
                    refTypeid = 26;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["AuthorizedID"].ToString());

                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "1")
                {
                    refTypeid = 11;
                    referenceid = IntPracticeID;

                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "15")
                {
                    refTypeid = 11;
                    referenceid = IntPracticeID;
                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "4")
                {
                    refTypeid = 2;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["Prov_ID"].ToString());
                }
                else if (HttpContext.Current.Session["Roleid"].ToString() == "13")
                {
                    refTypeid = 11;
                    referenceid = Convert.ToInt32(HttpContext.Current.Session["Provider_ID"].ToString());
                }


                SqlParameter[] paramlist = {
		new SqlParameter("@in_referencetype_id", refTypeid),
		new SqlParameter("@in_reference_id", Convert.ToInt32(HttpContext.Current.Session["Prov_ID"].ToString()))
	};
                clscommon.AddInParameters(paramlist);
                ObjDR = clscommon.GetDataReader("Help_dbo.st_CreditCard_list_paymentinfo");
                List<Paymentmethods> objlist = new List<Paymentmethods>();
                int slno = 1;
                while (ObjDR.Read())
                {
                    Paymentmethods objpract = new Paymentmethods();
                    objpract.SlNo = slno;
                    slno = slno + 1;
                    if (Convert.ToInt32(ObjDR["CreditCardInfo_ID"]) != 0)
                    {
                        objpract.CreditCardInfo_ID = Convert.ToInt32(ObjDR["CreditCardInfo_ID"]);
                    }
                    if (Convert.ToInt32(ObjDR["referencetype_id"]) != 0)
                    {
                        objpract.referencetype_id = Convert.ToInt32(ObjDR["referencetype_id"]);
                    }
                    if (Convert.ToInt32(ObjDR["referencetype_id"]) != 0)
                    {
                        objpract.reference_id = Convert.ToInt32(ObjDR["referencetype_id"]);
                    }
                    if (ObjDR["CardType"] != null)
                    {
                        objpract.CardType = Convert.ToString(ObjDR["CardType"]);
                    }
                    if (ObjDR["cardexpiryyear"] != null)
                    {
                        objpract.cardexpiryyear = Convert.ToString(ObjDR["cardexpiryyear"]);
                    }
                    if (ObjDR["cardexpirymonth"] != null)
                    {
                        objpract.cardexpirymonth = Convert.ToString(ObjDR["cardexpirymonth"]);
                    }
                    if (ObjDR["cardexpirymonth"] != null && ObjDR["cardexpiryyear"] != null)
                    {
                        objpract.cardexpiry = Convert.ToString(ObjDR["cardexpirymonth"]) + "/" + Convert.ToString(ObjDR["cardexpiryyear"]);
                    }
                    if (ObjDR["Vaultid"] != null)
                    {
                        objpract.Vaultid = Convert.ToString(ObjDR["Vaultid"]);
                    }
                    if (ObjDR["customerid"].ToString() != "")
                    {
                        objpract.customerid = Convert.ToString(ObjDR["customerid"]);
                        HttpContext.Current.Session.Add("Stripe_customerid", ObjDR["customerid"].ToString());
                    }
                    if (ObjDR["Firstname"] != null)
                    {
                        objpract.Firstname = Convert.ToString(ObjDR["Firstname"]);
                        HttpContext.Current.Session.Add("Stripe_Firstname", ObjDR["Firstname"].ToString());
                    }
                    if (ObjDR["lastname"] != null)
                    {
                        objpract.lastname = Convert.ToString(ObjDR["lastname"]);
                        HttpContext.Current.Session.Add("Stripe_lastname", ObjDR["lastname"].ToString());
                    }
                    objlist.Add(objpract);
                }
                ObjDR.Close();
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "PaymentMethods", "CreditCard_list_paymentinfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Paymentmethods> CreditCard_Expirylist_info(Paymentmethods objpayments)
        {
            clsCommonFunctions clscommon = new clsCommonFunctions();
            SqlDataReader ObjDR = default(SqlDataReader);
              List<Paymentmethods> objlist = new List<Paymentmethods>();
             try
            {
            SqlParameter[] paramlist = {
		new SqlParameter("@in_provider_id", Convert.ToInt32(HttpContext.Current.Session["Prov_ID"].ToString()))
	};
            clscommon.AddInParameters(paramlist);
            ObjDR = clscommon.GetDataReader("Help_dbo.st_get_ccinfoid_provider_id");
          
            while (ObjDR.Read())
            {
                Paymentmethods objpract = new Paymentmethods();
                if (ObjDR["ccinfo_id"] != DBNull.Value)
                {
                    if (ObjDR["ccinfo_id"] != null)
                    {
                        objpract.CreditCardInfo_ID = Convert.ToInt32(ObjDR["ccinfo_id"]);
                    }
                }
                else
                {
                    objpract.CreditCardInfo_ID = null;
                }
                if (ObjDR["Daysremaining"] != null)
                {
                    objpract.Daysremining = Convert.ToInt32(ObjDR["Daysremaining"]);
                }
                else
                {
                    objpract.Daysremining = null;
                }
                objlist.Add(objpract);
            }
            ObjDR.Close();
                    }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "CreditCard_Expirylist_info", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return objlist;
        }
        public override bool UpdateCreditCardVaultID(string ProviderID, string Vaultid, int? Out_CCID, string customer_id)
        {
            clsCommonFunctions objCommon = new clsCommonFunctions();

            try
            {
                IDataParameter[] objInParam = {
			new SqlParameter("@in_provider_id", (ProviderID == null? null : ProviderID)),
			new SqlParameter("@in_paypal_vaultid", (Vaultid == null ? null : Vaultid)),
            new SqlParameter("@In_customer_id", customer_id),
            new SqlParameter("@In_ccinfo_id", Out_CCID)
			
		};
                objCommon.AddInParameters(objInParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_upd_cc_vault_id");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "UpdateCreditCardVaultID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override bool DeleteCreditCard(string CreditCardInfo_ID, string UserID)
        {
            clsCommonFunctions objCommon = new clsCommonFunctions();

            try
            {
                IDataParameter[] objInParam = {
			new SqlParameter("@in_CreditCardInfo_ID", (CreditCardInfo_ID == null? null : CreditCardInfo_ID)),
			new SqlParameter("@in_UpdatedBy", (UserID == null ? null : UserID)),
			new SqlParameter("@in_Loginhistory_ID", (HttpContext.Current.Session["Loginhistory_ID"] == null ? null : HttpContext.Current.Session["Loginhistory_ID"] ))
		};
                objCommon.AddInParameters(objInParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_CreditCard_DEL_Card");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "DeleteCreditCard", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override string GetVaultID(string cardinfoid)
        {
            clsCommonFunctions objCommon = new clsCommonFunctions();
            try
            {
                IDataParameter[] objInParam = {
			new SqlParameter("@in_CreditCardInfo_ID", (cardinfoid ?? null))
			
		};
                objCommon.AddInParameters(objInParam);
                SqlDataReader dr = objCommon.GetDataReader("Help_dbo.st_get_paypalvalult_id_by_ccinfoid");
                if (dr.Read())
                {
                    if (dr["paypal_vaultid"].ToString() != "")
                    {
                        return dr["paypal_vaultid"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetVaultID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<BLL.Billing.CCProcess> CreditCard_Get_paymentinfo(BLL.Billing.CCProcess obj)
        {
            try
            {
                clsCommonFunctions clscommon = new clsCommonFunctions();
                var objlist = new List<BLL.Billing.CCProcess>();
                IDataParameter[] inparamlist = {
		new SqlParameter("@in_CreditCardInfo_ID", obj.CardID),
		new SqlParameter("@in_Login_ID", HttpContext.Current.Session["userid"]),
		new SqlParameter("@in_Provider_ID", obj.Provider_ID)
	};
                clscommon.AddInParameters(inparamlist);
                SqlDataReader objDR = clscommon.GetDataReader("Help_dbo.st_CreditCard_Get_paymentinfo");
                while (objDR.Read())
                {

                    obj.strx_card_code = objDR["CreditCardCode"] != null ? objDR["CreditCardCode"].ToString() : null;
                    if (objDR["cardexpirymonth"] != null)
                    {
                        obj.StrExpMon = Convert.ToInt32(objDR["cardexpirymonth"]);
                    }
                    else
                    {
                        obj.StrExpMon = null;
                    }
                    if (objDR["cardexpiryyear"] != null)
                    {
                        obj.StrExpYear = Convert.ToInt32(objDR["cardexpiryyear"]);
                    }
                    obj.strBillAddress1 = objDR["billingaddress_1"] != null ? objDR["billingaddress_1"].ToString() : null;
                    obj.strBillAddress2 = objDR["billingaddress_2"] != null ? objDR["billingaddress_2"].ToString() : null;
                    obj.strStateID = objDR["billingstate_id"] != null ? objDR["billingstate_id"].ToString() : null;
                    obj.strCityID = objDR["billingcity_id"] != null ? objDR["billingcity_id"].ToString() : null;
                    obj.strZipCode = objDR["billingzip"] != null ? objDR["billingzip"].ToString() : null;
                    obj.practice_ind = objDR["practice_ind"] != null ? objDR["practice_ind"].ToString() : null;
                    obj.ALLowCCCharges = objDR["AllowCCCharges"] != null ? objDR["AllowCCCharges"].ToString() : null;
                    obj.state = objDR["StateName"] != null ? objDR["StateName"].ToString() : null;
                    obj.city = objDR["CityName"] != null ? objDR["CityName"].ToString() : null;
                    objlist.Add(obj);
                    obj.DefaultChargeCC_ID = objDR["DefaultChargeCC_ID"] != null ? objDR["DefaultChargeCC_ID"].ToString() : null;
                    obj.FirstName = objDR["FirstName"] != null ? objDR["FirstName"].ToString() : null;
                    obj.LastName = objDR["LastName"] != null ? objDR["LastName"].ToString() : null;
                    obj.IssuingBank = objDR["IssuingBank"] != null ? objDR["IssuingBank"].ToString() : null;

                    if (objDR["customerid"].ToString() != null && objDR["customerid"].ToString() != "")
                    {
                        //obj.customerid = Convert.ToString(objDR["customerid"]);
                        HttpContext.Current.Session.Add("Stripe_customerid", objDR["customerid"].ToString());
                    }
                    else
                    {
                        HttpContext.Current.Session.Add("Stripe_customerid", null);
                    }
                    HttpContext.Current.Session.Add("Stripe_Firstname", objDR["Firstname"].ToString());
                    HttpContext.Current.Session.Add("Stripe_lastname", objDR["lastname"].ToString());

                }
                objDR.Close();
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "CreditCard_Get_paymentinfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<PTHome> GetPracticesOfPatient(int PatientLogin_ID, int? practice_ID, int? ProviderID = null)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] InparametersList = {
			new SqlParameter("@in_PatientLogin_ID", PatientLogin_ID),
			new SqlParameter("@In_Provider_ID", (ProviderID == 0 ? null : ProviderID)),
			new SqlParameter("@In_Practice_id", (practice_ID == 0 ? null : practice_ID))
		};
                objCommon.AddInParameters(InparametersList);
                SqlDataReader drPracticelist = default(SqlDataReader);
                drPracticelist = objCommon.GetDataReader("Help_dbo.st_Patient_List_Practices");
                List<PTHome> objdrPracticelist = new List<PTHome>();
                PTHome objPt = default(PTHome);
                while (drPracticelist.Read())
                {
                    objPt = new PTHome((drPracticelist["Practice_ID"] != DBNull.Value ? (int)drPracticelist["Practice_ID"] : 0), (drPracticelist["PracticeName"] != DBNull.Value ? (string)drPracticelist["PracticeName"] : null), (drPracticelist["Provider_ID"] != DBNull.Value ? (int)drPracticelist["Provider_ID"] : 0), (drPracticelist["ProviderName"] != DBNull.Value ? (string)drPracticelist["ProviderName"] : null), null, null, null, null, null);
                    objdrPracticelist.Add(objPt);
                }

                return objdrPracticelist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetPracticesOfPatient", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Provider_ProviderList> Get_ElProviderInProfile(string FirstName, string LastName, Nullable<int> Practice_ID, int PlaceOfService_ID, string OrderByItem, string OrderBy, Nullable<int> Login_ID, int Role_ID, string Status_Ind, int NoOfRecord,
        int PageNo, string providertype, ref int TotalRecords, string email, int? Provider_ID = 0)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@in_FirstName", FirstName),
			new SqlParameter("@in_LastName", LastName),
			new SqlParameter("@in_Practice_ID", Practice_ID),
			new SqlParameter("@in_PlaceOfService_ID", PlaceOfService_ID),
			new SqlParameter("@in_OrderbyItem", OrderByItem),
			new SqlParameter("@in_Order", OrderBy),
			new SqlParameter("@in_Login_ID", Login_ID),
			new SqlParameter("@in_Role_ID", Role_ID),
			new SqlParameter("@in_Status_Ind", Status_Ind),
			new SqlParameter("@In_NoOfRecords", NoOfRecord),
			new SqlParameter("@in_Email", email),
			new SqlParameter("@In_pageNo", PageNo),
			new SqlParameter("@In_Provider_ID", (Provider_ID == 0 ? null : Provider_ID))
		};
                objcommon.AddInParameters(objInparamlist);
                if (providertype == "nonverifiedprovider")
                {
                    objread = objcommon.GetDataReader("st_non_verified_providerslist_RDPaging");
                }
                else if (providertype == "verifiedprovider")
                {
                    objread = objcommon.GetDataReader("st_verified_providerslist_RDPaging");
                }
                else
                {
                    objread = objcommon.GetDataReader("Help_dbo.st_ADMIN_SearchProviders_RDPaging");
                }

                List<Provider_ProviderList> ProviderList = new List<Provider_ProviderList>();
                int SlNo = NoOfRecord * (PageNo - 1);
                while (objread.Read())
                {
                    SlNo += 1;
                    if (providertype == "nonverifiedprovider")
                    {
                        Provider_ProviderList ObjProv = new Provider_ProviderList(SlNo, (objread["Provider_ID"] != null ? Convert.ToInt32(objread["Provider_ID"]) : -1), Convert.ToString(objread["ProviderName"] != null ? objread["ProviderName"] : null), null, null, null, null, null, null, null, null, null, null);
                        ObjProv.state_id = Convert.ToString(objread["state_id"] != null ? objread["state_id"] : null);
                        ObjProv.LicenseNo = Convert.ToString(objread["LicenseNo"] != null ? objread["LicenseNo"] : null);
                        ObjProv.ElectricianFillename = Convert.ToString(objread["Electrician_license"] != null ? objread["Electrician_license"] : null);
                        ObjProv.DriverFilename = Convert.ToString(objread["Driver_license"] != null ? objread["Driver_license"] : null);
                        ObjProv.ElctricianCustmiseImage = Convert.ToString(objread["Electrician_license_customized"] != null ? objread["Electrician_license_customized"] : null);
                        ObjProv.DriverCustomiseImage = Convert.ToString(objread["Driver_license_customized"] != null ? objread["Driver_license_customized"] : null);
                        ProviderList.Add(ObjProv);
                    }
                    else if (providertype == "verifiedprovider")
                    {
                        Provider_ProviderList ObjProv = new Provider_ProviderList(SlNo, (objread["Provider_ID"] != null ? Convert.ToInt32(objread["Provider_ID"]) : -1), Convert.ToString(objread["ProviderName"] != null ? objread["ProviderName"] : null), Convert.ToString(objread["Address"] != null ? objread["Address"] : null), null, Convert.ToString(objread["UpdatedOn"] != null ? objread["UpdatedOn"] : null), Convert.ToInt32(objread["Login_ID"] != null ? objread["Login_ID"] : null), null, Convert.ToString(objread["Status_Ind"] != null ? objread["Status_Ind"] : null), null, null, null, null);

                        ObjProv.RoleID = Convert.ToString(objread["Role_ID"] != null ? objread["Role_ID"] : null);
                        ObjProv.verifiedon = Convert.ToString(objread["verifiedon"] != null ? objread["verifiedon"] : null);
                        ObjProv.verifiedby = Convert.ToString(objread["verifiedby"] != null ? objread["verifiedby"] : null);
                        ObjProv.providerdocuments = Convert.ToString(objread["Providerdocuments"] != null ? objread["Providerdocuments"] : null);
                        ObjProv.ElectricianFillename = Convert.ToString(objread["Electrician_license"] != null ? objread["Electrician_license"] : null);
                        ObjProv.DriverFilename = Convert.ToString(objread["Driver_license"] != null ? objread["Driver_license"] : null);
                        ObjProv.ElctricianCustmiseImage = Convert.ToString(objread["Electrician_license_customized"] != null ? objread["Electrician_license_customized"] : null);
                        ObjProv.DriverCustomiseImage = Convert.ToString(objread["Driver_license_customized"] != null ? objread["Driver_license_customized"] : null);
                        ProviderList.Add(ObjProv);

                    }
                    else
                    {
                        Provider_ProviderList ObjProv = new Provider_ProviderList(SlNo, (objread["Provider_ID"] != null ? Convert.ToInt32(objread["Provider_ID"]) : -1), Convert.ToString(objread["ProviderName"] != null ? objread["ProviderName"] : null), Convert.ToString(objread["Address"] != null ? objread["Address"] : null), null, Convert.ToString(objread["UpdatedOn"] != null ? objread["UpdatedOn"] : null), Convert.ToInt32(objread["Login_ID"] != null ? objread["Login_ID"] : null), null, Convert.ToString(objread["Status_Ind"] != null ? objread["Status_Ind"] : null), Convert.ToString(objread["CellPhone"] != null ? objread["CellPhone"] : null), Convert.ToString(objread["provideremail"] != null ? objread["provideremail"] : null), Convert.ToString(objread["RegisteredDate"] != null ? objread["RegisteredDate"] : null), Convert.ToInt32(objread["Techniciancount"] != null ? objread["Techniciancount"] : null));

                        ObjProv.TreatmentPlannerInd = Convert.ToString(objread["TreatmentPlannerInd"] != null ? objread["TreatmentPlannerInd"] : null);
                        ObjProv.RoleID = Convert.ToString(objread["Role_ID"] != null ? objread["Role_ID"] : null);
                        ObjProv.Debug_mode = Convert.ToString(objread["Debug_mode"] != null ? objread["Debug_mode"] : null);
                        ProviderList.Add(ObjProv);
                    }
                }
                if (objread.NextResult() == true)
                {
                    if (objread.Read())
                    {
                        TotalRecords = Convert.ToInt32(objread["TOTALRECORDS"]);
                    }
                }
                return ProviderList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_ElProviderInProfile", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Provider_ProviderList> Get_NewElesignups(string OrderByItem, string OrderBy, int NoOfRecord, string Fromdate, string Todate,
       int PageNo, ref int TotalRecords, string email, int? Provider_ID = 0)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@in_OrderbyItem", OrderByItem),
			new SqlParameter("@in_Order", OrderBy),
			new SqlParameter("@In_NoOfRecords", NoOfRecord),
			new SqlParameter("@in_Email", email),
			new SqlParameter("@In_pageNo", PageNo),
			new SqlParameter("@In_Provider_ID", (Provider_ID == 0 ? null : Provider_ID)),
            new SqlParameter("@In_Fromdate", Fromdate),
			new SqlParameter("@In_Todate", Todate)
		};
                objcommon.AddInParameters(objInparamlist);

                objread = objcommon.GetDataReader("st_ADMIN_Search_newsignup_Providers_RDPaging");
                List<Provider_ProviderList> ProviderList = new List<Provider_ProviderList>();
                int SlNo = NoOfRecord * (PageNo - 1);
                while (objread.Read())
                {
                    SlNo += 1;

                    Provider_ProviderList ObjProv = new Provider_ProviderList(SlNo, (objread["Provider_ID"] != null ? Convert.ToInt32(objread["Provider_ID"]) : -1), Convert.ToString(objread["ProviderName"] != null ? objread["ProviderName"] : null), Convert.ToString(objread["Address"] != null ? objread["Address"] : null), null, Convert.ToString(objread["CellPhone"] != null ? objread["CellPhone"] : null), Convert.ToString(objread["provideremail"] != null ? objread["provideremail"] : null), Convert.ToString(objread["RegisteredDate"] != null ? objread["RegisteredDate"] : null), Convert.ToString(objread["Registeredin"] != null ? objread["Registeredin"] : null));
                    ProviderList.Add(ObjProv);

                }
                if (objread.NextResult() == true)
                {
                    if (objread.Read())
                    {
                        TotalRecords = Convert.ToInt32(objread["TOTALRECORDS"]);
                    }
                }
                return ProviderList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_ElProviderInProfile", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Update_licensedetails(Provider_ProfilesInfo ObjProfileInfoProv)
        {
            try
            {
                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@IN_Provider_id", ObjProfileInfoProv.Provider_ID),
			new SqlParameter("@In_updatedby", (ObjProfileInfoProv.UpdatedBy == 0 ? null : ObjProfileInfoProv.UpdatedBy)),
			new SqlParameter("@In_LicenseNo", ObjProfileInfoProv.LicenseNo),
			new SqlParameter("@In_IsLicenseVerified", ObjProfileInfoProv.IsLicenseVerified),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"]),
			new SqlParameter("@in_licenseexpirydate", ObjProfileInfoProv.licenseexpirydate)
		};

                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_upd_provider_license_details");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Update_licensedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override List<Provider_ProviderList> list_licenseExpired(Nullable<int> Practice_ID, string fromdate, string todate, string OrderByItem, string OrderBy, int NoOfRecord, int PageNo, string toreferenceids)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@in_Practice_ID", Practice_ID),
			new SqlParameter("@in_fromdate", fromdate),
			new SqlParameter("@in_todate", todate),
			new SqlParameter("@in_OrderbyItem", OrderByItem),
			new SqlParameter("@in_Order", OrderBy),
			new SqlParameter("@In_NoOfRecords", 15),
			new SqlParameter("@In_pageNo", PageNo),
			new SqlParameter("@in_toreference_ids", toreferenceids)
		};

                objcommon.AddInParameters(objInparamlist);
                objread = objcommon.GetDataReader("st_Admin_licenseExpireList");
                List<Provider_ProviderList> ProviderList = new List<Provider_ProviderList>();
                int SlNo = NoOfRecord * (PageNo - 1);
                while (objread.Read())
                {
                    SlNo += 1;
                    Provider_ProviderList ObjProv = new Provider_ProviderList(SlNo, (objread["Provider_ID"] != null ? Convert.ToInt32(objread["Provider_ID"]) : -1), Convert.ToString(objread["ProviderName"] != null ? objread["ProviderName"] : null), Convert.ToString(objread["Address"] != null ? objread["Address"] : null), null, Convert.ToString(objread["UpdatedOn"] != null ? objread["UpdatedOn"] : null), Convert.ToInt32(objread["Login_ID"] != null ? objread["Login_ID"] : null), null, Convert.ToString(objread["Status_Ind"] != null ? objread["Status_Ind"] : null), null, null, null, null);

                    ObjProv.licenseexpirydate = Convert.ToString(objread["LicenseExpirydate"] != null ? objread["LicenseExpirydate"] : null);
                    ObjProv.Email = Convert.ToString(objread["Email"] != null ? objread["Email"] : null);


                    ProviderList.Add(ObjProv);
                }
                if (objread.NextResult() == true)
                {
                    if (objread.Read())
                    {
                        Provider_ProviderList.TotalRecords = Convert.ToString(objread["TOTALRECORDS"]);
                    }
                }
                return ProviderList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "list_licenseExpired", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Provider_ProviderList> signups_monthWisecount(string Year)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@in_Year", Year)
		};

                objcommon.AddInParameters(objInparamlist);
                objread = objcommon.GetDataReader("Help_dbo.ST_ADmin_new_signups_month_Wise_count");
                List<Provider_ProviderList> ProviderList = new List<Provider_ProviderList>();
                int PageNo = 1;
                int NoOfRecord = 10;
                int SlNo = NoOfRecord * (PageNo - 1);
                while (objread.Read())
                {
                    Provider_ProviderList ObjProv = new Provider_ProviderList();
                    if (objread["January"] != DBNull.Value)
                    {
                        ObjProv.January = Convert.ToString(objread["January"]);
                    }
                    else
                    {
                        ObjProv.January = "0";
                    }
                    if (objread["February"] != DBNull.Value)
                    {
                        ObjProv.February = Convert.ToString(objread["February"]);
                    }
                    else
                    {
                        ObjProv.February = "0";
                    }
                    if (objread["March"] != DBNull.Value)
                    {
                        ObjProv.March = Convert.ToString(objread["March"]);
                    }
                    else
                    {
                        ObjProv.March = "0";
                    }
                    if (objread["April"] != DBNull.Value)
                    {
                        ObjProv.April = Convert.ToString(objread["April"]);
                    }
                    else
                    {
                        ObjProv.April = "0";
                    }
                    if (objread["May"] != DBNull.Value)
                    {
                        ObjProv.May = Convert.ToString(objread["May"]);
                    }
                    else
                    {
                        ObjProv.May = "0";
                    }
                    if (objread["June"] != DBNull.Value)
                    {
                        ObjProv.June = Convert.ToString(objread["June"]);
                    }
                    else
                    {
                        ObjProv.June = "0";
                    }
                    if (objread["July"] != DBNull.Value)
                    {
                        ObjProv.July = Convert.ToString(objread["July"]);
                    }
                    else
                    {
                        ObjProv.July = "0";
                    }
                    if (objread["August"] != DBNull.Value)
                    {
                        ObjProv.August = Convert.ToString(objread["August"]);
                    }
                    else
                    {
                        ObjProv.August = "0";
                    }
                    if (objread["September"] != DBNull.Value)
                    {
                        ObjProv.September = Convert.ToString(objread["September"]);
                    }
                    else
                    {
                        ObjProv.September = "0";
                    }
                    if (objread["October"] != DBNull.Value)
                    {
                        ObjProv.October = Convert.ToString(objread["October"]);
                    }
                    else
                    {
                        ObjProv.October = "0";
                    }
                    if (objread["November"] != DBNull.Value)
                    {
                        ObjProv.November = Convert.ToString(objread["November"]);
                    }
                    else
                    {
                        ObjProv.November = "0";
                    }
                    if (objread["December"] != DBNull.Value)
                    {
                        ObjProv.December = Convert.ToString(objread["December"]);
                    }
                    else
                    {
                        ObjProv.December = "0";
                    }


                    ProviderList.Add(ObjProv);
                }
                return ProviderList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "signups_monthWisecount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool InslicenseRenewalsMailStatus(string ToreferenceId, string FacilityIds = null)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strInsParam = {
		new SqlParameter("@In_ToReference_IDs", ToreferenceId),
		new SqlParameter("@In_Facility_IDs", FacilityIds),
		new SqlParameter("@In_CreatedBy ", HttpContext.Current.Session["userid"])
	};
                objcommon.AddInParameters(strInsParam);
                objcommon.ExecuteNonQuerySP("st_Admin_Ins_licenseExpirySendMessage");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InslicenseRenewalsMailStatus", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override string UPDATE_Email_Message_Status(string EmailMessage_Transaction_IDs)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] objInparam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", EmailMessage_Transaction_IDs) };

                objCommon.AddInParameters(objInparam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                return string.Empty;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UPDATE_Email_Message_Status", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override Ms_UserCollection GetUsersByRoleid(int intSearchType, int RoleID, int PageNo, ref string Display_Name, ref int Total_Records, string lastname = null)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                string strStoredProcName = "";
                strStoredProcName = "Help_dbo.st_MessageStation_Search_ADMIN_Reciever_RDPAGING";
                int LoginRole_ID = Convert.ToInt32(HttpContext.Current.Session["RoleID"]);
                if (LoginRole_ID == 38)
                {
                    LoginRole_ID = 31;
                }
                else if (LoginRole_ID == 31)
                {
                    LoginRole_ID = 38;
                }

                AddParamToSQLCmd(sqlCmd, "@in_LoginRole_ID", SqlDbType.Int, 0, ParameterDirection.Input, LoginRole_ID);
                AddParamToSQLCmd(sqlCmd, "@in_Role_ID", SqlDbType.Int, 0, ParameterDirection.Input, RoleID);
                AddParamToSQLCmd(sqlCmd, "@in_Login_ID", SqlDbType.Int, 0, ParameterDirection.Input, HttpContext.Current.Session["userID"]);
                AddParamToSQLCmd(sqlCmd, "@in_LastName", SqlDbType.VarChar, 100, ParameterDirection.Input, (!string.IsNullOrEmpty(lastname) ? lastname : null));
                AddParamToSQLCmd(sqlCmd, "@in_PhoneNo", SqlDbType.NVarChar, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_City", SqlDbType.NVarChar, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_State", SqlDbType.NVarChar, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_Zip", SqlDbType.NVarChar, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@in_NoofRecords", SqlDbType.Int, 0, ParameterDirection.Input, Total_Records);
                AddParamToSQLCmd(sqlCmd, "@in_PageNo", SqlDbType.Int, 0, ParameterDirection.Input, PageNo);
                AddParamToSQLCmd(sqlCmd, "@In_OrderBy", SqlDbType.NVarChar, 20, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@In_OrderBYitem", SqlDbType.NVarChar, 20, ParameterDirection.Input, DBNull.Value);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, strStoredProcName);
                GenerateCollectionFromReaderTR sqlData = new GenerateCollectionFromReaderTR(GeneratemS_UserCollectionFromReader1);
                Ms_UserCollection stsCollection = (Ms_UserCollection)ExecuteReaderCmd1(sqlCmd, sqlData, ref Total_Records);
                return stsCollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetUsersByRoleid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        private CollectionBase ExecuteReaderCmd1(SqlCommand sqlCmd, GenerateCollectionFromReaderTR gcfr, ref int Total_Records)
        {
            if (ConnectionString == string.Empty)
            {
                throw new ArgumentOutOfRangeException("ConnectionString");
            }
            if (sqlCmd == null)
            {
                throw new ArgumentNullException("sqlCmd");
            }
            SqlConnection cn = new SqlConnection(this.ConnectionString);
            try
            {
                sqlCmd.Connection = cn;
                cn.Open();
                CollectionBase temp = gcfr(sqlCmd.ExecuteReader(), ref Total_Records);
                cn.Close();
                return temp;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ExecuteReaderCmd1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Inactivate_Provider_Status(int Provider_ID)
        {
            try
            {
                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = { new SqlParameter("@in_Provider_ID", Provider_ID) };

                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_InActivate_Provider");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Inactivate_Provider_Status", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool Upd_Vacation_Provider_Status(int Provider_ID)
        {
            try
            {
                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = { new SqlParameter("@in_Provider_ID", Provider_ID) };

                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Vacation_Activate_Provider");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Upd_Vacation_Provider_Status", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool Del_Provider(int Provider_ID, int DeletedBy)
        {
            try
            {
                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_Provider_ID", Provider_ID),
			new SqlParameter("@in_DeletedBy", DeletedBy)
		};


                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Provider_DEL_Provider");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Del_Provider", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override System.Collections.Generic.List<Login_history> Admin_Provider_Login_history(Login_history objlogindata, ref int Total_Records)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                SqlDataReader dataRead = default(SqlDataReader);
                IDataParameter[] Paramlist = {
			new SqlParameter("@In_noOfRec", objlogindata.NoofRecords),
			new SqlParameter("@in_pageNo", objlogindata.PageNo),
			new SqlParameter("@In_orderBY", objlogindata.OrderBy),
			new SqlParameter("@In_OrderByItem", objlogindata.OrderByItem),
			new SqlParameter("@In_Login_id", objlogindata.Login_ID),
			new SqlParameter("@in_BeginDate", objlogindata.BeginDate),
			new SqlParameter("@in_EndDate", objlogindata.EndDate)
		};
                objCommon.AddInParameters(Paramlist);
                dataRead = objCommon.GetDataReader("Help_dbo.st_Admin_List_provider_loginhistory_Rdpaging");
                List<Login_history> Adcollection = new List<Login_history>();

                while (dataRead.Read())
                {
                    Login_history objcollection = new Login_history();
                    if (dataRead["R_No"] != null)
                    {
                        objcollection.Sino = Convert.ToString(dataRead["R_No"]);
                    }
                    else
                    {
                        objcollection.Sino = null;
                    }
                    if (dataRead["Username"] != null)
                    {
                        objcollection.Username = Convert.ToString(dataRead["Username"]);
                    }
                    else
                    {
                        objcollection.Username = null;
                    }

                    if (dataRead["RoleName"] != null)
                    {
                        objcollection.RoleName = Convert.ToString(dataRead["RoleName"]);
                    }
                    else
                    {
                        objcollection.RoleName = null;
                    }

                    if (dataRead["IPAddress"] != null)
                    {
                        objcollection.IPAddress = Convert.ToString(dataRead["IPAddress"]);
                    }
                    else
                    {
                        objcollection.IPAddress = null;
                    }

                    if (dataRead["LogIn_DateTime"] != null)
                    {
                        objcollection.LogIn_DateTime = Convert.ToString(dataRead["LogIn_DateTime"]);
                    }
                    else
                    {
                        objcollection.LogIn_DateTime = null;
                    }

                    if (dataRead["LoginSuccess_Ind"] != null)
                    {
                        objcollection.LoginSuccess_Ind = Convert.ToBoolean(dataRead["LoginSuccess_Ind"]);
                        if (objcollection.LoginSuccess_Ind == true)
                        {
                            objcollection.logsuccess = "Yes";
                        }
                        else
                        {
                            objcollection.logsuccess = "No";
                        }
                    }
                    else
                    {
                        objcollection.LoginSuccess_Ind = false;
                        objcollection.logsuccess = "No";
                    }
                    if (dataRead["LoginHistory_ID"] != null)
                    {
                        objcollection.LoginHistory_ID = Convert.ToInt32(dataRead["LoginHistory_ID"]);
                    }
                    else
                    {
                        objcollection.LoginHistory_ID = null;
                    }
                    Adcollection.Add(objcollection);
                }
                if (dataRead.NextResult() == true)
                {
                    if (dataRead.Read() == true)
                    {
                        Total_Records = Convert.ToInt32(dataRead["totalRec"]);
                    }
                }
                return Adcollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_Provider_Login_history", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;


        }
        public override PracticeblHome Get_SendMailsToProviderDetails(int? objgethome, string Provider_ID = null)
        {
            try
            {
                string strusername = null;
                string strpassword = null;
                string strfullname = null;
                string stremail = null;
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparamlist = {
			new SqlParameter("@In_Login_ID", (objgethome == 0 ? null : objgethome)),
			new SqlParameter("@In_provider_ID", (Provider_ID == null ? null : Provider_ID))
		};
                objcommon.AddInParameters(objInparamlist);
                objread = objcommon.GetDataReader("Help_dbo.st_Security_Get_User");
                PracticeblHome ObjSecurity = default(PracticeblHome);
                if (objread.Read() == true)
                {
                    if (objread["Username"] != null)
                    {
                        strusername = Convert.ToString(objread["Username"]);
                    }
                    else
                    {
                        strusername = null;
                    }
                    if (objread["Password"] != null)
                    {
                        strpassword = Convert.ToString(objread["Password"]);
                    }
                    else
                    {
                        strpassword = null;
                    }

                    if (objread["UserFullName"] != null)
                    {
                        strfullname = Convert.ToString(objread["UserFullName"]);
                    }
                    else
                    {
                        strfullname = null;
                    }

                    if (objread["Email"] != null)
                    {
                        stremail = Convert.ToString(objread["Email"]);
                    }
                    else
                    {
                        stremail = null;
                    }
                    ObjSecurity = new PracticeblHome(strusername, strpassword, strfullname, stremail);
                    return ObjSecurity;
                }
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_SendMailsToProviderDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Temp_Ins_Password(Provider_Password obj)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] objParam = {
			new SqlParameter("@in_Password", Convert.ToString(obj.Password != null ? obj.Password : null)),
			new SqlParameter("@in_Login_ID", Convert.ToInt32(obj.Login_ID != 0 ? obj.Login_ID : null)),
			new SqlParameter("@In_CreatedBy", HttpContext.Current.Session["userid"])
		};
                objCommon.AddInParameters(objParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_Admin_INS_TempPassword");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Temp_Ins_Password", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override List<Admin_postajob> verificationusersList(Admin_postajob objusersList, ref int Total_Records)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader dataRead = default(SqlDataReader);

                IDataParameter[] iParameter = {
			new SqlParameter("@In_OrderBy", (!string.IsNullOrEmpty(objusersList.OrderBy) ? Convert.ToString(objusersList.OrderBy) : null)),
			new SqlParameter("@in_OrderbyItem", (!string.IsNullOrEmpty(objusersList.OrderByItem) ? objusersList.OrderByItem : null)),
			new SqlParameter("@In_pageno", (objusersList.PageNo != 0 ? objusersList.PageNo : null)),
			new SqlParameter("@in_NoofRec", (objusersList.NoOfRecords !=0 ? objusersList.NoOfRecords : null)),
			new SqlParameter("@In_Fullname", (!string.IsNullOrEmpty(objusersList.Full_Name) ? objusersList.Full_Name : null)),
			new SqlParameter("@In_Alphabet", (!string.IsNullOrEmpty(objusersList.LastName) ? objusersList.LastName : null))
		};

                objcommon.AddInParameters(iParameter);
                dataRead = objcommon.GetDataReader("St_Admin_List_verification_users_Rdpaging");
                List<Admin_postajob> objlist = new List<Admin_postajob>();

                while (dataRead.Read())
                {
                    Admin_postajob objuser = new Admin_postajob();
                    if (dataRead["R_No"] != DBNull.Value)
                    {
                        objuser.slno = Convert.ToString(dataRead["R_No"]);
                    }
                    else
                    {
                        objuser.slno = null;
                    }
                    if (dataRead["Full_name"] != DBNull.Value)
                    {
                        objuser.Full_Name = Convert.ToString(dataRead["Full_name"]);
                    }
                    else
                    {
                        objuser.Full_Name = null;
                    }
                    if (dataRead["FullAddress"] != DBNull.Value)
                    {
                        objuser.fulladdress = dataRead["FullAddress"].ToString();
                    }
                    else
                    {
                        objuser.fulladdress = null;
                    }
                    if (dataRead["WorkPhone"] != DBNull.Value)
                    {
                        objuser.WorkPhone = dataRead["WorkPhone"].ToString();
                    }
                    else
                    {
                        objuser.WorkPhone = null;
                    }
                    if (dataRead["verification_User_Id"] != DBNull.Value)
                    {
                        objuser.verification_User_Id = Convert.ToInt32(dataRead["verification_User_Id"]);
                    }
                    else
                    {
                        objuser.verification_User_Id = null;
                    }
                    if (dataRead["Login_ID"] != DBNull.Value)
                    {
                        objuser.Login_ID = Convert.ToInt32(dataRead["Login_ID"]);
                    }
                    else
                    {
                        objuser.Login_ID = null;
                    }
                    if (dataRead["Status_Ind"] != DBNull.Value)
                    {
                        objuser.Status_Ind = Convert.ToChar(dataRead["Status_Ind"]);
                    }
                    else
                    {
                        objuser.Status_Ind = null;
                    }
                    if (dataRead["Rolename"] != DBNull.Value)
                    {
                        objuser.RoleType = dataRead["Rolename"].ToString();
                    }
                    else
                    {
                        objuser.RoleType = null;
                    }
                    objlist.Add(objuser);
                }
                if (dataRead.NextResult() == true)
                {
                    if (dataRead.Read() == true)
                    {
                        Total_Records = (int)dataRead["TotalRec"];
                    }
                }
                return objlist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "verificationusersList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object Insert_verificationusers(Admin_postajob ObjIns, ref string Out_Msg, ref int Out_verification_User_ID, ref int Out_Login_ID, int? loginhistoryid = null)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] paramval = {
			new SqlParameter("@in_Prefix", ObjIns.Title),
			new SqlParameter("@in_FirstName", ObjIns.FirstName),
			new SqlParameter("@in_MI", ObjIns.MI),
			new SqlParameter("@in_LastName", ObjIns.LastName),
			new SqlParameter("@in_Suffix", ObjIns.Suffix),
			new SqlParameter("@in_DOB", ObjIns.DOB),
			new SqlParameter("@in_Gender", ObjIns.Gender),
			new SqlParameter("@in_Email", ObjIns.Email),
			new SqlParameter("@in_Address1", ObjIns.Address1),
			new SqlParameter("@in_Address2", ObjIns.Address2),
			new SqlParameter("@in_City_ID", ObjIns.City_ID),
			new SqlParameter("@in_State_ID", ObjIns.State_ID),
			new SqlParameter("@in_Country_ID", ObjIns.Country_ID),
			new SqlParameter("@in_Zip", ObjIns.zip),
			new SqlParameter("@in_HomePhone", ObjIns.HomePhone),
			new SqlParameter("@in_HomePhoneExtn", ObjIns.HPExtension),
			new SqlParameter("@in_HomePhoneLeaveMsg", ObjIns.HPLeaveMsg_Ind),
			new SqlParameter("@in_CellPhone", ObjIns.CellPhone),
			new SqlParameter("@in_CellPhoneLeaveMsg", null),
			new SqlParameter("@in_WorkPhone", ObjIns.WorkPhone),
			new SqlParameter("@in_WorkPhoneExtn", ObjIns.WPExtension),
			new SqlParameter("@in_WorkPhoneLeaveMsg", ObjIns.WPLeaveMsg_Ind),
			new SqlParameter("@in_Fax", ObjIns.Fax),
			new SqlParameter("@in_Reg_Username", ObjIns.UserName),
			new SqlParameter("@in_Reg_Password", ObjIns.Password),
			new SqlParameter("@in_CreatedBy", ObjIns.CreatedBy),
			new SqlParameter("@in_Loginhistory_ID", loginhistoryid),
			new SqlParameter("@in_Role_id", ObjIns.RoleType)
		};

                IDataParameter[] outparam = {
			new SqlParameter("Out_Msg", SqlDbType.VarChar, 500),
			new SqlParameter("@out_verification_User_ID", SqlDbType.VarChar, 500),
			new SqlParameter("@out_Login_ID", SqlDbType.VarChar, 500)
		};
                objdbwrapper.AddInParameters(paramval);
                objdbwrapper.AddInParameters(outparam);
                SqlCommand return_command = default(SqlCommand);
                return_command = objdbwrapper.ExecuteCommandDB("st_INS_verification_User", paramval, outparam, null);
                if (return_command.Parameters["@out_msg"].Value != DBNull.Value)
                {
                    Out_Msg = return_command.Parameters["@out_msg"].Value.ToString();
                }
                else
                {
                    Out_Msg = null;
                    if (return_command.Parameters["@out_verification_User_ID"].Value != DBNull.Value)
                    {
                        Out_verification_User_ID = Convert.ToInt32(return_command.Parameters["@out_verification_User_ID"].Value);
                    }
                    if (return_command.Parameters["@out_Login_ID"].Value != DBNull.Value)
                    {
                        Out_Login_ID = Convert.ToInt32(return_command.Parameters["@out_Login_ID"].Value);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Insert_verificationusers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<Admin_postajob> verificationusersView(int? loginID, int? verification_User_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader dataRead = default(SqlDataReader);

                IDataParameter[] iParameter = {
			new SqlParameter("@in_login_id", (loginID == 0 ? null : loginID)),
			new SqlParameter("@in_verification_User_ID", (verification_User_ID == 0 ? null : verification_User_ID))
		};

                objcommon.AddInParameters(iParameter);
                dataRead = objcommon.GetDataReader("st_get_verificationuser");
                List<Admin_postajob> objlist = new List<Admin_postajob>();

                while (dataRead.Read())
                {
                    Admin_postajob objusers = new Admin_postajob();

                    if (dataRead["Prefix"] != DBNull.Value)
                    {
                        objusers.Prefix = dataRead["Prefix"].ToString();
                    }
                    else
                    {
                        objusers.Prefix = null;
                    }
                    if (dataRead["FirstName"] != DBNull.Value)
                    {
                        objusers.FirstName = dataRead["FirstName"].ToString();
                    }
                    else
                    {
                        objusers.FirstName = null;
                    }
                    if (dataRead["MI"] != DBNull.Value)
                    {
                        objusers.MI = dataRead["MI"].ToString();
                    }
                    else
                    {
                        objusers.MI = null;
                    }
                    if (dataRead["LastName"] != DBNull.Value)
                    {
                        objusers.LastName = dataRead["LastName"].ToString();
                    }
                    else
                    {
                        objusers.LastName = null;
                    }
                    if (dataRead["Full_Name"] != DBNull.Value)
                    {
                        objusers.Full_Name = dataRead["Full_Name"].ToString();
                    }
                    else
                    {
                        objusers.Full_Name = null;
                    }
                    if (dataRead["Gender"] != DBNull.Value)
                    {
                        objusers.Gender = dataRead["Gender"].ToString();
                    }
                    else
                    {
                        objusers.Gender = null;
                    }
                    if (dataRead["WorkPhone"] != DBNull.Value)
                    {
                        objusers.WorkPhone = dataRead["WorkPhone"].ToString();
                    }
                    else
                    {
                        objusers.WorkPhone = null;
                    }
                    if (dataRead["WorkPhoneExtn"] != DBNull.Value)
                    {
                        objusers.WorkPhoneExtn = dataRead["WorkPhoneExtn"].ToString();
                    }
                    else
                    {
                        objusers.WorkPhoneExtn = null;
                    }
                    if (dataRead["WorkPhoneLeaveMsg"] != DBNull.Value)
                    {
                        objusers.WPLeaveMsg_Ind = dataRead["WorkPhoneLeaveMsg"].ToString();
                    }
                    else
                    {
                        objusers.WPLeaveMsg_Ind = null;
                    }
                    if (dataRead["CellPhone"] != DBNull.Value)
                    {
                        objusers.CellPhone = dataRead["CellPhone"].ToString();
                    }
                    else
                    {
                        objusers.CellPhone = null;
                    }
                    if (dataRead["Fax"] != DBNull.Value)
                    {
                        objusers.Fax = dataRead["Fax"].ToString();
                    }
                    else
                    {
                        objusers.Fax = null;
                    }
                    if (dataRead["Email"] != DBNull.Value)
                    {
                        objusers.Email = dataRead["Email"].ToString();
                    }
                    else
                    {
                        objusers.Email = null;
                    }
                    if (dataRead["Address1"] != DBNull.Value)
                    {
                        objusers.Address1 = dataRead["Address1"].ToString();
                    }
                    else
                    {
                        objusers.Address1 = null;
                    }
                    if (dataRead["Address2"] != DBNull.Value)
                    {
                        objusers.Address2 = dataRead["Address2"].ToString();
                    }
                    else
                    {
                        objusers.Address2 = null;
                    }
                    if (dataRead["Zip"] != DBNull.Value)
                    {
                        objusers.zip = dataRead["Zip"].ToString();
                    }
                    else
                    {
                        objusers.zip = null;
                    }
                    if (dataRead["Country_ID"] != DBNull.Value)
                    {
                        objusers.Country_ID = Convert.ToInt32(dataRead["Country_ID"]);
                    }
                    else
                    {
                        objusers.Country_ID = null;
                    }
                    if (dataRead["DCountry_Name"] != DBNull.Value)
                    {
                        objusers.Country = dataRead["DCountry_Name"].ToString();
                    }
                    else
                    {
                        objusers.Country = null;
                    }
                    if (dataRead["DState_Name"] != DBNull.Value)
                    {
                        objusers.statename = dataRead["DState_Name"].ToString();
                    }
                    else
                    {
                        objusers.statename = null;
                    }
                    if (dataRead["DCity_Name"] != DBNull.Value)
                    {
                        objusers.city = dataRead["DCity_Name"].ToString();
                    }
                    else
                    {
                        objusers.city = null;
                    }
                    if (dataRead["City_ID"] != DBNull.Value)
                    {
                        objusers.City_ID = Convert.ToInt32(dataRead["City_ID"]);
                    }
                    else
                    {
                        objusers.City_ID = 0;
                    }
                    if (dataRead["State_ID"] != DBNull.Value)
                    {
                        objusers.State_ID = Convert.ToInt32(dataRead["State_ID"]);
                    }
                    else
                    {
                        objusers.State_ID = null;
                    }
                    if (dataRead["Role_ID"] != DBNull.Value)
                    {
                        objusers.role_id = dataRead["Role_ID"].ToString();
                    }
                    else
                    {
                        objusers.role_id = null;
                    }
                    if (dataRead["Rolename"] != DBNull.Value)
                    {
                        objusers.Rolename = dataRead["Rolename"].ToString();
                    }
                    else
                    {
                        objusers.Rolename = null;
                    }
                    objlist.Add(objusers);
                }

                return objlist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "verificationusersView", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string verificationuserUpdate(Admin_postajob objusers)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParm = {
			new SqlParameter("@in_Login_ID", (objusers.Login_ID != null ? objusers.Login_ID : null)),
			new SqlParameter("@in_verification_User_ID", (objusers.verification_User_Id != null ? objusers.verification_User_Id : null)),
			new SqlParameter("@in_Prefix", (objusers.Prefix != null ? objusers.Prefix : null)),
			new SqlParameter("@in_FirstName", (objusers.FirstName != null ? objusers.FirstName : null)),
			new SqlParameter("@in_MI", (objusers.MI != null ? objusers.MI : null)),
			new SqlParameter("@in_LastName", (objusers.LastName != null ? objusers.LastName : null)),
			new SqlParameter("@in_Email", (objusers.Email != null ? objusers.Email : null)),
			new SqlParameter("@in_Address1", (objusers.Address1 != null ? objusers.Address1 : null)),
			new SqlParameter("@in_Address2", (objusers.Address2 != null ? objusers.Address2 : null)),
			new SqlParameter("@in_City_ID", objusers.City_ID),
			new SqlParameter("@in_State_ID", objusers.State_ID),
			new SqlParameter("@in_Country_ID", (objusers.Country_ID != null ? objusers.Country_ID : null)),
			new SqlParameter("@in_Zip", (objusers.zip != null ? objusers.zip : null)),
            new SqlParameter("@in_Gender",objusers.Gender),
			new SqlParameter("@in_CellPhone", (objusers.CellPhone != null ? objusers.CellPhone : null)),
			new SqlParameter("@in_WorkPhone", (objusers.WorkPhone != null ? objusers.WorkPhone : null)),
			new SqlParameter("@in_WorkPhoneExtn", (objusers.WorkPhoneExtn != null ? objusers.WorkPhoneExtn : null)),
			new SqlParameter("@in_WorkPhoneLeaveMsg", (objusers.WPLeaveMsg_Ind != null ? objusers.WPLeaveMsg_Ind : null)),
			new SqlParameter("@in_Fax", (objusers.Fax != null ? objusers.Fax : null)),
			new SqlParameter("@In_UpDatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};

                IDataParameter[] objoutParam = { new SqlParameter("@out_msg", SqlDbType.VarChar, 200) };
                objcommon.AddInParameters(objParm);
                objcommon.AddOutParameters(objoutParam);
                objcommon.ExecuteNonQuerySP("st_UPD_verification_user");

                string stroutmsg = null;
                if (objcommon.objdbCommandWrapper.Parameters["@out_msg"].Value != DBNull.Value)
                {
                    stroutmsg = objcommon.objdbCommandWrapper.Parameters["@out_msg"].Value.ToString();
                }
                return stroutmsg;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "verificationuserUpdate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override PracticeblHome UpdateDetails(int providerid, string EmailID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strParam = {
			new SqlParameter("@in_Provider_Id", providerid),
			new SqlParameter("@in_EmailID", EmailID),
			new SqlParameter("@In_UpdatedBy", HttpContext.Current.Session["UserID"])
		};
                objcommon.AddInParameters(strParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Provider_UPD_EmailInfo");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdateDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string InActive_verificationusers(Admin_postajob objstatus, int Loginhistory_id)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objParm = {
			new SqlParameter("@in_Login_ID", (objstatus.Login_ID != null ? objstatus.Login_ID : null)),
			new SqlParameter("@in_verification_User_ID", (objstatus.verification_User_Id != null ? objstatus.verification_User_Id : null)),
			new SqlParameter("@In_Status_Ind", (objstatus.Status_Ind != null ? objstatus.Status_Ind : null)),
			new SqlParameter("@In_UpDatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_id", Loginhistory_id)
		};
                objcommon.AddInParameters(objParm);
                objcommon.ExecuteNonQuerySP("st_UPD_verification_user");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InActive_verificationusers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<WebConfigSettings> Admin_List_WebConfigSettings(WebConfigSettings objWCS, ref int Total_Records)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Paramlist = {
			new SqlParameter("@In_Parameter_ID", (objWCS.Parameter_ID == 0 ? null : objWCS.Parameter_ID)),
			new SqlParameter("@in_NoofRecords", objWCS.NoofRecords),
			new SqlParameter("@in_PageNo", objWCS.PageNo),
			new SqlParameter("@In_OrderBy", objWCS.OrderBy),
			new SqlParameter("@In_OrderBYitem", objWCS.OrderByItem),
			new SqlParameter("@In_ParameterKey", objWCS.Parameter_Key)
		};
                objclsdbwraper.AddInParameters(Paramlist);
                returndata = objclsdbwraper.GetDataReader("Help_dbo.st_Application_GET_WebConfigSettings_Paging");
                List<WebConfigSettings> Adcollection = new List<WebConfigSettings>();
                while (returndata.Read())
                {
                    WebConfigSettings objwebconfigsettings = new WebConfigSettings(Convert.ToInt32(returndata["Parameter_ID"] != null ? returndata["Parameter_ID"] : 0), Convert.ToString(returndata["Parameter_Key"] != null ? returndata["Parameter_Key"] : null), Convert.ToString(returndata["Parameter_Value"] != null ? returndata["Parameter_Value"] : null), Convert.ToString(returndata["Inuse_Ind"] != null ? returndata["Inuse_Ind"] : null), Convert.ToString(returndata["Description"] != null ? returndata["Description"] : null), Convert.ToInt32(returndata["Project_ID"] != null ? returndata["Project_ID"] : 0), Convert.ToInt32(returndata["ProjectStatus_ID"] != null ? returndata["ProjectStatus_ID"] : 0));
                    Adcollection.Add(objwebconfigsettings);
                }
                if (returndata.NextResult() == true)
                {
                    if (returndata.Read())
                    {
                        Total_Records = Convert.ToInt32(returndata["Total_Records"]);
                    }
                }
                return Adcollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_List_WebConfigSettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override void Ad_UPD_WebConfigSettings(WebConfigSettings objWebConfigSettings)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@In_Project_ID", objWebConfigSettings.Project_ID),
			new SqlParameter("@In_Parameter_ID", objWebConfigSettings.Parameter_ID),
			new SqlParameter("@In_Parameter_Key", objWebConfigSettings.Parameter_Key),
			new SqlParameter("@In_Parameter_Value", objWebConfigSettings.Parameter_Value),
			new SqlParameter("@In_Inuse_Ind", objWebConfigSettings.InUse_Ind),
			new SqlParameter("@in_Description", objWebConfigSettings.Description),
			new SqlParameter("@In_ProjectStatus_ID", objWebConfigSettings.ProjectStatus_ID),
			new SqlParameter("@in_UpdatedBy", objWebConfigSettings.UpdatedBy),
			new SqlParameter("@in_Loginhistory_ID", objWebConfigSettings.Loginhistory_ID)
		};
                objdbwrapper.AddInParameters(objparm);
                objdbwrapper.ExecuteNonQuerySP("Help_dbo.st_Application_UPD_WebConfigsettings");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Ad_UPD_WebConfigSettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override void Ad_INS_WebConfigSettings(WebConfigSettings objWebConfigSettings)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@In_Project_ID", objWebConfigSettings.Project_ID),
			new SqlParameter("@In_Parameter_Key", objWebConfigSettings.Parameter_Key),
			new SqlParameter("@In_Parameter_Value", objWebConfigSettings.Parameter_Value),
			new SqlParameter("@In_Inuse_Ind", objWebConfigSettings.InUse_Ind),
			new SqlParameter("@in_Description", objWebConfigSettings.Description),
			new SqlParameter("@In_ProjectStatus_ID", objWebConfigSettings.ProjectStatus_ID),
			new SqlParameter("@in_CreatedBy", objWebConfigSettings.CreatedBy),
			new SqlParameter("@in_Loginhistory_ID", objWebConfigSettings.Loginhistory_ID)
		};


                objdbwrapper.AddInParameters(objparm);
                objdbwrapper.ExecuteNonQuerySP("Help_dbo.st_Application_INS_WebConfigsettings");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Ad_INS_WebConfigSettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        public override List<EmailMessageOption> ListEmailMessageOptions(EmailMessageOption EMO, ref int Total_records)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
            new SqlParameter("@In_EmailMessage_Option_ID", EMO.EmailMessage_Option_ID),
            new SqlParameter("@In_Email_OptionType_ID", EMO.EmailMessageOptionType_ID),
            new SqlParameter("@in_NoofRecords", EMO.NoOfRecords),
            new SqlParameter("@in_PageNo", EMO.pageNo),
            new SqlParameter("@In_OrderBy", EMO.OrderBy),
            new SqlParameter("@In_OrderBYitem", EMO.OrderByItem)
        };
                objclsdbwraper.AddInParameters(objparm);
                SqlDataReader ReturnData = default(SqlDataReader);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_EmailOptions_Paging");
                List<EmailMessageOption> objEmailMessageOptioncollection = new List<EmailMessageOption>();
                while (ReturnData.Read())
                {
                    EmailMessageOption objEmailMessageOption = default(EmailMessageOption);
                    objEmailMessageOption = new EmailMessageOption(Convert.ToInt32(ReturnData["EmailMsgOptionBody_ID"]), Convert.ToString(ReturnData["Message_type"]), Convert.ToString(ReturnData["Subcategory"]), Convert.ToString(ReturnData["Msg_Subject"]), Convert.ToString(ReturnData["Msg_Body"]));
                    objEmailMessageOptioncollection.Add(objEmailMessageOption);
                }
                if (ReturnData.NextResult())
                {
                    if (ReturnData.Read())
                    {
                        Total_records = Convert.ToInt32(ReturnData["Total_records"]);
                    }
                }
                return objEmailMessageOptioncollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ListEmailMessageOptions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<EmailMessageOption> ListEmail_Main_MessageOptions(EmailMessageOption EMO, ref int Total_records)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader ReturnData = default(SqlDataReader);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_Main_EmailMessage_Options");
                List<EmailMessageOption> objEmailMessageOptioncollection = new List<EmailMessageOption>();
                while (ReturnData.Read())
                {
                    EmailMessageOption objEmailMessageOption = default(EmailMessageOption);
                    objEmailMessageOption = new EmailMessageOption(Convert.ToInt32(ReturnData["EmailMessage_Option_ID"] != null ? ReturnData["EmailMessage_Option_ID"] : null), Convert.ToString(ReturnData["Category"] != null ? ReturnData["Category"] : null), Convert.ToString(ReturnData["Message_Title"] != null ? ReturnData["Message_Title"] : null));
                    objEmailMessageOptioncollection.Add(objEmailMessageOption);
                }

                if (ReturnData.NextResult())
                {
                    if (ReturnData.Read())
                    {
                        Total_records = Convert.ToInt32(ReturnData["emailmessagecount"]);
                    }
                }
                return objEmailMessageOptioncollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ListEmail_Main_MessageOptions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<ErrorsList> EList(ErrorsList objL)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader ReturnData = default(SqlDataReader);
                IDataParameter[] ObjParameters = {
             new SqlParameter("@In_Mobile_type", objL.Mobile_type),
			new SqlParameter("@in_Project_ID", objL.Project_ID),
			new SqlParameter("@in_Class_ID", objL.Class_ID),
			new SqlParameter("@in_Method_ID", objL.Method_ID),
			new SqlParameter("@in_Error_BeginDate", objL.Error_BeginDate),
			new SqlParameter("@in_Error_EndDate", objL.Error_EndDate),
			new SqlParameter("@in_NoofRecords", objL.NoofRecords),
			new SqlParameter("@in_PageNo", objL.pageNo),
			new SqlParameter("@In_OrderBy", objL.OrderBy),
			new SqlParameter("@In_OrderBYitem", objL.OrderByItem)
		};

                objcommon.AddInParameters(ObjParameters);
                ReturnData = objcommon.GetDataReader("Help_dbo.st_List_Errors_ASPX_rdPaging");

                List<ErrorsList> getError = new List<ErrorsList>();
                int SlNO = objL.NoofRecords * (objL.pageNo - 1);
                while (ReturnData.Read())
                {
                    SlNO += 1;
                    ErrorsList objE = new ErrorsList();
                    objE.CustomerRefID = Convert.ToString(ReturnData["CustomerRefID"] != null ? ReturnData["CustomerRefID"] : null);
                    objE.InsertDate = Convert.ToString(ReturnData["InsertDate"] != null ? ReturnData["InsertDate"] : null);
                    objE.Class_Name = Convert.ToString(ReturnData["Class_Name"] != null ? ReturnData["Class_Name"] : null);
                    objE.Method_Name = Convert.ToString(ReturnData["Method_Name"] != null ? ReturnData["Method_Name"] : null);
                    objE.ErrCategory = Convert.ToString(ReturnData["ErrCategory"] != null ? ReturnData["ErrCategory"] : null);
                    objE.ErrFile = Convert.ToString(ReturnData["ErrFile"] != null ? ReturnData["ErrFile"] : null);
                    objE.ErrLine = Convert.ToString(ReturnData["ErrLine"] != null ? ReturnData["ErrLine"] : null);
                    objE.Log_ID = Convert.ToString(ReturnData["Log_ID"] != null ? ReturnData["Log_ID"] : null);
                    objE.Exception_Name = Convert.ToString(ReturnData["Exception_Name"] != null ? ReturnData["Exception_Name"] : null);
                    objE.ErrSource = Convert.ToString(ReturnData["ErrSource"] != null ? ReturnData["ErrSource"] : null);
                    objE.ErrDescription = Convert.ToString(ReturnData["ErrDescription"] != null ? ReturnData["ErrDescription"] : null);
                    objE.Mobile_type = Convert.ToString(ReturnData["Mobile_Type"] != null ? ReturnData["Mobile_Type"] : null);
                    getError.Add(objE);

                }
                ReturnData.NextResult();
                if (ReturnData.HasRows == true)
                {
                    if (ReturnData.Read())
                    {
                        ErrorsList.TotalRecords = Convert.ToInt32(ReturnData[0]);
                    }

                }
                return getError;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "EList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<EmailMessageOption> ADMIN_List_Main_Email_OptionTypes()
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader ReturnData = default(SqlDataReader);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_List_Main_Email_OptionTypes");
                List<EmailMessageOption> objEmailMessageOptioncollection = new List<EmailMessageOption>();
                while (ReturnData.Read())
                {
                    EmailMessageOption objEmailMessageOption = default(EmailMessageOption);
                    objEmailMessageOption = new EmailMessageOption(Convert.ToInt32(ReturnData["EmailMessageOptionType_ID"]), Convert.ToString(ReturnData["Title"]));
                    objEmailMessageOptioncollection.Add(objEmailMessageOption);
                }
                return objEmailMessageOptioncollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ADMIN_List_Main_Email_OptionTypes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<EmailMessageOption> ADMIN_List_ExistingEmail_OptionTypes(EmailMessageOption OptionTypeID = null)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader ReturnData = default(SqlDataReader);
                IDataParameter[] objparm = { new SqlParameter("@in_Email_OptionType_ID", OptionTypeID.Email_OptionType_ID) };
                objclsdbwraper.AddInParameters(objparm);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_List_ExistingEmail_OptionTypes");
                List<EmailMessageOption> objEmailMessageOptioncollection = new List<EmailMessageOption>();
                while (ReturnData.Read())
                {
                    EmailMessageOption objEmailMessageOption = default(EmailMessageOption);
                    objEmailMessageOption = new EmailMessageOption(Convert.ToInt32(ReturnData["EmailMessage_Option_ID"]), Convert.ToString(ReturnData["Message_Title"]));
                    objEmailMessageOptioncollection.Add(objEmailMessageOption);
                }
                return objEmailMessageOptioncollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ADMIN_List_ExistingEmail_OptionTypes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override void InsertEmailMessageOption(EmailMessageOption EMP)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_EmailMessage_Option_ID", EMP.EmailMessage_Option_ID),
			new SqlParameter("@in_Msg_Subject", EMP.Msg_Subject),
			new SqlParameter("@in_Msg_Body", EMP.Msg_Body),
			new SqlParameter("@in_Msg_Footer", EMP.Msg_Footer),
			new SqlParameter("@in_BillingServiceIDS", EMP.ServiceIDs),
			new SqlParameter("@In_CreatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@In_Loginhistory_ID", EMP.Loginhistory_ID)
		};
                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_INS_EmailMessage");
            }

            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InsertEmailMessageOption", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override void UpdateEmailMessageOption(EmailMessageOption EMP)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_EmailMsgOptionBody_ID", EMP.EmailMsgOptionBody_ID),
			new SqlParameter("@in_EmailMessage_Option_ID", EMP.EmailMessage_Option_ID),
			new SqlParameter("@in_NewIDs", EMP.NewIDs),
			new SqlParameter("@in_Msg_Subject", EMP.Msg_Subject),
			new SqlParameter("@in_Msg_Body", EMP.Msg_Body),
			new SqlParameter("@in_Msg_Footer", EMP.Msg_Footer),
			new SqlParameter("@in_BillingServiceIDS", EMP.ServiceIDs),
			new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@IN_Loginhistory_ID", EMP.Loginhistory_ID)
		};
                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_UPD_EmailMessage");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdateEmailMessageOption", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override void InsertMainEmailMessageOption(int Email_OptionType_ID, string Message_Title)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_Email_OptionType_ID", Email_OptionType_ID),
			new SqlParameter("@in_Message_Title", Message_Title),
			new SqlParameter("@in_CreatedBy", HttpContext.Current.Session["userid"])
		};

                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_INS_Main_EmailOptionType");
            }

            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InsertmainEmailMessageOption", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override void UpdatemainEmailMessageOption(int EmailMessage_Option_ID, string Message_Title)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = {
			new SqlParameter("@in_EmailMessage_Option_ID", EmailMessage_Option_ID),
			new SqlParameter("@in_Message_Title", Message_Title),
			new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userid"])
		};
                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_UPD_Main_EmailOptionType");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdatemainEmailMessageOption", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override EmailMessageOption GetEmail_Main_MessageOptionbyID(int ID)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                IDataParameter[] objparm = { new SqlParameter("@In_EmailMessage_Option_ID", ID) };
                objclsdbwraper.AddInParameters(objparm);
                SqlDataReader ReturnData = default(SqlDataReader);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_Main_EmailMessage_Options");
                EmailMessageOption objEmailMessageOption = new EmailMessageOption();
                if (ReturnData.Read())
                {
                    objEmailMessageOption = new EmailMessageOption(Convert.ToInt32(ReturnData["Email_OptionType_ID"]), Convert.ToString(ReturnData["Message_Title"]));
                }
                return objEmailMessageOption;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetEmail_Main_MessageOptionbyID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<ErrorsList> DDL_CLS(ErrorsList objEL)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Obj_Param = { new SqlParameter("@in_Project_ID", objEL.Project_ID) };
                objcommon.AddInParameters(Obj_Param);
                returndata = objcommon.GetDataReader("Help_dbo.st_List_Classes");
                List<ErrorsList> getErr = new List<ErrorsList>();
                while (returndata.Read())
                {
                    ErrorsList objE = new ErrorsList(Convert.ToString(returndata["Class_ID"] != null ? returndata["Class_ID"] : null), Convert.ToString(returndata["Class_Name"] != null ? returndata["Class_Name"] : null));

                    getErr.Add(objE);
                }
                return getErr;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "DDL_CLS", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<ErrorsList> DDL_MTHDS(ErrorsList objM)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Obj_Param = { new SqlParameter("@in_Class_ID", objM.Class_ID) };
                objcommon.AddInParameters(Obj_Param);
                returndata = objcommon.GetDataReader("Help_dbo.st_List_ExistingMethods");
                List<ErrorsList> getErr = new List<ErrorsList>();
                while (returndata.Read())
                {
                    ErrorsList objError = new ErrorsList(null, Convert.ToString(returndata["Method_Name"] != null ? returndata["Method_Name"] : null), Convert.ToString(returndata["Method_ID"] != null ? returndata["Method_ID"] : null));

                    getErr.Add(objError);

                }
                return getErr;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "DDL_MTHDS", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Login_history> Admin_List_Login_history(Login_history objloginhistory, ref int Total_Records, string Rolename)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Paramlist = {
                 new SqlParameter("@In_Mobile_type", objloginhistory.Mobile_type),
			new SqlParameter("@In_LoginHistory_ID", (objloginhistory.LoginHistory_ID == 0 ? null : objloginhistory.LoginHistory_ID)),
			new SqlParameter("@in_NoofRecords", objloginhistory.NoofRecords),
			new SqlParameter("@in_PageNo", objloginhistory.PageNo),
			new SqlParameter("@In_OrderBy", objloginhistory.OrderBy),
			new SqlParameter("@In_OrderBYitem", objloginhistory.OrderByItem),
			new SqlParameter("@in_BeginDate", objloginhistory.BeginDate),
			new SqlParameter("@in_EndDate", objloginhistory.EndDate),
			new SqlParameter("@in_Project_ID", objloginhistory.Project_ID),
			new SqlParameter("@In_LogIn_ID", (objloginhistory.Login_ID != null ? objloginhistory.Login_ID : null)),
			new SqlParameter("@in_userName", (objloginhistory.Username != null ? objloginhistory.Username : null)),
			new SqlParameter("@In_IPAddress", (objloginhistory.IPAddress != null ? objloginhistory.IPAddress : null)),
			new SqlParameter("@in_RoleName", (Rolename != null ? Rolename : null))
		};
                objclsdbwraper.AddInParameters(Paramlist);
                returndata = objclsdbwraper.GetDataReader("Help_dbo.st_Admin_LIST_LoginHsitory_RdPaging");
                List<Login_history> Adcollection = new List<Login_history>();

                while (returndata.Read())
                {
                    string strrolename = null;
                    string strusername = null;
                    string strpassword = null;
                    string stripaddress = null;
                    string begindate = null;
                    string enddate = null;
                    if (returndata["RoleName"] != null)
                    {
                        strrolename = Convert.ToString(returndata["RoleName"]);
                    }
                    else
                    {
                        strrolename = null;
                    }
                    if (returndata["Username"] != null)
                    {
                        strusername = Convert.ToString(returndata["Username"]);
                    }
                    else
                    {
                        strusername = null;
                    }
                    if (returndata["Password"] != null)
                    {
                        strpassword = Convert.ToString(returndata["Password"]);
                    }
                    else
                    {
                        strpassword = null;
                    }
                    if (returndata["IPAddress"] != DBNull.Value)
                    {
                        stripaddress = Convert.ToString(returndata["IPAddress"]);
                    }
                    else
                    {
                        stripaddress = "-";
                    }
                    if (returndata["LogIn_DateTime"] != null)
                    {
                        begindate = Convert.ToString(returndata["LogIn_DateTime"]);
                    }
                    else
                    {
                        begindate = null;
                    }
                    if (returndata["LogOut_DateTime"] != null)
                    {
                        enddate = Convert.ToString(returndata["LogOut_DateTime"]);
                    }
                    else
                    {
                        enddate = null;
                    }
                    Login_history objlogin = new Login_history(Convert.ToInt32(returndata["LoginHistory_ID"]), strusername, strpassword, strrolename, stripaddress, begindate, enddate, Convert.ToBoolean(returndata["LoginSuccess_Ind"]), Convert.ToInt32(returndata["Project_ID"]));
                    if (returndata["Mobile_type"].ToString() != "")
                    {
                        objlogin.Mobile_type = Convert.ToString(returndata["Mobile_type"]);
                    }
                    else
                    {
                        objlogin.Mobile_type = null;
                    }
                    if (returndata["LoginSuccess_Ind"].ToString() != "")
                    {
                        objlogin.logsuccess = Convert.ToString(returndata["LoginSuccess_Ind"]);
                    }
                    else
                    {
                        objlogin.logsuccess = null;
                    }
                    if (returndata["R_NO"].ToString() != "")
                    {
                        objlogin.Sino = Convert.ToString(returndata["R_NO"]);
                    }
                    else
                    {
                        objlogin.Sino = null;
                    }
                    Adcollection.Add(objlogin);
                }
                if (returndata.NextResult() == true)
                {
                    if (returndata.Read())
                    {
                        Total_Records = Convert.ToInt32(returndata["Total_Records"]);
                    }
                }
                return Adcollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_List_Login_history", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<pagenumber> admin_listpagenumbers(pagenumber objlistpages, ref int Total_Records)
        {
            try
            {
                int slNo = 0;
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Paramlist = {
			new SqlParameter("@in_Page_ID", (objlistpages.Page_ID!= 0 ? objlistpages.Page_ID:null)),
			new SqlParameter("@in_Application_ID", (objlistpages.Application_ID!= 0 ? objlistpages.Application_ID: null)),
			new SqlParameter("@in_NoofRecords", objlistpages.NoOfRecords),
			new SqlParameter("@in_PageNo", objlistpages.PageNo),
			new SqlParameter("@In_OrderBy", objlistpages.OrderBy),
			new SqlParameter("@In_OrderBYitem", objlistpages.OrderBYitem)
		};
                objclsdbwraper.AddInParameters(Paramlist);
                returndata = objclsdbwraper.GetDataReader("Help_dbo.st_Page_GET_PageNumber_Paging");
                List<pagenumber> Adcollection = new List<pagenumber>();
                slNo = ((objlistpages.NoOfRecords) * (objlistpages.PageNo - 1)) + 1;
                while (returndata.Read())
                {
                    pagenumber objlist = new pagenumber();
                    if (returndata["Page_ID"] != null)
                    {
                        objlist.Page_ID = Convert.ToInt32(returndata["Page_ID"]);
                    }
                    if (returndata["PageNo"] != null)
                    {
                        objlist.PageNo = Convert.ToInt32(returndata["PageNo"]);
                    }
                    objlist.slno = slNo;
                    if (returndata["PagePath"] != null)
                    {
                        objlist.PagePath = Convert.ToString(returndata["PagePath"]);
                    }
                    if (returndata["Notes"] != null)
                    {
                        objlist.Notes = Convert.ToString(returndata["Notes"]);
                    }
                    Adcollection.Add(objlist);
                    slNo += 1;
                }
                if (returndata.NextResult() == true)
                {
                    if (returndata.Read())
                    {
                        Total_Records = Convert.ToInt32(returndata["Total_Records"]);
                    }
                }
                return Adcollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "admin_listpagenumbers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override void updatepagelist(pagenumber objview, ref string outmsg)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_Page_ID", objview.Page_ID),
			new SqlParameter("@in_PagePath", objview.PagePath),
			new SqlParameter("@in_Notes", objview.Notes),
			new SqlParameter("@in_Application_ID", objview.Application_ID),
			new SqlParameter("@in_Updatedby", HttpContext.Current.Session["userid"])
		};

                IDataParameter[] outparam = { new SqlParameter("Out_Msg", SqlDbType.VarChar, 100) };
                objdbwrapper.AddInParameters(outparam);
                objdbwrapper.AddInParameters(paramlist);
                SqlCommand return_command = default(SqlCommand);
                return_command = objdbwrapper.ExecuteCommandDB("Help_dbo.st_Page_UPD_Page", paramlist, outparam, null);
                if ((return_command.Parameters["@Out_Msg"].Value) != null)
                {
                    outmsg = Convert.ToString(return_command.Parameters["@Out_Msg"].Value);
                }
                else
                {
                    outmsg = null;
                }
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "updatepagelist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        public override void inserpagenumber(pagenumber objview, ref string outmsg)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_PagePath", objview.PagePath),
			new SqlParameter("@in_Notes", objview.Notes),
			new SqlParameter("@in_Application_ID", objview.Application_ID),
			new SqlParameter("@in_CreatedBy", objview.CreatedBy)
		};
                IDataParameter[] outparam = { new SqlParameter("Out_Msg", SqlDbType.VarChar, 100) };
                objdbwrapper.AddInParameters(paramlist);
                objdbwrapper.AddInParameters(outparam);
                SqlCommand return_command = default(SqlCommand);
                return_command = objdbwrapper.ExecuteCommandDB("Help_dbo.st_Page_INS_PageNumber", paramlist, outparam, null);
                if ((return_command.Parameters["@Out_Msg"].Value) != null)
                {
                    outmsg = Convert.ToString(return_command.Parameters["@Out_Msg"].Value);
                }
                else
                {
                    outmsg = null;
                }
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "inserpagenumber", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override void del_listpagenumbers(int Page_ID, int Application_ID)
        {
            try
            {
                clsDBWrapper objwraper = new clsDBWrapper();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_Page_ID", Page_ID),
			new SqlParameter("@in_Application_ID", Application_ID),
			new SqlParameter("@in_DeletedBy", HttpContext.Current.Session["userid"])
		};
                objwraper.AddInParameters(paramlist);
                objwraper.ExecuteNonQuerySP("Help_dbo.st_Page_DEL_Page");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "del_listpagenumbers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }


        }
        public override List<pagenumber> viewpagelist(pagenumber objview)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                List<pagenumber> objpage = new List<pagenumber>();
                IDataParameter[] paramlist = {
			new SqlParameter("@in_Page_ID", objview.Page_ID),
			new SqlParameter("@in_Application_ID", objview.Application_ID)
		};
                objclsdbwraper.AddInParameters(paramlist);
                SqlDataReader objReader = objclsdbwraper.GetDataReader("Help_dbo.st_Page_GET_PageNumber");
                pagenumber objlist = new pagenumber();
                if (objReader.Read())
                {
                    if (objReader["PagePath"] != null)
                    {
                        objlist.PagePath = Convert.ToString(objReader["PagePath"]);
                    }
                    else
                    {
                        objlist.PagePath = null;
                    }
                    if (objReader["Notes"] != null)
                    {
                        objlist.Notes = Convert.ToString(objReader["Notes"]);
                    }
                    else
                    {
                        objlist.Notes = null;
                    }
                    objpage.Add(objlist);
                }
                return objpage;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "viewpagelist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<pagenumber> dropdownlist(Nullable<int> Section_ID, string SearchBy)
        {
            try
            {
                clsDBWrapper objdbwrapper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] paramlist1 = {
			new SqlParameter("@in_Section_ID", null),
			new SqlParameter("@in_SearchBy", null)
		};
                objdbwrapper.AddInParameters(paramlist1);
                returndata = objdbwrapper.GetDataReader("Help_dbo.st_Security_LIST_DisplaySections");
                List<pagenumber> adcoll = new List<pagenumber>();
                while (returndata.Read())
                {
                    pagenumber objdropdown = default(pagenumber);
                    objdropdown = new pagenumber(Convert.ToInt32(returndata["Section_ID"] != null ? returndata["Section_ID"] : null), Convert.ToString(returndata["Sectionname"] != null ? returndata["Sectionname"] : null));
                    adcoll.Add(objdropdown);
                }
                return adcoll;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "dropdownlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet Admin_List_Site_history(string IPAddress, string ProjectName, string StateName, string CityName, string CountryName, string OrderBy, string OrderBYItem, int PageNo, int NoOfrec, string FromDate,
        string ToDate, ref int Total_Records)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = {
			new SqlParameter("@In_IPAddress", IPAddress),
			new SqlParameter("@In_ProjectName", ProjectName),
			new SqlParameter("@In_StateName", StateName),
			new SqlParameter("@In_CityName", CityName),
			new SqlParameter("@In_CountryName", CountryName),
			new SqlParameter("@In_OrderBy", OrderBy),
			new SqlParameter("@In_OrderBYItem", OrderBYItem),
			new SqlParameter("@in_PageNo", PageNo),
			new SqlParameter("@In_NoOfrec", NoOfrec),
			new SqlParameter("@in_Fromdate", FromDate),
			new SqlParameter("@in_todate", ToDate)
		};
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.St_Admin_List_BrowsingDetails_Rdpaging");
                if (dataset.Tables[1].Rows.Count > 0)
                {
                    Total_Records = Convert.ToInt32(dataset.Tables[1].Rows[0]["TotalRec"].ToString());
                }
                return dataset;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_List_Site_history", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }

        public override DataSet Admin_List_Site_history_Mobile(string FromDate, string ToDate, string orderby, string orderbyitem)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Fromdate", FromDate),
			new SqlParameter("@in_todate", ToDate),
            new SqlParameter("@in_orderby", orderby),
            new SqlParameter("@in_orderbyitem", orderbyitem)
		};
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.St_list_mobile_site_visting_history");
                //if (dataset.Tables[1].Rows.Count > 0)
                //{
                //    Total_Records = Convert.ToInt32(dataset.Tables[1].Rows[0]["TotalRec"].ToString());
                //}
                return dataset;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_List_Site_history_Mobile", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet Admin_Get_Site_history_Mobile(int Mobileowner_id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Mobileowner_id", Mobileowner_id)
		};
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.St_get_mobile_site_visting_history");
                //if (dataset.Tables[1].Rows.Count > 0)
                //{
                //    Total_Records = Convert.ToInt32(dataset.Tables[1].Rows[0]["TotalRec"].ToString());
                //}
                return dataset;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_Get_Site_history_Mobile", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public List<BLL.Billing.CCProcess> ccProcessResponse(BLL.Billing.CCProcess objResponse)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams = { 
                                         new SqlParameter ("@IN_STARTDATE",objResponse.STARTDATE),
                                         new SqlParameter("@IN_ENDDATE",objResponse.ENDDATE),
                                         new SqlParameter("@in_StatusCode",objResponse.StatusCode),
                                         new SqlParameter("@In_OrderbyItem",objResponse.OrderbyItem),
                                         new SqlParameter("@In_Orderby",objResponse.Orderby),
                                         new SqlParameter("@in_NoofRecords",objResponse.NoofRecords),
                                         new SqlParameter("@in_PageNo",objResponse.PageNo),
                                         new SqlParameter("@in_practicename",objResponse.practicename)
                                         };
                objcommon.AddInParameters(insparams);
                SqlDataReader drResponse = default(SqlDataReader);
                drResponse = objcommon.GetDataReader("Help_dbo.St_CCProcess_List_Responses_RdPaging");
                List<BLL.Billing.CCProcess> listRespose = new List<BLL.Billing.CCProcess>();
                while (drResponse.Read())
                {
                    BLL.Billing.CCProcess ccList = new BLL.Billing.CCProcess();
                    if (!DBNull.Value.Equals(drResponse["FromReferenceType"]))
                    {
                        ccList.FromReferenceType = Convert.ToString(drResponse["FromReferenceType"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["FullName"]))
                    {
                        ccList.FullName = Convert.ToString(drResponse["FullName"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["ToReferenceType"]))
                    {
                        ccList.ToReferenceType = Convert.ToString(drResponse["ToReferenceType"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["CreatedOn"]))
                    {
                        ccList.CreatedOn = Convert.ToString(drResponse["CreatedOn"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["Transaction_ID"]))
                    {
                        ccList.Transaction_ID = Convert.ToInt32(drResponse["Transaction_ID"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["Response_Data"]))
                    {
                        ccList.Response_Data = Convert.ToString(drResponse["Response_Data"]);
                    }
                    if (!DBNull.Value.Equals(drResponse["Status"]))
                    {
                        ccList.Status = Convert.ToString(drResponse["Status"]);
                    }

                    listRespose.Add(ccList);
                }
                drResponse.NextResult();
                if (drResponse.HasRows)
                {
                    while (drResponse.Read())
                    {
                        BLL.Billing.CCProcess.Totalnoofrecords = Convert.ToInt32(drResponse["TOTALRECORDS"]);

                    }
                }
                return listRespose;

            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ccProcessResponse", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet viewRequest(int? tranId)
        {
            try
            {
                clsCommonFunctions objccView = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_Transaction_ID",tranId)
                                        };
                objccView.AddInParameters(insparam);
                DataSet dsView = new DataSet();
                dsView = objccView.GetDataSet("Help_dbo.st_get_creditcard_ReqResponseDate");
                return dsView;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "viewRequest", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<BLL.Billing.CCProcess> Get_AdminCreditCardTransaction(BLL.Billing.CCProcess objcclist)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Practice_ID", objcclist.strPracticeID),
                                           new SqlParameter("@In_Patient_ID",null),
                                           new SqlParameter("@in_CardHolder_name",null),
                                           new SqlParameter("@In_CCNumber",null),
                                           new SqlParameter("@In_CCType",null),
                                           new SqlParameter("@In_CCStartDate",objcclist.STARTDATE),
                                           new SqlParameter("@In_CCEndDate",objcclist.ENDDATE),
                                           new SqlParameter("@in_NoofRecords",objcclist.NoofRecords),
                                           new SqlParameter("@in_PageNo",objcclist.PageNo),
                                           new SqlParameter("@in_OrderByItem",objcclist.OrderbyItem),
                                           new SqlParameter("@in_OrderBy",objcclist.Orderby),
                                           new SqlParameter("@In_firstname",null),
                                           new SqlParameter("@In_Lastname",null),
                                           new SqlParameter("@In_practicename",objcclist.practicename),
                                           new SqlParameter("@in_trans_type",null),
                                           new SqlParameter("@in_ToReferenceType_ID","2")
                                           
                                      };
                objcommon.AddInParameters(insparam);
                SqlDataReader drcclist = default(SqlDataReader);
                drcclist = objcommon.GetDataReader("Help_dbo.st_CreditCard_Search_AdminTransactions_RDPaging");
                List<BLL.Billing.CCProcess> listccLedger = new List<BLL.Billing.CCProcess>();
                while (drcclist.Read())
                {
                    BLL.Billing.CCProcess ccList = new BLL.Billing.CCProcess();

                    if (!DBNull.Value.Equals(drcclist["Tran_Date"]))
                    {
                        ccList.Tran_Date = Convert.ToString(drcclist["Tran_Date"]);
                        string s = Convert.ToDateTime(drcclist["Tran_Date"]).ToShortDateString();
                        string s1 = Convert.ToDateTime(DateTime.Now.AddDays(-60)).ToShortDateString();
                        DateTime dt = Convert.ToDateTime(s).Date;
                        DateTime dt1 = Convert.ToDateTime(s1).Date;
                        if (dt < dt1)
                        {
                            ccList.Refund_ind = "N";
                        }
                        else
                        {
                            ccList.Refund_ind = "Y";
                        }
                    }

                    if (!DBNull.Value.Equals(drcclist["CardHolder_name"]))
                    {
                        ccList.ccholdername = Convert.ToString(drcclist["CardHolder_name"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["FromUserName"]))
                    {
                        ccList.Paidby = Convert.ToString(drcclist["FromUserName"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["ToUserName"]))
                    {
                        ccList.PaidTo = Convert.ToString(drcclist["ToUserName"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["trans_type"]))
                    {
                        string strTran_Type = Convert.ToString(drcclist["trans_type"]);
                        if (strTran_Type == "R")
                        {
                            if (!DBNull.Value.Equals(drcclist["FromUserName"]))
                            {
                                ccList.PaidTo = Convert.ToString(drcclist["FromUserName"]);
                            }
                            if (!DBNull.Value.Equals(drcclist["ToUserName"]))
                            {
                                ccList.Paidby = Convert.ToString(drcclist["ToUserName"]);
                            }
                        }
                    }
                    if (!DBNull.Value.Equals(drcclist["CCType"]))
                    {
                        ccList.strStrCardType = Convert.ToString(drcclist["CCType"]);
                    }

                    if (!DBNull.Value.Equals(drcclist["CCNumber"]))
                    {
                        ccList.strx_card_num = null;
                    }
                    if (!DBNull.Value.Equals(drcclist["Amount"]))
                    {
                        ccList.transactionAmount = string.Format("{0:c}", drcclist["Amount"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["RefundedAmount"]))
                    {
                        ccList.refundAmount = string.Format("{0:c}", drcclist["RefundedAmount"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["Transaction_ID"]))
                    {
                        ccList.Transaction_ID = Convert.ToInt32(drcclist["Transaction_ID"]);
                    }
                    if (drcclist["IsRefundable_Ind"].ToString() == "Y")
                    {
                        int Amount = Convert.ToInt32(drcclist["Amount"]);
                        int RefundedAmount = Convert.ToInt32(drcclist["RefundedAmount"]);
                        int strtotamt = Convert.ToInt32(Amount + RefundedAmount);
                        ccList.totalamount = string.Format("{0:c}", strtotamt);
                    }
                    else
                    {
                        ccList.totalamount = null;
                    }

                    if (!DBNull.Value.Equals(drcclist["Status_code"]))
                    {
                        ccList.StatusCode = Convert.ToInt32(drcclist["Status_code"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["IsRefundable_Ind"]))
                    {
                        ccList.IsRefundable_Ind = Convert.ToString(drcclist["IsRefundable_Ind"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["PaymentTransID"]))
                    {
                        ccList.PaymentTransID = Convert.ToInt32(drcclist["PaymentTransID"]);
                    }
                    if (!DBNull.Value.Equals(drcclist["paypaltransactionid"]))
                    {
                        ccList.paypaltransactionid = drcclist["paypaltransactionid"].ToString();
                    }
                    if (!DBNull.Value.Equals(drcclist["paypalsaletransactionid"]))
                    {
                        ccList.paypalsaletransactionid = Convert.ToString(drcclist["paypalsaletransactionid"]);
                    }
                    listccLedger.Add(ccList);
                }
                drcclist.NextResult();
                if (drcclist.HasRows)
                {
                    while (drcclist.Read())
                    {
                        BLL.Billing.CCProcess.Totalnoofrecords = Convert.ToInt32(drcclist["TOTAL RECORDS"]);
                    }
                }
                return listccLedger;

            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_AdminCreditCardTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object upd_fieldname_description(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_fieldname", list.Fieldname),
			new SqlParameter("@in_FieldDescription", list.descriptionfield),
			new SqlParameter("@in_status_ind", list.statusindfield),
			new SqlParameter("@in_Fieldid", list.Fieldid)
		};
                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.st_upd_fieldname_description");
                return null;

            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "upd_fieldname_description", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override object Ins_module_field_description(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Module_id", list.Moduleid),
			new SqlParameter("@in_Modulename", list.Modulename),
			new SqlParameter("@in_Mod_Description", list.moduledescription),
			new SqlParameter("@in_Fieldname", list.Fieldname),
			new SqlParameter("@in_FieldDescription", list.descriptionfield),
			new SqlParameter("@in_Status_ind", list.statusindfield)
		};
                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.st_ins_module_field_description");
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Ins_module_field_description", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override List<Referrals> BindCaregories()
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader ReturnData = default(SqlDataReader);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_Admin_list_ArticlesCategories");
                List<Referrals> objCategorycollection = new List<Referrals>();
                while (ReturnData.Read())
                {
                    Referrals objCategoryOption = default(Referrals);
                    objCategoryOption = new Referrals(Convert.ToInt32(ReturnData["ArticleCategory_ID"]), Convert.ToString(ReturnData["Category_Title"]));
                    objCategorycollection.Add(objCategoryOption);
                }
                return objCategorycollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "BindCaregories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Referrals> BindArticalIndex(Referrals OptionTypeID = null)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader ReturnData = default(SqlDataReader);
                IDataParameter[] objparm = { new SqlParameter("@in_ParentCategory_ID", OptionTypeID.ParentCategory_ID) };
                objclsdbwraper.AddInParameters(objparm);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.st_Admin_list_ArticlesCategories");
                List<Referrals> objArticlecollection = new List<Referrals>();
                while (ReturnData.Read())
                {
                    Referrals objArticleOption = default(Referrals);
                    objArticleOption = new Referrals(Convert.ToInt32(ReturnData["ArticleCategory_ID"]), Convert.ToString(ReturnData["Category_Title"]));
                    objArticlecollection.Add(objArticleOption);
                }
                return objArticlecollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "BindArticalIndex", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<MediaInfo> GetMediaInfo(MediaInfo objMediaInfo)
        {

            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();

                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			new SqlParameter("@In_NoOfRecords", objMediaInfo.NoOfrecords),
			new SqlParameter("@In_MediaTitle", objMediaInfo.MediaTitle),
			new SqlParameter("@In_OrderBy", objMediaInfo.OrderBy),
			new SqlParameter("@In_OrderBYitem", objMediaInfo.OrderByItem),
			new SqlParameter("@In_PageNo", objMediaInfo.PageNO),
			new SqlParameter("@in_exceptids", (!string.IsNullOrEmpty(objMediaInfo.strexplictids) ? objMediaInfo.strexplictids : null))
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Admin_LIST_HelpMedia_RDPAGING");

                List<MediaInfo> MediaInfo1 = new List<MediaInfo>();
                int SINo = objMediaInfo.NoOfrecords * (objMediaInfo.PageNO - 1);

                while (objread.Read())
                {
                    SINo += 1;

                    MediaInfo objMeInfo = new MediaInfo();
                    objMeInfo.SINo = Convert.ToInt32(SINo);
                    objMeInfo.Helpmedia_ID = (objread["Helpmedia_ID"] == DBNull.Value ? null : objread["Helpmedia_ID"].ToString());
                    objMeInfo.Reference_ID = (objread["Reference_ID"] == DBNull.Value ? 0 : Convert.ToInt32(objread["Reference_ID"]));
                    objMeInfo.ReferenceType_ID = (objread["ReferenceType_ID"] == DBNull.Value ? 0 : Convert.ToInt32(objread["ReferenceType_ID"]));
                    objMeInfo.Title = (objread["Title"] == DBNull.Value ? null : objread["Title"].ToString());
                    objMeInfo.Description = (objread["Description"] == DBNull.Value ? null : objread["Description"].ToString());
                    objMeInfo.Duration = (objread["Duration"] == DBNull.Value ? null : objread["Duration"].ToString());
                    objMeInfo.FileType = (objread["FileType"] == DBNull.Value ? null : objread["FileType"].ToString());
                    objMeInfo.FilePath = (objread["FilePath"] == DBNull.Value ? null : objread["FilePath"].ToString());
                    objMeInfo.FilePath_Encrypted = (objread["FilePath_Encrypted"] == DBNull.Value ? null : objread["FilePath_Encrypted"].ToString());
                    objMeInfo.ImagepathSuffix = (objread["ImagepathSuffix"] == DBNull.Value ? null : objread["ImagepathSuffix"].ToString());
                    objMeInfo.IsDisplayToPractice = (objread["IsDisplayToPractice"] == DBNull.Value ? null : objread["IsDisplayToPractice"].ToString());
                    objMeInfo.CreadtedOn = (objread["CreatedOn"] == DBNull.Value ? null : objread["CreatedOn"].ToString());
                    MediaInfo1.Add(objMeInfo);
                }
                objread.NextResult();

                while (objread.Read())
                {
                    MediaInfo.TotalRecords = Convert.ToInt32(objread["TotalRecords"]);
                }
                return MediaInfo1;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetMediaInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<MediaInfo> GetVideoInfo(MediaInfo objMediaInfo)
        {

            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                List<MediaInfo> MediaInfo1 = new List<MediaInfo>();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			new SqlParameter("@In_HelpMedia_ID", objMediaInfo.Helpmedia_ID)
			
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Get_Helpmedia");


                while (objread.Read())
                {
                    MediaInfo objMeInfo = new MediaInfo();
                    objMeInfo.FileType = (objread["FileType"] == DBNull.Value ? null : objread["FileType"].ToString());
                    objMeInfo.FilePath = (objread["FilePath"] == DBNull.Value ? null : objread["FilePath"].ToString());
                    objMeInfo.FilePath_Encrypted = (objread["FilePath_Encrypted"] == DBNull.Value ? null : objread["FilePath_Encrypted"].ToString());


                    MediaInfo1.Add(objMeInfo);
                }
                objread.NextResult();
                return MediaInfo1;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetVideoInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string InsertMediaInfo(MediaInfo ObjInsMediaInfo, ref string out_Msg, ref string Loc_ImagePathSuffix)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] inparmlist = {
			new SqlParameter("@in_Reference_ID", ObjInsMediaInfo.Reference_ID),
			new SqlParameter("@in_ReferenceType_ID", ObjInsMediaInfo.ReferenceType_ID),
			new SqlParameter("@in_Title", ObjInsMediaInfo.Title),
			new SqlParameter("@in_Duration", ObjInsMediaInfo.Duration),
			new SqlParameter("@in_Description", ObjInsMediaInfo.Description),
			new SqlParameter("@in_FileType", ObjInsMediaInfo.FileType),
			new SqlParameter("@in_FilePath", ObjInsMediaInfo.FilePath),
			new SqlParameter("@in_FilePath_Encrypted", ObjInsMediaInfo.FilePath_Encrypted),
			new SqlParameter("@In_IsPracticeMedia_Ind ", ObjInsMediaInfo.IsPracticeMedia_Ind),
			new SqlParameter("@in_IsDisplayToPractice", ObjInsMediaInfo.IsDisplayToPractice),
			new SqlParameter("@In_CreatedBy", HttpContext.Current.Session["userid"])
		};
                IDataParameter[] outparamlist = {
			new SqlParameter("@Out_Msg", SqlDbType.NVarChar, 100),
			new SqlParameter("@Loc_ImagePathSuffix", SqlDbType.VarChar, 1000)
		};

                objclscommon.AddInParameters(inparmlist);
                objclscommon.AddOutParameters(outparamlist);
                objclscommon.ExecuteNonQuerySP("Help_dbo.st_Admin_INS_HelpMedia");
                if (objclscommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != DBNull.Value)
                {
                    out_Msg = objclscommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();

                }
                if (objclscommon.objdbCommandWrapper.Parameters["@Loc_ImagePathSuffix"].Value != DBNull.Value)
                {
                    Loc_ImagePathSuffix = objclscommon.objdbCommandWrapper.Parameters["@Loc_ImagePathSuffix"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InsertMediaInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override List<MediaInfo> GetNewMediaInfo(MediaInfo ObjNewMediaInfo)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = { new SqlParameter("@In_Helpmedia_ID", ObjNewMediaInfo.Helpmedia_ID) };

                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_HelpMedia_GET_HelpMedia");

                List<MediaInfo> MediaInfo = new List<MediaInfo>();

                while (objread.Read())
                {
                    MediaInfo objNewMeInfo = new MediaInfo();
                    objNewMeInfo.Title = (objread["Title"] == DBNull.Value ? null : objread["Title"].ToString());
                    objNewMeInfo.Description = (objread["Description"] == DBNull.Value ? null : objread["Description"].ToString());
                    objNewMeInfo.Duration = (objread["Duration"] == DBNull.Value ? null : objread["Duration"].ToString());
                    objNewMeInfo.FileType = (objread["FileType"] == DBNull.Value ? null : objread["FileType"].ToString());
                    objNewMeInfo.FilePath = (objread["FilePath"] == DBNull.Value ? null : objread["FilePath"].ToString());
                    objNewMeInfo.FilePath_Encrypted = (objread["FilePath_Encrypted"] == DBNull.Value ? null : objread["FilePath_Encrypted"].ToString());
                    objNewMeInfo.ImagepathSuffix = (objread["ImagepathSuffix"] == DBNull.Value ? null : objread["ImagepathSuffix"].ToString());

                    if (objread["IsDisplayToPractice"] != DBNull.Value)
                    {
                        objNewMeInfo.IsDisplayToPractice = objread["IsDisplayToPractice"].ToString();
                    }
                    else
                    {
                        objNewMeInfo.IsDisplayToPractice = null;
                    }
                    objNewMeInfo.IsDisplayToPractice = (objread["IsDisplayToPractice"] == DBNull.Value ? null : objread["IsDisplayToPractice"].ToString());
                    MediaInfo.Add(objNewMeInfo);

                }
                return MediaInfo;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetNewMediaInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<ListContactInfo> List_ContactUs(ListContactInfo objLstCont)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] paramlist = {
			new SqlParameter("@in_ContactLastName", (!string.IsNullOrEmpty(objLstCont.ContactLastName) ? objLstCont.ContactLastName : null)),
			new SqlParameter("@in_ContactMail", (!string.IsNullOrEmpty(objLstCont.ContactMail) ? objLstCont.ContactMail : null)),
			new SqlParameter("@in_ContactPhone", (!string.IsNullOrEmpty(objLstCont.ContactPhone) ? objLstCont.ContactPhone : null)),
			new SqlParameter("@in_ContactSubject", (!string.IsNullOrEmpty(objLstCont.ContactSubject) ? objLstCont.ContactSubject : null)),
			new SqlParameter("@in_ReplyStatus_Ind", (objLstCont.ReplyStatus_Ind != "0" ? objLstCont.ReplyStatus_Ind : null)),
			new SqlParameter("@in_ContactMessage", (!string.IsNullOrEmpty(objLstCont.ContactMessage) ? objLstCont.ContactMessage : null)),
			new SqlParameter("@in_NoofRecords", objLstCont.NoOfRecords),
			new SqlParameter("@in_PageNo", objLstCont.PageNo),
			new SqlParameter("@in_OrderByItem", objLstCont.OrderByitem),
			new SqlParameter("@in_OrderBy", objLstCont.OrderBy)
		};
                objcommon.AddInParameters(paramlist);
                returndata = objcommon.GetDataReader("Help_dbo.st_Cognode_List_Contactinfo");
                List<ListContactInfo> ContactList = new List<ListContactInfo>();
                while (returndata.Read())
                {
                    ListContactInfo objlst = new ListContactInfo();
                    objlst.Contact_ID = Convert.ToInt32(returndata["Contact_ID"] != null ? returndata["Contact_ID"] : null);
                    objlst.ContactMail = Convert.ToString(returndata["ContactMail"] != null ? returndata["ContactMail"] : null);
                    objlst.ContactPhone = Convert.ToString(returndata["ContactPhone"].ToString() != null & returndata["ContactPhone"].ToString() != "" ? clsCommonFunctions.UsPhoneFormat(returndata["ContactPhone"].ToString()) : null);
                    objlst.ContactSubject = Convert.ToString(returndata["ContactSubject"] != null ? returndata["ContactSubject"] : null);
                    objlst.DateOfSubmit = Convert.ToString(returndata["DateOfSubmit"] != null ? returndata["DateOfSubmit"] : null);
                    objlst.ContactMessage = Convert.ToString(returndata["ContactMessage"] != null ? returndata["ContactMessage"] : null);
                    objlst.ContactName = Convert.ToString(returndata["ContactName"] != null ? returndata["ContactName"] : null);
                    objlst.ReplyStatus_Ind = Convert.ToString(returndata["ReplyStatus_Ind"] != null ? returndata["ReplyStatus_Ind"] : null);
                    ContactList.Add(objlst);
                }
                returndata.NextResult();
                if (returndata.HasRows == true)
                {
                    if (returndata.Read())
                    {
                        ListContactInfo.TotalRecords = Convert.ToInt32(returndata[0]);
                    }
                }
                return ContactList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "List_ContactUs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override ListContactInfo View_Contacts(int Contact_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] inparamlist = { new SqlParameter("@In_Contact_ID", Contact_ID) };
                objcommon.AddInParameters(inparamlist);
                objread = objcommon.GetDataReader("Help_dbo.st_Cognode_GET_Contactinfo");
                ListContactInfo objDetails = new ListContactInfo();
                if (objread.Read())
                {
                    objDetails.ContactFirstName = Convert.ToString(objread["ContactFirstName"] != null ? objread["ContactFirstName"] : null);
                    objDetails.ContactLastName = Convert.ToString(objread["ContactLastName"] != null ? objread["ContactLastName"] : null);
                    objDetails.ContactMail = Convert.ToString(objread["ContactMail"] != null ? objread["ContactMail"] : null);
                    objDetails.ContactPhone = Convert.ToString(objread["ContactPhone"] != null ? objread["ContactPhone"] : null);
                    objDetails.ContactSubject = Convert.ToString(objread["ContactSubject"] != null ? objread["ContactSubject"] : null);
                    objDetails.ContactMessage = Convert.ToString(objread["ContactMessage"] != null ? objread["ContactMessage"] : null);
                    objDetails.ReplyStatus = Convert.ToString(objread["ReplyStatus"] != null ? objread["ReplyStatus"] : null);
                    objDetails.ReplyStatus_Ind = Convert.ToString(objread["ReplyStatus_Ind"] != null ? objread["ReplyStatus_Ind"] : null);
                }
                return objDetails;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "View_Contacts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override ListContactInfo UPD_ReplyStatus(int Contact_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] inparamlist = { new SqlParameter("@In_Contact_ID", Contact_ID) };
                objcommon.AddInParameters(inparamlist);
                objread = objcommon.GetDataReader("Help_dbo.st_Upd_ContactReplyStatus");
                ListContactInfo objDetails = new ListContactInfo();
                if (objread.Read())
                {
                    objDetails.ContactFirstName = Convert.ToString(objread["ContactFirstName"] != null ? objread["ContactFirstName"] : null);
                    objDetails.ContactLastName = Convert.ToString(objread["ContactLastName"] != null ? objread["ContactLastName"] : null);
                    objDetails.ContactMail = Convert.ToString(objread["ContactMail"] != null ? objread["ContactMail"] : null);
                    objDetails.ContactPhone = Convert.ToString(objread["ContactPhone"] != null ? objread["ContactPhone"] : null);
                    objDetails.ContactSubject = Convert.ToString(objread["ContactSubject"] != null ? objread["ContactSubject"] : null);
                    objDetails.ContactMessage = Convert.ToString(objread["ContactMessage"] != null ? objread["ContactMessage"] : null);
                    objDetails.ReplyStatus = Convert.ToString(objread["ReplyStatus"] != null ? objread["ReplyStatus"] : null);
                    objDetails.ReplyStatus_Ind = Convert.ToString(objread["ReplyStatus_Ind"] != null ? objread["ReplyStatus_Ind"] : null);
                }
                return objDetails;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UPD_ReplyStatus", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override bool DEL_Contacts(int Contact_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@In_Contact_ID", Contact_ID) };
                objcommon.AddInParameters(inparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Cognode_Del_Contactinfo");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "DEL_Contacts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override ErrorsList Get_Error_Details(int Log_ID)
        {
            try
            {
                SqlDataReader objRead = default(SqlDataReader);
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] ObjInParam = { new SqlParameter("@in_Log_ID", Log_ID) };
                objCommon.AddInParameters(ObjInParam);

                objRead = objCommon.GetDataReader("Help_dbo.st_GET_Error_ASPX");

                ErrorsList objErrDetails = new ErrorsList();


                if (objRead.Read())
                {
                    if (objRead["InsertDate"] != null)
                    {
                        objErrDetails.InsertDate = Convert.ToString(objRead["InsertDate"]);
                    }
                    if (objRead["Class_Name"] != null)
                    {
                        objErrDetails.Class_Name = Convert.ToString(objRead["Class_Name"]);
                    }
                    if (objRead["Method_Name"] != null)
                    {
                        objErrDetails.Method_Name = Convert.ToString(objRead["Method_Name"]);
                    }
                    if (objRead["ErrCategory"] != null)
                    {
                        objErrDetails.ErrCategory = Convert.ToString(objRead["ErrCategory"]);
                    }
                    if (objRead["ErrFile"] != null)
                    {
                        objErrDetails.ErrFile = Convert.ToString(objRead["ErrFile"]);
                    }
                    if (objRead["URL"] != null)
                    {
                        objErrDetails.URL = Convert.ToString(objRead["URL"]);
                    }
                    if (objRead["ErrLine"] != null)
                    {
                        objErrDetails.ErrLine = Convert.ToString(objRead["ErrLine"]);
                    }
                    if (objRead["ErrDescription"] != null)
                    {
                        objErrDetails.ErrDescription = Convert.ToString(objRead["ErrDescription"]);
                    }
                    if (objRead["LocalAddr"] != null)
                    {
                        objErrDetails.LocalAddr = Convert.ToString(objRead["LocalAddr"]);
                    }
                    if (objRead["HostAddress"] != null)
                    {
                        objErrDetails.HostAddress = Convert.ToString(objRead["HostAddress"]);
                    }
                    if (objRead["RequestMethod"] != null)
                    {
                        objErrDetails.RequestMethod = Convert.ToString(objRead["RequestMethod"]);
                    }
                    if (objRead["UserAgent"] != null)
                    {
                        objErrDetails.UserAgent = Convert.ToString(objRead["UserAgent"]);
                    }
                    if (objRead["CustomerRefID"] != null)
                    {
                        objErrDetails.CustomerRefID = Convert.ToString(objRead["CustomerRefID"]);
                    }
                    if (objRead["Exception_Name"] != null)
                    {
                        objErrDetails.Exception_Name = Convert.ToString(objRead["Exception_Name"]);
                    }
                    if (objRead["ErrSource"] != null)
                    {
                        objErrDetails.ErrSource = Convert.ToString(objRead["ErrSource"]);
                    }
                    if (objRead["Name"] != null)
                    {
                        objErrDetails.Name = Convert.ToString(objRead["Name"]);
                    }
                    return objErrDetails;
                }
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_Error_Details", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public List<ListMessage> getMessageListCategories(ListMessage lstMessage)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams ={
                                           new SqlParameter("@In_OrderByItem",lstMessage.OrderByItem ),
                                           new SqlParameter("@In_orderBy",lstMessage.OrderBy),
                                           new SqlParameter("@In_NoOfRecords",lstMessage.NoOfRecords),
                                           new SqlParameter("@In_PageNo",lstMessage.PageNo),
                                           new SqlParameter("@In_MessageCategory",lstMessage.Category),
                                            };
                objcommon.AddInParameters(insparams);
                SqlDataReader drlstmessage = default(SqlDataReader);
                drlstmessage = objcommon.GetDataReader("Help_dbo.st_MessageStation_LIST_Category_RDPaging");
                List<ListMessage> objlistMessage = new List<ListMessage>();
                while (drlstmessage.Read())
                {
                    ListMessage objlist = new ListMessage();
                    if (!DBNull.Value.Equals(drlstmessage["MessageCategory_ID"]))
                    {
                        objlist.MsgCategory_ID = Convert.ToInt32(drlstmessage["MessageCategory_ID"]);
                    }
                    if (!DBNull.Value.Equals(drlstmessage["MessageCategory"]))
                    {
                        objlist.Category = Convert.ToString(drlstmessage["MessageCategory"]);
                    }
                    if (!DBNull.Value.Equals(drlstmessage["Type"]))
                    {
                        objlist.messageType = Convert.ToString(drlstmessage["Type"]);
                    }
                    if (!DBNull.Value.Equals(drlstmessage["Status"]))
                    {
                        objlist.status = Convert.ToString(drlstmessage["Status"]);
                    }
                    if (!DBNull.Value.Equals(drlstmessage["Filepath"]))
                    {
                        objlist.messagePath = Convert.ToString(drlstmessage["Filepath"]);
                    }
                    objlistMessage.Add(objlist);
                }
                drlstmessage.NextResult();
                if (drlstmessage.HasRows)
                {
                    while (drlstmessage.Read())
                    {
                        ListMessage.TotalRecords = Convert.ToString(drlstmessage["TOTAL_RECORDS"]);
                    }
                }
                return objlistMessage;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getMessageListCategories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string insert_category(ListMessage insList)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_MessageCategory",insList.Category),
                                           new SqlParameter("@in_SysGenMsgs_Ind", insList.sysInd),
                                           new SqlParameter("@in_UserGenMsgs_Ind",insList.userInd),
                                           new SqlParameter("@in_CreatedBy",'1'),
                                           new SqlParameter("@in_Filepath",insList.messagePath),
                                           new SqlParameter("@in_Loginhistory_ID",insList.loginHistory),
                                      };
                IDataParameter[] outparams ={
                                            new SqlParameter("@Out_Msg",SqlDbType.VarChar, 100)
                                       };
                objcommon.AddInParameters(insparam);
                objcommon.AddOutParameters(outparams);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_MessageStation_INS_Category");
                string strout = string.Empty;
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    strout = Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value);
                }
                return strout;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "insert_category", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ListMessage> FillcategoryDdl()
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<ListMessage> lstCategory = new List<ListMessage>();
                DataSet dslist = new DataSet();
                dslist = objcommon.GetDataSet("Help_dbo.st_MessageStation_view_Category");
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drlist in dslist.Tables[0].Rows)
                    {
                        ListMessage objList = new ListMessage();
                        if (drlist["MessageCategory_ID"] != null)
                        {
                            objList.MsgCategory_ID = Convert.ToInt32(drlist["MessageCategory_ID"]);
                        }
                        if (drlist["MessageCategory"] != null)
                        {
                            objList.Category = Convert.ToString(drlist["MessageCategory"]);
                        }
                        lstCategory.Add(objList);
                    }
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillcategoryDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetCategoryinfo(int catId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_MessageCategory_ID",catId)
                                      };
                objcommon.AddInParameters(insparam);
                DataSet dsinfo = new DataSet();
                dsinfo = objcommon.GetDataSet("Help_dbo.st_MessageStation_GET_Category");
                return dsinfo;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetCategoryinfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string upd_category(ListMessage updlist)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_MessageCategory_ID",updlist.MsgCategory_ID),
                                           new SqlParameter("@in_MessageCategory",updlist.Category),
                                           new SqlParameter("@in_SysGenMsgs_Ind",updlist.sysInd),
                                           new SqlParameter("@in_UserGenMsgs_Ind",updlist.userInd),
                                           new SqlParameter("@in_UpdatedBy","1"),
                                           new SqlParameter("@in_Filepath",updlist.messagePath),
                                           new SqlParameter("@in_Loginhistory_ID",updlist.loginHistory)                                         
                                      };
                IDataParameter[] outparam = { 
                                            new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100) 
                                        };
                objcommon.AddInParameters(insparam);
                objcommon.AddOutParameters(outparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_MessageStation_UPD_Category");
                string strout = string.Empty;
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    strout = Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value);
                }
                return strout;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "upd_category", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public void statusCategory(int catId, int loghistory)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                          new SqlParameter("@in_MessageCategory_ID",catId),
                                          new SqlParameter("@in_UpdatedBy",1),
                                          new SqlParameter("@in_Loginhistory_ID",loghistory)
                                      };
                objcommon.AddInParameters(insparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_MessageStation_DEL_Category");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "statusCategory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public List<ListMessage> getRoleSCategory(int catId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_MessageCategory_ID",catId)
                                      };
                objcommon.AddInParameters(insparam);
                List<ListMessage> objlist = new List<ListMessage>();
                DataSet ds = new DataSet();
                ds = objcommon.GetDataSet("Help_dbo.st_MessageStation_list_RelatedRoles");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ListMessage objcls = new ListMessage();
                        if (!DBNull.Value.Equals(dr["Role_ID"]))
                        {
                            objcls.Roleid = Convert.ToString(dr["Role_ID"]);
                        }
                        if (!DBNull.Value.Equals(dr["RoleName"]))
                        {
                            objcls.RoleName = Convert.ToString(dr["RoleName"]);
                        }
                        if (!DBNull.Value.Equals(dr["cnt"]))
                        {
                            objcls.RoleChk = Convert.ToString(dr["cnt"]);
                        }
                        objlist.Add(objcls);
                    }
                }
                return objlist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getRoleSCategory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public void updRoleCategory(ListMessage objupd)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_MessageCategory_ID",objupd.MsgCategory_ID),
                                           new SqlParameter("@in_Message_Roles",objupd.Roleid),
                                           new SqlParameter("@in_user",1),
                                           new SqlParameter("@in_Filepath",null)
                                      };
                objcommon.AddInParameters(insparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Messagestation_INS_UPD_Roles");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "updRoleCategory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public List<ListMessage> MessageSettingGrid(ListMessage objlist)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Role_ID",objlist.Roleid),
                                           new SqlParameter("@in_MR_ID",null),
                                           new SqlParameter("@in_ToRole_ID",(objlist.ReplyToRoleID !=0 ? objlist.ReplyToRoleID.ToString() : null)),
                                           new SqlParameter("@in_MessageCategory_ID",(objlist.MsgCategory_ID!=0 ? objlist.MsgCategory_ID.ToString() :null)),
                                           new SqlParameter("@in_NoofRecords",objlist.NoOfRecords),
                                           new SqlParameter("@in_PageNo",objlist.PageNo),
                                           new SqlParameter("@In_OrderBYitem",objlist.OrderByItem),
                                           new SqlParameter("@In_OrderBy",objlist.OrderBy),
                                      };
                objcommon.AddInParameters(insparam);
                SqlDataReader drlist = default(SqlDataReader);
                drlist = objcommon.GetDataReader("Help_dbo.st_MessageStation_List_ExistRoles_RDPaging");
                List<ListMessage> lstSettings = new List<ListMessage>();
                while (drlist.Read())
                {
                    ListMessage objitems = new ListMessage();
                    if (!DBNull.Value.Equals(drlist["FromRole"]))
                    {
                        objitems.RoleName = Convert.ToString(drlist["FromRole"]);
                    }
                    if (!DBNull.Value.Equals(drlist["ToRole"]))
                    {
                        objitems.Sender = Convert.ToString(drlist["ToRole"]);
                    }
                    if (!DBNull.Value.Equals(drlist["GroupName"]))
                    {
                        objitems.groupName = Convert.ToString(drlist["GroupName"]);
                    }
                    if (!DBNull.Value.Equals(drlist["Category"]))
                    {
                        objitems.Category = Convert.ToString(drlist["Category"]);
                    }
                    if (!DBNull.Value.Equals(drlist["SendInd"]))
                    {
                        if (Convert.ToString(drlist["SendInd"]) == "Y")
                        {
                            objitems.userInd = "Yes";
                        }
                        else
                        {
                            objitems.userInd = "No";
                        }

                    }
                    if (!DBNull.Value.Equals(drlist["GroupInd"]))
                    {
                        if (Convert.ToString(drlist["GroupInd"]) == "Y")
                        {
                            objitems.groupInd = "Yes";
                        }
                        else
                        {
                            objitems.groupInd = "No";
                        }
                    }

                    if (!DBNull.Value.Equals(drlist["MR_ID"]))
                    {
                        objitems.MrId = Convert.ToInt32(drlist["MR_ID"]);
                    }
                    if (!DBNull.Value.Equals(drlist["FromID"]))
                    {
                        objitems.Roleid = Convert.ToString(drlist["FromID"]);
                    }
                    if (!DBNull.Value.Equals(drlist["ToID"]))
                    {
                        objitems.ReplyToRoleID = Convert.ToInt32(drlist["ToID"]);
                    }
                    lstSettings.Add(objitems);
                }
                drlist.NextResult();
                if (drlist.HasRows)
                {
                    while (drlist.Read())
                    {
                        ListMessage.TotalRecords = Convert.ToString(drlist["TOTAL_RECORDS"]);
                    }
                }
                return lstSettings;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "MessageSettingGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ListMessage> FillActivecategoryDdl()
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<ListMessage> lstCategory = new List<ListMessage>();
                DataSet dslist = new DataSet();
                dslist = objcommon.GetDataSet("Help_dbo.st_MessageStation_LIST_ActiveCategory ");
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drlist in dslist.Tables[0].Rows)
                    {
                        ListMessage objList = new ListMessage();
                        if (drlist["MessageCategory_ID"] != null)
                        {
                            objList.MsgCategory_ID = Convert.ToInt32(drlist["MessageCategory_ID"]);
                        }
                        if (drlist["MessageCategory"] != null)
                        {
                            objList.Category = Convert.ToString(drlist["MessageCategory"]);
                        }
                        lstCategory.Add(objList);
                    }
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillActivecategoryDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public List<ListMessage> FillsettingrolesDdl(int? roleId)
        {
            try
            {
                clsCommonFunctions oblcls = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Login_ID",1),
                                           new SqlParameter("@in_Role_ID",roleId)

                                      };
                oblcls.AddInParameters(insparam);
                DataSet dsfill = new DataSet();
                dsfill = oblcls.GetDataSet("Help_dbo.st_MessageStation_GET_SettingRoles");
                List<ListMessage> roleList = new List<ListMessage>();
                if (dsfill.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsfill.Tables[0].Rows)
                    {
                        ListMessage objitems = new ListMessage();
                        if (!DBNull.Value.Equals(dr["Role_ID"]))
                        {
                            objitems.Roleid = Convert.ToString(dr["Role_ID"]);
                        }
                        if (!DBNull.Value.Equals(dr["RoleName"]))
                        {
                            objitems.RoleName = Convert.ToString(dr["RoleName"]);
                        }
                        roleList.Add(objitems);
                    }
                }
                return roleList;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillsettingrolesDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ListMessage> FillNonExistRolesDdl(int? roleId)
        {
            try
            {
                clsCommonFunctions oblcls = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Login_ID",1),
                                           new SqlParameter("@in_Role_ID",roleId)

                                      };
                oblcls.AddInParameters(insparam);
                DataSet dsfill = new DataSet();
                dsfill = oblcls.GetDataSet("Help_dbo.st_MessageStation_List_NonExistRoles");
                List<ListMessage> roleList = new List<ListMessage>();
                if (dsfill.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsfill.Tables[0].Rows)
                    {
                        ListMessage objitems = new ListMessage();
                        if (!DBNull.Value.Equals(dr["Role_ID"]))
                        {
                            objitems.Roleid = Convert.ToString(dr["Role_ID"]);
                        }
                        if (!DBNull.Value.Equals(dr["RoleName"]))
                        {
                            objitems.RoleName = Convert.ToString(dr["RoleName"]);
                        }
                        roleList.Add(objitems);
                    }
                }
                return roleList;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillNonExistRolesDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<ListMessage> FillinscategoryDdl(int? roleId)
        {
            try
            {
                clsCommonFunctions oblcls = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Login_ID",null),
                                           new SqlParameter("@in_Role_ID",roleId),
                                           new SqlParameter("@in_UserGenMsgs_Ind","Y")

                                      };
                oblcls.AddInParameters(insparam);
                DataSet dsfill = new DataSet();
                dsfill = oblcls.GetDataSet("Help_dbo.st_MessageStation_List_Categories");
                List<ListMessage> roleList = new List<ListMessage>();
                if (dsfill.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsfill.Tables[0].Rows)
                    {
                        ListMessage objitems = new ListMessage();
                        if (!DBNull.Value.Equals(dr["MessageCategory_ID"]))
                        {
                            objitems.MsgCategory_ID = Convert.ToInt32(dr["MessageCategory_ID"]);
                        }
                        if (!DBNull.Value.Equals(dr["MessageCategory"]))
                        {
                            objitems.Category = Convert.ToString(dr["MessageCategory"]);
                        }
                        roleList.Add(objitems);
                    }
                }
                return roleList;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillinscategoryDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public void insMsgSettings(ListMessage InsMsgSettings)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams ={
                                       new SqlParameter("@in_Role_ID",InsMsgSettings.Roleid),
                                       new SqlParameter("@in_RelatedRole_ID",InsMsgSettings.ReplyToRoleID),
                                       new SqlParameter("@in_DisplayName",InsMsgSettings.groupName),
                                       new SqlParameter("@in_MessageCategory_ID",InsMsgSettings.MsgCategory_ID),
                                       new SqlParameter("@in_CreatedBy",1),
                                       new SqlParameter("@in_View_Ind",'N'),
                                       new SqlParameter("@in_SendUserLevel_Ind",InsMsgSettings.userInd),
                                       new SqlParameter("@in_SendGroupLevel_Ind",InsMsgSettings.groupInd),
                                       new SqlParameter("@in_Loginhistory_ID",InsMsgSettings.loginHistory)
                                        };
                objcommon.AddInParameters(insparams);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_MessageStation_INS_RelatedRoles");
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "insMsgSettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public DataSet getSetting(int MrId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Role_ID",null),
                                           new SqlParameter("@in_MR_ID",MrId),
                                           new SqlParameter("@in_ToRole_ID",null),
                                           new SqlParameter("@in_MessageCategory_ID",null)                                           
                                      };
                objcommon.AddInParameters(insparam);
                DataSet dsget = new DataSet();
                dsget = objcommon.GetDataSet("Help_dbo.st_MessageStation_List_ExistRoles");
                return dsget;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getSetting", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public void UpdMsgSettings(ListMessage Updset)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                      new SqlParameter("@in_MR_ID",Updset.MrId),
                                      new SqlParameter("@in_DisplayName",Updset.groupName),
                                      new SqlParameter("@in_MessageCategory_ID",Updset.MsgCategory_ID),
                                      new SqlParameter("@in_UpdatedBy",1),
                                      new SqlParameter("@in_View_Ind","N"),
                                      new SqlParameter("@in_SendUserLevel_Ind",Updset.userInd),
                                      new SqlParameter("@in_SendGroupLevel_Ind",Updset.groupInd),
                                      new SqlParameter("@in_Loginhistory_ID",Updset.loginHistory)
                                       };
                objcommon.AddInParameters(insparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_MessageStation_UPD_RelatedRoles");
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdMsgSettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void delsettings(int mrid, int loghistory)
        {
            try
            {
                clsCommonFunctions objcommn = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                          new SqlParameter("@in_MR_ID",mrid),
                                          new SqlParameter("@in_DeletedBy",1),
                                          new SqlParameter("@in_Loginhistory_ID",loghistory)
                                           };
                objcommn.AddInParameters(insparam);
                objcommn.ExecuteNonQuerySP("Help_dbo.st_MessageStation_DEL_RelatedRoles");
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "delsettings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override string UpdateMediaInfo(MediaInfo ObjUpdMediaInfo, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] inparmlist = {
			new SqlParameter("@In_HelpMedia_ID", ObjUpdMediaInfo.Helpmedia_ID),
			new SqlParameter("@in_Title", ObjUpdMediaInfo.Title),
			new SqlParameter("@in_Duration", ObjUpdMediaInfo.Duration),
			new SqlParameter("@in_Description", ObjUpdMediaInfo.Description),
			new SqlParameter("@in_FileType", ObjUpdMediaInfo.FileType),
			new SqlParameter("@in_FilePath", ObjUpdMediaInfo.FilePath),
			new SqlParameter("@in_FilePath_Encrypted", ObjUpdMediaInfo.FilePath_Encrypted),
			new SqlParameter("@in_IsDisplayToPractice", ObjUpdMediaInfo.IsDisplayToPractice),
			new SqlParameter("@In_Updatedby", HttpContext.Current.Session["userid"])
		};
                IDataParameter[] outparamlist = { new SqlParameter("@Out_Msg", SqlDbType.NVarChar, 100) };
                objclscommon.AddInParameters(inparmlist);
                objclscommon.AddOutParameters(outparamlist);
                objclscommon.ExecuteNonQuerySP("Help_dbo.st_Admin_UPD_HelpMedia");
                if (objclscommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != DBNull.Value)
                {
                    Out_Msg = objclscommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                return Out_Msg;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdateMediaInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool DeleteMediaInfo(MediaInfo ObjDelMediaInfo)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@In_Helpmedia_ID", ObjDelMediaInfo.Helpmedia_ID),
			new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userid"])
		};
                objcommon1.AddInParameters(objparm);
                objcommon1.ExecuteNonQuerySP("Help_dbo.st_Admin_DEL_HelpMedia");
                return true;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "DeleteMediaInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }

        public List<messageStatus> mailStatusGrid(messageStatus objMailstatus)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@In_Provider_id",objMailstatus.providerId),
                                           new SqlParameter("@In_Patient_ID",objMailstatus.patientId),
                                           new SqlParameter("@In_Mailstatus_ID",objMailstatus.statusId),
                                           new SqlParameter("@In_Fromdate",objMailstatus.fromdate),
                                           new SqlParameter("@In_Todate",objMailstatus.todate),
                                           new SqlParameter("@in_OrderByItem",objMailstatus.orderByItem),
                                           new SqlParameter("@in_OrderBy",objMailstatus.orderBy),
                                           new SqlParameter("@In_NoOfRecords",objMailstatus.noofrecords),
                                           new SqlParameter("@In_PageNo",objMailstatus.pageno)
                                      };
                objcommon.AddInParameters(insparam);
                SqlDataReader drlist = default(SqlDataReader);
                if (objMailstatus.isRec == "Yes")
                {
                    drlist = objcommon.GetDataReader("Help_dbo.St_Emailmessage_Get_RecurringComponentMessage");
                }
                else
                {
                    drlist = objcommon.GetDataReader("Help_dbo.st_Emailmessage_Get_DetailsOfClientMessages_RdPaging");
                }
                List<messageStatus> objlist = new List<messageStatus>();
                while (drlist.Read())
                {
                    messageStatus objitem = new messageStatus();
                    objitem.practiceName = Convert.ToString(drlist["PracticeName"]);
                    objitem.receiveName = Convert.ToString(drlist["PatientName"]);
                    objitem.reason = Convert.ToString(drlist["MessageOption"]);
                    objitem.mailDate = DateTime.Parse(Convert.ToString(drlist["Mail_GeneratedDate"])).ToShortDateString();
                    objitem.mailstatus = Convert.ToString(drlist["Mail_Status"]);
                    objitem.mailbody = Convert.ToString(drlist["Message_Body"]);
                    objitem.mailId = Convert.ToInt32(drlist["EmailMessage_Transaction_ID"]);
                    objitem.mailFrom = Convert.ToString(drlist["Mail_From"]);
                    objitem.mailTo = Convert.ToString(drlist["Mail_To"]);
                    objlist.Add(objitem);
                }
                drlist.NextResult();
                if (drlist.HasRows)
                {
                    if (drlist.Read())
                    {
                        messageStatus.Totalnoofrecords = Convert.ToInt32(drlist[0]);
                    }
                }
                return objlist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "mailStatusGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public List<messageStatus> FillProviderids()
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<messageStatus> lstCategory = new List<messageStatus>();
                DataSet dslist = new DataSet();
                dslist = objcommon.GetDataSet("Help_dbo.st_Practice_LIST_Practices");
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drlist in dslist.Tables[0].Rows)
                    {
                        messageStatus objList = new messageStatus();
                        if (drlist["provider_id"] != null)
                        {
                            objList.providerId = Convert.ToInt32(drlist["provider_id"]);
                        }
                        if (drlist["Practicename"] != null)
                        {
                            objList.practiceName = Convert.ToString(drlist["Practicename"]);
                        }
                        lstCategory.Add(objList);
                    }
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "FillPracticeDdl", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<messageStatus> fillPatientName(int pracId)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<messageStatus> lstCategory = new List<messageStatus>();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_Provider_id",pracId),
                                        };
                objcommon.AddInParameters(insparam);
                DataSet dslist = new DataSet();
                dslist = objcommon.GetDataSet("Help_dbo.st_Admin_List_PracticePatients");
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drlist in dslist.Tables[0].Rows)
                    {
                        messageStatus objList = new messageStatus();
                        if (drlist["Patient_ID"] != null)
                        {
                            objList.patientId = Convert.ToInt32(drlist["Patient_ID"]);
                        }
                        if (drlist["Name"] != null)
                        {
                            objList.patientName = Convert.ToString(drlist["Name"]);
                        }
                        lstCategory.Add(objList);
                    }
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "fillPatientName", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getmailInfo(int id)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                      new SqlParameter("@in_EmailMessage_Transaction_ID",id)
                                       };
                objcommon.AddInParameters(insparam);
                DataSet dsinfo = new DataSet();
                dsinfo = objcommon.GetDataSet("ST_sent_pending_email_info");
                return dsinfo;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getmailInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override DataSet Provider_DocumentuploadDetails(Provider_DocumentInfo ObjDocument)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] strParam = {
			new SqlParameter("@In_provider_id", (ObjDocument.Provider_ID != 0 ? ObjDocument.Provider_ID : null)),
			new SqlParameter("@In_providerName", (!string.IsNullOrEmpty(ObjDocument.ProvName) ? ObjDocument.ProvName : null)),
			new SqlParameter("@In_NoofRec", ObjDocument.NoOfRecords),
			new SqlParameter("@in_pageNo", ObjDocument.PageNo),
			new SqlParameter("@In_Ordeby", ObjDocument.OrderBy),
			new SqlParameter("@In_orderbyitem", ObjDocument.OrderBYItem),
			new SqlParameter("@In_Category", ObjDocument.Category)
		};
                objclscommon.AddInParameters(strParam);
                dataset = objclscommon.GetDataSet("Help_dbo.St_Admin_List_providerMemoryusage_Rdpaging");
                return dataset;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Provider_DocumentuploadDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public override System.Collections.Generic.List<LogInfo> Get_EvenLogInfo(LogInfo objLogInfo)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                List<LogInfo> objList = new List<LogInfo>();
                IDataParameter[] objParam = {
			new SqlParameter("@in_PatientLogin_id", (objLogInfo.PatientLoginID != 0 ? objLogInfo.PatientLoginID : null)),
			new SqlParameter("@in_EventCategory_ID", (objLogInfo.EventCategory_ID != 0 ? objLogInfo.EventCategory_ID : null)),
			new SqlParameter("@in_ProviderLogin_ID", (objLogInfo.ProviderLoginID != 0 ? objLogInfo.ProviderLoginID : null)),
			new SqlParameter("@In_Referencetype_id", (objLogInfo.ReferencetypeID != 0 ? objLogInfo.ReferencetypeID : null)),
			new SqlParameter("@In_NoOfRecords", (objLogInfo.NoofRecords != 0 ? objLogInfo.NoofRecords : null)),
			new SqlParameter("@In_PageNo", (objLogInfo.PageNo != 0 ? objLogInfo.PageNo : null)),
			new SqlParameter("@In_OrderByItem", (!string.IsNullOrEmpty(objLogInfo.OrderByItem) ? objLogInfo.OrderByItem : null)),
			new SqlParameter("@In_OrderBy", (!string.IsNullOrEmpty(objLogInfo.OrderBy) ? objLogInfo.OrderBy : null))
		};
                objclscommon.AddInParameters(objParam);
                objread = objclscommon.GetDataReader("Help_dbo.st_Patient_Get_InformationLog_RDPaging");
                while (objread.Read())
                {
                    LogInfo objLogsinfo = new LogInfo();
                    objLogsinfo.EventInfoLog_ID = (!DBNull.Value.Equals(objread["EventInfoLog_ID"]) ? Convert.ToInt32(objread["EventInfoLog_ID"]) : 0);
                    objLogsinfo.Event_Title = (!DBNull.Value.Equals(objread["Event_Title"]) ? objread["Event_Title"].ToString() : null);
                    objLogsinfo.Patientname = (!DBNull.Value.Equals(objread["Patientname"]) ? objread["Patientname"].ToString() : null);
                    objLogsinfo.Modifiedreference = (!DBNull.Value.Equals(objread["Modifiedreference"]) ? objread["Modifiedreference"].ToString() : null);
                    objLogsinfo.Modifiedby = (!DBNull.Value.Equals(objread["Modifiedby"]) ? objread["Modifiedby"].ToString() : null);
                    objLogsinfo.LoggedOn = (!DBNull.Value.Equals(objread["LoggedOn"]) ? objread["LoggedOn"].ToString() : null);
                    objLogsinfo.Eventtype_id = (!DBNull.Value.Equals(objread["Eventtype_id"]) ? Convert.ToInt32(objread["Eventtype_id"]) : 0);
                    objList.Add(objLogsinfo);
                }
                if (objread.NextResult())
                {
                    if (objread.Read())
                    {
                        objLogInfo.TotalRecords = (int)objread["TotalRecords"];
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_EvenLogInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<LogInfo> Get_PatientLoginfo(string EventLogID, string EventTypeID)
        {
            List<LogInfo> objlist = new List<LogInfo>();
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                DataSet ds = new DataSet();

                IDataParameter[] objParam = {
			new SqlParameter("@In_EventLog_id", (!string.IsNullOrEmpty(EventLogID) ? EventLogID : null)),
			new SqlParameter("@In_Eventtype_id", (!string.IsNullOrEmpty(EventTypeID) ? EventTypeID : null))
		};
                objclscommon.AddInParameters(objParam);
                ds = objclscommon.GetDataSet("Help_dbo.st_Patient_Get_PatientLoginfo");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow objDR in ds.Tables[0].Rows)
                        {
                            LogInfo objlog = new LogInfo();
                            if (objDR["Field"] != null)
                            {
                                objlog.Field = objDR["Field"].ToString();
                            }
                            else
                            {
                                objlog.Field = null;
                            }
                            if (objDR["PreviousValue"] != null)
                            {
                                objlog.PreviousValue = objDR["PreviousValue"].ToString();
                            }
                            else
                            {
                                objlog.PreviousValue = null;
                            }
                            if (objDR["Presentvalue"] != null)
                            {
                                objlog.Presentvalue = objDR["Presentvalue"].ToString();
                            }
                            else
                            {
                                objlog.Presentvalue = null;
                            }

                            objlist.Add(objlog);
                            objlog = null;
                        }
                    }


                }


                return objlist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_PatientLoginfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<AdminAddress> getAddressGrid(AdminAddress objAdmin)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams ={
                                       new SqlParameter("@in_Role_ID",objAdmin.roleId),
                                       new SqlParameter("@In_Name",objAdmin.Full_Name),
                                       new SqlParameter("@in_NoofRecords",objAdmin.noofrecords),
                                       new SqlParameter("@in_PageNo",objAdmin.pageno),
                                       new SqlParameter("@in_OrderByItem",objAdmin.orderByItem),
                                       new SqlParameter("@in_OrderBy",objAdmin.orderBy)                                       
                                        };
                objcommon.AddInParameters(insparams);
                List<AdminAddress> AddressList = new List<AdminAddress>();
                SqlDataReader drinfo = default(SqlDataReader);
                drinfo = objcommon.GetDataReader("Help_dbo.st_Admin_List_Addresses_RDPaging");
                while (drinfo.Read())
                {
                    AdminAddress addressItems = new AdminAddress();
                    if (!DBNull.Value.Equals(drinfo["Login_ID"]))
                    {
                        addressItems.loginId = Convert.ToInt32(drinfo["Login_ID"]);
                    }
                    addressItems.roleId = objAdmin.roleId;
                    if (!DBNull.Value.Equals(drinfo["name"]))
                    {
                        addressItems.Full_Name = Convert.ToString(drinfo["name"]);
                    }
                    if (!DBNull.Value.Equals(drinfo["Address"]))
                    {
                        addressItems.address = Convert.ToString(drinfo["Address"]);
                    }
                    AddressList.Add(addressItems);
                }
                drinfo.NextResult();
                if (drinfo.HasRows)
                {
                    if (drinfo.Read())
                    {
                        AdminAddress.Totalnoofrecords = Convert.ToInt32(drinfo["Total_Records"]);
                    }
                }
                return AddressList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getAddressGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getcontactinfo(int roleid, int loginid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@In_login_id",loginid),
                                           new SqlParameter("@In_role_id",roleid)
                                      };
                objcommon.AddInParameters(insparam);
                DataSet dsinfo = new DataSet();
                dsinfo = objcommon.GetDataSet("Help_dbo.St_Admin_get_User_details");
                return dsinfo;
            }
            catch (Exception ex)
            {

                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "getcontactinfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public List<clsClaimSessionInfo> GetAllProvidersBasedForClaim(int? Provider_ID, int? Practice_ID)
        //{
        //    try
        //    {
        //        var objcommon = new clsCommonFunctions();
        //        // var ds = new DataSet();
        //        var objDataList = new List<clsClaimSessionInfo>();
        //        IDataParameter[] InParamList = {
        //    new SqlParameter("@In_Provider_ID", (Provider_ID ?? null)),
        //    new SqlParameter("@In_Practice_ID", (Practice_ID ?? null))
        //};
        //        objcommon.AddInParameters(InParamList);
        //        DataSet ds = objcommon.GetDataSet("Help_dbo.st_Provider_List_ProviderPlaceOfServices");
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new clsClaimSessionInfo { PlaceOfService_ID = Convert.ToInt32(dr["PlaceOfService_ID"]), PlaceOfService = Convert.ToString(dr["PlaceOfserviceName"]) });
        //            }
        //        }

        //        return objDataList;
        //    }
        //    catch (Exception ex)
        //    {
        //        var Exobj = new clsExceptionLog();
        //        Exobj.LogException(ex, ClassName, "GetAllProvidersBasedForClaim", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public List<patients> ListOfProgressNotes(patients ObjcdLstProgress)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_Patient_ID",ObjcdLstProgress.Patient_ID),
                                           new SqlParameter("@in_OrderByItem",ObjcdLstProgress.orderByItem),
                                           new SqlParameter("@in_OrderBy",ObjcdLstProgress.OrderBy),
                                           new SqlParameter("@in_NoofRecords",ObjcdLstProgress.NoOfRecords),
                                           new SqlParameter("@in_PageNo",ObjcdLstProgress.page)
                                       };
                objcommon.AddInParameters(insparam);
                SqlDataReader drCdprogress = objcommon.GetDataReader("Help_dbo.CD_LIST_ProgressNotes_RdPaging");
                List<patients> prgressnoteslist = new List<patients>();
                if (drCdprogress.HasRows)
                {

                    while (drCdprogress.Read())
                    {
                        patients objprogrep = new patients();
                        if (!DBNull.Value.Equals(drCdprogress["Appointment_ID"]))
                        {
                            objprogrep.Appointment_ID = Convert.ToInt32(drCdprogress["Appointment_ID"]);
                        }
                        if (!DBNull.Value.Equals(drCdprogress["Appointment_Time"]))
                        {
                            objprogrep.Appointment_Date = drCdprogress["Appointment_Time"].ToString();
                            //DateTime.Parse(drCdprogress["Appointment_Date"].ToString()).ToString("dddd, MMMM d,yyyy") + " / " + DateTime.Parse(Convert.ToString(drCdprogress["Appointment_Time"])).ToString("t");
                        }
                        if (!DBNull.Value.Equals(drCdprogress["AppointmentStatus_ID"]))
                        {
                            objprogrep.AppointmentStatus_ID = Convert.ToInt32(drCdprogress["AppointmentStatus_ID"]);
                        }
                        if (!DBNull.Value.Equals(drCdprogress["Notes"]))
                        {
                            objprogrep.GeneralNarrative = drCdprogress["Notes"].ToString();
                        }
                        if (objprogrep.Claim_ind == 1 & objprogrep.AppointmentStatus_ID == 3)
                        {
                            objprogrep.StatusMsg = "CheckIn";
                        }
                        else
                        {
                            objprogrep.StatusMsg = "Scheduled";
                        }
                        prgressnoteslist.Add(objprogrep);
                    }
                    drCdprogress.NextResult();
                    while (drCdprogress.Read())
                    {
                        patients.TotalRecords = Convert.ToInt32(drCdprogress["TOTALRECORDS"]);
                    }
                }
                return prgressnoteslist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "ListOfProgressNotes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public override int InsertCreditCardResponse(CCProcess objcc)
        {
            clsCommonFunctions objcommon = new clsCommonFunctions();
            try
            {

                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);

                IDataParameter[] objinparam = {
            new SqlParameter("@in_Transaction_ID", objcc.strTransactionID),
            new SqlParameter("@in_Status", objcc.StrRespStatusCode),
            new SqlParameter("@in_Response_Data", objcc.strRetval),
            new SqlParameter("@in_Status_Code", objcc.ResponseCode),
            new SqlParameter("@in_CreatedBy", objcc.strUserID),
            new SqlParameter("@in_GatewayDetail_ID", objcc.GatewayDetail_ID),
            new SqlParameter("@In_IsPayPalProcess_Ind", objcc.PaypalProcessInd)
        };

                objclsdbwraper.AddInParameters(objinparam);
                returndata = objclsdbwraper.GetDataReader("Help_dbo.st_CreditCard_TransactionResponse");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "InsertCreditCardResponse", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }


        public override string LoadCreditCardInfo(CCProcess objcc)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] ParamPaidParty = {
			new SqlParameter("@in_ReferenceType_ID", objcc.ReferenceTypeID),
			new SqlParameter("@in_Reference_ID", objcc.ReferenceID)
		};
                objcommon.AddInParameters(ParamPaidParty);
                objread = objcommon.GetDataReader("Help_dbo.st_CreditCard_Get_Card");

                if (objread.Read() == true)
                {

                    objcc.ccinfo_id = objread["CreditCardInfo_ID"].ToString();
                    return objcc.ccinfo_id;
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.clsExceptionLog clsex = new ErrorLog.clsExceptionLog();
                //clsex.LogException(ex, ClassName, "LoadCreditCardInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override bool InsTechniciansInfo(TechniciansInfo objInsTechniciansInfo, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@in_Firstname", objInsTechniciansInfo.First_Name),
			new SqlParameter("@in_Lastname ", objInsTechniciansInfo.Last_Name),
			new SqlParameter("@in_Phonenumber", objInsTechniciansInfo.Phonenumber),
            new SqlParameter("@in_provider_id", objInsTechniciansInfo.Provider_id),
                        new SqlParameter("@in_Email", objInsTechniciansInfo.Email),
                                    new SqlParameter("@in_Password", objInsTechniciansInfo.Password), 
                                        new SqlParameter("@in_Role_id", objInsTechniciansInfo.Roleid),
                                        new SqlParameter("@in_Self_ind", "N")

		};
                IDataParameter[] OutParam = { new SqlParameter("@out_message", SqlDbType.VarChar, 500) };
                objclscommon.AddInParameters(objparm);
                objclscommon.AddOutParameters(OutParam);
                objclscommon.ExecuteNonQuerySP("Help_dbo.St_INS_Technician");
                if (!string.IsNullOrEmpty(Convert.ToString(objclscommon.objdbCommandWrapper.Parameters["@out_message"].Value) ))
                {
                    Out_Msg = objclscommon.objdbCommandWrapper.Parameters["@out_message"].Value.ToString();
                }
                else
                {
                    Out_Msg = null;
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "InsTechniciansInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override System.Collections.Generic.List<TechniciansInfo> GetTechniciansInfo(TechniciansInfo objTechniciansInfo)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			
			new SqlParameter("@in_provider_id", objTechniciansInfo.Provider_id),
			new SqlParameter("@in_Lastname", objTechniciansInfo.Last_Name),
			new SqlParameter("@in_orderbyitem", objTechniciansInfo.OrderByItem),
			new SqlParameter("@in_orderby", objTechniciansInfo.OrderBy),
            new SqlParameter("@in_noofrowsperpage", objTechniciansInfo.noofrowsperpage),
			 new SqlParameter("@in_pageno", objTechniciansInfo.PageNO)
		
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.St_List_Technicians");
                List<TechniciansInfo> lstTechniciansInfo = new List<TechniciansInfo>();
                while (objread.Read())
                {
                    TechniciansInfo objTechnicianInfo = new TechniciansInfo();
                    objTechnicianInfo.Technician_id = Convert.ToInt32(objread["Technician_id"]);
                    objTechnicianInfo.First_Name = Convert.ToString(objread["Firstname"] != null ? objread["Firstname"] : null);
                    objTechnicianInfo.Last_Name = Convert.ToString(objread["Lastname"] != null ? objread["Lastname"] : null);
                    objTechnicianInfo.Phonenumber = Convert.ToString(objread["Phonenumber"] != null ? objread["Phonenumber"] : null);
                    objTechnicianInfo.Email = Convert.ToString(objread["Email"] != null ? objread["Email"] : null);
                    objTechnicianInfo.Access = Convert.ToString(objread["Role_id"].ToString() != "39" ? "Limited access" : "Full access");

                    lstTechniciansInfo.Add(objTechnicianInfo);
                }
                objread.NextResult();
                while (objread.Read())
                {
                    TechniciansInfo.TotalRecords = Convert.ToInt32(objread["Totalrecordscount"]);
                }
                return lstTechniciansInfo;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetTechniciansInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override TechniciansInfo GetupdTechnicianInfo(int Technician_id)
        {

            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                IDataParameter[] objparm = { new SqlParameter("@in_Technician_id", Technician_id) };
                objcommon1.AddInParameters(objparm);
                SqlDataReader objread = objcommon1.GetDataReader("Help_dbo.St_Get_Technicians");
                TechniciansInfo objTechnicianInfo = new TechniciansInfo();
                if (objread.Read())
                {
                    objTechnicianInfo.First_Name = Convert.ToString(objread["Firstname"].ToString() == "" ? null : objread["Firstname"]);
                    objTechnicianInfo.Last_Name = Convert.ToString(objread["Lastname"].ToString() == "" ? null : objread["Lastname"]);
                    objTechnicianInfo.Phonenumber = Convert.ToString(objread["Phonenumber"].ToString() == "" ? null : objread["Phonenumber"]);
                    objTechnicianInfo.Email = Convert.ToString(objread["Email"].ToString() == "" ? null : objread["Email"]);
                    objTechnicianInfo.Tech_Image = Convert.ToString(objread["Technician_image"].ToString() == "" ? null : objread["Technician_image"]);

                }

                return objTechnicianInfo;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "GetupdTechnicianInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool UpdTechnicianInfo(TechniciansInfo objUpdTechnicianInfo, ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();

                IDataParameter[] objparm = {
			new SqlParameter("@in_Firstname", objUpdTechnicianInfo.First_Name),
		new SqlParameter("@in_Lastname", objUpdTechnicianInfo.Last_Name),
			new SqlParameter("@in_Phonenumber", objUpdTechnicianInfo.Phonenumber),
			new SqlParameter("@in_Technician_id", objUpdTechnicianInfo.Technician_id),
			new SqlParameter("@in_status_ind", objUpdTechnicianInfo.status_ind),
            new SqlParameter("@in_reassigned", objUpdTechnicianInfo.Reassigned),
             new SqlParameter("@in_email", objUpdTechnicianInfo.Email)
			
		};
                IDataParameter[] OutParam = { new SqlParameter("@out_Msg", SqlDbType.VarChar, 500) };
                objclscommon.AddInParameters(objparm);
                objclscommon.AddOutParameters(OutParam);
                objclscommon.ExecuteNonQuerySP("Help_dbo.St_UPD_Technician");
                if (objclscommon.GetCurrentCommand.Parameters["@out_msg"].Value != null)
                {
                    Out_Msg = objclscommon.GetCurrentCommand.Parameters["@out_msg"].Value.ToString();
                }
            }

            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UpdTechnicianInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override bool upd_technicianimage(int Technician_id, string imagepath)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();

                IDataParameter[] objparm = {
			new SqlParameter("@in_Technician_id", Technician_id),
		new SqlParameter("@in_Technician_image", imagepath)
			
		};
                objclscommon.AddInParameters(objparm);
                objclscommon.ExecuteNonQuerySP("mobile_dbo.st_upd_technician_image");
            }

            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "UpdTechnicianInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return true;
        }
        public override List<Electrician_Services> GET_ElectricianServices(Electrician_Services objservices, ref int Totalnoofrecords)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                SqlDataReader objread;
                IDataParameter[] objparm = {
		new SqlParameter("@In_NoOfRecords", objservices.NoOfRecords!=0?objservices.NoOfRecords:null),
        new SqlParameter("@In_PageNo", objservices.PageNo!=0?objservices.PageNo:null),
        new SqlParameter("@In_OrderByItem", objservices.OrderByItem??null),
        new SqlParameter("@In_OrderBy", objservices.OrderBy??null)
	};
                objcommon.AddInParameters(objparm);

                objread = objcommon.GetDataReader("Help_dbo.st_Billing_LIST_AdminServices_RDPaging");
                var objgetservices = new List<Electrician_Services>();

                while (objread.Read())
                {
                    var objdata = new Electrician_Services();
                    if (objread["BillingService_ID"].ToString() != null)
                    {
                        objdata.BillingService_ID = (int)objread["BillingService_ID"];
                    }
                    if (objread["BillingServiceTitle"].ToString() != null & objread["BillingServiceTitle"].ToString() != "")
                    {
                        objdata.BillingServiceTitle = objread["BillingServiceTitle"].ToString();
                    }
                    else
                    {
                        objdata.BillingServiceTitle = null;
                    }
                    if (objread["BillingSeviceDescription"].ToString() != null & objread["BillingSeviceDescription"].ToString() != "")
                    {
                        objdata.BillingServiceDescription = objread["BillingSeviceDescription"].ToString();
                    }
                    else
                    {
                        objdata.BillingServiceDescription = null;
                    }
                    objgetservices.Add(objdata);
                }
                objread.NextResult();

                if (objread.HasRows == true)
                {
                    if (objread.Read())
                    {
                        Totalnoofrecords = (int)objread[0];
                    }

                }
                return objgetservices;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GET_ElectricianServices", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }

        }
        public override string Insert_ElectricianServices(Electrician_Services objservices)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_BillingService_ID", (objservices.BillingService_ID != 0 ? objservices.BillingService_ID : null)),
        new SqlParameter("@In_BillingServiceTitle",(objservices.BillingServiceTitle??null)),
        new SqlParameter("@In_BillingServiceDescription",(objservices.BillingServiceDescription??null)),
		new SqlParameter("@In_Quantity", (objservices.Quantity ?? null)),
        new SqlParameter("@In_CreatedBy",(objservices.CreatedBy!=0?objservices .CreatedBy:null)),
        new SqlParameter("@In_Status_Ind",(objservices.StatusInd??null)),
        new SqlParameter("@In_ChargeValues",(objservices.Chargevalues??null)),
        new SqlParameter("@In_IsRenewed",(objservices.IsRenewed??null)),
        new SqlParameter("@In_IsUpgradable",(objservices.IsUpgradable??null))
    	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Billing_DML_AdminServiceFee");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "Insert_ElectricianServices", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }

        public DataSet showElectricianService(Electrician_Services objservice)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams = { new SqlParameter("@In_BillingService_ID", objservice.BillingService_ID) };
                objcommon.AddInParameters(insparams);
                DataSet dsservidetail = new DataSet();
                dsservidetail = objcommon.GetDataSet("Help_dbo.st_Admin_GET_ServiceChargeTemp");
                return dsservidetail;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "showElectricianService", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetlectricianServiceDetails(Electrician_Services objservice)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams = { new SqlParameter("@In_BillingService_ID", objservice.BillingService_ID) };
                objcommon.AddInParameters(insparams);
                DataSet dsservidetail = new DataSet();
                dsservidetail = objcommon.GetDataSet("Help_dbo.st_Billing_Admin_GET_Servicedetails");
                return dsservidetail;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetlectricianServiceDetails", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Del_ServiceInfo(int BillingService_ID, int UpdatedBy)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_BillingService_ID", BillingService_ID),
		new SqlParameter("@in_UpdatedBy", UpdatedBy)
	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_Admin_DEL_Billingservice");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Del_ServiceInfo", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }

        }
        public override string Insert_ServiceChargeInfo(CCProcess ObjInsServiceCharge)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_ServiceSt", (ObjInsServiceCharge.ServiceSt  ?? null)),
		new SqlParameter("@In_CreatedBY", (ObjInsServiceCharge.CreatedBy ?? null))
		
    	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_New_Billing_INS_ServiceCharge");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "Insert_ServiceChargeInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override string upgrade_ServiceCharge(CCProcess ObjInsServiceCharge)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_ServiceSt", (ObjInsServiceCharge.ServiceSt  ?? null)),
		new SqlParameter("@In_CreatedBY", (ObjInsServiceCharge.CreatedBy ?? null)),
        new SqlParameter("@In_amount", (ObjInsServiceCharge.transactionAmount ?? null))
		
    	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_New_Billing_INS_upgrade_ServiceCharge");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "Insert_ServiceChargeInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override string Insert_ServicePaymentInfo(CCProcess ObjInsServicePayment)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_ServiceSt", (ObjInsServicePayment.ServiceSt  ?? null)),
		new SqlParameter("@In_CreatedBY", (ObjInsServicePayment.CreatedBy ?? null)),
        	new SqlParameter("@In_NextRenewDate", (ObjInsServicePayment.NextRenewDate  ?? null)),
		new SqlParameter("@in_CCTransaction_ID", (ObjInsServicePayment.CCTransaction_ID ?? null))
		
    	};
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_New_Billing_INS_ServicePayment");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "Insert_ServicePaymentInfo", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public string getpaymentType(int providerlogin)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@IN_providerlogin_id", providerlogin)				
    	};
                objclsDBWrapper.AddInParameters(objparm);
                DataSet dspaydetail = new DataSet();
                dspaydetail = objclsDBWrapper.GetDataSet("Help_dbo.St_Get_Defaultchargetype_Id");
                string strPaytype = string.Empty;
                if (dspaydetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dspaydetail.Tables[0].Rows)
                    {
                        if (dr["Defaultchargetype_id"] != null)
                        {
                            strPaytype = Convert.ToString(dr["Defaultchargetype_id"]);
                        }
                    }
                }

                //return dspaydetail;
                return strPaytype;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "getpaymentType", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public string getDebugMode(int providerlogin)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_Provider_id", providerlogin)				
    	};
                objclsDBWrapper.AddInParameters(objparm);
                DataSet dspaydetail = new DataSet();
                dspaydetail = objclsDBWrapper.GetDataSet("Help_dbo.ST_provider_get_Debug_mode");
                string strPaytype = string.Empty;
                if (dspaydetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dspaydetail.Tables[0].Rows)
                    {
                        if (dr["Debug_mode"] != null)
                        {
                            strPaytype = Convert.ToString(dr["Debug_mode"]);
                        }
                    }
                }

                //return dspaydetail;
                return strPaytype;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "SQLDATAACESSLAYER", "getDebugMode", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public void updatePaytype(int plId, int DefaultPaytype)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();

                IDataParameter[] objparm = {
			new SqlParameter("@In_providerlogin_id", plId),
		new SqlParameter("@In_Defaultcgargetype_id", DefaultPaytype)
                                           };
                objclscommon.AddInParameters(objparm);
                objclscommon.ExecuteNonQuerySP("Help_dbo.St_UPD_Defaultchargetype_Id");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "updatePaytype", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void updateDebugMode(int plId, string DefaultDebugtype)
        {
            try
            {
                clsCommonFunctions objclscommon = new clsCommonFunctions();

                IDataParameter[] objparm = {
			new SqlParameter("@In_Provider_id", plId),
		new SqlParameter("@In_Debug_mode", DefaultDebugtype)
                                           };
                objclscommon.AddInParameters(objparm);
                objclscommon.ExecuteNonQuerySP("Help_dbo.ST_provider_UPD_Debug_mode");
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "updateDebugMode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public override List<Emaillog> List_Emaillog(Emaillog EMlog, ref int Total_records)
        {
            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
		new SqlParameter("@in_Searchby", (EMlog.SearchBy != null ? EMlog.SearchBy : null)),
		new SqlParameter("@in_Categoryid", EMlog.CategoryiId),
		new SqlParameter("@in_Startdate", EMlog.StartDate),
		new SqlParameter("@in_enddate", EMlog.EndDate),
		new SqlParameter("@in_OrderBYItem", EMlog.OrderBYItem),
        new SqlParameter("@In_OrderBy", EMlog.OrderBy),
        new SqlParameter("@In_NoOfRecords", EMlog.NoOfRecords),
		new SqlParameter("@In_PageNo", EMlog.PageNo)
	};
                objcommon.AddInParameters(objInparam);
                SqlDataReader objread = objcommon.GetDataReader("Help_dbo.st_Email_CategorySearch_paging");
                var objemaillog = new List<Emaillog>();
                while (objread.Read())
                {
                    var objdata = new Emaillog();
                    if (objread["ROWNUMBER"].ToString() != null)
                    {
                        objdata.Rownum = Convert.ToInt32(objread["ROWNUMBER"]);
                    }
                    if (objread["EmailLog_ID"].ToString() != null & objread["EmailLog_ID"].ToString() != "")
                    {
                        objdata.EmailLog_ID = Convert.ToInt32(objread["EmailLog_ID"]);
                    }
                    if (objread["CreatedOn"].ToString() != null & objread["CreatedOn"].ToString() != "")
                    {
                        objdata.CreatedOn = Convert.ToString(objread["CreatedOn"]);
                    }
                    if (objread["EmailMessage_Category_ID"].ToString() != null & objread["EmailMessage_Category_ID"].ToString() != "")
                    {
                        objdata.EmailMsgCtgid = Convert.ToInt32(objread["EmailMessage_Category_ID"]);
                    }

                    if (objread["Message_Category_Description"].ToString() != null & objread["Message_Category_Description"].ToString() != "")
                    {
                        objdata.MsgCatDescription = objread["Message_Category_Description"].ToString();
                    }

                    if (objread["RefCount"].ToString() != null & objread["RefCount"].ToString() != "")
                    {
                        objdata.RefCount = objread["RefCount"].ToString();
                    }

                    if (objread["ReferenceName"].ToString() != null & objread["ReferenceName"].ToString() != "")
                    {
                        objdata.ReferenceName = objread["ReferenceName"].ToString();
                    }
                    objemaillog.Add(objdata);

                }
                if (objread.NextResult() == true)
                {
                    if (objread.Read())
                    {
                        Total_records = Convert.ToInt32(objread["Totalrecords"]);
                    }
                }
                return objemaillog;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "List_Emaillog", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Emaillog> DDL_category(Emaillog objct)
        {
            var objDataList = new List<Emaillog>();
            try
            {
                var objcommon = new clsCommonFunctions();
                var ds = objcommon.GetDataSet("Help_dbo.st_Email_categories");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    objDataList.AddRange(from DataRow dr in ds.Tables[0].Rows select new Emaillog { EmailMsgCtgid = Convert.ToInt32(dr["EmailMessage_Category_ID"]), MsgCatDescription = Convert.ToString(dr["Message_Category_Description"]) });
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "DDL_category", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override List<Referrals> Admin_List_Articles(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara = {
			new SqlParameter("@In_Title", list.Title),
			new SqlParameter("@In_Orderbyitem", list.OrderByItem),
			new SqlParameter("@in_OrderBy", list.OrderBy),
			new SqlParameter("@In_NoOfrecords", list.NoofRecords),
			new SqlParameter("@In_PageNo", list.PageNo),
			new SqlParameter("@in_ToDate", (!string.IsNullOrEmpty(list.Todate) ? list.Todate : null)),
			new SqlParameter("@in_FromDate", (!string.IsNullOrEmpty(list.FromDate) ? list.FromDate : null)),
			new SqlParameter("@In_Articlecategory_id", (list.Category_id > 0 ? list.Category_id : null)),
			new SqlParameter("@In_ArticleIndex_Id", (list.ArticalIndex > 0 ? list.ArticalIndex : null))
		};
                cls.AddInParameters(inpara);
                dr = cls.GetDataReader("Help_dbo.St_Admin_ListArticle_RDPaging");
                List<Referrals> mylist = new List<Referrals>();
                while (dr.Read())
                {
                    int slno = 0;
                    Referrals Articles = new Referrals();
                    slno += 1;
                    if (dr["Title"].ToString() != "" && dr["Title"].ToString() != null)
                    {
                        Articles.Title = dr["Title"].ToString();
                    }
                    if (dr["Article_Desc"].ToString() != null)
                    {
                        Articles.Article_Desc = dr["Article_Desc"].ToString();
                    }
                    if (dr["Article_ID"].ToString() != null)
                    {
                        Articles.Article_ID = Convert.ToInt32(dr["Article_ID"].ToString());
                    }
                    if (dr["Createdon"].ToString() != null)
                    {
                        Articles.CreatedOn = dr["Createdon"].ToString();
                    }
                    if (dr["Online_ind"].ToString() != null)
                    {
                        Articles.Online_ind = dr["Online_ind"].ToString();
                    }
                    //if (!Information.IsDBNull(dr("Imagepath")))
                    //{
                    //    Articles.Imagepath = dr("Imagepath");
                    //}

                    mylist.Add(Articles);
                }
                dr.NextResult();
                while (dr.Read())
                {
                    //Referrals Articles1 = new Referrals();
                    Referrals.TotalRecords = (int)dr[0]; ;
                }
                dr.Close();

                return mylist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Admin_List_Articles", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object Ins_Article(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara = {
			new SqlParameter("@in_Title", list.Title),
			new SqlParameter("@in_Article_Desc", list.Article_Desc),
			new SqlParameter("@In_Online_ind", list.Online_ind),
			new SqlParameter("@in_CreatedBy", list.CreatedBy),
			new SqlParameter("@in_CreatedOn", list.CreatedOn),
			new SqlParameter("@in_Imagepath", list.Imagepath),
			new SqlParameter("@In_RelatedArticle_IDs", (list.Related_Article_ID == null ? null : list.Related_Article_ID)),
			new SqlParameter("@In_Relatedvideo_IDs", (list.Related_video_id == null ? null : list.Related_video_id)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                IDataParameter[] outpara = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100),
			new SqlParameter("@out_Article_ID", SqlDbType.Int, 100)
		};
                cls.AddInParameters(inpara);
                cls.AddOutParameters(outpara);
                dr = cls.GetDataReader("Help_dbo.st_Admin_INS_Articles");
                string strout = null;
                int strArticalID = 0;

                if (cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != null && cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != "")
                {
                    strout = cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                    //If Not IsDBNull(dr("Out_Msg")) Then
                    //    strout = dr("Out_Msg")
                    return strout;
                }
                else
                {
                    if (cls.objdbCommandWrapper.Parameters["@out_Article_ID"].Value != null)
                    {
                        strArticalID = Convert.ToInt32(cls.objdbCommandWrapper.Parameters["@out_Article_ID"].Value);
                        return strArticalID;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Ins_Article", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override object upd_Articles(Referrals id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara = {
			new SqlParameter("@in_Article_ID", id.Article_ID),
			new SqlParameter("@in_Title", id.Title),
			new SqlParameter("@in_Article_Desc", id.Article_Desc),
			new SqlParameter("@In_Online_ind", id.Online_ind),
			new SqlParameter("@in_CreatedOn", id.CreatedOn),
			new SqlParameter("@in_Imagepath", id.Imagepath),
			new SqlParameter("@In_Provider_id", id.Provider_ID),
			new SqlParameter("@in_Author_Desc", id.Author_Desc),
			new SqlParameter("@In_RelatedArticle_IDs", (id.Related_Article_ID != null ? id.Related_Article_ID : null)),
			new SqlParameter("@In_Relatedvideo_IDs", (id.Related_video_id != null ? id.Related_video_id : null)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                IDataParameter[] outpara = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100) };
                cls.AddInParameters(inpara);
                cls.AddOutParameters(outpara);
                dr = cls.GetDataReader("Help_dbo.st_Admin_INS_Articles");
                string strout = null;
                if (cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    strout = cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                    return strout;
                }
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "upd_Articles", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override object Del_Article(int id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara = {
			new SqlParameter("@in_Article_ID", id),
			new SqlParameter("@in_DeletedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};

                cls.AddInParameters(inpara);
                dr = cls.GetDataReader("Help_dbo.st_Admin_Del_Articles");
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Del_Article", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<MediaInfo> GetMediaInfo1(MediaInfo objMediaInfo)
        {

            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();

                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			new SqlParameter("@In_NoOfRecords", objMediaInfo.NoOfrecords),
			new SqlParameter("@In_MediaTitle", objMediaInfo.MediaTitle),
			new SqlParameter("@In_OrderBy", objMediaInfo.OrderBy),
			new SqlParameter("@In_OrderBYitem", objMediaInfo.OrderByItem),
			new SqlParameter("@In_PageNo", objMediaInfo.PageNO),
			new SqlParameter("@in_exceptids", (!string.IsNullOrEmpty(objMediaInfo.strexplictids) ? objMediaInfo.strexplictids : null))
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Admin_LIST_HelpMedialist");

                List<MediaInfo> MediaInfo1 = new List<MediaInfo>();


                int SINo = objMediaInfo.NoOfrecords * (objMediaInfo.PageNO - 1);

                while (objread.Read())
                {
                    SINo += 1;
                    MediaInfo objMeInfo = new MediaInfo();
                    objMeInfo.SINo = Convert.ToInt32(SINo);
                    objMeInfo.Helpmedia_ID = (objread["Helpmedia_ID"] == DBNull.Value ? null : objread["Helpmedia_ID"].ToString());
                    objMeInfo.Reference_ID = (objread["Reference_ID"] == DBNull.Value ? 0 : Convert.ToInt32(objread["Reference_ID"]));
                    objMeInfo.ReferenceType_ID = (objread["ReferenceType_ID"] == DBNull.Value ? 0 : Convert.ToInt32(objread["ReferenceType_ID"]));
                    objMeInfo.Title = (objread["Title"] == DBNull.Value ? null : objread["Title"].ToString());
                    objMeInfo.Description = (objread["Description"] == DBNull.Value ? null : objread["Description"].ToString());
                    objMeInfo.Duration = (objread["Duration"] == DBNull.Value ? null : objread["Duration"].ToString());
                    objMeInfo.FileType = (objread["FileType"] == DBNull.Value ? null : objread["FileType"].ToString());
                    objMeInfo.FilePath = (objread["FilePath"] == DBNull.Value ? null : objread["FilePath"].ToString());
                    objMeInfo.FilePath_Encrypted = (objread["FilePath_Encrypted"] == DBNull.Value ? null : objread["FilePath_Encrypted"].ToString());
                    objMeInfo.ImagepathSuffix = (objread["ImagepathSuffix"] == DBNull.Value ? null : objread["ImagepathSuffix"].ToString());
                    objMeInfo.IsDisplayToPractice = (objread["IsDisplayToPractice"] == DBNull.Value ? null : objread["IsDisplayToPractice"].ToString());
                    objMeInfo.CreadtedOn = (objread["CreatedOn"] == DBNull.Value ? null : objread["CreatedOn"].ToString());
                    MediaInfo1.Add(objMeInfo);
                }
                objread.NextResult();

                while (objread.Read())
                {
                    MediaInfo.TotalRecords = Convert.ToInt32(objread["TotalRecords"]);
                }
                return MediaInfo1;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "GetMediaInfo1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet Get_videos(int id)
        {

            try
            {


                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@in_PublicVideo_ID", id) };
                objCommon.AddInParameters(inparam);
                DataSet dset = new DataSet();
                dset = objCommon.GetDataSet("Help_dbo.St_Admin_Get_videos");
                return dset;

            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Get_videos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }


        public override object Del_videos(int id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_PublicVideo_ID", id),
			new SqlParameter("@in_DeletedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.St_Admin_Del_videos");
                return null;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "Del_videos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override object upd_videos(Articlevideos list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara = {
			new SqlParameter("@in_PublicVideo_ID", list.PublicVideo_ID),
			new SqlParameter("@in_Title", list.Title),
			new SqlParameter("@in_Video_Description", list.Video_Description),
			new SqlParameter("@in_FileType", list.FileType),
			new SqlParameter("@in_Video_Path", list.Video_path),
			new SqlParameter("@in_File_Path", list.File_Path),
			new SqlParameter("@In_ImagePath", list.ImagePath),
			new SqlParameter("@In_EncryptedFile_Path", list.EncryptedFile_Path),
			new SqlParameter("@in_Createdby", list.Createdby),
			new SqlParameter("@In_Online_ind", list.Online_ind),
			new SqlParameter("@in_Duration", list.Duration),
			new SqlParameter("@In_Youtube_embededtext", (list.Youtube_embededtext != null ? list.Youtube_embededtext : null)),
			new SqlParameter("@In_RelatedVideo_IDS", list.ExceptVideo_Ids),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};

                cls.AddInParameters(inpara);
                dr = cls.GetDataReader("Help_dbo.st_Admin_INS_Videos");
                List<Articlevideos> mylist = new List<Articlevideos>();
                while (dr.Read())
                {
                    Articlevideos video = new Articlevideos();
                    if (dr["PublicVideo_ID"].ToString() != "" && dr["PublicVideo_ID"].ToString() != null)
                    {
                        video.PublicVideo_ID = dr["PublicVideo_ID"].ToString();
                    }
                    if (dr["Title"].ToString() != null && dr["Title"].ToString() != "")
                    {
                        video.Title = dr["Title"].ToString();
                    }
                    if (dr["EncryptedFile_Path"].ToString() != null && dr["EncryptedFile_Path"].ToString() != "")
                    {
                        video.EncryptedFile_Path = dr["EncryptedFile_Path"].ToString();
                    }
                    if (dr["EncryptedImagepath"].ToString() != null && dr["EncryptedImagepath"].ToString() != "")
                    {
                        video.EncryptedImagepath = dr["EncryptedImagepath"].ToString();
                    }
                    if (dr["ImagepathSuffix"].ToString() != null && dr["ImagepathSuffix"].ToString() != "")
                    {
                        video.ImagepathSuffix = dr["ImagepathSuffix"].ToString();
                    }
                    if (dr["Createdon"].ToString() != null && dr["Createdon"].ToString() != "")
                    {
                        video.CreatedOn = dr["Createdon"].ToString();
                    }
                    if (dr["Video_Description"].ToString() != null && dr["Video_Description"].ToString() != "")
                    {
                        video.CreatedOn = dr["Video_Description"].ToString();
                    }
                    if (dr["FILENAME"].ToString() != null && dr["FILENAME"].ToString() != "")
                    {
                        video.CreatedOn = dr["FILENAME"].ToString();
                    }
                    if (dr["Imagepath"].ToString() != null && dr["Imagepath"].ToString() != "")
                    {
                        video.CreatedOn = dr["Imagepath"].ToString();
                    }
                    if (dr["Duration"].ToString() != null && dr["Duration"].ToString() != "")
                    {
                        video.CreatedOn = dr["Duration"].ToString();
                    }
                    mylist.Add(video);
                }
                return mylist;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "upd_videos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override List<Articlevideos> List_Relatedvideos(Articlevideos video)
        {
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
                IDataParameter[] objparm = {
			new SqlParameter("@in_ParentVideo_ID", (video.ParentVideo_ID == null ? null : video.ParentVideo_ID)),
			new SqlParameter("@in_Title", video.Title),
			new SqlParameter("@In_Desc", video.Description),
			new SqlParameter("@in_ExceptVideo_Ids", (video.ExceptVideo_Ids == null ? null : video.ExceptVideo_Ids)),
			new SqlParameter("@in_NoOfRec", video.NoofRecords),
			new SqlParameter("@in_pageNo", video.PageNo),
			new SqlParameter("@in_orderBY", video.OrderBy),
			new SqlParameter("@in_OrderByItem", video.OrderByItem)
		};
                objcommon1.AddInParameters(objparm);
                SqlDataReader objread = objcommon1.GetDataReader("Help_dbo.St_List_RelatedVideos_Rdpaging");
                List<Articlevideos> ArticlesInfo = new List<Articlevideos>();
                int SINo = video.NoofRecords * (Convert.ToInt32(video.PageNo) - 1);
                while (objread.Read())
                {
                    SINo += 1;
                    Articlevideos obj = new Articlevideos();
                    obj.SINo = Convert.ToInt32(SINo);

                    if (objread["PublicVideo_ID"].ToString() != null && objread["PublicVideo_ID"].ToString() != "")
                    {
                        obj.PublicVideo_ID = objread["PublicVideo_ID"].ToString();
                    }
                    if (objread["Title"].ToString() != null && objread["Title"].ToString() != "")
                    {
                        obj.Title = objread["Title"].ToString();
                    }
                    if (objread["Video_Description"].ToString() != null && objread["Video_Description"].ToString() != "")
                    {
                        obj.Video_Description = objread["Video_Description"].ToString();
                    }
                    ArticlesInfo.Add(obj);
                }

                objread.NextResult();

                while (objread.Read())
                {
                    Articlevideos.TotalRecords = Convert.ToInt32(objread["TatalRec"]);
                }
                return ArticlesInfo;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "List_Relatedvideos", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }

        public override object Insert_videos(Articlevideos list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();

                IDataParameter[] inpara = {
			new SqlParameter("@in_Title", list.Title),
			new SqlParameter("@in_Video_Description", list.Video_Description),
			new SqlParameter("@in_File_Path", list.File_Path),
			new SqlParameter("@in_Video_Path", list.Video_path),
			new SqlParameter("@In_EncryptedFile_Path", list.EncryptedFile_Path),
			new SqlParameter("@In_ImagePath", list.ImagePath),
			new SqlParameter("@In_Online_ind", list.Online_ind),
			new SqlParameter("@In_EncryptedImagePath", list.EncryptedImagepath),
			new SqlParameter("@in_Createdby", list.Createdby),
			new SqlParameter("@in_Duration", list.Duration),
			new SqlParameter("@In_Youtube_embededtext", (list.Youtube_embededtext != null ? list.Youtube_embededtext : null)),
			new SqlParameter("@In_RelatedVideo_IDS", list.ExceptVideo_Ids),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};


                IDataParameter[] outpara = {
			new SqlParameter("@Out_Msg", SqlDbType.VarChar, 100),
			new SqlParameter("@Loc_ImagePathSuffix", SqlDbType.NVarChar, 500)
		};
                cls.AddInParameters(inpara);
                cls.AddOutParameters(outpara);
                cls.ExecuteNonQuerySP("Help_dbo.st_Admin_INS_Videos");
                string ImagePathSuffix = null;

                if (cls.objdbCommandWrapper.Parameters["@Loc_ImagePathSuffix"].Value.ToString() != "" && cls.objdbCommandWrapper.Parameters["@Loc_ImagePathSuffix"].Value.ToString() != null)
                {
                    ImagePathSuffix = cls.objdbCommandWrapper.Parameters["@Loc_ImagePathSuffix"].Value.ToString();
                }
                if (cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != "" && cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString() != null)
                {
                    Articlevideos.Out_msg = cls.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }
                return ImagePathSuffix;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "Insert_videos", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Articlevideos> List_videos(Articlevideos list, string VideoIDs, string tabInd = null)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                if (!string.IsNullOrEmpty(tabInd))
                {
                    IDataParameter[] inpara1 = { new SqlParameter("@In_VideoIDs", (!string.IsNullOrEmpty(VideoIDs) ? VideoIDs : null)) };

                    cls.AddInParameters(inpara1);
                    dr = cls.GetDataReader("Help_dbo.st_public_List_MostPopularVideos");
                }
                else
                {
                    IDataParameter[] inpara = {
				new SqlParameter("@IN_Online_Ind", list.Online_ind),
				new SqlParameter("@In_OrderByItem", list.OrderByItem),
				new SqlParameter("@In_Orderby", list.OrderBy),
				new SqlParameter("@In_NoOfrecords", list.NoofRecords),
				new SqlParameter("@In_pageNo", list.PageNo),
				new SqlParameter("@In_bydescription", list.Video_Description),
				new SqlParameter("@In_bytitle", list.Title),
                new SqlParameter("@in_StartDate", list.Startdate),
                new SqlParameter("@in_EndDate", list.Enddate)
			};
                    cls.AddInParameters(inpara);
                    dr = cls.GetDataReader("Help_dbo.st_Admin_list_videos_RDPAGING");
                }

                List<Articlevideos> mylist = new List<Articlevideos>();
                while (dr.Read())
                {
                    Articlevideos video = new Articlevideos();
                    if (dr["PublicVideo_ID"].ToString() != null && dr["PublicVideo_ID"].ToString() != "")
                    {
                        video.PublicVideo_ID = dr["PublicVideo_ID"].ToString();
                    }
                    if (dr["Title"].ToString() != null && dr["Title"].ToString() != "")
                    {
                        video.Title = dr["Title"].ToString();
                    }
                    if (dr["EncryptedFile_Path"].ToString() != null && dr["EncryptedFile_Path"].ToString() != "")
                    {
                        video.EncryptedFile_Path = dr["EncryptedFile_Path"].ToString();
                    }
                    if (dr["ImagepathSuffix"].ToString() != null && dr["ImagepathSuffix"].ToString() != "")
                    {
                        video.ImagepathSuffix = dr["ImagepathSuffix"].ToString();
                    }
                    if (dr["Createdon"].ToString() != null && dr["Createdon"].ToString() != "")
                    {
                        video.CreatedOn = dr["Createdon"].ToString();
                    }
                    if (dr["Video_Description"].ToString() != null && dr["Video_Description"].ToString() != "")
                    {
                        video.Video_Description = dr["Video_Description"].ToString();
                    }

                    if (dr["FILENAME"].ToString() != null && dr["FILENAME"].ToString() != "")
                    {
                        video.FILENAME = dr["FILENAME"].ToString();
                    }
                    if (dr["File_Path"].ToString() != null && dr["File_Path"].ToString() != "")
                    {
                        video.File_Path = dr["File_Path"].ToString();
                    }
                    if (dr["IMAGENAME"].ToString() != null && dr["IMAGENAME"].ToString() != "")
                    {
                        video.IMAGENAME = dr["IMAGENAME"].ToString();
                    }
                    if (dr["Online_ind"].ToString() != null && dr["Online_ind"].ToString() != "")
                    {
                        video.Online_ind = dr["Online_ind"].ToString();
                    }
                    if (dr["Duration"].ToString() != null && dr["Duration"].ToString() != "")
                    {
                        video.Duration = dr["Duration"].ToString();
                    }
                    if (dr["Video_path"].ToString() != null && dr["Video_path"].ToString() != "")
                    {
                        video.Video_path = dr["Video_path"].ToString();
                    }
                    if (dr["ViewdCount"].ToString() != null && dr["ViewdCount"].ToString() != "")
                    {
                        video.ViewdCount = Convert.ToInt32(dr["ViewdCount"].ToString());
                    }
                    if (dr["Emailcount"].ToString() != null && dr["Emailcount"].ToString() != "")
                    {
                        video.Emailcount = Convert.ToInt32(dr["Emailcount"].ToString());
                    }
                    mylist.Add(video);
                }
                dr.NextResult();
                while (dr.Read())
                {
                    Articlevideos.TotalRecords = (int)dr[0];
                }
                dr.Close();
                return mylist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "List_videos", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }

        public override DataSet Get_Relatedvideos(int videoid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@in_ParentVideo_ID", videoid) };
                objcommon.AddInParameters(inparam);
                DataSet dslink = new DataSet();
                dslink = objcommon.GetDataSet("Help_dbo.St_Videos_List_RelatedVideos");
                return dslink;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Relatedvideos", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet Get_RelatedArticle(int Articleid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@In_Parent_Article_ID", Articleid) };
                objcommon.AddInParameters(inparam);
                DataSet dslink = new DataSet();
                dslink = objcommon.GetDataSet("Help_dbo.st_Article_Get_RelatedArticle");
                return dslink;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_RelatedArticle", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override List<Referrals> List_publicArticles(Referrals list, ref int Total_Records)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);


                IDataParameter[] inpara2 = {
				new SqlParameter("@In_Online_ind", list.Online_ind),
				new SqlParameter("@In_OrderByItem", list.OrderByItem),
				new SqlParameter("@In_Orderby", list.OrderBy),
				new SqlParameter("@In_NoOfrecords", list.NoofRecords),
				new SqlParameter("@In_pageNo", list.PageNo),
				new SqlParameter("@In_Title", list.searchby),
				new SqlParameter("@In_Description", list.Description),
				new SqlParameter("@In_Provider_ID", list.Provider_ID)
			};
                cls.AddInParameters(inpara2);
                dr = cls.GetDataReader("Help_dbo.st_public_List_Articles");


                List<Referrals> mylist = new List<Referrals>();
                while (dr.Read())
                {
                    int slno = 0;
                    Referrals Articles = new Referrals();
                    slno += 1;
                    if (dr["Title"].ToString() != "" && dr["Title"].ToString() != null)
                    {
                        Articles.Title = dr["Title"].ToString();
                    }
                    if (dr["imagepath"].ToString() != "" && dr["imagepath"].ToString() != null)
                    {
                        Articles.Imagepath = dr["imagepath"].ToString();
                    }
                    if (dr["Article_Desc"].ToString() != "" && dr["Article_Desc"].ToString() != null)
                    {
                        Articles.Article_Desc = dr["Article_Desc"].ToString();
                    }
                    if (dr["Article_ID"].ToString() != "" && dr["Article_ID"].ToString() != null)
                    {
                        Articles.Article_ID = Convert.ToInt32(dr["Article_ID"].ToString());
                    }
                    if (dr["Createdon"].ToString() != "" && dr["Createdon"].ToString() != null)
                    {
                        Articles.CreatedOn = dr["Createdon"].ToString();
                    }
                    if (dr["Online_ind"].ToString() != "" && dr["Online_ind"].ToString() != null)
                    {
                        Articles.Online_ind = dr["Online_ind"].ToString();
                    }
                    if (dr["ViewdCount"].ToString() != "" && dr["ViewdCount"].ToString() != null)
                    {
                        Articles.ArticlesViewCount = Convert.ToInt32(dr["ViewdCount"].ToString());
                    }
                    if (dr["ReadCount"].ToString() != "" && dr["ReadCount"].ToString() != null)
                    {
                        Articles.ArticlesReadCount = Convert.ToInt32(dr["ReadCount"].ToString());
                    }
                    if (dr["Provider_Name"].ToString() != "" && dr["Provider_Name"].ToString() != null)
                    {
                        Articles.ProviderName = dr["Provider_Name"].ToString();
                    }
                    if (dr["PublicURL"].ToString() != "" && dr["PublicURL"].ToString() != null)
                    {
                        Articles.PublicURL = dr["PublicURL"].ToString();
                    }
                    if (dr["Author_Desc"].ToString() != "" && dr["Author_Desc"].ToString() != null)
                    {
                        Articles.Author_Desc = dr["Author_Desc"].ToString();
                    }
                    mylist.Add(Articles);
                }
                dr.NextResult();

                while (dr.Read())
                {

                    Total_Records = (int)dr[0];

                    //Referrals obj1 = new Referrals();
                    Referrals.TotalRecords = (int)dr[0];

                }
                //   dr.Close();
                return mylist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "List_publicArticles", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override List<Referrals> List_publicArticles1(Referrals list, string ArticleIDs, ref int Total_Records)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] inpara1 = { new SqlParameter("@in_ArticleIDs", (!string.IsNullOrEmpty(ArticleIDs) ? ArticleIDs : null)) };
                cls.AddInParameters(inpara1);
                dr = cls.GetDataReader("Help_dbo.st_public_List_MostPopularArticles");


                List<Referrals> mylist = new List<Referrals>();
                while (dr.Read())
                {
                    int slno = 0;
                    Referrals Articles = new Referrals();
                    slno += 1;
                    if (dr["Title"].ToString() != "" && dr["Title"].ToString() != null)
                    {
                        Articles.Title = dr["Title"].ToString();
                    }
                    if (dr["imagepath"].ToString() != "" && dr["imagepath"].ToString() != null)
                    {
                        Articles.Imagepath = dr["imagepath"].ToString();
                    }
                    if (dr["Article_Desc"].ToString() != "" && dr["Article_Desc"].ToString() != null)
                    {
                        Articles.Article_Desc = dr["Article_Desc"].ToString();
                    }
                    if (dr["Article_ID"].ToString() != "" && dr["Article_ID"].ToString() != null)
                    {
                        Articles.Article_ID = Convert.ToInt32(dr["Article_ID"].ToString());
                    }
                    if (dr["Createdon"].ToString() != "" && dr["Createdon"].ToString() != null)
                    {
                        Articles.CreatedOn = dr["Createdon"].ToString();
                    }
                    if (dr["Online_ind"].ToString() != "" && dr["Online_ind"].ToString() != null)
                    {
                        Articles.Online_ind = dr["Online_ind"].ToString();
                    }
                    if (dr["ViewdCount"].ToString() != "" && dr["ViewdCount"].ToString() != null)
                    {
                        Articles.ArticlesViewCount = Convert.ToInt32(dr["ViewdCount"].ToString());
                    }
                    if (dr["ReadCount"].ToString() != "" && dr["ReadCount"].ToString() != null)
                    {
                        Articles.ArticlesReadCount = Convert.ToInt32(dr["ReadCount"].ToString());
                    }
                    if (dr["Provider_Name"].ToString() != "" && dr["Provider_Name"].ToString() != null)
                    {
                        Articles.ProviderName = dr["Provider_Name"].ToString();
                    }
                    if (dr["PublicURL"].ToString() != "" && dr["PublicURL"].ToString() != null)
                    {
                        Articles.PublicURL = dr["PublicURL"].ToString();
                    }
                    if (dr["Author_Desc"].ToString() != "" && dr["Author_Desc"].ToString() != null)
                    {
                        Articles.Author_Desc = dr["Author_Desc"].ToString();
                    }
                    mylist.Add(Articles);
                }
                dr.NextResult();

                while (dr.Read())
                {

                    Total_Records = (int)dr[0];

                    //Referrals obj1 = new Referrals();
                    Referrals.TotalRecords = (int)dr[0];

                }
                //   dr.Close();
                return mylist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "List_publicArticles", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override List<Referrals> ListBindArticles(string strSearchCondition, string strText, Referrals list, ref int Total_Records)
        {
            try
            {
                SqlDataReader dr = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsArticles = new DataSet();
                IDataParameter[] objparam = {
			new SqlParameter("@In_SearchCond", (strSearchCondition != null ? strSearchCondition : null)),
			new SqlParameter("@In_Title", (strText != null ? strText : null)),
            new SqlParameter("@in_noofrowsperpage", list.NoofRecords),
				new SqlParameter("@in_pageno", list.PageNo),
			new SqlParameter("@in_Index_ID", list.Index_Id),
			new SqlParameter("@in_Article_ID", null)
		};
                objcommon.AddInParameters(objparam);
                dr = objcommon.GetDataReader("Help_dbo.st_Public_Get_IndexArticles");
                List<Referrals> mylist = new List<Referrals>();
                while (dr.Read())
                {
                    int slno = 0;
                    Referrals Articles = new Referrals();
                    slno += 1;
                    if (dr["Title"].ToString() != "" && dr["Title"].ToString() != null)
                    {
                        Articles.Title = dr["Title"].ToString();
                    }
                    if (dr["imagepath"].ToString() != "" && dr["imagepath"].ToString() != null)
                    {
                        Articles.Imagepath = dr["imagepath"].ToString();
                    }
                    if (dr["Article_Desc"].ToString() != "" && dr["Article_Desc"].ToString() != null)
                    {
                        Articles.Article_Desc = dr["Article_Desc"].ToString();
                    }
                    if (dr["Category_Title"].ToString() != "" && dr["Category_Title"].ToString() != null)
                    {
                        Articles.Category_Title = dr["Category_Title"].ToString();
                        HttpContext.Current.Session.Add("Category_Title", dr["Category_Title"].ToString());
                    }
                    if (dr["Article_ID"].ToString() != "" && dr["Article_ID"].ToString() != null)
                    {
                        Articles.Article_ID = Convert.ToInt32(dr["Article_ID"].ToString());
                    }
                    if (dr["Createdon"].ToString() != "" && dr["Createdon"].ToString() != null)
                    {
                        Articles.CreatedOn = dr["Createdon"].ToString();
                    }
                    //if (dr["Online_ind"].ToString() != "" && dr["Online_ind"].ToString() != null)
                    //{
                    //    Articles.Online_ind = dr["Online_ind"].ToString();
                    //}
                    if (dr["ReadCount"].ToString() != "" && dr["ReadCount"].ToString() != null)
                    {
                        Articles.ArticlesReadCount = Convert.ToInt32(dr["ReadCount"].ToString());
                    }
                    if (dr["Provider_ID"].ToString() != "" && dr["Provider_ID"].ToString() != null)
                    {
                        Articles.Provider_ID = Convert.ToInt32(dr["Provider_ID"].ToString());
                    }
                    //if (dr["ReadCount"].ToString() != "" && dr["ReadCount"].ToString()!=null)
                    //{
                    //    Articles.ArticlesReadCount = Convert.ToInt32(dr["ReadCount"].ToString());
                    //}
                    if (dr["Provider_Name"].ToString() != "" && dr["Provider_Name"].ToString() != null)
                    {
                        Articles.ProviderName = dr["Provider_Name"].ToString();
                    }
                    if (dr["PublicURL"].ToString() != "" && dr["PublicURL"].ToString() != null)
                    {
                        Articles.PublicURL = dr["PublicURL"].ToString();
                    }
                    if (dr["Author_Desc"].ToString() != "" && dr["Author_Desc"].ToString() != null)
                    {
                        Articles.Author_Desc = dr["Author_Desc"].ToString();
                    }
                    mylist.Add(Articles);
                }
                return mylist;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "ListBindArticles", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }


        public override object Ins_ArticleAuthor(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();

                IDataParameter[] inpara = {
			new SqlParameter("@In_Article_ID", list.Article_ID),
			new SqlParameter("@In_Provider_ID", list.Provider_ID),
			new SqlParameter("@In_Author_Desc", list.Description),
			new SqlParameter("@in_CreatedBy", list.CreatedBy)
		};

                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.St_Articles_INS_ArticleAuthors");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Ins_ArticleAuthor", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override object Ins_ArticleIndexing(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@In_ArticleCategory_ID", list.ArticleCategory_ID),
			new SqlParameter("@In_Article_ID", list.Article_ID),
			new SqlParameter("@In_CreatedBy", HttpContext.Current.Session["userid"])
		};
                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.st_Admin_Ins_ArticleIndexing");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Ins_ArticleIndexing", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override object Upd_ArticleIndexing(Referrals list)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Article_category_ids", list.ArticleCategory_IDs),
			new SqlParameter("@in_Article_Id", list.Article_ID),
			new SqlParameter("@In_UpdatedBy", HttpContext.Current.Session["userid"])
		};
                cls.AddInParameters(inpara);
                cls.GetDataReader("Help_dbo.St_Admin_UPD_ArticlesIndexing");
                return null;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Upd_ArticleIndexing", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override List<Referrals> Getarticles1(Referrals objarticles)
        {
            try
            {
                //Dim objarticles As New Referrals
                clsCommonFunctions objcommon1 = new clsCommonFunctions();

                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			new SqlParameter("@in_NoOfRec", objarticles.NoofRecords),
			new SqlParameter("@in_Title", objarticles.Title),
			new SqlParameter("@In_OrderBy", objarticles.OrderBy),
			new SqlParameter("@In_OrderBYitem", objarticles.OrderByItem),
			new SqlParameter("@In_PageNo", objarticles.PageNo),
			new SqlParameter("@in_ExceptArticle_Ids", (!string.IsNullOrEmpty(objarticles.strexplictids) ? objarticles.strexplictids : null)),
			new SqlParameter("@in_ParentArticle_ID", (!string.IsNullOrEmpty(objarticles.Related_Article_ID) ? objarticles.Related_Article_ID : null)),
			new SqlParameter("@In_Desc", (!string.IsNullOrEmpty(objarticles.Article_Desc) ? objarticles.Article_Desc : null))
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.St_List_RelatedArticles_Rdpaging");
                List<Referrals> Referrals1 = new List<Referrals>();
                int SINo = (int)objarticles.NoofRecords * (Convert.ToInt32(objarticles.PageNo) - 1);

                while (objread.Read())
                {
                    Referrals objarticles1 = new Referrals();
                    SINo += 1;
                    objarticles1.slno = Convert.ToInt32(SINo);
                    objarticles1.Title = Convert.ToString(objread["Title"] == null ? null : objread["Title"]);
                    objarticles1.Article_Desc = Convert.ToString(objread["Article_Desc"] == null ? null : objread["Article_Desc"]);
                    objarticles1.Article_ID = Convert.ToInt32(objread["Article_ID"] == null ? null : objread["Article_ID"]);
                    Referrals1.Add(objarticles1);
                }

                objread.NextResult();
                while (objread.Read())
                {
                    Referrals.TotalRecords = Convert.ToInt32(objread["TatalRec"]);
                }
                return Referrals1;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Getarticles1", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }

        public override System.Data.DataSet Get_Articles(int id)
        {
            try
            {
                //clsCommonFunctions cls = new clsCommonFunctions();
                //SqlDataReader dr = default(SqlDataReader);
                //IDataParameter[] inpara = { new SqlParameter("@in_Article_ID", id) };
                //cls.AddInParameters(inpara);
                //dr = cls.GetDataReader("Help_dbo.st_Admin_Get_Articles");
                //Referrals Article = new Referrals();
                //if (dr.Read() == true)
                //{
                //    if (dr["Title"]!=null)
                //    {
                //        Article.Title = Convert.ToString(dr["Title"]);
                //    }
                //    if (dr["Article_Desc"]!=null)
                //    {
                //        Article.Article_Desc = Convert.ToString(dr["Article_Desc"]);
                //    }
                //    if (dr["Online_Ind"]!=null)
                //    {
                //        Article.Online_ind = Convert.ToString(dr["Online_Ind"]);
                //    }
                //    if (dr["Imagepath"]!=null)
                //    {
                //        Article.Imagepath = Convert.ToString(dr["Imagepath"]);
                //    }
                //    if (dr["Createdon"]!=null)
                //    {
                //        Article.CreatedOn = Convert.ToString(dr["Createdon"]);
                //    }
                //    if (dr["Provider_ID"]!=null)
                //    {
                //        Article.Provider_ID = Convert.ToInt32(dr["Provider_ID"]);
                //    }
                //    if (dr["PublicURL"]!=null)
                //    {
                //        Article.PublicURL = Convert.ToString(dr["PublicURL"]);
                //    }
                //    if (dr["Provider_Name"]!=null)
                //    {
                //        Article.ProviderName = Convert.ToString(dr["Provider_Name"]);
                //    }
                //    if (dr["Author_Desc"] != null)
                //    {
                //        Article.Author_Desc = Convert.ToString(dr["Author_Desc"]);
                //    }
                //    //If Not IsDBNull(dr("RelatedArticle_IDs")) Then
                //    //    Article.Related_Article_ID = dr("RelatedArticle_IDs")
                //    //End If
                //}
                //return Article;


                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@in_Article_ID", id) };
                objCommon.AddInParameters(inparam);
                DataSet dset = new DataSet();
                dset = objCommon.GetDataSet("Help_dbo.st_Admin_Get_Articles");
                return dset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Articles", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet Get_ArticleIndexing(int id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = { new SqlParameter("@In_Article_ID", id) };
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.st_Admin_Get_ArticleIndexing");
                return dataset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_ArticleIndexing", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override DataSet GetArticlesORVideosCount(string SiteStatisticID, string AVInd, string Title = null, string Desc = null)
        {
            try
            {
                DataSet dsIDs = new DataSet();
                // Warning!!! Optional parameters not supported
                // Warning!!! Optional parameters not supported
                if ((AVInd == "V"))
                {
                    clsCommonFunctions clscomm = new clsCommonFunctions();
                    IDataParameter[] objparam = {
		 new SqlParameter("@in_Statistic_ID", ( (SiteStatisticID != "") ? SiteStatisticID : null )),
		 new SqlParameter("@In_Title", ( (Title != "") ? Title : null )),
         new SqlParameter("@In_Description", ( (Desc != "") ? Desc : null ))
	};
                    clscomm.AddInParameters(objparam);
                    dsIDs = clscomm.GetDataSet("Help_dbo.st_Public_List_VideosViewdCount");
                }
                else
                {
                    clsCommonFunctions clscomm = new clsCommonFunctions();
                    IDataParameter[] objparam = {
		 new SqlParameter("@in_Statistic_ID", ( (SiteStatisticID != "") ? SiteStatisticID : null )),
		 new SqlParameter("@In_Title", ( (Title != "") ? Title : null )),
         new SqlParameter("@In_Description", ( (Desc != "") ? Desc : null ))
	};
                    clscomm.AddInParameters(objparam);
                    dsIDs = clscomm.GetDataSet("Help_dbo.st_Public_List_ArticlesViewdCount");
                }
                return dsIDs;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetArticlesORVideosCount", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }
        public override DataSet Get_RelatedArticleID(int id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = { new SqlParameter("@In_Parent_Article_ID", id) };
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.st_Article_Get_RelatedArticle");
                return dataset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_RelatedArticleID", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override DataSet Get_RelatedvideoID(int id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = { new SqlParameter("@In_Article_ID", id) };
                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.st_Article_Get_Relatedvideos");
                return dataset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_RelatedvideoID", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }


        public override DataSet list_ArticlesCategories(string strSearchCondition, string strText)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsArticles = new DataSet();
                IDataParameter[] objParam = {
			new SqlParameter("@In_SearchCond", (strSearchCondition != null ? strSearchCondition : null)),
			new SqlParameter("@In_Title", (strText != null ? strText : null))
		};
                objcommon.AddInParameters(objParam);
                dsArticles = objcommon.GetDataSet("Help_dbo.st_Admi_list_ArticlesCategories");
                return dsArticles;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "list_ArticlesCategories", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }



        public override List<Articlevideos> list_CategorieswiseIndexes(int CategoryID, string strSearchCondition, string strText)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader dr = default(SqlDataReader);
                IDataParameter[] objParam = {
			new SqlParameter("@in_Category_ID", CategoryID),
			new SqlParameter("@In_SearchCond", (strSearchCondition != null ? strSearchCondition : null)),
			new SqlParameter("@In_Title", (strText != null ? strText : null))
		};
                objcommon.AddInParameters(objParam);

                dr = objcommon.GetDataReader("Help_dbo.st_Admin_list_CategorieswiseIndexes");
                List<Articlevideos> mylist = new List<Articlevideos>();
                while (dr.Read())
                {
                    Articlevideos index = new Articlevideos();
                    if (dr["ArticleCategory_ID"].ToString() != null && dr["ArticleCategory_ID"].ToString() != "")
                    {
                        index.ArticleIndexId = Convert.ToInt32(dr["ArticleCategory_ID"].ToString());
                    }
                    if (dr["Title"].ToString() != null && dr["Title"].ToString() != "")
                    {
                        index.Title = dr["Title"].ToString();
                    }

                    mylist.Add(index);
                }
                return mylist;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "list_CategorieswiseIndexes", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }
        public override string Insert_Forum(Forums objforum)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objinparamlist = {
			new SqlParameter("@in_ForumName", objforum.ForumName),
			new SqlParameter("@in_Login_ID", (objforum.Login_ID != 0 ? objforum.Login_ID : null)),
			new SqlParameter("@in_Message", objforum.Message),
			new SqlParameter("@in_Public_Ind", objforum.Public_Ind),
			new SqlParameter("@in_ReadTopicsPosts", objforum.ReadTopicsPosts),
			new SqlParameter("@in_PostNewTopic", objforum.PostNewTopics),
			new SqlParameter("@in_PostNewReplies", objforum.PostNewReplies),
			new SqlParameter("@In_ModeratorForum_Ind", objforum.ModeratorForum_Ind),
			new SqlParameter("@in_Role_ID", (objforum.Role_ID == 0 ? null : objforum.Role_ID))
		};
                objcommon.AddInParameters(objinparamlist);
                IDataParameter[] strOutPram = { new SqlParameter("@Out_Msg", SqlDbType.VarChar, 500) };
                objcommon.AddOutParameters(strOutPram);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_INS_Forum");
                string stroutmsg = null;
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value != null)
                {
                    stroutmsg = objcommon.objdbCommandWrapper.Parameters["@Out_Msg"].Value.ToString();
                }

                return stroutmsg;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Insert_Forum", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }
        public override string INS_UPD_Forum_Moderators(Forums obj)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();

                IDataParameter[] ObjInParams = {
			new SqlParameter("@in_Moderator_ID", obj.Moderator_ID),
			new SqlParameter("@in_Forum_ID", obj.Forum_ID),
			new SqlParameter("@in_ReferenceType_ID", (obj.ReferenceType_ID == 0 ? null : obj.ReferenceType_ID)),
			new SqlParameter("@in_Reference_IDs", obj.Reference_IDs),
			new SqlParameter("@in_CreatedBy", (obj.Login_ID == 0 ? null : obj.Login_ID))
		};

                objCommon.AddInParameters(ObjInParams);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_INS_UPD_Forum_Moderators");

                return string.Empty;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "INS_UPD_Forum_Moderators", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }
        public override List<Forums> Get_Forum(Forums objforum)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);

                IDataParameter[] objinparamlist = {
			new SqlParameter("@in_Forum_ID", (objforum.Forum_ID != 0 ? objforum.Forum_ID : null)),
			new SqlParameter("@in_OrderByItem", (objforum.OrderByItem!= null ? objforum.OrderByItem : null)),
			new SqlParameter("@in_OrderBy", (objforum.OrderBy!= null ? objforum.OrderBy : null)),
			new SqlParameter("@In_Keyword", (objforum.Keyword!= null ? objforum.Keyword : null)),
			new SqlParameter("@in_StartDate", (objforum.StartDate!= null ? objforum.StartDate : null)),
			new SqlParameter("@in_EndDate", (objforum.EndDate!= null ? objforum.EndDate : null)),
			new SqlParameter("@In_NoOfRecords", (objforum.NoofRecords != 0 ? objforum.NoofRecords : null)),
			new SqlParameter("@In_Pageno", (objforum.PageNo != 0 ? objforum.PageNo : null))
		};

                objcommon.AddInParameters(objinparamlist);
                objread = objcommon.GetDataReader("Help_dbo.st_GETForum");
                List<Forums> objforumlist = new List<Forums>();
                while (objread.Read())
                {
                    Forums foruminfo = new Forums();
                    foruminfo.Slno = Convert.ToInt32(objread["ROWNUMBER"] != null ? objread["ROWNUMBER"] : null);
                    foruminfo.Forum_ID = Convert.ToInt32(objread["Forum_ID"] != null ? objread["Forum_ID"] : null);
                    foruminfo.ForumName = Convert.ToString(objread["ForumName"] != null ? objread["ForumName"] : null);
                    foruminfo.Message = Convert.ToString(objread["Message"] != null ? objread["Message"] : null);
                    foruminfo.Public_Ind = Convert.ToString(objread["Public_Ind"] != null ? objread["Public_Ind"] : null);
                    foruminfo.ReadTopicsPosts = Convert.ToString(objread["ReadTopicsPosts"] != null ? objread["ReadTopicsPosts"] : null);
                    foruminfo.PostNewTopics = Convert.ToString(objread["PostNewTopics"] != null ? objread["PostNewTopics"] : null);
                    foruminfo.PostNewReplies = Convert.ToString(objread["PostNewReplies"] != null ? objread["PostNewReplies"] : null);
                    foruminfo.Role_ID = Convert.ToInt32(objread["Role_ID"] != null ? objread["Role_ID"] : null);
                    foruminfo.ModeratorForum_Ind = Convert.ToString(objread["ModeratorForum_Ind"] != null ? objread["ModeratorForum_Ind"] : null);
                    foruminfo.Login_ID = Convert.ToInt32(objread["Login_ID"] != null ? objread["Login_ID"] : null);
                    foruminfo.Topics = Convert.ToInt32(objread["Topics"] != null ? objread["Topics"] : 0);
                    foruminfo.Replies = Convert.ToInt32(objread["Replies"] != null ? objread["Replies"] : 0);
                    foruminfo.Name = Convert.ToString(objread["Name"] != null ? objread["Name"] : null);
                    foruminfo.LatestPost = Convert.ToString(objread["LatestPost"] != null ? objread["LatestPost"] : null);
                    objforumlist.Add(foruminfo);
                }
                objread.NextResult();

                while (objread.Read())
                {
                    Forums.TotalRecords = Convert.ToInt32(objread[0]);
                }
                return objforumlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Forum", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            //return null;
        }

        public override bool Del_Forum(int Forum_ID)
        {

            try
            {

                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = { new SqlParameter("@in_Forum_ID", Forum_ID) };

                var _with1 = objcommon;
                _with1.AddInParameters(objInparam);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Forum_DELETE");
                return true;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Del_Forum", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;

        }

        public override bool Update_Forum(Forums objforum)
        {
            try
            {
                clsDBWrapper objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objInparam = {
			new SqlParameter("@in_Forum_ID", (objforum.Forum_ID != 0 ? objforum.Forum_ID : null)),
			new SqlParameter("@in_ForumName", (!string.IsNullOrEmpty(objforum.ForumName) ? objforum.ForumName : null)),
			new SqlParameter("@in_Login_ID", (objforum.Login_ID != 0 ? objforum.Login_ID : null)),
			new SqlParameter("@in_Message", objforum.Message),
			new SqlParameter("@in_ReadTopicsPosts", objforum.ReadTopicsPosts),
			new SqlParameter("@in_PostNewTopic", objforum.PostNewTopics),
			new SqlParameter("@in_PostNewReplies", objforum.PostNewReplies),
			new SqlParameter("@in_Role_ID", (objforum.Role_ID != 0 ? objforum.Role_ID : null)),
			new SqlParameter("@in_Public_ind", objforum.Public_Ind),
			new SqlParameter("@In_ModeratorForum_Ind", objforum.ModeratorForum_Ind)
		};
                objclsDBWrapper.AddInParameters(objInparam);
                objclsDBWrapper.ExecuteNonQuerySP("Help_dbo.st_UPD_Forum");

                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Update_Forum", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override List<Forums> Get_Topics_forum(Forums objforum)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);

                IDataParameter[] objinparamlist = {
			new SqlParameter("@in_ForumID", (objforum.Forum_ID != 0 ? objforum.Forum_ID : null)),
			new SqlParameter("@In_OrderByItem", (!string.IsNullOrEmpty(objforum.OrderByItem) ? objforum.OrderByItem : null)),
			new SqlParameter("@In_OrderBy", (!string.IsNullOrEmpty(objforum.OrderBy) ? objforum.OrderBy : null)),
			new SqlParameter("@in_NoOfRecords", (objforum.NoofRecords != 0 ? objforum.NoofRecords : null)),
			new SqlParameter("@In_PageNo", (objforum.PageNo != 0 ? objforum.PageNo : null)),
			new SqlParameter("@In_PublicInd", (objforum.Public_Ind != null ? objforum.Public_Ind : null)),
			new SqlParameter("@In_Login_ID", (objforum.Login_ID != 0 ? objforum.Login_ID : null)),
			new SqlParameter("@in_StartDate", (!string.IsNullOrEmpty(objforum.StartDate) ? objforum.StartDate : null)),
			new SqlParameter("@in_EndDate", (!string.IsNullOrEmpty(objforum.EndDate) ? objforum.EndDate : null)),
			new SqlParameter("@In_Keyword", (!string.IsNullOrEmpty(objforum.Keyword) ? objforum.Keyword : null)),
			new SqlParameter("@in_Role_id", (objforum.Role_ID != 0 ? objforum.Role_ID : null))
		};
                objcommon.AddInParameters(objinparamlist);
                objread = objcommon.GetDataReader("Help_dbo.st_GET_Forums_Topics_RDpaging");
                List<Forums> objlist = new List<Forums>();
                //Dim SLno As Integer = objlist.NoofRecords * (objlist.PageNo - 1)
                while (objread.Read())
                {
                    //SLno += 1
                    Forums foruminfo = new Forums();

                    foruminfo.Forum_ID = Convert.ToInt32(objread["Forum_ID"] != null ? objread["Forum_ID"] : -1);
                    foruminfo.ForumName = Convert.ToString(objread["ForumName"] != null ? objread["ForumName"] : null);
                    foruminfo.Moderator = Convert.ToString(objread["Moderator"] != null ? objread["Moderator"] : null);
                    foruminfo.Message = Convert.ToString(objread["Message"] != null ? objread["Message"] : null);
                    foruminfo.Topics = Convert.ToInt32(objread["Topics"] != null ? objread["Topics"] : -1);
                    foruminfo.Replies = Convert.ToInt32(objread["Replies"] != null ? objread["Replies"] : -1);
                    foruminfo.LastActivity = Convert.ToString(objread["LastActivity"] != null ? objread["LastActivity"] : null);
                    foruminfo.ReadTopicsPosts = Convert.ToString(objread["ReadTopicsPosts"]);
                    foruminfo.PostNewTopics = Convert.ToString(objread["PostNewTopics"]);
                    foruminfo.PostNewReplies = Convert.ToString(objread["PostNewReplies"]);
                    foruminfo.Login_ID = Convert.ToInt32(objread["Login_ID"] != null ? objread["Login_ID"] : null);
                    foruminfo.Role_ID = Convert.ToInt32(objread["Role_ID"] != null ? objread["Role_ID"] : -1);
                    foruminfo.ModeratorForum_Ind = Convert.ToString(objread["ModeratorForum_Ind"]);

                    objlist.Add(foruminfo);
                }

                objread.NextResult();


                while (objread.Read())
                {
                    Forums.TotalRecords = Convert.ToInt32(objread[0]);
                }


                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Topics_forum", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }

        public override List<Forums> Search_ForumTopic(Forums obj)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
			new SqlParameter("@in_TopicID", (obj.TOPIC_ID == 0 ? null : obj.TOPIC_ID)),
			new SqlParameter("@in_MessageID", (obj.MessageID != 0 ? obj.MessageID : null)),
			new SqlParameter("@In_Keyword", obj.Keyword),
			new SqlParameter("@in_Date", null),
			new SqlParameter("@In_Forum_ID", (obj.Forum_ID != 0 ? obj.Forum_ID : null)),
			new SqlParameter("@In_Status_Ind", obj.Status_Ind),
			new SqlParameter("@In_OrderByItem", obj.OrderByItem),
			new SqlParameter("@In_OrderBy", obj.OrderBy),
			new SqlParameter("@in_NoOfRecords", obj.NoofRecords),
			new SqlParameter("@in_StartDate", obj.StartDate),
			new SqlParameter("@in_EndDate", obj.EndDate),
			new SqlParameter("@In_PageNo", obj.PageNo)
		};
                objcommon.AddInParameters(objInparam);
                objread = objcommon.GetDataReader("Help_dbo.ST_GET_Messages_For_Topic");
                Forums objforum = new Forums();
                List<Forums> objlist = new List<Forums>();
                while (objread.Read())
                {
                    objforum = new Forums(Convert.ToString(objread["image_Name"] != null ? objread["image_Name"] : null), Convert.ToString(objread["MessageName"] != null ? objread["MessageName"] : null), Convert.ToString(objread["UserName"] != null ? objread["UserName"] : null), Convert.ToString(objread["MessageDescription"] != null ? objread["MessageDescription"] : null), Convert.ToString(objread["Message_ID"] != null ? objread["Message_ID"] : null), Convert.ToString(objread["Topicname"] != null ? objread["Topicname"] : null), Convert.ToString(objread["CreatedOn"] != null ? objread["CreatedOn"] : null), Convert.ToString(objread["Status_Ind"] != null ? objread["Status_Ind"] : null));

                    objlist.Add(objforum);

                }
                objread.NextResult();

                while (objread.Read())
                {
                    Forums.TotalRecords = Convert.ToInt32(objread[0]);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Search_ForumTopic", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Forums> Search_ForumMessages(Forums obj)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objInparam = {
			new SqlParameter("@in_TopicID", (obj.TOPIC_ID != 0 ? obj.TOPIC_ID : null)),
			new SqlParameter("@in_ForumID", (obj.Forum_ID != 0 ? obj.Forum_ID : null)),
			new SqlParameter("@In_Keyword", obj.Keyword),
			new SqlParameter("@in_Date", null),
			new SqlParameter("@In_Status_Ind", obj.Status_Ind),
			new SqlParameter("@In_OrderByItem", obj.OrderByItem),
			new SqlParameter("@In_OrderBy", obj.OrderBy),
			new SqlParameter("@in_NoOfRecords", (obj.NoofRecords != 0 ? obj.NoofRecords : null)),
			new SqlParameter("@in_StartDate", obj.StartDate),
			new SqlParameter("@in_EndDate", obj.EndDate),
			new SqlParameter("@In_PageNo", (obj.PageNo != 0 ? obj.PageNo : null))
		};
                objcommon.AddInParameters(objInparam);
                objread = objcommon.GetDataReader("Help_dbo.st_GET_Topics_Messages");
                Forums objforum = new Forums();
                List<Forums> objlist = new List<Forums>();
                while (objread.Read())
                {
                    objforum = new Forums(Convert.ToInt32(objread["TOPIC_ID"] != null ? objread["TOPIC_ID"] : -1), Convert.ToString(objread["TopicName"] != null ? objread["TopicName"] : null), Convert.ToString(objread["Image_Name"] != null ? objread["Image_Name"] : null), Convert.ToString(objread["Author"] != null ? objread["Author"] : null), Convert.ToInt32(objread["Replies"] != null ? objread["Replies"] : -1), Convert.ToString(objread["LastActivity"] != null ? objread["LastActivity"] : null), Convert.ToString(objread["CreatedBy"] != null ? objread["CreatedBy"] : null), Convert.ToString(objread["TopicDescription"] != null ? objread["TopicDescription"] : null), Convert.ToString(objread["Status_Ind"] != null ? objread["Status_Ind"] : null), Convert.ToString(objread["ForumDescription"] != null ? objread["ForumDescription"] : null));
                    objlist.Add(objforum);

                }
                objread.NextResult();

                while (objread.Read())
                {
                    Forums.TotalRecords = Convert.ToInt32(objread[0]);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Search_ForumMessages", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override bool Insert_Topic(Forums objforum)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objinparamlist = {
			new SqlParameter("@in_Topic_ID", (objforum.TOPIC_ID != 0 ? objforum.TOPIC_ID : null)),
			new SqlParameter("@in_Forum_ID", (objforum.Forum_ID != 0 ? objforum.Forum_ID : null)),
			new SqlParameter("@in_TopicName", (!string.IsNullOrEmpty(objforum.TopicName) ? objforum.TopicName : null)),
			new SqlParameter("@in_TopicDescription", (!string.IsNullOrEmpty(objforum.TopicDescription) ? objforum.TopicDescription : null)),
			new SqlParameter("@in_image_Name", (!string.IsNullOrEmpty(objforum.Image_Name) ? objforum.Image_Name : null)),
			new SqlParameter("@in_Reply_ID", (!string.IsNullOrEmpty(objforum.Reply_ID) ? objforum.Reply_ID : null)),
			new SqlParameter("@In_IsMainMessage", "Y"),
			new SqlParameter("@In_Reply_Ind", objforum.Reply_Ind),
			new SqlParameter("@in_CreatedBy", (objforum.CreatedBy != "0" ? objforum.CreatedBy : null)),
			new SqlParameter("@In_Status_Ind", objforum.Status_Ind)
		};
                objcommon.AddInParameters(objinparamlist);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_INS_ForumTopic");

                return true;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Insert_Topic", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }

        public override bool Del_FormTopics(int TopicID, int UpdatedBy)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
			new SqlParameter("@In_TopicID", TopicID),
			new SqlParameter("@in_UpdatedBy", UpdatedBy)
		};


                var _with1 = common;
                _with1.AddInParameters(InParamlist);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Del_Topics");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Del_FormTopics", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override bool Upd_TopicStatus(int TopicID, string Status_Ind)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_TopicID", TopicID),
			new SqlParameter("@In_StatusInd", Status_Ind)
		};
                var _with1 = common;
                _with1.AddInParameters(InParamList);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Upd_TopicStatus");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Upd_TopicStatus", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override List<Forums> Get_FourmTopicDescription(int Forum_ID, int? Topic_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dset = new DataSet();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] strParam = {
			new SqlParameter("@Forum_ID", Forum_ID),
			new SqlParameter("@topic_Id", (Topic_ID != 0 ? Topic_ID : null))
		};
                objcommon.AddInParameters(strParam);
                objread = objcommon.GetDataReader("Help_dbo.st_GET_Form_Messages");
                List<Forums> objlist = new List<Forums>();
                while (objread.Read())
                {
                    Forums obj = new Forums();
                    if (objread["ForumName"].ToString() != "")
                    {
                        obj.ForumName = objread["ForumName"].ToString();
                    }
                    else
                    {
                        obj.ForumName = null;
                    }

                    if (objread["Message"].ToString() != "")
                    {
                        obj.Message = objread["Message"].ToString();
                    }
                    else
                    {
                        obj.Message = null;
                    }

                    if (objread["TopicName"].ToString() != "")
                    {
                        obj.TopicName = objread["TopicName"].ToString();
                    }
                    else
                    {
                        obj.TopicName = null;
                    }
                    if (objread["TopicDescription"].ToString() != "")
                    {
                        obj.TopicDescription = objread["TopicDescription"].ToString();
                    }
                    else
                    {
                        obj.TopicDescription = null;
                    }

                    objlist.Add(obj);
                }
                return objlist;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_FourmTopicDescription", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override List<Forums> Get_Moderators(int? Forum_ID)
        {
            try
            {
                SqlDataReader objread = default(SqlDataReader);
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] objParam = { new SqlParameter("@in_Forum_ID", (Forum_ID != 0 ? Forum_ID : null)) };
                objCommon.AddInParameters(objParam);
                objread = objCommon.GetDataReader("Help_dbo.st_Get_Forum_Moderators");
                List<Forums> objlist = new List<Forums>();
                while (objread.Read())
                {
                    Forums objdata = new Forums();
                    if (objread["Provider_ID"] != null)
                    {
                        objdata.Provider_ID = Convert.ToInt32(objread["Provider_ID"]);
                    }
                    if (objread["ModeratorName"] != null)
                    {
                        objdata.ModeratorName = Convert.ToString(objread["ModeratorName"]);
                    }
                    objlist.Add(objdata);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Moderators", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override int Insert_ReplyMsg(Forums objforum)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objinparamlist = {
			new SqlParameter("@in_Topic_ID", (objforum.TOPIC_ID != 0 ? objforum.TOPIC_ID : null)),
			new SqlParameter("@in_Forum_ID", (objforum.Forum_ID != 0 ? objforum.Forum_ID : null)),
			new SqlParameter("@in_TopicName", (!string.IsNullOrEmpty(objforum.TopicName) ? objforum.TopicName : null)),
			new SqlParameter("@in_TopicDescription", (!string.IsNullOrEmpty(objforum.TopicDescription) ? objforum.TopicDescription : null)),
			new SqlParameter("@in_image_Name", (!string.IsNullOrEmpty(objforum.Image_Name) ? objforum.Image_Name : null)),
			new SqlParameter("@in_Reply_ID", (!string.IsNullOrEmpty(objforum.Reply_ID) ? objforum.Reply_ID : null)),
			new SqlParameter("@In_Reply_Ind", objforum.Reply_Ind),
			new SqlParameter("@In_IsMainMessage", null),
			new SqlParameter("@in_CreatedBy", (objforum.CreatedBy != "0" ? objforum.CreatedBy : null)),
			new SqlParameter("@In_Status_Ind", objforum.Status_Ind)
		};
                objcommon.AddInParameters(objinparamlist);
                IDataParameter[] strOutPram = { new SqlParameter("@out_Replycount", SqlDbType.Int, 100) };
                objcommon.AddOutParameters(strOutPram);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_INS_ForumTopic");
                int stroutmsg = 0;
                if (objcommon.objdbCommandWrapper.Parameters["@out_Replycount"].Value != null)
                {
                    stroutmsg = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_Replycount"].Value);
                }

                return stroutmsg;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Insert_ReplyMsg", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return 0;
        }
        public override bool Del_MessagesForTopics(int Message_ID, int UpdatedBy)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamlist = {
			new SqlParameter("@In_MessageID", Message_ID),
			new SqlParameter("@In_UpdatedBy", UpdatedBy)
		};


                var _with1 = common;
                _with1.AddInParameters(InParamlist);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Del_MessagesForTopics");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Del_MessagesForTopics", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }

        public override bool Upd_MessageStatus(int Message_ID, string Status_Ind)
        {
            try
            {
                clsCommonFunctions common = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@In_MessageID", Message_ID),
			new SqlParameter("@In_StatusInd", Status_Ind)
		};
                var _with1 = common;
                _with1.AddInParameters(InParamList);
                _with1.ExecuteNonQuerySP("Help_dbo.st_Upd_MessageStatus");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Upd_MessageStatus", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return false;
        }
        public override DataSet Get_FourmNameTopicDescription(int Forum_ID, int? Topic_ID, string Login_id)
        {
            try
            {
                clsCommonFunctions cls = new clsCommonFunctions();
                DataSet dataset = new DataSet();
                IDataParameter[] inpara = { new SqlParameter("@In_Forumid", Forum_ID),
		 new SqlParameter("@in_topic_id", Topic_ID),
         new SqlParameter("@in_Login_id", Login_id)
                                          };

                cls.AddInParameters(inpara);
                dataset = cls.GetDataSet("Help_dbo.ST_GET_FORUM_TOPIC_NAME_DESC");
                return dataset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_FourmTopicDescription", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        public override List<Forums> Get_Reply_ForumTopicMessages(int Reply_Message_Id)
        {
            DataSet objDset = new DataSet();
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] ObjInParam = { new SqlParameter("@In_Reply_Message_Id", Reply_Message_Id) };
                objCommon.AddInParameters(ObjInParam);
                objDset = objCommon.GetDataSet("Help_dbo.st_Get_Reply_ForumMessages");

                if (objDset.Tables.Count > 0)
                {
                    List<Forums> replyList = new List<Forums>();
                    foreach (DataRow dr in objDset.Tables[0].Rows)
                    {
                        Forums obj = new Forums();

                        if (dr["Message_ID"].ToString() != "")
                        {
                            obj.Message_ID = dr["Message_ID"].ToString();
                        }

                        if (dr["Topic_ID"].ToString() != "")
                        {
                            obj.TOPIC_ID = (int)dr["Topic_ID"];
                        }

                        if (dr["MessageName"].ToString() != "")
                        {
                            obj.MessageName = dr["MessageName"].ToString();
                        }

                        if (dr["MessageDescription"].ToString() != "")
                        {
                            obj.MessageDescription = dr["MessageDescription"].ToString();
                        }

                        if (dr["image_Name"].ToString() != "")
                        {
                            obj.Image_Name = dr["image_Name"].ToString();
                        }

                        if (dr["Topic_ID"].ToString() != "")
                        {
                            obj.TOPIC_ID = (int)dr["Topic_ID"];
                        }

                        if (dr["Locked_Ind"].ToString() != "")
                        {
                            obj.Locked_Ind = dr["Locked_Ind"].ToString();
                        }

                        if (dr["Reply_Message_Id"].ToString() != "")
                        {
                            obj.Reply_ID = dr["Reply_Message_Id"].ToString();
                        }
                        if (dr["Author"].ToString() != "")
                        {
                            obj.UserName = dr["Author"].ToString();
                        }
                        replyList.Add(obj);
                    }
                    return replyList;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Reply_ForumTopicMessages", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
            return null;

        }
        public override List<PracticeblHome> DebugItemsList(PracticeblHome objdebug)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] paramlist = {
            new SqlParameter("@In_Provider_id", (objdebug.Provider_ID !=null ? Convert.ToInt32(objdebug.Provider_ID) : 0))
			
		};

                objcommon.AddInParameters(paramlist);
                objread = objcommon.GetDataReader("mobile_dbo.ST_List_provider_Class_Method_Runtime");
                List<PracticeblHome> objdbuglist = new List<PracticeblHome>();
                while (objread.Read())
                {
                    PracticeblHome objdebug1 = new PracticeblHome();
                    if (objread["Class_name"] != null)
                    {
                        objdebug1.Class_name = objread["Class_name"].ToString();
                    }
                    else
                    {
                        objdebug1.Class_name = null;
                    }
                    if (objread["Method_name"] != null)
                    {
                        objdebug1.Method_name = objread["Method_name"].ToString();
                    }
                    else
                    {
                        objdebug1.Method_name = null;
                    }
                    objdebug1.Startruntime = Convert.ToDateTime(objread["Startruntime"].ToString() == "" ? null : objread["Startruntime"].ToString());

                    objdebug1.Endruntime = Convert.ToDateTime(objread["Endruntime"].ToString() == "" ? null : objread["Endruntime"].ToString());
                    if (objread["Time_Difference"].ToString() != "" && objread["Time_Difference"] != null)
                    {
                        objdebug1.Time_Difference = Convert.ToInt32(objread["Time_Difference"].ToString());
                    }
                    else
                    {
                        objdebug1.Time_Difference = null;
                    }
                    if (objread["Status_Ind"].ToString() != "" && objread["Status_Ind"] != null)
                    {
                        objdebug1.Status_Ind = objread["Status_Ind"].ToString();
                    }
                    else
                    {
                        objdebug1.Status_Ind = null;
                    }
                    objdbuglist.Add(objdebug1);
                }
                return objdbuglist;
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, ClassName, "DebugItemsList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Login_history> WebLoginHistoryDetails(Login_history objloginhistory, ref int Total_Records)
        {
            try
            {
                clsDBWrapper objclsdbwraper = new clsDBWrapper();
                SqlDataReader returndata = default(SqlDataReader);
                IDataParameter[] Paramlist = {
                 new SqlParameter("@in_login_id", objloginhistory.Login_ID),
			     new SqlParameter("@In_Ind", objloginhistory.Mobile_type == null ? null : objloginhistory.Mobile_type),
                 new SqlParameter("@In_noofrows", objloginhistory.NoofRecords),
			     new SqlParameter("@In_pageno", objloginhistory.PageNo)
			
		};
                objclsdbwraper.AddInParameters(Paramlist);
                returndata = objclsdbwraper.GetDataReader("Help_dbo.ST_Get_Loginhistory_details");
                List<Login_history> Adcollection = new List<Login_history>();

                while (returndata.Read())
                {
                    Login_history objlogin = new Login_history();
                    if (returndata["ipaddress"] != DBNull.Value)
                    {
                        objlogin.IPAddress = Convert.ToString(returndata["ipaddress"]);
                    }
                    else
                    {
                        objlogin.IPAddress = "-";
                    }
                    if (returndata["LogIn_DateTime"] != DBNull.Value)
                    {
                        objlogin.LogIn_DateTime = Convert.ToString(returndata["LogIn_DateTime"]);
                    }
                    else
                    {
                        objlogin.LogIn_DateTime = null;
                    }
                    if (returndata["LogOut_DateTime"] != DBNull.Value)
                    {
                        objlogin.LogOut_DateTime = Convert.ToString(returndata["LogOut_DateTime"]);
                    }
                    else
                    {
                        objlogin.LogOut_DateTime = null;
                    }
                    if (returndata["UserAgent"] != DBNull.Value)
                    {
                        objlogin.UserAgent = Convert.ToString(returndata["UserAgent"]);
                    }
                    else
                    {
                        objlogin.UserAgent = null;
                    }
                    if (returndata["Login_success"] != DBNull.Value)
                    {
                        objlogin.logsuccess = Convert.ToString(returndata["Login_success"]);
                    }
                    else
                    {
                        objlogin.logsuccess = null;
                    }
                    if (returndata["Deviceaddress"] != DBNull.Value)
                    {
                        objlogin.deviceaddress = Convert.ToString(returndata["Deviceaddress"]);
                    }
                    else
                    {
                        objlogin.deviceaddress = null;
                    }
                    if (returndata["Devicephonenumber"] != DBNull.Value)
                    {
                        objlogin.devicePhone = Convert.ToString(returndata["Devicephonenumber"]);
                    }
                    else
                    {
                        objlogin.devicePhone = null;
                    }
                    Adcollection.Add(objlogin);
                }
                if (returndata.NextResult() == true)
                {
                    if (returndata.Read())
                    {
                        Total_Records = Convert.ToInt32(returndata["Totalrecords"]);
                    }
                }
                return Adcollection;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "WebLoginHistoryDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public  System.Collections.Generic.List<NotesInfo> NotesType(string NoOfRecords, string pageno, string userid)
        {
            List<NotesInfo> NotesInfo = new List<NotesInfo>();
            if (pageno == "")
            {
                pageno = "1";
            }
          //  int value = 0;
            try
            {
                clsCommonFunctions objcommon1 = new clsCommonFunctions();
            
                SqlDataReader objread = default(SqlDataReader);
                IDataParameter[] objparm = {
			
			new SqlParameter("@In_NoOfRecords", NoOfRecords),
			new SqlParameter("@In_PageNo", pageno),
			new SqlParameter("@In_Fromreference_ID", userid),
			new SqlParameter("@In_OrderBy", "DESC"),	
		};
                objcommon1.AddInParameters(objparm);
                objread = objcommon1.GetDataReader("Help_dbo.st_Notes_list_Generalnotes_RDPAGING_VER2");

                while (objread.Read() && !string.IsNullOrEmpty(Convert.ToString(objread["Transaction_id"])))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(objread["Transaction_id"] ))) { 
                    NotesInfo objNoteInfo = new NotesInfo();
                    objNoteInfo.GeneralNote_ID = Convert.ToInt32(objread["GeneralNote_ID"] != null ? objread["GeneralNote_ID"] : null);
                    objNoteInfo.IsPatientNote_Ind = Convert.ToString(objread["Transaction_id"] != null ? objread["Transaction_id"] : null);
                    NotesInfo.Add(objNoteInfo);
                    }
                    //value = Convert.ToInt32(objread["Transaction_id"] != null ? objread["Transaction_id"] : null);
                 
                }

                return NotesInfo;
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, "Notes", "NotesType", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string UpdateDefaultCourt(Patient_Info objpatient, int? DefaultCrtId, int countryid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strParam = {
			new SqlParameter("@In_patient_id", Convert.ToInt32(objpatient.PatientID)),
			new SqlParameter("@In_Defaultcourtlocation_ID", DefaultCrtId),
			new SqlParameter("@In_provider_ID", Convert.ToInt32(objpatient.ProviderID)),
            new SqlParameter("@In_Court_location_name", !string.IsNullOrEmpty(objpatient.CourtLocationName)? objpatient.CourtLocationName : null),
            new SqlParameter("@In_Default_address", !string.IsNullOrEmpty(objpatient.Default_addr)? objpatient.Default_addr: null),
            new SqlParameter("@In_Address1", !string.IsNullOrEmpty(objpatient.Address1)? objpatient.Address1 : null),
            new SqlParameter("@In_Country_id", countryid),
            new SqlParameter("@In_Zip", !string.IsNullOrEmpty(objpatient.ZIP)? objpatient.ZIP : null),
            new SqlParameter("@In_State_id", Convert.ToInt32(objpatient.State_ID)),
            new SqlParameter("@In_City_id", Convert.ToInt32(objpatient.City_ID))
		};
                objcommon.AddInParameters(strParam);
                objcommon.ExecuteNonQuerySP("St_client_upd_defaultcourt_Location");
            }
            catch (Exception ex)
            {
                var Exobj = new clsExceptionLog();
                Exobj.LogException(ex, ClassName, "UpdateDefaultCourt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
    }
}