using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using PayPal.Tenniscoach;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.ProviderRegistration;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;
using System.Data.Entity.Core.Objects;
using System.Linq;
namespace MowerHelper.Controllers
{
    [App_Start.RequireHttpsByConfig]
    public class RegistrationController : Controller
    {
        //  string strContent = "";
        //  int intCounter;
        // private const int StartingYear = 2016;
        MowerHelperEntities context = new MowerHelperEntities();
        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(string Promocode = null)
        {
          

            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Show_license").ToUpper() == "YES")
            {
                ViewBag.Show_license = "Y";
            }
            else
            {
                ViewBag.Show_license = "N";
            }
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            //var reg1 = clsCommonCClist.GetStates();

            //var objstates = clsCountry.GetStatesByCountryId(1);
            //IList<SelectListItem> result1 = new List<SelectListItem>();

            //if (objstates.Count > 0)
            //{
            //    for (int i = 0; i <= objstates.Count - 1; i++)
            //    {
            //        result1.Add(new SelectListItem
            //        {
            //            Text = objstates[i].StateFullName,
            //            Value = Convert.ToString(objstates[i].StateId)
            //        });
            //    }
            //}
            //IList<SelectListItem> result2 = new List<SelectListItem>();
            //result2.Add(new SelectListItem
            //{
            //    Text = "--Select City--",
            //    Value = "0",
            //    Selected = true
            //});
            //var reg1 = new StateCity
            //  {
            //      StateList = result1,
            //      CityList = result2
            //  };

            //var objcommonfunction = new clsCommonFunctions();
            //  ViewBag.captcha = objcommonfunction.GetCaptcha("6");

            //var description = new Referrals { FieldIDString = "19" };
            //var ds = Referrals.List_field_description(description.FieldIDString);
            var result = context.st_list_module_fielddescription("19,23").ToList();
            if(result.Count>0)
            {
                ViewBag.licensemsg =!string.IsNullOrEmpty(result[0].description)? result[0].description:null;
                   ViewBag.referralmsg = !string.IsNullOrEmpty(result[1].description) ? result[1].description : null;
            }
            // var ds = Referrals.List_field_description("19,23");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ViewBag.licensemsg = Convert.ToString(ds.Tables[0].Rows[0][3]);
            //    ViewBag.referralmsg = Convert.ToString(ds.Tables[0].Rows[1][3]);
            //}
            //var description1 = new Referrals { FieldIDString = "23" };
            //var ds1 = Referrals.List_field_description(description1.FieldIDString);
            //var ds1 = Referrals.List_field_description("23");
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    ViewBag.referralmsg = Convert.ToString(ds1.Tables[0].Rows[0][3]);
            //}
            if (Promocode != null)
            {
                ViewBag.Promocode = Promocode;
            }
            else
            {
                ViewBag.Promocode = null;
            }
          
            return View("Index");
        }
        public ActionResult Termsandconditions()
        {
            var objcommonfunction = new clsCommonFunctions();
            //string strTerms = null;
            var strTerms = context.st_ADMIN_GET_SitePages(null, 55, null).ToList();
            //   objcommonfunction.GetSiteInfo(null, 55, null);
            if (strTerms.Count > 0)
            {
                if (!string.IsNullOrEmpty(strTerms[0].Subject_Text))
                {
                    ViewBag.InnerHtml = "<A Name='TOP'></A>" + strTerms[0].Subject_Text;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Index(Reg_CreditCardProcess obj)
        {
            
                if (Session["captchastring"] != null)
                {
                    if (Request["txtimgKey"] != Convert.ToString(HttpContext.Session["captchastring"]))
                    {
                        return Json(JsonResponseFactory.ErrorResponse("You need to enter valid Security Code to register"), JsonRequestBehavior.DenyGet);
                    }
                }
                if (Request["txtreferral"] != null && Request["txtreferral"] != "")
                {
                    string out_Msg = null;
                    Reg_ProvidersDetailInfo.check_validRefferalCode(Request["txtreferral"], ref out_Msg);
                    if (out_Msg != null)
                    {
                        return Json(JsonResponseFactory.ErrorResponse(out_Msg), JsonRequestBehavior.DenyGet);
                    }
                }
                //string strFormatedFirstName = "";
                //string strFormatedLastName = "";
                //strFormatedFirstName = Request["txtFirstName"] ?? null;
                //strFormatedLastName = Request["txtLastName"] ?? null;
                //string[] imgkey1 = Request["hdnkey"].Split(' ');
                //string imgkey2 = imgkey1[0] + imgkey1[1] + imgkey1[2] + imgkey1[3] + imgkey1[4] + imgkey1[5];
                //string imgkey3 = Request["txtimgKey"];
                //if (imgkey2 == imgkey3)
                //{
                //    //return Json(JsonResponseFactory.ErrorResponse("Enter valid Type verification code"), JsonRequestBehavior.DenyGet);
                //}
                //else
                //{
                //    return Json(JsonResponseFactory.ErrorResponse("Enter valid Type verification code"), JsonRequestBehavior.DenyGet);
                //}
                //string _strlicenseno = null;
                //if (Request["txtlicence"] != null)
                //{
                //    _strlicenseno = Request["txtlicence"];
                //}
                //else
                //{
                //    _strlicenseno = null;
                //}

                //string _strTemp = "";
                //string _strWorkPhone = "";
                //string _strTemp1 = "";
                //string _strCellPhone = "";
                //string _strStateID = "";
                //string _strCityID = "";
                //if ((Request["txtWorkPhone1"] != null) & (Request["txtWorkPhone2"] != null) & (Request["txtWorkPhone3"] != null))
                //{
                //    _strTemp = Request["txtWorkPhone1"] + Request["txtWorkPhone2"] + Request["txtWorkPhone3"];
                //}
                //else
                //{
                //    _strTemp = null;
                //}
                //if (!string.IsNullOrEmpty(_strTemp))
                //{
                //    _strWorkPhone = _strTemp;
                //}
                //else
                //{
                //    _strWorkPhone = null;
                //}
                //if ((Request["txtCellPhone1"] != null) & (Request["txtCellPhone2"] != null) & (Request["txtCellPhone3"] != null))
                //{
                //    _strTemp1 = Request["txtCellPhone1"] + Request["txtCellPhone2"] + Request["txtCellPhone3"];
                //}
                //else
                //{
                //    _strTemp1 = null;
                //}
                //if (!string.IsNullOrEmpty(_strTemp1))
                //{
                //    _strCellPhone = _strTemp1;
                //}
                //else
                //{
                //    _strCellPhone = null;
                //}
                //_strStateID = Request["DDState"] ?? null;
                //_strCityID = Request["DDCity"] ?? null;
                ViewBag.Actualpassword = Request["txtPassword"] ?? null;
              
                  string _strWorkPhone = "";
                string _strCellPhone = "";

                if (!string.IsNullOrEmpty(Request["txtWorkPhone1"]) && !string.IsNullOrEmpty(Request["txtWorkPhone2"]) && !string.IsNullOrEmpty(Request["txtWorkPhone3"]))
                {
                    _strWorkPhone = Request["txtWorkPhone1"] + Request["txtWorkPhone2"] + Request["txtWorkPhone3"];
                }
                else
                {
                    _strWorkPhone = null;
                }

                if (!string.IsNullOrEmpty(Request["txtCellPhone1"]) && !string.IsNullOrEmpty(Request["txtCellPhone2"]) && !string.IsNullOrEmpty(Request["txtCellPhone3"]))
                {
                    _strCellPhone = Request["txtCellPhone1"] + Request["txtCellPhone2"] + Request["txtCellPhone3"];
                }
                else
                {
                    _strCellPhone = null;
                }
                string outMsg = null;
                var objmd5 = new VBVMD5CryptoServiceProvider();
                //string hash = objmd5.getMd5Hash(Request["txtPassword"]);
                string outProviderId = null;
                int outPracticeId = 0;
                int outLoginId = 0;
            // var objInsert = new Reg_ProvidersDetailInfo((Request["txtEmail"].Trim() ?? null), objmd5.getMd5Hash(Request["txtPassword"]),
            // Request["txtFirstName"] ?? null, (Request["txtMiddle"].Trim() != "" ? Request["txtMiddle"].Trim() : null), Request["txtLastName"] ?? null, (Request["txtEmail"].Trim() ?? null), (Request["txtAdr1"].Trim() ?? null), (Request["txtAdr2"].Trim() != "" ? Request["txtAdr2"].Trim() : null), Convert.ToInt32(Request["DDCity"] ?? null),
            // Convert.ToInt32(Request["DDState"] ?? null), 1, (Request["txtZip"].Trim() ?? null), _strWorkPhone, "Y", (Request["txtBusinessName"].Trim() != "" ? Request["txtBusinessName"].Trim() : null), "Y", (Request["txtlicence"] ?? null), _strCellPhone);

            //  using (var context = new MowerHelperEntities())
            // {
           
                var Out_Msg = new ObjectParameter("Out_Msg", typeof(string));
               // Out_Msg.ParameterType = ParameterDirection.Output;
                var Out_Provider_ID = new ObjectParameter("Out_Provider_ID", typeof(int));
                var Out_Practice_ID = new ObjectParameter("Out_Practice_ID", typeof(int));
                var Out_Login_ID = new ObjectParameter("Out_Login_ID", typeof(int));
               
            context.St_provider_Ins_NEWFTProviderdetails(Request["txtEmail"].Trim() ?? null,
                    objmd5.getMd5Hash(Request["txtPassword"]), null, Request["txtFirstName"] ?? null,
                    (Request["txtMiddle"].Trim() != "" ? Request["txtMiddle"].Trim() : null),
                    Request["txtLastName"] ?? null, Request["txtEmail"].Trim() ?? null, Request["txtAdr1"].Trim() ?? null,
                    Request["txtAdr2"].Trim() != "" ? Request["txtAdr2"].Trim() : null,
                    Convert.ToInt32(Request["DDCity"] ?? null), Convert.ToInt32(Request["DDState"] ?? null), 1, Request["txtZip"].Trim() ?? null,
                    _strWorkPhone, _strCellPhone, "Y",
                    Request["txtBusinessName"].Trim() != "" ? Request["txtBusinessName"].Trim() : null, "" +
                    "Y", Out_Provider_ID, Out_Msg, Out_Practice_ID, Out_Login_ID, Request["txtlicence"] ?? null, null, "W", null, null).ToList();
                //   res.GetNextResult<>();
                //  res.ToList();
                //foreach (var value in res)
                // {
                //value.

                //  }
                // {

                //result.GetEnumerator();
                //value.loginid
                // context.SaveChanges();
                // outProviderId = res.GetNextResult<Out_Provider_ID>();
                // res.GetNextResult<>
                //foreach (var value in res)
                //{
                //}
                if (Out_Provider_ID.Value != DBNull.Value)
                    {
                        outProviderId = Out_Provider_ID.Value != null ? Out_Provider_ID.Value.ToString() : null;
                    }
                    if (Out_Practice_ID.Value != DBNull.Value)
                    {
                        outPracticeId = Out_Practice_ID.Value != null ? Convert.ToInt32(Out_Practice_ID.Value) : 0;
                    }
                    if (Out_Msg.Value != DBNull.Value)
                    {
                        outMsg = Out_Msg.Value != null ? Out_Msg.Value.ToString() : null;
                    }
                    if (Out_Login_ID.Value != DBNull.Value)
                    {
                        outLoginId = Out_Login_ID.Value != null ? Convert.ToInt32(Out_Login_ID.Value) : 0;
                    }
               // }
                //}
           // }
                // Reg_ProvidersDetailInfo.Insert_NewRegProvidersDetails(objInsert, ref outMsg, ref outProviderId, ref outPracticeId, ref outLoginId);
                obj.Provider_ID = Convert.ToInt32(outProviderId);
                obj.Practice_ID = outPracticeId;
                TempData["Provider_ID"] = Convert.ToInt32(outProviderId);
                TempData["Practice_ID"] = outPracticeId;
                TempData.Keep();
                if (outMsg == null)
                {
                    if (Request["txtreferral"] != null && Request["txtreferral"] != "")
                    {
                        Reg_ProvidersDetailInfo.INS_providerRefferalCode(Request["txtreferral"], Convert.ToInt32(outProviderId));
                        Session.Add("RefferalCode", Request["txtreferral"]);
                    }
                    Session.Add("Practice_ID", outPracticeId);
                    Session.Add("Provider_ID", outProviderId);
                    Session.Add("PWD", Request["txtPassword"]);
                    //Session.Add(
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

                }
                else
                {
                    return Json(JsonResponseFactory.ErrorResponse(outMsg), JsonRequestBehavior.DenyGet);
                }
          
           // return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
        }
        public ActionResult DisplayLogindetails(string Uid1, string Uid2)
        {
            //if (Session["PWD"] != null)
            //{
            //    System.Web.HttpContext.Current.Session.Add("PWD", Session["PWD"]);
            //}
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            string Practice_ID = Session["Practice_ID"] != null ? Session["Practice_ID"].ToString() : null;
            string ProviderID = Session["Provider_ID"] != null ? Session["Provider_ID"].ToString() : null;

            if (Practice_ID != Uid1 & ProviderID != Uid2)
            {
                //TempData["Practice_ID"] = null;
                //TempData["Provider_ID"] = null;
                return RedirectToAction("PageNotFound", "Error");
            }
            ViewBag.Practice_ID = Uid1 ?? null;
            ViewBag.Provider_ID = Uid2 ?? null;
            // Reg_ProviderConfirmation ObgCredn = new Reg_ProviderConfirmation();

            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Uid2));

            if ((obgCredn != null))
            {
                ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                ViewBag.Username = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                //if (!string.IsNullOrEmpty(ObgCredn.Password))
                //{
                //    ViewState("Password") = ViewState("Actualpassword");
                //    lblPassword.Text = "******";
                //    lblPassword.Style.Add("valign", "bottom");
                //}
                //if (!string.IsNullOrEmpty(ObgCredn.Email))
                //{
                //    lblemail.Text = ObgCredn.Email;
                //}
            }
            string days = "Please check your email, we sent an email with an activation link. When you click on the activation link, we will be able to confirm your email address.";
            //string days = "Congratulations! Your email address is verified and your account is successfully activated. You can login now to use Mower Helper-Find an Mower Helper website and mobile applications. Please remember you have $xx$ days for trying Mower Helper help application. Prior to the expiration period please chose either Monthly or Yearly payment option.";
            //Thank you for registering to use www.Dietitian-help.com website.Please use the same login credentials to use our mobile application as well. Mobile application will allow you to manage your business on the go. Please remember you have $xx$ days for trying Dietitian help application. Prior to the expiration period please chose either Monthly or Yearly payment option.";

            // days = days.Replace("$xx$", clsweb.GetConfigSettingsValue("Trial_package_days"));
            ViewBag.Trialperiodmessage = days;
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {
                //Email ActivationCode//
                var objclscommon = new clsCommonFunctions();
                var objMailMessage = new ClsMailMessage();
                string strsubject = null;
                string strbody = null;
                string mailfrom = null;
                string mailto = null;
                string providername = null;
              //  string uname = null;
                string stractivationcode = null;
                IDataParameter[] strParam = { new SqlParameter("@in_email", ViewBag.Username) };
                objclscommon.AddInParameters(strParam);
                SqlDataReader drReader = objclscommon.GetDataReader("Help_dbo.st_get_emailmessage_Activationcodeemail");
                if (drReader.Read())
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(drReader["Subject"])))
                    {
                        strsubject = drReader["Subject"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(drReader["Message_Body"])))
                    {
                        strbody = drReader["Message_Body"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(drReader["Mail_From"])))
                    {
                        mailfrom = drReader["Mail_From"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(drReader["Mail_To"])))
                    {
                        mailto = drReader["Mail_To"].ToString();
                    }
                    //if (!string.IsNullOrEmpty(Convert.ToString(drReader["Full_name"])))
                    //{
                    //    providername = drReader["Full_name"].ToString();
                    //}
                    //if (!string.IsNullOrEmpty(Convert.ToString(drReader["Activationcode"])))
                    //{
                    //    stractivationcode = drReader["Activationcode"].ToString();
                    //}
                    //if (strbody != null)
                    //{
                    //    strbody = strbody.Replace("$ActivationCode$", stractivationcode);
                    //    strbody = strbody.Replace("$ProviderName$", providername);
                    //}
                    if (!string.IsNullOrEmpty(Convert.ToString(drReader["Full_name"])) && !string.IsNullOrEmpty(Convert.ToString(drReader["Activationcode"])))
                    {
                        providername = drReader["Full_name"].ToString();
                        stractivationcode = drReader["Activationcode"].ToString();
                        strbody = strbody.Replace("$ActivationCode$", stractivationcode);
                        strbody = strbody.Replace("$ProviderName$", providername);
                    }
                }

