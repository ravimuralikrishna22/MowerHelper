using aspPDF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class ResellersController : Controller
    {
        EASYPDF pdf = new EASYPDF();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Resellerslist()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Request["txtfirstname"] != null && Request["txtfirstname"] != "")
            {
                ViewBag.FName = Request["txtfirstname"];
            }
            else
            {
                ViewBag.FName = null;
            }
            if (Request["txtLastname"] != null && Request["txtLastname"] != "")
            {
                ViewBag.LName = Request["txtLastname"];
            }
            else
            {
                ViewBag.LName = null;
            }
            string NoofRecords = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = "10";
            }


            List<CRMAgent> objlist = new List<CRMAgent>();
            CRMAgent obj = new CRMAgent();
            obj.CRM_Agent_ID = null;
            obj.FirstName = Request["txtfirstname"];
            obj.LastName = Request["txtLastname"];
            obj.PageNo = Convert.ToInt32(Request["page"] != null ? Request["page"] : "1");
            obj.NoofRecords = Convert.ToInt32(NoofRecords);
            obj.OrderBy = "ASC";
            obj.OrderByItem = "LastName";
            objlist = CRMAgent.GET_CRM_Agent_Details(obj);
            ViewBag.totrec = CRMAgent.TotalRecords;


            return View(objlist);
        }
        public ActionResult ResellerStatusChange(string CRM_Agent_ID, string Status)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            CRMAgent obj = new CRMAgent();
            obj.CRM_Agent_ID = CRM_Agent_ID;
            obj.Status_Ind = Status;
            CRMAgent.Active_InActive_CRM_Agent_Details(obj);
            //Forums.Upd_TopicStatus(Convert.ToInt32(Topic_ID), StatusID);
            return RedirectToAction("Resellerslist");
        }


        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddNewReseller()
        {
            //var reg1 = clsCommonCClist.GetStates();

                //List<clsCountry> objstates = new List<clsCountry>();
                //objstates = clsCountry.GetStatesByCountryId(1);
                //IList<SelectListItem> _result1 = new List<SelectListItem>();
                //if (objstates.Count > 0)
                //{
                //    for (int i = 0; i <= objstates.Count - 1; i++)
                //    {
                //        _result1.Add(new SelectListItem
                //        {
                //            Text = objstates[i].StateFullName,
                //            Value = Convert.ToString(objstates[i].StateId)
                //        });
                //    }
                //}
                //IList<SelectListItem> _result2 = new List<SelectListItem>();
                //_result2.Add(new SelectListItem
                //{
                //    Text = "--Select City--",
                //    Value = "0",
                //    Selected = true
                //});
                //StateCity reg1 = new StateCity();
                //reg1 = new StateCity
                //{
                //    StateList = _result1,
                //    CityList = _result2
                //};
                return View("AddNewReseller");
            
           
        }
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult AddNewReseller(string obj1)
        {
            
            CRMAgent obj = new CRMAgent();
            string HomePhone = null;
            string WorkPhone = null;
            if (string.IsNullOrEmpty(Request["Add_txt_WPhone1"]))
            {
                WorkPhone = null;
            }
            else
            {
                WorkPhone = Request["Add_txt_WPhone1"] + Request["Add_txt_WPhone2"] + Request["Add_txt_WPhone3"];
            }
            if (string.IsNullOrEmpty(Request["Add_txt_CPhone1"]))
            {
                HomePhone = null;
            }
            else
            {
                HomePhone = Request["Add_txt_CPhone1"] + Request["Add_txt_CPhone2"] + Request["Add_txt_CPhone3"];
            }
            obj.Prefix = (Request["Add_DDPrefix"] == " " ? null : Request["Add_DDPrefix"]);
            obj.FirstName = Request["Add_txtFName"];
            obj.MI = Request["Add_txtMName"];
            obj.LastName = Request["Add_txtLName"];
            obj.Suffix = (Request["Add_cbosuffix"] == " " ? null : Request["Add_cbosuffix"]);
            obj.Address1 = Request["Add_txt_Address1"];
            obj.Address2 = Request["Add_txt_Address2"];
            obj.City_ID = Convert.ToInt32(Request["DDCity"] != "0" ? Request["DDCity"] : null);
            obj.State_ID = Convert.ToInt32(Request["DDState"] != "" ? Request["DDState"] : null);
            obj.Email = Request["Add_txt_Email"];
            obj.ZIP = Request["txtZip"];
            obj.HomePhone = HomePhone;
            obj.WorkPhone = WorkPhone;
            obj.CRM_Agent_ID = null;
            //if (!string.IsNullOrEmpty(Request["Add_txt_DOB"]))
            //{
            //    obj.Expirydate = Request["Add_txt_DOB"];
            //}
            //else
            //{
            obj.Expirydate = null;
            //}
            //if (rdotherpist.SelectedValue == 0)
            //{
            //    obj.ISFTAgent = "Yes";
            //}
            //else if (rdotherpist.SelectedValue == 1)
            //{
            //    obj.ISFTAgent = "No";
            //}
            //else
            //{
            obj.ISFTAgent = null;
            //}
            obj.CompanyName = Request["Add_txt_Employer"];
            //if (string.IsNullOrEmpty(hidApprovalBody.Value))
            //{
            //    hidApprovalBody.Value = ViewState("hidApprovalBody");
            //}
            obj.IncentiveIds = null;

            obj.IsAgent = "Y";

            CRMAgent.Ins_Upd_CRM_Agent_Details(obj);

            return Json(JsonResponseFactory.SuccessResponse(obj1), JsonRequestBehavior.DenyGet);
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditReseller(string CRM_Agent_ID)
        {
          
                ViewBag.CRM_Agent_ID = CRM_Agent_ID;
            

                //List<clsCountry> objstates = new List<clsCountry>();
                //objstates = clsCountry.GetStatesByCountryId(1);
                //IList<SelectListItem> _result1 = new List<SelectListItem>();
                //if (objstates.Count > 0)
                //{
                //    for (int i = 0; i <= objstates.Count - 1; i++)
                //    {
                //        _result1.Add(new SelectListItem
                //        {
                //            Text = objstates[i].StateFullName,
                //            Value = Convert.ToString(objstates[i].StateId)
                //        });
                //    }
                //}
                //IList<SelectListItem> _result2 = new List<SelectListItem>();
                //_result2.Add(new SelectListItem
                //{
                //    Text = "--Select City--",
                //    Value = "0",
                //    Selected = true
                //});
                //StateCity reg1 = new StateCity();
                //reg1 = new StateCity
                //{
                //    StateList = _result1,
                //    CityList = _result2
                //};
                List<CRMAgent> objlist = new List<CRMAgent>();
                CRMAgent objedit = new CRMAgent();
                objedit.CRM_Agent_ID = CRM_Agent_ID;
                objedit.PageNo = 1;
                objedit.NoofRecords = 1;
                objlist = CRMAgent.GET_CRM_Agent_Details(objedit);
                //edit(objlist);
                return View("EditReseller", objlist);
            
        }
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult EditReseller()
        {
            
            CRMAgent obj = new CRMAgent();
            string HomePhone = null;
            string WorkPhone = null;
            if (string.IsNullOrEmpty(Request["txt_WPhone1"]))
            {
                WorkPhone = null;
            }
            else
            {
                WorkPhone = Request["txt_WPhone1"] + Request["txt_WPhone2"] + Request["txt_WPhone3"];
            }
            if (string.IsNullOrEmpty(Request["txt_CPhone1"]))
            {
                HomePhone = null;
            }
            else
            {
                HomePhone = Request["txt_CPhone1"] + Request["txt_CPhone2"] + Request["txt_CPhone3"];
            }
            obj.Prefix = (Request["DDPrefix"] == " " ? null : Request["DDPrefix"]);
            obj.FirstName = Request["txtFName"];
            obj.MI = Request["txtMName"];
            obj.LastName = Request["txtLName"];
            obj.Suffix = (Request["cbosuffix"] == " " ? null : Request["Add_cbosuffix"]);
            obj.Address1 = Request["txt_Address1"];
            obj.Address2 = Request["txt_Address2"];
            obj.City_ID = Convert.ToInt32(Request["DDCity"] != "0" ? Request["DDCity"] : null);
            obj.State_ID = Convert.ToInt32(Request["DDState"] != "" ? Request["DDState"] : null);
            obj.Email = Request["txt_Email"];
            obj.ZIP = Request["txtZip"];
            obj.HomePhone = HomePhone;
            obj.WorkPhone = WorkPhone;
            obj.CRM_Agent_ID = Request["CRM_Agent_ID"];
            //if (!string.IsNullOrEmpty(Request["Add_txt_DOB"]))
            //{
            //    obj.Expirydate = Request["Add_txt_DOB"];
            //}
            //else
            //{
            obj.Expirydate = null;
            //}
            //if (rdotherpist.SelectedValue == 0)
            //{
            //    obj.ISFTAgent = "Yes";
            //}
            //else if (rdotherpist.SelectedValue == 1)
            //{
            //    obj.ISFTAgent = "No";
            //}
            //else
            //{
            obj.ISFTAgent = null;
            //}
            obj.CompanyName = Request["txt_Employer"];
            //if (string.IsNullOrEmpty(hidApprovalBody.Value))
            //{
            //    hidApprovalBody.Value = ViewState("hidApprovalBody");
            //}
            obj.IncentiveIds = null;

            obj.IsAgent = "Y";

            CRMAgent.Ins_Upd_CRM_Agent_Details(obj);
            string obj1 = null;
            return Json(JsonResponseFactory.SuccessResponse(obj1), JsonRequestBehavior.DenyGet);
        }


        public void edit(List<CRMAgent> objlist)
        {
            //string CellPhone = null;
            //string WorkPhone = null;
            //objCommon.FillTitle(ddlPrefix);
            //objCommon.FillSuffix(ddlSuffix);
            //if (objlist[0].Prefix != null)
            //{
            //    ViewBag.Prefix = objlist[0].Prefix;
            //}
            //else
            //{
            //    ViewBag.Prefix = null;
            //}
            //if (objlist[0].FirstName != null)
            //{
            //    ViewBag.FirstName = objlist[0].FirstName;
            //}
            //else
            //{
            //    ViewBag.FirstName = null;
            //}
            //if (objlist[0].MI != null)
            //{
            //    ViewBag.MI = objlist[0].MI;
            //}
            //else
            //{
            //    ViewBag.MI = null;
            //}
            //if (objlist[0].LastName != null)
            //{
            //    ViewBag.LastName = objlist[0].LastName;
            //}
            //else
            //{
            //    ViewBag.LastName = null;
            //}

            //if (!string.IsNullOrEmpty(objlist[0].Suffix))
            //{
            //    ViewBag.Suffix = objlist[0].Suffix;
            //}
            //else
            //{
            //    ViewBag.Suffix = null;
            //}
            //if (objlist[0].MI != null)
            //{
            //    ViewBag.FullName = ViewBag.Prefix + " " + ViewBag.FirstName + " " + ViewBag.MI + " " + ViewBag.LastName + " " + ViewBag.Suffix;
            //}
            //else
            //{
            //    ViewBag.FullName = ViewBag.Prefix + " " + ViewBag.FirstName + " " + ViewBag.LastName + " " + ViewBag.Suffix;
            //}
            //if (objlist[0].Address1 != null)
            //{
            //    ViewBag.Address1 = objlist[0].Address1;
            //}
            //else
            //{
            //    ViewBag.Address1 = null;
            //}
            //if (objlist[0].Address2 != null)
            //{
            //    ViewBag.Address2 = objlist[0].Address2;
            //}
            //else
            //{
            //    ViewBag.Address2 = null;
            //}
            //if (objlist.Item(0).IncentiveIds != null)
            //{
            //}
            //else
            //{
            //    cbo1.Text = "";
            //}
            //FillAppbodies();

            //if (objlist[0].State_ID != 0)
            //{
            //    ViewBag.State_ID = objlist[0].State_ID;
            //    ViewBag.Statename = objlist[0].State;
            //    //fillcities();
            //    if (objlist[0].City_ID != 0)
            //    {
            //        ViewBag.City_ID = objlist[0].City_ID;
            //    }
            //    else
            //    {
            //        ViewBag.City_ID = null;
            //    }
            //    ViewBag.Cityname = objlist[0].City;
            //}
            //else
            //{
            //    ViewBag.Statename = null;
            //    ViewBag.State_ID = null;
            //    ViewBag.City_ID = null;
            //    ViewBag.Cityname = null;
            //}
            //if (objlist[0].Email != null)
            //{
            //    ViewBag.Email = objlist[0].Email;
            //}
            //else
            //{
            //    ViewBag.Email = null;
            //}
            //if (objlist[0].ZIP != null)
            //{
            //    ViewBag.ZIP = objlist[0].ZIP;
            //}
            //else
            //{
            //    ViewBag.ZIP = null;
            //}

            //CellPhone = objlist[0].HomePhone;
            //ViewBag.CellPhone = CellPhone;
            //if (CellPhone != null && CellPhone != "-")
            //{
            //    ViewBag.txtCellPhone1 = CellPhone.Substring(0, 3);
            //    ViewBag.txtCellPhone2 = CellPhone.Substring(4, 3);
            //    ViewBag.txtCellPhone3 = CellPhone.Substring(8, 4);
            //}
            //else
            //{
            //    ViewBag.txtCellPhone1 = null;
            //    ViewBag.txtCellPhone2 = null;
            //    ViewBag.txtCellPhone3 = null;
            //}
            //WorkPhone = objlist[0].WorkPhone;
            //ViewBag.WorkPhone = WorkPhone;
            //if (WorkPhone != null && WorkPhone != "-")
            //{
            //    ViewBag.txtWorkPhone1 = WorkPhone.Substring(0, 3);
            //    ViewBag.txtWorkPhone2 = WorkPhone.Substring(4, 3);
            //    ViewBag.txtWorkPhone3 = WorkPhone.Substring(8, 4);
            //}
            //else
            //{
            //    ViewBag.txtWorkPhone1 = null;
            //    ViewBag.txtWorkPhone2 = null;
            //    ViewBag.txtWorkPhone3 = null;
            //}

            ////ViewState("Status_Ind") = objlist.Item(0).Status_Ind;
            ////ViewState("CRM_Agent_ID") = objlist.Item(0).CRM_Agent_ID;
            //if (!string.IsNullOrEmpty(objlist[0].AgentCode))
            //{
            //    ViewBag.AgentCode = objlist[0].AgentCode;
            //}
            //else
            //{
            //    ViewBag.AgentCode = null;
            //}
            //if (!string.IsNullOrEmpty(objlist[0].Expirydate))
            //{
            //    ViewBag.Expirydate = objlist[0].Expirydate;
            //}
            //else
            //{
            //    ViewBag.Expirydate = null;
            //}
            //if (objlist[0].CompanyName != null)
            //{
            //    ViewBag.CompanyName = objlist[0].CompanyName;
            //}
            //else
            //{
            //    ViewBag.CompanyName = null;
            //}
            //if (objlist.Item(0).Promocodeurl != null)
            //{
            //    trUrl.Style.Add("display", "");
            //    string strwebsitename = null;
            //    string[] strUrl = HttpContext.Current.Request.Url.ToString().Split("/");
            //    if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
            //    {
            //        strwebsitename = strUrl(0) + "//" + strUrl(2) + "/" + strUrl(3) + "/";
            //    }
            //    else
            //    {
            //        strwebsitename = clsWebConfigsettings.GetConfigSettingsValue("PublicImageChecklocal");
            //    }
            //    lblurl.Text = strwebsitename + "Promocode/" + objlist.Item(0).Promocodeurl;
            //}
            //else
            //{
            //    lblurl.Text = null;
            //}
            //trincentives.Style.Add("display", "none");
            //tr3.Style.Add("display", "none");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Promocodelist(string txtCPT, string btnSearch1, string ddlrecords, string page, string CRM_Agent_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           
                dynamic bugList = GetList(CRM_Agent_ID);
                return View(bugList);
           
        }
        public List<CRMAgent> GetList(string CRMAgent_ID)
        {

            clsCommonFunctions objCommon = new clsCommonFunctions();
            dynamic modelList = new List<CRMAgent>();
            try
            {
            DataSet dsPromocode = new DataSet();
            IDataParameter[] objparam = { new SqlParameter("@In_CRM_Agent_ID", (CRMAgent_ID == "0" ? null : CRMAgent_ID)) };
            objCommon.AddInParameters(objparam);
            dsPromocode = objCommon.GetDataSet("Help_dbo.St_Admin_List_reseller_Promocodes");
            for (int i = 0; i <= dsPromocode.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new CRMAgent();
                //model.Slno = dsPromocode.Tables[0].Rows[i]["Slno"].ToString();
                model.PromoCode = Convert.ToString(dsPromocode.Tables[0].Rows[i]["PromoCode"]);
                model.Promocodeurl = "http://www.plumber-help.net/referral/" + Convert.ToString(dsPromocode.Tables[0].Rows[i]["Promocode_Url"]);
                model.Status_Ind = Convert.ToString(dsPromocode.Tables[0].Rows[i]["Status_ind"]);
                model.CreatedOn = Convert.ToString(dsPromocode.Tables[0].Rows[i]["CreatedOn"]);
                modelList.Add(model);
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ResellersController", "GetList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return modelList;
        }
        public ActionResult Resellersdetails(string CRM_Agent_ID)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
          
                List<CRMAgent> objlist = new List<CRMAgent>();
                CRMAgent objedit = new CRMAgent();
                objedit.CRM_Agent_ID = CRM_Agent_ID;
                objedit.PageNo = 1;
                objedit.NoofRecords = 1;
                objlist = CRMAgent.GET_CRM_Agent_Details(objedit);
                //edit(objlist);
                return View(objlist);
           
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ResellerReport()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        public JsonResult Get_CRM_Agents(string term)
        {
            var objlist = new List<Autocomplete>();
            var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
           new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = objcomman.GetDataReader("Help_dbo.st_Typeahead_CRM_Agent");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[1]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Promocodes(string term,string id)
        {
            var objlist = new List<Autocomplete>();
            var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
           new SqlParameter("@In_CRM_Agent_ID", (id !="" ? id : null)),
           new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = objcomman.GetDataReader("Help_dbo.St_Typeahead_Reseller_Promocode");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[0]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult resellerrpts(string name, string Promocode,string dt_filter,string Agent_id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                        string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "")
            {
                FromDate = null;
                ToDate = null;
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


            ViewBag.name = (name!="" ? name : null);
            ViewBag.Promocode = (Promocode!="" ? Promocode :null);
            ViewBag.From_date = FromDate;
            ViewBag.To_Date = ToDate;
            ViewBag.Agent_id = (Agent_id !="" ? Agent_id : null);
            Getresellerrpts((name != "" ? name : null), (Promocode != "" ? Promocode : null), FromDate, ToDate, (Agent_id != "" ? Agent_id : null));
            return View();
        }
        private void Getresellerrpts(string name,string Promocode, string From_date, string To_Date, string Agent_id)
        {
            try
            {
            clsDBWrapper objWrapper = new clsDBWrapper();

            IDataParameter[] strParam = {
	new SqlParameter("@in_Promocode", Promocode),
	new SqlParameter("@In_OrderByItem", null),
	new SqlParameter("@In_OrderBy",null),
	new SqlParameter("@In_NoOfRecords", null),
	new SqlParameter("@In_PageNo", null),
	new SqlParameter("@In_Fromdate", From_date),
	new SqlParameter("@In_Todate",To_Date),
	new SqlParameter("@In_Agent_Id", Agent_id)
};
            objWrapper.AddInParameters(strParam);
            DataSet dsAppointments = new DataSet();
            dsAppointments = objWrapper.GetDataSet("Help_dbo.st_CRM_List_PromoCodesLog");
            StringBuilder strprnt = new StringBuilder();
            if (name != "" && name != null)
            {
                strprnt = strprnt.Append("<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='100%' border='1'><tr class='gray' align='center' bordercolor='#666666'><td width='100%' height='25' align='center' colspan='5'><b>Reseller Report</b></td></tr>");
                strprnt = strprnt.Append("<tr width='100%'><td width='35%' align='right'> <strong>Reseller Name :</strong> </td><td width='35%' align='left'>" + name + "</td><td></td></tr>");
                strprnt = strprnt.Append("<tr><td width='35%' align='center'><strong>Mower helper name</strong> </td><td width='30%' align='center'><strong>Promo Code</strong> </td><td width='35%' align='center'><strong>Created On</strong> </td></tr>");
            }
            else
            {
                strprnt = strprnt.Append("<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='100%' border='1'><tr class='gray' align='center' bordercolor='#666666'><td width='100%' height='25' align='center' colspan='5'><b>Reseller Report</b></td></tr>");
                strprnt = strprnt.Append("<tr><td width='30%' align='center'><strong>Reseller Name</strong> </td><td width='30%' align='center'><strong>Mower helper name</strong> </td><td width='20%' align='center'><strong>Promo Code</strong> </td><td width='20%' align='center'><strong> 	Created On</strong> </td></tr>");
            }
            foreach (DataRow dr in dsAppointments.Tables[0].Rows)
            {
                string electriciannm = null;
                string resellernm = null;
                string promocode = null;
                string Createdon = null;
                if (!DBNull.Value.Equals(dr["Providername"]))
                {
                    electriciannm = (string)dr["Providername"];
                }
                if (!DBNull.Value.Equals(dr["Fullname"]))
                {
                    resellernm = (string)dr["Fullname"];
                }
                if (!DBNull.Value.Equals(dr["CRM_Promocode_ID"]))
                {
                    promocode = (string)dr["CRM_Promocode_ID"];
                }
                if (!DBNull.Value.Equals(dr["Createdon"]))
                {
                    Createdon = (string)dr["Createdon"];
                }
                if (name != "" && name != null)
                {
                    strprnt = strprnt.Append("<tr><td width='35%' align='center'>" + electriciannm + "</td><td width='30%' align='center'>" + promocode + "</td><td width='35%' align='center'>" + Createdon + "</td></tr>");
                }
                else
                {
                    strprnt = strprnt.Append("<tr><td width='30%' align='center'>" + resellernm + "</td><td width='30%' align='center'>" + electriciannm + "</td><td width='20%' align='center'>" + promocode + "</td><td width='20%' align='center'>" + Createdon + "</td></tr>");
                }
                }
            strprnt = strprnt.Append("</table>");
            ViewBag.prntview = Convert.ToString(strprnt);
            ViewBag.totalrecords = dsAppointments.Tables[1].Rows.Count > 0 ? Convert.ToString(dsAppointments.Tables[1].Rows[0][0]) : null;
            //ViewBag.GetDayViewList = dsAppointments.Tables[0].AsEnumerable().GetEnumerator();
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ResellersController", "Getresellerrpts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void Resellerrptsexcel(string Promocode, string From_date, string To_Date, string Agent_id, string name)
        {
            try
            {
            clsDBWrapper objWrapper = new clsDBWrapper();

            IDataParameter[] strParam = {
	new SqlParameter("@in_Promocode", Promocode),
	new SqlParameter("@In_OrderByItem", null),
	new SqlParameter("@In_OrderBy",null),
	new SqlParameter("@In_NoOfRecords", null),
	new SqlParameter("@In_PageNo", null),
	new SqlParameter("@In_Fromdate", From_date),
	new SqlParameter("@In_Todate",To_Date),
	new SqlParameter("@In_Agent_Id", Agent_id)
};
            objWrapper.AddInParameters(strParam);
            DataSet dsAppointments = new DataSet();
            dsAppointments = objWrapper.GetDataSet("Help_dbo.st_CRM_List_PromoCodesLog");
            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
            strhtml = strhtml + "<tr width='100%'>";
            strhtml = strhtml + "<td align='center' width='100%'><b><u>Reseller reports list</u></b></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (name != "" && name != null)
            {
                strhtml = strhtml+ "<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='100%' border='1'><tr width='100%'><td width='50%' align='right'> <strong>Reseller Name :</strong> </td><td width='50%' align='left'>" + name + "</td></tr>";
                strhtml = strhtml + "<tr><td width='30%' align='center'><strong>Reseller Name</strong> </td><td width='30%' align='center'><strong>Mower helper name</strong> </td><td width='20%' align='center'><strong>Promo Code</strong> </td><td width='20%' align='center'><strong> 	Created On</strong> </td></tr>";
            }
            else
            {
                strhtml = strhtml + "<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='100%' border='1'><tr><td width='30%' align='center'><strong>Reseller Name</strong> </td><td width='30%' align='center'><strong>Mower helper name</strong> </td><td width='20%' align='center'><strong>Promo Code</strong> </td><td width='20%' align='center'><strong> 	Created On</strong> </td></tr>";
            }
            foreach (DataRow dr in dsAppointments.Tables[0].Rows)
            {
                string electriciannm = null;
                string resellernm = null;
                string promocode = null;
                string Createdon = null;
                if (!DBNull.Value.Equals(dr["Providername"]))
                {
                    electriciannm = (string)dr["Providername"];
                }
                if (!DBNull.Value.Equals(dr["Fullname"]))
                {
                    resellernm = (string)dr["Fullname"];
                }
                if (!DBNull.Value.Equals(dr["CRM_Promocode_ID"]))
                {
                    promocode = (string)dr["CRM_Promocode_ID"];
                }
                if (!DBNull.Value.Equals(dr["Createdon"]))
                {
                    Createdon = (string)dr["Createdon"];
                }

                strhtml = strhtml + "<tr><td width='30%' align='center'>" + resellernm + "</td><td width='30%' align='center'>" + electriciannm + "</td><td width='20%' align='center'>" + promocode + "</td><td width='20%' align='center'>" + Createdon + "</td></tr>";
            }
            strhtml = strhtml + "</table>";
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=ResellerReports.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(strhtml);
            Response.End();
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ResellersController", "Resellerrptsexcel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public FileContentResult Resellerrptspdf(string Promocode, string From_date, string To_Date, string Agent_id, string name)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            pdf.Create();
            StringBuilder strhtml = new StringBuilder();
            pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
            Getpdf(Promocode, From_date, To_Date, Agent_id, name);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=ResellerReports.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            return null;
        }
        private void Getpdf(string Promocode, string From_date, string To_Date, string Agent_id, string name)
        {
            try
            {
            clsDBWrapper objWrapper = new clsDBWrapper();

            IDataParameter[] strParam = {
	new SqlParameter("@in_Promocode", Promocode),
	new SqlParameter("@In_OrderByItem", null),
	new SqlParameter("@In_OrderBy",null),
	new SqlParameter("@In_NoOfRecords", null),
	new SqlParameter("@In_PageNo", null),
	new SqlParameter("@In_Fromdate", From_date),
	new SqlParameter("@In_Todate",To_Date),
	new SqlParameter("@In_Agent_Id", Agent_id)
};
            objWrapper.AddInParameters(strParam);
            DataSet dsAppointments = new DataSet();
            dsAppointments = objWrapper.GetDataSet("Help_dbo.st_CRM_List_PromoCodesLog");




            //WebGrid grid = new WebGrid(source: dsAppointments, canPage: false, canSort: false);
            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr width='100%'>";
            strhtml = strhtml + "<td align='center' width='100%'><b><u>Reseller reports list</u></b></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (name != "" && name != null)
            {
                strhtml = strhtml + "<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='90%' border='1'><tr width='100%'><td width='50%' align='right'> <strong>Reseller Name :</strong> </td><td width='50%' align='left'>" + name + "</td></tr>";
                strhtml = strhtml + "<tr><td width='30%' align='center'><strong>Reseller Name</strong> </td><td width='30%' align='center'><strong>Mower helper name</strong> </td><td width='20%' align='center'><strong>Promo Code</strong> </td><td width='20%' align='center'><strong> 	Created On</strong> </td></tr>";
            }
            else
            {
                strhtml = strhtml + "<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='8' width='100%' border='1'><tr><td width='30%' align='center'><strong>Reseller Name</strong> </td><td width='30%' align='center'><strong>Mower helper name</strong> </td><td width='20%' align='center'><strong>Promo Code</strong> </td><td width='20%' align='center'><strong> 	Created On</strong> </td></tr>";
            }
            foreach (DataRow dr in dsAppointments.Tables[0].Rows)
            {
                string electriciannm = null;
                string resellernm = null;
                string promocode = null;
                string Createdon = null;
                if (!DBNull.Value.Equals(dr["Providername"]))
                {
                    electriciannm = (string)dr["Providername"];
                }
                if (!DBNull.Value.Equals(dr["Fullname"]))
                {
                    resellernm = (string)dr["Fullname"];
                }
                if (!DBNull.Value.Equals(dr["CRM_Promocode_ID"]))
                {
                    promocode = (string)dr["CRM_Promocode_ID"];
                }
                if (!DBNull.Value.Equals(dr["Createdon"]))
                {
                    Createdon = (string)dr["Createdon"];
                }

                strhtml = strhtml + "<tr><td width='30%' align='center'>" + resellernm + "</td><td width='30%' align='center'>" + electriciannm + "</td><td width='20%' align='center'>" + promocode + "</td><td width='20%' align='center'>" + Createdon + "</td></tr>";
            }
            strhtml = strhtml + "</table>";
           

            double pos3 = 300;
            pdf.AddHTMLPos(pos3, 10, strhtml);
               }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ResellersController", "Getpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
 
    }
}
