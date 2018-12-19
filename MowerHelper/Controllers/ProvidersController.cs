using Obout.Mvc.ComboBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Practice;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class ProvidersController : Controller
    {
        clsCommonFunctions objcommon = new clsCommonFunctions();
        //clsWebConfigsettings objconfig = new clsWebConfigsettings();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Providerprofile(string LastName)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            string PageNo = null;
            string NoofRecords = null;
            TempData.Remove("hdnPrid");
            PageNo =(Request["page"]!=null?Request["page"]:"1");
            Nullable<int> _NPracID = null;
            string PlaceOfService_ID = null;
            int totrec = 0;
            int ProvID = 0;
            string stremail = null;
            if (Request["ComboBox1"] != null & Request["ComboBox1"] != "")
            {
                ProvID = Convert.ToInt32(Request["ComboBox1"]);
                ViewBag.ProvID = Convert.ToInt32(Request["ComboBox1"]);
                TempData["combotext"] = Convert.ToString(Request["ComboBox1_SelectedText"]);
                //ViewBag.combo = Request["ComboBox1_SelectedText"].ToString();
            }
            else
            {
                ViewBag.ProvID = null;
                TempData["combotext"] = "";
            }
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = "10";
            }
            if (Request["txtEmail"] != null & Request["txtEmail"] != "")
            {
                ViewBag.email = Request["txtEmail"];
                stremail = Request["txtEmail"];
            }
            else
            {
                ViewBag.email = null;
                stremail = null;
            }
            List<Provider_ProviderList> ElProviderList = new List<Provider_ProviderList>();
            ElProviderList = Provider_ProviderList.Get_ElProviderInProfile(null, LastName, _NPracID, Convert.ToInt32(PlaceOfService_ID), (Request["sort"] ?? "ProviderName"), (Request["sortdir"] ?? "ASC"), Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["RoleId"]), "A", Convert.ToInt32(ViewBag.pagesize),
