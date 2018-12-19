using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.Classes;

namespace MowerHelper.Models.DAL.DALBilling
{
    public class BillingSqlDataAccessLayer
    {
        string ClassName = "BillingSqlDataAccessLayer";
        public  DataSet PracticePatientIncome(RPBilling billInfo)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlist ={
                                                  new SqlParameter("@in_patientLogin_Id",billInfo.PatientLogin_ID),
                                                  new SqlParameter("@in_Provider_id",billInfo.Provider_id),
                                                  new SqlParameter("@in_StartDate",billInfo.FromDate),
                                                  new SqlParameter("@in_EndDate",billInfo.ToDate),
                                                  new SqlParameter("@in_ToreferenceID",billInfo.Reference_id)
                                              };
                objcommon.AddInParameters(insparamlist);
                DataSet dsIncome = new DataSet();
                dsIncome = objcommon.GetDataSet("Help_dbo.st_Billing_Get_ProviderSummary");
                return dsIncome;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "PracticePatientIncome", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet Get_PracticeresposiblepartyTransactions(RPBilling Prespbill)
        {
            try
            {
                clsCommonFunctions objcom = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                               new SqlParameter("@In_PatientLogin_id",Prespbill.PatientLogin_ID),
                                               new SqlParameter("@In_Formreference_id",Prespbill.fromReferenceId),
                                               new SqlParameter("@In_Practice_Id",Prespbill.Practice_ID)
                                          };
                objcom.AddInParameters(insparam);
                DataSet dsRespBill= new DataSet();
                dsRespBill = objcom.GetDataSet("Help_dbo.st_patient_get_daysoutstanding");
                return dsRespBill;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_PracticeresposiblepartyTransactions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public List<RPBilling> GetPatientChargeslist(RPBilling objbilling)
        //{
        //    try
        //    {
        //    clsCommonFunctions objcommon = new clsCommonFunctions();
        //    IDataParameter[] inschgParam = { 
        //                                   new SqlParameter("@in_PatientLogin_ID",objbilling.PatientLogin_ID),
        //                                   new SqlParameter("@in_ReferenceType_ID",objbilling.ReferenceType_ID),
        //                                   new SqlParameter("@in_StartDate",objbilling.FromDate),
        //                                   new SqlParameter("@in_EndDate",objbilling.ToDate),
        //                                   new SqlParameter("@In_AuthorizedpatientLogin_id",objbilling.AuthorizedPatientLoginID),
        //                                   new SqlParameter("@in_OrderbyItem",objbilling.OrderbyItem),
        //                                   new SqlParameter("@In_orderBy",objbilling.orderBy),
        //                                   new SqlParameter("@in_Provider_ID",objbilling.Provider_id),
        //                                   new SqlParameter("@In_NoOfRecords",objbilling.NoOfRecords),
        //                                   new SqlParameter("@In_PageNo",objbilling.PageNo)
                                           
        //                                   };
        //    objcommon.AddInParameters(inschgParam);
        //    SqlDataReader objChgread = default(SqlDataReader);
        //    objChgread = objcommon.GetDataReader("Help_dbo.st_Billing_List_ChargesAndClaims_rdPaging");
        //    List<RPBilling> PatchgbillList = new List<RPBilling>();
                
        //    while (objChgread.Read())
        //    {
        //        RPBilling billInfo = new RPBilling();
        //        if (! DBNull.Value.Equals(objChgread["Transaction_Date"]))
        //        {
        //            billInfo.Transaction_Date =DateTime.Parse(objChgread["Transaction_Date"].ToString()).ToShortDateString();
        //        }
        //        if (!DBNull.Value.Equals(objChgread["Transaction_ID"]))
        //        {
        //            billInfo.Transaction_ID =Convert.ToInt32(objChgread["Transaction_ID"]);
        //        }
        //        if (!DBNull.Value.Equals(objChgread["Transaction_Amount"]))
        //        {
        //            string transAmount=string.Format("{0:c}",objChgread["Transaction_Amount"]);
        //            billInfo.Transaction_Amount =transAmount ;
        //        }
        //        //if (!DBNull.Value.Equals(objChgread["ChargeType"]))
        //        //{
        //        //    billInfo.ChargeType = objChgread["ChargeType"].ToString();
        //        //}
        //        if (!DBNull.Value.Equals(objChgread["Practice_ID"]))
        //        {
        //            billInfo.Practice_ID = Convert.ToInt32(objChgread["Practice_ID"]);
        //        }
        //        if (!DBNull.Value.Equals(objChgread["TransactionType_ID"]))
        //        {
        //            billInfo.TransactionType_ID = objChgread["TransactionType_ID"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objChgread["BillableParty"]))
        //        {
        //            billInfo.BillableParty=objChgread["BillableParty"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objChgread["BilledBy"]))
        //        {
        //            billInfo.BilledBy = objChgread["BilledBy"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objChgread["PatientLogin_ID"]))
        //        {
        //            billInfo.PatientLogin_ID = Convert.ToInt32(objChgread["PatientLogin_ID"]);
        //        }
        //        if (!DBNull.Value.Equals(objChgread["Notes"]))
        //        {
        //            billInfo.Notes = objChgread["Notes"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objChgread["patient_id"]))
        //        {
        //            billInfo.PatientID = objChgread["patient_id"].ToString();
        //        }

        //        PatchgbillList.Add(billInfo);
        //    }
        //    objChgread.NextResult();
        //    if (objChgread.HasRows)
        //    {
        //        while (objChgread.Read())
        //        {
        //           RPBilling.Totalnoofrecords =Convert.ToInt32(objChgread[0]);
        //        }
        //    }
            
        //    return PatchgbillList;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "GetPatientChargeslist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public List<RPBilling> GetPatienttransactionlist(RPBilling objbilling)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inschgParam = { 
                                           new SqlParameter("@in_Patient_ID",objbilling.PatientID),
                                           new SqlParameter("@in_StartDate",objbilling.FromDate),
                                           new SqlParameter("@in_EndDate",objbilling.ToDate),
                                           new SqlParameter("@in_OrderbyItem",objbilling.OrderbyItem),
                                           new SqlParameter("@In_orderBy",objbilling.orderBy),
                                           new SqlParameter("@in_Provider_ID",objbilling.Provider_id),
                                           new SqlParameter("@In_NoOfRecords",objbilling.NoOfRecords),
                                           new SqlParameter("@In_PageNo",objbilling.PageNo)

                                           };
                objcommon.AddInParameters(inschgParam);
                SqlDataReader objChgread = default(SqlDataReader);
                objChgread = objcommon.GetDataReader("Help_dbo.ST_get_ProviderPatient_Transactionlist");
                List<RPBilling> PatchgbillList = new List<RPBilling>();
                var PageChargeTotal = 0.00;
                var PagePaymentTotal = 0.00;
                while (objChgread.Read())
                {
                    RPBilling billInfo = new RPBilling();
                    if (!DBNull.Value.Equals(objChgread["Transaction_Date"]))
                    {
                        billInfo.Transaction_Date = DateTime.Parse(objChgread["Transaction_Date"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        billInfo.Transaction_Date = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["Appointmentdate"]))
                    {
                        billInfo.apptdate = DateTime.Parse(objChgread["Appointmentdate"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        billInfo.apptdate = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["PaymentMode"]))
                    {
                        billInfo.PaymentMode = objChgread["PaymentMode"].ToString();
                    }
                    else
                    {
                        billInfo.PaymentMode = "-";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(objChgread["Notes"])))
                    {
                        billInfo.Notes = objChgread["Notes"].ToString();
                    }
                    else
                    {
                        billInfo.Notes = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["Transactiontype"]))
                    {
                        billInfo.TransactionType = objChgread["Transactiontype"].ToString();
                    }
                    else
                    {
                        billInfo.TransactionType = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["BillableParty"]))
                    {
                        billInfo.BillableParty = objChgread["BillableParty"].ToString();
                    }
                    else
                    {
                        billInfo.BillableParty = "-";
                    }
                    //if (!DBNull.Value.Equals(objChgread["Transaction_Amount"]))
                    //{
                    //    string transAmount = string.Format("{0:c}", objChgread["Transaction_Amount"]);
                    //    billInfo.Transaction_Amount = transAmount;
                    //}
                    if (!DBNull.Value.Equals(objChgread["chargeamount"]))
                    {
                        string chargeamount = string.Format("{0:c}", objChgread["chargeamount"]);
                        billInfo.TotalCharges = chargeamount;
                    }
                    else
                    {
                        billInfo.TotalCharges = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["paymentamount"]))
                    {
                        string paymentamount = string.Format("{0:c}", objChgread["paymentamount"]);
                        billInfo.TotalPayments = paymentamount;
                    }
                    else
                    {
                        billInfo.TotalPayments = "-";
                    }


                    if (!DBNull.Value.Equals(objChgread["Transaction_ID"]))
                    {
                        billInfo.Transaction_ID = Convert.ToInt32(objChgread["Transaction_ID"]);
                    }

