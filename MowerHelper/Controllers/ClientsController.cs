using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using aspPDF;
using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Practice;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class ClientsController : Controller
    {


        clsCommonFunctions clscommon = new clsCommonFunctions();
        int j;

        double pos3 = 300;
        double pos4 = 100;
        double height = 100;
        EASYPDF pdf = new EASYPDF();
        NotesInfo objNotesInfo = new NotesInfo();
        //int strUser1;
        //int strrole1;
        //string strPatientInd1;
        DataSet ds = new DataSet();

        //string Strdate = null;
        string ind = null;
        //  string str = null;
        string stroutmsg = null;

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ClientsHome(string txtpatients, string GlbSearch_PatientType, string ddlrecords, string txtprovider, string ProviderID, string lastname)
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            PracticeblHome objPrHome = new PracticeblHome();
            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                }
            }
            ViewBag.checkallowpat = clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin") != "" ? clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin") : null;
            Session.Add("TopIndex", 0);
            if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
            {

                return RedirectToAction("Schedulespecs", "Schedule");

            }
            else if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("Schedulespecs", "Schedule");
            }

            //if (ProviderID != null)
            //{
            //    // ViewBag.PracticeID = PracticeID;
            //    ViewBag.ProviderID = ProviderID;
            //    //if (Session["Prov_ID"] == null)
            //    //{
            //    //    Session["Prov_ID"] = ProviderID;
            //    //}
            //}
            //else if (Session["Practice_ID"] != null & Session["Prov_ID"] != null)
            //{
            //    // ViewBag.PracticeID = Session["Practice_ID"];
            //    ViewBag.ProviderID = Session["Prov_ID"];
            //}
            //else
            //{
            //    //ViewBag.PracticeID = null;
            //    ViewBag.ProviderID = null;
            //}
            //objPrHome = BuildSet(ProviderID);
            //if (Convert.ToString(Session["RoleID"]) == "1")
            //{
            //    if (Request["txtprovider"] != null && Request["txtprovider"] != "")
            //    {
            //        objPrHome.ProviderName = Request["txtprovider"];
            //    }
            //    else
            //    {
            //        objPrHome.ProviderName = null;
            //    }
            //}
           objPrHome.LastName = lastname;
            return View(objPrHome);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ClientsHomePartial(PracticeblHome objPrHome)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (objPrHome.Provider_ID != null)
                {
                    // ViewBag.PracticeID = PracticeID;
                    ViewBag.ProviderID = objPrHome.Provider_ID;
                    //if (Session["Prov_ID"] == null)
                    //{
                    //    Session["Prov_ID"] = ProviderID;
                    //}
                }
                else if (Session["Practice_ID"] != null & Session["Prov_ID"] != null)
                {
                    // ViewBag.PracticeID = Session["Practice_ID"];
                    ViewBag.ProviderID = Session["Prov_ID"];
                }
                else
                {
                    //ViewBag.PracticeID = null;
                    ViewBag.ProviderID = Session["Prov_ID"];
                }
                objPrHome = BuildSet(objPrHome.Provider_ID);
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    if (Request["txtprovider"] != null && Request["txtprovider"] != "")
                    {
                        objPrHome.ProviderName = Request["txtprovider"];
                    }
                    else
                    {
                        objPrHome.ProviderName = null;
                    }
                }
                return PartialView("_ClientsHome", objPrHome);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        public void CustomerList(string lsatnm, string fullnm, string norec, string pgno, string orderby, string orderbyitem, string status, string proid)
        {
            //Clients/ClientsHome  xl img click
            try
            {
                List<PracticeblHome> objcoll = new List<PracticeblHome>();
                var objpractice = new PracticeblHome
                {
                    Patient_Status_ID = status,
                    Provider_ID = proid,
                    LastName = lsatnm,
                    fullname = fullnm,
                    NoofRecords = null,
                    PageNo = null,
                    OrderBy = orderby,
                    Orderbyitem = orderbyitem
                };
                objcoll = PracticeblHome.Get_PracticeHomePagePatientsList(objpractice);
                string strhtml = null;
                strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td >Clients List</td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "</table>";
                if (objcoll.Count > 0)
                {

                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";

                    strhtml = strhtml + "<tr>";
                    strhtml = strhtml + "<td><span>Client name</span></td>";
                    strhtml = strhtml + "<td ><label>Phone number</label></td>";
                    strhtml = strhtml + "<td><span>Address</span></td>";
                    strhtml = strhtml + "<td ><span>Email</span></td>";
                    strhtml = strhtml + "<td><span>Next visit</span></td>";
                    strhtml = strhtml + "<td><span>Total visits</span></td>";
                    strhtml = strhtml + "<td><span>Balance</span></td>";
                    strhtml = strhtml + "<td><span>Status</span></td>";
                    strhtml = strhtml + "</tr>";
                    foreach (var item in objcoll)
                    {
                        //string Status_Ind = (string)item.Status_Ind;
                        string amount1 = null;
                        int amount = (int)item.StrTotal;
                        //string hphno = null;
                        //string cphno = null;
                        //string wpho = null;
                        if (amount >= 0)
                        {

                            amount1 = "$" + String.Format("{0:0.00}", (decimal)(item.StrTotal));
                        }
                        else
                        {

                            amount1 = "($" + String.Format("{0:0.00}", (decimal)(item.StrTotal)) + ")";
                        }

                        strhtml = strhtml + "<tr >";
                        strhtml = strhtml + "<td><b>" + item.StrPatientName + "</b></td>";
                        string HomePhone = (string)item.HomePhone + " (";
                        string WorkPhone = (string)item.WorkPhone + " (";
                        string CellPhone = (string)item.CellPhone + " (";
                        if (Convert.ToString(HomePhone) != "- (" & Convert.ToString(HomePhone) != " (" & Convert.ToString(HomePhone) != null & Convert.ToString(CellPhone) != "- (" & Convert.ToString(CellPhone) != " (" & Convert.ToString(CellPhone) != null)
                        {
                            HomePhone = HomePhone + "H)";
                            CellPhone = CellPhone + "C)";
                            strhtml = strhtml + "<td><b>" + HomePhone + "</b><br/><b>" + CellPhone + "</b></td>";
                        }
                        else if (Convert.ToString(WorkPhone) != "- (" & Convert.ToString(WorkPhone) != " (" & Convert.ToString(WorkPhone) != null & Convert.ToString(CellPhone) != "- (" & Convert.ToString(CellPhone) != " (" & Convert.ToString(CellPhone) != null)
                        {
                            WorkPhone = WorkPhone + "W)";
                            CellPhone = CellPhone + "C)";
                            strhtml = strhtml + "<td><b>" + WorkPhone + "</b><br/><b>" + CellPhone + "</b></td>";
                        }
                        else if (Convert.ToString(WorkPhone) != "- (" & Convert.ToString(WorkPhone) != " (" & Convert.ToString(WorkPhone) != null)
                        {
                            WorkPhone = WorkPhone + "W)";
                            strhtml = strhtml + "<td><b>" + WorkPhone + "</b></td>";
                        }
                        else if (Convert.ToString(HomePhone) != "- (" & Convert.ToString(HomePhone) != " (" & Convert.ToString(HomePhone) != null)
                        {
                            HomePhone = HomePhone + "H)";
                            strhtml = strhtml + "<td><b>" + HomePhone + "</b></td>";
                        }
                        else if (Convert.ToString(CellPhone) != "- (" & Convert.ToString(CellPhone) != " (" & Convert.ToString(CellPhone) != null)
                        {
                            CellPhone = CellPhone + "C)";
                            strhtml = strhtml + "<td><b>" + CellPhone + "</b></td>";
                        }
                        else if (Convert.ToString(WorkPhone) != "- (" & Convert.ToString(WorkPhone) != " (" & Convert.ToString(WorkPhone) != null)
                        {
                            WorkPhone = WorkPhone + "W)";
                            strhtml = strhtml + "<td><b>" + WorkPhone + "</b></td>";
                        }
                        strhtml = strhtml + "<td><b>" + item.Address + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.email + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.StrNextAppt + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.StrApptCount + "</b></td>";
                        strhtml = strhtml + "<td><b>" + amount1 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.Status_Ind + "</b></td>";
                        strhtml = strhtml + "</tr>";

                    }

                    strhtml = strhtml + "</table>";
                }
                else
                {
                    strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td width='10px'><b> No Clients Found. " + "</b></td>";
                    strhtml = strhtml + "</table>";
                }
                //strhtml = strhtml + "</table>";
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=Clients.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(strhtml);
                Response.End();
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "CustomerList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public PracticeblHome BuildSet(string ProviderID = null)
        {
            try
            {
                //string sortingfield = (Request["sort"] ?? null);
                PracticeblHome objpractice = new PracticeblHome();
                objpractice.Orderbyitem = Request["sort"] ?? null;
                if (objpractice.Orderbyitem == "StrPatientName")
                {
                    objpractice.Orderbyitem = "LastName";
                }
                //string pagesize = null;
                if (Request["ddlrecords"] != null)
                {
                    objpractice.NoofRecords = Request["ddlrecords"];
                    Session.Add("Rowsperpage", objpractice.NoofRecords);
                }
                else if (Session["Rowsperpage"] != null)
                {
                    objpractice.NoofRecords = Convert.ToString(Session["Rowsperpage"]);
                }
                else
                {
                    objpractice.NoofRecords = "10";
                }
                objpractice.Patient_Status_ID = (Request["GlbSearch_PatientType"] ?? "6");
                if (Convert.ToString(Session["Roleid"]) == "1")
                {
                    if (ProviderID != null)
                    {
                        objpractice.Provider_ID = ProviderID;
                    }
                    else
                    {
                        objpractice.Provider_ID = null;
                    }
                }
                else if (Convert.ToString(Session["Roleid"]) != "13")
                {
                    //objpractice.Provider_ID = Convert.ToString(Session["Prov_ID"]);
                    objpractice.Provider_ID = Convert.ToString(ViewBag.ProviderID);
                }
                if (Request["hdnlname"] != null)
                {
                    objpractice.LastName = Request["hdnlname"];
                }
                else
                {

                    objpractice.LastName = Request["lastname"];
                }
                objpractice.fullname = (string.IsNullOrEmpty(Request["txtpatients"]) ? null : Request["txtpatients"]);
                objpractice.PageNo = (Request["page"] ?? "1");
                objpractice.OrderBy = (Request["sortdir"] ?? null);
                objpractice.PatientList = PracticeblHome.Get_PracticeHomePagePatientsList(objpractice);
                ViewBag.totrec = objpractice.PatientList.Count > 0 ? PracticeblHome.Totalnoofrecords : 0;
                return objpractice;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "BuildSet", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public JsonResult GetClientfullname(string term, string PracticeID)
        {
            var objlist = new List<Autocomplete>();
            //var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
		new SqlParameter("@In_practice_id", PracticeID!=""?PracticeID:null),
		new SqlParameter("@in_Keyword", term)
	};
            clscommon.AddInParameters(objparam);
            SqlDataReader drlist = clscommon.GetDataReader("Help_dbo.St_practice_Typeahead_Patient_Fullname");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[2]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetProviderNames(string term)
        {
            var objlist = new List<Autocomplete>();
            //var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
				new SqlParameter("@in_Keyword", term)
	};
            clscommon.AddInParameters(objparam);
            SqlDataReader drlist = clscommon.GetDataReader("Help_dbo.st_Typeahead_provider_practicename");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[2]) + "$" + Convert.ToString(drlist[3]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult quickview(int PId, string status)
        {

            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    if (Request["PracticeID"] != "" & Request["PracticeID"] != null & Request["ProviderID"] != null)
                    {
                        Session.Add("Prov_ID", Request["ProviderID"]);
                        Session.Add("Practice_ID", Request["PracticeID"]);
                    }
                }
            }
            Responseparty objRp = GetDetails(PId);
            objRp.Status_Ind = status;
            objRp.Patient_ID = PId;
            return View(objRp);

        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult quickview(Responseparty objRP)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (objRP.Patient_Status_ID == 2 | objRP.Patient_Status_ID == 4 | objRP.Patient_Status_ID == 5 | objRP.Patient_Status_ID == 3)
            {
                insertRandomUserDetails(objRP.Patient_ID, objRP.Username);
            }
            Responseparty.Patient_Archive_Reject_Activate(objRP.Patient_ID, Convert.ToInt32(Request["HdnStatusID"]), Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["Prov_ID"]));
            return RedirectToAction("ClientsHome");
        }
        public string insertRandomUserDetails(int? PId, string email)
        {
            string Insmsg = null;
            try
            {

                PatientRegistration PatUserInfo = new PatientRegistration();
                //  string PatientEmail = null;
                PatientRegistration Obj = default(PatientRegistration);

                Obj = MowerHelper.Models.BLL.Patients.PatientRegistration.Get_Random_UserCredentials();

                if (email != null & email != "")
                {
                    PatUserInfo.Username = email;
                }
                var objmd5 = new VBVMD5CryptoServiceProvider();
                //string hash = objmd5.getMd5Hash(Obj.Password);
                PatUserInfo.Password = objmd5.getMd5Hash(Obj.Password);
                PatUserInfo.Reference_ID = PId;
                PatUserInfo.ReferenceType_ID = 3;
                PatUserInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

                PatUserInfo.Role_ID = 5;
                PatUserInfo.RoleName = "Patient";
                PatUserInfo.Status_Ind = "N";
                Insmsg = PatientRegistration.Insert_Security_INS_FTUser(PatUserInfo);
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "insertRandomUserDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return Insmsg;
        }
        private Responseparty GetDetails(int PId)
        {

            try
            {

                Responseparty objRP = default(Responseparty);
                objRP = Responseparty.Get_ResponsibleParty_Info(PId, !string.IsNullOrEmpty(Convert.ToString(Session["Practice_ID"])) ? Convert.ToInt32(Session["Practice_ID"]) : 0, Convert.ToInt32(Session["Prov_ID"]));

                if ((objRP != null))
                {

                    objRP.Username = !string.IsNullOrEmpty(objRP.Email) ? objRP.Email : null;
                    objRP.PatientName = objRP.Prefix + " " + objRP.FirstName + " " + objRP.MI + " " + objRP.LastName + " " + objRP.Suffix;
                    if (!string.IsNullOrEmpty(objRP.Address1))
                    {
                        objRP.TotalAddress = objRP.Address1;
                    }
                    if (!string.IsNullOrEmpty(objRP.Address2))
                    {
                        objRP.TotalAddress = objRP.TotalAddress != null ? (objRP.TotalAddress + ", " + objRP.Address2) : objRP.Address2;
                    }
                    if (!string.IsNullOrEmpty(objRP.Zip))
                    {
                        objRP.TotalAddress = objRP.TotalAddress != null ? (objRP.TotalAddress + ", " + objRP.Zip) : objRP.Zip;
                    }
                    if (!string.IsNullOrEmpty(objRP.State))
                    {
                        objRP.TotalAddress = objRP.TotalAddress + ", " + objRP.State;
                    }
                    if (!string.IsNullOrEmpty(objRP.City))
                    {
                        objRP.TotalAddress = objRP.TotalAddress + ", " + objRP.City;
                    }
                    objRP.HomePhone = !string.IsNullOrEmpty(objRP.HomePhone) ? clsCommonFunctions.UsPhoneFormat(objRP.HomePhone) : null;

                    objRP.WPhone = !string.IsNullOrEmpty(objRP.WPhone) ? clsCommonFunctions.UsPhoneFormat(objRP.WPhone) : null;
                    if (!string.IsNullOrEmpty(objRP.MPhone))
                    {
                        objRP.MPhone = clsCommonFunctions.UsPhoneFormat(objRP.MPhone);
                    }

                }
                return objRP;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "GetDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult ClientStatusChange(int PId, int StatusID, string Email)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Request.IsAjaxRequest() && Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                if (Session["RoleID"] != null)
                {
                    if (Convert.ToString(Session["RoleID"]) == "1")
                    {
                        if (!string.IsNullOrEmpty(Request["PracticeID"]) & !string.IsNullOrEmpty(Request["ProviderID"]))
                        {
                            Session.Add("Prov_ID", Request["ProviderID"]);
                            Session.Add("Practice_ID", Request["PracticeID"]);
                        }
                    }
                }

                //Responseparty objPR = GetDetails(PId);
                if (Request["status"] == "Suspend")
                {
                    insertRandomUserDetails(PId, Email);
                }


                Responseparty.Patient_Archive_Reject_Activate(PId, StatusID, Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["Prov_ID"]));
                return RedirectToAction("ClientsHomePartial");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult Totalvisits()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string pat_id = Request["patid"];
                var totalvisits = Buildtotalvisits();
                return View(totalvisits);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public List<patients> Buildtotalvisits()
        {//clients total visits
            try
            {
                patients objappHistory = new patients();
                List<patients> appList = null;

                objappHistory.Patient_ID = Convert.ToInt32(Request["patid"] != null ? Request["patid"] : null);
                objappHistory.Provider_ID = Convert.ToInt32(Request["ProviderID"] != null ? Request["ProviderID"] : null);
                objappHistory.OrderByItem = (Request["sort"] == null ? null : Request["sort"]);
                objappHistory.order = (Request["sortdir"] == null ? null : Request["sortdir"]);
                objappHistory.NoofRecords = Convert.ToInt32(Request["ddlrecords"] == null ? "5" : Request["ddlrecords"]);
                objappHistory.page = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
                appList = patients.Get_AppointmentHistory(objappHistory);
                ViewBag.totrec = appList.Count > 0 ? patients.TotalRecords : 0;
                return appList;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Buildtotalvisits", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public string Getstatename(string stateid)
        {
            try
            {
                //clsCommonFunctions clscommon = new clsCommonFunctions();
                SqlDataReader dr_GetName;
                IDataParameter[] InParmList = { new SqlParameter("@in_State_ID", stateid) };
                clscommon.AddInParameters(InParmList);
                dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_StateName");
                while (dr_GetName.Read())
                {
                    return dr_GetName["State"].ToString() != null ? dr_GetName["State"].ToString() : null;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Getstatename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
            return null;
        }
        public string Getcityname(string cityid)
        {
            try
            {
                //clsCommonFunctions clscommon = new clsCommonFunctions();
                SqlDataReader dr_GetName;
                IDataParameter[] InParmList = { new SqlParameter("@in_City_ID", cityid) };
                clscommon.AddInParameters(InParmList);
                dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
                while (dr_GetName.Read())
                {
                    return dr_GetName["City"].ToString() != null ? dr_GetName["City"].ToString() : null;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Getcityname", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
            return null;
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult ClientFile(string id, int? PatientLoginID)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //if (!string.IsNullOrEmpty(hdnclientEmail))
            //{
            //    Session.Add("hdnclientEmail", hdnclientEmail);
            //}
            //else if (Session["hdnclientEmail"] != null)
            //{
            //    Session.Add("hdnclientEmail", Session["hdnclientEmail"]);
            //}
            //else
            //{
            //    Session.Add("hdnclientEmail", null);
            //}
            if (PatientLoginID != 0 & PatientLoginID != null)
            {
                Session.Add("PatientLoginID", PatientLoginID);
            }
            if (!string.IsNullOrEmpty(Request["CustomerID"]) || Session["PatientID"] != null)
            {
                if (Session["RoleID"] != null)
                {
                    if (Convert.ToString(Session["RoleID"]) == "1")
                    {
                        if (!string.IsNullOrEmpty(Request["ProviderID"]))
                        {
                            Session.Add("Prov_ID", Request["ProviderID"]);
                            Session.Add("ComboProv_ID", Request["ProviderID"]);
                            //Session.Add("Practice_ID", Request["PracticeID"]);
                        }
                    }
                }
                if (clscommon.SearchPatientExists(!string.IsNullOrEmpty(Request["CustomerID"]) ? Request["CustomerID"] : Convert.ToString(Session["PatientID"]), Convert.ToString(Session["Prov_ID"])).ToUpper() == "N")
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
                else
                {
                    Session.Add("PatientID", !string.IsNullOrEmpty(Request["CustomerID"]) ? Request["CustomerID"] : Session["PatientID"]);
                }

                //GetClientInfo();
                return View(GetClientInfo());
            }

            return View();
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult Getaddress(string address)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (Request.IsAjaxRequest() && Session["UserID"] == null)
                {
                    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
                }
                Patient_Info obj = new Patient_Info();

                //if (Session["roleid"].ToString() != "1")
                //{
                //    obj.PracticeID = null;
                //    obj.ProviderID = Session["Prov_ID"].ToString();
                //}
                //else
                //{
                //    obj.PracticeID = Session["ComboProv_ID"].ToString();
                //    obj.ProviderID = Session["ComboProv_ID"].ToString();
                //}
                //obj.PracticeID = Session["Practice_ID"].ToString();
                // obj.PatientID = Convert.ToString(patid);
                // obj = Patient_Info.ViewPatineInfo(obj);
                obj.l_GoogleMapPath = address.Replace("-", ",").Trim();
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public ActionResult PrintFile()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pdf.Create();
            StringBuilder strhtml = new StringBuilder();
            strhtml.Append("<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='80%'>");
            strhtml.Append("<tr align='center'>");
            strhtml.Append("<td align='center'><font size='15px'><b><u>Client file</u></b></font></td>");
            strhtml.Append("</tr>");
            strhtml.Append("<tr>");
            strhtml.Append("<td align='center' height='25%'></td>");
            strhtml.Append("</tr>");
            strhtml.Append("</table>");
            pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
            GetClientfile(Convert.ToString(Session["PatientLoginID"]));
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=ClientFile.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            return null;
        }
        [HttpPost()]
        public ActionResult PrintBill()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pdf.Create();
            string message = null;
            string fromdate = null;
            string todate = null;
            if (Request["ddlMessage"] != "0")
            {
                if (Request["ddlMessage"] == "1")
                {
                    message = "Thank you.";
                }
                else if (Request["ddlMessage"] == "2")
                {
                    message = "Thank you for your prompt payment.";
                }
                else if (Request["ddlMessage"] == "3")
                {
                    message = "Due upon receipt.";
                }
                else if (Request["ddlMessage"] == "4")
                {
                    message = "This bill sent multiple times.";
                }
                else if (Request["ddlMessage"] == "5")
                {
                    message = "Last bill before collection services.";
                }
                else
                {
                    message = "Thank you for your payment.";
                }
            }
            else
            {
                message = null;
            }
            if (Request["rdoNOnCustom_Custom"] != null)
            {
                fromdate = Request["txtFromDate"];
                todate = Request["txtToDate"];
            }
            if (Request["rdoNOnCustom_Today"] != null)
            {
                fromdate = DateTime.Now.ToShortDateString();
                todate = DateTime.Now.ToShortDateString();
            }
            if (Request["rdoNOnCustom_Last7Days"] != null)
            {
                string strDate = DateTime.Now.ToShortDateString();
                fromdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                todate = strDate;
            }
            if (Request["rdoNOnCustom_Last30Days"] != null)
            {
                string strDate = DateTime.Now.ToShortDateString();
                fromdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                todate = strDate;
            }
            //if (Request["hdnchktype"] != null)
            //{
            //    str = Request["hdnchktype"];
            //}
            if (Session["PatientID"] != null)
            {
                ListPrint(Convert.ToString(Session["PatientID"]), "3", Convert.ToString(Session["Practice_ID"]), Convert.ToString(Session["PatientLoginId"]), Convert.ToString(Session["Prov_ID"]), message, fromdate, todate);
            }

            if (ind != "Y")
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-disposition", "attachment; filename=Printbill.pdf");
                Response.BinaryWrite((byte[])pdf.SaveVariant());
                Response.End();
            }
            return null;
        }
        private void ListPrint(string ToreferenceID, string ToreferencetypeID, string PracticeID, string PatientLoginID, string ProviderID, string msg, string Fromdate, string Todate)
        {
            try
            {
                string strhtml = null;
                PatientBalance objbilling = new PatientBalance();
                List<PatientBalance> objlist = new List<PatientBalance>();
                objbilling.PatientLoginID = PatientLoginID;
                objbilling.FromDate = Fromdate;
                objbilling.ToDate = Todate;
                objbilling.PracticeID = PracticeID;
                objbilling.ProviderID = ProviderID;
                objbilling.TorefID = ToreferenceID;
                objbilling.ServiceID = 0;
                if (ToreferencetypeID == "3")
                {
                    objbilling.PaidBy = "Patient";
                }
                objlist = PatientBalance.Get_PrintBillstatement(objbilling);
                if (objlist.Count > 0)
                {
                    if (objlist[objlist.Count - 1].Totalbalance == "0.0000")
                    {
                        msg = "Thank you for your business";
                    }
                    int j = 2;
                    double Totalcharges = 0;
                    double Totalpayments = 0;
                    double Totalbal = 0;
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' border='0' bordercolor='#d3d3d3' width='85%' align='center'>";
                    strhtml = strhtml + "<tr bgcolor='white' height='40px'>";
                    strhtml = strhtml + "<td><h5>" + objlist[0].PracticeName + "</h5><br />";
                    if (objlist[0].ProviderAddress1 != null & objlist[0].ProviderAddress2 != null)
                    {

                        strhtml = strhtml + objlist[0].ProviderAddress1 + "," + "&nbsp;" + objlist[0].ProviderAddress2 + "&nbsp;&nbsp;<br />";
                    }
                    else
                    {
                        strhtml = strhtml + objlist[0].ProviderAddress1 + "" + objlist[0].ProviderAddress2 + "&nbsp;&nbsp;<br />";
                    }
                    if (objlist[0].ProviderCity != null & objlist[0].ProviderState != null)
                    {
                        strhtml = strhtml + objlist[0].ProviderCity + "," + "&nbsp;" + objlist[0].ProviderState + " " + objlist[0].ProviderZip + "&nbsp;&nbsp;<br />";
                    }
                    else
                    {
                        strhtml = strhtml + objlist[0].ProviderCity + "" + "&nbsp;" + objlist[0].ProviderState + " " + objlist[0].ProviderZip + "&nbsp;&nbsp;<br />";
                    }
                    if (objlist[0].Providerphone != null)
                    {
                        strhtml = strhtml + clsCommonFunctions.UsPhoneFormat(objlist[0].Providerphone) + "&nbsp;&nbsp;<br />";
                    }
                    else
                    {
                        strhtml = strhtml + "&nbsp;&nbsp;<br />";
                    }
                    strhtml = strhtml + "</td>";
                    strhtml = strhtml + "<td style='padding-right: 30px;' align='right' width='70%'>";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' border='0' bordercolor='#d3d3d3' width='40%'  align='right'>";
                    strhtml = strhtml + "<tr height='30px'>";
                    strhtml = strhtml + "<td align='left' style='font-family: Arial; color: #808080; font-size: 40px; font-weight: bold;'><font size='20px'  color='#808080'><b>Statement</b></font></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr bgcolor='white'>";
                    strhtml = strhtml + "<td align='left' style='font-family: Arial; font-size: 14px;'>Date:&nbsp;" + DateTime.Parse(objlist[0].FromDate.ToString()).ToShortDateString() + "</td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table></td></tr></table>";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' border='0' bordercolor='#d3d3d3' width='85%'  align='center'><tr><td>";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' border='1' bordercolor='#d3d3d3' width='35%'  align='left'>";
                    strhtml = strhtml + "<tr bgcolor='#EEEEEE' height='30px'>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' height='20px' valign='middle'>";
                    strhtml = strhtml + "&nbsp;" + "<b>Client :</b>";
                    strhtml = strhtml + "</td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr bgcolor='white' height='40px'>";
                    strhtml = strhtml + "<td>";
                    strhtml = strhtml + "&nbsp;&nbsp;" + objlist[0].PaidBy + "<br />";
                    strhtml = strhtml + "&nbsp;&nbsp;" + objlist[0].PaidByAddress1 + "<br />";
                    if (objlist[0].PaidByAddress2 != null)
                    {
                        strhtml = strhtml + "&nbsp;&nbsp;" + objlist[0].PaidByAddress2 + "<br />";
                    }
                    if (objlist[0].PaidByCity != null & objlist[0].PaidByState != null)
                    {
                        strhtml = strhtml + "&nbsp;&nbsp;" + objlist[0].PaidByCity + ", " + objlist[0].PaidByState + " " + objlist[0].PaidByZip + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "&nbsp;&nbsp;" + objlist[0].PaidByCity + "" + objlist[0].PaidByState + " " + objlist[0].PaidByZip + "</td>";
                    }
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table></td></tr></table><br/><br/>";
                    strhtml = strhtml + "<table cellspacing='0' align='center' bordercolor='#d3d3d3' cellpadding='0' width='85%' border='1'>";
                    strhtml = strhtml + "<tr bgcolor='#EEEEEE' height='20px'>";
                    strhtml = strhtml + "<td align='center' width='15%' height='22px' valign='middle'><b>Date</b></td>";
                    strhtml = strhtml + "<td align='center' width='22%' height='22px' valign='middle'><b>Transactions</b></td>";
                    strhtml = strhtml + "<td align='center' width='16%' height='22px' valign='middle'>&nbsp;<b>Charge</b></td>";
                    strhtml = strhtml + "<td align='center' width='16%' height='22px' valign='middle'>&nbsp;<b>Payment</b></td>";
                    strhtml = strhtml + "</tr>";
                    while (j < objlist.Count - 1)
                    {
                        strhtml = strhtml + "<tr>";
                        if (Convert.ToString(objlist[j].Transaction_Date) != "")
                        {
                            strhtml = strhtml + "<td align='center'>" + DateTime.Parse(Convert.ToString(objlist[j].Transaction_Date)).ToShortDateString() + "</td>";
                        }
                        else
                        {
                            strhtml = strhtml + "<td align='center'></td>";
                        }
                        strhtml = strhtml + "<td align='center'>" + objlist[j].ChargeType.Trim() + "</td>";

                        if (Convert.ToString(objlist[j].ChargeType.Trim()) == "Payment" || Convert.ToString(objlist[j].ChargeType.Trim()) == "Neg.Adjustment" || Convert.ToString(objlist[j].ChargeType.Trim()) == "Future credit" || Convert.ToString(objlist[j].ChargeType.Trim()) == "Write-Off" || Convert.ToString(objlist[j].ChargeType.Trim()) == "Refund via Check/Cash")
                        {
                            double Transaction_Amount = Convert.ToDouble(objlist[j].Transaction_Amount);
                            strhtml = strhtml + "<td></td><td align='Right'>" + "(" + Transaction_Amount.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";
                            Totalpayments += Convert.ToDouble(objlist[j].Transaction_Amount);
                        }
                        else if (Convert.ToString(objlist[j].ChargeType.Trim()) == "Charge & Payment")
                        {
                            double Transaction_Amount = Convert.ToDouble(objlist[j].Transaction_Amount);
                            strhtml = strhtml + "<td align='Right'>" + "(" + Transaction_Amount.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";
                            strhtml = strhtml + "<td align='Right'>" + "(" + Transaction_Amount.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";
                            Totalcharges += Convert.ToDouble(objlist[j].Transaction_Amount);
                            Totalpayments += Convert.ToDouble(objlist[j].Transaction_Amount);
                        }
                        else
                        {
                            double Transaction_Amount = Convert.ToDouble(objlist[j].Transaction_Amount);
                            strhtml = strhtml + "<td align='Right'>" + Transaction_Amount.ToString("C", CultureInfo.CurrentCulture) + "</td><td></td>";
                            //if (objlist[j].ChargeType.Trim().ToString() != "Charge & Payment")
                            //{
                            Totalcharges += Convert.ToDouble(objlist[j].Transaction_Amount);
                            //}
                        }
                        strhtml = strhtml + "</tr>";
                        j += 1;
                    }

                    strhtml = strhtml + "</table>";

                    strhtml = strhtml + "<table cellspacing='0' align='center' bordercolor='#d3d3d3' cellpadding='0' width='85%' border='0'>";
                    Totalbal = Totalcharges - Totalpayments;
                    strhtml = strhtml + "<tr height='25px'>";
                    strhtml = strhtml + "<td align='center' width='15%'></td>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' align='right' width='22%'>&nbsp;<b>Totals :</b></td>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' align='Right' width='16%'><b>" + Totalcharges.ToString("C", CultureInfo.CurrentCulture) + "</b></td>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' align='Right' width='16%'><b>" + Totalpayments.ToString("C", CultureInfo.CurrentCulture) + "</b></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr height='25px'>";
                    strhtml = strhtml + "<td align='center' width='15%'></td>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' align='right' width='22%'>&nbsp;<b>Balance :</b></td>";
                    strhtml = strhtml + "<td bgcolor='#EEEEEE' align='center' colspan='2'><b>" + Totalbal.ToString("C", CultureInfo.CurrentCulture) + "</b></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table><br/><br/><br/>";

                    DataSet dttable = new DataSet();
                    dttable = PatientBalance.Get_PatientresposiblepartyTransactions(PatientLoginID, ProviderID, ToreferenceID, ToreferencetypeID);
                    strhtml = strhtml + "<table cellspacing='0' align='center' bordercolor='#d3d3d3' cellpadding='0' width='85%' border='1'>";
                    strhtml = strhtml + "<tr bgcolor='#EEEEEE' height='20px'>";
                    strhtml = strhtml + "<td align='center' width='15%'> <b>0-30</b></td>";
                    strhtml = strhtml + "<td align='center' width='15%'> <b>31-60</b></td>";
                    strhtml = strhtml + "<td align='center' width='15%'> <b>61-90</b></td>";
                    strhtml = strhtml + "<td align='center' width='15%'> <b>91-120</b></td>";
                    strhtml = strhtml + "<td align='center' width='20%'> <b>Over 120 days</b></td>";
                    strhtml = strhtml + "<td align='center' width='20%'> <b>Amount due</b></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr>";
                    if (dttable.Tables[0].Rows.Count > 0)
                    {

                        double dueamount1 = 0;
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["0-30"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["0-30"]));
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";

                        }
                        else
                        {
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">$0.00</td>";
                        }
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["31-60"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["31-60"]));
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";

                        }
                        else
                        {
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">$0.00</td>";
                        }
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["61-90"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["61-90"]));
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";

                        }
                        else
                        {
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">$0.00</td>";
                        }
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["91-120"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["91-120"]));
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";

                        }
                        else
                        {
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">$0.00</td>";
                        }
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["over 120 days"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["over 120 days"]));
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";

                        }
                        else
                        {
                            strhtml = strhtml + "<td align=\"right\" width=\"15%\">$0.00</td>";
                        }
                        if (Convert.ToString(dttable.Tables[0].Rows[0]["Amount due"]) != "")
                        {
                            dueamount1 = Convert.ToDouble(Convert.ToString(dttable.Tables[0].Rows[0]["Amount due"]));
                            strhtml = strhtml + "<td align='right' width='20%'>" + dueamount1.ToString("C", CultureInfo.CurrentCulture) + "</td>";
                        }
                        else
                        {
                            strhtml = strhtml + "<td align='right' width='20%'>$0.00</td>";
                        }
                        strhtml = strhtml + "</tr>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td align='right' width='15%'>$0.00</td>";
                        strhtml = strhtml + "<td align='right' width='15%'>$0.00</td>";
                        strhtml = strhtml + "<td align='right' width='15%'>$0.00</td>";
                        strhtml = strhtml + "<td align='right' width='15%'>$0.00</td>";
                        strhtml = strhtml + "<td align='right' width='15%'>$0.00</td>";
                        strhtml = strhtml + "<td align='right' width='20%'>$0.00</td>";
                        strhtml = strhtml + "</tr>";
                    }
                    strhtml = strhtml + "</table>";
                    strhtml = strhtml + "<br/><br/><br/><br/><br/><br/><table cellspacing='0' align='center' bordercolor='#666666' cellpadding='0' width='85%' border='0'>";
                    strhtml = strhtml + "<tr style='background: #C0C0C0; height: 20px;'>";
                    strhtml = strhtml + "<td width='5%'></td>";
                    strhtml = strhtml + "<td align='center'>" + msg + "</td>";
                    strhtml = strhtml + "<td width='5%'></td>";
                    strhtml = strhtml + "</tr></table>";

                    //strhtml = strhtml + "</table>";
                    //strhtml = strhtml + "</td>";
                    //strhtml = strhtml + "</tr>";
                    //strhtml = strhtml + "</table>";
                    pdf.AddHTMLPos(300, 10, strhtml);
                }
                else
                {
                    ind = "Y";
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ClientsController", "ListPrint", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public FileContentResult PrintReceipt(string patientid, string patientlogin_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pdf.Create();
            //string message = null;
            //string fromdate = null;
            //string todate = null;
            //if (Request["ddlMessage"] != "0")
            //{
            //    if (Request["ddlMessage"] == "1")
            //    {
            //        message = "Thank you.";
            //    }
            //    else if (Request["ddlMessage"] == "2")
            //    {
            //        message = "Thank you for your prompt payment.";
            //    }
            //    else if (Request["ddlMessage"] == "3")
            //    {
            //        message = "Due upon receipt.";
            //    }
            //    else if (Request["ddlMessage"] == "4")
            //    {
            //        message = "This bill sent multiple times.";
            //    }
            //    else if (Request["ddlMessage"] == "5")
            //    {
            //        message = "Last bill before collection services.";
            //    }
            //    else
            //    {
            //        message = "Thank you for your payment.";
            //    }
            //}
            //else
            //{
            //    message = null;
            //}
            //if (Request["hdnchktype"] != null)
            //{
            //    str = Request["hdnchktype"];
            //}
            //if (Session["PatientID"] != null)
            //{
            ListPrint(patientid, "3", "", patientlogin_id, Convert.ToString(Session["Prov_ID"]), "Due upon receipt.", null, null);
            //}

            //if (ind != "Y")
            //{
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=Printbill.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            //}
            return null;
        }
        public ActionResult PrintSendBillNew(string patientid)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    if (Request["ProviderID"] != "" & Request["ProviderID"] != null)
                    {
                        Session.Add("Prov_ID", Request["ProviderID"]);
                        Session.Add("Practice_ID", Request["PracticeID"]);
                    }
                }
            }

            //clsCommonFunctions objcommon = new clsCommonFunctions();
            if (clscommon.SearchPatientExists(patientid, Convert.ToString(Session["Prov_ID"])).ToUpper() == "N")
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                string PatientLoginID = Billingreports.GetPatientLoginID(patientid);
                Session.Add("PatientLoginId", PatientLoginID);
                Session.Add("PatientID", patientid);
                List<Billingreports> objoptions = new List<Billingreports>();
                objoptions = Billingreports.Get_PrintBillMessages();
                IList<SelectListItem> PrintBill = new List<SelectListItem>();
                PrintBill.Add(new SelectListItem { Value = "0", Text = "--Select Message--", Selected = true });
                if (objoptions.Count > 0)
                {
                    for (int i = 0; i <= objoptions.Count - 1; i++)
                    {
                        PrintBill.Add(new SelectListItem
                        {
                            Value = Convert.ToString(objoptions[i].MessageOnBill_ID),
                            Text = Convert.ToString(objoptions[i].MessageOnBill)
                        });
                    }
                }

                ViewData["ddlMessage"] = PrintBill;
                return View();
            }
        }
        private void GetClientfile(string patientloginID)
        {
            //   /Clients/ClientFile
            try
            {
                Billingreports obj = new Billingreports();
                List<patients> objpatientslist = new List<patients>();
                patients objPatient = new patients();
                double charges = 0;
                double payments = 0;
                double posadj = 0;
                double negadj = 0;
                //  string EmergencyNo = null;
                string homePhone = null;
                string workPhone = null;
                string mobilePhone = null;
                obj.PatientLoginID = patientloginID;
                //obj.PracticeID = Session["Practice_ID"].ToString();
                obj.ProviderID = Convert.ToString(Session["Prov_ID"]);
                DataSet dsset = new DataSet();
                dsset = Billingreports.Get_Patientfiledetails(obj);
                if (dsset.Tables[0].Rows.Count > 0)
                {

                    if (Convert.ToString(dsset.Tables[0].Rows[0]["HomePhone"]) != "")
                    {
                        homePhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsset.Tables[0].Rows[0]["HomePhone"]));
                    }
                    else
                    {
                        homePhone = null;
                    }
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["WorkPhone"]) != "")
                    {
                        workPhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsset.Tables[0].Rows[0]["WorkPhone"]));
                    }
                    else
                    {
                        workPhone = null;
                    }
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["MobilePhone"]) != "")
                    {
                        mobilePhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsset.Tables[0].Rows[0]["MobilePhone"]));
                    }
                    else
                    {
                        mobilePhone = null;
                    }

                    string strhtml = null;
                    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='80%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td><b>Client name :" + Convert.ToString(dsset.Tables[0].Rows[0]["PatientName"]) + "</b></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table>";
                    strhtml = strhtml + "<br />";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='80%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td align='right' width='30%'><b>Mower helper :</b></td>";
                    strhtml = strhtml + "<td width='70%'>" + Convert.ToString(dsset.Tables[0].Rows[0]["DefaultProvidername"]) + "</td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table>";
                    strhtml = strhtml + "<br />";
                    strhtml = strhtml + "<br />";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='80%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td width='20%' align='right'><b>Date <br />Registered :</b></td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["Registereddate"]) != "")
                    {
                        strhtml = strhtml + "<td width='13%'>" + DateTime.Parse(Convert.ToString(dsset.Tables[0].Rows[0]["Registereddate"])).ToString("MM/dd/yyyy") + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td width='14%'>&nbsp;</td>";
                    }

                    strhtml = strhtml + "<td width='20%' align='right'><b>First appointment :</b></td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["FirstAppointment"]) != "")
                    {
                        strhtml = strhtml + "<td width='13%'>" + DateTime.Parse(Convert.ToString(dsset.Tables[0].Rows[0]["FirstAppointment"])).ToString("MM/dd/yyyy") + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td width='13%'>&nbsp;</td>";
                    }
                    strhtml = strhtml + "<td width='20%' align='right'><b>Last appointment :</b></td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["LastAppointment"]) != "")
                    {
                        strhtml = strhtml + "<td width='13%'>" + DateTime.Parse(Convert.ToString(dsset.Tables[0].Rows[0]["LastAppointment"])).ToString("MM/dd/yyyy") + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td width='13%'>&nbsp;</td>";
                    }
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table>";
                    strhtml = strhtml + "<br />";
                    strhtml = strhtml + "<br />";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["Charges"]) != "")
                    {
                        charges = Convert.ToDouble(dsset.Tables[0].Rows[0]["Charges"]);
                    }
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["Payments"]) != "")
                    {
                        payments = Convert.ToDouble(dsset.Tables[0].Rows[0]["Payments"]);
                    }
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["PosAdjustments"]) != "")
                    {
                        posadj = Convert.ToDouble(dsset.Tables[0].Rows[0]["PosAdjustments"]);
                    }
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["NegAdjustments"]) != "")
                    {
                        negadj = Convert.ToDouble(dsset.Tables[0].Rows[0]["NegAdjustments"]);
                    }
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='80%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td width='30%' align='right'><b>Account balance :</b></td>";
                    strhtml = strhtml + "<td width='35%'>" + (charges - payments + posadj - negadj).ToString("C", CultureInfo.CurrentCulture) + " as of " + DateTime.Parse(DateTime.Now.ToString()).ToString("MM/dd/yyyy") + "</td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table>";
                    strhtml = strhtml + "<br/>";
                    strhtml = strhtml + "<br/>";
                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='80%'>";
                    strhtml = strhtml + "<tr class='gray'>";
                    // strhtml = strhtml + "<td width='9%'></td>";
                    strhtml = strhtml + "<td align='center' width='25%' colspan='2'><b>Contacts</b></td>";
                    strhtml = strhtml + "<td align='center' width='41%'><b>Address</b></td>";
                    strhtml = strhtml + "<td align='center' width='25%' colspan='2'><b>Transactions</b></td>";
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr class='gray'>";
                    //  strhtml = strhtml + "<td width='9%'></td>";
                    strhtml = strhtml + "<td align='right'>Home :</td>";
                    strhtml = strhtml + "<td>" + homePhone + "</td>";
                    strhtml = strhtml + "<td>" + Convert.ToString(dsset.Tables[0].Rows[0]["Address1"]) + "</td>";
                    strhtml = strhtml + "<td align='right'>Charges :</td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["Charges"]) != "")
                    {
                        double strcharges;
                        strcharges = Convert.ToDouble(dsset.Tables[0].Rows[0]["Charges"]);
                        strhtml = strhtml + "<td>" + strcharges.ToString("C", CultureInfo.CurrentCulture) + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td>$0.00</td>";
                    }
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr class='gray'>";
                    // strhtml = strhtml + "<td width='9%'></td>";
                    strhtml = strhtml + "<td align='right'>Work :</td>";
                    strhtml = strhtml + "<td>" + workPhone + "</td>";
                    strhtml = strhtml + "<td>" + Convert.ToString(dsset.Tables[0].Rows[0]["Address2"]) + "</td>";
                    strhtml = strhtml + "<td align='right'>Payments :</td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["Payments"]) != "")
                    {
                        double strPayments;
                        strPayments = Convert.ToDouble(dsset.Tables[0].Rows[0]["Payments"]);
                        strhtml = strhtml + "<td>" + "(" + strPayments.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";

                    }
                    else
                    {
                        strhtml = strhtml + "<td>$0.00</td>";
                    }
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr class='gray'>";
                    // strhtml = strhtml + "<td width='9%'></td>";
                    strhtml = strhtml + "<td align='right'>Cell :</td>";
                    strhtml = strhtml + "<td>" + mobilePhone + "</td>";
                    if (Convert.ToString(dsset.Tables[0].Rows[0]["City"]) != "")
                    {
                        strhtml = strhtml + "<td>" + Convert.ToString(dsset.Tables[0].Rows[0]["City"]) + "," + Convert.ToString(dsset.Tables[0].Rows[0]["State"]) + " - " + Convert.ToString(dsset.Tables[0].Rows[0]["Zip"]) + "</td>";
                    }
                    else
                    {
                        strhtml = strhtml + "<td></td>";
                    }
                    //strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td align='right'><b>Balance :</b></td>";
                    strhtml = strhtml + "<td>" + (charges - payments + posadj - negadj).ToString("C", CultureInfo.CurrentCulture) + "</td>";
                    //strhtml = strhtml + "</tr>";
                    //strhtml = strhtml + "<td align='right'>Adjustments(-) :</td>";
                    //if (dsset.Tables[0].Rows[0]["NegAdjustments"].ToString() != "")
                    //{
                    //    double strnegadjust = Convert.ToDouble(dsset.Tables[0].Rows[0]["NegAdjustments"]);

                    //    strhtml = strhtml + "<td>" + "(" + strnegadjust.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";
                    //}
                    //else
                    //{
                    //    strhtml = strhtml + "<td>$0.00</td>";
                    //}
                    strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "<tr class='gray'>";
                    // strhtml = strhtml + "<td width='9%'></td>";
                    strhtml = strhtml + "<td align='right'>Email :</td>";
                    strhtml = strhtml + "<td colspan='2'>" + Convert.ToString(dsset.Tables[0].Rows[0]["Email"]) + "</td>";

                    //strhtml = strhtml + "<td align='right'>Adjustments(+) :</td>";
                    //if (dsset.Tables[0].Rows[0]["PosAdjustments"].ToString() != "")
                    //{
                    //    double strposadjust = Convert.ToDouble(dsset.Tables[0].Rows[0]["PosAdjustments"]);

                    //    strhtml = strhtml + "<td>" + "(" + strposadjust.ToString("C", CultureInfo.CurrentCulture) + ")" + "</td>";
                    //}
                    //else
                    //{
                    //    strhtml = strhtml + "<td>$0.00</td>";
                    //}

                    strhtml = strhtml + "</tr>";
                    //strhtml = strhtml + "<tr class='gray'>";
                    //strhtml = strhtml + "<td colspan='5' align='right'><b>Balance :</b></td>";
                    //strhtml = strhtml + "<td>" + (charges - payments + posadj - negadj).ToString("C", CultureInfo.CurrentCulture) + "</td>";
                    //strhtml = strhtml + "</tr>";
                    strhtml = strhtml + "</table>";

                    if (j != 0)
                    {
                        double height1;
                        height1 = pdf.GetTextHeight(strhtml);
                        height += height1;
                        pdf.AddHTMLPos(pos3, height, strhtml);
                    }
                    else
                    {
                        height = pdf.GetTextHeight(strhtml);
                        pdf.AddHTMLPos(pos3, pos4, strhtml);
                    }
                    j = j + 1;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "GetClientfile", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        private Patient_Info GetClientInfo()
        {
            try
            {
                Patient_Info obj = new Patient_Info();
                //obj.PracticeID = Session["Practice_ID"].ToString();
                obj.PatientID = Convert.ToString(Session["PatientID"]);
                obj.ProviderID = Convert.ToString(Session["Prov_ID"]);
                obj = Patient_Info.ViewPatineInfo(obj);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.PatientLoginID))
                    {
                        Session.Add("PatientLoginID", obj.PatientLoginID);
                    }
                    Session.Add("Patname", obj.Patientname);
                    //  string domain = null;                    
                    //Session.Add("hdnclientEmail", obj.PatientEmail != "" ? obj.PatientEmail : null);
                    if (!string.IsNullOrEmpty(obj.PatientEmail))
                    {
                        Session.Add("hdnclientEmail", obj.PatientEmail);
                    }
                    else
                    {
                        Session.Add("hdnclientEmail", null);
                    }
                    ViewBag.googleaddress = obj.l_GoogleMapPath.Replace("+", ",");
                    if (obj.WorkPhone != null & obj.WorkPhone != "")
                    {
                        obj.IsHomephoneMain_Ind = "2";
                    }
                    if (obj.HomePhone != "")
                    {
                        obj.IsHomephoneMain_Ind = "1";
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "GetClientInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult EditClientFile()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //GetClientInfo();
            
            if (Request["Imagetype"] == "I")
            {
                ViewBag.Imagetype = "I";
            }
            if (Request["lbloutmsg"] == "Y")
            {
                ViewBag.outmsg = "Y";
            }
            return View("EditClientFile", GetClientInfo());
        }
        //private string RandomPassword()
        //{
        //    return Convert.ToString(System.Guid.NewGuid());

        //}
        //private bool Getfilename(HttpPostedFileBase flPhoto)
        //{
        //    try
        //    {
        //        string strFilePath = null;
        //        ViewData["LogoName"] = System.IO.Path.GetFileNameWithoutExtension(flPhoto.FileName);
        //        ViewData["FileExtn1"] = System.IO.Path.GetExtension(flPhoto.FileName);
        //        ViewData["EncryptLogo"] = clsCommonCClist.RandomPassword();
        //        string filename = Convert.ToString(ViewData["EncryptLogo"]) + Convert.ToString(ViewData["FileExtn1"]);
        //        strFilePath = Path.Combine(Server.MapPath("~/Attachments/Patients"), filename);
        //        ViewData["LogoPath"] = Convert.ToString(ViewData["EncryptLogo"]) + Convert.ToString(ViewData["FileExtn1"]);
        //        flPhoto.SaveAs(strFilePath);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Getfilename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return false;
        //    }

        //}
        [HttpPost()]
        [ValidateAntiForgeryToken()]

        public JsonResult EditClientFile(Patient_Info obj, HttpPostedFileBase flPhoto)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            obj.ZIP = Request["txtZip"] != "" ? Request["txtZip"] : null;
            obj.State_ID = Request["DDState"] != "" ? Request["DDState"] : null;
            obj.City_ID = Request["DDCity"] != "" ? Request["DDCity"] : null;
            if (Request["txtCellPhone1"] != "" & Request["txtCellPhone2"] != "" & Request["txtCellPhone3"] != "")
            {
                obj.MobilePhone = Request["txtCellPhone1"] + Request["txtCellPhone2"] + Request["txtCellPhone3"];
                //Session["ClientMobilePhone"] = obj.MobilePhone;
            }
            if (Request["txtHomePhone1"] != "" & Request["txtHomePhone2"] != "" & Request["txtHomePhone3"] != "")
            {
                obj.HomePhone = Request["txtHomePhone1"] + Request["txtHomePhone2"] + Request["txtHomePhone3"];

            }
            if (Request["txtWorkPhone1"] != "" & Request["txtWorkPhone2"] != "" & Request["txtWorkPhone3"] != "")
            {
                obj.WorkPhone = Request["txtWorkPhone1"] + Request["txtWorkPhone2"] + Request["txtWorkPhone3"];

            }
            Session.Add("hdnclientEmail", !string.IsNullOrEmpty(obj.PatientEmail) ? obj.PatientEmail : null);
            string beforeAddress = Request["hdnbeforeAddress"];
            string CustEaddress = obj.Address1 + " " + Request["hdnESelectState"] + " " + Request["hdnESelectCity"] + " " + Request["txtZip"];

            if ((beforeAddress != CustEaddress) | (string.IsNullOrEmpty(obj.GmapLatitude)) & (string.IsNullOrEmpty(obj.GmapLongitude)))
            {

                try
                {
                    string urlPatient = "http://maps.google.com/maps/api/geocode/xml?address=" + CustEaddress + "&sensor=false";
                    WebRequest request = WebRequest.Create(urlPatient);
                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            DataSet dsResult = new DataSet();
                            dsResult.ReadXml(reader);
                            DataRow location = null;
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = Convert.ToString(dsResult.Tables["geometry"].Select("result_id = " + Convert.ToString(row["result_id"]))[0]["geometry_id"]);
                                location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                //dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);                             
                            }
                            obj.GmapLatitude = Convert.ToString(location["lat"]);
                            obj.GmapLongitude = Convert.ToString(location["lng"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    obj.GmapLatitude = null;
                    obj.GmapLongitude = null;
                }
            }
            else
            {
                obj.GmapLatitude = !string.IsNullOrEmpty(obj.GmapLatitude) ? obj.GmapLatitude : null;
                obj.GmapLongitude = !string.IsNullOrEmpty(obj.GmapLongitude) ? obj.GmapLongitude : null;
            }

            string Out_Msg = null;
            //string Out_login_id = null;
            obj.Ind = !string.IsNullOrEmpty(Request["EHdnInd"]) ? Request["EHdnInd"] : "N";
            Patient_Info.EditPatientPersonalInfo(obj, ref Out_Msg);
            
            if (!string.IsNullOrEmpty(Out_Msg) && Out_Msg.Contains("Do you want to update"))
            {
                return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
            }

            //if (Patient_Info.EditPatientPersonalInfo(obj) != null)
            //{
            //    return RedirectToAction("EditClientFile", new { lbloutmsg = "Y" });
            //}
            //else
            //{
            //    return RedirectToAction("ClientFile");
            //}
            //return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
        }


        #region "Patient fileTransactions
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult TransactionsList(string dt_filter, string page, string ibtnSearch, string pid, int? PatientLoginID, string hdnclientEmail = null)
        {
            if (!string.IsNullOrEmpty(hdnclientEmail))
            {
                if (hdnclientEmail.Length > 1)
                {
                    hdnclientEmail = hdnclientEmail.Remove(hdnclientEmail.Length - 1, 1);
                }
            }
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            RPBilling objBill = new RPBilling();
            objBill.Customer_Email = hdnclientEmail;
            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    if (!string.IsNullOrEmpty(Request["ProviderID"]))
                    {
                        Session.Add("Prov_ID", Request["ProviderID"]);
                        //Session.Add("Practice_ID", Request["PracticeID"]);
                    }
                }
            }
            //ViewBag.adjsort = Request["g2sort"] == null ? "Transaction_Date" : Request["g2sort"].ToString();

            //ViewBag.adjsortdir = Request["g2sortdir"] == null ? "DESC" : Request["g2sortdir"].ToString();
            //if (Request["frm"] == "ph")
            //{
            //    // ViewBag.frm = "ph";
            //}
            if (!string.IsNullOrEmpty(Request["pname"]))
            {
                ViewBag.Pname = Request["pname"];
                Session["Pname"] = Request["pname"];
                Session["Patname"] = Request["pname"];
            }
            else
            {
                ViewBag.Pname = Session["Pname"];
                //Session["Patname"] = Session["Patname"];
            }
            if (pid != null)
            {
                Session["PatientID"] = pid;
            }
            if (PatientLoginID != null & PatientLoginID != 0)
            {
                Session["PatientLoginID"] = PatientLoginID;
            }
            if (Session["PatientLoginID"] != null)
            {
                PatientLoginID = Convert.ToInt32(Session["PatientLoginID"]);
            }

            //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            /**************************DateSearch Functionality*****************
            if (dt_filter == "7")
            {
                Strdate = DateTime.Now.ToShortDateString();
                objBill.ToDate = DateTime.Now.ToShortDateString();
                objBill.FromDate = DateTime.Parse(Strdate).AddDays(-6).ToShortDateString();
                ViewBag.setDt = "7";
                ViewBag.fromdate = objBill.FromDate;
                ViewBag.todate = objBill.ToDate;
            }
            else if (dt_filter == "30")
            {
                Strdate = DateTime.Now.ToShortDateString();
                objBill.ToDate = DateTime.Now.ToShortDateString();
                objBill.FromDate = DateTime.Parse(Strdate).AddDays(-29).ToShortDateString();
                ViewBag.setDt = "30";
                ViewBag.fromdate = objBill.FromDate;
                ViewBag.todate = objBill.ToDate;
            }
            else if (dt_filter == "Today")
            {
                objBill.ToDate = DateTime.Now.ToShortDateString();
                objBill.FromDate = DateTime.Now.ToShortDateString();
                ViewBag.setDt = "Today";
                ViewBag.fromdate = objBill.FromDate;
                ViewBag.todate = objBill.ToDate;
            }
            else if (dt_filter == "Custom")
            {
                objBill.FromDate = Request["txt_FromDate"];
                objBill.ToDate = Request["txt_ToDate"];
                ViewBag.setDt = "Custom";
                ViewBag.fromdate = objBill.FromDate;
                ViewBag.todate = objBill.ToDate;
            }
            else
            {
                objBill.FromDate = null;
                objBill.ToDate = null;
                ViewBag.setDt = null;
                ViewBag.fromdate = null;
                ViewBag.todate = null;
            }
             *********************************************************************/
            //    RPBilling patinfo = new RPBilling();
            //    clsCommonFunctions objcommon = new clsCommonFunctions();
            //    IDataParameter[] InParamList = {
            //    new SqlParameter("@In_Patient_Ids", Session["PatientID"]),
            //    new SqlParameter("@iN_pROVIDER_ID", null)
            //};
            //    objcommon.AddInParameters(InParamList);
            //    DataSet objds = new DataSet();
            //    objds = objcommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
            //    if (objds.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in objds.Tables[0].Rows)
            //        {
            //            patinfo.PatientLogin_ID = Convert.ToInt32(dr["PatientLogin_ID"]);
            //        }
            //    }
            objBill.PatientLogin_ID = PatientLoginID;// patinfo.PatientLogin_ID;
            //// ViewBag.patlgnid = PatientLoginID;// patinfo.PatientLogin_ID;
            //objBill.Reference_id = Convert.ToInt32(Session["Prov_ID"]);
            //DataSet dsbill = new DataSet();
            //dsbill = RPBilling.PracticePatientIncome(objBill);
            //StringBuilder strbill = new StringBuilder();
            //strbill = strbill.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            //strbill = strbill.Append(" <td align='center' width='25%'><strong>Billing summary&nbsp;:</strong></td><td align='center' width='25%'><strong>Total charges</strong></td><td align='center' width='25%'><strong>Total payments</strong></td><td align='center' width='25%'><strong>Balance</strong></td></tr>");
            //foreach (DataRow dr in dsbill.Tables[0].Rows)
            //{
            //    string StrTCharge = string.Format("{0:c}", dr["TotalCharges"]);
            //    string strTPay = string.Format("{0:c}", dr["Netpayments"]);
            //    string strBal = string.Format("{0:c}", dr["Balance"]);
            //    double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
            //    string strCredit = string.Format("{0:c}", dblcredit);
            //    strbill = strbill.Append("<tr><td align='center' width='25%'><strong>Amount&nbsp;:</strong></td><td align='center' width='25%'>" + StrTCharge + " </td><td align='center' width='25%'>" + strTPay + "</td><td align='center' width='25%'>" + strBal + "</td></tr>");
            //}
            //strbill = strbill.Append("</table>");
            //ViewBag.patbillSummary = strbill;
            //// int patlogin = Convert.ToInt32(PatientLoginID);
            //DataSet dsdayout = objBill.getPatientFileDayout(Convert.ToInt32(PatientLoginID), Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
            //StringBuilder strdays = new StringBuilder();
            //strdays = strdays.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            //strdays = strdays.Append("<td align='center' width='20%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0-30</strong></td><td align='center' width='10%'><strong>31-60</strong></td><td align='center' width='10%'><strong>61-90</strong></td><td align='center' width='10%'><strong>91-120</strong></td><td align='center' width='20%'><strong>Over 120 days</strong></td><td align='center' width='20%'><strong>Total due</strong></td></tr>");
            //if (dsdayout.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
            //    {
            //        string str0_30 = string.Empty;
            //        string str31_60 = string.Empty;
            //        string str61_90 = string.Empty;
            //        string str91_120 = string.Empty;
            //        string strOver120 = string.Empty;
            //        double strAm30;
            //        double strAm60;
            //        double strAm90;
            //        double strAm120;
            //        double strAm150;
            //        if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
            //        {
            //            str0_30 = string.Format("{0:c}", dr1["0-30"]);
            //            strAm30 = Convert.ToDouble(dr1["0-30"]);
            //        }
            //        else
            //        {
            //            str0_30 = "$0.00";
            //            strAm30 = 0;

            //        }
            //        if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
            //        {
            //            str31_60 = string.Format("{0:c}", dr1["31-60"]);
            //            strAm60 = Convert.ToDouble(dr1["31-60"]);
            //        }
            //        else
            //        {
            //            str31_60 = "$0.00";
            //            strAm60 = 0;
            //        }
            //        if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
            //        {
            //            str61_90 = string.Format("{0:c}", dr1["61-90"]);
            //            strAm90 = Convert.ToDouble(dr1["61-90"]);
            //        }
            //        else
            //        {
            //            str61_90 = "$0.00";
            //            strAm90 = 0;
            //        }
            //        if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
            //        {
            //            str91_120 = string.Format("{0:c}", dr1["91-120"]);
            //            strAm120 = Convert.ToDouble(dr1["91-120"]);
            //        }
            //        else
            //        {
            //            str91_120 = "$0.00";
            //            strAm120 = 0;
            //        }
            //        if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
            //        {
            //            strOver120 = string.Format("{0:c}", dr1["over 120 days"]);
            //            strAm150 = Convert.ToDouble(dr1["over 120 days"]);
            //        }
            //        else
            //        {
            //            strOver120 = "$0.00";
            //            strAm150 = 0;
            //        }




            //        double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
            //        string amountdue = string.Format("{0:c}", amountdue1);
            //        objBill.BalanceAmount = amountdue;
            //        strdays = strdays.Append("<tr><td align='center' width='20%'><strong>Amount due&nbsp;:</strong></td><td align='center' width='10%'>" + str0_30 + "</td><td align='center' width='10%'>" + str31_60 + "</td><td align='center' width='10%'>" + str61_90 + "</td><td align='center' width='10%'>" + str91_120 + "</td><td align='center' width='20%'>" + strOver120 + "</td><td align='center' width='20%'>" + amountdue + "</td></tr>");

            //    }
            //}
            //else
            //{
            //}
            //strdays = strdays.Append("</table>");
            //ViewBag.patdayout = Convert.ToString(strdays);
            ////dynamic chglistgrid = getChglist(null, patinfo.PatientLogin_ID.ToString(), objBill.FromDate, objBill.ToDate);
            
            //objBill.BillingTransList = gettransactionlist(Request["ddlrecords"], Convert.ToString(Session["PatientID"]), objBill.FromDate, objBill.ToDate, Request["page"], Request["sort"], Request["sortdir"]);
            objBill.PatientID = Convert.ToString(Session["PatientID"]);

            ////if (!string.IsNullOrEmpty(clientphone))
            ////{
            ////    Session["ClientMobilePhone"] = clientphone;
            ////    //TempData["clientphone"] = clientphone;
            ////    //TempData.Keep("clientphone");

            ////}
            //if (!string.IsNullOrEmpty(hdnclientEmail))
            //{
            //    if (hdnclientEmail == "1")
            //    {
            //        hdnclientEmail = null;
            //    }
            //    ViewBag.hdnclientEmail = hdnclientEmail;
            //    Session.Add("hdnclientEmail", hdnclientEmail);
            //}
            //else if (Session["hdnclientEmail"] != null)
            //{
            //    ViewBag.hdnclientEmail = Session["hdnclientEmail"];
            //}
            return View(objBill);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult TransListPartial(RPBilling objBill)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (pid != null)
                //{
                //    Session["PatientID"] = pid;
                //}
                //if (PatientLoginID != null & PatientLoginID != 0)
                //{
                //    Session["PatientLoginID"] = PatientLoginID;
                //}
                //if (Session["PatientLoginID"] != null)
                //{
                //    PatientLoginID = Convert.ToInt32(Session["PatientLoginID"]);
                //}
                //objBill.PatientLogin_ID = PatientLoginID;// patinfo.PatientLogin_ID;
                // ViewBag.patlgnid = PatientLoginID;// patinfo.PatientLogin_ID;
                objBill.Reference_id = Convert.ToInt32(Session["Prov_ID"]);
                DataSet dsbill = new DataSet();
                dsbill = RPBilling.PracticePatientIncome(objBill);
                StringBuilder strbill = new StringBuilder();
                strbill = strbill.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
                strbill = strbill.Append(" <td align='center' width='25%'><strong>Billing summary&nbsp;:</strong></td><td align='center' width='25%'><strong>Total charges</strong></td><td align='center' width='25%'><strong>Total payments</strong></td><td align='center' width='25%'><strong>Balance</strong></td></tr>");
                foreach (DataRow dr in dsbill.Tables[0].Rows)
                {
                    string StrTCharge = string.Format("{0:c}", dr["TotalCharges"]);
                    string strTPay = string.Format("{0:c}", dr["Netpayments"]);
                    string strBal = string.Format("{0:c}", dr["Balance"]);
                    double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
                    string strCredit = string.Format("{0:c}", dblcredit);
                    strbill = strbill.Append("<tr><td align='center' width='25%'><strong>Amount&nbsp;:</strong></td><td align='center' width='25%'>" + StrTCharge + " </td><td align='center' width='25%'>" + strTPay + "</td><td align='center' width='25%'>" + strBal + "</td></tr>");
                }
                strbill = strbill.Append("</table>");
                ViewBag.patbillSummary = strbill;
                // int patlogin = Convert.ToInt32(PatientLoginID);
                DataSet dsdayout = objBill.getPatientFileDayout(Convert.ToInt32(objBill.PatientLogin_ID), Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
                StringBuilder strdays = new StringBuilder();
                strdays = strdays.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
                strdays = strdays.Append("<td align='center' width='20%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0-30</strong></td><td align='center' width='10%'><strong>31-60</strong></td><td align='center' width='10%'><strong>61-90</strong></td><td align='center' width='10%'><strong>91-120</strong></td><td align='center' width='20%'><strong>Over 120 days</strong></td><td align='center' width='20%'><strong>Total due</strong></td></tr>");
                if (dsdayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
                    {
                        string str0_30 = string.Empty;
                        string str31_60 = string.Empty;
                        string str61_90 = string.Empty;
                        string str91_120 = string.Empty;
                        string strOver120 = string.Empty;
                        double strAm30;
                        double strAm60;
                        double strAm90;
                        double strAm120;
                        double strAm150;
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
                        {
                            str0_30 = Convert.ToDouble(dr1["0-30"]) > 0 ? string.Format("{0:c}", dr1["0-30"]) : "$0.00";
                            strAm30 = Convert.ToDouble(dr1["0-30"]);
                        }
                        else
                        {
                            str0_30 = "$0.00";
                            strAm30 = 0;

                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
                        {
                            str31_60 = Convert.ToDouble(dr1["31-60"]) > 0 ? string.Format("{0:c}", dr1["31-60"]) : "$0.00";
                            strAm60 = Convert.ToDouble(dr1["31-60"]);
                        }
                        else
                        {
                            str31_60 = "$0.00";
                            strAm60 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
                        {
                            str61_90 = Convert.ToDouble(dr1["61-90"]) > 0 ? string.Format("{0:c}", dr1["61-90"]) : "$0.00";
                            strAm90 = Convert.ToDouble(dr1["61-90"]);
                        }
                        else
                        {
                            str61_90 = "$0.00";
                            strAm90 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
                        {
                            str91_120 = Convert.ToDouble(dr1["91-120"]) > 0 ? string.Format("{0:c}", dr1["91-120"]) : "$0.00";
                            strAm120 = Convert.ToDouble(dr1["91-120"]);
                        }
                        else
                        {
                            str91_120 = "$0.00";
                            strAm120 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
                        {
                            strOver120 = Convert.ToDouble(dr1["over 120 days"]) > 0 ? string.Format("{0:c}", dr1["over 120 days"]) : "$0:00";
                            strAm150 = Convert.ToDouble(dr1["over 120 days"]);
                        }
                        else
                        {
                            strOver120 = "$0.00";
                            strAm150 = 0;
                        }




                        double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
                        string amountdue = string.Format("{0:c}", amountdue1);
                        objBill.BalanceAmount = amountdue;
                        strdays = strdays.Append("<tr><td align='center' width='20%'><strong>Amount due&nbsp;:</strong></td><td align='center' width='10%'>" + str0_30 + "</td><td align='center' width='10%'>" + str31_60 + "</td><td align='center' width='10%'>" + str61_90 + "</td><td align='center' width='10%'>" + str91_120 + "</td><td align='center' width='20%'>" + strOver120 + "</td><td align='center' width='20%'>" + amountdue + "</td></tr>");

                    }
                }
                else
                {
                }
                strdays = strdays.Append("</table>");
                ViewBag.patdayout = Convert.ToString(strdays);
                //dynamic chglistgrid = getChglist(null, patinfo.PatientLogin_ID.ToString(), objBill.FromDate, objBill.ToDate);
                objBill.OrderbyItem = Request["sort"] == null ? "Transaction_Date" : Request["sort"];
                objBill.orderBy = Request["sortdir"] == null ? "DESC" : Request["sortdir"];
                objBill.PageNo = Request["page"] == null ? "1" : Request["page"];
                objBill.BillingTransList = gettransactionlist(Request["ddlrecords"], Convert.ToString(Session["PatientID"]), objBill.FromDate, objBill.ToDate, objBill.PageNo, objBill.OrderbyItem, objBill.orderBy);
                objBill.PatientID = Convert.ToString(Session["PatientID"]);

                //if (!string.IsNullOrEmpty(clientphone))
                //{
                //    Session["ClientMobilePhone"] = clientphone;
                //    //TempData["clientphone"] = clientphone;
                //    //TempData.Keep("clientphone");

                //}
                if (!string.IsNullOrEmpty(objBill.Customer_Email) && objBill.Customer_Email != "null")
                {
                    if (objBill.Customer_Email == "1")
                    {
                        objBill.Customer_Email = null;
                    }
                    ViewBag.hdnclientEmail = objBill.Customer_Email;
                    Session.Add("hdnclientEmail", objBill.Customer_Email);
                }
                else if (Session["hdnclientEmail"] != null)
                {
                    ViewBag.hdnclientEmail = Session["hdnclientEmail"];
                }
                return PartialView("_ClientTransList", objBill);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public List<RPBilling> gettransactionlist(string ddlrecords, string PatientID, string fdate, string tdate, string page, string sort, string sortdir)
        {
            try
            {
                RPBilling objtran = new RPBilling();
                List<RPBilling> chglist = new List<RPBilling>();
                objtran.PatientID = PatientID;
                objtran.FromDate = fdate;
                objtran.ToDate = tdate;
                objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                objtran.NoOfRecords = ddlrecords == null ? "10" : ddlrecords;
                objtran.PageNo = page == null ? "1" : page;
                objtran.OrderbyItem = sort == null ? "Transaction_Date" : sort;
                objtran.orderBy = sortdir == null ? "DESC" : sortdir;
                chglist = objtran.GetPatienttransactionlist(objtran);
                ViewBag.totrec = chglist.Count > 0 ? RPBilling.Totalnoofrecords : 0;
                return chglist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "gettransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }


        public FileContentResult Transactionpdf(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            pdf.Create();
            StringBuilder strhtml = new StringBuilder();
            pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
            Getpdf(fromdate, todate, hdnPid, sort, sortdir, noofrecords);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=Charges.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            return null;
        }
        private void Getpdf(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            try
            {

                RPBilling objtran = new RPBilling();
                List<RPBilling> chglist = new List<RPBilling>();
                if (fromdate == "")
                {
                    fromdate = null;
                }
                if (todate == "")
                {
                    todate = null;
                }
                objtran.PatientID = hdnPid;
                objtran.FromDate = fromdate;
                objtran.ToDate = todate;
                objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                objtran.NoOfRecords = null;
                objtran.PageNo = "1";
                objtran.OrderbyItem = sort;
                objtran.orderBy = sortdir;
                chglist = objtran.GetPatienttransactionlist(objtran);






                WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
                string strhtml = null;
                strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td >Transaction list</td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "</table>";
                if (chglist.Count > 0)
                {

                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";

                    strhtml = strhtml + "<tr>";
                    strhtml = strhtml + "<td><span>Transaction date</span></td>";
                    strhtml = strhtml + "<td><span>Appointment date</span></td>";
                    strhtml = strhtml + "<td><span>Payment mode</span></td>";
                    strhtml = strhtml + "<td><span>Notes</span></td>";
                    strhtml = strhtml + "<td><span>Charge</span></td>";
                    strhtml = strhtml + "<td><span>Payment</span></td>";
                    strhtml = strhtml + "</tr>";
                    foreach (var item in chglist)
                    {
                        string date1 = item.Transaction_Date;
                        string[] date = date1.Split(' ');
                        strhtml = strhtml + "<tr >";
                        strhtml = strhtml + "<td><b>" + item.Transaction_Date + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.apptdate + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.TotalCharges + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.TotalPayments + "</b></td>";
                        strhtml = strhtml + "</tr>";

                    }
                    strhtml = strhtml + "</table>";
                }

                // height = pdf.GetTextHeight(strhtml);
                double pos3 = 300;
                pdf.AddHTMLPos(pos3, 10, strhtml);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Getpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        public void Transactionexcel(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            try
            {
                RPBilling objtran = new RPBilling();
                List<RPBilling> chglist = new List<RPBilling>();
                if (fromdate == "")
                {
                    fromdate = null;
                }
                if (todate == "")
                {
                    todate = null;
                }
                objtran.PatientID = hdnPid;
                objtran.FromDate = fromdate;
                objtran.ToDate = todate;
                objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                objtran.NoOfRecords = null;
                objtran.PageNo = "1";
                objtran.OrderbyItem = sort;
                objtran.orderBy = sortdir;
                chglist = objtran.GetPatienttransactionlist(objtran);






                WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
                string strhtml = null;
                strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "<td >Transaction list</td>";
                strhtml = strhtml + "<td ></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "</table>";
                if (chglist.Count > 0)
                {

                    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";

                    strhtml = strhtml + "<tr>";
                    strhtml = strhtml + "<td><span>Transaction date</span></td>";
                    strhtml = strhtml + "<td><span>Appointment date</span></td>";
                    strhtml = strhtml + "<td><span>Payment mode</span></td>";
                    strhtml = strhtml + "<td><span>Notes</span></td>";
                    strhtml = strhtml + "<td><span>Charge</span></td>";
                    strhtml = strhtml + "<td><span>Payment</span></td>";
                    strhtml = strhtml + "</tr>";
                    foreach (var item in chglist)
                    {
                        string date1 = item.Transaction_Date;
                        string[] date = date1.Split(' ');
                        strhtml = strhtml + "<tr >";
                        strhtml = strhtml + "<td><b>" + item.Transaction_Date + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.apptdate + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.TotalCharges + "</b></td>";
                        strhtml = strhtml + "<td><b>" + item.TotalPayments + "</b></td>";
                        strhtml = strhtml + "</tr>";

                    }
                    strhtml = strhtml + "</table>";
                }
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=charges.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(strhtml);
                Response.End();
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Transactionexcel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult CustomerTransactions(string PId, string frm)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}

                try
                {
                    RPBilling patinfo = new RPBilling();
                    patinfo.ToReference_ID = !string.IsNullOrEmpty(PId) ? PId : Convert.ToString(Session["PatientID"]);
                    patinfo.formanme = frm;
                    IDataParameter[] InParamList = {
            new SqlParameter("@In_Patient_Ids",patinfo.ToReference_ID),
           // new SqlParameter("@in_Practice_ID", Session["PracticeID"]),
            new SqlParameter("@iN_pROVIDER_ID", null)
        };
                    clscommon.AddInParameters(InParamList);
                    SqlDataReader ObjReader = clscommon.GetDataReader("Help_dbo.st_Scheduling_Get_PatientDetails");
                    List<GetPatientDetails> objGetPatientDetailslist = new List<GetPatientDetails>();
                    while (ObjReader.Read())
                    {
                        GetPatientDetails objGetPatientDetails = new GetPatientDetails(Convert.ToInt32(ObjReader["Patient_ID"]), Convert.ToInt32(ObjReader["PatientLogin_ID"]), Convert.ToString(ObjReader["PatientName"]), Convert.ToInt32(ObjReader["Duration"]), Convert.ToString(ObjReader["PatientEmail"]), Convert.ToString(ObjReader["DefaultCourtLocation_ID"]));
                        objGetPatientDetails.cellphone = Convert.ToString(ObjReader["patient_cellphoneno"]);
                        //objGetPatientDetails.PracticeID = Convert.ToString(ObjReader["patient_cellphoneno"]);
                        Session["Prov_ID"] = Convert.ToString(ObjReader["pROVIDER_ID"]);
                        patinfo.PatientLogin_ID = Convert.ToInt32(ObjReader["PatientLogin_ID"]);
                        objGetPatientDetailslist.Add(objGetPatientDetails);
                    }
                    patinfo.custname = objGetPatientDetailslist[0].PatientName;
                    patinfo.Customer_Email = objGetPatientDetailslist[0].PatientEmail;
                    patinfo.clientphone = objGetPatientDetailslist[0].cellphone;

                    patinfo.Reference_id = !string.IsNullOrEmpty(PId) ? Convert.ToInt32(PId) : Convert.ToInt32(Session["PatientID"]);// Convert.ToInt32(Session["PatientID"]);
                    patinfo.ReferenceType_ID = "3";
                    //if (Session["roleid"].ToString() != "1")
                    //{
                    patinfo.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                    //}
                    //else
                    //{
                    //    patinfo.Provider_id = Convert.ToInt32(proid);
                    //}

                    patinfo.BillingTransList = patinfo.getUnpaidBalance(patinfo);
                    patinfo.BalanceAmount = patinfo.BillingTransList[0].BalanceAmount;

                    //RPBilling clspayMode = new RPBilling();
                    //List<RPBilling> listPaymode = new List<RPBilling>();
                    //listPaymode = patinfo.GetPaymentModes();
                    //IList<SelectListItem> paymentModeList = new List<SelectListItem>();
                    //if (listPaymode.Count > 0)
                    //{
                    //    for (int i = 0; i <= listPaymode.Count - 1; i++)
                    //    {
                    //        if (i != 3)
                    //        {
                    //            paymentModeList.Add(new SelectListItem
                    //            {
                    //                Value = Convert.ToString(listPaymode[i].PaymentMode_ID),
                    //                Text = Convert.ToString(listPaymode[i].PaymentMode)
                    //            });
                    //        }
                    //    }
                    //}
                    ViewBag.ddlPaymentMode=patinfo.GetPaymentModes();
                   // ViewData["ddlPaymentMode"] = paymentModeList;
                    //Referrals description = new Referrals();
                    //description.FieldIDString = "11";
                    DataSet dsnote = new DataSet();
                    dsnote = Referrals.List_field_description("11");
                    if (dsnote.Tables[0].Rows.Count > 0)
                    {
                        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                        ViewBag.pmtNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    }
                    return View(patinfo);
                }
                catch (Exception ex)
                {
                    var obj = new clsExceptionLog();
                    obj.LogException(ex, "Schedule", "CustomerTransactions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                return null;
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost()]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerTransactions(RPBilling objTran)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["roleid"].ToString() != "1")
                //{
                objTran.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                //}
                //else
                //{
                //    objTran.fromReferenceId = Convert.ToInt32(Request["hdnproid"]);
                //}
                objTran.ToReference_ID = !string.IsNullOrEmpty(objTran.ToReference_ID) ? objTran.ToReference_ID : null;
                objTran.FromReferenceType_ID = "2";
                objTran.ToReferenceType_ID = "3";
                objTran.Transaction_Date = !string.IsNullOrEmpty(Request["txtPayApptDate"]) ? Request["txtPayApptDate"] : null;
                objTran.Transaction_Amount = !string.IsNullOrEmpty(objTran.Transaction_Amount) ? objTran.Transaction_Amount : null;
                if (Request["hdnPaymentMode"] != "1")
                {
                    objTran.PaymentType_ID = Convert.ToInt32(Request["ddlPaymentMode"]);
                }
                else
                {
                    objTran.PaymentType_ID = null;
                }
                objTran.PcType_ID = Convert.ToInt32(Request["ddlPayment"]);

                objTran.Authorizednumber = !string.IsNullOrEmpty(objTran.Authorizednumber) ? objTran.Authorizednumber : null;
                objTran.ChecksNo = !string.IsNullOrEmpty(objTran.ChecksNo) ? objTran.ChecksNo : null;
                if (!string.IsNullOrEmpty(objTran.Customer_Email))
                {
                    Session.Add("hdnclientEmail", objTran.Customer_Email);
                }
                else
                {
                    objTran.Customer_Email = null;
                }
                objTran.Appointment_ID = null;
                objTran.apptdate = null;
                objTran.Notes = !string.IsNullOrEmpty(objTran.Notes) ? Sanitizer.GetSafeHtmlFragment(objTran.Notes) : null;
                string Out_Msg = null;

                objTran.emailcheck = objTran.chkemail ? "Y" : "N";
                objTran.Insert_Customertransaction(objTran, ref Out_Msg);
                objTran.formanme = !string.IsNullOrEmpty(objTran.formanme) ? objTran.formanme : null;
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //obj.schedule_type = Request["hdnschetype"].ToString();
                    if (!string.IsNullOrEmpty(objTran.clientphone))
                    {
                        string strbussinessname = string.Empty;
                        string strproviderphone = string.Empty;
                        string strclientphone = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
                        {
                            strbussinessname = Convert.ToString(Session["PracticeName"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
                        {
                            strproviderphone = Convert.ToString(Session["Providerphone"]);
                        }
                        if (!string.IsNullOrEmpty(objTran.clientphone))
                        {
                            strclientphone = objTran.clientphone;
                            strclientphone = strclientphone.Replace("-", "");
                        }
                        clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, null, null, objTran.Transaction_Amount, Convert.ToInt16(Request["ddlPayment"]));

                    }
                    return Json(JsonResponseFactory.SuccessResponse(objTran), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult CustInvoice(string patientid, int ProviderID, int ProviderLoginID, string ddlrecords = null, string dt_filter = null, string hdnPid = null, int? hdnPlId = null, string fdate = null, string tdate = null, string clientphone = null, string BillingInd = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                RPBilling objInv = new RPBilling
                {
                    PatientID = patientid,
                    Provider_id = ProviderID,
                    ProviderLoginID = ProviderLoginID,
                    NoOfRecords = ddlrecords,
                    Date_range = dt_filter,
                    PatientLogin_ID = hdnPlId,
                    FromDate = fdate,
                    ToDate = tdate,
                    clientphone = clientphone,
                    Ind = BillingInd
                };
                return View(objInv);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        public ActionResult CustInvoice(RPBilling objInv)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                try
                {
                    IDataParameter[] strMailParam = { new SqlParameter("@in_PatientEmail", objInv.Customer_Email),
                                                    new SqlParameter("@in_Provider_ID",objInv.Provider_id),
                                                    new SqlParameter("@in_Patient_ID", objInv.PatientID)
                                                };
                    clscommon.AddInParameters(strMailParam);
                    clscommon.ExecuteNonQuerySP("Help_dbo.ST_upd_patient_email");
                    if (objInv.Ind == "C" & !string.IsNullOrEmpty(objInv.Customer_Email)) { Session.Add("hdnclientEmail", objInv.Customer_Email); }
                }
                catch (Exception ex)
                {
                    clsExceptionLog clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "ClientsController", "CustInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }

                return RedirectToAction("CustomerInvoice", objInv);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        public ActionResult CustomerInvoice(RPBilling objInv)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            string startdate = DateTime.Now.ToShortDateString();
            string enddate = DateTime.Now.ToShortDateString();
            InsertInvoiceTransaction(objInv.Provider_id, objInv.ProviderLoginID, startdate, enddate, objInv.PatientID);
            if (objInv.Ind == "Y")
            {
                if(objInv.patSInd == "N")
                return null;
                else
                    return RedirectToAction("DisplayTransactions", "Billing", objInv);
                //return RedirectToAction("TransactionDisplay", "Billing", new { ddlrecords = objInv.NoOfRecords, dt_filter = objInv.Date_range, hdnPid = objInv.PatientID, hdnPlId = objInv.PatientLogin_ID, fdate = objInv.FromDate, tdate = objInv.ToDate, clientphone = objInv.clientphone, hdnclientEmail = objInv.Customer_Email });
                //return RedirectToAction("DisplayTransactions", "Billing", objInv);
            }
            else if (objInv.Ind == "C")
            {
                //return RedirectToAction("TransactionsList", "Clients");
                if (objInv.patSInd == "T")
                    return null;
                else
                    return RedirectToAction("TransListPartial", "Clients", objInv);
            }
            else
            {
                //return RedirectToAction("ClientsHome");
                if (objInv.Ind == "P")
                    return null;
                else
                    return RedirectToAction("ClientsHomePartial");
            }
        }
        public void InsertInvoiceTransaction(int? ProviderID, int LoginID, string stratdate, string enddate, string PatientID)
        {
            try
            {

                IDataParameter[] strMailParam = { new SqlParameter("@in_FromReference_ID", ProviderID),
                                                    new SqlParameter("@in_FromReferenceType_ID",2),
                                                    new SqlParameter("@in_ToReference_ID", PatientID),
                                                    new SqlParameter("@in_ToReferenceType_ID", 3),
                                                    new SqlParameter("@in_BeginDate", stratdate),
                                                    new SqlParameter("@in_EndDate", enddate),
                                                    new SqlParameter("@in_CreatedBy", LoginID),
                                                };
                clscommon.AddInParameters(strMailParam);
                clscommon.ExecuteNonQuerySP("Help_dbo.st_Billing_INS_electricantocustomerReceipt");
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "InsertInvoiceTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //public ActionResult pmtlist(string ddlrecords, string PatientLoginID, string fdate, string tdate, string frm)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    ViewBag.frm = frm;
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    dynamic paygrid = getPmtlist(ddlrecords, PatientLoginID, fdate, tdate);
        //    ViewBag.Pmtpagesize = ddlrecords;
        //    return View(paygrid);
        //}
        //public ActionResult AdjList(string ddlrecords, string PatientLoginID, string fdate, string tdate, string frm)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    ViewBag.frm = frm;
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    dynamic Adjgrid = getAdjList(ddlrecords, PatientLoginID, fdate, tdate);
        //    ViewBag.Adjpagesize = ddlrecords;
        //    return View(Adjgrid);
        //}
        //public List<RPBilling> getChglist(string ddlrecords, string PatientLoginID, string fromdate, string todate)
        //{
        //    List<RPBilling> chglist = new List<RPBilling>();
        //    try
        //    {
        //    RPBilling objchgBill = new RPBilling();

        //    if (!string.IsNullOrEmpty(PatientLoginID))
        //    {
        //        objchgBill.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //        ViewBag.PlId = objchgBill.PatientLogin_ID;
        //    }
        //    else
        //    {
        //        objchgBill.PatientLogin_ID = null;
        //        ViewBag.PlId = null;
        //    }
        //    objchgBill.ReferenceType_ID = null;

        //    if (!string.IsNullOrEmpty(fromdate))
        //    {
        //        objchgBill.FromDate = fromdate;
        //        ViewBag.frmdate = fromdate;
        //    }
        //    if (!string.IsNullOrEmpty(todate))
        //    {
        //        objchgBill.ToDate = todate;
        //        ViewBag.todate = todate;
        //    }

        //    objchgBill.AuthorizedPatientLoginID = null;

        //    if (Request["sort"] == null)
        //    {
        //        objchgBill.OrderbyItem = "Transaction_Date";
        //        ViewBag.chsort = "Transaction_Date";
        //    }
        //    else
        //    {
        //        objchgBill.OrderbyItem = Convert.ToString(Request["sort"]);
        //        ViewBag.chsort = Convert.ToString(Request["sort"]);
        //    }
        //    if (Request["sortdir"] == null)
        //    {
        //        objchgBill.orderBy = "DESC";
        //        ViewBag.chsortdir = "DESC";
        //    }
        //    else
        //    {
        //        objchgBill.orderBy = Convert.ToString(Request["sortdir"]);
        //        ViewBag.chsortdir = Convert.ToString(Request["sortdir"]);
        //    }
        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //    if (Request["page"] == null)
        //    {
        //        objchgBill.PageNo = 1;
        //    }
        //    else
        //    {
        //        objchgBill.PageNo = Convert.ToInt32(Request["page"]);
        //    }
        //    if (ddlrecords != null)
        //    {
        //        objchgBill.NoOfRecords = Convert.ToInt32(ddlrecords);
        //    }
        //    else
        //    {
        //        objchgBill.NoOfRecords = 5;
        //    }

        //    chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //    if (chglist.Count > 0)
        //    {
        //        totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //        ViewBag.totrec = totalrecords;
        //    }
        //    else
        //    {
        //        ViewBag.totrec = 0;
        //    }
        //    return chglist;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "getChglist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return chglist;
        //    }
        //}
        //public List<RPBilling> getPmtlist(string ddlrecords, string PatientLoginID, string fdate, string tdate)
        //{
        //    List<RPBilling> Adjlist = new List<RPBilling>();
        //    try
        //    {
        //        RPBilling objAdj = new RPBilling();
        //        if (PatientLoginID != null)
        //        {
        //            objAdj.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //            ViewBag.plId = Convert.ToInt32(PatientLoginID);
        //        }
        //        else
        //        {
        //            objAdj.PatientLogin_ID = null;
        //        }
        //        objAdj.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fdate))
        //        {
        //            if (fdate != "All")
        //            {
        //                objAdj.FromDate = fdate;
        //                objAdj.ToDate = tdate;

        //            }
        //            else
        //            {
        //                objAdj.FromDate = null;
        //                objAdj.ToDate = null;
        //            }
        //        }

        //        objAdj.AuthorizedPatientLoginID = null;
        //        if (Request["g2sort"] == null)
        //        {
        //            objAdj.OrderbyItem = "Transaction_Date";
        //        }
        //        else
        //        {
        //            objAdj.OrderbyItem = Convert.ToString(Request["g2sort"]);
        //        }
        //        if (Request["g2sortdir"] == null)
        //        {
        //            objAdj.orderBy = "DESC";
        //        }
        //        else
        //        {
        //            objAdj.orderBy = Convert.ToString(Request["g2sortdir"]);
        //        }
        //        objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //        if (Request["page"] == null)
        //        {
        //            objAdj.PageNo = "1";
        //        }
        //        else
        //        {
        //            objAdj.PageNo = Request["page"];
        //        }
        //        if (Request["g2p2"] == null)
        //        {
        //            objAdj.PageNo = "1";
        //        }
        //        else
        //        {
        //            objAdj.PageNo = Request["g2p2"];
        //        }
        //        if (ddlrecords != null)
        //        {
        //            objAdj.NoOfRecords = ddlrecords;
        //        }
        //        else
        //        {
        //            objAdj.NoOfRecords = "5";
        //        }

        //        Adjlist = objAdj.GetPatientPaymentList(objAdj);
        //        if (Adjlist.Count > 0)
        //        {
        //            totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //            ViewBag.Paytotrec = totalrecords;
        //        }
        //        else
        //        {
        //            ViewBag.Paytotrec = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "getPmtlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //    return Adjlist;
        //}
        //public List<RPBilling> getAdjList(string ddlrecords, string PatientLoginID, string fdate, string tdate)
        //{
        //    List<RPBilling> Adjlist = new List<RPBilling>();
        //    try
        //    {
        //        RPBilling objpmt = new RPBilling();
        //        if (PatientLoginID != null)
        //        {
        //            objpmt.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //            ViewBag.plId = Convert.ToInt32(PatientLoginID);
        //        }
        //        else
        //        {
        //            objpmt.PatientLogin_ID = null;
        //        }
        //        objpmt.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fdate))
        //        {
        //            if (fdate != "All")
        //            {
        //                objpmt.FromDate = fdate;
        //                objpmt.ToDate = tdate;
        //            }
        //            else
        //            {
        //                objpmt.FromDate = null;
        //                objpmt.ToDate = null;
        //            }
        //        }
        //        objpmt.AuthorizedPatientLoginID = null;
        //        if (Request["sort"] == null)
        //        {
        //            objpmt.OrderbyItem = "Transaction_Date";
        //        }
        //        else
        //        {
        //            objpmt.OrderbyItem = Convert.ToString(Request["sort"]);
        //        }
        //        if (Request["sortdir"] == null)
        //        {
        //            objpmt.orderBy = "DESC";
        //        }
        //        else
        //        {
        //            objpmt.orderBy = Convert.ToString(Request["sortdir"]);
        //        }
        //        objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        // objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //        if (Request["page"] == null)
        //        {
        //            objpmt.PageNo = "1";
        //        }
        //        else
        //        {
        //            objpmt.PageNo = Request["page"];
        //        }
        //        if (Request["g3p3"] == null)
        //        {
        //            objpmt.PageNo = "1";
        //        }
        //        else
        //        {
        //            objpmt.PageNo = Request["g3p3"];
        //        }
        //        if (ddlrecords != null)
        //        {
        //            objpmt.NoOfRecords = ddlrecords;
        //        }
        //        else
        //        {
        //            objpmt.NoOfRecords = "5";
        //        }

        //        Adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //        if (Adjlist.Count > 0)
        //        {
        //            totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //            ViewBag.Adjtotrec = totalrecords;
        //        }
        //        else
        //        {
        //            ViewBag.Adjtotrec = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "getAdjList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //    return Adjlist;
        //}

        //public ActionResult AddCharge(string PId, string frm)
        //{

        //    if (Session["RoleID"] != null)
        //    {
        //        if (Session["RoleID"].ToString() == "1")
        //        {
        //            if (Request["PracticeID"] != "" & Request["PracticeID"] != null & Request["ProviderID"] != null)
        //            {
        //                Session.Add("Prov_ID", Request["ProviderID"]);
        //                //Session.Add("Practice_ID", Request["PracticeID"]);
        //            }
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(frm))
        //    {
        //        ViewBag.frm = frm;
        //    }
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    if (PId != null)
        //    {
        //        RPBilling patinfo = new RPBilling();
        //        List<RPBilling> patinfolist = new List<RPBilling>();
        //        ViewBag.patselect = "Yes";
        //        //clsCommonFunctions objcommon = new clsCommonFunctions();
        //        ViewBag.hdnCPid = Convert.ToInt32(PId);
        //        ViewBag.hdnprId = Convert.ToInt32(PId);
        //        patinfo.Reference_id = Convert.ToInt32(PId);
        //        patinfo.ReferenceType_ID = "3";
        //        IDataParameter[] insaparam ={
        //                                  new SqlParameter("@In_reference_id",patinfo.Reference_id),
        //                                  new SqlParameter("@In_referencetype_id",patinfo.ReferenceType_ID),
        //                                  new SqlParameter("@in_Practice_ID",Session["Prov_ID"]),
        //                                  new SqlParameter("@in_PatientLogin_ID",null)
        //                                };
        //        clscommon.AddInParameters(insaparam);

        //        DataSet dspatBal = new DataSet();

        //        dspatBal = clscommon.GetDataSet("Help_dbo.st_Billing_GET_UnpaidBalance");
        //        if (dspatBal.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dspatBal.Tables[0].Rows)
        //            {
        //                RPBilling patdet = new RPBilling();
        //                if (!DBNull.Value.Equals(dr["Balance"]))
        //                {
        //                    string bal = string.Empty;
        //                    if (Convert.ToDouble(dr["Balance"]) >= 0)
        //                    {
        //                        bal = string.Format("{0:c}", dr["Balance"]);
        //                        patdet.BalanceAmount = "(Unpaid balance : " + bal + ")";
        //                        ViewBag.patbal = patdet.BalanceAmount;
        //                    }
        //                    else
        //                    {
        //                        bal = string.Format("{0:c}", dr["Balance"]);
        //                        bal = bal.Replace("(", "");
        //                        bal = bal.Replace(")", "");
        //                        patdet.BalanceAmount = "(Overpaid balance :" + bal + ")";
        //                        ViewBag.patbal = patdet.BalanceAmount;
        //                    }

        //                }
        //                if (!DBNull.Value.Equals(dr["Patientname"]))
        //                {
        //                    ViewBag.patname = dr["PatientName"];
        //                }
        //                if (!DBNull.Value.Equals(dr["Registerdate"]))
        //                {

        //                    string[] date1 = dr["Registerdate"].ToString().Split(' ');
        //                    ViewBag.RegOndate = Convert.ToString(date1[0]);
        //                }
        //            }
        //        }
        //    }
        //    //Referrals description = new Referrals();
        //    //description.FieldIDString = "10";
        //    DataSet dsnote = new DataSet();
        //    dsnote = Referrals.List_field_description("10");
        //    if (dsnote.Tables[0].Rows.Count > 0)
        //    {
        //        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //        ViewBag.chkgNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //    }
        //    return View();

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddCharge(RPBilling addNewCharge)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    int transId = 0;
        //    addNewCharge.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
        //    addNewCharge.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
        //    addNewCharge.Transaction_Amount = Request["txtAmount"];
        //    addNewCharge.ToReferenceType_ID = "3";
        //    addNewCharge.ToReference_ID = Request["hdnprId"];
        //    addNewCharge.BillingService_ID = "108";
        //    addNewCharge.BillingChargetype_ID = "1";
        //    addNewCharge.UserId = Convert.ToString(Session["UserID"]);
        //    //addNewCharge.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    addNewCharge.Transaction_Date = Request["txtDate"];
        //    addNewCharge.TransactionType = "1";
        //    addNewCharge.PatientLogin_ID = null;
        //    addNewCharge.ReferenceType_ID = "2";
        //    transId = addNewCharge.MakeTransactionlist(addNewCharge, Convert.ToInt32(Session["Loginhistory_ID"]));
        //    string total = string.Empty;
        //    total = addNewCharge.Get_UnAppliedAmount(addNewCharge);

        //    if (!string.IsNullOrEmpty(Request["frm"]))
        //    {
        //        if (Request["frm"] == "ph")
        //        {
        //            return RedirectToAction("ClientsHome");
        //        }
        //        else
        //        {
        //            return RedirectToAction("TransactionsList", new { frm = "ph", dt_filter = Request["dt_filter"] });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("TransactionsList", new { dt_filter = Request["dt_filter"] });
        //    }

        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult EditCharge(int tId, string PId, int PLId)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsECharge = new RPBilling();
                if (!string.IsNullOrEmpty(Request["pagetype"]))
                {
                    clsECharge.page_ind = Request["pagetype"];
                }
                clsECharge.PatientID = PId;
                clsECharge.PatientLogin_ID = PLId;
                clsECharge.Transaction_ID = tId;
                DataSet dsChgdetail = new DataSet();
                //dsChgdetail = clsECharge.ShowChargeAppliedAmount(clsECharge);
                dsChgdetail = clsECharge.showtransactiondetails(clsECharge);
                if (dsChgdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsChgdetail.Tables[0].Rows)
                    {
                        clsECharge.custname = Convert.ToString(dr["Patientname"]);
                        //ViewBag.tCharge = dr["Chargetype"];
                        clsECharge.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsECharge.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        clsECharge.Notes = HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"]));
                        clsECharge.ToReference_ID = Convert.ToString(dr["ToReference_ID"]);
                        clsECharge.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? Convert.ToString(dr["Email"]) : null;
                    }
                }
                //Referrals description = new Referrals();
                //description.FieldIDString = "10";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("10");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.chkgENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsECharge);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditCharge(RPBilling chgDetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                chgDetails.Transaction_Date = !string.IsNullOrEmpty(Request["txtECDate"]) ? Request["txtECDate"] : null;
                chgDetails.BalanceAmount = !string.IsNullOrEmpty(chgDetails.Transaction_Amount) ? chgDetails.Transaction_Amount : null;
                chgDetails.Notes = !string.IsNullOrEmpty(chgDetails.Notes) ? Sanitizer.GetSafeHtmlFragment(chgDetails.Notes) : null;
                if (Session["UserID"] != null)
                {
                    chgDetails.UserId = Session["UserID"].ToString();
                }

                chgDetails.Ind = "Y";
                if (!string.IsNullOrEmpty(chgDetails.Customer_Email))
                {
                    Session.Add("hdnclientEmail", chgDetails.Customer_Email);
                }
                else
                {
                    chgDetails.Customer_Email = null;
                }
                string Out_Msg = null;

                //var strbussinessname = Convert.ToString(Session["PracticeName"]);
                //var strproviderphone = Convert.ToString(Session["Providerphone"]);
                //var strclientphone = Convert.ToString(Session["ClientMobilePhone"]);

                //clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, null, null, Request["txtEcAmount"], 1);
                chgDetails.emailcheck = chgDetails.ECchkemail ? "Y" : "N";
                chgDetails.Edit_Payment_charges(chgDetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.SuccessResponse(chgDetails), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        //public ActionResult AddPayment(string PId, string frm)
        //{

        //    if (Session["RoleID"] != null)
        //    {
        //        if (Session["RoleID"].ToString() == "1")
        //        {
        //            if (Request["PracticeID"] != "" & Request["PracticeID"] != null & Request["ProviderID"] != null)
        //            {
        //                Session.Add("Prov_ID", Request["ProviderID"]);
        //                //Session.Add("Practice_ID", Request["PracticeID"]);
        //            }
        //        }
        //    }

        //    ViewBag.frm = frm;
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    if (PId != null)
        //    {
        //        RPBilling patinfo = new RPBilling();
        //        List<RPBilling> patinfolist = new List<RPBilling>();
        //        ViewBag.hdnpid = PId;
        //        //clsCommonFunctions objcommon = new clsCommonFunctions();
        //        ViewBag.hdnMPid = PId;
        //        patinfo.Reference_id = Convert.ToInt32(PId);
        //        patinfo.ReferenceType_ID = "3";
        //        IDataParameter[] insaparam ={
        //                                  new SqlParameter("@In_reference_id",patinfo.Reference_id),
        //                                  new SqlParameter("@In_referencetype_id",patinfo.ReferenceType_ID),
        //                                  new SqlParameter("@in_Practice_ID",Session["Prov_ID"]==null?Session["Prov_ID"]:Session["Prov_ID"]),
        //                                  new SqlParameter("@in_PatientLogin_ID",null)
        //                                };
        //        clscommon.AddInParameters(insaparam);

        //        DataSet dspatBal = new DataSet();

        //        dspatBal = clscommon.GetDataSet("Help_dbo.st_Billing_GET_UnpaidBalance");
        //        if (dspatBal.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dspatBal.Tables[0].Rows)
        //            {
        //                RPBilling patdet = new RPBilling();
        //                if (!DBNull.Value.Equals(dr["Balance"]))
        //                {
        //                    string bal = string.Empty;
        //                    if (Convert.ToDouble(dr["Balance"]) >= 0)
        //                    {
        //                        bal = string.Format("{0:c}", dr["Balance"]);
        //                        patdet.BalanceAmount = "(Unpaid balance : " + bal + ")";
        //                        ViewBag.patPaybal = patdet.BalanceAmount;
        //                    }
        //                    else
        //                    {
        //                        bal = string.Format("{0:c}", dr["Balance"]);
        //                        bal = bal.Replace("(", "");
        //                        bal = bal.Replace(")", "");
        //                        patdet.BalanceAmount = "(Overpaid balance :" + bal + ")";
        //                        ViewBag.patPaybal = patdet.BalanceAmount;
        //                    }

        //                }
        //                if (!DBNull.Value.Equals(dr["Patientname"]))
        //                {
        //                    ViewBag.patname = dr["PatientName"];
        //                }
        //                if (!DBNull.Value.Equals(dr["Registerdate"]))
        //                {

        //                    string[] date1 = dr["Registerdate"].ToString().Split(' ');
        //                    ViewBag.RegOndate = Convert.ToString(date1[0]);
        //                }
        //            }
        //        }
        //    }
        //    RPBilling clspayMode = new RPBilling();
        //    List<RPBilling> listPaymode = new List<RPBilling>();
        //    listPaymode = clspayMode.GetPaymentModes();
        //    IList<SelectListItem> paymentModeList = new List<SelectListItem>();
        //    if (listPaymode.Count > 0)
        //    {
        //        for (int i = 0; i <= listPaymode.Count - 1; i++)
        //        {
        //            if (i != 3)
        //            {
        //                paymentModeList.Add(new SelectListItem
        //                {
        //                    Value = Convert.ToString(listPaymode[i].PaymentMode_ID),
        //                    Text = Convert.ToString(listPaymode[i].PaymentMode)
        //                });
        //            }
        //        }
        //    }
        //    ViewData["ddlPaymentMode"] = paymentModeList;
        //    //Referrals description = new Referrals();
        //    //description.FieldIDString = "11";
        //    DataSet dsnote = new DataSet();
        //    dsnote = Referrals.List_field_description("11");
        //    if (dsnote.Tables[0].Rows.Count > 0)
        //    {
        //        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //        ViewBag.pmtNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //    }
        //    return View();

        //}
        //public JsonResult ddlpatBlaence(string patientid)
        //{
        //    if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
        //    {
        //        RPBilling patinfo = new RPBilling();
        //        List<RPBilling> patinfolist = new List<RPBilling>();

        //        patinfo.Reference_id = Convert.ToInt32(patientid);
        //        patinfo.ReferenceType_ID = "8";
        //        // patinfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        patinfolist = patinfo.getUnpaidBalance(patinfo);
        //        return Json(patinfolist, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
        //    }

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddPayment()
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    RPBilling objPayTran = new RPBilling();
        //    DataSet dsclaim = new DataSet();
        //    string respParty = string.Empty;
        //    respParty = Convert.ToString(Request["hdnMPid"]);
        //    int transId = 0;
        //    if (Request["ddlPaymentMode"] == "1")
        //    {
        //        objPayTran.TransactionType = "2";
        //        objPayTran.BillingChargetype_ID = "2";
        //        objPayTran.PaymentMode = "1";
        //    }
        //    else if (Request["ddlPaymentMode"] == "2")
        //    {
        //        objPayTran.PaymentMode = "2";
        //        objPayTran.BillingChargetype_ID = "2";
        //        if (!string.IsNullOrEmpty(Request["txtAuthNum"]))
        //        {
        //            objPayTran.Authorizednumber = Request["txtAuthNum"];
        //        }
        //        objPayTran.IsCCchargetoPrv = "N";
        //        objPayTran.TransactionType = "21";
        //    }
        //    else if (Request["ddlPaymentMode"] == "3")
        //    {
        //        objPayTran.PaymentMode = "3";
        //        if (!string.IsNullOrEmpty(Request["txtCheckNo"]))
        //        {
        //            objPayTran.ChecksNo = Request["txtCheckNo"];
        //        }
        //    }
        //    objPayTran.Transaction_Amount = Request["txtpAmount"];
        //    if (!string.IsNullOrEmpty(Request["txtNotes"]))
        //    {
        //        objPayTran.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
        //    }
        //    objPayTran.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
        //    objPayTran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    objPayTran.ReferenceType_ID = "2";
        //    objPayTran.ToReferenceType_ID = "3";
        //    objPayTran.Transaction_Date = Request["txtPayApptDate"];
        //    objPayTran.ToReference_ID = respParty;
        //    objPayTran.PatientID = respParty;
        //    objPayTran.UserId = Convert.ToString(Session["UserID"]);
        //    //objPayTran.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objPayTran.PatientLogin_ID = null;
        //    if (Request["ddlPaymentMode"] == "1")
        //    {
        //        transId = objPayTran.MakePaymentTransactionlist(objPayTran, Convert.ToInt32(Session["Loginhistory_ID"]));
        //    }
        //    else if (Request["ddlPaymentMode"] == "2")
        //    {
        //        transId = objPayTran.MakePayCcTransaction(objPayTran, Convert.ToInt32(Session["Loginhistory_ID"]));
        //    }
        //    else if (Request["ddlPaymentMode"] == "3")
        //    {
        //        objPayTran.ToReference_ID = respParty;
        //        objPayTran.PatientID = Request["hdnpid"];
        //        objPayTran.ToReferenceType_ID = "3";
        //        transId = objPayTran.Ins_ChequePayment(objPayTran, Convert.ToInt32(Session["Loginhistory_ID"]));
        //    }
        //    objPayTran.Transaction_ID = transId;
        //    objPayTran.Ind = "Y";
        //    DataSet dsViewDistt = new DataSet();
        //    dsViewDistt = objPayTran.viewpaymentAutodistrib(objPayTran);
        //    if (dsViewDistt.Tables[0].Rows.Count > 0)
        //    {
        //        double autoDistAmount = 0;
        //        if (Request["txtpAmount"] != "0")
        //        {
        //            autoDistAmount = Convert.ToDouble(Request["txtpAmount"]);
        //        }
        //        for (int i = 0; i <= dsViewDistt.Tables[0].Rows.Count - 1; i++)
        //        {
        //            string chrgId = string.Empty;
        //            double amtBal = 0;
        //            double prcAmt = 0;
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]))
        //            {
        //                chrgId = Convert.ToString(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]))
        //            {
        //                amtBal = Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]);
        //            }
        //            if (autoDistAmount > amtBal)
        //            {
        //                prcAmt = amtBal;
        //                autoDistAmount = autoDistAmount - amtBal;
        //            }
        //            else
        //            {
        //                prcAmt = autoDistAmount;
        //                autoDistAmount = 0;
        //            }
        //            objPayTran.chargeTransaction_ID = chrgId;
        //            objPayTran.PaymentTransaction_ID = Convert.ToString(transId);
        //            objPayTran.AppliedAmount = Convert.ToString(prcAmt);
        //            objPayTran.UserId = Convert.ToString(Session["UserID"]);
        //            objPayTran.Ins_Disttransaction(objPayTran, Convert.ToInt32(Session["Loginhistory_ID"]));

        //        }
        //    }
        //    if (!string.IsNullOrEmpty(Request["frm"]))
        //    {
        //        if (Request["frm"] == "ph")
        //        {
        //            return RedirectToAction("ClientsHome");
        //        }
        //        else
        //        {
        //            return RedirectToAction("TransactionsList", new { frm = "ph", dt_filter = Request["dt_filter"] });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("TransactionsList", new { dt_filter = Request["dt_filter"] });
        //    }


        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult EditPayment(int tId, string PId, int PLId)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsEPay = new RPBilling();
                if (!string.IsNullOrEmpty(Request["pagetype"]))
                {
                    clsEPay.page_ind = Request["pagetype"];
                }
                clsEPay.PatientID = PId;
                clsEPay.PatientLogin_ID = PLId;
                clsEPay.Transaction_ID = tId;
                DataSet dsPmtdetail = new DataSet();
                //dsPmtdetail = clsEPay.showPayAmount(clsEPay);
                dsPmtdetail = clsEPay.showtransactiondetails(clsEPay);
                if (dsPmtdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPmtdetail.Tables[0].Rows)
                    {
                        clsEPay.custname = Convert.ToString(dr["Patientname"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["PaymentType_ID"])))
                        {
                            clsEPay.paytype = Convert.ToString(dr["PaymentType_ID"]);
                            if (clsEPay.paytype == "1")
                            {
                                clsEPay.paytype = "Cash";
                            }
                            else if (clsEPay.paytype == "2")
                            {
                                clsEPay.paytype = "Credit Card";
                            }
                            else if (clsEPay.paytype == "3")
                            {
                                clsEPay.paytype = "Check";
                            }
                        }
                        //string[] regdate = dr["PatientRegDate"].ToString().Split(' ');
                        //ViewBag.regdate = regdate[0].ToString();
                        clsEPay.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsEPay.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        if (dr["ChequeNo"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["ChequeNo"])))
                            {
                                clsEPay.ChecksNo = Convert.ToString(dr["ChequeNo"]);
                            }
                            else
                            {
                                clsEPay.ChecksNo = null;
                            }
                        }
                        else
                        {
                            clsEPay.ChecksNo = null;
                        }
                        clsEPay.Notes = dr["Notes"] != DBNull.Value ? HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"])) : null;
                        if (dr["Authorizationnumber"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["Authorizationnumber"])))
                            {
                                clsEPay.Authorizednumber = Convert.ToString(dr["Authorizationnumber"]);
                            }
                            else
                            {
                                clsEPay.Authorizednumber = null;
                            }
                        }
                        else
                        {
                            clsEPay.Authorizednumber = null;
                        }
                        clsEPay.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? Convert.ToString(dr["Email"]) : null;
                    }
                }
                //if (dsPmtdetail.Tables.Count > 0)
                //{

                //    StringBuilder strEdiPaygrid = new StringBuilder("<table width='100%' align='center' cellpadding='0' cellspacing='0' border='0' id='tblDistribute' runat='server'> <tr height='25' class='white_color' align='center'><td class='lbl' style='font-size:14px;'> This payment is applied to the following claims and/or charges.</td></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table id='tblhead' runat='server' cellspacing='1' cellpadding='1' width='100%' align='center' border='0'><tr><th class='tr_bgcolor' align='left' width='100%' style='font-weight: bold;'>List of transactions</th></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table width='100%' cellpadding='4' cellspacing='1' border='0' class='border_style'><tr class='tr_bgcolor'><td width='25%' align='center'><strong>Date</strong></td><td width='25%' align='center'><strong>Transaction Type</strong></td><td width='25%' align='center'><strong>Amount</strong></td></tr>");
                //    if (dsPmtdetail.Tables[1].Rows.Count > 0)
                //    {
                //        foreach (DataRow drgrid in dsPmtdetail.Tables[1].Rows)
                //        {
                //            strEdiPaygrid = strEdiPaygrid.Append("<tr class='white_color'><td width='25%' align='center'>" + DateTime.Parse(drgrid["Transaction_Date"].ToString()).ToShortDateString() + "</td><td width='25%' align='center'>" + drgrid["ServiceName"] + "</td><td width='25%' align='center'>" + string.Format("{0:c}", drgrid["AppliedAmount"]) + "</td></tr>");


                //        }
                //        strEdiPaygrid = strEdiPaygrid.Append("</table>");
                //        ViewBag.editPaydist = strEdiPaygrid.ToString();
                //    }
                //    else
                //    {
                //        ViewBag.editPaydist = null;
                //    }

                //}
                //Referrals description = new Referrals();
                //description.FieldIDString = "11";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("11");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.pmtENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsEPay);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayment(RPBilling paydetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                paydetails.Transaction_Date = !string.IsNullOrEmpty(Request["txtPDate"]) ? Request["txtPDate"] : null;
                paydetails.BalanceAmount = !string.IsNullOrEmpty(paydetails.Transaction_Amount) ? paydetails.Transaction_Amount : null;
                paydetails.Notes = !string.IsNullOrEmpty(paydetails.Notes) ? Sanitizer.GetSafeHtmlFragment(paydetails.Notes) : null;
                paydetails.UserId = Convert.ToString(Session["UserID"]);
                paydetails.Ind = "N";
                paydetails.Authorizednumber = !string.IsNullOrEmpty(paydetails.Authorizednumber) ? paydetails.Authorizednumber : null;
                paydetails.ChecksNo = !string.IsNullOrEmpty(paydetails.ChecksNo) ? paydetails.ChecksNo : null;
                if (!string.IsNullOrEmpty(paydetails.Customer_Email))
                {
                    Session.Add("hdnclientEmail", paydetails.Customer_Email);
                }
                else
                {
                    paydetails.Customer_Email = null;
                }
                string Out_Msg = null;

                //var strbussinessname = string.Empty; 
                //var strproviderphone = string.Empty;
                //var strclientphone = string.Empty;
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["ClientMobilePhone"])))
                //{
                //    strclientphone = Convert.ToString(Session["ClientMobilePhone"]);
                //}

                //if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
                //{
                //    strbussinessname = Convert.ToString(Session["PracticeName"]);
                //}
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
                //{
                //    strproviderphone = Convert.ToString(Session["Providerphone"]);
                //}
                //clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, null, null, Request["txtPAmount"], 2);
                paydetails.emailcheck = paydetails.EPchkemail ? "Y" : "N";
                paydetails.Edit_Payment_charges(paydetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.SuccessResponse(paydetails), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult EditTransaction(int tId, string PId, int PLId)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsEPay = new RPBilling();
                if (!string.IsNullOrEmpty(Request["pagetype"]))
                {
                    clsEPay.page_ind = Request["pagetype"];
                }
                clsEPay.PatientID = PId;
                clsEPay.PatientLogin_ID = PLId;
                clsEPay.Transaction_ID = tId;
                DataSet dsPmtdetail = new DataSet();
                //dsPmtdetail = clsEPay.showPayAmount(clsEPay);
                dsPmtdetail = clsEPay.showtransactiondetails(clsEPay);
                if (dsPmtdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPmtdetail.Tables[0].Rows)
                    {
                        clsEPay.custname = Convert.ToString(dr["Patientname"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["PaymentType_ID"])))
                        {
                            clsEPay.paytype = Convert.ToString(dr["PaymentType_ID"]);
                            if (clsEPay.paytype == "1")
                            {
                                clsEPay.paytype = "Cash";
                            }
                            else if (clsEPay.paytype == "2")
                            {
                                clsEPay.paytype = "Credit Card";
                            }
                            else if (clsEPay.paytype == "3")
                            {
                                clsEPay.paytype = "Check";
                            }
                        }
                        //string[] regdate = dr["PatientRegDate"].ToString().Split(' ');
                        //ViewBag.regdate = regdate[0].ToString();
                        clsEPay.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsEPay.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        if (dr["ChequeNo"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["ChequeNo"])))
                            {
                                clsEPay.ChecksNo = Convert.ToString(dr["ChequeNo"]);
                            }
                            else
                            {
                                clsEPay.ChecksNo = null;
                            }
                        }
                        else
                        {
                            clsEPay.ChecksNo = null;
                        }
                        clsEPay.Notes = dr["Notes"] != DBNull.Value ? HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"])) : null;
                        if (dr["Authorizationnumber"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["Authorizationnumber"])))
                            {
                                clsEPay.Authorizednumber = Convert.ToString(dr["Authorizationnumber"]);
                            }
                            else
                            {
                                clsEPay.Authorizednumber = null;
                            }
                        }
                        else
                        {
                            clsEPay.Authorizednumber = null;
                        }
                        clsEPay.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? Convert.ToString(dr["Email"]) : null;
                    }
                }
                //if (dsPmtdetail.Tables.Count > 0)
                //{

                //    StringBuilder strEdiPaygrid = new StringBuilder("<table width='100%' align='center' cellpadding='0' cellspacing='0' border='0' id='tblDistribute' runat='server'> <tr height='25' class='white_color' align='center'><td class='lbl' style='font-size:14px;'> This payment is applied to the following claims and/or charges.</td></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table id='tblhead' runat='server' cellspacing='1' cellpadding='1' width='100%' align='center' border='0'><tr><th class='tr_bgcolor' align='left' width='100%' style='font-weight: bold;'>List of transactions</th></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table width='100%' cellpadding='4' cellspacing='1' border='0' class='border_style'><tr class='tr_bgcolor'><td width='25%' align='center'><strong>Date</strong></td><td width='25%' align='center'><strong>Transaction Type</strong></td><td width='25%' align='center'><strong>Amount</strong></td></tr>");
                //    if (dsPmtdetail.Tables[1].Rows.Count > 0)
                //    {
                //        foreach (DataRow drgrid in dsPmtdetail.Tables[1].Rows)
                //        {
                //            strEdiPaygrid = strEdiPaygrid.Append("<tr class='white_color'><td width='25%' align='center'>" + DateTime.Parse(drgrid["Transaction_Date"].ToString()).ToShortDateString() + "</td><td width='25%' align='center'>" + drgrid["ServiceName"] + "</td><td width='25%' align='center'>" + string.Format("{0:c}", drgrid["AppliedAmount"]) + "</td></tr>");


                //        }
                //        strEdiPaygrid = strEdiPaygrid.Append("</table>");
                //        ViewBag.editPaydist = strEdiPaygrid.ToString();
                //    }
                //    else
                //    {
                //        ViewBag.editPaydist = null;
                //    }

                //}
                //Referrals description = new Referrals();
                //description.FieldIDString = "11";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("11");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.pmtENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsEPay);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditTransaction(RPBilling paydetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                paydetails.Transaction_Date = !string.IsNullOrEmpty(Request["txt_PDate"]) ? Request["txt_PDate"] : null;
                paydetails.BalanceAmount = !string.IsNullOrEmpty(paydetails.Transaction_Amount) ? paydetails.Transaction_Amount : null;
                paydetails.Notes = !string.IsNullOrEmpty(paydetails.Notes) ? Sanitizer.GetSafeHtmlFragment(paydetails.Notes) : null;
                paydetails.UserId = Convert.ToString(Session["UserID"]);
                paydetails.Ind = "N";
                paydetails.Authorizednumber = !string.IsNullOrEmpty(paydetails.Authorizednumber) ? paydetails.Authorizednumber : null;
                paydetails.ChecksNo = !string.IsNullOrEmpty(paydetails.ChecksNo) ? paydetails.ChecksNo : null;
                if (!string.IsNullOrEmpty(paydetails.Customer_Email))
                {
                    Session.Add("hdnclientEmail", paydetails.Customer_Email);
                }
                else
                {
                    paydetails.Customer_Email = null;
                }
                string Out_Msg = null;

                //var strbussinessname = Convert.ToString(Session["PracticeName"]);
                //var strproviderphone = Convert.ToString(Session["Providerphone"]);
                //var strclientphone = Convert.ToString(Session["ClientMobilePhone"]);          
                //clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, null, null, Request["txt_PAmount"], 3);
                paydetails.emailcheck = paydetails.Echkemail ? "Y" : "N";
                paydetails.Edit_Payment_charges(paydetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.SuccessResponse(paydetails), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }



        //public ActionResult AddAdjustment(string PId, string frm)
        //{

        //    if (Session["RoleID"] != null)
        //    {
        //        if (Session["RoleID"].ToString() == "1")
        //        {
        //            if (Request["PracticeID"] != "" & Request["PracticeID"] != null & Request["ProviderID"] != null)
        //            {
        //                Session.Add("Prov_ID", Request["ProviderID"]);
        //                //Session.Add("Practice_ID", Request["PracticeID"]);
        //            }
        //        }
        //    }

        //    ViewBag.frm = frm;
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    RPBilling patinfo = new RPBilling();
        //    List<RPBilling> patinfolist = new List<RPBilling>();
        //    ViewBag.hdnAdjPid = PId;
        //    //clsCommonFunctions objcommon = new clsCommonFunctions();
        //    IDataParameter[] insaparam ={
        //                                  new SqlParameter("@In_reference_id",PId),
        //                                  new SqlParameter("@In_referencetype_id","3"),
        //                                  new SqlParameter("@in_Practice_ID",Session["Prov_ID"]==null?Session["Prov_ID"]:Session["Prov_ID"]),
        //                                  new SqlParameter("@in_PatientLogin_ID",null)
        //                                };
        //    clscommon.AddInParameters(insaparam);

        //    DataSet dspatBal = new DataSet();

        //    dspatBal = clscommon.GetDataSet("Help_dbo.st_Billing_GET_UnpaidBalance");
        //    if (dspatBal.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dspatBal.Tables[0].Rows)
        //        {

        //            if (!DBNull.Value.Equals(dr["Patientname"]))
        //            {
        //                ViewBag.patname = dr["PatientName"];
        //            }
        //            if (!DBNull.Value.Equals(dr["Registerdate"]))
        //            {

        //                string[] date1 = dr["Registerdate"].ToString().Split(' ');
        //                ViewBag.RegOndate = Convert.ToString(date1[0]);
        //            }
        //        }
        //    }
        //    List<RPBilling> lstAdjTypes = new List<RPBilling>();
        //    RPBilling clsAdjtypes = new RPBilling();
        //    lstAdjTypes = clsAdjtypes.getAdjTypes();
        //    IList<SelectListItem> AdjTypesList = new List<SelectListItem>();
        //    if (lstAdjTypes.Count > 0)
        //    {
        //        for (int i = 0; i <= lstAdjTypes.Count - 1; i++)
        //        {
        //            AdjTypesList.Add(new SelectListItem
        //            {
        //                Value = Convert.ToString(lstAdjTypes[i].AdjustmentType_ID),
        //                Text = Convert.ToString(lstAdjTypes[i].Adj_Title)
        //            });
        //        }
        //    }
        //    ViewData["AdjTypesList"] = AdjTypesList;
        //    //Referrals description = new Referrals();
        //    //description.FieldIDString = "12";
        //    DataSet dsnote = new DataSet();
        //    dsnote = Referrals.List_field_description("12");
        //    if (dsnote.Tables[0].Rows.Count > 0)
        //    {
        //        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //        ViewBag.adjNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //    }
        //    return View();

        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult AddAdjustment()
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    int adjtransid = 0;
        //    RPBilling objAdjtran = new RPBilling();
        //    DataSet dsclaim = new DataSet();
        //    string respAdjParty = Request["hdnAdjPid"];
        //    objAdjtran.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
        //    objAdjtran.ReferenceType_ID = "2";
        //    objAdjtran.ToReferenceType_ID = "3";
        //    objAdjtran.UserId = Convert.ToString(Session["UserID"]);
        //    //objAdjtran.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objAdjtran.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
        //    objAdjtran.Transaction_Amount = Request["txtadjAmount"];
        //    objAdjtran.Transaction_Date = Request["txtAdjApptDate"];
        //    objAdjtran.ToReference_ID = respAdjParty;
        //    objAdjtran.PatientLogin_ID = null;
        //    if (Request["AdjTypesList"] != null)
        //    {
        //        if (Request["AdjTypesList"] == "3")
        //        {
        //            objAdjtran.TransactionType_ID = "3";
        //            objAdjtran.AdjustmentType_ID = 3;
        //        }
        //        else if (Request["AdjTypesList"] == "1")
        //        {
        //            objAdjtran.Notes = Request["txtNotes"] != "" ? Request["txtNotes"] : "The Amount can be Use Later";
        //            objAdjtran.TransactionType_ID = "4";
        //            objAdjtran.AdjustmentType_ID = 1;
        //        }
        //        else if (Request["AdjTypesList"] == "2")
        //        {
        //            objAdjtran.Notes = Request["txtNotes"] != "" ? Request["txtNotes"] : "The Amount is Write off";
        //            objAdjtran.TransactionType_ID = "4";
        //            objAdjtran.AdjustmentType_ID = 2;
        //        }
        //        else if (Request["AdjTypesList"] == "6")
        //        {
        //            objAdjtran.TransactionType_ID = "4";
        //            objAdjtran.AdjustmentType_ID = 6;
        //        }
        //        else
        //        {
        //            objAdjtran.TransactionType_ID = "4";
        //            objAdjtran.AdjustmentType_ID = 4;
        //        }
        //    }
        //    adjtransid = objAdjtran.MakeAdjustmentTransactionlist(objAdjtran, Convert.ToInt32(Session["Loginhistory_ID"]));

        //    objAdjtran.Transaction_ID = adjtransid;
        //    objAdjtran.Ind = "Y";
        //    DataSet dsViewDistt = new DataSet();
        //    dsViewDistt = objAdjtran.viewpaymentAutodistrib(objAdjtran);
        //    if (dsViewDistt.Tables[0].Rows.Count > 0)
        //    {
        //        double autoDistAmount = 0;
        //        if (Request["txtadjAmount"] != "0")
        //        {
        //            autoDistAmount = Convert.ToDouble(Request["txtadjAmount"]);
        //        }
        //        for (int i = 0; i <= dsViewDistt.Tables[0].Rows.Count - 1; i++)
        //        {
        //            string chrgId = string.Empty;
        //            double amtBal = 0;
        //            double prcAmt = 0;
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]))
        //            {
        //                chrgId = Convert.ToString(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]))
        //            {
        //                amtBal = Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]);
        //            }
        //            if (autoDistAmount > amtBal)
        //            {
        //                prcAmt = amtBal;
        //                autoDistAmount = autoDistAmount - amtBal;
        //            }
        //            else
        //            {
        //                prcAmt = autoDistAmount;
        //                autoDistAmount = 0;
        //            }
        //            objAdjtran.chargeTransaction_ID = chrgId;
        //            objAdjtran.PaymentTransaction_ID = Convert.ToString(adjtransid);
        //            objAdjtran.AppliedAmount = Convert.ToString(prcAmt);
        //            objAdjtran.UserId = Convert.ToString(Session["UserID"]);
        //            objAdjtran.Ins_Disttransaction(objAdjtran, Convert.ToInt32(Session["Loginhistory_ID"]));

        //        }
        //    }
        //    if (!string.IsNullOrEmpty(Request["frm"]))
        //    {
        //        if (Request["frm"] == "ph")
        //        {
        //            return RedirectToAction("ClientsHome");
        //        }
        //        else
        //        {
        //            return RedirectToAction("TransactionsList", new { frm = "ph", dt_filter = Request["dt_filter"] });
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("TransactionsList", new { dt_filter = Request["dt_filter"] });
        //    }

        //}
        //public ActionResult EditAdjustment(string tId, string plid, string trTypeid)
        //{

        //    if (!string.IsNullOrEmpty(Request["frm"]))
        //    {
        //        ViewBag.frm = Request["frm"];
        //    }
        //    if (!string.IsNullOrEmpty(Request["dt_filter"]))
        //    {
        //        ViewBag.dt_filter = Request["dt_filter"];
        //    }
        //    RPBilling editAdjDetails = new RPBilling();
        //    editAdjDetails.Transaction_ID = Convert.ToInt32(tId);
        //    ViewBag.trAdjId = Convert.ToInt32(tId);
        //    ViewBag.plId = plid;
        //    editAdjDetails.TransactionType_ID = trTypeid;
        //    DataSet dsAdjDetails = new DataSet();
        //    dsAdjDetails = editAdjDetails.showAdjustDetails(editAdjDetails);
        //    if (dsAdjDetails.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dsAdjDetails.Tables[0].Rows)
        //        {
        //            ViewBag.patadjName = dr["Patientname"];
        //            if (Convert.ToString(dr["Patientname"]) != Convert.ToString(dr["ToRefName"]))
        //            {
        //                ViewBag.EAdjRefName = dr["ToRefName"];
        //            }
        //            ViewBag.dAdj = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
        //            ViewBag.AdjAmount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2);
        //            if (dr["Notes"] != DBNull.Value)
        //            {
        //                ViewBag.adjustmentnotes = dr["Notes"];
        //            }
        //            else
        //            {
        //                ViewBag.adjustmentnotes = null;
        //            }
        //            ViewBag.adjuDirection = dr["ChargeType"];
        //            ViewBag.lblTransactonType = dr["AdjustmentType"];
        //            ViewBag.patAdjId = dr["ToReference_ID"];
        //            ViewBag.ToRefType_ID = dr["ToReferenceType_ID"];
        //            ViewBag.FrmRef_ID = dr["FromReference_ID"];
        //            ViewBag.FrmRefType_ID = dr["FromReferenceType_ID"];
        //            string[] regdate = dr["PatientRegDate"].ToString().Split(' ');
        //            ViewBag.regdate = Convert.ToString(regdate[0]);
        //        }
        //    }
        //    //Referrals description = new Referrals();
        //    //description.FieldIDString = "12";
        //    DataSet dsnote = new DataSet();
        //    dsnote = Referrals.List_field_description("12");
        //    if (dsnote.Tables[0].Rows.Count > 0)
        //    {
        //        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //        ViewBag.adjENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //    }
        //    if (dsAdjDetails.Tables.Count > 0)
        //    {

        //        StringBuilder strEdiPaygrid = new StringBuilder("<table width='100%' align='center' cellpadding='0' cellspacing='0' border='0' id='tblDistribute' runat='server'> <tr height='25' class='white_color' align='center'><td class='lbl' style='font-size:14px;'> This adjustment is applied to the following payments and/or charges.</td></tr></table>");
        //        strEdiPaygrid = strEdiPaygrid.Append("<table id='tblhead' runat='server' cellspacing='1' cellpadding='1' width='100%' align='center' border='0'><tr><th class='tr_bgcolor' align='left' width='100%' style='font-weight: bold;'>List of transactions</th></tr></table>");
        //        strEdiPaygrid = strEdiPaygrid.Append("<table width='100%' cellpadding='4' cellspacing='1' border='0' class='border_style'><tr class='tr_bgcolor'><td width='25%' align='center'><strong>Date</strong></td><td width='25%' align='center'><strong>Transaction Type</strong></td><td width='25%' align='center'><strong>Amount</strong></td></tr>");
        //        if (dsAdjDetails.Tables[1].Rows.Count > 0)
        //        {
        //            foreach (DataRow drgrid in dsAdjDetails.Tables[1].Rows)
        //            {
        //                strEdiPaygrid = strEdiPaygrid.Append("<tr class='white_color'><td width='25%' align='center'>" + DateTime.Parse(Convert.ToString(drgrid["Transaction_Date"])).ToShortDateString() + "</td><td width='25%' align='center'>" + drgrid["TransactionType"] + "</td><td width='25%' align='center'>" + string.Format("{0:c}", drgrid["AppliedAmount"]) + "</td></tr>");

        //            }
        //            strEdiPaygrid = strEdiPaygrid.Append("</table>");
        //            ViewBag.editPaydist = Convert.ToString(strEdiPaygrid);
        //        }
        //        else
        //        {
        //            ViewBag.editPaydist = null;
        //        }

        //    }
        //    return View();

        //}
        //[HttpPost]
        //public ActionResult EditAdjustment()
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    RPBilling objEditAdj = new RPBilling();
        //    objEditAdj.Transaction_ID = Convert.ToInt32(Request["trAdjId"]);
        //    objEditAdj.Transaction_Date = Request["txtAdjDate"];
        //    objEditAdj.BalanceAmount = Request["txtAdjAmount"];
        //    objEditAdj.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
        //    objEditAdj.Ind = "Y";
        //    objEditAdj.UserId = Convert.ToString(Session["UserID"]);
        //    string Out_Msg = null;
        //    objEditAdj.Edit_Payment_charges(objEditAdj, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);
        //    objEditAdj.Ind = "Y";
        //    objEditAdj.PatientLogin_ID = Convert.ToInt32(Request["plId"]);
        //    objEditAdj.fromReferenceId = Convert.ToInt32(Request["FrmRef_ID"]);
        //    objEditAdj.ReferenceType_ID = Request["FrmRefType_ID"];
        //    objEditAdj.ToReference_ID = Request["patAdjId"];
        //    objEditAdj.ToReferenceType_ID = Request["ToRefType_ID"];
        //    DataSet dsViewDistt = new DataSet();
        //    dsViewDistt = objEditAdj.viewpaymentAutodistrib(objEditAdj);
        //    if (dsViewDistt.Tables[0].Rows.Count > 0)
        //    {
        //        double autoDistAmount = 0;
        //        if (Request["txtAdjAmount"] != "0")
        //        {
        //            autoDistAmount = Convert.ToDouble(Request["txtAdjAmount"]);
        //        }
        //        for (int i = 0; i <= dsViewDistt.Tables[0].Rows.Count - 1; i++)
        //        {
        //            string chrgId = string.Empty;
        //            double amtBal = 0;
        //            double prcAmt = 0;
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]))
        //            {
        //                chrgId = Convert.ToString(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]))
        //            {
        //                amtBal = Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]);
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["AppliedAmount"]))
        //            {
        //                amtBal += Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["AppliedAmount"]);
        //            }
        //            if (autoDistAmount > amtBal)
        //            {
        //                prcAmt = amtBal;
        //                autoDistAmount = autoDistAmount - amtBal;
        //            }
        //            else
        //            {
        //                prcAmt = autoDistAmount;
        //                autoDistAmount = 0;
        //            }
        //            //objEditAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //            objEditAdj.chargeTransaction_ID = chrgId;
        //            objEditAdj.PaymentTransaction_ID = Request["trAdjId"];
        //            objEditAdj.AppliedAmount = Convert.ToString(prcAmt);
        //            objEditAdj.UserId = Convert.ToString(Session["UserID"]);
        //            objEditAdj.Ins_Disttransaction(objEditAdj, Convert.ToInt32(Session["Loginhistory_ID"]));

        //        }
        //    }
        //    if (!string.IsNullOrEmpty(Request["frm"]))
        //    {
        //        return RedirectToAction("TransactionsList", new { frm = "ph", dt_filter = Request["dt_filter"] });
        //    }
        //    else
        //    {
        //        return RedirectToAction("TransactionsList", new { dt_filter = Request["dt_filter"] });
        //    }
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult DeleteTran(int tId, string trTypeid, string type)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                RPBilling objtran = new RPBilling();
                objtran.Transaction_ID = tId;
                objtran.TransactionType_ID = trTypeid;
                objtran.UserId = Convert.ToString(Session["UserID"]);
                objtran.Del_ProviderPatientTransaction(objtran, Convert.ToInt32(Session["Loginhistory_ID"]));
                if (!string.IsNullOrEmpty(Request["frm"]))
                {
                    //return RedirectToAction("TransactionsList", new { frm = "ph", dt_filter = Request["dt_filter"] });
                    return RedirectToAction("TransListPartial", new { frm = "ph", dt_filter = Request["dt_filter"], patientid = Request["patientid"], Provider_id = Convert.ToInt32(Session["Prov_ID"]), ProviderLoginID = Convert.ToInt32(Session["UserID"]), Customer_Email = Request["Customer_Email"], Ind = "C", PatientLogin_ID = Request["PatientLogin_ID"] });
                }
                else
                {
                    //return RedirectToAction("TransactionsList", new { dt_filter = Request["dt_filter"] });
                    return RedirectToAction("TransListPartial", new { dt_filter = Request["dt_filter"], patientid = Request["patientid"], Provider_id = Convert.ToInt32(Session["Prov_ID"]), ProviderLoginID = Convert.ToInt32(Session["UserID"]), Customer_Email = Request["Customer_Email"], Ind = "C", PatientLogin_ID = Request["PatientLogin_ID"] });
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }

        }
        # endregion "End of Patient file Transactions"
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult ClentAppointments()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Request.IsAjaxRequest() && Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            return View();
        }        
        [HttpGet()]        
        public ActionResult ProfileCDProgress(string ddlrecords ,string page = null)
        {
            //ViewBag.appPage = page;
            ////Clients/ClentAppointments
            //patients objCDProgress = getlistProgress();
            //return View(objCDProgress);
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisplayClientApptList(string ddlrecords, string S1P1 = "1", string page = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
               // if (string.IsNullOrEmpty(S1P1))
               // {
               //     S1P1 = "1";
               // }
                //int patid = Convert.ToInt32(Session["PatientID"]);
               // ViewBag.appPage = page;
                //Clients/ClentAppointments
                patients objCDProgress = getlistProgress(S1P1);

                return PartialView("_ClientAppList", objCDProgress);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public patients getlistProgress(string pageno)
        {
            patients objcdProgress = new patients();
            try
            {
                if (string.IsNullOrEmpty(pageno))
                {
                    pageno = "1";
                }
                
                objcdProgress.Patient_ID =Session["PatientID"]!=null? Convert.ToInt32(Session["PatientID"]):0;
                //objcdProgress.orderByItem = Request["S1sort"] == null ? "Appointment_Date" : Request["S1sort"];
                objcdProgress.orderByItem = string.IsNullOrEmpty(Request["S1sort"]) ? "Appointment_Date" : Request["S1sort"];
                objcdProgress.OrderBy = Request["S1sortdir"] == null ? "DESC" : Request["S1sortdir"];
                
                objcdProgress.NoOfRecords = 10;

                objcdProgress.page = string.IsNullOrEmpty(Request["S1P1"]) ? Convert.ToInt32(pageno) : Convert.ToInt32(Request["S1P1"]);
                //objcdProgress.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

                objcdProgress.PatientsList = objcdProgress.ListOfProgressNotes(objcdProgress);
                
                ViewBag.totrec = objcdProgress.PatientsList.Count > 0 ? patients.TotalRecords : 0;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "getlistProgress", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
            return objcdProgress;
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult Deleteappoint(int apptDelid, string S1P1, string S1sortdir = null, string S1sort = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{ 
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                //List<GetAppointment> objGetAppointment = GetAppointment.GetPatientAppointment(Convert.ToString(Appointment_ID), null, null);
                //if (objGetAppointment.Count > 0)
                //{
                //    string aptdate = objGetAppointment[0].AppointmentDate;
                //    string ToRefId = Convert.ToString(objGetAppointment[0].ToReference_ID);
                //    string AppointmentRec = Convert.ToString(objGetAppointment[0].AppointmentRecurrence_ID);
                //    string apptdate = DateTime.Parse(aptdate).ToShortDateString();
                DeleteAppointment delappt = new DeleteAppointment();
                delappt.DeleteAppt(Convert.ToString(apptDelid));
                // }
                return RedirectToAction("DisplayClientApptList", new { S1P1 = S1P1, S1sortdir = S1sortdir, S1sort = S1sort });
                //return RedirectToAction("ClentAppointments");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]        
        public ActionResult ClientNotes()
        {
            
            //List<NotesInfo> objcollection = new List<NotesInfo>();
            //objNotesInfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
            //objNotesInfo.ToReference_ID = Convert.ToInt32(Session["PatientID"]);
            //objNotesInfo.CreatedBy = Convert.ToString(Session["UserID"]);
            //objNotesInfo.NoOfrecords = 10;
            //objNotesInfo.PageNO = Convert.ToInt32(Request["S2P2"] == null ? "1" : Request["S2P2"]);
            //objNotesInfo.OrderBy = "DESC";
            //objNotesInfo.OrderByItem = "Notes_Date";
            ////ObjNotesinfo.PracticeID = Convert.ToInt32(Session["Practice_ID"]);
            //objcollection = NotesInfo.GetCustomerNotesInfo(objNotesInfo);
            //ViewBag.totrec = objcollection.Count > 0 ? objNotesInfo.TotalRecords : 0;
            //return PartialView(objcollection);
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
         public ActionResult DisplayClientNotesList(string S2P2)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                S2P2 = S2P2 ?? "1";
                List<NotesInfo> objcollection = new List<NotesInfo>();
                objNotesInfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
                objNotesInfo.ToReference_ID = Convert.ToInt32(Session["PatientID"]);
                objNotesInfo.CreatedBy = Convert.ToString(Session["UserID"]);
                objNotesInfo.NoOfrecords = 10;
                objNotesInfo.PageNO = Convert.ToInt32(Request["S2P2"] == null ? S2P2 : Request["S2P2"]);

                objNotesInfo.OrderBy = "DESC";
                objNotesInfo.OrderByItem = "Notes_Date";
                //ObjNotesinfo.PracticeID = Convert.ToInt32(Session["Practice_ID"]);
                objcollection = NotesInfo.GetCustomerNotesInfo(objNotesInfo);
                ViewBag.totrec = objcollection.Count > 0 ? objNotesInfo.TotalRecords : 0;

                return PartialView("ClientNotesList", objcollection);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
         public ActionResult AddNotes(int? S2P2)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (S2P2 == 0)
                {
                    S2P2 = 1;
                }
                //ViewBag.Todaydate = DateTime.Now.ToString("MM/dd/yyyy");
                objNotesInfo.PageNO = S2P2 ?? 1;
                return View(objNotesInfo);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(NotesInfo objNotesInfo)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                //int strUser1;
                //int strrole1;
                //string strPatientInd1;

                //strUser1 = Convert.ToInt32(Session["PatientID"]);
                //strrole1 = 5;
                //strPatientInd1 = "Y";
                objNotesInfo.ToReference_ID = Convert.ToInt32(Session["PatientID"]);
                objNotesInfo.ToReferenceType_ID = 5;
                objNotesInfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
                objNotesInfo.FromReferenceType_ID = Convert.ToInt32(Session["RoleID"]);
                objNotesInfo.IsPatientNote_Ind = "Y";
                objNotesInfo.Notes = Sanitizer.GetSafeHtmlFragment(objNotesInfo.Notes);
                if (!string.IsNullOrEmpty(Convert.ToString(objNotesInfo.Notes_Date)))
                {
                    objNotesInfo.Notes_Date = Convert.ToDateTime(objNotesInfo.Notes_Date);

                }
                else
                {
                    objNotesInfo.Notes_Date = null;

                }

                objNotesInfo.CreatedBy = Convert.ToString(Session["UserID"]);
                //if (Session["Roleid"].ToString() == "1")
                //{
                //    if (Session["Practice_ID"] != null)
                //    {
                //        objInsNotesInfo.PracticeID = Convert.ToInt32(Session["Practice_ID"]);
                //    }
                //    else
                //    {
                //        objInsNotesInfo.PracticeID = null;
                //    }
                //}
                //else
                //{
                //    objInsNotesInfo.PracticeID = Convert.ToInt32(Session["Practice_ID"]);
                //}
                objNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                NotesInfo.InsNotesInfo(objNotesInfo);
                return View(objNotesInfo);
                //return RedirectToAction("ClentAppointments");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditNotes(int Generalnoteid, int S2P2)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                objNotesInfo = EditGeneralnotedetails(Generalnoteid);
                objNotesInfo.PageNO = S2P2;
                return View(objNotesInfo);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditNotes(NotesInfo objNotes)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                Savenotes(objNotes);
                return View(objNotes);
                //return RedirectToAction("ClentAppointments");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public NotesInfo EditGeneralnotedetails(int noteid)
        {
            try
            {

                objNotesInfo = NotesInfo.GetupdNotesInfo(noteid);
                objNotesInfo.Notes = !string.IsNullOrEmpty(objNotesInfo.Notes) ? HttpUtility.HtmlDecode(objNotesInfo.Notes) : null;
                if (Convert.ToString(objNotesInfo.Notes_Date) != "")
                {
                    string notedate = Convert.ToString(objNotesInfo.Notes_Date);
                    ViewBag.Notes_Date = notedate.Remove(notedate.Length - 12);
                    if (ViewBag.Notes_Date == "01/01/0001" || ViewBag.notes_Date == "1/1/0001")
                    {
                        ViewBag.Notes_Date = null;
                    }

                }
                else
                {
                    ViewBag.Notes_Date = null;
                }
                if (objNotesInfo.IsPatientNote_Ind != null)
                {
                    if (objNotesInfo.IsPatientNote_Ind == "Y")
                    {
                        objNotesInfo.notetype = "1";
                    }
                    else if (objNotesInfo.IsPatientNote_Ind == "N")
                    {
                        objNotesInfo.notetype = "1";
                        objNotesInfo.ToReference_ID = null;
                        objNotesInfo.PatientName = null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "EditGeneralnotedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
            return objNotesInfo;
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ChangeStatus(int Generalnoteid, string pageno)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //Clients/ClentAppointments called when archive the notes

                updategeneralnotedetails(Generalnoteid);
                return RedirectToAction("DisplayClientNotesList", new { S2P2 = pageno });
                //return RedirectToAction("ClentAppointments");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public void updategeneralnotedetails(int Generalnoteid)
        {
            try
            {

                objNotesInfo.GeneralNote_ID = Generalnoteid;
                objNotesInfo.ToReferenceType_ID = 5;
                objNotesInfo.status_ind = "E";
                objNotesInfo.notetype = "General Notes";
                objNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objNotesInfo);

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "updategeneralnotedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        protected void Savenotes(NotesInfo objNotes)
        {
            try
            {
                
                objNotes.ToReferenceType_ID = 5;
                objNotes.Notes = Sanitizer.GetSafeHtmlFragment(objNotes.Notes);
                objNotes.IsPatientNote_Ind = "Y";
                if (!string.IsNullOrEmpty(Request["txt_Date_Edit"]))
                {
                    objNotes.Notes_Date = Convert.ToDateTime(Request["txt_Date_Edit"]);
                }
                else
                {
                    objNotes.Notes_Date = null;
                }
                objNotes.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objNotes);

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "Savenotes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult EditAppointment(string apptEid, string S1P1 = null, string pageRecCount = null, string S1sort = null, string S1sortdir = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //S1sort = "Appointment_Date";
                ViewBag.pageRecCount = pageRecCount;
                ViewBag.S1sort = S1sort;
                ViewBag.S1sortdir = S1sortdir;
                if (clsWebConfigsettings.GetConfigSettingsValue("Display_placeofservice").ToUpper() == "YES")
                {
                    ViewBag.hdnEplaceind = "Y";
                }
                else
                {
                    ViewBag.hdnEplaceind = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowRecurrence").ToUpper() == "YES")
                {
                    ViewBag.ShowRecurrence = "Y";
                }
                else
                {
                    ViewBag.ShowRecurrence = "N";
                }

                AddAppointmentModel objGetAppt = GetAppointmentDetails(apptEid, null, S1P1);
    //            IList<SelectListItem> OtherAddressItemList = new List<SelectListItem>();
    //            IDataParameter[] objparam = {
    //    new SqlParameter("@In_Provider_id",Convert.ToString(Session["roleid"]) != "1"? Convert.ToString(Session["Prov_id"]):Convert.ToString(Session["ComboProv_ID"]))       
    //};
    //            clscommon.AddInParameters(objparam);
    //            SqlDataReader objread = default(SqlDataReader);
    //            objread = clscommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
    //            while (objread.Read())
    //            {
    //                //if (Convert.ToString(objread["courtlocation"]) != "")
    //               // {
    //                    if (Convert.ToString(ViewBag.SelectedAddressid) == Convert.ToString(objread["Appointment_address_id"]))
    //                    {
    //                        ViewBag.SelectedAddressD = Convert.ToString(objread["courtlocation"]);
    //                        ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                    }
    //                    //if (Convert.ToString(objread["Defaultaddress_Ind"]) == "Y")
    //                    //{

    //                    //    ViewBag.SelectedAddressD = Convert.ToString(objread["Address"]);
    //                    //    ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                    //}
    //                    OtherAddressItemList.Add(new SelectListItem
    //                    {
    //                        Text = Convert.ToString(objread["courtlocation"]),
    //                        Value = Convert.ToString(objread["Appointment_address_id"])
    //                    });
    //               // }
    //            }
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "------------------------",
    //                Value = ""
    //            });
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "New Location",
    //                Value = "0"
    //            });
    //            ViewBag.EOtherAddressItemList = OtherAddressItemList;

                //FillTechnicians123(apptEid);
                Dictionary<int, string> weeklyitems = new Dictionary<int, string>();
                weeklyitems.Add(1, "Sun");
                weeklyitems.Add(2, "Mon");
                weeklyitems.Add(3, "Tue");
                weeklyitems.Add(4, "Wed");
                weeklyitems.Add(5, "Thu");
                weeklyitems.Add(6, "Fri");
                weeklyitems.Add(7, "Sat");
                ViewBag.weeklylist = weeklyitems;
                return View("EditAppointment", objGetAppt);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        private AddAppointmentModel GetAppointmentDetails(string patId, string ApptDate, string S1P1 = null)
        {
            AddAppointmentModel objAddAppt = new AddAppointmentModel();
            try
            {
                List<GetAppointment> objGetAppointment = GetAppointment.GetPatientAppointment(patId, null, ApptDate);
                if (objGetAppointment.Count > 0)
                {
                    objAddAppt.cellphone = objGetAppointment[0].phoneReminderHR;
                    objAddAppt.ProviderName = objGetAppointment[0].ProviderName;
                    objAddAppt.PatientName = objGetAppointment[0].PatientName;
                  //  ViewBag.Patnote = objGetAppointment[0].PTNotes;
                    // ViewBag.Aptnote = objGetAppointment[0].Notes.Trim();
                    objAddAppt.Notes = HttpUtility.HtmlDecode(objGetAppointment[0].Notes.Trim());
                    ViewBag.SelectedAddress = objGetAppointment[0].Defaultaddress_Ind;
                    ViewBag.SelectedAddressid = objGetAppointment[0].Appointment_address_ID;
                    objAddAppt.AppointmentID = objGetAppointment[0].Appointment_ID;
                    TempData["ApptType"] = objGetAppointment[0].AppointmentType_ID;
                    objAddAppt.FromPage = S1P1;
                    if (!string.IsNullOrEmpty(objGetAppointment[0].VoiceFileName))
                    {
                        objAddAppt.VoiceFileName = objGetAppointment[0].VoiceFileName;
                    }
                    if (!string.IsNullOrEmpty(objGetAppointment[0].Startdate))
                    {
                        objAddAppt.StartDate = DateTime.Parse(objGetAppointment[0].Startdate).ToShortDateString();
                    }
                    //ViewBag.techpositions = objGetAppointment[0].TechnicianPositions;
                    objAddAppt.Aptplaceofservice = objGetAppointment[0].Aptplaceofservice;
                    objAddAppt.Aptplaceofservicename = objGetAppointment[0].Aptplaceofservicename;
                    if (objGetAppointment[0].AppointmentType_ID == 1)
                    {
                        objAddAppt.ApptType = "Client";
                    }
                    else
                    {
                        objAddAppt.ApptType = "Blocked";
                    }
                    if ((objGetAppointment[0].AppointmentDate != null))
                    {
                        objAddAppt.AppointmentDate = objGetAppointment[0].AppointmentDate;
                    }
                    if (objGetAppointment[0].IntervalType_ID != 0)
                    {
                        if (objGetAppointment[0].IntervalType_ID == 1)
                        {
                            
                            objAddAppt.RepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "day(s) at " + " " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.WeekDay = "DayShow";
                            objAddAppt.dayEndDate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.interdayvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
                        }
                        else if (objGetAppointment[0].IntervalType_ID == 2)
                        {
                            
                            objAddAppt.WeekDay = "Weekshow";
                            string showweek = string.Empty;
                            string showweekind = string.Empty;
                            if (objGetAppointment[0].SelectedWeekdays != null)
                            {
                                string selectedweeks = objGetAppointment[0].SelectedWeekdays;
                               // selectedweeks.ToCharArray();

                                foreach (char i in selectedweeks)
                                {
                                    switch (i)
                                    {
                                        case '1':
                                            showweek += "Sun" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '2':
                                            showweek += "Mon" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '3':
                                            showweek += "Tue" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '4':
                                            showweek += "Wed" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '5':
                                            showweek += "Thu" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '6':
                                            showweek += "Fri" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '7':
                                            showweek += "Sat" + ",";
                                            showweekind += i + ",";
                                            break;
                                    }


                                }
                                showweek = showweek.Remove(showweek.LastIndexOf(","));
                                showweekind = showweekind.Remove(showweekind.LastIndexOf(","));
                            }
                            
                            objAddAppt.RepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "week(s) on " + " " + showweek + " " + "at  " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.weekEndDate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.interweekvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
                            ViewBag.chkweek = showweekind;
                        }
                    }
                    else
                    {
                        objAddAppt.RepeatType = "No Repeat";
                    }
                    objAddAppt.EndDate = !string.IsNullOrEmpty(objGetAppointment[0].Enddate) ? DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString() : null;
                    
                    //IList<SelectListItem> _ddlTimeMer = new List<SelectListItem>();

                    //_ddlTimeMer.Add(new SelectListItem
                    //{
                    //    Text = "AM",
                    //    Value = "0"
                    //});
                    //_ddlTimeMer.Add(new SelectListItem()
                    //{
                    //    Text = "PM",
                    //    Value = "1"
                    //});
                    //IList<SelectListItem> _ddlDuration = new List<SelectListItem>();
                    //for (int i = 30; i <= 120; i = i + 30)
                    //{
                    //    _ddlDuration.Add(new SelectListItem
                    //    {
                    //        Text = i.ToString(),
                    //        Value = i.ToString()
                    //    });
                    //}

                   
                    //ViewData["cboEtime"] = timelist;
                    //ViewBag.ddlEhr2 = _ddlhr;
                    //ViewBag.ddlEmin2 = _ddlmin;
                    //ViewBag.ddlETimeMer = _ddlTimeMer;
                    //ViewBag.ddlEDuration = _ddlDuration;
                    if (objGetAppointment[0].AppointmentRecurrence_ID != 0)
                    {
                        objAddAppt.AppointmentRecurrence_ID = objGetAppointment[0].AppointmentRecurrence_ID;
                    }
                    if (objGetAppointment[0].CurrentAppointmentTracking_ID != null)
                    {
                        objAddAppt.AppointmentTracking_ID = objGetAppointment[0].CurrentAppointmentTracking_ID;
                    }
                    if (objGetAppointment[0].FacilityReference_ID != null)
                    {
                        objAddAppt.FacilityReference_ID = objGetAppointment[0].FacilityReference_ID;
                    }
                    if ((objGetAppointment[0].AppointmentTime != null))
                    {
                        ViewBag.strTime = objGetAppointment[0].AppointmentTime;
                        string[] strsplit = objGetAppointment[0].AppointmentTime.Split(':');
                        if (strsplit[1] == "00AM" | strsplit[1] == "00PM")
                        {
                            ViewBag.getmin = "00";
                        }
                        else
                        {
                            ViewBag.getmin = "30";
                        }
                        if (strsplit[1] == "00AM" | strsplit[1] == "30AM")
                        {
                            ViewBag.getAMPM = "0";
                        }
                        else
                        {
                            ViewBag.getAMPM = "1";
                        }
                        ViewBag.comboEtime = strsplit[0] + ":" + ViewBag.getmin;
                        string[] timeapplist = { "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" };
                        var timelist = new ComboBoxItemList(timeapplist);
                        ViewBag.index = Array.IndexOf(timeapplist, ViewBag.comboEtime);
                        ViewData["cboEtime"] = timelist;
                        //var timeitems = new List<ComboBoxItem>();
                        //if (ViewBag.comboEtime != null & ViewBag.comboEtime != "")
                        //{
                        //    string timesapp = ViewBag.comboEtime;


                        //    foreach (ComboBoxItem item in timelist)
                        //    {
                        //        if (item.Text == timesapp.Trim())
                        //        {
                        //            item.Selected = true;

                        //        }
                        //        timeitems.Add(item);
                        //    }
                        //    ViewData["cboEtime"] = timeitems;
                        //}
                        //else
                        //{
                        //    ViewData["cboEtime"] = timelist;
                        //    //ViewData["cboyears"] = timeapplist;
                        //}
                    }
                    if (objGetAppointment[0].Duration != 0)
                    {
                        objAddAppt.Duration = objGetAppointment[0].Duration;
                    }
                    if (objGetAppointment[0].AppointmentStatus_ID != null)
                    {
                        objAddAppt.AppointmentStatus_ID = objGetAppointment[0].AppointmentStatus_ID;
                    }
                    objAddAppt.ToReference_ID = objGetAppointment[0].ToReference_ID;
                    objAddAppt.ToReferenceLogin_ID = objGetAppointment[0].ToReferenceLogin_ID;
                    // ViewBag.technician_ids = objGetAppointment[0].Technician_ids;
                    //string tech1 = null;
                    //string tech2 = null;
                    //if (techid.StartsWith(","))
                    //{
                    //    tech1 = techid.Remove(0, 1);
                    //}
                    //else
                    //{
                    //    tech1 = techid;
                    //}
                    //if (tech1 != null)
                    //{
                    //    if (tech1.EndsWith(","))
                    //    {
                    //        tech2 = tech1.Remove(tech1.Length - 1);
                    //    }
                    //    else
                    //    {
                    //        tech2 = tech1;
                    //    }
                    //}
                    //ViewBag.technician_ids = tech2;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ClientsController", "GetAppointmentDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objAddAppt;
        }
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAppointment(Models.AddAppointmentModel objUpdateAppointment)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string strFuturemessage = null;
                string strChangeRecur_Ind = "";
                //string strappttime1 = null;
                //string strTimeDifference = "";
                string ApptTime = "";
                //    string intEveryNo = "";
                string strStartDate = "";
                string dtEndOn = "";
                string SelectedDays = "";
                string strRecurenceValue = "";
                string strMessage = null;
                DateTime originalDate;
                DateTime modifDate;
                string originalval = string.Empty;
                string modifyval = string.Empty;
                string modify = string.Empty;
                string modify1 = string.Empty;
                string StrDateChange = "N";
                objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(objUpdateAppointment.AppointmentStatus_ID);
                //if (Request["rdoEApptPlace"] != null)
                //{
                //    if (Request["rdoEApptPlace"] == "PatientEname")
                //    {
                //        objUpdateAppointment.Aptplaceofservice = "C";
                //    }
                //    else
                //    {
                //        objUpdateAppointment.Aptplaceofservice = "P";
                //    }
                //}
                if (Request["hdnselectEother"] != null)
                {
                    //if (Request["hdnselectEother"] == "My Location")
                    //{
                    //    objUpdateAppointment.Aptplaceofservice = "P";
                    //}
                    //else if (Request["hdnselectEother"] == "Client Location")
                    //{
                    //    objUpdateAppointment.Aptplaceofservice = "C";
                    //}
                    if (Request["hdnselectEother"] == "New Location")
                    {
                        objUpdateAppointment.Aptplaceofservice = "O";
                        objUpdateAppointment.CourtLocationName = !string.IsNullOrEmpty(objUpdateAppointment.CourtLocationName) ? objUpdateAppointment.CourtLocationName : null;
                        objUpdateAppointment.defaultCoachAddress = objUpdateAppointment.ChkDefaultCourt == true ? "Y" : "N";
                    }
                    else
                    {
                        objUpdateAppointment.defaultCoachAddress = objUpdateAppointment.ChkDefaultCourt == true ? "Y" : "N";

                        objUpdateAppointment.Aptplaceofservice = "O";
                        objUpdateAppointment.otherAddressid = Convert.ToInt32(Request["EOtherAddressItemList"]);
                        objUpdateAppointment.CourtLocationName = !string.IsNullOrEmpty(objUpdateAppointment.CourtLocationName) ? objUpdateAppointment.CourtLocationName : null;

                    }
                }
                if (objUpdateAppointment.AppointmentStatus_ID == 1 || objUpdateAppointment.AppointmentStatus_ID == 3 || objUpdateAppointment.AppointmentStatus_ID == 5 || objUpdateAppointment.AppointmentStatus_ID == 7)
                {

                    ApptTime = Request["cboEtime_SelectedText"] + (Request["ddlETimeMer"] == "0" ? "AM" : "PM");

                    strStartDate = objUpdateAppointment.AppointmentDate;

                    if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) || !string.IsNullOrEmpty(objUpdateAppointment.weekEndDate))
                    {
                        if (Request["Hdnapt_date"] != strStartDate)
                        {
                            modify = "Y";
                            StrDateChange = "Y";
                        }
                    }
                    //if (!string.IsNullOrEmpty(Request["dayenddate"]) & Request["Recurring"] != "Weekly")
                    if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) & objUpdateAppointment.Recurring != "Weekly")
                    {
                        objUpdateAppointment.IntervalType_ID = "1";
                        if (!string.IsNullOrEmpty(objUpdateAppointment.DailyReccur))
                        {
                            strRecurenceValue = objUpdateAppointment.DailyReccur;
                            originalval = objUpdateAppointment.interdayvalue;
                            modifyval = objUpdateAppointment.DailyReccur;
                            if (originalval != modifyval)
                            {
                                modify = "N";
                                modify1 = "Y";
                            }
                        }
                        else
                        {
                            strRecurenceValue = objUpdateAppointment.interdayvalue;
                            //originalval = Request["interdayvalue"];
                            //modifyval = Request["interdayvalue"];
                            //if (originalval != modifyval)
                            //{
                            //    modify = "Y";
                            //}

                        }

                        if (objUpdateAppointment.Recurring == "Daily")
                        {
                            dtEndOn = objUpdateAppointment.EndDateForDaily;
                            originalDate = DateTime.Parse(objUpdateAppointment.dayEndDate);
                            modifDate = DateTime.Parse(objUpdateAppointment.EndDateForDaily);
                            if (originalDate != modifDate)
                            {
                                modify = "N";
                                modify1 = "Y";
                                //StrDateChange = "Y";
                            }
                        }
                        else
                        {
                            dtEndOn = objUpdateAppointment.dayEndDate;
                        }
                        SelectedDays = null;
                        //intEveryNo = Request["txtDailyRecur"];
                    }
                    ////else if (!string.IsNullOrEmpty(Request["dayenddate"]) & Request["Recurring"] == "Weekly")
                    //else if (!string.IsNullOrEmpty(objUpdateAppointment.EndDate) & objUpdateAppointment.Recurring == "Weekly")//Request["Recurring"] == "Weekly")
                    //{

                    //    //StrRecurrencetype_ID = "2";
                    //    objUpdateAppointment.IntervalType_ID = "2";

                    //    if (objUpdateAppointment.Recurring == "Weekly")//Request["Recurring"] == "Weekly")
                    //    {
                    //        dtEndOn = objUpdateAppointment.EndDateForWeek; //Request["txtEndDateforWeek"];
                    //        strRecurenceValue = objUpdateAppointment.WeeklyReccur; //Request["txtWeeklyRecur"];
                    //        modify = "Y";
                    //        modify1 = "Y";
                    //        //originalDate = DateTime.Parse(Request["weekenddate"]);
                    //        //modifDate = DateTime.Parse(dtEndOn);
                    //        //if (originalDate != modifDate)
                    //        //{
                    //        //    modify = "Y";
                    //        //}
                    //        //originalval = Request["interweekvalue"];
                    //        //modifyval = Request["txtWeeklyRecur"];
                    //        //if (originalval != modifyval)
                    //        //{
                    //        //    modify = "Y";
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        dtEndOn = objUpdateAppointment.EndDate; //Request["weekenddate"];
                    //        strRecurenceValue = objUpdateAppointment.IntervalValue; //Request["interweekvalue"];

                    //    }
                    //    SelectedDays = Request["Wselection"];
                    //    if (Request["chkweek"] != Request["Wselection"])
                    //    {
                    //        modify = "N";
                    //        modify1 = "Y";
                    //    }
                    //    SelectedDays = SelectedDays.Replace(",", "");
                    //}
                    //else if (!string.IsNullOrEmpty(Request["weekenddate"]) & Request["Recurring"] != "Daily")
                    else if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) & objUpdateAppointment.Recurring == "Weekly")
                    {

                        objUpdateAppointment.IntervalType_ID = "2";

                        if (objUpdateAppointment.Recurring == "Weekly")
                        {
                            dtEndOn = objUpdateAppointment.EndDateForWeek;
                            strRecurenceValue = objUpdateAppointment.WeeklyReccur;
                            modify = "Y";
                            modify1 = "Y";
                            //originalDate = DateTime.Parse(Request["weekenddate"]);
                            //modifDate = DateTime.Parse(dtEndOn);
                            //if (originalDate != modifDate)
                            //{
                            //    modify = "Y";
                            //}
                            //originalval = Request["interweekvalue"];
                            //modifyval = Request["txtWeeklyRecur"];
                            //if (originalval != modifyval)
                            //{
                            //    modify = "Y";
                            //}
                        }
                        else
                        {
                            dtEndOn = objUpdateAppointment.weekEndDate;
                            strRecurenceValue = objUpdateAppointment.interweekvalue;

                        }
                        SelectedDays = Request["Wselection"];
                        if (Request["chkweek"] != Request["Wselection"])
                        {
                            modify = "N";
                            modify1 = "Y";
                        }
                        SelectedDays = SelectedDays.Replace(",", "");
                    }
                    else if (!string.IsNullOrEmpty(objUpdateAppointment.weekEndDate) & objUpdateAppointment.Recurring != "Daily")
                    {
                        objUpdateAppointment.IntervalType_ID = "2";

                        if (objUpdateAppointment.Recurring == "Weekly")
                        {
                            dtEndOn = objUpdateAppointment.EndDateForWeek;
                            strRecurenceValue = objUpdateAppointment.WeeklyReccur;
                            originalDate = DateTime.Parse(objUpdateAppointment.weekEndDate);
                            modifDate = DateTime.Parse(objUpdateAppointment.EndDateForWeek);
                            if (originalDate != modifDate)
                            {
                                modify = "N";
                                modify1 = "Y";
                               // StrDateChange = "Y";
                            }
                            originalval = objUpdateAppointment.interweekvalue;
                            modifyval = objUpdateAppointment.WeeklyReccur;
                            if (originalval != modifyval)
                            {
                                modify = "N";
                                modify1 = "Y";
                            }
                        }
                        else
                        {
                            dtEndOn = objUpdateAppointment.weekEndDate;
                            strRecurenceValue = objUpdateAppointment.interweekvalue;

                        }
                        SelectedDays = Request["Wselection"];
                        if (Request["chkweek"] != Request["Wselection"])
                        {
                            modify = "N";
                            modify1 = "Y";
                        }
                        SelectedDays = SelectedDays.Replace(",", "");
                    }
                    //else if (!string.IsNullOrEmpty(Request["weekenddate"]) & Request["Recurring"] == "Daily")
                    else if (!string.IsNullOrEmpty(objUpdateAppointment.weekEndDate) & objUpdateAppointment.Recurring == "Daily")
                    {
                        objUpdateAppointment.IntervalType_ID = "1";
                        if (objUpdateAppointment.DailyReccur != "")
                        {
                            strRecurenceValue = objUpdateAppointment.DailyReccur;
                            modify = "N";
                            modify1 = "Y";
                            //originalval = Request["interdayvalue"];
                            //modifyval = Request["txtEditDailyRecur"];
                            //if (originalval != modifyval)
                            //{
                            //    modify = "Y";
                            //}
                        }
                        else
                        {
                            strRecurenceValue = objUpdateAppointment.interdayvalue;
                            //originalval = Request["interdayvalue"];
                            //modifyval = Request["interdayvalue"];
                            //if (originalval != modifyval)
                            //{
                            //    modify = "Y";
                            //}

                        }

                        if (objUpdateAppointment.Recurring == "Daily")
                        {
                            dtEndOn = objUpdateAppointment.EndDateForDaily;
                            //originalDate = DateTime.Parse(Request["dayenddate"]);
                            //modifDate = DateTime.Parse(Request["txtEndDateforDaily"]);
                            //if (originalDate != modifDate)
                            //{
                            //    modify = "Y";
                            //}
                        }
                        else
                        {
                            dtEndOn = objUpdateAppointment.dayEndDate;
                        }
                        SelectedDays = null;
                    }
                    //not recurrence
                    else
                    {
                        objUpdateAppointment.IntervalType_ID = null;
                        strRecurenceValue = null;
                        //  intEveryNo = null;
                        SelectedDays = null;
                        dtEndOn = null;
                    }


                }
                else if (objUpdateAppointment.AppointmentStatus_ID == 4)
                {
                    strStartDate = objUpdateAppointment.AppointmentDate;
                }
                if (objUpdateAppointment.ToReferenceLogin_ID != 0)
                {
                    objUpdateAppointment.ToReferenceLogin_ID = objUpdateAppointment.ToReferenceLogin_ID;
                }
                else
                {
                    objUpdateAppointment.ToReferenceLogin_ID = null;
                } //objUpdateAppointment.ToReference_ID = null;
                //}
                if (objUpdateAppointment.ToReference_ID != 0)
                {
                    objUpdateAppointment.ToReference_ID = objUpdateAppointment.ToReference_ID;
                }
                else
                {
                    objUpdateAppointment.ToReference_ID = null;
                }
                if (objUpdateAppointment.AppointmentID != null)
                {
                    objUpdateAppointment.Appointment_ID = objUpdateAppointment.AppointmentID;
                }
                else
                {
                    objUpdateAppointment.Appointment_ID = null;
                }
                if (objUpdateAppointment.AppointmentTracking_ID != 0)
                {
                    objUpdateAppointment.AppointmentTracking_ID = objUpdateAppointment.AppointmentTracking_ID;
                }
                else
                {
                    objUpdateAppointment.AppointmentTracking_ID = null;
                }
                objUpdateAppointment.FromReferenceType_ID = 2;
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
                }
                else
                {
                    objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                }
                objUpdateAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
                objUpdateAppointment.ToReferenceType_ID = 3;

                if (objUpdateAppointment.FacilityReference_ID != 0)
                {
                    objUpdateAppointment.FacilityReference_ID = objUpdateAppointment.FacilityReference_ID;
                }
                else
                {
                    objUpdateAppointment.FacilityReference_ID = null;
                }

                if (!string.IsNullOrEmpty(objUpdateAppointment.Editfutreapnt_Ind))
                {
                    if (objUpdateAppointment.Editfutreapnt_Ind == "Y")
                    {
                        objUpdateAppointment.Editfutreapnt_Ind = "Y";
                        objUpdateAppointment.IsValidate_Ind = "Y";
                    }
                    else if (objUpdateAppointment.Editfutreapnt_Ind == "N")
                    {
                        objUpdateAppointment.Editfutreapnt_Ind = "N";
                        objUpdateAppointment.IsValidate_Ind = "Y";
                    }
                    else if (objUpdateAppointment.Editfutreapnt_Ind == "Null")
                    {
                        objUpdateAppointment.Editfutreapnt_Ind = null;
                        objUpdateAppointment.IsValidate_Ind = "Y";
                    }
                    else
                    {
                        objUpdateAppointment.Editfutreapnt_Ind = null;
                        objUpdateAppointment.IsValidate_Ind = "Y";
                    }
                }
                else
                {
                    objUpdateAppointment.Editfutreapnt_Ind = "N";
                    objUpdateAppointment.IsValidate_Ind = "N";
                }
                objUpdateAppointment.GroupNo = null;
                objUpdateAppointment.GroupInd = "N";
                objUpdateAppointment.IntervalValue = strRecurenceValue;
                objUpdateAppointment.NumofOccurrences = null;

                if (objUpdateAppointment.AppointmentRecurrence_ID != 0)
                {
                    objUpdateAppointment.AppointmentRecurrence_ID = objUpdateAppointment.AppointmentRecurrence_ID;
                }
                else
                {
                    objUpdateAppointment.AppointmentRecurrence_ID = null;
                }

                if (string.IsNullOrEmpty(modify))
                {
                    objUpdateAppointment.StartDate = !string.IsNullOrEmpty(objUpdateAppointment.StartDate) ? objUpdateAppointment.StartDate : strStartDate;
                }
                else if (modify1 == "Y")
                {
                           if (StrDateChange == "Y")
                        {
                            objUpdateAppointment.StartDate = strStartDate;
                        }
                   else{
                        objUpdateAppointment.StartDate = !string.IsNullOrEmpty(objUpdateAppointment.StartDate) ? objUpdateAppointment.StartDate : strStartDate;          
                       }
                }
                else
                {
                    objUpdateAppointment.StartDate =  strStartDate;
                }
                objUpdateAppointment.VoiceFileName = !string.IsNullOrEmpty(Request["VoiceFileName"]) ? Request["VoiceFileName"] : null;
                objUpdateAppointment.EndDate = dtEndOn;
                objUpdateAppointment.SelectedWeekdays = SelectedDays;
                objUpdateAppointment.Notes = Sanitizer.GetSafeHtmlFragment(objUpdateAppointment.Notes);
                objUpdateAppointment.AppointmentTime = ApptTime;
                objUpdateAppointment.Duration = Convert.ToInt32(Request["ddlEDuration"]);
                if (objUpdateAppointment.ApptType == "Client")
                {
                    objUpdateAppointment.AppointmentType_ID = 1;
                }
                else
                {
                    objUpdateAppointment.AppointmentType_ID = 2;
                }
                if (Convert.ToString(Session["roleid"]) != "39")
                {
                    objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                }
                else
                {
                    objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["Techlog_id"]);
                }
                objUpdateAppointment.Edit_Ind = "Y";
                string reflogin = string.Empty;
                string refeId = string.Empty;
                //if (Request["combobox5"] != null & Request["combobox5"] != "")
                //{
                //    string techids = string.Join(",", Request["combobox5"].Split(',').Distinct().ToArray());
                //    if (techids.EndsWith(","))
                //    {
                //        objUpdateAppointment.Technician_ids = "," + techids;
                //    }
                //    else
                //    {
                //        objUpdateAppointment.Technician_ids = "," + techids + ",";
                //    }
                //}
                //else
                //{
                //    objUpdateAppointment.Technician_ids = "," + Request["hdntecid1"].ToString() + ",";
                //}
                //if (Request["hdntechpos"] != null & Request["hdntechpos"] != "")
                //{
                //    string techpos = string.Join(",", Request["hdntechpos"].Split(',').Distinct().ToArray());
                //    if (techpos.EndsWith(","))
                //    {
                //        objUpdateAppointment.TechnicianPositions = "," + techpos;
                //    }
                //    else
                //    {
                //        objUpdateAppointment.TechnicianPositions = "," + techpos + ",";
                //    }
                //    //objUpdateAppointment.TechnicianPositions = Request["hdntechpos"];
                //}
                //else
                //{
                //    objUpdateAppointment.TechnicianPositions ="," +"0"+",";
                //}
                GetAppointment onjcheck = new GetAppointment();
                string strDates = "";

                if (objUpdateAppointment.Recurring == "No Recurring")
                {
                    strDates = DateTime.Parse(strStartDate).ToShortDateString();
                }
                else if (objUpdateAppointment.Recurring == "Daily")
                {
                    while (DateTime.Parse(strStartDate) <= DateTime.Parse(dtEndOn))
                    {
                        if (string.IsNullOrEmpty(strDates))
                        {
                            strDates = DateTime.Parse(strStartDate).ToShortDateString();
                        }
                        else
                        {
                            strDates += "," + DateTime.Parse(strStartDate).ToShortDateString();
                        }
                        //StartDate = DateAndTime.DateAdd(DateInterval.Day, Convert.ToInt32(txtDailyRecur.Text), appatdate);
                        strStartDate = DateTime.Parse(strStartDate).AddDays(Convert.ToInt32(strRecurenceValue)).ToShortDateString();
                    }
                }
                else if (objUpdateAppointment.Recurring == "Weekly")
                {
                    strDates = onjcheck.weeklyApptDates(strStartDate, strRecurenceValue, dtEndOn, SelectedDays);

                }
                //else
                //{
                //    if (dtEndOn != null)
                //    {

                //        while (DateTime.Parse(strStartDate) <= DateTime.Parse(dtEndOn))
                //        {
                //            if (string.IsNullOrEmpty(strDates))
                //            {
                //                strDates = DateTime.Parse(strStartDate).ToShortDateString();
                //            }
                //            else
                //            {
                //                strDates += "," + DateTime.Parse(strStartDate).ToShortDateString();
                //            }
                //            //StartDate = DateAndTime.DateAdd(DateInterval.Day, Convert.ToInt32(txtDailyRecur.Text), appatdate);
                //            strStartDate = DateTime.Parse(strStartDate).AddDays(Convert.ToInt32(strRecurenceValue)).ToShortDateString();
                //        }
                //    }
                //    else
                //    {
                //        strDates = DateTime.Parse(strStartDate).ToShortDateString();
                //    }
                //}
                string torefid = Convert.ToString(objUpdateAppointment.ToReference_ID);

                if (modify == "Y")
                {
                    strMessage = onjcheck.checkApptExist(strDates, torefid, ApptTime, Convert.ToString(Session["roleid"] != (object)1 ? Session["Prov_ID"] : Session["ComboProv_ID"]), Convert.ToString(objUpdateAppointment.AppointmentRecurrence_ID));
                }
                if (strMessage == "Y")
                {
                    if (objUpdateAppointment.ApptType == "Client")
                    {
                        strMessage = "1";
                    }
                    else
                    {
                        strMessage = "2";
                    }
                    strMessage = strMessage.Replace("<br>", "\r\n");
                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                }

                if (ApptTime != Request["strTime"])
                {

                    strMessage = onjcheck.checkApptExist(strDates, torefid, ApptTime, Convert.ToString(Session["roleid"] != (object)1 ? Session["Prov_ID"] : Session["ComboProv_ID"]), Convert.ToString(objUpdateAppointment.AppointmentRecurrence_ID));
                    if (strMessage == "Y")
                    {
                        if (objUpdateAppointment.ApptType == "Client")
                        {
                            strMessage = "1";
                        }
                        else
                        {
                            strMessage = "2";
                        }
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                    }
                }
                if (modify != "Y" & StrDateChange=="N")
                {
                    strMessage = objUpdateAppointment.EditAppointment(objUpdateAppointment, ref strFuturemessage, ref strChangeRecur_Ind);
                }
                else
                {
                    //AddAppointmentModel objAddAppt = new AddAppointmentModel();

                    if (objUpdateAppointment.ToReferenceLogin_ID == null)
                    {
                        reflogin = "0";
                    }
                    else
                    {
                        reflogin = objUpdateAppointment.ToReferenceLogin_ID.ToString();
                    }
                    if (objUpdateAppointment.ToReference_ID == null)
                    {
                        refeId = "0";
                    }
                    else
                    {
                        refeId = objUpdateAppointment.ToReference_ID.ToString();
                    }
                  //  objUpdateAppointment.ToReference_IDs = reflogin + "," + refeId + "," + Convert.ToString(objUpdateAppointment.ToReferenceType_ID) + ",$";
                    objUpdateAppointment.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    objUpdateAppointment.PlaceOfService_ID = Convert.ToInt32(Session["PlaceOfService_ID"]);
                    //objUpdateAppointment.Autoreminder_Ind = "N";
                   // objUpdateAppointment.phoneAutoreminder_Ind = "N";
                    if (objUpdateAppointment.Editfutreapnt_Ind == "N")
                    {
                        objUpdateAppointment.IsValidate_Ind = "N";
                        objUpdateAppointment.Edit_Ind = "N";
                        objUpdateAppointment.Editfutreapnt_Ind = "N";
                    }
                    if (objUpdateAppointment.ApptType == "Client")
                    {
                       // objUpdateAppointment.Autoreminder_Ind = "Y";
                        objUpdateAppointment.AppointmentType_ID = 1;
                        objUpdateAppointment.AppointmentStatus_ID = 1;
                    }
                    else
                    {
                        objUpdateAppointment.AppointmentType_ID = 2;
                        objUpdateAppointment.AppointmentStatus_ID = 2;
                    }
                    //if (Convert.ToInt32(TempData["ApptType"]) == 1)
                    //{
                    //    objUpdateAppointment.Autoreminder_Ind = "Y";

                    //}
                    //else
                    //{
                    //    objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(TempData["ApptType"]);
                    //}
                    //GetAppointment onjcheck1 = new GetAppointment();
                    //string appatdate1 = strStartDate;
                    //if (Request["Recurring"] != null)
                    //{
                    //    appatdate1 = strDates;
                    //}
                    //string torefid1 = Request["hidPatientID"];
                    //string apptime1 = ApptTime;
                    //string prId1;
                    //if (Session["roleid"].ToString() != "1")
                    //{
                    //    prId1 = Session["Prov_ID"].ToString();
                    //}
                    //else
                    //{
                    //    prId1 = Session["ComboProv_ID"].ToString();
                    //}

                    //string apptrecid = string.Empty;

                    //if (recId != 0)
                    //{
                    //    apptrecid = Convert.ToString(recId);
                    //}
                    //strMessage = onjcheck1.checkApptExist(appatdate1, torefid1, apptime1, prId1, apptrecid);
                    //if (strMessage == "Y")
                    //{
                    //    if (strMessage == "Y")
                    //    {
                    //        if (Request["ApptType"] == "Client")
                    //        {
                    //            strMessage = "1";
                    //        }
                    //        else
                    //        {
                    //            strMessage = "2";
                    //        }
                    //        strMessage = strMessage.Replace("<br>", "\r\n");
                    //        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                    //    }
                    //}
                    // else
                    // {
                    if (Request["cancelappt"] != "Y")
                    {
                        strMessage = objUpdateAppointment.InsertAppointment(objUpdateAppointment, null);
                    }
                    if (strMessage == "")
                    {
                        if (objUpdateAppointment.ApptType == "Client")
                        {
                            DeleteRecurrenceappts(objUpdateAppointment.AppointmentRecurrence_ID);
                        }
                    }
                    // }
                }
                if (!string.IsNullOrEmpty(strMessage))
                {
                    strMessage = strMessage.Replace("<br>", "\r\n");
                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (string.IsNullOrEmpty(strMessage) & (!string.IsNullOrEmpty(objUpdateAppointment.cellphone)))
                    {
                        if (ApptTime != Request["strTime"] || strStartDate != Request["Hdnapt_date"])
                        {
                            if (objUpdateAppointment.ApptType == "Client")
                            {
                                string strbussinessname = string.Empty;
                                string strproviderphone = string.Empty;

                                if (!string.IsNullOrEmpty(objUpdateAppointment.cellphone))
                                {
                                    objUpdateAppointment.cellphone = objUpdateAppointment.cellphone.Replace("-", "");
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
                                {
                                    strbussinessname = Convert.ToString(Session["PracticeName"]);
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
                                {
                                    strproviderphone = Convert.ToString(Session["Providerphone"]);
                                }

                                clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, objUpdateAppointment.cellphone, strStartDate, ApptTime, null, 0);
                            }
                        }
                    }

                    return Json(JsonResponseFactory.SuccessResponse(objUpdateAppointment), JsonRequestBehavior.AllowGet);
                }                
                //return null;
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public void DeleteRecurrenceappts(int? ApptRecurrID)
        {

            try
            {
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_AppointmentRecurrence_ID",ApptRecurrID),
                                           new SqlParameter("@in_UpdatedBy",Convert.ToInt32(Session["UserID"]))
                                      };
                clsDBWrapper obj = new clsDBWrapper();
                obj.AddInParameters(insparam);
                obj.ExecuteNonQuerySP("help_dbo.st_Appointment_DEL_RecurrenceSchedule");
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "DeleteRecurrenceappts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //[HttpPost()]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken()]
        //public JsonResult EditAppointment1(Models.AddAppointmentModel objUpdateAppointment)
        //{
        //    if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            //string ApptTime = "";
        //            //string Duration = "";
        //            //string strStartDate = "";
        //            //string Validation_Ind = "N";
        //            //string strmin = null;
        //            //string strMessage = null;
        //            //string apptstatus = null;
        //            //string modify = string.Empty;

        //            string strFuturemessage = null;
        //            string strChangeRecur_Ind = "";
        //            //string strappttime1 = null;
        //            //string strTimeDifference = "";
        //            string ApptTime = "";
        //            string Duration = "";
        //            //    string intEveryNo = "";
        //            string strStartDate = "";
        //            string dtEndOn = "";
        //            string SelectedDays = "";
        //            string StrRecurrencetype_ID = "";
        //            string strRecurenceValue = "";
        //            string Validation_Ind = "N";
        //            string strInd = "N";
        //            string strmin = null;
        //            string strMessage = null;
        //            string apptstatus = null;
        //            DateTime originalDate;
        //            DateTime modifDate;
        //            string originalval = string.Empty;
        //            string modifyval = string.Empty;
        //            string modify = string.Empty;
        //            string modify1 = string.Empty;       
        //            if (Request["apptstatus"] != null)
        //            {
        //                apptstatus = Request["apptstatus"];
        //            }
        //            //if (Request["rdoEApptPlace"] != null)
        //            //{
        //            //    if (Request["rdoEApptPlace"] == "PatientEname")
        //            //    {
        //            //        objUpdateAppointment.Aptplaceofservice = "C";
        //            //    }
        //            //    else
        //            //    {
        //            //        objUpdateAppointment.Aptplaceofservice = "P";
        //            //    }
        //            //}
        //            if (Request["hdnselectEother"] != null)
        //            {
        //                if (Request["hdnselectEother"] == "My Location")
        //                {
        //                    objUpdateAppointment.Aptplaceofservice = "P";
        //                }
        //                else if (Request["hdnselectEother"] == "Client Location")
        //                {
        //                    objUpdateAppointment.Aptplaceofservice = "C";
        //                }
        //                else if (Request["hdnselectEother"] == "Other")
        //                {
        //                    objUpdateAppointment.Aptplaceofservice = "O";
        //                    objUpdateAppointment.otherAddress = Request["EotherAddress1"];
        //                    objUpdateAppointment.zip = Request["txtEZip"];
        //                    objUpdateAppointment.cityid = (Convert.ToInt32(Request["DDECity"]) != 0 ? Convert.ToInt32(Request["DDECity"]) : 0);
        //                    objUpdateAppointment.stateid = (Convert.ToInt32(Request["DDEState"]) != 0 ? Convert.ToInt32(Request["DDEState"]) : 0);
        //                }
        //                else
        //                {
        //                    objUpdateAppointment.Aptplaceofservice = "O";
        //                    objUpdateAppointment.otherAddressid = Convert.ToInt32(Request["EOtherAddressItemList"]);
        //                }
        //            }
        //            if (apptstatus == "1" || apptstatus == "3" || apptstatus == "5" || apptstatus == "7")
        //            {
        //                if (Request["ddlEmin2"] == "0")
        //                {
        //                    strmin = "00";
        //                }
        //                else { strmin = Request["ddlEmin2"]; }
        //                string strAP = null;
        //                if (Request["ddlETimeMer"] == "0")
        //                {
        //                    strAP = "AM";
        //                }
        //                else
        //                {
        //                    strAP = "PM";
        //                }
        //                //ApptTime = Request["ddlEhr2"] + ":" + strmin + strAP;
        //                ApptTime = Request["cboEtime_SelectedText"] + strAP;
        //                Duration = Request["ddlEDuration"];
        //                strStartDate = Request["txtEApptDate"];
        //            }
        //            else if (apptstatus == "4")
        //            {
        //                strStartDate = Request["txtEApptDate"];
        //            }
        //            if (Request["hidPatientLoginID"] != null && Request["hidPatientLoginID"] != "0")
        //            {
        //                objUpdateAppointment.ToReferenceLogin_ID = Convert.ToInt32(Request["hidPatientLoginID"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.ToReferenceLogin_ID = null;
        //            }
        //            if (Request["hidPatientID"] != null && Request["hidPatientID"] != "0")
        //            {
        //                objUpdateAppointment.ToReference_ID = Convert.ToInt32(Request["hidPatientID"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.ToReference_ID = null;
        //            }
        //            if (Request["ApptId"] != null)
        //            {
        //                objUpdateAppointment.Appointment_ID = Convert.ToInt32(Request["ApptId"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.Appointment_ID = null;
        //            }

        //            objUpdateAppointment.FromReferenceType_ID = 2;
        //            if (Session["roleid"].ToString() != "1")
        //            {
        //                objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
        //            }
        //            objUpdateAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
        //            objUpdateAppointment.ToReferenceType_ID = 3;
        //            if (Request["hdnFacRef_ID"] != null)
        //            {
        //                objUpdateAppointment.FacilityReference_ID = Convert.ToInt32(Request["hdnFacRef_ID"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.FacilityReference_ID = null;
        //            }
        //            int recId;
        //            recId = 0;
        //            objUpdateAppointment.StartDate = strStartDate;
        //            objUpdateAppointment.EndDate = null;
        //            objUpdateAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtENotes"]);
        //            objUpdateAppointment.AppointmentTime = ApptTime;
        //            objUpdateAppointment.Duration = Convert.ToInt32(Duration);
        //            objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(apptstatus);

        //            if (Session["roleid"].ToString() != "39")
        //            {
        //                objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["UserID"]);
        //            }
        //            else
        //            {
        //                objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["Techlog_id"]);
        //            }
        //            objUpdateAppointment.IsValidate_Ind = Validation_Ind;
        //            if (Request["combobox5"] != null & Request["combobox5"] != "")
        //            {
        //                string techids = string.Join(",", Request["combobox5"].Split(',').Distinct().ToArray());
        //                if (techids.StartsWith(","))
        //                {
        //                }
        //                else
        //                {
        //                    techids = "," + techids;
        //                }
        //                if (techids.EndsWith(","))
        //                {
        //                    objUpdateAppointment.Technician_ids = techids;
        //                }
        //                else
        //                {
        //                    objUpdateAppointment.Technician_ids = techids + ",";
        //                }
        //            }
        //            else
        //            {
        //                objUpdateAppointment.Technician_ids = "," + Request["hdntecid1"].ToString() + ",";
        //            }
        //                if (Request["hdntechpos"] != null & Request["hdntechpos"] != "")
        //                {
        //                    string techpos = string.Join(",", Request["hdntechpos"].Split(',').Distinct().ToArray());
        //                    if (techpos.EndsWith(","))
        //                    {
        //                        objUpdateAppointment.TechnicianPositions = "," + techpos;
        //                    }
        //                    else
        //                    {
        //                        objUpdateAppointment.TechnicianPositions = "," + techpos + ",";
        //                    }
        //                    //objUpdateAppointment.TechnicianPositions = Request["hdntechpos"];
        //                }
        //                else
        //                {
        //                    objUpdateAppointment.TechnicianPositions = "," + "0" + ",";
        //                }
        //            GetAppointment onjcheck = new GetAppointment();
        //            string appatdate = strStartDate;
        //            string torefid = Convert.ToString(objUpdateAppointment.ToReference_ID);
        //            string apptime = Convert.ToString(objUpdateAppointment.AppointmentTime);
        //            string prId;

        //                prId = Session["Prov_ID"].ToString();
        //            if (modify == "Y")
        //            {
        //                string apptrecid = string.Empty;

        //                if (recId != 0)
        //                {
        //                    apptrecid = Convert.ToString(recId);
        //                }

        //                strMessage = onjcheck.checkApptExist(appatdate, torefid, apptime, prId, apptrecid);
        //            }
        //            if (strMessage == "Y")
        //            {
        //                if (strMessage == "Y")
        //                {
        //                    if (Request["ApptType"] == "Client")
        //                    {
        //                        strMessage = "1";
        //                    }
        //                    else
        //                    {
        //                        strMessage = "2";
        //                    }
        //                    strMessage = strMessage.Replace("<br>", "\r\n");
        //                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
        //                }
        //            }
        //            if (ApptTime != Request["strTime"] || appatdate != Request["Hdnapt_date"])
        //            {

        //                strMessage = onjcheck.checkApptExist(appatdate, torefid, apptime, prId, string.Empty);
        //                if (strMessage == "Y")
        //                {
        //                    if (strMessage == "Y")
        //                    {
        //                        if (Request["ApptType"] == "Client")
        //                        {
        //                            strMessage = "1";
        //                        }
        //                        else
        //                        {
        //                            strMessage = "2";
        //                        }
        //                        strMessage = strMessage.Replace("<br>", "\r\n");
        //                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
        //                    }
        //                }
        //            }
        //            string strFuturemessage = null;
        //            string strChangeRecur_Ind = "";
        //            if (modify != "Y")
        //            {
        //                strMessage = objUpdateAppointment.EditAppointment(objUpdateAppointment, ref strFuturemessage, ref strChangeRecur_Ind);
        //            }

        //            if (!string.IsNullOrEmpty(strMessage))
        //            {
        //                strMessage = strMessage.Replace("<br>", "\r\n");
        //                return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
        //            }
        //            else
        //            {
        //                return Json(JsonResponseFactory.SuccessResponse(objUpdateAppointment), JsonRequestBehavior.DenyGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
        //    }
        //    //return null;
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public JsonResult GetOtherAddressDetails(int? AppAddressId, int? custId)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                Patient_Info otherdetails = new Patient_Info();
                //clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = {
		new SqlParameter("@In_Provider_id",Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"])),
        new SqlParameter("@In_customer_id",custId!=0?custId:null),
        new SqlParameter("In_Appointment_address_id",AppAddressId!=0?AppAddressId:null)
	};
                clscommon.AddInParameters(objparam);
                SqlDataReader objread = default(SqlDataReader);
                objread = clscommon.GetDataReader("Help_dbo.St_get_appointment_adress");
                while (objread.Read())
                {
                    //if (objread["Address1"].ToString().Contains(','))
                    //{

                    //    if (objread["Address1"] != null)
                    //    {
                    //        string[] strsplitaddress1 = objread["Address1"].ToString().Split(',');
                    //        if (strsplitaddress1.Length >= 3)
                    //        {
                    //            ViewBag.changedAddress1 = strsplitaddress1[0];
                    //            otherdetails.Address1 = strsplitaddress1[0];
                    //            otherdetails.CourtLocationName = Convert.ToString(strsplitaddress1[2]);
                    //            if (objread["Default_address"] != null)
                    //            {
                    //                otherdetails.Default_addr = Convert.ToString(objread["Default_address"]);
                    //            }
                    //            else
                    //            {
                    //                otherdetails.Default_addr = null;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    otherdetails.Address1 = Convert.ToString(objread["Address1"]);
                    //    otherdetails.CourtLocationName = "";
                    //}
                    //otherdetails.Address1 = Convert.ToString(objread["Address1"]);
                    if (objread["Address1"] != null)
                    {
                        otherdetails.Address1 = Convert.ToString(objread["Address1"]);
                    }
                    if (objread["CourtLocationName"] != null)
                    {
                        otherdetails.CourtLocationName = Convert.ToString(objread["CourtLocationName"]);
                    }
                    if (objread["Default_address"] != null)
                    {
                        otherdetails.Default_addr = Convert.ToString(objread["Default_address"]);
                    }
                    else
                    {
                        otherdetails.Default_addr = null;
                    }
                    otherdetails.cityzip = Convert.ToString(objread["cityzip"]);
                    otherdetails.State = Convert.ToString(objread["statename"]);
                }
                return Json(otherdetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
        }
        //public FileContentResult Transactionchargepdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    pdf.Create();
        //    StringBuilder strhtml = new StringBuilder();

        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getchargepdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Charges.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    return null;
        //}
        //private void Getchargepdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {

        //        RPBilling objchgBill = new RPBilling();
        //        if (!string.IsNullOrEmpty(palgnid))
        //        {
        //            objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //            ViewBag.PlId = objchgBill.PatientLogin_ID;
        //        }
        //        else
        //        {
        //            objchgBill.PatientLogin_ID = null;
        //            ViewBag.PlId = null;
        //        }
        //        objchgBill.ReferenceType_ID = null;
        //        if (fromdate == "")
        //        {
        //            fromdate = null;
        //        }
        //        if (todate == "")
        //        {
        //            todate = null;
        //        }
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            objchgBill.FromDate = fromdate;
        //        }
        //        else
        //        {
        //            objchgBill.FromDate = null;
        //        }
        //        if (!string.IsNullOrEmpty(todate))
        //        {
        //            objchgBill.ToDate = todate;
        //        }
        //        else
        //        {
        //            objchgBill.ToDate = null;
        //        }

        //        objchgBill.AuthorizedPatientLoginID = null;

        //        if (sort != null & sort != "")
        //        {
        //            objchgBill.OrderbyItem = sort.ToString();
        //        }
        //        else
        //        {
        //            objchgBill.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null & sortdir != "")
        //        {
        //            objchgBill.orderBy = sortdir.ToString();

        //        }
        //        else
        //        {
        //            objchgBill.orderBy = "DESC";
        //        }
        //        objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objchgBill.PageNo = "1";
        //        objchgBill.NoOfRecords = noofrecords;
        //        List<RPBilling> chglist = new List<RPBilling>();
        //        chglist = objchgBill.GetPatientChargeslist(objchgBill);

        //        //  double height = 100;
        //        double pos3 = 300;

        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (chglist.Count > 0)
        //        {
        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //            //strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in chglist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                //  strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                //  strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";
        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Charges Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }

        //        //height = pdf.GetTextHeight(strhtml);
        //        pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Getchargepdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public void Transactionchargeexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {

        //        RPBilling objchgBill = new RPBilling();
        //        if (!string.IsNullOrEmpty(palgnid))
        //        {
        //            objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //            ViewBag.PlId = objchgBill.PatientLogin_ID;
        //        }
        //        else
        //        {
        //            objchgBill.PatientLogin_ID = null;
        //            ViewBag.PlId = null;
        //        }
        //        objchgBill.ReferenceType_ID = null;
        //        if (fromdate == "")
        //        {
        //            fromdate = null;
        //        }
        //        if (todate == "")
        //        {
        //            todate = null;
        //        }
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            objchgBill.FromDate = fromdate;
        //        }
        //        else
        //        {
        //            objchgBill.FromDate = null;
        //        }
        //        if (!string.IsNullOrEmpty(todate))
        //        {
        //            objchgBill.ToDate = todate;
        //        }
        //        else
        //        {
        //            objchgBill.ToDate = null;
        //        }

        //        objchgBill.AuthorizedPatientLoginID = null;

        //        if (sort != null & sort != "")
        //        {
        //            objchgBill.OrderbyItem = sort.ToString();
        //        }
        //        else
        //        {
        //            objchgBill.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null & sortdir != "")
        //        {
        //            objchgBill.orderBy = sortdir.ToString();

        //        }
        //        else
        //        {
        //            objchgBill.orderBy = "DESC";
        //        }
        //        objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objchgBill.PageNo = "1";
        //        objchgBill.NoOfRecords = noofrecords;
        //        List<RPBilling> chglist = new List<RPBilling>();
        //        chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (chglist.Count > 0)
        //        {
        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //            //strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in chglist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                //  strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                //  strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";
        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Charges Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", "attachment; filename=charges.xls");
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Write(strhtml);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Transactionchargeexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public FileContentResult Transactionpaymentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    pdf.Create();
        //    var strhtml = new StringBuilder();

        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getpaymentpdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Payments.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    return null;
        //}

        //public void Getpaymentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //        RPBilling objAdj = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //            ViewBag.plId = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            objAdj.PatientLogin_ID = null;
        //        }
        //        objAdj.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objAdj.FromDate = fromdate;
        //                objAdj.ToDate = todate;

        //            }
        //            else
        //            {
        //                objAdj.FromDate = null;
        //                objAdj.ToDate = null;
        //            }
        //        }
        //        else
        //        {
        //            objAdj.FromDate = null;
        //            objAdj.ToDate = null;
        //        }

        //        objAdj.AuthorizedPatientLoginID = null;
        //        if (sort != null & sort != "")
        //        {
        //            objAdj.OrderbyItem = sort.ToString();

        //        }
        //        else
        //        {
        //            objAdj.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null & sortdir != "")
        //        {
        //            objAdj.orderBy = sortdir.ToString();

        //        }
        //        else
        //        {
        //            objAdj.orderBy = "DESC";
        //        }
        //        objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objAdj.PageNo = "1";

        //        objAdj.NoOfRecords = noofrecords;
        //        List<RPBilling> Adjlist = new List<RPBilling>();
        //        Adjlist = objAdj.GetPatientPaymentList(objAdj);
        //        //  double height = 100;
        //        double pos3 = 300;
        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";

        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }
        //        //height = pdf.GetTextHeight(strhtml);
        //        pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Getpaymentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public void Transactionpaymentexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //        RPBilling objAdj = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //            ViewBag.plId = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            objAdj.PatientLogin_ID = null;
        //        }
        //        objAdj.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objAdj.FromDate = fromdate;
        //                objAdj.ToDate = todate;

        //            }
        //            else
        //            {
        //                objAdj.FromDate = null;
        //                objAdj.ToDate = null;
        //            }
        //        }
        //        else
        //        {
        //            objAdj.FromDate = null;
        //            objAdj.ToDate = null;
        //        }

        //        objAdj.AuthorizedPatientLoginID = null;
        //        if (sort != null & sort != "")
        //        {
        //            objAdj.OrderbyItem = sort.ToString();

        //        }
        //        else
        //        {
        //            objAdj.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null & sortdir != "")
        //        {
        //            objAdj.orderBy = sortdir.ToString();

        //        }
        //        else
        //        {
        //            objAdj.orderBy = "DESC";
        //        }
        //        objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objAdj.PageNo = "1";

        //        objAdj.NoOfRecords = noofrecords;
        //        List<RPBilling> Adjlist = new List<RPBilling>();
        //        Adjlist = objAdj.GetPatientPaymentList(objAdj);
        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";

        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", "attachment; filename=payments.xls");
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Write(strhtml);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Transactionpaymentexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}

        //public FileContentResult Transactionadjustmentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    pdf.Create();
        //    var strhtml = new StringBuilder();

        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getadjustmentpdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Adjustments.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    return null;
        //}

        //public void Getadjustmentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //        RPBilling objpmt = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objpmt.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            objpmt.PatientLogin_ID = null;
        //        }
        //        objpmt.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objpmt.FromDate = fromdate;
        //                objpmt.ToDate = todate;
        //            }
        //            else
        //            {
        //                objpmt.FromDate = null;
        //                objpmt.ToDate = null;
        //            }
        //        }
        //        objpmt.AuthorizedPatientLoginID = null;
        //        if (sort != null)
        //        {
        //            objpmt.OrderbyItem = Request["sort"].ToString();

        //        }
        //        else
        //        {
        //            objpmt.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null)
        //        {
        //            objpmt.orderBy = Request["sortdir"].ToString();

        //        }
        //        else
        //        {
        //            objpmt.orderBy = "DESC";
        //        }
        //        objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        // objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //        objpmt.PageNo = "1";


        //        objpmt.NoOfRecords = noofrecords;

        //        List<RPBilling> Adjlist = new List<RPBilling>();
        //        Adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //        //  double height = 100;
        //        double pos3 = 300;
        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Adjustments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by/Charged by" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Adjustment type" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";

        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Adjustments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }
        //        // height = pdf.GetTextHeight(strhtml);
        //        pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Getadjustmentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public void Transactionadjustmentexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //        RPBilling objpmt = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objpmt.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            objpmt.PatientLogin_ID = null;
        //        }
        //        objpmt.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objpmt.FromDate = fromdate;
        //                objpmt.ToDate = todate;
        //            }
        //            else
        //            {
        //                objpmt.FromDate = null;
        //                objpmt.ToDate = null;
        //            }
        //        }
        //        objpmt.AuthorizedPatientLoginID = null;
        //        if (sort != null)
        //        {
        //            objpmt.OrderbyItem = Request["sort"].ToString();

        //        }
        //        else
        //        {
        //            objpmt.OrderbyItem = "Transaction_Date";
        //        }
        //        if (sortdir != null)
        //        {
        //            objpmt.orderBy = Request["sortdir"].ToString();

        //        }
        //        else
        //        {
        //            objpmt.orderBy = "DESC";
        //        }
        //        objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        // objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //        objpmt.PageNo = "1";


        //        objpmt.NoOfRecords = noofrecords;

        //        List<RPBilling> Adjlist = new List<RPBilling>();
        //        Adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //        //  double height = 100;
        //        // double pos3 = 300;
        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Adjustments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by/Charged by" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Adjustment type" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";

        //        }
        //        else
        //        {
        //            strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Adjustments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", "attachment; filename=adjustments.xls");
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Write(strhtml);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Transactionadjustmentexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public FileContentResult Alltransactionspdf(string fromdate, string todate, string palgnid, string chtotrec, string chsort, string chsortdir, string pasort, string pasortdir, string patotrec, string adjsort, string adjsortdir, string adjtotrec, string hdnpatid)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    pdf.Create();
        //    var strhtml = new StringBuilder();
        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getalltranspdf(fromdate, todate, palgnid, chtotrec, chsort, chsortdir, pasort, pasortdir, patotrec, adjsort, adjsortdir, adjtotrec, hdnpatid);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Alltransactions.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    return null;
        //}

        //public void Getalltranspdf(string fromdate, string todate, string palgnid, string chtotrec, string chsort, string chsortdir, string pasort, string pasortdir, string patotrec, string adjsort, string adjsortdir, string adjtotrec, string hdnpatid)
        //{
        //    try
        //    {
        //        int? palgnid1;
        //        if (palgnid != "" & palgnid != null)
        //        {
        //            palgnid1 = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            palgnid1 = null;
        //        }
        //        //clsCommonFunctions objcomm = new clsCommonFunctions();
        //        Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
        //        objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Convert.ToString(Session["Prov_ID"])));
        //        if ((objInfo != null))
        //        {
        //            if (!string.IsNullOrEmpty(objInfo.FirstName))
        //            {
        //                ViewBag.FirstName = objInfo.FirstName;
        //            }
        //            else
        //            {
        //                ViewBag.FirstName = null;
        //            }
        //            if (!string.IsNullOrEmpty(objInfo.LastName))
        //            {
        //                ViewBag.LastName = objInfo.LastName;
        //            }
        //            else
        //            {
        //                ViewBag.LastName = null;
        //            }
        //        }
        //        string patname = null;



        //        //clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] InParamList = {
        //    new SqlParameter("@In_Patient_Ids", Session["PatientID"]),
        //    new SqlParameter("@iN_pROVIDER_ID", null)
        //};
        //        clscommon.AddInParameters(InParamList);
        //        DataSet objds = new DataSet();
        //        objds = clscommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
        //        RPBilling patinfo = new RPBilling();
        //        if (objds.Tables[0].Rows.Count > 0)
        //        {

        //            foreach (DataRow dr in objds.Tables[0].Rows)
        //            {
        //                patinfo.PatientLogin_ID = Convert.ToInt32(dr["PatientLogin_ID"]);
        //                patname = Convert.ToString(dr["PatientName"]);
        //            }
        //        }
        //        string strproname = ViewBag.FirstName + " " + ViewBag.LastName;
        //        var objBill = new RPBilling();
        //        objBill.FromDate = fromdate != "" ? fromdate : null;
        //        objBill.ToDate = todate != "" ? todate : null;
        //        //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objBill.PatientLogin_ID = patinfo.PatientLogin_ID;

        //        objBill.Reference_id = Convert.ToInt32(Session["Prov_ID"]);
        //        DataSet dsbill = new DataSet();
        //        dsbill = RPBilling.PracticePatientIncome(objBill);
        //        string strTCharge = null;
        //        string strTPay = null;
        //        string strBal = null;
        //        string strCredit = null;
        //        foreach (DataRow dr in dsbill.Tables[0].Rows)
        //        {
        //            strTCharge = string.Format("{0:c}", dr["TotalCharges"]);
        //            strTPay = string.Format("{0:c}", dr["Netpayments"]);
        //            strBal = string.Format("{0:c}", dr["Balance"]);
        //            double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
        //            strCredit = string.Format("{0:c}", dblcredit);
        //        }
        //        //double height = 100;
        //        double pos3 = 300;

        //        string strhtml = null;
        //        strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Client billing summary</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='left'><font size='15px'><b>Mower helper name : " + strproname + "</b></font></td>";
        //        if (patname != null & patname != "")
        //        {
        //            strhtml = strhtml + "<td align='right'><font size='15px'><b>Client name : " + patname + "</b></font></td>";
        //        }
        //        else
        //        {
        //            strhtml = strhtml + "<td align='right'></td>";
        //        }
        //        strhtml = strhtml + "</tr>";

        //        strhtml = strhtml + "</table><br/>";

        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Total charges" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Total payments" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Total credits" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Balance" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td><b> " + strTCharge + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + strTPay + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + strCredit + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + strBal + "</b></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/><br/>";


        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>By days outstanding</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
        //        DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
        //        if (dsdayout.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
        //            {
        //                string str0_30 = string.Empty;
        //                string str31_60 = string.Empty;
        //                string str61_90 = string.Empty;
        //                string str91_120 = string.Empty;
        //                string strOver120 = string.Empty;
        //                double strAm30;
        //                double strAm60;
        //                double strAm90;
        //                double strAm120;
        //                double strAm150;
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
        //                {
        //                    str0_30 = string.Format("{0:c}", dr1["0-30"]);
        //                    strAm30 = Convert.ToDouble(dr1["0-30"]);
        //                }
        //                else
        //                {
        //                    str0_30 = "$0.00";
        //                    strAm30 = 0;

        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
        //                {
        //                    str31_60 = string.Format("{0:c}", dr1["31-60"]);
        //                    strAm60 = Convert.ToDouble(dr1["31-60"]);
        //                }
        //                else
        //                {
        //                    str31_60 = "$0.00";
        //                    strAm60 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
        //                {
        //                    str61_90 = string.Format("{0:c}", dr1["61-90"]);
        //                    strAm90 = Convert.ToDouble(dr1["61-90"]);
        //                }
        //                else
        //                {
        //                    str61_90 = "$0.00";
        //                    strAm90 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
        //                {
        //                    str91_120 = string.Format("{0:c}", dr1["91-120"]);
        //                    strAm120 = Convert.ToDouble(dr1["91-120"]);
        //                }
        //                else
        //                {
        //                    str91_120 = "$0.00";
        //                    strAm120 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
        //                {
        //                    strOver120 = string.Format("{0:c}", dr1["over 120 days"]);
        //                    strAm150 = Convert.ToDouble(dr1["over 120 days"]);
        //                }
        //                else
        //                {
        //                    strOver120 = "$0.00";
        //                    strAm150 = 0;
        //                }




        //                double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
        //                string amountdue = string.Format("{0:c}", amountdue1);
        //                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td align='center' ><label>Billable Party" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>Amount due" + "</label></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b> " + dr1["name"] + "</b></td>";
        //                strhtml = strhtml + "<td><b> </b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>Balance </b></td>";
        //                strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str91_120 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + amountdue + "</b></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "</table><br/><br/>";
        //            }
        //        }
        //        else
        //        {
        //        }

        //        RPBilling objchgBill = new RPBilling();
        //        if (!string.IsNullOrEmpty(palgnid))
        //        {
        //            objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //            ViewBag.PlId = objchgBill.PatientLogin_ID;
        //        }
        //        else
        //        {
        //            objchgBill.PatientLogin_ID = null;
        //            ViewBag.PlId = null;
        //        }
        //        objchgBill.ReferenceType_ID = null;
        //        if (fromdate == "")
        //        {
        //            fromdate = null;
        //        }
        //        if (todate == "")
        //        {
        //            todate = null;
        //        }
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            objchgBill.FromDate = fromdate;
        //        }
        //        else
        //        {
        //            objchgBill.FromDate = null;
        //        }
        //        if (!string.IsNullOrEmpty(todate))
        //        {
        //            objchgBill.ToDate = todate;
        //        }
        //        else
        //        {
        //            objchgBill.ToDate = null;
        //        }

        //        objchgBill.AuthorizedPatientLoginID = null;

        //        if (chsort != null & chsort != "")
        //        {
        //            objchgBill.OrderbyItem = chsort.ToString();
        //        }
        //        else
        //        {
        //            objchgBill.OrderbyItem = "Transaction_Date";
        //        }
        //        if (chsortdir != null & chsortdir != "")
        //        {
        //            objchgBill.orderBy = chsortdir.ToString();

        //        }
        //        else
        //        {
        //            objchgBill.orderBy = "DESC";
        //        }
        //        objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objchgBill.PageNo = "1";
        //        objchgBill.NoOfRecords = chtotrec;
        //        List<RPBilling> chglist = new List<RPBilling>();
        //        chglist = objchgBill.GetPatientChargeslist(objchgBill);



        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (chglist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            //strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //            //strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px'><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in chglist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";

        //                // strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                // strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table><br/><br/>";
        //        }
        //        else
        //        {
        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px' align='center' ><b> No Charges Found. " + "</b></td>";
        //            strhtml = strhtml + "</table><br/><br/>";
        //        }


        //        RPBilling objAdj = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);

        //        }
        //        else
        //        {
        //            objAdj.PatientLogin_ID = null;
        //        }
        //        objAdj.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objAdj.FromDate = fromdate;
        //                objAdj.ToDate = todate;

        //            }
        //            else
        //            {
        //                objAdj.FromDate = null;
        //                objAdj.ToDate = null;
        //            }

        //        }
        //        else
        //        {
        //            objAdj.FromDate = null;
        //            objAdj.ToDate = null;

        //        }

        //        objAdj.AuthorizedPatientLoginID = null;
        //        if (pasort != null & pasort != "")
        //        {
        //            objAdj.OrderbyItem = pasort.ToString();

        //        }
        //        else
        //        {
        //            objAdj.OrderbyItem = "Transaction_Date";
        //        }
        //        if (pasortdir != null & pasortdir != "")
        //        {
        //            objAdj.orderBy = pasortdir.ToString();

        //        }
        //        else
        //        {
        //            objAdj.orderBy = "DESC";
        //        }
        //        objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        objAdj.PageNo = "1";

        //        objAdj.NoOfRecords = patotrec;
        //        List<RPBilling> Adjlist = new List<RPBilling>();
        //        Adjlist = objAdj.GetPatientPaymentList(objAdj);

        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table> <br/> <br/>";

        //        }
        //        else
        //        {
        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }

        //        RPBilling objpmt = new RPBilling();
        //        if (palgnid != null)
        //        {
        //            objpmt.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        }
        //        else
        //        {
        //            objpmt.PatientLogin_ID = null;
        //        }
        //        objpmt.ReferenceType_ID = null;
        //        if (!string.IsNullOrEmpty(fromdate))
        //        {
        //            if (fromdate != "All")
        //            {
        //                objpmt.FromDate = fromdate;
        //                objpmt.ToDate = todate;
        //            }
        //            else
        //            {
        //                objpmt.FromDate = null;
        //                objpmt.ToDate = null;
        //            }
        //        }
        //        objpmt.AuthorizedPatientLoginID = null;
        //        if (adjsort != null)
        //        {
        //            objpmt.OrderbyItem = adjsort.ToString();

        //        }
        //        else
        //        {
        //            objpmt.OrderbyItem = "Transaction_Date";
        //        }
        //        if (adjsortdir != null)
        //        {
        //            objpmt.orderBy = adjsortdir.ToString();

        //        }
        //        else
        //        {
        //            objpmt.orderBy = "DESC";
        //        }
        //        objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //        // objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //        objpmt.PageNo = "1";


        //        objpmt.NoOfRecords = adjtotrec;

        //        List<RPBilling> Adjlist1 = new List<RPBilling>();
        //        Adjlist1 = objpmt.GetPatientAdjustmenttList(objpmt);

        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr align='center'>";
        //        strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Adjustments List</u></b></font></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td align='center' height='25%'></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/>";
        //        if (Adjlist1.Count > 0)
        //        {

        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Paid by/Charged by" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Adjustment type" + "</b></td>";
        //            strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //            strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //            strhtml = strhtml + "</tr>";
        //            foreach (var item in Adjlist1)
        //            {
        //                string date1 = item.Transaction_Date;
        //                string[] date = date1.Split(' ');
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //                strhtml = strhtml + "</tr>";

        //            }
        //            strhtml = strhtml + "</table>";

        //        }
        //        else
        //        {
        //            strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td width='10px' style='align:center;'><b> No Adjustments Found. " + "</b></td>";
        //            strhtml = strhtml + "</table>";
        //        }

        //        //height = pdf.GetTextHeight(strhtml);
        //        pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsExceptionLog clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "ClientsController", "Getalltranspdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult AuditLoginfo(LogInfo objLoginfo)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            try
            {

                if (objLoginfo.NoofRecords != null && objLoginfo.NoofRecords != 0)
                {
                    objLoginfo.NoofRecords = objLoginfo.NoofRecords;
                }
                else if (Session["Rowsperpage"] != null)
                {
                    objLoginfo.NoofRecords = Convert.ToInt32(Session["Rowsperpage"]);
                }
                else
                {
                    objLoginfo.NoofRecords = 10;
                }
                objLoginfo.PatientLoginID = Convert.ToInt32(Session["PatientLoginID"]);

                objLoginfo.EventCategory_ID = null;
                objLoginfo.ProviderLoginID = null;
                objLoginfo.ReferencetypeID = 3;
                objLoginfo.PageNo = !string.IsNullOrEmpty(Request["p1"]) ? Convert.ToInt32(Request["p1"]) : 1;
                objLoginfo.OrderByItem = "Createdon";
                objLoginfo.OrderBy = "DESC";

                objLoginfo.LogInfoList = LogInfo.Get_EvenLogInfo(objLoginfo);
                ViewBag.totrec = objLoginfo.LogInfoList.Count > 0 ? objLoginfo.TotalRecords : 0;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ClientsController", "AuditLoginfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

            return View("AuditLoginfo", objLoginfo);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult viewaudit(string logininfoid, string eventtypeid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            List<LogInfo> objList = new List<LogInfo>();
            objList = LogInfo.Get_PatientLoginfo(logininfoid, eventtypeid);
            ViewBag.totrec = objList.Count;
            return View("viewaudit", objList);

        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult AddNewAppointment(int? customerid, int? customerLid, string customername, string clientphone, string ProviderId = null, string courtId = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (ProviderId != null)
                {
                    Session["Prov_id"] = ProviderId;
                    Session.Add("ComboProv_ID", ProviderId);
                }

                //FillPatients();
                // FillTechnicians();

                if (clsWebConfigsettings.GetConfigSettingsValue("Display_placeofservice").ToUpper() == "YES")
                {
                    ViewBag.hdnplaceind = "Y";
                }
                else
                {
                    ViewBag.hdnplaceind = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowRecurrence").ToUpper() == "YES")
                {
                    ViewBag.ShowRecurrence = "Y";
                }
                else
                {
                    ViewBag.ShowRecurrence = "N";
                }
                ViewBag.SelectedAddress = courtId;
    //            IList<SelectListItem> OtherAddressItemList = new List<SelectListItem>();
    //            IDataParameter[] objparam = {
    //    new SqlParameter("@In_Provider_id",Convert.ToString( Session["Prov_id"]))       
    //};
    //            clscommon.AddInParameters(objparam);
    //            SqlDataReader objread = default(SqlDataReader);
    //            objread = clscommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
    //            while (objread.Read())
    //            {
    //                //if (Convert.ToString(objread["courtlocation"]) != "")
    //                //{
    //                    if (!string.IsNullOrEmpty(courtId))
    //                    {
    //                        if (courtId == Convert.ToString(objread["Appointment_address_id"]))
    //                        {
    //                            ViewBag.SelectedAddress = Convert.ToString(objread["courtlocation"]);
    //                            ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                        }
    //                    }
    //                    //else if (Convert.ToString(objread["Defaultaddress_Ind"]) == "Y")
    //                    //{

    //                    //    ViewBag.SelectedAddress = Convert.ToString(objread["Address"]);
    //                    //    ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                    //}
    //                    OtherAddressItemList.Add(new SelectListItem
    //                    {
    //                        Text = Convert.ToString(objread["courtlocation"]),
    //                        Value = Convert.ToString(objread["Appointment_address_id"])
    //                    });
    //                //}
    //            }
    //            //           IDataParameter[] objparam1 = {
    //            //       new SqlParameter("@In_Patient_Ids",customerid),
    //            //       new SqlParameter("@iN_pROVIDER_ID",ProviderId)
    //            //   };
    //            //           objcommon.AddInParameters(objparam1);
    //            //           SqlDataReader dr = default(SqlDataReader);
    //            //           dr = objcommon.GetDataReader("Help_dbo.st_Scheduling_Get_PatientDetails");
    //            //           while (dr.Read())
    //            //           {
    //            //               if (Convert.ToString(dr["DefaultCourtLocation_ID"]) != null)
    //            //               {
    //            //ViewBag.SelectedAddress
    //            //               }
    //            //           }
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "------------------------",
    //                Value = ""
    //            });
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "New Location",
    //                Value = "0"
    //            });
    //            ViewBag.OtherAddressItemList = OtherAddressItemList;
    //            IList<SelectListItem> _ddlTimeMer = new List<SelectListItem>();

    //            _ddlTimeMer.Add(new SelectListItem
    //            {
    //                Text = "AM",
    //                Value = "0"
    //            });
    //            _ddlTimeMer.Add(new SelectListItem()
    //            {
    //                Text = "PM",
    //                Value = "1"
    //            });
    //            IList<SelectListItem> _ddlDuration = new List<SelectListItem>();
    //            for (int i = 30; i <= 120; i = i + 30)
    //            {
    //                _ddlDuration.Add(new SelectListItem
    //                {
    //                    Text = i.ToString(),
    //                    Value = i.ToString()
    //                });
    //            }


                AddAppointmentModel objAddAppointment = new AddAppointmentModel();
                //ViewBag.ddlTimeMer = _ddlTimeMer;
                //ViewBag.ddlDuration = _ddlDuration;
                //ViewBag.apptdate = DateTime.Now.ToShortDateString();
                string[] timeapp = { "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" };
                var timelist = new ComboBoxItemList(timeapp);
                //var timeitems = new List<ComboBoxItem>();
                objAddAppointment.PatientName = customername;
                ViewData["cbotime"] = timelist;
                Dictionary<int, string> weeklyitems = new Dictionary<int, string>();
                weeklyitems.Add(1, "Sun");
                weeklyitems.Add(2, "Mon");
                weeklyitems.Add(3, "Tue");
                weeklyitems.Add(4, "Wed");
                weeklyitems.Add(5, "Thu");
                weeklyitems.Add(6, "Fri");
                weeklyitems.Add(7, "Sat");
                ViewBag.weeklylist = weeklyitems;
                objAddAppointment.Patient_ID = customerid;
                objAddAppointment.ToReferenceLogin_ID = customerLid;
                objAddAppointment.cellphone = clientphone;
                return View("AddNewAppointment", objAddAppointment);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddNewAppointment(Models.AddAppointmentModel addAppointment)
        {
            //if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            //{
            //    if (Request.IsAjaxRequest())
            //    {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string strMessage = null;
                int? AppointmentID = 0;
                if (string.IsNullOrEmpty(stroutmsg))
                {
                    if (Convert.ToString(Session["roleid"]) != "1")
                    {
                        addAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
                    }
                    else
                    {
                        addAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                    }
                    addAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
                    addAppointment.FromReferenceType_ID = 2;
                    addAppointment.GroupInd = "N";
                    addAppointment.NumofOccurrences = null;
                    if (addAppointment.ApptType != null)
                    {
                        if (addAppointment.ApptType == "Patient")
                        {
                            addAppointment.AppointmentType_ID = 1;
                            addAppointment.AppointmentStatus_ID = 1;
                            if (addAppointment.Patient_ID != null && addAppointment.Patient_ID != 0)
                            {
                                addAppointment.ToReference_ID = addAppointment.Patient_ID;
                                if (addAppointment.ToReferenceLogin_ID != null)
                                {
                                    addAppointment.ToReferenceLogin_ID = addAppointment.ToReferenceLogin_ID;
                                }
                                else
                                {
                                    addAppointment.ToReferenceLogin_ID = 0;
                                }
                            }
                        }
                        else
                        {
                            addAppointment.AppointmentType_ID = 2;
                            addAppointment.AppointmentStatus_ID = 7;
                        }
                    }
                   
                    addAppointment.ToReferenceType_ID = 3;
                   // addAppointment.ToReference_IDs = null;
                    addAppointment.StartDate = !string.IsNullOrEmpty(addAppointment.AppointmentDate) ? addAppointment.AppointmentDate : null;
                    //   addAppointment.EndDate = null;               
                    //addAppointment.PlaceOfService_ID = Convert.ToInt32(Session["PlaceOfService_ID"]);

                    addAppointment.Notes = !string.IsNullOrEmpty(addAppointment.Notes) ? Sanitizer.GetSafeHtmlFragment(addAppointment.Notes) : null;

                    addAppointment.AppointmentTime = Request["cbotime_SelectedText"] + (Request["ddlTimeMer"] == "0" ? "AM" : "PM");
                    if (Request["hdnValidInd"] != string.Empty)
                    {
                        addAppointment.IsValidate_Ind = Request["hdnValidInd"];
                    }
                    addAppointment.IsValidate_Ind = !string.IsNullOrEmpty(addAppointment.IsValidate_Ind) ? addAppointment.IsValidate_Ind : "N";
                    if (Request["ddlDuration"] != null)
                    {
                        addAppointment.Duration = Convert.ToInt32(Request["ddlDuration"]);
                    }
                    if (Convert.ToString(Session["roleid"]) != "39")
                    {
                        addAppointment.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    }
                    else
                    {
                        addAppointment.CreatedBy = Convert.ToInt32(Session["Techlog_id"]);
                    }
                    GetAppointment onjcheck = new GetAppointment();
                    string appatdate = addAppointment.StartDate;

                    //if (Request["rdoApptPlace"] != null)
                    //{
                    //    if (Request["rdoApptPlace"] == "Patientname")
                    //    {
                    //        addAppointment.Aptplaceofservice = "C";
                    //    }
                    //    else
                    //    {
                    //        addAppointment.Aptplaceofservice = "P";
                    //    }
                    //}
                    if (addAppointment.ApptType == "Patient")
                    {
                        if (Request["hdnselectother"] != null)
                        {
                            //if (Request["hdnselectother"] == "My Location")
                            //{
                            //    addAppointment.Aptplaceofservice = "P";
                            //}
                            //else if (Request["hdnselectother"] == "Client Location")
                            //{
                            //    addAppointment.Aptplaceofservice = "C";
                            //}
                            if (Request["hdnselectother"] == "New Location")
                            {
                                addAppointment.defaultCoachAddress = addAppointment.ChkDefaultCourt == true ? "Y" : "N";
                                addAppointment.Aptplaceofservice = "O";
                                addAppointment.CourtLocationName = !string.IsNullOrEmpty(addAppointment.CourtLocationName) ? addAppointment.CourtLocationName : null;
                            }
                            else
                            {
                                addAppointment.defaultCoachAddress = addAppointment.ChkDefaultCourt == true ? "Y" : "N";
                                addAppointment.Aptplaceofservice = "O";
                                addAppointment.otherAddressid = Convert.ToInt32(Request["OtherAddressItemList"]);
                                addAppointment.CourtLocationName = !string.IsNullOrEmpty(addAppointment.CourtLocationName) ? addAppointment.CourtLocationName : null;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(addAppointment.Recurring))
                    {
                        if (addAppointment.Recurring == "No Recurring")
                        {
                            addAppointment.EndDate = null;
                            addAppointment.SelectedWeekdays = null;
                            addAppointment.IntervalValue = null;
                            addAppointment.IntervalType_ID = null;
                        }
                        else if (addAppointment.Recurring == "Daily")
                        {
                            addAppointment.IntervalType_ID = "1";
                            addAppointment.IntervalValue = addAppointment.DailyReccur;
                            addAppointment.EndDate = addAppointment.EndDateForDaily;
                            addAppointment.SelectedWeekdays = null;
                        }
                        else
                        {
                            addAppointment.IntervalType_ID = "2";
                            addAppointment.IntervalValue = addAppointment.WeeklyReccur;
                            addAppointment.EndDate = addAppointment.EndDateForWeek;
                            addAppointment.SelectedWeekdays = Request["Wselection"].Replace(",", "");
                        }
                    }
                    else
                    {
                        addAppointment.EndDate = null;
                        addAppointment.SelectedWeekdays = null;
                        addAppointment.IntervalValue = null;
                        addAppointment.IntervalType_ID = null;
                    }
                    string strDates = "";

                    if (addAppointment.Recurring == "No Recurring")
                    {
                        strDates = DateTime.Parse(appatdate).ToShortDateString();
                    }
                    else if (addAppointment.Recurring == "Daily")
                    {
                        while (DateTime.Parse(appatdate) <= DateTime.Parse(addAppointment.EndDate))
                        {
                            if (string.IsNullOrEmpty(strDates))
                            {
                                strDates = DateTime.Parse(appatdate).ToShortDateString();
                            }
                            else
                            {
                                strDates += "," + DateTime.Parse(appatdate).ToShortDateString();
                            }
                            //StartDate = DateAndTime.DateAdd(DateInterval.Day, Convert.ToInt32(txtDailyRecur.Text), appatdate);
                            appatdate = DateTime.Parse(appatdate).AddDays(Convert.ToInt32(addAppointment.IntervalValue)).ToShortDateString();
                        }
                    }
                    else
                    {
                        strDates = onjcheck.weeklyApptDates(appatdate, addAppointment.IntervalValue, addAppointment.EndDate, addAppointment.SelectedWeekdays);


                    }
                    strMessage = onjcheck.checkApptExist(strDates, Convert.ToString(addAppointment.ToReference_ID), addAppointment.AppointmentTime, (Convert.ToString(Session["roleid"]) != "1" ? Convert.ToString(Session["Prov_ID"]) : Convert.ToString(Session["ComboProv_ID"])), string.Empty);
                    if (strMessage == "Y")
                    {
                        if (addAppointment.ApptType == "Patient")
                        {
                            strMessage = "1";
                        }
                        else
                        {
                            strMessage = "2";
                        }
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        strMessage = string.Empty;
                        strMessage = addAppointment.InsertAppointment(addAppointment, AppointmentID);
                    }
                    if (string.IsNullOrEmpty(strMessage) & addAppointment.ApptType == "Patient")
                    {
                        string strclientphone = string.Empty;

                        if (!string.IsNullOrEmpty(addAppointment.cellphone))
                        {
                            strclientphone = addAppointment.cellphone;
                            strclientphone = strclientphone.Replace("-", "");
                        }
                        clsCommonCClist.TwilioSms(Convert.ToString(Session["PracticeName"]), Convert.ToString(Session["Providerphone"]), strclientphone, strDates, addAppointment.AppointmentTime, null, 0);

                    }
                    //int emailTran=0;
                    //if ((AppointmentID != 0) )
                    //{

                    //    if (Convert.ToString(Request["hdncstmemail"]) !="" && Request["hdncstmid"].ToString() != null)
                    //    {
                    //        emailTran = addAppointment.getEmailTranId(addAppointment, AppointmentID);
                    //    }
                    //    else if (Request["combobox1"] != null && (!string.IsNullOrEmpty(Request["hdnEmailID"])))
                    //    {
                    //        emailTran = addAppointment.getEmailTranId(addAppointment, AppointmentID);
                    //    }
                    //}
                }
                else
                {
                    strMessage = stroutmsg.Replace("<br>", "\r\n");
                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                }
                if (string.IsNullOrEmpty(strMessage))
                {
                    //string redirectmsg = Request["txtApptDate"];
                    //DateTime dt = DateTime.Parse(redirectmsg);
                    //addAppointment.monthname = new DateTime(dt.Year, dt.Month, 1).ToShortDateString();
                    //if (Request["hdnfrompage"].ToString() != "month")
                    //{
                    //    int day;
                    //    day = (int)dt.DayOfWeek;
                    //    switch (day)
                    //    {
                    //        case 0:
                    //            addAppointment.monthname = dt.ToShortDateString();
                    //            break;
                    //        case 1:
                    //            addAppointment.monthname = dt.AddDays(-1).ToShortDateString();
                    //            break;
                    //        case 2:
                    //            addAppointment.monthname = dt.AddDays(-2).ToShortDateString();
                    //            break;
                    //        case 3:
                    //            addAppointment.monthname = dt.AddDays(-3).ToShortDateString();
                    //            break;
                    //        case 4:
                    //            addAppointment.monthname = dt.AddDays(-4).ToShortDateString();
                    //            break;
                    //        case 5:
                    //            addAppointment.monthname = dt.AddDays(-5).ToShortDateString();
                    //            break;
                    //        case 6:
                    //            addAppointment.monthname = dt.AddDays(-6).ToShortDateString();
                    //            break;
                    //    }
                    //}
                    return Json(JsonResponseFactory.SuccessResponse(addAppointment), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    strMessage = strMessage.Replace("<br>", "\r\n");
                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                }
                //    }
                //    else
                //    {
                //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
                //    }
                //}
                //else
                //{
                //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
                //}
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DefaultCourt(string PatientId, string courtId, string ProviderID = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.Selectedcourt = courtId;
                //IList<SelectListItem> OtherAddressItemList1 = new List<SelectListItem>();
                //clsCommonFunctions objcommon = new clsCommonFunctions();
                //IDataParameter[] objparam1 = { new SqlParameter("@In_Patient_Ids", PatientId) };
                //objcommon.AddInParameters(objparam1);
                //SqlDataReader dr = default(SqlDataReader);
                //dr = objcommon.GetDataReader("Help_dbo.st_Scheduling_Get_PatientDetails");
                //while (dr.Read())
                //{
                //    if (!string.IsNullOrEmpty(Convert.ToString(dr["DefaultCourtLocation_ID"])))
                //    {
                //        TempData["SelectedAddress_id"] = Convert.ToString(dr["DefaultCourtLocation_ID"]);
                //        TempData.Keep("SelectedAddress_id");
                //    }
                //}
                /******************************************/
    //            IDataParameter[] objparam = {
    //    new SqlParameter("@In_Provider_id",!string.IsNullOrEmpty( Convert.ToString(Session["Prov_id"]))?Convert.ToString( Session["Prov_id"]):ProviderID)
    //};
    //            clscommon.AddInParameters(objparam);
    //            SqlDataReader objread = default(SqlDataReader);
    //            objread = clscommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
    //            while (objread.Read())
    //            {
    //               // if (Convert.ToString(objread["courtlocation"]) != "")
    //               // {
    //                    if (!string.IsNullOrEmpty(courtId))
    //                    {
    //                        if (courtId == Convert.ToString(objread["Appointment_address_id"]))
    //                        {
    //                            OtherAddressItemList1.Add(new SelectListItem
    //                            {
    //                                Text = Convert.ToString(objread["courtlocation"]),
    //                                Value = Convert.ToString(objread["Appointment_address_id"]),
    //                                Selected = true
    //                            });
    //                        }
    //                        else
    //                        {
    //                            OtherAddressItemList1.Add(new SelectListItem
    //                            {
    //                                Text = Convert.ToString(objread["courtlocation"]),
    //                                Value = Convert.ToString(objread["Appointment_address_id"])
    //                            });
    //                        }
    //                    }
    //                    //else if (Convert.ToString(objread["Defaultaddress_Ind"]) == "Y")
    //                    //{

    //                        //ViewBag.SelectedAddress = Convert.ToString(objread["Address"]);
    //                    //ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                    //}
    //                    else
    //                    {
    //                        OtherAddressItemList1.Add(new SelectListItem
    //                        {
    //                            Text = Convert.ToString(objread["courtlocation"]),
    //                            Value = Convert.ToString(objread["Appointment_address_id"])
    //                        });
    //                    }
    //               // }
    //            }
    //            OtherAddressItemList1.Add(new SelectListItem
    //            {

    //                Text = "------------------------",
    //                Value = "-0"
    //            });
    //            OtherAddressItemList1.Add(new SelectListItem
    //            {

    //                Text = "New Location",
    //                Value = "0"
    //            });
                Patient_Info objPat = new Patient_Info();
                objPat.PatientID = PatientId;
                objPat.ProviderID = !string.IsNullOrEmpty(Convert.ToString(Session["Prov_id"])) ? Convert.ToString(Session["Prov_id"]) : ProviderID;
               // ViewData["OtherAddressItemList1"] = OtherAddressItemList1;
                return View(objPat);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DefaultCourt(Patient_Info obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                try
                {
                    int? DefaultCourtId = 0;
                    if (!string.IsNullOrEmpty(Request["hdncourtId"]) && Request["hdncourtId"] != "0")
                    {
                        DefaultCourtId = Convert.ToInt32(Request["hdncourtId"]);
                    }
                    else
                    {
                        DefaultCourtId = null;
                    }
                    obj.Address1 = Request["EotherAddress1"];
                    obj.CourtLocationName = !string.IsNullOrEmpty(obj.DefCourtLocationName) ? obj.DefCourtLocationName : null;
                    obj.ZIP = Request["txtEZip"];
                    obj.State_ID = Request["DDEState"];
                    obj.City_ID = Request["DDECity"];
                    obj.Default_addr = Request["hdncourtId"] == "0" ? "Y" : null;
                   // int countryid = 1;
                    obj.UpdateDefaultCourt(obj, DefaultCourtId, 1);
                    return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    clsExceptionLog clsex = new clsExceptionLog();
                    clsex.LogException(ex, "ClientsController", "DefaultCourt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SentInvoices(Invocie objInvoice)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            string startdate = DateTime.Now.ToString("MM/dd/yyyy");
            if (objInvoice.NoofRecords != 0)
            {
                Session["Rowsperpage"] = objInvoice.NoofRecords;
            }
            else if (Session["Rowsperpage"] != null)
            {
                objInvoice.NoofRecords = Convert.ToInt32(Session["Rowsperpage"]);
            }
            else
            {
                objInvoice.NoofRecords = 10;
            }
            objInvoice.Daterange = string.IsNullOrEmpty(objInvoice.Daterange) ? "30" : objInvoice.Daterange;
            objInvoice.orderBy = !string.IsNullOrEmpty(Request["sortdir"]) ? Request["sortdir"] : "ASC";
            objInvoice.OrderByItem = !string.IsNullOrEmpty(Request["sort"]) ? "Invoice_Date" : Request["sort"];
            objInvoice.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);

            if (objInvoice.Daterange == "0" || objInvoice.Daterange == null || objInvoice.Daterange == "" || objInvoice.Daterange == "Custom")
            {
                objInvoice.FromDate = (string.IsNullOrEmpty(objInvoice.FromDate) ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : objInvoice.FromDate);
                objInvoice.ToDate = (string.IsNullOrEmpty(objInvoice.ToDate) ? startdate : objInvoice.ToDate);
            }
            else
            {

                if (objInvoice.Daterange == "Today")
                {
                    objInvoice.FromDate = startdate;
                }
                else if (objInvoice.Daterange == "7")
                {
                    objInvoice.FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
                }
                else if (objInvoice.Daterange == "30")
                {
                    objInvoice.FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
                }
                objInvoice.ToDate = startdate;
            }
            objInvoice.ListOfInvoice = objInvoice.InvoiceList(Session["Prov_ID"] != null ? Session["Prov_ID"].ToString() : null, 2, objInvoice.NoofRecords, objInvoice.PageNo, objInvoice.orderBy, objInvoice.OrderByItem, objInvoice.FromDate, objInvoice.ToDate, Session["PatientID"] != null ? Session["PatientID"].ToString() : null, 3);
            ViewBag.totrec = objInvoice.ListOfInvoice.Count > 0 ? Invocie.TotalRecords : 0;
            return View(objInvoice);
        }
        
        public JsonResult FileExists(string InvoiceName)
        {
            string filename = null;
            string extn = null;
            string strFilePath = HttpContext.Server.MapPath("~/") + "Attachments\\Invoices" + "\\";
            filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + InvoiceName);
            extn = System.IO.Path.GetExtension(strFilePath + InvoiceName);
            string fullfilepath = InvoiceName;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                return Json(JsonResponseFactory.SuccessResponse("true"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonResponseFactory.SuccessResponse("false"), JsonRequestBehavior.AllowGet);
            }
        }
        public ContentResult DownloadInvoice(int invoiceid, string InvoiceName)
        {
            try
            {
                string filename = null;
                string extn = null;
                string strFilePath = HttpContext.Server.MapPath("~/") + "Attachments\\Invoices" + "\\";
                filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + InvoiceName);
                extn = System.IO.Path.GetExtension(strFilePath + InvoiceName);
                string fullfilepath = InvoiceName;
                strFilePath = strFilePath + "/" + fullfilepath;
                System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
                if (File.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=" + InvoiceName);
                    Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(File.FullName);
                    Response.End();
                }
                //Invocie objinvoice = new Invocie();
                //EASYPDF pdf = new EASYPDF();
                //DataSet dstran = new DataSet();
                //dstran = objinvoice.GetInvoiceData(Session["Prov_ID"] != null ? Session["Prov_ID"].ToString() : null, 2, Session["PatientID"] != null ? Session["PatientID"].ToString() : null, 3, invoiceid);

                //string EleAdd = dstran.Tables[0].Rows[0]["PracticeAddress"].ToString();
                //// string clientAdd = dstran.Tables[0].Rows[0]["ClientAddress"].ToString();
                //string[] arrayCustAddress = dstran.Tables[0].Rows[0]["ClientAddress"].ToString().Split(',');
                //string[] arrayEleAddress = EleAdd.Split(',');
                //string strRdate = Convert.ToString(dstran.Tables[0].Rows[0]["Invoice_Date"].ToString());
                //if (strRdate != null & strRdate != "")
                //{
                //    strRdate = DateTime.Parse(strRdate).ToShortDateString();
                //}


                //pdf.Create();
                //string strhtml = null;
                //strhtml = "<table cellpadding='1' cellspacing='1' align='center' border='0' width='85%'>";
                //strhtml = strhtml + "<tr class='background_color'><td align='center' colspan='4'><font size='20px'><strong>Invoice/Receipt</strong><br><br><br></font></td></tr>";
                //strhtml = strhtml + "<tr>";
                //if (arrayEleAddress.Length == 3)
                //{
                //    strhtml = strhtml + "<td width='61%' style='font-size:10px;'><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["FromReferenceName"].ToString()) + "</span><br><span>" + arrayEleAddress[0] + "</span><br><span>" + arrayEleAddress[1] + "</span><br><span>" + arrayEleAddress[2] + "</span><br><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["Providerphonenumber"]) + "</span> <br><br></span></td>";
                //}
                //else
                //{
                //    strhtml = strhtml + "<td width='61%' style='font-size:10px;'><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["FromReferenceName"].ToString()) + "</span><br><span>" + arrayEleAddress[0] + "</span><br><span>" + arrayEleAddress[1] + "</span><br><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["Providerphonenumber"]) + "</span><br><br></span></td>";
                //}

                //strhtml = strhtml + "<td width='39%' valign='top'><table cellpadding='2' cellspacing='0' align='center' borderColor='#d3d3d3' border='1' border-collapse='collapse' width='100%'><tr><td width='55%' align='left' style='font-size:10px;'><b>Receipt number :</b></td><td width='45%' align='center' style='font-size:10px;'><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["RecieptNo"].ToString()) + "</span></td></tr><tr><td width='55%' align='left' style='font-size:10px;'><b>Receipt date :</b></td><td width='45%' align='center' style='font-size:10px;'><span>" + strRdate + "</span></td></tr>";
                //strhtml = strhtml + "</table></td></tr>";


                //strhtml = strhtml + "<tr>";
                //if (arrayCustAddress.Length == 3)
                //{
                //    strhtml = strhtml + "<td width='61%' style='font-size:10px;'><font size='10px'><strong>Bill to</strong></font><br/><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["ToReferenceName"]) + "</span><br><span>" + arrayCustAddress[0] + "</span><br><span>" + arrayCustAddress[1] + "</span><br><span>" + arrayCustAddress[2] + "</span><br><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["Patientphonenumber"]) + "</span> <br><br></span></td>";
                //}
                //else
                //{
                //    strhtml = strhtml + "<td width='61%' style='font-size:10px;'><font size='10px'><strong>Bill to</strong></font><br/><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["ToReferenceName"]) + "</span><br><span>" + arrayCustAddress[0] + "</span><br><span>" + arrayCustAddress[1] + "</span><br><span>" + Convert.ToString(dstran.Tables[0].Rows[0]["Patientphonenumber"]) + "</span><br><br></span></td>";
                //}

                //strhtml = strhtml + "<td width='39%' valign='top'><table cellpadding='2' cellspacing='0' align='center' borderColor='#d3d3d3' border='1' border-collapse='collapse' width='100%'><tr><td width='55%' align='left' style='font-size:10px;'><b>Total charges :</b></td><td width='45%' align='right' style='font-size:10px;'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceCharges"])) + "</span></td></tr><tr><td width='55%' align='left' style='font-size:10px;'><b>Total payments :</b></td><td width='45%' align='right' style='font-size:10px;'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoicePayments"])) + "</span></td></tr><tr><td width='55%' align='left' style='font-size:10px;'><b>Balance :</b></td><td width='45%' align='right' style='font-size:10px;'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceBalance"])) + "</span></td></tr>";
                //strhtml = strhtml + "</table></td></tr>";
                //strhtml = strhtml + "</table>";



                //strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='85%'><tr><td align='center'><font size='15'><strong>Transactions</strong></font> </td></tr></table>";
                //strhtml = strhtml + "<br/>";
                //strhtml = strhtml + "<table cellpadding='0' cellspacing='1' align='center' border='1' borderColor='#d3d3d3' border-collapse='collapse' width='85%'>";
                //strhtml = strhtml + "<tr bgcolor='#EEEEEE' height='22px'><td style='font-size:12px;' align='center' height='25px' valign='middle'><b>Transaction date</b></td><td style='font-size:12px;' align='center' height='25px' valign='middle'><b>Appointment date</b></td><td style='font-size:12px;' align='center' height='25px' valign='middle'><b>Transaction type</b></td>";
                //strhtml = strhtml + "<td style='font-size:12px;' align='right' width='60px' height='25px' valign='middle'><b>Charge</b></td>";
                //strhtml = strhtml + "<td style='font-size:12px;' align='right' width='60px' height='25px' valign='middle'><b>Payment</b></td>";
                //strhtml = strhtml + "</tr>";
                //if (dstran.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i <= dstran.Tables[1].Rows.Count - 1; i++)
                //    {
                //        string strTdate = Convert.ToString(dstran.Tables[1].Rows[i]["Transaction_Date"].ToString());
                //        if (strTdate != null & strTdate != "")
                //        {
                //            strTdate = DateTime.Parse(strTdate).ToShortDateString();
                //        }
                //        string strAdate = Convert.ToString(dstran.Tables[1].Rows[i]["Appointmentdate"].ToString());

                //        if (strAdate != null & strAdate != "")
                //        {
                //            strAdate = DateTime.Parse(strAdate).ToShortDateString();
                //        }

                //        strhtml = strhtml + "<tr class='background_color' borderColor='#d3d3d3'><td style='font-size:10px;' align='center'>" + strTdate + "</td><td style='font-size:12px;'>" + strAdate + "</td><td style='font-size:10px;' align='center'>" + Convert.ToString(dstran.Tables[1].Rows[i]["Transactiontype"].ToString()) + "</td>";
                //        strhtml = strhtml + "<td style='font-size:10px;' align='right' width='60px'>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[1].Rows[i]["AmountCharged"])) + "</td>";
                //        strhtml = strhtml + "<td style='font-size:10px;' align='right' width='60px'>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[1].Rows[i]["Payment"])) + "</td>";
                //        strhtml = strhtml + "</tr>";
                //    }
                //}

                //strhtml = strhtml + "</table>";


                //strhtml = strhtml + "<table cellpadding='3' cellspacing='1' align='center' borderColor='#d3d3d3' border='0' border-collapse='collapse' width='85%'>";
                //strhtml = strhtml + "<tr borderColor='#d7d7d7'><td></td><td></td><td style='font-size:12px;' align='right' bgcolor='#EEEEEE' valign='middle'><b>Totals :</b></td>";
                //strhtml = strhtml + "<td style='font-size:12px;' align='right' width='60px' bgcolor='#EEEEEE' valign='middle'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceCharges"])) + "</span></td>";
                //strhtml = strhtml + "<td style='font-size:12px;' align='right' width='60px' bgcolor='#EEEEEE' valign='middle'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoicePayments"])) + "</span></td>";
                //strhtml = strhtml + "</tr>";
                //strhtml = strhtml + "<tr borderColor='#d3d3d3'><td></td><td></td><td style='font-size:12px;' align='right' bgcolor='#EEEEEE' valign='middle'><b>Balance :</b></td>";
                //strhtml = strhtml + "<td style='font-size:12px;' align='center' width='60px' bgcolor='#EEEEEE' colspan='2' valign='middle'><span>" + Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceBalance"])) + "</span></td>";
                //strhtml = strhtml + "</tr>";

                //strhtml = strhtml + "</table><br /><br /><br /><br />";
                //strhtml = strhtml + "<br /><br /><br /><br /><br /><br /><br /><table cellpadding='0' cellspacing='0' align='center' border='0' width='65%'><tr><td align='center'><font size='10'><strong>Thank you for your business</strong></font> </td></tr></table>";
                //pdf.AddHTMLPos(300, 10, strhtml);
                //string pdfname = "Invoice on " + strRdate + ".pdf";
                //Response.Clear();
                //Response.ClearHeaders();
                //Response.ClearContent();
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-disposition", "attachment; filename=" + pdfname);
                //Response.BinaryWrite((byte[])pdf.SaveVariant());
                //Response.End();

            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, "ClientsController", "DownloadInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
    }
}


