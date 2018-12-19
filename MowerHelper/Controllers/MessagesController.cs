using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using MowerHelper.App_Start;
using MowerHelper.Models;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.Technicians;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class MessagesController : Controller
    {

        List<ListMessage> Ms_ListmesssageCollection = null;
        List<patients> SentMessageCollection = null;
        MsgPtRequests ObjMsgPtRequests = new MsgPtRequests();
        List<MsgPtRequests> objcollection = null;
        DataSet data = new DataSet();
        clsCommonFunctions ObjCommFun = new clsCommonFunctions();
        public DataSet ds_Categories;
        int RefTypeID;
        string strloginids;
        string RetVal;
        //clsWebConfigsettings objconfig = new clsWebConfigsettings();

        public ActionResult Index()
        {
            return View();
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MessageFromHelp()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }

            if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("../Schedule/Schedulespecs");
            }
            Session.Add("TopIndex", "4");
            //ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            //GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            //if (Request["ddlListingMsgsType"] != null)
            //{
            //    ViewBag.Type = Request["ddlListingMsgsType"];
            //}
            //else
            //{
            //    ViewBag.Type = null;
            //}
            //string pgsize = null;
            //if (Request["ddlrecords"] != null && Request["ddlrecords"] != "undefined")
            //{
            //    ViewBag.Thpagesize = Request["ddlrecords"];
            //    pgsize = Request["ddlrecords"].ToString();
            //}
            //else if (Session["Rowsperpage"] != null)
            //{
            //    ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
            //    pgsize = Session["Rowsperpage"].ToString();
            //}
            //else
            //{
            //    ViewBag.Thpagesize = 10;
            //    pgsize = "10";
            //}
            //int TotalCount = 0;
            //Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 1, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Request["page"] == null ? Convert.ToInt32("1") : Convert.ToInt32(Request["page"]), "DESC", "SendingDate", ref TotalCount);
            //if (Ms_ListmesssageCollection.Count > 0)
            //{
            //    ViewBag.totrec = TotalCount;
            //}
            return View();
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisplayMessageFromHelp()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
                GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
                if (Request["ddlListingMsgsType"] != null)
                {
                    ViewBag.Type = Request["ddlListingMsgsType"];
                }
                else
                {
                    ViewBag.Type = null;
                }
                string pgsize = null;
                if (Request["ddlrecords"] != null && Request["ddlrecords"] != "undefined")
                {
                    ViewBag.Thpagesize = Request["ddlrecords"];
                    pgsize = Request["ddlrecords"].ToString();
                }
                else if (Session["Rowsperpage"] != null)
                {
                    ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                    pgsize = Session["Rowsperpage"].ToString();
                }
                else
                {
                    ViewBag.Thpagesize = 10;
                    pgsize = "10";
                }
                int TotalCount = 0;
                Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 1, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]), "DESC", "SendingDate", ref TotalCount);
                if (Ms_ListmesssageCollection.Count > 0)
                {
                    ViewBag.totrec = TotalCount;
                }
                return PartialView("_MessageFromHelp", Ms_ListmesssageCollection);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult FromVerificationSupervisor()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "3");


            if (clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin") != "")
            {
                ViewBag.checkallowpat = clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin");
            }
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 7, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Fromverificationuser()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "3");


            if (clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin") != "")
            {
                ViewBag.checkallowpat = clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin");
            }
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 6, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MessageReply()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.ddlListingMsgsType = Request["ddlListingMsgsType"];
                ViewBag.ddlrecords = Request["ddlrecords"];
                ViewBag.page = Request["curPage"];
                LoadOrginalMessage(Convert.ToInt32(Request["Msg_id"]), Convert.ToInt32(Session["Roleid"]));
                ViewBag.Ind = Request["Ind"];
                ViewBag.MsgCategory_ID = Request["MsgCategory_ID"];
                return View();
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        
        [HttpPost()]
        [ValidateJsonAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult MessageReply(string msgid)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                int RetVal = 0;
                string strTopic = null;
                if (Request["txt_Topic"] != null)
                {
                    strTopic = Request["txt_Topic"];
                }
                else
                {
                    if (Request["txt_Msg"].Length > 50)
                    {
                        strTopic = Sanitizer.GetSafeHtmlFragment(Request["txt_Msg"]).Substring(0, 50);
                    }
                    else
                    {
                        strTopic = Sanitizer.GetSafeHtmlFragment(Request["txt_Msg"]);
                    }

                }
                int strCatID = 0;

                Message newMessage = new Message(Convert.ToInt32(Session["userid"]), strTopic, Sanitizer.GetSafeHtmlFragment(Request["txt_Msg"]), strCatID, null, Convert.ToInt32(Request["txt_MsgID"]), Convert.ToInt32(Session["userid"]), Request["txt_RecieverID"], 0, 0,
                0, Convert.ToInt32(Session["RoleID"]), Convert.ToInt32(Request["txt_RecRoleID"]));
                if (!newMessage.Save())
                {
                    //lblMessage.Text = "Could not save Message";
                }
                else
                {
                    RetVal = Convert.ToInt32(newMessage.Message_ID);
                }
                HttpPostedFile fileAttachment = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    fileAttachment = System.Web.HttpContext.Current.Request.Files["fileAttachment"];
                    if (fileAttachment != null)
                    {
                        //if (Request.Files["fileAttachment"].ContentLength > 0)
                        //{
                        string FileExtn = null;
                        string FileName = null;
                        string encFileName = null;
                        FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["fileAttachment"].FileName);
                        FileExtn = System.IO.Path.GetExtension(Request.Files["fileAttachment"].FileName);
                        FileName = FileName.Replace("#", "");
                        encFileName = clsCommonCClist.RandomPassword();
                        FileName = FileName + FileExtn;
                        encFileName = encFileName + FileExtn;
                        string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\MessageStation" + "\\";
                        strFilePath = strFilePath + "\\" + FileName;
                        Request.Files["fileAttachment"].SaveAs(strFilePath);
                        MessageAttachment objmessage = new MessageAttachment(Convert.ToInt32(RetVal), FileName, encFileName);
                        objmessage.SaveAttachment();
                    }
                }
                //return RedirectToAction("MessageFromHelp", "Messages");
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        
        private void LoadOrginalMessage(int int_Msg_ID, int Int_RoleID)
        {

            try
            {

                ListMessage objlistmessage = default(ListMessage);
                objlistmessage = ListMessage.ReplyLoadMessageDetails(int_Msg_ID, Int_RoleID);
                if ((objlistmessage != null))
                {
                    ViewBag.Messagebody = objlistmessage.Messagebody;
                    ViewBag.Category = objlistmessage.Category;

                    if (objlistmessage.Sender == "Find-a-Therapist Admin")
                    {
                        ViewBag.Sender = "Mower Helper admin";
                    }
                    else
                    {
                        ViewBag.Sender = objlistmessage.Sender;
                    }
                    ViewBag.Message_ID = int_Msg_ID;
                    if (objlistmessage.MsgCategory_ID != 0)
                    {
                        ViewBag.CatID = objlistmessage.MsgCategory_ID;
                    }
                    ViewBag.Subject = objlistmessage.Subject;
                    ViewBag.Message_ID = objlistmessage.Message_ID;
                    ViewBag.Reciever_ID = objlistmessage.Sender_ID;
                    ViewBag.ReplyToRole_ID = objlistmessage.ReplyToRoleID;
                    ViewBag.SenderRole_ID = Session["RoleID"];
                }

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "LoadOrginalMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MessageDetails()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                LoadMessageDetails(Convert.ToInt32(Request["Msg_id"]), Convert.ToInt32(Session["Roleid"]));
                return View();
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        
        private void LoadMessageDetails(int Int_Msg_ID, int Int_Role_ID)
        {

            try
            {

                ListMessage objListMessage = default(ListMessage);
                objListMessage = ListMessage.GetMessageDetails(Int_Msg_ID, Int_Role_ID);
                if ((objListMessage != null))
                {
                    if (objListMessage.Message_ID > 0)
                    {
                        ViewBag.Message_ID = objListMessage.Message_ID;
                    }
                    if (objListMessage.Services != null)
                    {
                        ViewBag.Services = objListMessage.Services;
                    }
                    if (!string.IsNullOrEmpty(objListMessage.Read_Ind))
                    {
                        if (objListMessage.Read_Ind == "N")
                        {
                            ChangeStatusToRead(Int_Msg_ID, Convert.ToInt32(Session["UserID"]));
                        }
                    }

                    ViewBag.MsgDate = (objListMessage.MsgDate != null ? objListMessage.MsgDate : "");
                    ViewBag.Sender = (objListMessage.Sender != null ? objListMessage.Sender : "");
                    ViewBag.Category = (objListMessage.Category != null ? objListMessage.Category : "");
                    ViewBag.Subject = (objListMessage.Subject != null ? HttpUtility.HtmlDecode(objListMessage.Subject) : "");

                    ViewBag.Messagebody = (objListMessage.Messagebody != null ? HttpUtility.HtmlDecode(objListMessage.Messagebody) : "");
                    LoadAttachments(Int_Msg_ID);
                }

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "LoadMessageDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        
        private void ChangeStatusToRead(int Int_Msg_ID, int Int_Reciever_ID)
        {
            try
            {
                clsCommonFunctions objCommonFunctions = new clsCommonFunctions();
                IDataParameter[] ObjParameters = {
			new SqlParameter("@in_Message_ID", Int_Msg_ID),
			new SqlParameter("@in_Reciever_ID", Int_Reciever_ID)
		};
                objCommonFunctions.AddInParameters(ObjParameters);
                objCommonFunctions.ExecuteNonQuerySP("Help_dbo.st_MessageStation_UPD_SetReadMessage");
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "ChangeStatusToRead", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        
        private void LoadAttachments(int Int_Msg_ID)
        {
            try
            {
                MessageAttachment dr_Attachment = default(MessageAttachment);
                dr_Attachment = MessageAttachment.GetMessageAttachmentById(Int_Msg_ID);
                if ((dr_Attachment != null))
                {
                    if (!string.IsNullOrEmpty(dr_Attachment.AttachedFileName))
                    {
                        ViewBag.FileName = dr_Attachment.AttachedFileName;
                        ViewBag.URL = "~/Private/Attachments/MessageStation/" + dr_Attachment.AttachedFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "LoadAttachments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public ContentResult Download(string Imgpath)
        {
            string filename = null;
            string extn = null;
            string strFilePath = HttpContext.Server.MapPath("~/") + "Attachments\\MESSAGESTATION" + "\\";
            filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + Imgpath);
            extn = System.IO.Path.GetExtension(strFilePath + Imgpath);
            string fullfilepath = Imgpath;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + fullfilepath);
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
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SentMessages()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            Session.Add("TopIndex", "4");
            //ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            //GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            //if (Request["ddlrecords"] != null)
            //{
            //ViewBag.Msgpagesize = Request["ddlrecords"].ToString();
            //}
            //else if (Session["Rowsperpage"] != null)
            //{
            //    ViewBag.Msgpagesize = Session["Rowsperpage"].ToString();
            //}
            //else
            //{
            //    ViewBag.Msgpagesize = "10";
            //}

            //ViewBag.Type = Request["ddlListingMsgsType"];
            //dynamic SentpatMsgs = GetSentmessages();
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
            return View();
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisplaySentMessages()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
                GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
                if (Request["ddlrecords"] != null)
                {
                    ViewBag.Msgpagesize = Request["ddlrecords"].ToString();
                }
                else if (Session["Rowsperpage"] != null)
                {
                    ViewBag.Msgpagesize = Session["Rowsperpage"].ToString();
                }
                else
                {
                    ViewBag.Msgpagesize = "10";
                }

                ViewBag.Type = Request["ddlListingMsgsType"];
                dynamic SentpatMsgs = GetSentmessages();
                return PartialView("_SentMessages", SentpatMsgs);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        
        public List<patients> GetSentmessages()
        {
            try
            {
            string pagesize = null;
            if (Request["ddlrecords"] != null)
            {
                pagesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                pagesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                pagesize = "10";
            }
            SentMessageCollection = patients.GetMessageDeatails(0, (Request["ddlListingMsgsType"] != null ? Request["ddlListingMsgsType"] : "New"), Convert.ToInt32(Session["UserID"]), null, null, pagesize, (!string.IsNullOrEmpty(Request["page"]) ? Convert.ToInt32(Request["page"]) : 1));
            if (SentMessageCollection.Count > 0)
            {
                ViewBag.totrec = patients.MsgCount;
            }
            return SentMessageCollection;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetSentmessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ArchiveToSentMessages(int Msg_ID, int Rec_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage.ChangeStatusToArchive(Msg_ID, Rec_ID, "Y");
            //return RedirectToAction("SentMessages", "Messages");
            return RedirectToAction("DisplaySentMessages", "Messages", new { ddlListingMsgsType = Request["ddlListingMsgsType"], ddlrecords = Request["ddlrecords"], page = Request["curPage"] });
        }
        
        public List<MsgPtRequests> GetPatinfomessages()
        {
            try
        {
            ObjMsgPtRequests.LoginID = Convert.ToInt32(Session["Userid"]);
            ObjMsgPtRequests.RoleID = Convert.ToInt32(Session["RoleID"]);
            ObjMsgPtRequests.CategoryId = 79;
            ObjMsgPtRequests.OrderBy = "DESC";
            ObjMsgPtRequests.OrderByItem = "RequestedDate";

            objcollection = MsgPtRequests.GetMsgPatientRequests(ObjMsgPtRequests);
             }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetPatinfomessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return objcollection;
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult PublicMessages()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "4");
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));

            dynamic PublicPatients = GetPublicMessages();
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Publiclistingpagesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Publiclistingpagesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Publiclistingpagesize = "10";
            }
            ViewBag.Type = (Request["ddlListingMsgsType"] == null ? "N" : Request["ddlListingMsgsType"]);
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? null : Request["dt_filter"]);
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            return View(PublicPatients);
        }
        
        public ActionResult publicMessageview(int Msg_id)
        {
            
            data = Provider_Common.GetProviderpublicmessage(Msg_id);
            for (int i = 0; i <= data.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new Provider_Common();
                ViewBag.msg = Convert.ToString(data.Tables[0].Rows[i]["Message"]);
            }
            return View();
            
        }


        public List<Provider_Common> GetPublicMessages()
        {
            dynamic modelList = new List<Provider_Common>();
            try{
            Provider_Common obj = new Provider_Common();
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");

            if (Convert.ToString(Session["RoleID"]) != "28")
            {
                obj.ProviderID = Convert.ToInt32(Session["Provider_ID"] == null ? Session["Prov_ID"] : Session["Provider_ID"]);
            }
            else
            {
                obj.ProviderID = 0;
            }
            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            {
                obj.FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
                obj.ToDate = startdate;
            }
            else
            {

                if (Request["dt_filter"] == "Today")
                {
                    obj.FromDate = startdate;
                }
                else if (Request["dt_filter"] == "7")
                {
                    obj.FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
                }
                else if (Request["dt_filter"] == "30")
                {
                    obj.FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
                }
                obj.ToDate = startdate;
            }
            string pagesize = null;
            if (Request["ddlrecords"] != null)
            {
                pagesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                pagesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                pagesize = "10";
            }
            obj.OrderBy = (Request["sortdir"] == null ? "DESC" : Request["sortdir"]);
            obj.OrderBYItem = (Request["sort"] == null ? "CreatedOn" : Request["sort"]);
            obj.PageNO = (Request["page"] == null ? "1" : Request["page"]);
            obj.NoOfRecords = Convert.ToString(pagesize);
            obj.Loginid = Convert.ToInt32(Session["UserID"]);
            if (Request["ddlListingMsgsType"] == "All")
            {
                data = Provider_Common.Providerpublicmessages(obj, null);
            }
            else
            {
                data = Provider_Common.Providerpublicmessages(obj, (Request["ddlListingMsgsType"] == null ? "N" : Request["ddlListingMsgsType"]));
            }
            ViewBag.totrec = Convert.ToString(data.Tables[1].Rows[0]["Column1"]);
            for (int i = 0; i <= data.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new Provider_Common();
                model.CreatedOn = Convert.ToString(data.Tables[0].Rows[i]["CreatedOn"]);
                model.Sender_Name = Convert.ToString(data.Tables[0].Rows[i]["Sender_Name"]);
                model.Sender_Email = Convert.ToString(data.Tables[0].Rows[i]["Sender_Email"]);
                model.Subject = Convert.ToString(data.Tables[0].Rows[i]["Subject"]);
                model.Message = Convert.ToString(data.Tables[0].Rows[i]["Message"]);
                model.Status_Ind = Convert.ToString(data.Tables[0].Rows[i]["Status_Ind"]);
                model.Provider_PublicMessages_ID = Convert.ToInt32(data.Tables[0].Rows[i]["Provider_PublicMessages_ID"]);
                model.contactmessage = Convert.ToString(data.Tables[0].Rows[i]["contactmessage"]);
                modelList.Add(model);
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetPublicMessages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return modelList;
        }
        
        public ActionResult changestatustonewpatients(int Msg_id, string status_ind)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string status = null;

            if (status_ind == "A")
            {
                status = "V";
            }
            else if (status_ind == "V")
            {
                status = "I";
            }
            else if (status_ind == "I")
            {
                status = "I";
            }

            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
	new SqlParameter("@in_Status", status),
	new SqlParameter("@in_Provider_PublicMessages_ID",Msg_id)
};
            objcommon.AddInParameters(objparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.st_PublicMessages_ChageStatus");
            return RedirectToAction("PublicMessages", "Messages");
        }
        
        public void GetCategoriesDataset(string LoginID, string RoleID)
        {
            try
            {
            ds_Categories = Category.GetMs_GetCategoriesDS(Convert.ToInt32(LoginID), Convert.ToInt32(RoleID));
            for (int i = 0; i <= ds_Categories.Tables[0].Rows.Count - 1; i++)
            {
                if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "1")
                {
                    ViewBag.FromThhelpCount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "10")
                {
                    ViewBag.aptreqcount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "3")
                {
                    ViewBag.FromPublicCount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "8")
                {
                    ViewBag.Sentcount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }

                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "7")
                {
                    ViewBag.supervisercnt = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "6")
                {
                    ViewBag.vusercount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "5")
                {
                    ViewBag.contactcount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "2")
                {
                    ViewBag.paapcount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
                else if (Convert.ToString(ds_Categories.Tables[0].Rows[i]["MessageCategory_ID"]) == "4")
                {
                    ViewBag.Newpaapcount = Convert.ToString(ds_Categories.Tables[0].Rows[i]["NewMsgsCount"]);
                }
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetCategoriesDataset", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        
        public ActionResult Msg_Main(string Tabid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Tabid = Tabid;
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            return View();
        }
        
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ComposeMessage(string Tabid)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //ViewBag.ddlListingMsgsType = Request["ddlListingMsgsType"];
                //ViewBag.ddlrecords = Request["ddlrecords"];
                //ViewBag.curPage = Request["curPage"];
                ViewBag.Tabid = Tabid;
                ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
                ViewBag.showcontact = "You can also send attachments with your message";
                string url = Convert.ToString(Request.Url);
                if (Convert.ToString(Session["Roleid"]) == "31" || Convert.ToString(Session["Roleid"]) == "38")
                {
                    GetSearchDetails(11, Convert.ToInt32(Session["Roleid"]));
                }
                return View();
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        public void GetSearchDetails(int intSearchType, int RoleID)
        {
            try
            {

                int TotalRecords = 10000;
                string DisplayName = null;
                Ms_UserCollection DS_Ms_usercollection = null;
                int role_id = RoleID;
                if (RoleID == 31)
                {
                    role_id = 38;
                }
                else if (RoleID == 38)
                {
                    role_id = 31;

                }

                DS_Ms_usercollection = Ms_user.GetMs_usersByRoleid(intSearchType, role_id, 1, ref DisplayName, ref TotalRecords, null);
                if ((DS_Ms_usercollection != null))
                {
                    ComboBoxItemList custlist = new ComboBoxItemList(DS_Ms_usercollection, "Login_ID", "Username");
                    ViewData["combobox2"] = custlist;

                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetSearchDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ComposeMessage(string msgid, string id)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                if (Convert.ToString(Session["RoleID"]) == "4")
                {
                    RefTypeID = 1;
                    strloginids = "1";
                }
                if (Convert.ToString(Session["RoleID"]) == "31" || Convert.ToString(Session["RoleID"]) == "38")
                {
                    if (Convert.ToString(Session["RoleID"]) == "31" & Convert.ToString(Request["ddlRoles1"]) == "Mower Helper admin")
                    {
                        RefTypeID = 1;
                        strloginids = "1";
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "38" & Convert.ToString(Request["ddlRoles1"]) == "Mower Helper admin")
                    {
                        RefTypeID = 1;
                        strloginids = "1";
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "38" & Convert.ToString(Request["ddlRoles1"]) == "verification user")
                    {
                        strloginids = Request["combobox2"];
                        RefTypeID = 31;
                    }
                    else if (Convert.ToString(Session["RoleID"]) == "31" & Convert.ToString(Request["ddlRoles1"]) == "verification supervisor")
                    {
                        strloginids = Request["combobox2"];
                        RefTypeID = 38;
                    }
                }

                string strTopic = null;
                if (!string.IsNullOrEmpty(Request["txtTopics"]))
                {
                    strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtTopics"]);
                }
                else
                {
                    if (Request["txtMessages"].Length > 50)
                    {
                        strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]).Substring(0, 50) + "....";
                    }
                    else
                    {
                        strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]);
                    }
                }
                int MsgCategory_ID = 0;
                if (Convert.ToString(Session["RoleID"]) == "31")
                {
                    MsgCategory_ID = 6;
                }
                if (Convert.ToString(Session["RoleID"]) == "38")
                {
                    MsgCategory_ID = 7;
                }

                Message newMessage = new Message(Convert.ToInt32(Session["Userid"]), strTopic, Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]), MsgCategory_ID, "0", null, null, RefTypeID, strloginids);
                if (!newMessage.Save())
                {
                    //lblerror.Text = "Could not save Message";

                }
                else
                {
                    RetVal = Convert.ToString(newMessage.Message_ID);
                }



                //if (Request.Files["fileAttachment"].ContentLength > 0)
                //{
                HttpPostedFile fileAttachment = null;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    fileAttachment = System.Web.HttpContext.Current.Request.Files["fileAttachment"];
                    if (fileAttachment != null)
                    {
                        string FileExtn = null;
                        string FileName = null;
                        string encFileName = null;
                        FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["fileAttachment"].FileName);
                        FileExtn = System.IO.Path.GetExtension(Request.Files["fileAttachment"].FileName);
                        FileName = FileName.Replace("#", "");
                        encFileName = clsCommonCClist.RandomPassword();
                        FileName = FileName + FileExtn;
                        encFileName = encFileName + FileExtn;
                        string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\MessageStation" + "\\";
                        strFilePath = strFilePath + "\\" + FileName;
                        Request.Files["fileAttachment"].SaveAs(strFilePath);
                        MessageAttachment objmessage = new MessageAttachment(Convert.ToInt32(RetVal), FileName, encFileName);
                        objmessage.SaveAttachment();
                    }
                }
                if (Request["HdnTab_ID"] == "2")
                {
                    //return RedirectToAction("MessageFromHelp", "Messages");
                    return Json("DisplayMessageFromHelp", JsonRequestBehavior.DenyGet);
                }
                else if (Request["HdnTab_ID"] == "3")
                {
                    return RedirectToAction("PublicMessages", "Messages");
                }
                else if (Request["HdnTab_ID"] == "6")
                {
                    //return RedirectToAction("SentMessages", "Messages");
                    return Json("DisplaySentMessages", JsonRequestBehavior.DenyGet);
                }

                return View();
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MsgArchive()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage.ChangeStatusToArchive(Convert.ToInt32(Request["Msg_id"]), Convert.ToInt32(Session["UserID"]), "N");
            if (Convert.ToString(Session["Roleid"]) == "38")
            {
                return RedirectToAction("Fromverificationuser", "Messages");
            }
            else if (Convert.ToString(Session["Roleid"]) == "31")
            {
                return RedirectToAction("FromVerificationSupervisor", "Messages");

            }
            else
            {
                //return RedirectToAction("MessageFromHelp", "Messages");
                //return RedirectToAction("DisplayMessageFromHelp", "Messages");
                var ddlListingMsgsType = Request["ddlListingMsgsType"];
                var ddlrecords = Request["ddlrecords"];
                var page = Request["curPage"];
                return RedirectToAction("DisplayMessageFromHelp", "Messages", new { ddlListingMsgsType = ddlListingMsgsType, ddlrecords = ddlrecords, page = page });
            }
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Contactusentries()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            TempData.Remove("hdnPrid");
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            // Session.Add("TopIndex", "3");
            ViewBag.tabid = Request["tabid"];
            TempData["tabid"] = Request["tabid"];
            if (Request["hdntab"] != null)
            {
                ViewBag.tabid = Request["hdntab"];
            }

            string pagesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pagesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pagesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pagesize = "10";
            }

            List<ListContactInfo> objCL = new List<ListContactInfo>();

            ListContactInfo objContacts = new ListContactInfo();
            if (objContacts.ReplyStatus_Ind == "1")
            {
                objContacts.ReplyStatus = "Sent";
            }
            else
            {
                objContacts.ReplyStatus = "Not Yet Sent";
            }
            if (Request["txtlastname"] != null & Request["txtlastname"] != "")
            {
                ViewBag.txtlastname = Request["txtlastname"];
            }
            else
            {
                ViewBag.txtlastname = null;
            }
            if (Request["txtphone"] != null & Request["txtphone"] != "")
            {
                ViewBag.txtphone = Request["txtphone"];
            }
            else
            {
                ViewBag.txtphone = null;
            }
            if (Request["txtmail"] != null & Request["txtmail"] != "")
            {
                ViewBag.txtmail = Request["txtmail"];
            }
            else
            {
                ViewBag.txtmail = null;
            }
            if (Request["txtsubject"] != null & Request["txtsubject"] != "")
            {
                ViewBag.txtsubject = Request["txtsubject"];
            }
            else
            {
                ViewBag.txtsubject = null;
            }

            objContacts.ContactLastName = (Request["txtlastname"] != null ? Request["txtlastname"] : null);
            objContacts.ContactPhone = (Request["txtphone"] != null ? Request["txtphone"] : null);
            objContacts.ContactMail = (Request["txtmail"] != null ? Request["txtmail"] : null);
            objContacts.ContactSubject = (Request["txtsubject"] != null ? Request["txtsubject"] : null);
            objContacts.NoOfRecords = Convert.ToInt32(pagesize);
            objContacts.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);

            objContacts.OrderByitem = "DateOfSubmit";
            objContacts.OrderBy = "DESC";

            objCL = ListContactInfo.List_ContactUs(objContacts);
            ViewBag.totrec = ListContactInfo.TotalRecords;
            ViewBag.Thpagesize = pagesize;
            return View(objCL);
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ContactReply(int Contact_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewDetails(Contact_ID);
            ViewBag.contid = Contact_ID;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ContactReply()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {

                string categorycount = Convert.ToString(Category.EmailMsgcount(6));
                var objcategory = new Category
                {
                    EmailCategoryCount = Convert.ToInt32(categorycount),
                    EmailCategoryID = "6",
                    FromReference_id = "1",
                    Toreference_id = null,
                    ISWebService_IND = "N",
                    Application_Id = null,
                    FromReferenceType_id = "1",
                    Toreferencetype_id = null
                };
                Category.SentEmailLog(objcategory);
                //clsWebConfigsettings obj = new clsWebConfigsettings();
                string strOutMailId = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");

                IDataParameter[] strInsParam = {
			new SqlParameter("@In_ToReferenceType_ID", null),
			new SqlParameter("@In_ToReference_ID", null),
			new SqlParameter("@In_FromReferenceType_ID", null),
			new SqlParameter("@In_FromReference_ID", null),
			new SqlParameter("@In_Practice_ID", null),
			new SqlParameter("@In_MsgOption_ID", null),
			new SqlParameter("@in_TomailID", Request["txtEmail"]),
			new SqlParameter("@in_FromMailID", strOutMailId),
			new SqlParameter("@in_Subject", Request["txtSubject"]),
			new SqlParameter("@in_Message", Sanitizer.GetSafeHtmlFragment(Request["txtMsg"]))
		};

                IDataParameter[] strOutParam = { new SqlParameter("@Out_MsgTransaction_ID", SqlDbType.Int) };
                clsDBWrapper ObjWrapper = new clsDBWrapper();
                clsCommonFunctions ObjCommon = new clsCommonFunctions();
                ClsMailMessage objMailMessage = default(ClsMailMessage);

                var _with1 = ObjWrapper;
                _with1.AddInParameters(strInsParam);
                _with1.AddOutParameters(strOutParam);
                _with1.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_Ins_Message_Pr");
                string strEmailMessage_Transaction_ID = "";
                if (ObjWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value != null)
                {
                    strEmailMessage_Transaction_ID = Convert.ToString(ObjWrapper.objdbCommandWrapper.Parameters["@Out_MsgTransaction_ID"].Value);
                }
                objMailMessage = new ClsMailMessage();
                bool res = objMailMessage.SendMail(Request["txtEmail"], strOutMailId, Request["txtSubject"], Request["txtMsg"], EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (res == true)
                {
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessage_Transaction_ID) };
                    var _with2 = ObjWrapper;
                    _with2.AddInParameters(strMailParam);
                    _with2.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");



                }
            }


            Upd_ReplyStatus(Convert.ToInt32(Request["Hdncontid"]));

            if (TempData["tabid"] != null)
            {
                TempData.Keep("tabid");
                return RedirectToAction("Contactusentries", new { tabid = TempData["tabid"] });
              //  TempData.Keep("tabid");
            }
            else
            {
                return RedirectToAction("Contactusentries");
            }
        }


        public void ViewDetails(int Contact_ID)
        {
            try
            { 
            ListContactInfo objdetails = default(ListContactInfo);

            objdetails = ListContactInfo.View_Contacts(Contact_ID);

            if ((objdetails != null))
            {
                if (objdetails.ContactFirstName != null)
                {
                    ViewBag.ContactFirstName = objdetails.ContactFirstName;
                }
                else
                {
                    ViewBag.ContactFirstName = null;
                }
                if (objdetails.ContactLastName != null)
                {
                    ViewBag.ContactLastName = objdetails.ContactLastName;
                }
                else
                {
                    ViewBag.ContactLastName = null;

                }
                if (objdetails.ContactMail != null)
                {
                    ViewBag.ContactMail = objdetails.ContactMail;
                }
                else
                {
                    ViewBag.ContactMail = null;
                }

                if (objdetails.ContactPhone != null & objdetails.ContactPhone != "")
                {
                    //ViewBag.ContactPhone = clsCommonFunctions.UsPhoneFormat(objdetails.ContactPhone);
                    ViewBag.ContactPhone = objdetails.ContactPhone;
                }
                else
                {
                    ViewBag.ContactPhone = null;

                }
                if (objdetails.ContactSubject != null)
                {
                    ViewBag.ContactSubject = objdetails.ContactSubject;
                }
                else
                {
                    ViewBag.ContactSubject = null;

                }
                if (objdetails.ContactMessage != null)
                {
                    ViewBag.ContactMessage = objdetails.ContactMessage;
                }
                else
                {
                    ViewBag.ContactMessage = null;

                }
                if (objdetails.ReplyStatus_Ind == "True")
                {
                    ViewBag.ReplyStatus_Ind = "Sent";
                }
                else
                {
                    ViewBag.ReplyStatus_Ind = "Not Yet Sent";
                }
                //clsWebConfigsettings obj = new clsWebConfigsettings();
                ViewBag.strOutMailId = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "ViewDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
            }
        }

        private void Upd_ReplyStatus(int Contact_ID)
        {
            try
            {
            ListContactInfo objdetails = default(ListContactInfo);

            objdetails = ListContactInfo.UPD_ReplyStatus(Contact_ID);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "Upd_ReplyStatus", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
            }
        }
        
        public ActionResult Deletecontact(int Contact_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListContactInfo objCont = new ListContactInfo();

            ListContactInfo.DEL_Contacts(Contact_ID);
            return RedirectToAction("Contactusentries");
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Fromveriuserandsup()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            return View();
        }

        public ActionResult Fromveriuser()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 6, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["p1"] == null ? "1" : Request["p1"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }

        public ActionResult Fromverisupervisor()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}

            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 7, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["p2"] == null ? "1" : Request["p2"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }

        public ActionResult MessageArchive(int Msg_id, string tab_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage.ChangeStatusToArchive(Convert.ToInt32(Request["Msg_id"]), Convert.ToInt32(Session["UserID"]), "N");
            if (tab_id == "1")
            {
                return RedirectToAction("Fromverisupervisor", "Messages");
            }
            else if (tab_id == "2")
            {
                return RedirectToAction("Fromveriuser", "Messages");

            }
            else if (tab_id == "3")
            {
                return RedirectToAction("frompractices", "Messages");

            }
            else
            {
                return RedirectToAction("Fromverisupervisor", "Messages");
            }
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult frompractices()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            return View();
        }
        
        public ActionResult Frompaap()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 2, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["p1"] == null ? "1" : Request["p1"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }
        
        public ActionResult Fromnewpaap()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddlListingMsgsType"] != null)
            {
                ViewBag.Type = Request["ddlListingMsgsType"];
            }
            else
            {
                ViewBag.Type = null;
            }
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 4, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["p2"] == null ? "1" : Request["p2"]), "DESC", "SendingDate", ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }
        
        //private string RandomPassword()
        //{
        //    return Convert.ToString(Guid.NewGuid());

        //}
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ComposeMessage1(string Tabid)
        {
          
                ViewBag.Tabid = Tabid;
                ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
                ViewBag.showcontact = "You can also send attachments with your message";
                if (clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin") != "")
                {
                    ViewBag.checkallowpat = clsWebConfigsettings.GetConfigSettingsValue("allowpatientlogin");
                }
                string url = Convert.ToString(Request.Url);
                if (Convert.ToString(Session["Roleid"]) == "31" || Convert.ToString(Session["Roleid"]) == "38")
                {
                    GetSearchDetails(11, Convert.ToInt32(Session["Roleid"]));
                }
                return View();
           
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ComposeMessage1(string msgid, string id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Convert.ToString(Session["RoleID"]) == "1")
            {
                if (Convert.ToString(Request["ddlRoles"]) == "Mower helper")
                {
                    strloginids = Request["combobox1"];
                    RefTypeID = 4;
                }
                else if (Convert.ToString(Request["ddlRoles"]) == "Verification Users")
                {
                    strloginids = Request["combobox2"];
                    RefTypeID = 31;
                }
                else if (Convert.ToString(Request["ddlRoles"]) == "Verification supervisor")
                {
                    strloginids = Request["combobox3"];
                    RefTypeID = 38;
                }
                else if (Convert.ToString(Request["ddlRoles"]) == "To all my Mower helpers")
                {
                    strloginids = null;
                    RefTypeID = 4;
                }
                else if (Convert.ToString(Request["ddlRoles"]) == "To all my verification users")
                {
                    strloginids = null;
                    RefTypeID = 31;
                }
                else if (Convert.ToString(Request["ddlRoles"]) == "To all my verification supervisors")
                {
                    strloginids = null;
                    RefTypeID = 38;
                }
            }
            string strTopic = null;
            if (!string.IsNullOrEmpty(Request["txtTopics"]))
            {
                Sanitizer.GetSafeHtmlFragment(Request["txtTopics"]);
                //  strTopic = Request["txtTopics"];
                strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtTopics"]);
            }
            else
            {
                if (Request["txtMessages"].Length > 50)
                {
                    // strTopic = Request["txtMessages"].Substring(0, 50) + "....";
                    strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]).Substring(0, 50) + "....";
                }
                else
                {
                    strTopic = Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]);
                }
            }
            int MsgCategory_ID = 0;

            Message newMessage = new Message(Convert.ToInt32(Session["Userid"]), strTopic, Sanitizer.GetSafeHtmlFragment(Request["txtMessages"]), MsgCategory_ID, "0", null, null, RefTypeID, strloginids);
            if (Convert.ToString(Request["ddlRoles"]) == "To all my practices" || Convert.ToString(Request["ddlRoles"]) == "To all my verification users" || Convert.ToString(Request["ddlRoles"]) == "To all my verification supervisors")
            {
                if (!newMessage.SaveGroupMesssage())
                {
                    //lblerror.Text = "Could not save Message";
                }
                else
                {
                    RetVal = Convert.ToString(newMessage.Message_ID);
                }
            }
            else
            {
                if (!newMessage.Save())
                {
                    //lblerror.Text = "Could not save Message";

                }
                else
                {
                    RetVal = Convert.ToString(newMessage.Message_ID);
                }
            }

            if (Request.Files["fileAttachment"].ContentLength > 0)
            {
                string FileExtn = null;
                string FileName = null;
                string encFileName = null;
                FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files["fileAttachment"].FileName);
                FileExtn = System.IO.Path.GetExtension(Request.Files["fileAttachment"].FileName);
                FileName = FileName.Replace("#", "");
                encFileName = clsCommonCClist.RandomPassword();
                FileName = FileName + FileExtn;
                encFileName = encFileName + FileExtn;
                string strFilePath = HttpContext.Server.MapPath("../") + "Attachments\\MessageStation" + "\\";
                strFilePath = strFilePath + "\\" + FileName;
                Request.Files["fileAttachment"].SaveAs(strFilePath);
                MessageAttachment objmessage = new MessageAttachment(Convert.ToInt32(RetVal), FileName, encFileName);
                objmessage.SaveAttachment();
                //}
            }
            if (Request["HdnTab_ID"] == "1")
            {
                return RedirectToAction("Contactusentries", "Messages");
            }
            else if (Request["HdnTab_ID"] == "3")
            {
                return RedirectToAction("Fromveriuserandsup", "Messages");
            }
            else if (Request["HdnTab_ID"] == "4")
            {
                return RedirectToAction("frompractices", "Messages");
            }
            else if (Request["HdnTab_ID"] == "6")
            {
                return RedirectToAction("SentMessages", "Messages");
            }

            return View();
        }


        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
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
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Login_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox1"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetFilteredproviders", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public JsonResult LoadingItems2(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredverificationusers(args.Text, 0);

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
        protected ComboBoxItemList GetFilteredverificationusers(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboverificationusersearch(PracticeName, "31");


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Login_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox1"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetFilteredverificationusers", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public JsonResult LoadingItems3(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredverificationsupvisor(args.Text, 0);

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
        protected ComboBoxItemList GetFilteredverificationsupvisor(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboverificationusersearch(PracticeName, "38");


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Login_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox1"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "GetFilteredverificationsupvisor", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ScheduleMessages()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "4");
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            GetCategoriesDataset(Convert.ToString(Session["Userid"]), Convert.ToString(Session["Roleid"]));
            string pgsize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Thpagesize = Request["ddlrecords"];
                pgsize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Thpagesize = Session["Rowsperpage"].ToString();
                pgsize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Thpagesize = 10;
                pgsize = "10";
            }
            int TotalCount = 0;
            //Ms_ListmesssageCollection = ListMessage.GetMs_ListMessages(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]), 1, (Request["ddlListingMsgsType"] == null ? "NEWviewed" : Request["ddlListingMsgsType"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), "DESC", "SendingDate", ref TotalCount);
            Ms_ListmesssageCollection = ListMessage.GetAptrequest_ListMessages(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(pgsize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), ref TotalCount);
            if (Ms_ListmesssageCollection.Count > 0)
            {
                ViewBag.totrec = TotalCount;
            }
            return View(Ms_ListmesssageCollection);
        }


        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult EditAppointment(int apptEid, string apptdate, string recId, string frompage = null)
        {
           
                GetAppointmentDetails(apptEid, apptdate);
                FillTechnicians123(Convert.ToString(apptEid));
                Dictionary<int, string> weeklyitems = new Dictionary<int, string>();
                weeklyitems.Add(1, "Sun");
                weeklyitems.Add(2, "Mon");
                weeklyitems.Add(3, "Tue");
                weeklyitems.Add(4, "Wed");
                weeklyitems.Add(5, "Thu");
                weeklyitems.Add(6, "Fri");
                weeklyitems.Add(7, "Sat");

                ViewBag.weeklylist = weeklyitems;
                TempData["frompage"] = frompage;
                ViewBag.frompage = frompage;
                return View();
            
        }
        
        public void FillTechnicians123(string apptEid)
        {
            try
            {
            TechniciansInfo gettechnician = new TechniciansInfo();
            List<TechniciansInfo> objlist = new List<TechniciansInfo>();
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["Prov_ID"]), apptEid);
            }
            else
            {
                objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["ComboProv_ID"]), apptEid);
            }
            ViewBag.techcnt1 = objlist.Count;
            if (objlist.Count == 1)
            {
                ViewBag.tecid1 = objlist[0].Technician_id;
            }
            ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Technician_id", "Technician_Name");
            ViewData["combobox5"] = custlist;
            ViewData["combobox5_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "MessagesController", "FillTechnicians123", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
            }
        }
        
        private void GetAppointmentDetails(int patId, string apptdate)
        {
            try
            {
                List<GetAppointment> objGetAppointment = GetAppointment.GetPendingAppointment(Convert.ToString(patId), apptdate);
                if (objGetAppointment.Count > 0)
                {
                    ViewBag.ProvName = objGetAppointment[0].ProviderName;
                    ViewBag.patname = objGetAppointment[0].PatientName;
                   // ViewBag.Patnote = objGetAppointment[0].PTNotes;
                    ViewBag.Aptnote = objGetAppointment[0].Notes.Trim();
                    ViewBag.apptid = objGetAppointment[0].Appointment_ID;
                    TempData["ApptType"] = objGetAppointment[0].AppointmentType_ID;
                    ViewBag.techpositions = objGetAppointment[0].TechnicianPositions;
                    if (objGetAppointment[0].AppointmentType_ID == 1)
                    {
                        ViewBag.ApptType = "Client";
                    }
                    else
                    {
                        ViewBag.ApptType = "Blocked";
                    }
                    if ((objGetAppointment[0].AppointmentDate != null))
                    {
                        ViewBag.aptdate = objGetAppointment[0].AppointmentDate;
                    }
                    //IList<SelectListItem> _ddlhr = new List<SelectListItem>();
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "01",
                    //    Value = "01"
                    //});

                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "02",
                    //    Value = "02"
                    //});

                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "03",
                    //    Value = "03"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "04",
                    //    Value = "04"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "05",
                    //    Value = "05"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "06",
                    //    Value = "06"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "07",
                    //    Value = "07"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "08",
                    //    Value = "08"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "09",
                    //    Value = "09"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "10",
                    //    Value = "10"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "11",
                    //    Value = "11"
                    //});
                    //_ddlhr.Add(new SelectListItem
                    //{
                    //    Text = "12",
                    //    Value = "12"
                    //});

                    //IList<SelectListItem> _ddlmin = new List<SelectListItem>();
                    //_ddlmin.Add(new SelectListItem
                    //{
                    //    Text = "00",
                    //    Value = "00"
                    //});
                    //_ddlmin.Add(new SelectListItem
                    //{
                    //    Text = "30",
                    //    Value = "30"
                    //});
                    IList<SelectListItem> _ddlTimeMer = new List<SelectListItem>();

                    _ddlTimeMer.Add(new SelectListItem
                    {
                        Text = "AM",
                        Value = "0"
                    });
                    _ddlTimeMer.Add(new SelectListItem()
                    {
                        Text = "PM",
                        Value = "1"
                    });
                    IList<SelectListItem> _ddlDuration = new List<SelectListItem>();
                    for (int i = 30; i <= 120; i = i + 30)
                    {
                        _ddlDuration.Add(new SelectListItem
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                    }

                    //_ddlDuration.Add(new SelectListItem
                    //{
                    //    Text = "30",
                    //    Value = "30"
                    //});
                    //_ddlDuration.Add(new SelectListItem
                    //{
                    //    Text = "60",
                    //    Value = "60"
                    //});
                    //_ddlDuration.Add(new SelectListItem
                    //{
                    //    Text = "90",
                    //    Value = "90"
                    //});
                    //_ddlDuration.Add(new SelectListItem
                    //{
                    //    Text = "120",
                    //    Value = "120"
                    //});

                    //ViewData["cboEtime"] = timelist;
                    //ViewBag.ddlEhr2 = _ddlhr;
                    //ViewBag.ddlEmin2 = _ddlmin;
                    ViewBag.ddlETimeMer = _ddlTimeMer;
                    ViewBag.ddlEDuration = _ddlDuration;
                    if (objGetAppointment[0].CurrentAppointmentTracking_ID != null)
                    {
                        ViewBag.hidTreckingID = objGetAppointment[0].CurrentAppointmentTracking_ID;
                    }
                    if (objGetAppointment[0].FacilityReference_ID != null)
                    {
                        ViewBag.facRefId = objGetAppointment[0].FacilityReference_ID;
                    }
                    if ((objGetAppointment[0].AppointmentTime != null))
                    {
                        string strTime = objGetAppointment[0].AppointmentTime;
                        ViewBag.strTime = objGetAppointment[0].AppointmentTime;
                        string[] strsplit = strTime.Split(':');
                        string strhr = strsplit[0];
                        ViewBag.gethour = strhr;
                        if (strsplit[1] == "00AM" | strsplit[1] == "00PM")
                        {
                            ViewBag.getmin = "00";
                        }
                        else
                        {
                            ViewBag.getmin = "30";
                        }
                        if (strsplit[1] == "00AM" | strsplit[1] == "30AM")
                        {
                            ViewBag.getAMPM = "0";
                        }
                        else
                        {
                            ViewBag.getAMPM = "1";
                        }
                        ViewBag.comboEtime = ViewBag.gethour + ":" + ViewBag.getmin;
                        string[] timeapplist = { "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" };
                        var timelist = new ComboBoxItemList(timeapplist);
                        var timeitems = new List<ComboBoxItem>();
                        if (ViewBag.comboEtime != null & ViewBag.comboEtime != "")
                        {
                            string timesapp = ViewBag.comboEtime;


                            foreach (ComboBoxItem item in timelist)
                            {
                                if (item.Text == timesapp.Trim())
                                {
                                    item.Selected = true;

                                }
                                timeitems.Add(item);
                            }
                            ViewData["cboEtime"] = timeitems;
                        }
                        else
                        {
                            ViewData["cboEtime"] = timelist;
                            //ViewData["cboyears"] = timeapplist;
                        }
                    }
                    if (objGetAppointment[0].Duration != 0)
                    {
                        ViewBag.getDuration = objGetAppointment[0].Duration;
                    }
                    if (objGetAppointment[0].AppointmentStatus_ID != null)
                    {
                        ViewBag.appstatus = objGetAppointment[0].AppointmentStatus_ID;
                    }
                    ViewBag.hidPatientID = objGetAppointment[0].ToReference_ID;
                    ViewBag.hidPatientLoginID = objGetAppointment[0].ToReferenceLogin_ID;
                    ViewBag.technician_ids = objGetAppointment[0].Technician_ids;
                  //  string tech1 = null;
                  //  string tech2 = null;
                    //if (techid.StartsWith(","))
                    //{
                    //    tech1 = techid.Remove(0, 1);
                    //}
                    //else
                    //{
                    //    tech1 = techid;
                    //}
                    //if (tech1 != null)
                    //{
                    //    if (tech1.EndsWith(","))
                    //    {
                    //        tech2 = tech1.Remove(tech1.Length - 1);
                    //    }
                    //    else
                    //    {
                    //        tech2 = tech1;
                    //    }
                    //}
                    //ViewBag.technician_ids = tech2;
                }

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "MessagesController", "GetAppointmentDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public JsonResult EditAppointment(Models.AddAppointmentModel objUpdateAppointment)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string ApptTime = "";
                string Duration = "";
                string strStartDate = "";
                string Validation_Ind = "N";
                string strmin = null;
                string strMessage = null;
                string apptstatus = null;
                string modify = string.Empty;
                if (Request["apptstatus"] != null)
                {
                    apptstatus = Request["apptstatus"];
                }
                if (apptstatus == "1" || apptstatus == "3" || apptstatus == "5" || apptstatus == "7")
                {
                    if (Request["ddlEmin2"] == "0")
                    {
                        strmin = "00";
                    }
                    else { strmin = Request["ddlEmin2"]; }
                    string strAP = null;
                    if (Request["ddlETimeMer"] == "0")
                    {
                        strAP = "AM";
                    }
                    else
                    {
                        strAP = "PM";
                    }
                    //ApptTime = Request["ddlEhr2"] + ":" + strmin + strAP;
                    ApptTime = Request["cboEtime_SelectedText"] + strAP;
                    Duration = Request["ddlEDuration"];
                    strStartDate = Request["txtEApptDate"];
                }
                else if (apptstatus == "4")
                {
                    strStartDate = Request["txtEApptDate"];
                }
                if (Request["hidPatientLoginID"] != null && Request["hidPatientLoginID"] != "0")
                {
                    objUpdateAppointment.ToReferenceLogin_ID = Convert.ToInt32(Request["hidPatientLoginID"]);
                }
                else
                {
                    objUpdateAppointment.ToReferenceLogin_ID = null;
                }
                if (Request["hidPatientID"] != null && Request["hidPatientID"] != "0")
                {
                    objUpdateAppointment.ToReference_ID = Convert.ToInt32(Request["hidPatientID"]);
                }
                else
                {
                    objUpdateAppointment.ToReference_ID = null;
                }
                if (Request["ApptId"] != null)
                {
                    objUpdateAppointment.Appointment_ID = Convert.ToInt32(Request["ApptId"]);
                }
                else
                {
                    objUpdateAppointment.Appointment_ID = null;
                }

                objUpdateAppointment.FromReferenceType_ID = 2;
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
                }
                else
                {
                    objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                }
                objUpdateAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
                objUpdateAppointment.ToReferenceType_ID = 3;
                if (!string.IsNullOrEmpty( Request["hdnFacRef_ID"] ))
                {
                    objUpdateAppointment.FacilityReference_ID = Convert.ToInt32(Request["hdnFacRef_ID"]);
                }
                else
                {
                    objUpdateAppointment.FacilityReference_ID = null;
                }
                int recId;
                recId = 0;
                objUpdateAppointment.StartDate = strStartDate;
                objUpdateAppointment.EndDate = null;
                objUpdateAppointment.Notes = Sanitizer.GetSafeHtmlFragment(Request["txtENotes"]);
                objUpdateAppointment.AppointmentTime = ApptTime;
                objUpdateAppointment.Duration = Convert.ToInt32(Duration);
                objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(apptstatus);
                if (Convert.ToString(Session["roleid"]) != "39")
                {
                    objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                }
                else
                {
                    objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["Techlog_id"]);
                }
                objUpdateAppointment.IsValidate_Ind = Validation_Ind;
                //objUpdateAppointment.Technician_ids = null;
                //objUpdateAppointment.TechnicianPositions = null;
                objUpdateAppointment.status_ind = "A";
                GetAppointment onjcheck = new GetAppointment();
                string appatdate = strStartDate;
                string torefid = Convert.ToString(objUpdateAppointment.ToReference_ID);
                string apptime = Convert.ToString(objUpdateAppointment.AppointmentTime);
                string prId;

                //if (Request["combobox5"] != null & Request["combobox5"] != "")
                //{
                //    string techids = string.Join(",", Request["combobox5"].Split(',').Distinct().ToArray());
                //    if (techids.EndsWith(","))
                //    {
                //        objUpdateAppointment.Technician_ids = "," + techids;
                //    }
                //    else
                //    {
                //        objUpdateAppointment.Technician_ids = "," + techids + ",";
                //    }
                //}
                //else
                //{
                //    objUpdateAppointment.Technician_ids = "," + Convert.ToString(Request["hdntecid1"]) + ",";
                //}
                //if (Request["hdntechpos"] != null & Request["hdntechpos"] != "")
                //{
                //    string techpos = string.Join(",", Request["hdntechpos"].Split(',').Distinct().ToArray());
                //    if (techpos.EndsWith(","))
                //    {
                //        objUpdateAppointment.TechnicianPositions = "," + techpos;
                //    }
                //    else
                //    {
                //        objUpdateAppointment.TechnicianPositions = "," + techpos + ",";
                //    }
                //    //objUpdateAppointment.TechnicianPositions = Request["hdntechpos"];
                //}
                //else
                //{
                //    objUpdateAppointment.TechnicianPositions = "," + "0" + ",";
                //}
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    prId = Convert.ToString(Session["Prov_ID"]);
                }
                else
                {
                    prId = Convert.ToString(Session["ComboProv_ID"]);
                }
                if (modify == "Y")
                {
                    string apptrecid = string.Empty;

                    if (recId != 0)
                    {
                        apptrecid = Convert.ToString(recId);
                    }

                    strMessage = onjcheck.checkApptExist(appatdate, torefid, apptime, prId, apptrecid);
                }
                if (strMessage == "Y")
                {
                    if (strMessage == "Y")
                    {
                        if (Request["ApptType"] == "Client")
                        {
                            strMessage = "1";
                        }
                        else
                        {
                            strMessage = "2";
                        }
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                    }
                }
                //if (ApptTime != Request["strTime"] || appatdate != Request["Hdnapt_date"])
                //{

                strMessage = onjcheck.checkApptExist(appatdate, torefid, apptime, prId, string.Empty);
                if (strMessage == "Y")
                {
                    if (strMessage == "Y")
                    {
                        if (Request["ApptType"] == "Client")
                        {
                            strMessage = "1";
                        }
                        else
                        {
                            strMessage = "2";
                        }
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                    }
                }
                //}
                string strFuturemessage = null;
                string strChangeRecur_Ind = "";
                if (modify != "Y")
                {
                    strMessage = objUpdateAppointment.EditAppointment(objUpdateAppointment, ref strFuturemessage, ref strChangeRecur_Ind);
                }

                if (!string.IsNullOrEmpty(strMessage))
                {
                    strMessage = strMessage.Replace("<br>", "\r\n");
                    return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.SuccessResponse(objUpdateAppointment), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
            }
            //return null;
        }
        
        public ActionResult ApptDelete(string apptDelid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            DeleteAppointment delappt = new DeleteAppointment();
            delappt.DeleteAppt(apptDelid);
            return RedirectToAction("../Messages/ScheduleMessages");
        }

        #region"Admin Area"
        [ActionName("MessageStationSettings")]
        public ActionResult Msg_Listsettings()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objcls = new ListMessage();
            var objroleList = objcls.FillsettingrolesDdl(null);
            IList<SelectListItem> roleListItems = new List<SelectListItem>();
            if (objroleList.Count > 0)
            {
                for (int i = 0; i < objroleList.Count; i++)
                {
                    roleListItems.Add(new SelectListItem
                    {
                        Text = objroleList[i].RoleName,
                        Value = objroleList[i].Roleid
                    });
                }
            }
            ViewData["rolesDdl"] = roleListItems;
            ViewData["TorolesDdl"] = roleListItems;
            var objlist = objcls.FillActivecategoryDdl();
            IList<SelectListItem> listItems = new List<SelectListItem>();
            if (objlist.Count > 0)
            {
                for (int i = 0; i < objlist.Count; i++)
                {

                    listItems.Add(new SelectListItem
                    {
                        Text = objlist[i].Category,
                        Value = Convert.ToString(objlist[i].MsgCategory_ID)
                    });
                }
            }
            ViewData["ddlCategories"] = listItems;
            string ord;
            if (Request["sortdir"] == null)
            {
                ord = "ASC";
                ViewBag.sortdir = "ASC";
            }
            else
            {
                ord = Request["sortdir"].ToString();
                ViewBag.sortdir = Request["sortdir"].ToString();
            }
            int pno = 1;
            if (Request["page"] == null)
            {
                pno = 1;
            }
            else
            {
                pno = Convert.ToInt32(Request["page"]);
            }
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Summarypagesize = Request["ddlrecords"];
                pgesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                TempData["Summarypagesize"] = Session["Rowsperpage"].ToString();
                pgesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Summarypagesize = "10";
                TempData["Summarypagesize"] = "10";
                pgesize = "10";
            }
            if (pgesize != null)
            {
                pgesize = pgesize;
            }
            else
            {
                pgesize = "10";
            }
            string rId = string.Empty;
            int tId = 0;
            int catId = 0;
            if (Request["rolesDdl"] != null)
            {
                if (Request["rolesDdl"] != "")
                {
                    rId = Request["rolesDdl"];
                    ViewBag.FrmrId = rId;
                }
                else
                {
                    rId = null;
                }
            }
            else
            {
                rId = null;
            }
            if (Request["TorolesDdl"] != null)
            {
                if (Request["TorolesDdl"] != "")
                {
                    tId = Convert.ToInt32(Request["TorolesDdl"]);
                    ViewBag.TorId = tId;
                }
            }
            if (Request["ddlCategories"] != null)
            {
                if (Request["ddlCategories"] != "")
                {
                    catId = Convert.ToInt32(Request["ddlCategories"]);
                    ViewBag.CatId = catId;
                }
            }
            var objlst = new ListMessage
            {
                Roleid = rId,
                ReplyToRoleID = tId,
                MsgCategory_ID = catId,
                OrderByItem = "RoleName",
                OrderBy = ord,
                PageNo = pno,
                NoOfRecords = pgesize

            };
            dynamic settingsGrid = getMessageSettingsGrid(objlst);
            return View("Msg_Listsettings",settingsGrid);
        }
        public List<ListMessage> getMessageSettingsGrid(ListMessage objmessage)
        {
         
            List<ListMessage> messagegrid = new List<ListMessage>();
            try
            {
            messagegrid = objmessage.MessageSettingGrid(objmessage);
            if (messagegrid.Count > 0)
            {
                ViewBag.totrec = ListMessage.TotalRecords;
            }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "MessagesController", "getMessageSettingsGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return messagegrid;
        }
        [ActionName("MessageCategories")]
        public ActionResult Msg_CategoriesListver2()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            if (Request["hdnStatus"] == "Yes")
            {
                ListMessage objchange = new ListMessage();
                if (Request["hdnCId"] != null || Request["hdnCId"] != "")
                {
                    objchange.statusCategory(Convert.ToInt32(Request["hdnCId"]), Convert.ToInt32(Session["Loginhistory_ID"]));
                }
            }

            string ord;
            if (Request["sortdir"] == null)
            {
                ord = "ASC";
                ViewBag.sortdir = "ASC";
            }
            else
            {
                ord = Request["sortdir"].ToString();
                ViewBag.sortdir = Request["sortdir"].ToString();
            }
            int pno = 1;
            if (Request["page"] == null)
            {
                pno = 1;
            }
            else
            {
                pno = Convert.ToInt32(Request["page"]);
            }
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Summarypagesize = Request["ddlrecords"];
                pgesize = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                TempData["Summarypagesize"] = Session["Rowsperpage"].ToString();
                pgesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Summarypagesize = "10";
                TempData["Summarypagesize"] = "10";
                pgesize = "10";
            }
            if (pgesize != null)
            {
                pgesize = pgesize;
            }
            else
            {
                pgesize = "10";
            }
            string messagechar;
            if (Request["txtCategory"] != null)
            {
                messagechar = Convert.ToString(Request["txtCategory"]);
                TempData["txtCategory"] = Convert.ToString(Request["txtCategory"]);
            }
            else
            {
                TempData["txtCategory"] = null;
                messagechar = null;
            }
            var objlst = new ListMessage
            {
                OrderByItem = "MessageCategory",
                OrderBy = ord,
                PageNo = pno,
                NoOfRecords = pgesize,
                Category = messagechar
            };
            dynamic categoryGrid = getMessagelistGrid(objlst);
            return View("Msg_CategoriesListver2", categoryGrid);
        }
        public List<ListMessage> getMessagelistGrid(ListMessage objmessage)
        {
            List<ListMessage> messagegrid = new List<ListMessage>();
            try
            {
            messagegrid = objmessage.getMessageListCategories(objmessage);
            if (messagegrid.Count > 0)
            {
                ViewBag.totrec = ListMessage.TotalRecords;
            }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "MessagesController", "getMessagelistGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return messagegrid;

        }
        public JsonResult getAutoMessage(string term)
        {
            List<ListMessage> objlistmessage = new List<ListMessage>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={
                                      new SqlParameter("@in_searchkeyword",term)
                                       };
            objcommon.AddInParameters(insparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcommon.GetDataReader("Help_dbo.St_CE_Typeahead_MessageStation_Categories");
            while (drlist.Read())
            {
                ListMessage objcat = new ListMessage();
                objcat.Category = Convert.ToString(drlist[0]);
                objlistmessage.Add(objcat);
            }
            return Json(objlistmessage.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Msg_CategoriesAction()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        [HttpPost]
        public ActionResult Msg_CategoriesAction(ListMessage lstmsg)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["grpTye"] == "User")
            {
                lstmsg.userInd = "Y";
                lstmsg.sysInd = "N";
                lstmsg.messagePath = "../MessageStation/Msg_Main.aspx";
            }
            else if (Request["grpTye"] == "System")
            {
                lstmsg.sysInd = "Y";
                lstmsg.userInd = "N";
                if (Request["txt_FilePath"] != null)
                {
                    lstmsg.messagePath = Convert.ToString(Request["txt_FilePath"]);
                }
            }
            if (Request["txt_Name"] != null)
            {
                lstmsg.Category = Convert.ToString(Request["txt_Name"]);
            }
            lstmsg.loginHistory = Convert.ToInt32(Session["Loginhistory_ID"]);
            string strcat = lstmsg.insert_category(lstmsg);
            return RedirectToAction("MessageCategories");
        }
        public ActionResult Msg_CategoriesRoles(int cId)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.cid = cId;
            var objcls = new ListMessage();
            var objlist = objcls.FillcategoryDdl();
            IList<SelectListItem> listItems = new List<SelectListItem>();
            if (objlist.Count > 0)
            {
                for (int i = 0; i < objlist.Count; i++)
                {
                    if (cId != 0)
                    {
                        if (cId == objlist[i].MsgCategory_ID)
                        {
                            listItems.Add(new SelectListItem
                            {
                                Text = objlist[i].Category,
                                Value = Convert.ToString(objlist[i].MsgCategory_ID),
                                Selected = true
                            });
                        }
                        else
                        {

                            listItems.Add(new SelectListItem
                            {
                                Text = objlist[i].Category,
                                Value = Convert.ToString(objlist[i].MsgCategory_ID)
                            });
                        }
                    }
                    else
                    {

                        listItems.Add(new SelectListItem
                        {
                            Text = objlist[i].Category,
                            Value = Convert.ToString(objlist[i].MsgCategory_ID)
                        });
                    }
                }
            }
            ViewData["CatList"] = listItems;
            ListMessage obj = new ListMessage();
            List<ListMessage> objrole = new List<ListMessage>();
            objrole = obj.getRoleSCategory(cId);
            ViewBag.rolenames = objrole;
            ViewBag.total = objrole.Count;
            return View();
        }
        [HttpPost]
        public ActionResult Msg_CategoriesRoles(string[] chkRoles)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string chkIds = string.Empty;
            if (chkRoles != null)
            {

                foreach (var ids in chkRoles)
                {
                    chkIds += ids + ",";

                }

            }
            else
            {
                chkIds = null;
            }
            ListMessage objmes = new ListMessage();
            objmes.MsgCategory_ID = Convert.ToInt32(Request["cid"]);
            objmes.Roleid = chkIds;
            objmes.updRoleCategory(objmes);
            return RedirectToAction("MessageCategories");
        }
        public ActionResult Msg_CategoriesEdit(int cId)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (cId != 0)
            {
                ListMessage objcls = new ListMessage();
                DataSet dsCat = new DataSet();
                dsCat = objcls.GetCategoryinfo(cId);
                if (dsCat.Tables[0].Rows.Count > 0)
                {
                    ViewBag.hdnId = dsCat.Tables[0].Rows[0]["ID"];
                    ViewBag.sysind = dsCat.Tables[0].Rows[0]["Sysgenerated"];
                    ViewBag.userInd = dsCat.Tables[0].Rows[0]["Usergenerated"];
                    ViewBag.txtEdit_Name = dsCat.Tables[0].Rows[0]["Name"];
                    if (ViewBag.sysind == "Y")
                    {
                        ViewBag.txtedit_FilePath = dsCat.Tables[0].Rows[0]["Filepath"];
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Msg_CategoriesEdit()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage objmessage = new ListMessage();
            if (Request["hdnId"] != null)
            {
                objmessage.MsgCategory_ID = Convert.ToInt32(Request["hdnId"]);
            }
            if (Request["grpETye"] == "User")
            {
                objmessage.userInd = "Y";
                objmessage.sysInd = "N";
                objmessage.messagePath = "../MessageStation/Msg_Main.aspx";
            }
            else if (Request["grpETye"] == "System")
            {
                objmessage.sysInd = "Y";
                objmessage.userInd = "N";
                if (Request["txtedit_FilePath"] != null)
                {
                    objmessage.messagePath = Convert.ToString(Request["txtedit_FilePath"]);
                }
            }
            if (Request["txtEdit_Name"] != null)
            {
                objmessage.Category = Convert.ToString(Request["txtEdit_Name"]);
            }
            objmessage.loginHistory = Convert.ToInt32(Session["Loginhistory_ID"]);
            string strcat = objmessage.upd_category(objmessage);
            return RedirectToAction("MessageCategories");
        }
        public ActionResult Msg_SettingAction(int? Fr, int? Tr)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.HdnFr = Fr;
            var objcls = new ListMessage();
            var objroleList = objcls.FillsettingrolesDdl(null);
            IList<SelectListItem> roleListItems = new List<SelectListItem>();
            if (objroleList.Count > 0)
            {
                for (int i = 0; i < objroleList.Count; i++)
                {
                    if (Fr == Convert.ToInt32(objroleList[i].Roleid))
                    {
                        roleListItems.Add(new SelectListItem
                   {
                       Text = objroleList[i].RoleName,
                       Value = objroleList[i].Roleid,
                       Selected = true
                   });
                    }
                    else
                    {
                        roleListItems.Add(new SelectListItem
                   {
                       Text = objroleList[i].RoleName,
                       Value = objroleList[i].Roleid
                   });
                    }

                }
            }
            ViewData["fromrolesDdl"] = roleListItems;
            var objnoroleList = objcls.FillNonExistRolesDdl(Fr);
            IList<SelectListItem> NoroleListItems = new List<SelectListItem>();
            if (objnoroleList.Count > 0)
            {
                for (int i = 0; i < objnoroleList.Count; i++)
                {
                    if (Tr == Convert.ToInt32(objnoroleList[i].Roleid))
                    {
                        NoroleListItems.Add(new SelectListItem
                        {
                            Text = objnoroleList[i].RoleName,
                            Value = objnoroleList[i].Roleid,
                            Selected = true
                        });
                    }
                    else
                    {
                        NoroleListItems.Add(new SelectListItem
                        {
                            Text = objnoroleList[i].RoleName,
                            Value = objnoroleList[i].Roleid
                        });
                    }

                }
            }
            ViewData["toRolesDdl"] = NoroleListItems;
        //    if (Tr != null)
        //    {
        //    }
        //    else
        //    {
        //        Tr = 0;
        //    }
        //    var objCat = objcls.FillinscategoryDdl(Tr);
            IList<SelectListItem> catListItems = new List<SelectListItem>();
        //    if (objCat.Count > 0)
        //    {
        //        for (int i = 0; i < objCat.Count; i++)
        //        {

        //            catListItems.Add(new SelectListItem
        //{
        //    Text = objCat[i].Category,
        //    Value = Convert.ToString(objCat[i].MsgCategory_ID)
        //});
        //        }
        //    }
            ViewData["CatDdl"] = catListItems;
            return View();
        }
        public JsonResult FillRoles(int? Fr, int? Tr)
        {
            var objcls = new ListMessage();
            var objnoroleList = objcls.FillNonExistRolesDdl(Fr);
            //IList<SelectListItem> NoroleListItems = new List<SelectListItem>();
            //if (objnoroleList.Count > 0)
            //{
            //    for (int i = 0; i < objnoroleList.Count; i++)
            //    {
            //        if (Tr == Convert.ToInt32(objnoroleList[i].Roleid))
            //        {
            //            NoroleListItems.Add(new SelectListItem
            //            {
            //                Text = objnoroleList[i].RoleName,
            //                Value = objnoroleList[i].Roleid,
            //                Selected = true
            //            });
            //        }
            //        else
            //        {
            //            NoroleListItems.Add(new SelectListItem
            //            {
            //                Text = objnoroleList[i].RoleName,
            //                Value = objnoroleList[i].Roleid
            //            });
            //        }

            //    }
            //}
            //ViewData["toRolesDdl"] = NoroleListItems;
           
            var objCat = objcls.FillinscategoryDdl(Tr);
            //IList<SelectListItem> catListItems = new List<SelectListItem>();
            //if (objCat.Count > 0)
            //{
            //    for (int i = 0; i < objCat.Count; i++)
            //    {

            //        catListItems.Add(new SelectListItem
            //        {
            //            Text = objCat[i].Category,
            //            Value = Convert.ToString(objCat[i].MsgCategory_ID)
            //        });
            //    }
            //}
            //ViewData["CatDdl"] = catListItems;
            if (Tr != null)
            {
                return Json(JsonResponseFactory.SuccessResponse(objCat), JsonRequestBehavior.AllowGet);
            }
            else
            {
                Tr = 0;
            }
            return Json(JsonResponseFactory.SuccessResponse(objnoroleList), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Msg_SettingAction()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage objsettings = new ListMessage();
            if (Request["fromrolesDdl"] != null)
            {
                objsettings.Roleid = Request["fromrolesDdl"];
            }
            if (Request["toRolesDdl"] != null)
            {
                objsettings.ReplyToRoleID = Convert.ToInt32(Request["toRolesDdl"]);
            }
            if (Request["txtGroupName"] != null)
            {
                objsettings.groupName = Request["txtGroupName"];
            }
            if (Request["CatDdl"] != null)
            {
                objsettings.MsgCategory_ID = Convert.ToInt32(Request["CatDdl"]);
            }
            if (Request["chkUser"] != null && Request["chkUser"] != "" && Request["chkUser"] != "false")
            {
                objsettings.userInd = "Y";
            }
            else
            {
                objsettings.userInd = "N";
            }
            if (Request["chkGroup"] != null && Request["chkGroup"] != "" && Request["chkGroup"] != "false")
            {
                objsettings.groupInd = "Y";
            }
            else
            {
                objsettings.groupInd = "N";
            }
            objsettings.loginHistory = Convert.ToInt32(Session["Loginhistory_ID"]);
            objsettings.insMsgSettings(objsettings);
            return RedirectToAction("MessageStationSettings");
        }
        public ActionResult Msg_SettingEdit(int MrId)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage objedit = new ListMessage();
            DataSet dsEdit = new DataSet();
            dsEdit = objedit.getSetting(MrId);
            ViewBag.MrId = MrId;
            if (dsEdit.Tables[0].Rows.Count > 0)
            {
                ViewBag.txtfromRole = dsEdit.Tables[0].Rows[0]["FromRole"];
                ViewBag.txtToRole = dsEdit.Tables[0].Rows[0]["ToRole"];
                ViewBag.txtEGroupName = dsEdit.Tables[0].Rows[0]["GroupName"];
                ViewBag.txtCategorye = dsEdit.Tables[0].Rows[0]["Category"];
                ViewBag.CategoryID = dsEdit.Tables[0].Rows[0]["CategoryID"];
                ViewBag.SendInd = dsEdit.Tables[0].Rows[0]["SendInd"];
                ViewBag.GroupInd = dsEdit.Tables[0].Rows[0]["GroupInd"];
                ViewBag.roleId = dsEdit.Tables[0].Rows[0]["FromID"];
            }
            var objCat = objedit.FillinscategoryDdl(ViewBag.roleId);
            IList<SelectListItem> catListItems = new List<SelectListItem>();
            if (objCat.Count > 0)
            {
                for (int i = 0; i < objCat.Count; i++)
                {
                    if (ViewBag.CategoryID == objCat[i].MsgCategory_ID)
                    {
                        catListItems.Add(new SelectListItem
                        {
                            Text = objCat[i].Category,
                            Value = Convert.ToString(objCat[i].MsgCategory_ID),
                            Selected = true
                        });
                    }
                    else
                    {
                        catListItems.Add(new SelectListItem
                        {
                            Text = objCat[i].Category,
                            Value = Convert.ToString(objCat[i].MsgCategory_ID)
                        });
                    }

                }
            }
            ViewData["EditCatDdl"] = catListItems;
            return View();
        }
        [HttpPost]
        public ActionResult Msg_SettingEdit()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage objsettings = new ListMessage();
            if (Request["MrId"] != null)
            {
                objsettings.MrId = Convert.ToInt32(Request["MrId"]);
            }
            if (Request["txtEGroupName"] != null)
            {
                objsettings.groupName = Request["txtEGroupName"];
            }
            if (Request["EditCatDdl"] != null)
            {
                objsettings.MsgCategory_ID = Convert.ToInt32(Request["EditCatDdl"]);
            }
            if (Request["EchkUser"] != "false")
            {
                objsettings.userInd = "Y";
            }
            else
            {
                objsettings.userInd = "N";
            }
            if (Request["EchkGroup"] != "false")
            {
                objsettings.groupInd = "Y";
            }
            else
            {
                objsettings.groupInd = "N";
            }
            objsettings.loginHistory = Convert.ToInt32(Session["Loginhistory_ID"]);
            objsettings.UpdMsgSettings(objsettings);
            return RedirectToAction("MessageStationSettings");

        }
        public ActionResult DeleteSettings(int MrId)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ListMessage objset = new ListMessage();
            objset.delsettings(MrId, Convert.ToInt32(Session["Loginhistory_ID"]));
            return RedirectToAction("MessageStationSettings");
        }
        #endregion
        public JsonResult Getaddress(int patid)
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            Patient_Info obj = new Patient_Info();
            obj.PracticeID = Convert.ToString(Session["Practice_ID"]);
            obj.PatientID = Convert.ToString(patid);
            obj.ProviderID = Convert.ToString(Session["Prov_ID"]);
            obj = Patient_Info.ViewPatineInfo(obj);
            obj.l_GoogleMapPath = obj.l_GoogleMapPath.Replace("+", ",");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
