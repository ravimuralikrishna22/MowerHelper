using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;


namespace MowerHelper.Models.BLL.Patients
{
    public class Paymentmethods
    {
        public string CardType { get; set; }
        public int referencetype_id { get; set; }
        public string CardNumber { get; set; }
        public string UpdatedBy { get; set; }
        public int reference_id { get; set; }

        public int? CreditCardInfo_ID { get; set; }
        public int SlNo { get; set; }
        public string cardholdername { get; set; }
        public string Address { get; set; }
        public string Firstname { get; set; }
        public string lastname { get; set; }
        public string cardexpirymonth { get; set; }
        public string cardexpiryyear { get; set; }
        public string cardexpiry { get; set; }
        public string Vaultid { get; set; }
        public int? Daysremining { get; set; }
        public string customerid { get; set; }
        public static List<Paymentmethods> CreditCard_list_paymentinfo(Paymentmethods objpayments)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.CreditCard_list_paymentinfo(objpayments);
        }
        public static List<Paymentmethods> CreditCard_Expirylist_info(Paymentmethods objpayments)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.CreditCard_Expirylist_info(objpayments);
        }
    }
}