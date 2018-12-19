using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.LISTER
{
    public class ErrorsList
    {
        public string HostAddress { get; set; }
        public int Project_ID
        {
            get;
            set;
        }
        public string Class_ID
        {
            get;
            set;
        }
        public string Method_ID
        {
            get;
            set;
        }
        public string Error_BeginDate
        {
            get;
            set;
        }
        public int NoofRecords
        {
            get;
            set;
        }
        public int pageNo
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
        public static int TotalRecords
        {
            get;
            set;
        }
        public string Method_Name
        {
            get;
            set;
        }
        public string Log_ID
        {
            get;
            set;
        }
        public string InsertDate
        {
            get;
            set;
        }
        public string Error_EndDate
        {
            get;
            set;
        }
        public string ErrLine
        {
            get;
            set;
        }
        public string ErrFile
        {
            get;
            set;
        }
        public string ErrCategory
        {
            get;
            set;
        }
        public string CustomerRefID
        {
            get;
            set;
        }
        public string Class_Name
        {
            get;
            set;
        }
        public string ErrDescription
        {
            get;
            set;
        }
        public string LocalAddr
        {
            get;
            set;
        }
        public string RequestMethod
        {
            get;
            set;
        }
        public string URL
        {
            get;
            set;
        }
        public string UserAgent
        {
            get;
            set;
        }
        public string Exception_Name
        {
            get;
            set;
        }
        public string ErrSource
        {
            get;
            set;
        }
        public string Name { get; set; }
        public string Mobile_type { get; set; }
        public ErrorsList(string _Class_ID, string _Class_Name)
      {
	Class_ID = _Class_ID;
	Class_Name = _Class_Name;

     }

        public ErrorsList(string _Class_ID, string _Method_Name, string _Method_ID)
       {
	 Class_ID = _Class_ID;
	 Method_Name = _Method_Name;
	 Method_ID = _Method_ID;

     }
        public ErrorsList()
        {
        }
        public static List<ErrorsList> EList(ErrorsList objL)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.EList(objL);
        }
        public static List<ErrorsList> DDL_CLS(ErrorsList objEL)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.DDL_CLS(objEL);
        }

        public static List<ErrorsList> DDL_MTHDS(ErrorsList objM)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.DDL_MTHDS(objM);
        }

        public static ErrorsList Get_Error_Details(int Log_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_Error_Details(Log_ID);
        }
    }
}