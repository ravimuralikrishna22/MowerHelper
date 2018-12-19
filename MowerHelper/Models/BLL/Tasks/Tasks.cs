using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Tasks
{
    public class Tasks
    {
        private int _Task_ID;
        private string _Comments;
        private string _DueDate;
        private string _IsDone;
        //private int _Status_ind;
        private DateTime _LastAction;
        private string _Status_Ind;
        private int? _Reference_ID;
        private int? _ReferenceType_ID;
        private int _Practice_ID;
        private string _RoleInd;
        private string _sortField;
        private string _sortDirection;
        private string _Notes;
        private string _Actionitemtitle;
        private string _StartDate;
        private string _EndDate;
        private string _ChkStatus_Ind;
        private string _Taskfor;
        private string _Practicename;
        private string _TaskTitle;
        private string _TaskDate;
        private string _Assignedusers;
        private string _CreatedBy;
        private string _CreatedOn;
        private string _IsPracticeTask;
        private string _DisplaytoCoach_Ind;
        private int _Loginhistory_ID;
        private int? _Actionitem_ID;
        private string _ActiontitleDescription;
        private string _DdlAction;
        private string _CurrentDate;
        public string _ActionBy;
       public static int _Totalrecords;


        public static int Totalrecords
        {
            get { return _Totalrecords; }
            set { _Totalrecords = value; }
        }
        public string CurrentDate
        {
            get { return _CurrentDate; }
            set { _CurrentDate = value; }

        }
        public string ActionBy
        {
            get { return _ActionBy; }
            set { _ActionBy = value; }
        }

        public int Task_ID
        {
            get { return _Task_ID; }
            set { _Task_ID = value; }
        }
        public int? Actionitem_ID
        {
            get { return _Actionitem_ID; }
            set { _Actionitem_ID = value; }
        }
        public string DdlAction
        {
            get { return _DdlAction; }
            set { _DdlAction = value; }

        }

        public int Loginhistory_ID
        {
            get { return _Loginhistory_ID; }
            set { _Loginhistory_ID = value; }
        }

        public string IsPracticeTask
        {
            get { return _IsPracticeTask; }
            set { _IsPracticeTask = value; }
        }

        public string ActiontitleDescription
        {
            get { return _ActiontitleDescription; }
            set { _ActiontitleDescription = value; }
        }

        public string DisplaytoCoach_Ind
        {
            get { return _DisplaytoCoach_Ind; }
            set { _DisplaytoCoach_Ind = value; }
        }


        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        public string DueDate
        {
            get { return _DueDate; }
            set { _DueDate = value; }
        }
        public string CreatedBy
        {
         
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        
        }
        public string  CreatedOn
        {

            get { return _CreatedOn; }
            set { _CreatedOn = value; }

        }

        public string IsDone
        {
            get { return _IsDone; }
            set { _IsDone = value; }
        }
        public string Practicename
        {
            get { return _Practicename; }
            set { _Practicename = value; }
        }
        public string TaskTitle
        {
            get { return _TaskTitle; }
            set { _TaskTitle = value; }
        }
        public string TaskDate
        {
            get { return _TaskDate; }
            set { _TaskDate = value; }
        }

        public string Status_Ind
        {
            get { return _Status_Ind; }
            set { _Status_Ind = value; }
        }

        //public int Status_ind
        //{
        //    get { return _Status_ind; }
        //    set { _Status_ind = value; }
        //}

        public DateTime LastAction
        {
            get { return _LastAction; }
            set { _LastAction = value; }
        }

        public int? Reference_ID
        {
            get { return _Reference_ID; }
            set { _Reference_ID = value; }
        }
        public int? ReferenceType_ID
        {
            get { return _ReferenceType_ID; }
            set { _ReferenceType_ID = value; }
        }
        public int Practice_ID
        {
            get { return _Practice_ID; }
            set { _Practice_ID = value; }
        }
        public string RoleInd
        {
            get { return _RoleInd; }
            set { _RoleInd = value; }
        }

        public string sortField
        {
            get { return _sortField; }
            set { _sortField = value; }
        }
        public string Assignedusers
        {
            get { return _Assignedusers; }
            set { _Assignedusers = value; }
        }


        public string sortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }
        public string Actionitemtitle
        {
            get { return _Actionitemtitle; }
            set { _Actionitemtitle = value; }
        }

        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }
        public string ChkStatus_Ind
         {
             get { return _ChkStatus_Ind; }
            set { _ChkStatus_Ind = value; }
        }

        public string Taskfor
        {
            get { return _Taskfor; }
            set { _Taskfor = value; }
        }
        public static List<Tasks> Tasklist(Tasks objTask)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Tasklist(objTask);
        }
        public static bool AddNewTask(Tasks objTask)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.AddNewTask(objTask);
        }
        public static bool AddNewActionItem(Tasks objAction)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.AddNewActionItem(objAction);
        }
        public static List<Tasks> ActionItemsList(Tasks objAction)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.ActionItemsList(objAction);
        }
        public static bool UpdateTask(Tasks objTask)
        {
            SQLDataAccessLayer DBLayer =new  SQLDataAccessLayer();
            return DBLayer.UpdateTask(objTask);
        }
        public static List<Tasks> GetActionItemsList(Tasks objAction)
        {
            SQLDataAccessLayer DBLayer = new  SQLDataAccessLayer();
            return DBLayer.GetActionItemsList(objAction);
        }


    }
}