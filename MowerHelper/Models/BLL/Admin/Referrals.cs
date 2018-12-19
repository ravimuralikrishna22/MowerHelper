using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Admin
{
    public class Referrals
    {



        public int? Fieldid { get; set; }
        public int Moduleid { get; set; }
        public string Modulename { get; set; }
        public string Fieldname { get; set; }
        public string descriptionfield { get; set; }
        public string moduledescription { get; set; }
        public string statusindfield { get; set; }
        public string FieldIDString { get; set; }
        public string Title { get; set; }
        public string OrderByItem { get; set; }
        public string OrderBy { get; set; }
        public int? NoofRecords { get; set; }
        public string PageNo { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public int? Category_id { get; set; }
        public int? ArticalIndex { get; set; }
        public string Article_Desc { get; set; }
        public int? Article_ID { get; set; }
        public string CreatedOn { get; set; }
        public string Online_ind { get; set; }
        public string Imagepath { get; set; }
        public static int? TotalRecords { get; set; }
        public string Category_Title { get; set; }
        public int ArticleCategory_ID { get; set; }
        public int ParentCategory_ID { get; set; }
        public string CreatedBy { get; set; }
        public string Related_Article_ID { get; set; }
        public string Related_video_id { get; set; }
        public string Author_Desc { get; set; }
        public int? Provider_ID { get; set; }
        public int ArticlesViewCount { get; set; }
        public int ArticlesReadCount { get; set; }
        public string ProviderName { get; set; }
        public string PublicURL { get; set; }
        public string searchby { get; set; }
        public string Description { get; set; }
        public int slno { get; set; }
        public string strexplictids { get; set; }
        public int? Index_Id { get; set; }
        public string ArticleCategory_IDs { get; set; }
        public Referrals()
        {
        }
        public Referrals(int in_ArticleCategory_ID, string in_Category_Title)
        {
            ArticleCategory_ID = in_ArticleCategory_ID;
            Category_Title = in_Category_Title;
        }
        public static DataSet List_field_description(string id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.List_field_description(id);
        }

        public static DataSet GetRenewChargeDetails(string Login_id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.GetRenewChargeDetails(Login_id);
        }
        public static DataSet GetBillingChargeDetails(string Login_id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.GetBillingChargeDetails(Login_id);
        }


        public static object upd_fieldname_description(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.upd_fieldname_description(list);
        }
        public static object Ins_module_field_description(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Ins_module_field_description(list);
        }
        public List<Referrals> BindCaregories()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.BindCaregories();
        }
        public List<Referrals> BindArticalIndex(Referrals OptionTypeID = null)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.BindArticalIndex(OptionTypeID);
        }
        public static List<Referrals> Admin_List_Articles(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Admin_List_Articles(list);
        }
        public static object Ins_Article(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Ins_Article(list);
        }
        public static object upd_Articles(Referrals id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.upd_Articles(id);
        }
        public static object Del_Article(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Del_Article(id);
        }

        public static List<Referrals> List_publicArticles(Referrals list, ref int Total_Records)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.List_publicArticles(list, ref Total_Records);
        }
        public static List<Referrals> List_publicArticles1(Referrals list, string ArticleIDs, ref int Total_Records)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.List_publicArticles1(list, ArticleIDs, ref Total_Records);
        }
        public static List<Referrals> ListBindArticles(string strSearchCondition, string strText, Referrals list, ref int Total_Records)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.ListBindArticles(strSearchCondition, strText, list, ref Total_Records);
        }
        public static object Ins_ArticleAuthor(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Ins_ArticleAuthor(list);
        }
        public static object Ins_ArticleIndexing(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Ins_ArticleIndexing(list);
        }
        public static object Upd_ArticleIndexing(Referrals list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Upd_ArticleIndexing(list);
        }


        public static List<Referrals> Getarticles1(Referrals Objarticles)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Getarticles1(Objarticles);
        }
        public static System.Data.DataSet Get_Articles(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_Articles(id);
        }

        public static DataSet Get_ArticleIndexing(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_ArticleIndexing(id);
        }
        public static DataSet Get_RelatedArticleID(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_RelatedArticleID(id);
        }

        public static DataSet Get_RelatedvideoID(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_RelatedvideoID(id);
        }

    }
}