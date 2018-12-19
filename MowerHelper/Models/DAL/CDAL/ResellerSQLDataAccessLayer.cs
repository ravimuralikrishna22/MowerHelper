using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;

namespace MowerHelper.Models.DAL.CDAL
{
    public class ResellerSQLDataAccessLayer : ResellerDataAccessLayer
    {
        clsDBWrapper objclsdbwraper = new clsDBWrapper();
        const string ClassName = "MowerHelper.Models.DAL.CDAL.ResellerSQLDataAccessLayer";
        public override string Ins_Upd_CRM_Agent_Details(CRMAgent obj)
        {
            try
            {
                IDataParameter[] objParm = {
			new SqlParameter("@In_CRM_Agent_ID", (obj.CRM_Agent_ID != null ? obj.CRM_Agent_ID : null)),
			new SqlParameter("@In_CompanyName", (obj.CompanyName != null ? obj.CompanyName : null)),
			new SqlParameter("@In_Prefix", (obj.Prefix != null ? obj.Prefix : null)),
			new SqlParameter("@In_Firstname", (obj.FirstName != null ? obj.FirstName : null)),
			new SqlParameter("@In_Lastname", (obj.LastName != null ? obj.LastName : null)),
			new SqlParameter("@In_MI", (obj.MI != null ? obj.MI : null)),
			new SqlParameter("@In_Suffix", (obj.Suffix != null ? obj.Suffix : null)),
			new SqlParameter("@In_WorkPhone", (obj.WorkPhone != null ? obj.WorkPhone : null)),
			new SqlParameter("@In_MobilePhon", (obj.HomePhone != null ? obj.HomePhone : null)),
			new SqlParameter("@In_Address1", (obj.Address1 != null ? obj.Address1 : null)),
			new SqlParameter("@In_Address2", (obj.Address2 != null ? obj.Address2 : null)),
			new SqlParameter("@In_State_ID", (obj.State_ID != 0 ? obj.State_ID : null)),
			new SqlParameter("@In_City_ID", (obj.City_ID != 0 ? obj.City_ID : null)),
			new SqlParameter("@In_Expirydate", (!string.IsNullOrEmpty(obj.Expirydate) ? obj.Expirydate : null)),
			new SqlParameter("@in_ISFTAgent_Ind", (!string.IsNullOrEmpty(obj.ISFTAgent) ? obj.ISFTAgent : null)),
			new SqlParameter("@In_Incentive_ids", (!string.IsNullOrEmpty(obj.IncentiveIds) ? obj.IncentiveIds : null)),
			new SqlParameter("@In_Agent_Ind", (!string.IsNullOrEmpty(obj.IsAgent) ? obj.IsAgent : null)),
			new SqlParameter("@In_Zip", (!string.IsNullOrEmpty(obj.ZIP) ? obj.ZIP : null)),
			new SqlParameter("@In_Email", (obj.Email != null ? obj.Email : null))
		};

                objclsdbwraper.AddInParameters(objParm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.St_Admin_DML_CRM_Agents");
            }
            catch (Exception ex)
            {
                var obj1 = new clsExceptionLog();
                obj1.LogException(ex, ClassName, "Ins_Upd_CRM_Agent_Details", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }

        public override List<CRMAgent> GET_CRM_Agent_Details(CRMAgent obj)
        {
            SqlDataReader ReturnData = default(SqlDataReader);
            try
            {
                IDataParameter[] InParm = {
			new SqlParameter("@In_CRM_Agent_ID", (obj.CRM_Agent_ID != null ? obj.CRM_Agent_ID : null)),
			new SqlParameter("@In_Firstname", (obj.FirstName != null ? obj.FirstName : null)),
			new SqlParameter("@in_lastName", (obj.LastName != null ? obj.LastName : null)),
			new SqlParameter("@In_NoofRec", (obj.NoofRecords != null ? obj.NoofRecords : null)),
			new SqlParameter("@InPage_No", (obj.PageNo != null ? obj.PageNo : null)),
			new SqlParameter("@in_OrderbyItem", (obj.OrderByItem != null ? obj.OrderByItem : null)),
			new SqlParameter("@In_orderBy", (obj.OrderBy != null ? obj.OrderBy : null))
		};

                objclsdbwraper.AddInParameters(InParm);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.St_Admin_GET_CRM_Agents_Rdpaging");
                List<CRMAgent> objlist = new List<CRMAgent>();
                while (ReturnData.Read())
                {
                    CRMAgent objCRMAgent = new CRMAgent();
                    if (ReturnData["R_NO"]!=null)
                    {
                        objCRMAgent.Slno = Convert.ToInt32(ReturnData["R_NO"]);
                    }
                    if (ReturnData["CRM_Agent_ID"]!=null)
                    {
                        objCRMAgent.CRM_Agent_ID = Convert.ToString(ReturnData["CRM_Agent_ID"]);
                    }
                    if (ReturnData["AgentCode"]!=null)
                    {
                        objCRMAgent.AgentCode = Convert.ToString(ReturnData["AgentCode"]);
                    }
                    if (ReturnData["CompanyName"]!=null)
                    {
                        objCRMAgent.CompanyName = Convert.ToString(ReturnData["CompanyName"]);
                    }
                    if (ReturnData["Prefix"]!=null)
                    {
                        objCRMAgent.Prefix = Convert.ToString(ReturnData["Prefix"]);
                    }
                    if (ReturnData["Firstname"]!=null)
                    {
                        objCRMAgent.FirstName = Convert.ToString(ReturnData["Firstname"]);
                    }
                    if (ReturnData["Lastname"]!=null)
                    {
                        objCRMAgent.LastName = Convert.ToString(ReturnData["Lastname"]);
                    }
                    if (ReturnData["MI"]!=null)
                    {
                        objCRMAgent.MI = Convert.ToString(ReturnData["MI"]);
                    }
                    if (ReturnData["Suffix"]!=null)
                    {
                        objCRMAgent.Suffix = Convert.ToString(ReturnData["Suffix"]);
                    }
                    if (ReturnData["MI"] != null)
                    {
                        objCRMAgent.FullName = objCRMAgent.Prefix + " " + objCRMAgent.FirstName + " " + objCRMAgent.MI + " " + objCRMAgent.LastName + " " + objCRMAgent.Suffix;
                    }
                    else
                    {
                        objCRMAgent.FullName = objCRMAgent.Prefix + " " + objCRMAgent.FirstName + " " + objCRMAgent.LastName + " " + objCRMAgent.Suffix;
                    }
                    if (ReturnData["WorkPhone"].ToString()!= "")
                    {
                        //objCRMAgent.WorkPhone = Convert.ToString(ReturnData["WorkPhone"]);
                        objCRMAgent.WorkPhone = (ReturnData["WorkPhone"].ToString() == "" ? "-" : clsCommonFunctions.UsPhoneFormat(ReturnData["WorkPhone"].ToString()));
                    }
                    if (ReturnData["MobilePhon"].ToString()!= "")
                    {

                        //objCRMAgent.HomePhone = Convert.ToString(ReturnData["MobilePhon"]);
                        objCRMAgent.HomePhone = (ReturnData["MobilePhon"].ToString() == "" ? "-" : clsCommonFunctions.UsPhoneFormat(ReturnData["MobilePhon"].ToString()));
                    }
                    if (ReturnData["Address1"]!=null)
                    {
                        objCRMAgent.Address1 = Convert.ToString(ReturnData["Address1"]);
                    }
                    if (ReturnData["Address2"]!=null)
                    {
                        objCRMAgent.Address2 = Convert.ToString(ReturnData["Address2"]);
                    }
                    if (ReturnData["State_ID"].ToString() != "")
                    {
                        objCRMAgent.State_ID = Convert.ToInt32(ReturnData["State_ID"]);
                    }
                    if (ReturnData["State"] != null)
                    {
                        objCRMAgent.State = Convert.ToString(ReturnData["State"]);
                    }
                    if (ReturnData["City"].ToString() != "")
                    {
                        objCRMAgent.City = Convert.ToString(ReturnData["City"]);
                    }
                    if (ReturnData["City_ID"].ToString() != "")
                    {
                        objCRMAgent.City_ID = Convert.ToInt32(ReturnData["City_ID"]);
                    }
                    if (ReturnData["Email"] != null)
                    {
                        objCRMAgent.Email = Convert.ToString(ReturnData["Email"]);
                    }
                    if (ReturnData["Status_ind"] != null)
                    {
                        objCRMAgent.Status_Ind = Convert.ToString(ReturnData["Status_ind"]);
                    }
                    if (ReturnData["Expirydate"] != null)
                    {
                        objCRMAgent.Expirydate = Convert.ToString(ReturnData["Expirydate"]);
                    }
                    if (ReturnData["ISFTAgent_Ind"] != null)
                    {
                        objCRMAgent.ISFTAgent = Convert.ToString(ReturnData["ISFTAgent_Ind"]);
                    }
                    if (ReturnData["Incentives"] != null)
                    {
                        objCRMAgent.IncentiveIds = Convert.ToString(ReturnData["Incentives"]);
                    }
                    if (ReturnData["ZIP"] != null)
                    {
                        objCRMAgent.ZIP = Convert.ToString(ReturnData["ZIP"]);
                    }
                    if (ReturnData["Promocodeurl"] != null)
                    {
                        objCRMAgent.Promocodeurl = Convert.ToString(ReturnData["Promocodeurl"]);
                    }
                    if (ReturnData["Incentivetext"] != null)
                    {
                        objCRMAgent.Incentivetext = Convert.ToString(ReturnData["Incentivetext"]);
                    }
                    objlist.Add(objCRMAgent);
                }
                if (ReturnData.NextResult())
                {
                    if (ReturnData.Read())
                    {
                        CRMAgent.TotalRecords = Convert.ToInt32(ReturnData["TotlaRec"]);
                    }
                }
                return objlist;
            }
            catch (Exception ex)
            {
                var obj1 = new clsExceptionLog();
                obj1.LogException(ex, ClassName, "GET_CRM_Agent_Details", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string Active_InActive_CRM_Agent_Details(CRMAgent obj)
        {
            try
            {
                IDataParameter[] objParm = {
			new SqlParameter("@In_CRM_Agent_ID", (obj.CRM_Agent_ID != null ? obj.CRM_Agent_ID : null)),
			new SqlParameter("@In_Status_Ind", (obj.Status_Ind != null ? obj.Status_Ind : null)),
			new SqlParameter("@In_UpDatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_CRM_Promocode_ID", (obj.PromocodeId != 0 && obj.PromocodeId != null ? obj.PromocodeId : null))
		};
                objclsdbwraper.AddInParameters(objParm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.St_Admin_UPD_CRM_Agents");
            }
            catch (Exception ex)
            {
                var obj1 = new clsExceptionLog();
                obj1.LogException(ex, ClassName, "Active_InActive_CRM_Agent_Details", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
    }
}