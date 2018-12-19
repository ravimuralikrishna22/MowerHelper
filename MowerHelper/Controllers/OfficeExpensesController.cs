using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using aspPDF;
using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using MowerHelper.App_Start;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.Classes;


namespace MowerHelper.Controllers
{
    public class OfficeExpensesController : Controller
    {
        string taxded;
        EASYPDF pdf = new EASYPDF();
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ExpenseLedger()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
           // string pgesize = null;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("../Schedule/Schedulespecs");
            }
               //Session.Add("TopIndex", "4");
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInElectricianLogin").ToUpper() == "YES")
            {
                ViewBag.Showforums = "Y";
            }
            else
            {
                ViewBag.Showforums = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            {
                ViewBag.Showtask = "Y";
            }
            else
            {
                ViewBag.Showtask = "N";
            }
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");

            List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
            ExpenseLedgerCollection obj = new ExpenseLedgerCollection();
          //  string pgesize = null;

            //if (Request["ddlrecords"] != null && Request["ddlrecords"] != "undefined")
            //{
            //    ViewBag.expsize = Request["ddlrecords"].ToString();
            //    Session["Rowsperpage"] = ViewBag.expsize;
            //    pgesize = ViewBag.expsize;
            //}
            //else if (Session["Rowsperpage"] != null && Request["Rowsperpage"] != "undefined")
            //{
            //    ViewBag.expsize = Session["Rowsperpage"].ToString();
            //    pgesize = ViewBag.expsize;
            //}
            //else
            //{
            //    ViewBag.expsize = "10";
            //    pgesize = ViewBag.expsize;
            //}
           
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            ViewBag.cat = Request["txtCategory"];
            if (Request["sortdir"] != null & Request["sortdir"]!="")
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
            ViewBag.Fromdate1 = FromDate;
            ViewBag.Todate1 = ToDate;
            ViewBag.roleid = Session["RoleID"];
            //if (Convert.ToInt32(Session["RoleID"]) == 1)
            //{
            //    int practiceid1;
            //    if (Request["ComboBox6"] != "" && Request["ComboBox6"] != null)
            //    {
            //        //string[] combo = Request["ComboBox6"].ToString().Split('$');
            //        //practiceid1 = Convert.ToInt32(combo[1]);
            //        if (!string.IsNullOrEmpty(Request["ComboBox6"]))
            //        {
            //            practiceid1 = Convert.ToInt32(Request["ComboBox6"]);
            //            TempData["combotext"] = Convert.ToString(Request["ComboBox6_SelectedText"]);
            //        }
            //        ViewBag.combo = Request["ComboBox6"];
            //        Session["Prov_ID"] = Request["ComboBox6"];
            //    }
            //    else
            //    {
            //        practiceid1 = 0;
            //        TempData["combotext"] = "";
            //        Session["Prov_ID"] = null;
            //    }
            //    objList = obj.GetExpLedgerDataList(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(pgesize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), (Request["sortdir"] == null ? "ASC" : Request["sortdir"]), (Request["sort"] == null ? null : Request["sort"]), FromDate, ToDate, Request["txtCategory"]);
            //}
            //else
            //{

            //    objList = obj.GetExpLedgerDataList(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(pgesize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), (Request["sortdir"] == null ? "ASC" : Request["sortdir"]), (Request["sort"] == null ? null : Request["sort"]), FromDate, ToDate, Request["txtCategory"]);
            //}
            ViewBag.totrec = ExpenseLedgerList.NoofRecords;
            return View(objList);
        }


        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ExpensesPartial()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.Count = 2;
                Session["CurrentUrl1"] = Request.RawUrl;
                string pgesize = null;            
                string startdate;
                startdate = DateTime.Now.ToString("MM/dd/yyyy");
                List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
                ExpenseLedgerCollection obj = new ExpenseLedgerCollection();
                if (Request["ddlrecords"] != null && Request["ddlrecords"] != "undefined")
                {
                    ViewBag.expsize = Request["ddlrecords"].ToString();
                    Session["Rowsperpage"] = ViewBag.expsize;
                    pgesize = ViewBag.expsize;
                }
                else if (Session["Rowsperpage"] != null && Request["Rowsperpage"] != "undefined")
                {
                    ViewBag.expsize = Session["Rowsperpage"].ToString();
                    pgesize = ViewBag.expsize;
                }
                else
                {
                    ViewBag.expsize = "10";
                    pgesize = ViewBag.expsize;
                }

                ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
                ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
                ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
                ViewBag.cat = Request["txtCategory"];
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
                ViewBag.Fromdate1 = FromDate;
                ViewBag.Todate1 = ToDate;
                ViewBag.roleid = Session["RoleID"];
                if (Convert.ToInt32(Session["RoleID"]) == 1)
                {
                    int practiceid1;
                    if (Request["ComboBox6"] != "" && Request["ComboBox6"] != null)
                    {
                        //string[] combo = Request["ComboBox6"].ToString().Split('$');
                        //practiceid1 = Convert.ToInt32(combo[1]);
                        if (!string.IsNullOrEmpty(Request["ComboBox6"]))
                        {
                            practiceid1 = Convert.ToInt32(Request["ComboBox6"]);
                            TempData["combotext"] = Convert.ToString(Request["ComboBox6_SelectedText"]);
                        }
                        ViewBag.combo = Request["ComboBox6"];
                        Session["Prov_ID"] = Request["ComboBox6"];
                    }
                    else
                    {
                        practiceid1 = 0;
                        TempData["combotext"] = "";
                        Session["Prov_ID"] = null;
                    }
                    objList = obj.GetExpLedgerDataList(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(pgesize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), (Request["sortdir"] == null ? "DESC" : Request["sortdir"]), (Request["sort"] == null ? "Exp_Date" : Request["sort"]), FromDate, ToDate, Request["txtCategory"]);
                }
                else
                {

                    objList = obj.GetExpLedgerDataList(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(pgesize), Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]), (Request["sortdir"] == null ? "DESC" : Request["sortdir"]), (Request["sort"] == null ? "Exp_Date" : Request["sort"]), FromDate, ToDate, Request["txtCategory"]);
                }

                ViewBag.objList = objList.Count;
                ViewBag.PageNo = Request["page"] == null ? "1" : Request["page"];
                ViewBag.Sortdir = Request["sortdir"] == null ? "DESC" : Request["sortdir"];
                ViewBag.sort = Request["sort"] == null ? "Exp_Date" : Request["sort"];
                ViewBag.totrec = ExpenseLedgerList.NoofRecords;
                return PartialView("ExpensesPartial", objList);
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

        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddExpenseLedger(string Exp_Ledger_ID, string roleid, string obj1, int? page, string txtCategory, string dt_filter, string txt_FromDate, string txt_ToDate, int? ddlrecords,string sortdir,string sort)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.roleid = roleid;

                //var description = new Referrals { FieldIDString = "22" };
                var ds = Referrals.List_field_description("22");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.fileupload = Convert.ToString(ds.Tables[0].Rows[0][3]);

                }
                ExpenseLedgerCollection obj = new ExpenseLedgerCollection();
                //List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
                //objList = obj.GetAllCatagories();
                ViewBag.catList = obj.GetAllCatagories();

                ViewBag.page = page;
                ViewBag.txtCategory = txtCategory;
                ViewBag.dt_filter = dt_filter;
                ViewBag.txt_FromDate = txt_FromDate;
                ViewBag.txt_ToDate = txt_ToDate;
                ViewBag.ddlrecords = ddlrecords;
                ViewBag.sortdir = sortdir;
                ViewBag.sort = sort;

                //IList<SelectListItem> _result1 = new List<SelectListItem>();
                //if (objList.Count > 0)
                //{

                //    for (int i = 0; i <= objList.Count - 1; i++)
                //    {
                //        _result1.Add(new SelectListItem
                //        {
                //            Text = objList[i].Exp_Category,
                //            Value = Convert.ToString(objList[i].Exp_Category_ID)
                //        });
                //    }
                //}
                //StateCity reg1 = new StateCity();
                //reg1 = new StateCity
                //{
                //    StateList = _result1
                //};

                ViewBag.Exp_Date = DateTime.Now.ToString("MM/dd/yyyy");
                ViewBag.Exp_Category_ID = null;
                ViewBag.Item = "";
                ViewBag.Quantity = null;
                ViewBag.TotalCost = null;
                ViewBag.CheckNumber = null;
                ViewBag.Vendor = null;
                ViewBag.Deductible = "2";
                ViewBag.Notes = null;

                //return View("AddExpenseLedger", reg1);

                return View("AddExpenseLedger");
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
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddExpenseLedger(string id, string id1, HttpPostedFileBase flLogo1)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                if (flLogo1 != null)
                {
                    if (flLogo1.ContentLength > 0)
                    {
                        Getfilename(flLogo1);
                    }
                }
                //else
                //{

                //    if (!string.IsNullOrEmpty(Request["ExistedLogo"]))
                //    {
                //        ViewData["LogoPath"] = Request["strimage"];
                //    }
                //    else
                //    {
                //        ViewData["LogoPath"] = null;
                //    }
                //}
                ExpenseLedgerCollection objInsert = new ExpenseLedgerCollection();
                string strdate;
                int CategoryId;
                string Item;
                string Quantity;
                string strcost;
                string strchknum;
                string strvendor;
                string strnotes;
                //  int int_LocationID = 0;
                // string[] propraid = null;
                int proid;
                //  int practid;
                if (Request["ComboBox5"] != "" && Request["ComboBox5"] != null)
                {
                    //propraid = Request["ComboBox5"].ToString().Split('$');
                    proid = Convert.ToInt32(Request["ComboBox5"]);
                    //practid = Convert.ToInt32(propraid[1]);
                }
                else
                {
                    proid = 0;
                    // practid = 0;
                }
                if (Request["txtDate"] != null)
                {
                    strdate = Request["txtDate"];
                }
                else
                {
                    strdate = null;
                }
                if (Request["ddlCategory"] != null)
                {
                    CategoryId = Convert.ToInt32(Request["ddlCategory"]);
                }
                else
                {
                    CategoryId = 0;
                }
                if (Request["txtItem"] != null)
                {
                    Item = Request["txtItem"];
                }
                else
                {
                    Item = null;
                }
                if (Request["txtQuantity"] != null)
                {
                    Quantity = Request["txtQuantity"];
                }
                else
                {
                    Quantity = null;
                }
                if (Request["txtCost"] != null)
                {
                    strcost = Request["txtCost"];
                }
                else
                {
                    strcost = null;
                }
                if (Request["txtCheckNumber"] != null)
                {
                    strchknum = Request["txtCheckNumber"];
                }
                else
                {
                    strchknum = null;
                }
                if (Request["txtVendor"] != null)
                {
                    strvendor = Request["txtVendor"];
                }
                else
                {
                    strvendor = null;
                }
                if (Request["txtNotes"] != null)
                {
                    strnotes = Sanitizer.GetSafeHtmlFragment(Request["txtNotes"]);
                }
                else
                {
                    strnotes = null;
                }
                //(Request["rbtnDeductible0"] != null & Request["rbtnDeductible0"] == "0")
                if (Request["rbtnDeductible0"] != "false")
                {
                    taxded = "0";
                }
                else if (Request["rbtnDeductible1"] != "false")
                {
                    taxded = "1";
                }
                else if (Request["rbtnDeductible2"] != "false")
                {
                    taxded = "2";
                }
                int res;
                string Imagepath = ViewData["LogoPath"] == null ? null : ViewData["LogoPath"].ToString();
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    res = objInsert.Insert_Expense_Ledger(proid, CategoryId, strdate, Item, Quantity, strcost, strchknum, strvendor,
                   taxded, strnotes, Convert.ToInt32(Session["userID"]), Convert.ToInt32(Session["RoleID"]), Imagepath);

                }
                else
                {
                    res = objInsert.Insert_Expense_Ledger(Convert.ToInt32(Session["Prov_ID"]), CategoryId, strdate, Item, Quantity, strcost, strchknum, strvendor,
                    taxded, strnotes, Convert.ToInt32(Session["userID"]), Convert.ToInt32(Session["RoleID"]), Imagepath);

                }
                objInsert.page = Convert.ToInt32(Request["hdpage"]);
                objInsert.txtCategory = Request["hdnctgry"];
                objInsert.dt_filter = Request["hdnfilter"];
                objInsert.txt_FromDate = Request["hdntxtfromdate"];
                objInsert.txt_ToDate = Request["hdntxttodate"];
                objInsert.ddlrecords = Convert.ToInt32(Request["hdnrecords"]);
                objInsert.sortdir = Request["hdnsortdr"];
                objInsert.sort = Request["hdnsr"];

                //return RedirectToAction("ExpenseLedger");
                return Json(JsonResponseFactory.SuccessResponse(objInsert), JsonRequestBehavior.AllowGet);
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
        
        private bool Getfilename(HttpPostedFileBase flLogo)
        {
            try
            {
            string FileExtn1 = null;
            string strFilePath = null;
            FileExtn1 = System.IO.Path.GetExtension(flLogo.FileName);
            ViewData["EncryptLogo"] = clsCommonCClist.RandomPassword();
            string filename = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
            strFilePath = Path.Combine(Server.MapPath("~/Attachments/Expenses"), filename);
            ViewData["LogoPath"] = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
            flLogo.SaveAs(strFilePath);
            return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "OfficeExpensesController", "Getfilename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return false;
            }
        }
        //private string RandomPassword()
        //{
        //    return Convert.ToString(Guid.NewGuid());

        //}

        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditExpenseLedger(string Exp_Ledger_ID, int? page, string txtCategory, string dt_filter, string txt_FromDate, string txt_ToDate, int? ddlrecords, string sortdir,string sort, string RecPerPge = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //var description = new Referrals { FieldIDString = "22" };
                var ds = Referrals.List_field_description("22");
                ViewBag.RecPerPge = RecPerPge;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.fileupload1 = Convert.ToString(ds.Tables[0].Rows[0][3]);

                }
                ExpenseLedgerCollection obj = new ExpenseLedgerCollection();

                ViewBag.page = page;
                ViewBag.txtCategory = txtCategory;
                ViewBag.dt_filter = dt_filter;
                ViewBag.txt_FromDate = txt_FromDate;
                ViewBag.txt_ToDate = txt_ToDate;
                ViewBag.ddlrecords = ddlrecords;
                ViewBag.sortdir = sortdir;
                ViewBag.sort = sort;
                //List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
                //objList = obj.GetAllCatagories(); 
                ViewBag.Categorylist = obj.GetAllCatagories();                
                //IList<SelectListItem> _result1 = new List<SelectListItem>();
                //if (objList.Count > 0)
                //{

                //    for (int i = 0; i <= objList.Count - 1; i++)
                //    {
                //        _result1.Add(new SelectListItem
                //        {
                //            Text = objList[i].Exp_Category,
                //            Value = Convert.ToString(objList[i].Exp_Category_ID)
                //        });
                //    }
                //}
                //StateCity reg1 = new StateCity();
                //reg1 = new StateCity
                //{
                //    StateList = _result1
                //};

                ////obj.GetCategorylist.StateList = _result1;
                ViewBag.Exp_Ledger_ID = Exp_Ledger_ID;
                fillExpenseLedgerInfo(Convert.ToInt32(Exp_Ledger_ID));

                //return View("EditExpenseLedger", reg1);
                return View("EditExpenseLedger");
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
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditExpenseLedger(string id, string id1, HttpPostedFileBase flLogo)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                if (flLogo != null)
                {
                    if (flLogo.ContentLength > 0)
                    {
                        Getfilename(flLogo);
                        if (!string.IsNullOrEmpty(Request["strimage"]))
                        {
                            string filepath = Path.Combine(Server.MapPath("~/Attachments/Expenses"), Request["strimage"]);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(Request["ExistedLogo"]))
                    {
                        ViewData["LogoPath"] = Request["strimage"];
                    }
                    else
                    {
                        ViewData["LogoPath"] = null;
                        if (!string.IsNullOrEmpty(Request["strimage"]))
                        {
                            string filepath = Path.Combine(Server.MapPath("~/Attachments/Expenses"), Request["strimage"]);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                    }
                }
                ExpenseLedgerCollection objInsert = new ExpenseLedgerCollection();
                string strdate;
                int CategoryId;
                string Item;
                string Quantity;
                string strcost;
                string strchknum;
                string strvendor;
                string strnotes;
                int int_LocationID = 0;
                if (Request["Edit_txtDate"] != null)
                {
                    strdate = Request["Edit_txtDate"];
                }
                else
                {
                    strdate = null;
                }
                if (Request["Edit_ddlCategory"] != null)
                {
                    CategoryId = Convert.ToInt32(Request["Edit_ddlCategory"]);
                }
                else
                {
                    CategoryId = 0;
                }
                if (Request["Edit_txtItem"] != null)
                {
                    Item = Request["Edit_txtItem"];
                }
                else
                {
                    Item = null;
                }
                if (Request["Edit_txtQuantity"] != null)
                {
                    Quantity = Request["Edit_txtQuantity"];
                }
                else
                {
                    Quantity = null;
                }
                if (Request["Edit_txtCost"] != null)
                {
                    strcost = Request["Edit_txtCost"];
                }
                else
                {
                    strcost = null;
                }
                if (Request["Edit_txtCheckNumber"] != null)
                {
                    strchknum = Request["Edit_txtCheckNumber"];
                }
                else
                {
                    strchknum = null;
                }
                if (Request["Edit_txtVendor"] != null)
                {
                    strvendor = Request["Edit_txtVendor"];
                }
                else
                {
                    strvendor = null;
                }
                if (Sanitizer.GetSafeHtmlFragment(Request["Edit_txtNotes"]) != null)
                {
                    strnotes = Sanitizer.GetSafeHtmlFragment(Request["Edit_txtNotes"]);
                }
                else
                {
                    strnotes = null;
                }
                if (Request["Edit_rbtnDeductible0"] != "false")
                {
                    taxded = "0";
                }
                else if (Request["Edit_rbtnDeductible1"] != "false")
                {
                    taxded = "1";
                }
                else if (Request["Edit_rbtnDeductible2"] != "false")
                {
                    taxded = "2";
                }
                int providerid;
                // int praid; (Request["Edit_rbtnDeductible0"] != null & Request["Edit_rbtnDeductible0"] == "0")
                if (Convert.ToInt32(Session["RoleID"]) == 1)
                {
                    providerid = Convert.ToInt32(Request["hdnprovid"]);
                    //praid = Convert.ToInt32(Request["hdnpraid"]);
                }
                else
                {
                    providerid = Convert.ToInt32(Session["Prov_ID"]);
                    //praid = Convert.ToInt32(Session["Practice_ID"]);
                }

                ExpenseLedgerCollection objUPT = new ExpenseLedgerCollection();
                int res = objUPT.UPT_ExpenseLedger(Convert.ToInt32(Request["Edit_HdnExp_Ledger_ID"]), providerid, int_LocationID, CategoryId, strdate, Item, Quantity, strcost, strchknum, strvendor,
                 taxded, strnotes, Convert.ToString(Session["userID"]), Convert.ToInt32(Session["RoleID"]), (ViewData["LogoPath"] == null ? null : Convert.ToString(ViewData["LogoPath"])));
                objUPT.page = Convert.ToInt32(Request["hdnpage"]);
                objUPT.txtCategory = Request["hdncategory"];
                objUPT.dt_filter = Request["hdndtfilter"];
                objUPT.txt_FromDate = Request["hdnfromdate"];
                objUPT.txt_ToDate = Request["hdntodate"];
                objUPT.ddlrecords = Convert.ToInt32(Request["hdnddlrecords"]);
                objUPT.sortdir = Request["hdnsortdr"];
                objUPT.sort = Request["hdnsrdr"];

                //return RedirectToAction("ExpenseLedger");
                return Json(JsonResponseFactory.SuccessResponse(objUPT), JsonRequestBehavior.AllowGet);
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

        public void FillCatagory()
        {
            try
            {
            ExpenseLedgerCollection obj = new ExpenseLedgerCollection();
            List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
            objList = obj.GetAllCatagories();
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "OfficeExpensesController", "FillCatagory", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                
            }
        }
        
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ViewImage(string Imagepath, string TotalCost)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewBag.TotalCost = TotalCost;
                //clsWebConfigsettings clsWebConfigsettings = new clsWebConfigsettings();
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
                    domain = clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal");
                }
                if (Imagepath != null && Imagepath != "")
                {
                    string str = Imagepath;
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
                    string strings = Path.Combine(Server.MapPath("~/Attachments/Expenses"), str);
                    if (System.IO.File.Exists(strings))
                    {

                        ViewBag.imageavail = "Y";
                        ViewBag.image = "Y";
                        ViewBag.providerimage = domain + "Attachments/Expenses/" + str;

                    }
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
        public ActionResult NewCategory()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
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
        
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult NewCategory(ExpenseLedgerCollection obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string str = Request["txtcategory"];
                string out_Msg = null;
                ExpenseLedgerCollection obj1 = new ExpenseLedgerCollection();
                int role;
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    role = Convert.ToInt32(Session["RoleID"]);
                }
                else
                {
                    role = 11;
                }
                out_Msg = Convert.ToString(obj1.InsertExpense_Ledger(role, str, ref out_Msg));
                if (out_Msg != null)
                {
                    if (out_Msg.Contains("Category is exists."))
                    {
                        return Json(JsonResponseFactory.ErrorResponse(out_Msg), JsonRequestBehavior.DenyGet);
                    }
                }
                return Json(JsonResponseFactory.SuccessResponse(out_Msg), JsonRequestBehavior.DenyGet);
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
    
        private void fillExpenseLedgerInfo(int Exp_Ledger_ID)
       {       
        ExpenseLedgerCollection objExpenses = new ExpenseLedgerCollection();
        ExpenseLedgerList objData = new ExpenseLedgerList();
        List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
        List<ExpenseLedgerList> objFacilities = new List<ExpenseLedgerList>();
        List<ExpenseLedgerList> objProviders = new List<ExpenseLedgerList>();
        try
        {
            objList = objExpenses.GetExpenseLedgerDetailsBasedOnID(Exp_Ledger_ID);

            if (objList.Count > 0)
            {
                objData = objList[0];

                if ((objData.Exp_Date)!=null)
                 {
                    ViewBag.Exp_Date = objData.Exp_Date.ToString("MM/dd/yyyy");
                 }
                else
                 {
                     ViewBag.Exp_Date = null;
                 }
                if ((objData.providername) != null)
                {
                    ViewBag.PracticeName = objData.providername.ToString();
                }
                else
                {
                    ViewBag.PracticeName = null;
                }
                if (objData.Practice_ID != null)
                {
                    ViewBag.Practice_ID = objData.Practice_ID;
                }
                else
                {
                    ViewBag.Practice_ID = null;
                }
                if (objData.Provider_ID != 0)
                {
                    ViewBag.providerid = objData.Provider_ID;
                }
                else
                {
                    ViewBag.PracticeName = null;
                }
                ViewBag.roleid = Session["RoleID"];
                ViewBag.Exp_Category_ID = objData.Exp_Category_ID;
            
                if (!string.IsNullOrEmpty(objData.Item))
                {
                    ViewBag.Item = objData.Item;
                }
                else
                {
                    ViewBag.Item = null;
                }

                if (objData.Quantity != "0")
                {
                    ViewBag.Quantity = Convert.ToString(objData.Quantity);
                }
                else
                {
                    ViewBag.Quantity = null;
                }
                if (!string.IsNullOrEmpty(objData.TotalCost))
                {
                    ViewBag.TotalCost = objData.TotalCost;
                }
                else
                {
                    ViewBag.TotalCost = null;
                }
                if (!string.IsNullOrEmpty(objData.CheckNumber))
                {
                    ViewBag.CheckNumber = objData.CheckNumber;
                }
                else
                {
                    ViewBag.CheckNumber = null;
                }

                if (!string.IsNullOrEmpty(objData.Vendor))
                {
                    ViewBag.Vendor = objData.Vendor;
                }
                else
                {
                    ViewBag.Vendor = null;
                }
                if (!string.IsNullOrEmpty(objData.Deductible))
                {
                    ViewBag.Deductible = objData.Deductible;
                }
                else
                {
                    ViewBag.Deductible = null;
                }
                if (!string.IsNullOrEmpty(objData.Notes))
                {
                    ViewBag.Notes = objData.Notes;
                }
                else
                {
                    ViewBag.Notes = null;
                }

                //clsWebConfigsettings objconfig = new clsWebConfigsettings();
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
                    domain = clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal");
                }
                if (objData.Imagepath != null && objData.Imagepath != "")
                {
                    string str = objData.Imagepath.ToString();
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
                    string strings = Path.Combine(Server.MapPath("~/Attachments/Expenses"), str);
                    if (System.IO.File.Exists(strings))
                    {

                        ViewBag.imageavail = "Y";
                        ViewBag.image = "Y";
                        ViewBag.providerimage = domain + "Attachments/Expenses/" + str;

                    }


                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog clsex = new clsExceptionLog();
            clsex.LogException(ex, "OfficeExpensesController", "fillExpenseLedgerInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        }
    }
    
        public void DelBoard(int Exp_Ledger_ID)
        {
        try
        {
        ExpenseLedgerCollection objData = new ExpenseLedgerCollection();

        objData.DeleteExpense_Ledger(Exp_Ledger_ID);
        }
        catch (Exception ex)
        {
            clsExceptionLog clsex = new clsExceptionLog();
            clsex.LogException(ex, "OfficeExpensesController", "DelBoard", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
          
        }
        }
    
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DeleteExp(int Exp_Ledger_ID, int? page, string txtCategory, string dt_filter, string txt_FromDate, string txt_ToDate, int? ddlrecords, string sortdir,string sort)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                ExpenseLedgerCollection objData = new ExpenseLedgerCollection();
                objData.page = page;
                objData.txtCategory = txtCategory;
                objData.dt_filter = dt_filter;
                objData.txt_FromDate = txt_FromDate;
                objData.txt_ToDate = txt_ToDate;
                objData.ddlrecords = ddlrecords;
                objData.sortdir = sortdir;
                objData.sort = sort;
                objData.DeleteExpense_Ledger(Exp_Ledger_ID);
                return RedirectToAction("ExpensesPartial", objData);
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

        public JsonResult ExpensesCategory(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
		new SqlParameter("@in_provider_id", (Session["Prov_ID"] == null ? null : Session["Prov_ID"])),
		new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.st_ExpensesCategory_typeahead");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString(drlist[0]));
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public FileContentResult Expenseledgerpdf(string fromdate, string todate, string category, string daterange, string noofrecords, string sortdir)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pdf.Create();
            StringBuilder strhtml = new StringBuilder();

            pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
            Getexpensepdf(fromdate, todate, noofrecords, category, sortdir, daterange);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=Officeexpenses.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            return null;
        }
        
        private void Getexpensepdf(string fromdate, string todate, string noofrecords, string category, string sortdir, string daterange)
        {
            try
            {
            List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
            ExpenseLedgerCollection obj = new ExpenseLedgerCollection();
            ExpenseLedgerCollection objExpList = new ExpenseLedgerCollection();
            if (fromdate != "")
            {
                fromdate = Convert.ToString(fromdate);
            }
            else
            {
                fromdate = null;
            }
            if (todate != "")
            {
                todate = todate.ToString();
            }
            else
            {
                todate = null;
            }
            if (category != "")
            {
                category = category.ToString();
            }
            else
            {
                category = null;
            }
            if (sortdir != "")
            {
                sortdir = sortdir.ToString();
            }
            else
            {
                sortdir = "ASC";
            }
            objList = objExpList.GetExpLedgerDataList(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(noofrecords), 1, sortdir, null, fromdate, todate, category);
           
         //   double height = 100;
            double pos3 = 300;
      
            string strhtml = null;
            strhtml="<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml=strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>My Business expenses</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (objList.Count > 0)
            {
                strhtml = strhtml +"<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='98%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td width='30px' align='center' ><label>#" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
                //strhtml = strhtml + "<td align='center' ><label>Item" + "</label></td>";
                //strhtml = strhtml + "<td align='center' ><label>Quantity" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Cost" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Check number" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Vendor" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Tax deductible" + "</b></td>";
                strhtml = strhtml + "<td align='center' ><label>Category" + "</label></td>";
                strhtml = strhtml + "<td width='80px' align='center' ><label>Notes" + "</label></td>";
                strhtml = strhtml + "</tr>";

                foreach (var item in objList)
                {
                    string date1 = Convert.ToString(item.Exp_Date);
                    string[] date = date1.Split(' ');
                    strhtml = strhtml + "<tr class='gray'>";
                    strhtml = strhtml + "<td width='30px' align='center'><b> " + item.SLNo + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + Convert.ToString(date[0]) + "</b></td>";
                    //strhtml = strhtml + "<td><b>" + item.Item + "</b></td>";
                    //strhtml = strhtml + "<td><b>" + item.Quantity + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + item.TotalCost + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + item.CheckNumber + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + item.Vendor + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + item.tax + "</b></td>";
                    strhtml = strhtml + "<td align='center'><b>" + item.Category + "</b></td>";
                    strhtml = strhtml + "<td width='80px' align='center'><b>" + item.Notes + "</b></td>";
                    strhtml = strhtml + "</tr>";

                }
                strhtml = strhtml + "</table>";
               // height = pdf.GetTextHeight(strhtml);
                pdf.AddHTMLPos(pos3, 10, strhtml);
            }
            else
            {
                strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td width='10px'><b> No Expenses Found. " + "</b></td>";
                strhtml = strhtml + "</table>";
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "OfficeExpensesController", "Getexpensepdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
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
        
        public JsonResult LoadingItems2(ComboBoxLoadingItemsEventArgs args)
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
        
        protected ComboBoxItemList GetFilteredproviders(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter;
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox5"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "OfficeExpensesController", "GetFilteredproviders", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        
        protected ComboBoxItemList GetFilteredproviders1(string filter, int offset)
        {
            try
            {
            TempData.Keep();
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = null;
            if (filter != null)
            {
                PracticeName = filter;
                TempData["combotext"] = "";
            }
            else if (Convert.ToString(TempData["combotext"]) != "")
            {
                PracticeName = Convert.ToString(TempData["combotext"]).Substring(0, 2);
            }
            else
            {
                PracticeName = null;
            }
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            ViewData["ComboBox6"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "OfficeExpensesController", "GetFilteredproviders1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
    }
}
