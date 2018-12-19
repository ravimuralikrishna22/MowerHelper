using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.Classes;



namespace MowerHelper.Controllers
{
    public class TechnicalareaController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Webconfigsettings()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            int NoofRecords = 0;

            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Convert.ToInt32(Request["ddlrecords"].ToString());
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Convert.ToInt32(Session["Rowsperpage"].ToString());
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = 10;
            }
            int? In_Parameter_ID = null;
            In_Parameter_ID = null;
            int PageNo = (Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1);
            string OrderBy = null;
            OrderBy = "ASC";
            string OrderBYitem = null;
            OrderBYitem = "Parameter_Key";
            string ParameterKey = null;
            ParameterKey = (Request["txtParameterKeySearch"] != null ? Request["txtParameterKeySearch"] : null);
            ViewBag.search = (Request["txtParameterKeySearch"] != null ? Request["txtParameterKeySearch"] : null);
            List<WebConfigSettings> Ad_WebConfigSettingsCollection = null;
            WebConfigSettings objWebConfigSettings = new WebConfigSettings(In_Parameter_ID, NoofRecords, PageNo, OrderBy, OrderBYitem, ParameterKey);
            int totcount = 0;
            Ad_WebConfigSettingsCollection = WebConfigSettings.Admin_List_WebConfigSettings(objWebConfigSettings, ref totcount);
            ViewBag.totrec = totcount;
            return View(Ad_WebConfigSettingsCollection);

        }
        public ActionResult Addnew()
        {
           
                return View();
         
           
        }
        [HttpPost]
        public ActionResult Addnew(string obj)
        {
           //if (Session["UserID"] == null)
           // {
           //     return RedirectToAction("SessionExpire", "Home");
           // }
                string In_Parameter_Key = null;
                In_Parameter_Key = (!string.IsNullOrEmpty(Request["txtParameter_Key"]) ? Request["txtParameter_Key"] : null);
                string In_Parameter_Value = null;
                In_Parameter_Value = (!string.IsNullOrEmpty((Request["txtParameter_Value"])) ? Request["txtParameter_Value"] : null);
                string In_Inuse_Ind = null;
                if (Request["rblInUseyes"] == "1")
                {
                    In_Inuse_Ind = "Y";
                }
                else
                {
                    In_Inuse_Ind = "N";
                }
                string in_Description = null;
                in_Description = (Request["txtDescription"] != null ? Request["txtDescription"] : null);
                int In_ProjectStatus_ID = 0;
                In_ProjectStatus_ID = Convert.ToInt32(Request["ddlProject_Status_Ind"]);
                int in_CreatedBy = 0;
                in_CreatedBy = Convert.ToInt32(Session["userid"]);
                int Loginhistory_ID = 0;
                Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                WebConfigSettings objWebConfigSettings = new WebConfigSettings(Convert.ToInt32(Request["ddlProject"]), In_ProjectStatus_ID, In_Parameter_Key, In_Parameter_Value, In_Inuse_Ind, in_Description, in_CreatedBy, Loginhistory_ID);
                WebConfigSettings.Ad_INS_WebConfigSettings(objWebConfigSettings);
                //var objconfig = new clsWebConfigsettings();
                clsWebConfigsettings.SaveConfigSettings();
                return RedirectToAction("Webconfigsettings");
           
        }
        public ActionResult EditWebconfig(string Parameter_ID)
        {
           
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] objparm = { new SqlParameter("@In_Parameter_ID", Parameter_ID) };
                objclscommon.AddInParameters(objparm);
                SqlDataReader objReader = default(SqlDataReader);
                objReader = objclscommon.GetDataReader("Help_dbo.st_Application_GET_WebConfigSettings");
                if (objReader.Read())
                {
                    if (objReader["Project_ID"] != null)
                    {
                        ViewBag.Project_ID = objReader["Project_ID"];
                    }
                    if (objReader["ProjectStatus_ID"] != null)
                    {
                        ViewBag.ProjectStatus_ID = objReader["ProjectStatus_ID"];
                    }
                    if (objReader["Parameter_Key"] != null)
                    {
                        ViewBag.Parameter_Key = objReader["Parameter_Key"];
                    }
                    if (!DBNull.Value.Equals(objReader["Parameter_Value"]))
                    {
                        ViewBag.Parameter_Value = objReader["Parameter_Value"];
                    }
                    else
                    {
                        ViewBag.Parameter_Value = "";
                    }
                    if (objReader["Description"] != null)
                    {
                        ViewBag.Description = objReader["Description"];
                        ViewBag.txt_Description = objReader["Description"];
                    }
                    else
                    {
                        ViewBag.Description = "";
                    }
                    if (objReader["Inuse_Ind"] != null)
                    {
                        ViewBag.Inuse_Ind = objReader["Inuse_Ind"];
                    }
                }
                ViewBag.Parameter_ID = Parameter_ID;
                return View();
           
        }
        [HttpPost]
        public ActionResult EditWebconfig()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                int In_Parameter_ID = 0;
                In_Parameter_ID = Convert.ToInt32(Request["HdnParameter_ID"]);
                string In_Parameter_Value = null;
                In_Parameter_Value = (!string.IsNullOrEmpty(Request["txtParameter"]) ? Request["txtParameter"] : null);
                string In_Inuse_Ind = null;
                if (Request["rbtnInUseyes"] == "1")
                {
                    In_Inuse_Ind = "Y";
                }
                else
                {
                    In_Inuse_Ind = "N";
                }
                string in_Description = null;
                in_Description = (!string.IsNullOrEmpty(Request["txt_Description"]) ? Request["txt_Description"] : null);
                int In_ProjectStatus_ID = 0;
                In_ProjectStatus_ID = Convert.ToInt32(Request["ddlProject_Ind"]);
                int in_UpdatedBy = 0;
                in_UpdatedBy = Convert.ToInt32(Session["userid"]);
                int Loginhistory_ID = 0;
                Loginhistory_ID = Convert.ToInt32(Session["Loginhistory_ID"]);
                WebConfigSettings objWebConfigSettings = new WebConfigSettings(Convert.ToInt32(Request["ddlProj"]), In_Parameter_ID, (!string.IsNullOrEmpty((Request["txtParameter_Key_edit"])) ? Request["txtParameter_Key_edit"] : null), In_Parameter_Value, In_Inuse_Ind, in_Description, In_ProjectStatus_ID, in_UpdatedBy, Loginhistory_ID);
                WebConfigSettings.Ad_UPD_WebConfigSettings(objWebConfigSettings);

                //var objconfig = new clsWebConfigsettings();
                clsWebConfigsettings.SaveConfigSettings();

                return RedirectToAction("Webconfigsettings");
           
        }

        public JsonResult GetWebConfigParameterKey(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
		new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.St_Typeahead_getWebConfigParameterKey");
            while (drlist.Read())
            {
                objlist.Add(drlist[0].ToString());
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Emailcontent()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddllistCategory"] != null & Request["ddllistCategory"] != "")
            {
                ViewBag.cat = Request["ddllistCategory"];
            }
            else
            {
                ViewBag.cat = "0";
            }

            if (Request["ddllistSubCategory"] != null & Request["ddllistSubCategory"] != "")
            {
                ViewBag.subcat = Request["ddllistSubCategory"];
            }
            else
            {
                ViewBag.subcat = "0";
            }

            List<EmailMessageOption> catlist = new List<EmailMessageOption>();
            EmailMessageOption EMO = new EmailMessageOption();
            catlist = EMO.ADMIN_List_Main_Email_OptionTypes();
            IList<SelectListItem> _result1 = new List<SelectListItem>();
            if (catlist.Count > 0)
            {
                for (int i = 0; i <= catlist.Count - 1; i++)
                {
                    _result1.Add(new SelectListItem
                    {
                        Text = catlist[i].Message_Title,
                        Value = catlist[i].EmailMessage_Option_ID.ToString()
                    });
                }
            }
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            if (Request["ddllistCategory"] != null & Request["ddllistCategory"] != "")
            {
                List<EmailMessageOption> Subcatlist = new List<EmailMessageOption>();
                EmailMessageOption EMO1 = new EmailMessageOption();
                EMO1.Email_OptionType_ID = Convert.ToInt32(Request["ddllistCategory"]);
                Subcatlist = EMO1.ADMIN_List_ExistingEmail_OptionTypes(EMO1);
                if (Subcatlist.Count > 0)
                {

                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Subcategory--",
                        Value = "0",
                        Selected = true
                    });

                    for (int i = 0; i <= Subcatlist.Count - 1; i++)
                    {
                        _result2.Add(new SelectListItem
                        {

                            Text = Subcatlist[i].Message_Title,
                            Value = Subcatlist[i].EmailMessage_Option_ID.ToString()
                        });
                    }
                }
                else
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Subcategory--",
                        Value = "0",
                        Selected = true
                    });
                }
            }
            else
            {
                _result2.Add(new SelectListItem
                {
                    Text = "--Select Subcategory--",
                    Value = "0",
                    Selected = true
                });
            }
            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result1,
                CityList = _result2
            };
            return View(reg1);
        }

        public ActionResult EmailcontentGrid()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            EmailMessageOption EMO = new EmailMessageOption();
            List<EmailMessageOption> EMailmsglist = new List<EmailMessageOption>();

            int NoofRecords = 0;

            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Convert.ToInt32(Request["ddlrecords"].ToString());
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Convert.ToInt32(Session["Rowsperpage"].ToString());
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = 10;
            }

            EMO.EmailMessageOptionType_ID = null;
            EMO.Email_OptionType_ID = null;
            EMO.NoOfRecords = NoofRecords;
            EMO.pageNo = (Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1);
            EMO.OrderBy = "ASC";
            EMO.OrderByItem = "Message_type";
            if (Request["ddllistCategory"] != null & Request["ddllistCategory"] != "0" & Request["ddllistCategory"] != "")
            {
                EMO.EmailMessageOptionType_ID = Convert.ToInt32(Request["ddllistCategory"]);
            }
            if (Request["ddllistSubCategory"] != null & Request["ddllistSubCategory"] != "0" & Request["ddllistSubCategory"] != "")
            {
                if (EMO.EmailMessageOptionType_ID != null)
                {
                    EMO.EmailMessage_Option_ID = Convert.ToInt32(Request["ddllistSubCategory"]);
                }
                else
                {
                    EMO.EmailMessage_Option_ID = null;
                }

            }
            int totalrecords = 0;
            EMailmsglist = EMO.ListEmailMessageOptions(EMO, ref totalrecords);
            ViewBag.totrec = totalrecords;
            return View(EMailmsglist);

        }
        public ActionResult Emailcontentadd()
        {
            ViewBag.Count = 5;
            Session["CurrentUrl4"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            List<EmailMessageOption> catlist = new List<EmailMessageOption>();
            EmailMessageOption EMO = new EmailMessageOption();
            catlist = EMO.ADMIN_List_Main_Email_OptionTypes();
            IList<SelectListItem> _result3 = new List<SelectListItem>();
            if (catlist.Count > 0)
            {
                for (int i = 0; i <= catlist.Count - 1; i++)
                {
                    _result3.Add(new SelectListItem
                    {
                        Text = catlist[i].Message_Title,
                        Value = catlist[i].EmailMessage_Option_ID.ToString()
                    });
                }
            }
            IList<SelectListItem> _result4 = new List<SelectListItem>();

            _result4.Add(new SelectListItem
            {
                Text = "--Select Subcategory--",
                Value = "0",
                Selected = true
            });
            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result3,
                CityList = _result4
            };
            return View(reg1);
        }
        [HttpPost]
        public ActionResult Emailcontentadd(string obj)
        {

            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            EmailMessageOption EMO = new EmailMessageOption();
            EMO.EmailMessage_Option_ID = Convert.ToInt32(Request["DDLCategorysub"]);
            // End If
            EMO.Msg_Subject = (Request["txtSubjectTitle"] != null ? Request["txtSubjectTitle"] : null);
            EMO.Msg_Body = (Request["txtSubjectText"] != null ? Request["txtSubjectText"] : null);
            EMO.Msg_Footer = (Request["txtFooter"] != null ? Request["txtFooter"] : null);
            EMO.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);
            EMO.SaveEmailMessageOption();
            return RedirectToAction("Emailcontent");
        }
        public ActionResult EmailcontentEdit(int EmailMsgOptionBody_ID, string Title)
        {
            ViewBag.Count = 5;
            Session["CurrentUrl4"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            clsCommonFunctions objclscommon = new clsCommonFunctions();
            IDataParameter[] objparm = { new SqlParameter("@In_EmailMsgOptionBody_ID", EmailMsgOptionBody_ID) };
            objclscommon.AddInParameters(objparm);
            EmailMessageOption EMO = new EmailMessageOption();

            EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(Convert.ToInt32(EmailMsgOptionBody_ID), null);

            if ((EMO != null))
            {

                ViewBag.EmailMessage_Option_ID = EMO.EmailMessage_Option_ID;
                ViewBag.EmailMsgOptionBody_ID = EmailMsgOptionBody_ID;

                if (Title != null)
                {
                    ViewBag.category = Title;
                }
                else
                {
                    ViewBag.category = null;
                }

                if ((EMO.Title != null))
                {
                    ViewBag.subcategory = EMO.Title;
                }
                else
                {
                    ViewBag.subcategory = "";
                }
                if ((EMO.Msg_Subject != null))
                {
                    ViewBag.Edit_txtSubjectTitle = EMO.Msg_Subject;
                }
                else
                {
                    ViewBag.Edit_txtSubjectTitle = null;
                }
                if ((EMO.Msg_Body != null))
                {
                    ViewBag.Edit_txtSubjectText = EMO.Msg_Body;
                }
                else
                {
                    ViewBag.Edit_txtSubjectText = null;
                }
                if (ViewBag.Edit_txtSubjectText != null & ViewBag.Edit_txtSubjectText != "")
                {
                    ViewData["Edit_txtSubjectText"] = System.Web.HttpUtility.HtmlDecode(ViewBag.Edit_txtSubjectText);
                }
                else
                {
                    ViewData["Edit_txtSubjectText"] = string.Empty;
                }
                if ((EMO.Msg_Footer != null))
                {
                    ViewBag.Edit_txtFooter = EMO.Msg_Footer;
                }
                else
                {
                    ViewBag.Edit_txtFooter = null;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult EmailcontentEdit()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            EmailMessageOption EMO = new EmailMessageOption();
            EMO.EmailMsgOptionBody_ID = Convert.ToInt32(Request["HdnOptionBody_ID"]);
            EMO.Msg_Subject = (Request["Edit_txtSubjectTitle"] != null ? Request["Edit_txtSubjectTitle"] : null);
            EMO.Msg_Body = (Request["Edit_txtSubjectText"] != null ? Request["Edit_txtSubjectText"] : null);
            EMO.Msg_Footer = (Request["Edit_txtFooter"] != null ? Request["Edit_txtFooter"] : null);
            EMO.EmailMessage_Option_ID = Convert.ToInt32(Request["HdnOption_ID"]);
            EMO.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);
            EMO.UpdateEmailMessageOption();
            return RedirectToAction("Emailcontent");
        }

        public JsonResult GetCitiesByStates(int id)
        {

            List<EmailMessageOption> Subcatlist = new List<EmailMessageOption>();
            EmailMessageOption EMO1 = new EmailMessageOption();
            EMO1.Email_OptionType_ID = id;
            Subcatlist = EMO1.ADMIN_List_ExistingEmail_OptionTypes(EMO1);
            IList<SelectListItem> _result = new List<SelectListItem>();
            if (Subcatlist.Count > 0)
            {
                for (int i = 0; i <= Subcatlist.Count - 1; i++)
                {
                    _result.Add(new SelectListItem
                    {
                        Text = Subcatlist[i].Message_Title,
                        Value = Subcatlist[i].EmailMessage_Option_ID.ToString()
                    });
                }
            }
            else
            {
                _result.Add(new SelectListItem
                {
                    Text = "--Select Subcategory--",
                    Value = "0",
                    Selected = true
                });
            }
            StateCity reg1 = new StateCity();
            reg1 = new StateCity { CityList = _result };
            return Json(reg1.CityList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Emailoption()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            List<EmailMessageOption> EMailoptlist = new List<EmailMessageOption>();

            EmailMessageOption EMO = new EmailMessageOption();
            int Total_records = 0;
            EMailoptlist = EMO.ListEmail_Main_MessageOptions(EMO, ref Total_records);
            return View(EMailoptlist);
        }
        public ActionResult AddEmailoption()
        {
           
                List<EmailMessageOption> optlist = new List<EmailMessageOption>();
                EmailMessageOption EMO = new EmailMessageOption();

                optlist = EMO.ADMIN_List_Main_Email_OptionTypes();
                IList<SelectListItem> _result1 = new List<SelectListItem>();
                if (optlist.Count > 0)
                {
                    for (int i = 0; i <= optlist.Count - 1; i++)
                    {
                        _result1.Add(new SelectListItem
                        {
                            Text = optlist[i].Title,
                            Value = optlist[i].EmailMessageOptionType_ID.ToString()
                        });
                    }
                }
                StateCity reg1 = new StateCity();
                reg1 = new StateCity
                {
                    StateList = _result1
                };
                return View(reg1);
           
        }
        [HttpPost]
        public ActionResult AddEmailoption(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                EmailMessageOption EMO = new EmailMessageOption();

                EMO.SaveMainEmailMessageOption(Convert.ToInt32(Request["DDLCategory"]), (Request["txtTitle"] != null ? Request["txtTitle"] : null));
                return RedirectToAction("Emailoption");
           
        }
        public ActionResult EditEmailoption(int EmailMessage_Option_ID, string Title)
        {
           
                ViewBag.Option_ID = EmailMessage_Option_ID;
                ViewBag.Title = Title;

                EmailMessageOption EMO1 = default(EmailMessageOption);
                EMO1 = EmailMessageOption.GetEmail_Main_MessageOptionbyID(Convert.ToInt32(EmailMessage_Option_ID));
                if ((EMO1 != null))
                {
                    if ((EMO1.EmailMessageOptionType_ID != null))
                    {
                        ViewBag.Category = EMO1.EmailMessageOptionType_ID;
                    }
                    if ((EMO1.Message_Title != null))
                    {
                        ViewBag.Edit_txtTitle = EMO1.Message_Title;
                    }
                }
                return View();
          
        }
        [HttpPost]
        public ActionResult EditEmailoption()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                EmailMessageOption EMO = new EmailMessageOption();

                EMO.UpdateMainEmailMessageOption(Convert.ToInt32(Request["hdnoptionid"]), (Request["Edit_txtTitle"] != null ? Request["Edit_txtTitle"] : null));
                return RedirectToAction("Emailoption");
           
        }
        public ActionResult MainEmailoption()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            clsCommonFunctions objclscommon = new clsCommonFunctions();
            string In_EmailMessageOptionType_ID = null;
            In_EmailMessageOptionType_ID = null;
            List<EmailOptions> collection = default(List<EmailOptions>);
            EmailOptions obj = new EmailOptions();
            obj.EmailMessageOptionType_ID = In_EmailMessageOptionType_ID;
            int Total_records = 0;
            collection = EmailOptions.Emails_List(obj, ref Total_records);
            return View(collection);
        }
        public ActionResult AddMainoption()
        {
          
                return View();
           
        }
        [HttpPost]
        public ActionResult AddMainoption(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                clsCommonFunctions objclscomm = new clsCommonFunctions();
                EmailOptions objemail = new EmailOptions();
                objemail.EmailMessageOptionType_ID = null;
                objemail.Title = Request["txtEmailOptiontype"];
                objemail.Description = Request["txtDescription"];
                objemail.CreatedBy = Convert.ToString(Session["userid"]);

                EmailOptions.Emails_insert(objemail);
                return RedirectToAction("MainEmailoption");
            
        }
        public ActionResult EditMainaction(string EmailMessageOptionType_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                ViewBag.typeid = EmailMessageOptionType_ID;
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                EmailOptions objemail = new EmailOptions();

                objemail = EmailOptions.Emails_Get(Convert.ToInt32(EmailMessageOptionType_ID));

                if (!string.IsNullOrEmpty(objemail.Title))
                {
                    ViewBag.Title = objemail.Title;
                }
                else
                {
                    ViewBag.Title = null;
                }
                if (!string.IsNullOrEmpty(objemail.Description))
                {
                    ViewBag.Description = objemail.Description;
                }
                else
                {
                    ViewBag.Description = null;
                }
                return View();
          
        }
        [HttpPost]
        public ActionResult EditMainaction()
        {

            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            clsCommonFunctions objclscomm = new clsCommonFunctions();
            EmailOptions objemail = new EmailOptions();
            objemail.EmailMessageOptionType_ID = Request["Hdntypeid"];
            objemail.Title = Request["Edit_txtEmailOptiontype"];
            objemail.Description = Request["Edit_txtDescription"];
            objemail.UpdatedBy = Convert.ToString(Session["userid"]);

            EmailOptions.Emails_upd(objemail);
            return RedirectToAction("MainEmailoption");
        }
        public ActionResult DeleteMainaction(string EmailMessageOptionType_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            clsCommonFunctions objclscommon = new clsCommonFunctions();
            EmailOptions objemp = new EmailOptions();
            objemp.EmailMessageOptionType_ID = EmailMessageOptionType_ID;
            objemp.UpdatedBy = Convert.ToString(Session["userid"]);

            EmailOptions.Emails_Delete(objemp);
            return RedirectToAction("MainEmailoption");
        }
        public ActionResult Errors()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddlsite"] != null)
            {
                ViewBag.ddlsite = Request["ddlsite"];
            }
            else
            {
                ViewBag.ddlsite = "20";
            }
            if (Request["dd_Classes"] != null & Request["dd_Classes"] != "")
            {
                ViewBag.classes = Request["dd_Classes"];
            }
            else
            {
                ViewBag.classes = "0";
            }

            if (Request["dd_Events"] != null & Request["dd_Events"] != "")
            {
                ViewBag.events = Request["dd_Events"];
            }
            else
            {
                ViewBag.events = "0";
            }
            if (Request["ddlmobile"] != "" && Request["ddlmobile"] != null)
            {
                ViewBag.Mobile_type = Request["ddlmobile"].ToString();

            }
            else
            {
                ViewBag.Mobile_type = "0";
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            List<ErrorsList> classlist = default(List<ErrorsList>);
            ErrorsList objEL = new ErrorsList();
            objEL.Project_ID = Convert.ToInt32(Request["ddlsite"] == null ? "20" : Request["ddlsite"]);

            classlist = ErrorsList.DDL_CLS(objEL);



            IList<SelectListItem> _result1 = new List<SelectListItem>();
            if (classlist.Count > 0)
            {
                for (int i = 0; i <= classlist.Count - 1; i++)
                {
                    _result1.Add(new SelectListItem
                    {
                        Text = classlist[i].Class_Name,
                        Value = classlist[i].Class_ID.ToString()
                    });
                }
            }
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            if (Request["dd_Classes"] != null & Request["dd_Classes"] != "")
            {
                List<ErrorsList> Methodlist = default(List<ErrorsList>);
                ErrorsList objEc = new ErrorsList();
                objEc.Class_ID = Convert.ToString(Request["dd_Classes"]);

                Methodlist = ErrorsList.DDL_MTHDS(objEc);
                if (Methodlist.Count > 0)
                {

                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Events--",
                        Value = "0",
                        Selected = true
                    });

                    for (int i = 0; i <= Methodlist.Count - 1; i++)
                    {
                        _result2.Add(new SelectListItem
                        {

                            Text = Methodlist[i].Method_Name,
                            Value = Methodlist[i].Method_ID.ToString()
                        });
                    }
                }
                else
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Events--",
                        Value = "0",
                        Selected = true
                    });
                }
            }
            else
            {
                _result2.Add(new SelectListItem
                {
                    Text = "--Select Events--",
                    Value = "0",
                    Selected = true
                });
            }
            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result1,
                CityList = _result2
            };

            return View(reg1);
        }
        public ActionResult ErrorsGrid()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
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

            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }

            string Str_ClassID = null;
            if (Request["dd_Classes"] != "0" & Request["dd_Classes"] != "")
            {
                Str_ClassID = Request["dd_Classes"];
            }
            else
            {
                Str_ClassID = null;
            }
            string Str_MethodID = null;

            if (Request["dd_Events"] != "0" & Request["dd_Events"] != "")
            {
                Str_MethodID = Request["dd_Events"];
            }
            else
            {
                Str_MethodID = null;
            }

            List<ErrorsList> objCL = new List<ErrorsList>();
            ErrorsList objerr = new ErrorsList();
            if (Request["ddlmobile"] != "" && Request["ddlmobile"] != null)
            {
                ViewBag.Mobile_type = Request["ddlmobile"].ToString();

            }
            else
            {
                ViewBag.Mobile_type = "0";
            }
            objerr.Mobile_type = null;
            objerr.Project_ID = Convert.ToInt32(Request["ddlsite"] == null ? "20" : Request["ddlsite"]);
            objerr.Class_ID = Str_ClassID;
            objerr.Method_ID = Str_MethodID;
            objerr.Error_BeginDate = FromDate;
            objerr.Error_EndDate = ToDate;
            objerr.NoofRecords = Convert.ToInt32(pgesize);
            objerr.pageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            objerr.OrderBy = "Desc";
            objerr.OrderByItem = "InsertDate";
            objCL = ErrorsList.EList(objerr);
            ViewBag.totrec = ErrorsList.TotalRecords;
            return View(objCL);
        }
        public ActionResult Errordetails(int Log_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
          
                GetErrorDetials(Log_ID);
                return View();
           
        }
        public ActionResult MobileErrorsGrid()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
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

            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }

            string Str_ClassID = null;
            if (Request["dd_Classes"] != "0" & Request["dd_Classes"] != "")
            {
                Str_ClassID = Request["dd_Classes"];
            }
            else
            {
                Str_ClassID = null;
            }
            string Str_MethodID = null;

            if (Request["dd_Events"] != "0" & Request["dd_Events"] != "")
            {
                Str_MethodID = Request["dd_Events"];
            }
            else
            {
                Str_MethodID = null;
            }
            List<ErrorsList> objCL = new List<ErrorsList>();
            ErrorsList objerr = new ErrorsList();
            if (Request["ddlmobile"] != "" && Request["ddlmobile"] != null && Request["ddlmobile"] != "0")
            {
                objerr.Mobile_type = Request["ddlmobile"].ToString();

            }
            else
            {
                objerr.Mobile_type = null;
            }
            objerr.Project_ID = Convert.ToInt32(Request["ddlsite"] == null ? "20" : Request["ddlsite"]);
            objerr.Class_ID = Str_ClassID;
            objerr.Method_ID = Str_MethodID;
            objerr.Error_BeginDate = FromDate;
            objerr.Error_EndDate = ToDate;
            objerr.NoofRecords = Convert.ToInt32(pgesize);
            objerr.pageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            objerr.OrderBy = "Desc";
            objerr.OrderByItem = "InsertDate";
            objCL = ErrorsList.EList(objerr);
            ViewBag.totrec = ErrorsList.TotalRecords;
            return View(objCL);
        }

        public ActionResult MobileErrordetails(int Log_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            
                GetErrorDetials(Log_ID);
                return View();
           
        }

        private void GetErrorDetials(int Int_Log_ID)
        {
            try
            {
            ErrorsList objErr = new ErrorsList();

            objErr = ErrorsList.Get_Error_Details(Int_Log_ID);

            if ((objErr != null))
            {
                ViewBag.lbl_Date = objErr.InsertDate;
                if (objErr.Class_Name != null & objErr.Class_Name != "")
                {
                    ViewBag.lbl_Class = objErr.Class_Name;
                }
                else
                {
                    ViewBag.lbl_Class = " - ";
                }
                if (objErr.Method_Name != null & objErr.Method_Name != "")
                {
                    ViewBag.lbl_Method = objErr.Method_Name;
                }
                else
                {
                    ViewBag.lbl_Method = " - ";
                }
                ViewBag.Name = objErr.Name;
                ViewBag.lbl_ErrorType = objErr.ErrCategory;
                ViewBag.lbl_FileName = objErr.ErrFile;
                ViewBag.lbl_PgUrl = objErr.URL;
                ViewBag.lbl_LineNo = objErr.ErrLine;
                ViewBag.lbl_Description = objErr.ErrDescription;
                ViewBag.lbl_IPAddress = objErr.LocalAddr;
                ViewBag.HostAddress = objErr.HostAddress;
                ViewBag.lbl_RequestedMethod = objErr.RequestMethod;
                ViewBag.lbl_BrowserDetials = objErr.UserAgent;
                ViewBag.lbl_ReferenceNo = objErr.CustomerRefID;
                ViewBag.Exception_Name = objErr.Exception_Name;
                ViewBag.ErrSource = objErr.ErrSource;
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "TechnicalareaController", "GetErrorDetials", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult Eventlog()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            EventLog aLog = new EventLog();
            aLog.Log = "System";
            aLog.MachineName = ".";
            List<EventLogEntry> arr = new List<EventLogEntry>();
            foreach (EventLogEntry entry in aLog.Entries)
            {
                arr.Add(entry);
            }

            arr.Reverse();

            return View(arr);
        }
        public ActionResult Loginhistorydetails()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
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
            int NoofRecords = 0;

            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Convert.ToInt32(Request["ddlrecords"].ToString());
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Convert.ToInt32(Session["Rowsperpage"].ToString());
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = 10;
            }
            int? stateid = null;
            if (Request["ddlsite"] != null)
            {
                ViewBag.ddlsite = Request["ddlsite"];
                stateid = Convert.ToInt32(Request["ddlsite"]);
            }
            else
            {
                ViewBag.ddlsite = "20";
                stateid = 20;
            }
            string username = null;
            if (Request["txtSearchUserName"] != null & Request["txtSearchUserName"] != "")
            {
                ViewBag.txtSearchUserName = Request["txtSearchUserName"];
                username = Request["txtSearchUserName"];
            }
            else
            {
                ViewBag.txtSearchUserName = null;
                username = null;
            }
            string ipaddress = null;
            if (Request["txtipaddress"] != null & Request["txtipaddress"] != "")
            {
                ViewBag.txtipaddress = Request["txtipaddress"];
                ipaddress = Request["txtipaddress"];
            }
            else
            {
                ViewBag.txtipaddress = null;
                ipaddress = null;
            }
            string role = null;
            if (Request["ddlrole"] != null & Request["ddlrole"] != "0")
            {
                ViewBag.ddlrole = Request["ddlrole"];
                role = Request["ddlrole"];
            }
            else
            {
                ViewBag.ddlrole = null;
                role = null;
            }

            List<Login_history> Login_historycollection = default(List<Login_history>);
            Login_history objloginhistory = new Login_history();
            List<Login_history> objlist = new List<Login_history>();
            if (Request["ddlmobile"] != "" && Request["ddlmobile"] != null)
            {
                ViewBag.Mobile_type = Request["ddlmobile"].ToString();
                if (Request["ddlmobile"].ToString() != "0")
                {

                    objloginhistory.Mobile_type = Request["ddlmobile"].ToString();
                }
                else
                {
                    objloginhistory.Mobile_type = null;
                }
            }
            else
            {
                objloginhistory.Mobile_type = null;
                ViewBag.Mobile_type = "0";
            }
            objloginhistory.LoginHistory_ID = null;
            objloginhistory.NoofRecords = NoofRecords;
            objloginhistory.PageNo = (Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1);
            objloginhistory.OrderBy = "DESC";
            objloginhistory.OrderByItem = "LogIn_DateTime";
            objloginhistory.BeginDate = FromDate;
            objloginhistory.EndDate = ToDate;
            objloginhistory.Project_ID = Convert.ToInt32(stateid);
            objloginhistory.Username = username;
            objloginhistory.IPAddress = ipaddress;
            objloginhistory.Login_ID = null;
            int totcount = 0;
            Login_historycollection = Login_history.Admin_List_Login_history(objloginhistory, ref totcount, role);
            ViewBag.totrec = totcount;
            return View(Login_historycollection);
        }
        public ActionResult Logininformation(int LoginHistory_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                showlogininfo(LoginHistory_ID);
                return View();
          
        }

        public void showlogininfo(int loginhistoryid)
        {
            try
            {
            clsCommonFunctions clscommon = new clsCommonFunctions();
            DataSet dslogininfo = new DataSet();
            IDataParameter[] InParmList = { new SqlParameter("@in_LoginHistory_ID", loginhistoryid) };
            var _with1 = clscommon;
            _with1.AddInParameters(InParmList);
            dslogininfo = _with1.GetDataSet("Help_dbo.st_get_loginhistory");
            if ((dslogininfo != null))
            {
                if (dslogininfo.Tables[0].Rows[0]["Username"].ToString() != "")
                {
                    ViewBag.lblusername = dslogininfo.Tables[0].Rows[0]["Username"];
                }
                else
                {
                    ViewBag.lblusername = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["Useragent"].ToString() != "")
                {
                    ViewBag.lbluseragent = dslogininfo.Tables[0].Rows[0]["Useragent"];
                }
                else
                {
                    ViewBag.lbluseragent = null;
                }

                if (dslogininfo.Tables[0].Rows[0]["IPAddress"].ToString() != "")
                {
                    ViewBag.lblipaddress = dslogininfo.Tables[0].Rows[0]["IPAddress"];
                }
                else
                {
                    ViewBag.lblipaddress = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["zip"].ToString() != "")
                {
                    ViewBag.lblzip = dslogininfo.Tables[0].Rows[0]["zip"];
                }
                else
                {
                    ViewBag.lblzip = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["state"].ToString() != "")
                {
                    ViewBag.lblstate = dslogininfo.Tables[0].Rows[0]["state"];
                }
                else
                {
                    ViewBag.lblstate = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["city"].ToString() != "")
                {
                    ViewBag.lblcity = dslogininfo.Tables[0].Rows[0]["city"];
                }
                else
                {
                    ViewBag.lblcity = null;
                }

                if (dslogininfo.Tables[0].Rows[0]["LogIn_DateTime"].ToString() != "")
                {
                    ViewBag.lbllogintime = dslogininfo.Tables[0].Rows[0]["LogIn_DateTime"];
                }
                else
                {
                    ViewBag.lbllogintime = null;
                }
                if (dslogininfo.Tables[0].Rows[0]["LogOut_DateTime"].ToString() != "")
                {
                    ViewBag.lbllogouttime = dslogininfo.Tables[0].Rows[0]["LogOut_DateTime"];
                }
                else
                {
                    ViewBag.lbllogouttime = null;
                }
            }
             }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "TechnicalareaController", "showlogininfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public ActionResult Pagenos()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string pagesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.pagesize;
                pagesize = ViewBag.pagesize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                pagesize = ViewBag.pagesize;
            }
            else
            {
                ViewBag.pagesize = "10";
                pagesize = ViewBag.pagesize;
            }
            if (Request["txtpagenumber"] != null & Request["txtpagenumber"] != "")
            {
                ViewBag.pno = Request["txtpagenumber"];
            }
            else
            {
                ViewBag.pno = null;
            }

            List<pagenumber> objcollection = default(List<pagenumber>);
            int totrec = 1;
            pagenumber objlistpagenum = new pagenumber(null, Convert.ToInt32(Request["txtpagenumber"] != null & Request["txtpagenumber"] != "" ? Request["txtpagenumber"] : null), 3, Convert.ToInt32(Convert.ToInt32(Request["page"] == null ? "1" : Request["page"])), Convert.ToInt32(pagesize), Convert.ToString("DESC"), Convert.ToString("Page_ID"));
            objcollection = pagenumber.admin_listpagenumbers(objlistpagenum, ref totrec);
            ViewBag.totrec = totrec;
            return View(objcollection);
        }
        public ActionResult Addpageno()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult Addpageno(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string outmsg = "";
            string strnotes = null;
            if (Request["txtnotes"] != null & Request["txtnotes"] != "")
            {
                strnotes = Request["txtnotes"];
            }
            else
            {
                strnotes = null;
            }

            string pagepath = null;
            if (Request["txtpagepath"] != null & Request["txtpagepath"] != "")
            {
                pagepath = Request["txtpagepath"];
            }
            else
            {
                pagepath = null;
            }

            Nullable<int> sectionid = default(Nullable<int>);
            if (Request["ddsectionDiv"] != null & Request["ddsectionDiv"] != "")
            {
                sectionid = Convert.ToInt32(Request["ddsectionDiv"]);
            }
            else
            {
                sectionid = null;
            }
            int createdby = 0;
            createdby = Convert.ToInt32(Session["UserID"]);
            pagenumber objadd = new pagenumber(null, pagepath, strnotes, Convert.ToInt32(3), createdby);
            pagenumber.inserpagenumber(objadd, ref outmsg);

            return RedirectToAction("Pagenos");
        }
        public ActionResult Editpageno(int Page_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Page_ID = Page_ID;
            FillPageDetails(Page_ID);
            return View();
        }


        private void FillPageDetails(int Page_ID)
        {
            try
            {
            Nullable<int> sectionid = default(Nullable<int>);
            pagenumber objpages = new pagenumber(sectionid, Page_ID, 3);
            List<pagenumber> objlist = pagenumber.viewpagelist(objpages);

            if (objlist.Count > 0)
            {
                if (objlist[0].Section_ID != 0)
                {
                    ViewBag.sectionid = objlist[0].Section_ID;
                }
                else
                {
                    ViewBag.sectionid = null;
                }

                if (!string.IsNullOrEmpty(objlist[0].PagePath))
                {
                    ViewBag.path = objlist[0].PagePath;
                }
                else
                {
                    ViewBag.path = null;
                }

                if (!string.IsNullOrEmpty(objlist[0].Notes))
                {
                    ViewBag.notes = objlist[0].Notes;
                }
                else
                {
                    ViewBag.notes = null;
                }
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "TechnicalareaController", "FillPageDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [HttpPost]
        public ActionResult Editpageno()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string outmsg = "";
            string strnotes = null;
            if (Request["Edit_txtnotes"] != null & Request["Edit_txtnotes"] != "")
            {
                strnotes = Request["Edit_txtnotes"];
            }
            else
            {
                strnotes = null;
            }

            string pagepath = null;
            if (Request["Edit_txtpagepath"] != null & Request["Edit_txtpagepath"] != "")
            {
                pagepath = Request["Edit_txtpagepath"];
            }
            else
            {
                pagepath = null;
            }
            int createdby = 0;
            createdby = Convert.ToInt32(Session["UserID"]);

            pagenumber objsave = new pagenumber(Convert.ToInt32(Request["hdnpageid"]), null, pagepath, strnotes, 3, 1);
            pagenumber.updatepagelist(objsave, ref outmsg);
            return RedirectToAction("Pagenos");
        }
        public ActionResult Deletepageno(int Page_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pagenumber.del_listpagenumbers(Page_ID, 3);
            return RedirectToAction("Pagenos");
        }

        public ActionResult Sitepage()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            SqlDataReader objread = default(SqlDataReader);
            clsDBWrapper objclsdbwraper = new clsDBWrapper();
            objread = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_SiteCategories");
            ViewData["txtDescription"] = "";
            IList<SelectListItem> _result1 = new List<SelectListItem>();
            while (objread.Read())
            {

                _result1.Add(new SelectListItem
                {
                    Text = objread["Category"].ToString(),
                    Value = objread["SiteCategory_ID"].ToString()
                });
            }

            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result1
            };
            return View(reg1);
        }
        [HttpPost]
        public ActionResult Sitepage(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            SitePage _SitePage = new SitePage();
            _SitePage.SiteCategory_ID = Convert.ToInt32(Request["DDLCategory"]);
            _SitePage.Subject_Title = Request["txtSubjectTitle"];
            _SitePage.Subject_Text = Request["txtSubjectText"].ToString();
            _SitePage.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);

            SitePage.Save(_SitePage);
            return RedirectToAction("Sitepages");
        }
        public ActionResult Sitepages()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            int NoofRecords = 0;

            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Convert.ToInt32(Request["ddlrecords"].ToString());
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Convert.ToInt32(Session["Rowsperpage"].ToString());
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = 10;
            }
            if (Request["txtCategory"] != null & Request["txtCategory"] != "")
            {
                ViewBag.search = Request["txtCategory"];
            }
            else
            {
                ViewBag.search = null;
            }

            SitePage _sitepage = new SitePage();
            _sitepage.NoOfRecords = NoofRecords;
            _sitepage.OrderByItem = "SiteCategory";
            _sitepage.OrderBy = "ASC";
            _sitepage.pageNo = Convert.ToInt32(Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1);
            _sitepage.Category = Convert.ToString(Request["txtCategory"] != null & Request["txtCategory"] != "" ? Request["txtCategory"] : null);
            List<SitePage> sitepageslist = new List<SitePage>();
            int totcount = 0;
            sitepageslist = SitePage.ADMIN_List_SitePages(_sitepage, ref totcount);
            ViewBag.totrec = totcount;
            return View(sitepageslist);
        }
        public JsonResult GetSitePagesCategorys(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
		new SqlParameter("@in_keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.st_Typeahead_Sitepages_Categorys");
            while (drlist.Read())
            {
                objlist.Add(drlist[0].ToString());
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSitepages(int SitePage_ID)
        {
            ViewBag.Count = 5;
            Session["CurrentUrl4"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            SqlDataReader objread = default(SqlDataReader);
            clsDBWrapper objclsdbwraper = new clsDBWrapper();
            objread = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_SiteCategories");
            ViewData["txtDescription"] = "";
            IList<SelectListItem> _result1 = new List<SelectListItem>();
            while (objread.Read())
            {

                _result1.Add(new SelectListItem
                {
                    Text = objread["Category"].ToString(),
                    Value = objread["SiteCategory_ID"].ToString()
                });
            }

            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result1
            };
            SitePage objsitepage = default(SitePage);

            objsitepage = SitePage.ADMIN_GET_SitePage(SitePage_ID);

            if (objsitepage.SiteCategory_ID != null)
            {
                ViewBag.DDLCategory = objsitepage.SiteCategory_ID;
            }
            else
            {
                ViewBag.DDLCategory = null;
            }
            if (objsitepage.Subject_Title != null)
            {
                ViewBag.txtSubjectTitle = objsitepage.Subject_Title;
            }
            else
            {
                ViewBag.txtSubjectTitle = null;
            }

            if (objsitepage.Subject_Text != null)
            {
                ViewBag.txtSubjectText = objsitepage.Subject_Text;
            }
            else
            {
                ViewBag.txtSubjectText = null;
            }
            if (ViewBag.txtSubjectText != null & ViewBag.txtSubjectText != "")
            {
                ViewData["txtSubjectText"] = System.Web.HttpUtility.HtmlDecode(ViewBag.txtSubjectText);
            }
            else
            {
                ViewData["txtSubjectText"] = string.Empty;
            }
            ViewBag.SitePage_ID = SitePage_ID;
            return View(reg1);
        }
        [HttpPost]
        public ActionResult EditSitepages()
        {

            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            SitePage _SitePage = new SitePage();
            _SitePage.SitePage_ID = Convert.ToInt16(Request["hdnsitepageid"]);
            _SitePage.SiteCategory_ID = Convert.ToInt32(Request["DDLCategory"]);
            _SitePage.Subject_Title = Request["txtSubjectTitle"];
            _SitePage.Subject_Text = Request["txtSubjectText"];
            _SitePage.Loginhistory_ID = Convert.ToString(Session["Loginhistory_ID"]);

            SitePage.Update(_SitePage);
            return RedirectToAction("Sitepages");
        }

        public ActionResult Sitevisitinghistory()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["ddlsite"] != null)
            {
                ViewBag.ddlsite = Request["ddlsite"];
            }
            else
            {
                ViewBag.ddlsite = "20";
            }
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);

            string ipaddress = null;
            if (Request["txtipaddress"] != null & Request["txtipaddress"] != "")
            {
                ipaddress = Request["txtipaddress"];
                ViewBag.ipaddress = Request["txtipaddress"];
            }
            else
            {
                ipaddress = null;
                ViewBag.ipaddress = null;
            }
            return View();
        }

        public ActionResult SitevisitinghistoryGrid()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }
            //ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            //ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            //ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
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

            string ipaddress = null;
            if (Request["txtipaddress"] != null & Request["txtipaddress"] != "")
            {
                ipaddress = Request["txtipaddress"];
                ViewBag.ipaddress = Request["txtipaddress"];
            }
            else
            {
                ipaddress = null;
                ViewBag.ipaddress = null;
            }
            DataSet dsSiteHistory = new DataSet();
            int totalrec = 0;

            dsSiteHistory = Login_history.Admin_List_Site_history(ipaddress, null, null, null, null, "CreatedOn", "DESC", Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), Convert.ToInt32(pgesize), FromDate,
            ToDate, ref totalrec);
            ViewBag.totrec = totalrec;
            dynamic modelList = new List<Login_history>();
            for (int i = 0; i <= dsSiteHistory.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new Login_history();
                model.Sino = dsSiteHistory.Tables[0].Rows[i]["R_NO"].ToString();
                model.State = dsSiteHistory.Tables[0].Rows[i]["State"].ToString();
                model.City = dsSiteHistory.Tables[0].Rows[i]["City"].ToString();
                model.Country = dsSiteHistory.Tables[0].Rows[i]["Country"].ToString();
                model.Latitude = dsSiteHistory.Tables[0].Rows[i]["Latitude"].ToString();
                model.Longitude = dsSiteHistory.Tables[0].Rows[i]["Longitude"].ToString();
                model.IP_Address = dsSiteHistory.Tables[0].Rows[i]["IP_Address"].ToString();
                model.UserAgent = dsSiteHistory.Tables[0].Rows[i]["UserAgent"].ToString();
                model.CreatedOn = dsSiteHistory.Tables[0].Rows[i]["CreatedOn"].ToString();
                model.Siteurl = dsSiteHistory.Tables[0].Rows[i]["Siteurl"].ToString();

                modelList.Add(model);
            }
            return View(modelList);
        }
        public ActionResult MobileSitevisitinghistory()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }
            //ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            //ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            //ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
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

            //string ipaddress = null;
            //if (Request["txtipaddress"] != null & Request["txtipaddress"] != "")
            //{
            //    ipaddress = Request["txtipaddress"];
            //    ViewBag.ipaddress = Request["txtipaddress"];
            //}
            //else
            //{
            //    ipaddress = null;
            //    ViewBag.ipaddress = null;
            //}
            DataSet dsSiteHistory_list = new DataSet();
           // int totalrec = 9;
            string sortingfield = (Request["sort"] ?? null);
            string sortdir = (Request["sortdir"] ?? null);

            dsSiteHistory_list = Login_history.Admin_List_Site_history_Mobile(FromDate, ToDate, sortdir, sortingfield);
            //ViewBag.totrec = totalrec;
            dynamic modelList = new List<Login_history>();
            for (int i = 0; i <= dsSiteHistory_list.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new Login_history();
                model.Mobileowner_id = dsSiteHistory_list.Tables[0].Rows[i]["Mobileowner_id"].ToString();
                model.deviceownername = dsSiteHistory_list.Tables[0].Rows[i]["deviceownername"].ToString();
                model.deviceaddress = dsSiteHistory_list.Tables[0].Rows[i]["deviceaddress"].ToString();
                model.devicePhone = dsSiteHistory_list.Tables[0].Rows[i]["devicePhone"].ToString();
                model.CreatedOn = dsSiteHistory_list.Tables[0].Rows[i]["CreatedOn"].ToString();
                model.Mobile_type = dsSiteHistory_list.Tables[0].Rows[i]["Mobile_type"].ToString();

                modelList.Add(model);
            }
            return View(modelList);

        }
        public ActionResult MobileSitevisitinghistoryView(int Mobileowner_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            DataSet dsSiteHistory_mobile_view = new DataSet();

            dsSiteHistory_mobile_view = Login_history.Admin_Get_Site_history_Mobile(Mobileowner_id);
            dynamic modelList = new List<Login_history>();
            dynamic model = new Login_history();
            //ViewBag.Mobileowner_id = dsSiteHistory_mobile_view.Tables[0].Rows[0]["Mobileowner_id"].ToString();
            //ViewBag.deviceownername = dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceownername"].ToString();
            //ViewBag.SimOperatorName = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimOperatorName"].ToString();
            //ViewBag.CreatedOn = dsSiteHistory_mobile_view.Tables[0].Rows[0]["CreatedOn"].ToString();

            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["AllCellInfo"].ToString() != null)
            {
                ViewBag.AllCellInfo = dsSiteHistory_mobile_view.Tables[0].Rows[0]["AllCellInfo"].ToString();
            }
            else
            {
                ViewBag.AllCellInfo = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["CallState"].ToString() != null)
            {
                ViewBag.CallState = dsSiteHistory_mobile_view.Tables[0].Rows[0]["CallState"].ToString();
            }
            else
            {
                ViewBag.CallState = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["CellLocation"].ToString() != null)
            {
                ViewBag.CellLocation = dsSiteHistory_mobile_view.Tables[0].Rows[0]["CellLocation"].ToString();
            }
            else
            {
                ViewBag.CellLocation = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["DataActivity"].ToString() != null)
            {
                ViewBag.DataActivity = dsSiteHistory_mobile_view.Tables[0].Rows[0]["DataActivity"].ToString();
            }
            else
            {
                ViewBag.DataActivity = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["DataState"].ToString() != null)
            {
                ViewBag.DataState = dsSiteHistory_mobile_view.Tables[0].Rows[0]["DataState"].ToString();
            }
            else
            {
                ViewBag.DataState = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["DeviceId"].ToString() != null)
            {
                ViewBag.DeviceId = dsSiteHistory_mobile_view.Tables[0].Rows[0]["DeviceId"].ToString();
            }
            else
            {
                ViewBag.DeviceId = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["DeviceSoftwareVersion"].ToString() != null)
            {
                ViewBag.DeviceSoftwareVersion = dsSiteHistory_mobile_view.Tables[0].Rows[0]["DeviceSoftwareVersion"].ToString();
            }
            else
            {
                ViewBag.DeviceSoftwareVersion = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["GroupIdLevel1"].ToString() != null)
            {
                ViewBag.GroupIdLevel1 = dsSiteHistory_mobile_view.Tables[0].Rows[0]["GroupIdLevel1"].ToString();
            }
            else
            {
                ViewBag.GroupIdLevel1 = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["Handle"].ToString() != null)
            {
                ViewBag.Handle = dsSiteHistory_mobile_view.Tables[0].Rows[0]["Handle"].ToString();
            }
            else
            {
                ViewBag.Handle = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["HasIccCard"].ToString() != null)
            {
                ViewBag.HasIccCard = dsSiteHistory_mobile_view.Tables[0].Rows[0]["HasIccCard"].ToString();
            }
            else
            {
                ViewBag.HasIccCard = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["IsNetworkRoaming"].ToString() != null)
            {
                ViewBag.IsNetworkRoaming = dsSiteHistory_mobile_view.Tables[0].Rows[0]["IsNetworkRoaming"].ToString();
            }
            else
            {
                ViewBag.IsNetworkRoaming = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["NeighboringCellInfo"].ToString() != null)
            {
                ViewBag.NeighboringCellInfo = dsSiteHistory_mobile_view.Tables[0].Rows[0]["NeighboringCellInfo"].ToString();
            }
            else
            {
                ViewBag.NeighboringCellInfo = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkCountryIso"].ToString() != null)
            {
                ViewBag.NetworkCountryIso = dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkCountryIso"].ToString();
            }
            else
            {
                ViewBag.NetworkCountryIso = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkOperator"].ToString() != null)
            {
                ViewBag.NetworkOperator = dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkOperator"].ToString();
            }
            else
            {
                ViewBag.NetworkOperator = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkOperatorName"].ToString() != null)
            {
                ViewBag.NetworkOperatorName = dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkOperatorName"].ToString();
            }
            else
            {
                ViewBag.NetworkOperatorName = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkType"].ToString() != null)
            {
                ViewBag.NetworkType = dsSiteHistory_mobile_view.Tables[0].Rows[0]["NetworkType"].ToString();
            }
            else
            {
                ViewBag.NetworkType = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["PhoneType"].ToString() != null)
            {
                ViewBag.PhoneType = dsSiteHistory_mobile_view.Tables[0].Rows[0]["PhoneType"].ToString();
            }
            else
            {
                ViewBag.PhoneType = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimCountryIso"].ToString() != null)
            {
                ViewBag.SimCountryIso = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimCountryIso"].ToString();
            }
            else
            {
                ViewBag.SimCountryIso = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimOperator"].ToString() != null)
            {
                ViewBag.SimOperator = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimOperator"].ToString();
            }
            else
            {
                ViewBag.SimOperator = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimOperatorName"].ToString() != null)
            {
                ViewBag.SimOperatorName = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimOperatorName"].ToString();
            }
            else
            {
                ViewBag.SimOperatorName = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimSerialNumber"].ToString() != null)
            {
                ViewBag.SimSerialNumber = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimSerialNumber"].ToString();
            }
            else
            {
                ViewBag.SimSerialNumber = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimState"].ToString() != null)
            {
                ViewBag.SimState = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SimState"].ToString();
            }
            else
            {
                ViewBag.SimState = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["SubscriberId"].ToString() != null)
            {
                ViewBag.SubscriberId = dsSiteHistory_mobile_view.Tables[0].Rows[0]["SubscriberId"].ToString();
            }
            else
            {
                ViewBag.SubscriberId = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["VoiceMailAlphaTag"].ToString() != null)
            {
                ViewBag.VoiceMailAlphaTag = dsSiteHistory_mobile_view.Tables[0].Rows[0]["VoiceMailAlphaTag"].ToString();
            }
            else
            {
                ViewBag.VoiceMailAlphaTag = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["VoiceMailNumber"].ToString() != null)
            {
                ViewBag.VoiceMailNumber = dsSiteHistory_mobile_view.Tables[0].Rows[0]["VoiceMailNumber"].ToString();
            }
            else
            {
                ViewBag.VoiceMailNumber = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceownername"].ToString() != null)
            {
                ViewBag.deviceownername = dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceownername"].ToString();
            }
            else
            {
                ViewBag.deviceownername = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceadderessMaxline"].ToString() != null)
            {
                ViewBag.deviceadderessMaxline = dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceadderessMaxline"].ToString();
            }
            else
            {
                ViewBag.deviceadderessMaxline = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceaddress"].ToString() != null)
            {
                ViewBag.deviceaddress = dsSiteHistory_mobile_view.Tables[0].Rows[0]["deviceaddress"].ToString();
            }
            else
            {
                ViewBag.deviceaddress = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["devicePhone"].ToString() != null)
            {
                ViewBag.devicePhone = dsSiteHistory_mobile_view.Tables[0].Rows[0]["devicePhone"].ToString();
            }
            else
            {
                ViewBag.devicePhone = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["createdon"].ToString() != null)
            {
                ViewBag.createdon = dsSiteHistory_mobile_view.Tables[0].Rows[0]["createdon"].ToString();
            }
            else
            {
                ViewBag.createdon = " - ";
            }
            if (dsSiteHistory_mobile_view.Tables[0].Rows[0]["Mobile_type"].ToString() != null)
            {
                ViewBag.Mobile_type = dsSiteHistory_mobile_view.Tables[0].Rows[0]["Mobile_type"].ToString();
            }
            else
            {
                ViewBag.Mobile_type = " - ";
            }
            return View();
        }
        public ActionResult Emaillog()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            if (Request["ddlcategory"] != null & Request["ddlcategory"] != "")
            {
                ViewBag.category = Request["ddlcategory"];
            }
            else
            {
                ViewBag.category = "0";
            }
            Emaillog category = new Emaillog();
            List<Emaillog> catlist = new List<Emaillog>();


            catlist = MowerHelper.Models.BLL.Admin.Emaillog.DDL_category(category);



            IList<SelectListItem> _result1 = new List<SelectListItem>();
            if (catlist.Count > 0)
            {

                for (int i = 0; i <= catlist.Count - 1; i++)
                {


                    _result1.Add(new SelectListItem
                    {
                        Text = catlist[i].MsgCatDescription,
                        Value = catlist[i].EmailMsgCtgid.ToString()
                    });
                }
            }

            StateCity reg1 = new StateCity();
            reg1 = new StateCity
            {
                StateList = _result1,
            };


            return View(reg1);
        }
        public ActionResult Emailloggrid()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Emaillog emlog = new Emaillog();
            List<Emaillog> Emloglist = new List<Emaillog>();
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string FromDate = null;
            string ToDate = null;
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
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

            int totalrecords = 0;
            emlog.SearchBy = null;
            if (Request["ddlcategory"] != null & Request["ddlcategory"] != "")
            {
                emlog.CategoryiId = Convert.ToInt32(Request["ddlcategory"]);
            }
            else
            {
                emlog.CategoryiId = null;
            }
            emlog.StartDate = FromDate;
            emlog.EndDate = ToDate;
            if (Request["sort"] != null && Request["sort"] != "")
            {
                emlog.OrderBYItem = Request["sort"].ToString();
            }
            else
            {
                emlog.OrderBYItem = null;
            }
            if (Request["sortdir"] != "" && Request["sortdir"] != null)
            {
                emlog.OrderBy = Request["sortdir"].ToString();
            }
            else
            {
                emlog.OrderBy = "ASC";
            }
            string pgesize = null;
           
            if (Request["ddlrecords"] != null)
            {
                ViewBag.gridsize = Request["ddlrecords"].ToString();
                Session["Rowsperpage"] = ViewBag.gridsize;
                pgesize = ViewBag.gridsize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.gridsize = Session["Rowsperpage"].ToString();
                pgesize = ViewBag.gridsize;
            }
            else
            {
                ViewBag.gridsize = "10";
                pgesize = ViewBag.gridsize;
            }
            TempData["pgesize"] = pgesize;
            emlog.NoOfRecords = Convert.ToInt32(pgesize);

            if (Request["page"] != null && Request["page"] != "")
            {
                emlog.PageNo = Convert.ToInt32(Request["page"]);
            }
            else
            {
                emlog.PageNo = 1;
            }
            Emloglist = emlog.List_Emaillog(emlog, ref totalrecords);
            ViewBag.totrec = totalrecords;
            return View(Emloglist);
        }
    }
}
