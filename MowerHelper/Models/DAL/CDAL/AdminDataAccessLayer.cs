using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Common;
namespace MowerHelper.Models.DAL.CDAL

{
    public abstract class DataAccessLayerBaseClass
    {
        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];
                if (str == null || str.Length <= 0)
                {
                    throw new ApplicationException("ConnectionString configuration is missing from you web.config. It should contain  <appSettings><add key=\"ConnectionString\" value=\"database=IssueTrackerStarterKit;server=localhost;Trusted_Connection=true\" /></appSettings> ");
                }
                else
                {
                    return str;
                }
            }
            set { _connectionString = value; }
        }  
        public delegate CollectionBase GenerateCollectionFromReader(IDataReader returnData);
        public delegate CollectionBase GenerateCollectionFromReaderTR(IDataReader returnData, ref int Total_Records);
        public abstract List<Publicfaq> BindCategory(string role_id);
        public abstract bool InsertQuestion(Publicfaq objQuestion);
        public abstract List<Publicfaq> GetAnswer(Publicfaq objQuestion);
        public abstract bool InsertComment(Publicfaq objQuestion);
        public abstract List<Publicfaq> GetFaqs(Publicfaq objQuestion);
        public abstract List<Publicfaq> GetTopFaqs(Publicfaq objQuestion);
        public abstract DataSet GetRelatedLink(string objlinkID);
        public abstract DataSet Get_Relatedfaqs(Publicfaq objlist);
        public abstract List<clsCountry> GetCitiesofProvidersBasedonStateID(int? StateID, string ZIP, int? Distance, int? CountryID);
        public abstract List<Publicfaq> ListFAQComments(Publicfaq objFaq);
        public abstract bool ArchiveFAQComment(Publicfaq obj);
        public abstract DataSet EditQuestion(string objqid);
        public abstract bool ADMIN_Insert_SitePage(SitePage obj);
        public abstract List<SitePage> ADMIN_List_SitePages(SitePage obj, ref int Total_records);
        public abstract SitePage ADMIN_GET_SitePage(int SitePage_id);
        public abstract bool ADMIN_Update_SitePage(SitePage obj);
        public abstract List<Publicfaq> GetQuestions(Publicfaq objFaq);
        public abstract List<Publicfaq> GetQuesCategories();
        public abstract List<Publicfaq> GetRelatedQuestions(Publicfaq objFaq);
        public abstract int insertAdminQuestions(Publicfaq objitems);
        public abstract bool AddRelatedLink(Publicfaq objlink);
        public abstract bool DML_Relatedfaqs(Publicfaq objFaq);
        public abstract bool DeleteQuestion(string Qid, int Loginhistory_ID);
        public abstract bool UpdateQuestion(Publicfaq objqid);
        public abstract bool Resetratings(Publicfaq objfaq);
        public abstract List<Publicfaq> Get_FaqsComments(Publicfaq objFaq);
        public abstract string GetObjectname(string ObjectID);

    }
}

