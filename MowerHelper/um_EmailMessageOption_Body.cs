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
    
    public partial class um_EmailMessageOption_Body
    {
        public int EmailMsgOptionBody_ID { get; set; }
        public Nullable<int> EmailMessage_Option_ID { get; set; }
        public string Msg_Subject { get; set; }
        public string Msg_Body { get; set; }
        public string Msg_Footer { get; set; }
        public string Status_Ind { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string BillingService_IDs { get; set; }
        public Nullable<long> Loginhistory_ID { get; set; }
    
        public virtual um_EmailMessage_Options um_EmailMessage_Options { get; set; }
    }
}