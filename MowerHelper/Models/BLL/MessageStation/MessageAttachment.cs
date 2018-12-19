using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class MessageAttachment
    {
        public int Attach_ID
        {
            get;
            set;
        }
        public string AttachedFileName
        {
            get;
            set;
        }
        public string AttachedFileName_Encrypted
        {
            get;
            set;
        }

        public MessageAttachment(int VAttach_ID, string VAttachedFileName, string VAttachedFileName_Encrypted)
{
    Attach_ID = VAttach_ID;
    AttachedFileName = VAttachedFileName;
    AttachedFileName_Encrypted = VAttachedFileName_Encrypted;
}
        public bool SaveAttachment()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            int TempId = DBLayer.CreateNewMessageAttachment(this);

            if (TempId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static MessageAttachment GetMessageAttachmentById(int Message_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GetMessageAttachmentById(Message_ID);
        }
        //GetIssueAttachmentById
    }
}