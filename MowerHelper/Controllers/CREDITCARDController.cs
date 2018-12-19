using Newtonsoft.Json;
using Obout.Mvc.ComboBox;
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
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.Classes;
//using PayPal.Api;

namespace MowerHelper.Controllers
{
    public class CREDITCARDController : Controller
    {
        string totalrecords = "0";
        SqlDataReader dreaderType = default(SqlDataReader);
        SqlConnection cnn12 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        SqlConnection cnn13 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        [ActionName("CCTransactionResponse")]
        public ActionResult CreditCardResponse()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            CCProcess objCcProcess = new CCProcess();
            if (Request["dt_filter"] == "30")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }

            else if (Request["dt_filter"] == "Today")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = strDate;
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "Today";
            }
            else if (Request["dt_filter"] == "Custom")
            {
                if (!string.IsNullOrEmpty(Request["txt_FromDate"]))
                {
                    ViewBag.frmdate = Request["txt_FromDate"];

                }
                if (!string.IsNullOrEmpty(Request["txt_ToDate"]))
                {
                    ViewBag.todate = Request["txt_ToDate"];

                }
                ViewBag.setDt = "Custom";
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = ViewBag.todate;
            }
            else if (Request["dt_filter"] == "7")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "7";
            }
            else
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }
            if (Request["ddlStatus"] == "1")
            {
                objCcProcess.StatusCode = 1;
                ViewBag.StatusCode = "1";
            }
            else if (Request["ddlStatus"] == "2")
            {
                objCcProcess.StatusCode = 2;
                ViewBag.StatusCode = "2";
            }
            else
            {
                objCcProcess.StatusCode = null;
                ViewBag.StatusCode = "0";
            }

            objCcProcess.OrderbyItem = "Responsedate";
            if (Request["sortdir"] == null)
            {
                objCcProcess.Orderby = "DESC";
                ViewBag.sortdir = "DESC";
            }
            else
            {
                objCcProcess.Orderby = Request["sortdir"].ToString();
                ViewBag.sortdir = Request["sortdir"].ToString();
            }
            if (Request["page"] == null)
            {
                objCcProcess.PageNo = 1;
            }
            else
            {
                objCcProcess.PageNo = Convert.ToInt32(Request["page"]);
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
            if (Request["ddlPaidto"] == "2")
            {
                objCcProcess.practicename = "Provider";
                ViewBag.paidto = "2";
            }
            else if (Request["ddlPaidto"] == "1")
            {
                objCcProcess.practicename = "Tenniscoach-Help";
                ViewBag.paidto = "1";
            }
            else
            {
                objCcProcess.practicename = null;
                ViewBag.paidto = "0";
            }

            if (pgesize != null)
            {
                objCcProcess.NoofRecords = Convert.ToInt32(pgesize);
            }
            else
            {
                objCcProcess.NoofRecords = 10;
            }
            dynamic CreditcardResponselist = getCCResponse(objCcProcess);
            return View("CreditCardResponse",CreditcardResponselist);
        }
        public ActionResult ViewRequestResponse(int? tranid)
        {
            
                if (tranid != null)
                {
                    DataSet dsview = new DataSet();
                    dsview = CCProcess.viewRequest(tranid);
                    if (dsview.Tables[0].Rows.Count > 0)
                    {
                        string strreq = Convert.ToString(dsview.Tables[0].Rows[0]["Posted_Data"]);
                        string[] req = strreq.Split('}');
                        if (req.Length > 7)
                        {
                            ViewBag.requestdata = req[0] + "}" + req[1] + "}" + req[2] + "}" + req[3] + "}" + req[4] + "}" + req[5] + "}" + req[6] + "}";
                        }
                        else if (req.Length >= 4)
                        {
                            ViewBag.requestdata = req[0] + "}" + req[1] + "}" + req[2] + "}" + req[3] + "}";
                        }
                        else
                        {
                            ViewBag.requestdata = null;
                        }
                        ViewBag.responsedata = Convert.ToString(dsview.Tables[0].Rows[0]["Response_Data"]);// Decrypt(dsview.Tables[0].Rows[0]["Response_Data"].ToString());
                    }
                }
                return View();
            
        }
        //private string Decrypt(string cipherText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    cipherText = cipherText.Replace(" ", "+");
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}
        [ActionName("CCTransactionsLedger")]
        public ActionResult CreditCardDetails()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string hdnPrid = Request["ComboBoxPr"];
            string hdnname = Request["ComboBoxPr_SelectedText"];
            //string strSearchPrid = Request["hdnPrid"] != null ? Request["hdnPrid"] : null;
            string strSearchPrid = hdnPrid != null ? hdnPrid : null;
            CCProcess objCcProcess = new CCProcess();
            if (!string.IsNullOrEmpty(strSearchPrid))
            {

                //string[] strlength = strSearchPrid.Split('$');
                //TempData["Practiceid"] = strlength[0];
                TempData["providerid"] = hdnPrid;
                if (!string.IsNullOrEmpty(hdnname))
                {
                    Session["ComboProvFullName"] = hdnname;
                    TempData["combotext"] = hdnname;
                }
                else
                {
                    TempData["combotext"] = "";
                }
                //string strid = strlength[4].Trim();
                objCcProcess.practicename = Convert.ToString(Session["ComboProvFullName"]);
                FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strSearchPrid);

            }
            else
            {
                TempData["combotext"] = "";
            }
            if (Request["dt_filter"] == "30")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }

            else if (Request["dt_filter"] == "Today")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = strDate;
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "Today";
            }
            else if (Request["dt_filter"] == "Custom")
            {
                if (!string.IsNullOrEmpty(Request["txt_FromDate"]))
                {
                    ViewBag.frmdate = Request["txt_FromDate"];

                }
                if (!string.IsNullOrEmpty(Request["txt_ToDate"]))
                {
                    ViewBag.todate = Request["txt_ToDate"];

                }
                ViewBag.setDt = "Custom";
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = ViewBag.todate;
            }
            else if (Request["dt_filter"] == "All")
            {
                objCcProcess.STARTDATE = null;
                objCcProcess.ENDDATE = null;
                ViewBag.frmdate = null;
                ViewBag.todate = null;
                ViewBag.setDt = "All";
            }
            else if (Request["dt_filter"] == "7")
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "7";
            }
            else
            {
                string strDate = DateTime.Now.ToShortDateString();
                ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                objCcProcess.STARTDATE = ViewBag.frmdate;
                objCcProcess.ENDDATE = strDate;
                ViewBag.todate = strDate;
                ViewBag.setDt = "30";
            }
            objCcProcess.strPracticeID = null;
            objCcProcess.OrderbyItem = "Tran_Date";
            if (Request["sortdir"] == null)
            {
                objCcProcess.Orderby = "DESC";
                ViewBag.sortdir = "DESC";
            }
            else
            {
                objCcProcess.Orderby = Request["sortdir"].ToString();
                ViewBag.sortdir = Request["sortdir"].ToString();
            }
            if (Request["page"] == null)
            {
                objCcProcess.PageNo = 1;
            }
            else
            {
                objCcProcess.PageNo = Convert.ToInt32(Request["page"]);
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
                objCcProcess.NoofRecords = Convert.ToInt32(pgesize);
            }
            else
            {
                objCcProcess.NoofRecords = 10;
            }
            dynamic CreditcardDetailslist = getCCDetails(objCcProcess);
            return View("CreditCardDetails",CreditcardDetailslist);
        }
        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredinsurances1(args.Text, 0);

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
        protected ComboBoxItemList GetFilteredinsurances1(string filter, int offset)
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
            ViewData["ComboBoxPr"] = payerlist;

            return payerlist;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CREDITCARDController", "GetFilteredinsurances1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }

        }
        public List<CCProcess> getCCResponse(CCProcess objCCResponse)
        {

            List<CCProcess> objgetCCResponse = new List<CCProcess>();
            try{
            objgetCCResponse = objCCResponse.ccProcessResponse(objCCResponse);
            if (objgetCCResponse.Count > 0)
            {
                totalrecords = Convert.ToString(CCProcess.Totalnoofrecords);
                ViewBag.totrec = totalrecords;
            }
            else
            {
                ViewBag.totrec = "0";
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CREDITCARDController", "getCCResponse", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objgetCCResponse;
        }
        public List<CCProcess> getCCDetails(CCProcess objCCDetails)
        {
            List<CCProcess> objgetCCDetails = new List<CCProcess>();
            try{
            objgetCCDetails = objCCDetails.Get_AdminCreditCardTransaction(objCCDetails);
            if (objgetCCDetails.Count > 0)
            {
                totalrecords = Convert.ToString(CCProcess.Totalnoofrecords);
                ViewBag.totrec = totalrecords;
            }
            else
            {
                ViewBag.totrec = "0";
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CREDITCARDController", "getCCDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objgetCCDetails;
        }
        public void FillProvider(string filter, int offset, string hdnprid)
        {
            try{
            PatientRegistration obj = new PatientRegistration();
            List<PatientRegistration> objDataList = new List<PatientRegistration>();
            string PracticeName = filter.Trim();
            objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


            ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
            List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
            if (!string.IsNullOrEmpty(hdnprid))
            {
                foreach (ComboBoxItem item in payerlist)
                {
                    if (item.Value == hdnprid)
                    {
                        item.Selected = true;
                    }
                    items.Add(item);
                }
                ViewData["ComboBoxPr"] = items;
            }
            else
            {
                ViewData["ComboBoxPr"] = payerlist;
            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CREDITCARDController", "FillProvider", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        [HttpGet]
        public ActionResult GetrefundDetails(int tranid, int PaymentID, int PaymentTransID)
        {
           
                ViewBag.PaymentTransID = PaymentTransID;
                //    DataSet dsset = CCProcess.Get_ChildorParentCCTransactions(tranid, "Y", null);
                LoadCreditCardInfo(tranid, PaymentID);
                ViewBag.Transactionno = Convert.ToString(tranid);
                ViewBag.paypaltransactionid = Request["paypaltransactionid"];
                ViewBag.paypalsaletransactionid = Request["paypalsaletransactionid"];
                return View();
            
        }
        [HttpGet]
        public JsonResult GetrefundDetails12(string paypalsaleid, string Refund_amount, string ToReference_ID, string Transactionno, string paypaltransactionid, string paypalsaletransactionid, string PaymentTransID, string tomailid, string proname)
        {
            string msg =null;
            //public JsonResult GetrefundDetails12(string paypalsaleid, string Refund_amount, string ToReference_ID, string Transactionno, string tomailid,string proname)
            //Session["UserID"] = null;
            if (Request.IsAjaxRequest() & Session["UserID"] == null)
            {
                //string errmsg = "false"
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
                //return Json(errmsg,JsonRequestBehavior.AllowGet);
            }

            try
            {

                if (paypalsaletransactionid != null && paypalsaletransactionid != "")
                {
                    SqlConnection cnn11 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                    CCProcess obj = new CCProcess();
                    obj.paypalsaleid = paypalsaleid;
                    obj.Refund_amount = Refund_amount;
                    obj.ToReference_ID = ToReference_ID;
                    obj.Transactionno = Transactionno;
                    string Loc_TransactionID = null;
                    APIContext apiContext = Configuration.GetAPIContext();
                    Sale sale = new Sale();
                    sale.id = obj.paypalsaleid;
                    Amount amount = new Amount();
                    amount.total = Convert.ToString(obj.Refund_amount);
                    amount.currency = "USD";
                    Refund refund = new Refund();
                    refund.amount = amount;
                    Refund newRefund = sale.Refund(apiContext, refund);
                    string saleid = sale.ConvertToJson();
                    string Posted_Data = new StringBuilder(saleid).Append(refund.ConvertToJson()).ToString();
                    string response = Newtonsoft.Json.Linq.JObject.Parse(newRefund.ConvertToJson()).ToString(Formatting.Indented);
                    if (newRefund.state == "completed")
                    {
                        SqlDataReader objread = null;
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Help_dbo.st_CreditCard_INS_Transaction";
                        cmd.Connection = cnn11;
                        cnn11.Open();
                        IDataParameter[] paramlist = {
                new SqlParameter("@in_FromReference_ID", "1"),
                new SqlParameter("@in_FromReferenceType_ID", "1"),
                new SqlParameter("@in_ToReference_ID", obj.ToReference_ID),
                new SqlParameter("@in_ToReferenceType_ID", "2"),
                new SqlParameter("@in_Posted_Data", Posted_Data),// newRefund.ConvertToJson()),
                new SqlParameter("@in_InvoiceNum", null),
                new SqlParameter("@in_TransactionNo", null),
                new SqlParameter("@in_ManualTransaction_Ind", 0),
                new SqlParameter("@in_Amount", Convert.ToString(obj.Refund_amount)),
                new SqlParameter("@in_CreditCardNo", null),
                new SqlParameter("@in_CreditCardType_Code", null),
                new SqlParameter("@in_CVV2", null),
                new SqlParameter("@in_CardHolderName", null),
                new SqlParameter("@In_Firstname", null),
                new SqlParameter("@In_Lastname", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "Y"),
                new SqlParameter("@in_CardExpiryMonth", null),
                new SqlParameter("@in_CardExpiryYear", null),
                new SqlParameter("@in_BillingAddress_1", null),
                new SqlParameter("@in_BillingAddress_2", null),
                new SqlParameter("@in_BillingCity_ID", null),
                new SqlParameter("@in_BillingState_ID", null),
                new SqlParameter("@in_BillingCountry_ID", null),
                new SqlParameter("@in_BillingZIP", null),
                new SqlParameter("@in_ParentTransaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", 0),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_Practice_ID", null),
                new SqlParameter("@in_provider_id", ToReference_ID),
                new SqlParameter("@In_RefpatientLogin_ID", null),
                 new SqlParameter("@in_paypaltransactionid", (newRefund.id!= null ? newRefund.id : null)),
                    new SqlParameter("@in_paypalsaletransactionid", (obj.paypalsaleid != null? obj.paypalsaleid : null))
            };
                        foreach (SqlParameter param in paramlist)
                        {
                            cmd.Parameters.Add(param);
                        }
                        objread = cmd.ExecuteReader();

                        if (objread.Read())
                        {
                            Loc_TransactionID = Convert.ToString(objread["TransactionID"]);
                        }
                        else
                        {
                            Loc_TransactionID = null;
                        }
                        cnn11.Close();
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "Help_dbo.st_CreditCard_TransactionResponse";
                        cmd1.Connection = cnn12;
                        cnn12.Open();
                        IDataParameter[] paramlists = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                new SqlParameter("@in_Status", newRefund.state),
                new SqlParameter("@in_Response_Data", response),
                new SqlParameter("in_Status_Code", "1"),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_GatewayDetail_ID", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "Y")
            };
                        foreach (SqlParameter param in paramlists)
                        {
                            cmd1.Parameters.Add(param);
                        }

                        cmd1.ExecuteNonQuery();
                        cnn12.Close();

                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.CommandText = "Help_dbo.st_CreditCard_UPD_RefundTransaction";
                        cmd3.Connection = cnn13;
                        cnn13.Open();
                        IDataParameter[] paramlist2 = {
                new SqlParameter("@in_Transaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", "N"),
                new SqlParameter("@in_UpdatedBy", "1"),
                new SqlParameter("@in_CCTransaction_ID", Loc_TransactionID),
                new SqlParameter("@in_ReferencetypeID", "2")
            };
                        foreach (SqlParameter param in paramlist2)
                        {
                            cmd3.Parameters.Add(param);
                        }
                        cmd3.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "Help_dbo.st_CreditCard_UPD_ReRefundTransaction";
                        cmd2.Connection = cnn13;
                        IDataParameter[] paramlists1 = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                new SqlParameter("@in_IsRefundable_Ind", "R"),
                new SqlParameter("@in_UpdatedBy", "1")
            };
                        foreach (SqlParameter param in paramlists1)
                        {
                            cmd2.Parameters.Add(param);
                        }
                        cmd2.ExecuteNonQuery();
                        cnn13.Close();






                        RPBilling adminNewAdj = new RPBilling();
                        // string currentdt = null;
                        // currentdt = DateTime.Now.ToShortDateString();
                        adminNewAdj.fromReferenceId = 1;
                        adminNewAdj.ReferenceType_ID = "1";
                        adminNewAdj.ToReferenceType_ID = "2";
                        adminNewAdj.ToReference_ID = obj.ToReference_ID;
                        adminNewAdj.BillingService_ID = null;
                        adminNewAdj.Transaction_Date = DateTime.Now.ToShortDateString();
                        adminNewAdj.Transaction_Amount = obj.Refund_amount;
                        adminNewAdj.TransactionType_ID = "3";
                        adminNewAdj.AdjustmentType_ID = 5;
                        adminNewAdj.Notes = null;
                        adminNewAdj.Practice_ID = null;
                        if (PaymentTransID != "" && PaymentTransID != null)
                        {
                            adminNewAdj.RefTransaction_ID = Convert.ToInt32(PaymentTransID);
                        }
                        else
                        {
                            adminNewAdj.RefTransaction_ID = null;
                        }
                        adminNewAdj.UserId = Convert.ToString(Session["UserID"]);
                        adminNewAdj.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));

                        RPBilling adminNewAdj1 = new RPBilling();
                        adminNewAdj1.fromReferenceId = 1;
                        adminNewAdj1.ReferenceType_ID = "1";
                        adminNewAdj1.ToReferenceType_ID = "2";
                        adminNewAdj1.ToReference_ID = obj.ToReference_ID;
                        adminNewAdj1.BillingService_ID = null;
                        adminNewAdj1.Transaction_Date = DateTime.Now.ToShortDateString();
                        adminNewAdj1.Transaction_Amount = obj.Refund_amount;
                        adminNewAdj1.TransactionType_ID = "4";
                        adminNewAdj1.AdjustmentType_ID = 5;
                        adminNewAdj1.Notes = null;
                        adminNewAdj1.Practice_ID = null;
                        if (PaymentTransID != "" && PaymentTransID != null)
                        {
                            adminNewAdj1.RefTransaction_ID = Convert.ToInt32(PaymentTransID);
                        }
                        else
                        {
                            adminNewAdj1.RefTransaction_ID = null;
                        }
                        adminNewAdj1.UserId = Convert.ToString(Session["UserID"]);
                        adminNewAdj1.MakeAdminAdjTransaction(adminNewAdj1, Convert.ToInt32(Session["Loginhistory_ID"]));
                         msg = "1";
                        //return Json(msg);
                        //return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                         msg = "2";
                        //return Json(msg);
                       // return Json(JsonResponseFactory.ErrorResponse(msg), JsonRequestBehavior.DenyGet);
                    }
                    //  adminNewAdj1.UserId = Session["UserID"].ToString();
                    // adminNewAdj1.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));
                    //string msg1 = "1";
                    //return Json(msg);
                    SendEmailtoElectrician(tomailid, Loc_TransactionID, proname, DateTime.Now.ToShortDateString(), obj.Refund_amount);
                    return Json(msg, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    SqlConnection cnn11 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                    CCProcess obj = new CCProcess();
                    obj.paypalsaleid = paypalsaleid;
                    obj.Refund_amount = Refund_amount;
                    obj.ToReference_ID = ToReference_ID;
                    obj.Transactionno = Transactionno;
                    string Loc_TransactionID = null;
                    //APIContext apiContext = Configuration.GetAPIContext();
                    //Sale sale = new Sale();
                    //sale.id = obj.paypalsaleid;
                    //Amount amount = new Amount();
                    //amount.total = Convert.ToString(obj.Refund_amount);
                    //amount.currency = "USD";
                    //Refund refund = new Refund();
                    //refund.amount = amount;
                    //Refund newRefund = sale.Refund(apiContext, refund);
                    //string response = Newtonsoft.Json.Linq.JObject.Parse(newRefund.ConvertToJson()).ToString(Formatting.Indented);

                    double strip_amount = Convert.ToDouble(obj.Refund_amount) * 100;
                    //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                    var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                    var refundCharge = gateway.Post(new RefundStripeCharge
                    {
                        ChargeId = paypaltransactionid,
                        Amount = Convert.ToInt32(strip_amount),
                    });

                    //if (newRefund.state == "completed")
                    //{
                    SqlDataReader objread = null;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Help_dbo.st_CreditCard_INS_Transaction";
                    cmd.Connection = cnn11;
                    cnn11.Open();
                    IDataParameter[] paramlist = {
                new SqlParameter("@in_FromReference_ID", "1"),
                new SqlParameter("@in_FromReferenceType_ID", "1"),
                new SqlParameter("@in_ToReference_ID", obj.ToReference_ID),
                new SqlParameter("@in_ToReferenceType_ID", "2"),
                //new SqlParameter("@in_Posted_Data", newRefund.ConvertToJson()),
                 new SqlParameter("@in_Posted_Data", null),
                new SqlParameter("@in_InvoiceNum", null),
                new SqlParameter("@in_TransactionNo", null),
                new SqlParameter("@in_ManualTransaction_Ind", 0),
                new SqlParameter("@in_Amount", Convert.ToString(obj.Refund_amount)),
                new SqlParameter("@in_CreditCardNo", null),
                new SqlParameter("@in_CreditCardType_Code", null),
                new SqlParameter("@in_CVV2", null),
                new SqlParameter("@in_CardHolderName", null),
                new SqlParameter("@In_Firstname", null),
                new SqlParameter("@In_Lastname", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "N"),
                new SqlParameter("@in_CardExpiryMonth", null),
                new SqlParameter("@in_CardExpiryYear", null),
                new SqlParameter("@in_BillingAddress_1", null),
                new SqlParameter("@in_BillingAddress_2", null),
                new SqlParameter("@in_BillingCity_ID", null),
                new SqlParameter("@in_BillingState_ID", null),
                new SqlParameter("@in_BillingCountry_ID", null),
                new SqlParameter("@in_BillingZIP", null),
                new SqlParameter("@in_ParentTransaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", 0),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_Practice_ID", null),
                new SqlParameter("@in_provider_id", ToReference_ID),
                new SqlParameter("@In_RefpatientLogin_ID", null),
                 //new SqlParameter("@in_paypaltransactionid", (newRefund.id!= null ? newRefund.id : null)),
                  new SqlParameter("@in_paypaltransactionid", (refundCharge.Id!= null ? refundCharge.Id : null)),
                    //new SqlParameter("@in_paypalsaletransactionid", (obj.paypalsaleid != null? obj.paypalsaleid : null))
                    new SqlParameter("@in_paypalsaletransactionid", null)
            };
                    foreach (SqlParameter param in paramlist)
                    {
                        cmd.Parameters.Add(param);
                    }
                    objread = cmd.ExecuteReader();

                    if (objread.Read())
                    {
                        Loc_TransactionID = Convert.ToString(objread["TransactionID"]);
                    }
                    else
                    {
                        Loc_TransactionID = null;
                    }
                    cnn11.Close();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandText = "Help_dbo.st_CreditCard_TransactionResponse";
                    cmd1.Connection = cnn12;
                    cnn12.Open();
                    IDataParameter[] paramlists = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                //new SqlParameter("@in_Status", newRefund.state),
                new SqlParameter("@in_Status", "completed"),
                //new SqlParameter("@in_Response_Data", response),
                new SqlParameter("@in_Response_Data", null),
                new SqlParameter("in_Status_Code", "1"),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_GatewayDetail_ID", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "N")
            };
                    foreach (SqlParameter param in paramlists)
                    {
                        cmd1.Parameters.Add(param);
                    }

                    cmd1.ExecuteNonQuery();
                    cnn12.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandText = "Help_dbo.st_CreditCard_UPD_RefundTransaction";
                    cmd3.Connection = cnn13;
                    cnn13.Open();
                    IDataParameter[] paramlist2 = {
                new SqlParameter("@in_Transaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", "N"),
                new SqlParameter("@in_UpdatedBy", "1"),
                new SqlParameter("@in_CCTransaction_ID", Loc_TransactionID),
                new SqlParameter("@in_ReferencetypeID", "2")
            };
                    foreach (SqlParameter param in paramlist2)
                    {
                        cmd3.Parameters.Add(param);
                    }
                    cmd3.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandText = "Help_dbo.st_CreditCard_UPD_ReRefundTransaction";
                    cmd2.Connection = cnn13;
                    IDataParameter[] paramlists1 = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                new SqlParameter("@in_IsRefundable_Ind", "R"),
                new SqlParameter("@in_UpdatedBy", "1")
            };
                    foreach (SqlParameter param in paramlists1)
                    {
                        cmd2.Parameters.Add(param);
                    }
                    cmd2.ExecuteNonQuery();
                    cnn13.Close();






                    RPBilling adminNewAdj = new RPBilling();
                    // string currentdt = null;
                    // currentdt = DateTime.Now.ToShortDateString();
                    adminNewAdj.fromReferenceId = 1;
                    adminNewAdj.ReferenceType_ID = "1";
                    adminNewAdj.ToReferenceType_ID = "2";
                    adminNewAdj.ToReference_ID = obj.ToReference_ID;
                    adminNewAdj.BillingService_ID = null;
                    adminNewAdj.Transaction_Date = DateTime.Now.ToShortDateString();
                    adminNewAdj.Transaction_Amount = obj.Refund_amount;
                    adminNewAdj.TransactionType_ID = "3";
                    adminNewAdj.AdjustmentType_ID = 5;
                    adminNewAdj.Notes = null;
                    adminNewAdj.Practice_ID = null;
                    if (PaymentTransID != "" && PaymentTransID != null)
                    {
                        adminNewAdj.RefTransaction_ID = Convert.ToInt32(PaymentTransID);
                    }
                    else
                    {
                        adminNewAdj.RefTransaction_ID = null;
                    }
                    adminNewAdj.UserId = Convert.ToString(Session["UserID"]);
                    adminNewAdj.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));

                    RPBilling adminNewAdj1 = new RPBilling();
                    adminNewAdj1.fromReferenceId = 1;
                    adminNewAdj1.ReferenceType_ID = "1";
                    adminNewAdj1.ToReferenceType_ID = "2";
                    adminNewAdj1.ToReference_ID = obj.ToReference_ID;
                    adminNewAdj1.BillingService_ID = null;
                    adminNewAdj1.Transaction_Date = DateTime.Now.ToShortDateString();
                    adminNewAdj1.Transaction_Amount = obj.Refund_amount;
                    adminNewAdj1.TransactionType_ID = "4";
                    adminNewAdj1.AdjustmentType_ID = 5;
                    adminNewAdj1.Notes = null;
                    adminNewAdj1.Practice_ID = null;
                    if (PaymentTransID != "" && PaymentTransID != null)
                    {
                        adminNewAdj1.RefTransaction_ID = Convert.ToInt32(PaymentTransID);
                    }
                    else
                    {
                        adminNewAdj1.RefTransaction_ID = null;
                    }
                    adminNewAdj1.UserId = Convert.ToString(Session["UserID"]);
                    adminNewAdj1.MakeAdminAdjTransaction(adminNewAdj1, Convert.ToInt32(Session["Loginhistory_ID"]));
                     msg = "1";
                    //return Json(msg);
                    SendEmailtoElectrician(tomailid, Loc_TransactionID, proname, DateTime.Now.ToShortDateString(), obj.Refund_amount);
                    return Json(msg, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    string msg = "2";
                    //    //return Json(msg);
                    //    return Json(JsonResponseFactory.ErrorResponse(msg), JsonRequestBehavior.DenyGet);
                    //}
                }
            }
            catch (StripeException ex)
            {
                string errormessage = ex.Message;
                if (errormessage != null)
                {
                    SqlConnection cnn11 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
                    string Loc_TransactionID = null;
                    SqlDataReader objread = null;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Help_dbo.st_CreditCard_INS_Transaction";
                    cmd.Connection = cnn11;
                    cnn11.Open();
                    IDataParameter[] paramlist = {
                new SqlParameter("@in_FromReference_ID", "1"),
                new SqlParameter("@in_FromReferenceType_ID", "1"),
                new SqlParameter("@in_ToReference_ID", ToReference_ID),
                new SqlParameter("@in_ToReferenceType_ID", "2"),
                //new SqlParameter("@in_Posted_Data", newRefund.ConvertToJson()),
                 new SqlParameter("@in_Posted_Data", ex.Message),
                new SqlParameter("@in_InvoiceNum", null),
                new SqlParameter("@in_TransactionNo", null),
                new SqlParameter("@in_ManualTransaction_Ind", 0),
                new SqlParameter("@in_Amount", Convert.ToString(Refund_amount)),
                new SqlParameter("@in_CreditCardNo", null),
                new SqlParameter("@in_CreditCardType_Code", null),
                new SqlParameter("@in_CVV2", null),
                new SqlParameter("@in_CardHolderName", null),
                new SqlParameter("@In_Firstname", null),
                new SqlParameter("@In_Lastname", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "N"),
                new SqlParameter("@in_CardExpiryMonth", null),
                new SqlParameter("@in_CardExpiryYear", null),
                new SqlParameter("@in_BillingAddress_1", null),
                new SqlParameter("@in_BillingAddress_2", null),
                new SqlParameter("@in_BillingCity_ID", null),
                new SqlParameter("@in_BillingState_ID", null),
                new SqlParameter("@in_BillingCountry_ID", null),
                new SqlParameter("@in_BillingZIP", null),
                new SqlParameter("@in_ParentTransaction_ID", Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", 0),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_Practice_ID", null),
                new SqlParameter("@in_provider_id", ToReference_ID),
                new SqlParameter("@In_RefpatientLogin_ID", null),
                  new SqlParameter("@in_paypaltransactionid",null),
                    new SqlParameter("@in_paypalsaletransactionid", null)
            };
                    foreach (SqlParameter param in paramlist)
                    {
                        cmd.Parameters.Add(param);
                    }
                    objread = cmd.ExecuteReader();

                    if (objread.Read())
                    {
                        Loc_TransactionID = Convert.ToString(objread["TransactionID"]);
                    }
                    else
                    {
                        Loc_TransactionID = null;
                    }
                    cnn11.Close();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandText = "Help_dbo.st_CreditCard_TransactionResponse";
                    cmd1.Connection = cnn12;
                    cnn12.Open();
                    IDataParameter[] paramlists = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                //new SqlParameter("@in_Status", newRefund.state),
                new SqlParameter("@in_Status", Convert.ToString(ex.StatusCode)),
                //new SqlParameter("@in_Response_Data", response),
                new SqlParameter("@in_Response_Data", ex.Message),
                new SqlParameter("in_Status_Code", ex.Code),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_GatewayDetail_ID", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "N")
            };
                    foreach (SqlParameter param in paramlists)
                    {
                        cmd1.Parameters.Add(param);
                    }

                    cmd1.ExecuteNonQuery();
                    cnn12.Close();
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, "CREDITCARDController", "GetrefundDetails12", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            string msg2 = "1";
            return Json(msg2, JsonRequestBehavior.AllowGet);

        }
        public void SendEmailtoElectrician(string strTomailid, string strTransaction_ID, string strProvidername, string strtransactiondate, string stramoumt)
        {
            //var objconfig = new clsWebConfigsettings();
            try
            {
            if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
            {
                var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(84, 0);
                if ((EMO != null))
                {
                    ViewBag.Subject = EMO.Msg_Subject ?? null;
                    ViewBag.Content = EMO.Msg_Body ?? null;
                }
                string str_Content = ViewBag.Content;
                if (str_Content != null)
                {
                    if (strProvidername != null)
                    {
                        str_Content = str_Content.Replace("$ProviderName$", strProvidername);
                    }
                    str_Content = str_Content.Replace("$CSRName$", "Mower helper");
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
                    //if (strtransactiondate != null)
                    //{
                    //    str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                    //}
                }
                string strFrommailid = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                var objSendMessage = new ClsMailMessage();
                bool strvalid = objSendMessage.SendMail(strTomailid, strFrommailid, ViewBag.Subject, str_Content, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                if (strvalid == true)
                {
                    clsCommonFunctions objclscommon = new clsCommonFunctions();
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strTransaction_ID) };
                    objclscommon.AddInParameters(strMailParam);
                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }

            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CREDITCARDController", "SendEmailtoElectrician", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetrefundDetails1(CCProcess obj)
        {
            if (Request.IsAjaxRequest() & Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }

            //ViewBag.Hdnsaleid = Request["Hdnsaleid"];
            SqlConnection cnn11 = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

            string Loc_TransactionID = null;
            APIContext apiContext = Configuration.GetAPIContext();
            Sale sale = new Sale();
            sale.id = obj.paypalsaleid;
            Amount amount = new Amount();
            amount.total = Convert.ToString(obj.Refund_amount);
            amount.currency = "USD";
            Refund refund = new Refund();
            refund.amount = amount;
            Refund newRefund = sale.Refund(apiContext, refund);
            string response = Newtonsoft.Json.Linq.JObject.Parse(newRefund.ConvertToJson()).ToString(Formatting.Indented);
            if (newRefund.state == "completed")
            {
                SqlDataReader objread = null;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Help_dbo.st_CreditCard_INS_Transaction";
                cmd.Connection = cnn11;
                cnn11.Open();
                IDataParameter[] paramlist = {
                new SqlParameter("@in_FromReference_ID", "1"),
                new SqlParameter("@in_FromReferenceType_ID", "1"),
                new SqlParameter("@in_ToReference_ID", obj.ToReference_ID),
                new SqlParameter("@in_ToReferenceType_ID", "2"),
                new SqlParameter("@in_Posted_Data", null),
                new SqlParameter("@in_InvoiceNum", null),
                new SqlParameter("@in_TransactionNo", null),
                new SqlParameter("@in_ManualTransaction_Ind", 0),
                new SqlParameter("@in_Amount", Convert.ToString(obj.Refund_amount)),
                new SqlParameter("@in_CreditCardNo", null),
                new SqlParameter("@in_CreditCardType_Code", null),
                new SqlParameter("@in_CVV2", null),
                new SqlParameter("@in_CardHolderName", null),
                new SqlParameter("@In_Firstname", null),
                new SqlParameter("@In_Lastname", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "Y"),
                new SqlParameter("@in_CardExpiryMonth", null),
                new SqlParameter("@in_CardExpiryYear", null),
                new SqlParameter("@in_BillingAddress_1", null),
                new SqlParameter("@in_BillingAddress_2", null),
                new SqlParameter("@in_BillingCity_ID", null),
                new SqlParameter("@in_BillingState_ID", null),
                new SqlParameter("@in_BillingCountry_ID", null),
                new SqlParameter("@in_BillingZIP", null),
                new SqlParameter("@in_ParentTransaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", 0),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_Practice_ID", null),
                new SqlParameter("@In_RefpatientLogin_ID", null),
                 new SqlParameter("@in_paypaltransactionid", (newRefund.id!= null ? newRefund.id : null)),
                    new SqlParameter("@in_paypalsaletransactionid", (obj.paypalsaleid != null? obj.paypalsaleid : null))
            };
                foreach (SqlParameter param in paramlist)
                {
                    cmd.Parameters.Add(param);
                }
                objread = cmd.ExecuteReader();

                if (objread.Read())
                {
                    Loc_TransactionID = Convert.ToString(objread["TransactionID"]);
                }
                else
                {
                    Loc_TransactionID = null;
                }
                cnn11.Close();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "Help_dbo.st_CreditCard_TransactionResponse";
                cmd1.Connection = cnn12;
                cnn12.Open();
                IDataParameter[] paramlists = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                new SqlParameter("@in_Status", newRefund.state),
                new SqlParameter("@in_Response_Data", response),
                new SqlParameter("in_Status_Code", "1"),
                new SqlParameter("@in_CreatedBy", 1),
                new SqlParameter("@in_GatewayDetail_ID", null),
                new SqlParameter("@In_IsPayPalProcess_Ind", "Y")
            };
                foreach (SqlParameter param in paramlists)
                {
                    cmd1.Parameters.Add(param);
                }

                cmd1.ExecuteNonQuery();
                cnn12.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "Help_dbo.st_CreditCard_UPD_RefundTransaction";
                cmd3.Connection = cnn13;
                cnn13.Open();
                IDataParameter[] paramlist2 = {
                new SqlParameter("@in_Transaction_ID", obj.Transactionno),
                new SqlParameter("@in_IsRefundable_Ind", "N"),
                new SqlParameter("@in_UpdatedBy", "1"),
                new SqlParameter("@in_CCTransaction_ID", Loc_TransactionID),
                new SqlParameter("@in_ReferencetypeID", "2")
            };
                foreach (SqlParameter param in paramlist2)
                {
                    cmd3.Parameters.Add(param);
                }
                cmd3.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Help_dbo.st_CreditCard_UPD_ReRefundTransaction";
                cmd2.Connection = cnn13;
                IDataParameter[] paramlists1 = {
                new SqlParameter("@in_Transaction_ID", Loc_TransactionID),
                new SqlParameter("@in_IsRefundable_Ind", "R"),
                new SqlParameter("@in_UpdatedBy", "1")
            };
                foreach (SqlParameter param in paramlists1)
                {
                    cmd2.Parameters.Add(param);
                }
                cmd2.ExecuteNonQuery();
                cnn13.Close();






                RPBilling adminNewAdj = new RPBilling();
                // string currentdt = null;
                // currentdt = DateTime.Now.ToShortDateString();
                adminNewAdj.fromReferenceId = 1;
                adminNewAdj.ReferenceType_ID = "1";
                adminNewAdj.ToReferenceType_ID = "2";
                adminNewAdj.ToReference_ID = obj.ToReference_ID;
                adminNewAdj.BillingService_ID = null;
                adminNewAdj.Transaction_Date = DateTime.Now.ToShortDateString();
                adminNewAdj.Transaction_Amount = obj.Refund_amount;
                adminNewAdj.TransactionType_ID = "3";
                adminNewAdj.AdjustmentType_ID = 5;
                adminNewAdj.Notes = null;
                adminNewAdj.Practice_ID = null;
                if (Request["hdnPaymentTransID"] != "" && Request["hdnPaymentTransID"] != null)
                {
                    adminNewAdj.RefTransaction_ID = Convert.ToInt32(Request["hdnPaymentTransID"]);
                }
                else
                {
                    adminNewAdj.RefTransaction_ID = null;
                }
                adminNewAdj.UserId = Convert.ToString(Session["UserID"]);
                adminNewAdj.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));

                RPBilling adminNewAdj1 = new RPBilling();
                adminNewAdj1.fromReferenceId = 1;
                adminNewAdj1.ReferenceType_ID = "1";
                adminNewAdj1.ToReferenceType_ID = "2";
                adminNewAdj1.ToReference_ID = obj.ToReference_ID;
                adminNewAdj1.BillingService_ID = null;
                adminNewAdj1.Transaction_Date = DateTime.Now.ToShortDateString();
                adminNewAdj1.Transaction_Amount = obj.Refund_amount;
                adminNewAdj1.TransactionType_ID = "4";
                adminNewAdj1.AdjustmentType_ID = 5;
                adminNewAdj1.Notes = null;
                adminNewAdj1.Practice_ID = null;
                if (Request["hdnPaymentTransID"] != "" && Request["hdnPaymentTransID"] != null)
                {
                    adminNewAdj1.RefTransaction_ID = Convert.ToInt32(Request["hdnPaymentTransID"]);
                }
                else
                {
                    adminNewAdj1.RefTransaction_ID = null;
                }
                adminNewAdj1.UserId = Convert.ToString(Session["UserID"]);
                adminNewAdj1.MakeAdminAdjTransaction(adminNewAdj, Convert.ToInt32(Session["Loginhistory_ID"]));
                string msg = "1";
                return Json(msg);

            }
            else
            {
                string msg = "2";
                return Json(msg);
            }
        }
        private void LoadCreditCardInfo(int tranid, int? PaymentID)
        {
            try
            {
                clsCommonFunctions ObjCommFun = new clsCommonFunctions();
                SqlParameter[] paramlist = {
			new SqlParameter("@In_Transaction_ID", tranid),
			new SqlParameter("@in_AdminTran_id", (PaymentID!= 0 ? PaymentID : null))
		};
                ObjCommFun.AddInParameters(paramlist);
                dreaderType = ObjCommFun.GetDataReader("Help_dbo.st_CreditCard_Get_PatientTransactionInfo");
                if (dreaderType.Read())
                {
                    if (dreaderType["Tran_Date"] != null)
                    {
                        string strdate = Convert.ToString(dreaderType["Tran_Date"]);
                        string[] Tran_Date = strdate.Split(' ');
                        ViewBag.lblTransDate = Tran_Date[0];
                    }
                    else
                    {
                        ViewBag.lblTransDate = null;
                    }
                    //if (!Information.IsDBNull(dreaderType("FromReference_ID")))
                    //{
                    //    FromReference_ID = dreaderType("FromReference_ID");
                    //}
                    if (dreaderType["ToReference_ID"] != null)
                    {
                        ViewBag.ToReference_ID = dreaderType["ToReference_ID"];
                    }
                    //if (!Information.IsDBNull(dreaderType("FromReferencetype_ID")))
                    //{
                    //    FromReferencetype_ID = dreaderType("FromReferencetype_ID");
                    //}
                    //if (!Information.IsDBNull(dreaderType("ToReferencetype_ID")))
                    //{
                    //    ToReferencetype_ID = dreaderType("ToReferencetype_ID");
                    //}
                    //if (!Information.IsDBNull(dreaderType("Practice_ID")))
                    //{
                    //    ViewState("PracticeID") = dreaderType("Practice_ID");
                    //}
                    //else
                    //{
                    //    ViewState("PracticeID") = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("Provider_ID")))
                    //{
                    //    ViewState("Provider_ID") = dreaderType("Provider_ID");
                    //}
                    //if (!Information.IsDBNull(dreaderType("CreditCardNo")))
                    //{
                    //    GetCCNumber = objEncryption.GetDecryptString(dreaderType("CreditCardNo"));
                    //    StrEcryCCNumber = Strings.Right(GetCCNumber, 4);
                    //    lblCCNo.Text = "************" + StrEcryCCNumber;
                    //}
                    //else
                    //{
                    //    GetCCNumber = null;
                    //}
                    if (dreaderType["CreditCardType_ID"] != null)
                    {
                        if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "1")
                        {
                            ViewBag.lblCardType = "MasterCard";
                        }
                        else if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "2")
                        {
                            ViewBag.lblCardType = "VisaCard";
                        }
                        else if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "3")
                        {
                            ViewBag.lblCardType = "American Express";
                        }
                        else if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "4")
                        {
                            ViewBag.lblCardType = "Discover";
                        }
                        else if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "5")
                        {
                            ViewBag.lblCardType = "enRoute";
                        }
                        else if (Convert.ToString(dreaderType["CreditCardType_ID"]) == "6")
                        {
                            ViewBag.lblCardType = "JCB";
                        }
                    }
                    if (dreaderType["FromUserName"] != null)
                    {
                        ViewBag.lblPatientName = dreaderType["FromUserName"];
                    }
                    else
                    {
                        ViewBag.lblPatientName = null;

                    }

                    if (dreaderType["CardHolderName"] != null)
                    {
                        ViewBag.name = dreaderType["CardHolderName"];
                    }
                    else
                    {
                        ViewBag.name = null;
                    }
                    if (dreaderType["To_Emailid"].ToString() != "")
                    {
                        ViewBag.Tomailid = dreaderType["To_Emailid"];
                    }
                    else
                    {
                        ViewBag.Tomailid = null;
                    }
                    string ExpYear = null;
                    string StrExpMon = null;
                    if (dreaderType["CardExpiryMonth"] != null)
                    {
                        StrExpMon = dreaderType["CardExpiryMonth"].ToString();
                    }
                    else
                    {
                        StrExpMon = null;
                    }
                    if (dreaderType["CardExpiryYear"] != null)
                    {
                        ExpYear = dreaderType["CardExpiryYear"].ToString();
                    }
                    else
                    {
                        ExpYear = null;
                    }

                    ViewBag.lblExpiredDate = StrExpMon + "/" + ExpYear;
                    //if (!string.IsNullOrEmpty(lblAmount.Text))
                    //{
                    if (dreaderType["Amount"] != null)
                    {
                        //string stramount = Strings.FormatCurrency(dreaderType["Amount"]);
                        double stramount = Convert.ToDouble(dreaderType["Amount"]);
                        ViewBag.lblAmount = stramount;
                        //if (stramount.Contains("$"))
                        //{
                        //    ViewBag.lblAmount = stramount.Remove(0, 1);
                        //}
                    }
                    else
                    {
                        ViewBag.lblAmount = null;
                    }
                    if (dreaderType["paypalsaletransactionid"] != null)
                    {
                        ViewBag.paypalsaleid = dreaderType["paypalsaletransactionid"];
                    }
                    //else
                    //{
                    //}
                    //}
                    //if (!Information.IsDBNull(dreaderType("TotalAmount")))
                    //{
                    //    ViewState("TotalAmount") = dreaderType("TotalAmount");
                    //}
                    //else
                    //{
                    //    ViewState("TotalAmount") = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("CVV2")))
                    //{
                    //    cvvno = objEncryption.GetDecryptString(dreaderType("CVV2"));
                    //}
                    //else
                    //{
                    //    cvvno = null;
                    //}

                    //if (!Information.IsDBNull(dreaderType("InvoiceNum")))
                    //{
                    //    x_invoice_num = dreaderType("InvoiceNum");
                    //}
                    //else
                    //{
                    //    x_invoice_num = null;
                    //}

                    //if (!Information.IsDBNull(dreaderType("BillingAddress_1")))
                    //{
                    //    x_address1 = dreaderType("BillingAddress_1");
                    //    x_address1 = Strings.Replace(x_address1, "#", "");
                    //}
                    //else
                    //{
                    //    x_address1 = "";
                    //}
                    //if (!Information.IsDBNull(dreaderType("BillingAddress_2")))
                    //{
                    //    x_address2 = dreaderType("BillingAddress_2");
                    //    x_address2 = Strings.Replace(x_address2, "#", "");
                    //}
                    //else
                    //{
                    //    x_address2 = null;
                    //}

                    //if (!Information.IsDBNull(dreaderType("BillingCity_ID")))
                    //{
                    //    BillingCity_ID = dreaderType("BillingCity_ID");
                    //}
                    //else
                    //{
                    //    BillingCity_ID = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("BillingState_ID")))
                    //{
                    //    BillingState_ID = dreaderType("BillingState_ID");
                    //}
                    //else
                    //{
                    //    BillingState_ID = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("BillingZIP")))
                    //{
                    //    x_zip = dreaderType("BillingZIP");
                    //}
                    //else
                    //{
                    //    x_zip = "";
                    //}
                    //if (!Information.IsDBNull(dreaderType("City")))
                    //{
                    //    x_city = dreaderType("City");
                    //}
                    //else
                    //{
                    //    x_city = "";
                    //}
                    //if (!Information.IsDBNull(dreaderType("State")))
                    //{
                    //    x_state = dreaderType("State");
                    //}
                    //else
                    //{
                    //    x_state = "";
                    //}
                    //if (!Information.IsDBNull(dreaderType("Country")))
                    //{
                    //    x_country = dreaderType("Country");
                    //}
                    //else
                    //{
                    //    x_country = "";

                    //}
                    //if (Label4.Text != null)
                    //{
                    //    x_first_name = Label4.Text;
                    //}
                    //else
                    //{
                    //    x_first_name = "";
                    //}
                    //x_last_name = "";

                    //if (!string.IsNullOrEmpty(x_address2))
                    //{
                    //    x_address = x_address1 + "," + x_address2;
                    //}
                    //else
                    //{
                    //    x_address = x_address1;
                    //}
                    //if (!Information.IsDBNull(dreaderType("GatewayDetail_ID")))
                    //{
                    //    x_trans_id = dreaderType("GatewayDetail_ID");
                    //}
                    //else
                    //{
                    //    x_trans_id = "";
                    //}
                    //if (!Information.IsDBNull(dreaderType("IsPayPalProcess_Ind")))
                    //{
                    //    strIsPaypalInd = dreaderType("IsPayPalProcess_Ind");
                    //}
                    //else
                    //{
                    //    strIsPaypalInd = "N";
                    //}
                    //if (!Information.IsDBNull(dreaderType("Firstname")))
                    //{
                    //    ViewState("Firstname") = dreaderType("Firstname");
                    //}
                    //else
                    //{
                    //    ViewState("Firstname") = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("Lastname")))
                    //{
                    //    ViewState("Lastname") = dreaderType("Lastname");
                    //}
                    //else
                    //{
                    //    ViewState("Lastname") = null;
                    //}
                    //if (!Information.IsDBNull(dreaderType("BillingService_ID")))
                    //{
                    //    ViewState("BillingService_ID") = dreaderType("BillingService_ID");
                    //}
                    //else
                    //{
                    //    ViewState("BillingService_ID") = null;
                    //}
                }
            }
            catch (Exception ex)
            {
                var Expobj = new clsExceptionLog();
                Expobj.LogException(ex, "CREDITCARDController", "LoadCreditCardInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
    }
}
