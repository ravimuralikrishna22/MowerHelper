using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.DAL.DALBilling;
namespace MowerHelper.Models.BLL.Billing
{
    public class Invocie
    {
        public int NoofRecords
        {
            get;
            set;
        }
        public int SLNo
        {
            get;
            set;
        }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string orderBy { get; set; }
        public string OrderByItem { get; set; }
        public int? PageNo { get; set; }
        public string Daterange { get; set; }
        public static int TotalRecords { get; set; }
        public List<Invocie> ListOfInvoice { get; set; }

        public int InvoiceID { get; set; }
        public string ReceiptNo { get; set; }
        public string Invoice_Date { get; set; }
        public string InvoiceFileName { get; set; }
        public List<Invocie> InvoiceList(string Provider_ID,int FromReferenceTypeID, int? NoofRecords, int? PageNo, string OrderBy, string OrderByItem, string strStartDate, string strEndDate, string ClientID,int ToReferenceTypeID)
        {
            BillingSqlDataAccessLayer objList = new BillingSqlDataAccessLayer();
            return objList.InvoiceList(Provider_ID, FromReferenceTypeID, NoofRecords, PageNo, OrderBy, OrderByItem, strStartDate, strEndDate, ClientID,ToReferenceTypeID);
        }
        public DataSet GetInvoiceData(string Provider_ID, int FromReferenceTypeID, string ClientID, int ToReferenceTypeID, int InvoiceID)
        {
            BillingSqlDataAccessLayer obj = new BillingSqlDataAccessLayer();
            return obj.GetInvoiceData(Provider_ID, FromReferenceTypeID, ClientID, ToReferenceTypeID,InvoiceID);
        }
    }
}