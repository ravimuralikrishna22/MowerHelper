using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obout.Mvc.ComboBox;
using PayPal.Api;
using PayPal.Tenniscoach;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;


namespace MowerHelper.Controllers
{
    public class AdminHomeController : Controller
    {
        clsDBWrapper objDBWrapper = new clsDBWrapper();
        DataSet dsSections = new DataSet();
        //string totalrecords = "0";


        // int intCounter;
        string objstorecreditcardresponse;
        // private const int StartingYear = 2014;



        //public ActionResult Index()
        //{
        //    return View();
        //}
        public DataSet ChildSections(int login_id, string section_id, int Role_id, char IsBasicAccProv)
        {
            try
            {
                IDataParameter[] objInparameters = {
			new SqlParameter("@in_Login_ID", login_id),
			new SqlParameter("@in_Section_ID", section_id),
			new SqlParameter("@in_RoleId", Role_id),
			new SqlParameter("@in_Provider_ID", null),
			new SqlParameter("@in_Practice_ID", null),
			new SqlParameter("@In_IsBasicACPrv", IsBasicAccProv),
			new SqlParameter("@In_IsClerk_ind", null),
			new SqlParameter("@In_IsRecept_Ind", null)
		};


                objDBWrapper.AddInParameters(objInparameters);
                if (section_id == "2291")
                {
                    dsSections = objDBWrapper.GetDataSet("Help_dbo.st_Security_Get_HomeChildSections");
                }
                else
                {
                    dsSections = objDBWrapper.GetDataSet("Help_dbo.St_Security_Get_ChildSections");
                }
                return dsSections;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "ChildSections", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult Tools(string sectionid)
        {
            if (string.IsNullOrEmpty(sectionid) || sectionid == "365")
            {
                ViewBag.Count = 1;
                Session["CurrentUrl"] = Request.RawUrl;
            }
            else
            {
                ViewBag.Count = 3;
                Session["CurrentUrl2"] = Request.RawUrl;
            }
            if (Session["RoleID"] != null)
            {
                if (Session["RoleID"].ToString() == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            TempData.Remove("hdnPrid");
            Session.Add("TopIndex", "4");
            char AcPrv;
            if (Session["strOutIsPaid"] != null)
            {
                AcPrv = Convert.ToChar(Session["strOutIsPaid"]);
            }
            else
            {
                AcPrv = 'Y';
            }
            if (sectionid != null)
            {
                dsSections = ChildSections(Convert.ToInt32(Session["userid"]), sectionid, Convert.ToInt32(Session["Roleid"]), AcPrv);
            }
            else
            {
                dsSections = ChildSections(Convert.ToInt32(Session["userid"]), "365", Convert.ToInt32(Session["Roleid"]), AcPrv);
            }


            StringBuilder strAdmin = new StringBuilder();
            strAdmin = strAdmin.Append("<table id='Toolstable' border='0' cellpadding='20' cellspacing='0' width='100%' >");
            if (dsSections.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                strAdmin = strAdmin.Append("<tr class='background_color'>");
                string RedirectPath = string.Empty;
                foreach (DataRow drs in dsSections.Tables[0].Rows)
                {
                    if (Convert.ToString(drs["SectionPath"]) == "Providers/ProviderProfile.aspx")
                    {
                        RedirectPath = "../Providers/ProviderProfile";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "3200")
                    {
                        RedirectPath = "../AdminHome/Login";
                    }
                    else if (Convert.ToString(drs["SectionName"]) == "Admin Area" && Convert.ToString(drs["SectionPath"]) == "Admin/ToolsHome.aspx")
                    {
                        RedirectPath = "../AdminHome/AdminArea";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "2874")
                    {
                        RedirectPath = "../Profiles/Users";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "4711")
                    {
                        RedirectPath = "../Technicalarea/Emailcontent";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "4712")
                    {
                        RedirectPath = "../Technicalarea/Emailoption";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "4712")
                    {
                        RedirectPath = "../Technicalarea/Emailoption";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "11848")
                    {
                        RedirectPath = "../Technicalarea/MainEmailoption";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "3202")
                    {
                        RedirectPath = "../Technicalarea/Errors";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "12980")
                    {
                        RedirectPath = "../Technicalarea/Eventlog";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "3259")
                    {
                        RedirectPath = "../Technicalarea/Feedback";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "4757")
                    {
                        RedirectPath = "../Technicalarea/Loginhistorydetails";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "3203")
                    {
                        RedirectPath = "../Technicalarea/Pagenos";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "9545")
                    {
                        RedirectPath = "../Technicalarea/Sitepage";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "4596")
                    {
                        RedirectPath = "../Technicalarea/Sitepages";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "13325")
                    {
                        RedirectPath = "../AdminHome/ArticlesVideos";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "17437")
                    {
                        RedirectPath = "../Technicalarea/Sitevisitinghistory";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "11210")
                    {
                        RedirectPath = "../Forums/ForumsList";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Tools/newsignups.aspx")
                    {
                        RedirectPath = "../signups/NewSignups";
                    }
                    //else if (drs["SectionsInRole_ID"].ToString() == "13325")
                    //{
                    //    RedirectPath = "../AdminHome/ArticlesVideos?sectionid=" + drs["Section_ID"].ToString();
                    //}
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/Master_Add_Or_UpdateWebConfig_Parameters.aspx")
                    {
                        RedirectPath = "../Technicalarea/Webconfigsettings";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Billing/AdminBillingSummary.aspx")
                    {
                        RedirectPath = "../AdminHome/AdminBillingSummary";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "CREDITCARD/PatientCreditCardResponse.aspx")
                    {
                        RedirectPath = "../CREDITCARD/CCTransactionResponse";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Creditcard/PatientCreditCardDetails.aspx")
                    {
                        RedirectPath = "../CREDITCARD/CCTransactionsLedger";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/AdminDetails.aspx")
                    {
                        RedirectPath = "../Adminprofile/Admindetails";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/FTServicesDes.aspx")
                    {
                        RedirectPath = "../AdminHome/AdminBillingSummary";
                    }

                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/ExpenseLedger.aspx")
                    {
                        RedirectPath = "../OfficeExpenses/ExpenseLedger";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/ToolsHome.aspx")
                    {
                        RedirectPath = "../AdminFaqs/Help";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/MailStatus.aspx")
                    {
                        RedirectPath = "../Admin/MailStatus";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Tools/Electricianservices.aspx")
                    {
                        RedirectPath = "../Services/AdminServices";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Home/Home.aspx")
                    {
                        RedirectPath = "../Clients/ClientsHome";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/Tasklist.aspx")
                    {
                        RedirectPath = "../Task/TaskList";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Patients/ProfileNotes.aspx")
                    {
                        RedirectPath = "../Notes/ProfileNotes";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/Emaillog.aspx")
                    {
                        RedirectPath = "../Technicalarea/Emaillog";
                    }
                    else
                    {
                        RedirectPath = Convert.ToString(drs["SectionPath"]);
                    }
                    if (i % 4 == 0)
                    {
                        strAdmin = strAdmin.Append("</tr><tr><td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'> <a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a> <br /> <span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span></td>  ");
                    }
                    else
                    {
                        strAdmin = strAdmin.Append("<td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'><a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a><br /><span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span> </td> ");
                    }
                    i++;
                }
                strAdmin = strAdmin.Append("</tr>");
            }
            strAdmin = strAdmin.Append("</table>");
            ViewBag.adminHome = Convert.ToString(strAdmin);
            return View();
        }

        //public ActionResult HomePatients()
        //{
        //    return View();
        //}
        [ActionName("AdminArea")]
        public ActionResult ToolsHome(string sectionid)
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            if (Session["RoleID"] != null)
            {
                if (Session["RoleID"].ToString() == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            TempData.Remove("hdnPrid");
            Session.Add("TopIndex", "4");
            char AcPrv = Session["strOutIsPaid"] != null ? Convert.ToChar(Session["strOutIsPaid"]) : 'Y';
            
            dsSections = ChildSections(Convert.ToInt32(Session["userid"]), sectionid != null ? sectionid : "168", Convert.ToInt32(Session["Roleid"]), AcPrv);


            StringBuilder strAdmin = new StringBuilder();
            strAdmin = strAdmin.Append("<table id='Toolstable' border='0' cellpadding='20' cellspacing='0' width='100%' >");
            if (dsSections.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                strAdmin = strAdmin.Append("<tr class='background_color'>");
                string RedirectPath = string.Empty;
                foreach (DataRow drs in dsSections.Tables[0].Rows)
                {
                    if (Convert.ToString(drs["SectionPath"]) == "MessageStation/Msg_Listsettings.aspx")
                    {
                        RedirectPath = "../Messages/MessageStationSettings";
                    }

                    else if (Convert.ToString(drs["SectionPath"]) == "MessageStation/Msg_CategoriesListver2.aspx")
                    {
                        RedirectPath = "../Messages/MessageCategories";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/ListContactInfo.aspx")
                    {
                        RedirectPath = "../Messages/Contactusentries?tabid=1";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/ExceptionReports.aspx")
                    {
                        RedirectPath = "../Adminarea/Exceptionreports";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/ProvideruploadDetails.aspx")
                    {
                        RedirectPath = "../Adminarea/Uploaddetails";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/MobileAccessStats.aspx")
                    {
                        RedirectPath = "../Adminarea/Mobilestats";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/AddressInformation.aspx")
                    {
                        RedirectPath = "../Admin/AddressInformation";
                    }
                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/DBObjects.aspx")
                    {
                        RedirectPath = "../Adminarea/Dbscripts";
                    }
                    else if (Convert.ToString(drs["SectionsInRole_ID"]) == "13368")
                    {
                        RedirectPath = "../Resellers/Resellerslist";
                    }

                    else
                    {
                        RedirectPath = Convert.ToString(drs["SectionPath"]);
                    }
                    if (i % 4 == 0)
                    {
                        strAdmin = strAdmin.Append("</tr><tr><td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'> <a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a> <br /> <span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span></td>  ");
                    }
                    else
                    {
                        strAdmin = strAdmin.Append("<td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'><a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a><br /><span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span> </td> ");
                    }
                    i++;
                }
                strAdmin = strAdmin.Append("</tr>");
            }
            strAdmin = strAdmin.Append("</table>");
            ViewBag.adminUserHome = Convert.ToString(strAdmin);
            return View("ToolsHome");
        }

        public ActionResult AdminBillingSummary()
        {
            ViewBag.Count = 2; 
            Session["CurrentUrl1"] = Request.RawUrl;
            TempData.Remove("Practiceid");
            TempData.Remove("pracname");
           
            TempData["combotext"] = !string.IsNullOrEmpty(Request["hdnname"]) ? Request["hdnname"] : "";
            RPBilling objBill = new RPBilling();

            if (Request["dt_filter"] == "30")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objBill.FromDate = ViewBag.frmdate;
                objBill.ToDate = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }
            else if (Request["dt_filter"] == "All")
            {
                objBill.FromDate = null;
                objBill.ToDate = null;
                ViewBag.frmdate = null;
                ViewBag.todate = null;
                ViewBag.setDt = "All";

            }
            else if (Request["dt_filter"] == "Custom")
            {
                if (!string.IsNullOrEmpty(Request["txt_FromDate"]))
                {
                    ViewBag.frmdate = Request["txt_FromDate"];
                }
                if (!string.IsNullOrEmpty(Request["txt_ToDate"]))
                {
                    ViewBag.todate = Request["txt_ToDate"];
                }
                ViewBag.setDt = "Custom";
                objBill.FromDate = ViewBag.frmdate;
                objBill.ToDate = ViewBag.todate;
            }
            else if (Request["dt_filter"] == "Today")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "Today";
                objBill.FromDate = ViewBag.frmdate;
                objBill.ToDate = ViewBag.todate;
            }
            else if (Request["dt_filter"] == "7")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                objBill.FromDate = ViewBag.frmdate;
                objBill.ToDate = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "7";
            }

            else
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objBill.FromDate = ViewBag.frmdate;
                objBill.ToDate = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }
            if (string.IsNullOrEmpty(Request["sort"]))
            {
                objBill.OrderbyItem = "Electricianname";
                ViewBag.sort = "Electricianname";
            }
            else
            {
                objBill.OrderbyItem = Request["sort"];
                ViewBag.sort = Request["sort"];
            }
            if (string.IsNullOrEmpty(Request["sortdir"]))
            {
                objBill.orderBy = "DESC";
                ViewBag.sortdir = "DESC";
            }
            else
            {
                objBill.orderBy = Request["sortdir"];
                ViewBag.sortdir = Request["sortdir"];
            }
            
            objBill.PageNo = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
            //string pgesize = null;
            if (!string.IsNullOrEmpty(Request["ddlrecords"]))
            {
                ViewBag.Summarypagesize = Request["ddlrecords"];
                objBill.NoOfRecords = Request["ddlrecords"];
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                objBill.NoOfRecords = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Summarypagesize = "10";
                objBill.NoOfRecords = "10";
            }

            string hdnPrid = Request["ComboBoxPr"];
            string hdnname = Request["ComboBoxPr_SelectedText"];
            //string strSearchPrid = Request["hdnPrid"] != null ? Request["hdnPrid"] : null;
            string strSearchPrid = hdnPrid != null ? hdnPrid : null;
            if (!string.IsNullOrEmpty(strSearchPrid))
            {

                //string[] strlength = strSearchPrid.Split('$');
                //TempData["Practiceid"]=strlength[0];
                //TempData["providerid"]=strlength[1];
                //string strid = strlength[4].Trim();
                //objBill.ProviderName = strid;
                TempData["providerid"] = strSearchPrid;
                if (!string.IsNullOrEmpty(hdnname))
                {
                    Session["ComboProvFullName"] = hdnname;
                }
                //string strid = strlength[4].Trim();
                objBill.ProviderName = Convert.ToString(Session["ComboProvFullName"]);
                FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strSearchPrid);
                //FillProvider(strid, 0, strSearchPrid);

            }
            else
            {
                objBill.ProviderName = null;
            }
            dynamic summaryGrid = getAdminSummary(objBill);
            return View(summaryGrid);
        }
        public ActionResult Login()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string obj)
        {
            if (Request["txtUsername"] != null & Request["txtPwd"] != null)
            {
                //var objconfig = new clsWebConfigsettings();
                string UserName = clsWebConfigsettings.GetConfigSettingsValue("SecurityUserName");
                string Pwd = clsWebConfigsettings.GetConfigSettingsValue("SecurityPassword");
                if (UserName != null & Pwd != null)
                {
                    if ((UserName == Request["txtUsername"]) & (Request["txtPwd"] == Pwd))
                    {
                        return RedirectToAction("Tools", "AdminHome", new { sectionid = "333" });
                    }
                    else
                    {
                        return RedirectToAction("Tools", "AdminHome", new { sectionid = "365" });
                    }
                }
                else
                {
                    return RedirectToAction("Tools", "AdminHome", new { sectionid = "365" });
                }
            }
            return View();
        }
        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredinsurances1(args.Text, 0);

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

        protected ComboBoxItemList GetFilteredinsurances1(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = null;
            if (filter != null)
            {
                PracticeName = filter;
                TempData["combotext"] = "";
            }
            else if (Convert.ToString(TempData["combotext"]) != "")
            {
                PracticeName = Convert.ToString(TempData["combotext"]).Substring(0, 2);
            }
            else
            {
                PracticeName = null;
            }
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBoxPr"] = payerlist;

            return payerlist;
              }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "GetFilteredinsurances1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult AdminLevelTransactionsList(int pracId = 0, string pracname = null, string disind = null)
        {
            //if (pracId == 0)
            //{
            //    ViewBag.pracId = pracId;
            //    ViewBag.pracname = pracname;
            //}
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            if (TempData["Practiceid"] != null)
            {
                if (Convert.ToInt32(TempData["Practiceid"]) != pracId || Convert.ToString(TempData["pracname"]) != pracname)
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            TempData.Remove("providerid");
            TempData.Remove("Practiceid");
            TempData.Remove("pracname");
            TempData.Remove("disind");
            if (!string.IsNullOrEmpty(disind))
            {
                ViewBag.disind = disind;
                TempData["disind"] = disind;
            }

            RPBilling objinfo = new RPBilling();
            //if (pracId != 0)
            //{
            //    if (pracId != null)
            //    {
            //        DataSet dsget = new DataSet();
            //        dsget = objinfo.getprovider(pracId);
            //        if (dsget.Tables[0].Rows.Count > 0)
            //        {
            //            TempData["providerid"] = dsget.Tables[0].Rows[0]["Provider_ID"];
            //            TempData["Practiceid"] = pracId;
            //            TempData["pracname"] = pracname;
            //            ViewBag.pracnmae = pracname;
            //            ViewBag.proid = pracId;
            //            ViewBag.proid = Convert.ToString(TempData["providerid"]);
            //        }
            //    }

            //}

            if (Request["Daterange"] == "30")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                TempData["frmdate"] = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objinfo.FromDate = ViewBag.frmdate;
                objinfo.ToDate = strDate;
                ViewBag.todate = strDate;
                TempData["ToDate"] = strDate;
                ViewBag.setDt = "30";
            }
            else if (Request["Daterange"] == "All")
            {
                objinfo.FromDate = null;
                objinfo.ToDate = null;
                ViewBag.frmdate = null;
                ViewBag.todate = null;
                ViewBag.setDt = "All";
                TempData["ToDate"] = null;
                TempData["frmdate"] = null;
            }
            else if (Request["Daterange"] == "Custom")
            {
                if (!string.IsNullOrEmpty(Request["frmdate"]))
                {
                    ViewBag.frmdate = Request["frmdate"];
                    TempData["frmdate"] = Request["frmdate"];
                }
                if (!string.IsNullOrEmpty(Request["todate"]))
                {
                    ViewBag.todate = Request["todate"];
                    TempData["ToDate"] = Request["todate"];
                }
                ViewBag.setDt = "Custom";
                objinfo.FromDate = ViewBag.frmdate;
                objinfo.ToDate = ViewBag.todate;
            }
            else if (Request["Daterange"] == "Today")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = strDate;
                ViewBag.todate = strDate;
                TempData["ToDate"] = strDate;
                TempData["frmdate"] = strDate;
                ViewBag.setDt = "Today";
                objinfo.FromDate = ViewBag.frmdate;
                objinfo.ToDate = ViewBag.todate;
            }
            else
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                objinfo.FromDate = ViewBag.frmdate;
                TempData["frmdate"] = objinfo.FromDate;
                TempData["ToDate"] = strDate;
                objinfo.ToDate = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "7";
            }
            if (string.IsNullOrEmpty(Request["sort"]))
            {
                objinfo.OrderbyItem = "Transaction_Date";
                ViewBag.sort = "Transaction_Date";
            }
            else
            {
                objinfo.OrderbyItem = Request["sort"];
                ViewBag.sort = Request["sort"];
            }
            if (string.IsNullOrEmpty(Request["sortdir"]))
            {
                objinfo.orderBy = "DESC";
                ViewBag.sortdir = "DESC";
            }
            else
            {
                objinfo.orderBy = Request["sortdir"];
                ViewBag.sortdir = Request["sortdir"];
            }
            
            objinfo.PageNo = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
            
            if (!string.IsNullOrEmpty(Request["hdnpagesize"]))
            {
                ViewBag.Summarypagesize = Request["hdnpagesize"];
                TempData["Summarypagesize"] = Request["hdnpagesize"];
                objinfo.NoOfRecords = Request["hdnpagesize"];
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                TempData["Summarypagesize"] = Session["Rowsperpage"].ToString();
                objinfo.NoOfRecords = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Summarypagesize = "10";
                TempData["Summarypagesize"] = "10";
                objinfo.NoOfRecords = "10";
            }

            if (pracId != 0)
            {
                string strSearchPrid = !string.IsNullOrEmpty(Convert.ToString(pracId)) ? Convert.ToString(pracId) : null;
                if (!string.IsNullOrEmpty(strSearchPrid))
                {
                    //string[] strlength = strSearchPrid.Split('$');
                    //TempData["Practiceid"] = strlength[0];
                    TempData["Practiceid"] = pracId;
                    //TempData["pracname"] = pracname;
                    //ViewBag.pracnmae = pracname;
                    ViewBag.proid = pracId;
                    TempData["providerid"] = strSearchPrid;
                    if (!string.IsNullOrEmpty(pracname))
                    {
                        Session["ComboProvFullName"] = pracname;
                    }
                    //string strid = strlength[4].Trim();
                    objinfo.ProviderName = Convert.ToString(Session["ComboProvFullName"]);
                    TempData["pracname"] = Convert.ToString(Session["ComboProvFullName"]);
                    ViewBag.pracnmae = Convert.ToString(Session["ComboProvFullName"]);
                    FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strSearchPrid);
                    //string strid = strlength[4].Trim();
                    //objinfo.ProviderName = strid;
                    //TempData["pracname"] = strid;
                    //ViewBag.pracnmae = strid;
                    //if (!string.IsNullOrEmpty(disind))
                    //{
                    //    ViewBag.disind = disind;
                    //    TempData["disind"] = disind;
                    //}
                    //FillProvider(strid, 0,strSearchPrid);

                }
            }
            else
            {
                objinfo.ProviderName = null;
            }
            if (!string.IsNullOrEmpty(pracname) && disind == "N")
            {
                FillProvider(pracname, 0, null);
            }
            objinfo.TransactionTypeIDs = "1";
            objinfo.ReferenceType_ID = "1";
            objinfo.fromReferenceId = 1;
            //objinfo.Facility_ID = null;
            //objinfo.ToReferenceType_ID = null;
            //objinfo.ToReference_ID = null;
            //objinfo.ServiceID = null;
            objinfo.Provider_id = Convert.ToInt32(TempData["providerid"]);
            if (TempData["practiceid"] != null)
            {
                objinfo.Practice_ID = Convert.ToInt32(TempData["practiceid"]);

            }

            TempData.Keep("Practiceid");
            TempData.Keep("providerid");
            TempData.Keep("pracname");
            ViewBag.pracId = Request["pracId"];
            DataSet dsinfo = new DataSet();
            dsinfo = objinfo.PracticeAdminSummary(objinfo);
            StringBuilder strbill = new StringBuilder();
            strbill = strbill.Append("<table width='100%' cellpadding='6' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            strbill = strbill.Append("<td align='center'><strong>Total charges</strong></td><td align='center'><strong>Total payments</strong></td><td align='center'><strong>Total credits</strong></td><td align='center'><strong>Balance</strong></td></tr>");
            foreach (DataRow dr in dsinfo.Tables[0].Rows)
            {
                //string StrTCharge = string.Format("{0:c}", dr["TotalCharges"]);
                //string strTPay = string.Format("{0:c}", dr["TotalPayments"]);
                //string strBal = string.Format("{0:c}", dr["Balance"]);
                //string strCredit = string.Format("{0:c}", dr["TotalCredits"]);
                strbill = strbill.Append("<tr><td align='center'>" + string.Format("{0:c}", dr["TotalCharges"]) + " </td><td align='center'>" + string.Format("{0:c}", dr["TotalPayments"]) + "</td><td align='center'>" + string.Format("{0:c}", dr["TotalCredits"]) + "</td><td align='center'>" + string.Format("{0:c}", dr["Balance"]) + "</td></tr>");
            }
            strbill = strbill.Append("</table>");
            ViewBag.ProvBillsum = Convert.ToString(strbill);
            dynamic transgrid = getPracticeTransactions(objinfo);
            return View(transgrid);
        }
        public ActionResult AddPracticeCharge()
        {

            ViewBag.pracname = TempData["pracname"];
            TempData.Keep("pracname");
            return View();

        }
        [HttpPost]
        public ActionResult AddPracticeCharge(RPBilling AdminNewCharge)
        {
            int tranId = 0;
            AdminNewCharge.fromReferenceId = 1;
            AdminNewCharge.ReferenceType_ID = "1";
            AdminNewCharge.ToReferenceType_ID = "2";
            AdminNewCharge.ToReference_ID = Convert.ToString(TempData["providerid"]).Trim();
            TempData.Keep("providerid");
            //AdminNewCharge.BillingService_ID = null;
            AdminNewCharge.BillingChargetype_ID = "3";
            AdminNewCharge.Transaction_Date = Request["txtDate"];
            AdminNewCharge.Transaction_Amount = Request["txtAmount"];
            AdminNewCharge.Notes = Request["txtNotes"];
            //if (TempData["Practiceid"] != null)
            //{
            //    AdminNewCharge.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            //}

            TempData.Keep("Practiceid");
            //AdminNewCharge.Facility_ID = null;
            //AdminNewCharge.ServiceTaxAmount = null;
            //AdminNewCharge.ServiceTaxPercentage = null;
            AdminNewCharge.UserId = Convert.ToString(Session["UserID"]);
            tranId = AdminNewCharge.MakeAdminChgTransactionlist(AdminNewCharge, Convert.ToInt32(Session["Loginhistory_ID"]));
            return RedirectToAction("AdminLevelTransactionsList", new { pracId = TempData["Practiceid"], pracname = TempData["pracname"] });

        }
        public ActionResult AddPracticePayment()
        {

            ViewBag.pracname = TempData["pracname"];
            TempData.Keep("pracname");

            //return View();
            var cardstype = clsCommonCClist.GetCCList();

            var month = clsCommonCClist.GetCCMonth();

            var year = clsCommonCClist.GetCCYear();

            //var reg1 = clsCommonCClist.GetStates();


            ViewData["Exmonth"] = month;
            ViewData["Exyear"] = year;
            ViewData["ExCardType"] = cardstype;
            //ViewBag.Practice_ID = Session["Practice_ID"].ToString();
            //ViewBag.Provider_ID = Session["Prov_ID"].ToString();
            CCProcess objcc = new CCProcess();
            objcc.ReferenceTypeID = "2";
            objcc.ReferenceID = Request["pracId"];
            string id = CCProcess.LoadCreditCardInfo(objcc);
            //string vaultid = CCProcess.GetVaultID(id);
            //Filldata(id, vaultid, Request["pracId"]);
            FillProviderAddress(Request["pracId"]);

            return View("AddPracticePayment", Filldata(id, CCProcess.GetVaultID(id), Request["pracId"]));

        }
        private List<CCProcess> Filldata(string id, string valutid, string provid)
        {
            try
            {
                ViewBag.CardID = id;
                ViewBag.vaultid = valutid;

                var obj = new CCProcess { CardID = id, Provider_ID = provid };
                List<CCProcess> objlist = CCProcess.CreditCard_Get_paymentinfo(obj);
                if (string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"])))
                {
                    APIContext apiContext = Configuration.GetAPIContext();
                    //try
                    //{
                    CreditCard getcardinfo = CreditCard.Get(apiContext, valutid);

                    if (getcardinfo != null)
                    {
                        ViewBag.cardnumber = getcardinfo.number;
                        ViewBag.cvv2 = getcardinfo.cvv2;
                        ViewBag.FirstName = getcardinfo.first_name;
                        ViewBag.LastName = getcardinfo.last_name;
                    }
                    //}
                    //catch (Exception)
                    //{


                    //}

                }
                else
                {
                    //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                    var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                    var card = gateway.Get(new GetStripeCard
                    {
                        CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                        CardId = valutid,
                    });
                    ViewBag.cardnumber = "************" + card.Last4;
                    ViewBag.cvv2 = card.Cvc;
                    ViewBag.FirstName = Session["Stripe_Firstname"];
                    ViewBag.LastName = Session["Stripe_lastname"];
                }
                if (objlist.Count > 0)
                {
                   
                    //ViewBag.CardType = objlist[0].strx_card_code ?? null;
                    //if (ViewBag.CardType == "VisaCard")
                    //{
                    //    ViewBag.CardType = "Visa";
                    //}
                    //ViewBag.ExipryMonth = objlist[0].StrExpMon ?? null;
                    //ViewBag.ExipryYear = objlist[0].StrExpYear ?? null;
                    //ViewBag.Address1 = objlist[0].strBillAddress1 ?? null;
                    //ViewBag.Address2 = objlist[0].strBillAddress2 ?? null;
                    //ViewBag.Zip = objlist[0].strZipCode ?? null;
                    //ViewBag.State_ID = objlist[0].strStateID ?? null;
                    //ViewBag.City_ID = objlist[0].strCityID ?? null;
                    //ViewBag.Statename = objlist[0].state ?? null;
                    //ViewBag.Cityname = objlist[0].city ?? null;
                    //ViewBag.practice_ind = objlist[0].practice_ind ?? null;
                    //ViewBag.FirstName = objlist[0].FirstName ?? null;
                    //ViewBag.LastName = objlist[0].LastName ?? null;
                }
                return objlist;
            }
            catch (Exception ex)
            {
                var log = new clsExceptionLog();
                log.LogException(ex, "AdminController", "Filldata", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                Session.Add("IsPopUp", "true");
                //ViewBag.ExipryMonth = null;
                //ViewBag.ExipryYear = null;
                //ViewBag.Address1 = null;
                //ViewBag.Address2 = null;
                //ViewBag.Zip = null;
                //ViewBag.State_ID = null;
                //ViewBag.City_ID = null;
                //ViewBag.Statename = null;
                //ViewBag.Cityname = null;
                //ViewBag.practice_ind = null;
                return null;
            }
        }
        private void FillProviderAddress(string provid)
        {
            try
            {
            var objcomm = new clsCommonFunctions();
            Reg_ProvidersDetailInfo objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(provid));
            if ((objInfo != null))
            {

                ViewBag.PAddress1 = !string.IsNullOrEmpty(objInfo.Address1) ? objInfo.Address1 : null;
                ViewBag.PAddress2 = !string.IsNullOrEmpty(objInfo.Address2) ? objInfo.Address2 : null;
                ViewBag.PZip = !string.IsNullOrEmpty(objInfo.Zip) ? objInfo.Zip : null;
                ViewBag.PFirstName = !string.IsNullOrEmpty(objInfo.FirstName) ? objInfo.FirstName : null;
                ViewBag.PLastName = !string.IsNullOrEmpty(objInfo.LastName) ? objInfo.LastName : null;

            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "FillProviderAddress", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
            }
        }
        [HttpPost]
        public JsonResult AddPracticePayment(RPBilling AdminNewPay)
        {
            int? Out_CCID = 0;
            int tranId = 0;
            AdminNewPay.fromReferenceId = 1;
            AdminNewPay.ReferenceType_ID = "1";
            AdminNewPay.ToReferenceType_ID = "2";
            AdminNewPay.ToReference_ID = Convert.ToString(TempData["providerid"]).Trim();
            TempData.Keep("providerid");
            //AdminNewPay.BillingService_ID = null;
            //AdminNewPay.BillingChargetype_ID = null;
            AdminNewPay.Transaction_Date = Request["txtpayDate"];
            AdminNewPay.Transaction_Amount = Request["txtPayAmount"];
            AdminNewPay.Notes = Request["txtPayNotes"];
            //AdminNewPay.CCTransactionID = null;
            AdminNewPay.PaymentType_ID = Convert.ToInt32(Request["ddlPayment"]);
            //if (TempData["Practiceid"] != null)
            //{
            //    AdminNewPay.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            //}

            TempData.Keep("Practiceid");
            TempData.Keep("pracname");
            //AdminNewPay.Facility_ID = null;
            AdminNewPay.UserId = Convert.ToString(Session["UserID"]);
            if (Request["ddlPayment"] != "2")
            {
                tranId = AdminNewPay.MakeAdminPayTransaction(AdminNewPay, Convert.ToInt32(Session["Loginhistory_ID"]));
                if (tranId != 0)
                {
                    if (Request["ddlPayment"] == "3")
                    {
                        //AdminNewPay.ChecksNo = null;
                        AdminNewPay.Transaction_ID = tranId;
                        AdminNewPay.UserId = Convert.ToString(Session["UserID"]);
                        AdminNewPay.Transaction_Amount = Request["txtPayAmount"];
                        AdminNewPay.INS_AdminChequeTransaction(AdminNewPay);
                    }
                }
            }
            if (Request["ddlPayment"] == "2")
            {
                CreditCard crdtCard = new CreditCard();

                if (!string.IsNullOrEmpty(Request["ExCardType"]))
                {
                    switch (Request["ExCardType"])
                    {
                        case "1": crdtCard.type = "mastercard";
                            break;
                        case "2": crdtCard.type = "visa";
                            break;
                        case "3": crdtCard.type = "amex";
                            break;
                        case "4": crdtCard.type = "discover";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(Request["ExCardNumber"]))
                {
                    crdtCard.number = Request["ExCardNumber"];
                }
                
                crdtCard.cvv2 = !string.IsNullOrEmpty(Request["ExtxtCVV2"]) ? Request["ExtxtCVV2"] : null;
                
                crdtCard.first_name = !string.IsNullOrEmpty(Request["Extxtfirstname"]) ? Request["Extxtfirstname"] : null;
                
                crdtCard.last_name = !string.IsNullOrEmpty(Request["Extxtlastname"]) ? Request["Extxtlastname"] : null;
                if (!string.IsNullOrEmpty(Request["Exmonth"]))
                {
                    crdtCard.expire_month = Convert.ToInt32(Request["Exmonth"]);
                }

                if (!string.IsNullOrEmpty(Request["Exyear"]))
                {
                    crdtCard.expire_year = Convert.ToInt32(Request["Exyear"]);
                }
                if (!string.IsNullOrEmpty(Request["ExHdnProvider_ID"]))
                {
                    crdtCard.payer_id = Convert.ToString(TempData["providerid"]).Trim();
                }

                Address billingAddress = new Address {
                    country_code = "US",
                    line1 = !string.IsNullOrEmpty(Request["Exhdnadd1"]) ? Request["Exhdnadd1"] : null,
                    line2 = !string.IsNullOrEmpty(Request["Exhdnadd2"]) ? Request["Exhdnadd2"] : null,
                    postal_code = !string.IsNullOrEmpty(Request["Exhdnzip"]) ? Request["Exhdnzip"] : null,
                    state = !string.IsNullOrEmpty(Request["Exhdnstate"]) ? clsCommonCClist.Getstatename(Request["Exhdnstate"]) : null,
                    city = !string.IsNullOrEmpty(Request["Exhdncity"]) ? clsCommonCClist.Getcityname(Request["Exhdncity"]) : null
                };
                
                crdtCard.billing_address = billingAddress;
                Amount amnt = new Amount();
                amnt.currency = "USD";
                if (!string.IsNullOrEmpty(Request["Exhdmamount"]))
                {
                    amnt.total = Request["Exhdmamount"];
                }

                Transaction tran = new Transaction();
                tran.amount = amnt;
                tran.description = "SmbHelpGroup.";

                List<Transaction> transactions = new List<Transaction>();
                transactions.Add(tran);
                FundingInstrument fundInstrument = new FundingInstrument();
                fundInstrument.credit_card = crdtCard;
                List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
                fundingInstrumentList.Add(fundInstrument);
                Payer payr = new Payer();
                payr.funding_instruments = fundingInstrumentList;
                payr.payment_method = "credit_card";
                Payment pymnt = new Payment();
                pymnt.intent = "sale";
                pymnt.payer = payr;
                pymnt.transactions = transactions;
                try
                {
                    //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                    if (clsWebConfigsettings.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                    {
                        APIContext apiContext = Configuration.GetAPIContext();
                        Payment createdPayment = pymnt.Create(apiContext);
                        JsonResult objser = new JsonResult();
                        string objrequest = JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented);
                        if (createdPayment != null & objrequest != "" & createdPayment.state == "approved")
                        {
                            CCProcess objinsert = new CCProcess();
                            objinsert.StrPaidRefID = "1";
                            objinsert.StrPaidRefTypeID = "1";
                            objinsert.StrChargebleRefID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrChargebleRefTypeID = "2";
                            objinsert.strpost = objrequest;
                            objinsert.RefLoginID = 0;
                            objinsert.strPracticeID = Convert.ToString(TempData["Practiceid"]);
                            objinsert.CVV2 = null;
                            objinsert.strx_card_code = crdtCard.type;
                            objinsert.strStrCardType = null;
                            objinsert.strStateID = Request["Exhdnstate"];
                            objinsert.strZipCode = Request["Exhdnzip"];
                            objinsert.strCityID = Request["Exhdncity"];
                            objinsert.strBillAddress1 = billingAddress.line1;
                            objinsert.strBillAddress2 = billingAddress.line2;
                            objinsert.FirstName = Request["Extxtfirstname"];
                            objinsert.LastName = Request["Extxtlastname"];
                            objinsert.strx_invoice_num = null;
                            objinsert.strx_card_num = null;
                            objinsert.IsPaypalInd = "Y";
                            objinsert.Provider_ID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrExpMon = crdtCard.expire_month;
                            objinsert.StrExpYear = crdtCard.expire_year;
                            objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                            objinsert.paypaltransactionid = createdPayment.id;

                            //string Mycardno = Convert.ToString(Request["ExCardNumber"]);
                            objinsert.LastFourdigitCCNo = Request["ExCardNumber"].Substring(Request["ExCardNumber"].Length - Math.Min(4, Request["ExCardNumber"].Length));
                            List<Transaction> objlist = new List<Transaction>();
                            List<RelatedResources> objfund = new List<RelatedResources>();
                            Sale objsale = new Sale();
                            objlist = createdPayment.transactions;
                            objfund = objlist[0].related_resources;
                            objsale = objfund[0].sale;
                            objinsert.paypalsaletransactionid = objsale.id;
                            int Loc_transid;
                            clsCommonFunctions objcommon = new clsCommonFunctions();
                            objinsert.CreatedBy = objcommon.GetProviderLoginID(Convert.ToString(TempData["providerid"]).Trim());
                            objinsert.ALLowCCCharges = "Y";
                            if (!string.IsNullOrEmpty(Request["Echkaddress"]))
                            {
                                objinsert.practice_ind = Request["Echkaddress"] == "true,false" ? "Y" : "N";
                            }


                            Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                           // CreditCard strcard = new CreditCard();
                            //try
                            //{
                          //  strcard = crdtCard.Create(apiContext);
                            //}
                            //catch (Exception)
                            //{


                            //}

                            objstorecreditcardresponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);
                            //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(TempData["providerid"]));

                            CCProcess objccins = new CCProcess();
                            objccins.strTransactionID = Loc_transid;
                            objccins.StrRespStatusCode = createdPayment.state;
                            objccins.strRetval = objstorecreditcardresponse;// Encrypt(objstorecreditcardresponse);
                            objccins.ResponseCode = null;
                            objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(TempData["providerid"]));
                            objccins.GatewayDetail_ID = null;
                            objccins.PaypalProcessInd = "Y";
                            Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                            if (createdPayment.state == "approved")
                            {
                                AdminNewPay.CCTransactionID = Loc_transid;
                                tranId = AdminNewPay.MakeAdminPayTransaction(AdminNewPay, Convert.ToInt32(Session["Loginhistory_ID"]));

                                //if (Request["ExhdnCardid"] != null)
                                //{
                                //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                                //}
                                CreditCard newCreditCard = new CreditCard();
                                //try
                                //{
                                newCreditCard = crdtCard.Create(apiContext);
                                //}
                                //catch (Exception)
                                //{


                                //}

                                //if (newCreditCard.state == "ok")
                                //{
                                MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                                //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                                CCProcess.UpdateCreditCardVaultID(Convert.ToString(TempData["providerid"]).Trim(), newCreditCard.id, Out_CCID);
                                // }


                                var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(TempData["Practiceid"]));

                                if ((obgCredn != null))
                                {
                                    ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                                    string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                                    string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                                    string strdate = DateTime.Now.ToShortDateString();

                                    SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total);
                                }

                                AdminNewPay.Provider_id = Convert.ToInt32(TempData["Practiceid"]);
                                AdminNewPay.ProviderName = Convert.ToString(TempData["pracname"]);
                                return Json(JsonResponseFactory.SuccessResponse(AdminNewPay), JsonRequestBehavior.DenyGet);
                            }
                            else
                            {
                                return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on Process"), JsonRequestBehavior.DenyGet);
                            }
                        }
                        else
                        {
                            return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                        }
                    }




                    else
                    {

                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"])))
                        {
                            double strip_amount = Convert.ToDouble(amnt.total) * 100;
                            //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                            var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                            var card = gateway.Post(new CreateStripeCard
                            {
                                CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                                Card = new StripeCard
                                {
                                    Name = Request["Extxtfirstname"] + Request["Extxtlastname"],
                                    Number = Request["ExCardNumber"],
                                    Cvc = Request["ExtxtCVV2"] != "" ? Request["ExtxtCVV2"] : null,
                                    ExpMonth = Convert.ToInt32(Request["Exmonth"]),
                                    ExpYear = Convert.ToInt32(Request["Exyear"]),
                                    AddressLine1 = Request["Exhdnadd1"],
                                    AddressLine2 = Request["Exhdnadd2"],
                                    AddressZip = Request["Exhdnzip"],
                                    AddressState = Request["Exhdnstate"],
                                    AddressCountry = "US",
                                },
                            });


                            var charge = gateway.Post(new ChargeStripeCustomer
                            {
                                Amount = Convert.ToInt32(strip_amount),
                                Customer = Convert.ToString(Session["Stripe_customerid"]),
                                Currency = "usd",
                                Description = "Test Charge Customer",
                                Card = card.Id
                            });

                            //APIContext apiContext = Configuration.GetAPIContext();
                            //Payment createdPayment = pymnt.Create(apiContext);
                            JsonResult objser = new JsonResult();
                            //string objrequest = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);
                            //if (createdPayment != null & objrequest != "")
                            //{
                            CCProcess objinsert = new CCProcess();
                            objinsert.StrPaidRefID = "1";
                            objinsert.StrPaidRefTypeID = "1";
                            objinsert.StrChargebleRefID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrChargebleRefTypeID = "2";
                            //objinsert.strpost = objrequest;
                            objinsert.strpost = null;
                            objinsert.RefLoginID = 0;
                            objinsert.strPracticeID = Convert.ToString(TempData["Practiceid"]);
                            objinsert.CVV2 = null;
                            objinsert.strx_card_code = crdtCard.type;
                            objinsert.strStrCardType = null;
                            objinsert.strStateID = Request["Exhdnstate"];
                            objinsert.strZipCode = Request["Exhdnzip"];
                            objinsert.strCityID = Request["Exhdncity"];
                            objinsert.strBillAddress1 = billingAddress.line1;
                            objinsert.strBillAddress2 = billingAddress.line2;
                            objinsert.FirstName = Request["Extxtfirstname"];
                            objinsert.LastName = Request["Extxtlastname"];
                            objinsert.strx_invoice_num = null;
                            objinsert.strx_card_num = null;
                            objinsert.IsPaypalInd = "N";
                            objinsert.Provider_ID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrExpMon = crdtCard.expire_month;
                            objinsert.StrExpYear = crdtCard.expire_year;
                            objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                            //objinsert.paypaltransactionid = createdPayment.id;
                            objinsert.paypaltransactionid = charge.Id;

                            //string Mycardno = Convert.ToString(Request["ExCardNumber"]);
                            objinsert.LastFourdigitCCNo = Request["ExCardNumber"].Substring(Request["ExCardNumber"].Length - Math.Min(4, Request["ExCardNumber"].Length));
                            //List<Transaction> objlist = new List<Transaction>();
                            //List<RelatedResources> objfund = new List<RelatedResources>();
                            //Sale objsale = new Sale();
                            //objlist = createdPayment.transactions;
                            //objfund = objlist[0].related_resources;
                            //objsale = objfund[0].sale;
                            //objinsert.paypalsaletransactionid = objsale.id;
                            objinsert.paypalsaletransactionid = null;
                            int Loc_transid;
                            clsCommonFunctions objcommon = new clsCommonFunctions();
                            objinsert.CreatedBy = objcommon.GetProviderLoginID(Convert.ToString(TempData["providerid"]).Trim());
                            objinsert.ALLowCCCharges = "Y";
                            if (!string.IsNullOrEmpty(Request["Echkaddress"]))
                            {
                                objinsert.practice_ind = Request["Echkaddress"] == "true,false" ? "Y" : "N";
                            }


                            Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                            //CreditCard strcard = crdtCard.Create(apiContext);
                            //objstorecreditcardresponse = JObject.Parse(strcard.ConvertToJson()).ToString(Formatting.Indented);
                            //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32((TempData["providerid"])));

                            CCProcess objccins = new CCProcess();
                            objccins.strTransactionID = Loc_transid;
                            //objccins.StrRespStatusCode = createdPayment.state;
                            objccins.StrRespStatusCode = "approved";
                            //objccins.strRetval = Encrypt(objstorecreditcardresponse);
                            objccins.strRetval = null;
                            objccins.ResponseCode = null;
                            objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(TempData["providerid"]));
                            objccins.GatewayDetail_ID = null;
                            objccins.PaypalProcessInd = "N";
                            Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                            //if (createdPayment.state == "approved")
                            //{
                            AdminNewPay.CCTransactionID = Loc_transid;
                            tranId = AdminNewPay.MakeAdminPayTransaction(AdminNewPay, Convert.ToInt32(Session["Loginhistory_ID"]));

                            //if (Request["ExhdnCardid"] != null)
                            //{
                            //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                            //}
                            //CreditCard newCreditCard = crdtCard.Create(apiContext);
                            //if (newCreditCard.state == "ok")
                            //{
                            MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                            //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                            //CCProcess.UpdateCreditCardVaultID(Convert.ToString(TempData["providerid"]).Trim(), newCreditCard.id);
                            CCProcess.UpdateCreditCardVaultID(Convert.ToString(TempData["providerid"]).Trim(), card.Id, Out_CCID, Convert.ToString(Session["Stripe_customerid"]));
                            //}


                            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(TempData["providerid"]));

                            if ((obgCredn != null))
                            {
                                ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                                string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                                string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                                string strdate = DateTime.Now.ToShortDateString();

                                SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total);
                            }

                            AdminNewPay.Provider_id = Convert.ToInt32(TempData["Practiceid"]);
                            AdminNewPay.ProviderName = Convert.ToString(TempData["pracname"]);
                            return Json(JsonResponseFactory.SuccessResponse(AdminNewPay), JsonRequestBehavior.DenyGet);
                        }

                        else
                        {
                            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(TempData["providerid"]));
                            string customer_mail = null;
                            if ((obgCredn != null))
                            {
                                customer_mail = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                            }
                            double strip_amount = Convert.ToDouble(amnt.total) * 100;
                            //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                            var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                            var customer = gateway.Post(new CreateStripeCustomer
                            {
                                //AccountBalance = 10000,
                                //Card = new StripeCard
                                //{
                                //    Name = "Test Card",
                                //    Number = "4242424242424242",
                                //    Cvc = "123",
                                //    ExpMonth = 1,
                                //    ExpYear = 2015,
                                //    AddressLine1 = "1 Address Road",
                                //    AddressLine2 = "12345",
                                //    AddressZip = "City",
                                //    AddressState = "NY",
                                //    AddressCountry = "US",
                                //},
                                Description = "Description",
                                Email = customer_mail,
                            });
                            var card = gateway.Post(new CreateStripeCard
                            {
                                CustomerId = customer.Id,
                                Card = new StripeCard
                                {
                                    Name = Request["Extxtfirstname"] + Request["Extxtlastname"],
                                    Number = Request["ExCardNumber"],
                                    Cvc = Request["ExtxtCVV2"] != "" ? Request["ExtxtCVV2"] : null,
                                    ExpMonth = Convert.ToInt32(Request["Exmonth"]),
                                    ExpYear = Convert.ToInt32(Request["Exyear"]),
                                    AddressLine1 = Request["Exhdnadd1"],
                                    AddressLine2 = Request["Exhdnadd2"],
                                    AddressZip = Request["Exhdnzip"],
                                    AddressState = Request["Exhdnstate"],
                                    AddressCountry = "US",
                                },
                            });
                            var charge = gateway.Post(new ChargeStripeCustomer
                            {
                                Amount = Convert.ToInt32(strip_amount),
                                Customer = customer.Id,
                                Currency = "usd",
                                Description = "Test Charge Customer",
                                Card = card.Id
                            });

                            //APIContext apiContext = Configuration.GetAPIContext();
                            //Payment createdPayment = pymnt.Create(apiContext);
                            JsonResult objser = new JsonResult();
                            //string objrequest = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);
                            //if (createdPayment != null & objrequest != "")
                            //{
                            CCProcess objinsert = new CCProcess();
                            objinsert.StrPaidRefID = "1";
                            objinsert.StrPaidRefTypeID = "1";
                            objinsert.StrChargebleRefID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrChargebleRefTypeID = "2";
                            //objinsert.strpost = objrequest;
                            objinsert.strpost = null;
                            objinsert.RefLoginID = 0;
                            objinsert.strPracticeID = Convert.ToString(TempData["Practiceid"]);
                            objinsert.CVV2 = null;
                            objinsert.strx_card_code = crdtCard.type;
                            objinsert.strStrCardType = null;
                            objinsert.strStateID = Request["Exhdnstate"];
                            objinsert.strZipCode = Request["Exhdnzip"];
                            objinsert.strCityID = Request["Exhdncity"];
                            objinsert.strBillAddress1 = billingAddress.line1;
                            objinsert.strBillAddress2 = billingAddress.line2;
                            objinsert.FirstName = Request["Extxtfirstname"];
                            objinsert.LastName = Request["Extxtlastname"];
                            objinsert.strx_invoice_num = null;
                            objinsert.strx_card_num = null;
                            objinsert.IsPaypalInd = "N";
                            objinsert.Provider_ID = Convert.ToString(TempData["providerid"]).Trim();
                            objinsert.StrExpMon = crdtCard.expire_month;
                            objinsert.StrExpYear = crdtCard.expire_year;
                            objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                            //objinsert.paypaltransactionid = createdPayment.id;
                            objinsert.paypaltransactionid = charge.Id;

                            //string Mycardno = Convert.ToString(Request["ExCardNumber"]);
                            objinsert.LastFourdigitCCNo = Request["ExCardNumber"].Substring(Request["ExCardNumber"].Length - Math.Min(4, Request["ExCardNumber"].Length));
                            //List<Transaction> objlist = new List<Transaction>();
                            //List<RelatedResources> objfund = new List<RelatedResources>();
                            //Sale objsale = new Sale();
                            //objlist = createdPayment.transactions;
                            //objfund = objlist[0].related_resources;
                            //objsale = objfund[0].sale;
                            //objinsert.paypalsaletransactionid = objsale.id;
                            objinsert.paypalsaletransactionid = null;
                            int Loc_transid;
                            clsCommonFunctions objcommon = new clsCommonFunctions();
                            objinsert.CreatedBy = objcommon.GetProviderLoginID(Convert.ToString(TempData["providerid"]).Trim());
                            objinsert.ALLowCCCharges = "Y";
                            if (!string.IsNullOrEmpty(Request["Echkaddress"]))
                            {
                                objinsert.practice_ind = Request["Echkaddress"] == "true,false" ? "Y" : "N";
                            }


                            Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                            //CreditCard strcard = crdtCard.Create(apiContext);
                            //objstorecreditcardresponse = JObject.Parse(strcard.ConvertToJson()).ToString(Formatting.Indented);
                            //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32((TempData["providerid"])));

                            CCProcess objccins = new CCProcess();
                            objccins.strTransactionID = Loc_transid;
                            //objccins.StrRespStatusCode = createdPayment.state;
                            objccins.StrRespStatusCode = "approved";
                            //objccins.strRetval = Encrypt(objstorecreditcardresponse);
                            objccins.strRetval = null;
                            objccins.ResponseCode = null;
                            objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(TempData["providerid"]));
                            objccins.GatewayDetail_ID = null;
                            objccins.PaypalProcessInd = "N";
                            Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                            //if (createdPayment.state == "approved")
                            //{
                            AdminNewPay.CCTransactionID = Loc_transid;
                            tranId = AdminNewPay.MakeAdminPayTransaction(AdminNewPay, Convert.ToInt32(Session["Loginhistory_ID"]));

                            //if (Request["ExhdnCardid"] != null)
                            //{
                            //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                            //}
                            //CreditCard newCreditCard = crdtCard.Create(apiContext);
                            //if (newCreditCard.state == "ok")
                            //{
                            MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                            //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                            //CCProcess.UpdateCreditCardVaultID(Convert.ToString(TempData["providerid"]).Trim(), newCreditCard.id);
                            CCProcess.UpdateCreditCardVaultID(Convert.ToString(TempData["providerid"]).Trim(), card.Id, Out_CCID, customer.Id);
                            //}


                            //var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Request["Practiceid"]));

                            if ((obgCredn != null))
                            {
                                ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                                string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                                string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                                string strdate = DateTime.Now.ToShortDateString();

                                SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total);
                            }

                            AdminNewPay.Provider_id = Convert.ToInt32(TempData["Practiceid"]);
                            AdminNewPay.ProviderName = Convert.ToString(TempData["pracname"]);
                            return Json(JsonResponseFactory.SuccessResponse(AdminNewPay), JsonRequestBehavior.DenyGet);
                        }
                    }
                }

                catch (PayPal.PayPalException ex)
                {
                    var objex = new clsExceptionLog();
                    objex.LogException(ex, "Tools", "ExpiryPaymentMethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                catch (StripeException ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminHomeController", "AddPracticePayment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    return Json(JsonResponseFactory.ErrorResponse(ex.Message), JsonRequestBehavior.DenyGet);
                }
                catch (Exception ex)
                {
                    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                }
            }
            //return RedirectToAction("AdminLevelTransactionsList", new { pracId = TempData["Practiceid"], pracname = TempData["pracname"] });
            AdminNewPay.Provider_id = Convert.ToInt32(TempData["Practiceid"]);
            AdminNewPay.ProviderName = Convert.ToString(TempData["pracname"]);
            return Json(JsonResponseFactory.SuccessResponse(AdminNewPay), JsonRequestBehavior.DenyGet);
        }
        public void SendEmailtoElectrician(string strTomailid, string strTransaction_ID, string strProvidername, string strtransactiondate, string stramoumt)
        {
            //var objconfig = new clsWebConfigsettings();
            try
            {
            if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
            {
                var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(61, 0);
                if ((EMO != null))
                {
                    ViewBag.Subject = EMO.Msg_Subject ?? null;
                    ViewBag.Content = EMO.Msg_Body ?? null;
                }
                string str_Content = ViewBag.Content;
                if (str_Content != null)
                {
                    if (strProvidername != null)
                    {
                        str_Content = str_Content.Replace("$ProviderName$", strProvidername);
                    }
                    str_Content = str_Content.Replace("$CSRName$", "Mower Helper");
                    str_Content = str_Content.Replace("$CCtransactionnumber$", strTransaction_ID);
                    if (strtransactiondate != null)
                    {
                        str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                    }
                    str_Content = str_Content.Replace("$ServiceContent$", "Manual Payments");
                    if (stramoumt != null)
                    {
                        double str_amoumt = Convert.ToDouble(stramoumt);
                        str_Content = str_Content.Replace("$Amount$", string.Format("{0:c}", str_amoumt));
                    }
                    if (strtransactiondate != null)
                    {
                        double str_amoumt = Convert.ToDouble(stramoumt);
                        str_Content = str_Content.Replace("$Amount$", string.Format("{0:c}", str_amoumt));
                    }
                }
                string strFrommailid = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                var objSendMessage = new ClsMailMessage();
                bool strvalid = objSendMessage.SendMail(strTomailid, strFrommailid, ViewBag.Subject, str_Content, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (strvalid == true)
                {
                    clsCommonFunctions objclscommon = new clsCommonFunctions();
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strTransaction_ID) };
                    objclscommon.AddInParameters(strMailParam);
                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }

            }
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "SendEmailtoElectrician", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
            }
        }
        //private string Encrypt(string clearText)
        //{
        //    try
        //    {
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "AdminHomeController", "Encrypt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return clearText;
        //}
        //public string Getstatename(string stateid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_state_id", stateid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.St_get_stateabbrevation");
        //    while (dr_GetName.Read())
        //    {
        //        return dr_GetName["state"]!= null ? dr_GetName["state"].ToString() : null;
        //    }
        //    return null;
        //}
        //public string Getcityname(string cityid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_City_ID", cityid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
        //    while (dr_GetName.Read())
        //    {
        //        return dr_GetName["City"] != null ? dr_GetName["City"].ToString() : null;
        //    }
        //    return null;
        //}

        public ActionResult AddPracticeAdjustment()
        {

            ViewBag.pracname = TempData["pracname"];
            TempData.Keep("pracname");
            return View();

        }
        [HttpPost]
        public ActionResult AddPracticeAdjustment(RPBilling adminNewAdj)
        {
            int tranId = 0;
            adminNewAdj.fromReferenceId = 1;
            adminNewAdj.ReferenceType_ID = "1";
            adminNewAdj.ToReferenceType_ID = "2";
            adminNewAdj.ToReference_ID = Convert.ToString(TempData["providerid"]).Trim();
            TempData.Keep("providerid");
            //adminNewAdj.BillingService_ID = null;
            adminNewAdj.Transaction_Date = Request["txtAdjDate"];
            adminNewAdj.Transaction_Amount = Request["txtAdjAmount"];
            adminNewAdj.TransactionType_ID = "4";
            if (Request["ddlAdjtype"] == "1")
            {
                adminNewAdj.AdjustmentType_ID = 4;
                adminNewAdj.Notes = "The Amount can be Use Later";
            }
            else if (Request["ddlAdjtype"] == "2")
            {
                adminNewAdj.AdjustmentType_ID = 5;
                adminNewAdj.Notes = "The Amount is Write off";
            }
            else if (Request["ddlAdjtype"] == "3")
            {
                adminNewAdj.Notes = Request["txtAdjNotes"];
            }


            //if (TempData["Practiceid"] != null)
            //{
            //    adminNewAdj.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            //}

            TempData.Keep("Practiceid");
            adminNewAdj.UserId = Convert.ToString(Session["UserID"]);
            //adminNewAdj.RefTransaction_ID = null;
            tranId = adminNewAdj.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));
            return RedirectToAction("AdminLevelTransactionsList", new { pracId = TempData["Practiceid"], pracname = TempData["pracname"] });
        }
        public List<RPBilling> getAdminSummary(RPBilling objAdmin)
        {

            List<RPBilling> listSummary = new List<RPBilling>();
            try
            {
            if (objAdmin.ProviderName == null)
            {
                listSummary = objAdmin.GetAdminListSummary(objAdmin);
            }
            else
            {
                listSummary = objAdmin.GetAdminSearchListSummary(objAdmin);
            }
            if (listSummary.Count > 0)
            {
                //totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
                ViewBag.totrec = RPBilling.Totalnoofrecords;
                //StringBuilder stradminsummary = RPBilling.stradminSummary;
                ViewBag.ProvBillsum = Convert.ToString(RPBilling.stradminSummary);
            }
            else
            {
                //StringBuilder stradminsummary = RPBilling.stradminSummary;
                ViewBag.ProvBillsum = Convert.ToString(RPBilling.stradminSummary);
                ViewBag.totrec = 0;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getAdminSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return listSummary;
        }
        public void FillProvider(string filter, int offset, string hdnprid)
        {
            try
            {
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter.Trim();
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            if (!string.IsNullOrEmpty(hdnprid) || !string.IsNullOrEmpty(PracticeName))
            {
                foreach (ComboBoxItem item in payerlist)
                {
                    if (item.Value == hdnprid || item.Text == PracticeName)
                    {
                        item.Selected = true;
                    }
                    items.Add(item);
                }
                ViewData["ComboBoxPr"] = items;
            }
            else
            {
                ViewData["ComboBoxPr"] = payerlist;
            }

            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "FillProvider", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult AdminviewTransaction(int tranid)
        {

            DataSet dsview = new DataSet();
            RPBilling viewInfo = new RPBilling();
            dsview = viewInfo.getViewPrTransaction(tranid);
            StringBuilder strView = new StringBuilder();
            strView = strView.Append("<table cellspacing='1' cellpadding='6' width='100%' class='border_style'>");
            if (dsview.Tables[0].Rows.Count > 0)
            {
                strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Practice name :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["ToRefName"] + "</td></tr>");
                if (Convert.ToString(dsview.Tables[0].Rows[0]["TransactionType"]) != "Payment")
                {
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Charged to :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["ChargedToorPaidby"] + "</td></tr>");
                    //strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Service name :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["ServiceName"] + "</td></tr>");
                }
                else if (Convert.ToString(dsview.Tables[0].Rows[0]["TransactionType"]) == "Payment")
                {
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Paid By :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["ChargedToorPaidby"] + "</td></tr>");
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Payment mode :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["PaymentMode"] + "</td></tr>");
                }
                strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Transaction type :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["TransactionType"] + "</td></tr>");
                strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Transaction date :</strong></td><td width='80%' align='left'>" + DateTime.Parse(Convert.ToString(dsview.Tables[0].Rows[0]["Transaction_Date"])).ToShortDateString() + "</td></tr>");
                if (Convert.ToString(dsview.Tables[0].Rows[0]["TransactionType"]) != "Payment")
                {
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Amount charged($) :</strong></td><td width='80%' align='left'>" + string.Format("{0:c}", dsview.Tables[0].Rows[0]["Transaction_Amount"]) + "</td></tr>");
                }
                else if (Convert.ToString(dsview.Tables[0].Rows[0]["TransactionType"]) == "Payment")
                {
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Amount paid($) :</strong></td><td width='80%' align='left'>" + string.Format("{0:c}", dsview.Tables[0].Rows[0]["Transaction_Amount"]) + "</td></tr>");
                }
                else
                {
                    strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Adjustment Amount($) :</strong></td><td width='80%' align='left'>" + string.Format("{0:c}", dsview.Tables[0].Rows[0]["Transaction_Amount"]) + "</td></tr>");

                }
                strView = strView.Append("<tr class='white_color'><td width='20%' align='right'><strong>Notes :</strong></td><td width='80%' align='left'>" + dsview.Tables[0].Rows[0]["Notes"] + "</td></tr>");
            }
            strView = strView.Append("</table>");
            ViewBag.providertran = Convert.ToString(strView);
            return View();

        }
        public List<RPBilling> getPracticeTransactions(RPBilling objAdmin)
        {

            List<RPBilling> listPracticeTransactions = new List<RPBilling>();
            try
            {
            listPracticeTransactions = objAdmin.Get_PracticeTrasnactions(objAdmin);
            if (listPracticeTransactions.Count > 0)
            {
                //totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
                ViewBag.totrec = RPBilling.Totalnoofrecords;
            }
            else
            {
                ViewBag.totrec = 0;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getPracticeTransactions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return listPracticeTransactions;
        }
        public List<RPBilling> getPracticePaymentGrid()
        { 
            List<RPBilling> listPracticepayTransactions = new List<RPBilling>();
            try
            {
            RPBilling objadmin = new RPBilling();
            if (TempData["frmdate"] != null)
            {
                objadmin.FromDate = Convert.ToString(TempData["frmdate"]);
                TempData.Keep("frmdate");
            }
            if (TempData["ToDate"] != null)
            {
                objadmin.ToDate = Convert.ToString(TempData["ToDate"]);
                TempData.Keep("ToDate");
            }
            if (TempData["Summarypagesize"] != null)
            {
                objadmin.NoOfRecords = Convert.ToString(TempData["Summarypagesize"]);
                ViewBag.Summarypagesize = TempData["Summarypagesize"];
                TempData.Keep("Summarypagesize");
            }
            else
            {
                objadmin.NoOfRecords = "10";
                ViewBag.Summarypagesize = "10";
            }
            objadmin.OrderbyItem = string.IsNullOrEmpty(Request["g2sort"]) ? "Transaction_Date" : Request["g2sort"];
            objadmin.orderBy = string.IsNullOrEmpty(Request["g2sortdir"]) ? "DESC" : Request["g2sortdir"];
            objadmin.PageNo = string.IsNullOrEmpty(Request["g2p2"]) ? "1" : Request["g2p2"];
            objadmin.TransactionTypeIDs = "2";
            objadmin.ReferenceType_ID = "1";
            objadmin.fromReferenceId = 1;
            //objadmin.Facility_ID = null;
            //objadmin.ToReferenceType_ID = null;
            //objadmin.ToReference_ID = null;
            //objadmin.ServiceID = null;
            if (TempData["Practiceid"] != null)
            {
                objadmin.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            }

            TempData.Keep("Practiceid");
           
            listPracticepayTransactions = objadmin.Get_PracticeTrasnactions(objadmin);
            if (listPracticepayTransactions.Count > 0)
            {
                //totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
                ViewBag.totrec = RPBilling.Totalnoofrecords;
            }
            else
            {
                ViewBag.totrec = 0;
            }
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getPracticePaymentGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return listPracticepayTransactions;

        }
        public List<RPBilling> getPracticeAdjGrid()
        {
             List<RPBilling> listPracticeAdjTransactions = new List<RPBilling>();
            try
            {
            RPBilling objadmin = new RPBilling();
            if (TempData["frmdate"] != null)
            {
                objadmin.FromDate = Convert.ToString(TempData["frmdate"]);
                TempData.Keep("frmdate");
            }
            if (TempData["ToDate"] != null)
            {
                objadmin.ToDate = Convert.ToString(TempData["ToDate"]);
                TempData.Keep("ToDate");
            }
            if (TempData["Summarypagesize"] != null)
            {
                objadmin.NoOfRecords = Convert.ToString(TempData["Summarypagesize"]);
                ViewBag.Summarypagesize = TempData["Summarypagesize"];
                TempData.Keep("Summarypagesize");
            }
            else
            {
                objadmin.NoOfRecords = "10";
                ViewBag.Summarypagesize = "10";
            }
            objadmin.OrderbyItem = string.IsNullOrEmpty(Request["g3sort"]) ? "Transaction_Date" : Request["g3sort"];
            objadmin.orderBy = string.IsNullOrEmpty(Request["g3sortdir"]) ? "DESC" : Request["g3sortdir"];
            objadmin.PageNo = string.IsNullOrEmpty(Request["g3p3"]) ? "1" : Request["g3p3"];
            objadmin.TransactionTypeIDs = "3,4";
            objadmin.ReferenceType_ID = "1";
            objadmin.fromReferenceId = 1;
            //objadmin.Facility_ID = null;
            //objadmin.ToReferenceType_ID = null;
            //objadmin.ToReference_ID = null;
            //objadmin.ServiceID = null;
            if (TempData["Practiceid"] != null)
            {
                objadmin.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            }

            TempData.Keep("Practiceid");
           
            listPracticeAdjTransactions = objadmin.Get_PracticeTrasnactions(objadmin);
            if (listPracticeAdjTransactions.Count > 0)
            {
                //totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
                ViewBag.totrec = RPBilling.Totalnoofrecords;
            }
            else
            {
                ViewBag.totrec = 0;
            }
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getPracticeAdjGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return listPracticeAdjTransactions;

        }
        public ActionResult practicepmtlist()
        {
            dynamic paymenttranGrid = getPracticePaymentGrid();
            return View(paymenttranGrid);
        }
        public ActionResult PracticeAdjList()
        {
            dynamic AdjustmentGrid = getPracticeAdjGrid();
            return View(AdjustmentGrid);
        }
        public ActionResult InvoiceWizardSearch()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            RPBilling objAdmin = new RPBilling();
            objAdmin.fromReferenceId = 1;
            objAdmin.ReferenceType_ID = "1";
            if (TempData["Practiceid"] != null)
            {
                objAdmin.ToReference_ID = Convert.ToString(TempData["Practiceid"]);
                TempData.Keep("Practiceid");
                ViewBag.proid = Convert.ToInt32(TempData["Practiceid"]);
            }
            ViewBag.proname = TempData["pracname"];
            TempData.Keep("pracname");
            objAdmin.ToReferenceType_ID = "2";
            string begindate = objAdmin.Get_InvoiceBeginDate(objAdmin);
            
            ViewBag.begindate = !string.IsNullOrEmpty(begindate) ? DateTime.Parse(begindate).ToShortDateString() : null;
            return View();
        }
        [HttpPost]
        public ActionResult InvoiceWizardSearch(string str)
        {
            if (!string.IsNullOrEmpty(Request["txtDateRangeFrom"]))
            {
                TempData["txtDateRangeFrom"] = Request["txtDateRangeFrom"];
            }
            if (!string.IsNullOrEmpty(Request["txtDateRangeTo"]))
            {
                TempData["txtDateRangeTo"] = Request["txtDateRangeTo"];
            }
            if (!string.IsNullOrEmpty(Request["txtInvoiceDate"]))
            {
                TempData["txtInvoiceDate"] = Request["txtInvoiceDate"];
            }
            return RedirectToAction("InvoiceWizardList");//, new { proname = Convert.ToString(Request["hdnproname"]) });
        }
        public ActionResult ReceiptList(string btnAdjsave, string btnSearch, string[] chkreciptbox)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            if (btnAdjsave != "save")
            {
                RPBilling reciptinfo = new RPBilling();
                reciptinfo.fromReferenceId = 1;
                reciptinfo.ReferenceType_ID = "1";
                reciptinfo.ToReference_ID = null;
                reciptinfo.ToReferenceType_ID = null;
                if (TempData["Practiceid"] != null)
                {
                    reciptinfo.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                    ViewBag.providername = TempData["pracname"];
                    ViewBag.provider_id = Convert.ToInt32(TempData["Practiceid"]);
                }
                TempData.Keep("Practiceid");
                TempData.Keep("pracname");
                if (Request["dt_filter"] == "30")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                    reciptinfo.FromDate = ViewBag.frmdate;
                    reciptinfo.ToDate = strDate;
                    ViewBag.todate = strDate;
                    ViewBag.setDt = "30";
                }
                else if (Request["dt_filter"] == "All")
                {
                    reciptinfo.FromDate = null;
                    reciptinfo.ToDate = null;
                    ViewBag.frmdate = null;
                    ViewBag.todate = null;
                    ViewBag.setDt = "All";
                }
                else if (Request["dt_filter"] == "Custom")
                {
                    if (!string.IsNullOrEmpty(Request["txt_FromDate"]))
                    {
                        ViewBag.frmdate = Request["txt_FromDate"];
                        TempData["frmdate"] = Request["txt_FromDate"];
                    }
                    if (!string.IsNullOrEmpty(Request["txt_ToDate"]))
                    {
                        ViewBag.todate = Request["txt_ToDate"];
                        TempData["ToDate"] = Request["txt_ToDate"];
                    }
                    ViewBag.setDt = "Custom";
                    reciptinfo.FromDate = ViewBag.frmdate;
                    reciptinfo.ToDate = ViewBag.todate;
                }
                else if (Request["dt_filter"] == "Today")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    ViewBag.frmdate = strDate;
                    ViewBag.todate = strDate;
                    ViewBag.setDt = "Today";
                    reciptinfo.FromDate = ViewBag.frmdate;
                    reciptinfo.ToDate = ViewBag.todate;
                }
                else
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                    reciptinfo.FromDate = ViewBag.frmdate;
                    reciptinfo.ToDate = strDate;
                    ViewBag.todate = strDate;
                    ViewBag.setDt = "7";
                }
                if (btnSearch == "Search")
                {
                    if (!string.IsNullOrEmpty(Request["Charges"]))
                    {
                        if (Request["Charges"] != "false")
                        {
                            reciptinfo.ChargeType = "Y";
                            ViewBag.ChargeType = "true";
                        }
                        else
                        {
                            reciptinfo.ChargeType = "N";
                            ViewBag.ChargeType = "false";
                        }

                    }
                    else
                    {
                        reciptinfo.ChargeType = "N";
                        ViewBag.ChargeType = "falsee";
                    }
                    if (!string.IsNullOrEmpty(Request["Payments"]))
                    {
                        if (Request["Payments"] != "false")
                        {
                            reciptinfo.paytype = "Y";
                            ViewBag.paytype = "true";
                        }
                        else
                        {
                            reciptinfo.paytype = "N";
                            ViewBag.paytype = "false";
                        }

                    }
                    else
                    {
                        reciptinfo.paytype = "N";
                        ViewBag.paytype = "false";
                    }
                    if (!string.IsNullOrEmpty(Request["Adjustments"]))
                    {
                        if (Request["Adjustments"] != "false")
                        {
                            reciptinfo.CreditInd = "Y";
                            ViewBag.CreditInd = "true";
                        }
                        else
                        {
                            reciptinfo.CreditInd = "N";
                            ViewBag.CreditInd = "false";
                        }

                    }
                    else
                    {
                        reciptinfo.CreditInd = "N";
                        ViewBag.CreditInd = "false";
                    }
                }
                else
                {

                    reciptinfo.ChargeType = "Y";
                    reciptinfo.paytype = "Y";
                    reciptinfo.CreditInd = "Y";
                    ViewBag.ChargeType = "true";
                    ViewBag.paytype = "true";
                    ViewBag.CreditInd = "true";
                }
                if (string.IsNullOrEmpty(Request["sort"]))
                {
                    reciptinfo.OrderbyItem = "Transaction_Date";
                    ViewBag.sort = "Transaction_Date";
                }
                else
                {
                    reciptinfo.OrderbyItem = Request["sort"];
                    ViewBag.sort = Request["sort"];
                }
                if (string.IsNullOrEmpty(Request["sortdir"]))
                {
                    reciptinfo.orderBy = "DESC";
                    ViewBag.sortdir = "DESC";
                }
                else
                {
                    reciptinfo.orderBy = Request["sortdir"];
                    ViewBag.sortdir = Request["sortdir"];
                }
                
                reciptinfo.PageNo = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
                
                if (Request["ddlrecords"] != null)
                {
                    ViewBag.Summarypagesize = Request["ddlrecords"];
                    TempData["Summarypagesize"] = Request["ddlrecords"];
                    reciptinfo.NoOfRecords = Request["ddlrecords"];
                }
                else if (Session["Rowsperpage"] != null)
                {
                    ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                    TempData["Summarypagesize"] = Session["Rowsperpage"].ToString();
                    reciptinfo.NoOfRecords = Session["Rowsperpage"].ToString();
                }
                else
                {
                    ViewBag.Summarypagesize = "10";
                    TempData["Summarypagesize"] = "10";
                    reciptinfo.NoOfRecords = "10";
                }

                dynamic getRecipt = getGenerateRecipt(reciptinfo);
                return View(getRecipt);

            }
            else
            {
                string tranids = string.Empty;
                foreach (var ids in chkreciptbox)
                {

                    tranids += ids + ",";
                    TempData["tranids"] = tranids;
                }
                //string proname = Convert.ToString(Request["hdnprovidername"]);
                return RedirectToAction("PrintReceipt");//, new { proname = proname });
            }
        }

        public List<RPBilling> getGenerateRecipt(RPBilling objadmin)
        {

            List<RPBilling> objgetrecipt = new List<RPBilling>();
            try{
            objgetrecipt = objadmin.Get_AdminAddRecipts(objadmin);
            //if (objgetrecipt.Count > 0)
            //{
            //    totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
            //    ViewBag.totrec = totalrecords;
            //}
            //else
            //{
            //    ViewBag.totrec = "0";
            //}
                ViewBag.totrec = objgetrecipt.Count > 0 ? RPBilling.Totalnoofrecords : 0;
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getGenerateRecipt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objgetrecipt;
        }
        public ActionResult InvoiceWizardList()//string proname, string proid)
        {
            ViewBag.Count = 5;
            Session["CurrentUrl4"] = Request.RawUrl;
            //if (TempData["pracname"] != null)
            //{
            //    if (Convert.ToString(TempData["pracname"]) != proname)
            //    {
            //        return RedirectToAction("PageNotFound", "Error");
            //    }
            //}
            RPBilling adminInvoice = new RPBilling();
            adminInvoice.fromReferenceId = 1;
            adminInvoice.ReferenceType_ID = "1";
            if (TempData["Practiceid"] != null)
            {
                ViewBag.proid = Convert.ToInt32(TempData["Practiceid"]);
                adminInvoice.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                ViewBag.proname = Convert.ToString(TempData["pracname"]);// proname;
            }
            if (TempData["txtDateRangeFrom"] != null)
            {
                adminInvoice.FromDate = Convert.ToString(TempData["txtDateRangeFrom"]);
                ViewBag.fromdate = Convert.ToString(TempData["txtDateRangeFrom"]);
            }
            if (TempData["txtDateRangeTo"] != null)
            {
                adminInvoice.ToDate = Convert.ToString(TempData["txtDateRangeTo"]);
                ViewBag.ToDate = Convert.ToString(TempData["txtDateRangeTo"]);
            }
            ViewBag.pracname = TempData["pracname"];
            TempData.Keep("pracname");
            TempData.Keep("Practiceid");
            TempData.Keep("txtDateRangeTo");
            TempData.Keep("txtDateRangeFrom");
            adminInvoice.orderBy = "Asc";
            adminInvoice.OrderbyItem = "Transaction_Date";
            dynamic invoicelist = getGenerateInvoice(adminInvoice);

            return View(invoicelist);
        }
        [HttpPost]
        public ActionResult InvoiceWizardList(string str)
        {
            return RedirectToAction("InvoiceWizardPreview");//, new { proid = Convert.ToString(Request["hdnproid"]), proname = Convert.ToString(Request["hdnproname"]) });
        }
        public ActionResult InvoiceWizardPreview()//string proid, string proname)
        {
            //if (TempData["Practiceid"] != null)
            //{
            //    if (Convert.ToString(TempData["Practiceid"]) != proid || Convert.ToString(TempData["pracname"]) != proname)
            //    {
            //        return RedirectToAction("PageNotFound", "Error");
            //    }
            //}
            RPBilling objInvoice = new RPBilling();
            objInvoice.fromReferenceId = 1;
            objInvoice.ReferenceType_ID = "1";
            objInvoice.ToReference_ID = Convert.ToString(TempData["Practiceid"]);
            objInvoice.ToReferenceType_ID = "2";
            if (TempData["Practiceid"] != null)
            {
                objInvoice.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                ViewBag.proid = Convert.ToInt32(TempData["Practiceid"]);
                ViewBag.proname = Convert.ToString(TempData["pracname"]);// proname;
            }
            if (TempData["txtDateRangeFrom"] != null)
            {
                objInvoice.FromDate = Convert.ToString(TempData["txtDateRangeFrom"]);
            }
            if (TempData["txtDateRangeTo"] != null)
            {
                objInvoice.ToDate = Convert.ToString(TempData["txtDateRangeTo"]);

            }
            objInvoice.Messagetouser = "Thankyou";
            objInvoice.TermsAndConditions = "Thankyou";
            objInvoice.UserId = Convert.ToString(Session["UserID"]);
            TempData.Keep("Practiceid");
            TempData.Keep("pracname");
            TempData.Keep("txtDateRangeFrom");
            TempData.Keep("txtDateRangeTo");
            int invId = 0;
            invId = objInvoice.INS_AdmintoPracticeInvoice(objInvoice);
            if (invId != 0)
            {
                objInvoice.Invoice_ID = invId;
                DataSet dsInvoice = new DataSet();
                dsInvoice = objInvoice.practiceInvoicelist(objInvoice);
                StringBuilder strPreview = new StringBuilder();

                for (int i = 0; i <= dsInvoice.Tables[0].Rows.Count - 1; i++)
                {
                    string prAddress = Convert.ToString(dsInvoice.Tables[0].Rows[0]["FullAddress"]);
                    ViewBag.invoiceno = dsInvoice.Tables[0].Rows[0]["InvoiceNo"];
                    ViewBag.invoiceDate = DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Invoice_Date"])).ToShortDateString();
                    string[] arrayAddress = prAddress.Split(',');
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' width='80%'><tr class='background_color'><td width='50%'>");
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='0' height='100%' width='100%'><tr class='background_color'><td style='PADDING-LEFT: 10px'><b>Bill To:&nbsp; </b></td></tr>");
                    strPreview = strPreview.Append("<tr class='background_color' height='52'><td><span id='lblBillable'>" + dsInvoice.Tables[0].Rows[0]["PracticeName"] + "</span><br><span id='lblAddress'>" + arrayAddress[0] + ",<br>" + arrayAddress[1] + ",<br>" + arrayAddress[2] + "</span></td></tr></table></td>");
                    strPreview = strPreview.Append("<td><table cellpadding='1' cellspacing='1' border='0' height='100%' width='100%'><tr class='background_color' height='22'><td colspan='2' align='center'><b>Summary</b></td></tr>");
                    strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Previous Balance&nbsp;:&nbsp;</b></td><td width='50%'><span id='lblBalenceforwarded'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["PrevBalance"]) + "</span></td></tr>");
                    strPreview = strPreview.Append("<tr class='background_color'><td style='HEIGHT: 14px' align='right'><b>Current&nbsp;Charges&nbsp;:&nbsp;</b></td><td style='HEIGHT: 14px'><span id='lblCInvoice'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceCharges"]) + "</span></td></tr>");
                    strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Payments&nbsp;:&nbsp;</b></td><td><span id='lblPayments'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoicePayments"]) + "</span></td></tr>");
                    strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Balance&nbsp;:&nbsp;</b></td><td><span id='lblBalence'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceBalance"]) + "</span></td></tr></table></td></tr></table>");
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'>Invoice period &nbsp;&nbsp;From&nbsp;: &nbsp;" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Begin_Date"])).ToShortDateString() + " &nbsp;&nbsp;To&nbsp;:&nbsp;" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["End_Date"])).ToShortDateString() + "  </td></tr></table>");
                }
                strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'><font size='3'><strong>Transactions</strong></font> </td></tr></table>");
                strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'>");
                strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'><b>Date</b></td><td style='width:100px;'><b>Transaction Type</b></td><td style='width:100px;'><b>Items</b></td><td style='width:250px;'><b>Notes</b></td><td style='width:100px;' align='right'><b>Amount</b></td></tr>");
                for (int j = 0; j <= dsInvoice.Tables[1].Rows.Count - 1; j++)
                {
                    strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'>" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[1].Rows[j]["Transaction_Date"])).ToShortDateString() + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["TransactionType"] + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["BillingChargeType"] + "</td><td style='width:250px;'>" + dsInvoice.Tables[1].Rows[j]["Notes"] + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td></tr>");
                }
                strPreview = strPreview.Append("</table>");
                ViewBag.invoiceprview = Convert.ToString(strPreview);
            }
            return View();
        }
        public List<RPBilling> getGenerateInvoice(RPBilling objadmin)
        {

            List<RPBilling> objInvoice = new List<RPBilling>();
            try
            {
            objInvoice = objadmin.List_AdmintoPracticeChargesToGenerateInvoice(objadmin);
            //if (objInvoice.Count > 0)
            //{
            //    totalrecords = Convert.ToString(objInvoice.Count);
            //    ViewBag.totrec = totalrecords;
            //}
            //else
            //{
            //    ViewBag.totrec = "0";
            //}
            ViewBag.totrec = objInvoice.Count > 0 ? objInvoice.Count : 0;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminHomeController", "getGenerateInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objInvoice;
        }
        public ActionResult PrintReceipt()//string proname)
        {
            //if (TempData["pracname"] != null)
            //{
            //    if (Convert.ToString(TempData["pracname"]) != proname)
            //    {
            //        return RedirectToAction("PageNotFound", "Error");
            //    }
            //}
            TempData.Keep("tranids");
            TempData.Keep("pracname");
            string transRecipt = string.Empty;
            if (Convert.ToString(TempData["tranids"]) != "")
            {
                transRecipt = Convert.ToString(TempData["tranids"]);

                RPBilling objRecipt = new RPBilling();
                objRecipt.fromReferenceId = 1;
                objRecipt.ReferenceType_ID = "1";

                if (TempData["Practiceid"] != null)
                {
                    objRecipt.ToReference_ID = Convert.ToString(TempData["Practiceid"]);
                    objRecipt.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                    ViewBag.proid = Convert.ToString(TempData["Practiceid"]);
                    ViewBag.proname = Convert.ToString(TempData["pracname"]);// proname;
                }
                objRecipt.ToReferenceType_ID = "2";
                objRecipt.Transaction_IDs = transRecipt;
                objRecipt.FromDate = DateTime.Now.ToString();
                objRecipt.ToDate = DateTime.Now.ToString();
                objRecipt.UserId = Convert.ToString(Session["UserID"]);
                TempData.Keep("Practiceid");
                int recid = 0;
                recid = objRecipt.INS_AdmintoPracticeReceipt(objRecipt);
                if (recid != 0)
                {
                    objRecipt.Invoice_ID = recid;
                    DataSet dsInvoice = new DataSet();
                    dsInvoice = objRecipt.practiceInvoicelist(objRecipt);
                    StringBuilder strPreview = new StringBuilder();

                    for (int i = 0; i <= dsInvoice.Tables[0].Rows.Count - 1; i++)
                    {
                        string prAddress = Convert.ToString(dsInvoice.Tables[0].Rows[0]["FullAddress"]);
                        ViewBag.recid = dsInvoice.Tables[0].Rows[0]["recieptno"];
                        ViewBag.recDate = DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Invoice_Date"])).ToShortDateString();
                        string[] arrayAddress = prAddress.Split(',');
                        strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' width='80%'><tr class='background_color'><td width='50%'>");
                        strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='0' height='100%' width='100%'><tr class='background_color'><td style='PADDING-LEFT: 10px'><b>Bill To:&nbsp; </b></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color' height='52'><td><span id='lblBillable'>" + dsInvoice.Tables[0].Rows[0]["PracticeName"] + "</span><br><span id='lblAddress'>" + arrayAddress[0] + ",<br>" + arrayAddress[1] + ",<br>" + arrayAddress[2] + "</span></td></tr></table></td>");
                        strPreview = strPreview.Append("<td><table cellpadding='1' cellspacing='1' border='0' height='100%' width='100%'><tr class='background_color' height='22'><td colspan='2' align='center'><b>Summary</b></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Previous Balance&nbsp;:&nbsp;</b></td><td width='50%'><span id='lblBalenceforwarded'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["PrevBalance"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td style='HEIGHT: 14px' align='right'><b>Current&nbsp;Charges&nbsp;:&nbsp;</b></td><td style='HEIGHT: 14px'><span id='lblCInvoice'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceCharges"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Payments&nbsp;:&nbsp;</b></td><td><span id='lblPayments'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoicePayments"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Balance&nbsp;:&nbsp;</b></td><td><span id='lblBalence'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceBalance"]) + "</span></td></tr></table></td></tr></table>");

                    }
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'><font size='3'><strong>Transactions</strong></font> </td></tr></table>");
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'>");
                    strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'><b>Transaction date</b></td><td style='width:100px;'><b>Transaction Type</b></td><td style='width:250px;'><b>Notes</b></td><td style='width:100px;' align='right'><b>Amount</b></td><td style='width:100px;' align='right'><b>Total amount</b></td></tr>");
                    for (int j = 0; j <= dsInvoice.Tables[1].Rows.Count - 1; j++)
                    {
                        strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'>" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[1].Rows[j]["Transaction_Date"])).ToShortDateString() + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["TransactionType"] + "</td><td style='width:250px;'>" + dsInvoice.Tables[1].Rows[j]["Notes"] + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td></tr>");
                    }
                    strPreview = strPreview.Append("</table>");
                    ViewBag.invoiceprview = Convert.ToString(strPreview);
                }
            }
            return View();
        }
        public ActionResult DelAdminTran(int tId)
        {
            RPBilling admindel = new RPBilling();
            admindel.Transaction_ID = tId;
            admindel.UserId = Convert.ToString(Session["UserID"]);
            admindel.Del_AdminTransaction(admindel, Convert.ToInt32(Session["Loginhistory_ID"]));
            return RedirectToAction("AdminLevelTransactionsList", new { pracId = TempData["Practiceid"], pracname = TempData["pracname"], disind = TempData["disind"] });
        }
        public ActionResult ArticlesVideos(string sectionid)
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            if (Session["RoleID"] != null)
            {
                if (Session["RoleID"].ToString() == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            TempData.Remove("hdnPrid");
            Session.Add("TopIndex", "4");
            
            char AcPrv = Session["strOutIsPaid"] != null ? Convert.ToChar(Session["strOutIsPaid"]) : 'Y';
            //  if (sectionid != null)
            //{
            dsSections = ChildSections(Convert.ToInt32(Session["userid"]), "7434", Convert.ToInt32(Session["Roleid"]), AcPrv);
            //}
            //else
            //{
            //    dsSections = ChildSections(Convert.ToInt32(Session["userid"]), Convert.ToInt32(168), Convert.ToInt32(Session["Roleid"]), AcPrv);
            //}


            StringBuilder strAdmin = new StringBuilder();
            strAdmin = strAdmin.Append("<table id='Toolstable' border='0' cellpadding='20' cellspacing='0' width='100%' >");
            if (dsSections.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                strAdmin = strAdmin.Append("<tr class='background_color'>");
                string RedirectPath = string.Empty;
                foreach (DataRow drs in dsSections.Tables[0].Rows)
                {
                    if (Convert.ToString(drs["SectionPath"]) == "Admin/Articles.aspx")
                    {
                        RedirectPath = Url.Action("Articles", "Admin", null);// "../Admin/Articles";
                    }

                    else if (Convert.ToString(drs["SectionPath"]) == "Admin/publicvideos.aspx")
                    {
                        RedirectPath = Url.Action("publicvideos", "Admin", null); //"../Admin/publicvideos";
                    }
                    else
                    {
                        RedirectPath = Convert.ToString(drs["SectionPath"]);
                    }
                    if (i % 4 == 0)
                    {
                        strAdmin = strAdmin.Append("</tr><tr><td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'> <a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a> <br /> <span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span></td>  ");
                    }
                    else
                    {
                        strAdmin = strAdmin.Append("<td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'><a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a><br /><span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span> </td> ");
                    }
                    i++;
                }
                strAdmin = strAdmin.Append("</tr>");
            }
            strAdmin = strAdmin.Append("</table>");
            ViewBag.articlesvideos = Convert.ToString(strAdmin);
            return View();

        }
    }
}
