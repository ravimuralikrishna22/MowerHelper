using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.Classes;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.BLL.Providers;

using MowerHelper.Models;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.MessageStation;
using aspPDF;
using System.Text;


namespace MowerHelper.Controllers
{
    public class VerificationUserController : Controller
    {
        EASYPDF pdf = new EASYPDF();
        static string HomePageFileName;
        static string CustomizeFileName;
        static string encCustomizeFileName;
        clsCommonFunctions ObjCommFun = new clsCommonFunctions();
        List<patients> objcollection = new List<patients>();
        ClsMailMessage objMailMessage = new ClsMailMessage();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProviderProfile()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
          //  Session.Add("TopIndex", "1");
            List<Provider_ProviderList> ElProviderList = new List<Provider_ProviderList>();
            int count = 0;
            int page;
            if (Request["page"] != null & Request["page"] != "")
            {
                page = Convert.ToInt32(Request["page"]);
            }
            else
            {
                page = 1;
            }
            ElProviderList = Provider_ProviderList.Get_ElProviderInProfile(null, null, null, Convert.ToInt32(null), "ProviderName", "ASC", Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["RoleId"]), "A", 10,
            page, "nonverifiedprovider", ref count, null, null);
            ViewBag.totrec = count;
            return View(ElProviderList);
        }
        public ActionResult Userprofile()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "1");
            string providertype = "nonverifiedprovider";
            if (Request["providertype"] != null)
            {
                providertype = Request["providertype"];
            }
            if (providertype == "nonverifiedprovider")
            {
                ViewBag.verify = "N";
            }
            else
            {
                ViewBag.verify = "Y";
            }
            List<Provider_ProviderList> ElProviderList = new List<Provider_ProviderList>();
            int count = 0;
            ElProviderList = Provider_ProviderList.Get_ElProviderInProfile(null, null, null, Convert.ToInt32(null), "ProviderName", "ASC", Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["RoleId"]), "A", 10,
            Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), providertype, ref count, null, null);
            ViewBag.totrec = count;
            return View(ElProviderList);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ProofCertificates(int ProviderId, int? PracticeID)
        {
           
                Provider_DocumentInfo ObjDocInfo1 = new Provider_DocumentInfo();
                ObjDocInfo1.Practice_ID = PracticeID;
                ObjDocInfo1.Provider_ID = ProviderId;
                ObjDocInfo1.ProviderDocument_ID = null;
                ObjDocInfo1.OrderBYItem = "Title";
                ObjDocInfo1.OrderBy = "ASC";
                ObjDocInfo1.PageNo = 1;
                ObjDocInfo1.NoOfRecords = 10;
                ObjDocInfo1.verifieddocument = "P";
                List<Provider_DocumentInfo> DocList1 = new List<Provider_DocumentInfo>();
                DocList1 = Provider_DocumentInfo.Get_DocumentInfo(ObjDocInfo1);
                ViewBag.provid = ProviderId;
                ViewBag.practiceid = PracticeID;
                return View(DocList1);
           
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditProofCertificate(int id, string provid, string practiceid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetAttachmentList(id, provid, practiceid);
            ViewBag.provid = provid;
            ViewBag.practiceid = practiceid;
            ViewBag.docid = id;
            return View();
        }
        [HttpPost()]
        public ActionResult EditProofCertificate(Provider_DocumentInfo obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                string StrTitle = null;
                string StrDesc = null;
                string isdisplaypublic = "N";
                double filesize = 0;
                string FileExtn2 = null;
                int? docid = null;
                string strFilePath = null;
                string StrImagePath = null;
                strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\VerifiedDocuments" + "\\";
                if (Request["hdndocid"] != null)
                {
                    docid = Convert.ToInt32(Request["hdndocid"]);
                }
                if (Request.Files["FileUpLoad"].FileName == string.Empty)
                {
                    if (!string.IsNullOrEmpty(Request["txtTitle"].ToString()))// != null & Request["txtTitle"] != "")
                    {
                        StrTitle = Request["txtTitle"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Request["txtDescription"].ToString()))// != null & Request["txtDescription"] != "")
                    {
                        StrDesc = Request["txtDescription"].ToString();
                    }

                    Provider_DocumentInfo.Update_Attachment(docid,  StrTitle, StrDesc, CustomizeFileName, encCustomizeFileName, HomePageFileName, Convert.ToInt32(Session["userid"]), "P", filesize, isdisplaypublic);
                }
                else
                {
                    if (Request.Files["FileUpLoad"].ContentLength > 0)
                    {

                        double filelength = Request.Files["FileUpLoad"].ContentLength;

                        HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["FileUpLoad"].FileName);
                        FileExtn2 = System.IO.Path.GetExtension(Request.Files["FileUpLoad"].FileName);
                        HomePageFileName = HomePageFileName + FileExtn2;
                        CustomizeFileName = "PROVDoc" + Convert.ToInt32(Request["hdnprovid"]) + FileExtn2;
                        encCustomizeFileName = clsCommonCClist.RandomPassword();
                       if (!string.IsNullOrEmpty(Request["txtTitle"].ToString()))// != null & Request["txtTitle"] != "")
                        {
                            StrTitle = Request["txtTitle"].ToString();
                        }

                            isdisplaypublic = "N";
                       if (!string.IsNullOrEmpty(Request["txtDescription"].ToString()))// != null & Request["txtDescription"] != "")
                        {
                            StrDesc = Request["txtDescription"].ToString();
                        }
                        if (filelength != 0)
                        {

                            filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                        }
                        Provider_DocumentInfo.Update_Attachment(docid,  StrTitle, StrDesc, CustomizeFileName, encCustomizeFileName, HomePageFileName, Convert.ToInt32(Session["userid"]), "P", filesize, isdisplaypublic);

                        string strfile = Convert.ToString(Request["hdnstrpath"]);
                        if (!string.IsNullOrEmpty(strfile))
                        {
                            string[] strPathup = strfile.Split('.');
                            string strPath1 = strPathup[0];
                            string strPath2 = strPathup[1];
                            string strSuffix = Request["hdnPathSuffix"];
                            string strFinalPath = strPath1 + strSuffix + "." + strPath2;
                            StrImagePath = strFilePath + "/" + strFinalPath;
                            string StrRequest = StrImagePath;
                            if (!string.IsNullOrEmpty(StrRequest))
                            {
                                System.IO.FileInfo File = new System.IO.FileInfo(StrRequest);
                                if (File.Exists)
                                {
                                    File.Attributes = System.IO.FileAttributes.Normal;
                                    File.Delete();
                                }
                            }
                        }
                        strFilePath = strFilePath + "/" + "PROVDoc" + Convert.ToInt32(Request["hdnprovid"]) + Request["hdnPathSuffix"] + FileExtn2;
                        Request.Files["FileUpLoad"].SaveAs(strFilePath);
                    }
                }

            }
            return RedirectToAction("Userprofile");
        }
        private void GetAttachmentList(Int32 IntProvDocID, string provid, string practiceid)
        {
            try
            {
                Provider_DocumentInfo obj = new Provider_DocumentInfo();
                obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32(provid), IntProvDocID);
                if ((obj != null))
                {
                    ViewBag.docid = IntProvDocID;
                    if (!string.IsNullOrEmpty(obj.Title))
                    {
                        ViewBag.txtTitle = obj.Title;
                    }
                    else
                    {
                        ViewBag.txtTitle = null;
                    }

                    if (!string.IsNullOrEmpty(obj.DocDescription))
                    {
                        ViewBag.txtDescription = obj.DocDescription;
                    }
                    else
                    {
                        ViewBag.txtDescription = null;
                    }
                    string StrTempFilePath = "";
                    if (!string.IsNullOrEmpty(obj.FileName))
                    {
                        ViewBag.hdnPaTH = obj.FileName;
                        StrTempFilePath = obj.FileName;
                        ViewBag.FileName = StrTempFilePath;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(obj.Path))
                        {
                            ViewBag.hdnPaTH = obj.Path;
                            StrTempFilePath = obj.Path;
                            ViewBag.FileName = StrTempFilePath;
                        }

                        ViewBag.hypLnkAttachedFileName = "";
                    }
                    if (obj.Displayinpublic != null)
                    {
                        ViewBag.Displayinpublic = obj.Displayinpublic;
                    }
                    if (obj.Path_Suffix != null)
                    {
                        ViewBag.Path_Suffix = obj.Path_Suffix;
                    }
                    if (!string.IsNullOrEmpty(obj.Path))
                    {
                        string strPath = obj.Path;
                        ViewData["strpath"] = strPath;
                        ViewBag.strpath = strPath;
                        string[] strExtension = strPath.Split('.');
                        ViewData["Extn"] = strExtension[1];
                    }

                    CustomizeFileName = "PROVDoc" + ((Session["Provider_ID"] == null) ? Session["Prov_ID"] : Session["Provider_ID"]) + "." + ViewData["Extn"];
                    HomePageFileName = ViewBag.FileName;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "VerificationUserController", "GetAttachmentList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                clsCustomEx = null;
            }
        }
        public ActionResult Licensesettings(int ProviderId, int? PracticeID)
        {
           
                Provider_ProfilesInfo ObjProfilesInfo = new Provider_ProfilesInfo();
                ObjProfilesInfo = Provider_ProfilesInfo.Get_Prividerlicensedetails(ProviderId);
                ViewBag.ProviderId = ProviderId;
                if ((ObjProfilesInfo != null))
                {
                    if (!string.IsNullOrEmpty(ObjProfilesInfo.LicenseNo))
                    {
                        ViewBag.LicenseNo = ObjProfilesInfo.LicenseNo;
                    }
                    else
                    {
                        ViewBag.LicenseNo = null;
                    }
                    if (!string.IsNullOrEmpty(ObjProfilesInfo.IsLicenseVerified))
                    {
                        if (ObjProfilesInfo.IsLicenseVerified == "Y")
                        {
                            ViewBag.IsLicenseVerified = "Yes";
                        }
                        else if (ObjProfilesInfo.IsLicenseVerified == "N")
                        {
                            ViewBag.IsLicenseVerified = "No";
                        }
                    }
                    else
                    {
                        ViewBag.IsLicenseVerified = "No";
                    }

                    if (ObjProfilesInfo.licenseexpirydate != null)
                    {
                        string[] licenseexpirydate = ObjProfilesInfo.licenseexpirydate.Split(' ');
                        ViewBag.licenseexpirydate = Convert.ToString(licenseexpirydate[0]);

                    }
                    else
                    {
                        ViewBag.licenseexpirydate = null;


                    }
                }
                return View();
           
        }

        [HttpPost]
        public ActionResult Licensesettings()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                        string strlisense = "";



            string expirydate = null;

            if (!string.IsNullOrEmpty(Request["txtLicense"]))
            {
                strlisense = Request["txtLicense"];
            }
            else
            {
                strlisense = null;
            }
            string strIsLisense = "";

                            if (Request["chkIsLicense"] != null)
                {
                    string[] CellphoneLeaveMsg = null;
                    if (Request["chkIsLicense"].Contains(","))
                    {
                        CellphoneLeaveMsg = Request["chkIsLicense"].Split(',');
                        if (CellphoneLeaveMsg.Length == 2)
                        {
                            if (CellphoneLeaveMsg[0] == "true")
                            {
                                strIsLisense = "Y";
                            }
                            else if (CellphoneLeaveMsg[0] == "false")
                            {
                                strIsLisense = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkIsLicense"] == "false")
                        {
                            strIsLisense = "N";
                        }
                        else if (Request["chkIsLicense"] == "true")
                        {
                            strIsLisense = "Y";
                        }
                    }
                }


            if (Request["txtexpirydate"] != "mm/dd/yyyy" & !string.IsNullOrEmpty(Request["txtexpirydate"]))
                {
                    expirydate = Request["txtexpirydate"];
                }
                else
                {
                    expirydate = null;
                }
            Provider_ProfilesInfo objUpdate = new Provider_ProfilesInfo(Convert.ToInt32(Request["HdnProvider_ID"]), Convert.ToInt32(Session["UserID"]), strlisense, strIsLisense, expirydate);
            Provider_ProfilesInfo.Update_licensedetails(objUpdate);
            return RedirectToAction("Userprofile");
        }

        public ActionResult UploadCertificates(int ProviderId, int? PracticeID)
        {
          
                Provider_DocumentInfo ObjDocInfo1 = new Provider_DocumentInfo();
                ObjDocInfo1.Practice_ID = PracticeID;
                ObjDocInfo1.Provider_ID = ProviderId;
                ObjDocInfo1.ProviderDocument_ID = null;
                ObjDocInfo1.OrderBYItem = "Title";
                ObjDocInfo1.OrderBy = "ASC";
                ObjDocInfo1.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
                ObjDocInfo1.NoOfRecords = 10;
                ObjDocInfo1.verifieddocument = "Y";
                List<Provider_DocumentInfo> DocList1 = new List<Provider_DocumentInfo>();
                DocList1 = Provider_DocumentInfo.Get_DocumentInfo(ObjDocInfo1);
                if (DocList1.Count > 0)
                {
                    ViewBag.totrec1 = Provider_DocumentInfo.TotalRecords;
                }
                else
                {
                    ViewBag.totrec1 = null;
                }
                ViewBag.provid = ProviderId;
                ViewBag.practiceid = PracticeID;
                return PartialView("UploadCertificates", DocList1);
          
        }
        public ContentResult electricianslicDownload(string certname,string certname1)
        {
            string filename = null;
            string extn = null;
            string strFilePath = HttpContext.Server.MapPath("~/") + "Attachments\\VerifiedDocuments" + "\\";
            filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + certname);
            extn = System.IO.Path.GetExtension(strFilePath + certname);
            string fullfilepath = certname;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + certname1);
                Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                Response.ContentType = extn;
                Response.WriteFile(File.FullName);
                Response.End();
            }
            else
            {

            }
            return null;
        }
        public ContentResult CertificateDownload(int id, string provid, string practiceid)
        {
            string filename = null;
            string extn = null;
            string patencrypt = null;
            string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\VerifiedDocuments" + "\\";
            Provider_DocumentInfo obj = new Provider_DocumentInfo();
            obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32(provid), id);
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
        public ActionResult Verifyprovider(int ProviderId, int? PracticeID, string licenseno)
        {
          
                if (licenseno != null & licenseno != "")
                {
                    ViewBag.licensno = licenseno;
                }
                else
                {
                    ViewBag.licensno = null;
                }
                ViewBag.provid = ProviderId;
                ViewBag.practiceid = PracticeID;
                return View();
           
        }
   [HttpPost]
        public ActionResult Verifyprovider(Provider_DocumentInfo obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                if (Request.Files["myFile1"].ContentLength > 0)
                {
                    Update_licenseInfo();

                    string StrTitle = null;
                    string strImgSuffix = null;
                    string StrDesc = null;
                    string FileExtn2 = null;
                    double filesize = 0;
                    double filelength = Request.Files["myFile1"].ContentLength;
                    string isdisplaypublic = "N";
                    HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["myFile1"].FileName);
                    FileExtn2 = System.IO.Path.GetExtension(Request.Files["myFile1"].FileName);
                    HomePageFileName = HomePageFileName + FileExtn2;
                    CustomizeFileName = "PROVDoc" + Convert.ToInt32((Request["Hdnprovid"] ?? Request["Hdnprovid"])) + FileExtn2;
                    encCustomizeFileName = clsCommonCClist.RandomPassword();
                    if (!string.IsNullOrEmpty(Request["txtTitle"].ToString()))// != null & Request["txtTitle"] != "")
                    {
                        StrTitle = Request["txtTitle"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Request["txtDescription"].ToString()))// != null & Request["txtDescription"] != "")
                    {
                        StrDesc = Request["txtDescription"].ToString();
                    }
                    if (filelength != 0)
                    {

                        filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                    }
                    Provider_DocumentInfo.Insert_Attachment(Convert.ToInt32((Request["Hdnprovid"] ?? Request["Hdnprovid"])), Convert.ToString(StrTitle),  Convert.ToString(StrDesc),
                        Convert.ToString(CustomizeFileName), Convert.ToString(encCustomizeFileName), Convert.ToString(HomePageFileName), isdisplaypublic, Convert.ToInt32(Session["userid"]),
"P", ref strImgSuffix, filesize);
                    string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\VerifiedDocuments" + "\\";
                    strFilePath = strFilePath + "/" + "PROVDoc" + Convert.ToInt32((Request["Hdnprovid"] ?? Request["Hdnprovid"])) + strImgSuffix + FileExtn2;
                    Request.Files["myFile1"].SaveAs(strFilePath);
                }

            }
            return RedirectToAction("ProviderProfile");
        }
   private void Update_licenseInfo()
   {
       try
       {
       string expirydate = null;

       if (Request["txtexpirydate"] != "mm/dd/yyyy" & Request["txtexpirydate"] != "" & Request["txtexpirydate"]!=null)
           {
               expirydate = Request["txtexpirydate"];
           }
           else
           {
               expirydate = null;
           }
       Provider_ProfilesInfo objUpdate = new Provider_ProfilesInfo(Convert.ToInt32(Request["Hdnprovid"]), Convert.ToInt32(Session["UserID"]), Request["txtLicense"], "N", expirydate);
       Provider_ProfilesInfo.Update_licensedetails(objUpdate);
       }
       catch (Exception ex)
       {
           var clsexp = new clsExceptionLog();

           clsexp.LogException(ex, "VerificationUserController", "Update_licenseInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
       }
   }
   public ActionResult Licenseexpirelist()
   {
       //if (Session["UserID"] == null)
       //{
       //    return RedirectToAction("SessionExpire", "Home");
       //}
       ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
       ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
       ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "0" : Request["dt_filter"]);

       return View();
   }
   [HttpPost()]
   public JsonResult Licenseexpirelist(string obj)
   {
      
       if (Request.Form["hdnid"] == "" || Request.Form["hdnid"] == null)
       {
           return Json(JsonResponseFactory.ErrorResponse("Please select atleast one provider"), JsonRequestBehavior.DenyGet);
       }
       string strTorefIds = "";
       string strPracticeIds = "";
       string strProviderIDs = null;
       string id = Request.Form["hdnid"];
       string[] payerID;
       if (id != null)
       {
           payerID = id.Split(',');

           for (int i = 0; i <= payerID.Length - 1; i++)
           {
               strTorefIds += payerID[i] + "," + "0" + "," + "0" + "," + "0" + ",$";
           }
           strPracticeIds = Request.Form["hdnid"];
           strProviderIDs = Request.Form["hdnid"];
       }
       PhoneList.InslicenseRenewalsMailStatus(strTorefIds, Convert.ToString(Session["userid"]));
       objcollection = patients.GetlicenseRenewalsMailStatus(strProviderIDs);
       if (string.IsNullOrEmpty(strTorefIds))
       {
           return Json(JsonResponseFactory.ErrorResponse("Please select atleast one provider"), JsonRequestBehavior.DenyGet);
       }
       string[] str = strPracticeIds.Split(',');
       //var ct = new clsWebConfigsettings();
       if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
       {
           if (objcollection.Count > 0)
           {
               for (int i = 0; i <= objcollection.Count - 1; i++)
               {
                   if ((objcollection[i].EmailMessage_Transaction_ID != 0))
                   {
                       int CategoryCount = 0;
                       Category objCategory = new Category();
                       if (str.Length - 1 == objcollection.Count)
                       {
                           CategoryCount = Category.EmailMsgcount(1);
                           objCategory.EmailCategoryID = "1";
                           objCategory.EmailCategoryCount = CategoryCount;
                           objCategory.FromReference_id = "1";
                           objCategory.Toreference_id = str[i];
                           objCategory.ISWebService_IND = "N";
                           objCategory.Application_Id = null;
                           objCategory.FromReferenceType_id = "1";
                           objCategory.Toreferencetype_id = "2";
                           Category.SentEmailLog(objCategory);
                       }
                       string MsgBody = "";

                       patients obj1 = patients.Get_licenseexpireUserDetails(Convert.ToString(objcollection[i].Provider_ID));
                       ViewBag.providername = ((obj1.Name != null) ? obj1.Name : null);
                       ViewBag.expirydate = (obj1.licenseexpiredate != null ? obj1.licenseexpiredate : null);
                       MsgBody = objcollection[i].Message_Body;
                       if (!string.IsNullOrEmpty(MsgBody))
                       {
                           MsgBody = MsgBody.Replace("$ProviderName$", ViewBag.providername);
                           MsgBody = MsgBody.Replace("$date$", ViewBag.expirydate);
                           MsgBody = MsgBody + "<br/> 1-" + (CategoryCount + 1);
                       }

                       bool res = objMailMessage.SendMail(objcollection[i].Mail_To, objcollection[i].Mail_From, objcollection[i].Subject, MsgBody, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);

                       if (res == true)
                       {

                           PhoneList.UPDATE_Email_Message_Status(Convert.ToString(objcollection[i].EmailMessage_Transaction_ID));
                       }

                   }
               }
           }
       }



       return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
   }
   public ActionResult Liclist()
   {
       //if (Session["UserID"] == null)
       //{
       //    return RedirectToAction("SessionExpire", "Home");
       //}
       string FromDate = null;
       string ToDate = null;

       if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "5")
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
               ToDate = startdate;
           }


           else if (Request["dt_filter"] == "1")
           {
               FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
               ToDate = startdate;
           }
           else if (Request["dt_filter"] == "2")
           {
               FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
               ToDate = startdate;
           }




           else if (Request["dt_filter"] == "7")
           {
               FromDate = startdate;
               ToDate = DateTime.Parse(startdate).AddDays(+6).ToString("MM/dd/yyyy");
           }
           else if (Request["dt_filter"] == "30")
           {
               FromDate = startdate;
               ToDate = DateTime.Parse(startdate).AddDays(+29).ToString("MM/dd/yyyy");
           }
       }
       List<Provider_ProviderList> objlist = Provider_ProviderList.list_licenseExpired(null, FromDate, ToDate, null, null, 10, Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), null);
       ViewBag.FromDate = FromDate;
       ViewBag.ToDate = ToDate;
       ViewBag.totrec = Provider_ProviderList.TotalRecords;
       return View(objlist);
   }
   public FileContentResult Expenseledgerpdf(string FromDate, string ToDate)
   {
       //if (Session["UserID"] == null)
       //{
       //    return RedirectToAction("SessionExpire", "Home");
       //}
       pdf.Create();
       StringBuilder strhtml = new StringBuilder();
       pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
       Getexpensepdf(FromDate, ToDate);
       Response.Clear();
       Response.ClearHeaders();
       Response.ClearContent();
       Response.ContentType = "application/pdf";
       Response.AddHeader("Content-disposition", "attachment; filename=Licenseexpirelist.pdf");
       Response.BinaryWrite((byte[])pdf.SaveVariant());
       Response.End();
       return null;
   }
   private void Getexpensepdf(string FromDate, string ToDate)
   {
       try
       { 
       List<Provider_ProviderList> objList = Provider_ProviderList.list_licenseExpired(null, FromDate, ToDate, null, null, 10, Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), null);
       double height = 100;
       double pos3 = 300;
       string strhtml = null;
       strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
       strhtml = strhtml + "<tr align='center'>";
       strhtml = strhtml + "<td align='center'><font size='15px'><b><u>License Expire List</u></b></font></td>";
       strhtml = strhtml + "</tr>";
       strhtml = strhtml + "<tr>";
       strhtml = strhtml + "<td align='center' height='25%'></td>";
       strhtml = strhtml + "</tr>";
       strhtml = strhtml + "</table><br/>";
       if (objList.Count > 0)
       {
           strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
           strhtml = strhtml + "<tr class='gray'>";
           strhtml = strhtml + "<td align='center' width='25%' ><b><label>Provider name " + "</label></b></td>";
           strhtml = strhtml + "<td align='center' width='35%' ><b><label>Address " + "</label></b></td>";
           strhtml = strhtml + "<td align='center' width='20%' ><b><label>license expirydate" + "</label></b></td>";
           strhtml = strhtml + "<td align='center' width='20%' ><b><label>Email" + "</label></b></td>";
           strhtml = strhtml + "</tr>";

           foreach (var item in objList)
           {
               string date1 = Convert.ToString(item.licenseexpirydate);
               string[] date = date1.Split(' ');
               strhtml = strhtml + "<tr class='gray'>";
               strhtml = strhtml + "<td align='center'>" + item.ProviderName + "</td>";
               strhtml = strhtml + "<td align='center'>" + item.Address + "</td>";
               strhtml = strhtml + "<td align='center'>" + Convert.ToString(date[0]) + "</td>";
               strhtml = strhtml + "<td align='center'>" + item.Email + "</td>";
               strhtml = strhtml + "</tr>";

           }
           strhtml = strhtml + "</table>";
           height = pdf.GetTextHeight(strhtml);
           pdf.AddHTMLPos(pos3, height, strhtml);
       }
       else
       {
           strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
           strhtml = strhtml + "<tr class='gray'>";
           strhtml = strhtml + "<td width='10px'><b> No providers found. " + "</b></td>";
           strhtml = strhtml + "</table>";
       }
       }
       catch (Exception ex)
       {
           var clsexp = new clsExceptionLog();

           clsexp.LogException(ex, "VerificationUserController", "Getexpensepdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
       }
   }
   //private string RandomPassword()
   //{
   //    return Convert.ToString(Guid.NewGuid());

   //}


    }
}
