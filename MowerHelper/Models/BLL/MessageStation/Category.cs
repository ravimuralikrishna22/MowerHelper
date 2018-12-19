using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DALSchedule;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
using System.Data;
using System.Data.SqlClient;
namespace MowerHelper.Models.BLL.MessageStation
{
    public class Category
    {
        #region "PRIVATE FIELDS"
        private string _MessageCategory;
        private int _MessageCategory_ID;
        private int _NewMsgsCount;
        private string _FilePath;
        private string _Type;
        private string _Status;
        private string _Sysgenerated;

        private string _Usergenerated;
        //Mail fields
        private string _strEmailCategoryID;
        private int _intCategoryCount;
        private string _strFromReference_id;
        private string _strToreference_id;
        private string _strISWebService_IND;
        private string _strApplication_Id;

        private int _Prov_id;
        private string _yourname;
        private string _youremail;
        private string _friendsname;
        private string _friendsemail;
        private string _verificationcode;
        private string _message;
        private string _providerid;
        private string _providername;
        private string _address;
        private string _publicurl;
        private string _mobiletext;
        private string _subject;
        private string _practiceid;
        private string _provideremail;
        private string _hdnkey;
        private string _randomnumber;
        #endregion

        #region "PROPERTIES"
        public string hdnkey
        {
            get { return _hdnkey; }
            set { _hdnkey = value; }
        }
        public string PracticeId
        {
            get { return _practiceid; }
            set { _practiceid = value; }
        }
        public string ProviderEmail
        {
            get { return _provideremail; }
            set { _provideremail = value; }
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public string MobileText
        {
            get { return _mobiletext; }
            set { _mobiletext = value; }
        }
        public string randomnumber
        {
            get { return _randomnumber; }
            set { _randomnumber = value; }
        }
        public string ProviderNmae
        {
            get { return _providername; }
            set { _providername = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public string PublicURL
        {
            get { return _publicurl; }
            set { _publicurl = value; }
        }
        public string Providerid
        {
            get { return _providerid; }
            set { _providerid = value; }
        }
        public string Yourname
        {
            get { return _yourname; }
            set { _yourname = value; }
        }
        public string YourEmail
        {
            get { return _youremail; }
            set { _youremail = value; }
        }
        public string FriendsName
        {
            get { return _friendsname; }
            set { _friendsname = value; }
        }
        public string FriendsEmail
        {
            get { return _friendsemail; }
            set { _friendsemail = value; }
        }
        public string Vrificationcode
        {
            get { return _verificationcode; }
            set { _verificationcode = value; }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        //Public ReadOnly Property MessageCategory_ID() As Integer
        //    Get
        //        Return _MessageCategory_ID
        //    End Get
        //End Property
        public int MessageCategory_ID
        {
            get { return _MessageCategory_ID; }
            set { _MessageCategory_ID = value; }
        }
        public int Prov_id
        {
            get { return _Prov_id; }
            set { _Prov_id = value; }
        }

        public string MessageCategory
        {
            get
            {
                if (_MessageCategory == null || _MessageCategory.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _MessageCategory;
                }
            }
            set { _MessageCategory = value; }
        }

        public string FilePath
        {
            get
            {
                if (_FilePath == null || _FilePath.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _FilePath;
                }
            }
            set { _FilePath = value; }
        }

        public string Sysgenerated
        {
            get
            {
                if (_Sysgenerated == null || _Sysgenerated.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _Sysgenerated;
                }
            }
            set { _Sysgenerated = value; }
        }

        public string Usergenerated
        {
            get
            {
                if (_Usergenerated == null || _Usergenerated.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _Usergenerated;
                }
            }
            set { _Usergenerated = value; }
        }

        //Public ReadOnly Property NewMsgsCount() As Integer
        //    Get
        //        Return _NewMsgsCount
        //    End Get
        //End Property
        public int NewMsgsCount
        {
            get { return _NewMsgsCount; }
            set { _NewMsgsCount = value; }
        }

        public string Type
        {
            get
            {
                if (_Type == null || _Type.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _Type;
                }
            }
            set { _Type = value; }
        }

        public string Status
        {
            get
            {
                if (_Status == null || _Status.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return _Status;
                }
            }
            set { _Status = value; }
        }

        public string EmailCategoryID
        {
            get { return _strEmailCategoryID; }
            set { _strEmailCategoryID = value; }
        }


        public int EmailCategoryCount
        {
            get { return _intCategoryCount; }
            set { _intCategoryCount = value; }
        }
        public string FromReference_id
        {
            get { return _strFromReference_id; }
            set { _strFromReference_id = value; }
        }
        public string Toreference_id
        {
            get { return _strToreference_id; }
            set { _strToreference_id = value; }
        }
        public string ISWebService_IND
        {
            get { return _strISWebService_IND; }
            set { _strISWebService_IND = value; }
        }
        public string Application_Id
        {
            get { return _strApplication_Id; }
            set { _strApplication_Id = value; }
        }
        public string FromReferenceType_id
        {
            get;
            set;
        }
        public string Toreferencetype_id
        {
            get;
            set;
        }

        #endregion
        public static List<Category> SentEmailLog(Category objCategory)
        {
            SCHSqlDataAccessLayer DbLayer = new SCHSqlDataAccessLayer();
            return DbLayer.SentEmailLog(objCategory);
        }
        public static int EmailMsgcount(int intCategoryID)
        {
            SCHSqlDataAccessLayer DbLayer = new SCHSqlDataAccessLayer();
            return DbLayer.EmailCategoryCount(intCategoryID);
        }
        public static List<Category> GetMs_GetCategorieslist(int LoginID, int RoleID, int Prov_id)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.GetMs_GetCategorieslist(LoginID, RoleID, Prov_id);
        }
        public static DataSet GetMs_GetCategoriesDS(int LoginID, int RoleID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetMs_GetCategoriesDS(LoginID, RoleID);
        }
        //GetMs_GetCategoriesDS


    }
}