using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MowerHelper.Models.DAL;
using MowerHelper.Models.DAL.DataAccessLayer;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class ListMessage
    {
        public int Message_ID
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Messagebody
        {
            get;
            set;
        }
        public string SendingDate
        {
            get;
            set;
        }
        public int MsgCategory_ID
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public int Sender_ID
        {
            get;
            set;
        }
        public string Sender
        {
            get;
            set;
        }
        public string Time
        {
            get;
            set;
        }
        public string Duration
        {
            get;
            set;
        }
        public string ArchiveInd
        {
            get;
            set;
        }
        public string Read_IndWrite
        {
            get;
            set;
        }
        public int Reply_ID
        {
            get;
            set;
        }
        public int? SlNo
        {
            get;
            set;
        }
        public string Services
        {
            get;
            set;
        }
        public object Practice_ID
        {
            get;
            set;
        }
        public object BillingServicesComapny_ID
        {
            get;
            set;
        }
        public string MsgDate
        {
            get;
            set;
        }
        public string MsgDatetime
        {
            get;
            set;
        }
        public int ReplyToRoleID
        {
            get;
            set;
        }
        //public string OpenedBy
        //{
        //    get;
        //    set;
        //}
        public string Read_Ind
        {
            get;
            set;
        }
        public string Archive_Ind
        {
            get;
            set;

        }
        public string CreatedOn
        {
            get;
            set;
        }
        public string NoOfRecords
        {
            get;
            set;
        }
        public string OrderByItem
        {
            get;
            set;
        }
        public int PageNo
        {
            get;
            set;
        }
        public string PatientName
        {
            get;
            set;
        }
        public string Payer_ID
        {
            get;
            set;
        }
        public string PayerName
        {
            get;
            set;
        }
        public int Appointment_ID { get; set; }
        public string customername { get; set; }
        public string Plan_ID
        { get; set; }
        public string PlanName { get; set; }
        public int? providerloginid { get; set; }
        public string ProviderName { get; set; }
        public string providerNotificationind { get; set; }
        public static string TotalRecords { get; set; }
        public string OrderBy { get; set; }
        public string messageType { get; set; }
        public string status { get; set; }
        public string messagePath { get; set; }
        public string sysInd { get; set; }
        public string userInd { get; set; }
        public int loginHistory { get; set; }
        public string Roleid { get; set; }
        public string RoleName { get; set; }
        public string RoleChk { get; set; }
        public string groupName { get; set; }
        public int MrId { get; set; }
        public string groupInd { get; set; }
        public string custphonenumber { get; set; }
        public string Customeraddress { get; set;}
        public string Patient_Id { get; set; }
        public ListMessage()
        {
        }
        public ListMessage(int VMessage_ID, string VSender, string VSubject, string VMessagebody, string VMsgDate, int intMsgLevel, string strOpenedBy, string strOpenedByName, string strOpenedOn, string strRead_Ind,
        string strArchive_Ind, string strMsgCategory, string strArchivedBy, string strArchivedByName, string strArchivedOn, int CategoryID, int SenderID)
{
    Message_ID = VMessage_ID;
    Sender = VSender;
    Subject = VSubject;
    Messagebody = VMessagebody;
    MsgDate = VMsgDate;
    //MsgLevel = intMsgLevel;
    Read_Ind = strRead_Ind;
    Archive_Ind = strArchive_Ind;
    Category = strMsgCategory;
    //OpenedBy = strOpenedBy;
    //OpenedByName = strOpenedByName;
    //OpenedOn = strOpenedOn;
    //ArchivedBy = strArchivedBy;
    //ArchivedByName = strArchivedByName;
    //ArchivedOn = strArchivedOn;
    MsgCategory_ID = CategoryID;
    Sender_ID = SenderID;
}
        public ListMessage(int RMessage_ID, int intReplyToRoleID, int intSender_ID, string strSubject, string strMessagebody, string strRead_Ind, string strArchive_Ind, string strMsgCategory, string SenderName, int Categoryid)
{
	Message_ID = RMessage_ID;
    ReplyToRoleID = intReplyToRoleID;
	Sender_ID = intSender_ID;
	Subject = strSubject;
	Messagebody = strMessagebody;
    Read_Ind = strRead_Ind;
    Archive_Ind = strArchive_Ind;
	Category = strMsgCategory;
	Sender = SenderName;
	MsgCategory_ID = Categoryid;
}
        public static ListMessage GetMessageDetails(int Message_Id, int Role_id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.LoadMessageDetails(Message_Id, Role_id);
        }
        public static List<ListMessage> GetMs_ListMessages(int Reciever_ID, int RoleID, int MsgCategory_ID, string ListingMsgsType, int NoofRecords, int PageNo, string OrderBy, string OrderBYitem, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ListMessages(Reciever_ID, RoleID, MsgCategory_ID, ListingMsgsType, NoofRecords, PageNo, OrderBy, OrderBYitem, ref Total_Records);
        }
        public static ListMessage ReplyLoadMessageDetails(int Message_Id, int Role_id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ReplyLoadMessageDetails(Message_Id, Role_id);
        }
        public static void ChangeStatusToArchive(int Message_Id, int Int_Reciever_ID, string sentArchive_ind)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.ChangeStatusToArchive(Message_Id, Int_Reciever_ID, sentArchive_ind);
        }
        //public static List<ListMessage> MissingInsurencePayers(ListMessage obj, string Providername = null, string StDate = null, string EndDate = null)
        //{
        //    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
        //    return DBLayer.MissingInsurencePayers(obj, Providername, StDate, EndDate);
        //}
        public List<ListMessage> getMessageListCategories(ListMessage lstMessage)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getMessageListCategories(lstMessage);
        }
        public string insert_category(ListMessage insList)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.insert_category(insList);
        }
        public List<ListMessage> FillcategoryDdl()
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillcategoryDdl();
        }
        public DataSet GetCategoryinfo(int catId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.GetCategoryinfo(catId);
        }
        public string upd_category(ListMessage updlist)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.upd_category(updlist);
        }
        public void statusCategory(int catId,int loghistory)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.statusCategory(catId, loghistory);
        }
        public List<ListMessage> getRoleSCategory(int catId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.getRoleSCategory(catId);
        }
        public void updRoleCategory(ListMessage objupd)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
             dblayer.updRoleCategory(objupd);
        }
        public List<ListMessage> MessageSettingGrid(ListMessage objlist)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.MessageSettingGrid(objlist);
        }
        public List<ListMessage> FillActivecategoryDdl()
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillActivecategoryDdl();
        }
        public List<ListMessage> FillsettingrolesDdl(int? roleId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillsettingrolesDdl(roleId);
        }
        public List<ListMessage> FillNonExistRolesDdl(int? roleId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillNonExistRolesDdl(roleId);
        }
        public List<ListMessage> FillinscategoryDdl(int? roleId)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            return dblayer.FillinscategoryDdl(roleId);
        }
        public void insMsgSettings(ListMessage InsMsgSettings)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.insMsgSettings(InsMsgSettings);
        }
        public DataSet getSetting(int MrId)
        {
            SQLDataAccessLayer dblayer=new SQLDataAccessLayer();
            return dblayer.getSetting(MrId);
        }
        public void UpdMsgSettings(ListMessage Updset)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.UpdMsgSettings(Updset);
        }
        public void delsettings(int mrid, int loghistory)
        {
            SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
            dblayer.delsettings(mrid, loginHistory);
        }
        public static List<ListMessage> GetAptrequest_ListMessages(int Login_Id, int NoofRecords, int PageNo, ref int Total_Records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetAptrequest_ListMessages(Login_Id, NoofRecords,PageNo, ref Total_Records);
        }
    }
}