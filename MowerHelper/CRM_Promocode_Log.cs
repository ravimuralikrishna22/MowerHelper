//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MowerHelper
{
    using System;
    using System.Collections.Generic;
    
    public partial class CRM_Promocode_Log
    {
        public long CRM_Promocode_Log_ID { get; set; }
        public Nullable<long> CRM_Promocode_ID { get; set; }
        public Nullable<long> Provider_ID { get; set; }
        public Nullable<long> Login_ID { get; set; }
        public Nullable<System.DateTime> Registereddate { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public Nullable<long> Createdby { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<long> Promocode_incentive_ID { get; set; }
        public Nullable<int> UpgradeCount { get; set; }
        public string IS_Renew_ind { get; set; }
        public string Incentive_Info { get; set; }
    }
}
