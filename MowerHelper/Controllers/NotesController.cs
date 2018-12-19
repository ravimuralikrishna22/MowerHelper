using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using MowerHelper.Models;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class NotesController : Controller
    {
        List<NotesInfo> objcollection = null;
        NotesInfo objInsNotesInfo = new NotesInfo();
        clsCommonFunctions objcom = new clsCommonFunctions();
        DataSet ds = new DataSet();
        NotesInfo objUpdNotesInfo = new NotesInfo();
        int strUser1;
        int strrole1;
        string strPatientInd1;
        //clsWebConfigsettings objconfig = new clsWebConfigsettings();
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult NotesType(string NoOfRecords, string pageno, string userid)
        {          
            NotesInfo notes = new NotesInfo();
            List<NotesInfo> val =    notes.NotesType1(NoOfRecords, pageno, userid);
            return Json(JsonResponseFactory.SuccessResponse(val), JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisplayProfileNotes()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.Count = 2;
                Session["CurrentUrl1"] = Request.RawUrl;

                if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInElectricianLogin").ToUpper() == "YES")
                {
                    ViewBag.Showforums = "Y";
                }
                else
                {
                    ViewBag.Showforums = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
                {
                    ViewBag.Showtask = "Y";
                }
                else
                {
                    ViewBag.Showtask = "N";
                }
                string startdate;
                startdate = DateTime.Now.ToString("MM/dd/yyyy");
                if (Session["Prov_ID"] != null)
                {

                    ViewBag.ProviderID = Session["Prov_ID"];
                }
                else
                {

                    ViewBag.ProviderID = null;
                }
                Session.Add("TopIndex", "4");
                NotesInfo ObjNotesinfo = new NotesInfo();
                ObjNotesinfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
                //if(Session["RoleID"].ToString()!="1")
                //{
                //ObjNotesinfo.PracticeID =  Convert.ToInt32(Session["Practice_ID"]);
                //}
                if (!string.IsNullOrEmpty(Request["hdnnoteclientid"]) && !string.IsNullOrEmpty(Request["txtcustomer"]))
                {
                    TempData["hdnnoteclientid"] = "";
                    TempData["hdnnoteclientid"] = Request["hdnnoteclientid"];
                    ObjNotesinfo.ToReference_ID = Convert.ToInt32(Request["hdnnoteclientid"]);
                }
                if (!string.IsNullOrEmpty(Request["txtcustomer"]))
                {
                    ViewBag.txtcustomer = Request["txtcustomer"];
                }
                if (Request["rdoActiveArchive"] != null)
                {
                    if (Request["rdoActiveArchive"] == "Active")
                    {
                        ObjNotesinfo.status_ind = "A";
                    }
                    else
                    {
                        ObjNotesinfo.status_ind = "E";
                    }
                }
                else
                {
                    ObjNotesinfo.status_ind = "A";
                }
                ViewBag.status_ind = ObjNotesinfo.status_ind;
                ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
                ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
                ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
                ViewBag.cat = Request["txtCategory"];

                ObjNotesinfo.IsPatientNote_Ind = null;

                string FromDate = null;
                string ToDate = null;

                if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
                {
                    FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"]);
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
                ViewBag.Fromdate = FromDate;
                ObjNotesinfo.from_date = FromDate;
                ObjNotesinfo.to_date = ToDate;
                if (Request["txtnotes"] != "" && Request["txtnotes"] != null)
                {


                    ObjNotesinfo.notes_keyword = Convert.ToString(Request["txtnotes"]);
                    ViewBag.notes = HttpUtility.HtmlDecode(Convert.ToString(Request["txtnotes"]));
                }
                else
                {
                    ObjNotesinfo.notes_keyword = null;
                    ViewBag.notes = null;
                }
                //ObjNotesinfo.ToReference_ID = null;
                ObjNotesinfo.NoOfrecords = 10;
                if (!string.IsNullOrEmpty(Request["page"]))
                {
                    ObjNotesinfo.PageNO = Convert.ToInt32(Request["page"]);
                }
                else
                {
                    ObjNotesinfo.PageNO = 1;
                    ViewBag.page = "1";
                }
                ObjNotesinfo.OrderBy = (string.IsNullOrEmpty(Request["sortdir"]) ? "DESC" : Request["sortdir"]);
                ObjNotesinfo.OrderByItem = (string.IsNullOrEmpty(Request["sort"]) ? "Notes_Date" : Request["sort"]);
                objcollection = NotesInfo.GetNotesInfo(ObjNotesinfo);
                ViewBag.statusind = ObjNotesinfo.status_ind;
                ViewBag.totrec = ObjNotesinfo.TotalRecords;

                return PartialView("_ProfileNotes", objcollection);
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
        public ActionResult ProfileNotes()
        {
            //ViewBag.Count = 2;
            //Session["CurrentUrl1"] = Request.RawUrl;

            //if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInElectricianLogin").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            //{
            //    ViewBag.Showtask = "Y";
            //}
            //else
            //{
            //    ViewBag.Showtask = "N";
            //}
            //string startdate;
            //startdate = DateTime.Now.ToString("MM/dd/yyyy");
            //if (Session["Prov_ID"] != null)
            //{

            //    ViewBag.ProviderID = Session["Prov_ID"];
            //}
            //else
            //{

            //    ViewBag.ProviderID = null;
            //}
            //Session.Add("TopIndex", "4");
            //NotesInfo ObjNotesinfo = new NotesInfo();
            //ObjNotesinfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
            ////if(Session["RoleID"].ToString()!="1")
            ////{
            ////ObjNotesinfo.PracticeID =  Convert.ToInt32(Session["Practice_ID"]);
            ////}
            //if (!string.IsNullOrEmpty(Request["hdnnoteclientid"]) && !string.IsNullOrEmpty(Request["txtcustomer"]))
            //{
            //    TempData["hdnnoteclientid"] = "";
            //    TempData["hdnnoteclientid"] = Request["hdnnoteclientid"];
            //    ObjNotesinfo.ToReference_ID = Convert.ToInt32(Request["hdnnoteclientid"]);
            //}
            //if (!string.IsNullOrEmpty(Request["txtcustomer"]))
            //{
            //    ViewBag.txtcustomer = Request["txtcustomer"];
            //}
            if (Request["rdoActiveArchive"] != null)
            {
                if (Request["rdoActiveArchive"] == "Active")
                {
                    ViewBag.status_ind = "A";
                }
                else
                {
                    ViewBag.status_ind = "E";
                }
            }
            else
            {
                ViewBag.status_ind = "A";
            }
            //ViewBag.status_ind = ObjNotesinfo.status_ind;
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            //ViewBag.cat = Request["txtCategory"];

            //ObjNotesinfo.IsPatientNote_Ind = null;

            //string FromDate = null;
            //string ToDate = null;

            //if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            //{
            //    FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"]);
            //    ToDate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? startdate : Request["txt_ToDate"]);
            //}
            //else
            //{

            //    if (Request["dt_filter"] == "Today")
            //    {
            //        FromDate = startdate;
            //    }
            //    else if (Request["dt_filter"] == "7")
            //    {
            //        FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
            //    }
            //    else if (Request["dt_filter"] == "30")
            //    {
            //        FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
            //    }
            //    ToDate = startdate;
            //}
            //ObjNotesinfo.from_date = FromDate;
            //ObjNotesinfo.to_date = ToDate;
            //if (Request["txtnotes"] != "" && Request["txtnotes"] != null)
            //{


            //    ObjNotesinfo.notes_keyword = Convert.ToString(Request["txtnotes"]);
            //    ViewBag.notes = HttpUtility.HtmlDecode(Convert.ToString(Request["txtnotes"]));
            //}
            //else
            //{
            //    ObjNotesinfo.notes_keyword = null;
            //    ViewBag.notes = null;
            //}
            ////ObjNotesinfo.ToReference_ID = null;
            //ObjNotesinfo.NoOfrecords = 10;
            //ObjNotesinfo.PageNO = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            //ObjNotesinfo.OrderBy = (Request["sortdir"] ?? "DESC");
            //ObjNotesinfo.OrderByItem = (Request["sort"] ?? "Notes_Date");
            //objcollection = NotesInfo.GetNotesInfo(ObjNotesinfo);
            //ViewBag.statusind = ObjNotesinfo.status_ind;
            //ViewBag.totrec = ObjNotesinfo.TotalRecords;
            //return View(objcollection);
            return View();
        }
        
        public JsonResult NotesList(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
                                            new SqlParameter("@in_FromReference_ID", (Session["UserID"] == null ? null : Session["UserID"])),
		new SqlParameter("@in_keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.st_typeahead_get_notes");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString(drlist[0]));
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddNotes()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.ToReference_ID = Request["ToReference_ID"];
                ViewBag.txtsnotes = Request["txtnotes"];
                ViewBag.txtcustomer = Request["txtcustomer"];
                ViewBag.dt_filter = Request["dt_filter"];
                ViewBag.txt_FromDate = Request["txt_FromDate"];
                ViewBag.txt_ToDate = Request["txt_ToDate"];
                ViewBag.notespage = Request["notespage"];
                ViewBag.rdoActiveArchive = Request["rdoActiveArchive"];
                if (string.IsNullOrEmpty(Request["notespage"]))
                {
                    ViewBag.notespage = "1";
                }
                FillPatients();
                ViewBag.Todaydate = DateTime.Now.ToString("MM/dd/yyyy");
                return View();
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddNotes(string id)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                int strUser1;
                int strrole1;
                string strPatientInd1;
                try
                {
                    if (Request["RBPatient"] != null)
                    {
                        if (Request["ComboPatients"] != null)
                        {
                            strUser1 = Convert.ToInt32(Request["ComboPatients"]);
                        }
                        else
                        {
                            strUser1 = Convert.ToInt32(Session["PatientID"]);
                        }

                        strrole1 = 5;
                        strPatientInd1 = "Y";
                    }
                    else
                    {
                        strUser1 = 0;
                        strrole1 = 0;
                        strPatientInd1 = "N";
                    }
                    objInsNotesInfo.ToReference_ID = strUser1;
                    objInsNotesInfo.ToReferenceType_ID = strrole1;
                    objInsNotesInfo.FromReference_ID = Convert.ToInt32(Session["UserID"]);
                    objInsNotesInfo.FromReferenceType_ID = Convert.ToInt32(Session["RoleID"]);
                    objInsNotesInfo.IsPatientNote_Ind = strPatientInd1;
                    objInsNotesInfo.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                    if (Request["txt_Date"] != null & Request["txt_Date"] != "")
                    {
                        objInsNotesInfo.Notes_Date = Convert.ToDateTime(Request["txt_Date"]);

                    }
                    else
                    {
                        objInsNotesInfo.Notes_Date = null;

                    }

                    objInsNotesInfo.CreatedBy = Convert.ToString(Session["UserID"]);
                    //if (Session["Roleid"].ToString() == "1")
                    //{
                    //    objInsNotesInfo.PracticeID = null;
                    //}
                    //else
                    //{
                    //    objInsNotesInfo.PracticeID = Convert.ToInt32(Session["Practice_ID"]);
                    //}
                    objInsNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                    NotesInfo.InsNotesInfo(objInsNotesInfo);
                    return Json(JsonResponseFactory.SuccessResponse("true"), JsonRequestBehavior.DenyGet);
                    //return RedirectToAction("ProfileNotes");
                }
                catch (Exception ex)
                {
                    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
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
        
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditNotes(string Generalnoteid)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.txtcustomer = Request["txtcustomer"];
                //ViewBag.RecPerPage = RecPerPage;
                ViewBag.dt_filter = Request["dt_filter"];
                ViewBag.txt_FromDate = Request["txt_FromDate"];
                ViewBag.txt_ToDate = Request["txt_ToDate"];
                ViewBag.page = Request["page"];
                ViewBag.txtnotes = Request["txtnotes"];
                ViewBag.RecPerPage = Request["RecPerPage"];

                ViewBag.sortdir = (string.IsNullOrEmpty(Request["sortdir"]) ? "DESC" : Request["sortdir"]);
                ViewBag.sort = (string.IsNullOrEmpty(Request["sort"]) ? "Notes_Date" : Request["sort"]); ;
                ViewBag.NID = Generalnoteid;

                if (Request["Generalnoteid"] != null)
                {
                    EditGeneralnotedetails(Convert.ToInt32(Request["Generalnoteid"]));
                }
                return View();
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
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditNotes()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                try
                {
                    Savenotes();
                    //return RedirectToAction("ProfileNotes");
                    return Json(JsonResponseFactory.SuccessResponse("true"), JsonRequestBehavior.DenyGet);
                }
                catch (Exception ex)
                {

                    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
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


        public void EditGeneralnotedetails(int noteid)
        {
            try
            {
            NotesInfo objNotesInfo = new NotesInfo();
            objNotesInfo = NotesInfo.GetupdNotesInfo(noteid);
            if ((objNotesInfo.Notes != null))
            {
                ViewBag.Notes=objNotesInfo.Notes;
            }
            else
            {
                ViewBag.Notes = objNotesInfo.Notes;
            }
            if (Convert.ToString(objNotesInfo.Notes_Date) != "")
            {
                string[] notedate = Convert.ToString(objNotesInfo.Notes_Date).Split(' ');
                ViewBag.Notes_Date = notedate[0];
                if (ViewBag.Notes_Date == "01/01/0001" || ViewBag.Notes_Date == "1/1/0001")
                {
                    ViewBag.Notes_Date = null;
                }
            }
            else
            {
                ViewBag.Notes_Date = null;
            }
            if ((objNotesInfo.IsPatientNote_Ind != null))
            {
                if (objNotesInfo.IsPatientNote_Ind == "Y")
                {
                    ViewBag.type = "1";
                    ViewBag.note_type = "1";
                    IDataParameter[] objInparam = { new SqlParameter("@in_Patient_ID", objNotesInfo.ToReference_ID) };
                    objcom.AddInParameters(objInparam);
                    ds = objcom.GetDataSet("Help_dbo.st_GET_Patient_Details");
                    if ((ds.Tables[0].Rows[0]["patientname"]) != null)
                    {
                        ViewBag.patientname = ds.Tables[0].Rows[0]["patientname"];
                    }
                    else
                    {
                        ViewBag.patientname = null;
                    }
                    ViewBag.PID = objNotesInfo.ToReference_ID;
                }
                else if (objNotesInfo.IsPatientNote_Ind == "N")
                {
                    ViewBag.type = "2";
                    ViewBag.note_type = "1";
                    ViewBag.PID = null;
                    ViewBag.patientname = null;
                }
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "NotesController", "EditGeneralnotedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }


        protected void Savenotes()
        {
            try
            {
            if (Request["RBPatient_Edit"] == "1")
            {
                strUser1 = Convert.ToInt32(Request["HdnPID_Edit"]);


                strrole1 = 5;
                strPatientInd1 = "Y";
            }
            else
            {
                strUser1 = 0;
                strrole1 = 0;
                strPatientInd1 = "N";
            }
            if (Request["RBNonPatient_Edit"] == "1")
            {
                objUpdNotesInfo.GeneralNote_ID = Convert.ToInt32(Request["HdnNID_Edit"]);
                objUpdNotesInfo.ToReference_ID = strUser1;
                objUpdNotesInfo.ToReferenceType_ID = strrole1;
                objUpdNotesInfo.Notes =Sanitizer.GetSafeHtmlFragment(Request["txtNotes_Edit"]);
                objUpdNotesInfo.IsPatientNote_Ind = strPatientInd1;
                if ((Convert.ToString(Request["txt_Date_Edit"]) == " ") || (Convert.ToString(Request["txt_Date_Edit"]) == ""))
                {
                    objUpdNotesInfo.Notes_Date = null;
                }
                else
                {
                    objUpdNotesInfo.Notes_Date = Convert.ToDateTime(Request["txt_Date_Edit"]);
                }
                objUpdNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objUpdNotesInfo);
            }
            else
            {
                objUpdNotesInfo.GeneralNote_ID = Convert.ToInt32(Request["HdnNID_Edit"]);
                objUpdNotesInfo.ToReference_ID = strUser1;
                objUpdNotesInfo.ToReferenceType_ID = strrole1;
                objUpdNotesInfo.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes_Edit"]);
                objUpdNotesInfo.IsPatientNote_Ind = strPatientInd1;
                if ((Convert.ToString(Request["txt_Date_Edit"])) != "")
                {
                    objUpdNotesInfo.Notes_Date = Convert.ToDateTime(Request["txt_Date_Edit"]);
                    
                }
                else
                {
                    objUpdNotesInfo.Notes_Date = null; 
                }
                objUpdNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objUpdNotesInfo);
            }

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "NotesController", "Savenotes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }

        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ChangeStatus(int Generalnoteid, string notetype, String ispatient, string dt_filter = null, string txt_FromDate = null, string txt_ToDate = null, int? page = null, string sortdir = null, int? RecPerPage = null, string ToReference_ID = null, string txtnotes = null, string txtcustomer = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                NotesInfo objnotes = new NotesInfo();
                objnotes.GeneralNote_ID = Generalnoteid;
                objnotes.status_ind = "A";
                objnotes.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                int patientrole = 0;

                if (ispatient == "Y")
                {
                    patientrole = 5;
                }
                else
                {
                    patientrole = 0;
                }
                if (notetype == "General Notes")
                {
                    updategeneralnotedetails(objnotes, patientrole, notetype, "Archive");
                }
                if (RecPerPage == 1)
                {
                    page = page - 1;
                }
                return RedirectToAction("DisplayProfileNotes", new { dt_filter = dt_filter, txt_FromDate = txt_FromDate, txt_ToDate = txt_ToDate, page = page, sortdir = sortdir, hdnnoteclientid = ToReference_ID, txtnotes = txtnotes, txtcustomer = txtcustomer });
                //return RedirectToAction("ProfileNotes");
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
        public ActionResult deleteNote(int Generalnoteid, string notetype, String ispatient, string dt_filter = null, string txt_FromDate = null, string txt_ToDate = null, string page = null, string sortdir = null, string txtnotes = null, string txtcustomer = null, string hdnnoteclientid = null)    
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                NotesInfo objnotes = new NotesInfo();
                objnotes.GeneralNote_ID = Generalnoteid;
                objnotes.status_ind = "E";
                objnotes.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                int patientrole = 0;

                if (ispatient == "Y")
                {
                    patientrole = 5;
                }
                else
                {
                    patientrole = 0;
                }
                if (notetype == "General Notes")
                {
                    updategeneralnotedetails(objnotes, patientrole, notetype, "Delete");
                }
                //return RedirectToAction("../Notes/ProfileNotes", new { rdoActiveArchive = "Archieve" });
                return RedirectToAction("DisplayProfileNotes", new { dt_filter = dt_filter, txt_FromDate = txt_FromDate, txt_ToDate = txt_ToDate, page = page, rdoActiveArchive = "Archieve", sortdir = sortdir, txtnotes = txtnotes, txtcustomer = txtcustomer, hdnnoteclientid = hdnnoteclientid });
                //return RedirectToAction("ProfileNotes");
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
        
        public void updategeneralnotedetails(NotesInfo objnotes, int patientrole, string notetype, string id)
        {
            try
            {
            if (objnotes.status_ind == "A")
            {
                objUpdNotesInfo.GeneralNote_ID = objnotes.GeneralNote_ID;
                objUpdNotesInfo.ToReferenceType_ID = patientrole;
                objUpdNotesInfo.status_ind = "E";
                objUpdNotesInfo.notetype = notetype;
                objUpdNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objUpdNotesInfo);
            }
            if (objnotes.status_ind == "E")
            {
                objUpdNotesInfo.GeneralNote_ID = objnotes.GeneralNote_ID;
                objUpdNotesInfo.ToReferenceType_ID = patientrole;

                if (id == "Delete")
                {
                    objUpdNotesInfo.status_ind = "D";
                }
                else
                {
                    objUpdNotesInfo.status_ind = "A";
                }
                objUpdNotesInfo.notetype = notetype;
                objUpdNotesInfo.LoginHistory_ID = Convert.ToInt32(Session["LoginHistory_ID"]);
                NotesInfo.UpdNotesInfo(objUpdNotesInfo);
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "NotesController", "updategeneralnotedetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        
        public bool CheckingRemoteFileExistance(string strpath)
        {
            WebClient myclient = new WebClient();
            try
            {
                byte[] responseBytes = myclient.DownloadData(strpath);
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "NotesController", "CheckingRemoteFileExistance", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return false;
            }
            finally
            {
                myclient = null;
            }
        }

        public void FillPatients()
        {
            try
            {
            ListofPatients getPatient = new ListofPatients();
            List<ListofPatients> objlist = new List<ListofPatients>();
            objlist = getPatient.getPatientList(Convert.ToInt32(Session["Prov_ID"]));
            ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
            ViewData["ComboPatients"] = custlist;
            if (Convert.ToString(Session["RoleID"]) != "1")
            {
                string str = Convert.ToString(Session["Practice_ID"]);
                string struid = Convert.ToString(Session["UserID"]);
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "NotesController", "FillPatients", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }

    }
}
