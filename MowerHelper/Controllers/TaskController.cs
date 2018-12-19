using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Tasks;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class TaskController : Controller
    {
        string totalrecords = "0";
        string chkStaus;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult TaskList()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInElectricianLogin").ToUpper() == "YES")
            {
                ViewBag.Showforums = "Y";
            }
            else
            {
                ViewBag.Showforums = "N";
            }        
             string txttitle;
            if (Request["txtTitle"] != null & Request["txtTitle"] != "")
            {
                ViewBag.Title = Request["txtTitle"];
                txttitle = Request["txtTitle"];
            }
            else
            {
                ViewBag.Title = null;
                txttitle = null;
            }

            ViewBag.Status = (Request["ddlStatus"]==null ||Request["ddlStatus"]=="" ? "All" : Request["ddlStatus"]);
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            ViewBag.cat = Request["txtCategory"];

            string FromDate = null;
            string ToDate = null;
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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

            Tasks objTask = new Tasks();
            var _with1 = objTask;

            _with1.RoleInd = "N";
            if (Convert.ToString( Session["RoleID"]) == "1")
            {
                _with1.Reference_ID = 1;
                _with1.ReferenceType_ID = 1;
            }
            else
            {
                _with1.Reference_ID = null;
                _with1.ReferenceType_ID =null;
            }

            _with1.sortField = "TaskTitle";
            _with1.sortDirection = "ASC";
            _with1.TaskTitle = txttitle;
            _with1.Notes = txttitle; ;
            _with1.Actionitemtitle = txttitle; ;
            if (Request["ddlStatus"] == "All" || Request["ddlStatus"] == null || Request["ddlStatus"] == "")
            {
                _with1.Status_Ind = null;
            }
            else
            {
                _with1.Status_Ind = Request["ddlStatus"];
            }
                _with1.CreatedBy = Convert.ToString(Session["userid"]);
            objTask.StartDate = FromDate;
            objTask.EndDate = ToDate;
            List<Tasks> Taskslist = null;
            Taskslist = Tasks.Tasklist(objTask);
            ViewBag.totrec = Tasks.Totalrecords;
            return View(Taskslist);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult TaskList(string name)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string txttitle;
            if (Convert.ToString( Request["txtTitle"]) != "")
            {
                ViewBag.Title = Request["txtTitle"];
                txttitle = Request["txtTitle"];
            }
            else
            {
                ViewBag.Title = null;
                txttitle = null;
            }
            
            ViewBag.Status = Request["ddlStatus"];
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? null : Request["dt_filter"]);
            ViewBag.cat = Request["txtCategory"];

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] =="1")
            {
                FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
                ToDate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            }
            else
            {
                string startdate;
                startdate = DateTime.Now.ToString("MM/dd/yyyy");

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

            Tasks objTask = new Tasks();
            var _with1 = objTask;

            _with1.RoleInd = "N";
            if (Convert.ToString( Session["RoleID"]) == "1")
            {
                _with1.Reference_ID = 1;
                _with1.ReferenceType_ID = 1;
            }
            else
            {
                _with1.Reference_ID = null;
                _with1.ReferenceType_ID = null;
            }

            _with1.sortField = "TaskTitle";
            _with1.sortDirection = "ASC";
            _with1.TaskTitle = txttitle;
            _with1.Notes = txttitle; ;
            _with1.Actionitemtitle = txttitle; ;
            if (Request["ddlStatus"] == "All")
            {
                _with1.Status_Ind = null;
            }
            else
            {
                _with1.Status_Ind = Request["ddlStatus"];
            }
                _with1.CreatedBy = Convert.ToString(Session["userid"]);
            objTask.StartDate = FromDate;
            objTask.EndDate = ToDate;
            List<Tasks> Taskslist = null;
            Taskslist = Tasks.Tasklist(objTask);
            ViewBag.totrec = Tasks.Totalrecords;
            return View(Taskslist);
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public List<Tasks> GetTaskList(string taskname)
        {
            Tasks objgettask = new Tasks();

            List<Tasks> tasklist = null;
            try{
            tasklist = Tasks.Tasklist(objgettask);
            if (tasklist.Count > 0)
            {
                totalrecords = Convert.ToString(Tasks.Totalrecords);
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "TaskController", "GetTaskList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return tasklist;
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult TaskTitle(string term)
        {
            //if (Session["UserID"] == null)
            //{
            //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
            //}
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@In_KeyWord", term), 
                                            new SqlParameter("@In_reference_id", Convert.ToString( Session["RoleID"])!="1"? Session["Prov_ID"]:"1") };
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.St_Typeahead_TaskTitle");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString( drlist[0]));
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Addnewtask()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            
           
                ViewBag.roleid = Convert.ToString( Session["roleid"]);
                return View();
          
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Addnewtask(string Name)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                string Status;

                if (Request["rdStatus_0"] == "Complete")
                {
                    Status = "Y";
                }
                else
                {
                    Status = "N";
                }

                if (Request["chkmsg"] != null)
                {
                    string[] WorkphoneLeaveMsg = null;
                    if (Request["chkmsg"].Contains(","))
                    {
                        WorkphoneLeaveMsg = Request["chkmsg"].Split(',');
                        if (WorkphoneLeaveMsg.Length == 2)
                        {
                            if (WorkphoneLeaveMsg[0] == "true")
                            {
                                chkStaus = "Y";
                            }
                            else if (WorkphoneLeaveMsg[0] == "false")
                            {
                                chkStaus = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkmsg"] == "false")
                        {
                            chkStaus = "N";
                        }
                        else if (Request["chkmsg"] == "true")
                        {
                            chkStaus = "Y";
                        }
                    }
                }
                Tasks objTask = new Tasks();
                var _with1 = objTask;
                if (Convert.ToString( Session["RoleID"]) == "1")
                {
                    _with1.Reference_ID = 1;
                    _with1.ReferenceType_ID = 1;
                }
                else
                {
                    _with1.Reference_ID = Convert.ToInt32(Session["Prov_ID"]);
                    _with1.ReferenceType_ID = Convert.ToInt32("2");
                }
                _with1.IsPracticeTask = "N";
                _with1.TaskTitle = Request["txt_title"];
                if (!string.IsNullOrEmpty(Request["txtDate"]))
                {
                    _with1.DueDate = Convert.ToString(Request["txtDate"]);
                }
                else
                {
                    _with1.DueDate = null;
                }
                _with1.Notes = Request["txtnotes"];
                _with1.IsDone = Status;
                _with1.ChkStatus_Ind = chkStaus;
                _with1.Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                Tasks.AddNewTask(objTask);
                return RedirectToAction("TaskList", "Task");
            
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Addaction(string taskid)
        {
           
                ViewBag.lblDate = System.DateTime.Now.ToShortDateString();
                ViewBag.TaskId = taskid;
                return View();
           
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Addaction(string taskid, string name1)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            int ReferencetypeID;
            int? ReferenceID = null;
            ReferenceID = Convert.ToInt32(Session["userid"]);
            if (Convert.ToString( Session["RoleID"]) == "1")
            {

                ReferencetypeID = 1;
            }
            else
            {
                ReferencetypeID = 13;
            }
            Tasks objTask = new Tasks();
            var _with1 = objTask;
            _with1.Task_ID = Convert.ToInt32(Request["hdnTaskId"]);
            _with1.Actionitem_ID = null;
            _with1.Actionitemtitle =Request["txtActionItem"];
            _with1.ActiontitleDescription = null;
            _with1.Reference_ID = ReferenceID;
            _with1.ReferenceType_ID = ReferencetypeID;
            Tasks.AddNewActionItem(objTask);
            return RedirectToAction("TaskList", "Task");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editaction(int Action_id)
        {
           
                ViewBag.lblDate = System.DateTime.Now.ToShortDateString();
                ViewBag.Action_id = Action_id;
                Tasks objTask = new Tasks();
                var _with1 = objTask;
                _with1.Actionitem_ID = Action_id;
                List<Tasks> Taskslist = null;
                Taskslist = Tasks.GetActionItemsList(objTask);
                if (Taskslist.Count > 0)
                {
                    if (Taskslist[0].Actionitemtitle != null)
                    {
                        ViewBag.Actionitemtitle = Taskslist[0].Actionitemtitle;
                    }
                    else
                    {
                        ViewBag.Actionitemtitle = null;
                    }
                    if (Taskslist[0].Task_ID != 0)
                    {
                        ViewBag.Task_ID = Taskslist[0].Task_ID;
                    }
                }
                return View();
           
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editaction()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                int ReferencetypeID = 13;
                int? ReferenceID = null;
                ReferenceID = Convert.ToInt32(Session["userid"]);
                Tasks objTask = new Tasks();
                var _with1 = objTask;
                _with1.Task_ID = Convert.ToInt32(Request["hdntask_id"]);
                _with1.Actionitem_ID = Convert.ToInt32(Request["hdnaction_id"]);
                _with1.Actionitemtitle = Request["txtActionItem"];
                _with1.ActiontitleDescription = null;
                _with1.Reference_ID = ReferenceID;
                _with1.ReferenceType_ID = ReferencetypeID;
                Tasks.AddNewActionItem(objTask);

                return RedirectToAction("TaskList", "Task");
            
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Deleteaction(int Action_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Tasks objAction = new Tasks();
            var _with1 = objAction;
            _with1.Actionitem_ID = Action_id;
            _with1.Status_Ind = "D";
            Tasks.AddNewActionItem(objAction);
            return RedirectToAction("TaskList");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edittask(string taskid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            
          
                ViewBag.roleid = Convert.ToString( Session["roleid"]);
                ViewBag.TaskId = taskid;
                Tasks objTask = new Tasks();
                var _with1 = objTask;
                _with1.Task_ID = Convert.ToInt32(taskid);
                _with1.RoleInd = "N";

                List<Tasks> Taskslist = null;
                Taskslist = Tasks.Tasklist(objTask);
                if (Taskslist.Count > 0)
                {

                    if (Taskslist[0].TaskTitle != null)
                    {
                        ViewBag.TaskTitle = Taskslist[0].TaskTitle;
                    }
                    else
                    {
                        ViewBag.TaskTitle = null;
                    }
                    if (Taskslist[0].DueDate != null)
                    {
                        ViewBag.DueDate = Taskslist[0].DueDate;
                    }
                    else
                    {
                        ViewBag.DueDate = null;
                    }
                    if (Taskslist[0].IsDone != null)
                    {
                        if (Taskslist[0].IsDone == "Y")
                        {
                            ViewBag.IsDone = "Y";
                        }
                        else
                        {
                            ViewBag.IsDone = "N";
                        }
                    }
                    if (Taskslist[0].ChkStatus_Ind == "Y")
                    {
                        ViewBag.ChkStatus_Ind = Taskslist[0].ChkStatus_Ind;
                    }
                    else
                    {
                        ViewBag.ChkStatus_Ind = "N";
                    }
                    if (Taskslist[0].Notes != null)
                    {
                        ViewBag.Notes = Taskslist[0].Notes;
                    }
                    else
                    {
                        ViewBag.Notes = null;
                    }
                }
                return View();
           
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Edittask()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                string Status;

                if (Request["rdStatus_0_Edit"] == "Complete")
                {
                    Status = "Y";
                }
                else
                {
                    Status = "N";
                }
                if (Request["chkmsg_Edit"] != null)
                {
                    string[] WorkphoneLeaveMsg = null;
                    if (Request["chkmsg_Edit"].Contains(","))
                    {
                        WorkphoneLeaveMsg = Request["chkmsg_Edit"].Split(',');
                        if (WorkphoneLeaveMsg.Length == 2)
                        {
                            if (WorkphoneLeaveMsg[0] == "true")
                            {
                                chkStaus = "Y";
                            }
                            else if (WorkphoneLeaveMsg[0] == "false")
                            {
                                chkStaus = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkmsg_Edit"] == "false")
                        {
                            chkStaus = "N";
                        }
                        else if (Request["chkmsg_Edit"] == "true")
                        {
                            chkStaus = "Y";
                        }
                    }
                }


                Tasks objTask = new Tasks();
                var _with1 = objTask;
                _with1.Task_ID = Convert.ToInt32(Request["Hdntask_ID"]);
                _with1.TaskTitle = Request["txt_title_Edit"];
                if (Convert.ToString( Session["RoleID"]) == "1")
                {
                    _with1.Reference_ID = 1;
                    _with1.ReferenceType_ID = 1;
                }
                else
                {
                    _with1.Reference_ID = Convert.ToInt32(Session["Prov_ID"]);
                    _with1.ReferenceType_ID = Convert.ToInt32("2");
                }
                if (!string.IsNullOrEmpty(Request["txtDate_Edit"]))
                {
                    _with1.DueDate = Request["txtDate_Edit"];
                }
                else
                {
                    _with1.DueDate = null;
                }
                _with1.Notes = Request["txtnotes_Edit"];
                _with1.IsDone = Status;
                _with1.ChkStatus_Ind = chkStaus;
                _with1.Assignedusers = null;
                _with1.Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                Tasks.UpdateTask(objTask);
                return RedirectToAction("TaskList", "Task");
            
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Viewaction(int taskid)
        {
           
                Tasks objTask = new Tasks();
                var _with1 = objTask;
                _with1.sortField = "LastAction";
                _with1.sortDirection = "DESC";
                _with1.Actionitemtitle = null;
                _with1.Task_ID = taskid;
                List<Tasks> ActionItemsList = null;
                ActionItemsList = Tasks.ActionItemsList(objTask);
                ViewBag.totrec = ActionItemsList.Count;
                return View(ActionItemsList);
            
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MarkCompleted(string taskid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Tasks objTask = new Tasks();
            var _with1 = objTask;
            _with1.Task_ID = Convert.ToInt32(taskid);
            _with1.RoleInd = "N";

            List<Tasks> Taskslist = null;
            Taskslist = Tasks.Tasklist(objTask);
            if (Taskslist.Count > 0)
            {

                if (Taskslist[0].TaskTitle != null)
                {
                    _with1.TaskTitle = Taskslist[0].TaskTitle;
                }
                else
                {
                    _with1.TaskTitle = null;
                }
                if (Taskslist[0].DueDate != null)
                {
                    _with1.DueDate = Taskslist[0].DueDate;
                }
                else
                {
                    _with1.DueDate = null;
                }
                if (Taskslist[0].Notes != null)
                {
                    _with1.Notes = Taskslist[0].Notes;
                }
                else
                {
                    _with1.Notes = null;
                }
            }
                _with1.IsDone = "Y";
                _with1.ChkStatus_Ind = "N";
                if (Convert.ToString( Session["RoleID"]) == "1")
                {
                    _with1.Reference_ID = 1;
                    _with1.ReferenceType_ID = 1;
                }
                else
                {
                    _with1.Reference_ID = Convert.ToInt32(Session["Prov_ID"]);
                    _with1.ReferenceType_ID = Convert.ToInt32("13");
                }
            Tasks.UpdateTask(objTask);
            return RedirectToAction("TaskList", "Task");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Archivethistask(int taskid)
        {
            //if (Request.IsAjaxRequest() && Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Tasks objTask = new Tasks();
            var _with1 = objTask;
            _with1.Task_ID = taskid;
                _with1.Status_Ind = "E";
            _with1.Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
            Tasks.UpdateTask(objTask);
            return RedirectToAction("TaskList", "Task");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Activethistask(int taskid)
        {
            //if (Request.IsAjaxRequest() && Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Tasks objTask = new Tasks();
            var _with1 = objTask;
            _with1.Task_ID = taskid;
                _with1.Status_Ind = "A";
            _with1.Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
            Tasks.UpdateTask(objTask);
            return RedirectToAction("TaskList", "Task");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Deletetask(int taskid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Tasks objTask = new Tasks();
            var _with1 = objTask;
            _with1.Task_ID = taskid;
            _with1.Status_Ind = "D";
            _with1.Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
            Tasks.UpdateTask(objTask);
            return RedirectToAction("TaskList", "Task");
        }

    }
}


         
       
     
           
      


               
       
