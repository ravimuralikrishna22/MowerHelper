using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using System.Data;

namespace MowerHelper.Models.BLL.Forums
{
    public class Forums
    {
        public string ForumName { get; set; }
        public int? Login_ID { get; set; }
        public string Message { get; set; }
        public string ModeratorForum_Ind { get; set; }
        public string PostNewReplies { get; set; }
        public string PostNewTopics { get; set; }
        public string Public_Ind { get; set; }
        public string ReadTopicsPosts { get; set; }
        public int? Role_ID { get; set; }
        public int? Forum_ID { get; set; }
        public int Moderator_ID { get; set; }
        public string Reference_IDs { get; set; }
        public int? ReferenceType_ID { get; set; }
        public string EndDate { get; set; }
        public string Keyword { get; set; }
        public string LatestPost { get; set; }
        public string Name { get; set; }
        public int? NoofRecords { get; set; }
        public string OrderByItem { get; set; }
        public string OrderBy { get; set; }
        public int? PageNo { get; set; }
        public int Replies { get; set; }
        public int Slno { get; set; }
        public string StartDate { get; set; }
        public int Topics { get; set; }
      public static int TotalRecords { get; set; }
      public string LastActivity { get; set; }
      public string Moderator { get; set; }
      public int? MessageID { get; set; }
      public string Status_Ind { get; set; }
      public int? TOPIC_ID { get; set; }
      public string CreatedBy { get; set; }
      public string Image_Name { get; set; }
      public string TopicDescription { get; set; }
      public string TopicName { get; set; }
      public string Reply_ID { get; set; }
      public string Reply_Ind { get; set; }
      public string Author { get; set; }
      public static string Forumdescription { get; set; }
      public string ModeratorName { get; set; }
      public int Provider_ID { get; set; }
      public string CreatedOn { get; set; }
      public string Message_ID { get; set; }
      public string MessageDescription { get; set; }
      public string MessageName { get; set; }
      public string UserName { get; set; }
      public string Locked_Ind { get; set; }
      public Forums()
      {
      }
        //Topic Search
      public Forums(int _TOPIC_ID, string _TopicName, string _Image_Name, string _Author, int _Replies, string _LastActivity, string _CreatedBy, string _TopicDescription, string _Status_Ind, string _Forumdescription)
{
	TOPIC_ID = _TOPIC_ID;
	TopicName = _TopicName;
	Image_Name = _Image_Name;
	Author = _Author;
	Replies = _Replies;
	LastActivity = _LastActivity;
	CreatedBy = _CreatedBy;
	TopicDescription = _TopicDescription;
	Status_Ind = _Status_Ind;
	Forumdescription = _Forumdescription;
}

      public Forums(int _Forum_ID, string _OrderByItem, string _OrderBy)
{
	Forum_ID = _Forum_ID;
	OrderByItem = _OrderByItem;
	OrderBy = _OrderBy;
}

      public Forums(string _image_Name, string _MessageName, string _UserName, string _MessageDescription, string _Message_ID, string _TopicName, string _CreatedOn, string _Status_Ind)
{
	Image_Name = _image_Name;
	MessageName = _MessageName;
	UserName = _UserName;
	MessageDescription = _MessageDescription;
	Message_ID = _Message_ID;
	TopicName = _TopicName;
	CreatedOn = _CreatedOn;
	Status_Ind = _Status_Ind;

}


        public static string Insert_Forum(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Insert_Forum(objforum);
        }
        public static string INS_UPD_Forum_Moderators(Forums obj)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.INS_UPD_Forum_Moderators(obj);
        }
        public static List<Forums> Get_Forum(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_Forum(objforum);
        }

        public static bool Del_Forum(int Forum_ID)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Del_Forum(Forum_ID);
        }
        public static bool Update_Forum(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Update_Forum(objforum);
        }
        public static List<Forums> Get_Topics_forum(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_Topics_forum(objforum);
        }

        public static List<Forums> Search_ForumTopic(Forums obj)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Search_ForumTopic(obj);
        }

        public static List<Forums> Search_ForumMessages(Forums obj)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Search_ForumMessages(obj);
        }
        public static bool Insert_Topic(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Insert_Topic(objforum);
        }

        public static bool Del_FormTopics(int TopicID, int UpdatedBy)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Del_FormTopics(TopicID, UpdatedBy);
        }

        public static bool Upd_TopicStatus(int TopicID, string Status_Ind)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Upd_TopicStatus(TopicID, Status_Ind);
        }

        public static List<Forums> Get_Moderators(int Forum_ID)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_Moderators(Forum_ID);
        }
        public static int Insert_ReplyMsg(Forums objforum)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Insert_ReplyMsg(objforum);
        }
        public static bool Upd_MessageStatus(int Message_ID, string Status_Ind)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Upd_MessageStatus(Message_ID, Status_Ind);
        }

        public static bool Del_MessagesForTopics(int Message_ID, int UpdatedBy)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Del_MessagesForTopics(Message_ID, UpdatedBy);
        }

       public static List<Forums> Get_FourmTopicDescription(int Forum_ID, int? Topic_ID)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_FourmTopicDescription(Forum_ID, Topic_ID);
        }
       public DataSet Get_FourmNameTopicDescription(int Forum_ID, int? Topic_ID, string Login_id)
       {
           SQLDataAccessLayer layer = new SQLDataAccessLayer();
           return layer.Get_FourmNameTopicDescription(Forum_ID, Topic_ID,Login_id);
       }

       public static List<Forums> Get_Reply_ForumTopicMessages(int Reply_Message_Id)
       {
           SQLDataAccessLayer layer = new SQLDataAccessLayer();
           return layer.Get_Reply_ForumTopicMessages(Reply_Message_Id);
       }
    }
}