Convert.ToInt32(PageNo), "ThAdmin", ref totrec, stremail, ProvID);
            ViewBag.totrec = totrec;
           
            return View(ElProviderList);
        }
        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredproviders(args.Text, 0);

            JsonResult result = new JsonResult
            {
                Data = new
                {
                    Items = items
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return result;
        }

        protected ComboBoxItemList GetFilteredproviders(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName =null;
            if (filter != null)
            {
               PracticeName = filter;
               TempData["combotext"] = "";
            }
            else if (Convert.ToString(TempData["combotext"]) != "")
            {
                PracticeName = TempData["combotext"].ToString().Substring(0,2);
            }
            else
            {
                PracticeName = null;
            }
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox1"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ProvidersController", "GetFilteredproviders", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult Providerinactive(string Provider_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Provider_ProviderList.Inactivate_Provider_Status(Convert.ToInt32(Provider_ID));
            return RedirectToAction("Providerprofile");
        }
        public ActionResult Provideractive(string Provider_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Provider_ProviderList.Upd_Vacation_Provider_Status(Convert.ToInt32(Provider_ID));
            return RedirectToAction("Providerprofile");
        }
        public ActionResult ProviderDelete(string Provider_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Provider_ProviderList.Del_Provider(Convert.ToInt32(Provider_ID), Convert.ToInt32(Session["userid"]));
            return RedirectToAction("Providerprofile");
        }

        public ActionResult Providerloginhistory(string Login_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string loginid = null;
            if (Login_ID != null)
            {
                ViewBag.Login_ID = Login_ID;
                loginid = Login_ID;
            }
            else
            {
                loginid = Request["hdnloginid"];
                ViewBag.Login_ID = loginid;
            }
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");

            List<Login_history> Login_historycollection = null;
            Login_history objHistory = new Login_history();



            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.expsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.expsize;
                pgesize = ViewBag.expsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.expsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.expsize;
            }
            else
            {
                ViewBag.expsize = "10";
                pgesize = ViewBag.expsize;
            }

            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            ViewBag.cat = Request["txtCategory"];
            if (Request["sortdir"] != null & Request["sortdir"] != "")
            {
                ViewBag.sortdirection = Request["sortdir"];
            }
            else
            {
                ViewBag.sortdirection = null;
            }
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            {
                FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy") : Request["txt_FromDate"]);
                ToDate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? startdate : Request["txt_ToDate"]);
            }
            else
            {

                if (Request["dt_filter"] == "Today")
                {
                    FromDate = startdate;
                }
                else if (Request["dt_filter"] == "7")
                {
                    FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
                }
                else if (Request["dt_filter"] == "30")
                {
                    FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
                }
                ToDate = startdate;
            }
            if (Request["ddlsite"] != null)
            {
                ViewBag.ProjectId = Request["ddlsite"];
                objHistory.Project_ID = Convert.ToInt32(Request["ddlsite"]);
            }
            else
            {
                ViewBag.ProjectId = "20";
                objHistory.Project_ID = 20;
            }
            if (Request["ddlmobile"] != "" && Request["ddlmobile"] != null)
            {
                ViewBag.Mobile_type = Request["ddlmobile"].ToString();
                if (Request["ddlmobile"].ToString() != "0")
                {

                    objHistory.Mobile_type = Request["ddlmobile"].ToString();
                }
                else
                {
                    objHistory.Mobile_type = null;
                }
            }
            else
            {
                objHistory.Mobile_type = null;
                ViewBag.Mobile_type = "0";
            }
            objHistory.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            objHistory.NoofRecords = Convert.ToInt32(pgesize);
            objHistory.Login_ID = Convert.ToInt32(loginid);
            objHistory.OrderBy = "DESC";
            objHistory.OrderByItem = "LogIn_DateTime";
            objHistory.BeginDate = FromDate;
            objHistory.EndDate = ToDate;
            //objHistory.Project_ID = Convert.ToInt32(stateid);
            //objHistory.Mobile_type = Request["ddlmobile"];
            //ViewBag.ProjectId = Request["ddlsite"];
            //ViewBag.Mobile_type = Request["ddlmobile"];
            int totrec = 0;
            //Login_historycollection = Login_history.Admin_Provider_Login_history(objHistory, ref totrec);
            Login_historycollection = Login_history.Admin_List_Login_history(objHistory, ref totrec, null);
            ViewBag.totrec = totrec;
            if (!string.IsNullOrEmpty(Request["ProName"]))
            {
                ViewBag.UserName = Request["ProName"];
            }
            else {
                ViewBag.UserName = Request["hdnUname"];
            }
            return View(Login_historycollection);
        }
        public ActionResult Temporarypassword(string Login_ID, string Provider_ID, string ProviderName, string Practice_ID)
        {
           
                PracticeblHome obj = PracticeblHome.Get_SendMailsToProviderDetails(Convert.ToInt32(Login_ID), Provider_ID);
                if ((obj != null))
                {
                    ViewBag.email = obj.email;
                    ViewBag.ProviderName = ProviderName;
                    ViewBag.Login_ID = Login_ID;
                    ViewBag.Practice_ID = Practice_ID;
                }

                PatientRegistration Obj1 = default(PatientRegistration);
                Obj1 = PatientRegistration.Get_Random_UserCredentials();
                ViewBag.password = Obj1.Password;
                return View();
          
        }
        [HttpPost]
        public ActionResult Temporarypassword()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string strPassword = null;
            var objmd = new VBVMD5CryptoServiceProvider();
            strPassword = objmd.getMd5Hash(Request["Hdnpassword"]);
            Provider_Password obj = new Provider_Password();
            obj.Password = strPassword;
            obj.Login_ID = Convert.ToInt32(Request["HdnLoginiD"]);
            Provider_Password.Temp_Ins_Password(obj);
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {
                EmailMessageOption EMO = default(EmailMessageOption);
                EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(Convert.ToInt32(1), null);
                if ((EMO != null))
                {
                    if ((EMO.Msg_Subject != null))
                    {
                        ViewBag.Subject = EMO.Msg_Subject;
                    }
                    else
                    {
                        ViewBag.Subject = null;
                    }
                    if ((EMO.Msg_Body != null))
                    {
                        ViewBag.Content = EMO.Msg_Body;
                    }
                    else
                    {
                        ViewBag.Content = null;
                    }
                }
                string strContent = ViewBag.Content;
                if (strContent != null)
                {
                    if (Request["HdnProviderName"] != null)
                    {
                        strContent = strContent.Replace("$Providername$", Request["HdnProviderName"]);
                    }
                    if (Request["Hdnusername"] != null)
                    {
                        strContent = strContent.Replace("$Username$", Request["Hdnusername"]);
                    }
                    if (Request["Hdnpassword"] != null)
                    {
                        strContent = strContent.Replace("$Password$", Request["Hdnpassword"]);
                    }
                }
                Reg_ProviderConfirmation ObjEmail = new Reg_ProviderConfirmation();
                ObjEmail.ProviderLogin_ID = Convert.ToInt32(Request["HdnLoginiD"]);
                ObjEmail.Email = Convert.ToString(Request["Hdnusername"]);
               string stremail= Reg_ProviderConfirmation.Reg_Insert_EmailMessage(ObjEmail);
               string strOutMailId = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                ClsMailMessage ObjSendMessage = new ClsMailMessage();
               bool strvalid= ObjSendMessage.SendMail(Convert.ToString(Request["Hdnusername"]), strOutMailId, ViewBag.Subject, strContent, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
               if (strvalid == true)
               {
                   clsCommonFunctions objclscommon = new clsCommonFunctions();
                   IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", stremail) };
                   objclscommon.AddInParameters(strMailParam);
                   objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
               }
            }




            return RedirectToAction("Providerprofile");
        }
        public ActionResult Emailusercredentials(string Login_ID, string Provider_ID, string ProviderName, string Practice_ID)
        {
            
                PracticeblHome obj = PracticeblHome.Get_SendMailsToProviderDetails(Convert.ToInt32(Login_ID), Provider_ID);
                if ((obj != null))
                {
                    ViewBag.email = obj.email;
                    ViewBag.ProviderName = ProviderName;
                    ViewBag.Provider_ID = Provider_ID;
                    ViewBag.Practice_ID = Practice_ID;
                    ViewBag.Login_ID = Login_ID;
                }
                return View();
            
          
        }
        [HttpPost]
        public ActionResult Emailusercredentials()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["Hdnupdmail"] == "Y")
            {
                PracticeblHome.UpdateDetails(Convert.ToInt32(Request["HdnProvid"]), Request["txtEmailAddress"]);
            }
                        PracticeblHome obj = PracticeblHome.Get_SendMailsToProviderDetails(Convert.ToInt32(Request["HdnLoginiD"]),Request["HdnProvid"]);
                        if ((obj != null))
                        {
                            ViewBag.Username = obj.email;
                            //ViewBag.Password = obj.password;
                        }
                        PatientRegistration Obj1 = default(PatientRegistration);
                        Obj1 = PatientRegistration.Get_Random_UserCredentials();
                        ViewBag.Password = Obj1.Password;

                        var objmd = new VBVMD5CryptoServiceProvider();
                       string strPassword = objmd.getMd5Hash(ViewBag.Password);


                        Provider_Password obj2 = new Provider_Password();
                        obj2.Password = strPassword;
                        obj2.Login_ID = Convert.ToInt32(Request["HdnLoginiD"]);
                        Provider_Password.Temp_Ins_Password(obj2);

                        if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
                     {

                string categorycount = Convert.ToString(Category.EmailMsgcount(13));
                var objcategory = new Category
                {
                    EmailCategoryCount = Convert.ToInt32(categorycount),
                    EmailCategoryID = "13",
                    FromReference_id = "1",
                    Toreference_id = Request["HdnProvid"],
                    ISWebService_IND = "N",
                    Application_Id = null,
                    FromReferenceType_id = "1",
                    Toreferencetype_id = "2"
                };
                Category.SentEmailLog(objcategory);

                string strProvider = Request["HdnProviderName"];
                EmailMessageOption EMO = default(EmailMessageOption);
                EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(Convert.ToInt32(1), null);
                if ((EMO != null))
                {
                    if ((EMO.Msg_Subject != null))
                    {
                        ViewBag.Subject = EMO.Msg_Subject;
                    }
                    else
                    {
                        ViewBag.Subject = null;
                    }
                    if ((EMO.Msg_Body != null))
                    {
                        ViewBag.Content = EMO.Msg_Body;
                    }
                    else
                    {
                        ViewBag.Content = null;
                    }
                }
                string strContent = ViewBag.Content;
                if (strContent != null)
                {
                    if (strProvider != null)
                    {
                        strContent = strContent.Replace("$Providername$", strProvider);
                    }
                    if (ViewBag.Username != null)
                    {
                        strContent = strContent.Replace("$Username$", ViewBag.Username);
                    }
                    if (ViewBag.Password != null)
                    {
                        strContent = strContent.Replace("$Password$", ViewBag.Password);
                    }
                }
                Reg_ProviderConfirmation ObjEmail = new Reg_ProviderConfirmation();
                ObjEmail.ProviderLogin_ID = Convert.ToInt32(Request["HdnLoginiD"]);
                ObjEmail.Email = Convert.ToString(Request["txtEmailAddress"]);
                string emailTranid = Reg_ProviderConfirmation.Reg_Insert_EmailMessage(ObjEmail);
                string strOutMailId = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                ClsMailMessage ObjSendMessage = new ClsMailMessage();
               bool strvalid= ObjSendMessage.SendMail(Convert.ToString(Request["txtEmailAddress"]), strOutMailId, ViewBag.Subject, strContent, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
               if (strvalid == true)
               {
                   clsCommonFunctions objclscommon = new clsCommonFunctions();
                   IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", emailTranid) };
                   objclscommon.AddInParameters(strMailParam);
                   objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");

               }

            }
            return RedirectToAction("Providerprofile");
        }
        public ActionResult Providersprofile(string LastName)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string PageNo = null;
            PageNo = (Request["page"] != null ? Request["page"] : "1");
            Nullable<int> _NPracID = null;
            string PlaceOfService_ID = null;
            int totrec = 0;
            int ProvID = 0;
            string stremail = null;
            if (Request["ComboBox1"] != null & Request["ComboBox1"] != "")
            {
                ProvID = Convert.ToInt32(Request["ComboBox1"]);
                ViewBag.ProvID = Convert.ToInt32(Request["ComboBox1"]);
            }
            else
            {
                ViewBag.ProvID = null;
            }
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.pagesize = "10";
            }
            if (Request["txtEmail"] != null & Request["txtEmail"] != "")
            {
                ViewBag.email = Request["txtEmail"];
                stremail = Request["txtEmail"];
            }
            else
            {
                ViewBag.email = null;
                stremail = null;
            }
            List<Provider_ProviderList> ElProviderList = new List<Provider_ProviderList>();
            ElProviderList = Provider_ProviderList.Get_ElProviderInProfile(null, LastName, _NPracID, Convert.ToInt32(PlaceOfService_ID), "ProviderName", "ASC", Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["RoleId"]), "A", Convert.ToInt32(ViewBag.pagesize),
