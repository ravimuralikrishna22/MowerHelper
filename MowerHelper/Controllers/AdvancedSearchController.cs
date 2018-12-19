using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    [App_Start.NotRequireHttps]
    public class AdvancedSearchController : Controller
    {
        public string Currentpage = "";
        public string Strwebsitename = "";
        public string Strwebsiteimg = "";
        public string Strwebsiteurl;
        public string State = "";
        public string City= "";
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string ZipValue{ get; set; }
        public string IsSearch{ get; set; }
        public int CityId{ get; set; }
        public int StateId{ get; set; }
        public string StateName{ get; set; }
        public string CityName{ get; set; }
        public int CountryId{ get; set; }
        public string Distance{ get; set; }
        public string FlgValue{ get; set; }
        List<ProviderAdvertising> objDataList;
        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProviderSearch(string page)
        {
            if (Request.Url != null)
            {
                //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
                {
                    ViewBag.ShowArticles = "Y";
                }
                else
                {
                    ViewBag.ShowArticles = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
                {
                    ViewBag.Showforums = "Y";
                }
                else
                {
                    ViewBag.Showforums = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
                {
                    ViewBag.ShowElectricians = "Y";
                }
                else
                {
                    ViewBag.ShowElectricians = "N";
                }
                Strwebsiteurl = Convert.ToString(Request.Url);
                string[] strurl = Strwebsiteurl.Split('/');
                //var objconfig = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    if (Strwebsiteurl.Contains("localhost:"))
                    {
                   
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                        ViewBag.topimg = strurl[0] + "//" + strurl[2] + "/" + "Images/123.gif";
                        ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";
                    }
                    else if (Strwebsiteurl.Contains("vbv"))
                    {
                
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                        ViewBag.topimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/123.gif";
                        ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/speacer.gif";
                    }

                }
                else
                {
                    ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                    ViewBag.topimg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/123.gif";
                    ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/speacer.gif";


                }
                if (page != null)
                {
                    ViewBag.pageno = page;
                    int pagNum = Convert.ToInt32(page);
                    ViewBag.Pagenation = pagNum;
                   string zip;
                    string fullname;
                 int distance;
                    string freesession;
                    string advanceCity = null;
                    const int advanceState = 0;
            
                    if (Request["txtfullname"] != "")
                    {
                        fullname = Request["txtfullname"];
                        ViewBag.fullname = fullname;
                    }
                    else
                    {
                        fullname = null;
                    }
                    if (Request["txtZIP"] != "")
                    {
                        zip = Request["txtZIP"];
                        ViewBag.zip = zip;
                    }
                    else
                    {
                        zip = null; 
                    }
                    if (Request["ddlDistance"] != "0")
                    {
                        distance = Convert.ToInt32(Request["ddlDistance"]);
                        ViewBag.distance = distance;
                    }
                    else
                    {
                        distance = 0;
                    }
                    if (Request["ddlfee"] != null & Request["ddlfee"]!="")
                    {
                        freesession = Request["ddlfee"];
                        ViewBag.freesession=freesession;
                    }
                    else
                    {
                        freesession = null;
                        ViewBag.freesession = 0;
                    }
                    var objData = new ProviderAdvertising();
                    var obj = new ProviderAdvertising();

          
                    objData.CityNm = null;
            
                    objData.State_ID = null;
                    string strradius = ValidateZipcode(zip);
                    if (!string.IsNullOrEmpty(strradius))
                    {
                        ViewBag.zippcode = zip;
                        objData.ZIPValue = zip;
                        if (strradius != "0")
                        {
                            if (distance == 0)
                            {
                                distance = Convert.ToInt32(strradius);
                                ViewBag.distance = strradius;
                            }
                        }
                    }
                    else
                    {
                        if (zip != null)
                        {
                            if (zip.Contains("("))
                            {
                                string[] strValues = zip.Split('(');
                                if (strValues.Length == 2)
                                {
                                    string strCity = strValues[0].Trim();
                                    string strState = strValues[1].Replace("(", "").Trim();
                                    strState = strState.Replace(")", "").Trim();
                                    objData.CityNm = strCity;
                                    objData.State_ID = getStateIDFromAbbrevation(strState);
                                    objData.ZIPValue = null;
                                    ViewBag.distance = 0;
                                }
                            }
                            else
                            {
                                string strCity = zip.Trim();
                                objData.CityNm = strCity;
                                objData.ZIPValue = null;
                                ViewBag.distance = 0;
                            }

                        }
                        else
                        {
                            objData.ZIPValue = null;
                        }
                    }

                    objData.Radius = distance;
                    objData.LastName = fullname;
                    if (freesession =="0")
                    {
                        objData.firstfee = null;
                    }
                    else if (freesession == "1")
                    {
                        objData.firstfee = "Y";
                    }
                    else if (freesession == "2")
                    {
                        objData.firstfee = "N";
                    }

                    objData.PageNo = pagNum;
                    objData.NoOfRecords = 10;
                    int totalNoofRecords = 0;
                    if (objData.ZIPValue == null)
                    {
                        objData.Radius = 0;
                    }
                    try
                    {
                       
                        if ((advanceCity == null) & (advanceState == 0) & (zip == null) &   (fullname == null) & (freesession == null))
                        {
                            //objDataList = obj.GetProviders_ListWithAllFieldsNULL_ForAdvanceSearch(pagNum, 10, ref totalNoofRecords);
                        }
                        else if ( distance == 0)
                        {
                            objDataList = obj.GetProviders_ListWithOutRadius_ForAdvanceSearch(objData,ref totalNoofRecords);
                        }
                        else
                        {
                            objDataList = obj.GetProvidersListForAdvanceSearch(objData,ref totalNoofRecords);
                        }

                        if (objDataList.Count > 0)
                        {
                            ViewBag.totadd = ProviderAdvertising.fulladdress;
                            ViewBag.totadd = Convert.ToString(ProviderAdvertising.fulladdress).Replace('+', ',');
                            string domain = null;
                            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                            {
                                if (Strwebsiteurl.Contains("localhost:"))
                                {
                                    domain = strurl[0] + "//" + strurl[2] + "/";

                                }
                                else if (Strwebsiteurl.Contains("vbv"))
                                {
                                    domain = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/";

                                }
                            }
                            else
                            {
                                domain = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal");
                            }
                            ViewBag.imgpath = domain + "Images/green-dot.png";
                            ViewBag.list = objDataList;
                            ViewBag.listcount = objDataList.Count;
                            TempData["therapistlist"] = objDataList;
                            TempData["therapistlistcnt"] = objDataList.Count;
                            string strproviderids = null;
                            int i;
                            for (i = 0; i <= objDataList.Count - 1; i++)
                            {
                                if (i == 0)
                                {
                                    strproviderids = Convert.ToString(objDataList[i].ProviderID);
                                }
                                else
                                {
                                    strproviderids = strproviderids + "," + Convert.ToString(objDataList[i].ProviderID);
                                }
                            }
                            strproviderids = strproviderids + ",";

                            var objj = new ProviderAdvertising();
                            objj.CountingStateswiseSearch(strproviderids, null, 'Y', StateName, CityName);
                        }
                        if (totalNoofRecords > 0)
                        {
                            GetDataListPaging(ref totalNoofRecords);
                        }
                        ViewBag.listing = "[  " + totalNoofRecords + "  listings ]";
                        ViewBag.records = totalNoofRecords;
                    }
                    catch (Exception ex)
                    {
                        var clsexp = new clsExceptionLog();
                        clsexp.LogException(ex, "AdvancedSearchController", "ProviderSearch", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    }
                  
                }
                else
                {

                    ViewBag.totadd = null;
                    ViewBag.gender = 0;
                    ViewBag.freesession = 0;
                    ViewBag.pageno = 1;

                    if ((Request.Cookies["State"] != null))
                    {
                        var httpCookie = Request.Cookies["City"];
                        if (httpCookie != null && Request.Cookies["State"].Value != null & httpCookie.Value != null)
                        {
                            if ((Request.Cookies["TrackID"] == null))
                            {
                                InsertUserTracking();
                            }
                        }
                    }
                    ViewBag.url = Convert.ToString(Request.Url);

                    
                }
            }
              DataListFeaturedTherapistsBind();
            try
            {

                return View("ProviderSearch");
            }
    
            catch (Exception ex)
            {

                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "AdvancedSearchController", "ProviderSearch", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
             
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProviderSearch(string id,string page)
        {
            if (Request.Url != null) Strwebsiteurl = Request.Url.ToString();
            string[] strurl = Strwebsiteurl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Strwebsiteurl.Contains("localhost:"))
                {

                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                    ViewBag.topimg = strurl[0] + "//" + strurl[2] + "/" + "Images/123.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";
                }
                else if (Strwebsiteurl.Contains("vbv"))
                {

                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                     ViewBag.topimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/123.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/speacer.gif";
                }

            }
            else
            {

                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                ViewBag.topimg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/123.gif";
                ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/speacer.gif";


            }
            int pagNum;
            if (Request["bpageno"]!=null)
            {
                ViewBag.pageno = Request["bpageno"];
                pagNum = Convert.ToInt32(Request["bpageno"]);
            }
            else
            {
                ViewBag.pageno = 1;
                pagNum = 1;
            }
            ViewBag.Pagenation = pagNum;
            string zip;
            string fullname;
            int distance;
            string freesession=null;
            string advanceCity = null;
            const int advanceState = 0;
            
             if (Request["txtfullname"] != "")
            {
                fullname = Request["txtfullname"];
                ViewBag.fullname = fullname;
            }
            else
            {
                fullname = null;
            }
            if (Request["txtZIP"] != "")
            {
                 zip = Request["txtZIP"];
                 ViewBag.zip = zip;
            }
            else
            {
                zip = null; 
            }
            if (Request["ddlDistance"] != "0")
            {
                distance = Convert.ToInt32(Request["ddlDistance"]);
                ViewBag.distance = distance;
            }
            else
            {
                distance = 0;
            }
            //if (Request["ComboBox1"] != "")
                if (Request["rdfreeyes"] != "" & Request["rdfreeyes"] != null)
                {
                    freesession = Request["rdfreeyes"];
                    ViewBag.freesession = freesession;
                }
                else
                {
                    freesession = Request["rdfreeno"];
                    ViewBag.freesession = freesession;
                }
             //if (Request["ddlfee"] != "")
             //{
             //    freesession = Request["ddlfee"];
             //    ViewBag.freesession=freesession;
             //}
             //else
             //{
             //    freesession = null;
             //    ViewBag.freesession = 0;
             //}
            var objData = new ProviderAdvertising();
            var obj = new ProviderAdvertising();

          
                objData.CityNm = null;
            
            objData.State_ID = null;
            string strradius = ValidateZipcode(zip);
            if (!string.IsNullOrEmpty(strradius))
            {
                ViewBag.zippcode = zip;
                objData.ZIPValue = zip;
                if (strradius != "0")
                {
                    if (distance == 0)
                    {
                        distance = Convert.ToInt32(strradius);
                        ViewBag.distance = strradius;
                    }
                }

            }
            else
            {
                if (zip != null)
                {
                    if (zip.Contains("("))
                    {
                        string[] strValues = zip.Split('(');
                        if (strValues.Length == 2)
                        {
                            string strCity = strValues[0].Trim();
                            string strState = strValues[1].Replace("(", "").Trim();
                            strState = strState.Replace(")", "").Trim();
                            objData.CityNm = strCity;
                            objData.State_ID = getStateIDFromAbbrevation(strState);
                            objData.ZIPValue = null;
                            ViewBag.distance = 0;
                        }
                    }
                    else
                    {
                        string strCity = zip.Trim();
                        objData.CityNm = strCity;
                        objData.ZIPValue = null;
                        ViewBag.distance = 0;
                    }

                }
                else
                {
                    objData.ZIPValue = null;
                }
            }

            objData.Radius = Convert.ToInt32(distance);
            objData.LastName = fullname;
            if (freesession =="0")
            {
                objData.firstfee = null;
            }
            else if (freesession == "1")
            {
                objData.firstfee = "Y";
            }
            else if (freesession == "2")
            {
                objData.firstfee = "N";
            }


            objData.PageNo = pagNum;
            objData.NoOfRecords = 10;
            int totalNoofRecords = 0;
            if (objData.ZIPValue == null)
            {
                objData.Radius = 0;
            }
            try
            {
                if ((advanceCity == null) & (advanceState == 0) & (zip == null) &  (fullname == null) & (freesession == null))
                {
                   // objDataList = obj.GetProviders_ListWithAllFieldsNULL_ForAdvanceSearch(pagNum, 10, ref totalNoofRecords);
                }
                else if ( distance == 0)
                {
                    objDataList = obj.GetProviders_ListWithOutRadius_ForAdvanceSearch(objData,ref totalNoofRecords);
                }
                else
                {
                    objDataList = obj.GetProvidersListForAdvanceSearch(objData,ref totalNoofRecords);
                }

                string strproviderids = null;


                int i;
                for (i = 0; i <= objDataList.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        strproviderids = Convert.ToString(objDataList[i].ProviderID);
                    }
                    else
                    {
                        strproviderids = strproviderids + "," + Convert.ToString(objDataList[i].ProviderID);
                    }
                }
                strproviderids = strproviderids + ",";

                var objj = new ProviderAdvertising();
                    objj.CountingStateswiseSearch(strproviderids, null, 'Y', StateName, CityName);
               
                if (objDataList.Count > 0)
                {
                    ViewBag.list = objDataList;
                    ViewBag.listcount = objDataList.Count;
                    TempData["therapistlist"] = objDataList;
                    TempData["therapistlistcnt"] = objDataList.Count;
                    ViewBag.totadd = ProviderAdvertising.fulladdress;
                    ViewBag.totadd = Convert.ToString(ProviderAdvertising.fulladdress).Replace('+', ',');
                    string domain = null;
                    if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                    {
                        if (Strwebsiteurl.Contains("localhost:"))
                        {
                            domain = strurl[0] + "//" + strurl[2] + "/";

                        }
                        else if (Strwebsiteurl.Contains("vbv"))
                        {
                            domain = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/";

                        }
                    }
                    else
                    {
                        domain = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal");
                    }
                    ViewBag.imgpath = domain + "Images/green-dot.png";

                }
                if (totalNoofRecords > 0)
                {
                    GetDataListPaging(ref totalNoofRecords);
                }
                ViewBag.listing = "[  " + totalNoofRecords + "  listings ]";
                ViewBag.records = totalNoofRecords;
                DataListFeaturedTherapistsBind();

                return View("ProviderSearch");        
            }
            catch (Exception ex)
            {
              var clsexp = new clsExceptionLog();
              clsexp.LogException(ex, "AdvancedSearchController", "ProviderSearch", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
           

            return View();
        }
        public string ValidateZipcode(string zipcode)
        {

            try
            {
                var objcommon = new clsCommonFunctions();
               // string strradius = "";
                IDataParameter[] objparam = { new SqlParameter("@in_Zip", zipcode) };
                IDataParameter[] objoutparam = { new SqlParameter("@Out_Radius", SqlDbType.Int) };
                objcommon.AddInParameters(objparam);
                objcommon.AddOutParameters(objoutparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.ST_Public_Validate_zipcode");
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value!=null)
                {
                    return objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                  var clsexp = new clsExceptionLog();
                  clsexp.LogException(ex, "AdvancedSearchController", "ValidateZipcode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        private int getStateIDFromAbbrevation(string stateAbbrevation)
        {
            try{
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_state", stateAbbrevation) };
            objcommon.AddInParameters(objparam);
            SqlDataReader drlist = objcommon.GetDataReader("Help_dbo.st_get_stateid_state");
            while (drlist.Read())
            {
                if (drlist["State_id"] != null)
                {
                    return Convert.ToInt32(drlist["State_id"]);
                }
            }
             }
            catch (Exception ex)
            {
                  var clsexp = new clsExceptionLog();
                  clsexp.LogException(ex, "AdvancedSearchController", "getStateIDFromAbbrevation", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }

        public JsonResult GetProviderfullnames(string term)
        {
        var objlist = new List<string>();
            var objcom = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Keyword", term) };
            objcom.AddInParameters(objparam);

            SqlDataReader drlist = objcom.GetDataReader("Help_dbo.st_Public_Typeahead_provider_fullname");
                while (drlist.Read())
                {
                    objlist.Add(Convert.ToString(drlist[0]));
                }
                return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
            }
         
        public void DataListFeaturedTherapistsBind()
        {
            try{
            if (TempData["therapistlist"] != null)
            {
                ViewBag.FeaturedList = TempData["therapistlist"];
                ViewBag.FeaturedListcount = TempData["therapistlistcnt"];
                ViewBag.Deafult = "N";
                //foreach (var item in ViewBag.FeaturedList)
                //{
                //    ProviderAdvertising objproviderAdvertise = new ProviderAdvertising();
                //    objproviderAdvertise.SiteStatic_ID = "2";
                //    objproviderAdvertise.State_Name = StateName;
                //    objproviderAdvertise.CityNm = CityName;
                //    objproviderAdvertise.Provider_ID = Convert.ToString(item.ProviderID);
                //    objproviderAdvertise.CountingProviderVisitToHisProfile(objproviderAdvertise);
                //}
            }
            else
            {
                if (ZipValue == null & StateName == null & CityName == null & StateId == 0 & CityId == 0)
                {
                    if ((Request.Cookies["State"] != null))
                    {
                        if (Request.Cookies["State"].Value != null)
                        {
                            StateName = Request.Cookies["State"].Value.Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["City"] != null))
                    {
                        if (Request.Cookies["City"].Value != null)
                        {
                            CityName = Request.Cookies["City"].Value.Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["Latitude"] != null))
                    {
                        if (Request.Cookies["Latitude"].Value != null)
                        {
                            Lattitude = Request.Cookies["Latitude"].Value.Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["Longitude"] != null))
                    {
                        if (Request.Cookies["Longitude"].Value != null)
                        {
                            Longitude = Request.Cookies["Longitude"].Value.Replace("%20", " ");
                        }
                    }

                }
                var objData = new Provider_AdvancedSearch
                {
                    ZIPValue = ZipValue,
                    City_ID = CityId,
                    State_ID = StateId,
                    StateName = StateName,
                    CityName = CityName,
                    Country_ID = CountryId,
                    Distance = Convert.ToInt32(Distance),
                    Longitude = Longitude,
                    Lattitude = Lattitude
                };
                if (ZipValue != null | CityId != 0 | CityName != null | StateName != null | StateId != 0 | CountryId != 0 | Distance != null)
                {
                    objData.IsSearch = "Y";
                }
                else
                {
                    objData.IsSearch = null;
                }
                var objlist = Provider_AdvancedSearch.list_Featuredtherapists(objData);
                ViewBag.FeaturedList = objlist.Tables[0].AsEnumerable();
                ViewBag.FeaturedListcount = objlist.Tables[0].Rows.Count;
                ViewBag.Deafult = "Y";
                //if (objlist.Tables[0].Rows.Count > 0)
                //{
                //    int cnt;
                //    for (cnt = 0; cnt <= objlist.Tables[0].Rows.Count - 1; cnt++)
                //    {

                //        ProviderAdvertising objproviderAdvertise = new ProviderAdvertising();
                //        objproviderAdvertise.SiteStatic_ID = "2";
                //        objproviderAdvertise.State_Name = StateName;
                //        objproviderAdvertise.CityNm = CityName;
                //        objproviderAdvertise.Provider_ID = objlist.Tables[0].Rows[cnt]["Provider_ID"].ToString();
                //        objproviderAdvertise.CountingProviderVisitToHisProfile(objproviderAdvertise);

                //    }
                //}
            }

            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "AdvancedSearchController", "DataListFeaturedTherapistsBind", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void InsertUserTracking()
        {
            try
            {
            if (Request.Url != null)
            {
                string[] strUrl = Request.Url.ToString().Split('/');
                //var cs = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    Strwebsitename = strUrl[0] + "//" + strUrl[2] + "/" + strUrl[3] + "/";
                }
                else
                {
                    Strwebsitename = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl");
                }
            }
            var objdata = new ProviderAdvertising();
            if ((Request.Cookies["State"] != null))
            {
                if (Request.Cookies["State"].Value != null)
                {
                    objdata.State_Name = Request.Cookies["State"].Value.Replace("%20", " ");
                }
            }
            if ((Request.Cookies["City"] != null))
            {
                if (Request.Cookies["City"].Value != null)
                {
                    objdata.CityNm = Request.Cookies["City"].Value.Replace("%20", " ");
                }
            }
            if ((Request.Cookies["Latitude"] != null))
            {
                if (Request.Cookies["Latitude"].Value != null)
                {
                    objdata.Latitude = Request.Cookies["Latitude"].Value.Replace("%20", " ");
                }
            }
            if ((Request.Cookies["Longitude"] != null))
            {
                if (Request.Cookies["Longitude"].Value != null)
                {
                    objdata.Longitude = Request.Cookies["Longitude"].Value.Replace("%20", " ");
                }
            }
            if ((Request.Cookies["Country"] != null))
            {
                if (Request.Cookies["Country"].Value != null)
                {
                    objdata.Country = Request.Cookies["Country"].Value.Replace("%20", " ");
                }
            }
            if ((Request.Cookies["Countrycode"] != null))
            {
                if (Request.Cookies["Countrycode"].Value != null)
                {
                    objdata.CountryCode = Request.Cookies["Countrycode"].Value.Replace("%20", " ");
                }
            }
            objdata.IPAddress = HttpContext.Request.ServerVariables["remote_addr"];
            objdata.UserAgent = HttpContext.Request.UserAgent;
            objdata.Siteurl = Strwebsitename;
            long trackId = objdata.InsertVisitorTracking(objdata);
            var newCookie = new HttpCookie("TrackID") {Value = Convert.ToString(trackId)};
            Response.Cookies.Add(newCookie);
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "AdvancedSearchController", "InsertUserTracking", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void GetDataListPaging(ref int totNoRecords)
        {
            try{
            var objData = new ProviderListPaging();
            var objDataList = objData.GetPagingforDataList(ref totNoRecords);
            if (objDataList.Count > 0)
            {
                ViewBag.listpaging = objDataList;
                ViewBag.listpagingcount = objDataList.Count;

            }
            else
            {
                ViewBag.listpaging = null;

            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "AdvancedSearchController", "GetDataListPaging", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
    }
    
}

	
	
		
		
