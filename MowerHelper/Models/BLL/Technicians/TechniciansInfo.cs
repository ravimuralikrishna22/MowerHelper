using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.Classes;

namespace MowerHelper.Models.BLL.Technicians
{
    public class TechniciansInfo
    {
        public string First_Name
        {
            get;
            set;
        }
        public string Last_Name
        {
            get;
            set;
        }
        public string Phonenumber
        {
            get;
            set;
        }
        public int? Provider_id
        {
            get;
            set;
        }
        public int PageNO
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
        public  int noofrowsperpage
        {
            get;
            set;
    }
        public int NoOfrecords
        {
            get;
            set;
        }
        public int? Technician_id
        {
            get;
            set;
        }
        public static int TotalRecords
        {
            get;
            set;
        }
        public char? status_ind
        {
            get;
            set;
        }
        public string Technician_Name
        {
            get;
            set;
        }
        public string Self_ind
        {
            get;
            set;
        }
        public string Reassigned
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Roleid
        {
            get;
            set;
        }
        public string Access
        {
            get;
            set;
        }
        public string billingchange_ind
        {
            get;
            set;
        }
        public string Tech_Image { get; set; }
        public static bool InsTechniciansInfo(TechniciansInfo objInsTechniciansInfo, ref string Out_Msg)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.InsTechniciansInfo(objInsTechniciansInfo, ref Out_Msg);
        }
        public static System.Collections.Generic.List<TechniciansInfo> GetTechniciansInfo(TechniciansInfo objTechniciansInfo)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetTechniciansInfo(objTechniciansInfo);
        }
        public static TechniciansInfo GetupdTechnicianInfo(int Technician_id)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetupdTechnicianInfo(Technician_id);
        }
        public static bool UpdTechnicianInfo(TechniciansInfo objUpdTechnicianInfo, ref string Out_Msg)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.UpdTechnicianInfo(objUpdTechnicianInfo,ref Out_Msg);
        }
        public static bool upd_technicianimage(int Technician_id, string imagepath)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.upd_technicianimage(Technician_id, imagepath);
        }
        public List<TechniciansInfo> GetTechnicianList(int ProviderID,string AppointmentID)
        {
            List<TechniciansInfo> TechnicianList = new List<TechniciansInfo>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@in_provider_id", (ProviderID != 0 ? ProviderID.ToString() : null)),
        new SqlParameter("@in_appointment_id", (Convert.ToInt32(AppointmentID) != 0 ? AppointmentID.ToString() : null))
	};
            objcommon.AddInParameters(objparam);
            SqlDataReader dr = default(SqlDataReader);
            dr = objcommon.GetDataReader("Help_dbo.st_get_technician_names_list");

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TechniciansInfo obj = new TechniciansInfo();
                    if (dr["Technician_id"] != null)
                    {
                        obj.Technician_id = Convert.ToInt32(dr["Technician_id"]);
                    }
                    if (dr["Name"] != null)
                    {
                        obj.Technician_Name = dr["Name"].ToString();
                    }
                    if (dr["Phonenumber"] != null)
                    {
                        obj.Phonenumber = dr["Phonenumber"].ToString();
                    }
                    if (dr["Self_ind"] != null)
                    {
                        obj.Self_ind = dr["Self_ind"].ToString();
                    }
                    TechnicianList.Add(obj);
                }
            }
            return TechnicianList;
        }


        public List<TechniciansInfo> GetTechniciancount(int provlogid)
        {
            List<TechniciansInfo> TechnicianList = new List<TechniciansInfo>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@in_provider_login_id", (provlogid != 0 ? provlogid.ToString() : null))
	};
            objcommon.AddInParameters(objparam);
            SqlDataReader dr = default(SqlDataReader);
            dr = objcommon.GetDataReader("Help_dbo.St_check_billing__technician_count");

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TechniciansInfo obj = new TechniciansInfo();
                    if (dr["billingchange_ind"] != null)
                    {
                        obj.billingchange_ind = Convert.ToString(dr["billingchange_ind"]);
                    }
                    TechnicianList.Add(obj);
                }
            }
            return TechnicianList;
        }
    }
}