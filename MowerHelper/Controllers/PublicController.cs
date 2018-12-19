using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.Technicians;
using MowerHelper.Models.Classes;
using System.Threading;
namespace MowerHelper.Controllers
{
    [App_Start.NotRequireHttps()]
    public class PublicController : Controller
    {
        public string Requesturl;
        DataSet objds = new DataSet();
        int StartRec = 0;
        int PreviousRec;
        int NextRec;
        string aPublicURL = "";
        //clsWebConfigsettings cs = new clsWebConfigsettings();
       // [AllowAnonymous]
        [HttpGet]
       // [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCitiesByStates(int id)
        {
            List<clsCountry> objcity = new List<clsCountry>();
            objcity = clsCountry.GetCityByStateId(id);

            //IList<SelectListItem> _result = clsCountry.GetCityByStateId(id);
                //new List<SelectListItem>();
            
           // if (objcity.Count > 0)
           // {
                //if (objcity.Count > 100)
                //{
                ////var watch1 = System.Diagnostics.Stopwatch.StartNew();
                //Parallel.ForEach(objcity, City =>
                //{
                //    //Your stuff
                //    _result.Add(new SelectListItem
                //    {
                //        Text = City.CityName,
                //        Value = Convert.ToString(City.CityId)
                //    });
                //});
                ////watch1.Stop();
                ////Session["elapsedMs1"] = watch1.ElapsedMilliseconds;
                //}
                //else
                //{
                ////var watch = System.Diagnostics.Stopwatch.StartNew();
                //for (int i = 0; i <= objcity.Count - 1; i++)
                //{
                //    _result.Add(new SelectListItem
                //    {
                //        Text = objcity[i].CityName,
                //        Value = Convert.ToString(objcity[i].CityId)
                //    });
                //}
                ////watch.Stop();
                ////Session["elapsedMs"] = watch.ElapsedMilliseconds;
                //}
          
            //}
            //else
            //{
            //    _result.Add(new SelectListItem
            //    {
            //        Text = "--Select City--",
            //        Value = "0",
            //        Selected = true
            //    });
            //}
           // StateCity reg1 = new StateCity();
            //reg1 = new StateCity { CityList = _result };

            return Json(objcity, JsonRequestBehavior.AllowGet);
        }
        //[AllowAnonymous]
        [HttpGet]
   // [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetStateCitiesByZip(string zipcode)
        {
           // Thread.Sleep(10000);
            List<clsCountry> objzip = new List<clsCountry>();
            List<clsCountry> objCities = new List<clsCountry>();
            objzip = clsCountry.Reg_GetStatesByZipCode(zipcode);
           // List<clsCountry> _result = new List<clsCountry>();
            //List<clsCountry> _result1 = new List<clsCountry>();
           // StateCity reg1 = new StateCity();
            if (!string.IsNullOrEmpty(zipcode))
            {
                if (objzip.Count > 0 & objzip.Count < 2)
                {
                  //  for (int i = 0; i <= objzip.Count - 1; i++)
                   // {
                        //_result.Add(new clsCountry
                        //{
                        //    st = objzip[i].StateFullName,
                        //    Value = Convert.ToString(objzip[i].StateId),
                        //    Selected = true
                        //});
                        objCities = clsCountry.Reg_GetCityByZipCode(zipcode, objzip[0].StateId);
                        //if (objCities.Count > 0)
                        //{
                        //    for (int j = 0; j <= objCities.Count - 1; j++)
                        //    {
                        //        _result1.Add(new SelectListItem
                        //        {
                        //            Text = objstate[j].CityName,
                        //            Value = Convert.ToString(objstate[j].CityId),
                        //            Selected = true
                        //        });
                        //    }
                        //}
                 //   }
                }
                //else if (objzip.Count >= 2)
                //{
                //    for (int i = 0; i <= objzip.Count - 1; i++)
                //    {
                //        _result.Add(new SelectListItem
                //        {
                //            Text = objzip[i].StateFullName,
                //            Value = Convert.ToString(objzip[i].StateId),
                //            Selected = true
                //        });
                //    }
                //}
                //else
                //{
                //    _result.Add(new SelectListItem
                //    {
                //        Text = "Select State",
                //        Value = "0"
                //    });
                //    //_result1.Add(new SelectListItem
                //    //{
                //    //    Text = "Select City",
                //    //    Value = "0"
                //    //});
                //}
                //reg1 = new StateCity
                //{
                //    StateList = _result,
                //   CityList = _result1
                //};
            }
            if (objzip.Count >= 2)
            {
                return Json(objzip, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(objzip[0].StateFullName + "," + objzip[0].StateId + "," + objCities[0].CityName + "," + objCities[0].CityId, JsonRequestBehavior.AllowGet);
            }
        }
        //[AllowAnonymous]
        [HttpGet]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCitiesByZip(string zipcode, int id)
        {
            List<clsCountry> objCities = new List<clsCountry>();
          //  IList<SelectListItem> _result1 = new List<SelectListItem>();
            objCities = clsCountry.Reg_GetCityByZipCode(zipcode, id);
            //if (objstate.Count > 0)
            //{
            //    for (int j = 0; j <= objstate.Count - 1; j++)
            //    {
            //        _result1.Add(new SelectListItem
            //        {
            //            Text = objstate[j].CityName,
            //            Value = Convert.ToString(objstate[j].CityId),
            //            Selected = true
            //        });
            //    }
            //}
            //else
            //{
            //    _result1.Add(new SelectListItem
            //    {
            //        Text = "--Select City--",
            //        Value = "0"
            //    });
            //}
            //StateCity reg1 = new StateCity();
            //reg1 = new StateCity { CityList = _result1 };
            return Json(objCities, JsonRequestBehavior.AllowGet);
        }
        //[AllowAnonymous]
        [HttpGet]
       // [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetStateByZip()
        {
            //Thread.Sleep(10000);
            List<clsCountry> objstates = new List<clsCountry>();
            objstates = clsCountry.GetStatesByCountryId(1);
            //IList<SelectListItem> _result1 = new List<SelectListItem>();
            //StateCity reg1 = new StateCity();
            //if (objstates.Count > 0)
            //{
            //    if (objstates.Count > 100)
            //    {
            //        Parallel.ForEach(objstates, State =>
            //        {
            //            //Your stuff
            //            _result1.Add(new SelectListItem
            //            {
            //                Text = State.StateFullName,
            //                Value = Convert.ToString(State.StateId)
            //            });
            //        });
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= objstates.Count - 1; i++)
            //        {
            //            _result1.Add(new SelectListItem
            //            {
            //                Text = objstates[i].StateFullName,
            //                Value = Convert.ToString(objstates[i].StateId)
            //            });
            //        }
            //    }
            //}
            //IList<SelectListItem> _result2 = new List<SelectListItem>();
            //_result2.Add(new SelectListItem
            //{
            //    Text = "--Select City--",
            //    Value = "0",
            //    Selected = true
            //});

            //reg1 = new StateCity { StateList = _result1 };
            return Json(objstates, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous()]
        public ActionResult Profile(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
            
         }
        public ActionResult Services(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             List<ProviderAdvertising> objDataList = new List<ProviderAdvertising>();
             ProviderAdvertising obj = new ProviderAdvertising();
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }
        public ActionResult Contact(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }
        public ActionResult Comments(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }
        public ActionResult ProviderDocuments(string randomnumber)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }

         public ActionResult MailToFriend(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
             var obj3 = new clsCommonFunctions();
             ViewBag.captcha = obj3.GetCaptcha("4");
             TempData["ind1"] = 1;
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 if (Request.Url != null)
                 {
                     string url = Request.Url.ToString();
                     string[] strurl = url.Split('/');
                     //var objconfig = new clsWebConfigsettings();
                     if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                     {
                         if (url.Contains("localhost:"))
                         {
                             ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "images/raight-heading.gif";
                         }
                         else if (url.Contains("vbv"))
                         {
                             ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/raight-heading.gif";
                         }

                     }
                     else
                     {
                         ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/raight-heading.gif";

                     }
                 }
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }

