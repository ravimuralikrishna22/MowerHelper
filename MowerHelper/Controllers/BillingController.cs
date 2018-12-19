using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using aspPDF;
using Microsoft.Security.Application;
using Obout.Mvc.ComboBox;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class BillingController : Controller
    {      
        EASYPDF pdf = new EASYPDF();
        //clsWebConfigsettings obj = new clsWebConfigsettings();
        //#region "ClaimInformation"
        //public ActionResult ClaimInformation()
        //{
        //    return View();
        //}
        //#endregion
        #region "PatientTransactions from Tools module"
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult TransactionDisplay(string ddlrecords, string dt_filter, string page, string hdnPid, int? hdnPlId, string fdate, string tdate, int? type_id = null, string clientphone = null, string hdnclientEmail=null)
        {
            //Session["Msgitem"] = null;

            //RPBilling objBill = new RPBilling();
            if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("../Schedule/Schedulespecs");
            }
            //if (!string.IsNullOrEmpty(clientphone))
            //{
            //    objBill.clientphone = clientphone;
            //}
            //string strDate = DateTime.Now.ToShortDateString();
            //objBill.FromDate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
            //objBill.ToDate = strDate;
            //objBill.Date_range = "30";
            //ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
            ////objBill.Practice_ID = Convert.ToInt32(Session["Prov_ID"]);
            //objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            ////ViewBag.selectedPat = hdnPid;
            //objBill.PatientID = hdnPid;
            ////FillPatientsmain();
            //if (hdnPlId != null)
            //{
            //    objBill.PatientLogin_ID = hdnPlId;

            //}
            //else
            //{
            //    objBill.PatientLogin_ID = null;
            //}
            //objBill.Reference_id = null;
            //DataSet dsbill = new DataSet();
            //dsbill = RPBilling.PracticePatientIncome(objBill);
            //StringBuilder strbill = new StringBuilder();
            //strbill = strbill.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            //strbill = strbill.Append("<td align='center'><strong>Total charges</strong></td><td align='center'><strong>Total payments</strong></td><td align='center'><strong>Total credits</strong></td><td align='center'><strong>Balance</strong></td></tr>");
            //foreach (DataRow dr in dsbill.Tables[0].Rows)
            //{
            //    string StrTCharge = string.Format("{0:c}", dr["TotalCharges"]);
            //    string strTPay = string.Format("{0:c}", dr["Netpayments"]);
            //    string strBal = string.Format("{0:c}", dr["Balance"]);
            //    double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
            //    string strCredit = string.Format("{0:c}", dblcredit);
            //    strbill = strbill.Append("<tr><td align='center'>" + StrTCharge + " </td><td align='center'>" + strTPay + "</td><td align='center'>" + strCredit + "</td><td align='center'>" + strBal + "</td></tr>");
            //}
            //strbill = strbill.Append("</table>");
            //ViewBag.PatBillsum = strbill.ToString();
            //if (!string.IsNullOrEmpty(hdnPid))
            //{
            //    int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
            //    DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
            //    StringBuilder strdays = new StringBuilder();
            //    strdays = strdays.Append("<table width='100%' cellpadding='11' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            //    strdays = strdays.Append("<td align='right' width='22%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0-30</strong></td><td align='center' width='10%'><strong>31-60</strong></td><td align='center' width='10%'><strong>61-90</strong></td><td align='center' width='10%'><strong>91-120</strong></td><td align='center' width='20%'><strong>Over 120 days</strong></td><td align='center' width='18%'><strong>Total due</strong></td></tr>");
            //    if (dsdayout.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
            //        {
            //            string str0_30 = string.Empty;
            //            string str31_60 = string.Empty;
            //            string str61_90 = string.Empty;
            //            string str91_120 = string.Empty;
            //            string strOver120 = string.Empty;
            //            double strAm30;
            //            double strAm60;
            //            double strAm90;
            //            double strAm120;
            //            double strAm150;
            //            if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
            //            {
            //                str0_30 = string.Format("{0:c}", dr1["0-30"]);
            //                strAm30 = Convert.ToDouble(dr1["0-30"]);
            //            }
            //            else
            //            {
            //                str0_30 = "$0.00";
            //                strAm30 = 0;

            //            }
            //            if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
            //            {
            //                str31_60 = string.Format("{0:c}", dr1["31-60"]);
            //                strAm60 = Convert.ToDouble(dr1["31-60"]);
            //            }
            //            else
            //            {
            //                str31_60 = "$0.00";
            //                strAm60 = 0;
            //            }
            //            if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
            //            {
            //                str61_90 = string.Format("{0:c}", dr1["61-90"]);
            //                strAm90 = Convert.ToDouble(dr1["61-90"]);
            //            }
            //            else
            //            {
            //                str61_90 = "$0.00";
            //                strAm90 = 0;
            //            }
            //            if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
            //            {
            //                str91_120 = string.Format("{0:c}", dr1["91-120"]);
            //                strAm120 = Convert.ToDouble(dr1["91-120"]);
            //            }
            //            else
            //            {
            //                str91_120 = "$0.00";
            //                strAm120 = 0;
            //            }
            //            if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
            //            {
            //                strOver120 = string.Format("{0:c}", dr1["over 120 days"]);
            //                strAm150 = Convert.ToDouble(dr1["over 120 days"]);
            //            }
            //            else
            //            {
            //                strOver120 = "$0.00";
            //                strAm150 = 0;
            //            }




            //            double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
            //            string amountdue = string.Format("{0:c}", amountdue1);
            //            ViewBag.BalanceDue = amountdue;
            //            strdays = strdays.Append("<tr><td height='31px' align='right' width='22%'><strong>Amount due&nbsp;:</strong></td><td align='center' width='10%'>" + str0_30 + "</td><td align='center' width='10%'>" + str31_60 + "</td><td align='center' width='10%'>" + str61_90 + "</td><td align='center' width='10%'>" + str91_120 + "</td><td align='center' width='20%'>" + strOver120 + "</td><td align='center' width='18%'>" + amountdue + "</td></tr>");

            //        }
            //    }
            //    else
            //    {
            //    }
            //    strdays = strdays.Append("</table>");
            //    ViewBag.DaysOut = Convert.ToString(strdays); ;
            //}
            //else
            //{

            //    StringBuilder strDayout = new StringBuilder();
            //    strDayout = strDayout.Append("<table width='100%' cellpadding='12' cellspacing='1' border='0' class='border_style' id='tblPracticeDaysByoutstanding' runat='server'><tr class='tr_bgcolor'>");
            //    strDayout = strDayout.Append("<td align='right' width='22%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0 - 30</strong></td><td align='center' width='10%'><strong>31 - 60</strong></td>");
            //    strDayout = strDayout.Append("<td align='center' width='10%'><strong>61 - 90</strong></td><td align='center' width='13%'><strong>91 - 120</strong> </td>");
            //    strDayout = strDayout.Append("<td align='center' width='17%'><strong>Over 120 days</strong> </td><td align='center' width='18%'><strong>Total due</strong></td></tr>");
            //    DataSet dsRespBill = new DataSet();
            //    //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

            //    objBill.PatientLogin_ID = null;

            //    objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
            //    Session["firsttrdt"] = dsRespBill.Tables[1].Rows[0]["Firsttransactiondate"];
            //    foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
            //    {
            //        string str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
            //        string str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
            //        string str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
            //        string str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
            //        string strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
            //        string strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

            //        strDayout = strDayout.Append("<tr><td height='31px' width='22px' align='right'><strong>Amount due&nbsp;:</strong></td>");
            //        if (str0_30 != "$0.00")
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(1)' >" + str0_30 + "</a> </td>");
            //        }
            //        else
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str0_30 + "</span> </td>");
            //        }
            //        if (str31_60 != "$0.00")
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(2)' >" + str31_60 + "</a> </td>");
            //        }
            //        else
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str31_60 + "</span></td>");
            //        }
            //        if (str61_90 != "$0.00")
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(3)' >" + str61_90 + "</a> </td>");
            //        }
            //        else
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str61_90 + "</span></a></td>");
            //        }
            //        if (str90_120 != "$0.00")
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(4)' >" + str90_120 + "</a> </td>");
            //        }
            //        else
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str90_120 + "</span></td>");
            //        }
            //        if (strOver120 != "$0.00")
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(5)' >" + strOver120 + "</a> </td>");
            //        }
            //        else
            //        {
            //            strDayout = strDayout.Append("<td align='center' width='20%'><span>" + strOver120 + "</span></td>");
            //        }
            //        strDayout = strDayout.Append("<td align='center' width='18%'>" + strAmountDue + "</td></tr>");
                    
                    
                
            //    }
            //    strDayout = strDayout.Append("</table>");
            //    strDayout = strDayout.Append("<div id='divDays0_30' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
            //    strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
            //    RPBilling day030 = new RPBilling();
            //    day030.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    // day030.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //    DataSet ds030 = new DataSet();
            //    ds030 = day030.patientwiseDaysoutstanding(day030, 1);
            //    if (ds030.Tables[0].Rows.Count > 0)
            //    {
            //        int i = 1;
            //        string tclass = "white_color";
            //        foreach (DataRow dr in ds030.Tables[0].Rows)
            //        {
            //            if ((i % 2) == 0)
            //            {
            //                tclass = "nav_blue_color";
            //            }
            //            else
            //            {
            //                tclass = "white_color";
            //            }

            //            strDayout = strDayout.Append("<tr class='" + tclass + "'><td width='50%' align='center'>" + dr["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr["Amount"]) + "</td></tr>");
            //            i++;
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.day030 = "N";
            //    }
            //    strDayout = strDayout.Append("</table></div>");
            //    strDayout = strDayout.Append("<div id='divDays30_60' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
            //    strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
            //    RPBilling day3060 = new RPBilling();
            //    day3060.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    //day3060.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //    DataSet ds3060 = new DataSet();
            //    ds3060 = day3060.patientwiseDaysoutstanding(day3060, 2);
            //    if (ds3060.Tables[0].Rows.Count > 0)
            //    {
            //        int j = 1;
            //        string t2class = "white_color";
            //        foreach (DataRow dr2 in ds3060.Tables[0].Rows)
            //        {
            //            if ((j % 2) == 0)
            //            {
            //                t2class = "nav_blue_color";
            //            }
            //            else
            //            {
            //                t2class = "white_color";
            //            }

            //            strDayout = strDayout.Append("<tr class='" + t2class + "'><td width='50%' align='center'>" + dr2["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr2["Amount"]) + "</td></tr>");
            //            j++;
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.day3060 = "N";
            //    }
            //    strDayout = strDayout.Append("</table></div>");
            //    strDayout = strDayout.Append("<div id='divDays60_90' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
            //    strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
            //    RPBilling day6090 = new RPBilling();
            //    day6090.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    //day6090.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //    DataSet ds6090 = new DataSet();
            //    ds6090 = day6090.patientwiseDaysoutstanding(day6090, 3);
            //    if (ds6090.Tables[0].Rows.Count > 0)
            //    {
            //        int k = 1;
            //        string t3class = "white_color";
            //        foreach (DataRow dr3 in ds6090.Tables[0].Rows)
            //        {
            //            if ((k % 2) == 0)
            //            {
            //                t3class = "nav_blue_color";
            //            }
            //            else
            //            {
            //                t3class = "white_color";
            //            }

            //            strDayout = strDayout.Append("<tr class='" + t3class + "'><td width='50%' align='center'>" + dr3["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr3["Amount"]) + "</td></tr>");
            //            k++;
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.day6090 = "N";
            //    }
            //    strDayout = strDayout.Append("</table></div>");
            //    strDayout = strDayout.Append("<div id='divDays90_120' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
            //    strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
            //    RPBilling day90120 = new RPBilling();
            //    day90120.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    // day90120.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //    DataSet ds90120 = new DataSet();
            //    ds90120 = day90120.patientwiseDaysoutstanding(day90120, 4);
            //    if (ds90120.Tables[0].Rows.Count > 0)
            //    {
            //        int l = 1;
            //        string t4class = "white_color";
            //        foreach (DataRow dr4 in ds90120.Tables[0].Rows)
            //        {
            //            if ((l % 2) == 0)
            //            {
            //                t4class = "nav_blue_color";
            //            }
            //            else
            //            {
            //                t4class = "white_color";
            //            }

            //            strDayout = strDayout.Append("<tr class='" + t4class + "'><td width='50%' align='center'>" + dr4["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr4["Amount"]) + "</td></tr>");
            //            l++;
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.day90120 = "N";
            //    }
            //    strDayout = strDayout.Append("</table></div>");
            //    strDayout = strDayout.Append("<div id='divDaysOver120' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
            //    strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
            //    RPBilling dayOver120 = new RPBilling();
            //    dayOver120.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
            //    //dayOver120.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //    DataSet dsOver120 = new DataSet();
            //    dsOver120 = dayOver120.patientwiseDaysoutstanding(dayOver120, 5);
            //    if (dsOver120.Tables[0].Rows.Count > 0)
            //    {
            //        int m = 1;
            //        string t5class = "white_color";
            //        foreach (DataRow dr5 in dsOver120.Tables[0].Rows)
            //        {
            //            if ((m % 2) == 0)
            //            {
            //                t5class = "nav_blue_color";
            //            }
            //            else
            //            {
            //                t5class = "white_color";
            //            }

            //            strDayout = strDayout.Append("<tr class='" + t5class + "'><td width='50%' align='center'>" + dr5["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr5["Amount"]) + "</td></tr>");
            //            m++;
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.dayOver120 = "N";
            //    }

            //    strDayout = strDayout.Append("</table></div>");
            //    ViewBag.DaysOut = Convert.ToString(strDayout);
            //}
            //dynamic chglistgrid = getChglist(pgesize, hdnPlId, objBill.FromDate, objBill.ToDate);
            return View();
          
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DisplayTransactions(RPBilling objBill)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //System.Threading.Thread.Sleep(1000);
                objBill.PatientID = objBill.PatientID == "null" ? null : objBill.PatientID;
                if (!string.IsNullOrEmpty(objBill.NoOfRecords))
                {
                    Session["Rowsperpage"] = objBill.NoOfRecords;
                }
                else if (Session["Rowsperpage"] != null)
                {
                    objBill.NoOfRecords = Session["Rowsperpage"].ToString();
                }
                else
                {
                    objBill.NoOfRecords = "10";
                }
                if (objBill.Date_range == "7")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    objBill.FromDate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
                    objBill.ToDate = strDate;
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + strDate;
                }
                else if (objBill.Date_range == "30")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    objBill.FromDate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                    objBill.ToDate = strDate;
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + strDate;
                }
                else if (objBill.Date_range == "All")
                {
                    objBill.FromDate = null;
                    objBill.ToDate = null;
                }
                else if (objBill.Date_range == "Custom")
                {
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                }
                else if (objBill.Date_range == "Today")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    objBill.FromDate = strDate;
                    objBill.ToDate = strDate;
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;

                }
                else if (objBill.Date_range == "last15days")
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    objBill.FromDate = DateTime.Parse(strDate).AddDays(-14).ToShortDateString();
                    objBill.ToDate = strDate;
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                }
                else
                {
                    string strDate = DateTime.Now.ToShortDateString();
                    objBill.FromDate = DateTime.Parse(strDate).AddDays(-29).ToShortDateString();
                    objBill.ToDate = strDate;
                    objBill.Date_range = "30";
                    ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                }
                objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                ViewBag.selectedPat = objBill.PatientID;
                FillPatientsmain();
                if (!string.IsNullOrEmpty(objBill.PatientID))
                {
                    int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
                    DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
                    StringBuilder strdays = new StringBuilder();
                    strdays = strdays.Append("<table width='100%' cellpadding='11' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
                    strdays = strdays.Append("<td align='right' width='22%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0-30</strong></td><td align='center' width='10%'><strong>31-60</strong></td><td align='center' width='10%'><strong>61-90</strong></td><td align='center' width='10%'><strong>91-120</strong></td><td align='center' width='20%'><strong>Over 120 days</strong></td><td align='center' width='18%'><strong>Total due</strong></td></tr>");
                    if (dsdayout.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
                        {
                            string str0_30 = string.Empty;
                            string str31_60 = string.Empty;
                            string str61_90 = string.Empty;
                            string str91_120 = string.Empty;
                            string strOver120 = string.Empty;
                            double strAm30;
                            double strAm60;
                            double strAm90;
                            double strAm120;
                            double strAm150;
                            if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
                            {
                                str0_30 = Convert.ToDouble(dr1["0-30"]) > 0 ? string.Format("{0:c}", dr1["0-30"]) : "$0.00";
                                strAm30 = Convert.ToDouble(dr1["0-30"]);
                            }
                            else
                            {
                                str0_30 = "$0.00";
                                strAm30 = 0;

                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
                            {
                                str31_60 = Convert.ToDouble(dr1["31-60"]) > 0 ? string.Format("{0:c}", dr1["31-60"]) : "$0.00";
                                strAm60 = Convert.ToDouble(dr1["31-60"]);
                            }
                            else
                            {
                                str31_60 = "$0.00";
                                strAm60 = 0;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
                            {
                                str61_90 = Convert.ToDouble(dr1["61-90"]) > 0 ? string.Format("{0:c}", dr1["61-90"]) : "$0.00";
                                strAm90 = Convert.ToDouble(dr1["61-90"]);
                            }
                            else
                            {
                                str61_90 = "$0.00";
                                strAm90 = 0;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
                            {
                                str91_120 =Convert.ToDouble(dr1["91-120"])>0?string.Format("{0:c}", dr1["91-120"]):"$0.00";
                                strAm120 = Convert.ToDouble(dr1["91-120"]);
                            }
                            else
                            {
                                str91_120 = "$0.00";
                                strAm120 = 0;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
                            {
                                strOver120 = Convert.ToDouble(dr1["over 120 days"])>0?string.Format("{0:c}", dr1["over 120 days"]):"$0:00";
                                strAm150 = Convert.ToDouble(dr1["over 120 days"]);
                            }
                            else
                            {
                                strOver120 = "$0.00";
                                strAm150 = 0;
                            }




                            double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
                            string amountdue = string.Format("{0:c}", amountdue1);
                            ViewBag.BalanceDue = amountdue;
                            strdays = strdays.Append("<tr><td height='31px' align='right' width='22%'><strong>Amount due&nbsp;:</strong></td><td align='center' width='10%'>" + str0_30 + "</td><td align='center' width='10%'>" + str31_60 + "</td><td align='center' width='10%'>" + str61_90 + "</td><td align='center' width='10%'>" + str91_120 + "</td><td align='center' width='20%'>" + strOver120 + "</td><td align='center' width='18%'>" + amountdue + "</td></tr>");

                        }
                    }
                    //else
                    //{
                    //}
                    strdays = strdays.Append("</table>");
                    ViewBag.DaysOut = Convert.ToString(strdays); ;
                }
                else
                {

                    StringBuilder strDayout = new StringBuilder();
                    strDayout = strDayout.Append("<table width='100%' cellpadding='12' cellspacing='1' border='0' class='border_style' id='tblPracticeDaysByoutstanding' runat='server'><tr class='tr_bgcolor'>");
                    strDayout = strDayout.Append("<td align='right' width='22%'><strong>Days outstanding&nbsp;:</strong></td><td align='center' width='10%'><strong>0 - 30</strong></td><td align='center' width='10%'><strong>31 - 60</strong></td>");
                    strDayout = strDayout.Append("<td align='center' width='10%'><strong>61 - 90</strong></td><td align='center' width='13%'><strong>91 - 120</strong> </td>");
                    strDayout = strDayout.Append("<td align='center' width='17%'><strong>Over 120 days</strong> </td><td align='center' width='18%'><strong>Total due</strong></td></tr>");
                    DataSet dsRespBill = new DataSet();
                    //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

                    objBill.PatientLogin_ID = null;

                    objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                    dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
                    if (dsRespBill.Tables[1].Rows.Count > 0)
                    {
                        Session["firsttrdt"] = dsRespBill.Tables[1].Rows[0]["Firsttransactiondate"];
                    }
                    string str0_30 = null, str31_60 = null, str61_90 = null, str90_120 = null, strOver120 = null, strAmountDue = null;
                    if (dsRespBill.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
                        {
                             str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
                             str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
                             str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
                             str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
                             strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
                             strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

                            strDayout = strDayout.Append("<tr><td height='31px' width='22px' align='right'><strong>Amount due&nbsp;:</strong></td>");
                            if (str0_30 != "$0.00")
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk030' href='#' onclick='return lnkTypeSearch(1)' >" + str0_30 + "</a> </td>");
                            }
                            else
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str0_30 + "</span> </td>");
                            }
                            if (str31_60 != "$0.00")
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk3060' href='#' onclick='return lnkTypeSearch(2)' >" + str31_60 + "</a> </td>");
                            }
                            else
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str31_60 + "</span></td>");
                            }
                            if (str61_90 != "$0.00")
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk6090' href='#' onclick='return lnkTypeSearch(3)' >" + str61_90 + "</a> </td>");
                            }
                            else
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str61_90 + "</span></a></td>");
                            }
                            if (str90_120 != "$0.00")
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnk90120' href='#' onclick='return lnkTypeSearch(4)' >" + str90_120 + "</a> </td>");
                            }
                            else
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><span>" + str90_120 + "</span></td>");
                            }
                            if (strOver120 != "$0.00")
                            {
                                strDayout = strDayout.Append("<td align='center' width='10%'><a id='lnkOver120' href='#' onclick='return lnkTypeSearch(5)' >" + strOver120 + "</a> </td>");
                            }
                            else
                            {
                                strDayout = strDayout.Append("<td align='center' width='20%'><span>" + strOver120 + "</span></td>");
                            }
                            strDayout = strDayout.Append("<td align='center' width='18%'>" + strAmountDue + "</td></tr>");



                        }
                    }
                    strDayout = strDayout.Append("</table>");
                    RPBilling day030 = new RPBilling();
                    day030.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                    if (str0_30 != "$0.00")
                    {
                        strDayout = strDayout.Append("<div id='divDays0_30' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
                        strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");

                        // day030.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                        DataSet ds030 = new DataSet();
                        ds030 = day030.patientwiseDaysoutstanding(day030, 1);
                        if (ds030.Tables[0].Rows.Count > 0)
                        {
                            int i = 1;
                            string tclass = "white_color";
                            foreach (DataRow dr in ds030.Tables[0].Rows)
                            {
                                if ((i % 2) == 0)
                                {
                                    tclass = "nav_blue_color";
                                }
                                else
                                {
                                    tclass = "white_color";
                                }

                                strDayout = strDayout.Append("<tr class='" + tclass + "'><td width='50%' align='center'>" + dr["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr["Amount"]) + "</td></tr>");
                                i++;
                            }
                        }
                        else
                        {
                            ViewBag.day030 = "N";
                        }
                        strDayout = strDayout.Append("</table></div>");
                    }
                    if (str31_60 != "$0.00")
                    {
                        strDayout = strDayout.Append("<div id='divDays30_60' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
                        strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
                        //RPBilling day3060 = new RPBilling();
                        // day3060.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                        //day3060.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                        DataSet ds3060 = new DataSet();
                        ds3060 = day030.patientwiseDaysoutstanding(day030, 2);
                        if (ds3060.Tables[0].Rows.Count > 0)
                        {
                            int j = 1;
                            string t2class = "white_color";
                            foreach (DataRow dr2 in ds3060.Tables[0].Rows)
                            {
                                if ((j % 2) == 0)
                                {
                                    t2class = "nav_blue_color";
                                }
                                else
                                {
                                    t2class = "white_color";
                                }

                                strDayout = strDayout.Append("<tr class='" + t2class + "'><td width='50%' align='center'>" + dr2["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr2["Amount"]) + "</td></tr>");
                                j++;
                            }
                        }
                        else
                        {
                            ViewBag.day3060 = "N";
                        }
                        strDayout = strDayout.Append("</table></div>");
                    }
                    if (str61_90 != "$0.00")
                    {
                        strDayout = strDayout.Append("<div id='divDays60_90' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
                        strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
                        // RPBilling day6090 = new RPBilling();
                        // day6090.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                        //day6090.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                        DataSet ds6090 = new DataSet();
                        ds6090 = day030.patientwiseDaysoutstanding(day030, 3);
                        if (ds6090.Tables[0].Rows.Count > 0)
                        {
                            int k = 1;
                            string t3class = "white_color";
                            foreach (DataRow dr3 in ds6090.Tables[0].Rows)
                            {
                                if ((k % 2) == 0)
                                {
                                    t3class = "nav_blue_color";
                                }
                                else
                                {
                                    t3class = "white_color";
                                }

                                strDayout = strDayout.Append("<tr class='" + t3class + "'><td width='50%' align='center'>" + dr3["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr3["Amount"]) + "</td></tr>");
                                k++;
                            }
                        }
                        else
                        {
                            ViewBag.day6090 = "N";
                        }
                        strDayout = strDayout.Append("</table></div>");
                    }
                    if (str90_120 != "$0.00")
                    {
                        strDayout = strDayout.Append("<div id='divDays90_120' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
                        strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
                        // RPBilling day90120 = new RPBilling();
                        //  day90120.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                        // day90120.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                        DataSet ds90120 = new DataSet();
                        ds90120 = day030.patientwiseDaysoutstanding(day030, 4);
                        if (ds90120.Tables[0].Rows.Count > 0)
                        {
                            int l = 1;
                            string t4class = "white_color";
                            foreach (DataRow dr4 in ds90120.Tables[0].Rows)
                            {
                                if ((l % 2) == 0)
                                {
                                    t4class = "nav_blue_color";
                                }
                                else
                                {
                                    t4class = "white_color";
                                }

                                strDayout = strDayout.Append("<tr class='" + t4class + "'><td width='50%' align='center'>" + dr4["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr4["Amount"]) + "</td></tr>");
                                l++;
                            }
                        }
                        else
                        {
                            ViewBag.day90120 = "N";
                        }
                        strDayout = strDayout.Append("</table></div>");
                    }
                    if (strOver120 != "$0.00")
                    {
                        strDayout = strDayout.Append("<div id='divDaysOver120' style='display:none;width:350px;'><table  border='0' cellpadding='4' cellspacing='1' width='100%' class='border_style'>");
                        strDayout = strDayout.Append("<tr class='tr_bgcolor'><td width='50%' align='center'>Client name</td><td width='50%' align='center'>Amount</td></tr>");
                        //RPBilling dayOver120 = new RPBilling();
                        // dayOver120.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                        //dayOver120.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                        DataSet dsOver120 = new DataSet();
                        dsOver120 = day030.patientwiseDaysoutstanding(day030, 5);
                        if (dsOver120.Tables[0].Rows.Count > 0)
                        {
                            int m = 1;
                            string t5class = "white_color";
                            foreach (DataRow dr5 in dsOver120.Tables[0].Rows)
                            {
                                if ((m % 2) == 0)
                                {
                                    t5class = "nav_blue_color";
                                }
                                else
                                {
                                    t5class = "white_color";
                                }

                                strDayout = strDayout.Append("<tr class='" + t5class + "'><td width='50%' align='center'>" + dr5["Patientname"] + "</td><td width='50%' align='center'>" + string.Format("{0:c}", dr5["Amount"]) + "</td></tr>");
                                m++;
                            }
                        }
                        else
                        {
                            ViewBag.dayOver120 = "N";
                        }

                        strDayout = strDayout.Append("</table></div>");
                    }
                    ViewBag.DaysOut =strDayout;
                }
                objBill.orderBy = Request["sortdir"] == null ? "DESC" : Request["sortdir"].ToString();
                objBill.OrderbyItem = Request["sort"] == null ? "Transaction_Date" : Request["sort"].ToString();
                if (objBill.type_id != null)
                {
                    objBill.BillingTransList = gettransactionlist1(objBill.type_id, objBill.NoOfRecords, objBill.PageNo);
                    string todaydate = null;
                    todaydate = DateTime.Now.ToShortDateString();
                    if (objBill.type_id == 1)
                    {
                        if (Convert.ToDateTime(Session["firsttrdt"]) < Convert.ToDateTime(DateTime.Parse(todaydate).AddDays(-29)))
                        {
                            objBill.FromDate = DateTime.Parse(todaydate).AddDays(-29).ToShortDateString();
                        }
                        else
                        {
                            objBill.FromDate = Session["firsttrdt"].ToString();
                        }
                        objBill.ToDate = todaydate;
                        objBill.Date_range = "30";
                        ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                    }
                    else if (objBill.type_id == 2)
                    {
                        if (Convert.ToDateTime(Session["firsttrdt"]) < Convert.ToDateTime(DateTime.Parse(todaydate).AddDays(-59)))
                        {
                            objBill.FromDate = DateTime.Parse(todaydate).AddDays(-59).ToShortDateString();
                        }
                        else
                        {
                            objBill.FromDate = Session["firsttrdt"].ToString();
                        }
                        objBill.ToDate = DateTime.Parse(todaydate).AddDays(-30).ToShortDateString();
                        objBill.Date_range = "Custom";
                        ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                    }
                    else if (objBill.type_id == 3)
                    {
                        if (Convert.ToDateTime(Session["firsttrdt"]) < Convert.ToDateTime(DateTime.Parse(todaydate).AddDays(-89)))
                        {
                            objBill.FromDate = DateTime.Parse(todaydate).AddDays(-89).ToShortDateString();
                        }
                        else
                        {
                            objBill.FromDate = Session["firsttrdt"].ToString();
                        }
                        objBill.ToDate = DateTime.Parse(todaydate).AddDays(-60).ToShortDateString();
                        objBill.Date_range = "Custom";
                        ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                    }
                    else if (objBill.type_id == 4)
                    {
                        if (Convert.ToDateTime(Session["firsttrdt"]) < Convert.ToDateTime(DateTime.Parse(todaydate).AddDays(-119)))
                        {
                            objBill.FromDate = DateTime.Parse(todaydate).AddDays(-119).ToShortDateString();
                        }
                        else
                        {
                            objBill.FromDate = Session["firsttrdt"].ToString();
                        }
                        objBill.ToDate = DateTime.Parse(todaydate).AddDays(-90).ToShortDateString();
                        objBill.Date_range = "Custom";
                        ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                    }
                    else if (objBill.type_id == 5)
                    {
                        if (Convert.ToDateTime(Session["firsttrdt"]) <= Convert.ToDateTime(DateTime.Parse(todaydate).AddDays(-120)))
                        {
                            objBill.FromDate = Session["firsttrdt"].ToString();
                        }
                        else
                        {
                            objBill.FromDate = DateTime.Parse(todaydate).AddDays(-120).ToShortDateString();
                        }
                        objBill.ToDate = DateTime.Parse(todaydate).AddDays(-120).ToShortDateString();
                        objBill.Date_range = "Custom";
                        ViewBag.lblHeaderTran = "from " + objBill.FromDate + " to " + objBill.ToDate;
                    }
                }
                else
                {
                    objBill.BillingTransList = gettransactionlist(objBill);
                }
                if (Session["Prov_ID"] != null & Session["UserID"] != null)
                {
                    objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                    objBill.UserId = Convert.ToString(Session["UserID"]);
                }
                objBill.Customer_Email = objBill.Customer_Email == "null" ? null : objBill.Customer_Email;
                ViewBag.hdnclientEmail = objBill.Customer_Email;
                if (!string.IsNullOrEmpty(objBill.Customer_Email))
                {
                    Session.Add("hdnclientEmail", objBill.Customer_Email);
                }
                else
                {
                    objBill.Customer_Email = null;
                }
                return PartialView("_TransDisplay", objBill);
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
        //public ActionResult pmtlist(string ddlrecords, string PatientLoginID, string fdate, string tdate, string setdate, string hdnPid)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
         
        //    ViewBag.chgpagesize = ddlrecords;
        //    ViewBag.Vplid = PatientLoginID;
        //    ViewBag.selectedPat = hdnPid;
        //    ViewBag.setDt = setdate;
        //    dynamic paygrid = getPmtlist(ddlrecords, PatientLoginID,fdate,tdate);
        //    ViewBag.Pmtpagesize = ddlrecords;
        //    return View(paygrid);
           
        //}
        //public ActionResult AdjList(string ddlrecords, string PatientLoginID, string fdate, string tdate, string setdate, string hdnPid)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
         
        //    ViewBag.chgpagesize = ddlrecords;
        //    ViewBag.Vplid = PatientLoginID;
        //    ViewBag.selectedPat = hdnPid;
        //    ViewBag.setDt = setdate;
        //    dynamic Adjgrid = getAdjList(ddlrecords, PatientLoginID,fdate,tdate);
        //    ViewBag.Adjpagesize = ddlrecords;
        //    return View(Adjgrid);  
      
        //}
        //public List<RPBilling> getChglist(string pgesize, string PatientLoginID, string fromdate, string todate)
        //{
        //    try{
        //    RPBilling objchgBill = new RPBilling();
          
        //    if (!string.IsNullOrEmpty(PatientLoginID))
        //    {
        //        objchgBill.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //        ViewBag.PlId = objchgBill.PatientLogin_ID;
        //    }
        //    else
        //    {
        //        objchgBill.PatientLogin_ID = null;
        //        ViewBag.PlId = null;
        //    }
        //    objchgBill.ReferenceType_ID = null;
         
        //    objchgBill.FromDate = fromdate;
        //    objchgBill.ToDate = todate;
        //    objchgBill.AuthorizedPatientLoginID = null;

        //    if (Request["sort"] == null)
        //    {
        //        objchgBill.OrderbyItem = "Transaction_Date";
        //        ViewBag.sort = "Transaction_Date";
        //    }
        //    else
        //    {
        //        objchgBill.OrderbyItem = Request["sort"].ToString();
        //        ViewBag.sort = Request["sort"].ToString();
        //    }
        //    if (Request["sortdir"] == null)
        //    {
        //        objchgBill.orderBy = "DESC";
        //        ViewBag.sortdir = "DESC";
        //    }
        //    else
        //    {
        //        objchgBill.orderBy = Request["sortdir"].ToString();
        //        ViewBag.sortdir = Request["sortdir"].ToString();
        //    }
        //    //if (Session["Prov_ID"] == null)
        //    //{
        //    //    return RedirectToAction("../Home/SessionExpire");
        //    //}
        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //    if (Request["page"] == null)
        //    {
        //        objchgBill.PageNo = "1";
        //    }
        //    else
        //    {
        //        objchgBill.PageNo = Request["page"];
        //    }
        //    if (pgesize != null)
        //    {
        //        objchgBill.NoOfRecords = pgesize;
        //    }
        //    else
        //    {
        //        objchgBill.NoOfRecords = "10";
        //    }
        //    List<RPBilling> chglist = new List<RPBilling>();
        //    chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //    if (chglist.Count > 0)
        //    {
        //        totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //        ViewBag.totrec = totalrecords;
        //    }
        //    else
        //    {
        //        ViewBag.totrec = 0;
        //    }
        //    return chglist;  
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "getChglist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
            
        //}

        public List<RPBilling> gettransactionlist(RPBilling objtran)
        {
            List<RPBilling> chglist = new List<RPBilling>();
            try{

            chglist = objtran.GetPatienttransactionlist(objtran);
            if (chglist.Count > 0)
            {
                ViewBag.totrec = RPBilling.Totalnoofrecords;
            }
            else
            {
                ViewBag.totrec = 0;
            }
            ViewBag.PageChargeTotal = RPBilling.PageChargeTotal;
            ViewBag.PagePaymentTotal = RPBilling.PagePaymentTotal;

            ViewBag.GrandChargeTotal = RPBilling.GrandChargeTotal;
            ViewBag.GrandPaymentTotal = RPBilling.GrandPaymentTotal;
            ViewBag.RecordCount = chglist.Count;
              }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "gettransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return chglist;
            }
            return chglist;
        }
        public List<RPBilling> gettransactionlist1(int? type_id, string NoOfRecords = null, string PageNo = null)
        {
            try{
            RPBilling objtran = new RPBilling();
            List<RPBilling> chglist = new List<RPBilling>();
            objtran.Formreference_id = Convert.ToInt32(Session["Prov_ID"]);
            objtran.type_id = type_id;
            objtran.NoOfRecords = NoOfRecords;
            objtran.PageNo = PageNo;
            chglist = objtran.GetPatienttransactionlist1(objtran);
            if (chglist.Count > 0)
            {
                ViewBag.totrec = RPBilling.Totalnoofrecords;
            }
            else
            {
                ViewBag.totrec = 0;
            }
            ViewBag.PageChargeTotal = RPBilling.PageChargeTotal;
            ViewBag.PagePaymentTotal = RPBilling.PagePaymentTotal;

            ViewBag.GrandChargeTotal = RPBilling.GrandChargeTotal;
            ViewBag.GrandPaymentTotal = RPBilling.GrandPaymentTotal;
            ViewBag.RecordCount = chglist.Count;
            return chglist;
                }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "gettransactionlist1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        //public List<RPBilling> getPmtlist(string ddlrecords, string PatientLoginID,string fdate,string tdate)
        //{
        //    try{
        //    RPBilling objAdj=new RPBilling();
        //    if (!string.IsNullOrEmpty(PatientLoginID))
        //    {
        //        objAdj.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //    }
        //    else
        //    {
        //        objAdj.PatientLogin_ID = null;
        //    }
        //    objAdj.ReferenceType_ID = null;
        //    objAdj.FromDate = fdate;
        //    objAdj.ToDate = tdate;
        //    objAdj.AuthorizedPatientLoginID = null;
        //    objAdj.OrderbyItem = Request["g2sort"] == null ? "Transaction_Date" : Request["g2sort"].ToString();
        //    ViewBag.psort = objAdj.OrderbyItem;
        //    objAdj.orderBy = Request["g2sortdir"] == null ? "DESC" : Request["g2sortdir"].ToString();
        //    ViewBag.psortdir = objAdj.orderBy;
        //    //if (Session["Prov_ID"] == null)
        //    //{
        //    //    return RedirectToAction("../Home/SessionExpire");
        //    //}
        //    objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

           
        //    objAdj.PageNo = Request["g2p2"] == null ? "1" : Request["g2p2"];
        //    objAdj.NoOfRecords = ddlrecords != null ? ddlrecords : "10";
        //    List<RPBilling> Adjlist = new List<RPBilling>();
        //    Adjlist = objAdj.GetPatientPaymentList(objAdj);
        //    if (Adjlist.Count > 0)
        //    {
        //        totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //        ViewBag.Paytotrec = totalrecords;
        //    }
        //    else
        //    {
        //        ViewBag.Paytotrec = 0;
        //    }
        //    return Adjlist;
        //     }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "getPmtlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
        //}
        //public List<RPBilling> getAdjList(string ddlrecords, string PatientLoginID, string fdate, string tdate)
        //{
        //    try{
        //    RPBilling objpmt = new RPBilling();
        //    if (!string.IsNullOrEmpty(PatientLoginID))
        //    {
        //        objpmt.PatientLogin_ID = Convert.ToInt32(PatientLoginID);
        //    }
        //    else
        //    {
        //        objpmt.PatientLogin_ID = null;
        //    }
        //    objpmt.ReferenceType_ID = null;
        //    objpmt.FromDate = fdate;
        //    objpmt.ToDate = tdate;
        //    objpmt.AuthorizedPatientLoginID = null;
        //    objpmt.OrderbyItem = Request["g3sort"] == null ? "Transaction_Date" : Request["g3sort"].ToString();
        //    objpmt.orderBy = Request["g3sortdir"] == null ? "DESC" : Request["g3sortdir"].ToString();
        //    //if (Session["Prov_ID"] == null)
        //    //{
        //    //    return RedirectToAction("../Home/SessionExpire");
        //    //}
        //    objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

           
        //    objpmt.PageNo = Request["g3p3"] == null ? "1" : Request["g3p3"];
        //    objpmt.NoOfRecords = ddlrecords != null ? ddlrecords : "10";
        //    List<RPBilling> Adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //    if (Adjlist.Count > 0)
        //    {
        //        totalrecords = Convert.ToString(RPBilling.Totalnoofrecords);
        //        ViewBag.Adjtotrec = totalrecords;
        //    }
        //    else
        //    {
        //        ViewBag.Adjtotrec = 0;
        //    }
        //    return Adjlist;
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "getAdjList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
        //}

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult AddTransaction(string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate, string clientphone = null, int? type_id = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling patinfo = new RPBilling();
                patinfo.Date_range = dt_filter;
                patinfo.PageNo = (page != null ? page : "1");
                patinfo.NoOfRecords = ddlrecords;
                patinfo.FromDate = fdate;
                patinfo.ToDate = tdate;
                patinfo.type_id = type_id;
                if (hdnPid != null)
                {
                    ViewData["combobox2_Folderstyle"] = string.Empty;

                    List<RPBilling> patinfolist = new List<RPBilling>();
                    patinfo.ToReference_ID = hdnPid;
                    patinfo.PatientLogin_ID = Convert.ToInt32(hdnPlId);
                    ViewBag.patselect = "Yes";
                    clsCommonFunctions objcommon = new clsCommonFunctions();
                    patinfo.Reference_id = Convert.ToInt32(hdnPid);
                    patinfo.ReferenceType_ID = "3";
                    IDataParameter[] insaparam ={
                                          new SqlParameter("@In_reference_id",patinfo.Reference_id),
                                          new SqlParameter("@In_referencetype_id",patinfo.ReferenceType_ID),
                                          new SqlParameter("@in_Practice_ID",Session["Prov_ID"]),
                                          new SqlParameter("@in_PatientLogin_ID",null)
                                        };
                    objcommon.AddInParameters(insaparam);

                    DataSet dspatBal = new DataSet();

                    dspatBal = objcommon.GetDataSet("Help_dbo.st_Billing_GET_UnpaidBalance");
                    if (dspatBal.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspatBal.Tables[0].Rows)
                        {
                            if (!DBNull.Value.Equals(dr["Balance"]))
                            {
                                string bal = string.Empty;
                                if (Convert.ToDouble(dr["Balance"]) >= 0)
                                {
                                    bal = string.Format("{0:c}", dr["Balance"]);
                                    patinfo.BalanceAmount = "(Unpaid balance : " + bal + ")";
                                }
                                else
                                {
                                    bal = string.Format("{0:c}", dr["Balance"]);
                                    bal = bal.Replace("(", "");
                                    bal = bal.Replace(")", "");
                                    patinfo.BalanceAmount = "(Overpaid balance :" + bal + ")";

                                }

                            }
                            if (!DBNull.Value.Equals(dr["Patientname"]))
                            {
                                patinfo.custname = dr["PatientName"].ToString();
                            }
                            if (!DBNull.Value.Equals(dr["Registerdate"]))
                            {

                                string[] date1 = dr["Registerdate"].ToString().Split(' ');
                                patinfo.RegOndate = Convert.ToString(date1[0]);
                            }
                            if (!DBNull.Value.Equals(dr["PatientEmail"]))
                            {
                                patinfo.Customer_Email = Convert.ToString(dr["PatientEmail"]);
                            }
                            if (!DBNull.Value.Equals(dr["Patient_phoneno"]))
                            {
                                patinfo.clientphone = Convert.ToString(dr["Patient_phoneno"]);
                            }
                        }
                    }
                }
                else
                {
                    //IList<SelectListItem> ddlpayPat121 = new List<SelectListItem>();
                    ViewBag.patselect = "no";
                    //ViewData["ddlpayPat"] = ddlpayPat121;
                    FillPatients1();
                }
                // List<RPBilling> listPaymode = new List<RPBilling>();
                // listPaymode = patinfo.GetPaymentModes();
                //IList<SelectListItem> paymentModeList = new List<SelectListItem>();
                //if (listPaymode.Count > 0)
                //{
                //    for (int i = 0; i <= listPaymode.Count - 1; i++)
                //    {
                //        if (i != 3)
                //        {
                //            paymentModeList.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(listPaymode[i].PaymentMode_ID),
                //                Text = Convert.ToString(listPaymode[i].PaymentMode)
                //            });
                //        }
                //    }
                //}
                ViewBag.ddlPaymentMode = patinfo.GetPaymentModes();
                // ViewData["ddlPaymentMode"] = listPaymode;// paymentModeList;
                //Referrals description = new Referrals();
                //description.FieldIDString = "11";
                DataSet dsnote = new DataSet();
                //dsnote = Referrals.List_field_description(description.FieldIDString);
                dsnote = Referrals.List_field_description("11");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.pmtNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                //if (!string.IsNullOrEmpty(clientphone))
                //{
                //    ViewBag.clientphone = clientphone;
                //}

                return View(patinfo);
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTransaction(RPBilling objTran)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                objTran.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                objTran.FromReferenceType_ID = "2";
                objTran.ToReferenceType_ID = "3";
                if (!string.IsNullOrEmpty(Request["combobox2"]))
                {
                    objTran.ToReference_ID = Request["combobox2"];
                }
                objTran.Transaction_Date = !string.IsNullOrEmpty(Request["txtPayApptDate"]) ? Request["txtPayApptDate"] : null;
                objTran.Transaction_Amount = !string.IsNullOrEmpty(objTran.Transaction_Amount) ? objTran.Transaction_Amount : null; //Request["txtPayAmount"];

                if (Request["ddlPayment"] == "1")
                {
                    objTran.PaymentType_ID = null;
                }
                else
                {
                    objTran.PaymentType_ID = Convert.ToInt32(Request["ddlPaymentMode"]);
                }
                objTran.PcType_ID = Convert.ToInt32(Request["ddlPayment"]);
                objTran.Authorizednumber = (!string.IsNullOrEmpty(objTran.Authorizednumber)) ? objTran.Authorizednumber : null;
                objTran.ChecksNo = (!string.IsNullOrEmpty(objTran.ChecksNo)) ? objTran.ChecksNo : null;

                if (!string.IsNullOrEmpty(objTran.Customer_Email))
                {
                    Session.Add("hdnclientEmail", objTran.Customer_Email);
                }
                else
                {
                    objTran.Customer_Email = null;
                }
                objTran.Notes = Sanitizer.GetSafeHtmlFragment(objTran.Notes);
                string Out_Msg = null;
                objTran.emailcheck = Request["Hdncheck"] == "Y" ? "Y" : "N";
                DataSet dstran = new DataSet();
                dstran = objTran.Insert_Customertransaction(objTran, ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.AllowGet);
                }

                //if (Request["Hdncheck"] == "Y")
                //{
                //    if (dstran.Tables[0].Rows.Count > 0)
                //    {
                //        string straddress = Convert.ToString(dstran.Tables[0].Rows[0]["FullAddress"].ToString());
                //        string strPbal = Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["PrevBalance"]));
                //        string strCcharges = Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceCharges"]));
                //        string strCpayments = Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoicePayments"]));
                //        string strCbalance = Convert.ToString(string.Format("{0:c}", dstran.Tables[0].Rows[0]["InvoiceBalance"]));
                //        string strRno = Convert.ToString(dstran.Tables[0].Rows[0]["recieptno"].ToString());
                //        string strRdate = Convert.ToString(dstran.Tables[0].Rows[0]["Invoice_Date"].ToString());
                //        if (strRdate != null & strRdate != "")
                //        {
                //            strRdate = DateTime.Parse(strRdate).ToShortDateString();
                //        }
                //        string ToReferenceName = Convert.ToString(dstran.Tables[0].Rows[0]["ToReferenceName"].ToString());
                //        string mailfrom = dstran.Tables[0].Rows[0]["electricianemail"].ToString();
                //        string mailto = dstran.Tables[0].Rows[0]["customeremail"].ToString();
                //        string provAdd = dstran.Tables[0].Rows[0]["providerAddress"].ToString();
                //        string strTdate = Convert.ToString(dstran.Tables[1].Rows[0]["Transaction_Date"].ToString());
                //        string strAdate = Convert.ToString(dstran.Tables[1].Rows[0]["Appointmentdate"].ToString());
                //        if (strTdate != null & strTdate != "")
                //        {
                //            strTdate = DateTime.Parse(strTdate).ToShortDateString();
                //        }
                //        if (strAdate != null & strAdate != "")
                //        {
                //            strAdate = DateTime.Parse(strAdate).ToShortDateString();
                //        }
                //        string strNotes = Convert.ToString(dstran.Tables[1].Rows[0]["Notes"].ToString());
                //        string strCamount = Convert.ToString(string.Format("{0:c}", dstran.Tables[1].Rows[0]["chargeamount"]));
                //        string strPamount = Convert.ToString(string.Format("{0:c}", dstran.Tables[1].Rows[0]["paymentAmount"]));
                //        string Transactiontype = Convert.ToString(dstran.Tables[1].Rows[0]["Transactiontype"].ToString());
                //        string Transaction_ID = Convert.ToString(dstran.Tables[1].Rows[0]["Transaction_ID"].ToString());
                //        SendEmailReceipt(straddress, strPbal, strCcharges, strCpayments, strCbalance, strRno, strRdate, strTdate, strAdate, strNotes, strCamount, strPamount, ToReferenceName, Transactiontype, Transaction_ID, mailto, mailfrom, provAdd);
                //    }
                //}

                //if ((AppointmentID != 0) & Request["rdoApptType"] == "Patient")
                //{
                string strbussinessname = string.Empty;
                string strproviderphone = string.Empty;
                string strclientphone = string.Empty;
                //if (Session["PracticeName"] == null || Session["Providerphone"] == null)
                //{
                //    return RedirectToAction("../Home/SessionExpire");
                //}
                if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
                {
                    strbussinessname = Convert.ToString(Session["PracticeName"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
                {
                    strproviderphone = Convert.ToString(Session["Providerphone"]);
                }
                if (!string.IsNullOrEmpty(objTran.clientphone))
                {
                    strclientphone = objTran.clientphone;
                    strclientphone = strclientphone.Replace("-", "");
                }

                clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, null, null, objTran.Transaction_Amount, Convert.ToInt32(Request["ddlPayment"]));

                return Json(JsonResponseFactory.SuccessResponse(objTran), JsonRequestBehavior.AllowGet);
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
        //public void FillPatients()
        //{
        //    try{
        //    ListofPatients getPatient = new ListofPatients();
        //    List<ListofPatients> objlist = new List<ListofPatients>();
        //    objlist = getPatient.getPatientList(Convert.ToInt32(Session["Prov_ID"]));
        //    ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
        //    ViewData["combobox1"] = custlist;            
        //    ViewData["combobox1_FolderStyle"] = "../Content/Obout/ComboBox/styles/black_glass";
        //      }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "FillPatients", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);              
        //    }
        //}
        private void FillPatientsmain()
        {
          try{
            ListofPatients getPatient = new ListofPatients();
            List<ListofPatients> objlist = new List<ListofPatients>();
            objlist = getPatient.getPatientDetails(Convert.ToInt32(Session["Prov_ID"]));
            ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            if (ViewBag.selectedPat != null)
            {
                foreach (ComboBoxItem item in custlist)
                {
                    if (item.Value == ViewBag.selectedPat)
                    {
                        item.Selected = true;
                    }
                    items.Add(item);
                }
                ViewData["comboboxMain"] = items;
            }
            else
            {
                ViewData["comboboxMain"] = custlist;
            }           
            ViewData["comboboxMain_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";       
              }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "FillPatientsmain", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        private void FillPatients1()
        {
           try{
            ListofPatients getPatient = new ListofPatients();
            List<ListofPatients> objlist = new List<ListofPatients>();
            objlist = getPatient.getPatientList(Convert.ToInt32(Session["Prov_ID"]));
            ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
            ViewData["combobox2"] = custlist;
            ViewData["combobox2_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
           }
           catch (Exception ex)
           {
               var clsCustomEx = new clsExceptionLog();
               clsCustomEx.LogException(ex, "BillingController", "FillPatients1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

           }
        }
        //public void FillPatients2()
        //{
        //  try{
        //    ListofPatients getPatient = new ListofPatients();
        //    List<ListofPatients> objlist = new List<ListofPatients>();
        //    objlist = getPatient.getPatientList(Convert.ToInt32(Session["Prov_ID"]));
        //    ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
        //    ViewData["combobox3"] = custlist;
        //    ViewData["combobox3_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
        //  }
        //  catch (Exception ex)
        //  {
        //      var clsCustomEx = new clsExceptionLog();
        //      clsCustomEx.LogException(ex, "BillingController", "FillPatients2", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //  }
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult DeleteTran(int tId, string trTypeid, string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate, string clientphone = null, string CustEmail = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return RedirectToAction("SessionExpire", "Home");
                //}
                try
                {
                    RPBilling objtran = new RPBilling();
                    objtran.Transaction_ID = tId;
                    objtran.TransactionType_ID = trTypeid;
                    objtran.UserId = Convert.ToString(Session["UserID"]);
                    objtran.Del_ProviderPatientTransaction(objtran, Convert.ToInt32(Session["Loginhistory_ID"]));
                    //if (!string.IsNullOrEmpty(hdnPid))
                    //{
                    //    return RedirectToAction("TransactionDisplay", new { ddlrecords = ddlrecords, dt_filter = dt_filter, page = page, hdnPid = hdnPid, hdnPlId = hdnPlId, fdate = fdate, tdate = tdate, clientphone = clientphone });
                    //}
                    //else
                    //{
                    //        return RedirectToAction("TransactionDisplay", new { ddlrecords = ddlrecords, dt_filter = dt_filter, fdate = fdate, tdate = tdate });

                    //}
                    objtran.NoOfRecords = ddlrecords;
                    objtran.Date_range = dt_filter;
                    objtran.PageNo = page;
                    objtran.PatientID = hdnPid;
                    objtran.PatientLogin_ID = Convert.ToInt32(hdnPlId);
                    objtran.FromDate = fdate;
                    objtran.ToDate = tdate;
                    objtran.clientphone = clientphone;
                    objtran.Customer_Email = CustEmail;
                    return RedirectToAction("DisplayTransactions", objtran);
                }
                catch (Exception ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "BillingController", "DeleteTran", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    //return RedirectToAction("Display", "Error");
                    return View();
                }
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
        [AllowAnonymous()]
        public JsonResult patBlaence(int patientid)
        {
          
            if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
              
                RPBilling patinfo = new RPBilling();
             List<RPBilling> patinfolist = new List<RPBilling>();
                  patinfo.Reference_id = patientid;
                   patinfo.ReferenceType_ID = "3";
                   patinfo.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                patinfo.PatientLogin_ID = null;
                patinfolist = patinfo.getUnpaidBalance(patinfo);
                //if (!string.IsNullOrEmpty(patinfolist[0].Customer_Email))
                //{
                    Session.Add("hdnclientEmail", !string.IsNullOrEmpty(patinfolist[0].Customer_Email) ? patinfolist[0].Customer_Email : null);
                //}
                return Json(patinfolist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
            }
            
        }

        //public JsonResult DdlpatBlaence(string patientid)
        //{
           
        //    if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
        //    {
        //        RPBilling patinfo = new RPBilling();
        //        DataSet ds = new DataSet();
        //        ds = patinfo.GETPatientPayment_Details(patientid, Convert.ToString(Session["Practice_ID"]));
        //        IList<SelectListItem> ddlpayPat = new List<SelectListItem>();
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {

        //            for (int j = 0; j <= ds.Tables[0].Rows.Count - 1; j++)
        //            {
        //                string indmsg = string.Empty;
        //                if (Convert.ToString(ds.Tables[0].Rows[j]["Status_Ind"]) == "I")
        //                {
        //                    indmsg = "InActive";
        //                }
        //                else
        //                {
        //                    indmsg = "Active";
        //                }
        //                ddlpayPat.Add(new SelectListItem()
        //                {


        //                    Value = Convert.ToString(ds.Tables[0].Rows[j]["ID"]),
        //                    Text = Convert.ToString(ds.Tables[0].Rows[j]["NAME"]) + " ( " + indmsg + " )"

        //                }

        //                    );
        //            }

        //        }
        //        return Json(ddlpayPat, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
        //    }
        
        //}

        //public JsonResult DdlSelpatBlaence(string patientid)
        //{
          
        //    if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
        //    {
        //        RPBilling patinfo = new RPBilling();
        //        List<RPBilling> patinfolist = new List<RPBilling>();

        //        patinfo.Reference_id = Convert.ToInt32(patientid);
        //        patinfo.ReferenceType_ID = "8";
        //        patinfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //        patinfolist = patinfo.getUnpaidBalance(patinfo);
        //        return Json(patinfolist, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
        //    }
         
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult EditCharge(string tId, string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate, string hdnnew, string clientphone = null, int? type_id = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsECharge = new RPBilling();
                clsECharge.Date_range = dt_filter;
                clsECharge.PageNo = !string.IsNullOrEmpty(page) ? page : "1";
                clsECharge.NoOfRecords = ddlrecords;
                clsECharge.PatientID = hdnPid;
                clsECharge.PatientLogin_ID = Convert.ToInt32(hdnPlId);
                clsECharge.FromDate = fdate;
                clsECharge.ToDate = tdate;
                clsECharge.page_ind = hdnnew;
                clsECharge.type_id = type_id;
                if (!string.IsNullOrEmpty(clientphone))
                {
                    clsECharge.clientphone = clientphone;
                }
                ViewBag.pageRowCount = Convert.ToInt32(Request["pageRowCount"]);
                clsECharge.Transaction_ID = Convert.ToInt32(tId);
                ViewBag.trCid = Convert.ToInt32(tId);
                DataSet dsChgdetail = new DataSet();
                //dsChgdetail = clsECharge.ShowChargeAppliedAmount(clsECharge);
                dsChgdetail = clsECharge.showtransactiondetails(clsECharge);
                if (dsChgdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsChgdetail.Tables[0].Rows)
                    {
                        clsECharge.custname = Convert.ToString(dr["Patientname"]);
                        //ViewBag.tCharge = dr["Chargetype"];
                        clsECharge.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsECharge.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        clsECharge.Notes = dr["Notes"] != DBNull.Value ? HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"])) : null;
                        clsECharge.ToReference_ID = Convert.ToString(dr["ToReference_ID"]);
                        clsECharge.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? Convert.ToString(dr["Email"]) : null;
                        //string date1 = dr["PAtientRegDate"].ToString();
                        //string[] regdate = date1.Split(' ');
                        //ViewBag.regdate = regdate[0];
                    }
                }
                //Referrals description = new Referrals();
                //description.FieldIDString = "10";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("10");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.chkgENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsECharge);
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditCharge(RPBilling chgDetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //  int transId = 0;
                chgDetails.Transaction_ID = Convert.ToInt32(Request["trCid"]);
                chgDetails.BalanceAmount = !string.IsNullOrEmpty(chgDetails.Transaction_Amount) ? chgDetails.Transaction_Amount : null;
                chgDetails.Customer_Email = !string.IsNullOrEmpty(chgDetails.Customer_Email) ? chgDetails.Customer_Email : null;
                chgDetails.Transaction_Date = !string.IsNullOrEmpty(Request["TransactionChgDate"]) ? Request["TransactionChgDate"] : null;
                chgDetails.Notes = !string.IsNullOrEmpty(chgDetails.Notes) ? chgDetails.Notes : null;
                chgDetails.UserId = Convert.ToString(Session["UserID"]);
                chgDetails.Ind = "Y";
                string Out_Msg = null;

                chgDetails.emailcheck = chgDetails.ECchkemail ? "Y" : "N";
                chgDetails.Edit_Payment_charges(chgDetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);



                if (Out_Msg != "")
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                return Json(JsonResponseFactory.SuccessResponse(chgDetails), JsonRequestBehavior.DenyGet);
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
        [AllowAnonymous()]
        public ActionResult EditPayment(string tId, string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate, string hdnnew, int? type_id = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsEPay = new RPBilling();
                ViewBag.patid = Request["patid"];
                clsEPay.Date_range = dt_filter;
                clsEPay.PageNo = (page != null ? page : "1");
                clsEPay.NoOfRecords = ddlrecords;
                clsEPay.PatientID = hdnPid;
                clsEPay.PatientLogin_ID = Convert.ToInt32(hdnPlId);
                clsEPay.FromDate = fdate;
                clsEPay.ToDate = tdate;
                clsEPay.page_ind = hdnnew;
                clsEPay.type_id = type_id;
                clsEPay.Transaction_ID = Convert.ToInt32(tId);
                ViewBag.pageRowCount = Convert.ToInt32(Request["pageRowCount"]);
                DataSet dsPmtdetail = new DataSet();
                //dsPmtdetail = clsEPay.showPayAmount(clsEPay);
                dsPmtdetail = clsEPay.showtransactiondetails(clsEPay);
                if (dsPmtdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPmtdetail.Tables[0].Rows)
                    {
                        clsEPay.custname = dr["Patientname"].ToString();
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["PaymentType_ID"])))
                        {
                            clsEPay.paytype = Convert.ToString(dr["PaymentType_ID"]);
                            if (clsEPay.paytype == "1")
                            {
                                clsEPay.paytype = "Cash";
                            }
                            else if (clsEPay.paytype == "2")
                            {
                                clsEPay.paytype = "Credit Card";
                            }
                            else if (clsEPay.paytype == "3")
                            {
                                clsEPay.paytype = "Check";
                            }
                        }
                        if (dr["ChequeNo"] != DBNull.Value)
                        {
                            if (Convert.ToString(dr["ChequeNo"]) != "")
                            {
                                clsEPay.ChecksNo = dr["ChequeNo"].ToString();
                            }
                            else
                            {
                                clsEPay.ChecksNo = null;
                            }
                        }
                        else
                        {
                            clsEPay.ChecksNo = null;
                        }
                        if (dr["Authorizationnumber"] != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["Authorizationnumber"])))
                            {
                                clsEPay.Authorizednumber = dr["Authorizationnumber"].ToString();
                            }
                            else
                            {
                                clsEPay.Authorizednumber = null;
                            }
                        }
                        else
                        {
                            clsEPay.Authorizednumber = null;
                        }
                        //string[] regdate = dr["PatientRegDate"].ToString().Split(' ');
                        //ViewBag.regdate = regdate[0].ToString();
                        clsEPay.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsEPay.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        clsEPay.Notes = dr["Notes"] != DBNull.Value ? HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"])) : null;
                        clsEPay.ToReference_ID = Convert.ToString(dr["ToReference_ID"]);
                        clsEPay.fromReferenceId = Convert.ToInt32(dr["FromReference_ID"]);
                        clsEPay.FromReferenceType_ID = Convert.ToString(dr["FromReferenceType_ID"]);
                        clsEPay.ToReferenceType_ID = Convert.ToString(dr["ToReferenceType_ID"]);
                        clsEPay.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? dr["Email"].ToString() : null;
                    }
                }
                //if (dsPmtdetail.Tables.Count > 0)
                //{

                //    StringBuilder strEdiPaygrid = new StringBuilder("<table width='100%' align='center' cellpadding='0' cellspacing='0' border='0' id='tblDistribute' runat='server'> <tr height='25' class='white_color' align='center'><td class='lbl' style='font-size:14px;'> This payment is applied to the following claims and/or charges.</td></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table id='tblhead' runat='server' cellspacing='1' cellpadding='1' width='100%' align='center' border='0'><tr><th class='tr_bgcolor' align='left' width='100%' style='font-weight: bold;'>List of transactions</th></tr></table>");
                //    strEdiPaygrid = strEdiPaygrid.Append("<table width='100%' cellpadding='4' cellspacing='1' border='0' class='border_style'><tr class='tr_bgcolor'><td width='25%' align='center'><strong>Date</strong></td><td width='25%' align='center'><strong>Transaction Type</strong></td><td width='25%' align='center'><strong>Amount</strong></td></tr>");
                //    if (dsPmtdetail.Tables[1].Rows.Count > 0)
                //    {
                //        foreach (DataRow drgrid in dsPmtdetail.Tables[1].Rows)
                //        {
                //            strEdiPaygrid = strEdiPaygrid.Append("<tr class='white_color'><td width='25%' align='center'>" + DateTime.Parse(drgrid["Transaction_Date"].ToString()).ToShortDateString() + "</td><td width='25%' align='center'>" + drgrid["ServiceName"] + "</td><td width='25%' align='center'>" + string.Format("{0:c}",  drgrid["AppliedAmount"]) + "</td></tr>");


                //        }
                //        strEdiPaygrid = strEdiPaygrid.Append("</table>");
                //        ViewBag.editPaydist = strEdiPaygrid.ToString();
                //    }
                //    else
                //    {
                //        ViewBag.editPaydist = null;
                //    }

                //}

                //Referrals description = new Referrals();
                //description.FieldIDString = "11";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("11");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.pmtENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsEPay);
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditPayment(RPBilling paydetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                paydetails.BalanceAmount = !string.IsNullOrEmpty(paydetails.Transaction_Amount) ? paydetails.Transaction_Amount : null;
                paydetails.Customer_Email = !string.IsNullOrEmpty(paydetails.Customer_Email) ? paydetails.Customer_Email : null;
                paydetails.Transaction_Date = !string.IsNullOrEmpty(Request["TransactionPmtDate"]) ? Request["TransactionPmtDate"] : null;
                paydetails.Notes = !string.IsNullOrEmpty(paydetails.Notes) ? paydetails.Notes : null;
                paydetails.Authorizednumber = !string.IsNullOrEmpty(paydetails.Authorizednumber) ? paydetails.Authorizednumber : null;
                paydetails.ChecksNo = !string.IsNullOrEmpty(paydetails.ChecksNo) ? paydetails.ChecksNo : null;
                paydetails.UserId = Convert.ToString(Session["UserID"]);
                paydetails.Ind = "N";
                string Out_Msg = null;
                paydetails.emailcheck = paydetails.EPchkemail ? "Y" : "N";
                paydetails.Edit_Payment_charges(paydetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);

                if (Out_Msg != "")
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                return Json(JsonResponseFactory.SuccessResponse(paydetails), JsonRequestBehavior.DenyGet);
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
        [AllowAnonymous()]
        public ActionResult EditTransaction(string tId, string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate, string hdnnew, string clientphone = null, int? type_id = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling clsEPay = new RPBilling();
                ViewBag.patid = Request["patid"];
                clsEPay.Date_range = dt_filter;
                clsEPay.PageNo = (page != null ? page : "1");
                clsEPay.NoOfRecords = ddlrecords;
                clsEPay.PatientID = hdnPid;
                clsEPay.PatientLogin_ID = Convert.ToInt32(hdnPlId);
                clsEPay.FromDate = fdate;
                clsEPay.ToDate = tdate;
                clsEPay.page_ind = hdnnew;
                clsEPay.type_id = type_id;
                ViewBag.pageRowCount = Convert.ToInt32(Request["pageRowCount"]);
                if (!string.IsNullOrEmpty(clientphone))
                {
                    clsEPay.clientphone = clientphone;
                }

                clsEPay.Transaction_ID = Convert.ToInt32(tId);
                DataSet dsPmtdetail = new DataSet();
                //dsPmtdetail = clsEPay.showPayAmount(clsEPay);
                dsPmtdetail = clsEPay.showtransactiondetails(clsEPay);

                if (dsPmtdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPmtdetail.Tables[0].Rows)
                    {
                        clsEPay.custname = dr["Patientname"].ToString();
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["PaymentType_ID"])))
                        {
                            clsEPay.paytype = dr["PaymentType_ID"].ToString();
                            if (clsEPay.paytype == "1")
                            {
                                clsEPay.paytype = "Cash";
                            }
                            else if (clsEPay.paytype == "2")
                            {
                                clsEPay.paytype = "Credit Card";
                            }
                            else if (clsEPay.paytype == "3")
                            {
                                clsEPay.paytype = "Check";
                            }
                        }
                        clsEPay.ChecksNo = dr["ChequeNo"] != DBNull.Value ? (!string.IsNullOrEmpty(Convert.ToString(dr["ChequeNo"])) ? dr["ChequeNo"].ToString() : null) : null;

                        clsEPay.Authorizednumber = dr["Authorizationnumber"] != DBNull.Value ? (!string.IsNullOrEmpty(Convert.ToString(dr["Authorizationnumber"])) ? dr["Authorizationnumber"].ToString() : null) : null;
                        //string[] regdate = dr["PatientRegDate"].ToString().Split(' ');
                        //ViewBag.regdate = regdate[0].ToString();
                        clsEPay.Transaction_Date = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
                        clsEPay.Transaction_Amount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2).ToString();
                        clsEPay.Notes = dr["Notes"] != DBNull.Value ? HttpUtility.HtmlDecode(Convert.ToString(dr["Notes"])) : null;
                        clsEPay.ToReference_ID = dr["ToReference_ID"].ToString();
                        clsEPay.fromReferenceId = Convert.ToInt32(dr["FromReference_ID"]);
                        clsEPay.FromReferenceType_ID = dr["FromReferenceType_ID"].ToString();
                        clsEPay.ToReferenceType_ID = dr["ToReferenceType_ID"].ToString();
                        clsEPay.Customer_Email = !string.IsNullOrEmpty(Convert.ToString(dr["Email"])) ? dr["Email"].ToString() : null;
                    }
                }
                //Referrals description = new Referrals();
                //description.FieldIDString = "11";
                DataSet dsnote = new DataSet();
                dsnote = Referrals.List_field_description("11");
                if (dsnote.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    ViewBag.pmtENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                }
                return View(clsEPay);
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTransaction(RPBilling paydetails)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                paydetails.BalanceAmount = !string.IsNullOrEmpty(paydetails.Transaction_Amount) ? paydetails.Transaction_Amount : null;
                paydetails.Customer_Email = !string.IsNullOrEmpty(paydetails.Customer_Email) ? paydetails.Customer_Email : null;
                paydetails.Authorizednumber = !string.IsNullOrEmpty(paydetails.Authorizednumber) ? paydetails.Authorizednumber : null;
                paydetails.Transaction_Date = !string.IsNullOrEmpty(Request["TransactionEDate"]) ? Request["TransactionEDate"] : null;
                paydetails.Notes = !string.IsNullOrEmpty(paydetails.Notes) ? paydetails.Notes : null;
                paydetails.ChecksNo = !string.IsNullOrEmpty(paydetails.ChecksNo) ? paydetails.ChecksNo : null;
                paydetails.UserId = Convert.ToString(Session["UserID"]);
                paydetails.Ind = "N";
                string Out_Msg = null;
                paydetails.emailcheck = paydetails.Echkemail ? "Y" : "N";
                paydetails.Edit_Payment_charges(paydetails, Convert.ToInt32(Session["Loginhistory_ID"]), ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                }
                return Json(JsonResponseFactory.SuccessResponse(paydetails), JsonRequestBehavior.DenyGet);
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
        //[HttpGet()]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //[AllowAnonymous()]
        //public ActionResult EditAdjustment(string tId, string trTypeid, string ddlrecords, string dt_filter, string page, string hdnPid, string hdnPlId, string fdate, string tdate)
        //{
        
        //    ViewBag.dtfill = dt_filter;
        //    ViewBag.vpage = page;
        //    ViewBag.Vddl = ddlrecords;
        //    ViewBag.hdnPid = hdnPid;
        //    ViewBag.hdnPlId = hdnPlId;
        //    ViewBag.Vfdate = fdate;
        //    ViewBag.Vtdate = tdate;
        //    RPBilling editAdjDetails = new RPBilling();
        //    editAdjDetails.Transaction_ID = Convert.ToInt32(tId);
        //    ViewBag.trAdjId = Convert.ToInt32(tId);
        //    editAdjDetails.TransactionType_ID = trTypeid;
        //    DataSet dsAdjDetails = new DataSet();
        //    dsAdjDetails = editAdjDetails.showAdjustDetails(editAdjDetails);
        //    if(dsAdjDetails.Tables[0].Rows.Count>0)
        //    {
        //        foreach (DataRow dr in dsAdjDetails.Tables[0].Rows)
        //        {
        //            ViewBag.patadjName = dr["Patientname"];
        //            ViewBag.dAdj = DateTime.Parse(Convert.ToString(dr["Transaction_Date"])).ToShortDateString();
        //            ViewBag.AdjAmount = Math.Round(Convert.ToDecimal(dr["Transaction_Amount"]), 2);
        //            if (dr["Notes"] != DBNull.Value)
        //            {

        //                ViewBag.adjustmentnotes = HttpUtility.HtmlDecode(dr["Notes"].ToString());
        //            }
        //            else
        //            {
        //                ViewBag.adjustmentnotes = null;
        //            }
        //            ViewBag.adjuDirection = dr["ChargeType"];
        //            ViewBag.lblTransactonType = dr["AdjustmentType"];
        //            ViewBag.patAdjId = dr["ToReference_ID"];
        //            ViewBag.ToRefType_ID = dr["ToReferenceType_ID"];
        //            ViewBag.FrmRef_ID = dr["FromReference_ID"];
        //            ViewBag.FrmRefType_ID = dr["FromReferenceType_ID"];
        //            string[] regdate = Convert.ToString(dr["PatientRegDate"]).Split(' ');
        //            ViewBag.regdate =Convert.ToString( regdate[0]);
        //        }
        //    }
        //    //Referrals description = new Referrals();
        //    //description.FieldIDString = "12";
        //    DataSet dsnote = new DataSet();
        //    dsnote = Referrals.List_field_description("12");
        //    if (dsnote.Tables[0].Rows.Count > 0)
        //    {
        //        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //        ViewBag.adjENote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
        //    }
        //    //if (dsAdjDetails.Tables.Count > 0)
        //    //{

        //    //    StringBuilder strEdiPaygrid = new StringBuilder("<table width='100%' align='center' cellpadding='0' cellspacing='0' border='0' id='tblDistribute' runat='server'> <tr height='25' class='white_color' align='center'><td class='lbl' style='font-size:14px;'> This adjustment is applied to the following payments and/or charges.</td></tr></table>");
        //    //    strEdiPaygrid = strEdiPaygrid.Append("<table id='tblhead' runat='server' cellspacing='1' cellpadding='1' width='100%' align='center' border='0'><tr><th class='tr_bgcolor' align='left' width='100%' style='font-weight: bold;'>List of transactions</th></tr></table>");
        //    //    strEdiPaygrid = strEdiPaygrid.Append("<table width='100%' cellpadding='4' cellspacing='1' border='0' class='border_style'><tr class='tr_bgcolor'><td width='25%' align='center'><strong>Date</strong></td><td width='25%' align='center'><strong>Transaction Type</strong></td><td width='25%' align='center'><strong>Amount</strong></td></tr>");
        //    //    if (dsAdjDetails.Tables[1].Rows.Count > 0)
        //    //    {
        //    //        foreach (DataRow drgrid in dsAdjDetails.Tables[1].Rows)
        //    //        {
        //    //            strEdiPaygrid = strEdiPaygrid.Append("<tr class='white_color'><td width='25%' align='center'>" + DateTime.Parse(drgrid["Transaction_Date"].ToString()).ToShortDateString() + "</td><td width='25%' align='center'>" + drgrid["TransactionType"] + "</td><td width='25%' align='center'>" + string.Format("{0:c}", drgrid["AppliedAmount"]) + "</td></tr>");

        //    //        }
        //    //        strEdiPaygrid = strEdiPaygrid.Append("</table>");
        //    //        ViewBag.editPaydist = strEdiPaygrid.ToString();
        //    //    }
        //    //    else
        //    //    {
        //    //        ViewBag.editPaydist = null;
        //    //    }
                
        //    //}
           
        //    return View();
         
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken()]
        //public ActionResult EditAdjustment()
        //{
        //    if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
        //    {
             
        //    RPBilling objEditAdj = new RPBilling();
        //    objEditAdj.Transaction_ID =Convert.ToInt32(Request["trAdjId"]);
        //    objEditAdj.Transaction_Date = Request["txtAdjDate"];
        //    objEditAdj.BalanceAmount = Request["txtAdjAmount"];
        //    objEditAdj.Notes =Sanitizer.GetSafeHtmlFragment(Request["txtEadjNotes"]);
        //    objEditAdj.Ind = "Y";
        //    objEditAdj.UserId = Convert.ToString(Session["UserID"]);
        //    string Out_Msg = null;
        //    objEditAdj.Edit_Payment_charges(objEditAdj, Convert.ToInt32(Session["Loginhistory_ID"]),ref Out_Msg);

        //    objEditAdj.Ind = "Y";
        //    objEditAdj.fromReferenceId = Convert.ToInt32(Request["FrmRef_ID"]);
        //    objEditAdj.ReferenceType_ID = Request["FrmRefType_ID"];
        //    objEditAdj.ToReference_ID = Request["patAdjId"];
        //    objEditAdj.ToReferenceType_ID = Request["ToRefType_ID"];
        //    DataSet dsViewDistt = new DataSet();
        //    dsViewDistt = objEditAdj.viewpaymentAutodistrib(objEditAdj);
        //    if (dsViewDistt.Tables[0].Rows.Count > 0)
        //    {
        //        double autoDistAmount = 0;
        //        if (Request["txtAdjAmount"] != "0")
        //        {
        //            autoDistAmount = Convert.ToDouble(Request["txtAdjAmount"]);
        //        }
        //        for (int i = 0; i <= dsViewDistt.Tables[0].Rows.Count - 1; i++)
        //        {
        //            string chrgId = string.Empty;
        //            double amtBal = 0;
        //            double prcAmt = 0;
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"]))
        //            {
        //                chrgId = dsViewDistt.Tables[0].Rows[i]["chargeTransaction_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]))
        //            {
        //                amtBal = Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["BalanceAmount"]);
        //            }
        //            if (!DBNull.Value.Equals(dsViewDistt.Tables[0].Rows[i]["AppliedAmount"]))
        //            {
        //                amtBal += Convert.ToDouble(dsViewDistt.Tables[0].Rows[i]["AppliedAmount"]);
        //            }
        //            if (autoDistAmount > amtBal)
        //            {
        //                prcAmt = amtBal;
        //                autoDistAmount = autoDistAmount - amtBal;
        //            }
        //            else
        //            {
        //                prcAmt = autoDistAmount;
        //                autoDistAmount = 0;
        //            }
        //            objEditAdj.Practice_ID = Convert.ToInt32(Request["FrmRef_ID"]);
        //            objEditAdj.chargeTransaction_ID = chrgId;
        //            objEditAdj.PaymentTransaction_ID = Request["trAdjId"];
        //            objEditAdj.AppliedAmount = Convert.ToString(prcAmt);
        //            objEditAdj.UserId = Convert.ToString(Session["UserID"]);
        //            objEditAdj.Ins_Disttransaction(objEditAdj, Convert.ToInt32(Session["Loginhistory_ID"]));

        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Request["hdnPid"]))
        //    {
        //        return RedirectToAction("TransactionDisplay", new { ddlrecords = Request["Vddl"], dt_filter = Request["dtfill"], page = Request["vpage"], hdnPid = Request["hdnPid"], hdnPlId = Request["hdnPlId"], fdate = Request["Vfdate"], tdate = Request["Vtdate"] });
        //    }
        //    else
        //    {
        //        return RedirectToAction("TransactionDisplay", new { ddlrecords = Request["Vddl"], dt_filter = Request["dtfill"], page = Request["vpage"], fdate = Request["Vfdate"], tdate = Request["Vtdate"] });
        //    }
                
        //    }
        //    else
        //    {
        //        return RedirectToRoute("SessionExpire");
        //    }
        //}
        #endregion

        //public FileContentResult Transactionchargepdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{

        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    try{
        //    pdf.Create();
        //    StringBuilder strhtml = new StringBuilder();
        //    pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
        //    Getchargepdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Charges.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionchargepdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
        //    return null;
        //}
        //public void Transactionchargeexcel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    RPBilling objchgBill = new RPBilling();
        //    try{
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        ViewBag.PlId = objchgBill.PatientLogin_ID;
        //    }
        //    else
        //    {
        //        objchgBill.PatientLogin_ID = null;
        //        ViewBag.PlId = null;
        //    }
        //    if (fromdate == "") 
        //    {
        //        fromdate = null;
        //    }
        //    if (todate == "")
        //    {
        //        todate = null;
        //    }
        //    objchgBill.ReferenceType_ID = null;

        //    objchgBill.FromDate = fromdate;
        //    objchgBill.ToDate = todate;
        //    objchgBill.AuthorizedPatientLoginID = null;
        //    objchgBill.OrderbyItem = sort;
        //    objchgBill.orderBy = sortdir;
        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objchgBill.PageNo = "1";
        //    objchgBill.NoOfRecords = noofrecords;
        //    List<RPBilling> chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //    WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
        //    //Response.Clear();
        //    //Response.Charset = "";
        //    //string gridData = grid.GetHtml(
        //    //    columns: grid.Columns(
        //    //            grid.Column("Transaction_Date", "Date"),
        //    //            grid.Column("BillableParty", "Charged to"),
        //    //            grid.Column("Notes", "Notes"),
        //    //            grid.Column("Transaction_Amount", "Amount")
        //    //            )
        //    //        ).ToString();
        //      string strhtml = null;
        //      strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
        //      strhtml = strhtml + "<tr>";
        //      strhtml = strhtml + "<td ></td>";
        //      strhtml = strhtml + "<td ></td>";
        //      strhtml = strhtml + "<td >Charges List</td>";
        //      strhtml = strhtml + "<td ></td>";
        //      strhtml = strhtml + "</tr>";
        //      strhtml = strhtml + "</table>";
        //      if (chglist.Count > 0)
        //    {

        //        strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";
               
        //        strhtml = strhtml + "<tr>";
        //        strhtml = strhtml + "<td><span>Date</span></td>";
        //        // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //        strhtml = strhtml + "<td><span>Charged to</span></td>";
        //        // strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //        strhtml = strhtml + "<td><span>Notes</span></td>";
        //        strhtml = strhtml + "<td><span>Amount</span></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in chglist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr >";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            // strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //            // strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";
        //    }
        //    //gridData = strhtml + gridData;
        //    //  Response.ClearContent();
        //    //HttpContext.Response.AddHeader("content-disposition", "attachment; filename=chargeslist.xls");
        //    //Response.ContentType = "application/vnd.ms-excel";
        //    //Response.Charset = "";
        //    //StringWriter sw = new StringWriter();
        //    //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    //htw.Write(strhtml);
        //    //Response.Output.Write(sw.ToString());
        //    //Response.Flush();
        //    //Response.End();
        //      Response.ClearContent();
        //      Response.AddHeader("content-disposition", "attachment; filename=charges.xls");
        //      Response.ContentType = "application/vnd.ms-excel";
        //      Response.Write(strhtml);
        //      Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionchargeexcel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             
        //    }
        //}
        //private void Getchargepdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{  
        //    RPBilling objchgBill = new RPBilling();
        //    try{
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        ViewBag.PlId = objchgBill.PatientLogin_ID;
        //    }
        //    else
        //    {
        //        objchgBill.PatientLogin_ID = null;
        //        ViewBag.PlId = null;
        //    }
        //    if (fromdate == "")
        //    {
        //        fromdate = null;
        //    }
        //    if (todate == "")
        //    {
        //        todate = null; 
        //    }
        //    objchgBill.ReferenceType_ID = null;

        //    objchgBill.FromDate = fromdate;
        //    objchgBill.ToDate = todate;
        //    objchgBill.AuthorizedPatientLoginID = null;
        //    objchgBill.OrderbyItem = sort;
        //    objchgBill.orderBy = sortdir;
        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objchgBill.PageNo = "1";
        //    objchgBill.NoOfRecords = noofrecords;
        //    List<RPBilling> chglist = objchgBill.GetPatientChargeslist(objchgBill);
        // //   double height = 100;
        //    double pos3 = 300;

        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (chglist.Count > 0)
        //    {
               
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //       // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //       // strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in chglist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //          // strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //           // strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";
        //    }
        //    else
        //    {
        //        strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Charges Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
          
        //   // height = pdf.GetTextHeight(strhtml);
        //    pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Getchargepdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
        //    }
        //}
        public FileContentResult Transactionpdf(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            try
            {
            pdf.Create();
            StringBuilder strhtml = new StringBuilder();
            pdf.AddHTMLPos(100, 50, Convert.ToString(strhtml));
            Getpdf(fromdate, todate, hdnPid, sort, sortdir, noofrecords);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=Charges.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
               }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "Transactionpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return null;
        }
        private void Getpdf(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            RPBilling objtran = new RPBilling();
            try{
            List<RPBilling> chglist = new List<RPBilling>();
            if (fromdate == "")
            {
                fromdate = null;
            }
            if (todate == "")
            {
                todate = null;
            }
            objtran.PatientID = hdnPid;
            objtran.FromDate = fromdate;
            objtran.ToDate = todate;
            objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objtran.NoOfRecords = null;
            objtran.PageNo = "1";
            objtran.OrderbyItem = sort;
            objtran.orderBy = sortdir;
            chglist = objtran.GetPatienttransactionlist(objtran);






            WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td></td>";
            strhtml = strhtml + "<td align='center'><b><u>Transaction list</u></b></td>";
            strhtml = strhtml + "<td></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (chglist.Count > 0)
            {

                strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";

                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td><span>Transaction date</span></td>";
                strhtml = strhtml + "<td><span>Appointment date</span></td>";
                strhtml = strhtml + "<td><span>Client name</span></td>";
                strhtml = strhtml + "<td><span>Payment mode</span></td>";
                strhtml = strhtml + "<td><span>Notes</span></td>";
                strhtml = strhtml + "<td><span>Charge</span></td>";
                strhtml = strhtml + "<td><span>Payment</span></td>";
                strhtml = strhtml + "</tr>";
                foreach (var item in chglist)
                {
                    string date1 = item.Transaction_Date;
                    string[] date = date1.Split(' ');
                    strhtml = strhtml + "<tr >";
                    strhtml = strhtml + "<td><b>" + item.Transaction_Date + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.apptdate + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalCharges + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalPayments + "</b></td>";
                    strhtml = strhtml + "</tr>";

                }
                strhtml = strhtml + "</table>";
            }

            // height = pdf.GetTextHeight(strhtml);
            double pos3 = 300;
            pdf.AddHTMLPos(pos3, 10, strhtml);
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "Getpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             
            }
        }
        public void Transactionexcel(string fromdate, string todate, string hdnPid, string sort, string sortdir, string noofrecords)
        {
            try
            {
           RPBilling objtran = new RPBilling();
            List<RPBilling> chglist = new List<RPBilling>();
            if (fromdate == "")
            {
                fromdate = null;
            }
            if (todate == "")
            {
                todate = null;
            }
            objtran.PatientID = hdnPid;
            objtran.FromDate = fromdate;
            objtran.ToDate = todate;
            objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objtran.NoOfRecords = null;
            objtran.PageNo = "1";
            objtran.OrderbyItem = sort;
            objtran.orderBy = sortdir;
            chglist = objtran.GetPatienttransactionlist(objtran);






            WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='100%'>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td ></td>";
            strhtml = strhtml + "<td ></td>";
            strhtml = strhtml + "<td >Transaction list</td>";
            strhtml = strhtml + "<td ></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table>";
            if (chglist.Count > 0)
            {

                strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";

                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td><span>Transaction date</span></td>";
                strhtml = strhtml + "<td><span>Appointment date</span></td>";
                strhtml = strhtml + "<td><span>Client name</span></td>";
                strhtml = strhtml + "<td><span>Payment mode</span></td>";
                strhtml = strhtml + "<td><span>Notes</span></td>";
                strhtml = strhtml + "<td><span>Charge</span></td>";
                strhtml = strhtml + "<td><span>Payment</span></td>";
                strhtml = strhtml + "</tr>";
                foreach (var item in chglist)
                {
                    string date1 = item.Transaction_Date;
                    string[] date = date1.Split(' ');
                    strhtml = strhtml + "<tr >";
                    strhtml = strhtml + "<td><b>" + item.Transaction_Date + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.apptdate + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalCharges + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalPayments + "</b></td>";
                    strhtml = strhtml + "</tr>";

                }
                strhtml = strhtml + "</table>";
            }
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=charges.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(strhtml);
            Response.End();
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "Transactionexcel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //public FileContentResult Transactionpaymentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    try
        //    {
        //    pdf.Create();
        //    var strhtml = new StringBuilder();
        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getpaymentpdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Payments.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionpaymentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
        //    return null;
        //}
        //public void Getpaymentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //    var objAdj = new RPBilling();
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        objAdj.PatientLogin_ID = null;
        //    }
        //    objAdj.ReferenceType_ID = null;
        //    objAdj.FromDate = fromdate;
        //    objAdj.ToDate = todate;
        //    objAdj.AuthorizedPatientLoginID = null;

        //    objAdj.OrderbyItem = sort;


        //    objAdj.orderBy = sortdir;
           
        //    objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

          
        //        objAdj.PageNo = "1";

        //        objAdj.NoOfRecords = noofrecords != null ? noofrecords : "10";
        //    List<RPBilling> adjlist = objAdj.GetPatientPaymentList(objAdj);
        //  //  double height = 100;
        //    double pos3 = 300;
        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (adjlist.Count > 0)
        //    {
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in adjlist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";
               
        //    }
        //    else
        //    {
        //        strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
        //   // height = pdf.GetTextHeight(strhtml);
        //    pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Getpaymentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
        //    }
        //}
        //public void Transactionpaymentexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try{
        //    var objAdj = new RPBilling();
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        objAdj.PatientLogin_ID = null;
        //    }
        //    objAdj.ReferenceType_ID = null;
        //    objAdj.FromDate = fromdate;
        //    objAdj.ToDate = todate;
        //    objAdj.AuthorizedPatientLoginID = null;

        //    objAdj.OrderbyItem = sort;


        //    objAdj.orderBy = sortdir;

        //    objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //    objAdj.PageNo = "1";

        //    objAdj.NoOfRecords = noofrecords != null ? noofrecords : "10";
        //    List<RPBilling> adjlist = objAdj.GetPatientPaymentList(objAdj);
        //    //  double height = 100;
        //  //  double pos3 = 300;
        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (adjlist.Count > 0)
        //    {
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in adjlist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";

        //    }
        //    else
        //    {
        //        strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=Payments.xls");
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Write(strhtml);
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionpaymentexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //}
        //public FileContentResult Transactionadjustmentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    try
        //    {
        //    pdf.Create();
        //    var strhtml = new StringBuilder();

        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getadjustmentpdf(fromdate, todate, palgnid, sort, sortdir, noofrecords);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Adjustments.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionadjustmentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null;
        //    }
        //    return null;
        //}
        //public void Getadjustmentpdf(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //    var objpmt = new RPBilling();
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objpmt.PatientLogin_ID = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        objpmt.PatientLogin_ID = null;
        //    }
        //    objpmt.ReferenceType_ID = null;

        //    objpmt.FromDate = fromdate;
        //    objpmt.ToDate = todate;
        //    objpmt.AuthorizedPatientLoginID = null;
        //    objpmt.OrderbyItem = sort;
        //    objpmt.orderBy =  sortdir;
        //    objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objpmt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //    objpmt.PageNo = "1";
        //    objpmt.NoOfRecords = noofrecords;
        //    var adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //   // double height = 100;
        //    double pos3 = 300;

        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Adjustments List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (adjlist.Count > 0)
        //    {

        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Paid by/Charged by" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Adjustment type" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in adjlist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.ChargeType  + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";
               
        //    }
        //    else
        //    {
        //        strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Adjustments Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
        //   // height = pdf.GetTextHeight(strhtml);
        //    pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Getadjustmentpdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                
        //    }
        //}
        //public void Transactionadjustmentexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string noofrecords)
        //{
        //    try
        //    {
        //    var objpmt = new RPBilling();
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objpmt.PatientLogin_ID = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        objpmt.PatientLogin_ID = null;
        //    }
        //    objpmt.ReferenceType_ID = null;

        //    objpmt.FromDate = fromdate;
        //    objpmt.ToDate = todate;
        //    objpmt.AuthorizedPatientLoginID = null;
        //    objpmt.OrderbyItem = sort;
        //    objpmt.orderBy = sortdir;
        //    objpmt.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    objpmt.PageNo = "1";
        //    objpmt.NoOfRecords = noofrecords;
        //    var adjlist = objpmt.GetPatientAdjustmenttList(objpmt);
        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Adjustments List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (adjlist.Count > 0)
        //    {

        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Paid by/Charged by" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Adjustment type" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in adjlist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table>";

        //    }
        //    else
        //    {
        //        strhtml = "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Adjustments Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=adjustments.xls");
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Write(strhtml);
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Transactionadjustmentexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //}
        //public FileContentResult Alltransactionspdf(string fromdate, string todate, string palgnid, string chtotrec, string chsort, string chsortdir, string pasort, string pasortdir, string patotrec, string hdnpatid)
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    try
        //    {
        //    pdf.Create();
        //    var strhtml = new StringBuilder();

        //    pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
        //    Getalltranspdf(fromdate, todate, palgnid, chtotrec, chsort, chsortdir, pasort, pasortdir, patotrec, hdnpatid);
        //    Response.Clear();
        //    Response.ClearHeaders();
        //    Response.ClearContent();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-disposition", "attachment; filename=Alltransactions.pdf");
        //    Response.BinaryWrite((byte[])pdf.SaveVariant());
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Alltransactionspdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //        return null; 
        //    }
        //    return null; 
        //}
        //public void Getalltranspdf(string fromdate, string todate, string palgnid, string chtotrec, string chsort, string chsortdir, string pasort, string pasortdir, string patotrec, string hdnpatid)
        //{
        //    try
        //    {
        //    int? palgnid1;
        //    if (palgnid != "" & palgnid!=null)
        //    {
        //        palgnid1 = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        palgnid1 = null;
        //    }
        //       clsCommonFunctions objcomm = new clsCommonFunctions();
        //    Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
        //    objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
        //    if ((objInfo != null))
        //    {
        //        if (!string.IsNullOrEmpty(objInfo.FirstName))
        //        {
        //            ViewBag.FirstName = objInfo.FirstName;
        //        }
        //        else
        //        {
        //            ViewBag.FirstName = null;
        //        }
        //        if (!string.IsNullOrEmpty(objInfo.LastName))
        //        {
        //            ViewBag.LastName = objInfo.LastName;
        //        }
        //        else
        //        {
        //            ViewBag.LastName = null;
        //        }
        //    }
        //    string patname = null;
        //    if (!string.IsNullOrEmpty(hdnpatid))
        //    {
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] InParamList = {
        //    new SqlParameter("@In_Patient_Ids", hdnpatid),
        //    new SqlParameter("@iN_pROVIDER_ID", null)
        //};
        //        objcommon.AddInParameters(InParamList);
        //        DataSet objds = new DataSet();
        //        objds = objcommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
        //        if (objds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in objds.Tables[0].Rows)
        //            {
        //                patname = Convert.ToString(dr["PatientName"]);
        //            }
        //        }
        //    }
        //    string strproname = ViewBag.FirstName +" "+ ViewBag.LastName;
        //    var objBill = new RPBilling();
        //    objBill.FromDate = fromdate;
        //    objBill.ToDate = todate;
        //    //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    objBill.PatientLogin_ID = palgnid1;
        //    objBill.Reference_id = null;
        //    var dsbill = RPBilling.PracticePatientIncome(objBill);
        //    string strTCharge = null;
        //    string strTPay = null;
        //    string strBal = null;
        //    string strCredit = null;
        //    foreach (DataRow dr in dsbill.Tables[0].Rows)
        //    {
        //         strTCharge = string.Format("{0:c}", dr["TotalCharges"]);
        //        strTPay = string.Format("{0:c}", dr["Netpayments"]);
        //         strBal = string.Format("{0:c}", dr["Balance"]);
        //        double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
        //         strCredit = string.Format("{0:c}", dblcredit);
        //    }
        //  //  double height = 100;
        //    double pos3 = 300;

        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Client billing summary</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";

        //    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='left'><font size='15px'><b>Mower helper name : " + strproname + "</b></font></td>";
        //    if (patname != null & patname != "")
        //    {
        //        strhtml = strhtml + "<td align='right'><font size='15px'><b>Client name : " + patname + "</b></font></td>";
        //    }
        //    else
        //    {
        //        strhtml = strhtml + "<td align='right'></td>";
        //    }
        //    strhtml = strhtml + "</tr>";
           
        //    strhtml = strhtml + "</table><br/>";

        //    strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //    strhtml = strhtml + "<tr class='gray'>";
        //    strhtml = strhtml + "<td align='center' ><label>Total charges" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Total payments" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Total credits" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Balance" + "</label></td>";         
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr class='gray'>";
        //    strhtml = strhtml + "<td><b> " + strTCharge + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strTPay + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strCredit + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strBal + "</b></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/><br/>";
        //  strhtml =strhtml+ "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //   strhtml = strhtml + "<tr align='center'>";
        //   strhtml = strhtml + "<td align='center'><font size='15px'><b><u>By days outstanding</u></b></font></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "<tr>";
        //   strhtml = strhtml + "<td align='center' height='25%'></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "</table><br/>";
        //   if (!string.IsNullOrEmpty(hdnpatid))
        //   {
        //       int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
        //       DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
        //       if (dsdayout.Tables[0].Rows.Count > 0)
        //       {
        //           foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
        //           {
        //               string str0_30 = string.Empty;
        //               string str31_60 = string.Empty;
        //               string str61_90 = string.Empty;
        //               string str91_120 = string.Empty;
        //               string strOver120 = string.Empty;
        //               double strAm30;
        //               double strAm60;
        //               double strAm90;
        //               double strAm120;
        //               double strAm150;
        //               if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
        //               {
        //                   str0_30 = string.Format("{0:c}", dr1["0-30"]);
        //                   strAm30 = Convert.ToDouble(dr1["0-30"]);
        //               }
        //               else
        //               {
        //                   str0_30 = "$0.00";
        //                   strAm30 = 0;

        //               }
        //               if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
        //               {
        //                   str31_60 = string.Format("{0:c}", dr1["31-60"]);
        //                   strAm60 = Convert.ToDouble(dr1["31-60"]);
        //               }
        //               else
        //               {
        //                   str31_60 = "$0.00";
        //                   strAm60 = 0;
        //               }
        //               if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
        //               {
        //                   str61_90 = string.Format("{0:c}", dr1["61-90"]);
        //                   strAm90 = Convert.ToDouble(dr1["61-90"]);
        //               }
        //               else
        //               {
        //                   str61_90 = "$0.00";
        //                   strAm90 = 0;
        //               }
        //               if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
        //               {
        //                   str91_120 = string.Format("{0:c}", dr1["91-120"]);
        //                   strAm120 = Convert.ToDouble(dr1["91-120"]);
        //               }
        //               else
        //               {
        //                   str91_120 = "$0.00";
        //                   strAm120 = 0;
        //               }
        //               if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
        //               {
        //                   strOver120 = string.Format("{0:c}", dr1["over 120 days"]);
        //                   strAm150 = Convert.ToDouble(dr1["over 120 days"]);
        //               }
        //               else
        //               {
        //                   strOver120 = "$0.00";
        //                   strAm150 = 0;
        //               }




        //               double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
        //               string amountdue = string.Format("{0:c}", amountdue1);
        //               strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //               strhtml = strhtml + "<tr class='gray'>";
        //               strhtml = strhtml + "<td align='center' ><label>Billable Party" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
        //               strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
        //               strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b> "+dr1["name"] + "</b></td>";
        //               strhtml = strhtml + "<td><b> </b></td>";
        //               strhtml = strhtml + "<td><b></b></td>";
        //               strhtml = strhtml + "<td><b></b></td>";
        //               strhtml = strhtml + "<td><b></b></td>";
        //               strhtml = strhtml + "<td><b></b></td>";
        //               strhtml = strhtml + "<td><b></b></td>";
        //               strhtml = strhtml + "</tr>";
        //               strhtml = strhtml + "<tr class='gray'>";
        //               strhtml = strhtml + "<td><b>Balance </b></td>";
        //               strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
        //               strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
        //               strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
        //               strhtml = strhtml + "<td><b>" + str91_120 + "</b></td>";
        //               strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
        //               strhtml = strhtml + "<td><b>" + amountdue + "</b></td>";
        //               strhtml = strhtml + "</tr>";
        //               strhtml = strhtml + "</table><br/><br/>";
        //           }
        //       }
        //   }
        //   else
        //   {
        //       DataSet dsRespBill = new DataSet();
        //       //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //       objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
        //       dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
        //       string str0_30 = null;
        //       string str31_60 = null;
        //       string str61_90 = null;
        //       string str90_120 = null;
        //       string strOver120 = null;
        //       string strAmountDue = null;
        //       foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
        //       {
        //           str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
        //           str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
        //           str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
        //           str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
        //           strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
        //           strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

        //       }
        //       strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
        //       strhtml = strhtml + "</tr>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
        //       strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
        //       strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
        //       strhtml = strhtml + "<td><b>" + str90_120 + "</b></td>";
        //       strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
        //       strhtml = strhtml + "<td><b>" + strAmountDue + "</b></td>";
        //       strhtml = strhtml + "</tr>";
        //       strhtml = strhtml + "</table><br/><br/>";

        //   }
        //   RPBilling objchgBill = new RPBilling();

        //   if (!string.IsNullOrEmpty(palgnid))
        //   {
        //       objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //       ViewBag.PlId = objchgBill.PatientLogin_ID;
        //   }
        //   else
        //   {
        //       objchgBill.PatientLogin_ID = null;
        //       ViewBag.PlId = null;
        //   }
        //   if (fromdate == "")
        //   {
        //       fromdate = null;
        //   }
        //   if (todate == "")
        //   {
        //       todate = null;
        //   }
        //   objchgBill.ReferenceType_ID = null;

        //   objchgBill.FromDate = fromdate;
        //   objchgBill.ToDate = todate;
        //   objchgBill.AuthorizedPatientLoginID = null;

        //    objchgBill.OrderbyItem = chsort;


        //    objchgBill.orderBy = chsortdir;
           
        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //   //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //   objchgBill.PageNo = "1";
        //   objchgBill.NoOfRecords = chtotrec;
        //   List<RPBilling> chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //   strhtml =strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //   strhtml = strhtml + "<tr align='center'>";
        //   strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "<tr>";
        //   strhtml = strhtml + "<td align='center' height='25%'></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "</table><br/>";
        //   if (chglist.Count > 0)
        //   {
        //       strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //      // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //     //  strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //       strhtml = strhtml + "<td align='center' width='40px'><label>Notes" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //       strhtml = strhtml + "</tr>";
        //       foreach (var item in chglist)
        //       {
        //           string date1 = item.Transaction_Date;
        //           string[] date = date1.Split(' ');
        //           strhtml = strhtml + "<tr class='gray'>";
        //           strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //         // strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //          // strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //           strhtml = strhtml + "</tr>";

        //       }
        //       strhtml = strhtml + "</table><br/><br/>";
        //   }
        //   else
        //   {
        //       strhtml =strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td width='10px' align='center' ><b> No Charges Found. " + "</b></td>";
        //       strhtml = strhtml + "</table><br/><br/>";
        //   }


        //   var objAdj = new RPBilling();
        //   if (!string.IsNullOrEmpty(palgnid))
        //   {
        //       objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //   }
        //   else
        //   {
        //       objAdj.PatientLogin_ID = null;
        //   }
        //   objAdj.ReferenceType_ID = null;
        //   objAdj.FromDate = fromdate;
        //   objAdj.ToDate = todate;
        //   objAdj.AuthorizedPatientLoginID = null;

        //   objAdj.OrderbyItem = pasort;


        //   objAdj.orderBy = pasortdir;

        //   objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //   //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //   objAdj.PageNo = "1";

        //   objAdj.NoOfRecords = patotrec != null ? patotrec : "10";
        //   List<RPBilling> adjlist = objAdj.GetPatientPaymentList(objAdj);
      
        //   strhtml = strhtml+"<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //   strhtml = strhtml + "<tr align='center'>";
        //   strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "<tr>";
        //   strhtml = strhtml + "<td align='center' height='25%'></td>";
        //   strhtml = strhtml + "</tr>";
        //   strhtml = strhtml + "</table><br/>";
        //   if (adjlist.Count > 0)
        //   {

        //       strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //       strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //       strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //       strhtml = strhtml + "</tr>";
        //       foreach (var item in adjlist)
        //       {
        //           string date1 = item.Transaction_Date;
        //           string[] date = date1.Split(' ');
        //           strhtml = strhtml + "<tr class='gray'>";
        //           strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //           strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //           strhtml = strhtml + "</tr>";

        //       }
        //       strhtml = strhtml + "</table> <br/> <br/>";

        //   }
        //   else
        //   {
        //       strhtml =strhtml+ "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //       strhtml = strhtml + "<tr class='gray'>";
        //       strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //       strhtml = strhtml + "</table>";
        //   }
        // // height = pdf.GetTextHeight(strhtml);
        //   pdf.AddHTMLPos(pos3, 10, strhtml);
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Getalltranspdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              
        //    }
        //}
        //public void Alltransactionsexel(string fromdate, string todate, string palgnid, string chtotrec, string chsort, string chsortdir, string pasort, string pasortdir, string patotrec, string hdnpatid)
        //{
        //    try
        //    {
        //    int? palgnid1;
        //    if (palgnid != "" & palgnid != null)
        //    {
        //        palgnid1 = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        palgnid1 = null;
        //    }
        //    clsCommonFunctions objcomm = new clsCommonFunctions();
        //    Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
        //    objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
        //    if ((objInfo != null))
        //    {
        //        if (!string.IsNullOrEmpty(objInfo.FirstName))
        //        {
        //            ViewBag.FirstName = objInfo.FirstName;
        //        }
        //        else
        //        {
        //            ViewBag.FirstName = null;
        //        }
        //        if (!string.IsNullOrEmpty(objInfo.LastName))
        //        {
        //            ViewBag.LastName = objInfo.LastName;
        //        }
        //        else
        //        {
        //            ViewBag.LastName = null;
        //        }
        //    }
        //    string patname = null;
        //    if (!string.IsNullOrEmpty(hdnpatid))
        //    {
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] InParamList = {
        //    new SqlParameter("@In_Patient_Ids", hdnpatid),
        //    new SqlParameter("@iN_pROVIDER_ID", null)
        //};
        //        objcommon.AddInParameters(InParamList);
        //        DataSet objds = new DataSet();
        //        objds = objcommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
        //        if (objds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in objds.Tables[0].Rows)
        //            {
        //                patname = Convert.ToString(dr["PatientName"]);
        //            }
        //        }
        //    }
        //    string strproname = ViewBag.FirstName + " " + ViewBag.LastName;
        //    var objBill = new RPBilling();
        //    objBill.FromDate = fromdate;
        //    objBill.ToDate = todate;
        //    //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    objBill.PatientLogin_ID = palgnid1;
        //    objBill.Reference_id = null;
        //    var dsbill = RPBilling.PracticePatientIncome(objBill);
        //    string strTCharge = null;
        //    string strTPay = null;
        //    string strBal = null;
        //    string strCredit = null;
        //    foreach (DataRow dr in dsbill.Tables[0].Rows)
        //    {
        //        strTCharge = string.Format("{0:c}", dr["TotalCharges"]);
        //        strTPay = string.Format("{0:c}", dr["Netpayments"]);
        //        strBal = string.Format("{0:c}", dr["Balance"]);
        //        double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
        //        strCredit = string.Format("{0:c}", dblcredit);
        //    }
        //    //  double height = 100;
        //   // double pos3 = 300;

        //    string strhtml = null;
        //    strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Client billing summary</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";

        //    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='left'><font size='15px'><b>Mower helper name : " + strproname + "</b></font></td>";
        //    if (patname != null & patname != "")
        //    {
        //        strhtml = strhtml + "<td align='right'><font size='15px'><b>Client name : " + patname + "</b></font></td>";
        //    }
        //    else
        //    {
        //        strhtml = strhtml + "<td align='right'></td>";
        //    }
        //    strhtml = strhtml + "</tr>";

        //    strhtml = strhtml + "</table><br/>";

        //    strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //    strhtml = strhtml + "<tr class='gray'>";
        //    strhtml = strhtml + "<td align='center' ><label>Total charges" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Total payments" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Total credits" + "</label></td>";
        //    strhtml = strhtml + "<td align='center' ><label>Balance" + "</label></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr class='gray'>";
        //    strhtml = strhtml + "<td><b> " + strTCharge + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strTPay + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strCredit + "</b></td>";
        //    strhtml = strhtml + "<td><b>" + strBal + "</b></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/><br/>";
        //    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>By days outstanding</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (!string.IsNullOrEmpty(hdnpatid))
        //    {
        //        int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
        //        DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
        //        if (dsdayout.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
        //            {
        //                string str0_30 = string.Empty;
        //                string str31_60 = string.Empty;
        //                string str61_90 = string.Empty;
        //                string str91_120 = string.Empty;
        //                string strOver120 = string.Empty;
        //                double strAm30;
        //                double strAm60;
        //                double strAm90;
        //                double strAm120;
        //                double strAm150;
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
        //                {
        //                    str0_30 = string.Format("{0:c}", dr1["0-30"]);
        //                    strAm30 = Convert.ToDouble(dr1["0-30"]);
        //                }
        //                else
        //                {
        //                    str0_30 = "$0.00";
        //                    strAm30 = 0;

        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
        //                {
        //                    str31_60 = string.Format("{0:c}", dr1["31-60"]);
        //                    strAm60 = Convert.ToDouble(dr1["31-60"]);
        //                }
        //                else
        //                {
        //                    str31_60 = "$0.00";
        //                    strAm60 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
        //                {
        //                    str61_90 = string.Format("{0:c}", dr1["61-90"]);
        //                    strAm90 = Convert.ToDouble(dr1["61-90"]);
        //                }
        //                else
        //                {
        //                    str61_90 = "$0.00";
        //                    strAm90 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
        //                {
        //                    str91_120 = string.Format("{0:c}", dr1["91-120"]);
        //                    strAm120 = Convert.ToDouble(dr1["91-120"]);
        //                }
        //                else
        //                {
        //                    str91_120 = "$0.00";
        //                    strAm120 = 0;
        //                }
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
        //                {
        //                    strOver120 = string.Format("{0:c}", dr1["over 120 days"]);
        //                    strAm150 = Convert.ToDouble(dr1["over 120 days"]);
        //                }
        //                else
        //                {
        //                    strOver120 = "$0.00";
        //                    strAm150 = 0;
        //                }




        //                double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
        //                string amountdue = string.Format("{0:c}", amountdue1);
        //                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td align='center' ><label>Billable Party" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
        //                strhtml = strhtml + "<td align='center' ><label>Amount due" + "</label></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b> " + dr1["name"] + "</b></td>";
        //                strhtml = strhtml + "<td><b> </b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "<td><b></b></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "<tr class='gray'>";
        //                strhtml = strhtml + "<td><b>Balance </b></td>";
        //                strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + str91_120 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
        //                strhtml = strhtml + "<td><b>" + amountdue + "</b></td>";
        //                strhtml = strhtml + "</tr>";
        //                strhtml = strhtml + "</table><br/><br/>";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        DataSet dsRespBill = new DataSet();
        //        //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

        //        objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
        //        dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
        //        string str0_30 = null;
        //        string str31_60 = null;
        //        string str61_90 = null;
        //        string str90_120 = null;
        //        string strOver120 = null;
        //        string strAmountDue = null;
        //        foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
        //        {
        //            str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
        //            str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
        //            str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
        //            str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
        //            strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
        //            strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

        //        }
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + str90_120 + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
        //        strhtml = strhtml + "<td><b>" + strAmountDue + "</b></td>";
        //        strhtml = strhtml + "</tr>";
        //        strhtml = strhtml + "</table><br/><br/>";

        //    }
        //    RPBilling objchgBill = new RPBilling();

        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objchgBill.PatientLogin_ID = Convert.ToInt32(palgnid);
        //        ViewBag.PlId = objchgBill.PatientLogin_ID;
        //    }
        //    else
        //    {
        //        objchgBill.PatientLogin_ID = null;
        //        ViewBag.PlId = null;
        //    }
        //    if (fromdate == "")
        //    {
        //        fromdate = null;
        //    }
        //    if (todate == "")
        //    {
        //        todate = null;
        //    }
        //    objchgBill.ReferenceType_ID = null;

        //    objchgBill.FromDate = fromdate;
        //    objchgBill.ToDate = todate;
        //    objchgBill.AuthorizedPatientLoginID = null;

        //    objchgBill.OrderbyItem = chsort;


        //    objchgBill.orderBy = chsortdir;

        //    objchgBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objchgBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
        //    objchgBill.PageNo = "1";
        //    objchgBill.NoOfRecords = chtotrec;
        //    List<RPBilling> chglist = objchgBill.GetPatientChargeslist(objchgBill);
        //    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Charges List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (chglist.Count > 0)
        //    {
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        // strhtml = strhtml + "<td align='center' ><label>Charged by" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Charged to" + "</label></td>";
        //        //  strhtml = strhtml + "<td align='center' ><label>Chargetype" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px'><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in chglist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            // strhtml = strhtml + "<td><b>" + item.BilledBy + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
        //            // strhtml = strhtml + "<td><b>" + item.ChargeType + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table><br/><br/>";
        //    }
        //    else
        //    {
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px' align='center' ><b> No Charges Found. " + "</b></td>";
        //        strhtml = strhtml + "</table><br/><br/>";
        //    }


        //    var objAdj = new RPBilling();
        //    if (!string.IsNullOrEmpty(palgnid))
        //    {
        //        objAdj.PatientLogin_ID = Convert.ToInt32(palgnid);
        //    }
        //    else
        //    {
        //        objAdj.PatientLogin_ID = null;
        //    }
        //    objAdj.ReferenceType_ID = null;
        //    objAdj.FromDate = fromdate;
        //    objAdj.ToDate = todate;
        //    objAdj.AuthorizedPatientLoginID = null;

        //    objAdj.OrderbyItem = pasort;


        //    objAdj.orderBy = pasortdir;

        //    objAdj.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
        //    //objAdj.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);


        //    objAdj.PageNo = "1";

        //    objAdj.NoOfRecords = patotrec != null ? patotrec : "10";
        //    List<RPBilling> adjlist = objAdj.GetPatientPaymentList(objAdj);

        //    strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //    strhtml = strhtml + "<tr align='center'>";
        //    strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Payments List</u></b></font></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "<tr>";
        //    strhtml = strhtml + "<td align='center' height='25%'></td>";
        //    strhtml = strhtml + "</tr>";
        //    strhtml = strhtml + "</table><br/>";
        //    if (adjlist.Count > 0)
        //    {

        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td align='center' ><label>Date" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Paid by" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Payment mode" + "</b></td>";
        //        strhtml = strhtml + "<td align='center' width='40px' ><label>Notes" + "</label></td>";
        //        strhtml = strhtml + "<td align='center' ><label>Amount" + "</label></td>";
        //        strhtml = strhtml + "</tr>";
        //        foreach (var item in adjlist)
        //        {
        //            string date1 = item.Transaction_Date;
        //            string[] date = date1.Split(' ');
        //            strhtml = strhtml + "<tr class='gray'>";
        //            strhtml = strhtml + "<td><b>" + Convert.ToString(date[0]) + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaidByName + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
        //            strhtml = strhtml + "<td><b>" + item.Transaction_Amount + "</b></td>";
        //            strhtml = strhtml + "</tr>";

        //        }
        //        strhtml = strhtml + "</table> <br/> <br/>";

        //    }
        //    else
        //    {
        //        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
        //        strhtml = strhtml + "<tr class='gray'>";
        //        strhtml = strhtml + "<td width='10px'><b> No Payments Found. " + "</b></td>";
        //        strhtml = strhtml + "</table>";
        //    }
        //    // height = pdf.GetTextHeight(strhtml);
        //    // pdf.AddHTMLPos(pos3, 10, strhtml);
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=Alltransactions.xls");
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Write(strhtml);
        //    Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsCustomEx = new clsExceptionLog();
        //        clsCustomEx.LogException(ex, "BillingController", "Alltransactionsexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //}
        public FileContentResult Totaltransactionspdf(string fromdate, string todate, string palgnid, string sort, string sortdir,string hdnpatid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            try
            {
            pdf.Create();
            var strhtml = new StringBuilder();

            pdf.AddHTMLPos(100, 10, Convert.ToString(strhtml));
            GetTottranspdf(fromdate, todate, palgnid, sort, sortdir, hdnpatid);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-disposition", "attachment; filename=Alltransactions.pdf");
            Response.BinaryWrite((byte[])pdf.SaveVariant());
            Response.End();
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "Totaltransactionspdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
            return null;
        }
        public void GetTottranspdf(string fromdate, string todate, string palgnid,string sort, string sortdir, string hdnpatid)
        {
            try
            {
            int? palgnid1;
            if (palgnid != "" & palgnid != null)
            {
                palgnid1 = Convert.ToInt32(palgnid);
            }
            else
            {
                palgnid1 = null;
            }
            clsCommonFunctions objcomm = new clsCommonFunctions();
            Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
            objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
            if ((objInfo != null))
            {
                if (!string.IsNullOrEmpty(objInfo.FirstName))
                {
                    ViewBag.FirstName = objInfo.FirstName;
                }
                else
                {
                    ViewBag.FirstName = null;
                }
                if (!string.IsNullOrEmpty(objInfo.LastName))
                {
                    ViewBag.LastName = objInfo.LastName;
                }
                else
                {
                    ViewBag.LastName = null;
                }
            }
            string patname = null;
            if (!string.IsNullOrEmpty(hdnpatid))
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
            new SqlParameter("@In_Patient_Ids", hdnpatid),
            new SqlParameter("@iN_pROVIDER_ID", null)
        };
                objcommon.AddInParameters(InParamList);
                DataSet objds = new DataSet();
                objds = objcommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
                if (objds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in objds.Tables[0].Rows)
                    {
                        patname = Convert.ToString(dr["PatientName"]);
                    }
                }
            }
            string strproname = ViewBag.FirstName + " " + ViewBag.LastName;
            var objBill = new RPBilling();
            objBill.FromDate = fromdate;
            objBill.ToDate = todate;
            //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objBill.PatientLogin_ID = palgnid1;
            objBill.Reference_id = null;
            //var dsbill = RPBilling.PracticePatientIncome(objBill);
            //string strTCharge = null;
            //string strTPay = null;
            //string strBal = null;
            //string strCredit = null;
            //foreach (DataRow dr in dsbill.Tables[0].Rows)
            //{
            //    strTCharge = string.Format("{0:c}", dr["TotalCharges"]);
            //    strTPay = string.Format("{0:c}", dr["Netpayments"]);
            //    strBal = string.Format("{0:c}", dr["Balance"]);
            //    double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
            //    strCredit = string.Format("{0:c}", dblcredit);
            //}
            //  double height = 100;
            double pos3 = 300;

            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Client billing summary</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";

            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='left'><font size='15px'><b>Mower helper name : " + strproname + "</b></font></td>";
            if (patname != null & patname != "")
            {
                strhtml = strhtml + "<td align='right'><font size='15px'><b>Client name : " + patname + "</b></font></td>";
            }
            else
            {
                strhtml = strhtml + "<td align='right'></td>";
            }
            strhtml = strhtml + "</tr>";

            strhtml = strhtml + "</table><br/>";

            //strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
            //strhtml = strhtml + "<tr class='gray'>";
            //strhtml = strhtml + "<td align='center' ><label>Total charges" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Total payments" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Total credits" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Balance" + "</label></td>";
            //strhtml = strhtml + "</tr>";
            //strhtml = strhtml + "<tr class='gray'>";
            //strhtml = strhtml + "<td><b> " + strTCharge + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strTPay + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strCredit + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strBal + "</b></td>";
            //strhtml = strhtml + "</tr>";
            //strhtml = strhtml + "</table><br/><br/>";
            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>By days outstanding</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (!string.IsNullOrEmpty(hdnpatid))
            {
                int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
                DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
                if (dsdayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
                    {
                        string str0_30 = string.Empty;
                        string str31_60 = string.Empty;
                        string str61_90 = string.Empty;
                        string str91_120 = string.Empty;
                        string strOver120 = string.Empty;
                        double strAm30;
                        double strAm60;
                        double strAm90;
                        double strAm120;
                        double strAm150;
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
                        {
                            str0_30 = Convert.ToDouble(dr1["0-30"]) > 0 ? string.Format("{0:c}", dr1["0-30"]) : "$0.00";
                            strAm30 = Convert.ToDouble(dr1["0-30"]);
                        }
                        else
                        {
                            str0_30 = "$0.00";
                            strAm30 = 0;

                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
                        {
                            str31_60 = Convert.ToDouble(dr1["31-60"]) > 0 ? string.Format("{0:c}", dr1["31-60"]) : "$0.00";
                            strAm60 = Convert.ToDouble(dr1["31-60"]);
                        }
                        else
                        {
                            str31_60 = "$0.00";
                            strAm60 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
                        {
                            str61_90 = Convert.ToDouble(dr1["61-90"]) > 0 ? string.Format("{0:c}", dr1["61-90"]) : "$0.00";
                            strAm90 = Convert.ToDouble(dr1["61-90"]);
                        }
                        else
                        {
                            str61_90 = "$0.00";
                            strAm90 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
                        {
                            str91_120 = Convert.ToDouble(dr1["91-120"]) > 0 ? string.Format("{0:c}", dr1["91-120"]) : "$0.00";
                            strAm120 = Convert.ToDouble(dr1["91-120"]);
                        }
                        else
                        {
                            str91_120 = "$0.00";
                            strAm120 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
                        {
                            strOver120 = Convert.ToDouble(dr1["over 120 days"]) > 0 ? string.Format("{0:c}", dr1["over 120 days"]) : "$0:00";
                            strAm150 = Convert.ToDouble(dr1["over 120 days"]);
                        }
                        else
                        {
                            strOver120 = "$0.00";
                            strAm150 = 0;
                        }




                        double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
                        string amountdue = string.Format("{0:c}", amountdue1);
                        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
                        strhtml = strhtml + "<tr class='gray'>";
                        strhtml = strhtml + "<td><label>Days outstanding" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
                        strhtml = strhtml + "</tr>";
                        //strhtml = strhtml + "<tr class='gray'>";
                        //strhtml = strhtml + "<td><b> " + dr1["name"] + "</b></td>";
                        //strhtml = strhtml + "<td><b> </b></td>";
                        //strhtml = strhtml + "<td><b></b></td>";
                        //strhtml = strhtml + "<td><b></b></td>";
                        //strhtml = strhtml + "<td><b></b></td>";
                        //strhtml = strhtml + "<td><b></b></td>";
                        //strhtml = strhtml + "<td><b></b></td>";
                        //strhtml = strhtml + "</tr>";
                        strhtml = strhtml + "<tr class='gray'>";
                        strhtml = strhtml + "<td><label>Amount due" + "</label></td>";
                        strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str91_120 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + amountdue + "</b></td>";
                        strhtml = strhtml + "</tr>";
                        strhtml = strhtml + "</table><br/><br/>";
                    }
                }
            }
            else
            {
                DataSet dsRespBill = new DataSet();
                //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

                objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
                string str0_30 = null;
                string str31_60 = null;
                string str61_90 = null;
                string str90_120 = null;
                string strOver120 = null;
                string strAmountDue = null;
                foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
                {
                    str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
                    str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
                    str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
                    str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
                    strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
                    strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

                }
                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str90_120 + "</b></td>";
                strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
                strhtml = strhtml + "<td><b>" + strAmountDue + "</b></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "</table><br/><br/>";

            }



            RPBilling objtran = new RPBilling();
            List<RPBilling> chglist = new List<RPBilling>();
            if (fromdate == "")
            {
                fromdate = null;
            }
            if (todate == "")
            {
                todate = null;
            }
            objtran.PatientID = hdnpatid;
            objtran.FromDate = fromdate;
            objtran.ToDate = todate;
            objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objtran.NoOfRecords = null;
            objtran.PageNo = "1";
            objtran.OrderbyItem = sort;
            objtran.orderBy = sortdir;
            chglist = objtran.GetPatienttransactionlist(objtran);
            string PCtotal = RPBilling.PageChargeTotal;
            string PPtotal = RPBilling.PagePaymentTotal;

            string GCtotal = RPBilling.GrandChargeTotal;
            string GPtotal = RPBilling.GrandPaymentTotal;
            int RecordCount = chglist.Count;
            WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
            //string strhtml = null;
            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Transaction list</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (chglist.Count > 0)
            {

                strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='95%'>";

                strhtml = strhtml + "<tr width='100%'>";
                strhtml = strhtml + "<td style='color: #808080;' width='10%'><span>Transaction date</span></td>";
                strhtml = strhtml + "<td width='10%'><span>Appointment date</span></td>";
                strhtml = strhtml + "<td width='15%'><span>Client name</span></td>";
                strhtml = strhtml + "<td width='10%'><span>Payment mode</span></td>";
                strhtml = strhtml + "<td width='25%'><span>Notes</span></td>";
                strhtml = strhtml + "<td width='10%'><span>Charge</span></td>";
                strhtml = strhtml + "<td width='10%'><span>Payment</span></td>";
                strhtml = strhtml + "</tr>";
                foreach (var item in chglist)
                {
                    string date1 = item.Transaction_Date;
                    string[] date = date1.Split(' ');
                    strhtml = strhtml + "<tr width='100%'>";
                    strhtml = strhtml + "<td width='10%'><b>" + item.Transaction_Date + "</b></td>";
                    strhtml = strhtml + "<td width='10%'><b>" + item.apptdate + "</b></td>";
                    strhtml = strhtml + "<td width='15%'><b>" + item.BillableParty + "</b></td>";
                    strhtml = strhtml + "<td width='10%'><b>" + item.PaymentMode + "</b></td>";
                    strhtml = strhtml + "<td width='25%'><b>" + item.Notes + "</b></td>";
                    strhtml = strhtml + "<td width='10%'><b>" + item.TotalCharges + "</b></td>";
                    strhtml = strhtml + "<td width='10%'><b>" + item.TotalPayments + "</b></td>";
                    strhtml = strhtml + "</tr>";

                }
                strhtml = strhtml + "<tr><td align='right' colspan='5'><b>Total of these " + RecordCount + "  transactions : </b></td><td style='text-align:center;'><b>" + PCtotal + "</b></td><td style='text-align:center;'><b> " + PPtotal + "</b></td></tr><tr><td align='right' colspan='5'><b>Grand total : </b></td><td style='text-align:center;'><b>" + GCtotal + "</b></td><td style='text-align:center;'><b>" + GPtotal + "</b></td></tr>";
                strhtml = strhtml + "</table>";
            }
            else
            {
                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td width='10px' align='center' ><b> No transaction found. " + "</b></td>";
                strhtml = strhtml + "</table><br/><br/>";
            }

            // height = pdf.GetTextHeight(strhtml);
            pdf.AddHTMLPos(pos3, 10, strhtml);
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "GetTottranspdf", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            
            }
        }
        public void Toataltransactionsexel(string fromdate, string todate, string palgnid, string sort, string sortdir, string hdnpatid)
        { 
            try
            {
            int? palgnid1;
            if (palgnid != "" & palgnid != null)
            {
                palgnid1 = Convert.ToInt32(palgnid);
            }
            else
            {
                palgnid1 = null;
            }
            clsCommonFunctions objcomm = new clsCommonFunctions();
            Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
            objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
            if ((objInfo != null))
            {
                if (!string.IsNullOrEmpty(objInfo.FirstName))
                {
                    ViewBag.FirstName = objInfo.FirstName;
                }
                else
                {
                    ViewBag.FirstName = null;
                }
                if (!string.IsNullOrEmpty(objInfo.LastName))
                {
                    ViewBag.LastName = objInfo.LastName;
                }
                else
                {
                    ViewBag.LastName = null;
                }
            }
            string patname = null;
            if (!string.IsNullOrEmpty(hdnpatid))
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
            new SqlParameter("@In_Patient_Ids", hdnpatid),
            new SqlParameter("@iN_pROVIDER_ID", null)
        };
                objcommon.AddInParameters(InParamList);
                DataSet objds = new DataSet();
                objds = objcommon.GetDataSet("Help_dbo.st_Scheduling_Get_PatientDetails");
                if (objds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in objds.Tables[0].Rows)
                    {
                        patname = Convert.ToString(dr["PatientName"]);
                    }
                }
            }
            string strproname = ViewBag.FirstName + " " + ViewBag.LastName;
            var objBill = new RPBilling();
            objBill.FromDate = fromdate;
            objBill.ToDate = todate;
            //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            objBill.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objBill.PatientLogin_ID = palgnid1;
            objBill.Reference_id = null;
            //var dsbill = RPBilling.PracticePatientIncome(objBill);
            //string strTCharge = null;
            //string strTPay = null;
            //string strBal = null;
            //string strCredit = null;
            //foreach (DataRow dr in dsbill.Tables[0].Rows)
            //{
            //    strTCharge = string.Format("{0:c}", dr["TotalCharges"]);
            //    strTPay = string.Format("{0:c}", dr["Netpayments"]);
            //    strBal = string.Format("{0:c}", dr["Balance"]);
            //    double dblcredit = Convert.ToDouble(dr["posAdjustments"]) - Convert.ToDouble(dr["NegAdjustments"]);
            //    strCredit = string.Format("{0:c}", dblcredit);
            //}
            //  double height = 100;
           // double pos3 = 300;

            string strhtml = null;
            strhtml = "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Client billing summary</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";

            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='left'><font size='15px'><b>Mower helper name : " + strproname + "</b></font></td>";
            if (patname != null & patname != "")
            {
                strhtml = strhtml + "<td align='right'><font size='15px'><b>Client name : " + patname + "</b></font></td>";
            }
            else
            {
                strhtml = strhtml + "<td align='right'></td>";
            }
            strhtml = strhtml + "</tr>";

            strhtml = strhtml + "</table><br/>";

            //strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
            //strhtml = strhtml + "<tr class='gray'>";
            //strhtml = strhtml + "<td align='center' ><label>Total charges" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Total payments" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Total credits" + "</label></td>";
            //strhtml = strhtml + "<td align='center' ><label>Balance" + "</label></td>";
            //strhtml = strhtml + "</tr>";
            //strhtml = strhtml + "<tr class='gray'>";
            //strhtml = strhtml + "<td><b> " + strTCharge + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strTPay + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strCredit + "</b></td>";
            //strhtml = strhtml + "<td><b>" + strBal + "</b></td>";
            //strhtml = strhtml + "</tr>";
            //strhtml = strhtml + "</table><br/><br/>";
            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>By days outstanding</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
            if (!string.IsNullOrEmpty(hdnpatid))
            {
                int patlogin = Convert.ToInt32(objBill.PatientLogin_ID);
                DataSet dsdayout = objBill.getPatientFileDayout(patlogin, Convert.ToInt32(Session["Prov_ID"]), objBill.FromDate, objBill.ToDate);
                if (dsdayout.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dsdayout.Tables[0].Rows)
                    {
                        string str0_30 = string.Empty;
                        string str31_60 = string.Empty;
                        string str61_90 = string.Empty;
                        string str91_120 = string.Empty;
                        string strOver120 = string.Empty;
                        double strAm30;
                        double strAm60;
                        double strAm90;
                        double strAm120;
                        double strAm150;
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["0-30"])))
                        {
                            str0_30 = Convert.ToDouble(dr1["0-30"]) > 0 ? string.Format("{0:c}", dr1["0-30"]) : "$0.00";
                            strAm30 = Convert.ToDouble(dr1["0-30"]);
                        }
                        else
                        {
                            str0_30 = "$0.00";
                            strAm30 = 0;

                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["31-60"])))
                        {
                            str31_60 = Convert.ToDouble(dr1["31-60"]) > 0 ? string.Format("{0:c}", dr1["31-60"]) : "$0.00";
                            strAm60 = Convert.ToDouble(dr1["31-60"]);
                        }
                        else
                        {
                            str31_60 = "$0.00";
                            strAm60 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["61-90"])))
                        {
                            str61_90 = Convert.ToDouble(dr1["61-90"]) > 0 ? string.Format("{0:c}", dr1["61-90"]) : "$0.00";
                            strAm90 = Convert.ToDouble(dr1["61-90"]);
                        }
                        else
                        {
                            str61_90 = "$0.00";
                            strAm90 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["91-120"])))
                        {
                            str91_120 = Convert.ToDouble(dr1["91-120"]) > 0 ? string.Format("{0:c}", dr1["91-120"]) : "$0.00";
                            strAm120 = Convert.ToDouble(dr1["91-120"]);
                        }
                        else
                        {
                            str91_120 = "$0.00";
                            strAm120 = 0;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr1["over 120 days"])))
                        {
                            strOver120 = Convert.ToDouble(dr1["over 120 days"]) > 0 ? string.Format("{0:c}", dr1["over 120 days"]) : "$0:00";
                            strAm150 = Convert.ToDouble(dr1["over 120 days"]);
                        }
                        else
                        {
                            strOver120 = "$0.00";
                            strAm150 = 0;
                        }




                        double amountdue1 = strAm30 + strAm60 + strAm90 + strAm120 + strAm150;
                        string amountdue = string.Format("{0:c}", amountdue1);
                        strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
                        strhtml = strhtml + "<tr class='gray'>";
                        strhtml = strhtml + "<td align='center' ><label>Billable Party" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
                        strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
                        strhtml = strhtml + "</tr>";
                        strhtml = strhtml + "<tr class='gray'>";
                        strhtml = strhtml + "<td><b> " + dr1["name"] + "</b></td>";
                        strhtml = strhtml + "<td><b> </b></td>";
                        strhtml = strhtml + "<td><b></b></td>";
                        strhtml = strhtml + "<td><b></b></td>";
                        strhtml = strhtml + "<td><b></b></td>";
                        strhtml = strhtml + "<td><b></b></td>";
                        strhtml = strhtml + "<td><b></b></td>";
                        strhtml = strhtml + "</tr>";
                        strhtml = strhtml + "<tr class='gray'>";
                        strhtml = strhtml + "<td><b>Balance </b></td>";
                        strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + str91_120 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
                        strhtml = strhtml + "<td><b>" + amountdue + "</b></td>";
                        strhtml = strhtml + "</tr>";
                        strhtml = strhtml + "</table><br/><br/>";
                    }
                }
            }
            else
            {
                DataSet dsRespBill = new DataSet();
                //objBill.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);

                objBill.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                dsRespBill = RPBilling.Get_PracticeresposiblepartyTransactions(objBill);
                string str0_30 = null;
                string str31_60 = null;
                string str61_90 = null;
                string str90_120 = null;
                string strOver120 = null;
                string strAmountDue = null;
                foreach (DataRow drResp in dsRespBill.Tables[0].Rows)
                {
                    str0_30 = string.Format("{0:c}", drResp["0 - 30"]);
                    str31_60 = string.Format("{0:c}", drResp["31 - 60"]);
                    str61_90 = string.Format("{0:c}", drResp["61 - 90"]);
                    str90_120 = string.Format("{0:c}", drResp["91 - 120"]);
                    strOver120 = string.Format("{0:c}", drResp["Over 120 days"]);
                    strAmountDue = string.Format("{0:c}", drResp["Amountdue"]);

                }
                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='1' width='90%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td align='center' ><label>0 - 30" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>31 - 60" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>61 - 90" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>91 - 120" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Over 120 days" + "</label></td>";
                strhtml = strhtml + "<td align='center' ><label>Total due" + "</label></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td><b> " + str0_30 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str31_60 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str61_90 + "</b></td>";
                strhtml = strhtml + "<td><b>" + str90_120 + "</b></td>";
                strhtml = strhtml + "<td><b>" + strOver120 + "</b></td>";
                strhtml = strhtml + "<td><b>" + strAmountDue + "</b></td>";
                strhtml = strhtml + "</tr>";
                strhtml = strhtml + "</table><br/><br/>";

            }


            RPBilling objtran = new RPBilling();
            List<RPBilling> chglist = new List<RPBilling>();
            if (fromdate == "")
            {
                fromdate = null;
            }
            if (todate == "")
            {
                todate = null;
            }
            objtran.PatientID = hdnpatid;
            objtran.FromDate = fromdate;
            objtran.ToDate = todate;
            objtran.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            objtran.NoOfRecords = null;
            objtran.PageNo = "1";
            objtran.OrderbyItem = sort;
            objtran.orderBy = sortdir;
            chglist = objtran.GetPatienttransactionlist(objtran);
            string PCtotal = RPBilling.PageChargeTotal;
            string PPtotal = RPBilling.PagePaymentTotal;

            string GCtotal = RPBilling.GrandChargeTotal;
            string GPtotal = RPBilling.GrandPaymentTotal;
            int RecordCount = chglist.Count;
            WebGrid grid = new WebGrid(source: chglist, canPage: false, canSort: false);
            //string strhtml = null;
            strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
            strhtml = strhtml + "<tr align='center'>";
            strhtml = strhtml + "<td align='center'><font size='15px'><b><u>Transaction list</u></b></font></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "<tr>";
            strhtml = strhtml + "<td align='center' height='25%'></td>";
            strhtml = strhtml + "</tr>";
            strhtml = strhtml + "</table><br/>";
               if (chglist.Count > 0)
            {

                strhtml = strhtml + "<table cellpadding='0' cellspacing='0' align='center' bordercolor='#666666' border='1' width='100%'>";

                strhtml = strhtml + "<tr>";
                strhtml = strhtml + "<td><span>Transaction date</span></td>";
                strhtml = strhtml + "<td><span>Appointment date</span></td>";
                strhtml = strhtml + "<td><span>Client name</span></td>";
                strhtml = strhtml + "<td><span>Payment mode</span></td>";
                strhtml = strhtml + "<td><span>Notes</span></td>";
                strhtml = strhtml + "<td><span>Charge</span></td>";
                strhtml = strhtml + "<td><span>Payment</span></td>";
                strhtml = strhtml + "</tr>";
                foreach (var item in chglist)
                {
                    string date1 = item.Transaction_Date;
                    string[] date = date1.Split(' ');
                    strhtml = strhtml + "<tr >";
                    strhtml = strhtml + "<td><b>" + item.Transaction_Date + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.apptdate + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.BillableParty + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.PaymentMode + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.Notes + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalCharges + "</b></td>";
                    strhtml = strhtml + "<td><b>" + item.TotalPayments + "</b></td>";
                    strhtml = strhtml + "</tr>";

                }
                strhtml = strhtml + "<tr><td style='text-align:right;' colspan='5'><b>Total of these " + RecordCount + "  transactions : </b></td><td style='text-align:center;'><b>" + PCtotal + "</b></td><td style='text-align:center;'><b> " + PPtotal + "</b></td></tr><tr><td style='text-align:right;' colspan='5'><b>Grand total : </b></td><td style='text-align:center;'><b>" + GCtotal + "</b></td><td style='text-align:center;'><b>" + GPtotal + "</b></td></tr>";
                strhtml = strhtml + "</table>";
            }
            else
            {
                strhtml = strhtml + "<table cellpadding='1' cellspacing='0' align='center' bordercolor='#666666' border='0' width='90%'>";
                strhtml = strhtml + "<tr class='gray'>";
                strhtml = strhtml + "<td width='10px' align='center' ><b> No transaction found. " + "</b></td>";
                strhtml = strhtml + "</table><br/><br/>";
            }


            // height = pdf.GetTextHeight(strhtml);
            // pdf.AddHTMLPos(pos3, 10, strhtml);
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Alltransactions.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(strhtml);
            Response.End();
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "BillingController", "Toataltransactionsexel", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
    }
}