                    if (!DBNull.Value.Equals(objChgread["chargeamount"]))
                    {
                        PageChargeTotal = PageChargeTotal + Convert.ToDouble(objChgread["chargeamount"]);
                    }
                    if (!DBNull.Value.Equals(objChgread["paymentamount"]))
                    {
                        PagePaymentTotal = PagePaymentTotal + Convert.ToDouble(objChgread["paymentamount"]);
                    }
                    PatchgbillList.Add(billInfo);
                }

                RPBilling.PageChargeTotal = string.Format("{0:c}", PageChargeTotal);
                RPBilling.PagePaymentTotal = string.Format("{0:c}", PagePaymentTotal);
                
                
                objChgread.NextResult();
                if (objChgread.HasRows)
                {
                    while (objChgread.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(objChgread[0]);
                    }
                }
                objChgread.NextResult();
                if (objChgread.HasRows)
                {
                    while (objChgread.Read())
                    {
                        if (!DBNull.Value.Equals(objChgread["TotalCharges"]))
                        {
                            RPBilling.GrandChargeTotal = string.Format("{0:c}", objChgread["TotalCharges"]);
                        }
                        else
                        {
                            RPBilling.GrandChargeTotal = string.Format("{0:c}", objChgread["TotalCharges"]);
                        }
                        if (!DBNull.Value.Equals(objChgread["Netpayments"]))
                        {
                            RPBilling.GrandPaymentTotal = string.Format("{0:c}", objChgread["Netpayments"]);
                        }
                        else
                        {
                            RPBilling.GrandPaymentTotal = null;
                        }
                    }
                }



                //RPBilling billInfo = new RPBilling();
                //List<RPBilling> PatchgbillList = new List<RPBilling>();
                //        billInfo.Transaction_Date = "11/01/2014";
                //        billInfo.apptdate = "11/01/2014";
                //        billInfo.PaymentMode = "Check";
                //        billInfo.Notes = "Check payment";
                //        billInfo.TotalCharges = "$10.00";
                //        billInfo.TotalPayments = "$10.00";
                //    PatchgbillList.Add(billInfo);
                //RPBilling.Totalnoofrecords = 9;

                return PatchgbillList;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPatienttransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> GetPatienttransactionlist1(RPBilling objbilling)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inschgParam = { 
                                           new SqlParameter("@In_Formreference_id",objbilling.Formreference_id),
                                           new SqlParameter("@in_type_id",objbilling.type_id),
                                           new SqlParameter("@In_NoOfRecords",objbilling.NoOfRecords),
                                           new SqlParameter("@In_PageNo",objbilling.PageNo)

                                           };
                objcommon.AddInParameters(inschgParam);
                SqlDataReader objChgread = default(SqlDataReader);
                objChgread = objcommon.GetDataReader("Help_dbo.st_patient_get_daysoutstanding_Typewise");
                List<RPBilling> PatchgbillList = new List<RPBilling>();
                var PageChargeTotal = 0.00;
                var PagePaymentTotal = 0.00;
                while (objChgread.Read())
                {
                    RPBilling billInfo = new RPBilling();
                    if (!DBNull.Value.Equals(objChgread["Transaction_Date"]))
                    {
                        billInfo.Transaction_Date = DateTime.Parse(objChgread["Transaction_Date"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        billInfo.Transaction_Date = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["Appointmentdate"]))
                    {
                        billInfo.apptdate = DateTime.Parse(objChgread["Appointmentdate"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        billInfo.apptdate = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["PaymentMode"]))
                    {
                        billInfo.PaymentMode = objChgread["PaymentMode"].ToString();
                    }
                    else
                    {
                        billInfo.PaymentMode = "-";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(objChgread["Notes"])))
                    {
                        billInfo.Notes = objChgread["Notes"].ToString();
                    }
                    else
                    {
                        billInfo.Notes = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["Transactiontype"]))
                    {
                        billInfo.TransactionType = objChgread["Transactiontype"].ToString();
                    }
                    else
                    {
                        billInfo.TransactionType = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["BillableParty"]))
                    {
                        billInfo.BillableParty = objChgread["BillableParty"].ToString();
                    }
                    else
                    {
                        billInfo.BillableParty = "-";
                    }
                    //if (!DBNull.Value.Equals(objChgread["Transaction_Amount"]))
                    //{
                    //    string transAmount = string.Format("{0:c}", objChgread["Transaction_Amount"]);
                    //    billInfo.Transaction_Amount = transAmount;
                    //}
                    if (!DBNull.Value.Equals(objChgread["chargeamount"]))
                    {
                        string chargeamount = string.Format("{0:c}", objChgread["chargeamount"]);
                        billInfo.TotalCharges = chargeamount;
                    }
                    else
                    {
                        billInfo.TotalCharges = "-";
                    }
                    if (!DBNull.Value.Equals(objChgread["paymentamount"]))
                    {
                        string paymentamount = string.Format("{0:c}", objChgread["paymentamount"]);
                        billInfo.TotalPayments = paymentamount;
                    }
                    else
                    {
                        billInfo.TotalPayments = "-";
                    }


                    if (!DBNull.Value.Equals(objChgread["Transaction_ID"]))
                    {
                        billInfo.Transaction_ID = Convert.ToInt32(objChgread["Transaction_ID"]);
                    }

                    if (!DBNull.Value.Equals(objChgread["chargeamount"]))
                    {
                        PageChargeTotal = PageChargeTotal + Convert.ToDouble(objChgread["chargeamount"]);
                    }
                    if (!DBNull.Value.Equals(objChgread["paymentamount"]))
                    {
                        PagePaymentTotal = PagePaymentTotal + Convert.ToDouble(objChgread["paymentamount"]);
                    }
                    PatchgbillList.Add(billInfo);
                }

                RPBilling.PageChargeTotal = string.Format("{0:c}", PageChargeTotal);
                RPBilling.PagePaymentTotal = string.Format("{0:c}", PagePaymentTotal);


                objChgread.NextResult();
                if (objChgread.HasRows)
                {
                    while (objChgread.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(objChgread[0]);
                    }
                }
                objChgread.NextResult();
                if (objChgread.HasRows)
                {
                    while (objChgread.Read())
                    {
                        if (!DBNull.Value.Equals(objChgread["TotalCharges"]))
                        {
                            RPBilling.GrandChargeTotal = string.Format("{0:c}", objChgread["TotalCharges"]);
                        }
                        else
                        {
                            RPBilling.GrandChargeTotal = string.Format("{0:c}", objChgread["TotalCharges"]);
                        }
                        if (!DBNull.Value.Equals(objChgread["Netpayments"]))
                        {
                            RPBilling.GrandPaymentTotal = string.Format("{0:c}", objChgread["Netpayments"]);
                        }
                        else
                        {
                            RPBilling.GrandPaymentTotal = null;
                        }
                    }
                }



                //RPBilling billInfo = new RPBilling();
                //List<RPBilling> PatchgbillList = new List<RPBilling>();
                //        billInfo.Transaction_Date = "11/01/2014";
                //        billInfo.apptdate = "11/01/2014";
                //        billInfo.PaymentMode = "Check";
                //        billInfo.Notes = "Check payment";
                //        billInfo.TotalCharges = "$10.00";
                //        billInfo.TotalPayments = "$10.00";
                //    PatchgbillList.Add(billInfo);
                //RPBilling.Totalnoofrecords = 9;

                return PatchgbillList;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPatienttransactionlist1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public List<RPBilling> GetPatientPaymentList(RPBilling objPaybill)
        //{
        //    try
        //    {
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] inspayParam ={
        //                                          new SqlParameter("@in_PatientLogin_ID",objPaybill.PatientLogin_ID),
        //                                          new SqlParameter("@in_ReferenceType_ID",objPaybill.ReferenceType_ID),
        //                                          new SqlParameter("@in_StartDate",objPaybill.FromDate),
        //                                          new SqlParameter("@in_EndDate",objPaybill.ToDate),
        //                                          new SqlParameter("@In_AuthorizedpatientLogin_id",objPaybill.AuthorizedPatientLoginID),
        //                                          new SqlParameter("@in_Provider_ID",objPaybill.Provider_id),
        //                                          new SqlParameter("@in_NoofRecords",objPaybill.NoOfRecords),
        //                                          new SqlParameter("@in_PageNo",objPaybill.PageNo),
        //                                          new SqlParameter("@in_OrderBy",objPaybill.orderBy),
        //                                          new SqlParameter("@in_OrderByitem",objPaybill.OrderbyItem)
                                                  
        //                                     };
        //        objcommon.AddInParameters(inspayParam);
        //        SqlDataReader objChgread = default(SqlDataReader);
        //        objChgread = objcommon.GetDataReader("Help_dbo.st_Billing_Get_Payments_RDPAGING");
        //        List<RPBilling> PatPaybillList = new List<RPBilling>();

        //        while (objChgread.Read())
        //        {
        //            RPBilling billInfo = new RPBilling();
        //            if (!DBNull.Value.Equals(objChgread["Transaction_Date"]))
        //            {
        //                billInfo.Transaction_Date = DateTime.Parse(objChgread["Transaction_Date"].ToString()).ToShortDateString();
        //            }
                   
        //            if (!DBNull.Value.Equals(objChgread["Transaction_Amount"]))
        //            {
        //                string transAmount = string.Format("{0:c}", objChgread["Transaction_Amount"]);
        //                billInfo.Transaction_Amount = transAmount;
        //            }
        //            if (!DBNull.Value.Equals(objChgread["Transaction_ID"]))
        //            {
        //                billInfo.Transaction_ID = Convert.ToInt32(objChgread["Transaction_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaidByName"]))
        //            {
        //                billInfo.PaidByName = objChgread["PaidByName"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaymentMode"]))
        //            {
        //                billInfo.PaymentMode = objChgread["PaymentMode"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaidBy_ID"]))
        //            {
        //                billInfo.PaidBy_ID = objChgread["PaidBy_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["TransactionType_ID"]))
        //            {
        //                billInfo.TransactionType_ID = objChgread["TransactionType_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaidBytype"]))
        //            {
        //                billInfo.PaidBytype = objChgread["PaidBytype"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaidBytype_ID"]))
        //            {
        //                billInfo.PaidBytype_ID = objChgread["PaidBytype_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["AppliedAmount"]))
        //            {
        //                billInfo.AppliedAmount = objChgread["AppliedAmount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["Distribute"]))
        //            {
        //                billInfo.Distribute = objChgread["Distribute"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["Provider_ID"]))
        //            {
        //                billInfo.Provider_id =Convert.ToInt32(objChgread["Provider_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(objChgread["FromReferenceType_ID"]))
        //            {
        //                billInfo.ReferenceType_ID = objChgread["FromReferenceType_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["ToReferenceType_ID"]))
        //            {
        //                billInfo.ToReferenceType_ID = objChgread["ToReferenceType_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["TransactionType"]))
        //            {
        //                billInfo.TransactionType = objChgread["TransactionType"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["PaymentType_ID"]))
        //            {
        //                billInfo.PaymentType_ID =Convert.ToInt32(objChgread["PaymentType_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(objChgread["InvoiceGenerated_Ind"]))
        //            {
        //                billInfo.InvoiceGenerated_Ind = objChgread["InvoiceGenerated_Ind"].ToString();
        //            }
                    
                    
        //            if (!DBNull.Value.Equals(objChgread["PatientLogin_ID"]))
        //            {
        //                billInfo.PatientLogin_ID = Convert.ToInt32(objChgread["PatientLogin_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(objChgread["ProviderName"]))
        //            {
        //                billInfo.ProviderName = objChgread["ProviderName"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["Notes"]))
        //            {
        //                billInfo.Notes = objChgread["Notes"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(objChgread["RoleName"]))
        //            {
        //                billInfo.RoleName = objChgread["RoleName"].ToString();
        //            }
                    
        //            if (!DBNull.Value.Equals(objChgread["patient_id"]))
        //            {
        //                billInfo.PatientID = objChgread["patient_id"].ToString();
        //            }

        //            PatPaybillList.Add(billInfo);
        //        }
        //        objChgread.NextResult();
        //        if (objChgread.HasRows)
        //        {
        //            while (objChgread.Read())
        //            {
        //                RPBilling.Totalnoofrecords = Convert.ToInt32(objChgread[0]);
        //            }
        //        }

        //        return PatPaybillList;

        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "GetPatientPaymentList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        //public List<RPBilling> GetPatientAdjustmenttList(RPBilling objAdj)
        //{
        //    try
        //    {
        //    clsCommonFunctions objcommon = new clsCommonFunctions();
        //    IDataParameter[] insAdjParam ={
        //                                          new SqlParameter("@in_PatientLogin_ID",objAdj.PatientLogin_ID),
        //                                          new SqlParameter("@in_ReferenceType_ID",objAdj.ReferenceType_ID),
        //                                          new SqlParameter("@in_StartDate",objAdj.FromDate),
        //                                          new SqlParameter("@in_EndDate",objAdj.ToDate),
        //                                          new SqlParameter("@In_AuthorizedpatientLogin_id",objAdj.AuthorizedPatientLoginID),
        //                                          new SqlParameter("@in_Provider_ID",objAdj.Provider_id),
        //                                          new SqlParameter("@in_NoofRecords",objAdj.NoOfRecords),
        //                                          new SqlParameter("@in_PageNo",objAdj.PageNo),
        //                                          new SqlParameter("@in_OrderBy",objAdj.orderBy),
        //                                          new SqlParameter("@in_OrderByitem",objAdj.OrderbyItem)
                                                  
        //                                     };
        //    objcommon.AddInParameters(insAdjParam);
        //    SqlDataReader objAdjRead = default(SqlDataReader);
        //    objAdjRead = objcommon.GetDataReader("Help_dbo.st_Billing_List_Adjustments_RDPAGING");
        //    List<RPBilling> PatAdjbillList = new List<RPBilling>();
        //    while (objAdjRead.Read())
        //    {
        //        RPBilling objadjCls = new RPBilling();
        //        if (!DBNull.Value.Equals(objAdjRead["Transaction_Date"]))
        //        {
        //            objadjCls.Transaction_Date = DateTime.Parse(objAdjRead["Transaction_Date"].ToString()).ToShortDateString();
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["Transaction_ID"]))
        //        {
        //            objadjCls.Transaction_ID = Convert.ToInt32(objAdjRead["Transaction_ID"]);
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["TransactionType_ID"]))
        //        {
        //            objadjCls.TransactionType_ID = objAdjRead["TransactionType_ID"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["Transaction_Amount"]))
        //        {
        //            string transAmount = string.Format("{0:c}", objAdjRead["Transaction_Amount"]);
        //            objadjCls.Transaction_Amount = transAmount;
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["BillableParty"]))
        //        {
                    
        //            objadjCls.BillableParty = objAdjRead["BillableParty"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["ChargeType"]))
        //        {

        //            objadjCls.ChargeType = objAdjRead["ChargeType"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["ProviderName"]))
        //        {

        //            objadjCls.BilledBy = objAdjRead["ProviderName"].ToString();
        //        }
        //        if (!DBNull.Value.Equals(objAdjRead["Notes"]))
        //        {
        //            objadjCls.Notes = objAdjRead["Notes"].ToString();
        //        }
        //        PatAdjbillList.Add(objadjCls);

        //    }
        //    objAdjRead.NextResult();
        //    if (objAdjRead.HasRows)
        //    {
        //        while (objAdjRead.Read())
        //        {
        //            RPBilling.Totalnoofrecords = Convert.ToInt32(objAdjRead[0]);
        //        }
        //    }
        //    return PatAdjbillList;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "GetPatientAdjustmenttList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public List<RPBilling> GetPaymentModes()
        {
            try
            {
                List<RPBilling> payModeList = new List<RPBilling>();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsPayMode = new DataSet();
                dsPayMode = objcommon.GetDataSet("Help_dbo.st_paymentmode_GET_PaymentModes");
                if (dsPayMode.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPayMode.Tables[0].Rows)
                    {
                        RPBilling objMode = new RPBilling();
                        if (!DBNull.Value.Equals(dr["PaymentMode_ID"]))
                        {
                            objMode.PaymentMode_ID = Convert.ToInt32(dr["PaymentMode_ID"]);
                        }
                        if (!DBNull.Value.Equals(dr["PaymentMode"]))
                        {
                            objMode.PaymentMode = dr["PaymentMode"].ToString();
                        }
                        payModeList.Add(objMode);

                    }
                }
                return payModeList.Where(p=>p.PaymentMode_ID!=5).ToList();
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetPaymentModes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public List<RPBilling> getAdjTypes()
        //{
        //    try
        //    {
        //        List<RPBilling> AdjtypeList = new List<RPBilling>();
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparam = { new SqlParameter("@in_role_id", null) };
        //        objcommon.AddInParameters(insparam);
        //        DataSet dsAdjtype = new DataSet();
        //        dsAdjtype = objcommon.GetDataSet("Help_dbo.st_Billing_List_AdjustmentTypes");
        //        if (dsAdjtype.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dsAdjtype.Tables[0].Rows)
        //            {
        //                RPBilling objAdjType = new RPBilling();
        //                if (!DBNull.Value.Equals(dr["AdjustmentType_ID"]))
        //                {
        //                    objAdjType.AdjustmentType_ID = Convert.ToInt32(dr["AdjustmentType_ID"]);
        //                }
        //                if (!DBNull.Value.Equals(dr["Title"]))
        //                {
        //                    objAdjType.Adj_Title = dr["Title"].ToString();
        //                }
        //                AdjtypeList.Add(objAdjType);

        //            }
        //        }
        //        return AdjtypeList;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "getAdjTypes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public RPBilling Del_ProviderPatientTransaction(RPBilling objtran, int Loginhistoryid)
        {
            try
            {

                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlst ={ 
                                            new SqlParameter("@In_Transaction_ID",objtran.Transaction_ID),
                                            new SqlParameter("@In_TransactionType_ID",objtran.TransactionType_ID),
                                            new SqlParameter("@in_UpdatedBy",objtran.UserId),
                                            new SqlParameter("@in_Loginhistory_id",Loginhistoryid),
                                       };
                objcommon.AddInParameters(insparamlst);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_DEL_ProviderPatientTransaction");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Del_ProviderPatientTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public DataSet GETPatientPayment_Details(string patientid,string practiceId)
        //{
        //    try
        //    {

           
        //    clsCommonFunctions objcommon = new clsCommonFunctions();
        //    IDataParameter[] insParamlist ={
        //                                       new SqlParameter("@in_Patient_ID",patientid),
        //                                       new SqlParameter("@in_Practice_ID",practiceId)
        //                                  };
        //    objcommon.AddInParameters(insParamlist);
        //    List<RPBilling> patPayDetails = new List<RPBilling>();
        //    DataSet dsPatinfo = new DataSet();
        //    dsPatinfo = objcommon.GetDataSet("Help_dbo.st_GET_PatientPayment_Details");
        //    return dsPatinfo;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "GETPatientPayment_Details", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public List<RPBilling> getUnpaidBalance(RPBilling patinfo)
        {
            try
            {

          
            clsCommonFunctions objcommons = new clsCommonFunctions();
            IDataParameter[] insaparam ={
                                          new SqlParameter("@In_reference_id",patinfo.Reference_id),
                                          new SqlParameter("@In_referencetype_id",patinfo.ReferenceType_ID),
                                          new SqlParameter("@in_Practice_ID",patinfo.Provider_id),
                                          new SqlParameter("@in_PatientLogin_ID",patinfo.PatientLogin_ID)
                                        };
            objcommons.AddInParameters(insaparam);
            List<RPBilling> patbalList = new List<RPBilling>();
            DataSet dspatBal = new DataSet();

            dspatBal = objcommons.GetDataSet("Help_dbo.st_Billing_GET_UnpaidBalance");
            if (dspatBal.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dspatBal.Tables[0].Rows)
                {
                    RPBilling patdet = new RPBilling();
                    if (!DBNull.Value.Equals(dr["Balance"]))
                    {
                        string bal = string.Empty;
                        if (Convert.ToDouble(dr["Balance"]) >= 0)
                        {
                            bal=string.Format("{0:c}",dr["Balance"]);
                            patdet.BalanceAmount = "(Unpaid balance : " + bal + ")";
                        }
                        else
                        {
                            bal = string.Format("{0:c}", dr["Balance"]);
                            bal = bal.Replace("(", "");
                            bal = bal.Replace(")","" );
                            patdet.BalanceAmount = "(Overpaid balance :" + bal + ")";
                        }
                       
                    }
                    if (!DBNull.Value.Equals(dr["patientlogin_id"]))
                    {
                        patdet.PatientLogin_ID = Convert.ToInt32(dr["patientlogin_id"]);
                    }
                    if (!DBNull.Value.Equals(dr["Patientname"]))
                    {
                        patdet.custname = Convert.ToString(dr["Patientname"]);
                    }
                    patdet.Customer_Email = Convert.ToString(dr["PatientEmail"]);
                    patdet.RegOndate = patinfo.RegOndate;
                  
                    patdet.patSInd = patinfo.patSInd;
                    patdet.patBillName = patinfo.patBillName;
                    patdet.Reference_id = patinfo.Reference_id;
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Patient_phoneno"])))
                    {
                        patdet.clientphone = Convert.ToString(dr["Patient_phoneno"]);
                    }
                    patbalList.Add(patdet);
                }
            }
            return patbalList;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getUnpaidBalance", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public int MakeTransactionlist(RPBilling pattran,int Loginhistoryid)
        //{
        //    try
        //    {
        //        int TransactionID = 0;
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparamlist ={
        //                                       new SqlParameter("@In_FromReference_ID",pattran.fromReferenceId),
        //                                       new SqlParameter("@In_FromReferenceType_ID",pattran.ReferenceType_ID),
        //                                       new SqlParameter("@In_ToReferenceType_ID",pattran.ToReferenceType_ID),
        //                                       new SqlParameter("@In_ToReference_ID",pattran.ToReference_ID),
        //                                       new SqlParameter("@In_BillingService_ID",pattran.BillingService_ID),
        //                                       new SqlParameter("@In_BillingChargetype_ID",pattran.BillingChargetype_ID),
        //                                       new SqlParameter("@In_Transaction_Date",pattran.Transaction_Date),
        //                                       new SqlParameter("@In_TransactionAmount",pattran.Transaction_Amount),
        //                                       new SqlParameter("@In_Notes",pattran.Notes),
        //                                       new SqlParameter("@in_Practice_ID",pattran.Practice_ID),
        //                                       new SqlParameter("@In_CreatedBy",pattran.UserId),
        //                                       new SqlParameter("@in_loginhistory_id",Loginhistoryid)
                                               
        //                                  };

        //        IDataParameter[] outParamlist = { 
        //                                    new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
        //                                    };
        //        objcommon.AddInParameters(insparamlist);
        //        objcommon.AddOutParameters(outParamlist);
        //        DataSet ds = new DataSet();
        //        ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_ProviderPatientChargeTransaction");
        //        if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
        //        {
        //            TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
        //        }
        //        return TransactionID;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "MakeTransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return 0;
        //}
        //public int MakePaymentTransactionlist(RPBilling pattran, int Loginhistoryid)
        //{
        //    try
        //    {
        //        int TransactionID = 0;
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparamlist ={
        //                                       new SqlParameter("@In_FromReference_ID",pattran.fromReferenceId),
        //                                       new SqlParameter("@In_FromReferenceType_ID",pattran.ReferenceType_ID),
        //                                       new SqlParameter("@In_ToReferenceType_ID",pattran.ToReferenceType_ID),
        //                                       new SqlParameter("@In_ToReference_ID",pattran.ToReference_ID),
        //                                       new SqlParameter("@In_BillingService_ID",pattran.BillingService_ID),
        //                                       new SqlParameter("@In_BillingChargetype_ID",pattran.BillingChargetype_ID),
        //                                       new SqlParameter("@In_Transaction_Date",pattran.Transaction_Date),
        //                                       new SqlParameter("@In_TransactionAmount",pattran.Transaction_Amount),
        //                                       new SqlParameter("@In_Notes",pattran.Notes),
        //                                       new SqlParameter("@In_PaymentType_ID",1),
        //                                       new SqlParameter("@in_Practice_ID",pattran.Practice_ID),
        //                                       new SqlParameter("@In_CreatedBy",pattran.UserId),
        //                                       new SqlParameter("@in_loginhistory_id",Loginhistoryid)
        //                                  };

        //        IDataParameter[] outParamlist = { 
        //                                    new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
        //                                    };
        //        objcommon.AddInParameters(insparamlist);
        //        objcommon.AddOutParameters(outParamlist);
        //        DataSet ds = new DataSet();
        //        ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_ProviderPatientPaymentTransaction");
        //        if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
        //        {
        //            TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
        //        }
        //        return TransactionID;
        //    }
        //    catch(Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "MakePaymentTransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return 0;
        //}
        //public int MakeAdjustmentTransactionlist(RPBilling pattran, int Loginhistoryid)
        //{
        //    try
        //    {
        //        int TransactionID = 0;
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparamlist ={
        //                                       new SqlParameter("@In_FromReference_ID",pattran.fromReferenceId),
        //                                       new SqlParameter("@In_FromReferenceType_ID",pattran.ReferenceType_ID),
        //                                       new SqlParameter("@In_ToReferenceType_ID",pattran.ToReferenceType_ID),
        //                                       new SqlParameter("@In_ToReference_ID",pattran.ToReference_ID),
        //                                       new SqlParameter("@in_PatientLogin_ID",pattran.PatientLogin_ID),
        //                                       new SqlParameter("@In_Transaction_Date",pattran.Transaction_Date),
        //                                       new SqlParameter("@in_Adjustmenttype_ID",pattran.AdjustmentType_ID),
        //                                       new SqlParameter("@In_TransactionType_ID",pattran.TransactionType_ID),
        //                                       new SqlParameter("@In_Transaction_Amount",pattran.Transaction_Amount),
        //                                       new SqlParameter("@In_Notes",pattran.Notes),
        //                                       new SqlParameter("@in_Practice_ID",pattran.Practice_ID),
        //                                       new SqlParameter("@In_CreatedBy",pattran.UserId),
        //                                       new SqlParameter("@in_loginhistory_id",Loginhistoryid)
        //                                  };

        //        IDataParameter[] outParamlist = { 
        //                                    new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
        //                                    };
        //        objcommon.AddInParameters(insparamlist);
        //        objcommon.AddOutParameters(outParamlist);
        //        DataSet ds = new DataSet();
        //        ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_ProviderPatientAdjustmentTransaction");
        //        if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
        //        {
        //            TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
        //        }
        //        return TransactionID;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "MakeAdjustmentTransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return 0;
        //}
        //public int MakePayCcTransaction(RPBilling pattran, int Loginhistoryid)
        //{
        //    try
        //    {
        //        int TransactionID = 0;
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparamlist ={
        //                                       new SqlParameter("@In_FromReference_ID",pattran.fromReferenceId),
        //                                       new SqlParameter("@In_FromReferenceType_ID",pattran.ReferenceType_ID),
        //                                       new SqlParameter("@In_ToReferenceType_ID",pattran.ToReferenceType_ID),
        //                                       new SqlParameter("@In_ToReference_ID",pattran.ToReference_ID),
        //                                       new SqlParameter("@In_BillingService_ID",pattran.BillingService_ID),
        //                                       new SqlParameter("@In_BillingChargetype_ID",pattran.BillingChargetype_ID),
        //                                       new SqlParameter("@In_Transaction_Date",pattran.Transaction_Date),
        //                                       new SqlParameter("@In_TransactionAmount",pattran.Transaction_Amount),
        //                                       new SqlParameter("@In_Notes",pattran.Notes),
        //                                       new SqlParameter("@in_Practice_ID",pattran.Practice_ID),
        //                                       new SqlParameter("@In_CreatedBy",pattran.UserId),
        //                                       new SqlParameter("@in_CCTransaction_ID",pattran.CCTransactionID),
        //                                       new SqlParameter("@In_IsCCTransChargeToprv", pattran.IsCCchargetoPrv),
        //                                       new SqlParameter("@in_loginhistory_id",Loginhistoryid),
        //                                       new SqlParameter("@in_authorizationnumber",pattran.Authorizednumber)
        //                                  };

        //        IDataParameter[] outParamlist = { 
        //                                    new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
        //                                    };
        //        objcommon.AddInParameters(insparamlist);
        //        objcommon.AddOutParameters(outParamlist);
        //        DataSet ds = new DataSet();
        //        ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_ProviderPatientCCPaymentTransaction");
        //        if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
        //        {
        //            TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
        //        }
        //        return TransactionID;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "MakePayCcTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return 0;
        //}
        //public int Ins_ChequePayment(RPBilling patchkTran, int Loginhistoryid)
        //{
        //    try
        //    {
        //        int TransactionID = 0;
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparam ={
        //                                   new SqlParameter("@in_Practice_ID",patchkTran.Practice_ID),
        //                                   new SqlParameter("@in_PlaceofService_Id",patchkTran.Facility_ID),
        //                                   new SqlParameter("@in_Patient_ID",patchkTran.PatientID),
        //                                   new SqlParameter("@in_Provider_ID",patchkTran.Provider_id),
        //                                   new SqlParameter("@in_Reference_ID",patchkTran.ToReference_ID),
        //                                   new SqlParameter("@in_ReferenceType_ID",patchkTran.ToReferenceType_ID),
        //                                   new SqlParameter("@in_PaymentMode",patchkTran.PaymentMode),
        //                                   new SqlParameter("@in_TransactionDate",patchkTran.Transaction_Date),
        //                                   new SqlParameter("@in_Amount",patchkTran.Transaction_Amount),
        //                                   new SqlParameter("@in_CheckNumber",patchkTran.ChecksNo),
        //                                   new SqlParameter("@in_CheckDate", patchkTran.CheckDate),
        //                                   new SqlParameter("@in_IssuedBank",patchkTran.BankName),
        //                                   new SqlParameter("@in_PaymentNotes",patchkTran.Notes),
        //                                   new SqlParameter("@in_CreatedBy",patchkTran.UserId),
        //                                   new SqlParameter("@in_loginhistory_id",Loginhistoryid),
        //                              };
        //        objcommon.AddInParameters(insparam);
        //        IDataParameter[] outparamlist = {
        //                                        new SqlParameter("@out_PaymentTransaction_ID",SqlDbType.Int) 
        //                                    };
        //        objcommon.AddOutParameters(outparamlist);
        //        DataSet ds = new DataSet();
        //        ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_PatientBehalfPayment");
        //        if (objcommon.objdbCommandWrapper.Parameters["@out_PaymentTransaction_ID"].Value != null)
        //        {
        //            TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_PaymentTransaction_ID"].Value);
        //        }
        //        return TransactionID;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "Ins_ChequePayment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return 0;
        //}
        //public List<RPBilling> viewpaymentdistrib(RPBilling objpayd)
        //{
        //    try
        //    {
        //        List<RPBilling> objrpbillList = new List<RPBilling>();
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparam ={
        //                                   new SqlParameter("@in_Transaction_ID",objpayd.Transaction_ID),
        //                                   new SqlParameter("@in_FromReference_ID",objpayd.fromReferenceId),
        //                                   new SqlParameter("@in_FromReferencetype_ID",objpayd.ReferenceType_ID),
        //                                   new SqlParameter("@in_ToReference_ID",objpayd.ToReference_ID),
        //                                   new SqlParameter("@in_ToReferenceType_ID",objpayd.ToReferenceType_ID),
        //                                   new SqlParameter("@in_PatientLogin_ID",objpayd.PatientLogin_ID),
        //                                   new SqlParameter("@in_Ind",objpayd.Ind)
        //                              };
        //        objcommon.AddInParameters(insparam);
        //        SqlDataReader datareader = default(SqlDataReader);
        //        datareader = objcommon.GetDataReader("Help_dbo.st_Billing_List_DistributingPaymentTransaction");
        //        while (datareader.Read())
        //        {
        //            RPBilling objbill = new RPBilling();
        //            if (!DBNull.Value.Equals(datareader["Transaction_Date"]))
        //            {
        //                objbill.Transaction_Date = datareader["Transaction_Date"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["Transaction_Amount"]))
        //            {
        //                objbill.Transaction_Amount = datareader["Transaction_Amount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["SessionTotal"]))
        //            {
        //                objbill.SessionTotal = datareader["SessionTotal"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["DistributedoRAppliedAmount"]))
        //            {
        //                objbill.DistributedoRAppliedAmount = datareader["DistributedoRAppliedAmount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["BalanceAmount"]))
        //            {
        //                objbill.BalanceAmount = datareader["BalanceAmount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["AppliedAmount"]))
        //            {
        //                objbill.AppliedAmount = datareader["AppliedAmount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["ApplicableDistributedAmount"]))
        //            {
        //                objbill.ApplicableDistributedAmount = datareader["ApplicableDistributedAmount"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["Distribution_ID"]))
        //            {
        //                objbill.Distribution_ID = datareader["Distribution_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["PatientLogin_ID"]))
        //            {
        //                objbill.PatientLogin_ID = Convert.ToInt32(datareader["PatientLogin_ID"]);
        //            }
        //            if (!DBNull.Value.Equals(datareader["ChargeType"]))
        //            {
        //                objbill.ChargeType = datareader["ChargeType"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["chargeTransaction_ID"]))
        //            {
        //                objbill.chargeTransaction_ID = datareader["chargeTransaction_ID"].ToString();
        //            }
        //            if (!DBNull.Value.Equals(datareader["PaymentTransaction_ID"]))
        //            {
        //                objbill.PaymentTransaction_ID = datareader["PaymentTransaction_ID"].ToString();
        //            }
        //            objrpbillList.Add(objbill);
        //        }
        //        return objrpbillList;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "viewpaymentdistrib", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        //public DataSet viewpaymentAutodistrib(RPBilling objpayd)
        //{
        //    try
        //    {
        //        DataSet dsviewDist = new DataSet();
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparam ={
        //                                    new SqlParameter("@in_Transaction_ID",objpayd.Transaction_ID),
        //                                   new SqlParameter("@in_FromReference_ID",objpayd.fromReferenceId),
        //                                   new SqlParameter("@in_FromReferencetype_ID",objpayd.ReferenceType_ID),
        //                                   new SqlParameter("@in_ToReference_ID",objpayd.ToReference_ID),
        //                                   new SqlParameter("@in_ToReferenceType_ID",objpayd.ToReferenceType_ID),
        //                                   new SqlParameter("@in_PatientLogin_ID",objpayd.PatientLogin_ID),
        //                                   new SqlParameter("@in_Ind",objpayd.Ind)
        //                              };
        //        objcommon.AddInParameters(insparam);
        //        dsviewDist = objcommon.GetDataSet("Help_dbo.st_Billing_List_DistributingPaymentTransaction");
        //        return dsviewDist;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "viewpaymentAutodistrib", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        //public string Get_UnAppliedAmount(RPBilling patunApplied)
        //{
        //    try
        //    {
        //        string strTotal = string.Empty;
        //        clsCommonFunctions objcmn = new clsCommonFunctions();
        //        IDataParameter[] insparamlist ={
        //                                       new SqlParameter("@in_FromReferenceType_ID",patunApplied.ReferenceType_ID),
        //                                       new SqlParameter("@in_FromReference_ID",patunApplied.fromReferenceId),
        //                                       new SqlParameter("@in_ToReference_ID",patunApplied.ToReference_ID),
        //                                       new SqlParameter("@in_ToReferenceType_ID",patunApplied.ToReferenceType_ID)
        //                                  };
        //        objcmn.AddInParameters(insparamlist);
        //        SqlDataReader dr = default(SqlDataReader);
        //        dr = objcmn.GetDataReader("Help_dbo.st_Billing_Get_unappliedamount");
        //        while (dr.Read())
        //        {
        //            if (!DBNull.Value.Equals(dr["Amount"]))
        //            {
        //                strTotal = dr["Amount"].ToString();
        //            }
        //            else
        //            {
        //                strTotal = null;
        //            }
        //        }

        //        return strTotal;
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "Get_UnAppliedAmount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        //public DataSet ShowChargeAppliedAmount(RPBilling Showchgdetail)
        //{
        //    try
        //    {
        //    DataSet dschgdetail = new DataSet();
        //    clsCommonFunctions objcommon = new clsCommonFunctions();
        //    IDataParameter[] insparam = {
        //                                new SqlParameter("@in_ChargeTransactionID",Showchgdetail.Transaction_ID)
        //                                };
        //    objcommon.AddInParameters(insparam);
        //    dschgdetail = objcommon.GetDataSet("Help_dbo.st_billing_view_ChargeAppliedPayments");
        //    return dschgdetail;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "ShowChargeAppliedAmount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        //public DataSet showPayAmount(RPBilling showPaydeatails)
        //{
        //    try
        //    {
        //        clsCommonFunctions objcommon = new clsCommonFunctions();
        //        IDataParameter[] insparams = { new SqlParameter("@in_PaymentTransactionID",showPaydeatails.Transaction_ID)};
        //        objcommon.AddInParameters(insparams);
        //        DataSet dsPaydetail = new DataSet();
        //        dsPaydetail = objcommon.GetDataSet("Help_dbo.st_billing_view_PaymentAppliedPayments");
        //        return dsPaydetail;

        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "showPayAmount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public DataSet showtransactiondetails(RPBilling showPaydeatails)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams = { new SqlParameter("@in_ChargepaymentTransactionID", showPaydeatails.Transaction_ID) };
                objcommon.AddInParameters(insparams);
                DataSet dsPaydetail = new DataSet();
                dsPaydetail = objcommon.GetDataSet("Help_dbo.st_billing_view_Chargepayment");
                return dsPaydetail;

            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "showPayAmount", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public DataSet showAdjustDetails(RPBilling showAdjDetails)
        //{
        //    try
        //    {
        //        clsCommonFunctions objcomon = new clsCommonFunctions();
        //        IDataParameter[] insparas = { 
        //                                    new SqlParameter("@in_Transaction_ID",showAdjDetails.Transaction_ID),
        //                                    new SqlParameter("@in_TransactionType_ID",showAdjDetails.TransactionType_ID)
        //                                    };
        //        objcomon.AddInParameters(insparas);
        //        DataSet dsAdjustment = new DataSet();
        //        dsAdjustment = objcomon.GetDataSet("Help_dbo.st_billing_view_TransactionsDistributionDetail");
        //        return dsAdjustment;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "showAdjustDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public void Edit_Payment_charges(RPBilling editPAy, int Loginhistoryid,ref string Out_Msg)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamEdit = {
                                                new  SqlParameter("@in_Transaction_ID",editPAy.Transaction_ID),
                                                new  SqlParameter("@in_Transaction_Date",editPAy.Transaction_Date),
                                                new  SqlParameter("@in_Amount",editPAy.BalanceAmount),
                                                new  SqlParameter("@in_Notes",editPAy.Notes),
                                                new  SqlParameter("@in_UpdatedBy",editPAy.UserId),
                                                new  SqlParameter("@in_Ind",editPAy.Ind),
                                                new  SqlParameter("@in_loginhistory_id",Loginhistoryid),
                                                new  SqlParameter("@in_Authorizationnumber",editPAy.Authorizednumber),
                                                new  SqlParameter("@in_email",editPAy.Customer_Email),
                                                new  SqlParameter("@in_emailcheck",editPAy.emailcheck),
                                                new SqlParameter("@in_ChequeNo",editPAy.ChecksNo)
                                                };

                IDataParameter[] OutParam = { new SqlParameter("@out_Msg", SqlDbType.VarChar, 500) };
                objcommon.AddInParameters(insparamEdit);
                objcommon.AddOutParameters(OutParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_billing_edit_Charge");
                                if (objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value != null)
                {
                    Out_Msg = objcommon.GetCurrentCommand.Parameters["@Out_Msg"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Edit_Payment_charges", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
           
        }
        //public RPBilling Ins_Disttransaction(RPBilling paydis, int Loginhistoryid)
        //{
        //    try
        //    {
        //    clsCommonFunctions objclscommon = new clsCommonFunctions();
        //    IDataParameter[] insparamlist = {
        //                                    new SqlParameter("@in_Practice_ID",null),
        //                                    new SqlParameter("@in_ChargeTransaction_ID",paydis.chargeTransaction_ID),
        //                                    new SqlParameter("@in_PaymentTransaction_ID",paydis.PaymentTransaction_ID),
        //                                     new SqlParameter("@in_AppliedAmount",paydis.AppliedAmount),
        //                                    new SqlParameter("@in_CreatedBy",paydis.UserId),
        //                                    new SqlParameter("@in_Loginhistory_id",Loginhistoryid)
        //                                    };
        //    objclscommon.AddInParameters(insparamlist);
        //    objclscommon.ExecuteNonQuerySP("Help_dbo.st_Billing_INS_DistributionTransaction");

        //    return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, ClassName, "Ins_Disttransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return null;
        //}
        public DataSet patientwiseDaysoutstanding(RPBilling daysinfo,int typeid)
        {
            try
            {
            clsCommonFunctions objclsdays = new clsCommonFunctions();
            IDataParameter[] insdayparams = { 
                                            new SqlParameter("@In_PatientLogin_id",daysinfo.PatientLogin_ID),
                                            new SqlParameter("@In_Formreference_id",daysinfo.fromReferenceId),
                                            new SqlParameter("@In_Practice_Id",daysinfo.Practice_ID),
                                            new SqlParameter("@in_Startdate",daysinfo.FromDate),
                                            new SqlParameter("@in_Enddate",daysinfo.ToDate),
                                            new SqlParameter("@in_type_id",typeid)
                                            };
            objclsdays.AddInParameters(insdayparams);
            DataSet dsDaysinfo = new DataSet();
            dsDaysinfo = objclsdays.GetDataSet("Help_dbo.st_patient_get_daysoutstanding_patientwise");
            return dsDaysinfo;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "patientwiseDaysoutstanding", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;

        }
        public DataSet PracticeAdminSummary(RPBilling prSummary)
        {
            try
            {
            DataSet dsSummary = new DataSet();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam = { 
                                        new SqlParameter("@in_Provider_id",prSummary.Provider_id),
                                        new SqlParameter("@In_FromDate",prSummary.FromDate),
                                        new SqlParameter("@In_ToDate",prSummary.ToDate),
                                        new SqlParameter("@in_ToReferenceType_ID",prSummary.ToReferenceType_ID),
                                        new SqlParameter("@in_ToReference_ID",prSummary.ToReference_ID)
                                        };
            objcommon.AddInParameters(insparam);
            dsSummary= objcommon.GetDataSet("Help_dbo.st_Billing_Get_PracticeAdminsummary");
            return dsSummary;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "PracticeAdminSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> Get_PracticeUserTrasnactions(RPBilling prTrans)
        {
            try
            {
                List<RPBilling> TranlistPr = new List<RPBilling>();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_TransactionType_IDs",prTrans.TransactionTypeIDs),
                                        new SqlParameter("@in_FromReferenceType_ID",prTrans.ReferenceType_ID),
                                        new SqlParameter("@in_FromReference_ID",prTrans.fromReferenceId),
                                        new SqlParameter("@in_Provider_id",prTrans.Provider_id),
                                       // new SqlParameter("@in_Facility_ID",prTrans.Facility_ID),
                                        new SqlParameter("@in_ToReferenceType_ID",prTrans.ToReferenceType_ID),
                                        new SqlParameter("@in_ToReference_ID",prTrans.ToReference_ID),
                                        new SqlParameter("@In_Fromdate",prTrans.FromDate),
                                        new SqlParameter("@In_Todate",prTrans.ToDate),
                                       // new SqlParameter("@In_Service_ID",prTrans.ServiceID),
                                        new SqlParameter("@IN_OrderByItem",prTrans.OrderbyItem),
                                        new SqlParameter("@In_OrderBy",prTrans.orderBy),
                                        new SqlParameter("@In_NoOfRecords",prTrans.NoOfRecords),
                                        new SqlParameter("@In_PageNo",prTrans.PageNo)
                                        };
                objcommon.AddInParameters(insparam);
                SqlDataReader drTran = default(SqlDataReader);
                drTran = objcommon.GetDataReader("Help_dbo.st_Billing_LIST_PracticeUserTrasnactions_RDPAGING_VER5");
                while (drTran.Read())
                {
                    RPBilling tranInfo = new RPBilling();
                    if (!DBNull.Value.Equals(drTran["Transaction_Date"]))
                    {
                        tranInfo.Transaction_Date = drTran["Transaction_Date"].ToString();
                    }
                    if (!DBNull.Value.Equals(drTran["ServiceName"]))
                    {
                        tranInfo.servicename = drTran["ServiceName"].ToString();
                    }
                    if (prTrans.TransactionTypeIDs != "2" && prTrans.TransactionTypeIDs != "3,4")
                    {
                        if (!DBNull.Value.Equals(drTran["BillableBy"]))
                        {
                            tranInfo.BilledBy = drTran["BillableBy"].ToString();
                        }
                    }
                    if (!DBNull.Value.Equals(drTran["Notes"]))
                    {
                        tranInfo.Notes = drTran["Notes"].ToString();
                    }
                    if (!DBNull.Value.Equals(drTran["Transaction_Amount"]))
                    {
                        string transAmount = string.Format("{0:c}", drTran["Transaction_Amount"]);

                        tranInfo.Transaction_Amount = transAmount;
                    }
                    if (!DBNull.Value.Equals(drTran["PaidBy"]))
                    {
                        tranInfo.PaidByName = drTran["PaidBy"].ToString();
                    }
                    if (!DBNull.Value.Equals(drTran["PaymentMode"]))
                    {
                        tranInfo.PaymentMode = drTran["PaymentMode"].ToString();
                    }
                    if (!DBNull.Value.Equals(drTran["TransactionType"]))
                    {
                        tranInfo.TransactionType = drTran["TransactionType"].ToString();
                    }

                    TranlistPr.Add(tranInfo);
                }
                drTran.NextResult();
                if (drTran.HasRows)
                {
                    while (drTran.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(drTran[0]);
                    }
                }
                drTran.NextResult();
                if (drTran.HasRows)
                {
                    RPBilling billInfo1 = new RPBilling();
                    while (drTran.Read())
                    {
                        billInfo1.GrandTotal = drTran["GrandTotal"].ToString();
                        billInfo1.ServiceTaxTotal = drTran["ServiceTaxTotal"].ToString();
                        billInfo1.Total = drTran["Total"].ToString();
                    }
                }
                return TranlistPr;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_PracticeUserTrasnactions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> GET_Invoice(RPBilling prInvoice)
        {
            try
            {
                List<RPBilling> invoiceList = new List<RPBilling>();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@In_FromReference_ID", prInvoice.fromReferenceId),
                                           new SqlParameter("@In_FromReferenceType_ID",prInvoice.ReferenceType_ID),
                                           new SqlParameter("@in_ToReference_ID",prInvoice.ToReference_ID),
                                           new SqlParameter("@in_ToReferenceType_ID",prInvoice.ToReferenceType_ID),
                                           new SqlParameter("@in_Invoice_Date",prInvoice.Invoice_Date),
                                           new SqlParameter("@in_Orderby",prInvoice.orderBy),
                                           new SqlParameter("@in_OrderbyItem",prInvoice.OrderbyItem),
                                           new SqlParameter("@in_Start_Date",prInvoice.FromDate),
                                           new SqlParameter("@in_End_Date",prInvoice.ToDate),
                                           new SqlParameter("@in_Invoice_ID",prInvoice.Invoice_ID),
                                           new SqlParameter("@in_NoofRecords",prInvoice.NoOfRecords),
                                           new SqlParameter("@In_PageNo",prInvoice.PageNo),
                                           new SqlParameter("@in_Provider_ID",prInvoice.Practice_ID)
                                      };
                objcommon.AddInParameters(insparam);
                SqlDataReader drInvoice = default(SqlDataReader);
                drInvoice = objcommon.GetDataReader("Help_dbo.st_Billing_GET_Invoice_rdPaging");
                if (drInvoice.HasRows)
                {
                    while (drInvoice.Read())
                    {
                        RPBilling Invoiceinfo = new RPBilling();
                        if (!DBNull.Value.Equals(drInvoice["Invoice_ID"]))
                        {
                            Invoiceinfo.Invoice_ID = Convert.ToInt32(drInvoice["Invoice_ID"]);
                        }
                        if (!DBNull.Value.Equals(drInvoice["InvoiceNo"]))
                        {
                            Invoiceinfo.InvoiceNo = drInvoice["InvoiceNo"].ToString();
                        }
                        if (!DBNull.Value.Equals(drInvoice["Invoice_Date"]))
                        {
                            Invoiceinfo.Invoice_Date = drInvoice["Invoice_Date"].ToString();
                        }
                        if (!DBNull.Value.Equals(drInvoice["Begin_Date"]))
                        {
                            Invoiceinfo.FromDate = drInvoice["Begin_Date"].ToString();
                        }
                        if (!DBNull.Value.Equals(drInvoice["End_Date"]))
                        {
                            Invoiceinfo.ToDate = drInvoice["End_Date"].ToString();
                        }
                        if (!DBNull.Value.Equals(drInvoice["InvoiceCharges"]))
                        {
                            string InsChag = string.Format("{0:c}", drInvoice["InvoiceCharges"]);
                            Invoiceinfo.InvoiceCharges = InsChag;
                        }
                        if (!DBNull.Value.Equals(drInvoice["InvoicePayments"]))
                        {
                            string InsPay = string.Format("{0:c}", drInvoice["InvoicePayments"]);
                            Invoiceinfo.InvoicePaments = InsPay;
                        }
                        if (!DBNull.Value.Equals(drInvoice["PrevAdvance"]))
                        {
                            string inspAdvance = string.Format("{0:c}", drInvoice["PrevAdvance"]);
                            Invoiceinfo.PrevAdvance = inspAdvance;
                        }
                        if (!DBNull.Value.Equals(drInvoice["InvoiceBalance"]))
                        {
                            string insBal = string.Format("{0:c}", drInvoice["InvoiceBalance"]);
                            Invoiceinfo.InvoiceBalance = insBal;
                        }
                        if (!DBNull.Value.Equals(drInvoice["PrevBalance"]))
                        {
                            string insPbal = string.Format("{0:c}", drInvoice["PrevBalance"]);
                            Invoiceinfo.PrevBalance = insPbal;
                        }
                        invoiceList.Add(Invoiceinfo);

                    }
                }
                drInvoice.NextResult();
                if (drInvoice.HasRows)
                {
                    if (drInvoice.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(drInvoice[0]);
                    }
                }
                return invoiceList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GET_Invoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> Get_AdmintopracticeInvoiceReciept(RPBilling prReciept)
        {
            try
            {
                List<RPBilling> ReieptList = new List<RPBilling>();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_FromReference_ID", prReciept.fromReferenceId),
                                           new SqlParameter("@in_FromReferenceType_ID",prReciept.ReferenceType_ID),
                                           new SqlParameter("@in_ToReference_ID",prReciept.ToReference_ID),
                                           new SqlParameter("@in_ToReferenceType_ID",prReciept.ToReferenceType_ID),
                                           new SqlParameter("@in_Invoice_Date",prReciept.Invoice_Date),
                                           new SqlParameter("@in_Orderby",prReciept.orderBy),
                                           new SqlParameter("@in_OrderbyItem",prReciept.OrderbyItem),
                                           new SqlParameter("@in_Start_Date",prReciept.FromDate),
                                           new SqlParameter("@in_End_Date",prReciept.ToDate),
                                           new SqlParameter("@in_Invoice_ID",prReciept.Invoice_ID),
                                           new SqlParameter("@In_NoOfRecords",prReciept.NoOfRecords),
                                           new SqlParameter("@In_PageNo",prReciept.PageNo),
                                           new SqlParameter("@In_Provider_ID",prReciept.Practice_ID)
                                      };
                objcommon.AddInParameters(insparam);
                SqlDataReader drReciept = default(SqlDataReader);
                drReciept = objcommon.GetDataReader("Help_dbo.st_Billing_GET_AdminToPracticeInvoiceReceipt_RDPaging");
                if (drReciept.HasRows)
                {
                    while (drReciept.Read())
                    {
                        RPBilling Recieptinfo = new RPBilling();
                        if (!DBNull.Value.Equals(drReciept["RecieptNo"]))
                        {
                            Recieptinfo.RecieptNo = drReciept["RecieptNo"].ToString();
                        }
                        if (!DBNull.Value.Equals(drReciept["Invoice_ID"]))
                        {
                            Recieptinfo.Invoice_ID = Convert.ToInt32(drReciept["Invoice_ID"]);
                        }
                        if (!DBNull.Value.Equals(drReciept["Invoice_Date"]))
                        {
                            Recieptinfo.Invoice_Date = DateTime.Parse(drReciept["Invoice_Date"].ToString()).ToShortDateString();
                        }
                        if (!DBNull.Value.Equals(drReciept["Begin_Date"]))
                        {
                            Recieptinfo.FromDate = DateTime.Parse(drReciept["Begin_Date"].ToString()).ToShortDateString();
                        }
                        if (!DBNull.Value.Equals(drReciept["End_Date"]))
                        {
                            Recieptinfo.ToDate = DateTime.Parse(drReciept["End_Date"].ToString()).ToShortDateString();
                        }
                        if (!DBNull.Value.Equals(drReciept["InvoiceCharges"]))
                        {
                            string InsChag = string.Format("{0:c}", drReciept["InvoiceCharges"]);
                            Recieptinfo.InvoiceCharges = InsChag;
                        }
                        if (!DBNull.Value.Equals(drReciept["InvoicePayments"]))
                        {
                            string InsPay = string.Format("{0:c}", drReciept["InvoicePayments"]);
                            Recieptinfo.InvoicePaments = InsPay;
                        }
                        if (!DBNull.Value.Equals(drReciept["PrevAdvance"]))
                        {
                            string inspAdvance = string.Format("{0:c}", drReciept["PrevAdvance"]);
                            Recieptinfo.PrevAdvance = inspAdvance;
                        }
                        if (!DBNull.Value.Equals(drReciept["PrevBalance"]))
                        {
                            string insPbal = string.Format("{0:c}", drReciept["PrevBalance"]);
                            Recieptinfo.PrevBalance = insPbal;
                        }
                        ReieptList.Add(Recieptinfo);

                    }
                }
                drReciept.NextResult();
                if (drReciept.HasRows)
                {
                    if (drReciept.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(drReciept[0]);
                    }
                }
                return ReieptList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_AdmintopracticeInvoiceReciept", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getPatientFileDayout(int patLiogn,int providerId,string startdate,string endDate)
        {
            try
            {
                DataSet dsbil = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_patientlogin_id",patLiogn),
                                           new SqlParameter("@in_provider_id",providerId),
                                           new SqlParameter("@in_StartDate",startdate),
                                           new SqlParameter("@in_EndDate",endDate)
                                      };
                objcommon.AddInParameters(insparam);
                dsbil = objcommon.GetDataSet("Help_dbo.St_Provider_Patient_get_daysoutstanding");
                return dsbil;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getPatientFileDayout", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string GetPatientLoginID(string PatientID)
        {
            try
            {
                SqlDataReader dr;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = {
                                                new SqlParameter("@in_Patient_Id", (PatientID != null ? PatientID : null))
                                             };
                objcommon.AddInParameters(objparam);
                dr = objcommon.GetDataReader("Help_dbo.st_Patient_GetLoginid");
                while (dr.Read())
                {
                    if (dr["PatientLoginid"] != null)
                    {
                        return dr["PatientLoginid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetPatientLoginID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet Get_Patientfiledetails(Billingreports obj)
        {
            DataSet dsset = default(DataSet);
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
               
                IDataParameter[] objparam = {
		new SqlParameter("@in_PatientLogin_id", (obj.PatientLoginID != null ? obj.PatientLoginID : null)),
		new SqlParameter("@In_Practice_id", (obj.PracticeID !=null ? obj.PracticeID : null)),
		new SqlParameter("@in_Fromdate", (!string.IsNullOrEmpty(obj.Fromdate) ? obj.Fromdate : null)),
		new SqlParameter("@in_Todate", (!string.IsNullOrEmpty(obj.Todate) ? obj.Todate : null)),
		new SqlParameter("@in_status", (!string.IsNullOrEmpty(obj.Status) ? obj.Status : null)),
		new SqlParameter("@in_provider_ID", (obj.ProviderID != null ? obj.ProviderID : null))
	};
                objcommon.AddInParameters(objparam);
                dsset = objcommon.GetDataSet("Help_dbo.St_Patient_get_PatientProfilereportdetails");
               
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "billingDataaccesslayer", "Get_Patientfiledetails", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return dsset;
        }
        public List<Billingreports> Get_PrintBillMessages()
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                SqlDataReader objread = default(SqlDataReader);
                List<Billingreports> objList = new List<Billingreports>();
                objread = objcommon.GetDataReader("Help_dbo.st_PrintBillMessages_GET");
                while (objread.Read())
                {
                    Billingreports objsess = new Billingreports();
                    objsess.MessageOnBill_ID = objread["MessageOnBill_ID"]!=null ? objread["MessageOnBill_ID"].ToString() : null;
                    objsess.MessageOnBill = objread["MessageOnBill"] != null ? objread["MessageOnBill"].ToString() : null; 
                    objList.Add(objsess);
                }
                return objList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_PrintBillMessages", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
//        public List<Billingreports> Get_BillablePartyList(Billingreports obj)
//        {
//            try
//            {
//                clsCommonFunctions objcommon = new clsCommonFunctions();
//                SqlDataReader objread = default(SqlDataReader);
//                List<Billingreports> objList = new List<Billingreports>();
//                IDataParameter[] ObjParam = {
//    new SqlParameter("@in_Patient_ID", obj.PatientID != null ? obj.PatientID : null),
//    new SqlParameter("@in_Practice_ID", obj.PracticeID != null ? obj.PracticeID : null),
//    new SqlParameter("@In_Provider_id", obj.ProviderID != null ? obj.ProviderID : null),
//    new SqlParameter("@in_ind", obj.Ind != null ? obj.Ind : null),
//    new SqlParameter("@In_Claim_ID", obj.Claim_ID != null ? obj.Claim_ID : null)
//};
//                objcommon.AddInParameters(ObjParam);
//                objread = objcommon.GetDataReader("Help_dbo.st_GET_PatientPayment_Details");
//                while (objread.Read())
//                {
//                    Billingreports objsess = new Billingreports();
//                    objsess.Name = objread["Name"] != null ? objread["Name"].ToString() : null;
//                    objsess.Type = objread["Type"] != null ? objread["Type"].ToString() : null;
//                    objsess.ID = objread["ID"] != null ? objread["ID"].ToString() : null;
//                    objsess.Status_Ind = objread["Status_Ind"] != null ? objread["Status_Ind"].ToString() : null;
//                    objList.Add(objsess);
//                }
//                return objList;
//            }
//            catch (Exception ex)
//            {
//                clsExceptionLog clsex = new clsExceptionLog();
//                clsex.LogException(ex, ClassName, "Get_BillablePartyList", HttpContext.Current.Request, HttpContext.Current.Session);
//            }
//            return null;
//        }
        public List<PatientBalance> Get_PrintBillstatement(PatientBalance objbilling)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                int i = 0;
                int j = 0;
                double Charges = 0.0;
                double Payments = 0.0;
                List<PatientBalance> objlist = new List<PatientBalance>();
                DataSet dsbill = new DataSet();
                IDataParameter[] objparam = {
		new SqlParameter("@in_Patient_ID", (objbilling.PatientLoginID != null ? objbilling.PatientLoginID : null)),
		new SqlParameter("@in_PaidBy_ID", (objbilling.TorefID != null ? objbilling.TorefID : null)),
		new SqlParameter("@in_PaidBy", (!string.IsNullOrEmpty(objbilling.PaidBy) ? objbilling.PaidBy : null)),
		new SqlParameter("@in_PaidByPlanID", (objbilling.PlanID != null ? objbilling.PlanID : null)),
		new SqlParameter("@in_StartDate", (!string.IsNullOrEmpty(objbilling.FromDate) ? objbilling.FromDate : null)),
		new SqlParameter("@in_EndDate", (!string.IsNullOrEmpty(objbilling.ToDate) ? objbilling.ToDate : null)),
		new SqlParameter("@in_Billingservice_ID", (objbilling.ServiceID != 0 ? objbilling.ServiceID : null)),
		new SqlParameter("@in_Practice_ID", (objbilling.PracticeID != null ? objbilling.PracticeID : null)),
		new SqlParameter("@in_Provider_ID", (objbilling.ProviderID != null ? objbilling.ProviderID : null))
	};
                objcommon.AddInParameters(objparam);
                dsbill = objcommon.GetDataSet("Help_dbo.st_Report_Patient_BillingStatement");
                if ((dsbill != null))
                {
                    PatientBalance objbill = new PatientBalance();
                    if (dsbill.Tables[3].Rows.Count > 0)
                    {
                        PatientBalance objbils = new PatientBalance();
                        if (dsbill.Tables[3].Rows[0]["PaidByName"].ToString()!="")
                        {
                            objbils.PaidBy = dsbill.Tables[3].Rows[0]["PaidByName"].ToString();
                        }
                        else
                        {
                            objbils.PaidBy = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderName"].ToString()!="")
                        {
                            objbils.ProviderName = dsbill.Tables[3].Rows[0]["ProviderName"].ToString();
                        }
                        if (dsbill.Tables[3].Rows[0]["PracticeName"].ToString()!="")
                        {
                            objbils.PracticeName = dsbill.Tables[3].Rows[0]["PracticeName"].ToString();
                        }
                        if (dsbill.Tables[3].Rows[0]["PaidByCity"].ToString()!="")
                        {
                            objbils.PaidByCity = dsbill.Tables[3].Rows[0]["PaidByCity"].ToString();
                        }
                        else
                        {
                            objbils.PaidByCity = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["PaidByAddress1"].ToString()!="")
                        {
                            objbils.PaidByAddress1 = dsbill.Tables[3].Rows[0]["PaidByAddress1"].ToString();
                        }
                        else
                        {
                            objbils.PaidByAddress1 = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["PaidByAddress2"].ToString()!="")
                        {
                            objbils.PaidByAddress2 = dsbill.Tables[3].Rows[0]["PaidByAddress2"].ToString();
                        }
                        else
                        {
                            objbils.PaidByAddress2 = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["PatientName"].ToString() != "")
                        {
                            objbils.PatientName = dsbill.Tables[3].Rows[0]["PatientName"].ToString();
                        }
                        else
                        {
                            objbils.PatientName = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["PaidByState"].ToString()!="")
                        {
                            objbils.PaidByState = dsbill.Tables[3].Rows[0]["PaidByState"].ToString();
                        }
                        else
                        {
                            objbils.PaidByState = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["PaidByZip"].ToString()!="")
                        {
                            objbils.PaidByZip = dsbill.Tables[3].Rows[0]["PaidByZip"].ToString();
                        }
                        else
                        {
                            objbils.PaidByZip = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderAddress1"].ToString()!="")
                        {
                            objbils.ProviderAddress1 = dsbill.Tables[3].Rows[0]["ProviderAddress1"].ToString();
                        }
                        else
                        {
                            objbils.ProviderAddress1 = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderAddress2"].ToString()!="")
                        {
                            objbils.ProviderAddress2 = dsbill.Tables[3].Rows[0]["ProviderAddress2"].ToString();
                        }
                        else
                        {
                            objbils.ProviderAddress2 = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderCity"].ToString()!="")
                        {
                            objbils.ProviderCity = dsbill.Tables[3].Rows[0]["ProviderCity"].ToString();
                        }
                        else
                        {
                            objbils.ProviderCity = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderState"].ToString()!="")
                        {
                            objbils.ProviderState = dsbill.Tables[3].Rows[0]["ProviderState"].ToString();
                        }
                        else
                        {
                            objbils.ProviderState = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["ProviderZip"].ToString()!="")
                        {
                            objbils.ProviderZip = dsbill.Tables[3].Rows[0]["ProviderZip"].ToString();
                        }
                        else
                        {
                            objbils.ProviderZip = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["Providerphone"].ToString()!="")
                        {
                            objbils.Providerphone = dsbill.Tables[3].Rows[0]["Providerphone"].ToString();
                        }
                        else
                        {
                            objbils.Providerphone = null;
                        }
                        if (dsbill.Tables[3].Rows[0]["BillDate"].ToString()!="")
                        {
                            objbils.FromDate = dsbill.Tables[3].Rows[0]["BillDate"].ToString();
                        }
                        else
                        {
                            objbils.FromDate = null;
                        }

                        objlist.Add(objbils);
                    }

                    if (dsbill.Tables[1].Rows.Count > 0)
                    {
                        while (i < dsbill.Tables[1].Rows.Count)
                        {
                            if (dsbill.Tables[1].Rows[i]["TransactionType_ID"].ToString() == "1" || dsbill.Tables[1].Rows[i]["TransactionType_ID"].ToString() == "3")
                            {
                                Charges += Convert.ToDouble(dsbill.Tables[1].Rows[i]["Transaction_Amount"]);
                            }
                            if (dsbill.Tables[1].Rows[i]["TransactionType_ID"].ToString() == "2" || dsbill.Tables[1].Rows[i]["TransactionType_ID"].ToString() == "4")
                            {
                                Payments += Convert.ToDouble(dsbill.Tables[1].Rows[i]["Transaction_Amount"]);
                            }
                            i += 1;
                        }
                        objbill.Transaction_Amount =Convert.ToString(Charges - Payments);
                        objbill.ChargeType = "Prev Bal";
                        objlist.Add(objbill);
                    }
                    else
                    {
                        objbill.Transaction_Amount = "0.0";
                        objbill.ChargeType = "Prev Bal";
                        objlist.Add(objbill);
                    }
                    if (dsbill.Tables[0].Rows.Count > 0)
                    {
                        while (j < dsbill.Tables[0].Rows.Count)
                        {
                            PatientBalance objbil = new PatientBalance();
                            if (dsbill.Tables[0].Rows[j]["Transaction_Date"].ToString()!="")
                            {
                                objbil.Transaction_Date = dsbill.Tables[0].Rows[j]["Transaction_Date"].ToString();
                            }
                            else
                            {
                                objbil.Transaction_Date = null;
                            }
                            if (dsbill.Tables[0].Rows[j]["ChargeType"].ToString()!="")
                            {
                                objbil.ChargeType = dsbill.Tables[0].Rows[j]["ChargeType"].ToString();
                            }
                            else
                            {
                                objbil.ChargeType = null;
                            }
                            if (dsbill.Tables[0].Rows[j]["SessionTotal"].ToString()!="")
                            {
                                objbil.SessionTotal = dsbill.Tables[0].Rows[j]["SessionTotal"].ToString();
                            }
                            else
                            {
                                objbil.SessionTotal = null;
                            }
                            if (dsbill.Tables[0].Rows[j]["Transaction_Amount"].ToString()!="")
                            {
                                objbil.Transaction_Amount = dsbill.Tables[0].Rows[j]["Transaction_Amount"].ToString();
                            }
                            else
                            {
                                objbil.Transaction_Amount = null;
                            }

                            objlist.Add(objbil);
                            j += 1;
                        }
                    }
                    if (dsbill.Tables[2].Rows.Count > 0)
                    {
                        PatientBalance objbil = new PatientBalance();
                        if (dsbill.Tables[2].Rows[0]["Totalbalance"].ToString() != "")
                        {
                            objbil.Totalbalance = dsbill.Tables[2].Rows[0]["Totalbalance"].ToString();
                        }
                        else
                        {
                            objbil.Totalbalance = null;
                        }
                        objlist.Add(objbil);
                    }
                }

                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_PrintBillstatement", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet Get_PatientresposiblepartyTransactions(string PatientLogin_id, string ProviderID, string ToreferenceID, string ToreferencetypeID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsbill = new DataSet();
                IDataParameter[] Objparam = {
		new SqlParameter("@in_patientlogin_id", (PatientLogin_id != null ? PatientLogin_id : null)),
		new SqlParameter("@in_provider_id", (ProviderID != null ? ProviderID : null)),
		new SqlParameter("@in_reference_id", (ToreferenceID != null ? ToreferenceID : null)),
		new SqlParameter("@in_referencetype_id", (ToreferencetypeID != null ? ToreferencetypeID : null))
		
	};
                objcommon.AddInParameters(Objparam);
                dsbill = objcommon.GetDataSet("Help_dbo.St_Provider_Patient_get_daysoutstanding_printbill");
                if ((dsbill != null))
                {
                    if (dsbill.Tables[0].Rows.Count > 0)
                    {
                        return dsbill;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_PatientresposiblepartyTransactions", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getChargePartition( int Transaction_ID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsCharge = new DataSet();
                IDataParameter[] Objparam = {
		new SqlParameter("@in_Transaction_ID",Transaction_ID )
		
	};
                objcommon.AddInParameters(Objparam);
                dsCharge = objcommon.GetDataSet("Help_dbo.st_Billing_List_billlablepartychargeparticipation");
                if ((dsCharge != null))
                {
                    if (dsCharge.Tables[0].Rows.Count > 0)
                    {
                        return dsCharge;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "getChargePartition", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetOpenPayments(RPBilling objPay)
        {
            try
            {

                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsPay = new DataSet();
                IDataParameter[] Objparam = {
		    new SqlParameter("@in_Transaction_ID",objPay.Transaction_ID ),
        	new SqlParameter("@in_FromReference_ID",objPay.fromReferenceId ),
            new SqlParameter("@in_FromReferencetype_ID",objPay.ReferenceType_ID ),
        	new SqlParameter("@in_ToReference_ID",objPay.ToReference_ID ),
            new SqlParameter("@in_ToReferenceType_ID",objPay.ToReferenceType_ID ),
        	new SqlParameter("@in_PatientLogin_ID",objPay.PatientLogin_ID ),
            new SqlParameter("@in_Ind",objPay.Ind )
        	};
                objcommon.AddInParameters(Objparam);
                dsPay = objcommon.GetDataSet("Help_dbo.st_Billing_List_ApplyingChargeTransaction");
                if ((dsPay != null))
                {
                    if (dsPay.Tables[0].Rows.Count > 0)
                    {
                        return dsPay;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                
               clsExceptionLog clsex = new clsExceptionLog();
               clsex.LogException(ex, ClassName, "GetOpenPayments", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetAdminSummary(RPBilling AdminSummary)
        {
            try
            {
                DataSet dsSummary = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_StartDate",AdminSummary.FromDate),
                                        new SqlParameter("@in_EndDate",AdminSummary.ToDate),
                                        new SqlParameter("@in_PageNo",AdminSummary.PageNo),
                                        new SqlParameter("@in_NoofRecords",AdminSummary.NoOfRecords),
                                        new SqlParameter("@in_OrderByItem",AdminSummary.OrderbyItem),
                                        new SqlParameter("@in_OrderBy",AdminSummary.orderBy)
                                        };
                objcommon.AddInParameters(insparam);
                dsSummary = objcommon.GetDataSet("Help_dbo.st_Billing_Get_AdminBillingSummary");
                return dsSummary;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetAdminSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetSearchAdminSummary(RPBilling AdminSummary)
        {
            try
            {
                DataSet dsSummary = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_StartDate",AdminSummary.FromDate),
                                        new SqlParameter("@in_EndDate",AdminSummary.ToDate),
                                        new SqlParameter("@in_ElectricianName",AdminSummary.ProviderName),
                                        new SqlParameter("@in_PageNo",AdminSummary.PageNo),
                                        new SqlParameter("@in_NoofRecords",AdminSummary.NoOfRecords),
                                        new SqlParameter("@in_OrderByItem",AdminSummary.OrderbyItem),
                                        new SqlParameter("@in_OrderBy",AdminSummary.orderBy)
                                        };
                objcommon.AddInParameters(insparam);
                dsSummary = objcommon.GetDataSet("Help_dbo.st_Billing_Get_AdminBillingSummary_rdPaging_Ver5");
                return dsSummary;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetSearchAdminSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> GetAdminListSummary(RPBilling AdminSummary)
        {
            try
            {
                 clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_StartDate",AdminSummary.FromDate),
                                        new SqlParameter("@in_EndDate",AdminSummary.ToDate),
                                        new SqlParameter("@in_PageNo",AdminSummary.PageNo),
                                        new SqlParameter("@in_NoofRecords",AdminSummary.NoOfRecords),
                                        new SqlParameter("@in_OrderByItem",AdminSummary.OrderbyItem),
                                        new SqlParameter("@in_OrderBy",AdminSummary.orderBy)
                                        };
                objcommon.AddInParameters(insparam);
                SqlDataReader objAdminsum = default(SqlDataReader);
                objAdminsum = objcommon.GetDataReader("Help_dbo.st_Billing_Get_AdminBillingSummary");
                List<RPBilling> lstAdminsum = new List<RPBilling>();
                while (objAdminsum.Read())
                {
                    RPBilling AdminBill = new RPBilling();
                    if (!DBNull.Value.Equals(objAdminsum["Provider_id"]))
                    {
                        AdminBill.Practice_ID = Convert.ToInt32(objAdminsum["Provider_id"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["Electricianname"]))
                    {
                        AdminBill.ProviderName = Convert.ToString(objAdminsum["Electricianname"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalCharges"]))
                    {
                        AdminBill.TotalCharges = string.Format("{0:c}", objAdminsum["TotalCharges"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalPayments"]))
                    {
                        AdminBill.TotalPayments =string.Format("{0:c}", objAdminsum["TotalPayments"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalCredits"]))
                    {
                        AdminBill.TotalCredits = string.Format("{0:c}",objAdminsum["TotalCredits"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["Balance"]))
                    {
                        AdminBill.Balence =string.Format("{0:c}",  objAdminsum["Balance"]);
                    }
                    lstAdminsum.Add(AdminBill);
                }
                objAdminsum.NextResult();
                if (objAdminsum.HasRows)
                {
                    while (objAdminsum.Read())
                    {
                        RPBilling.Totalnoofrecords =Convert.ToInt32(objAdminsum["TotalRecords"]);
                    }
                }
                objAdminsum.NextResult();
                if (objAdminsum.HasRows)
                {
                    while (objAdminsum.Read())
                    {
                        StringBuilder strbill = new StringBuilder();
                        strbill = strbill.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
                        strbill = strbill.Append("<td align='center'><strong>Total charges</strong></td><td align='center'><strong>Total payments</strong></td><td align='center'><strong>Total credits</strong></td><td align='center'><strong>Balance</strong></td></tr>");
                        string StrTCharge = string.Format("{0:c}", objAdminsum["TotalCharges"]);
                        string strTPay = string.Format("{0:c}", objAdminsum["TotalPayments"]);
                        string strBal = string.Format("{0:c}", objAdminsum["Balance"]);
                        string strCredit = string.Format("{0:c}", objAdminsum["TotalCredits"]);
                        strbill = strbill.Append("<tr><td align='center'>" + StrTCharge + " </td><td align='center'>" + strTPay + "</td><td align='center'>" + strCredit + "</td><td align='center'>" + strBal + "</td></tr>");
                        strbill = strbill.Append("</table>");
                        RPBilling.stradminSummary = strbill;
                       
                    }
                }
                return lstAdminsum;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetAdminListSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> GetAdminSearchListSummary(RPBilling AdminSummary)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam = { 
                                        new SqlParameter("@in_StartDate",AdminSummary.FromDate),
                                        new SqlParameter("@in_EndDate",AdminSummary.ToDate),
                                        new SqlParameter("@in_ElectricianName",AdminSummary.ProviderName),
                                        new SqlParameter("@in_PageNo",AdminSummary.PageNo),
                                        new SqlParameter("@in_NoofRecords",AdminSummary.NoOfRecords),
                                        new SqlParameter("@in_OrderByItem",AdminSummary.OrderbyItem),
                                        new SqlParameter("@in_OrderBy",AdminSummary.orderBy)
                                        };
                objcommon.AddInParameters(insparam);
                SqlDataReader objAdminsum = default(SqlDataReader);
                objAdminsum = objcommon.GetDataReader("Help_dbo.st_Billing_Get_AdminBillingSummary_rdPaging_Ver5");
                List<RPBilling> lstAdminsum = new List<RPBilling>();
                while (objAdminsum.Read())
                {
                    RPBilling AdminBill = new RPBilling();
                    if (!DBNull.Value.Equals(objAdminsum["Provider_id"]))
                    {
                        AdminBill.Practice_ID = Convert.ToInt32(objAdminsum["Provider_id"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["Electricianname"]))
                    {
                        AdminBill.ProviderName = Convert.ToString(objAdminsum["Electricianname"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalCharges"]))
                    {
                        AdminBill.TotalCharges = string.Format("{0:c}", objAdminsum["TotalCharges"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalPayments"]))
                    {
                        AdminBill.TotalPayments = string.Format("{0:c}", objAdminsum["TotalPayments"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["TotalCredits"]))
                    {
                        AdminBill.TotalCredits = string.Format("{0:c}", objAdminsum["TotalCredits"]);
                    }
                    if (!DBNull.Value.Equals(objAdminsum["Balance"]))
                    {
                        AdminBill.Balence = string.Format("{0:c}", objAdminsum["Balance"]);
                    }
                    lstAdminsum.Add(AdminBill);
                }
                objAdminsum.NextResult();
                if (objAdminsum.HasRows)
                {
                    while (objAdminsum.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(objAdminsum["TotalRecords"]);
                    }
                }
                objAdminsum.NextResult();
                if (objAdminsum.HasRows)
                {
                    while (objAdminsum.Read())
                    {
                        StringBuilder strbill = new StringBuilder();
                        strbill = strbill.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
                        strbill = strbill.Append("<td align='center'><strong>Total charges</strong></td><td align='center'><strong>Total payments</strong></td><td align='center'><strong>Total credits</strong></td><td align='center'><strong>Balance</strong></td></tr>");
                        string StrTCharge = string.Format("{0:c}", objAdminsum["TotalCharges"]);
                        string strTPay = string.Format("{0:c}", objAdminsum["TotalPayments"]);
                        string strBal = string.Format("{0:c}", objAdminsum["Balance"]);
                        string strCredit = string.Format("{0:c}", objAdminsum["TotalCredits"]);
                        strbill = strbill.Append("<tr><td align='center'>" + StrTCharge + " </td><td align='center'>" + strTPay + "</td><td align='center'>" + strCredit + "</td><td align='center'>" + strBal + "</td></tr>");
                        strbill = strbill.Append("</table>");
                        RPBilling.stradminSummary = strbill;

                    }
                }
                return lstAdminsum;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetAdminSearchListSummary", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public int MakeAdminChgTransactionlist(RPBilling provtran, int Loginhistoryid)
        {
            try
            {
                int TransactionID = 0;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlist ={
                                               new SqlParameter("@In_FromReference_ID",provtran.fromReferenceId),
                                               new SqlParameter("@In_FromReferenceType_ID",Convert.ToInt32(provtran.ReferenceType_ID)),
                                                new SqlParameter("@In_ToReference_ID",Convert.ToInt32(provtran.ToReference_ID)),
                                               new SqlParameter("@In_ToReferenceType_ID",Convert.ToInt32(provtran.ToReferenceType_ID)),
                                                new SqlParameter("@In_BillingService_ID",provtran.BillingService_ID),
                                               new SqlParameter("@In_BillingChargetype_ID",Convert.ToInt32(provtran.BillingChargetype_ID)),
                                               new SqlParameter("@In_Transaction_Date",provtran.Transaction_Date),
                                               new SqlParameter("@In_TransactionAmount",Convert.ToInt32(provtran.Transaction_Amount)),
                                               new SqlParameter("@in_Notes",provtran.Notes),
                                               new SqlParameter("@in_Practice_ID",null),
                                               new SqlParameter("@in_Facility_ID",null),                                              
                                               new SqlParameter("@In_CreatedBy",provtran.UserId),
                                               new SqlParameter("@in_loginhistoryid",Loginhistoryid)
                                          };

                IDataParameter[] outParamlist = { 
                                            new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
                                            };
                objcommon.AddInParameters(insparamlist);
                objcommon.AddOutParameters(outParamlist);
                DataSet ds = new DataSet();
                ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_AdminChargeTransaction");
                if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
                {
                    TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
                }
                return TransactionID;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "MakeAdminChgTransactionlist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public int MakeAdminPayTransaction(RPBilling provtran, int Loginhistoryid)
        {
            try
            {
                int TransactionID = 0;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlist ={
                                               new SqlParameter("@In_FromReference_ID",provtran.fromReferenceId),
                                               new SqlParameter("@In_FromReferenceType_ID",Convert.ToInt32(provtran.ReferenceType_ID)),
                                               new SqlParameter("@In_ToReferenceType_ID",provtran.ToReferenceType_ID),
                                               new SqlParameter("@In_ToReference_ID",provtran.ToReference_ID),
                                               new SqlParameter("@In_BillingService_ID",provtran.BillingService_ID),
                                               new SqlParameter("@In_BillingChargetype_ID",provtran.BillingChargetype_ID),
                                               new SqlParameter("@In_Transaction_Date",provtran.Transaction_Date),
                                               new SqlParameter("@In_TransactionAmount",provtran.Transaction_Amount),
                                               new SqlParameter("@In_Notes",provtran.Notes),
                                               new SqlParameter("@In_ChequeOrCCTran_ID",provtran.CCTransactionID),
                                               new SqlParameter("@In_PaymentType_ID",provtran.PaymentType_ID),
                                               new SqlParameter("@in_Practice_ID",null),
                                               new SqlParameter("@in_Facility_ID",null),
                                               new SqlParameter("@In_CreatedBy",provtran.UserId),
                                               new SqlParameter("@in_loginhistory_id",Loginhistoryid)
                                          };

                IDataParameter[] outParamlist = { 
                                            new SqlParameter("@out_ChargeTran_ID",SqlDbType.Int)
                                            };
                objcommon.AddInParameters(insparamlist);
                objcommon.AddOutParameters(outParamlist);
                DataSet ds = new DataSet();
                ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_AdminPaymentTransaction");
                if (objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value != null)
                {
                    TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_ChargeTran_ID"].Value);
                }
                return TransactionID;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "MakeAdminPayTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public int MakeAdminAdjTransaction(RPBilling provtran, int Loginhistoryid)
        {
            try
            {
                int TransactionID = 0;
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlist ={
                                               new SqlParameter("@In_FromReference_ID",provtran.fromReferenceId),
                                               new SqlParameter("@In_FromReferenceType_ID",provtran.ReferenceType_ID),
                                               new SqlParameter("@In_ToReference_ID",provtran.ToReference_ID),
                                               new SqlParameter("@In_ToReferenceType_ID",provtran.ToReferenceType_ID),
                                               new SqlParameter("@In_Transaction_Date",provtran.Transaction_Date),
                                               new SqlParameter("@In_TransactionType_ID",provtran.TransactionType_ID),
                                               new SqlParameter("@In_Transaction_Amount",provtran.Transaction_Amount),
                                               new SqlParameter("@In_BillingService_ID",provtran.BillingService_ID),
                                               new SqlParameter("@In_Notes",provtran.Notes),
                                               new SqlParameter("@In_RefTransaction_ID",provtran.RefTransaction_ID),
                                               new SqlParameter("@in_Practice_ID",null),
                                               new SqlParameter("@in_Facility_ID",null),
                                               new SqlParameter("@In_CreatedBy",provtran.UserId),                                               
                                               new SqlParameter("@in_AdjustmentType_ID",provtran.AdjustmentType_ID),
                                               new SqlParameter("@In_Ref_ManualTran_id",null),
                                               new SqlParameter("@in_loginhistory_id",Loginhistoryid)
                                          };

                IDataParameter[] outParamlist = { 
                                            new SqlParameter("@Out_Tran_ID",SqlDbType.Int)
                                            };
                objcommon.AddInParameters(insparamlist);
                objcommon.AddOutParameters(outParamlist);
                DataSet ds = new DataSet();
                ds = objcommon.GetDataSet("Help_dbo.st_Billing_INS_AdminAdjustmentTransaction");
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Tran_ID"].Value != null)
                {
                    TransactionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@Out_Tran_ID"].Value);
                }
                return TransactionID;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "MakeAdminAdjTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public List<RPBilling> Get_PracticeTrasnactions(RPBilling AdminTransactions)
        {
            try
            {
                  clsCommonFunctions objcommon = new clsCommonFunctions();
                  IDataParameter[] insparam = { new SqlParameter("@in_TransactionType_IDs",AdminTransactions.TransactionTypeIDs),
                                                new SqlParameter("@in_FromReferenceType_ID",AdminTransactions.ReferenceType_ID),
                                                new SqlParameter("@in_FromReference_ID",AdminTransactions.fromReferenceId),
                                                new SqlParameter("@in_Provider_id",AdminTransactions.Practice_ID),
                                               // new SqlParameter("@in_Facility_ID",AdminTransactions.Facility_ID),
                                                new SqlParameter("@in_ToReferenceType_ID",AdminTransactions.ToReferenceType_ID),
                                                new SqlParameter("@in_ToReference_ID",AdminTransactions.ToReference_ID),
                                                new SqlParameter("@In_Fromdate",AdminTransactions.FromDate),
                                                new SqlParameter("@In_Todate",AdminTransactions.ToDate),
                                                new SqlParameter("@In_Service_ID",AdminTransactions.ServiceID),
                                                new SqlParameter("@IN_OrderByItem",AdminTransactions.OrderbyItem),
                                                new SqlParameter("@In_OrderBy",AdminTransactions.orderBy),
                                                new SqlParameter("@In_NoOfRecords",AdminTransactions.NoOfRecords),
                                                new SqlParameter("@In_PageNo",AdminTransactions.PageNo)
                                              };
                  objcommon.AddInParameters(insparam);
                  SqlDataReader objPracTran = default(SqlDataReader);
                  objPracTran = objcommon.GetDataReader("Help_dbo.st_Billing_LIST_PracticeUserTrasnactions_RDPAGING_VER5 ");
                  List<RPBilling> lstPracticeTransactions = new List<RPBilling>();
                  while (objPracTran.Read())
                  {
                      RPBilling objpractice = new RPBilling();
                      if (!DBNull.Value.Equals(objPracTran["Transaction_id"]))
                      {
                          objpractice.Transaction_ID = Convert.ToInt32(objPracTran["Transaction_id"]);
                      }
                      if (!DBNull.Value.Equals(objPracTran["Transaction_Date"]))
                      {
                          objpractice.Transaction_Date = Convert.ToString(objPracTran["Transaction_Date"]);
                      }
                      //if (!DBNull.Value.Equals(objPracTran["ServiceName"]))
                      //{
                      //    objpractice.servicename = Convert.ToString(objPracTran["ServiceName"]);
                      //}
                       if (!DBNull.Value.Equals(objPracTran["Transaction_Amount"]))
                      {
                          objpractice.Transaction_Amount =string.Format("{0:c}", objPracTran["Transaction_Amount"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["Notes"]))
                      {
                          objpractice.Notes = Convert.ToString(objPracTran["Notes"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["TransactionType"]))
                      {
                          objpractice.TransactionType = Convert.ToString(objPracTran["TransactionType"]);
                      }
                        if (!DBNull.Value.Equals(objPracTran["InvoiceGenerated_Ind"]))
                      {
                          objpractice.InvoiceGenerated_Ind = Convert.ToString(objPracTran["InvoiceGenerated_Ind"]);
                      }
                      if (!DBNull.Value.Equals(objPracTran["PaymentType_ID"]))
                      {
                          objpractice.PaymentType_ID = Convert.ToInt32(objPracTran["PaymentType_ID"]);
                      }
                      if (!DBNull.Value.Equals(objPracTran["PaidBy"]))
                      {
                          objpractice.PaidByName = Convert.ToString(objPracTran["PaidBy"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["BillableBy"]))
                      {
                          objpractice.BilledBy = Convert.ToString(objPracTran["BillableBy"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["PaymentMode"]))
                      {
                          objpractice.PaymentMode = Convert.ToString(objPracTran["PaymentMode"]);
                      }
                      if (!DBNull.Value.Equals(objPracTran["ChequeOrCCTran_ID"]))
                      {
                          objpractice.ChequeOrCCTran_ID = Convert.ToInt32(objPracTran["ChequeOrCCTran_ID"]);
                      }
                      if (!DBNull.Value.Equals(objPracTran["RefTransaction_ID"]))
                      {
                          objpractice.RefTransID = Convert.ToInt32(objPracTran["RefTransaction_ID"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["IsRefundTransaction"]))
                      {
                          objpractice.IsRefundTransaction = Convert.ToString(objPracTran["IsRefundTransaction"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["IsReference"]))
                      {
                          objpractice.IsReference = Convert.ToString(objPracTran["IsReference"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["ReturnAmount"]))
                      {
                          objpractice.ReturnAmount = Convert.ToString(objPracTran["ReturnAmount"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["provider_id"]))
                      {
                          objpractice.Practice_ID = Convert.ToInt32(objPracTran["provider_id"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["Ref_ManualTran_id"]))
                      {
                          objpractice.strmanualrefid = Convert.ToString(objPracTran["Ref_ManualTran_id"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["ToReference_ID"]))
                      {
                          objpractice.ToReference_ID = Convert.ToString(objPracTran["ToReference_ID"]);
                      }
                       if (!DBNull.Value.Equals(objPracTran["ToReferenceType_ID"]))
                      {
                          objpractice.ToReferenceType_ID = Convert.ToString(objPracTran["ToReferenceType_ID"]);
                      }
                       lstPracticeTransactions.Add(objpractice);
                  }
                  objPracTran.NextResult();
                  if (objPracTran.HasRows)
                  {
                      while (objPracTran.Read())
                      {
                          RPBilling.Totalnoofrecords= Convert.ToInt32(objPracTran[0]);
                      }
                  }
                return lstPracticeTransactions;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_PracticeTrasnactions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getprovider(int? practiceId)
        {
            try
            {
                 clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dsProvider = new DataSet();
                IDataParameter[] Objparam = {
                                                new SqlParameter("@in_Provider_id",practiceId)};
                objcommon.AddInParameters(Objparam);
                dsProvider = objcommon.GetDataSet("Help_dbo.st_Practice_DDLIST_Providers");
                return dsProvider;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getprovider", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet getViewPrTransaction(int tranId)
        {
            try
            {
            clsCommonFunctions objadmin = new clsCommonFunctions();
            DataSet dstran = new DataSet();
            IDataParameter[] objparam = { new SqlParameter("@in_PaymentTransaction_ID",tranId) };
            objadmin.AddInParameters(objparam);
            dstran = objadmin.GetDataSet("Help_dbo.St_billing_GetTransactionDetails");
            return dstran;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "getViewPrTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string Get_InvoiceBeginDate( RPBilling adminbilling)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparams = { 
                                             new SqlParameter("@in_FromReference_ID",adminbilling.fromReferenceId),
                                             new SqlParameter("@in_FromReferenceType_ID",adminbilling.ReferenceType_ID),
                                             new SqlParameter("@in_ToReference_ID",adminbilling.ToReference_ID),
                                             new SqlParameter("@in_ToReferenceType_ID",adminbilling.ToReferenceType_ID)
                                             };
                IDataParameter[] outparams = { new SqlParameter("@out_BeginDate",SqlDbType.Date)};
                objcommon.AddInParameters(objparams);
                objcommon.AddOutParameters(outparams);
                string objBegindate = string.Empty;
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_Get_InvoiceBeginDate");
                if (!DBNull.Value.Equals(objcommon.objdbCommandWrapper.Parameters["@out_BeginDate"].Value))
                {
                    objBegindate = objcommon.objdbCommandWrapper.Parameters["@out_BeginDate"].Value.ToString();
                }
                return objBegindate;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_InvoiceBeginDate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> List_AdmintoPracticeChargesToGenerateInvoice(RPBilling adminBilling)
        {
            try
            {

          
            clsCommonFunctions objcommon=new clsCommonFunctions();
            IDataParameter[] insparam={
                                      new SqlParameter("@In_FromReference_ID",adminBilling.fromReferenceId),
                                      new SqlParameter("@In_FromReferenceType_ID",adminBilling.ReferenceType_ID),
                                      new SqlParameter("@in_Practice_ID",adminBilling.Practice_ID),
                                      new SqlParameter("@in_Begin_Date",adminBilling.FromDate),
                                      new SqlParameter("@in_End_date",adminBilling.ToDate),
                                      new SqlParameter("@In_OrderBy",adminBilling.orderBy),
                                      new SqlParameter("@in_ToReference_ID",adminBilling.ToReference_ID),
                                      new SqlParameter("@in_ToReferenceType_ID",adminBilling.ToReferenceType_ID),
                                      new SqlParameter("@In_OrderByItem",adminBilling.OrderbyItem)
                                      };
            objcommon.AddInParameters(insparam);
            SqlDataReader objPracInvoice = default(SqlDataReader);
            objPracInvoice = objcommon.GetDataReader("Help_dbo.st_Billing_List_AdmintoPracticeChargesToGenerateInvoice");
            List<RPBilling> listInvoice = new List<RPBilling>();
            while (objPracInvoice.Read())
            {
                RPBilling objinvoice = new RPBilling();
                if (!DBNull.Value.Equals(objPracInvoice["Transaction_Amount"]))
                {
                    objinvoice.Transaction_Amount = string.Format("{0:c}", objPracInvoice["Transaction_Amount"]);
                }
                if (!DBNull.Value.Equals(objPracInvoice["ChargeType"]))
                {
                    objinvoice.ChargeType = Convert.ToString(objPracInvoice["ChargeType"]);
                }
                if (!DBNull.Value.Equals(objPracInvoice["Transaction_Date"]))
                {
                    objinvoice.Transaction_Date =DateTime.Parse(objPracInvoice["Transaction_Date"].ToString()).ToShortDateString();
                }
                if (!DBNull.Value.Equals(objPracInvoice["Notes"]))
                {
                    objinvoice.Notes =Convert.ToString(objPracInvoice["Notes"]);
                }
                listInvoice.Add(objinvoice);
            }
            return listInvoice;

            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "List_AdmintoPracticeChargesToGenerateInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public int INS_AdmintoPracticeInvoice(RPBilling obj)
        {
            try 
	     {	        
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={
                                           new SqlParameter("@in_FromReference_ID",obj.fromReferenceId),
                                           new SqlParameter("@in_FromReferenceType_ID",obj.ReferenceType_ID),
                                           new SqlParameter("@in_ToReference_ID",obj.ToReference_ID),
                                           new SqlParameter("@in_ToReferenceType_ID",obj.ToReferenceType_ID),
                                           new SqlParameter("@in_Provider_ID",obj.Practice_ID),
                                           new SqlParameter("@in_Begin_Date",obj.FromDate),
                                           new SqlParameter("@in_End_Date",obj.ToDate),
                                           new SqlParameter("@in_Messagetouser",obj.Messagetouser),
                                           new SqlParameter("@in_TermsAndConditions",obj.TermsAndConditions),
                                           new SqlParameter("@in_CreatedBy",obj.UserId)
                                      };
            IDataParameter[] outparam = {new SqlParameter("@out_Invoice_ID",SqlDbType.Int) };
            objcommon.AddInParameters(insparam);
            objcommon.AddOutParameters(outparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_INS_AdmintoPracticeInvoice");
            int invoiceid=0;
            if(!DBNull.Value.Equals(objcommon.objdbCommandWrapper.Parameters["@out_Invoice_ID"].Value))
            {
                invoiceid=Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_Invoice_ID"].Value);
            }
            return invoiceid;
           }
	        catch (Exception ex)
	        {

                var objex = new clsExceptionLog();
                objex.LogException(ex, ClassName, "INS_AdmintoPracticeInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
	        }
            return 0;
        }
        public DataSet practiceInvoicelist(RPBilling pracInvoice)
        {
            try
            {
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={
                                           new SqlParameter("@In_FromReference_ID",pracInvoice.fromReferenceId),
                                           new SqlParameter("@in_FromReferenceType_ID",pracInvoice.ReferenceType_ID),
                                           new SqlParameter("@in_ToReference_ID",pracInvoice.ToReference_ID),
                                           new SqlParameter("@in_ToReferenceType_ID",pracInvoice.ToReferenceType_ID),
                                           //new SqlParameter("@in_Practice_ID",pracInvoice.Practice_ID),
                                           new SqlParameter("@in_Invoice_ID",pracInvoice.Invoice_ID)
                                      };
            objcommon.AddInParameters(insparam);
            DataSet dsInvoice = new DataSet();
            dsInvoice = objcommon.GetDataSet("Help_dbo.st_Billing_REPORT_PracticeInvoice");
            return dsInvoice;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "practiceInvoicelist", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public List<RPBilling> Get_AdminAddRecipts(RPBilling adminRecipt)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparams = { 
                                             new SqlParameter("@In_FromReference_ID",adminRecipt.fromReferenceId),
                                             new SqlParameter("@In_FromReferenceType_ID",adminRecipt.ReferenceType_ID),
                                             new SqlParameter("@In_ToReference_ID",adminRecipt.ToReference_ID),
                                             new SqlParameter("@In_ToReferenceType_ID",adminRecipt.ToReferenceType_ID),
                                             new SqlParameter("@In_DateFrom",adminRecipt.FromDate),
                                             new SqlParameter("@In_DateTo",adminRecipt.ToDate),
                                             new SqlParameter("@In_Charge_Ind",adminRecipt.ChargeType),
                                             new SqlParameter("@In_Payment_Ind",adminRecipt.paytype),
                                             new SqlParameter("@In_Credit_Ind",adminRecipt.CreditInd),
                                             new SqlParameter("@in_Practice_ID",adminRecipt.Practice_ID),
                                             new SqlParameter("@In_OrderByItem",adminRecipt.OrderbyItem),
                                             new SqlParameter("@In_OrderBy",adminRecipt.orderBy),
                                             new SqlParameter("@In_NoOfRecords",adminRecipt.NoOfRecords),
                                             new SqlParameter("@In_PageNo",adminRecipt.PageNo)
                                             };
                objcommon.AddInParameters(insparams);
                SqlDataReader objPracrecipt = default(SqlDataReader);
                objPracrecipt = objcommon.GetDataReader("Help_dbo.st_Billing_Get_AdminToPracticeTransactionsAddDateReceipts_rdpaging");
                List<RPBilling> listrecipt = new List<RPBilling>();
                while (objPracrecipt.Read())
                {
                    RPBilling objrecipt = new RPBilling();
                    if (!DBNull.Value.Equals(objPracrecipt["Transaction_ID"]))
                    {
                        objrecipt.Transaction_ID = Convert.ToInt32(objPracrecipt["Transaction_ID"]);
                    }
                    if (!DBNull.Value.Equals(objPracrecipt["Transaction_Date"]))
                    {
                        objrecipt.Transaction_Date = DateTime.Parse(objPracrecipt["Transaction_Date"].ToString()).ToShortDateString();
                    }
                    if (!DBNull.Value.Equals(objPracrecipt["ServiceName"]))
                    {
                        objrecipt.servicename =Convert.ToString(objPracrecipt["ServiceName"]);
                    }
                    if (!DBNull.Value.Equals(objPracrecipt["Transaction_Amount"]))
                    {
                        objrecipt.Transaction_Amount = string.Format("{0:c}" ,objPracrecipt["Transaction_Amount"]);
                    }
                    if (!DBNull.Value.Equals(objPracrecipt["Notes"]))
                    {
                        objrecipt.Notes =Convert.ToString(objPracrecipt["Notes"]);
                    }
                    if (!DBNull.Value.Equals(objPracrecipt["TransactionType"]))
                    {
                        objrecipt.TransactionType =Convert.ToString(objPracrecipt["TransactionType"]);
                    }
                    listrecipt.Add(objrecipt);
                }
                objPracrecipt.NextResult();
                if (objPracrecipt.HasRows)
                {
                    while (objPracrecipt.Read())
                    {
                        RPBilling.Totalnoofrecords = Convert.ToInt32(objPracrecipt[0]);
                    }
                }
                return listrecipt;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_AdminAddRecipts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public int INS_AdmintoPracticeReceipt(RPBilling objinjsrecipt)
        {
            try
            {
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam ={
                                         new SqlParameter("@in_FromReference_ID",objinjsrecipt.fromReferenceId),
                                         new SqlParameter("@in_FromReferenceType_ID",objinjsrecipt.ReferenceType_ID),
                                         new SqlParameter("@in_ToReference_ID",objinjsrecipt.ToReference_ID),
                                         new SqlParameter("@in_ToReferenceType_ID",objinjsrecipt.ToReferenceType_ID),
                                         new SqlParameter("@in_Provider_ID",objinjsrecipt.Practice_ID),
                                         new SqlParameter("@In_Transaction_IDs",objinjsrecipt.Transaction_IDs),
                                         new SqlParameter("@in_BeginDate",objinjsrecipt.FromDate),
                                         new SqlParameter("@in_EndDate",objinjsrecipt.ToDate),
                                         new SqlParameter("@in_CreatedBy",objinjsrecipt.UserId)
                                      };
           
            IDataParameter[] outparam ={
                                           new SqlParameter("@out_Invoice_ID",SqlDbType.Int)
                                      };
            objcommon.AddInParameters(insparam);
            objcommon.AddOutParameters(outparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_INS_AdmintoPracticeReceipt");
            int reciptid = 0;
            if (!DBNull.Value.Equals(objcommon.objdbCommandWrapper.Parameters["@out_Invoice_ID"].Value))
            {
                reciptid = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@out_Invoice_ID"].Value);
            }
            else
            {
                //reciptid = null;
            }
            return reciptid;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "INS_AdmintoPracticeReceipt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public RPBilling Del_AdminTransaction(RPBilling objtran, int Loginhistoryid)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparamlst ={ 
                                            new SqlParameter("@In_Transaction_ID",objtran.Transaction_ID),
                                            new SqlParameter("@In_UpdatedBy",objtran.UserId),
                                            new SqlParameter("@in_loginhistory_id",Loginhistoryid),
                                       };
                objcommon.AddInParameters(insparamlst);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_DEL_AdminTransaction");
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Del_AdminTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public void INS_AdminChequeTransaction(RPBilling objbill)
        {
            try
            {
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] insparam = { 
                                        new SqlParameter("@In_ChequeNo",objbill.ChecksNo ),
                                        new SqlParameter("@In_ChequeDate",null),
                                        new SqlParameter("@In_IssuedBankName",null),
                                        new SqlParameter("@In_ChequeAmount",objbill.Transaction_Amount),
                                        new SqlParameter("@In_CreatedBy",objbill.UserId),
                                        new SqlParameter("@in_TransactionID",objbill.Transaction_ID)
                                        };
            objcommon.AddInParameters(insparam);
            objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_INS_AdminChequeTransaction");
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "INS_AdminChequeTransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
        public void invoiceRegenerate(RPBilling objgenrate)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                               new SqlParameter("@in_ToReference_ID",objgenrate.ToReference_ID),
                                               new SqlParameter("@in_ToReferenceType_ID",objgenrate.ToReferenceType_ID),
                                               new SqlParameter("@in_Provider_ID",objgenrate.ToReference_ID),
                                               new SqlParameter("@in_Invoice_ID",objgenrate.Invoice_ID),
                                               new SqlParameter("@in_UpdatedBy",objgenrate.UserId)                                               
                                          };
                objcommon.AddInParameters(insparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.st_Billing_ReGenerate_AdmintoPracticeInvoiceforSingle");
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "invoiceRegenerate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public DataSet Insert_Customertransaction(RPBilling ObjInsTransaction, ref string Out_Msg)
        {
            try
            {
                var objclsDBWrapper = new clsDBWrapper();
                IDataParameter[] objparm = {
		new SqlParameter("@In_FromReference_ID", (ObjInsTransaction.fromReferenceId != null ? ObjInsTransaction.fromReferenceId : null)),
		new SqlParameter("@In_FromReferenceType_ID", (ObjInsTransaction.FromReferenceType_ID ?? null)),
		new SqlParameter("@In_ToReferenceType_ID", (ObjInsTransaction.ToReferenceType_ID ?? null)),
		new SqlParameter("@In_ToReference_ID", (ObjInsTransaction.ToReference_ID ?? null)),
		new SqlParameter("@In_Transaction_Date", (ObjInsTransaction.Transaction_Date ?? null)),
		new SqlParameter("@In_TransactionAmount", (ObjInsTransaction.Transaction_Amount ?? null)),
		new SqlParameter("@In_CreatedBy", HttpContext.Current.Session["UserID"].ToString()),
		new SqlParameter("@In_PaymentType_ID", ObjInsTransaction.PaymentType_ID),
        new SqlParameter("@in_pctype_id", ObjInsTransaction.PcType_ID),
        new SqlParameter("@in_authorizationnumber", ObjInsTransaction.Authorizednumber),
        new SqlParameter("@in_CheckNumber", ObjInsTransaction.ChecksNo),
        new SqlParameter("@in_email", ObjInsTransaction.Customer_Email),
        new SqlParameter("@in_appointment_id",ObjInsTransaction.Appointment_ID),
        new SqlParameter("@in_appointment_date",ObjInsTransaction.apptdate),
        new SqlParameter("@in_notes", ObjInsTransaction.Notes),
        new SqlParameter("@in_emailcheck", ObjInsTransaction.emailcheck) 
    	};
                IDataParameter[] OutParam = { new SqlParameter("@out_Msg", SqlDbType.VarChar, 500) };
                objclsDBWrapper.AddInParameters(objparm);
                objclsDBWrapper.AddOutParameters(OutParam);
                DataSet dsTransaction = new DataSet();
                dsTransaction = objclsDBWrapper.GetDataSet("Help_dbo.st_Billing_INS_ProviderPatientChargepaymentTransactions");
                if (!string.IsNullOrEmpty(Convert.ToString(objclsDBWrapper.objdbCommandWrapper.Parameters["@Out_msg"].Value )))
                {
                    Out_Msg = objclsDBWrapper.objdbCommandWrapper.Parameters["@Out_msg"].Value.ToString();
                }
                else
                {
                    Out_Msg = null;
                }
                return dsTransaction;

            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Insert_Customertransaction", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public string Get_Appointmentdate(RPBilling patappid)
        {
            try
            {
                string Appdate = string.Empty;
                clsCommonFunctions objcmn = new clsCommonFunctions();
                IDataParameter[] insparamlist ={
                                               new SqlParameter("@in_appointment_id",patappid.Appointment_ID)
                                              
                                          };
                objcmn.AddInParameters(insparamlist);
                SqlDataReader dr = default(SqlDataReader);
                dr = objcmn.GetDataReader("Help_dbo.st_get_appointmentdate");
                while (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["AppointmentDate"]))
                    {
                        Appdate = dr["AppointmentDate"].ToString();
                    }
                    else
                    {
                        Appdate = null;
                    }
                }

                return Appdate;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_Appointmentdate", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public List<Invocie> InvoiceList(string Provider_ID, int FromReferenceTypeID, int? NoofRecords, int? PageNo, string OrderBy, string OrderByItem, string strStartDate, string strEndDate, string ClientID, int ToReferenceTypeID)
        {
            try
            {

               // DataSet ds = new DataSet();
                clsCommonFunctions objcommon = new clsCommonFunctions();
                List<Invocie> objList = new List<Invocie>();
                int SLNo = 0;
                IDataParameter[] InParm = {
			new SqlParameter("@In_FromReference_ID", Provider_ID),
			new SqlParameter("@In_FromReferenceType_ID", FromReferenceTypeID),
			new SqlParameter("@in_ToReference_ID", ClientID == null ? null : ClientID),
            new SqlParameter("@in_ToReferenceType_ID",ToReferenceTypeID),
            new SqlParameter("@in_Orderby", OrderBy == null ? null : OrderBy),
			new SqlParameter("@in_OrderbyItem", OrderByItem == null ? null : OrderByItem),
			new SqlParameter("@in_Start_Date", strStartDate == null ? null : strStartDate),
			new SqlParameter("@in_End_Date", strEndDate == null ? null : strEndDate),
			new SqlParameter("@in_NoofRecords", NoofRecords == 0 ? null : NoofRecords),
			new SqlParameter("@In_PageNo", PageNo == 0 ? null : PageNo)
		};
               SLNo= Convert.ToInt32(NoofRecords * (PageNo - 1) + 1);
                objcommon.AddInParameters(InParm);              
                SqlDataReader drrecipt = default(SqlDataReader);
                drrecipt = objcommon.GetDataReader("Help_dbo.st_Billing_GET_Client_Invoice_RDPaging");
                while (drrecipt.Read())
                {
                    Invocie objrecipt = new Invocie();
                    objrecipt.SLNo = SLNo;
                    SLNo = SLNo + 1;
                    if (!DBNull.Value.Equals(drrecipt["Invoice_id"]))
                    {
                        objrecipt.InvoiceID = Convert.ToInt32(drrecipt["Invoice_id"]);
                    }
                    if (!DBNull.Value.Equals(drrecipt["Invoice_Date"]))
                    {
                        objrecipt.Invoice_Date = DateTime.Parse(drrecipt["Invoice_Date"].ToString()).ToShortDateString();
                    }
                    if (!DBNull.Value.Equals(drrecipt["RecieptNo"]))
                    {
                        objrecipt.ReceiptNo = Convert.ToString(drrecipt["RecieptNo"]);
                    }
                    if (!DBNull.Value.Equals(drrecipt["Invoicefilename"]))
                    {
                        objrecipt.InvoiceFileName = Convert.ToString(drrecipt["Invoicefilename"]);
                    }
                    objList.Add(objrecipt);
                }
                drrecipt.NextResult();
                if (drrecipt.HasRows)
                {
                    while (drrecipt.Read())
                    {
                        Invocie.TotalRecords = Convert.ToInt32(drrecipt[0]);
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "InvoiceList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public DataSet GetInvoiceData(string Provider_ID, int FromReferenceTypeID, string ClientID, int ToReferenceTypeID,int InvoiceID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={
                                           new SqlParameter("@In_FromReference_ID",Provider_ID),
                                           new SqlParameter("@in_FromReferenceType_ID",FromReferenceTypeID),
                                           new SqlParameter("@in_ToReference_ID",ClientID),
                                           new SqlParameter("@in_ToReferenceType_ID",ToReferenceTypeID),
                                           new SqlParameter("@in_Invoice_ID",InvoiceID)
                                      };
                objcommon.AddInParameters(insparam);
                DataSet dsInvoice = new DataSet();
                dsInvoice = objcommon.GetDataSet("Help_dbo.st_Billing_REPORT_client_Invoice");
                return dsInvoice;
            }
            catch (Exception ex)
            {

                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetInvoiceData", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
    }
}