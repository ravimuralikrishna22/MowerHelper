using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models.DAL.CDAL;
    public class Publicfaq
    {
  
        
        #region "Properties of Livesupports"
        public string providerAcc_ind
        {
            get;
            set;
        }
        public int addedby
        {
            get;
            set;
        }
        public string strcanmsg
        {
            get;
            set;
        }
        public string processedcan
        {
            get;
            set;
        }
        public string quickword
        {
            get;
            set;
        }
        public string depts
        {
            get;
            set;
        }
        public string users
        {
            get;
            set;
        }
        public int killid
        {
            get;
            set;
        }
        public int canid
        {
            get;
            set;
        }
        public string userid
        {
            get;
            set;
        }
        public int currentpage
        {
            get;
            set;
        }
        public int pagesize
        {
            get;
            set;
        }
        public string strcanname
        {
            get;
            set;
        }
        public string strusername
        {
            get;
            set;
        }
        public string strcanscope
        {
            get;
            set;
        }
        public string cancmd
        {
            get;
            set;
        }
        #endregion
        #region "Properties"
        public string msg
        {
            get;
            set;
        }
        public string hdnverificationcode
        {
            get;
            set;
        }
        //public string sitename
        //{
        //    get;
        //    set;
        //}
        public string DateFrom
        {
            get;
            set;
        }

        public string DateTo
        {
            get;
            set;
        }
        public string Rating_id
        {
            get;
            set;
        }
        public string R_No
        {
            get;
            set;
        }
        public string strindex_id
        {
            get;
            set;
        }
        //public string CE_Vendor_ind
        //{
        //    get;
        //    set;
        //}

        //public string CE_Provider_ind
        //{
        //    get;
        //    set;
        //}
        //public string Jobposter_ind
        //{
        //    get;
        //    set;
        //}
        //public string BillingClerk_ind
        //{
        //    get;
        //    set;
        //}
        public string strfromdate
        {
            get;
            set;
        }
        public string strtodate
        {
            get;
            set;
        }
        public int? strVideoid
        {
            get;
            set;
        }
        public int AutofaqID
        {
            get;
            set;
        }
        public string strExcept
        {
            get;
            set;
        }
        public string ViewedCount
        {
            get;
            set;
        }
        public int? Relatedfaq_Id
        {
            get;
            set;
        }
        public string strDelfaqIds
        {
            get;
            set;
        }
        public string strDelLinkIds
        {
            get;
            set;
        }
        public int? UpdatedBy
        {
            get;
            set;
        }
        public static int TotalRecords
        {
            get;
            set;
        }
        public string Provider_Ind
        {
            get;
            set;
        }
        //public string Patient_Ind
        //{
        //    get;
        //    set;
        //}
        public int AutorelFaqID
        {
            get;
            set;
        }
        public int AutoLinkID
        {
            get;
            set;
        }
        public string Category_Id
        {
            get;
            set;
        }
        public string Category_Name
        {
            get;
            set;
        }
        public string Question_id
        {
            get;
            set;
        }
        public string Question
        {
            get;
            set;
        }


        public string Answer
        {
            get;
            set;
        }
        public int Answer_id
        {
            get;
            set;
        }
        public string Status_Ind
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }
        public int? role_id
        {
            get;
            set;
        }
        public int? From_Roleid
        {
            get;
            set;
        }
        public int? To_Roleid
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string public_ind
        {
            get;
            set;
        }
        public int? Ratevalue
        {
            get;
            set;
        }
        public string Comment
        {
            get;
            set;
        }
        public string Searchby
        {
            get;
            set;
        }

        public string Avgrating
        {
            get;
            set;
        }
        public string Loginhistory_ID
        {
            get;

            set;
        }
        public string Verfication_User_ind
        {
            get;
            set;
        }
        public string verificationcode
        {
            get;
            set;
        }
       
        public string Rate_value
        {
            get;
            set;
        }
        public static bool InsertQuestion(Publicfaq objQuestion)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.InsertQuestion(objQuestion);
        }
        public static List<Publicfaq> GetAnswer(Publicfaq objQuestion)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.GetAnswer(objQuestion);
        }

        public static bool InsertComment(Publicfaq objQuestion)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.InsertComment(objQuestion);
        }
        public static List<Publicfaq> GetFaqs(Publicfaq objQuestion)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.GetFaqs(objQuestion);
        }

        public static List<Publicfaq> GetTopFaqs(Publicfaq objQuestion)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.GetTopFaqs(objQuestion);
        }
        public static List<Publicfaq> ListFAQComments(Publicfaq objFaq)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.ListFAQComments(objFaq);
        }
        public static List<Publicfaq> GetQuesCategories()
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.GetQuesCategories();
        }
        public static int insertAdminQuestions(Publicfaq objitems)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.insertAdminQuestions(objitems);
        }
        public static bool AddRelatedLink(Publicfaq objlink)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.AddRelatedLink(objlink);
        }
        public static bool DML_Relatedfaqs(Publicfaq objfaq)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.DML_Relatedfaqs(objfaq);
        }
        public static bool UpdateQuestion(Publicfaq objqid)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.UpdateQuestion(objqid);
        }
        public static bool Resetratings(Publicfaq objfaq)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.Resetratings(objfaq);
        }
        public static List<Publicfaq> Get_FaqsComments(Publicfaq objFaq)
        {
            SQLDataAccessLayer DbLayer = new SQLDataAccessLayer();
            return DbLayer.Get_FaqsComments(objFaq);
        }
        public string Status
        {
            get;
            set;
        }
        public string OrderBy
        {
            get;
            set;
        }
        public string OrderByItem
        {
            get;
            set;
        }
        public string PageNo
        {
            get;
            set;
        }
        public string NoofRecords
        {
            get;
            set;
        }
        public int Sino
        {
            get;
            set;
        }
        public string CreatedOn
        {
            get;
            set;
        }
        public string UpdatedOn
        {
            get;
            set;
        }
        public string Description
        {

            get;
            set;
        }
        public string Keyword
        {
            get;
            set;
        }
        public string LinkUrl
        {
            get;
            set;
        }
        public string verified_Ind
        {
            get;
            set;
        }
        public string LinkName
        {
            get;
            set;
        }
        public string LinkUrlDescription
        {
            get;
            set;
        }
        public string RelatedLink_Id
        {
            get;
            set;
        }
        public string display_Ind
        {
            get;
            set;
        }
        public string AvailableTo
        {
            get;
            set;
        }
        public string Keywords
        {
            get;
            set;
        }
        public string relatedlinks
        {
            get;
            set;
        }
        public string relatedfaqs1
        {
            get;
            set;
        }
        public string rating
        {
            get;
            set;
        }
        public string countstring
        {
            get;
            set;
        }

        public string Postedby
        {
            get;
            set;
        }

        public static List<Publicfaq> BindCategory(string role_id)
        {
            SQLDataAccessLayer obj = new SQLDataAccessLayer();
            return obj.BindCategory(role_id);
        }
        public static DataSet GetRelatedLink(string objlinkID)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.GetRelatedLink(objlinkID);
        }
        
        public static DataSet Get_Relatedfaqs(Publicfaq objlist)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.Get_Relatedfaqs(objlist);
        }
        public static bool ArchiveFAQComment(Publicfaq obj)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.ArchiveFAQComment(obj);
        }
        public static DataSet EditQuestion(string objqid)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.EditQuestion(objqid);
        }
        public static List<Publicfaq> GetQuestions(Publicfaq objFaq)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.GetQuestions(objFaq);
        }
        public static List<Publicfaq> GetRelatedQuestions(Publicfaq objFaq)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.GetRelatedQuestions(objFaq);
        }
        public static bool DeleteQuestion(string Qid, int Loginhistory_ID)
        {
            MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer DbLayer = new MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer();
            return DbLayer.DeleteQuestion(Qid, Loginhistory_ID);
        }
     #endregion
        #region "MVC"
        private IEnumerable<SelectListItem> _CategoryList;
        public IEnumerable<SelectListItem> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; }
        }
        private string _Category;
        private string _Category1;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public string Category1
        {
            get { return _Category1; }
            set { _Category1 = value; }
        }
     
        public string FriendsEmail
        {
            get;
            set;
        }
        public string Yourname
        {
            get;
            set;
        }
        public string YourEmail
        {
            get;
            set;
        }
        public string Friendsname
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string ImageKeyOne
        {
            get;
            set;
        }
        public string Verifycode
        {
            get;
            set;
        }
        public string qid
        {
            get;
            set;
        }
        public string qtn
        {
            get;
            set;
        }
        public string parentdropdownid
        {
            get;
            set;
        }
        #endregion

    }