Convert.ToInt32(PageNo), "ThAdmin", ref totrec, stremail, ProvID);
            ViewBag.totrec = totrec;
            return View(ElProviderList);
        }
        public ActionResult ProviderPayType(string providerLoginId)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //string monthYearType = PracticeblHome.getpaymentType(Convert.ToInt32(providerLoginId));
             ViewBag.Login_ID = providerLoginId;
                PracticeblHome obj = new PracticeblHome();
                string monthYearType = obj.getpaymentType(Convert.ToInt32(providerLoginId));
                if (!string.IsNullOrEmpty(monthYearType))
                {
                    ViewBag.monthYearType = monthYearType;
                }
                else
                {
                    ViewBag.monthYearType = "22";
                }
                return View();
           
        }
        [HttpPost]
        public ActionResult ProviderPayType()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
         int pId= Convert.ToInt32(Request["HdnLoginiD"]);
         string defPayType = Request["rblMonthly"];
         string defPayType1 = Request["rblYearly"];
         int payvalue;
         if (!string.IsNullOrEmpty(defPayType) )
         {
             payvalue = 22;
         }
         else if (!string.IsNullOrEmpty(defPayType1))
         {
             payvalue = 23;
         }
         else
         {
             payvalue = 22;
         }
         PracticeblHome obj = new PracticeblHome();
         obj.updatePaytype(pId, payvalue);
         return RedirectToAction("Providerprofile");
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProviderDebugMode(string Provider_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                ViewBag.Provider_ID = Provider_ID;
                PracticeblHome obj = new PracticeblHome();
                string DebugModeType = obj.getDebugType(Convert.ToInt32(Provider_ID));
                if (!string.IsNullOrEmpty(DebugModeType))
                {
                    ViewBag.DebugModeType = DebugModeType;
                }
                else
                {
                    ViewBag.DebugModeType = "N";
                }
                return View();
            
          
        }
        [HttpPost]
        public ActionResult ProviderDebugMode()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            int pId = Convert.ToInt32(Request["HdnProvider_ID"]);
            string debugType = Request["rblDebugYes"];
            string defbugType1 = Request["rblDebugNo"];
            string debugvalue;
            if (!string.IsNullOrEmpty(debugType))
            {
                debugvalue = "Y";
            }
            else if (!string.IsNullOrEmpty(defbugType1))
            {
                debugvalue = "N";
            }
            else
            {
                debugvalue = "N";
            }
            PracticeblHome obj = new PracticeblHome();
            obj.updateDebugtype(pId, debugvalue);
            return RedirectToAction("Providerprofile");
        }
        public ActionResult ProviderDebugstatus(string Provider_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                PracticeblHome objdbug = new PracticeblHome();
                List<PracticeblHome> DebugList = null;
                objdbug.Provider_ID = Provider_ID;
                DebugList = PracticeblHome.DebugItemsList(objdbug);
                ViewBag.totrec = DebugList.Count;
                return View(DebugList);
           
        }


        public ActionResult ProviderLoginInformation(int LoginHistory_ID)
        {
            clsCommonFunctions clscommon = new clsCommonFunctions();
            DataSet dslogininfo = new DataSet();
            IDataParameter[] InParmList = { new SqlParameter("@in_LoginHistory_ID", LoginHistory_ID) };
            var _with1 = clscommon;
            _with1.AddInParameters(InParmList);
            dslogininfo = _with1.GetDataSet("Help_dbo.st_get_loginhistory");
            if ((dslogininfo != null))
            {
                if (dslogininfo.Tables[0].Rows[0]["Username"].ToString() != "")
                {
                    ViewBag.lblusername = dslogininfo.Tables[0].Rows[0]["Username"];
                }
                else
                {
                    ViewBag.lblusername = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["Useragent"].ToString() != "")
                {
                    ViewBag.lbluseragent = dslogininfo.Tables[0].Rows[0]["Useragent"];
                }
                else
                {
                    ViewBag.lbluseragent = null;
                }

                if (dslogininfo.Tables[0].Rows[0]["IPAddress"].ToString() != "")
                {
                    ViewBag.lblipaddress = dslogininfo.Tables[0].Rows[0]["IPAddress"];
                }
                else
                {
                    ViewBag.lblipaddress = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["zip"].ToString() != "")
                {
                    ViewBag.lblzip = dslogininfo.Tables[0].Rows[0]["zip"];
                }
                else
                {
                    ViewBag.lblzip = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["state"].ToString() != "")
                {
                    ViewBag.lblstate = dslogininfo.Tables[0].Rows[0]["state"];
                }
                else
                {
                    ViewBag.lblstate = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["city"].ToString() != "")
                {
                    ViewBag.lblcity = dslogininfo.Tables[0].Rows[0]["city"];
                }
                else
                {
                    ViewBag.lblcity = null;
                }

                if (dslogininfo.Tables[0].Rows[0]["LogIn_DateTime"].ToString() != "")
                {
                    ViewBag.lbllogintime = dslogininfo.Tables[0].Rows[0]["LogIn_DateTime"];
                }
                else
                {
                    ViewBag.lbllogintime = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["LogOut_DateTime"].ToString() != "")
                {
                    ViewBag.lbllogouttime = dslogininfo.Tables[0].Rows[0]["LogOut_DateTime"];
                }
                else
                {
                    ViewBag.lbllogouttime = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["Mobile_type"].ToString() != "")
                {
                    ViewBag.lblmobiletype = dslogininfo.Tables[0].Rows[0]["Mobile_type"];
                }
                else
                {
                    ViewBag.lblmobiletype = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["Deviceaddress"].ToString() != "")
                {
                    ViewBag.lblDeviceaddress = dslogininfo.Tables[0].Rows[0]["Deviceaddress"];
                }
                else
                {
                    ViewBag.lblDeviceaddress = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["Devicephonenumber"].ToString() != "")
                {
                    ViewBag.lblDevicephonenumber = dslogininfo.Tables[0].Rows[0]["Devicephonenumber"];
                }
                else
                {
                    ViewBag.lblDevicephonenumber = null;
                }
            }
            return View();
        }
    }
}
