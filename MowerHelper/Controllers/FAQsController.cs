using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class FAQsController : Controller
    {
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProviderFaqs()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            Session.Add("TopIndex", 0);
            if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
            {

                return RedirectToAction("Schedulespecs", "Schedule");

            }
            else if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("Schedulespecs", "Schedule");
            }
            Publicfaq obj = new Publicfaq();
            DataSet Lst = new DataSet();
            List<Publicfaq> Lst1 = new List<Publicfaq>();
            obj.role_id = Convert.ToInt32(Session["RoleId"]);
            obj.Searchby = null;
            obj.Category_Id = null;
            obj.OrderBy = "DESC";
            obj.OrderByItem = "ViewedCount";
            List<Publicfaq> faqlist = new List<Publicfaq>();
            faqlist = Publicfaq.GetTopFaqs(obj);
            ViewBag.Faques = null;
            ViewBag.Faques = faqlist;
            ViewBag.faqscount = faqlist.Count;
            Lst1 = Publicfaq.GetFaqs(obj);
            ViewBag.recentfaqs = null;
            ViewBag.recentfaqs = Lst1;
            ViewBag.recentfaqscnt = Lst1.Count;
            List<Publicfaq> catlist = new List<Publicfaq>();
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            catlist = Publicfaq.BindCategory("15");
            if (catlist.Count > 0)
            {
                for (int i = 0; i < catlist.Count; i++)
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = Convert.ToString(catlist[i].Category_Name),
                        Value = Convert.ToString(catlist[i].Category_Id),
                        Selected = false
                    });
                }
            }
            obj = new Publicfaq { CategoryList = _result2 };
            return View("Index", obj);
            
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult ProviderFaqs(string id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string lblcategory = "";
            if (Request["Category"] != null)
            {
                lblcategory = Request["Category"];
            }
            else
            {
                lblcategory = null;
            }
            ViewBag.category = lblcategory;
            List<Publicfaq> Lst = new List<Publicfaq>();
            List<Publicfaq> Lst1 = new List<Publicfaq>();
            Publicfaq obj = new Publicfaq();
            obj.role_id = Convert.ToInt32(Session["RoleId"]); ;
            obj.Searchby = Request["txtSearch"];
            ViewBag.search = obj.Searchby;
            obj.Category_Id = lblcategory;
            if (Request["rbomost"] == null)
            {
                obj.OrderBy = "DESC";
                obj.OrderByItem = "ViewedCount";
            }

            if (Request["rbomost"] == "V")
            {
                obj.OrderBy = "DESC";
                obj.OrderByItem = "ViewedCount";
            }
            else if (Request["rbomost"] == "R")
            {
                obj.OrderBy = "DESC";
                obj.OrderByItem = "Rate_value";
            }
            ViewBag.rbomost = Request["rbomost"];
            Lst = Publicfaq.GetTopFaqs(obj);
            ViewBag.Faques = null;
            ViewBag.Faques = Lst;
            ViewBag.faqscount = Lst.Count;
            Lst1 = Publicfaq.GetFaqs(obj);
            ViewBag.recentfaqs = null;
            ViewBag.recentfaqs = Lst1;
            ViewBag.recentfaqscnt = Lst1.Count;
            List<Publicfaq> catlist = new List<Publicfaq>();
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            catlist = Publicfaq.BindCategory("15");
            if (catlist.Count > 0)
            {
                for (int i = 0; i < catlist.Count; i++)
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = Convert.ToString(catlist[i].Category_Name),
                        Value = Convert.ToString(catlist[i].Category_Id),
                        Selected = false
                    });
                }
            }
            obj = new Publicfaq { CategoryList = _result2 };
            return View("Index", obj);
        }
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SubmitFAQ()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq obj = new Publicfaq();
            List<Publicfaq> catlist = new List<Publicfaq>();
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            catlist = Publicfaq.BindCategory("15");
            if (catlist.Count > 0)
            {
                for (int i = 0; i < catlist.Count; i++)
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = Convert.ToString(catlist[i].Category_Name),
                        Value = Convert.ToString(catlist[i].Category_Id),
                        Selected = false
                    });
                }
            }
            obj = new Publicfaq { CategoryList = _result2 };
            return View("SubmitFAQ", obj);

        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFAQ(string id1)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string strdomain=null;
            string url = Convert.ToString(Request.Url);
            string[] strUrl = url.Split('/');
            string categoryid="," + Convert.ToString(Request["Category1"]) + ",";
            //clsWebConfigsettings ct = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                strdomain = strUrl[0] + "//" + strUrl[2] + "/" + strUrl[3] + "/";
            }
            else
            {
                strdomain = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl");
            }
            Publicfaq insertQuestions = new Publicfaq();
            insertQuestions.From_Roleid = 4;
            insertQuestions.To_Roleid = 1;
            insertQuestions.Category_Id = categoryid;
            insertQuestions.Username = null;
            insertQuestions.Email = null;
            insertQuestions.Question = Convert.ToString(Request["txtQuestion12"]);
            insertQuestions.Status_Ind = "In Active";
            insertQuestions.LinkUrl = (!string.IsNullOrEmpty(strdomain) ? strdomain : null);
            insertQuestions.CreatedBy = Convert.ToString(Session["UserId"]);
            if (Convert.ToString(Session["Roleid"]) == "4")
            {
                insertQuestions.Provider_Ind = "Y";
            }
            else
            {
                insertQuestions.Provider_Ind = "N";
            }
            if (Convert.ToString(Session["Roleid"]) == "31" || Convert.ToString(Session["Roleid"]) == "38")
            {
                insertQuestions.verified_Ind = "Y";
            }
            else
            {
                insertQuestions.verified_Ind = "N";
            }
            Publicfaq.InsertQuestion(insertQuestions);

            return RedirectToAction("ProviderFaqs", "FAQs", null);
        }
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AnswerFAQ(string id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.QUEid = id;
            string Qid = null;
            if (id != null)
            {
                Qid = id;
            }
            else
            {
                Qid = null;
            }
            GetAnswer(Qid);
            Referencelinks(Qid);
            BindRelatedfaqs(Qid);
        return View();
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerFAQ()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Publicfaq obj = new Publicfaq();
            int rate = Convert.ToInt32(Request["myRadio"]) + 1;
            obj.Ratevalue = rate;
            obj.Comment = (!string.IsNullOrEmpty(Convert.ToString(Request["txtMessage"])) ? Request["txtMessage"].ToString() : null);
            obj.Question_id = Convert.ToString(Request["hdnquid"]);
            if (Session["Roleid"] != null)
            {
                obj.role_id = Convert.ToInt32(Session["Roleid"]);
            }
            else
            {
                obj.role_id = null;
            }
            Publicfaq.InsertComment(obj);
            return RedirectToAction("ProviderFaqs", "FAQs", null);
        }
        public void GetAnswer(string Questionid)
        {
            try{
            Publicfaq obj = new Publicfaq();

            List<Publicfaq> objlist = new List<Publicfaq>();
            obj.Question_id = Questionid;
            objlist = Publicfaq.GetAnswer(obj);

            if (objlist.Count >0)
            {

                if (objlist[0].Question_id != null)
                {
                    ViewBag.QUEid = objlist[0].Question_id;
                }
                else
                {
                    ViewBag.QUEid = null;
                }
                if (objlist[0].Question != null)
                {
                    ViewBag.Text = objlist[0].Question;
                    if (ViewBag.Text.EndsWith("?") == false)
                    {
                        ViewBag.Text = ViewBag.Text + " ?";
                    }
                }
                else
                {
                    ViewBag.Text = null;
                }
                if (objlist[0].Avgrating != null)
                {
                    if (objlist[0].Avgrating.Contains("."))
                    {
                        objlist[0].Avgrating = objlist[0].Avgrating.Substring(0, 3);
                        string[] strs = objlist[0].Avgrating.Split('.');
                        if ((strs[1]) == "0")
                        {
                            objlist[0].Avgrating = strs[0];
                        }
                    }
                    ViewBag.rating = "(Rating : " + objlist[0].Avgrating + "/5 )";
                }
                if (objlist[0].ViewedCount != null)
                {
                    ViewBag.ViewedCount = "(View Count : " + objlist[0].ViewedCount + ")";
                }
                if (objlist[0].UpdatedOn != null)
                {
                    ViewBag.updatedon = "(UpdatedOn : " + Convert.ToDateTime(objlist[0].UpdatedOn).Date + ")";
                }
                if (objlist[0].Answer != null)
                {
                    ViewBag.answer = objlist[0].Answer;
                }
                else
                {
                    ViewBag.answer = null;
                }
            }
             }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "FAQsController", "GetAnswer", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        public void Referencelinks(string Questionid)
        {
            try{
            Publicfaq obj = new Publicfaq();
            obj.Question_id = Questionid;
            DataSet returnset = new DataSet();
            returnset = Publicfaq.GetRelatedLink(Questionid);
            ViewBag.relatedlinks = null;
            ViewBag.relatedlinks = returnset.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = 0;
            ViewBag.relatedlinkcount = returnset.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "FAQsController", "Referencelinks", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void BindRelatedfaqs(string QuestionID)
        {
            try
            {
            DataSet dsfaq = new DataSet();
            Publicfaq objlist = new Publicfaq();
            objlist.Question_id = QuestionID;
            objlist.public_ind = "Y";
            objlist.display_Ind = null;
            dsfaq = Publicfaq.Get_Relatedfaqs(objlist);
            ViewBag.relatedfaqs = null;
            ViewBag.relatedfaqs = dsfaq.Tables[0].AsEnumerable();
            ViewBag.relatedfaqcount = 0;
            ViewBag.relatedfaqcount = dsfaq.Tables[0].Rows.Count;
                 }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "FAQsController", "BindRelatedfaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult download2(string id = null, string ext = null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.relatedlink = "../../Attachments/Media/" + id;
            ViewBag.ext = ext;
            return View();
        }
    }
}
