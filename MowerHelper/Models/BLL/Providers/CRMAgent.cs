using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.CDAL;

namespace MowerHelper.Models.BLL.Providers
{
    public class CRMAgent
    {
        public string CRM_Agent_ID { get; set; }
        public string CompanyName { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string Suffix { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? State_ID { get; set; }
        public int? City_ID { get; set; }
        public string Expirydate { get; set; }
        public string ISFTAgent { get; set; }
        public string IncentiveIds { get; set; }
        public string IsAgent { get; set; }
        public string ZIP { get; set; }
        public string Email { get; set; }
        public string OrderBy { get; set; }
        public static int TotalRecords;
        public int? PageNo { get; set; }
        public string OrderByItem { get; set; }
        public int? NoofRecords { get; set; }
        public string Status_Ind { get; set; }
        public string State { get; set; }
        public int Slno { get; set; }
        public string Promocodeurl { get; set; }
        public string Incentivetext { get; set; }
        public string City { get; set; }
        public string AgentCode { get; set; }
        public string FullName { get; set; }
        public int? PromocodeId { get; set; }
        public string PromoCode { get; set; }
        public string CreatedOn { get; set; }

        public static string Ins_Upd_CRM_Agent_Details(CRMAgent obj)
        {
            ResellerSQLDataAccessLayer layer = new ResellerSQLDataAccessLayer();
            return layer.Ins_Upd_CRM_Agent_Details(obj);
        }

        public static List<CRMAgent> GET_CRM_Agent_Details(CRMAgent obj)
        {
            ResellerSQLDataAccessLayer layer = new ResellerSQLDataAccessLayer();
            return layer.GET_CRM_Agent_Details(obj);
        }
        public static string Active_InActive_CRM_Agent_Details(CRMAgent obj)
        {
            ResellerSQLDataAccessLayer layer = new ResellerSQLDataAccessLayer();
            return layer.Active_InActive_CRM_Agent_Details(obj);
        }
    }
}