using Obout.Mvc.ComboBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Forums;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class ForumsController : Controller
    {
        //
        // GET: /Forums/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForumsList()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

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

            List<Forums> forumlist = default(List<Forums>);
            Forums objforum = new Forums();
            objforum.Forum_ID = null;
            objforum.OrderByItem = "LatestPost";
            objforum.OrderBy = "DESC";
            objforum.Keyword = txtKeyword;
            objforum.NoofRecords = 10;
            objforum.PageNo = Convert.ToInt32(Request["Page"]!=null ? Request["Page"] : "1");
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;
            forumlist = Forums.Get_Forum(objforum);
            ViewBag.totrec = Forums.TotalRecords;




            return View(forumlist);
        }
        public ActionResult AddNewForum()
        {
            
                return View();
           
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult AddNewForum(string obj1)
        {
           
            int? IntDDreceiver = 0;
            int IntRoleId = 0;

            if (Request["rdStatus_1"] == "No")
            {
                IntDDreceiver = 1;
                IntRoleId = 1;
            }
            else
            {
                if (Request["ddGroup"] == "1")
                {
                    IntDDreceiver = 1;
                    IntRoleId = 1;
                }
                else
                {
                    IntDDreceiver = null;
                    IntRoleId = Convert.ToInt32(Request["ddGroup"]);
                }
            }

            string strOutMsg = string.Empty;
            Forums objIns = new Forums();

            objIns.ForumName = Request["txtformname"];
            objIns.Login_ID = IntDDreceiver;
            objIns.Message = Request["txtdesc"];
            objIns.Public_Ind = (Request["rdStatus_2"] == "Yes" ? "Y" : "N");
            objIns.ReadTopicsPosts = "Y";
            //objIns.PostNewTopics = (PostNewTopics.Checked == true ? "Y" : "N");
            objIns.PostNewTopics = "Y";
            objIns.PostNewReplies = "Y";
            objIns.Role_ID = IntRoleId;
            objIns.ModeratorForum_Ind = (Request["rdStatus_0"] == "Yes" ? "Y" : "N");
            strOutMsg = Forums.Insert_Forum(objIns);
            if (strOutMsg != null & strOutMsg.Length > 8)
            {
                //lblforum.Text = strOutMsg;
                return Json(JsonResponseFactory.ErrorResponse(strOutMsg), JsonRequestBehavior.DenyGet);
            }
            else
            {
                //lblforum.Text = "";
                int strNewForumId = Convert.ToInt32(strOutMsg);

                if (Request["rdStatus_0"] == "Yes")
                {
                    if (Request["ddGroup"] != "1")
                    {
                        Forums obj = new Forums();
                        obj.Forum_ID = strNewForumId;
                        obj.Reference_IDs = Request["combobox1"];
                        obj.ReferenceType_ID = 2;
                        obj.Login_ID = Convert.ToInt32(Session["UserId"]);
                        Forums.INS_UPD_Forum_Moderators(obj);
                    }
                }

            }
            return Json(JsonResponseFactory.SuccessResponse(obj1), JsonRequestBehavior.DenyGet);
        }
        public ActionResult EditForum()
        {
            
                ViewBag.Forum_ID = Request["Forum_ID"];
                Forums obj = new Forums();
                List<Forums> objlist = default(List<Forums>);
                Forums objforum = new Forums(Convert.ToInt32(Request["Forum_ID"]), "LatestPost", "DESC");
                objlist = Forums.Get_Forum(objforum);
                if (objlist[0].ForumName != null)
                {
                    ViewBag.txt_formname = objlist[0].ForumName;
                }
                if (objlist[0].ForumName != null)
                {
                    ViewBag.Role_ID = objlist[0].Role_ID;
                }
                else
                {
                    ViewBag.Role_ID = 0;
                }
                if (ViewBag.Role_ID == 4)
                {
                    List<Forums> objlist1 = new List<Forums>();
                    objlist1 = Forums.Get_Moderators(Convert.ToInt32(Request["Forum_ID"]));
                    TempData["combobox2"] = objlist1[0].ModeratorName;
                    ViewBag.ProvID = objlist1[0].Provider_ID;
                    string abc = objlist1[0].ModeratorName;
                    GetFilteredproviders(abc, 0);
                }

                if (objlist[0].Message != null)
                {
                    ViewBag.txt_desc = objlist[0].Message;
                }

                if (objlist[0].Public_Ind == "Y")
                {
                    ViewBag.IsPublic_Ind = "0";
                }
                else
                {
                    ViewBag.IsPublic_Ind = "1";
                }

                if (objlist[0].ModeratorForum_Ind == "Y")
                {
                    ViewBag.IsModerator = "0";
                }
                else
                {
                    ViewBag.IsModerator = "1";
                }
                return View();
            
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult EditForum(string obj1)
        {
            
            int? IntDDreceiver = 0;
            int IntRoleId = 0;

            if (Request["rd_Status_1"] == "No")
            {
                IntDDreceiver = 1;
                IntRoleId = 1;
            }
            else
            {
                if (Request["dd_Group"] == "1")
                {
                    IntDDreceiver = 1;
                    IntRoleId = 1;
                }
                else
                {
                    IntDDreceiver = null;
                    IntRoleId = Convert.ToInt32(Request["dd_Group"]);
                }
            }



            bool objlist = false;

            Forums objUpd = new Forums();
            objUpd.Forum_ID = Convert.ToInt32(Request["hdnForum_ID"]);
            objUpd.Login_ID = IntDDreceiver;
            objUpd.ForumName = Request["txt_formname"];
            objUpd.Message = Request["txt_desc"];
            objUpd.ReadTopicsPosts = "Y";
            //objUpd.PostNewTopics = (PostNewTopics.Checked == true ? "Y" : "N");
            objUpd.PostNewTopics = "Y";
            objUpd.PostNewReplies = "Y";
            objUpd.Public_Ind = (Request["rd_Status_2"] == "Yes" ? "Y" : "N");
            objUpd.Role_ID = IntRoleId;
            objUpd.ModeratorForum_Ind = (Request["rd_Status_0"] == "Yes" ? "Y" : "N");
            objlist = Forums.Update_Forum(objUpd);

            if (Request["rd_Status_0"] == "Yes")
            {
                if (Request["dd_Group"] != "1")
                {


                    Forums obj = new Forums();
                    obj.Forum_ID = Convert.ToInt32(Request["hdnForum_ID"]);
                    obj.Reference_IDs = Request["combobox2"];
                    obj.ReferenceType_ID = 2;
                    obj.Login_ID = Convert.ToInt32(Session["UserId"]);
                    Forums.INS_UPD_Forum_Moderators(obj);
                }
            }
            //return Json(JsonResponseFactory.ErrorResponse(strOutMsg), JsonRequestBehavior.DenyGet);
            return Json(JsonResponseFactory.SuccessResponse(obj1), JsonRequestBehavior.DenyGet);

        }
        public JsonResult LoadingItems(ComboBoxLoadingItemsEventArgs args)
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
            try{
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["combobox2"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "ForumsController", "GetFilteredproviders", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredproviders1(args.Text, 0);

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
        protected ComboBoxItemList GetFilteredproviders1(string filter, int offset)
        {
            try{
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["combobox1"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "ForumsController", "GetFilteredproviders1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult DeleteForum(int Forum_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums.Del_Forum(Forum_ID);
            return RedirectToAction("ForumsList");
        }
        public ActionResult TopicsList(int Forum_ID)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Forum_ID = Forum_ID;
            List<Forums> objTopiclist = new List<Forums>();
            objTopiclist = Forums.Get_FourmTopicDescription(Forum_ID, null);
            ViewBag.ForumName = Convert.ToString(objTopiclist[0].ForumName);

            //ViewBag.ForumName = Request["ForumName"];
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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



            Forums objforum = new Forums();
            List<Forums> objlist = new List<Forums>();

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == 1 | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objforum.Status_Ind = "P";
            //    }
            //    else
            //    {
            //        objforum.Status_Ind = "A";
            //    }
            //}
            //else
            //{
            //if (Session["RoleId"] == "1")
            //{
            //    objforum.Status_Ind = "P";
            //}
            //else
            //{
            //    objforum.Status_Ind = "A";
            //}
            //}
            objforum.Status_Ind = "P";

            objforum.TOPIC_ID = null;
            objforum.Forum_ID = Convert.ToInt32(Request["Forum_ID"]);
            objforum.Keyword = Request["txtKeyword"];
            objforum.OrderBy = "DESC";
            objforum.OrderByItem = "lastActivity";
            objforum.PageNo = Convert.ToInt32(Request["Page"] != null ? Request["Page"] : "1");
            objforum.NoofRecords = 10;
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;
            objlist = Forums.Search_ForumMessages(objforum);
            ViewBag.totrec = Forums.TotalRecords;
            return View(objlist);
        }

        public ActionResult AddNewTopic(string ind = null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                ViewBag.Forum_ID = Request["Forum_ID"];
                ViewBag.ind = ind;
                Forums objforum = new Forums();
                List<Forums> objlist = new List<Forums>();
                objlist = Forums.Get_FourmTopicDescription(Convert.ToInt32(Request["Forum_ID"]), null);
                ViewBag.ForumName = Convert.ToString(objlist[0].ForumName);
                return View();
            
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddNewTopic(string hdnForum_ID, string hdnind)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            bool objlist = false;

            string strimage = "";
            if (Request["rdStatus_0"] == "0")
            {
                strimage = null;
            }
            else if (Request["rdStatus_1"] == "1")
            {
                strimage = "exclamation.gif";
            }
            else if (Request["rdStatus_2"] == "2")
            {
                strimage = "question.gif";
            }

            Forums objins = new Forums();
            objins.Forum_ID = Convert.ToInt32(Request["hdnForum_ID"]);
            objins.TopicName = Request["txtSubject"];
            objins.TopicDescription = Request["txtdesc"];
            objins.Image_Name = strimage;
            objins.CreatedBy = Convert.ToString(Session["UserId"]);

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == "1" | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objins.Status_Ind = "A";
            //    }
            //    else
            //    {
            //        objins.Status_Ind = "P";
            //    }
            //}
            //else
            //{
            if (hdnind == "1")
            {
                objins.Status_Ind = "P";
            }
            else
            {

                objins.Status_Ind = "A";
            }
            //}

            objlist = Forums.Insert_Topic(objins);
            if (hdnind == "1")
            {
                return RedirectToAction("ForumTopics", new { Forum_ID = Convert.ToInt32(hdnForum_ID) });
            }
            else
            {
                return RedirectToAction("TopicsList", new { Forum_ID = hdnForum_ID });
            }
        }

        public ActionResult DeleteTopic(int TOPIC_ID, string Forum_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums.Del_FormTopics(TOPIC_ID, Convert.ToInt32(Session["UserID"]));
            return RedirectToAction("TopicsList", new { Forum_ID = Forum_ID });
        }
        public ActionResult TopicStatusChange(int Topic_ID, string StatusID,string Forum_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums.Upd_TopicStatus(Convert.ToInt32(Topic_ID), StatusID);
                return RedirectToAction("TopicsList",new{Forum_ID=Forum_ID});
        }


        public ActionResult MessagesList(int Forum_ID, int Topic_ID)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums obj_forum = new Forums();
            DataSet dsmsg = obj_forum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID,"1");
            ViewBag.Forum_ID = Forum_ID;
            ViewBag.TOPIC_ID = Topic_ID;
            ViewBag.TopicDescription = Convert.ToString(dsmsg.Tables[0].Rows[0]["TopicDescription"]);
            ViewBag.TopicName = Convert.ToString(dsmsg.Tables[0].Rows[0]["TopicName"]);
            ViewBag.ForumName = Convert.ToString(dsmsg.Tables[0].Rows[0]["Forumname"]);


            //ViewBag.TopicDescription = Request["TopicDescription"];
            //ViewBag.TopicName = Request["TopicName"];
            //ViewBag.ForumName = Request["ForumName"];
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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

            Forums objforum = new Forums();
            List<Forums> objlist = new List<Forums>();

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == 1 | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objforum.Status_Ind = "P";
            //    }
            //    else
            //    {
            //        objforum.Status_Ind = "A";
            //    }
            //}
            //else
            //{
            //    if (Session["RoleId"] == 1)
            //    {
            //        objforum.Status_Ind = "P";
            //    }
            //    else
            //    {
            //        objforum.Status_Ind = "A";
            //    }
            //}
            objforum.Status_Ind = "P";
            objforum.TOPIC_ID = Convert.ToInt32(Request["TOPIC_ID"]);
            objforum.Keyword = Request["txtKeyword"];
            objforum.Forum_ID = Convert.ToInt32(Request["Forum_ID"]);
            objforum.OrderByItem = "CreatedOn";
            objforum.OrderBy = "DESC";
            objforum.PageNo = Convert.ToInt32(Request["Page"] != null ? Request["Page"] : "1");
            objforum.NoofRecords = 10;
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;

            objlist = Forums.Search_ForumTopic(objforum);
            ViewBag.totrec = Forums.TotalRecords;
            return View(objlist);
        }
        public ActionResult MessageStatusChange(int Message_ID, string StatusID,string Forum_ID, string TOPIC_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums.Upd_MessageStatus(Message_ID, StatusID);
            return RedirectToAction("MessagesList", new { Message_ID = Message_ID, Forum_ID = Forum_ID,TOPIC_ID = TOPIC_ID });
        }

        public ActionResult AddNewMessage(int Forum_ID, int Topic_ID,string ind=null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            
                DataSet dsdayout = new DataSet();
                Forums objforum = new Forums();
                ViewBag.ind = ind;
                if (ind == "1")
                {
                    dsdayout = objforum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID, null);
                }
                else
                {
                    dsdayout = objforum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID, "1");
                }

                ViewBag.Forum_ID = Forum_ID;
                ViewBag.TOPIC_ID = Topic_ID;
                ViewBag.ForumName = Convert.ToString(dsdayout.Tables[0].Rows[0]["Forumname"]);
                ViewBag.TopicName = Convert.ToString(dsdayout.Tables[0].Rows[0]["TopicName"]);
                ViewBag.TopicDescription = Convert.ToString(dsdayout.Tables[0].Rows[0]["TopicDescription"]);
                return View();
           
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddNewMessage(string Forum_ID, string TOPIC_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           // bool objlist = false;

            string strimage = "";
            if (Request["rdStatus_0"] == "0")
            {
                strimage = null;
            }
            else if (Request["rdStatus_1"] == "1")
            {
                strimage = "exclamation.gif";
            }
            else if (Request["rdStatus_2"] == "2")
            {
                strimage = "question.gif";
            }
            int replyout = 0;
            Forums objMsgIns = new Forums();
            objMsgIns.TOPIC_ID = Convert.ToInt32(Request["TOPIC_ID"]);
            objMsgIns.Forum_ID = Convert.ToInt32(Request["Forum_ID"]);
            objMsgIns.TopicName = Request["txtSubject"];
            objMsgIns.TopicDescription = Request["txtdesc"];
            objMsgIns.Image_Name = strimage;
            objMsgIns.Reply_ID = null;
            objMsgIns.Reply_Ind = "N";
            objMsgIns.CreatedBy = Convert.ToString(Session["UserId"]);

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == 1 | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objMsgIns.Status_Ind = "A";
            //    }
            //    else
            //    {
            //        objMsgIns.Status_Ind = "P";
            //    }
            //}
            //else
            //{
            if (Request["hdnind"] != "1")
            {
                objMsgIns.Status_Ind = "A";
            }
            else
            {
                objMsgIns.Status_Ind = "P";
            }
            //}

            replyout = Forums.Insert_ReplyMsg(objMsgIns);
            if (Request["hdnind"] != "1")
            {
                return RedirectToAction("MessagesList", new { Forum_ID = Forum_ID, TOPIC_ID = TOPIC_ID });
            }
            else
            {
                return RedirectToAction("ForumMessages", new { Forum_ID = Forum_ID, TOPIC_ID = TOPIC_ID });
            }
        }


        public ActionResult DeleteMessage(int Message_ID, string Forum_ID,string TOPIC_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums.Del_MessagesForTopics(Message_ID, Convert.ToInt32(Session["UserID"]));
            return RedirectToAction("MessagesList", new { Forum_ID = Forum_ID, TOPIC_ID = TOPIC_ID });
        }

        public ActionResult MessageReply(int Forum_ID, int Topic_ID,string ind=null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
          
                ViewBag.ind = ind;
                DataSet dsdayout = new DataSet();
                Forums objforum = new Forums();
                if (ind != "1")
                {
                    dsdayout = objforum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID, "1");
                }
                else
                {
                    dsdayout = objforum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID, null);
                }
                ViewBag.Forum_ID = Forum_ID;
                ViewBag.TOPIC_ID = Topic_ID;
                ViewBag.Msg_id = Request["Msg_id"];
                ViewBag.MessageName = "Re : " + Request["MessageName"];

                ViewBag.ForumName = Convert.ToString(dsdayout.Tables[0].Rows[0]["Forumname"]);
                ViewBag.TopicName =Convert.ToString( dsdayout.Tables[0].Rows[0]["TopicName"]);
                ViewBag.TopicDescription = Convert.ToString(dsdayout.Tables[0].Rows[0]["TopicDescription"]);
                return View();
           
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MessageReply(string hdn_Forum_ID, string hdn_TOPIC_ID, string hdn_TopicDescription)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           // bool objlist = false;

            string strimage = "";
            if (Request["rd_Status_0"] == "0")
            {
                strimage = null;
            }
            else if (Request["rd_Status_1"] == "1")
            {
                strimage = "exclamation.gif";
            }
            else if (Request["rd_Status_2"] == "2")
            {
                strimage = "question.gif";
            }
            int replyout = 0;
            Forums objMsgIns = new Forums();
            objMsgIns.TOPIC_ID = Convert.ToInt32(hdn_TOPIC_ID);
            objMsgIns.Forum_ID = Convert.ToInt32(hdn_Forum_ID);
            objMsgIns.TopicName = Request["txt_Subject"];
            objMsgIns.TopicDescription = Request["txt_desc"];
            objMsgIns.Image_Name = strimage;
            objMsgIns.Reply_ID = Request["Msg_id"];
            objMsgIns.Reply_Ind = "Y";
            objMsgIns.CreatedBy = Convert.ToString(Session["UserId"]);

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == 1 | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objMsgIns.Status_Ind = "A";
            //    }
            //    else
            //    {
            //        objMsgIns.Status_Ind = "P";
            //    }
            //}
            //else
            //{
            if (Request["hdnind"] != "1")
            {
                objMsgIns.Status_Ind = "A";
            }
            else
            {
                objMsgIns.Status_Ind = "P";

            }
            //}

            replyout = Forums.Insert_ReplyMsg(objMsgIns);

            if (Request["hdnind"] != "1")
            {
                return RedirectToAction("MessagesList", new { Forum_ID = hdn_Forum_ID, TOPIC_ID = hdn_TOPIC_ID });
            }
            else
            {
                return RedirectToAction("ForumMessages", new { Forum_ID = hdn_Forum_ID, Topic_ID = hdn_TOPIC_ID });
            }
        }

        public ActionResult ForumsHome()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
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
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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

            List<Forums> forumlist = default(List<Forums>);
            Forums objforum = new Forums();
            objforum.Forum_ID = null;
            objforum.OrderByItem = "LatestPost";
            objforum.OrderBy = "DESC";
            objforum.Keyword = txtKeyword;
            objforum.Public_Ind = null;
            objforum.NoofRecords = Convert.ToInt32(pgesize);
            objforum.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            objforum.Login_ID = Convert.ToInt32(Session["UserId"]);
            objforum.Role_ID = Convert.ToInt32(Session["roleid"]);
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;
            forumlist = Forums.Get_Topics_forum(objforum);
            ViewBag.totrec = Forums.TotalRecords;
            return View(forumlist);
        }

        public ActionResult ForumTopics(int Forum_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Forum_ID = Forum_ID;
            List<Forums> objTopiclist = new List<Forums>();
            objTopiclist = Forums.Get_FourmTopicDescription(Forum_ID, null);
            ViewBag.ForumName = Convert.ToString(objTopiclist[0].ForumName);

            //ViewBag.ForumName = Request["ForumName"];
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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



            Forums objforum = new Forums();
            List<Forums> objlist = new List<Forums>();

         
            objforum.Status_Ind = "A";

            objforum.TOPIC_ID = null;
            objforum.Forum_ID = Convert.ToInt32(Request["Forum_ID"]);
            objforum.Keyword = Request["txtKeyword"];
            objforum.OrderBy = "DESC";
            objforum.OrderByItem = "lastActivity";
            objforum.PageNo = Convert.ToInt32(Request["Page"] != null ? Request["Page"] : "1");
            objforum.NoofRecords = 10;
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;
            objlist = Forums.Search_ForumMessages(objforum);
            ViewBag.totrec = Forums.TotalRecords;
            return View(objlist);
        }

        public ActionResult ForumMessages(int Forum_ID, int Topic_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Forums obj_forum = new Forums();
            DataSet dsmsg = obj_forum.Get_FourmNameTopicDescription(Forum_ID, Topic_ID,null);
            ViewBag.Forum_ID = Forum_ID;
            ViewBag.TOPIC_ID = Topic_ID;
            ViewBag.TopicDescription = Convert.ToString(dsmsg.Tables[0].Rows[0]["TopicDescription"]);
            ViewBag.TopicName = Convert.ToString(dsmsg.Tables[0].Rows[0]["TopicName"]);
            ViewBag.ForumName = Convert.ToString(dsmsg.Tables[0].Rows[0]["Forumname"]);


            //ViewBag.TopicDescription = Request["TopicDescription"];
            //ViewBag.TopicName = Request["TopicName"];
            //ViewBag.ForumName = Request["ForumName"];
            string txtKeyword;
            if (Request["txtKeyword"] != null & Request["txtKeyword"] != "")
            {
                ViewBag.Title = Request["txtKeyword"];
                txtKeyword = Request["txtKeyword"];
            }
            else
            {
                ViewBag.Title = null;
                txtKeyword = null;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);

            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5" || Request["dt_filter"] == "1")
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

            Forums objforum = new Forums();
            List<Forums> objlist = new List<Forums>();

            //if (ViewState("ModeratorForumInd") == "Y")
            //{
            //    if (Session["RoleId"] == 1 | ViewState("ModeratorStatus") == "Y")
            //    {
            //        objforum.Status_Ind = "P";
            //    }
            //    else
            //    {
            //        objforum.Status_Ind = "A";
            //    }
            //}
            //else
            //{
            //    if (Session["RoleId"] == 1)
            //    {
            //        objforum.Status_Ind = "P";
            //    }
            //    else
            //    {
            //        objforum.Status_Ind = "A";
            //    }
            //}
            objforum.Status_Ind = "A";
            objforum.TOPIC_ID = Convert.ToInt32(Request["TOPIC_ID"]);
            objforum.Keyword = Request["txtKeyword"];
            objforum.Forum_ID = Convert.ToInt32(Request["Forum_ID"]);
            objforum.OrderByItem = "CreatedOn";
            objforum.OrderBy = "DESC";
            objforum.PageNo = Convert.ToInt32(Request["Page"] != null ? Request["Page"] : "1");
            objforum.NoofRecords = 10;
            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;

            objlist = Forums.Search_ForumTopic(objforum);
            ViewBag.totrec = Forums.TotalRecords;
            return View(objlist);
        }
    }
}