         [HttpPost()]
         [ValidateInput(false)]
         public JsonResult SendEmail(Category category)
         {
             string msg = null;
             if (Session["UserID"] == null)
             {
                 return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
             }
             string strKeyMsg = null;
             if (category.Vrificationcode != Convert.ToString(HttpContext.Session["captchastring"]))
             {
                 strKeyMsg = "You need to enter valid Security Code to register";
             }
             if (strKeyMsg == null)
             {

                 if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
                 {
                     int categoryCount = 0;
                     var objCategory = new Category();
                     categoryCount = Category.EmailMsgcount(5);
                     objCategory.EmailCategoryID = "5";
                     objCategory.EmailCategoryCount = categoryCount;
                     objCategory.FromReference_id = null;
                     objCategory.Toreference_id = null;
                     objCategory.ISWebService_IND = "N";
                     objCategory.Application_Id = null;
                     Category.SentEmailLog(objCategory);
                     string str = null;
                     var objMailMessage = new ClsMailMessage();
                     str = "<b> Message : </b>" + Sanitizer.GetSafeHtmlFragment(category.Message) + "<br />";
                     str = str + "  <b> Provider Name : </b> " + category.ProviderNmae + "<br />";
                     str = str + "  <b> Provider Address : </b> " + category.Address + " <br />";
                     str = str + "  <b> Tel : </b> " + category.MobileText + " <br />";
                     str = str + "  <b> Provider Url : </b> " + category.PublicURL + " <br />";
                     str = str + "  5-" + (categoryCount + 1);
                     bool ress = objMailMessage.SendMail(category.FriendsEmail, category.YourEmail, "Therapist profile", str, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                     Upd_EmailStatistics(category.Providerid);
                     if (ress == true)
                     {
                          msg = "1";
                         //return Json(msg);
                     }
                     else
                     {
                          msg = "2";
                        // return Json(msg);
                     }
                 }
                 else
                 {
                     Upd_EmailStatistics(category.Providerid);

                     msg = "1";
                    

                 }
             }
             else
             {
                  msg = "false";
                 //return Json(msg);
             }
             return Json(msg);
             //return null;
         }
         [ValidateInput(false)]
         public JsonResult ContactWithTherapist(Category category)
         {
             if (Session["UserID"] == null)
             {
                 return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
             }
             string strKeyMsg = null;
             if (category.Vrificationcode != Convert.ToString(HttpContext.Session["captchastring"]))
             {
                 strKeyMsg = "You need to enter valid Security Code to register";
             }
             if (strKeyMsg == null)
             {
                 if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
                 {
                     int categoryCount = 0;
                     var objCategory = new Category();
                     categoryCount = Category.EmailMsgcount(4);
                     objCategory.EmailCategoryID = "4";
                     objCategory.EmailCategoryCount = categoryCount;
                     objCategory.FromReference_id = null;
                     objCategory.Toreference_id = category.Providerid;
                     objCategory.ISWebService_IND = "N";
                     objCategory.Application_Id = null;
                     objCategory.FromReferenceType_id = null;
                     objCategory.Toreferencetype_id = "2";
                     Category.SentEmailLog(objCategory);
                     try
                     {
                         string strMsg = null;
                         string subject;
                         switch (category.Subject)
                         {
                             case "1":
                                 subject = "Appointment Request";
                                 break;
                             case "2":
                                 subject = "More Information";
                                 break;
                             case "3":
                                 subject = "Other";
                                 break;
                             default:
                                 subject = null;
                                 break;
                         }
                         strMsg = "<b>Name :</b> " + category.Yourname + "<br/> <b> Email : </b>" + category.YourEmail + "<br/> <b> Phone Number : </b>" + category.MobileText + "<br/> <b> Subject : </b> " + subject + "<br/> <b> Message : </b> " + Sanitizer.GetSafeHtmlFragment(category.Message) + "<br/> 4-" + (categoryCount + 1);
                         IDataParameter[] strInsParam = {
				new SqlParameter("@In_ToReferenceType_ID", 2),
				new SqlParameter("@In_ToReference_ID", category.Providerid),
				new SqlParameter("@In_FromReferenceType_ID", 3),
				new SqlParameter("@In_FromReference_ID", null),
				new SqlParameter("@In_Practice_ID", category.PracticeId),
				new SqlParameter("@In_MsgOption_ID", 20),
				new SqlParameter("@in_TomailID", category.ProviderEmail),
				new SqlParameter("@in_FromMailID", category.YourEmail),
				new SqlParameter("@in_Subject", subject),
				new SqlParameter("@in_Message", strMsg)
			};

                         IDataParameter[] strOutParam = { new SqlParameter("@Out_MsgTransaction_ID", SqlDbType.Int) };
                         var objWrapper = new clsDBWrapper();
                         var objCommon = new clsCommonFunctions();
                         ClsMailMessage objMailMessage = default(ClsMailMessage);

                         var with1 = objWrapper;
                         with1.AddInParameters(strInsParam);
                         with1.AddOutParameters(strOutParam);
                         with1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_Ins_Message_Pr");
                         string strEmailMessageTransactionId = "";
                         if ((objWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value) != null)
                         {
                             strEmailMessageTransactionId = objWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value.ToString();
                         }

                         string strfrommail = !string.IsNullOrEmpty(category.YourEmail.Trim()) ? category.YourEmail : clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                         Providerdetails(Convert.ToString(category.randomnumber));
                         objMailMessage = new ClsMailMessage();
                         bool res = objMailMessage.SendMail(category.ProviderEmail, strfrommail, subject, strMsg, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                         if (res == true)
                         {
                             IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessageTransactionId) };
                             var with2 = objWrapper;
                             with2.AddInParameters(strMailParam);
                             with2.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                         }

                         IDataParameter[] param = {
					new SqlParameter("@In_Provider_ID", category.Providerid),
					new SqlParameter("@In_sendername", (!string.IsNullOrEmpty(category.Yourname) ? category.Yourname : null)),
					new SqlParameter("@In_SenderEmail", (!string.IsNullOrEmpty(category.YourEmail) ? category.YourEmail : null)),
					new SqlParameter("@In_Subject", subject),
					new SqlParameter("@In_Messgae", strMsg)
				};
                         var with3 = objWrapper;
                         with3.AddInParameters(param);
                         with3.ExecuteNonQuerySP("Help_dbo.st_Provider_INS_PublicMessages");

                         //IDataParameter[] objparam = { new SqlParameter("@In_Provider_ID", category.Providerid) };
                         //var with4 = objWrapper;
                         //with4.AddInParameters(objparam);
                         //with4.ExecuteNonQuerySP("Help_dbo.st_Public_UPD_MessagingCount");
                         const string msg = "1";
                         return Json(msg);
                     }
                     catch (Exception ex)
                     {
                         var obj = new clsExceptionLog();
                         obj.LogException(ex, "PublicController", "ContactWithTherapist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

                     }
                 }
                 else
                 {
                     const string msg = "2";
                     return Json(msg);
                 }
             }
             else
             {
                 const string msg = "3";
                 return Json(msg);
             }
             return null;

         }
         public ActionResult PublicProviderDocuments(int id, int practiceid)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             ViewBag.providerid = id;
           ViewBag.practiceid = practiceid;
           int  pageNo = Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1;
             var obj = new ProviderWebSites();
             var objDataList = new List<ProviderWebSites>();
             try
             {
                 objDataList = obj.GetProviderDocumentsForDifferentWebSites(id, 10, pageNo);

                 if (objDataList.Count > 0)
                 {
                     ViewBag.documentslist = objDataList;
                     ViewBag.totrec = ProviderWebSites.TotalNoRecords;
                 }
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "publicProviderDocuments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }


             return View("publicProviderDocuments", objDataList);
         }
         public ActionResult ProviderGreetings(string randomnumber, string page)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             ViewBag.id = randomnumber;
             Providerdetails(randomnumber);
             var objDataList = new List<ProviderWebSites>();
             try
             {

                 var objcommon = new clsCommonFunctions();
                 IDataParameter[] inParamList = {
			new SqlParameter("@In_Provider_ID", Convert.ToInt32(TempData["Providerid1"])),
          new SqlParameter("@In_Public", "Y"),
			new SqlParameter("@In_NoOfRecords", "10"),
			new SqlParameter("@In_Pageno", page != null ? Convert.ToInt32(page) : 1)
		};
                 objcommon.AddInParameters(inParamList);
              var   ds = objcommon.GetDataSet("Help_dbo.st_Provider_List_Greetings");
                 if (ds.Tables.Count > 0)
                 {
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         foreach (DataRow dr in ds.Tables[0].Rows)
                         {
                             var objData = new ProviderWebSites();
                             if (dr["Title"] != null)
                             {
                                 objData.Title = (string)dr["Title"];
                                 ViewBag.title12 = (string)dr["Title"];
                                 TempData["title"] = (string)dr["Title"];
                             }
                             else
                             {
                                 objData.Title = null;
                                 TempData["title"] = null;
                             }
                             if (dr["ImagePathSuffix"] != null)
                             {
                                 objData.ImagePathSuffix = (string)dr["ImagePathSuffix"];
                             }
                             else
                             {
                                 objData.ImagePathSuffix = null;
                             }

                             if (dr["Provider_Greeting_ID"] != null)
                             {
                                 objData.GreetingID = (int)dr["Provider_Greeting_ID"];
                             }
                             else
                             {
                                 objData.GreetingID = 0;
                             }
                             if (dr["Description"] != null)
                             {
                                 objData.DocDescription = (string)dr["Description"];
                                 ViewBag.desc = (string)dr["Description"];
                                 TempData["Description"] = (string)dr["Description"];
                             }
                             else
                             {
                                 objData.DocDescription = null;
                                 TempData["Description"] = null;
                             }
                             objDataList.Add(objData);
                         }
                     }
                     if (ds.Tables[1].Rows.Count > 0)
                     {
                         ProviderWebSites.TotalNoRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                         ViewBag.totrec = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                     }
                 }


               
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();
                 clsexp.LogException(ex, "PublicController", "ProviderGreetings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

             }
             return View("ProviderGreetings", objDataList);
         }
         [HttpGet]
         [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
         [ValidateInput(false)]
         public ActionResult Playvideo(string randomnumber, int greetingid, string Therapistname)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             Providerdetails(randomnumber);
            // ViewBag.id = id;
             try
             {

                 var objcommon = new clsCommonFunctions();
                 IDataParameter[] inParamList = {
			new SqlParameter("@In_Provider_ID", Convert.ToInt32(TempData["Providerid1"])),
			new SqlParameter("@In_Public", "Y"),
			new SqlParameter("@In_NoOfRecords", 10),
			new SqlParameter("@In_Pageno", 1),
            new SqlParameter("@in_Provider_Greeting_ID",greetingid)
		};
                 objcommon.AddInParameters(inParamList);
                 var ds = objcommon.GetDataSet("Help_dbo.st_Provider_List_Greetings");
                 if (ds.Tables.Count > 0)
                 {
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         foreach (DataRow dr in ds.Tables[0].Rows)
                         {
                             var objData = new ProviderWebSites();
                             if (dr["Title"] != null)
                             {
                                 objData.Title = (string)dr["Title"];
                                 ViewBag.videotitle = objData.Title;
                                 
                             }
                             else
                             {
                                 objData.Title = null;
                                 ViewBag.videotitle = null;
                                
                             }
                             if (dr["ImagePathSuffix"] != null)
                             {
                                 objData.ImagePathSuffix = (string)dr["ImagePathSuffix"];
                             }
                             else
                             {
                                 objData.ImagePathSuffix = null;
                             }

                            
                             if (dr["Description"] != null)
                             {
                                 objData.DocDescription = (string)dr["Description"];
                                 ViewBag.description = (string)dr["Description"];
                                
                             }
                             else
                             {
                                 objData.DocDescription = null;
                                 ViewBag.description = null;
                             }
                         }
                     }
                     if (ds.Tables[1].Rows.Count > 0)
                     {
                         ProviderWebSites.TotalNoRecords = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                         ViewBag.totrec = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                     }
                 }



             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();
                 clsexp.LogException(ex, "PublicController", "Playvideo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

             }
             if (greetingid != 0)
             {
                 Updstatistics(Convert.ToInt32(TempData["Providerid1"]), greetingid);
             }
            // Providerdetails(Convert.ToInt32(id));
             string youtubetext = GetProviderYouTubeEmbeddedText(greetingid);
             ViewBag.youtube = youtubetext;
             return PartialView("Playvideo", youtubetext);
         }
         public ActionResult ProviderVerification(string randomnumber)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             Providerdetails(randomnumber);
             ViewBag.id = randomnumber;
             if (TempData["Providerid1"] != null & Convert.ToString(TempData["Providerid1"]) != "" & Convert.ToString(TempData["Providerid1"]) != "0")
             {
                 return View();

             }
             else
             {
                 return RedirectToAction("PageNotFound", "Error");
             }
         }
         public ContentResult Download(int id, int practiceid, int providerid)
         {
             ProviderAdvertising obj1 = new ProviderAdvertising();
             obj1.CountingProviderVisitToHisProfileDocument(providerid, 12, id, null, null);
             string filename = null;
             string extn = null;
             string patencrypt = null;
             string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\Documents" + "\\";
             Provider_DocumentInfo obj = new Provider_DocumentInfo();
             obj = Provider_DocumentInfo.GetAttachmentList(providerid, id);
             if ((obj != null))
             {
                 filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + obj.Path);
                 extn = System.IO.Path.GetExtension(strFilePath + obj.Path);
                 patencrypt = obj.Path_Suffix;
             }
             string fullfilepath = filename + patencrypt + extn;
             strFilePath = strFilePath + "/" + fullfilepath;
             System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
             if (File.Exists)
             {
                 Response.Clear();
                 Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                 Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                 Response.ContentType = extn;
                 Response.WriteFile(File.FullName);
                 Response.End();
             }
             else
             {
                 Response.Clear();
                 Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                 Response.AddHeader("Content-Length", "0");
                 Response.ContentType = extn;
                 Response.Write("file not found.");
                 Response.End();
             }
             return null;
         }
         public ActionResult visitmywebsite(int id, string url)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             string stateName = null;
             string cityName = null;
             ViewBag.url = url;
             if ((Request.Cookies["State"] != null))
             {
                 if (Request.Cookies["State"].Value != null)
                 {
                     stateName = Request.Cookies["State"].Value.Replace("%20", " ");
                 }
             }
             if ((Request.Cookies["City"] != null))
             {
                 if (Request.Cookies["City"].Value != null)
                 {
                     cityName = Request.Cookies["City"].Value.Replace("%20", " ");
                 }
             }

             var obj = new ProviderWebSites();
             obj.GetproviderWebsiteCount(id, stateName, cityName);
             Response.Redirect(ViewBag.url);
             return null;
         }
         private void Providerdetails(string randomnumber)
         {
             //var objconfig = new clsWebConfigsettings();
            try
            {
             var obj = new ProviderAdvertising();
             var objData = obj.GetTheProviderProfile(randomnumber);
             TempData["Providerid1"] = objData.ProviderID;
             if (Convert.ToString(objData.ProviderID) != "" & objData.ProviderID != 0)
             {
                 ViewBag.ProviderID = objData.ProviderID;
                     int providerid = Convert.ToInt32(objData.ProviderID);
                 ViewBag.videocnt = objData.VideoCount;
                 ViewBag.ProviderName = objData.ProviderFullName;
                 ViewBag.picture = objData.Picture;
                 ViewBag.rnum = objData.Random_number;
                   string Requesturl = null;
                 if (Request.Url != null) Requesturl = Request.Url.ToString();
                 var strurl = Requesturl.Split('/');
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
                 if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
                 {
                     ViewBag.Showschedulecog = "Y";
                 }
                 else
                 {
                     ViewBag.Showschedulecog = "N";
                 }
                 if (!string.IsNullOrEmpty(objData.Picture))
                 {
                     string str = objData.Picture;
                     ViewData["str"] = str;
                     ViewBag.strimage = str;
                     if (str.Contains(".") == true)
                     {
                         string[] str1 = str.Split('.');
                         ViewData["ExistedLogo"] = str1[0];
                         ViewBag.ExistedLogo = str1[0];
                         ViewData["ExistedExtn"] = str1[1];
                         ViewBag.ExistedExtn = str1[1];
                     }

                     string strings = null;
                     strings = Path.Combine(Server.MapPath("~/Attachments/Providers"), str);
                     if (System.IO.File.Exists(strings))
                     {

                         ViewBag.imageavail = "Y";
                         ViewBag.image = "Y";
                         ViewBag.providerimage = domain + "Attachments/Providers/" + str;

                     }
                    
                     else
                     {
                         ViewBag.providerimage = domain + "images/Coach1.JPG";

                     }

                 }
                 else
                 {
                    
                         ViewBag.providerimage = domain + "images/Coach1.JPG";
                        


                 }

                 ViewBag.BusinessName = objData.Businessname;
                 ViewBag.provideremail = objData.Email;
                 ViewBag.businessnm1 = objData.PublicURL.Trim().Replace(" ", "-");
                 ViewBag.licenseverify = objData.IsLicenseVerified;
                 TempData["proname"] = ViewBag.businessnm1;
                 ViewBag.fbcomments = objData.fbcomments;
                 string strcookiestate = null;
                 string strcookiecity = null;
                 if ((Request.Cookies["State"] != null))
                 {
                     if (Request.Cookies["State"].Value != null)
                     {
                         strcookiestate = Request.Cookies["State"].Value.Replace("%20", " ");
                     }
                 }
                 if ((Request.Cookies["City"] != null))
                 {
                     if (Request.Cookies["City"].Value != null)
                     {
                         strcookiecity = Request.Cookies["City"].Value.Replace("%20", " ");
                     }
                 }

                 //var obj1 = new ProviderAdvertising
                 //{
                 //    Provider_ID = Convert.ToString(providerId),
                 //    SiteStatic_ID = "4",
                 //    State_Name = strcookiestate,
                 //    CityNm = strcookiecity
                 //};
                 //obj.CountingProviderVisitToHisProfile(obj1);
                     providerid = objData.ProviderID;
                     if (objData.Is_Contact_Info == "Y")
                     {
                         ViewBag.contactinfo = "Y";
                     }
                 if (objData.Country != null)
                 {
                     ViewBag.country = objData.Country;
                     ViewBag.Title = "Mower helpers in " + objData.Country;
                     ViewBag.country = objData.Country;
                     ViewBag.countryid = objData.Country_ID;

                 }
                 else
                 {
                     ViewBag.country = "United States";
                     ViewBag.Title = "Mower helpers in United States";
                     ViewBag.countryid = "1";
                 }
                 if (objData.State_Name != null)
                 {
                     ViewBag.statenm = objData.State_Name;
                     string statenm = objData.State_Name;
                     string statenm1 = objData.State_Name;
                     ViewBag.estatenm = objData.State_Name;
                     if (statenm1.Contains("/ "))
                     {
                         statenm1 = statenm1.Replace("/ ", " ");
                         ViewBag.estatenm = statenm1;

                     }
                     if (statenm1.Contains(" "))
                     {
                         statenm1 = statenm1.Replace(" ", "-");
                         ViewBag.estatenm = statenm1;
                     }
                     ViewBag.statetitle = "Mower helpers in " + objData.State_Name;
                     if (statenm.Contains("-"))
                     {
                         statenm = statenm.Replace("-", " ");
                         ViewBag.statenm = statenm;

                     }
                     string stateab = Convert.ToString(obj.GetProviderStateab(statenm));
                     ViewBag.stateab = stateab;

                 }
                 else
                 {
                     ViewBag.statenm = null;
                 }


                 ViewBag.stateid = objData.State_ID ?? null;
                 if (objData.CityNm != null)
                 {
                     string ecity = objData.CityNm;
                     ViewBag.ecity = objData.CityNm;
                     if (ecity.Contains("/ "))
                     {
                         ecity = ecity.Replace("/ ", " ");
                         ViewBag.ecity = ecity;

                     }
                     if (ecity.Contains(" "))
                     {
                         ecity = ecity.Replace(" ", "-");
                         ViewBag.ecity = ecity;
                     }
                     if (ecity.Contains("'"))
                     {
                         ecity = ecity.Replace("'", "-");
                         ViewBag.ecity = ecity;
                     }
                     ViewBag.citynm = objData.CityNm;
                     ViewBag.citytitle = "Mower helpers in " + objData.CityNm;
                     ViewBag.city = objData.CityNm + " (" + ViewBag.stateab + ")";
                 }
                 else
                 {
                     ViewBag.citynm = null;
                 }

                 if (objData.ZIPValue != null)
                 {
                     ViewBag.zipvalue = objData.ZIPValue;
                     string zipCode = objData.ZIPValue;
                     DataListCities_DataListZIPCodes_Bind(zipCode);
                 }
                 if (objData.Address != null)
                 {
                     ViewBag.address = objData.Address;
                 }
                 else
                 {
                     ViewBag.address = null;
                     ViewBag.mapaddress = null;
                 }
                 ViewBag.fax = objData.Fax ?? null;
                 ViewBag.Vmoffice = objData.Vmoffice ?? null;
                 ViewBag.cellphone = objData.cellphone ?? null;
                 ViewBag.Availability = objData.Availability ?? null;
                 ViewBag.showschedule = objData.show_schedule ?? null;
                         ViewBag.Personalwebsite = "1";
                         if (objData.Webaddress != null)
                         {
                             if (objData.Webaddress.StartsWith("http"))
                             {
                                 objData.Webaddress = objData.Webaddress;
                                 ViewBag.Webaddress = objData.Webaddress;
                             }
                             else if (objData.Webaddress.StartsWith("https"))
                             {
                                 objData.Webaddress = objData.Webaddress;
                                 ViewBag.Webaddress = objData.Webaddress;
                             }
                             else
                             {
                                 objData.Webaddress = "http://" + objData.Webaddress;
                                 ViewBag.Webaddress = "http://" + objData.Webaddress;
                             }
                         }
                         else
                         {
                             ViewBag.Webaddress = null;
                         }
                         ViewBag.lnkPersonalNavigateUrl = objData.Webaddress;

                 if (objData.Description != null)
                 {
                     ViewBag.desc = objData.Description;
                 }
                 if (objData.Payments != null)
                 {
                     ViewBag.paymentsandconditions = objData.Payments.Trim();
                 }

                 if (objData.Avgcostepersessionfrom != null)
                 {
                     string s = objData.Avgcosepersessionto;
                     if (Convert.ToDecimal(objData.Avgcostepersessionfrom) >= 0 & Convert.ToDecimal(objData.Avgcosepersessionto) >= 0)
                     {
                         if (Convert.ToDouble(objData.Avgcosepersessionto) > 0)
                         {
                             ViewBag.avcostpersession = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(objData.Avgcostepersessionfrom))) + " - " + "$" + string.Format("{0:0.00}", (Convert.ToDecimal(objData.Avgcosepersessionto)));
                         }
                         else
                         {
                             ViewBag.avcostpersession = string.Format(objData.Avgcostepersessionfrom);
                         }

                     }
                 }
                 if (objData.License != null)
                 {
                     string str = objData.License.ToUpper();
                     if (str.Contains("XXXX") | str.Contains("XXX") | str.Contains("XX"))
                     {
                         ViewBag.license = null;

                     }
                     else
                     {
                         ViewBag.license = objData.License;
                     }
                 }
                 else
                 {
                     ViewBag.license = null;
                 }

                 ViewBag.Yearsinpractice = objData.Yearsinpractice != 0 ? (dynamic) objData.Yearsinpractice : null;
                 if (objData.twitterurl != null)
                 {

                     if (objData.twitterurl.StartsWith("http"))
                     {
                         ViewBag.twitterurl = objData.twitterurl;
                     }
                     else if (objData.twitterurl.StartsWith("https"))
                     {
                         ViewBag.twitterurl = objData.twitterurl;
                     }
                     else
                     {
                         ViewBag.twitterurl = "http://" + objData.twitterurl;
                     }

                 }
                 else
                 {
                     ViewBag.twitterurl = null;
                 }
                 if (objData.facebookurl != null)
                 {

                     if (objData.facebookurl.StartsWith("http"))
                     {
                         ViewBag.facebookurl = objData.facebookurl;
                     }
                     else if (objData.facebookurl.StartsWith("https"))
                     {
                         ViewBag.facebookurl = objData.facebookurl;
                     }
                     else
                     {
                         ViewBag.facebookurl = "http://" + objData.facebookurl;
                     }



                 }
                 else
                 {
                     ViewBag.facebookurl = null;
                 }
                 ViewBag.PublicURL = objData.PublicURL;
                 ViewBag.publicurl = objData.PublicURL;
                 objds = obj.GetPreviousNextRecords(objData.State_ID, objData.City_ID);

                 {
                     if (objds.Tables[0].Rows.Count > 1)
                     {

                         DataTable dt = objds.Tables[0];
                         for (int row = 0; row <= dt.Rows.Count - 1; row++)
                         {
                             if (Convert.ToString(objds.Tables[0].Rows[row]["Random_number"]) == randomnumber)
                             {
                                 StartRec = row;
                             }
                         }
                         if (StartRec == 0)
                         {
                             PreviousRec = objds.Tables[0].Rows.Count - 1;
                         }
                         else
                         {
                             PreviousRec = StartRec - 1;
                         }
                         if (StartRec == objds.Tables[0].Rows.Count - 1)
                         {
                             NextRec = 0;
                         }
                         else
                         {
                             NextRec = StartRec + 1;
                         }
                         for (int row = 0; row <= dt.Rows.Count - 1; row++)
                         {
                             aPublicURL = Convert.ToString(objds.Tables[0].Rows[row]["PublicURL"]);
                             string[] prevNextUrl = aPublicURL.Split('.');
                             string randomnumber1 = Convert.ToString(objds.Tables[0].Rows[row]["Random_number"]);
                             string nextprezip = Convert.ToString(objds.Tables[0].Rows[row]["l_zipcode"]);
                             if (row == PreviousRec)
                             {
                                 ViewBag.previousrecord = randomnumber1;
                                 ViewBag.preproname = prevNextUrl[0].Trim().Replace(" ", "-");
                                 ViewBag.nextprezip = nextprezip;
                             }
                             if (row == NextRec)
                             {
                                 ViewBag.nextrecord = randomnumber1;
                                 ViewBag.nextprovnme = prevNextUrl[0].Trim().Replace(" ", "-");
                                 ViewBag.nextprezip = nextprezip;

                             }
                         }
                     }
                     else
                     {
                         ViewBag.previousrecord = null;
                         ViewBag.nextrecord = null;

                     }
                 }
             }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "PublicController", "Providerdetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                
            }
         }
         public void DataListCities_DataListZIPCodes_Bind(string zipCode)
         {
             var objCitiesList = new List<clsNearestCities>();
             var objZipCodesList = new List<clsNearestZIPCodes>();
             var obj = new clsNearestCities();

             try
             {
                 obj.GetNearestZIPCodesandNearestCities(zipCode, ref objCitiesList, ref objZipCodesList);
                 if (objCitiesList.Count > 0)
                 {
                     ViewBag.DtlistCities = objCitiesList;
                     ViewBag.Dtlistcitescount = objCitiesList.Count;
                     int rowcount = objCitiesList.Count / 2;
                     ViewBag.rows1 = rowcount;

                 }
                 else
                 {
                     ViewBag.Dtlistcitescount = 0;
                 }
                 if (objZipCodesList.Count > 0)
                 {
                     ViewBag.DtlistZIPCodes = objZipCodesList;
                     ViewBag.DtlistZIPCodescount = objZipCodesList.Count;
                     int rowcount = objZipCodesList.Count / 2;
                     ViewBag.rows2 = rowcount;

                 }
                 else
                 {
                     ViewBag.DtlistZIPCodescount = 0;

                 }
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "DataListCities_DataListZIPCodes_Bind", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }

         }
         public bool CheckingRemoteFileExistance(string strpath)
         {
             var myclient = new WebClient();
             try
             {
                 byte[] responseBytes = myclient.DownloadData(strpath);
                 return true;
             }
             catch (Exception ex)
             {
                 var obj = new clsExceptionLog();
                 obj.LogException(ex, "PublicController", "CheckingRemoteFileExistance", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                 return false;
             }
             finally
             {
                 myclient = null;
             }

         }
         public void Upd_EmailStatistics(string providerId)
         {

             try
             {
             var obj1 = new ProviderAdvertising();
           var obj = new ProviderAdvertising();
             string stateName = "";
             string cityName = "";
             if ((Request.Cookies["State"] != null))
             {
                 if (Request.Cookies["State"].Value != null)
                 {
                     stateName = Request.Cookies["State"].Value.Replace("%20", " ");
                 }
             }
             if ((Request.Cookies["City"] != null))
             {
                 if (Request.Cookies["City"].Value != null)
                 {
                     cityName = Request.Cookies["City"].Value.Replace("%20", " ");
                 }
             }
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "Upd_EmailStatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }
              //obj1.Provider_ID =providerId;
              //   obj1.SiteStatic_ID = "17";
              //   obj1.State_Name = stateName;
              //   obj1.CityNm = cityName;
              //   obj.CountingProviderVisitToHisProfile(obj1);
            
            
            
         }
         public void Upd_PrintProfileStatistics(int providerId)
         {
             try
             {
             var obj1 = new ProviderAdvertising();

             string stateName = "";
             string cityName = "";
             if ((Request.Cookies["State"] != null))
             {
                 if (Request.Cookies["State"].Value != null)
                 {
                     stateName = Request.Cookies["State"].Value.Replace("%20", " ");
                 }
             }
             if ((Request.Cookies["City"] != null))
             {
                 if (Request.Cookies["City"].Value != null)
                 {
                     cityName = Request.Cookies["City"].Value.Replace("%20", " ");
                 }
             }
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "Upd_PrintProfileStatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }
             //try
             //{
             //    obj1.Provider_ID = Convert.ToString(providerId);
             //    obj1.SiteStatic_ID = "16";
             //    obj1.State_Name = stateName;
             //    obj1.CityNm = cityName;
             //    obj1.CountingProviderVisitToHisProfile(obj1);
             //}
             //catch (Exception ex)
             //{
             //    var obj = new clsExceptionLog();
             //    obj.LogException(ex, "PublicController", "Upd_PrintProfileStatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             //}
             //finally
             //{
             //    obj1 = null;

             //}
         }
         public void Updstatistics(int providerId, int videoId)
         {
             try
             {
             string stateName = "";
             string cityName = "";
             if ((Request.Cookies["State"] != null))
             {
                 if (Request.Cookies["State"].Value != null)
                 {
                     stateName = Request.Cookies["State"].Value.Replace("%20", " ");
                 }
             }
             if ((Request.Cookies["City"] != null))
             {
                 if (Request.Cookies["City"].Value != null)
                 {
                     cityName = Request.Cookies["City"].Value.Replace("%20", " ");
                 }
             }
             var obj1 = new ProviderAdvertising();

             obj1.UpdAudioVideoStatistics(providerId, 2, videoId, 6, stateName, cityName);
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "Updstatistics", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }
         }

         public string GetProviderYouTubeEmbeddedText(int videoid)
         {
             try
             {
             var objcommon = new clsCommonFunctions();
             IDataParameter[] objInParam = { new SqlParameter("@In_Provider_Greeting_ID", videoid) };
             objcommon.AddInParameters(objInParam);
             SqlDataReader dRinfo = objcommon.GetDataReader("Help_dbo.st_Provider_Youtube_EmbedText");
             if (dRinfo.Read())
             {
                 return Convert.ToString(dRinfo["Embed_Text"]);
             }
             }
             catch (Exception ex)
             {
                 var clsexp = new clsExceptionLog();

                 clsexp.LogException(ex, "PublicController", "GetProviderYouTubeEmbeddedText", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             }
             return null;
         }
         public ActionResult _MasterPage()
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             return View();
         }
         public ActionResult _Reg_New_Top_Section()
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             return View();
         }
         public ActionResult _New_bottom_section()
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             return View();
         }
         public ActionResult ProviderSchedule(string randomnumber, string Therapistname, string date)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
             Providerdetails(randomnumber);
             if (Request.Url != null)
             {
                 string url = Request.Url.ToString();
                 string[] strurl = url.Split('/');
                 //var clsWebConfigsettings = new clsWebConfigsettings();
                 if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                 {
                     if (url.Contains("localhost:"))
                     {
                         ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "images/raight-heading.gif";
                     }
                     else if (url.Contains("vbv"))
                     {
                         ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/raight-heading.gif";
                     }

                 }
                 else
                 {
                     ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/raight-heading.gif";

                 }
             }

            // int prov_id = id;

             DataSet dsAppointmentsList = new DataSet();
             string Fromdate = null;
             string Todate = null;

             if (date != null)
             {
                 Fromdate = date.Replace('-','/');
                 Todate = Convert.ToDateTime(Fromdate).AddDays(+4).ToShortDateString();
             }
             else
             {
                 Fromdate = DateTime.Now.ToShortDateString();
                 Todate = DateTime.Now.AddDays(+4).ToShortDateString();
             }
             ViewBag.nextdate = Convert.ToDateTime(Todate).AddDays(+1).ToShortDateString().Replace('/', '-');
             ViewBag.prevdate = Convert.ToDateTime(Fromdate).AddDays(-5).ToShortDateString().Replace('/', '-');
             ViewData["Previoustooltip"] = DateTime.Parse(Fromdate).AddDays(-5).ToShortDateString() + " To " + DateTime.Parse(Fromdate).AddDays(-1).ToShortDateString();
             ViewData["Nexttooltip"] = DateTime.Parse(Todate).AddDays(+1).ToShortDateString() + " To " + DateTime.Parse(Todate).AddDays(+5).ToShortDateString();
            

            // ViewBag.businessname = Therapistname;
             Therapistname = ViewBag.BusinessName;
             ViewBag.id = randomnumber;
             int duration = 60;

             dsAppointmentsList = GetAppointment.GetProviderOpenSlots(Convert.ToInt32(TempData["Providerid1"]), Fromdate, Todate, "07:00 AM", "07:00 PM", duration);
             int day1 = 0;
             int day2 = 0;
             int day3 = 0;
             int day4 = 0;
             int day5 = 0;


             if (date != null)
             {
                 day1 = (int)Convert.ToDateTime(Fromdate).DayOfWeek + 1;
                 day2 = (int)Convert.ToDateTime(Fromdate).AddDays(+1).DayOfWeek + 1;
                 day3 = (int)Convert.ToDateTime(Fromdate).AddDays(+2).DayOfWeek + 1;
                 day4 = (int)Convert.ToDateTime(Fromdate).AddDays(+3).DayOfWeek + 1;
                 day5 = (int)Convert.ToDateTime(Fromdate).AddDays(+4).DayOfWeek + 1;
             }
             else
             {
                 day1 = (int)DateTime.Now.DayOfWeek + 1;
                 day2 = (int)DateTime.Now.AddDays(+1).DayOfWeek + 1;
                 day3 = (int)DateTime.Now.AddDays(+2).DayOfWeek + 1;
                 day4 = (int)DateTime.Now.AddDays(+3).DayOfWeek + 1;
                 day5 = (int)DateTime.Now.AddDays(+4).DayOfWeek + 1;
             }
            ViewBag.practicename = "Schedules for  " + Therapistname.Replace("-", " ") + "  from  " + Fromdate + "  -  " + Todate;

            string backcolorTr = string.Empty;


            string strQuery2 = "wday = '" + day1 + "'";
            DataRow[] drFilterRows2 = dsAppointmentsList.Tables[0].Select(strQuery2);
            DataSet dsAppointmentsListResults2 = dsAppointmentsList.Clone();
            foreach (DataRow dr in drFilterRows2)
                dsAppointmentsListResults2.Tables[0].ImportRow(dr);

            StringBuilder strdayappt = new StringBuilder();
            strdayappt =
                strdayappt.Append(
                    "<table id='tblAppointments' cellspacing='1' cellpadding='4' width='100%' style='background-color:#d0e4f4'>");
            strdayappt = strdayappt.Append(" <tr>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12.5%;'><a href='../Schedule/Schedulespecs?apptdate=" + Convert.ToDateTime(Fromdate).ToString("dddd - MM/dd") + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + Convert.ToDateTime(Fromdate).ToString("dddd - MM/dd") + "</a></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12.5%;'><a href='../Schedule/Schedulespecs?apptdate=" + Convert.ToDateTime(Fromdate).AddDays(+1).ToString("dddd - MM/dd") + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + Convert.ToDateTime(Fromdate).AddDays(+1).ToString("dddd - MM/dd") + "</a></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12.5%;'><a href='../Schedule/Schedulespecs?apptdate=" + Convert.ToDateTime(Fromdate).AddDays(+2).ToString("dddd - MM/dd") + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + Convert.ToDateTime(Fromdate).AddDays(+2).ToString("dddd - MM/dd") + "</a></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12.5%;'><a href='../Schedule/Schedulespecs?apptdate=" + Convert.ToDateTime(Fromdate).AddDays(+3).ToString("dddd - MM/dd") + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + Convert.ToDateTime(Fromdate).AddDays(+3).ToString("dddd - MM/dd") + "</a></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12.5%;'><a href='../Schedule/Schedulespecs?apptdate=" + Convert.ToDateTime(Fromdate).AddDays(+4).ToString("dddd - MM/dd") + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + Convert.ToDateTime(Fromdate).AddDays(+4).ToString("dddd - MM/dd") + "</a></td>");

            strdayappt = strdayappt.Append("</tr>");
            strdayappt = strdayappt.Append("<tr id='trAppointments' style='height:20px;'>");
            for (int i = 1; i <= 5; i++)
            {
                strdayappt = strdayappt.Append("<td valign='top'><table cellspacing='1' cellpadding='10' width='100%' style='background-color:#d0e4f4'>");
                string j = null;
                if (i == 1)
                {
                    j = Convert.ToString(day1);
                }
                else if (i == 2)
                {
                    j = Convert.ToString(day2);
                }
                else if (i == 3)
                {
                    j = Convert.ToString(day3);
                }
                else if (i == 4)
                {
                    j = Convert.ToString(day4);
                }
                else if (i == 5)
                {
                    j = Convert.ToString(day5);
                }
                string strQuery = "wday = '" + j + "'";
                DataRow[] drFilterRows = dsAppointmentsList.Tables[0].Select(strQuery);
                DataSet dsAppointmentsListResults = dsAppointmentsList.Clone();
                foreach (DataRow dr in drFilterRows)
                    dsAppointmentsListResults.Tables[0].ImportRow(dr);

                foreach (DataRow drappt in dsAppointmentsListResults.Tables[0].Rows)
                {
                    string timperiod = string.Empty;
                    timperiod = Convert.ToString(drappt["AppointmentTime"]);
                    string strdate = Convert.ToDateTime(drappt["Appointmentdate"]).ToShortDateString();

                    strdayappt =
               strdayappt.Append("<tr><td> <a id='atestadd' class='Mytestclss' href='../../Public/AddNewAppointment' onclick=''  title='Click here to add the Appointment'>" + drappt["AppointmentTime"] +
                                 "</a></br></td></tr>");
                }
                    strdayappt = strdayappt.Append("</table></td>");

            }


            strdayappt = strdayappt.Append("</tr>");

            strdayappt = strdayappt.Append("</table>");
            ViewBag.daysch = Convert.ToString(strdayappt);




             return View();
         }
         public ActionResult AddNewAppointment(string id, string apptdate, string time,string randomnumber, int duration, string name)
         {
             //if (Session["UserID"] == null)
             //{
             //    return RedirectToAction("SessionExpire", "Home");
             //}
                ViewData["hdnrandmnm"] = randomnumber;
                Providerdetails(randomnumber);
             FillTechnicians(id);
             //var clsWebConfigsettings = new clsWebConfigsettings();
             string Requesturl = null;
             if (Request.Url != null) Requesturl = Request.Url.ToString();
             var strurl = Requesturl.Split('/');
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
             ViewBag.domain = domain;
             if (clsWebConfigsettings.GetConfigSettingsValue("ShowTechnicians").ToUpper() == "YES")
             {
                 ViewBag.Showtech = "Y";
                 ViewData["hdnshotech"] = "Y";
             }
             else
             {
                 ViewBag.Showtech = "N";
                 ViewData["hdnshotech"] = "N";
             }
             ViewData["hdnapptdate"] = apptdate;
             ViewData["hdntime"] = time;
             ViewData["hdnEleid"] = id;
             ViewData["hdnduration"] = duration;
             ViewBag.id1 = id;
             ViewBag.Therapistname = name;

             var reg9 = clsCommonCClist.GetStates();

             //List<clsCountry> objstates1 = new List<clsCountry>();
             //objstates1 = clsCountry.GetStatesByCountryId(1);
             //IList<SelectListItem> _result3 = new List<SelectListItem>();
             //if (objstates1.Count > 0)
             //{
             //    for (int i = 0; i <= objstates1.Count - 1; i++)
             //    {
             //        _result3.Add(new SelectListItem
             //        {
             //            Text = objstates1[i].StateFullName,
             //            Value = Convert.ToString(objstates1[i].StateId)
             //        });
             //    }
             //}
             //IList<SelectListItem> _result4 = new List<SelectListItem>();
             //_result4.Add(new SelectListItem
             //{
             //    Text = "--Select City--",
             //    Value = "0",
             //    Selected = true
             //});

             //StateCity reg9 = new StateCity();
             //reg9 = new StateCity
             //{
             //    StateList = _result3,
             //    CityList = _result4
             //};

             return View(reg9);
         }
        [HttpPost()]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
         public JsonResult AddNewAppointment(Models.AddAppointmentModel addAppointment,GetAppointment obj)
         {
             if (Request.IsAjaxRequest() && Session["UserID"] == null)
             {
                 return Json(JsonResponseFactory.ErrorResponse("expire"), JsonRequestBehavior.DenyGet);
             }
             if (Request["txtimgKey"] != Convert.ToString(HttpContext.Session["captchastring"]))
             {
                 return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code"), JsonRequestBehavior.DenyGet);
             }
             if (Convert.ToString(Request["hdncustmexists"]) == "N")
             {
                 PatientRegistration PatInfo = new PatientRegistration();
                
                     PatInfo.HomePhone = null;
              
                     PatInfo.WPhone = null;

                     if (string.IsNullOrEmpty(Request["txtMobile1"]) & string.IsNullOrEmpty(Request["txtMobile2"]) & string.IsNullOrEmpty(Request["txtMobile3"]))
                 {
                     PatInfo.MPhone = null;
                 }
                 else
                 {
                     PatInfo.MPhone = Request["txtMobile1"] + Request["txtMobile2"] + Request["txtMobile3"];
                 }
                     if (Request["txtMiddleName"] != null & Request["txtMiddleName"] != "" & Request["txtMiddleName"] != "MI")
                 {
                     PatInfo.MI = Request["txtMiddleName"];
                 }
                 else
                 {
                     PatInfo.MI = null;
                 }
                 //PatInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                     PatInfo.Provider_ID = Convert.ToInt32(Request["hdnproid"]);
                 PatInfo.Prefix = null;
                 PatInfo.FirstName = (!string.IsNullOrEmpty(Request["txtFirstName"].Trim()) ? Request["txtFirstName"].Trim() : null);
                 PatInfo.LastName = (!string.IsNullOrEmpty(Request["txtLastName"].Trim())  ? Request["txtLastName"].Trim() : null);
                 PatInfo.Suffix = null;
                 PatInfo.PatientEmail = (Request["txtEmail"] != "" ? Request["txtEmail"] : null);
                 PatInfo.DrivingLicenceNo = null;
                 PatInfo.Address1 = (!string.IsNullOrEmpty(Request["txtAddress1"]) ? Request["txtAddress1"] : null);
                 PatInfo.Address2 = (!string.IsNullOrEmpty(Request["txtAddress2"]) ? Request["txtAddress2"] : null);
                 PatInfo.City_ID = (Convert.ToInt32(Request["DDCity"]) != 0 ? Convert.ToInt32(Request["DDCity"]) : 0);
                 PatInfo.State_ID = (Convert.ToInt32(Request["DDState"]) != 0 ? Convert.ToInt32(Request["DDState"]) : 0);
                 PatInfo.Country_ID = 1;
                 PatInfo.ZIP = (!string.IsNullOrEmpty(Request["txtZip"]) ? Request["txtZip"] : null);
                 PatInfo.UserId = Convert.ToInt32(Request["hdnprologinid"]);
                 string Out_Msg = null;
                 string Out_login_id = null;
                 MowerHelper.Models.BLL.Patients.PatientRegistration.Insert_Patinet_FT(PatInfo, ref Out_Msg, ref Out_login_id);
                 if (Out_Msg != "" && Out_Msg != null && Out_login_id != "" && Out_login_id!=null)
                 {
                     obj.Customerexists = "Y";
                     obj.Provider_ID = Convert.ToInt32(Request["hdnproid"]);
                     addAppointment.FromReference_ID = Convert.ToInt32(Request["hdnproid"]);
                     addAppointment.FromReferenceLogin_ID = Convert.ToInt32(Request["hdnprologinid"]);
                     addAppointment.FromReferenceType_ID = 2;
                     addAppointment.AppointmentType_ID = 1;
                     addAppointment.Patient_ID = Convert.ToInt32(Out_Msg);
                     addAppointment.ToReference_ID = Convert.ToInt32(Out_Msg);
                     addAppointment.ToReferenceLogin_ID = Convert.ToInt32(Out_login_id);
                     addAppointment.ToReferenceType_ID = 3;
                     //addAppointment.ToReference_IDs = null;
                     addAppointment.StartDate = Request["hdnapptdate"];
                     addAppointment.EndDate = null;
                     addAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                     //addAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                     // string strappointmentTime = Request["cbotime_SelectedText"];
                     addAppointment.AppointmentTime = Request["hdntime"];
                     addAppointment.Duration = Convert.ToInt32(Request["hdnduration"]);
                     addAppointment.AppointmentStatus_ID = 1;
                     addAppointment.CreatedBy = Convert.ToInt32(Request["hdnprologinid"]);
                     addAppointment.IsValidate_Ind = null;
                     //if (Convert.ToString(Request["hdnshotech"]) == "Y" && Convert.ToInt32(Request["hdntechcnt"]) > 1)
                     //{
                     //    addAppointment.Technician_ids = "," + Convert.ToString(Request["combobox1"]) + ",";
                     //    addAppointment.TechnicianPositions = "," + Convert.ToString(Request["combobox1_SelectedIndex"]) + ",";
                     //}
                     //else
                     //{
                     //    addAppointment.Technician_ids = Request["hdntechids"];
                     //    addAppointment.TechnicianPositions =","+ "0"+",";
                     //}
                   string   strMessage1 = string.Empty;
                     int? AppointmentID = 0;
                     strMessage1 = addAppointment.InsertPublicAppointment(addAppointment, ref AppointmentID);
                     CallNotifications(obj.Provider_ID);
                     return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                 }
             }
             else
             {
                 string strMessage = null;
                 string email = Request["txtEmail"];
                 string providerid = Request["hdnEleid"];
                 string Phonenumber = Request["txtMobile1"] + Request["txtMobile2"] + Request["txtMobile3"];
                 List<GetAppointment> objGetCustomerdetails = GetAppointment.GetCustomerdetails(email, Phonenumber, providerid);
                 // AddAppointmentModel addAppointment = new AddAppointmentModel(); 
                 if (objGetCustomerdetails.Count > 0)
                 {
                     if (objGetCustomerdetails[0].Customerexists == "Y")
                     {
                         obj.Customerexists = objGetCustomerdetails[0].Customerexists;
                         obj.Provider_ID = objGetCustomerdetails[0].Provider_ID;
                         addAppointment.FromReference_ID = objGetCustomerdetails[0].Provider_ID;
                         addAppointment.FromReferenceLogin_ID = objGetCustomerdetails[0].ProviderLogin_ID;
                         addAppointment.FromReferenceType_ID = 2;
                         addAppointment.AppointmentType_ID = 1;
                         addAppointment.Patient_ID = objGetCustomerdetails[0].Patient_ID;
                         addAppointment.ToReference_ID = objGetCustomerdetails[0].Patient_ID;
                         addAppointment.ToReferenceLogin_ID = objGetCustomerdetails[0].PatientLogin_ID;
                         //if (Convert.ToString(Request["hdnshotech"]) == "Y" && Convert.ToInt32(Request["hdntechcnt"]) > 1)
                         //{
                         //    addAppointment.Technician_ids = "," + Convert.ToString(Request["combobox1"]) + ",";
                         //    addAppointment.TechnicianPositions = "," + Convert.ToString(Request["combobox1_SelectedIndex"]) + ",";
                         //}
                         //else
                         //{
                         //    addAppointment.Technician_ids = objGetCustomerdetails[0].Technician_ids;
                         //    addAppointment.TechnicianPositions =","+ "0"+",";
                         //}
                         addAppointment.ToReferenceType_ID = 3;
                      //   addAppointment.ToReference_IDs = null;
                         addAppointment.StartDate = Request["hdnapptdate"];
                         addAppointment.EndDate = null;
                         addAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                         //addAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                         // string strappointmentTime = Request["cbotime_SelectedText"];
                         addAppointment.AppointmentTime = Request["hdntime"];
                         addAppointment.Duration = Convert.ToInt32(Request["hdnduration"]);
                         addAppointment.AppointmentStatus_ID = 1;
                         addAppointment.CreatedBy = Convert.ToInt32(objGetCustomerdetails[0].ProviderLogin_ID);
                         addAppointment.IsValidate_Ind = null;
                         strMessage = string.Empty;
                         int? AppointmentID = 0;
                         strMessage = addAppointment.InsertPublicAppointment(addAppointment, ref AppointmentID);
                         CallNotifications(obj.Provider_ID);
                         return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                     }
                     else if (objGetCustomerdetails[0].Customerexists == "N")
                     {
                         obj.Provider_ID = objGetCustomerdetails[0].Provider_ID;
                         obj.Technician_ids = objGetCustomerdetails[0].Technician_ids;
                         obj.ProviderLogin_ID = objGetCustomerdetails[0].ProviderLogin_ID;
                         obj.Customerexists = objGetCustomerdetails[0].Customerexists;
                         //string Out_Msg = "You are the new customer to our site. We need some more Information.";
                         return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                     }
                 }
             }
           
             return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
         }

        public ActionResult ElectricianSchedule(string randomnumber, string Therapistname, string date)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Providerdetails(randomnumber);
            if (Request.Url != null)
            {
                string url = Request.Url.ToString();
                string[] strurl = url.Split('/');
                //var clsWebConfigsettings = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    if (url.Contains("localhost:"))
                    {
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + "images/raight-heading.gif";
                    }
                    else if (url.Contains("vbv"))
                    {
                        ViewBag.imgsrc24 = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "images/raight-heading.gif";
                    }

                }
                else
                {
                    ViewBag.imgsrc24 = clsWebConfigsettings.GetConfigSettingsValue("providerimagestageurl") + "images/raight-heading.gif";

                }
            }
            if (Request.Url != null) Requesturl = Request.Url.ToString();
            var strurl1 = Requesturl.Split('/');
            //var clsWebConfigsettings = new clsWebConfigsettings();
            string domain = null;
            if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            {
                if (Requesturl.Contains("localhost:"))
                {
                    domain = strurl1[0] + "//" + strurl1[2] + "/";

                }
                else if (Requesturl.Contains("vbv"))
                {
                    domain = strurl1[0] + "//" + strurl1[2] + "/" + strurl1[3] + "/";

                }
            }
            else
            {
                domain = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal");
            }
            //int prov_id = id;

            DataSet dsAppointmentsList = new DataSet();
            string Fromdate = null;
            string Todate = null;

            if (date != null)
            {
                Fromdate = date.Replace('-', '/');
                Todate = Convert.ToDateTime(Fromdate).AddDays(+4).ToShortDateString();
            }
            else
            {
                Fromdate = DateTime.Now.ToShortDateString();
                Todate = DateTime.Now.AddDays(+4).ToShortDateString();
            }
            ViewBag.nextdate = Convert.ToDateTime(Todate).AddDays(+1).ToShortDateString().Replace('/', '-');
            ViewBag.prevdate = Convert.ToDateTime(Fromdate).AddDays(-5).ToShortDateString().Replace('/', '-');
            ViewData["Previoustooltip"] = DateTime.Parse(Fromdate).AddDays(-5).ToShortDateString() + " To " + DateTime.Parse(Fromdate).AddDays(-1).ToShortDateString();
            ViewData["Nexttooltip"] = DateTime.Parse(Todate).AddDays(+1).ToShortDateString() + " To " + DateTime.Parse(Todate).AddDays(+5).ToShortDateString();
            ViewBag.Previoustooltip = ViewData["Previoustooltip"];
            ViewBag.Nexttooltip = ViewData["Nexttooltip"];

           // ViewBag.businessname = Therapistname;
            Therapistname = ViewBag.BusinessName;
            ViewBag.id = randomnumber;
            int duration = 60;

            dsAppointmentsList = GetAppointment.GetProviderOpenSlots(Convert.ToInt32(TempData["Providerid1"]), Fromdate, Todate, "07:00 AM", "07:00 PM", duration);
            int day1 = 0;
            int day2 = 0;
            int day3 = 0;
            int day4 = 0;
            int day5 = 0;


            if (date != null)
            {
                day1 = (int)Convert.ToDateTime(Fromdate).DayOfWeek + 1;
                day2 = (int)Convert.ToDateTime(Fromdate).AddDays(+1).DayOfWeek + 1;
                day3 = (int)Convert.ToDateTime(Fromdate).AddDays(+2).DayOfWeek + 1;
                day4 = (int)Convert.ToDateTime(Fromdate).AddDays(+3).DayOfWeek + 1;
                day5 = (int)Convert.ToDateTime(Fromdate).AddDays(+4).DayOfWeek + 1;
            }
            else
            {
                day1 = (int)DateTime.Now.DayOfWeek + 1;
                day2 = (int)DateTime.Now.AddDays(+1).DayOfWeek + 1;
                day3 = (int)DateTime.Now.AddDays(+2).DayOfWeek + 1;
                day4 = (int)DateTime.Now.AddDays(+3).DayOfWeek + 1;
                day5 = (int)DateTime.Now.AddDays(+4).DayOfWeek + 1;
            }
            ViewBag.practicename = "Schedules for  " + Therapistname.Replace("-", " ") + "  from  " + Fromdate + "  -  " + Todate;

            string backcolorTr = string.Empty;
            string colorfont = string.Empty;

            string strQuery2 = "wday = '" + day1 + "'";
            DataRow[] drFilterRows2 = dsAppointmentsList.Tables[0].Select(strQuery2);
            DataSet dsAppointmentsListResults2 = dsAppointmentsList.Clone();
            foreach (DataRow dr in drFilterRows2)
                dsAppointmentsListResults2.Tables[0].ImportRow(dr);

            StringBuilder strdayappt = new StringBuilder();
            strdayappt =
                strdayappt.Append(
                    "<table id='tblAppointments' cellspacing='1' cellpadding='1' width='100%' style='background-color:#d0e4f4;' >");
            strdayappt = strdayappt.Append(" <tr>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).AddDays(+1).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).AddDays(+2).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'>" + Convert.ToDateTime(Fromdate).AddDays(+3).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'>" + Convert.ToDateTime(Fromdate).AddDays(+4).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("</tr>");
            strdayappt = strdayappt.Append("</table>");
            strdayappt =
               strdayappt.Append(
                   "<table id='tblAppointments1' cellspacing='0' cellpadding='0' width='100%' >");

            strdayappt = strdayappt.Append("<tr id='trAppointments' style='background: white'>");
            for (int i = 1; i <= 5; i++)
            {
                strdayappt = strdayappt.Append("<td valign='top' style='background: white'><table cellspacing='1' cellpadding='0' width='100%' style='background-color:#d0e4f4'>");
                string j = null;
                if (i == 1)
                {
                    j = Convert.ToString(day1);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                else if (i == 2)
                {
                    j = Convert.ToString(day2);
                    backcolorTr = "#FFFF99";
                    colorfont = "#044f96";
                }
                else if (i == 3)
                {
                    j = Convert.ToString(day3);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                else if (i == 4)
                {
                    j = Convert.ToString(day4);
                    backcolorTr = "#FFFF99";
                    colorfont = "#044f96";
                }
                else if (i == 5)
                {
                    j = Convert.ToString(day5);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                string strQuery = "wday = '" + j + "'";
              
                DataRow[] drFilterRows = dsAppointmentsList.Tables[0].Select(strQuery);
                DataSet dsAppointmentsListResults = dsAppointmentsList.Clone();
                foreach (DataRow dr in drFilterRows)
                    dsAppointmentsListResults.Tables[0].ImportRow(dr);

                foreach (DataRow drappt in dsAppointmentsListResults.Tables[0].Rows)
                {
                    string timperiod = string.Empty;
                    timperiod = Convert.ToString(drappt["AppointmentTime"]);
                    string strdate = Convert.ToDateTime(drappt["Appointmentdate"]).ToShortDateString();

                    strdayappt =
                   strdayappt.Append("<tr ><td id='tcIntervel' style='background-color: " + backcolorTr + ";width: 13%; padding:10px; " +
                                     "' align='center' valign='top'> <a class='Mytestadd' style='color:" + colorfont + ";text-decoration:none;font-weight:bold; font-size:small;' href='"+domain+"/Public/AddNewAppointment?id=" +
                                     Convert.ToInt32(TempData["Providerid1"]) + "" + "&apptdate=" + "" + strdate + "" + "&time=" + "" + timperiod + "" + "&randomnumber=" + randomnumber+"" + "&duration=" + "" + duration + "" + "&name=" + "" + Therapistname + "'  title='Click here to add the Appointment'>" + drappt["AppointmentTime"] +
                                     "</a></br></td></tr>");
                }
                strdayappt = strdayappt.Append("</table></td>");

            }
            strdayappt = strdayappt.Append("</tr>");

            strdayappt = strdayappt.Append("</table>");
            ViewBag.daysch = Convert.ToString(strdayappt);
            return View();
        }
        public void FillTechnicians(string id)
        {
            try
            {
            TechniciansInfo gettechnician = new TechniciansInfo();
            List<TechniciansInfo> objlist = new List<TechniciansInfo>();

            objlist = gettechnician.GetTechnicianList(Convert.ToInt32(id), null);
                ViewBag.protechid = objlist[0].Technician_id;
           
            ViewBag.techcnt = objlist.Count;
            ViewData["hdntechcnt"] = objlist.Count;
            if (objlist.Count == 1)
            {
                ViewBag.tecid = objlist[0].Technician_id;
            }
            ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Technician_id", "Technician_Name");
            ViewData["combobox1"] = custlist;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "FillTechnicians", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            //ViewData["combobox2_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
        }
        void CallNotifications(int ProviderID)
        {
            ///create the puchbroker object
var push = new PushBroker();
//Wire up the events for all the services that the broker registers
push.OnNotificationSent += NotificationSent;
push.OnChannelException += ChannelException;
push.OnServiceException += ServiceException;
push.OnNotificationFailed += NotificationFailed;
push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
push.OnChannelCreated += ChannelCreated;
push.OnChannelDestroyed += ChannelDestroyed;
            //List<Device> rows = new List<Device>(CommonMethods.entity.Devices.ToList());
            //foreach (Device row in rows)
            //{
            //    if (row.devicename == "ios")
            //    {
                    //-------------------------
                    // APPLE NOTIFICATIONS
                    //-------------------------
                    //Configure and start Apple APNS
                    // IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to
                    //generate one for connecting to Sandbox, and one for connecting to Production.  You must
                    // use the right one, to match the provisioning profile you build your
                    //   app with!
                    try
                    {
                        var appleCert = System.IO.File.ReadAllBytes(Server.MapPath("../Resources/pushnotification.p12"));
                        //IMPORTANT: If you are using a Development provisioning Profile, you must use
                        // the Sandbox push notification server 
                        //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as
                        // 'false')
                        //  If you are using an AdHoc or AppStore provisioning profile, you must use the 
                        //Production push notification server
                        //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 
                        //'true')
                        push.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, "applemac#123"));
                        //Extension method
                        //Fluent construction of an iOS notification
                        //IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets
                        // generated within your iOS app itself when the Application Delegate
                        //  for registered for remote notifications is called, 
                        // and the device token is passed back to you
                       // push.QueueNotification<
                        push.QueueNotification(new AppleNotification()
                                                    .ForDeviceToken("4948ff3186f449a420288545560e03b7656d1886410feb24824b313af9d8af14")//the recipient device id
                                                    .WithAlert("You Have Pending Appointment Requests")//the message
                                                    .WithBadge(1)
                                                    .WithSound("sound.caf")
                                                    .WithCustomItem("ProviderID",ProviderID)
                                                    
                                                    );


                    }
                    catch (Exception ex)
                    {
                        clsExceptionLog obj = new clsExceptionLog();
                        obj.LogException(ex, "PublicController", "CallNotifications", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session, null);
                    }
                    push.StopAllServices(waitForQueuesToFinish: true); 
                //}
            //}
        }
       
        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            try
            {
            //Currently this event will only ever happen for Android GCM
            Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "DeviceSubscriptionChanged", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        static void NotificationSent(object sender, INotification notification)
        { 
            try
            {
            Console.WriteLine("Sent: " + sender + " -> " + notification);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "NotificationSent", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {
            try
            {
            Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "NotificationFailed", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        static void ChannelException(object sender, IPushChannel channel, Exception exception)
        {
            try
            {
            Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "ChannelException", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        static void ServiceException(object sender, Exception exception)
        {
            try
            {
            Console.WriteLine("Service Exception: " + sender + " -> " + exception);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "ServiceException", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        { 
          try
          {
            Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
            
           }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "DeviceSubscriptionExpired", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
           }
        }

        static void ChannelDestroyed(object sender)
        { try
            {
            Console.WriteLine("Channel Destroyed for: " + sender);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "ChannelDestroyed", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        static void ChannelCreated(object sender, IPushChannel pushChannel)
        { 
            try
            {
            Console.WriteLine("Channel Created for: " + sender);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "ChannelCreated", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //Currently it will raise only for android devices
        //static void DeviceSubscriptionChanged(object sender,
        //string oldSubscriptionId, string newSubscriptionId, INotification notification)
        //{
        //    //Do something here
        //}

        ////this even raised when a notification is successfully sent
        //static void NotificationSent(object sender, INotification notification)
        //{
        //    //Do something here
        //}

        ////this is raised when a notification is failed due to some reason
        //static void NotificationFailed(object sender,
        //INotification notification, Exception notificationFailureException)
        //{
        //    //Do something here
        //}

        ////this is fired when there is exception is raised by the channel
        //static void ChannelException
        //    (object sender, IPushChannel channel, Exception exception)
        //{
        //    //Do something here
        //}

        ////this is fired when there is exception is raised by the service
        //static void ServiceException(object sender, Exception exception)
        //{
        //    //Do something here
        //}

        ////this is raised when the particular device subscription is expired
        //static void DeviceSubscriptionExpired(object sender,
        //string expiredDeviceSubscriptionId,
        //    DateTime timestamp, INotification notification)
        //{
        //    //Do something here
        //}

        ////this is raised when the channel is destroyed
        //static void ChannelDestroyed(object sender)
        //{
        //    //Do something here
        //}

        ////this is raised when the channel is created
        //static void ChannelCreated(object sender, IPushChannel pushChannel)
        //{
        //    //Do something here
        //}
    }
}