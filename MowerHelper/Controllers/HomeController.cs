using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using MowerHelper.App_Start;
using MowerHelper.Models;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class HomeController : Controller
    {
       public  int Rowcount;
       public string Requesturl;
        public string StateName{get;set;}
        public string CityName { get;set; }
        public string Lattitude { get;set; }
        public string Longitude { get;set; }
        public string IsSearch { get; set; }
           [AllowAnonymous]
        [NotRequireHttps]
        [OutputCache(CacheProfile="short",VaryByParam="*")]
        public ActionResult Index()
        {
           // string ipAddress = HttpContext.Request.UserHostAddress;

            //System.Web.HttpContext.Current.Response.Expires = -1;
            //Session.RemoveAll();
            //Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
             Session["Practice_ID"] = null;
               Session["Provider_ID"] = null;
              //  var obj = new StatesList();

              //var  objDataList = obj.GetStatesBasedOnDirectoryID();
              //  if (objDataList.Count > 0)
              //  {
              //      ViewBag.States = objDataList;
              //      ViewBag.Statescount = objDataList.Count;
              //      Rowcount = objDataList.Count / 4;
              //      ViewBag.rows = Rowcount;
              //  }
              //  else
              //  {
              //      ViewBag.States = null;
              //      ViewBag.Statescount = 0;
              //  }
               //clsWebConfigsettings clsweb = new clsWebConfigsettings();
               //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
               //{
               //    ViewBag.ShowArticles = "Y";
               //}
               //else
               //{
               //    ViewBag.ShowArticles = "N";
               //}
               //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
               //{
               //    ViewBag.Showforums = "Y";
               //}
               //else
               //{
               //    ViewBag.Showforums = "N";
               //}
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
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
                {
                    ViewBag.ShowElectricians = "Y";
                    FeaturedTherspists();
                }
                else
                {
                    ViewBag.ShowElectricians = "N";
                }
               
            return View();
        }

        [HttpPost]
        public ActionResult Index(string zip)
        {
            string ziptext = Request["txtzip"];
            ziptext = ziptext!=null?ziptext.Trim():null;
            if (string.IsNullOrEmpty(ziptext))
            {
                return RedirectToAction("Directory", "Search");
            }
             if (!string.IsNullOrEmpty(ziptext))
            {
                string strradius = ValidateZipcode(ziptext);
                if (!string.IsNullOrEmpty(strradius))
                {
                    if (strradius == "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToAction("ListOfProviders", "Search", new { id = ziptext });
                    }
                    RouteData.Values.Clear();
                    return RedirectToRoute("ZipcodeTherapists", new { id = ziptext, distance = strradius });
                }
                if (ziptext.Contains("("))
                {
                    var strValues = ziptext.Split('(');
                    if (strValues.Length == 2)
                    {
                        string strCity = strValues[0].Trim();
                        string strState = strValues[1].Replace("(", "").Trim();
                        strState = strState.Replace(")", "").Trim();
                        strState = getStatenameFromAbbrevation(strState);
                        if (!string.IsNullOrEmpty(strState) & !string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            RouteData.Values.Clear();
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        if (!string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            strCity = strCity.Replace(" ", "-");
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            RouteData.Values.Clear();
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        RouteData.Values.Clear();
                        return RedirectToAction("Directory", "Search", new { id = "N" });
                    }
                }
                else
                { 
                    //if (!string.IsNullOrEmpty(getStatenameFromCity(ziptext.Trim())))
                    string strState = getStatenameFromCity(ziptext.Trim());
                    if (!string.IsNullOrEmpty(strState))
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("Cities", new { city = ziptext.Trim().Replace(" ", "-") });
                    }
                    RouteData.Values.Clear();
                    return RedirectToAction("Directory", "Search", new { id = "N" });
                }
            }
            return null;
        }
        public string ValidateZipcode(string zipcode)
        {
            try{
            var objcommon = new clsCommonFunctions();
            string strradius = "";
            IDataParameter[] objparam = { new SqlParameter("@in_Zip", zipcode) };
            IDataParameter[] objoutparam = { new SqlParameter("@Out_Radius", SqlDbType.Int) };
            objcommon.AddInParameters(objparam);
            objcommon.AddOutParameters(objoutparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.ST_Public_Validate_zipcode");
            if (objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value!=null)
            {
                strradius = objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value.ToString();
            }
            return strradius;
             }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "ValidateZipcode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        private string getStatenameFromAbbrevation(string stateAbbrevation)
        {
            try{
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_State_Abbrevation", stateAbbrevation) };
            objcommon.AddInParameters(objparam);
            SqlDataReader drlist = objcommon.GetDataReader("Help_dbo.st_Admin_Get_StateName");
            while (drlist.Read())
            {
                if (drlist["StateName"] != null)
                {
                    return drlist["StateName"].ToString();
                }
            }
                }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "getStatenameFromAbbrevation", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return null;
        }
        private string getStatenameFromCity(string cityname)
        {
            try
            {
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_City", cityname) };
            objcommon.AddInParameters(objparam);
            SqlDataReader drlist = objcommon.GetDataReader("Help_dbo.st_Admin_Get_StateNameFromCity");
            while (drlist.Read())
            {
                if (drlist["Statename"]!=null)
                {
                    return drlist["Statename"].ToString();
                }
            }
              }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "getStatenameFromCity", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return null;
        }

        public JsonResult Zipcodes(string term)
        {
            
            var objlist = new List<string>();
            var objcom = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Keyword", term) };
            objcom.AddInParameters(objparam);
            SqlDataReader drlist;
            int zipcode;
            bool result = int.TryParse(term, out zipcode);
            if (Equals(result, true))
            {
                drlist = objcom.GetDataReader("Help_dbo.st_Public_Typeahead_Zipcode_1");
                while (drlist.Read())
                {
                    objlist.Add(Convert.ToString(drlist[0]));
                }
            }
            else
            {
                drlist = objcom.GetDataReader("Help_dbo.st_Public_Typeahead_Cities");
                while (drlist.Read())
                {
                    objlist.Add(Convert.ToString(drlist[0]));
                }
            }
            return Json(objlist.ToArray(),JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getzipbylatlon(string lat,string lon)
        {
            string zip = null;
            DataSet dszip = new DataSet();
            if (lat != null && lon != null)
            {
                dszip = Ziplist.GetZipBasedOnLatLon(lat, lon);
            }
            if (dszip.Tables[0].Rows.Count > 0)
            {
                zip = Convert.ToString(dszip.Tables[0].Rows[0][0]);
               
            }
            var obj = new clsNearestCities();
            var objCitiesList = new List<clsNearestCities>();
            var objZipCodesList = new List<clsNearestZIPCodes>();
            obj.GetNearestZIPCodesandNearestCities(zip, ref objCitiesList, ref objZipCodesList);
                   if (objCitiesList.Count > 0)
            {
                 return Json(objCitiesList, JsonRequestBehavior.AllowGet);
              

            }
            else
            {
                var obj1 = new StatesList();
                var objDataList = obj1.GetStatesBasedOnDirectoryID();
             //   if (objDataList.Count > 0)
               // {
                    return Json(objDataList, JsonRequestBehavior.AllowGet);
                //}
                
           }
            //return Json( JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [NotRequireHttps]
        [OutputCache(CacheProfile = "short", VaryByParam = "*")]
        public ActionResult Authorization()
        {
            Session["Practice_ID"] = null;
            Session["Provider_ID"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Authorization(clsAuthorization model, string returnurl)
        {
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("SecurityUserName") == model.Username & clsWebConfigsettings.GetConfigSettingsValue("SecurityPassword") == model.Password)
            {
                return RedirectToRoute("Home");
            }
            else
            {
                TempData["msg"] = "Login was unsuccessful";
            }
            ModelState.AddModelError("", Convert.ToString(TempData["msg"]));
            return View(model);
        }
        [AllowAnonymous]
        [RequireHttpsByConfig]
        public ActionResult Login()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            Session["Practice_ID"] = null;
            Session["Provider_ID"] = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttpsByConfig]
        public ActionResult Login(clsAuthorization model, string returnurl)
        {
            var clsAuth = new clsAuthorization();
            string outispaid=null;
            string outmsg=null;
            
            if (model.Username != null && model.Password != null)
            {
                int intLoginId = clsAuth.IsValidUser(model.Username, model.Password, ref outmsg);
                ViewBag.outispaid = outispaid;
                if (intLoginId == 1)
                {
                    if (Convert.ToString(Session["RoleID"]) == "31")
                    {
                        return RedirectToAction("ProviderProfile", "VerificationUser");
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "38")
                    {
                        return RedirectToAction("Userprofile", "VerificationUser");
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "1")
                    {
                        return RedirectToAction("Providerprofile", "Providers");
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "39")
                    {
                        return RedirectToAction("WeeklyAppts", "Schedule");
                    }
                    else if (Convert.ToString(Session["Registeredin"]) == "M" && Convert.ToString(Session["Msgitem"]) == "5" && Convert.ToString(Session["Serviceactive"]) != "N" && Convert.ToString(Session["phoneno_read_ind"]) == "Y")
                    {
                        return RedirectToAction("WeeklyAppts", "Schedule");
                    }
                    else if (Convert.ToString(Session["CCexists"]) == "N" || Convert.ToString(Session["Msgitem"]) != "5")
                    {
                        return RedirectToAction("Schedulespecs", "Schedule");
                    }
                    else if (Convert.ToString(Session["Serviceactive"]) == "N")
                    {
                        return RedirectToAction("Schedulespecs", "Schedule");
                    }
                    else if (Convert.ToString(Session["Registeredin"]) == "M" && Convert.ToString(Session["phoneno_read_ind"]) == "N" && Convert.ToString(Session["ccexists"]) == "N")
                    {
                        return RedirectToAction("Schedulespecs", "Schedule");
                    }
                    else
                    {
                        if (Convert.ToString(Session["Licenseverified"]) == "Y" & Convert.ToString(Session["CCexists"]) == "Y" & Convert.ToString(Session["ccexpiry"]) == "N")
                    {
                        return RedirectToAction("WeeklyAppts", "Schedule");
                    }
                        if (Convert.ToString(Session["Licenseverified"]) != "Y" & Convert.ToString(Session["ccexpiry"]) == "N")
                    {
                        return RedirectToAction("WeeklyAppts", "Schedule");
                    }
                        return RedirectToAction("WeeklyAppts", "Schedule");
                    }
                }
                if (outmsg != null)
                {
                    TempData["msg"] = outmsg;
                }
                else
                {
                    TempData["msg"] = "Login was unsuccessful";

                }
                ModelState.AddModelError("", Convert.ToString(TempData["msg"]));
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Convert.ToString(Session["userId"]) != "")
            {
                var objCommon = new clsCommonFunctions();
                IDataParameter[] strParam = { new SqlParameter("@In_Login_ID", Session["userId"]),
                                            new SqlParameter("@in_project_id","20") };

                objCommon.AddInParameters(strParam);
                objCommon.ExecuteNonQuerySP("Help_dbo.st_UPD_LoginHistory");
            }
          //  System.Web.HttpContext.Current.Response.Expires = -1;
            Session.RemoveAll();
            Session.Abandon();
          //  Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
         return View();
        }
        public ActionResult SessionExpire()
        {
            //if (Request.IsAjaxRequest())
            //{
            //    //Session.RemoveAll();
            //    //Session.Abandon();
            //    //return RedirectToAction("SessionExpire");
                
            //}
            //System.Web.HttpContext.Current.Response.Buffer = true;
            //System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
            //System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //System.Web.HttpContext.Current.Response.Expires = -1;
            //System.Web.HttpContext.Current.Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
            Session.RemoveAll();
            Session.Abandon();
            return View();
        }
        public ActionResult SessionExpirepopup()
        {
            
            //System.Web.HttpContext.Current.Response.Buffer = true;
            //System.Web.HttpContext.Current.Response.CacheControl = "no-cache";
            //System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //System.Web.HttpContext.Current.Response.Expires = -1;
            //System.Web.HttpContext.Current.Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1d) });
            Session.RemoveAll();
            Session.Abandon();
            return View();
        }
         [NotRequireHttps]
        public ActionResult Contact()
        {
            try
            {
                //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
                //{
                //    ViewBag.ShowArticles = "Y";
                //}
                //else
                //{
                //    ViewBag.ShowArticles = "N";
                //}
                //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
                //{
                //    ViewBag.Showforums = "Y";
                //}
                //else
                //{
                //    ViewBag.Showforums = "N";
                //}
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
                {
                    //ViewBag.ShowElectricians = "Y";
                
                if (Request.Url != null) Requesturl = Request.Url.ToString();
                var strurl = Requesturl.Split('/');
                //var objconfig = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    if (Requesturl.Contains("localhost:"))
                    {
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                        ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + "Images/line-bg.gif";
                        ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";
                    }
                    else if (Requesturl.Contains("vbv"))
                    {
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                        ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/line-bg.gif";
                        ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/speacer.gif";
                    }

                }
                else
                {
                    ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                    ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + "Images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";


                }
                FeaturedTherspists();
                }
                //else
                //{
                //    ViewBag.ShowElectricians = "N";
                //}
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "Contact", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return View();
        }
         [ValidateInput(false)]
         [NotRequireHttps]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Contact(ContactCog obj)
        {
            if (Session["captchastring"] != null)
            {
                if (Request["txtimgKey"] != Convert.ToString(HttpContext.Session["captchastring"]))
                {
                    return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code"), JsonRequestBehavior.DenyGet);
                }
            }
             var contact = new ContactCog();
             //var cs = new clsWebConfigsettings();
             string strOutMailId = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
             string strInMailId = clsWebConfigsettings.GetConfigSettingsValue("In_Email_Id");
             const string strFax = "";
             string categorycount = Convert.ToString(Category.EmailMsgcount(6));
             var objcategory = new Category
             {
                 EmailCategoryCount = Convert.ToInt32(categorycount),
                 EmailCategoryID = "6",
                 FromReference_id = null,
                 Toreference_id = "1",
                 ISWebService_IND = "N",
                 Application_Id = null
             };


             Category.SentEmailLog(objcategory);
             //string txtPhone;
             //if (Request["txtphone1"] != null & Request["txtphone2"] != null & Request["txtphone3"] != null)
             //{
             //    txtPhone = Request["txtphone1"] + Request["txtphone2"] + Request["txtphone3"];
             //}
             //else
             //{
             //    txtPhone = null;
             //}
            //string strphone = !string.IsNullOrEmpty(txtPhone) ? txtPhone : null;
             string strphone = Request["txtphone1"] != null && Request["txtphone2"] != null && Request["txtphone3"] != null ? Request["txtphone1"] + Request["txtphone2"] + Request["txtphone3"] : null;
             contact.ContactFirstName = Request["txtName"];
             contact.ContactLastName = Request["txtLastName"] ;
             contact.ContactMail = Request["txtEmail"];
             contact.ContactPhone = (!string.IsNullOrEmpty(strphone) ? strphone : null);
             contact.ContactSubject = Request["txtSubject"];
             contact.ContactMessage = Sanitizer.GetSafeHtmlFragment(Request["txtMessage"]);
             contact.ContactFax = (!string.IsNullOrEmpty(strFax) ? strFax : null);
             contact.CreatedBy = null;
             contact.siteurl = null;
             ContactCog.Contact_Us(contact);
             if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
             {
                 IDataParameter[] strInsParam = {
                     new SqlParameter("@In_ToReferenceType_ID", null),
                     new SqlParameter("@In_ToReference_ID", null),
                     new SqlParameter("@In_FromReferenceType_ID", null),
                     new SqlParameter("@In_FromReference_ID", null),
                     new SqlParameter("@In_Practice_ID", null),
                     new SqlParameter("@In_MsgOption_ID", null),
                     new SqlParameter("@in_TomailID", strOutMailId),
                     new SqlParameter("@in_FromMailID", strInMailId),
                     new SqlParameter("@in_Subject", contact.ContactSubject),
                     new SqlParameter("@in_Message", contact.ContactMessage)
                 };
                 IDataParameter[] strOutParam = { new SqlParameter("@Out_MsgTransaction_ID", SqlDbType.Int) };
                 var objWrapper = new clsDBWrapper();
                 var objMailMessage = new ClsMailMessage();

                 var with1 = objWrapper;
                 with1.AddInParameters(strInsParam);
                 with1.AddOutParameters(strOutParam);
                 with1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_Ins_Message_Pr");
                 string strEmailMessageTransactionId = "";
                 if (objWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value != null)
                 {
                     strEmailMessageTransactionId = objWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value.ToString();
                 }
                //string str;
                 string str = " <b> Subject : </b> " + Request["txtSubject"] + "<br />";
                 str = str + " <b> First name : </b>" + Request["txtName"] + " <br />";
                 str = str + " <b> Last name : </b> " + Request["txtLastName"] + " <br />";
                 str = str + " <b> Email id : </b> " + Request["txtEmail"] + " <br />";
                 str = str + " <b> Phone : </b> " + strphone + " <br />";
            //     str = str + " <b> Fax : </b> " + strFax + " <br />";
                 str = str + " <b> Message : </b>" + Request["txtMessage"];
                 bool res = objMailMessage.SendMail(strOutMailId, strInMailId, contact.ContactSubject, str, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                 if (res == true)
                 {
                     IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessageTransactionId) };
                     var with2 = objWrapper;
                     with2.AddInParameters(strMailParam);
                     with2.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                 }

             }
             return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
        }
         public CaptchaImageResult ShowCaptchaImage()
         {
             return new CaptchaImageResult();
         }
        public void FeaturedTherspists()
        {
            try{
            if ( StateName == null & CityName == null )
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
                Zip = null,
                City_ID = 0,
                State_ID = 0,
                StateName = StateName,
                CityName = CityName,
                Country_ID = 0,
                Distance = 0,
                Longitude = Longitude,
                Lattitude = Lattitude
            };
            if ( CityName != null | StateName != null )
            {
                objData.IsSearch = "Y";
            }
            else
            {
                objData.IsSearch = null;
            }
            var objlist = Provider_AdvancedSearch.list_Featuredtherapists(objData);
           if (objlist.Tables[0].Rows.Count > 0)
           {
               ViewBag.FeaturedList = objlist.Tables[0].AsEnumerable();
               ViewBag.FeaturedListcount = objlist.Tables[0].Rows.Count;
           }
           else
           {
               ViewBag.FeaturedList = null;
               ViewBag.FeaturedListcount = 0;
           }
            //if (objlist.Tables[0].Rows.Count > 0)
            //{
            //    int cnt;
            //   for (cnt = 0; cnt <= objlist.Tables[0].Rows.Count - 1; cnt++)
            //    {

            //         ProviderAdvertising objproviderAdvertise=new ProviderAdvertising();
            //        objproviderAdvertise.SiteStatic_ID = "2";
            //        objproviderAdvertise.State_Name = StateName;
            //        objproviderAdvertise.CityNm = CityName;
            //        objproviderAdvertise.Provider_ID = objlist.Tables[0].Rows[cnt]["Provider_ID"].ToString();
            //        objproviderAdvertise.CountingProviderVisitToHisProfile(objproviderAdvertise);
                  
            //    }
            //}
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "FeaturedTherspists", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        [AllowAnonymous]
        [RequireHttpsByConfig]
        public ActionResult ForgotPassword()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                //ViewBag.ShowElectricians = "Y";
                FeaturedTherspists();
            }
            else
            {
                //ViewBag.ShowElectricians = "N";
            }
               
            if (Request["msg"] != null & Request["msg"] != "")
            {
                ViewBag.msg = Convert.ToString(Request["msg"]);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttpsByConfig]
        public ActionResult ForgotPassword(string username)
        {
            //var objconfig = new clsWebConfigsettings();
	try {
        if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES" | clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "NO")
        {
			var objMailMessage = new ClsMailMessage();
            string strsubject = null;
			string strbody = null;
			string mailfrom = null;
			string mailto = null;
			string providername = null;
            string uname = null;
            if (Request["txtUserName"] != "")
            {
                uname = Request["txtUserName"];
                TempData["UserName"] = uname;
            }
            string categorycount = Convert.ToString(Category.EmailMsgcount(15));
            var objcategory = new Category
            {
                EmailCategoryCount = Convert.ToInt32(categorycount),
                EmailCategoryID = "15",
                FromReference_id = "1",
                Toreference_id = null,
                ISWebService_IND = "N",
                Application_Id = null,
                FromReferenceType_id = "1",
                Toreferencetype_id = null
            };
            Category.SentEmailLog(objcategory);
			var objclscommon = new clsCommonFunctions();
            IDataParameter[] objInparameters = { new SqlParameter("@in_email", uname) };
			IDataParameter[] objOutParameters = { new SqlParameter("@OUT_MSG", SqlDbType.VarChar, 500) };
			objclscommon.AddInParameters(objInparameters);
			objclscommon.AddOutParameters(objOutParameters);
			objclscommon.GetDataReader("Help_dbo.st_ins_accesscode_and_email_user");
			if (Convert.ToString(objclscommon.objdbCommandWrapper.Parameters["@OUT_MSG"].Value)!="" )
			{
			    string straccesscode = Convert.ToString(objclscommon.objdbCommandWrapper.Parameters["@OUT_MSG"].Value);
			    if (straccesscode.Contains("Please enter valid email")) {
                    return RedirectToAction("ForgotPassword",new{msg="t"});
				}
			    IDataParameter[] strParam = {
			        new SqlParameter("@in_email", uname),
			        new SqlParameter("@in_ind", 1)
			    };
			    objclscommon.AddInParameters(strParam);
			    SqlDataReader drReader = objclscommon.GetDataReader("Help_dbo.st_get_emailmessage_forgotpasswordemail");
			    if (drReader.Read()) {
			        if (!string.IsNullOrEmpty(Convert.ToString(drReader["Subject"]))) {
			            strsubject = drReader["Subject"].ToString();
			        }
			        if (!string.IsNullOrEmpty(Convert.ToString(drReader["Message_Body"]))) {
			            strbody = drReader["Message_Body"].ToString();
			        }
			        if (!string.IsNullOrEmpty(Convert.ToString(drReader["Mail_From"]))) {
			            mailfrom = drReader["Mail_From"].ToString();
			        }
			        if (!string.IsNullOrEmpty(Convert.ToString(drReader["Mail_To"]))) {
			            mailto = drReader["Mail_To"].ToString();
			        }
                    //if (!string.IsNullOrEmpty(Convert.ToString(drReader["Full_name"]))) {
                    //    providername = drReader["Full_name"].ToString();
                    //}
                    if (strbody != null && !string.IsNullOrEmpty(Convert.ToString(drReader["Full_name"])))
                    {
                        providername = drReader["Full_name"].ToString();
			            strbody = strbody.Replace("$AccessCode$", straccesscode);
			            strbody = strbody.Replace("$ProviderName$", providername);
			        }
			    }

			    bool strvalid = objMailMessage.SendMail(mailto, mailfrom, strsubject, strbody, EMailFormats.MailFormatText, EMailPriorities.PriorityNormal);
                string strEmailMessageTransactionId = Convert.ToString(drReader["EmailMessage_Transaction_ID"]);
                if (strvalid == true)
                {
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessageTransactionId) };
                    objclscommon.AddInParameters(strMailParam);
                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }
			    if (strvalid == true) {
			        return RedirectToAction("ForgotPassword", new { msg = "t" });

			    }
			    return RedirectToAction("ForgotPassword",new{msg="t"});
			}
        }
	}
    catch (Exception ex) {
		var clscustomexc = new clsExceptionLog();
        clscustomexc.LogException(ex, "HomeController", "ForgotPassword", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
	}

    return RedirectToAction("ForgotPassword");
        }
        private void InsertUserTracking()
        {
            try{
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
            objdata.IPAddress = HttpContext.Request.ServerVariables["remote_addr"];//find Client IP Address on which the Website is running
            objdata.UserAgent = HttpContext.Request.UserAgent;//identify the device
            if (Request.Url != null) objdata.Siteurl = Request.Url.ToString();
            long trackId = objdata.InsertVisitorTracking(objdata);
            var newCookie = new HttpCookie("TrackID") {Value = Convert.ToString(trackId)};
            Response.Cookies.Add(newCookie);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "HomeController", "InsertUserTracking", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        [AllowAnonymous]
        [RequireHttpsByConfig]
        public ActionResult verification(string accesscode)
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();


            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            //if (TempData["validmsg"]!=null)
            //{
            //    ViewBag.validmsg = TempData["validmsg"];
            //}
            if (!string.IsNullOrEmpty(accesscode))
            {
                ViewBag.txtAccesscode = accesscode;
            }
            //TempData.Keep("validmsg");
            return View();
        }
        [AllowAnonymous]
        [RequireHttpsByConfig]
        public ActionResult Activate(string code)
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();


            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            string days = "Congratulations! Your email address is verified and your account is successfully activated. You can login now to use Mower Helper website and mobile applications. Please remember you have $xx$ days for trying Mower Helper application. Prior to the expiration period please chose either Monthly or Yearly payment option.";

            days = days.Replace("$xx$", clsWebConfigsettings.GetConfigSettingsValue("Trial_package_days"));
            ViewBag.Trialperiodmessage = days;
            if (!string.IsNullOrEmpty(code))
            {
                ViewBag.code = code;
            }
            var objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={new SqlParameter("@In_activationcode", ViewBag.code)};
            objcommon.AddInParameters(insparam);
            SqlDataReader drReader = objcommon.GetDataReader("Help_dbo.st_new_provider_change_status");
            if (drReader.Read())
            {
                if (!string.IsNullOrEmpty(Convert.ToString(drReader["Text_message"])))
                {
                    ViewBag.msg = drReader["Text_message"].ToString();
                }
            }
            drReader.Close();
            return View();
        }
        public ActionResult ActivationLink()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();


            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttpsByConfig]
        public ActionResult verification()
        {
            var  objmd5 = new VBVMD5CryptoServiceProvider();
            
            //  string hash = string.Empty;                     ---------
            //if(!string.IsNullOrEmpty(Request["txtPwd"]))
            //{
            //    hash= objmd5.getMd5Hash(Request["txtPwd"]);
            //}
            var objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={
                                      new SqlParameter("@in_email", null),
                                      new SqlParameter("@in_Accesscode",Request["txtAccesscode"]!= "" ?Request["txtAccesscode"].Trim() : null),
                                        new SqlParameter("@in_Password",
                                            !string.IsNullOrEmpty(Request["txtPwd"])?objmd5.getMd5Hash(Request["txtPwd"]):null),
                                      new SqlParameter("@in_Encrypted_Password",
                                          !string.IsNullOrEmpty(Request["txtPwd"])?objmd5.getMd5Hash(Request["txtPwd"]):null)     
                                      //new SqlParameter("@in_Password",hash),
                                      //new SqlParameter("@in_Encrypted_Password",hash)                                     
                                       };
            IDataParameter[] outparam={
                                          new SqlParameter("@OUT_MSG",SqlDbType.VarChar,1000),
                                          new SqlParameter("@OUT_MSG1",SqlDbType.VarChar,100),
                                          new SqlParameter("@OUT_MSG2",SqlDbType.VarChar,100)
                                      };
            objcommon.AddInParameters(insparam);
            objcommon.AddOutParameters(outparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.st_ins_passworddetails");
            if(!DBNull.Value.Equals(objcommon.objdbCommandWrapper.Parameters["@OUT_MSG"].Value)){
                TempData["validmsg"] = objcommon.objdbCommandWrapper.Parameters["@OUT_MSG"].Value.ToString();
                return RedirectToAction("verification");
            }
            else{
                string strmessage = sendEmaillostpassword(Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@OUT_MSG2"].Value), Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@OUT_MSG1"].Value));
             if (strmessage != null)
             {
                 if (strmessage == "true")
                 {
                     TempData["validmsg"] = "Your Mower Helper Password has been reset successfully.";
                     return RedirectToAction("verification");
                 }
                 else
                 {
                      TempData["validmsg"] = "Your email has not been sent.Due to some technical problems" ;
                     return RedirectToAction("verification");
                 }
             }

            }
            return View();
        }
        public string sendEmaillostpassword(string electEmail, string electlogin)
        {
            //var objconfig = new clsWebConfigsettings();
            string strResult = null;
             
             try
             {
                 if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES" | clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "NO")
                 {
                     string strsubject = null;
                     string strbody = null;
                     string mailfrom = null;
                     string mailto = null;
                     string Providername = null;
                  
                     var objcommon=new clsCommonFunctions();
                     IDataParameter[] insparam={ new SqlParameter("@In_ReferenceLogin_ID",electlogin!=""?electlogin:null),
                                                   new SqlParameter("@In_Practice_ID",null),
                                                   new SqlParameter("@in_ToMailId",electEmail!=""?electEmail:null)
                     };
                     objcommon.AddInParameters(insparam);
                     objcommon.GetDataReader("Help_dbo.st_EmailMessage_INS_forgotmailconfirmationMessage");
                     IDataParameter[] insmail ={
                                              new SqlParameter("@in_email",electEmail!="" ? electEmail : null  ),
                                              new SqlParameter("@in_ind",2)
                                              };
                     objcommon.AddInParameters(insmail);
                     SqlDataReader drEmail = objcommon.GetDataReader("Help_dbo.st_get_emailmessage_forgotpasswordemail");
                     string strEmailMessage_Transaction_ID = string.Empty;
                     if (drEmail.Read())
                     {
                         if (!DBNull.Value.Equals(drEmail["Subject"]))
                         {
                             strsubject = Convert.ToString(drEmail["Subject"]);
                         }
                         if (!DBNull.Value.Equals(drEmail["Message_Body"]))
                         {
                             strbody = Convert.ToString(drEmail["Message_Body"]);
                         }
                         if (!DBNull.Value.Equals(drEmail["Mail_From"]))
                         {
                             mailfrom = Convert.ToString(drEmail["Mail_From"]);   
                         }
                         if (!DBNull.Value.Equals(drEmail["Mail_To"]))
                         {
                             mailto = Convert.ToString(drEmail["Mail_To"]);
                         }
                         //if (!DBNull.Value.Equals(drEmail["Full_name"]))
                         //{
                         //    Providername = Convert.ToString(drEmail["Full_name"]);
                         //}

                         if (!string.IsNullOrEmpty(strbody) && !DBNull.Value.Equals(drEmail["Full_name"]))
                         {
                             Providername = Convert.ToString(drEmail["Full_name"]);
                             strbody = strbody.Replace("$ProviderName$",Providername);
                         }
                     }
                     ClsMailMessage objmessage=new ClsMailMessage();
                     bool strvalid = objmessage.SendMail(mailto, mailfrom, strsubject, strbody, EMailFormats.MailFormatText, EMailPriorities.PriorityNormal);
                     strEmailMessage_Transaction_ID =Convert.ToString(drEmail["EmailMessage_Transaction_ID"]);
                     if (strvalid == true)
                     {
                         IDataParameter[] strmail ={
                                                   new SqlParameter("@In_EmailMessage_Transaction_IDs",strEmailMessage_Transaction_ID)
                                              };
                         objcommon.AddInParameters(strmail);
                         objcommon.GetDataReader("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                     }
                     if (strvalid == true)
                     {
                         strResult = "true";

                         //return RedirectToAction("verification", new { validmsg = "Your Electrician help Password has been reset successfully." });
                     }
                     else
                     {
                         strResult = "False";
                         //return RedirectToAction("verification", new { validmsg = "Your email has not been sent.Due to some technical problems" });
                     }
                    
                 }
                 return strResult;
             }
            catch(Exception ex){
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "HomeController", "sendEmaillostpassword", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
             return strResult;
        }
        public ActionResult Termsofuse()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            var objcommonfunction = new clsCommonFunctions();
            //string strTerms = null;
            string strTerms = objcommonfunction.GetSiteInfo(null,65, null);
            ViewBag.InnerHtml = "<A Name='TOP'></A>" + strTerms;
            return View();
        }
        public ActionResult PartnerSites()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                    ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + "Images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";
                }
                else if (Requesturl.Contains("vbv"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                    ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/speacer.gif";
                }

            }
            else
            {
                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                ViewBag.lineimg = strurl[0] + "//" + strurl[2] + "/" + "Images/line-bg.gif";
                ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "Images/speacer.gif";


            }
            FeaturedTherspists();
            return View();
        }
        public ActionResult LinkedSites()
        {
            return View();
        }
        public ActionResult Acknowledgement()
        {
            return View();
        }
    }
}

