using MowerHelper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MowerHelper.Models.BLL.Admin
{
    public class Emaillog
    {
        public string SearchBy { get; set; }
        public int? CategoryiId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string OrderBYItem { get; set; }
        public string OrderBy { get; set; }
        public int NoOfRecords { get; set; }
        public int PageNo { get; set; }
        public int Rownum { get; set; }
        public int EmailLog_ID { get; set; }
        public int EmailMsgCtgid { get; set; }
        public string MsgCatDescription { get; set;}
        public string RefCount { get; set; }
        public string ReferenceName { get; set; }
        public string CreatedOn { get; set; }
        public List<Emaillog> List_Emaillog(Emaillog obj, ref int Total_records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.List_Emaillog(this, ref Total_records);
        }
        public static List<Emaillog> DDL_category(Emaillog objct)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.DDL_category(objct);
        }
    }
}