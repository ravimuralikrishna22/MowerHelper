using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class LearnExploreController : Controller
    {
        //
        // GET: /LearnExplore/
        public string ObjKeyGen = "";
        public string Requesturl;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Articles(string page=null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            List<Referrals> list = new List<Referrals>();
            Referrals video = new Referrals();

            if (Convert.ToInt32(Request["ddlsearch"]) == 0)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    video.searchby = Request["txtsearch"];
                    video.Description = null;
                }
                else
                {
                    video.searchby = null;
                    video.Description = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 1)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    video.searchby = null;
                    video.Description = Convert.ToString(Request["txtsearch"]);
                }
                else
                {
                    video.searchby = null;
                    video.Description = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 2)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Request["txtsearch"])))
                {
                    video.searchby = Request["txtsearch"].ToString();
                    video.Description = Request["txtsearch"].ToString();
                }
                else
                {
                    video.searchby = null;
                    video.Description = null;
                }
            }
            ViewBag.ddlsearch = Request["ddlsearch"];
            ViewBag.txtsearch = Request["txtsearch"];
            //If txtsearch.Text <> "" Then
            //    video.searchby = txtsearch.Text
            //Else
            //    video.searchby = Nothing
            //End If


            List<Articlevideos> myvideo = new List<Articlevideos>();

            string pagNum = page != null ? page : "1";
            ViewBag.Pagenation = pagNum;
            video.PageNo = pagNum;


            video.Online_ind = "Y";
            int Total_Records = 0;
            if (Request["rbomost"] != null && Request["rbomost"] != "" && Request["rbomost"] != "1")
            {
                getArticleIDs();
                string vids = (string)TempData["Vids"];
                list = Referrals.List_publicArticles1(video, vids, ref Total_Records);
            }
            else
            {
                list = Referrals.List_publicArticles(video, ref Total_Records);
            }

            Referrals obj1 = new Referrals();
            ViewBag.articles = null;
            if (list.Count > 0)
            {
                ViewBag.articles = list;
                GetDataListPaging(ref Total_Records);
            }
            ViewBag.norec = Total_Records;
            ViewBag.articlescnt = list.Count;


            return View();
        }
        public void getArticleIDs()
        {
            try
            {

            clsCommonFunctions objcommon = new clsCommonFunctions();
            TempData["tabind"] = null;
            string strSiteStatisticID = "";
            ViewBag.rbomost = "1";
            if ((Convert.ToInt32(Request["rbomost"]) == 2))
            {
                ViewBag.rbomost = "2";
                TempData["tabind"] = "1";
                strSiteStatisticID = "18";
            }
            else if ((Convert.ToInt32(Request["rbomost"]) == 3))
            {
                ViewBag.rbomost = "3";
                TempData["tabind"] = "1";
                strSiteStatisticID = "19";
            }
            // Dim objparam() As IDataParameter = {New SqlParameter("@in_Statistic_ID", IIf(strSiteStatisticID <> "", strSiteStatisticID, Nothing))}
            DataSet dsIDs = new DataSet();
            // objcommon.AddInParameters(objparam)
            // dsIDs = objcommon.GetDataSet("cogadmin_dbo.st_Public_List_VideosViewdCount")
            string title = "";
            string Desc = "";
            if (Convert.ToInt32(Request["ddlsearch"]) == 0)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    title = Request["txtsearch"].ToString();
                    Desc = null;

                }
                else
                {
                    title = null;
                    Desc = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 1)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    title = null;
                    Desc = Request["txtsearch"].ToString();

                }
                else
                {

                    title = null;
                    Desc = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 2)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    title = Request["txtsearch"].ToString();
                    Desc = Request["txtsearch"].ToString();

                }
                else
                {
                    title = null;
                    Desc = null;
                }
            }
            ViewBag.Vids = null;
            dsIDs = Articlevideos.GetArticlesORVideosCount(strSiteStatisticID, "A", title, Desc);
            if (!(dsIDs == null))
            {
                if ((dsIDs.Tables.Count > 0))
                {
                    if ((dsIDs.Tables[0].Rows.Count > 0))
                    {
                        for (int i = 0; (i
                                    <= (dsIDs.Tables[0].Rows.Count - 1)); i++)
                        {
                            if (Convert.ToString(dsIDs.Tables[0].Rows[i]["Article_ID"]) != "")
                            {
                                ViewBag.Vids = ViewBag.Vids + Convert.ToString(dsIDs.Tables[0].Rows[i]["Article_ID"]) + "," + Convert.ToString(dsIDs.Tables[0].Rows[i]["MaxValue"]) + ",$";
                                //(dsIDs.Tables[0].Rows[i]["Article_ID"] + (","
                                //            + (dsIDs.Tables[0].Rows[i].Item["MaxValue"] + ",$")));
                            }
                        }
                    }
                }
            }
            TempData["Vids"] = ViewBag.Vids;
            }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "getArticleIDs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void GetDataListPaging(ref int totNoRecords)
        {
            try{
            var objData = new ProviderListPaging();
            List<ProviderListPaging> objDataList = objData.GetPagingforDataList(ref totNoRecords);
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
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "GetDataListPaging", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public void BindArticle(int articleid)
        {
            try
            {
            IDataParameter[] sqlparam = { new SqlParameter("@atricle_id", articleid) };
            clsCommonFunctions objcommon = new clsCommonFunctions();
            objcommon.AddInParameters(sqlparam);
            DataSet ds = new DataSet();
            ds = objcommon.GetDataSet("Help_dbo.st_Admin_ListArticles");
            if ((ds != null))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Article_Desc"])!="")
                        {
                            ViewBag.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["Article_Desc"]);
                        }
                       // article.Visible = false;
                        // divarticle.Visible = False
                       // tdpaging.Visible = false;
                       // Fulldescription.Visible = true;
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Title"])!="")
                        {
                            ViewBag.Title = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
                           
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Createdon"])!="")
                        {
                            ViewBag.Date = Convert.ToString(ds.Tables[0].Rows[0]["Createdon"]);
                        }
                    
                        if (Convert.ToString(ds.Tables[0].Rows[0]["imagepath"])!="")
                        {
                            ViewBag.path = Convert.ToString(ds.Tables[0].Rows[0]["imagepath"]);
                        }
                        //if (!string.IsNullOrEmpty(lblpath.Text))
                        //{
                        //    string strPath = clsWebConfigsettings.GetConfigSettingsValue("ImageChecklocal") + "Attachments/Articles/";
                        //    //Server.MapPath("~/Private/Attachments/Articles/")
                        //    strPath = strPath + lblpath.Text;
                        //    if (CheckingRemoteFileExistance(strPath))
                        //    {
                        //        imgImage.ImageUrl = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal") + "PublicImagepreview.aspx?strImgPath=Private/Attachments/Articles/" + lblpath.Text + "&Width=80" + "&Height=120&ind=Y";
                        //        //If System.IO.File.Exists(strPath) Then
                        //        //    imgImage.ImageUrl = strarticlesdomain & "PublicImagepreview.aspx?strImgPath=" & "~/Private/Attachments/Articles/" & lblpath.Text & "&Width=80" & "&Height=120&ind=Y"
                        //        //"~/Private/Attachments/Articles/" & lblpath.Text
                        //        imgImage.Style.Add("display", "");
                        //    }
                        //    else
                        //    {
                        //        imgImage.Style.Add("display", "none");
                        //    }
                        //}
                        //else
                        //{
                        //    imgImage.Style.Add("display", "none");
                        //}
                       // string strprovider = "";
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Provider_Name"])!="")
                        {
                            ViewBag.strprovider = Convert.ToString(ds.Tables[0].Rows[0]["Provider_Name"]);
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["PublicURL"])!="")
                        {
                            ViewBag.strPublicUrl = Convert.ToString(ds.Tables[0].Rows[0]["PublicURL"]);
                        }
                        //lblAuthorFull.Text = "<font size=2>Author : </font>" + "<a runat='server' style='text-decoration:none;color:#044a7d;' target='_blank' href='" + strtherapistsdomain + strPublicUrl + "'>" + strprovider + "</a>";
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Author_Desc"]) != "")
                        {
                            ViewBag.AuthorFulldesc = Convert.ToString(ds.Tables[0].Rows[0]["Author_Desc"]);
                        }
                        //if (!string.IsNullOrEmpty(strprovider))
                        //{
                        //    trauthor.Style.Add("display", "");
                        //}
                        //else
                        //{
                        //    trauthor.Style.Add("display", "none");
                        //}
                        ViewBag.datestrng = DateTime.Parse(ViewBag.Date).DayOfWeek + ", " + DateTime.Parse(ViewBag.Date).ToString("dd MMMM yyyy"); 
                    }
                }
            }
             }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "BindArticle", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public void BindRelatedarticles(int Article_ID)
        {
            try
            {
            var returnset = Articlevideos.Get_RelatedArticle(Article_ID);
           // IDataParameter[] sqlparam = { new SqlParameter("@In_Parent_Article_ID", Article_ID) };
            //clsCommonFunctions objcommon = new clsCommonFunctions();
           // objcommon.AddInParameters(sqlparam);
           // DataSet dsRelatedarticles = new DataSet();
           // dsRelatedarticles = objcommon.GetDataSet("Help_dbo.st_Article_Get_RelatedArticle");
            ViewBag.relatedart = null;
            ViewBag.relatedart = returnset.Tables[0].AsEnumerable();
            ViewBag.relatedartcount = 0;
            ViewBag.relatedartcount = returnset.Tables[0].Rows.Count;
            if (ViewBag.relatedartcount > 0)
            {
                int rowcount = ViewBag.relatedartcount / 2;
                ViewBag.rows = rowcount;
            }
               }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "BindRelatedarticles", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void BindRelatedvideos(int Article_ID)
        {
           try
           {
            IDataParameter[] sqlparam = { new SqlParameter("@In_Article_ID", Article_ID) };
            clsCommonFunctions objcommon = new clsCommonFunctions();
            objcommon.AddInParameters(sqlparam);
            DataSet dsRelatedarticles = new DataSet();
            dsRelatedarticles = objcommon.GetDataSet("Help_dbo.st_Article_Get_Relatedvideos");
             ViewBag.relatedlinks = null;
            ViewBag.relatedlinks = dsRelatedarticles.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = 0;
            ViewBag.relatedlinkcount = dsRelatedarticles.Tables[0].Rows.Count;
           }
           catch (Exception ex)
           {
               var clscustomexc = new clsExceptionLog();
               clscustomexc.LogException(ex, "LearnExplorerController", "BindRelatedvideos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
           }
        }

        public void UpdArticlestatistics(int Article_ID, int SiteStatistics_ID)
        {
            try
            {
                string StateName = "";
                string CityName = "";
                if ((Request.Cookies["State"] != null))
                {
                    if (Request.Cookies["State"].Value != null)
                    {
                        StateName = Convert.ToString(Request.Cookies["State"].Value).Replace("%20", " ");
                    }
                }
                if ((Request.Cookies["City"] != null))
                {
                    if (Request.Cookies["City"].Value != null)
                    {
                        CityName = Convert.ToString(Request.Cookies["City"].Value).Replace("%20", " ");
                    }
                }
                ProviderAdvertising obj1 = new ProviderAdvertising();
                obj1.UpdArticlesStatistics(Article_ID, SiteStatistics_ID, StateName, CityName);
                obj1 = null;
            }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "UpdArticlestatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public ActionResult ElectricianArticle(int arid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.arid = arid;
            UpdArticlestatistics(arid, 18);
         BindArticle(arid);
            BindRelatedarticles(arid);
            BindRelatedvideos(arid);
            return View();
        }
        public ActionResult SendArticle(int arid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.arid = arid;
            ViewBag.inid = Request["inid"];
            BindArticle(arid);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SendArticle(Publicfaq obj)
        {
            if (Request.IsAjaxRequest() && Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("expire"), JsonRequestBehavior.DenyGet);
            }
            if (Request["txtimgKey2"] != Convert.ToString(HttpContext.Session["captchastring"]))
            {
                return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code to register"), JsonRequestBehavior.DenyGet);
            }
            obj.Question_id = Request["hdnartid"];
            obj.strindex_id = Request["hdninid"];
            if (Request.Url != null) Requesturl = Convert.ToString(Request.Url);
            ViewBag.url = Requesturl;
            var strUrl = Requesturl.Split('/');
            Requesturl = strUrl[0] + "//" + strUrl[2] + "/" + strUrl[3] + "/" + "ElectricianArticle?arid=" + Request["hdnartid"];
            string categorycount = Convert.ToString(Category.EmailMsgcount(2));
            var objcategory = new Category
            {
                EmailCategoryCount = Convert.ToInt32(categorycount),
                EmailCategoryID = "2",
                FromReference_id = null,
                Toreference_id = null,
                ISWebService_IND = "N",
                Application_Id = null,
                FromReferenceType_id = null,
                Toreferencetype_id = null
            };
            Category.SentEmailLog(objcategory);
            string strYourName = Request["txtYourName"];
            string strYourEmail = Request["txtYourEmail"];
            string strFriendsEmail = Request["txtFriendsEmail"];
            string strMessage = Sanitizer.GetSafeHtmlFragment(Request["txtMsg"]);
            string strquestion = Request["hdnarticle"];

            //var cs = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {
                var objMailMessage = new ClsMailMessage();
                string str = "<b> Sender Name : </b>" + strYourName + "<br />";
                str = str + "  <b> Message : </b>" + strMessage + "<br />";
                str = str + "  <b> Article Title : </b>" + strquestion + "<br />";
                str = str + "  <b> Article URL : </b> " + Requesturl + "<br />";
                str = str + "  2-" + (categorycount + 1);
                bool ress = objMailMessage.SendMail(strFriendsEmail, strYourEmail, "Article Details", str, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (ress == true)
                {
                    UpdArticlestatistics(Convert.ToInt32(Request["hdnartid"]), 19);
                    ViewBag.ress = true;

                    obj.msg = "Your request will be processed soon";
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                }

                obj.msg = "Due to some technical problems your request will not be processed now";
                return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

            }

            obj.msg = "Your request will be processed soon";
            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

        }
        public ActionResult Videos(string page = null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Articlevideos video = new Articlevideos();
            if (Convert.ToInt32(Request["ddlsearch"]) == 0)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    video.Video_Description = null;
                video.Title = Request["txtsearch"];
                 
                }
                else
                {
                    video.Video_Description = null;
                    video.Title = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 1)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    video.Title = null;
                    video.Video_Description = Convert.ToString(Request["txtsearch"]);
                }
                else
                {
                    video.Title = null;
                    video.Video_Description = null;
                }
            }
            else if (Convert.ToInt32(Request["ddlsearch"]) == 2)
            {
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    video.Title = Convert.ToString(Request["txtsearch"]);
                    video.Video_Description = Convert.ToString(Request["txtsearch"]);
                }
                else
                {
                    video.Title = null;
                    video.Video_Description = null;
                }
            }

            getVideoIDs();
            string pagNum = page != null ? page : "1";
            ViewBag.Pagenation = pagNum;
            ViewBag.txtsearch = (string)Request["txtsearch"];
            ViewBag.ddlsearch = Request["ddlsearch"];
            video.NoofRecords = 6;
            video.PageNo = pagNum;
            video.OrderByItem = null;
              video.OrderBy = null;
              video.Online_ind = "Y";
              string vids = (string)TempData["Vids"];
              string tabind =(string) TempData["tabind"];
            List<Articlevideos> videolist = new List<Articlevideos>();
            videolist = Articlevideos.List_videos(video, vids, tabind);
            ViewBag.videos = null;
            int Total_records = 0;
            if (videolist.Count > 0)
            {
               Total_records = Articlevideos.TotalRecords;
                ViewBag.videos = videolist;
                GetDataListPaging(ref Total_records);
            }
            ViewBag.norec = Total_records;
            ViewBag.videoscnt = videolist.Count;

            return View();
        }
        public ActionResult Video(int videoid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.videoid = videoid;
            UpdArticlestatistics(videoid, 20);
            BindRelatedpublicvideos(videoid);
            BindSingleVideo(videoid);
            
            return View();
        }
        public void BindSingleVideo(int videoid)
        {
            try
            {
            IDataParameter[] sqlparam = { new SqlParameter("@publicvideos_ID", videoid) };
            clsCommonFunctions objcommon = new clsCommonFunctions();
            objcommon.AddInParameters(sqlparam);
            DataSet ds = new DataSet();
            ds = objcommon.GetDataSet("Help_dbo.st_get_Adminlist_videos");
            if ((ds != null))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Video_Description"]) != "")
                        {
                            ViewBag.Video_Description = Convert.ToString(ds.Tables[0].Rows[0]["Video_Description"]);
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Title"]) != "")
                        {
                            ViewBag.Title = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);

                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["CreatedOn"]) != "")
                        {
                            ViewBag.Date = Convert.ToString(ds.Tables[0].Rows[0]["CreatedOn"]);
                        }

                        if (Convert.ToString(ds.Tables[0].Rows[0]["Youtube_embededtext"]) != "")
                        {
                            ViewBag.path = HttpUtility.HtmlDecode(Convert.ToString(ds.Tables[0].Rows[0]["Youtube_embededtext"]));
                        }

                        ViewBag.datestrng = DateTime.Parse(ViewBag.Date).DayOfWeek + ", " + DateTime.Parse(ViewBag.Date).ToString("dd MMMM yyyy");
                    }
                }
            }
            }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "BindSingleVideo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void BindRelatedpublicvideos(int videoid)
        {
            try{
            IDataParameter[] sqlparam = { new SqlParameter("@in_PublicVideo_ID", videoid) };
            clsCommonFunctions objcommon = new clsCommonFunctions();
            objcommon.AddInParameters(sqlparam);
            DataSet dsRelatedarticles = new DataSet();
            dsRelatedarticles = objcommon.GetDataSet("Help_dbo.st_PublicVideo_GetRelatedVideos");
            ViewBag.relatedlinks = null;
            ViewBag.relatedlinks = dsRelatedarticles.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = 0;
            ViewBag.relatedlinkcount = dsRelatedarticles.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                var clscustomexc = new clsExceptionLog();
                clscustomexc.LogException(ex, "LearnExplorerController", "BindRelatedpublicvideos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void getVideoIDs()
        {
           try{
                clsCommonFunctions objcommon = new clsCommonFunctions();
                TempData["tabind"] = null;
                string strSiteStatisticID = "";
                ViewBag.rbomost = "1";
                if ((Convert.ToInt32(Request["rbomost"]) == 2))
                {
                    ViewBag.rbomost = "2";
                    TempData["tabind"] = "1";
                    strSiteStatisticID = "20";
                }
                else if ((Convert.ToInt32(Request["rbomost"]) == 3))
                {
                    ViewBag.rbomost = "3";
                    TempData["tabind"] = "1";
                    strSiteStatisticID = "21";
                }
                // Dim objparam() As IDataParameter = {New SqlParameter("@in_Statistic_ID", IIf(strSiteStatisticID <> "", strSiteStatisticID, Nothing))}
                DataSet dsIDs = new DataSet();
                // objcommon.AddInParameters(objparam)
                // dsIDs = objcommon.GetDataSet("cogadmin_dbo.st_Public_List_VideosViewdCount")
                string title = "";
                string Desc = "";
                if (Convert.ToInt32(Request["ddlsearch"]) == 0)
                {
                    if (!string.IsNullOrEmpty(Request["txtsearch"]))
                    {
                        title = Convert.ToString(Request["txtsearch"]);
                        Desc = null;

                    }
                    else
                    {
                        title = null;
                        Desc = null;
                    }
                }
                else if (Convert.ToInt32(Request["ddlsearch"]) == 1)
                {
                    if (!string.IsNullOrEmpty(Request["txtsearch"]))
                    {
                        title = null;
                        Desc = Convert.ToString(Request["txtsearch"]);
                       
                    }
                    else
                    {

                        title = null;
                        Desc = null;
                    }
                }
                else if (Convert.ToInt32(Request["ddlsearch"]) == 2)
                {
                    if (!string.IsNullOrEmpty(Request["txtsearch"]))
                    {
                        title =Convert.ToString( Request["txtsearch"]);
                        Desc = Convert.ToString(Request["txtsearch"]);
                       
                    }
                    else
                    {
                        title = null;
                        Desc = null;
                    }
                }
                ViewBag.Vids = null;
                dsIDs = Articlevideos.GetArticlesORVideosCount(strSiteStatisticID, "V", title, Desc);
                if (!(dsIDs == null))
                {
                    if ((dsIDs.Tables.Count > 0))
                    {
                        if ((dsIDs.Tables[0].Rows.Count > 0))
                        {
                            for (int i = 0; (i
                                        <= (dsIDs.Tables[0].Rows.Count - 1)); i++)
                            {
                                if (Convert.ToString(dsIDs.Tables[0].Rows[i]["Article_ID"])!="")
                                {
                                    ViewBag.Vids = ViewBag.Vids + Convert.ToString(dsIDs.Tables[0].Rows[i]["Article_ID"]) + "," + Convert.ToString(dsIDs.Tables[0].Rows[i]["MaxValue"]) + ",$";
                                    //(dsIDs.Tables[0].Rows[i]["Article_ID"] + (","
                                    //            + (dsIDs.Tables[0].Rows[i].Item["MaxValue"] + ",$")));
                                }
                            }
                        }
                    }
                }
                TempData["Vids"] = ViewBag.Vids;
           }
           catch (Exception ex)
           {
               var clscustomexc = new clsExceptionLog();
               clscustomexc.LogException(ex, "LearnExplorerController", "getVideoIDs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
           }
        }
        public ActionResult SendVideo(int vid)
        {
            //if (Request.IsAjaxRequest() && Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.videoid = vid;
            BindSingleVideo(vid);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SendVideo(Publicfaq obj)
        {
            if (Request.IsAjaxRequest() && Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("expire"), JsonRequestBehavior.DenyGet);
            }
            if (Request["txtimgKey2"] != Convert.ToString(HttpContext.Session["captchastring"]))
            {
                return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code to register"), JsonRequestBehavior.DenyGet);
            }
            obj.Question_id = Request["hdnvid"];
            if (Request.Url != null) Requesturl = Convert.ToString(Request.Url);
            ViewBag.url = Requesturl;
            var strUrl = Requesturl.Split('/');
            Requesturl = strUrl[0] + "//" + strUrl[2] + "/" + strUrl[3] + "/" + "Video?videoid=" + Request["hdnvid"];
            string categorycount = Convert.ToString(Category.EmailMsgcount(2));
            var objcategory = new Category
            {
                EmailCategoryCount = Convert.ToInt32(categorycount),
                EmailCategoryID = "3",
                FromReference_id = null,
                Toreference_id = null,
                ISWebService_IND = "N",
                Application_Id = null,
                FromReferenceType_id = null,
                Toreferencetype_id = null
            };
            Category.SentEmailLog(objcategory);
            string strYourName = Request["txtYourName"];
            string strYourEmail = Request["txtYourEmail"];
            string strFriendsEmail = Request["txtFriendsEmail"];
            string strMessage = Sanitizer.GetSafeHtmlFragment(Request["txtMsg"]);
            string strquestion = Request["hdntitle"];

            //var cs = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag") == "YES")
            {
                var objMailMessage = new ClsMailMessage();
                string str = "<b> Sender Name : </b>" + strYourName + "<br />";
                str = str + "  <b> Message : </b>" + strMessage + "<br />";
                str = str + "  <b> Article Title : </b>" + strquestion + "<br />";
                str = str + "  <b> Article URL : </b> " + Requesturl + "<br />";
                str = str + "  2-" + (categorycount + 1);
                bool ress = objMailMessage.SendMail(strFriendsEmail, strYourEmail, "Video Details", str, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (ress == true)
                {
                    UpdArticlestatistics(Convert.ToInt32(Request["hdnvid"]), 21);
                    ViewBag.ress = true;

                    obj.msg = "Your request will be processed soon";
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                }

                obj.msg = "Due to some technical problems your request will not be processed now";
                return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

            }

            obj.msg = "Your request will be processed soon";
            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

        }
        public ActionResult ArticlesIndex()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            DataSet dsindex = new DataSet();
            List<Articlevideos> catlist = new List<Articlevideos>();
            string strSearchCondition=null;
            string strText=null;
            dsindex = Articlevideos.list_ArticlesCategories(strSearchCondition, strText);
            ViewBag.category = null;
            ViewBag.category = dsindex.Tables[0].AsEnumerable();
            ViewBag.categorycnt = 0;
            ViewBag.categorycnt = dsindex.Tables[0].Rows.Count;
         
            return View();
        }
        public ActionResult ArticlesIndex1(int inid, string page=null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            List<Referrals> list = new List<Referrals>();
            Referrals objindex = new Referrals();
            string strSearchCondition = null;
            string strText = null;
            ViewBag.ddlsearch = Request["ddlsearch"];
            ViewBag.txtsearch = Request["txtsearch"];
                if (!string.IsNullOrEmpty(Request["txtsearch"]))
                {
                    strSearchCondition = (string)Request["ddlsearch"];
                    strText = (string)Request["txtsearch"];
                }
                else
                {
                    strSearchCondition = null;
                    strText = null;
                }
                ViewBag.inid = inid;
                objindex.Index_Id = inid;
                string pagNum = page != null ? page : "1";
                ViewBag.Pagenation = pagNum;
                objindex.PageNo = pagNum;
                objindex.NoofRecords = 10;
            int Total_Records = 0;
            list = Referrals.ListBindArticles(strSearchCondition, strText, objindex, ref Total_Records);
            Referrals obj1 = new Referrals();
            ViewBag.articles = null;
            if (list.Count > 0)
            {
                ViewBag.articles = list;
                GetDataListPaging(ref Total_Records);
            }
            ViewBag.norec = Total_Records;
            ViewBag.articlescnt = list.Count;

            return View();
        }
        public ActionResult IndexArticle(int arid, int inid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.inid = inid;
            ViewBag.arid = arid;
            UpdArticlestatistics(arid, 18);
            BindIndexArticle(arid, inid);
            BindRelatedarticles(arid);
            BindRelatedvideos(arid);
            return View();
        }
        public void BindIndexArticle(int articleid, int Index_Id)
        {
           try
           {
            IDataParameter[] sqlparam = {
			new SqlParameter("@In_SearchCond", null),
			new SqlParameter("@In_Title", null),
            new SqlParameter("@in_noofrowsperpage",10),
				new SqlParameter("@in_pageno", 1),
			new SqlParameter("@in_Index_ID", Index_Id),
			new SqlParameter("@in_Article_ID", articleid)
		};
           
            clsCommonFunctions objcommon = new clsCommonFunctions();
            objcommon.AddInParameters(sqlparam);
            DataSet ds = new DataSet();
            ds = objcommon.GetDataSet("Help_dbo.st_Public_Get_IndexArticles");
            if ((ds != null))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Article_Desc"]) != "")
                        {
                            ViewBag.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["Article_Desc"]);
                        }
                      
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Title"]) != "")
                        {
                            ViewBag.Title = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);

                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Createdon"]) != "")
                        {
                            ViewBag.Date = Convert.ToString(ds.Tables[0].Rows[0]["Createdon"]);
                        }

                        if (Convert.ToString(ds.Tables[0].Rows[0]["imagepath"]) != "")
                        {
                            ViewBag.path = Convert.ToString(ds.Tables[0].Rows[0]["imagepath"]);
                        }
                       
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Provider_Name"]) != "")
                        {
                            ViewBag.strprovider = Convert.ToString(ds.Tables[0].Rows[0]["Provider_Name"]);
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["PublicURL"]) != "")
                        {
                            ViewBag.strPublicUrl = Convert.ToString(ds.Tables[0].Rows[0]["PublicURL"]);
                        }
                        
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Author_Desc"]) != "")
                        {
                            ViewBag.AuthorFulldesc = Convert.ToString(ds.Tables[0].Rows[0]["Author_Desc"]);
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["Category_Title"]) != "")
                        {
                            ViewBag.Category_Title = Convert.ToString(ds.Tables[0].Rows[0]["Category_Title"]);
                        }
                        ViewBag.datestrng = DateTime.Parse(ViewBag.Date).DayOfWeek + ", " + DateTime.Parse(ViewBag.Date).ToString("dd MMMM yyyy");
                    }
                }
            }
               }
           catch (Exception ex)
           {
               var clscustomexc = new clsExceptionLog();
               clscustomexc.LogException(ex, "LearnExplorerController", "BindIndexArticle", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
           }
        }

    }
}
