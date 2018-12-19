using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class EmailMessageOption
    {
        private int _id;
        private string _Title;
        private string _Msg_Subject;
        private string _Msg_Body;
        private string _Msg_Footer;
        private int? _Email_OptionType_ID;
        private int? _EmailMessage_Option_ID;
        private string _ServiceIDs;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public string Msg_Subject
        {
            get { return _Msg_Subject; }
            set { _Msg_Subject = value; }
        }
        public string Msg_Body
        {
            get { return _Msg_Body; }
            set { _Msg_Body = value; }
        }
        public string Msg_Footer
        {
            get { return _Msg_Footer; }
            set { _Msg_Footer = value; }
        }

        public int? Email_OptionType_ID
        {
            get { return _Email_OptionType_ID; }
            set { _Email_OptionType_ID = value; }
        }
        public int? EmailMessage_Option_ID
        {
            get { return _EmailMessage_Option_ID; }
            set { _EmailMessage_Option_ID = value; }
        
       }
        public string ServiceIDs
        {
            get { return _ServiceIDs; }
            set { _ServiceIDs = value; }
        }
        public int? EmailMessageOptionType_ID
        {
            get;
            set;
        }
        public int NoOfRecords
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public int pageNo
        {
            get;
            set;
        }
        public string OrderByItem
        {
            get;
            set;
        }
        public int EmailMsgOptionBody_ID
        {
            get;
            set;
        }
        public string Message_Title
        {
            get;
            set;
        }
        public string Loginhistory_ID
        {
            get;
            set;
        }
        public string NewIDs
        {
            get;
            set;
        }
        public EmailMessageOption()
        {
        }

        public EmailMessageOption(string strMsg_title, string strMsg_subject, string strMsg_Body, string strMsg_Footer, int Email_OptionType_ID)
{
	_Title = strMsg_title;
	_Msg_Subject = strMsg_subject;
	_Msg_Body = strMsg_Body;
	_Msg_Footer = strMsg_Footer;
	_Email_OptionType_ID = Email_OptionType_ID;
}
        public EmailMessageOption(int _EmailMsgOptionBody_ID, string Message_type, string in_Message_Title, string in_Msg_Subject, string in_Msg_Body)
{
	EmailMsgOptionBody_ID = _EmailMsgOptionBody_ID;
	_Title = Message_type;
	Message_Title = in_Message_Title;
	_Msg_Subject = in_Msg_Subject;
	_Msg_Body = in_Msg_Body;
}
        public EmailMessageOption(int intEmailMessage_Option_ID, string strCategory, string strMessage_Title)
{
	_EmailMessage_Option_ID = intEmailMessage_Option_ID;
	_Title = strCategory;
	Message_Title = strMessage_Title;
}
        public EmailMessageOption(int in_EmailMessage_OptionorOptionType_ID, string in_Message_Title)
{
	_EmailMessage_Option_ID = in_EmailMessage_OptionorOptionType_ID;
	EmailMessageOptionType_ID = in_EmailMessage_OptionorOptionType_ID;
	Message_Title = in_Message_Title;
	_Title = in_Message_Title;
}



        public static EmailMessageOption GET_EmailMessage_OptionBasedonID(int id, int? EmailMessage_Option_ID = 0)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GET_EmailMessage_OptionBasedonID(id, EmailMessage_Option_ID);
        }
        public List<EmailMessageOption> ListEmailMessageOptions(EmailMessageOption obj, ref int Total_records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ListEmailMessageOptions(obj, ref Total_records);
        }
        public List<EmailMessageOption> ListEmail_Main_MessageOptions(EmailMessageOption obj, ref int Total_records)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ListEmail_Main_MessageOptions(this,ref Total_records);
        }
        public List<EmailMessageOption> ADMIN_List_Main_Email_OptionTypes()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_List_Main_Email_OptionTypes();
        }
        public List<EmailMessageOption> ADMIN_List_ExistingEmail_OptionTypes(EmailMessageOption OptionTypeID = null)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ADMIN_List_ExistingEmail_OptionTypes(OptionTypeID);
        }
        public void SaveEmailMessageOption()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.InsertEmailMessageOption(this);
        }
        public void UpdateEmailMessageOption()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
             DBLayer.UpdateEmailMessageOption(this);
        }

        public void SaveMainEmailMessageOption(int Email_OptionType_ID, string Message_Title)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
           DBLayer.InsertMainEmailMessageOption(Email_OptionType_ID, Message_Title);
        }

        public void UpdateMainEmailMessageOption(int EmailMessage_Option_ID, string Message_Title)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.UpdatemainEmailMessageOption(EmailMessage_Option_ID, Message_Title);
        }
        public static EmailMessageOption GetEmail_Main_MessageOptionbyID(int id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetEmail_Main_MessageOptionbyID(id);
        }
    }
}