using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class messageStatus
    {
        public string receiveName { get; set; }
        public string practiceName { get; set; }
        public string patientName { get; set; }
        public string reason { get; set; }
        public string mailDate { get; set; }
        public string mailstatus { get; set; }
        public static int Totalnoofrecords { get; set; }
        public int? practiceId { get; set; }
        public int? providerId { get; set; }
        public int? patientId { get; set; }
        public int? statusId { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string orderByItem { get; set; }
        public string orderBy { get; set; }
        public string noofrecords { get; set; }
        public string pageno { get; set; }
        public string mailbody { get; set; }
        public int mailId { get; set; }
        public string isRec { get; set; }
        public string mailFrom { get; set; }
        public string mailTo { get; set; }
        public List<messageStatus> mailStatusGrid(messageStatus objMailstatus)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.mailStatusGrid(objMailstatus);
        }
        public List<messageStatus> FillProviderids()
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillProviderids();
        }
        public List<messageStatus> fillPatientName(int pracId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.fillPatientName(pracId);
        }
        public DataSet getmailInfo(int id)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getmailInfo(id);
        }
    }
}