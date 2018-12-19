using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    [App_Start.NotRequireHttps]
    public class SearchController : Controller
    {
        public string ObjKeyGen = "";
        public string Requesturl;
        string strOutmsg = "";
        public string Lattitude{get;set;}
       
        public string Longitude{get;set;}
      
        public string ZipValue{get;set;}
        
        public string IsSearch{get;set;}
       
        public int CityId{get;set;}
       
        public int StateId{get;set;}
        
        public string StateName{get;set;}
       
        public string CityName{get;set;}
       
        public int CountryId { get; set; }
      
        public int Distance{get;set;}

        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Faqs()
        {
            string lblcategory = Request["Category"] ?? null;
            ViewBag.category = lblcategory;
            var obj = new Publicfaq
            {
                role_id = 17,
                Searchby = null,
                Category_Id = lblcategory,
                OrderBy = "DESC",
                OrderByItem = "ViewedCount"
            };

            //var lst = Publicfaq.GetTopFaqs(obj);
            // ViewBag.Faques = null;
            // ViewBag.Faques = lst;
            //  ViewBag.faquescount = lst.Count;
            var lst1 = Publicfaq.GetFaqs(obj);
            ViewBag.recentfaqs = null;
            ViewBag.recentfaqs = lst1;
            ViewBag.recentfaqscnt = lst1.Count;
            IList<SelectListItem> result2 = new List<SelectListItem>();
            var catlist = Publicfaq.BindCategory("17");
            //if (catlist.Count > 0)
            //{
            //    foreach (var t in catlist)
            //    {
            //        result2.Add(new SelectListItem
            //        {
            //            Text = t.Category_Name,
            //            Value = t.Category_Id,
            //            Selected = false
            //        });
            //    }
            //}
            ViewBag.categlist = null;
            ViewBag.categlist = catlist;


            //  obj = new Publicfaq { CategoryList = result2 };
            return View("FAQs", obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Faqs(string id)
        {
            TempData["imgind"] = "1";

            var obj = new Publicfaq { role_id = 17, Searchby = Request["txtSearch"] };
            ViewBag.search = obj.Searchby;
            // obj.Category_Id = lblcategory;
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
            //  ViewBag.rbomost = Request["rbomost"];
            // var lst = Publicfaq.GetTopFaqs(obj);
            // ViewBag.Faques = null;
            //  ViewBag.Faques = lst;
            //  ViewBag.faquescount = lst.Count;
            var lst1 = Publicfaq.GetFaqs(obj);
            ViewBag.recentfaqs = null;
            ViewBag.recentfaqs = lst1;
            ViewBag.recentfaqscnt = lst1.Count;
            IList<SelectListItem> result2 = new List<SelectListItem>();
            var catlist = Publicfaq.BindCategory("17");
            ViewBag.categlist = null;
            ViewBag.categlist = catlist;
            //if (catlist.Count > 0)
            //{
            //    foreach (var t in catlist)
            //    {
            //        result2.Add(new SelectListItem
            //        {
            //            Text = t.Category_Name,
            //            Value = t.Category_Id,
            //            Selected = false
            //        });
            //    }
            //}
            //obj = new Publicfaq { CategoryList = result2 };
            return View("FAQs", obj);
        }

        public ActionResult SubmitFaq()
        {
            IList<SelectListItem> result2 = new List<SelectListItem>();
            List<Publicfaq> catlist = Publicfaq.BindCategory("17");
            if (catlist.Count > 0)
            {
                foreach (var t in catlist)
                {
                    result2.Add(new SelectListItem
                    {
                        Text = t.Category_Name,
                        Value = t.Category_Id,
                        Selected = false
                    });
                }
            }
          var  obj = new Publicfaq { CategoryList = result2 };
            return View("SubmitFAQ", obj);

        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SubmitFaq(Publicfaq faq)
        {
            if (Request["txtimgKey"] != Convert.ToString(HttpContext.Session["captchastring"]))
            {
                return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code"), JsonRequestBehavior.DenyGet);
            }
            var insertQuestions = new Publicfaq
            {
                From_Roleid = 17,
                To_Roleid = 1,
                Category_Id = (!string.IsNullOrEmpty(Request["Category"]) ? Request["Category"] : null),
                Username = (!string.IsNullOrEmpty(Request["txtName"]) ? Request["txtName"] : null),
                Email = (!string.IsNullOrEmpty(Request["txtEmail"]) ? Request["txtEmail"] : null),
                Question = Sanitizer.GetSafeHtmlFragment(!string.IsNullOrEmpty(Request["txtMessage"]) ? Request["txtMessage"] : null),
                Status_Ind = "In Active",
                public_ind = "Y",
                CreatedBy = "17",
                LinkUrl = null
            };
            Publicfaq.InsertQuestion(insertQuestions);
            return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
        }
        public ActionResult _FeaturedTherapists()
        {
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                   
                    ViewBag.img12ind = strurl[0] + "//" + strurl[2] + "/" + "images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "images/speacer.gif";
                }
                else if (Requesturl.Contains("vbv"))
                {
   
                    ViewBag.img12ind = strurl[0] + "//" + strurl[2] + "/" +strurl[3] +"/"+ "images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/speacer.gif";
                }
            }
            else
            {
                ViewBag.img12ind = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/line-bg.gif";
                ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/speacer.gif";
            }
           
            try
            {
                if (ZipValue == null & StateName == null & CityName == null & StateId == 0 & CityId == 0)
                {
                    if ((Request.Cookies["State"] != null))
                    {
                        if (Request.Cookies["State"].Value != null)
                        {
                            StateName = Request.Cookies["State"].Value.ToString().Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["City"] != null))
                    {
                        if (Request.Cookies["City"].Value != null)
                        {
                            CityName = Request.Cookies["City"].Value.ToString().Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["Latitude"] != null))
                    {
                        if (Request.Cookies["Latitude"].Value != null)
                        {
                            Lattitude = Request.Cookies["Latitude"].Value.ToString().Replace("%20", " ");
                        }
                    }
                    if ((Request.Cookies["Longitude"] != null))
                    {
                        if (Request.Cookies["Longitude"].Value != null)
                        {
                            Longitude = Request.Cookies["Longitude"].Value.ToString().Replace("%20", " ");
                        }
                    }
                }
                var objData = new Provider_AdvancedSearch
                {
                    Zip = ZipValue,
                    City_ID = CityId,
                    State_ID = StateId,
                    StateName = StateName,
                    CityName = CityName,
                    Country_ID = CountryId,
                    Distance = Distance,
                    Longitude = Longitude,
                    Lattitude = Lattitude
                };
                if (ZipValue != null | CityId != 0 | CityName != null | StateName != null | StateId != 0 | CountryId != 0 | Distance != 0)
                {
                    objData.IsSearch = "Y";
                }
                else
                {
                    objData.IsSearch = null;
                }
                var objlist = Provider_AdvancedSearch.list_Featuredtherapists(objData);
                if (objlist.Tables[0].Rows.Count > 0)
                {
                    ViewBag.FeaturedList = objlist.Tables[0].AsEnumerable();
                    ViewBag.FeaturedListcount = objlist.Tables[0].Rows.Count;
                }
                else
                {
                    ViewBag.FeaturedList = null;
                }
                //if (objlist.Tables[0].Rows.Count > 0)
                //{
                //    int cnt;
                //    for (cnt = 0; cnt <= objlist.Tables[0].Rows.Count - 1; cnt++)
                //    {

                //        ProviderAdvertising objproviderAdvertise = new ProviderAdvertising();
                //        objproviderAdvertise.SiteStatic_ID = "2";
                //        objproviderAdvertise.State_Name = StateName;
                //        objproviderAdvertise.CityNm = CityName;
                //        objproviderAdvertise.Provider_ID = objlist.Tables[0].Rows[cnt]["Provider_ID"].ToString();
                //        objproviderAdvertise.CountingProviderVisitToHisProfile(objproviderAdvertise);

                //    }
                //}
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "SearchController", "_FeaturedTherapists", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            
            return View();

        }
        public ActionResult AnswerFaq(string id = null)
        {
            TempData["ind1"] = "1";
            GetAnswer(id);
            Referencelinks(id);
            BindRelatedfaqs(id);
            ViewBag.quesid = id;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult AnswerFaq()
        {
            if (!string.IsNullOrEmpty(Request["txtMessage"]))
            {
                if (Request["txtimgKey1"] != Convert.ToString(HttpContext.Session["captchastringValue"]))
                {
                    return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code"), JsonRequestBehavior.DenyGet);
                }
            }
            var obj = new Publicfaq();
            obj.Ratevalue = !string.IsNullOrEmpty(Request["hdnchk"]) ? Convert.ToInt32(Request["hdnchk"]) : 0;
            obj.Comment = Sanitizer.GetSafeHtmlFragment(!string.IsNullOrEmpty(Request["txtMessage"]) ? Request["txtMessage"] : null);
            obj.Question_id = Request["Hdnqid"];
            obj.role_id = null;
            Publicfaq.InsertComment(obj);
            return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
        }
        public Captchaimg ShowCaptchaimg()
        {
            return new Captchaimg();
        }

        public void GetAnswer(string questionid)
        {
            try
            {
            var obj = new Publicfaq {Question_id = questionid};
            var objlist = Publicfaq.GetAnswer(obj);
           
            if (objlist.Count >0)
            {
               
                ViewBag.QUEid = objlist[0].Question_id ?? null;
                if (objlist[0].Question !=null)
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
                if (objlist[0].Avgrating!=null)
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
                if (objlist[0].ViewedCount!=null)
                {
                    ViewBag.ViewedCount = "(View Count : " + objlist[0].ViewedCount + ")";
                }
                if (objlist[0].UpdatedOn!=null)
                {
                    ViewBag.updatedon = "(UpdatedOn : " + Convert.ToDateTime(objlist[0].UpdatedOn).Date + ")";
                }
                ViewBag.answer = objlist[0].Answer ?? null;
            }
              }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "GetAnswer", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void Referencelinks(string questionid)
        {
            try{
            var returnset = Publicfaq.GetRelatedLink(questionid);
            ViewBag.relatedlinks = null;
            ViewBag.relatedlinks = returnset.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = 0;
            ViewBag.relatedlinkcount = returnset.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "Referencelinks", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void BindRelatedfaqs(string questionId)
        {
            try
            {
            var objlist = new Publicfaq {Question_id = questionId, public_ind = "Y", display_Ind = null};
            var dsfaq = Publicfaq.Get_Relatedfaqs(objlist);
            ViewBag.relatedfaqs = null;
            ViewBag.relatedfaqs = dsfaq.Tables[0].AsEnumerable();
            ViewBag.relatedfaqcount = 0;
            ViewBag.relatedfaqcount = dsfaq.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "BindRelatedfaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SendFaq(string id=null)
        {
             ViewBag.qid=id;
            GetAnswer(id);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SendFaq(Publicfaq obj)
        {
            if (Request["txtimgKey2"] != Convert.ToString(HttpContext.Session["captchastring"]))
            {
                return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code to register"), JsonRequestBehavior.DenyGet);
            }
            obj.Question_id = Request["hdnqid"];
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            ViewBag.url = Requesturl;
            var strUrl = Requesturl.Split('/');
            Requesturl = strUrl[0] + "//" + strUrl[2] + "/" + strUrl[3] + "/" + "AnswerFAQ/" + Request["hdnqid"];
            string categorycount = Convert.ToString(Category.EmailMsgcount(7));
            var objcategory = new Category
            {
                EmailCategoryCount = Convert.ToInt32(categorycount),
                EmailCategoryID = "7",
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
            string strMessage =Sanitizer.GetSafeHtmlFragment(Request["txtMsg"]);
            string strquestion = Request["hdnquestion"];
           // string strFrommailid = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
            //var clsWebConfigsettings = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {
                var objMailMessage = new ClsMailMessage();
                string str = "<b> Sender Name : </b>" + strYourName + "<br />";
                str = str + "  <b> Message : </b>" + strMessage + "<br />";
                str = str + "  <b> Question : </b>" + strquestion + "<br />";
                str = str + "  <b> Question URL : </b> " + Requesturl + "<br />";
                str = str + "  7-" + (categorycount + 1);
                bool ress = objMailMessage.SendMail(strFriendsEmail, strYourEmail, "Qusetion Details", str, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (ress == true)
                {
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
        public JsonResult Zipcodes(string term)
        {
            var objlist = new List<string>();
            var objcom = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Keyword", term) };
            objcom.AddInParameters(objparam);
            SqlDataReader drlist;
            int zipcode;
            var result = int.TryParse(term, out zipcode);
            if (result == true)
            {
                drlist = objcom.GetDataReader("Help_dbo.st_Public_Typeahead_Zipcode_1");
                while (drlist.Read())
                {
                    objlist.Add(Convert.ToString(drlist[0]));
                }
            }
            else
            {
                drlist = objcom.GetDataReader("Help_dbo.st_Public_Typeahead_Cities");
                while (drlist.Read())
                {
                    objlist.Add(Convert.ToString(drlist[0]));
                }
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult download2(string id = null, string ext = null)
        {
            ViewBag.relatedlink = "../../Attachments/Media/" + id;
            ViewBag.ext = ext;
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ListOfProviders(string id, string city, string page, string stateid = null, string stateNm = null, string cityNm = null, string countryid = null, string countryname = null, string ctyid = null, string distance = null, string distance1 = null)
        {
            
            var obj = new ProviderAdvertising();
         var objDataList = new List<ProviderAdvertising>();
            int countryId;
            if (id == null & city == null)
            {
                ViewBag.textvalue = "US City or ZIP Code";
            }
            if (Request.Url != null) Requesturl = Convert.ToString(Request.Url);
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    ViewBag.Imageurl = strurl[0] + "//" + strurl[2] + "/";
                    ViewBag.citylistimg = strurl[0] + "//" + strurl[2] + "/" + "images/links-bg2.jpg";
                    ViewBag.leftimg = strurl[0] + "//" + strurl[2] + "/" + "Images/tdsbg-left.gif";
                    ViewBag.rightimg = strurl[0] + "//" + strurl[2] + "/" + "Images/tdsbg-right.gif";
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "images/speacer.gif";
                }
                else if (Requesturl.Contains("vbv"))
                {
                    ViewBag.citylistimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/links-bg2.jpg";
                    ViewBag.leftimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/tdsbg-left.gif";
                    ViewBag.rightimg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/tdsbg-right.gif";
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/speacer.gif";
                }

            }
            else
            {
                ViewBag.citylistimg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/links-bg2.jpg";
                ViewBag.leftimg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/tdsbg-left.gif";
                ViewBag.rightimg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/tdsbg-right.gif";
                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/speacer.gif";

            }
            if (id != null & id != "US City or ZIP Code")
            {
                ViewBag.id = id;
                ViewBag.spaceline = "";
            }
            else
            {
                ViewBag.id = null;
            }
            if (city != null)
            {
                ViewBag.city = city.Replace("-", " ");
                ViewBag.city1 = city.ToUpper() == "BLAIRSDEN-GRAEAGLE" ? city : city.Replace("-", " ");
            }
            else
            {
                ViewBag.mycity = null;
                ViewBag.city1 = null;
            }
            if (countryname!= null)
            {
                countryId = 1;
                ViewBag.countryid = 1;
            }
            else
            {
                countryId = 0;
                ViewBag.countryid = null;
            }
            if (stateid != null)
            {
                ViewBag.stateid12 = stateid;
                ViewBag.stateid = stateid;
            }
            else
            {
                ViewBag.stateid12 = null;
            }
            if (stateNm != null)
            {
                if (stateNm.Contains("-"))
                {
                    stateNm = stateNm.Replace("-", " ");
                }
                ViewBag.StateNm = stateNm;
                ViewBag.statename1 = stateNm;
            }
            else
            {
                ViewBag.StateNm = null;
            }
            ViewBag.CityNm = cityNm ?? null;
            ViewBag.countryname = countryname ?? null;
            string zipcode = id;
            string strstate = "";
            int totalNoofRecords = 0;
          int  pagNum = page != null ? Convert.ToInt32(page) : 1;
            ViewBag.Pagenation = pagNum;
            if (distance != null)
            {
                Distance = Convert.ToInt32(distance);
                ViewBag.distance = distance;
                ViewBag.Radius = distance;
            }
            else if (distance1 != null & distance1 != "")
            {
                Distance = Convert.ToInt32(distance1);
                ViewBag.distance = distance1;
                ViewBag.Radius = distance1;
            }
            else
            {
                Distance = 0;
                ViewBag.distance = "0";
                ViewBag.Radius = "0";
            }
            if (Request.QueryString["InActive"] != null)
            {
                ViewBag.text = "Sorry, the Mower Helper you requested does not exist. Please search with other criteria.";
            }


            if (id != null & city != null)
            {
                if (city.Contains("--"))
                {
                    city = city.Replace("--", "/ ").Replace("-", " ");
                }
                else if (city.Contains("-") & city.ToUpper()!= "BLAIRSDEN-GRAEAGLE")
                {
                    city = city.Replace("-", " ");
                }
                ViewBag.textvalue = city;
                ViewBag.CityId = ctyid ?? null;
                string zipCode;
                int cityId = Convert.ToInt32(ctyid);
                string strOutmsg = "";
                if (city.Contains("("))
                {
                    ViewBag.textcitynm = city;
                    var strValues = city.Split('(');
                    if (strValues.Length == 2)
                    {
                        city = strValues[0].Trim();
                        strstate = strValues[1].Replace("(", "").Trim();
                        strstate = strstate.Replace(")", "").Trim();
                        ViewBag.cityname123 = city + ", ";
                        ViewBag.textcity = city;
                        ViewBag.cityabname = city;
                        ViewBag.cityname1232 = strstate;
                        ViewBag.statee1 = strstate;
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo=getStatenameFromAbbrevation(strstate);
                        if (objstateinfo.StateName.Contains("-"))
                        {
                            strstate = objstateinfo.StateName.Replace("-", " ");
                        }
                        ViewBag.staenm1 = objstateinfo.StateName;
                        ViewBag.mystateid =objstateinfo.StateId;
                        ViewBag.Radius = "0";
                    }
                }
                else
                {
                 ViewBag.stateAb=obj.GetProviderStateab(stateNm);
                 ViewBag.statee1 = ViewBag.stateAb;
                 ViewBag.cityabname = city;
                 ViewBag.textvalue = city + " " + "(" + ViewBag.stateAb + ")";
                 ViewBag.Radius = "0";
                }
                if (id != null)
                {
                    zipCode = id;
                    ViewBag.zippcode123 = id;
                }
                else
                {
                    zipCode = null;
                }
                    cityId = GetCityID(city, stateNm);
                     objDataList = obj.GeneralListProviders(zipCode,Distance, null, cityId, pagNum, ref totalNoofRecords, ref strOutmsg);

                if (distance != null)
                {
                    ViewBag.text = distance + " miles around postal code";
                }
                else
                {

                    ViewBag.text = "0 miles around postal code";
                }
            }

            else if (!string.IsNullOrEmpty(zipcode))
            {

                ViewBag.zippcode = zipcode;
                string zip = zipcode ?? null;
                     objDataList = obj.GeneralListProviders(zip, Distance, null, 0, pagNum, ref totalNoofRecords, ref strOutmsg);
                    DtListCitiesBind(zipcode, null,Distance);
                GetStatecitywithZip(zipcode);
                
                ViewBag.textvalue = null;
                ViewBag.textvalue = zipcode;
                if (distance != null)
                {
                    ViewBag.text = distance + " miles around postal code";
                }
                else
                {
                    ViewBag.text = "0 miles around postal code";
                }
            }


            else if (city != null )
            {
                if (city.Contains("--"))
                {
                    city = city.Replace("--", "/ ").Replace("-", " ");
                }
                else if (city.Contains("-") & city.ToUpper() != "BLAIRSDEN-GRAEAGLE")
                {
                    city = city.Replace("-", " ");
                }
                string strziptext = city;
                ViewBag.searchcity = city;
                string state1=null;
                if (strziptext.Contains("(") || stateNm != null)
                {
                    ViewBag.textvalue = strziptext;
                    ViewBag.textcity = strziptext;
                    var strValues = strziptext.Split('(');
                    if (strValues.Length == 2)
                    {
                        city = strValues[0].Trim();
                        state1 = strValues[1].Trim();
                        strstate = strValues[1].Replace("(", "").Trim();
                        strstate = strstate.Replace(")", "").Trim();

                        ViewBag.cityname123 = city + ", ";
                        ViewBag.statee1 = strstate;
                         clsCountry objstateinfo = new clsCountry();
                        objstateinfo=getStatenameFromAbbrevation(strstate);
                        ViewBag.mystate234 = objstateinfo.StateName;
                        ViewBag.cityabname = city + ", ";
                        ViewBag.text = "In";
                        if (objstateinfo.StateName.Contains("-"))
                        {
                            strstate = objstateinfo.StateName.Replace("-", " ");
                            ViewBag.mystate234 = objstateinfo.StateName;
                        }
                        ViewBag.staenm1 = objstateinfo.StateName;
                        ViewBag.mystateid = objstateinfo.StateId;
                        ViewBag.stateid = objstateinfo.StateId;
                    }
                    else
                    {
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo = obj.GetProviderStateID(stateNm);
                        ViewBag.statee1 = objstateinfo.StateName;
                        ViewBag.textvalue = strziptext + " " + "(" + ViewBag.statee1 + ")";
                        ViewBag.textcity = strziptext;
                        ViewBag.cityname123 = city + ", ";
                        ViewBag.mystate234 = stateNm;
                        ViewBag.cityabname = city + ", ";
                        ViewBag.text = "In";
                        ViewBag.staenm1 = stateNm;
                        ViewBag.mystateid = objstateinfo.StateId;
                        ViewBag.stateid = objstateinfo.StateId;
                    }
                }
                else
                {

                    ViewBag.cityname01 = city;
                     objDataList = obj.BindStatesList(city);
                    if ((objDataList != null))
                    {
                        if (objDataList.Count > 0)
                        {

                            ViewBag.statecount12 = objDataList.Count;
                            ViewBag.states = objDataList;

                            int rowcount = objDataList.Count / 4;
                            ViewBag.rows = rowcount;
                            ViewBag.count = 1;


                        }
                        else
                        {
                            ViewBag.states = null;
                            ViewBag.count = 0;
                        }
                    }
                    ViewBag.citylist = null;
                    ViewBag.textvalue = strziptext;
                    if (!string.IsNullOrEmpty(state1))
                    {
                        ViewBag.therapistsin = city + "," + "(" + state1;
                    }
                    else
                    {
                        ViewBag.therapistsin = city;
                    }
                    ViewBag.search2 = city + "," + strstate;
                    ViewBag.stateid = null;

                }

                totalNoofRecords = 0;
                string cityNme = city ?? null;
                    objDataList = obj.ListProvidersBasedOnStateOrCountryOrCityIDs(cityNme, stateid, countryId, pagNum, ref totalNoofRecords, ref strOutmsg);


            }
            else if (stateid != null | stateNm != null | countryId != 0)
            {
                ViewBag.stateid = stateid;
                ViewBag.statename1 = stateNm;
                totalNoofRecords = 0;
                string cityNme = cityNm ?? null;
                 if (stateNm != null)
                {
                    clsCountry objstateinfo = new clsCountry();
                    objstateinfo = obj.GetProviderStateID(stateNm);
                    stateid = Convert.ToString(objstateinfo.StateId);
                    ViewBag.stateid = stateid;
                }

                 objDataList = obj.ListProvidersBasedOnStateOrCountryOrCityIDs(cityNm, stateid, countryId, pagNum, ref totalNoofRecords, ref strOutmsg);
                     DtListCitiesBind(zipcode, stateid, null);
               
            }


            else if (zipcode == null & city == null & stateid == null & countryId == 0)
            {
                
                totalNoofRecords = 0;

                 objDataList = obj.GeneralListProviders(zipcode, Distance,null, 0, pagNum, ref totalNoofRecords, ref strOutmsg);
                DtListCitiesBind(zipcode, null,null);
               

            }
        
            if (objDataList.Count > 0)
            {
                int cnt;
                string strproviderids = null;
                for (cnt = 0; cnt <= objDataList.Count - 1; cnt++)
                {
                    if (cnt == 0)
                    {
                        strproviderids = Convert.ToString(objDataList[cnt].ProviderID);
                    }
                    else
                    {
                        strproviderids = strproviderids + "," + Convert.ToString(objDataList[cnt].ProviderID);
                    }
                }
                if (!string.IsNullOrEmpty(strproviderids))
                {
                    strproviderids = strproviderids + ",";
                }

                obj.CountingStateswiseSearch(strproviderids, null, 'Y', StateName, CityName);
            }
            if (objDataList.Count == 0)
            {
                TempData["therapistlist"] = null;
                totalNoofRecords = 0;
                ViewBag.listcount = 0;
                TempData["therapistlistcnt"] = 0;


            }
            else
            {
                TempData["therapistlist"] = objDataList;
                ViewBag.list = objDataList;
                ViewBag.listcount = objDataList.Count;
                TempData["therapistlistcnt"] = objDataList.Count;
                ViewBag.totadd = ProviderAdvertising.fulladdress;
                ViewBag.totadd = Convert.ToString(ProviderAdvertising.fulladdress).Replace('+', ',');
            }
            if (totalNoofRecords >= 0)
            {
                GetDataListPaging(ref totalNoofRecords);
            }
            ViewBag.listing = "[  " + totalNoofRecords + "  listings ]";

            //if (distance != null || distance1 != null)
            //{
            //    ViewBag.imgpath = "../../Images/green-dot.png";

            //}
            //else
            //{
            //    ViewBag.imgpath = "../Images/green-dot.png";
            //}
            string domain = null;
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    domain = strurl[0] + "//" + strurl[2] + "/";

                }
                else if (Requesturl.Contains("vbv"))
                {
                    domain = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/";

                }
            }
            else
            {
                domain = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal");
            }
            ViewBag.imgpath = domain + "Images/green-dot.png";

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Directory(string id)
        {
            if (Request.Url != null) Requesturl = Convert.ToString(Request.Url);
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "Images/raight-heading.gif";
                    ViewBag.lefttdbg = strurl[0] + "//" + strurl[2] + "/" + "Images/tdsbg-left.gif";
                    ViewBag.righttdbg = strurl[0] + "//" + strurl[2] + "/" + "Images/tdsbg-right.gif";
                    ViewBag.uslistingtdbag = strurl[0] + "//" + strurl[2] + "/" + "Images/links-bg.jpg";
                    ViewBag.citytdbg = strurl[0] + "//" + strurl[2] + "/" + "Images/links-bg.jpg";
                    ViewBag.othcitytdbg = strurl[0] + "//" + strurl[2] + "/" + "Images/links-bg.jpg";
                    ViewBag.inttdbg = strurl[0] + "//" + strurl[2] + "/" + "Images/links-bg.jpg";
                }
                else if (Requesturl.Contains("vbv"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/raight-heading.gif";
                    ViewBag.lefttdbg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/tdsbg-left.gif";
                    ViewBag.righttdbg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/tdsbg-right.gif";
                    ViewBag.uslistingtdbag = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/links-bg.jpg";
                    ViewBag.citytdbg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/links-bg.jpg";
                    ViewBag.othcitytdbg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/links-bg.jpg";
                    ViewBag.inttdbg = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Images/links-bg.jpg";
                }
            }
            else
            {
                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/raight-heading.gif";
                ViewBag.lefttdbg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/tdsbg-left.gif";
                ViewBag.righttdbg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/tdsbg-right.gif";
                ViewBag.uslistingtdbag = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/links-bg.jpg";
                ViewBag.citytdbg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/links-bg.jpg";
                ViewBag.othcitytdbg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/links-bg.jpg";
                ViewBag.inttdbg = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "Images/links-bg.jpg";

            }

            ViewBag.textvalue = "US City or ZIP Code";
            int rowcount;
            var obj1 = new StatesList();
            var obj2 = new ProviderAdvertising();
            var objDataList = obj1.GetStatesBasedOnDirectoryID();
            if (objDataList.Count > 0)
            {
                ViewBag.states = objDataList;
                ViewBag.Statescount = objDataList.Count;
                rowcount = objDataList.Count / 4;
                ViewBag.rows = rowcount;
            }
            else
            {
                ViewBag.States = null;
                ViewBag.Statescount = 0;
            }
            var objDataList1 = obj2.GetTopcitisList();
            if (objDataList1.Count > 0)
            {
                ViewBag.cities = objDataList1;
                ViewBag.citiescount = objDataList1.Count;
                rowcount = objDataList1.Count / 4;
                ViewBag.rows1 = rowcount;
            }
            else
            {
                ViewBag.cities = null;
                ViewBag.citiescount = 0;
            }
            var objDataList2 = obj2.GetCountriesBasedOnDirectoryID(null);
            if (objDataList2.Count > 0)
            {
                ViewBag.international = objDataList2;
                ViewBag.internationalcnt = objDataList2.Count;
                rowcount = objDataList2.Count / 4;
                ViewBag.rows2 = rowcount;
            }
            else
            {
                ViewBag.international = null;
                ViewBag.internationalcnt = 0;
            }
            ViewBag.ind = id ?? null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Directory()
        {
            string txtzip;
            string ddlRadius;
            if (Request["txtZip"] != null & Request["txtZip"] != "" & Request["txtZip"] != "US City or ZIP Code")
            {
                txtzip = Request["txtZip"];
                txtzip = txtzip.Trim();
            }
            else
            {
                txtzip = null;
            }
            if (Request["ddlRadius"] != "0")
            {
                ddlRadius = Request["ddlRadius"];
                ViewBag.radius = ddlRadius;
            }
            else
            {
                ddlRadius = null;
            }

            if (string.IsNullOrEmpty(txtzip) & ddlRadius == null)
            {
                RouteData.Values.Clear();
                return RedirectToRoute("DefaultSearch", null);
            }

            if (!string.IsNullOrEmpty(txtzip) & ddlRadius == null)
            {
                string strradius = ValidateZipcode(txtzip);
                if (!string.IsNullOrEmpty(strradius))
                {
                    if (strradius == "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeSearch", new { id = txtzip });
                    }
                    RouteData.Values.Clear();
                    return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = strradius });
                }
                if (txtzip.Contains("("))
                {
                    var strValues = txtzip.Split('(');
                    if (strValues.Length == 2)
                    {
                        string strCity = strValues[0].Trim();
                        string strState = strValues[1].Replace("(", "").Trim();
                        strState = strState.Replace(")", "").Trim();
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo = getStatenameFromAbbrevation(strState);
                        strState = objstateinfo.StateName;
                        if (!string.IsNullOrEmpty(strState) & !string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        if (!string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        RouteData.Values.Clear();
                        return RedirectToAction("Directory", "Search", new { id = "N" });
                    }
                }
                else
                {
                    string strCity = txtzip.Trim();
                    string strState = getStatenameFromCity(strCity);
                    if (!string.IsNullOrEmpty(strState))
                    {
                        RouteData.Values.Clear();
                        strCity = strCity.Replace("/", "-");
                        strCity = strCity.Replace(".", "-");
                        strCity = strCity.Replace("'", "-");
                        return RedirectToRoute("Cities", new { city = strCity.Replace(" ", "-") });
                    }
                    RouteData.Values.Clear();
                    return RedirectToAction("Directory", "Search", new { id = "N" });
                }
            }
            else if (!string.IsNullOrEmpty(txtzip) & ddlRadius != "0")
            {
                string strradius = ValidateZipcode(txtzip);
                if (!string.IsNullOrEmpty(strradius))
                {
                    if (strradius == "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = ddlRadius });
                    }
                    if (ddlRadius != "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = ddlRadius });
                    }
                    RouteData.Values.Clear();
                    return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = strradius });
                }
                if (txtzip.Contains("("))
                {
                    string[] strValues = txtzip.Split('(');
                    if (strValues.Length == 2)
                    {
                        string strCity = strValues[0].Trim();
                        string strState = strValues[1].Replace("(", "").Trim();
                        strState = strState.Replace(")", "").Trim();
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo = getStatenameFromAbbrevation(strState);
                        strState = objstateinfo.StateName;
                        if (!string.IsNullOrEmpty(strState) & !string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        if (!string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        RouteData.Values.Clear();
                        return RedirectToAction("Directory", "Search", new { id = "N" });
                    }
                }
                else
                {
                    string strCity = txtzip.Trim();
                    string strState = getStatenameFromCity(strCity);
                    if (!string.IsNullOrEmpty(strState))
                    {
                        RouteData.Values.Clear();
                        strCity = strCity.Replace("/", "-");
                        strCity = strCity.Replace(".", "-");
                        strCity = strCity.Replace("'", "-");
                        return RedirectToRoute("Cities", new { city = strCity.Replace(" ", "-") });
                    }
                    RouteData.Values.Clear();
                    return RedirectToAction("Directory", "Search", new { id = "N" });
                }
            }
            else if (string.IsNullOrEmpty(txtzip) & ddlRadius != null)
            {
                RouteData.Values.Clear();
                return RedirectToRoute("DefaultSearch", null);
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListOfProviders()
        {
            string txtzip;
            string ddlRadius;
            if (Request["txtZip"] != null & Request["txtZip"] != "" & Request["txtZip"] != "US City or ZIP Code")
            {
                txtzip = Request["txtZip"];
                txtzip = txtzip.Trim();
            }
            else
            {
                txtzip = null;
            }
            if (Request["ddlRadius"] != "0")
            {
                ddlRadius = Request["ddlRadius"];
                ViewBag.radius = ddlRadius;
            }
            else
            {
                ddlRadius = null;
            }

            if (string.IsNullOrEmpty(txtzip) & ddlRadius == null)
            {
                RouteData.Values.Clear();
                return RedirectToRoute("DefaultSearch", null);
            }

            if (!string.IsNullOrEmpty(txtzip) & ddlRadius == null)
            {
                string strradius = ValidateZipcode(txtzip);
                if (!string.IsNullOrEmpty(strradius))
                {
                    if (strradius == "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeSearch", new { id = txtzip });
                    }
                    RouteData.Values.Clear();
                    return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = strradius });
                }
                if (txtzip.Contains("("))
                {
                    var strValues = txtzip.Split('(');
                    if (strValues.Length == 2)
                    {
                        string strCity = strValues[0].Trim();
                        string strState = strValues[1].Replace("(", "").Trim();
                        strState = strState.Replace(")", "").Trim();
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo = getStatenameFromAbbrevation(strState);
                        strState = objstateinfo.StateFullName;
                        if (!string.IsNullOrEmpty(strState) & !string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        if (!string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        RouteData.Values.Clear();
                        return RedirectToAction("Directory", "Search", new { id = "N" });
                    }
                }
                else
                {
                    string strCity = txtzip.Trim();
                    string strState = getStatenameFromCity(strCity);
                    if (!string.IsNullOrEmpty(strState))
                    {
                        RouteData.Values.Clear();
                        strCity = strCity.Replace("/", "-");
                        strCity = strCity.Replace(".", "-");
                        strCity = strCity.Replace("'", "-");
                        return RedirectToRoute("Cities", new { city = strCity.Replace(" ", "-") });
                    }
                    RouteData.Values.Clear();
                    return RedirectToAction("Directory", "Search", new { id = "N" });
                }
            }
            else if (!string.IsNullOrEmpty(txtzip) & ddlRadius != "0")
            {
                string strradius = ValidateZipcode(txtzip);
                if (!string.IsNullOrEmpty(strradius))
                {
                    if (strradius == "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = ddlRadius });
                    }
                    if (ddlRadius != "0")
                    {
                        RouteData.Values.Clear();
                        return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = ddlRadius }); 
                    }
                    RouteData.Values.Clear();
                    return RedirectToRoute("ZipcodeTherapists", new { id = txtzip, distance = strradius });
                }
                if (txtzip.Contains("("))
                {
                    string[] strValues = txtzip.Split('(');
                    if (strValues.Length == 2)
                    {
                        string strCity = strValues[0].Trim();
                        string strState = strValues[1].Replace("(", "").Trim();
                        strState = strState.Replace(")", "").Trim();
                        clsCountry objstateinfo = new clsCountry();
                        objstateinfo = getStatenameFromAbbrevation(strState);
                        strState = objstateinfo.StateName;
                        if (!string.IsNullOrEmpty(strState) & !string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        if (!string.IsNullOrEmpty(getStatenameFromCity(strCity)))
                        {
                            RouteData.Values.Clear();
                            strCity = strCity.Replace("/", "-");
                            strCity = strCity.Replace(".", "-");
                            strCity = strCity.Replace("'", "-");
                            return RedirectToAction("ListOfProviders", "Search", new { city = strCity.Replace(" ", "-"), StateNm = strState });
                        }
                        RouteData.Values.Clear();
                        return RedirectToAction("Directory", "Search", new { id = "N" });
                    }
                }
                else
                {
                    string strCity = txtzip.Trim();
                    string strState = getStatenameFromCity(strCity);
                    if (!string.IsNullOrEmpty(strState))
                    {
                        RouteData.Values.Clear();
                        strCity = strCity.Replace("/", "-");
                        strCity = strCity.Replace(".", "-");
                        strCity = strCity.Replace("'", "-");
                        return RedirectToRoute("Cities", new { city = strCity.Replace(" ", "-") });
                    }
                    RouteData.Values.Clear();
                    return RedirectToAction("Directory", "Search", new { id = "N" });
                }
            }
            else if (string.IsNullOrEmpty(txtzip) & ddlRadius != null)
            {
                RouteData.Values.Clear();
                return RedirectToRoute("DefaultSearch", null);
            }
                return View();
            }
         public ActionResult Uctrl_Zipcodes(string id, string stateid, string stateNm, int listingcount, string citynm, string zip, string distance, string stateab)
{
             if (Request.Url != null) Requesturl = Request.Url.ToString();
             var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
             if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                ViewBag.img = strurl[0] + "//" + strurl[2] + "/" +"images/links-bg2.jpg";
               
                }
                else if (Requesturl.Contains("vbv"))
                {
                ViewBag.img = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" +"images/links-bg2.jpg";
                }
              
            }
            else
            {
                ViewBag.img = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/links-bg2.jpg";
        
            }

    int? cityid = null;

    ViewBag.distance = distance;
    var obj = new ProviderAdvertising();
    int stateid1;
    ViewBag.listcount = listingcount;
    if (id == null)
    {
        ViewBag.id1 = null;
        ViewBag.count = 1;
        ViewBag.Text1 = "Find a Mower Helper with Zip Codes :";
    }
	string strCity = null;
	string strState = null;
    if (stateid != null & stateid!="")
    {
        stateid1 = Convert.ToInt32(stateid);
    }
    else
    {
        stateid1 = 0;
    }
    
    if (id != null)
    {
        if (id.Contains("("))
        {
            var strValues = id.Split('(');
            if (strValues.Length == 2)
            {
                strCity = strValues[0].Trim();
                ViewBag.cty23 = strCity;
                strState = strValues[1].Replace("(", "").Trim();
                strState = strState.Replace(")", "").Trim();
                ViewBag.stab12 = stateab;
                strState = stateNm; 
                cityid = Convert.ToInt32(obj.GetProviderCityID(strCity, stateid1));
            }
            ViewBag.count = 1;
        }
        else
        {
            if (citynm != null)
            {
                ViewBag.cty23 = citynm;
                strCity = citynm;
            }
            ViewBag.count = 0;
        }
    }
    else
    {
        ViewBag.cty23 = null;
        
    }
    if (stateNm != null)
    {
        ViewBag.StateNm = stateNm;
        ViewBag.stab12 = stateab;
    }
    
    ViewBag.zip = zip;
    if (zip == null)
    {

        DtListZipcodeBind(strCity, cityid, strState, stateid1, stateab, stateNm, distance);
    }
	return View();
}
               
        public void DtListZipcodeBind(string strCity, int? cityid, string strState, int stateid, string stateab, string stateNm, string distance)
{
            try
            {
	var obj = new ProviderAdvertising();
    var objDataList = obj.GetZipcodesbasedonStatecities(stateid, cityid, null, null, Convert.ToInt32(distance), strCity);
    if ((objDataList != null))
    {
        if (objDataList.Count > 0)
        {
            ViewBag.zipcount = objDataList.Count;
            ViewBag.Zipcodes = objDataList;
            string zipcode1 = objDataList[0].Zipcode;
            int rowcount = objDataList.Count / 4;
            ViewBag.rows = rowcount;
        }
        else
        {
            ViewBag.Zipcodes = null;
            ViewBag.zipcount = 0;
        }
        
    }
    if (strCity != null)
    {
        if (strCity.Contains("  "))
        {
            strCity = strCity.Replace("  ", "/ ");
        }
    }
    if (strCity != null & stateab != null)
    {
        ViewBag.Text = strCity + " (" + stateab + ") Electricians with Zipcodes :";
        ViewBag.id1 = 1;
    }
    else if (strState != null)
    {
        ViewBag.Text = strState + " Electricians with Zipcodes :";
        ViewBag.id1 = 1;
    }
    else if (strCity != null)
    {
        ViewBag.Text = strCity + " Electricians with Zipcodes :";
        ViewBag.id1 = 1;
    }
    else if (stateNm != null)
    {
        ViewBag.Text = stateNm + " Electricians with Zipcodes :";
        ViewBag.id1 = 1;
    }
      
   }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "DtListZipcodeBind", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
  }


        private clsCountry getStatenameFromAbbrevation(string stateAbbrevation)
        {

            try
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = { new SqlParameter("@in_State_Abbrevation", stateAbbrevation) };
                objcommon.AddInParameters(objparam);
               var drlist = objcommon.GetDataReader("Help_dbo.st_Admin_Get_StateName");
                while (drlist.Read())
                {
                    return new clsCountry { StateFullName = Convert.ToString(drlist["StateName"]), StateId = Convert.ToInt32(drlist["State_ID"]) };
                }
               

            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "getStatenameFromAbbrevation", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                
            }
            return null;
        }

        private string getStatenameFromCity(string cityname)
        {
            try
            {
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_City", cityname) };
            objcommon.AddInParameters(objparam);
          var  drlist = objcommon.GetDataReader("Help_dbo.st_Admin_Get_StateNameFromCity");
            while (drlist.Read())
            {
                if (drlist["Statename"]!=null)
                {
                    return drlist["Statename"].ToString();
                }
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "getStatenameFromCity", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        private int GetCityID(string cityname,string statename)
        {
            try{
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam =
            {
                new SqlParameter("@in_state", statename),
                 new SqlParameter("@in_city", cityname)
            };
            objcommon.AddInParameters(objparam);
            var drlist = objcommon.GetDataReader("Help_dbo.St_Get_Cityid_Using_City");
            while (drlist.Read())
            {
                if (drlist["City_ID"] != null)
                {
                    return Convert.ToInt32(drlist["City_ID"]);
                }
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "GetCityID", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public string ValidateZipcode(string zipcode)
        {
            try
            {
            var objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@in_Zip", zipcode) };
            IDataParameter[] objoutparam = { new SqlParameter("@Out_Radius", SqlDbType.Int) };
            objcommon.AddInParameters(objparam);
            objcommon.AddOutParameters(objoutparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.ST_Public_Validate_zipcode");
            if (objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value!=null)
            {
                return Convert.ToString(objcommon.objdbCommandWrapper.Parameters["@Out_Radius"].Value);
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "ValidateZipcode", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public void GetStatecitywithZip(string strzipcode)
        {
            try
            {
            var objdata = new ProviderAdvertising();
            clsCountry obj = objdata.GetStatecitywithZip(strzipcode);
            if ((obj == null)) return;
            ViewBag.mycity = obj.CityName ?? null;
            ViewBag.state = obj.StateName ?? null;
             }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "GetStatecitywithZip", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void GetDataListPaging(ref int totNoRecords)
        {
            try
            {
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
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "GetDataListPaging", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void DtListCitiesBind(string zipcode, string stateid,int?distance,string issueid=null,string langid=null)
{
            try
            {
	var obj = new ProviderAdvertising();
   int stateId = stateid != null ? Convert.ToInt32(stateid) : 0;

	string zip = zipcode ?? null;
    var objDataList = obj.GetCitiesofProvidersBasedonStateID(stateId,zip, distance, 0);
	if (objDataList.Count > 0) {
		ViewBag.namecity = null;
		ViewBag.Displaybold = null;
		ViewBag.StateName = null;
		ViewBag.CountInCity = null;
		ViewBag.StateFullName = null;
		ViewBag.CityId = null;
		ViewBag.namecity = objDataList;
        ViewBag.namecitycount = objDataList.Count;
        int rowcount = objDataList.Count / 4;
        ViewBag.rows = rowcount;
		ViewBag.Displaybold = objDataList[0].Displaybold;
		ViewBag.StateName = objDataList[0].StateName;
		ViewBag.CountInCity = objDataList[0].CountInCity;
		ViewBag.StateFullName = objDataList[0].StateFullName;

	}
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "SearchController", "DtListCitiesBind", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
}
        public ActionResult _MasterPage()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();


            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" +"images/raight-heading.gif";
               
                }
                else if (Requesturl.Contains("vbv"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/raight-heading.gif";
                }
              
            }
            else
            {
                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/raight-heading.gif";
        
            }
            return View();
        }
        public ActionResult __MasterPage1()
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();


            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "images/raight-heading.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "images/speacer.gif";

                }
                else if (Requesturl.Contains("vbv"))
                {
                    ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/raight-heading.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/speacer.gif";
                }

            }
            else
            {
                ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/raight-heading.gif";
                ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "/" + "images/speacer.gif";

            }
            return View();
                  }
        public ActionResult _Therapistlist()
        {
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl = Requesturl.Split('/');
            //var objconfig = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                   
                    ViewBag.img12ind = strurl[0] + "//" + strurl[2] + "/" + "images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + "images/speacer.gif";
                }
                else if (Requesturl.Contains("vbv"))
                {
                  
                    ViewBag.img12ind = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/line-bg.gif";
                    ViewBag.speacer = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/speacer.gif";
                }

            }
            else
            {
                ViewBag.Imageurl = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl");
                ViewBag.img12ind = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/line-bg.gif";
                ViewBag.speacer = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/speacer.gif";

            }
            if (TempData["therapistlist"] !=null)
            {
                ViewBag.FeaturedList = TempData["therapistlist"];
                ViewBag.FeaturedListcount = TempData["therapistlistcnt"];
            }
            else
            {
                ViewBag.FeaturedList = null;
                ViewBag.FeaturedListcount = 0;
            }
            //if (ViewBag.FeaturedListcount > 0)
            //{
            //    foreach (var item in ViewBag.FeaturedList)
            //    {
            //        ProviderAdvertising objproviderAdvertise = new ProviderAdvertising();
            //        objproviderAdvertise.SiteStatic_ID = "2";
            //        objproviderAdvertise.State_Name = StateName;
            //        objproviderAdvertise.CityNm = CityName;
            //        objproviderAdvertise.Provider_ID = Convert.ToString(item.ProviderID);
            //        objproviderAdvertise.CountingProviderVisitToHisProfile(objproviderAdvertise);
            //    }
            //}
            return View();
        }
    }
}
