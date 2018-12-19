using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class Message
    {
        public int Sender_ID
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
        public int MsgCategory_ID
        {
            get;
            set;
        }
        public string AddSignature
        {
            get;
            set;
        }
        public int Reply_ID
        {
            get;
            set;
        }
        public int CreatedBy
        {
            get;
            set;
        }
        public string Reciever_IDs
        {
            get;
            set;
        }
        public int Service_ID
        {
            get;
            set;
        }
        public int Patient_ID
        {
            get;
            set;
        }
        public int Provider_ID
        {
            get;
            set;
        }
        public int From_RoleID
        {
            get;
            set;
        }
        public int Reciever_RoleID
        {
            get;
            set;
        }
        //public static int Practice_ID
        //{
        //    get;
        //    set;
        //}
        public int Message_ID
        {
            get;
            set;
        }
        public string AttachedFilename
        {
            get;
            set;
        }
        public string Encrypted_FileName
        {
            get;
            set;
        }
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

        public Message(int VSender_ID, string VSubject, string VMessagebody, int VMsgCategory_ID, string VAddSignature, int VReply_ID, int VCreatedBy, string VReciever_IDs, int VService_ID, int VPatient_ID,

int VProvider_ID, int VFrom_RoleID, int VReciever_RoleID)
{
	Sender_ID = VSender_ID;
	Subject = VSubject;
	Messagebody = VMessagebody;
    MsgCategory_ID = VMsgCategory_ID;
    AddSignature = VAddSignature;
    Reply_ID = VReply_ID;
    CreatedBy = VCreatedBy;
    Reciever_IDs = VReciever_IDs;
    Service_ID = VService_ID;
    Patient_ID = VPatient_ID;
    Provider_ID = VProvider_ID;
    From_RoleID = VFrom_RoleID;
    Reciever_RoleID = VReciever_RoleID;

}

        public Message(int VSender_ID, string VSubject, string VMessagebody, int VMsgCategory_ID, string VAddSignature, string VAttachedFilename, string VEncrypted_FileName, int VReciever_RoleID, string VReciever_IDs)
        {
            Sender_ID = VSender_ID;
            Subject = VSubject;
            Messagebody = VMessagebody;
            MsgCategory_ID = VMsgCategory_ID;
            AddSignature = VAddSignature;
            AttachedFilename = VAttachedFilename;
            Encrypted_FileName = VEncrypted_FileName;
            Reciever_RoleID = VReciever_RoleID;
            Reciever_IDs = VReciever_IDs;
        }
        public Message(int VAttach_ID, string VAttachedFileName, string VAttachedFileName_Encrypted)
{
	Attach_ID = VAttach_ID;
	AttachedFileName = VAttachedFileName;
	AttachedFileName_Encrypted = VAttachedFileName_Encrypted;
}
        public bool Save()
        {
            //DataAccessLayerBaseClass DBLayer = DataAccessLayerBaseClassHelper.GetDataAccessLayer();
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            
            int TempId = DBLayer.InsertMessage(this);
            if (TempId > 0)
            {
                Message_ID = TempId;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SaveGroupMesssage()
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            int TempId = DBLayer.InsertGroupMessage(this);
            if (TempId > 0)
            {
                Message_ID = TempId;
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool MessageStation_INS_ArchivedMessages(int _id, int Reference_Id)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.MessageStation_INS_ArchivedMessages(_id, Reference_Id);
            return true;
        }
    }
}