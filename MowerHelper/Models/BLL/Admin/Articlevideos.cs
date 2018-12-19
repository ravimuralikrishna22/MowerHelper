using MowerHelper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MowerHelper.Models.BLL.Admin
{
    public class Articlevideos
    {

        public string Description { get; set; }
        public int? ParentVideo_ID { get; set; }
        public string ExceptVideo_Ids { get; set; }
        public string videoCount { get; set; }
        public string EmailCount { get; set; }
        public string Video_path { get; set; }
        public string Duration { get; set; }
        public string Online_ind { get; set; }
        public int Messages { get; set; }
        public double BALANCE { get; set; }
        public DateTime NEXTAPPOINTMENt { get; set; }
        public static string Out_msg { get; set; }
        public string IMAGENAME { get; set; }
        public string PublicVideo_ID { get; set; }
        public string Title { get; set; }
        public string Video_Description{ get; set; }
        public string FILENAME{ get; set; }
        public string FileType { get; set; }
        public string File_Path { get; set; }   
        public string EncryptedFile_Path { get; set; }
        public string ImagePath { get; set; }      
        public string EncryptedImagepath { get; set; }      
        public string ImagepathSuffix { get; set; }      
        public char Status_Ind { get; set; }
        public string CreatedOn { get; set; }
        public int Createdby { get; set; }
        public string UdatedOn { get; set; }     
        public int UpdatedBy { get; set; }   
        public int slno { get; set; }       
        public string OutMsg { get; set; }
        public int NoofRecords { get; set; }       
        public string PageNo { get; set; }     
        public string OrderByItem { get; set; }      
        public string OrderBy { get; set; }       
        public static int TotalRecords { get; set; }       
        public string searchby { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
   
        public string Youtube_embededtext{ get; set; }
    
        public string VideoId{ get; set; }
      
        public string VideoType{ get; set; }
        public int SINo { get; set; }
        public int ViewdCount { get; set; }
        public int Emailcount { get; set; }
        public int ArticleCategory_ID { get; set; }
        public string Category_Title { get; set; }
        public int ArticleIndexId { get; set; }
        public static DataSet Get_videos(int id)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.Get_videos(id);
        }

        public static object Insert_videos(Articlevideos list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Insert_videos(list);
        }
        public static object upd_videos(Articlevideos list)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.upd_videos(list);
        }
        public static object Del_videos(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Del_videos(id);
        }
        public static List<Articlevideos> List_Relatedvideos(Articlevideos video)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.List_Relatedvideos(video);
        }
        public static List<Articlevideos> List_videos(Articlevideos list, string videoIDs, string tabInd = null)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.List_videos(list, videoIDs, tabInd);
        }
        public static DataSet Get_Relatedvideos(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_Relatedvideos(id);
        }
        public static DataSet Get_RelatedArticle(int id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_RelatedArticle(id);
        }
            public static DataSet GetArticlesORVideosCount(string SiteStatisticID, string AVInd, string Title=null, string Desc=null) {
        SQLDataAccessLayer layer = new SQLDataAccessLayer();
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        return layer.GetArticlesORVideosCount(SiteStatisticID, AVInd, Title, Desc);
    }
            public static DataSet list_ArticlesCategories(string strSearchCondition, string strText)
            {
                SQLDataAccessLayer layer = new SQLDataAccessLayer();
                return layer.list_ArticlesCategories(strSearchCondition, strText);
            }
            public static List<Articlevideos> list_CategorieswiseIndexes(int CategoryID, string strSearchCondition, string strText)
            {
                SQLDataAccessLayer layer = new SQLDataAccessLayer();
                return layer.list_CategorieswiseIndexes(CategoryID, strSearchCondition, strText);
            }
    }
}