                bool strvalid = objMailMessage.SendMail(mailto, mailfrom, strsubject, strbody, EMailFormats.MailFormatText, EMailPriorities.PriorityNormal);
                string strEmailMessageTransactionId = Convert.ToString(drReader["EmailMessage_Transaction_ID"]);
                if (strvalid == true)
                {
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessageTransactionId) };
                    objclscommon.AddInParameters(strMailParam);
                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }
            }
            return View();
        }
        public void SendEmailtoElectrician(string strTomailid, string strTransaction_ID, string strProvidername, string strtransactiondate, string stramoumt)
        {
            string str_Content = "";
            string str_Subject = "";
            //var objconfig = new clsWebConfigsettings();
            if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
            {
                try
                {
                var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(56, 0);
                if ((EMO != null))
                {
                    str_Content = EMO.Msg_Body ?? null;
                    str_Subject = EMO.Msg_Subject ?? null;
                    //ViewBag.Subject = EMO.Msg_Subject ?? null;
                    //ViewBag.Content = EMO.Msg_Body ?? null;
                }
               
                if (str_Content != null)
                {
                    if (strProvidername != null)
                    {
                        str_Content = str_Content.Replace("$ProviderName$", strProvidername);
                    }
                    str_Content = str_Content.Replace("$CSRName$", "Mower Helper");
                    str_Content = str_Content.Replace("$transactionnumber$", strTransaction_ID);
                    if (strtransactiondate != null)
                    {
                        str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                    }
                    str_Content = str_Content.Replace("$ServiceContent$", "credit card verification process");
                    if (stramoumt != null)
                    {
                        double str_amoumt = Convert.ToDouble(stramoumt);
                        str_Content = str_Content.Replace("$Amount$", string.Format("{0:c}", str_amoumt));
                    }
                    if (strtransactiondate != null)
                    {
                        str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                    }
                }
                string strFrommailid = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                var objSendMessage = new ClsMailMessage();
                bool strvalid = objSendMessage.SendMail(strTomailid, strFrommailid, str_Subject, str_Content, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (strvalid == true)
                {
                    clsCommonFunctions objclscommon = new clsCommonFunctions();
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strTransaction_ID) };
                    objclscommon.AddInParameters(strMailParam);
                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }
                 }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "RegistrationController", "DisplayLogindetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            }
        }

        [HttpPost]
        public ActionResult DisplayLogindetails(EmailMessageOption obj)
        {
            //if (Request["Hdnchk"] =="Y")
            //{
            //  var  objconfig = new clsWebConfigsettings();
            //if ((objconfig.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
            //{
            //    //EmailMessageOption EMO = default(EmailMessageOption);
            //    string categorycount = Convert.ToString(Category.EmailMsgcount(13));
            //    var objcategory = new Category
            //    {
            //        EmailCategoryCount = Convert.ToInt32(categorycount),
            //        EmailCategoryID = "13",
            //        FromReference_id = "1",
            //        Toreference_id = Request["HdnProvider_ID"],
            //        ISWebService_IND = "N",
            //        Application_Id = null,
            //      FromReferenceType_id = "1",
            //    Toreferencetype_id = "2"
            //    };
            //    Category.SentEmailLog(objcategory);
            //  var  EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(1, 0);
            //    if ((EMO != null))
            //    {
            //        ViewBag.Subject = EMO.Msg_Subject ?? null;
            //        ViewBag.Content = EMO.Msg_Body ?? null;
            //    }
            //    string strUserName = Request["HdnEmail"];
            //    string strPassword = Session["PWD"]!=null?Session["PWD"].ToString():null;
            //    var objcommonfunction = new clsCommonFunctions();
            //    string strProvider = objcommonfunction.getprovidername(Request["HdnProvider_ID"]);
            //    strContent = ViewBag.Content;
            //    if (strContent != null)
            //    {
            //        if (strProvider != null)
            //        {
            //            strContent = strContent.Replace("$Providername$", strProvider);
            //        }
            //        if (strUserName != null)
            //        {
            //            strContent = strContent.Replace("$Username$", strUserName);
            //        }
            //        strContent = strContent.Replace("$Password$", strPassword);
            //    }

            //    var objEmail = new Reg_ProviderConfirmation
            //    {
            //        ProviderLogin_ID = Convert.ToInt32(Request["HdnLogin_ID"]),
            //        Practice_ID = Convert.ToInt32(Request["HdnPractice_ID"]),
            //        Email = Convert.ToString(Request["HdnEmail"])
            //    };
            //    //(CInt(ViewState("Login_ID")), CInt(hdnRegPracticeID.Value), CStr(lblemail.Text))
            //  string emailTranid=  Reg_ProviderConfirmation.Reg_Insert_EmailMessage(objEmail);

            //    string strOutMailId = objconfig.GetConfigSettingsValue("Out_Email_ID");
            //    var objSendMessage = new ClsMailMessage();
            //    bool strvalid= objSendMessage.SendMail(Convert.ToString(Request["HdnEmail"]), strOutMailId, ViewBag.Subject, strContent, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
            //    if (strvalid == true)
            //    {
            //        clsCommonFunctions objclscommon = new clsCommonFunctions();
            //        IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", emailTranid) };
            //        objclscommon.AddInParameters(strMailParam);
            //        objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
            //    }
            //}
            //}
            if (Request["Hdntype"] == "Done")
            {
                Session["Practice_ID"] = null;
                Session["Provider_ID"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var clsAuth = new clsAuthorization();
                // int intLoginID;
                //string outispaid = null;
                //  string providerid = null;
                //  string practiceid = null;
                //ViewBag.outispaid = outispaid;
                string outmsg = null;
                int intLoginId = clsAuth.IsValidUser(Request["HdnEmail"], Convert.ToString(Session["PWD"]), ref outmsg);
               
                if (intLoginId == 1)
                {
                    //return RedirectToAction("ToolsHome", "Tools");
                    if (Convert.ToString(Session["Licenseverified"]) == "Y")
                    {
                        return RedirectToAction("Schedulespecs", "Schedule");
                    }
                    if (Convert.ToString(Session["Licenseverified"]) != "Y")
                    {
                        return RedirectToAction("ViewIdentifyingInfo", "Directory");
                    }
                    return RedirectToAction("ToolsHome", "Tools");
                }
            }

            return View();
        }
        public ActionResult Conformation()
        {
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CCProcess(string Uid1, string Uid2)
        {      ViewBag.practice_ind = "Y";
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            //if (clsweb.GetConfigSettingsValue("ShowArticles").ToUpper() == "YES")
            //{
            //    ViewBag.ShowArticles = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowArticles = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowForumsInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.Showforums = "Y";
            //}
            //else
            //{
            //    ViewBag.Showforums = "N";
            //}
            //if (clsweb.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            //{
            //    ViewBag.ShowElectricians = "Y";
            //}
            //else
            //{
            //    ViewBag.ShowElectricians = "N";
            //}
            string Practice_ID = Session["Practice_ID"] != null ? Session["Practice_ID"].ToString() : null;
            string ProviderID = Session["Provider_ID"] != null ? Session["Provider_ID"].ToString() : null;

            if (Practice_ID != Uid1 || ProviderID != Uid2)
            {
                TempData["Practice_ID"] = null;
                TempData["Provider_ID"] = null;
                return RedirectToAction("PageNotFound", "Error");
            }
            //GetProviderDetails(Uid2);  //*
            if (Uid1 != null)
            {
                //ViewBag.Practice_ID = Uid1;
                TempData["Practice_ID"] = Uid1;
            }
            //else
            //{
            //    ViewBag.Practice_ID = null;
            //}
            if (Uid2 != null)
            {
                //ViewBag.Provider_ID = Uid2;
                TempData["Provider_ID"] = Uid2;
            }
            //else
            //{
            //    ViewBag.Provider_ID = null;
            //}
            TempData.Keep();
            //var description = new Referrals { FieldIDString = "9" };
            //var ds = Referrals.List_field_description(description.FieldIDString);
            var ds = Referrals.List_field_description("9,26");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //string str = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.regFee = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.IssuingNotes = Convert.ToString(ds.Tables[0].Rows[1][3]);
            }
            //var cardstype = clsCommonCClist.GetCCList();

            //var month = clsCommonCClist.GetCCMonth();

            //var year = clsCommonCClist.GetCCYear();

            //var reg1 = clsCommonCClist.GetStates();

            //var cardtype = new Reg_CreditCardProcess();
            //var cardlist = cardtype.Get_Creditcardtype();
            //IList<SelectListItem> cardstype = new List<SelectListItem>();
            //if (cardlist.Count > 0)
            //{
            //    for (int i = 0; i <= cardlist.Count - 1; i++)
            //    {
            //        if (i == 0)
            //        {

            //            cardstype.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
            //                Text = "American Express",
            //                Selected = true
            //            });
            //        }
            //        else if (i == 3)
            //        {

            //            cardstype.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
            //                Text = "Visa"
            //            });
            //        }
            //        else
            //        {

            //            cardstype.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
            //                Text = cardlist[i].code
            //            });
            //        }
            //    }
            //}
            //int[] monthArray = null;
            //monthArray = new int[12];
            //for (int intVal = 0; intVal <= 11; intVal++)
            //{
            //    monthArray[intVal] = intVal + 1;
            //}
            //IList<SelectListItem> month = new List<SelectListItem>();
            //if (monthArray.Length > 0)
            //{
            //    for (int i = 0; i <= monthArray.Length - 1; i++)
            //    {
            //        if (monthArray[i] == 1)
            //        {
            //            month.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(monthArray[i]),
            //                Text = Convert.ToString(monthArray[i]),
            //                Selected = true
            //            });
            //        }
            //        else
            //        {
            //            month.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(monthArray[i]),
            //                Text = Convert.ToString(monthArray[i]),
            //                Selected = false
            //            });
            //        }

            //    }
            //}
            //IList<SelectListItem> year = new List<SelectListItem>();
            //for (this.intCounter = DateTime.Now.Year; this.intCounter <= DateTime.Now.Year + 12; this.intCounter++)
            //{
            //    year.Add(new SelectListItem
            //    {
            //        Value = this.intCounter.ToString(),
            //        Text = this.intCounter.ToString(),
            //        Selected = true
            //    });
            //}
            ////for (this.intCounter = StartingYear; this.intCounter <= DateTime.Now.Year + 12; this.intCounter++)
            ////{

            ////    if (this.intCounter == DateTime.Now.Year)
            ////    {
            ////        year.Add(new SelectListItem
            ////        {
            ////            Value = Convert.ToString(this.intCounter),
            ////            Text = Convert.ToString(this.intCounter),
            ////            Selected = true
            ////        });
            ////    }
            ////    else
            ////    {
            ////        year.Add(new SelectListItem
            ////        {
            ////            Value = Convert.ToString(this.intCounter),
            ////            Text = Convert.ToString(this.intCounter),
            ////            Selected = false
            ////        });
            ////    }

            ////}
         
            ViewData["month"] = clsCommonCClist.GetCCMonth(); ;
            ViewData["year"] = clsCommonCClist.GetCCYear();
            ViewData["CardType"] = clsCommonCClist.GetCCList();
            return View("CCProcess", GetProviderDetails(Uid2));
        }

        public Reg_ProvidersDetailInfo GetProviderDetails(string Uid2)
        {

            Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
            objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Uid2));
            //if ((objInfo != null))
            //{
            //    if (!string.IsNullOrEmpty(objInfo.FirstName))
            //    {
            //        ViewBag.FirstName = objInfo.FirstName;
            //    }
            //    else
            //    {
            //        ViewBag.FirstName = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.LastName))
            //    {
            //        ViewBag.LastName = objInfo.LastName;
            //    }
            //    else
            //    {
            //        ViewBag.LastName = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.Address1))
            //    {
            //        ViewBag.PAddress1 = objInfo.Address1;
            //    }
            //    else
            //    {
            //        ViewBag.PAddress1 = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.Address2))
            //    {
            //        ViewBag.PAddress2 = objInfo.Address2;
            //    }
            //    else
            //    {
            //        ViewBag.PAddress2 = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.Zip))
            //    {
            //        ViewBag.PZip = objInfo.Zip;
            //    }
            //    else
            //    {
            //        ViewBag.PZip = null;
            //    }


            //    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.State_ID)))
            //    {
            //        ViewBag.State_ID = objInfo.State_ID;
            //    }
            //    else
            //    {
            //        ViewBag.State_ID = null;
            //    }
            //    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.City_ID)))
            //    {
            //        ViewBag.City_ID = objInfo.City_ID;
            //    }
            //    else
            //    {
            //        ViewBag.City_ID = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.Statename))
            //    {
            //        ViewBag.Statename = objInfo.Statename;
            //    }
            //    else
            //    {
            //        ViewBag.Statename = null;
            //    }
            //    if (!string.IsNullOrEmpty(objInfo.Cityname))
            //    {
            //        ViewBag.Cityname = objInfo.Cityname;
            //    }
            //    else
            //    {
            //        ViewBag.Cityname = null;
            //    }
            //}
            return objInfo;
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult CCProcess(Reg_CreditCardProcess obj)
        {
            string firstName = null, lastName = null; 
            int? Out_CCID = null;
            var crdtCard = new CreditCard();

            if (!string.IsNullOrEmpty(obj.FullName))
            {
                if (obj.FullName.Contains(" "))
                {
                    var names = obj.FullName.Split(' ');
                    if (names.Length >= 2)
                    {
                        firstName = names[0] ?? null;
                        lastName = names[1] ?? null;
                    }
                }
                else
                {
                    firstName = obj.FullName;
                }
            }

            if (Request["CardType"] != "")
            {
                switch (Request["CardType"])
                {
                    case "1": crdtCard.type = "mastercard";
                        break;
                    case "2": crdtCard.type = "visa";
                        break;
                    case "3": crdtCard.type = "amex";
                        break;
                    case "4": crdtCard.type = "discover";
                        break;
                }
            }
            if (Request["CardNumber"] != "")
            {
                crdtCard.number = Request["CardNumber"];
            }
            crdtCard.cvv2 = Request["txtCVV2"] != "" ? Request["txtCVV2"] : null;
            //crdtCard.first_name = Request["txtfirstname"] != "" ? Request["txtfirstname"] : null;
            //crdtCard.last_name = Request["txtlastname"] != "" ? Request["txtlastname"] : null;
            crdtCard.first_name = !string.IsNullOrEmpty(firstName) ? firstName : null;
            crdtCard.last_name = !string.IsNullOrEmpty(lastName) ? lastName : null;
            if (Request["month"] != "")
            {
                crdtCard.expire_month = Convert.ToInt32(Request["month"]);
            }

            if (Request["year"] != "")
            {
                crdtCard.expire_year = Convert.ToInt32(Request["year"]);
            }
            if (Request["HdnProvider_ID"] != null)
            {
                crdtCard.payer_id = Request["HdnProvider_ID"];
            }

            var billingAddress = new Address
            {
                country_code = "US",
                // request[hdnadd1,hdnadd2,hdnzip,hdnstate,hdncity] are not required already these elements had with ids address1,and soon...
                line1 = Request["hdnadd1"] != "" ? Request["hdnadd1"] : null,
                line2 = Request["hdnadd2"] != "" ? Request["hdnadd2"] : null,
                postal_code = Request["hdnzip"] != "" ? Request["hdnzip"].ToString() : null,
                state = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                city = Request["hdncity"] != "" ? clsCommonCClist.Getcityname(Request["hdncity"]) : null
            };
            crdtCard.billing_address = billingAddress;
            var amnt = new Amount { currency = "USD" };
            if (Request["hdmamount"] != "")
            {
                amnt.total = Request["hdmamount"];
            }

            var tran = new Transaction { amount = amnt, description = "SmbHelpGroup." };

            var transactions = new List<Transaction> { tran };
            var fundInstrument = new FundingInstrument { credit_card = crdtCard };
            var fundingInstrumentList = new List<FundingInstrument> { fundInstrument };
            var payr = new Payer { funding_instruments = fundingInstrumentList, payment_method = "credit_card" };
            var pymnt = new Payment { intent = "sale", payer = payr, transactions = transactions };
            try
            {

                //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                {

                    var apiContext = Configuration.GetAPIContext();
                    Payment createdPayment = pymnt.Create(apiContext);
                    string objrequest = JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented);

                    if (objrequest != "" & createdPayment.state == "approved")
                    {
                        //if (createdPayment.state == "approved")
                        //{
                        var objinsert = new CCProcess
                        {


                            StrPaidRefID = "1",
                            StrPaidRefTypeID = "1",
                            StrChargebleRefID = Request["HdnProvider_ID"],
                            StrChargebleRefTypeID = "2",
                            strpost = objrequest,
                            RefLoginID = 0,
                            Provider_ID = Request["HdnProvider_ID"],
                            CVV2 = null,
                            strx_card_code = crdtCard.type,
                            strStrCardType = null,
                            strStateID = Request["hdnstate"],
                            strZipCode = Request["hdnzip"],
                            strCityID = Request["hdncity"],
                            strBillAddress1 = billingAddress.line1,
                            strBillAddress2 = billingAddress.line2,
                            FirstName=!string.IsNullOrEmpty(firstName) ? firstName : null,
                            LastName=!string.IsNullOrEmpty(lastName) ? lastName : null,
                            //FirstName = Request["txtfirstname"],
                            //LastName = Request["txtlastname"],
                            strx_invoice_num = null,
                            strx_card_num = null,
                            IsPaypalInd = "Y",
                            StrExpMon = crdtCard.expire_month,
                            StrExpYear = crdtCard.expire_year,
                            dblx_amount = Convert.ToDouble(amnt.total),
                            paypaltransactionid = createdPayment.id
                        };

                        var objlist = new List<Transaction>();
                        var objfund = new List<RelatedResources>();
                        var objsale = new Sale();
                        objlist = createdPayment.transactions;
                        objfund = objlist[0].related_resources;
                        objsale = objfund[0].sale;
                        objinsert.paypalsaletransactionid = objsale.id;
                        var objcommonfunction = new clsCommonFunctions();
                        objinsert.CreatedBy = objcommonfunction.GetProviderLoginID(Request["HdnProvider_ID"]);
                        objinsert.ALLowCCCharges = "Y";
                        if (Request["chkaddress"] != null)
                        {
                            objinsert.practice_ind = Request["chkaddress"] == "true,false" ? "Y" : "N";
                        }
                        string Mycardno = Convert.ToString(Request["CardNumber"]);
                        objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;

                        Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,null);
                        //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                        int locTransid = Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                        CreditCard strcard = new CreditCard();
                        //try
                        //{
                            strcard = crdtCard.Create(apiContext);
                        //}
                        //catch (Exception)
                        //{


                        //}

                        string objstorecreditcardresponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);
                        //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));

                        CCProcess objccins = new CCProcess();
                        objccins.strTransactionID = locTransid;
                        objccins.StrRespStatusCode = createdPayment.state;
                        objccins.strRetval = objstorecreditcardresponse;// Encrypt(objstorecreditcardresponse);
                        objccins.ResponseCode = null;
                        objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));
                        objccins.GatewayDetail_ID = null;
                        objccins.PaypalProcessInd = "Y";
                        Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                        if (createdPayment.state == "approved")
                        {
                            Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, Session["RefferalCode"] != null ? Session["RefferalCode"].ToString() : null, strcard.id);

                            obj.Provider_ID = Convert.ToInt32(Request["HdnProvider_ID"]);
                            obj.Practice_ID = Convert.ToInt32(Request["HdnPractice_ID"]);
                            // Reg_ProviderConfirmation.Ins_trialpackage_users(LoginId);

                            //Message newMessage = new Message(LoginId, "Registration Alert", "New Dietitian Registration", 4, "0", null, null, 1, "1");
                            //newMessage.Save();

                            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Request["HdnProvider_ID"]));

                            if ((obgCredn != null))
                            {
                                //ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                                //string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                                //string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                                //string strdate = DateTime.Now.ToShortDateString();

                                //SendEmailtoElectrician(strtoMailid, Convert.ToString(locTransid), strprovidername, strdate, amnt.total);
                                SendEmailtoElectrician((obgCredn.UserName) != null ? obgCredn.UserName : null
                                    , Convert.ToString(locTransid),
                                    (obgCredn.providername) != null ? obgCredn.providername : null
                                    , DateTime.Now.ToShortDateString(), amnt.total);
                            }
                            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                        }
                        else
                        {

                            return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                        }
                    }
                    else
                    {

                        return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                    }
                }







                else
                {
                    //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                    var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                  //  StripeCard Card;
                    var cardToken = gateway.Post(new CreateStripeToken
                    {
                        Card = new StripeCard
                        {
                            //Name = Request["txtfirstname"] + Request["txtlastname"],
                            Name = Request["FullName"],
                            Number = Request["CardNumber"],
                            Cvc = Request["txtCVV2"] != "" ? Request["txtCVV2"] : null,
                            ExpMonth = Convert.ToInt32(Request["month"]),
                            ExpYear = Convert.ToInt32(Request["year"]),
                            AddressLine1 = Request["hdnadd1"] != "" ? Request["hdnadd1"] : null,
                            AddressLine2 = Request["hdnadd2"] != "" ? Request["hdnadd2"] : null,
                            AddressZip = Request["hdnzip"] != "" ? Request["hdnzip"].ToString() : null,
                            AddressState = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                            AddressCountry = "US",
                        },
                    });
                    var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Request["HdnProvider_ID"]));
                    //string strcustomermail = null;
                    //if ((obgCredn != null))
                    //{
                    //    strcustomermail = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                    //}
                    var customer = gateway.Post(new CreateStripeCustomerWithToken
                    {

                        Card = cardToken.Id,
                        Description = "Description",
                        Email = (obgCredn.UserName) != null ? obgCredn.UserName : null,
                        //Email = strcustomermail,
                    });
                    //var customerinfo = gateway.Get(new GetStripeCustomer { Id = customer.Id });
                    //Console.WriteLine(customerinfo);

                    var charge = gateway.Post(new ChargeStripeCustomer
                    {
                        Amount = 100,
                        Customer = customer.Id,
                        Currency = "usd",
                        Description = "Test Charge Client",
                    });

                    //if (charge.FailureCode != "")
                    //{
                    //}
                    //if (charge.FailureMessage != "")
                    //{
                    //}
                    var card = gateway.Get(new GetStripeCard
                    {
                        CustomerId = customer.Id,
                        CardId = cardToken.Card.Id,
                    });

                    //APIContext apiContext = Configuration.GetAPIContext();
                    //Payment createdPayment = pymnt.Create(apiContext);
                    //string objrequest = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);

                    //if (objrequest != "")
                    //{
                    var objinsert = new CCProcess
                    {


                        StrPaidRefID = "1",
                        StrPaidRefTypeID = "1",
                        StrChargebleRefID = Request["HdnProvider_ID"],
                        StrChargebleRefTypeID = "2",
                        //strpost = objrequest,
                        strpost = Convert.ToString(card),
                        RefLoginID = 0,
                        Provider_ID = Request["HdnProvider_ID"],
                        CVV2 = null,
                        strx_card_code = crdtCard.type,
                        strStrCardType = null,
                        strStateID = Request["hdnstate"],
                        strZipCode = Request["hdnzip"],
                        strCityID = Request["hdncity"],
                        strBillAddress1 = billingAddress.line1,
                        strBillAddress2 = billingAddress.line2,
                        FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                        LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                        //FirstName = Request["txtfirstname"],
                        //LastName = Request["txtlastname"],
                        strx_invoice_num = null,
                        strx_card_num = null,
                        IsPaypalInd = "N",
                        StrExpMon = crdtCard.expire_month,
                        StrExpYear = crdtCard.expire_year,
                        dblx_amount = Convert.ToDouble(amnt.total),
                        //paypaltransactionid = createdPayment.id
                        paypaltransactionid = charge.Id
                    };

                    //var objlist = new List<Transaction>();
                    //var objfund = new List<RelatedResources>();
                    //var objsale = new Sale();
                    //objlist = createdPayment.transactions;
                    //objfund = objlist[0].related_resources;
                    //objsale = objfund[0].sale;
                    //objinsert.paypalsaletransactionid = objsale.id;

                    //objinsert.paypalsaletransactionid = customer.Id;
                    objinsert.paypalsaletransactionid = null;
                    var objcommonfunction = new clsCommonFunctions();
                    objinsert.CreatedBy = objcommonfunction.GetProviderLoginID(Request["HdnProvider_ID"]);
                    objinsert.ALLowCCCharges = "Y";
                    if (Request["chkaddress"] != null)
                    {
                        objinsert.practice_ind = Request["chkaddress"] == "true,false" ? "Y" : "N";
                    }
                    string Mycardno = Convert.ToString(Request["CardNumber"]);
                    objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                    objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                    Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,null);
                    objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                    int locTransid = Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                    //CreditCard strcard = crdtCard.Create(apiContext);
                    //string objstorecreditcardresponse = JObject.Parse(strcard.ConvertToJson()).ToString(Formatting.Indented);
                    //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));

                    CCProcess objccins = new CCProcess();
                    objccins.strTransactionID = locTransid;
                    //objccins.StrRespStatusCode = createdPayment.state;
                    objccins.StrRespStatusCode = "approved";
                    //objccins.strRetval = Encrypt(objstorecreditcardresponse);
                    objccins.ResponseCode = null;
                    objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));
                    objccins.GatewayDetail_ID = null;
                    objccins.PaypalProcessInd = "N";
                    Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                    //if (createdPayment.state == "approved")
                    if (charge.FailureMessage == null)
                    {
                        //Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, null, strcard.id);
                        Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, Session["RefferalCode"] != null ? Session["RefferalCode"].ToString() : null, cardToken.Card.Id, customer.Id);

                        obj.Provider_ID = Convert.ToInt32(Request["HdnProvider_ID"]);
                        obj.Practice_ID = Convert.ToInt32(Request["HdnPractice_ID"]);
                        // Reg_ProviderConfirmation.Ins_trialpackage_users(LoginId);

                        //Message newMessage = new Message(LoginId, "Registration Alert", "New Dietitian Registration", 4, "0", null, null, 1, "1");
                        //newMessage.Save();

                        //var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Request["HdnProvider_ID"]));

                        if ((obgCredn != null))
                        {
                            //ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                            //string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                            //string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                            //string strdate = DateTime.Now.ToShortDateString();

                            //SendEmailtoElectrician(strtoMailid, Convert.ToString(locTransid), strprovidername, strdate, amnt.total);
                            SendEmailtoElectrician((obgCredn.UserName) != null ? obgCredn.UserName : null,
                                 Convert.ToString(locTransid),
                                 (obgCredn.providername) != null ? obgCredn.providername : null, 
                                 DateTime.Now.ToShortDateString(), amnt.total);
                        }
                        return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                    }
                    else
                    {

                        return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                    }
                    //}
                    //else
                    //{

                    //    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                    //}
                }
            }

            catch (PayPal.PayPalException ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "ElectricianRegistrationController", "CCProcess", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            catch (StripeException ex)
            {
                //if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                //{

                //}
                string errormessage = ex.Message;
                if (errormessage != null)
                {
                    var objinsert = new CCProcess
                    {


                        StrPaidRefID = "1",
                        StrPaidRefTypeID = "1",
                        StrChargebleRefID = Request["HdnProvider_ID"],
                        StrChargebleRefTypeID = "2",
                        strpost = ex.Message,
                        RefLoginID = 0,
                        Provider_ID = Request["HdnProvider_ID"],
                        CVV2 = null,
                        strx_card_code = crdtCard.type,
                        strStrCardType = null,
                        strStateID = Request["hdnstate"],
                        strZipCode = Request["hdnzip"],
                        strCityID = Request["hdncity"],
                        strBillAddress1 = billingAddress.line1,
                        strBillAddress2 = billingAddress.line2,
                        FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                        LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                        //FirstName = Request["txtfirstname"],
                        //LastName = Request["txtlastname"],
                        strx_invoice_num = null,
                        strx_card_num = null,
                        IsPaypalInd = "N",
                        StrExpMon = crdtCard.expire_month,
                        StrExpYear = crdtCard.expire_year,
                        dblx_amount = Convert.ToDouble(amnt.total),
                        //paypaltransactionid = charge.Id
                        paypaltransactionid = null
                    };
                    objinsert.paypalsaletransactionid = null;
                    var objcommonfunction = new clsCommonFunctions();
                    objinsert.CreatedBy = objcommonfunction.GetProviderLoginID(Request["HdnProvider_ID"]);
                    objinsert.ALLowCCCharges = "Y";
                    if (Request["chkaddress"] != null)
                    {
                        objinsert.practice_ind = Request["chkaddress"] == "true,false" ? "Y" : "N";
                    }
                    string Mycardno = Convert.ToString(Request["CardNumber"]);
                    objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                    objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                    Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,null);
                    objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                    int locTransid = Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                    //int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));
                    CCProcess objccins = new CCProcess();
                    objccins.strTransactionID = locTransid;
                    objccins.StrRespStatusCode = Convert.ToString(ex.Code);
                    objccins.strRetval = Convert.ToString(ex.StatusCode);
                    objccins.ResponseCode = null;
                    objccins.strUserID = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));
                    objccins.GatewayDetail_ID = null;
                    objccins.PaypalProcessInd = "N";
                    Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonResponseFactory.ErrorResponse("some problem existing."), JsonRequestBehavior.DenyGet);
            }

            return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
        }
        //private string Encrypt(string clearText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}
        //public string Getstatename(string stateid)
        //{
        //    var objcommonfunction = new clsCommonFunctions();
        //    IDataParameter[] inParmList = { new SqlParameter("@in_state_id", stateid) };
        //    objcommonfunction.AddInParameters(inParmList);
        //    SqlDataReader drGetName = objcommonfunction.GetDataReader("Help_dbo.St_get_stateabbrevation");
        //    while (drGetName.Read())
        //    {
        //        return Convert.ToString(drGetName["state"]);
        //    }
        //    return null;
        //}
        //public string Getcityname(string cityid)
        //{
        //    var objcommonfunction = new clsCommonFunctions();
        //    IDataParameter[] inParmList = { new SqlParameter("@in_City_ID", cityid) };
        //    objcommonfunction.AddInParameters(inParmList);
        //    SqlDataReader drGetName = objcommonfunction.GetDataReader("Help_dbo.st_Get_CityName");
        //    while (drGetName.Read())
        //    {
        //        return Convert.ToString(drGetName["City"]);
        //    }
        //    return null;
        //}


    }
}
