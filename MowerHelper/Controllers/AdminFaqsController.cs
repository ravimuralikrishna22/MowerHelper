using Obout.Mvc.ComboBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class AdminFaqsController : Controller
    {
        static string CustomizeFileName;
        static string encCustomizeFileName;
        static string HomePageFileName;
        clsCommonFunctions ObjCommFun = new clsCommonFunctions();
        DataSet dsSections = new DataSet();
        clsDBWrapper objDBWrapper = new clsDBWrapper();
        public ActionResult Help()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "4");
            char? AcPrv;
            if (Session["strOutIsPaid"] != null)
            {
                AcPrv = Convert.ToChar(Session["strOutIsPaid"]);
            }
            else
            {
                AcPrv = 'Y';
            }
            dsSections = ChildSections(Convert.ToString(Session["userid"]), "366", Convert.ToString(Session["Roleid"]), AcPrv);
            StringBuilder strAdmin = new StringBuilder();
            strAdmin = strAdmin.Append("<table id='Toolstable' border='0' cellpadding='20' cellspacing='0' width='100%' >");
            if (dsSections.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                strAdmin = strAdmin.Append("<tr class='background_color'>");
                string RedirectPath = string.Empty;
                foreach (DataRow drs in dsSections.Tables[0].Rows)
                {
                    if (Convert.ToString(drs["SectionsInRole_ID"]) != "13710" & Convert.ToString(drs["SectionsInRole_ID"]) != "13328" & Convert.ToString(drs["SectionsInRole_ID"]) != "2826")
                    {
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/FAQComments.aspx")
                        {
                            RedirectPath = Url.Action("FAQComments", "AdminFaqs", null);
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/FAQuestionsList.aspx")
                        {
                            RedirectPath = Url.Action("AdminFaQuestionsList","AdminFaqs",null);
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/HelpContent.aspx")
                        {
                            RedirectPath = Url.Action("HelpContent", "AdminFaqs", null);
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/Knowledgebase.aspx")
                        {
                            RedirectPath = Url.Action("Knowledgebase", "AdminFaqs", null);
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/Phoneandemailsupport.aspx")
                        {
                            RedirectPath = Url.Action("Phoneandemailsupport", "AdminFaqs", null);
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/TutorialsHome.aspx")
                        {
                            RedirectPath = Url.Action("TutorialsHome", "AdminFaqs", null);
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
                }
                strAdmin = strAdmin.Append("</tr>");
            }
            strAdmin = strAdmin.Append("</table>");
            ViewBag.Adminhelp = strAdmin;
            return View();
        }
        public DataSet ChildSections(string login_id, string section_id, string Role_id, char? IsBasicAccProv)
        {

            try
            {
            string IS_Billing_Ind = null;
                string IS_receptionist_ind = null;
                DataSet dsSections = new DataSet();
                int? strProviderID = 0;
                int? strPracticeID = 0;
                if (Convert.ToString(Session["RoleID"]) == "15" | Convert.ToString(Session["RoleID"]) == "4")
                {
                    strProviderID = Convert.ToInt32(Session["Prov_ID"] == null ? Session["Provider_ID"] : Session["Prov_ID"]);
                    strPracticeID = Convert.ToInt32(Session["Prov_ID"]);
                }
                IDataParameter[] objInparameters = {
			new SqlParameter("@in_Login_ID", Convert.ToInt32(login_id)),
			new SqlParameter("@in_Section_ID", Convert.ToInt32(section_id)),
			new SqlParameter("@in_RoleId", Convert.ToInt32(Role_id)),
			new SqlParameter("@in_Provider_ID", (strProviderID == 0 ? null : strProviderID)),
			new SqlParameter("@in_Practice_ID", (strPracticeID == 0 ? null : strPracticeID)),
			new SqlParameter("@In_IsBasicACPrv", (IsBasicAccProv == null ? 'Y' : IsBasicAccProv)),
			new SqlParameter("@In_IsClerk_ind", IS_Billing_Ind),
			new SqlParameter("@In_IsRecept_Ind", IS_receptionist_ind)
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
                clsCustomEx.LogException(ex, "AdminFaqsController", "ChildSections", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult FAQComments()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            Publicfaq obj = new Publicfaq();
            var objList =new List<Publicfaq>();
            if (Request["hdnQuestion"] != null && Request["hdnQuestion"] != "")
            {
                obj.Question_id = Request["hdnQuestion"];
            }
            if (Request["txtQuestion"] != null && Request["txtQuestion"] != "")
            {
                ViewBag.question1 = Request["txtQuestion"];
                obj.Question = Request["txtQuestion"];
            }
            else
            {
                obj.Question = null;
            }
           if (!string.IsNullOrEmpty(Convert.ToString(Request["page"])))//!=null & Request["page"]!="")
            {
                obj.PageNo=Request["page"].ToString();
            }
            else{
                 obj.PageNo = "1";
            }
           
            obj.NoofRecords = "10";
            if (!string.IsNullOrEmpty(Convert.ToString(Request["sortdir"])))// != null & Request["sortdir"] != "")
            {
                obj.OrderBy = Request["sortdir"].ToString();
            }
            else
            {
                obj.OrderBy = "DESC";
            }
            
            obj.OrderByItem = "CreatedOn";
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

            obj.DateFrom = FromDate;
            obj.DateTo = ToDate;
            
            
            obj.Question = null;
            obj.Status_Ind = null;
            objList = Publicfaq.ListFAQComments(obj);
            ViewBag.totalrec = Publicfaq.TotalRecords;

            return View("FAQComments",objList);
        }


        public JsonResult AdminFAQList(string term)
        {

            var objlist = new List<Autocomplete>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Keyword", term) };

            objcommon.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcommon.GetDataReader("Help_dbo.St_Admin_Typeahead_FAQ");
            while (drlist.Read())
            {

                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[1]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ChangeStatus(string Question_id, string Rating_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq obj = new Publicfaq();
            obj.Relatedfaq_Id = Convert.ToInt32(Rating_id);

            Publicfaq.ArchiveFAQComment(obj);
            return RedirectToAction("FAQComments");
        }
        public ActionResult ViewFaqComments(string Question_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            DataSet ds=new DataSet();
            ds = Publicfaq.EditQuestion(Question_ID);
            if (ds.Tables[0].Rows[0]["Categories"]!=DBNull.Value)
            {
                ViewBag.lblCategory = Convert.ToString(ds.Tables[0].Rows[0]["Categories"]);
                if (ViewBag.lblCategory.Contains("$"))
                {
                    ViewBag.lblCategory = ViewBag.lblCategory.Replace("$", ", ");
                }
                else
                {
                    ViewBag.lblCategory = ViewBag.lblCategory;
                }
            }
            else
            {
                ViewBag.lblCategory = null;
            }
            if (ds.Tables[0].Rows[0]["Question"]!=DBNull.Value)
            {
               ViewBag.question = ds.Tables[0].Rows[0]["Question"].ToString();
            }
            else
            {
                ViewBag.question = null;
            }

            if (ds.Tables[0].Rows[0]["Answertext"]!=DBNull.Value)
            {
                ViewBag.lblAnswer = ds.Tables[0].Rows[0]["Answertext"].ToString();
            }
            else
            {
                ViewBag.lblAnswer = null;
            }

            if (ds.Tables[0].Rows[0]["Status"]!=DBNull.Value)
            {
               ViewBag.lblStatus = ds.Tables[0].Rows[0]["Status"].ToString();
            }
            else
            {
                ViewBag.lblStatus = null;
            }
            return View();
        }
        public JsonResult Dropdownindexchange(string ddlPageType, string ddroles, string txtPageno)
        {
            var obj = new Clshelp();
            string strPagetype = null;
            if (ddroles == "NO ROLe")
            {
            }
            if (ddlPageType == "Insert")
            {
                strPagetype = "I";
            }
            else if (ddlPageType == "Edit")
            {
                strPagetype = "E";
            }
            else if (ddlPageType == "View")
            {
                strPagetype = "V";
            }
            else if (ddlPageType == "Accounts")
            {
                strPagetype = "A";
            }
            else if (ddlPageType == "Physicians")
            {
                strPagetype = "P";
            }
            else if (ddlPageType == "Attorneys")
            {
                strPagetype = "T";
            }
            else if (ddlPageType == "Collection Agencies")
            {
                strPagetype = "C";
            }

            Clshelp objdrhelp = new Clshelp();
           
            SqlDataReader drhelp = default(SqlDataReader);
            string Str_RoleID = null;
            Str_RoleID = ddroles;
            if (ddroles == "NO ROLE")
            {
                Str_RoleID = null;
            }
            
            IDataParameter[] paramlist = {
	new SqlParameter("@in_Role_ID", Str_RoleID),
	new SqlParameter("@in_Pagetype_Ind", strPagetype),
	new SqlParameter("@in_Screen_ID", (!string.IsNullOrEmpty(txtPageno) ? txtPageno.ToString() : null))
};
            drhelp = objdrhelp.Get_DataReader("Help_dbo.st_Help_Get_HelptextByPageType", paramlist);
            if (drhelp.Read())
            {
                obj.HelpText_ID = Convert.ToString(drhelp["ScreenHelpText_ID"]);
                if (drhelp["HelpText"]!=DBNull.Value)
                {
                    obj.HelpText = drhelp["HelpText"].ToString();
                }
                if (drhelp["AdminNote"]!=DBNull.Value)
                {
                    obj.AdminNote = drhelp["AdminNote"].ToString();
                }
               // return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                obj.HelpText_ID = null;
                obj.HelpText = "";
                obj.AdminNote = "";
               // return Json(obj, JsonRequestBehavior.AllowGet);
            }

           
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Knowledgebase()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objDataList = new List<Referrals>();
            try
            {
                Referrals video = new Referrals();
                video.FieldIDString = null;
                DataSet ds = default(DataSet);
                ds = Referrals.List_field_description(video.FieldIDString);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var objData = new Referrals();
                            if (dr["Fieldid"] != null)
                            {
                                objData.Fieldid = Convert.ToInt32(dr["Fieldid"]);
                            }
                            else
                            {
                                objData.Fieldid = null;
                            }
                            if (dr["Modulename"] != null)
                            {
                                objData.Modulename = dr["Modulename"].ToString();
                            }
                            else
                            {
                                objData.Modulename = null;
                            }
                            objData.Fieldname = Convert.ToString(dr["Fieldname"]) ?? null;

                            objData.Description = Convert.ToString(dr["description"]) ?? null;


                            objDataList.Add(objData);
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminFaqsController", "Knowledgebase", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            ViewBag.totalrec = objDataList.Count;


            return View("Knowledgebase", objDataList);
        }
        public ActionResult AddKnowledgebase()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                return View();
            
        }
        public ActionResult Phoneandemailsupport()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        [HttpPost()]
        public ActionResult AddKnowledgebase(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                var Articles = new Referrals();
                try
                {
                    int modid = 0;
                    string category_name = (!string.IsNullOrEmpty(Request["txtcategoryname"]) ? Request["txtcategoryname"].ToString() : null);
                    List<string> objlist = new List<string>();
                    clsCommonFunctions objcommon = new clsCommonFunctions();
                    IDataParameter[] objparam = { new SqlParameter("@in_Modulename", Convert.ToString(Request["txtcategoryname"])) };
                    objcommon.AddInParameters(objparam);
                    SqlDataReader drlist = default(SqlDataReader);
                    drlist = objcommon.GetDataReader("Help_dbo.st_get_typeahead_modulename");
                    while (drlist.Read())
                    {
                        objlist.Add("'" + Convert.ToString(drlist[0]) + "'");
                        modid = Convert.ToInt32(drlist[0]);
                    }

                    string category_descr = null;
                    string Title = null;
                    Title = (Request["txtArticleName"] != null ? Request["txtArticleName"].ToString() : null);
                    string field_Desc = Request["txtArticleDesc"];
                    field_Desc = (Request["txtArticleDesc"] != null ? Request["txtArticleDesc"] : null);
                    string field_status_ind = null;
                    field_status_ind = "A";
                    Articles.Moduleid = modid;
                    Articles.Fieldid = null;
                    Articles.Modulename = category_name;
                    Articles.moduledescription = category_descr;
                    Articles.Fieldname = Title;
                    Articles.descriptionfield = field_Desc;
                    Articles.statusindfield = field_status_ind;
                    Referrals.Ins_module_field_description(Articles);

                }
                catch (Exception ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminFaqsController", "AddKnowledgebase", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

                }
                return RedirectToAction("Knowledgebase");
            
        }
        public ActionResult EditKnowledgebase(string Fieldid)
        {
            
                ViewBag.Fieldid = Fieldid;
                DataSet ds = default(DataSet);
                ds = Referrals.List_field_description(Fieldid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.txtArticleDesc1 = Convert.ToString(ds.Tables[0].Rows[0][3]);
                    ViewBag.txtArticleName1 = Convert.ToString(ds.Tables[0].Rows[0][2]);
                    ViewBag.lblcategoryname = Convert.ToString(ds.Tables[0].Rows[0][1]);
                }
                return View();
            
        }
        [HttpPost()]
        public ActionResult EditKnowledgebase()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                var Articles = new Referrals();
                try
                {
                    int modid = 0;
                    string category_name = (!string.IsNullOrEmpty(Request["txtcategoryname1"]) ? Request["txtcategoryname1"].ToString() : null);
                    List<string> objlist = new List<string>();
                    clsCommonFunctions objcommon = new clsCommonFunctions();
                    IDataParameter[] objparam = { new SqlParameter("@in_Modulename", Convert.ToString(Request["txtcategoryname1"])) };
                    objcommon.AddInParameters(objparam);
                    SqlDataReader drlist = default(SqlDataReader);
                    drlist = objcommon.GetDataReader("Help_dbo.st_get_typeahead_modulename");
                    while (drlist.Read())
                    {
                        objlist.Add("'" + Convert.ToString(drlist[0]) + "'");
                        modid = Convert.ToInt32(drlist[0]);
                    }
                    string category_descr = null;
                    string Title = null;
                    Title = (Request["txtArticleName1"] != null ? Request["txtArticleName1"].ToString() : null);
                    string field_Desc = Request["txtArticleDesc1"];
                    field_Desc = (Request["txtArticleDesc1"] != null ? Request["txtArticleDesc1"] : null);
                    string field_status_ind = null;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["hdnFieldid"])))
                    {
                        field_status_ind = null;
                    }
                    else
                    {
                        field_status_ind = "A";
                    }
                    Articles.Moduleid = modid;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["hdnFieldid"])))
                    {
                        Articles.Fieldid = Convert.ToInt32(Request["hdnFieldid"]);
                    }
                    else
                    {
                        Articles.Fieldid = null;
                    }
                    Articles.Modulename = category_name;
                    Articles.moduledescription = category_descr;
                    Articles.Fieldname = Title;
                    Articles.descriptionfield = field_Desc;
                    Articles.statusindfield = field_status_ind;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["hdnFieldid"])))
                    {
                        Referrals.upd_fieldname_description(Articles);

                    }
                    else
                    {
                        Referrals.Ins_module_field_description(Articles);

                    }

                }
                catch (Exception ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminFaqsController", "EditKnowledgebase", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

                }
                return RedirectToAction("Knowledgebase");
            
        }
        public ActionResult DeleteKnowledgebase(int Fieldid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Referrals fieldstatus = new Referrals();
            fieldstatus.statusindfield = "D";
            fieldstatus.Fieldid = Fieldid;
            Referrals.upd_fieldname_description(fieldstatus);
            return RedirectToAction("Knowledgebase");
        }
        public ActionResult TutorialsHome()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objcollection = new List<MediaInfo>();

            try
            {
                objcollection = Returnlist();


            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminFaqsController", "TutorialsHome", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return View("TutorialsHome", objcollection);
        }

        private List<MediaInfo> Returnlist(bool flag = false)
        {

            MediaInfo ObjMediainfo = new MediaInfo();
           var objcollection = new List<MediaInfo>();
            try{
                ObjMediainfo.NoOfrecords = 10;
                ObjMediainfo.PageNO = Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1;
                if (!string.IsNullOrEmpty(Convert.ToString(Request["txtTitle1"])))// != null && Request["txtTitle1"] != "")
                {
                    ViewBag.title = Request["txtTitle1"].ToString();
                    ObjMediainfo.MediaTitle = Request["txtTitle1"].ToString();
                }
                else
                {
                    ObjMediainfo.MediaTitle = null;
                }
            ObjMediainfo.OrderBy = "DESC";
            ObjMediainfo.OrderByItem = "CreadtedOn";

            objcollection = MediaInfo.GetMediaInfo(ObjMediainfo);

            ViewBag.totalrec = MediaInfo.TotalRecords;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "Returnlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objcollection;
        }
        public ActionResult AddNewMedia()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["lbloutmsg"] != null & Request["lbloutmsg"]!="")
            {
                ViewBag.outmsg = Request["lbloutmsg"].ToString();
            }
            return View();
        }

          [HttpPost()]
         public ActionResult AddNewMedia( string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            double getlength = 0;
            string strtitle = null;
            string strdescription = null;
            int i = Request.Files["myFile1"].ContentLength;
            if (i != 0)
            {
                getlength = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(i / 1024))));

            }
            if (getlength > Convert.ToDouble(10 * 1024))
            {
                string msg = "Select 10 MB video file only ";
                return RedirectToAction("AddNewMedia", new { lbloutmsg = msg });
               
            }
            else
            {
                try
                {
                    if (Request.Files["myFile1"].ContentLength > 0)
                    {
                        string out_Msg = "";
                        string Loc_ImagePathSuffix = "";
                        MediaInfo ObjInsMediaInfo = new MediaInfo();
                        ObjInsMediaInfo.Reference_ID = Convert.ToInt32(Session["userid"]);
                        ObjInsMediaInfo.ReferenceType_ID = Convert.ToInt32(Session["Roleid"]);
                        ObjInsMediaInfo.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);
                        ObjInsMediaInfo.Title = Convert.ToString(Request["txtTitle"]).Trim();
                        strtitle = Request["txtTitle"].Trim();
                        string hrs = null;
                        if (Request["txtHours"] != null)
                        {
                            hrs = Request["txtHours"].ToString();
                        }
                        else
                        {
                            hrs = "00";
                        }
                        string mins = null;
                        if (Request["txtmins"] != null)
                        {
                            mins = Request["txtmins"].ToString();
                        }
                        else
                        {
                            mins = "00";
                        }
                        string duration = hrs + ":" + mins;
                        string FileExtn2 = null;
                        HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["myFile1"].FileName);
                        FileExtn2 = System.IO.Path.GetExtension(Request.Files["myFile1"].FileName);
                        string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\Media" + "\\";
                        string hdn_LogoPaths = strFilePath + HomePageFileName + FileExtn2;
                        encCustomizeFileName = clsCommonCClist.RandomPassword();
                        hdn_LogoPaths = strFilePath +"/" + HomePageFileName + FileExtn2;
                        HomePageFileName = HomePageFileName + FileExtn2;
                        ObjInsMediaInfo.Duration = (duration != null ? duration : null);
                        ObjInsMediaInfo.Description = Request["txtDescription"];
                        strdescription = Request["txtDescription"];
                        ObjInsMediaInfo.FileType = (!string.IsNullOrEmpty(HomePageFileName) ? HomePageFileName : null);
                        ObjInsMediaInfo.FilePath = (!string.IsNullOrEmpty(hdn_LogoPaths) ? hdn_LogoPaths : null);
                        ObjInsMediaInfo.FilePath_Encrypted = (!string.IsNullOrEmpty(encCustomizeFileName) ? encCustomizeFileName : null);
                        ObjInsMediaInfo.IsPracticeMedia_Ind = null;

                        if (Request["chkactive"] == "Yes")
                        {
                            ObjInsMediaInfo.IsDisplayToPractice = "Y";
                        }
                        else
                        {
                            ObjInsMediaInfo.IsDisplayToPractice = "N";
                        }

                        MediaInfo.InsertMediaInfo(ObjInsMediaInfo, ref out_Msg, ref Loc_ImagePathSuffix);

                        
                        string ImagePathSuffix = Loc_ImagePathSuffix;
                        if (out_Msg != null && out_Msg!="")
                        {
                            return RedirectToAction("AddNewMedia", new { lbloutmsg = out_Msg });
                          

                        }
                        else
                        {
                            Request.Files["myFile1"].SaveAs(hdn_LogoPaths);


                        }
                    }

                }
                catch (Exception ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminFaqsController", "AddNewMedia", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
            }

            return RedirectToAction("TutorialsHome");
        }
          public ContentResult Download(int id)
          {
              MediaInfo ObjMediainfo = new MediaInfo();
              var objcollection = new List<MediaInfo>();
              ObjMediainfo.Helpmedia_ID = Convert.ToString(id);
              objcollection = MediaInfo.GetVideoInfo(ObjMediainfo);
              string filenae = objcollection[0].FileType;
              HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(filenae);
              string FileExtn2 = System.IO.Path.GetExtension(filenae);

              string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\Media" + "\\";
              string hdn_LogoPaths = strFilePath + HomePageFileName + FileExtn2;
              System.IO.FileInfo File = new System.IO.FileInfo(hdn_LogoPaths);
              if (File.Exists)
              {
                  Response.Clear();
                  Response.AddHeader("Content-Disposition", "Attachment;filename=" + HomePageFileName);
                  Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                  Response.ContentType = FileExtn2;
                  Response.WriteFile(File.FullName);
                  Response.End();
              }
              else
              {
                  Response.Clear();
                  Response.AddHeader("Content-Disposition", "Attachment;filename=" + HomePageFileName);
                  Response.AddHeader("Content-Length", "0");
                  Response.ContentType = FileExtn2;
                  Response.Write("file not found.");
                  Response.End();
              }
              
             
              return null;
          }
          public ActionResult download2(int id )
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              MediaInfo ObjMediainfo = new MediaInfo();
              var objcollection = new List<MediaInfo>();
              ObjMediainfo.Helpmedia_ID = Convert.ToString(id);
              objcollection = MediaInfo.GetVideoInfo(ObjMediainfo);
              string filenae = objcollection[0].FileType;
              HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(filenae);
              string FileExtn2 = System.IO.Path.GetExtension(filenae);

              string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\Media" + "\\";
               ViewBag.relatedlink = "../../Attachments/Media/" + HomePageFileName + FileExtn2;
              ViewBag.ext = FileExtn2;
              return View();
          }
          public ActionResult EditMedia(string Helpmedia_ID)
          {
              ViewBag.Count = 4;
              Session["CurrentUrl3"] = Request.RawUrl;
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              ViewBag.Helpmedia_ID = Helpmedia_ID;
              try
              {
 MediaInfo ObjNewMediaInfo = new MediaInfo();
                  ObjNewMediaInfo.Helpmedia_ID = Helpmedia_ID;
                  List<MediaInfo> objcollection = default(List<MediaInfo>);

                  objcollection = MediaInfo.GetNewMediaInfo(ObjNewMediaInfo);

                  if (objcollection.Count > 0)
                  {
                      MediaInfo ObjMediaInfo = new MediaInfo();
                      ObjMediaInfo = objcollection[0];
                      if ((ObjMediaInfo.Title != null))
                      {
                         ViewBag.txtTitle = ObjMediaInfo.Title;
                      }
                      if ((ObjMediaInfo.Duration != null))
                      {
                         
                          if (ObjMediaInfo.Duration != null)
                          {
                              string[] dura = null;
                              string duration = Convert.ToString(ObjMediaInfo.Duration);
                              dura = duration.Split(':');
                              if (dura.Length > 1)
                              {
                                 ViewBag.txtHours = dura[0];
                                 ViewBag.txtmins = dura[1];
                              }
                              else
                              {
                                  ViewBag.txtHours= dura[0];
                              }
                          }
                          else
                          {
                              ViewBag.txtHours = null;
                              ViewBag.txtmins = null;
                          }

                      }
                      if (ObjMediaInfo.IsDisplayToPractice == "Y")
                      {
                          ViewBag.chkactive = "true";
                          ViewBag.chkinactive= "false";
                      }
                      else
                      {
                          ViewBag.chkinactive = "true";
                          ViewBag.chkactive= "false";
                      }
                      if ((ObjMediaInfo.Description != null))
                      {
                          ViewBag.txtDescription = ObjMediaInfo.Description;
                      }
                      if ((ObjMediaInfo.ImagepathSuffix != null))
                      {
                         ViewBag.Imagepath_Suffix = ObjMediaInfo.ImagepathSuffix;
                      }
                      if (ObjMediaInfo.FilePath_Encrypted != null)
                      {
                          encCustomizeFileName = ObjMediaInfo.FilePath_Encrypted;
                      }
                      if (ObjMediaInfo.FileType != null)
                      {
                          HomePageFileName = ObjMediaInfo.FileType;
                          string[] filetype1 = HomePageFileName.Split('.');
                          string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\Media" + "\\";
                          CustomizeFileName = strFilePath + filetype1[0] + "." + filetype1[1];
                      }
                      if ((ObjMediaInfo.FileType != null))
                      {
                        ViewBag.lblName= ObjMediaInfo.FileType;
                      }
                  }

              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminFaqsController", "EditMedia", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }



              return View();
          }
          //private string RandomPassword()
          //{
          //    return Convert.ToString(Guid.NewGuid());                 
          //}
          [HttpPost()]
          public ActionResult EditMedia()
          {
            //  if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                  string hrs = null;
                  string out_Msg = "";
                  string FileExtn2 = null;
                  MediaInfo ObjUpdMediaInfo = new MediaInfo();
                  string Helpmedia_ID = null;
                  if (Request["hdnHelpmedia_ID"] != null)
                  {
                      Helpmedia_ID = Request["hdnHelpmedia_ID"].ToString();
                  }
                  else
                  {
                      Helpmedia_ID = Request["hdnHelpmedia_ID"].ToString();
                  }

                  double getlength = 0;
                  string strtitle = null;
                  string strdescription = null;
                  int i = Request.Files["FileUpLoad"].ContentLength;
                  if (i != 0)
                  {
                      getlength = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(i / 1024))));

                  }
                  //clsWebConfigsettings cs = new clsWebConfigsettings();
                  //string hdnMaxSize = cs.GetConfigSettingsValue("VideoMaxSize");
                  if (getlength > Convert.ToDouble(10 * 1024))
                  {
                      string msg = "Select 10 MB video file only ";
                      return RedirectToAction("EditMedia", new { Helpmedia_ID = Helpmedia_ID, lbloutmsg = msg });

                  }

                  else
                  {
                      try
                      {
                          if (Request.Files["FileUpLoad"].ContentLength > 0)
                          {
                              ObjUpdMediaInfo.Helpmedia_ID = Helpmedia_ID;
                              ObjUpdMediaInfo.Title = Convert.ToString(Request["txtTitle"]).Trim();
                              strtitle = Convert.ToString(Request["txtTitle"]).Trim();
                              if (Request["txtHours"] != null)
                              {
                                  hrs = Request["txtHours"].ToString();
                              }
                              else
                              {
                                  hrs = "00";
                              }
                              string mins = null;
                              if (Request["txtmins"] != null)
                              {
                                  mins = Request["txtmins"].ToString();
                              }
                              else
                              {
                                  mins = "00";
                              }
                              string duration = hrs + ":" + mins;
                              HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["FileUpLoad"].FileName);
                              FileExtn2 = System.IO.Path.GetExtension(Request.Files["FileUpLoad"].FileName);
                              string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\Media" + "\\";
                              string hdn_LogoPaths = strFilePath + HomePageFileName;
                              encCustomizeFileName = null;
                              hdn_LogoPaths = strFilePath + "/" + HomePageFileName + FileExtn2;
                              HomePageFileName = HomePageFileName + FileExtn2;
                              ObjUpdMediaInfo.Duration = (duration != null ? duration : null);
                              ObjUpdMediaInfo.Description = Request["txtDescription"];
                              strdescription = Request["txtDescription"];
                              ObjUpdMediaInfo.FileType = (HomePageFileName != null ? HomePageFileName : null);
                              ObjUpdMediaInfo.FilePath = (!string.IsNullOrEmpty(hdn_LogoPaths) ? hdn_LogoPaths : null);
                              ObjUpdMediaInfo.FilePath_Encrypted = (!string.IsNullOrEmpty(encCustomizeFileName) ? encCustomizeFileName : null);
                              if (Request["chkactive"] == "Yes")
                              {
                                  ObjUpdMediaInfo.IsDisplayToPractice = "Y";
                              }
                              else
                              {
                                  ObjUpdMediaInfo.IsDisplayToPractice = "N";
                              }

                              out_Msg = MediaInfo.UpdateMediaInfo(ObjUpdMediaInfo, ref out_Msg);

                              if (out_Msg != null && out_Msg != "")
                              {
                                  return RedirectToAction("EditMedia", new { lbloutmsg = out_Msg, Helpmedia_ID = Helpmedia_ID });


                              }
                              else
                              {
                                  Request.Files["FileUpLoad"].SaveAs(hdn_LogoPaths);
                              }
                          }
                          else if (Request["hdnlblname"] != null)
                          {
                              ObjUpdMediaInfo.Helpmedia_ID = Helpmedia_ID;
                              ObjUpdMediaInfo.Title = Convert.ToString(Request["txtTitle"]).Trim();
                              strtitle = Convert.ToString(Request["txtTitle"]).Trim();
                             if (!string.IsNullOrEmpty(Convert.ToString(Request["txtHours"])))// != null && Request["txtHours"] != "")
                              {
                                  hrs = Request["txtHours"].ToString();
                              }
                              else
                              {
                                  hrs = "00";
                              }
                              string mins = null;
                              if (!string.IsNullOrEmpty(Convert.ToString(Request["txtmins"])))// != null && Request["txtmins"] != "")
                              {
                                  mins = Request["txtmins"].ToString();
                              }
                              else
                              {
                                  mins = "00";
                              }
                              string duration = hrs + ":" + mins;
                              ObjUpdMediaInfo.Duration = (duration != null ? duration : null);
                              ObjUpdMediaInfo.Description = Request["txtDescription"];
                              strdescription = Request["txtDescription"];
                              ObjUpdMediaInfo.FileType = (HomePageFileName != null ? HomePageFileName : null);

                              ObjUpdMediaInfo.FilePath = (CustomizeFileName != null ? CustomizeFileName : null);
                              ObjUpdMediaInfo.FilePath_Encrypted = (!string.IsNullOrEmpty(encCustomizeFileName) ? encCustomizeFileName : null);
                              if (Request["chkactive"] == "Yes")
                              {
                                  ObjUpdMediaInfo.IsDisplayToPractice = "Y";
                              }
                              else
                              {
                                  ObjUpdMediaInfo.IsDisplayToPractice = "N";
                              }

                              out_Msg = MediaInfo.UpdateMediaInfo(ObjUpdMediaInfo, ref out_Msg);

                              if (out_Msg != null && out_Msg != "")
                              {
                                  return RedirectToAction("EditMedia", new { lbloutmsg = out_Msg, Helpmedia_ID = Helpmedia_ID });


                              }
                              else
                              {
                                  Request.Files["FileUpLoad"].SaveAs(CustomizeFileName);


                              }

                          }
                      }
                      catch (Exception ex)
                      {
                          var clsCustomEx = new clsExceptionLog();
                          clsCustomEx.LogException(ex, "AdminFaqsController", "EditMedia", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                      }
                  }



                  return RedirectToAction("TutorialsHome");
            
          }
          public ActionResult DeleteMedia(string HelpMedia_ID)
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              MediaInfo ObjDelMediaInfo = new MediaInfo();
              ObjDelMediaInfo.Helpmedia_ID = HelpMedia_ID;

              MediaInfo.DeleteMediaInfo(ObjDelMediaInfo);
              return RedirectToAction("TutorialsHome");
          }
          public ActionResult FAQuestionsList()
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              string startdate;
              startdate = DateTime.Now.ToString("MM/dd/yyyy");
              Publicfaq objques = new Publicfaq();
              if (!string.IsNullOrEmpty(Convert.ToString(Request["txtquestions"])))// != "" && Request["txtquestions"] != null)
              {
               
                  objques.Question = Request["txtquestions"].ToString();
              }
              else
              {

                  objques.Question = null;
              }
              if (Request["ddlstatus"]!="" && Request["ddlstatus"] !=null && Request["ddlstatus"] != "--Select one--")
              {
                  ViewBag.status = Request["ddlstatus"].ToString();
                  if (Request["ddlstatus"] == "Active")
                  {
                      objques.Status = "Active";
                  }
                  else if (Request["ddlstatus"] == "Inactive")
                  {
                      objques.Status = "In Active";
                  }
              }
              else
              {

                  objques.Status = null;
              }
              if (!string.IsNullOrEmpty(Convert.ToString(Request["ddlcategorys"])))// != "" && Request["ddlcategorys"] != null)
              {
                  objques.Category_Id = Request["ddlcategorys"].ToString();
              }
              else
              {
                  objques.Category_Id = null;
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
              objques.CreatedOn = null;
              objques.UpdatedOn = null;
              objques.OrderBy = "ASC";
              objques.OrderByItem = "Question";
              if (!string.IsNullOrEmpty(Convert.ToString(Request["page"])))// != "" && Request["page"] != null)
              {
                  objques.PageNo = Request["page"].ToString();
              }
              else
              {
                  objques.PageNo = "1";
              }
           
              objques.NoofRecords = "10";
              objques.strfromdate = FromDate;
              objques.strtodate = ToDate;

           var   objlist = Publicfaq.GetQuestions(objques);
           if (objlist.Count > 0)
           {
               ViewBag.totalrec = Publicfaq.TotalRecords;
           }
           return View("FAQuestionsList", objlist);
          }
          public ActionResult AdminFaQuestionsList()
          {
              ViewBag.Count = 3;
              Session["CurrentUrl2"] = Request.RawUrl;
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              if (Request["txtquestions"] != "" && Request["txtquestions"] != null)
              {
                  ViewBag.question123 = Request["txtquestions"];
              }
              else
              {
                  ViewBag.question123 = null;
              }
              if (Request["ddlstatus"] != "" && Request["ddlstatus"] != null && Request["ddlstatus"] != "--Select one--")
              {
                  ViewBag.status = Request["ddlstatus"];
              }
              else
              {
                  ViewBag.status = null;
              }
              if (Request["ddlcategorys"] != "" && Request["ddlcategorys"] != null)
              {
                  ViewBag.category = Request["ddlcategorys"];
              }
              else
              {
                  ViewBag.category = null;
              }
              if (Request["txt_FromDate"] != "" && Request["txt_FromDate"] != null)
              {
                  ViewBag.frodate = Request["txt_FromDate"];
                  ViewBag.todate = Request["txt_ToDate"];
              }
              else
              {
                  ViewBag.frodate = null;
                  ViewBag.todate = null;
              }
              if (Request["dt_filter"] != null && Request["dt_filter"] != "")
              {
                  ViewBag.date = Request["dt_filter"];
              }
              else
              {
                  ViewBag.date = "7";
              }
              List<Publicfaq> dataques = new List<Publicfaq>();
              dataques = Publicfaq.GetQuesCategories();
              IList<SelectListItem> _result3 = new List<SelectListItem>();
              if (dataques.Count > 0)
              {
                  for (int i = 0; i <= dataques.Count - 1; i++)
                  {
                      _result3.Add(new SelectListItem
                      {
                          Text = dataques[i].Category_Name,
                          Value = Convert.ToString(dataques[i].Category_Id)
                      });
                  }
              }
              else
              {
                  _result3.Add(new SelectListItem
                  {
                      Text = "--Select Category--",
                      Value = "0",
                      Selected = true
                  });
              }
              StateCity reg9 = new StateCity();
              reg9 = new StateCity
              {
                  CategoryList = _result3,

              };
              return View("AdminFaQuestionsList",reg9);
          }
          public ActionResult AddNewFaq()
          {
              ViewBag.Count = 4;
              Session["CurrentUrl3"] = Request.RawUrl;
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              Bindcatgory();
              return View();
          }
          [HttpPost()]
          public ActionResult AddNewFaq(string obj)
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              List<Publicfaq> objqaList = new List<Publicfaq>();
              Publicfaq objqa = new Publicfaq();
              objqa.From_Roleid = Convert.ToInt32(Session["roleid"]);
              objqa.Question = Convert.ToString(Request["txtQuestion"]);
              objqa.Answer = Convert.ToString(Request["txtAnswer"]) ;
              objqa.Category_Id = "," + Request["ComboCategorylist"] + ",";
              objqa.Description = null;
              objqa.Keyword = Request["txtKeywords"];

              if (Request["rbtnStatus1"] == "Yes")
              {
                  objqa.Status = "Active";
              }
              else
              {
                  objqa.Status = "Inactive";
              }
              if (Request["hdnchecklist1"] == "True")
              {
                  objqa.public_ind = "Y";
              }
              else
              {
                  objqa.public_ind = "N";
              }

              if (Request["hdnchecklist2"] == "True")
              {
                  objqa.Provider_Ind = "Y";
              }
              else
              {
                  objqa.Provider_Ind = "N";
              }
              if (Request["hdnchecklist4"] == "True")
              {
                  objqa.Verfication_User_ind = "Y";
              }
              else
              {
                  objqa.Verfication_User_ind = "N";
              }
              objqa.CreatedBy = Convert.ToString(Session["UserID"]);
              objqa.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);

             int qid = Publicfaq.insertAdminQuestions(objqa);
             ViewBag.qid = qid;
             InsertMediaLinks(qid);
             InsertRelatedFaqs(qid);

             return RedirectToAction("AdminFaQuestionsList");

          }


          public void InsertMediaLinks(int qid)
          {
              try
              {
              string[] medi3 = null;
              int intcount = 0;
              if (Request["hndmediaid"] !=""&& Request["hndmediaid"] != null)
              {
                  medi3 = Convert.ToString(Request["hndmediaid"]).Split(',');
              }
              if (Request["hndmediaid"] != "" && Request["hndmediaid"] != null)
              {
                  for (intcount = 0; intcount <= medi3.Count() - 1; intcount++)
                  {
                      if (medi3[intcount] != "" && medi3[intcount] != null)
                      {
                          Publicfaq objurl = new Publicfaq();
                          if (qid != 0)
                          {
                              objurl.Question_id = qid.ToString();
                          }
                          objurl.strVideoid = Convert.ToInt32(medi3[intcount]);
                          string title = "hdn" +Convert.ToString( medi3[intcount]);
                          objurl.LinkName = Convert.ToString(Request[title]);
                          objurl.CreatedBy = Convert.ToString(Session["UserID"]);
                          objurl.Status_Ind = "A";

                          Publicfaq.AddRelatedLink(objurl);
                      }
                  }
              }
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "InsertMediaLinks", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }
          }

          public void InsertRelatedFaqs(int qid)
          {
              try{
              string[] faq3 = null;
              int intcount = 0;
              if (Request["hdnfaqid"] != "" && Request["hdnfaqid"]!=null)
              {
                  faq3 = Convert.ToString(Request["hdnfaqid"]).Split(',');
              }
              if (Request["hdnfaqid"] != "" && Request["hdnfaqid"] != null)
              {
                  for (intcount = 0; intcount <= faq3.Count() - 1; intcount++)
                  {
                      Publicfaq objfaq = new Publicfaq();
                      if (qid != 0)
                      {
                          objfaq.Question_id = qid.ToString();
                      }
                      string title = "hdnfaq" + Convert.ToString(faq3[intcount]);
                      if (!string.IsNullOrEmpty(Convert.ToString(Request[title])))
                      {
                          objfaq.Question = Request[title].ToString();
                      }
                      else
                      {
                          objfaq.Question = null;
                      }
                      if (!string.IsNullOrEmpty(faq3[intcount]))
                      {
                          objfaq.Relatedfaq_Id = Convert.ToInt32(faq3[intcount]);
                      }
                      else
                      {
                          objfaq.Relatedfaq_Id = null;
                      }
                      objfaq.CreatedBy = Convert.ToString(Session["UserID"]);
                      objfaq.Status_Ind = "A";

                      if (faq3[intcount] != "" && faq3[intcount] != null)
                      {
                          Publicfaq.DML_Relatedfaqs(objfaq);

                      }
                  }
              }
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "InsertRelatedFaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }
          }
     
   [HttpGet()]
   [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
          public ActionResult Videolist(string media, string delmeid)
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              MediaInfo ObjMediainfo = new MediaInfo();
              List<MediaInfo> objcollection = default(List<MediaInfo>);
              ObjMediainfo.NoOfrecords = 10;
              if (Request["page"] != "" && Request["page"] != null)
              {
                  ObjMediainfo.PageNO = Convert.ToInt32(Request["page"]);
              }
              else
              {
                  ObjMediainfo.PageNO = 1;
              }
             
              if (!string.IsNullOrEmpty(Convert.ToString(Request["txtTitle1"])))// != null && Request["txtTitle1"] != "")
              {
                  ObjMediainfo.MediaTitle = Request["txtTitle1"].ToString();
              }
              else
              {
                  ObjMediainfo.MediaTitle = null;
              }
              ObjMediainfo.OrderBy = "DESC";
              ObjMediainfo.OrderByItem = "Title";
              if (media != null & media != "")
              {
                  if (media.EndsWith(","))
                  {
                      ObjMediainfo.strexplictids = media.Trim();
                  }
                  else{
                  ObjMediainfo.strexplictids = media.Trim() + ",";
                  }
              }
              else
              {
                  ObjMediainfo.strexplictids = null;
              }
                 objcollection = MediaInfo.GetMediaInfo1(ObjMediainfo);

                 ViewBag.totalrec = MediaInfo.TotalRecords;
                 ViewBag.medialist = media;
                 return View("Videolist", objcollection);
          }
   public PartialViewResult SubmitVideolist(string media, string delmeid)
   {
       //if (Session["UserID"] == null)
       //{
       //    return RedirectToAction("SessionExpire", "Home");
       //}
       if (Request["hdnmedia"] != null && Request["hdnmedia"] != "")
       {
           media = Request["hdnmedia"];
       }
       if (!string.IsNullOrEmpty(Convert.ToString(Request["hdndelmeid"])))// != null && Request["hdndelmeid"] != "")
       {
           delmeid = Request["hdndelmeid"].ToString();
       }
       MediaInfo ObjMediainfo = new MediaInfo();
       List<MediaInfo> objcollection = default(List<MediaInfo>);
       ObjMediainfo.NoOfrecords = 10;
       if (!string.IsNullOrEmpty(Convert.ToString(Request["page"])))// != "" && Request["page"] != null)
       {
           ObjMediainfo.PageNO = Convert.ToInt32(Request["page"]);
       }
       else
       {
           ObjMediainfo.PageNO = 1;
       }

       if (!string.IsNullOrEmpty(Request["txtTitle1"]))//!= null && Request["txtTitle1"] != "")
       {
           ViewBag.title = Request["txtTitle1"].ToString(); ;
           ObjMediainfo.MediaTitle = Request["txtTitle1"].ToString();
       }
       else
       {
           ViewBag.title = null;
           ObjMediainfo.MediaTitle = null;
       }
       ObjMediainfo.OrderBy = "DESC";
       ObjMediainfo.OrderByItem = "Title";
       if (media != null & media != "")
       {
           if (media.EndsWith(","))
           {
               ObjMediainfo.strexplictids = media.Trim();
           }
           else
           {
               ObjMediainfo.strexplictids = media.Trim() + ",";
           }
       }
       else
       {
           ObjMediainfo.strexplictids = null;
       }
       objcollection = MediaInfo.GetMediaInfo1(ObjMediainfo);

       ViewBag.totalrec = MediaInfo.TotalRecords;
       ViewBag.medialist = media;
       return PartialView("_SubmitVideolist", objcollection);
   }
          public void Bindcatgory()
          {
              List<Publicfaq> dataques = new List<Publicfaq>();
              try{

              dataques = Publicfaq.GetQuesCategories();
              ComboBoxItemList objlistCategory = new ComboBoxItemList(dataques, "Category_Id", "Category_Name");
              ViewData["ComboCategorylist"] = objlistCategory;
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "Bindcatgory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }
          }
        [HttpGet()]
          public ActionResult AddNewCategory()
          {
              //if (Session["UserID"] == null)
              //{
              //    return RedirectToAction("SessionExpire", "Home");
              //}
              
                  return View();
              
          }
        [HttpPost()]
          public JsonResult AddNewCategory(string obj)
        {
            
              IDataParameter[] InParam = { new SqlParameter("@in_Category_Name", (!string.IsNullOrEmpty(Request["txtCategoryName"]) ? Request["txtCategoryName"].ToString() : null)) };
              IDataParameter[] OutParam = { new SqlParameter("@Out_msg", SqlDbType.VarChar, 100),
                                          new SqlParameter("@out_category_id", SqlDbType.Int, 0)};
              clsCommonFunctions objcommon = new clsCommonFunctions();
              objcommon.AddInParameters(InParam);
              objcommon.AddOutParameters(OutParam);
              string strout = null;
              string catid = null;
              objcommon.ExecuteNonQuerySP("Help_dbo.st_LOOKUP_INS_Um_FAQ_Categories");
              if (objcommon.objdbCommandWrapper.Parameters["@Out_msg"].Value!=DBNull.Value)
              {
                  strout = objcommon.objdbCommandWrapper.Parameters["@Out_msg"].Value.ToString();
                  return Json(JsonResponseFactory.ErrorResponse(strout), JsonRequestBehavior.DenyGet);
              }
              else
              {
                  strout = null;
              }

              if (objcommon.objdbCommandWrapper.Parameters["@out_category_id"].Value != DBNull.Value)
              {
                  catid = objcommon.objdbCommandWrapper.Parameters["@out_category_id"].Value.ToString();
                  return Json(JsonResponseFactory.SuccessResponse(catid), JsonRequestBehavior.DenyGet);
              }
              else
              {
                  strout = null;
              }
     
           
                  return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
             
          }
        public ActionResult Faqslist(string checklist1, string checklist2, string checklist4, string faq, string delfaqid, string quesid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.checklist1 = checklist1;
            ViewBag.checklist2 = checklist2;
            ViewBag.checklist4 = checklist4;
            ViewBag.faq = faq;
            ViewBag.delfaqid = delfaqid;
            var objques = new Publicfaq();


            if (checklist1 != "False" || checklist2 != "False" || checklist4 != "False")
            {
                if (checklist1 == "True")
                {
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.public_ind = "N";
                }
                if (checklist2 == "True")
                {
                    objques.Provider_Ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Provider_Ind = "N";
                }
                if (checklist4 == "True")
                {
                    objques.Verfication_User_ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Verfication_User_ind = "N";
                }

            }
            else
            {
                objques.public_ind = null;
                objques.Provider_Ind = null;
            }
            if (faq != null & faq != "")
            {
                if (faq.EndsWith(","))
                {
                    objques.strExcept = faq.Trim();
                }
                else
                {
                    objques.strExcept = faq.Trim() + ",";
                }
            }
            else
            {
                objques.strExcept = null;
            }

            objques.Question_id = quesid;
            objques.CreatedOn = null;
            objques.UpdatedOn = null;
            objques.Status = "Active";
            objques.PageNo = "1";
            objques.NoofRecords = "10";
            var objlist = new List<Publicfaq>();
            objlist = Publicfaq.GetRelatedQuestions(objques);
            ViewBag.totalrec = Publicfaq.TotalRecords;
            return View("Faqslist", objlist);
        }

        public ActionResult Bindfaqs(string check1, string check2, string check3, string check4, string faq, string delfaqid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objques = new Publicfaq();


            if (check1 != "False" || check2 != "False" || check3 != "False" || check4 != "False")
            {
                if (check1 == "True")
                {
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.public_ind = "N";
                }
                if (check2 == "True")
                {
                    objques.Provider_Ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Provider_Ind = "N";
                }
                if (check3 == "True")
                {
                    objques.public_ind = "Y";
                }
                if (check4 == "True")
                {
                    objques.Verfication_User_ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Verfication_User_ind = "N";
                }

            }
            else
            {
                objques.public_ind = null;
                objques.Provider_Ind = null;
            }
            if (faq != null & faq != "")
            {
                if (faq.EndsWith(","))
                {
                    objques.strExcept = faq.Trim();
                }
                else
                {
                    objques.strExcept = faq.Trim() + ",";
                }
            }
            else
            {
                objques.strExcept = null;
            }
              

            objques.CreatedOn = null;
            objques.UpdatedOn = null;
            objques.PageNo =  "1";
            objques.NoofRecords = "10";
            var objlist = new List<Publicfaq>();
            objlist = Publicfaq.GetRelatedQuestions(objques);
            ViewBag.totalrec = Publicfaq.TotalRecords;

            return View("Bindfaqs", objlist);
        }
        public PartialViewResult SubmitFaqs(string check1, string check2, string check3, string check4, string faq, string delfaqid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["hdnchecklist1"] != null && Request["hdnchecklist1"]!="")
            {
                check1 = Request["hdnchecklist1"];
            }
            if (Request["hdnchecklist2"] != null && Request["hdnchecklist2"] != "")
            {
                check2 = Request["hdnchecklist2"];
            }
            if (Request["hdnchecklist3"] != null && Request["hdnchecklist3"] != "")
            {
                check3 = Request["hdnchecklist3"];
            }
            if (Request["hdnchecklist4"] != null && Request["hdnchecklist4"] != "")
            {
                check4 = Request["hdnchecklist4"];
            }
            if (Request["hdnfaq"] != null && Request["hdnfaq"] != "")
            {
                faq = Request["hdnfaq"];
            }
            if (Request["hdndelfaqid"] != null && Request["hdndelfaqid"] != "")
            {
                delfaqid = Request["hdndelfaqid"];
            }
            var objques = new Publicfaq();

            if (Request["txtrelfaq"] != null && Request["txtrelfaq"] != "")
            {
                objques.Question = Request["txtrelfaq"];
                ViewBag.question = Request["txtrelfaq"];
            }
            else
            {
                objques.Question = null;
                ViewBag.question = null;
            }
            if (check1 != "False" || check2 != "False" || check3 != "False" || check4 != "False")
            {
                if (check1 == "True")
                {
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.public_ind = "N";
                }
                if (check2 == "True")
                {
                    objques.Provider_Ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Provider_Ind = "N";
                }
                if (check3 == "True")
                {
                    objques.public_ind = "Y";
                }
                if (check4 == "True")
                {
                    objques.Verfication_User_ind = "Y";
                    objques.public_ind = "Y";
                }
                else
                {
                    objques.Verfication_User_ind = "N";
                }

            }
            else
            {
                objques.public_ind = null;
                objques.Provider_Ind = null;
            }
            if (faq != null & faq != "")
            {
                if (faq.EndsWith(","))
                {
                    objques.strExcept = faq.Trim();
                }
                else
                {
                    objques.strExcept = faq.Trim() + ",";
                }
            }
            else
            {
                objques.strExcept = null;
            }

            objques.Status = "Active";
            objques.CreatedOn = null;
            objques.UpdatedOn = null;
            objques.PageNo = "1";
            objques.NoofRecords = "10";
            var objlist = new List<Publicfaq>();
            objlist = Publicfaq.GetRelatedQuestions(objques);
            ViewBag.totalrec = Publicfaq.TotalRecords;

            return PartialView("_SubmitFaqs", objlist);
        }
        public ActionResult EditFaq(string questionid)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.qid = questionid;
            Bindcatgory();
            GetMedialinks(questionid);
            GetRelatedfaqs(questionid);
            var ds = new DataSet();
            ds = Publicfaq.EditQuestion(questionid);
            if (ds.Tables[0].Rows[0]["Question"] != DBNull.Value)
            {

                ViewBag.question = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Question"])) ? ds.Tables[0].Rows[0]["Question"].ToString() : null;
            }
            else
            {
                ViewBag.question = null;
            }
            if (ds.Tables[0].Rows[0]["Answertext"] != DBNull.Value)
            {

                ViewBag.answer = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Answertext"])) ? ds.Tables[0].Rows[0]["Answertext"].ToString() : null;
            }
            else
            {
                ViewBag.answer = null;
            }
            if (ds.Tables[0].Rows[0]["Category_id"] != DBNull.Value)
            {

                ViewBag.Categorylist = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Category_id"])) ? ds.Tables[0].Rows[0]["Category_id"].ToString() : null;
            }
            else
            {
                ViewBag.Categorylist = null;
            }
            if (ds.Tables[0].Rows[0]["Tages"] != DBNull.Value)
            {

                ViewBag.Tages = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Tages"])) ? ds.Tables[0].Rows[0]["Tages"].ToString() : null;
            }
            else
            {
                ViewBag.Tages = null;
            }
            if (ds.Tables[0].Rows[0]["public_ind"] != null)
            {

                ViewBag.public_ind = ds.Tables[0].Rows[0]["public_ind"].ToString();
            }
            else
            {
                ViewBag.public_ind = null;
            }
            if (ds.Tables[0].Rows[0]["provider_Ind"] != null)
            {

                ViewBag.provider_Ind = ds.Tables[0].Rows[0]["provider_Ind"].ToString();
            }
            else
            {
                ViewBag.provider_Ind = null;
            }
            if (ds.Tables[0].Rows[0]["verificationuser_ind"] != null)
            {

                ViewBag.verificationuser_ind = ds.Tables[0].Rows[0]["verificationuser_ind"].ToString();
            }
            else
            {
                ViewBag.verificationuser_ind = null;
            }
            if (ds.Tables[0].Rows[0]["Status"] != null)
            {

                ViewBag.Status = ds.Tables[0].Rows[0]["Status"].ToString();
            }
            else
            {
                ViewBag.Status = null;
            }

            return View();
        }

        [HttpPost()]
        public ActionResult EditFaq()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            List<Publicfaq> objqaList = new List<Publicfaq>();
            Publicfaq objqa = new Publicfaq();
            objqa.Question_id = Convert.ToString(Request["hdnqid"]);
            string qid = Convert.ToString(Request["hdnqid"]);
            objqa.From_Roleid = Convert.ToInt32(Session["roleid"]);
            objqa.Question = Convert.ToString(Request["txtQuestion"]);
            objqa.Answer = Convert.ToString(Request["txtAnswer"]);
            objqa.Category_Id = Request["ComboCategorylist"];
            objqa.Description = null;
            objqa.Keyword = Request["txtKeywords"];

            if (Request["rbtnStatus1"] == "Yes")
            {
                objqa.Status = "Active";
            }
            else
            {
                objqa.Status = "Inactive";
            }
            if (Request["hdnchecklist1"] == "True")
            {
                objqa.public_ind = "Y";
            }
            else
            {
                objqa.public_ind = "N";
            }

            if (Request["hdnchecklist2"] == "True")
            {
                objqa.Provider_Ind = "Y";
            }
            else
            {
                objqa.Provider_Ind = "N";
            }
            if (Request["hdnchecklist4"] == "True")
            {
                objqa.Verfication_User_ind = "Y";
            }
            else
            {
                objqa.Verfication_User_ind = "N";
            }

            objqa.CreatedBy = Convert.ToString(Session["UserID"]);
            objqa.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);
            Publicfaq.UpdateQuestion(objqa);
            InsertRelMediaLinks(qid);
            InsertRelatedFaqs(qid);
            return RedirectToAction("AdminFaQuestionsList");
        }

        public void InsertRelMediaLinks(string qid)
        {
            try{
            Publicfaq objurl = new Publicfaq();
            string editmedia = Convert.ToString(Request["hdneditmid"]);
            string addmedia = Request["hndmediaid"];
            string[] editmedia2 = null;
            if (editmedia != null && addmedia != null)
            {
                editmedia2 = editmedia.Split(',');
                for (int i = 0; i < editmedia2.Count(); i++)
                {
                    if (!addmedia.EndsWith(","))
                    {
                        addmedia = addmedia + ",";
                    }
                    if (editmedia2[i] != "" && editmedia2[i] != null)
                    {
                        if (addmedia.Contains(editmedia2[i]))
                        {
                            string media123 = Convert.ToString(editmedia2[i]) + ",";
                            addmedia = addmedia.Replace(media123, null);
                        }
                        else
                        {
                        }
                    }
                }
            }
            string[] addmei1 = null;
            if (addmedia !="" && addmedia != null)
            {
                addmei1 = addmedia.Split(',');

            }
            if (addmei1 != null)
            {
                for (int i = 0; i < addmei1.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(addmei1[i])))// != "" && addmei1[i] != null)
                    {
                        objurl.Question_id = Convert.ToString(qid);
                        string hdnlink = "hdn" + Convert.ToString(addmei1[i]);
                        objurl.LinkName = Convert.ToString(Request[hdnlink]);
                        objurl.strVideoid = Convert.ToInt32(addmei1[i]);
                        objurl.CreatedBy =Convert.ToString( Session["UserID"]);
                        objurl.Status_Ind = "A";
                        Publicfaq.AddRelatedLink(objurl);
                    }
                }
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "InsertRelMediaLinks", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void InsertRelatedFaqs(string qid)
        {
            try
            {
            Publicfaq objfaq = new Publicfaq();
            string editfaq = Convert.ToString(Request["hdneditfaqid"]);
            string addfaq = Request["hdnfaqid"];
            string[] editfaq2 = null;
            if (editfaq != null && addfaq != null)
            {
                editfaq2 = editfaq.Split(',');
                for (int i = 0; i < editfaq2.Count(); i++)
                {
                    if (!addfaq.EndsWith(","))
                    {
                        addfaq = addfaq + ",";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(editfaq2[i])))// != "" && editfaq2[i] != null)
                    {
                        if (addfaq.Contains(editfaq2[i]))
                        {
                            string media123 = editfaq2[i].ToString() + ",";
                            addfaq = addfaq.Replace(media123, null);
                        }
                        else
                        {
                        }
                    }
                }
            }


            string[] addfaq1 = null;
            if (addfaq != "" && addfaq != null)
            {
                addfaq1 = addfaq.Split(',');

            }
            if (addfaq1 != null)
            {
                for (int i = 0; i < addfaq1.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(addfaq1[i])))// != "" && addfaq1[i] != null)
                    {
                        objfaq.Question_id = Convert.ToString(qid);
                        string hdnlink = "hdnfaq" + Convert.ToString(addfaq1[i]);
                        objfaq.Question = Convert.ToString(Request[hdnlink]);
                        objfaq.Relatedfaq_Id = Convert.ToInt32(addfaq1[i]);
                        objfaq.CreatedBy = Convert.ToString(Session["UserID"]);
                        objfaq.Status_Ind = "A";
                        Publicfaq.DML_Relatedfaqs(objfaq);
                    }
                }
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "InsertRelatedFaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
       

        public void GetMedialinks(string questionid)
        {
            try
            {
            var returnset = Publicfaq.GetRelatedLink(questionid);
            ViewBag.relatedlinks = null;
            ViewBag.relatedlinks = returnset.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = 0;
            ViewBag.relatedlinkcount = returnset.Tables[0].Rows.Count;
            ViewBag.mediaid = null;
            if (returnset.Tables.Count > 0)
            {
                if (returnset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow objDR in returnset.Tables[0].Rows)
                    {
                        if (ViewBag.mediaid == null)
                        {
                            ViewBag.mediaid = Convert.ToString(objDR["Video_id"]);
                        }
                        else
                        {
                            ViewBag.mediaid = ViewBag.mediaid + "," + Convert.ToString(objDR["Video_id"]);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "GetMedialinks", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public void GetRelatedfaqs(string questionid)
        {

            try
            {
            Publicfaq objlist = new Publicfaq();
            objlist.Question_id = questionid;

           var dsqfaq = Publicfaq.Get_Relatedfaqs(objlist);
           ViewBag.relatedfaqs = null;
           ViewBag.relatedfaqs = dsqfaq.Tables[0].AsEnumerable();
           ViewBag.relatedfaqcount = 0;
           ViewBag.relatedfaqcount = dsqfaq.Tables[0].Rows.Count;
           ViewBag.Faqid = null;
           if (dsqfaq.Tables.Count > 0)
           {
               if (dsqfaq.Tables[0].Rows.Count > 0)
               {
                   foreach (DataRow objDR in dsqfaq.Tables[0].Rows)
                   {
                       if (ViewBag.Faqid == null)
                       {
                           ViewBag.Faqid = Convert.ToString(objDR["ChildFaq_id"]);
                       }
                       else
                       {
                           ViewBag.Faqid = ViewBag.Faqid + "," + Convert.ToString(objDR["ChildFaq_id"]);
                       }
                   }
               }
           }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "GetRelatedfaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult DeleteFaq(string questionid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq.DeleteQuestion(questionid, Convert.ToInt32(Session["Loginhistory_ID"]));
            return RedirectToAction("AdminFaQuestionsList");
        }

        public ActionResult ResetFaq(string questionid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq obj = new Publicfaq();
            obj.Question_id = questionid;

            Publicfaq.Resetratings(obj);
            return RedirectToAction("AdminFaQuestionsList");
        }
        public ActionResult viewcomments(string questionid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq obj = new Publicfaq();
            obj.Question_id = questionid;
           

            var comments = Publicfaq.Get_FaqsComments(obj);
            ViewBag.totalnorec = comments.Count;

            return View("viewcomments", comments);
        }

        public JsonResult Getquestion(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Keyword", term) };

            objcommon.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcommon.GetDataReader("Help_dbo.St_Admin_Typeahead_FAQuestionsList");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString(drlist[0]));
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
