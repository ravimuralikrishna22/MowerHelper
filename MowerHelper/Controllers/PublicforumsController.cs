using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Forums;

namespace MowerHelper.Controllers
{
    public class PublicforumsController : Controller
    {
        //
        // GET: /Publicforums/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForumsHome()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            ViewBag.txttopic = Request["txttopic"];
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

            List<Forums> forumlist = default(List<Forums>);
            string publicind = null;
            publicind = "Y";
            Forums objforum = new Forums();
            objforum.Forum_ID = null;
            objforum.OrderByItem = "lastActivity";
            objforum.OrderBy = "DESC";
            objforum.NoofRecords = 10;
            objforum.PageNo = 1;
            objforum.Public_Ind = publicind;
            objforum.Login_ID = Convert.ToInt32(Session["UserId"]);
            objforum.Role_ID = Convert.ToInt32(Session["roleid"]);
            objforum.Keyword = Request["txttopic"];

            objforum.StartDate = FromDate;
            objforum.EndDate = ToDate;
           
            forumlist = Forums.Get_Topics_forum(objforum);
            ViewBag.totrec = Forums.TotalRecords;

            return View(forumlist);
        }
        [HttpGet()]
        public ActionResult ForumTopics(int ForumID,string page=null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.ForumID = ForumID;
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            ViewBag.txttopic = Request["txttopic"];
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

            Forums objforum = new Forums();
            List<Forums> objlist = new List<Forums>();
            objlist = Forums.Get_FourmTopicDescription(ForumID, null);
            ViewBag.ForumName = Convert.ToString(objlist[0].ForumName);
            ViewBag.Forum_ID = ForumID;
          
            Forums objforum1 = new Forums();
            List<Forums> objlist1 = new List<Forums>();

            if (string.IsNullOrEmpty(ForumID.ToString()))
            {
                objforum1.Forum_ID = 0;
            }
            else
            {
                objforum1.Forum_ID = ForumID;
            }


            objforum1.TOPIC_ID = null;
            if (Request["txttopic"] != "" && Request["txttopic"] != null)
            {
                objforum1.Keyword = Request["txttopic"];
            }
            else
            {
                objforum1.Keyword = null;
            }
            objforum1.Status_Ind = "A";
            objforum1.OrderByItem = "lastActivity";
            if (Request["sortdir"] != null & Request["sortdir"] != "")
            {
                objforum1.OrderBy = Request["sortdir"];
            }
            else
            {
                objforum1.OrderBy = "DESC";
            }
            if (Request["page"] != "" && Request["page"] != null)
            {
                objforum1.PageNo = Convert.ToInt32(Request["page"]);
                ViewBag.page = Request["page"];
            }
            else
            {
                objforum1.PageNo = 1;
            }

            objforum1.NoofRecords = 10;
            
          
                objforum1.StartDate = FromDate;
                objforum1.EndDate = ToDate;
            objlist1 = Forums.Search_ForumMessages(objforum1);
            ViewBag.totrec = Forums.TotalRecords;
            return View(objlist1);
            
        }
      
          [HttpGet()]
        public ActionResult FourmMessages(int Forum_ID,int Topic_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.forid = Forum_ID;
            ViewBag.Topic_ID = Topic_ID;
            Forums objforum = new Forums();
            DataSet dsdayout = objforum.Get_FourmNameTopicDescription( Forum_ID, Topic_ID,null);
            ViewBag.fourmnm = Convert.ToString(dsdayout.Tables[0].Rows[0]["Forumname"]);
            ViewBag.topicnm = Convert.ToString(dsdayout.Tables[0].Rows[0]["TopicName"]);
            ViewBag.topicdes = Convert.ToString(dsdayout.Tables[0].Rows[0]["TopicDescription"]);
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            ViewBag.txttopic = Request["txttopic"];
        
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

         
            Forums objForum = new Forums();
            List<Forums> objlist = new List<Forums>();

            objForum.TOPIC_ID = Topic_ID;
            if (Request["txttopic"] != "" && Request["txttopic"] != null)
            {
                objForum.Keyword = Request["txttopic"];
            }
            else
            {
                objForum.Keyword = null;
            }

            objForum.Forum_ID = Forum_ID;
            objForum.Status_Ind = "A";
            objForum.OrderByItem = "CreatedOn";
            if (Request["sortdir"] != null & Request["sortdir"] != "")
            {
                objForum.OrderBy = Request["sortdir"];
            }
            else
            {
                objForum.OrderBy = "DESC";
            }
            if (Request["page"] != "" && Request["page"] != null)
            {
                objForum.PageNo = Convert.ToInt32(Request["page"]);
            }
            else
            {
                objForum.PageNo = 1;
            }
            objForum.NoofRecords = 10;


            objForum.StartDate = FromDate;
            objForum.EndDate = ToDate;
          
            objlist = Forums.Search_ForumTopic(objForum);
            ViewBag.totrec = Forums.TotalRecords;

            return View(objlist);
        }
    }
}
