using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models.DAL.CDAL
{

    public class SQLDataAccessLayer : DataAccessLayerBaseClass
    {
        clsDBWrapper objclsdbwraper = new clsDBWrapper();
        const string ClassName = "MowerHelper.Models.DAL.CDAL.SQLDataAccessLayer";
        public override List<Publicfaq> BindCategory(string role_id)
        {
            try
            {
                IDataParameter[] InParm = { new SqlParameter("@in_Role_id", (role_id !=null ? role_id : null)) };
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParm);
                DataSet returndata = null;
                List<Publicfaq> objdatalist = new List<Publicfaq>();
                returndata = objcommon.GetDataSet("Help_dbo.st_Cognode_LIST_FAQ_Categories");
                if (returndata.Tables.Count > 0)
                {
                    if (returndata.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in returndata.Tables[0].Rows)
                        {
                            Publicfaq obj = new Publicfaq();
                            if (dr["Category_Id"].ToString() != null)
                            {
                                obj.Category_Id = dr["Category_Id"].ToString();
                            }
                            else
                            {
                                obj.Category_Id = null;
                            }
                            if (dr["Category_Name"].ToString() != null)
                            {
                                obj.Category_Name = dr["Category_Name"].ToString();
                            }
                            else
                            {
                                obj.Category_Name = null;
                            }
                            objdatalist.Add(obj);
                            obj = null;
                        }
                    }
                }
                return objdatalist;
            }
            catch (Exception ex)
            {

                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "BindCategory", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public  override List<Publicfaq> GetTopFaqs(Publicfaq objQuestion)
        {
            try
            {
                IDataParameter[] InParm = {
			new SqlParameter("@in_Role_id", (objQuestion.role_id != 0 ? objQuestion.role_id : 0)),
			new SqlParameter("@In_Serachby", (string.IsNullOrEmpty(objQuestion.Searchby) ? null : objQuestion.Searchby)),
			new SqlParameter("@In_category_Id", (string.IsNullOrEmpty(objQuestion.Category_Id) ? null : objQuestion.Category_Id)),
			new SqlParameter("@in_Orderby", (string.IsNullOrEmpty(objQuestion.OrderBy) ? null : objQuestion.OrderBy)),
			new SqlParameter("@in_Orderbyitem", (string.IsNullOrEmpty(objQuestion.OrderByItem) ? null : objQuestion.OrderByItem))
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParm);
                DataSet ds = null;
                List<Publicfaq> objDataList = new List<Publicfaq>();
                if (objQuestion.role_id == 17)
                {
                    ds = objcommon.GetDataSet("Help_dbo.St_FAQ_Get_Public_TopFAQs");
                }
                else
                {
                    ds = objcommon.GetDataSet("Help_dbo.St_FAQ_List_TopQuestions");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Publicfaq objData = new Publicfaq();
                          
                            if (dr["Question_Id"].ToString() != null)
                            {
                                objData.Question_id = dr["Question_Id"].ToString();
                            }
                            else
                            {
                                objData.Question_id = null;
                            }

                            if (dr["Question"].ToString() != null)
                            {
                                objData.Question = (string)dr["Question"];
                            }
                            else
                            {
                                objData.Question = null;
                            }
                            objDataList.Add(objData);
                            objData = null;
                        }
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetTopFaqs", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Data.DataSet GetRelatedLink(string objlinkID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@in_Question_id", objlinkID) };
                objcommon.AddInParameters(inparam);
                DataSet dslink = new DataSet();
                dslink = objcommon.GetDataSet("Help_dbo.St_FAQ_List_RelatedLinks");
                return dslink;

            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetRelatedLink", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override bool InsertQuestion(Publicfaq objQuestion)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strBasicInfoParam = {
			new SqlParameter("@In_FromRole_Id", (objQuestion.From_Roleid == 0 ? null : objQuestion.From_Roleid)),
			new SqlParameter("@In_ToRole_ID", (objQuestion.To_Roleid == 0 ? null : objQuestion.To_Roleid)),
			new SqlParameter("@In_Category_id", (string.IsNullOrEmpty(objQuestion.Category_Id) ? null : objQuestion.Category_Id)),
			new SqlParameter("@In_Username", (string.IsNullOrEmpty(objQuestion.Username) ? null : objQuestion.Username)),
			new SqlParameter("@In_Email", (string.IsNullOrEmpty(objQuestion.Email) ? null : objQuestion.Email)),
			new SqlParameter("@In_Question", (string.IsNullOrEmpty(objQuestion.Question) ? null : objQuestion.Question)),
			new SqlParameter("@In_Status", (string.IsNullOrEmpty(objQuestion.Status_Ind) ? null : objQuestion.Status_Ind)),
			new SqlParameter("@In_public_ind", (string.IsNullOrEmpty(objQuestion.public_ind) ? null : objQuestion.public_ind)),
			new SqlParameter("@In_provider_Ind", (string.IsNullOrEmpty(objQuestion.Provider_Ind) ? null : objQuestion.Provider_Ind)),
			new SqlParameter("@In_CreatedBy", (objQuestion.CreatedBy == "0" ? null : objQuestion.CreatedBy)),
            new SqlParameter("@in_verified_Ind", (string.IsNullOrEmpty(objQuestion.verified_Ind) ? null : objQuestion.verified_Ind)),
			new SqlParameter("@in_Loginhistory_ID", HttpContext.Current.Session["Loginhistory_ID"])
		};
                objcommon.AddInParameters(strBasicInfoParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_INS_Questions");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "InsertQuestion", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }
           
        }
        public override System.Collections.Generic.List<Publicfaq> GetAnswer(Publicfaq objQuestion)
        {
            try
            {
                SqlDataReader ReturnData = default(SqlDataReader);
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strBasicInfoParam = { new SqlParameter("@in_Question_id", (objQuestion.Question_id != null? objQuestion.Question_id:null)) };
                objclsdbwraper.AddInParameters(strBasicInfoParam);
                ReturnData = objclsdbwraper.GetDataReader("Help_dbo.St_FAQ_Get_PublicAnswer");
                List<Publicfaq> objlist = new List<Publicfaq>();
                while (ReturnData.Read())
                {
                    Publicfaq objfaq = new Publicfaq();
                    if (ReturnData["Question_id"]!=null)
                    {
                        objfaq.Question_id = ReturnData["Question_id"].ToString();
                    }
                    if (ReturnData["Question"]!=null)
                    {
                        objfaq.Question = ReturnData["Question"].ToString();
                    }
                    if (ReturnData["Answertext"]!=null)
                    {
                        objfaq.Answer = ReturnData["Answertext"].ToString();
                    }
                    if (ReturnData["Avgrating"]!=null)
                    {
                        objfaq.Avgrating = ReturnData["Avgrating"].ToString();
                    }
                    if (ReturnData["ViewedCount"]!=null)
                    {
                        objfaq.ViewedCount = ReturnData["ViewedCount"].ToString();
                    }
                    if (ReturnData["UpdatedOn"]!=null)
                    {
                        objfaq.UpdatedOn = ReturnData["UpdatedOn"].ToString();
                    }
                    objlist.Add(objfaq);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetAnswer", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override bool InsertComment(Publicfaq objQuestion)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] strBasicInfoParam = {
			new SqlParameter("@In_Question_id", (objQuestion.Question_id != null ? objQuestion.Question_id:null)),
			new SqlParameter("@In_Rate_value", (objQuestion.Ratevalue != 0 ? objQuestion.Ratevalue:null)),
			new SqlParameter("@In_Comment", (string.IsNullOrEmpty(objQuestion.Comment) ? null : objQuestion.Comment)),
			new SqlParameter("@In_Role_Id", (objQuestion.role_id != null? objQuestion.role_id : null))
		};

                objcommon.AddInParameters(strBasicInfoParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_INS_Question_Rating");
                return true;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "InsertComment", HttpContext.Current.Request, HttpContext.Current.Session);
                return false;
            }
        }
        public override List<Publicfaq> GetFaqs(Publicfaq objQuestion)
        {
            try
            {
                IDataParameter[] InParm = {
			new SqlParameter("@in_Role_id", (objQuestion.role_id != 0 ? objQuestion.role_id : null)),
			new SqlParameter("@In_Serachby", (string.IsNullOrEmpty(objQuestion.Searchby) ? null : objQuestion.Searchby)),
			new SqlParameter("@In_category_Id", (string.IsNullOrEmpty(objQuestion.Category_Id) ? null : objQuestion.Category_Id))
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParm);
                DataSet ds = null;
                List<Publicfaq> objDataList = new List<Publicfaq>();
                if (objQuestion.role_id == 17)
                {
                    ds = objcommon.GetDataSet("Help_dbo.St_FAQ_Get_Public_FAQs");
                }
                else
                {
                    ds = objcommon.GetDataSet("Help_dbo.St_FAQ_List_questions");
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Publicfaq objData = new Publicfaq();
                           
                            if (dr["Question_Id"].ToString() != null  )
                            {
                                objData.Question_id = dr["Question_Id"].ToString();
                            }
                            else
                            {
                                objData.Question_id = null;
                            }

                            if (dr["Question"].ToString() != null)
                            {
                                objData.Question = (string)dr["Question"];
                            }
                            else
                            {
                                objData.Question = null;
                            }
                           
                            objDataList.Add(objData);
                            objData = null;
                        }
                    }
                }
                return objDataList;
              
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "GetFaqs", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Data.DataSet Get_Relatedfaqs(Publicfaq objlist)
        {
            try
            {
                DataSet dsset = null;
                IDataParameter[] InParam = {
			new SqlParameter("@in_Question_ID", (objlist.Question_id != null ? objlist.Question_id : null)),
			new SqlParameter("@In_public_ind", (string.IsNullOrEmpty(objlist.public_ind) ? null : objlist.public_ind)),
			new SqlParameter("@In_provider_Ind", (string.IsNullOrEmpty(objlist.Provider_Ind) ? null : objlist.Provider_Ind)),
			new SqlParameter("@in_verificationuser_ind", (string.IsNullOrEmpty(objlist.Verfication_User_ind) ? null : objlist.Verfication_User_ind)),
			new SqlParameter("@In_display_Ind", (!string.IsNullOrEmpty(objlist.display_Ind) ? objlist.display_Ind : null))
		};
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParam);
                dsset = objcommon.GetDataSet("Help_dbo.St_FAQ_Get_RelatedFaqsList");
                return dsset;
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, ClassName, "Get_Relatedfaqs", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }
        public override List<clsCountry> GetCitiesofProvidersBasedonStateID(int? StateID, string ZIP, int? Distance, int? CountryID)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] InParamList = {
			new SqlParameter("@in_State_ID", (StateID == 0 ? null : StateID)),
			new SqlParameter("@In_ZIP", (string.IsNullOrEmpty(ZIP) ? null : ZIP)),
			new SqlParameter("@In_Country_ID", (CountryID == 0 ? null : CountryID)),
			new SqlParameter("@In_Distance", (Distance == 0 ? null : Distance))
		};
                DataSet ds = new DataSet();
                List<clsCountry> objDataList = new List<clsCountry>();
                objcommon.AddInParameters(InParamList);
                ds = objcommon.GetDataSet("Help_dbo.st_Provider_Public_List_Cities");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objDataList.Add(new clsCountry
                            {
                                CityName = Convert.ToString(dr["City"]),
                                CityId = Convert.ToInt32(dr["City_ID"]),
                                StateName = Convert.ToString(dr["stateabr"]),
                                CountInCity = Convert.ToInt32(dr["CountInCity"]),
                                StateFullName=Convert.ToString(dr["state"]),
                                StateId=Convert.ToInt32(dr["State_ID"]),
                                Displaybold = Convert.ToString(dr["DisplayinBold"])

                            });
                        }
                    }
                }
                return objDataList;
            }
            catch (Exception ex)
            {
               clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, ClassName, "GetCitiesofProvidersBasedonStateID", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<Publicfaq> ListFAQComments(Publicfaq objFaq)
        {
            try
            {
                clsCommonFunctions clscomm = new clsCommonFunctions();
                SqlDataReader List_Clerksdr = default(SqlDataReader);
                List<Publicfaq> LstOfList_Clerks = new List<Publicfaq>();
                IDataParameter[] In_List_CE_CourseAuthorsParamList = {
			new SqlParameter("@in_Question_Id", (Convert.ToInt32(objFaq.Question_id) == 0 ? null : objFaq.Question_id)),
			new SqlParameter("@in_PageNo", objFaq.PageNo),
			new SqlParameter("@In_NoofRec", objFaq.NoofRecords),
			new SqlParameter("@In_OrderBy", objFaq.OrderBy),
			new SqlParameter("@In_orderBYItem", objFaq.OrderByItem),
			new SqlParameter("@in_Ouestion", objFaq.Question),
			new SqlParameter("@In_DateFrom", objFaq.DateFrom),
			new SqlParameter("@In_Dateto", objFaq.DateTo),
			new SqlParameter("@In_Status_ind", objFaq.Status_Ind)
		};

             clscomm.AddInParameters(In_List_CE_CourseAuthorsParamList);
                List_Clerksdr = clscomm.GetDataReader("Help_dbo.St_FAQ_List_QuestionComments_RDPaging");

                while (List_Clerksdr.Read())
                {
                    Publicfaq objList = new Publicfaq();
                    var _with1 = objList;
                    _with1.R_No = (!string.IsNullOrEmpty(List_Clerksdr["R_No"].ToString()) ? List_Clerksdr["R_No"].ToString() : null);
                    _with1.Question_id = (!string.IsNullOrEmpty(List_Clerksdr["Question_id"].ToString()) ? List_Clerksdr["Question_id"].ToString() : null);
                    _with1.Ratevalue = (!string.IsNullOrEmpty(List_Clerksdr["Ratevalue"].ToString()) ? Convert.ToInt32(List_Clerksdr["Ratevalue"].ToString()) : 0);
                    _with1.Comment = (!string.IsNullOrEmpty(List_Clerksdr["Comment"].ToString()) ? List_Clerksdr["Comment"].ToString() : null);
                    _with1.CreatedOn = (!string.IsNullOrEmpty(List_Clerksdr["CreatedOn"].ToString()) ? List_Clerksdr["CreatedOn"].ToString() : null);
                    _with1.Question = (!string.IsNullOrEmpty(List_Clerksdr["Question"].ToString()) ? List_Clerksdr["Question"].ToString() : null);
                    _with1.Rating_id = (!string.IsNullOrEmpty(List_Clerksdr["Rating_id"].ToString()) ? List_Clerksdr["Rating_id"].ToString() : null);
                    _with1.Postedby = (!string.IsNullOrEmpty(List_Clerksdr["Postedby"].ToString()) ? List_Clerksdr["Postedby"].ToString() : null);
                    LstOfList_Clerks.Add(objList);
                }
                List_Clerksdr.NextResult();
                while (List_Clerksdr.Read())
                {
                    Publicfaq.TotalRecords = Convert.ToInt32(List_Clerksdr["Totalrec"]);
                }
                return LstOfList_Clerks;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ListFAQComments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool ArchiveFAQComment(Publicfaq obj)
        {

            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inpara = { new SqlParameter("@in_Rating_id", obj.Relatedfaq_Id) };
                objcommon.AddInParameters(inpara);
                objcommon.ExecuteNonQuerySP("Help_dbo.ST_ARCHIVE_FAQCOMMENTS");
                return true;

            }
            catch (Exception ex)
            {
                var objex = new clsExceptionLog();
                objex.LogException(ex, ClassName, "ArchiveFAQComment", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override System.Data.DataSet EditQuestion(string objqid)
        {
            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] inparam = { new SqlParameter("@in_Question_Id", objqid) };
                objCommon.AddInParameters(inparam);
                DataSet dset = new DataSet();
                dset = objCommon.GetDataSet("Help_dbo.St_FAQ_Admin_Get_QuestionDetails");
                return dset;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "EditQuestion", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool ADMIN_Insert_SitePage(SitePage _SitePage)
        {
            try
            {
                IDataParameter[] objparm = {
			new SqlParameter("@in_SiteCategory_ID", _SitePage.SiteCategory_ID),
			new SqlParameter("@in_Subject_Title", _SitePage.Subject_Title),
			new SqlParameter("@in_Subject_Text", _SitePage.Subject_Text),
			new SqlParameter("@in_CreatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", _SitePage.Loginhistory_ID)
		};
                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_INS_SitePages");
                return true;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ADMIN_Insert_SitePage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;

        }
        public override List<SitePage> ADMIN_List_SitePages(SitePage objSitePage, ref int Total_records)
        {
            SqlDataReader SitePageReader = default(SqlDataReader);
            try
            {
                IDataParameter[] objparm = {
			new SqlParameter("@In_SitePage_ID", objSitePage.SitePage_ID),
			new SqlParameter("@In_SiteCategory_ID", (objSitePage.SiteCategory_ID!= 0 ? objSitePage.SiteCategory_ID : null)),
			new SqlParameter("@in_NoofRecords", objSitePage.NoOfRecords),
			new SqlParameter("@in_PageNo", objSitePage.pageNo),
			new SqlParameter("@In_Application_ID", objSitePage.Application_ID),
			new SqlParameter("@In_OrderBy", objSitePage.OrderBy),
			new SqlParameter("@In_OrderBYitem", objSitePage.OrderByItem),
			new SqlParameter("@In_SiteCategory", objSitePage.Category)
		};
                objclsdbwraper.AddInParameters(objparm);
                SitePageReader = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_SitePages_RDPaging");
                List<SitePage> _sitepagecoll = new List<SitePage>();
                while (SitePageReader.Read())
                {
                    SitePage _SitePage = new SitePage();

                    if (SitePageReader["SitePage_ID"]!=null)
                    {
                        _SitePage.SitePage_ID = Convert.ToInt32(SitePageReader["SitePage_ID"]);
                    }
                    if (SitePageReader["SiteCategory_ID"]!=null)
                    {
                        _SitePage.SiteCategory_ID = Convert.ToInt32(SitePageReader["SiteCategory_ID"]);
                    }
                    if (SitePageReader["Category"]!=null)
                    {
                        _SitePage.Category = Convert.ToString(SitePageReader["Category"]);
                    }
                    if (SitePageReader["Subject_Title"]!=null)
                    {
                        _SitePage.Subject_Title = Convert.ToString(SitePageReader["Subject_Title"]);
                    }
                    if (SitePageReader["Subject_Text"]!=null)
                    {
                        _SitePage.Subject_Text = Convert.ToString(SitePageReader["Subject_Text"]);
                    }
                    if (SitePageReader["ApplicationName"] != null)
                    {
                        _SitePage.ApplicationName = Convert.ToString(SitePageReader["ApplicationName"]);
                    }
                    _sitepagecoll.Add(_SitePage);
                }
                if (SitePageReader.NextResult())
                {
                    if (SitePageReader.Read())
                    {
                        Total_records = Convert.ToInt32(SitePageReader["Total_records"]);
                    }

                }
                return _sitepagecoll;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ADMIN_List_SitePages", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override SitePage ADMIN_GET_SitePage(int SitePage_id)
        {

            try
            {
                SqlDataReader SitePageReader = default(SqlDataReader);
                IDataParameter[] objparm = { new SqlParameter("@In_SitePage_ID", SitePage_id) };
                objclsdbwraper.AddInParameters(objparm);
                SitePageReader = objclsdbwraper.GetDataReader("Help_dbo.st_ADMIN_GET_SitePages");
                SitePage _SitePage = new SitePage();
                if (SitePageReader.Read())
                {
                    if (SitePageReader["SitePage_ID"]!=null)
                    {
                        _SitePage.SitePage_ID = Convert.ToInt32(SitePageReader["SitePage_ID"]);
                    }
                    if (SitePageReader["SiteCategory_ID"]!=null)
                    {
                        _SitePage.SiteCategory_ID = Convert.ToInt32(SitePageReader["SiteCategory_ID"]);
                    }
                    if (SitePageReader["SiteCategory"]!=null)
                    {
                        _SitePage.Category = Convert.ToString(SitePageReader["SiteCategory"]);
                    }
                    if (SitePageReader["Application_ID"]!=null)
                    {
                        _SitePage.Application_ID = Convert.ToInt32(SitePageReader["Application_ID"]);
                    }

                    if (SitePageReader["Subject_Title"]!=null)
                    {
                        _SitePage.Subject_Title = Convert.ToString(SitePageReader["Subject_Title"]);
                    }
                    if (SitePageReader["Subject_Text"]!=null)
                    {
                        _SitePage.Subject_Text = Convert.ToString(SitePageReader["Subject_Text"]);
                    }
                    if (SitePageReader["ApplicationName"] != null)
                    {
                        _SitePage.ApplicationName = Convert.ToString(SitePageReader["ApplicationName"]);
                    }
                }
                return _SitePage;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ADMIN_GET_SitePage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override bool ADMIN_Update_SitePage(SitePage _SitePage)
        {
            try
            {
                IDataParameter[] objparm = {
			new SqlParameter("@in_SitePage_ID", _SitePage.SitePage_ID),
			new SqlParameter("@in_SiteCategory_ID", _SitePage.SiteCategory_ID),
			new SqlParameter("@in_Subject_Title", _SitePage.Subject_Title),
			new SqlParameter("@in_Subject_Text", _SitePage.Subject_Text),
			new SqlParameter("@in_UpdatedBy", HttpContext.Current.Session["userid"]),
			new SqlParameter("@in_Loginhistory_ID", _SitePage.Loginhistory_ID)
		};
                objclsdbwraper.AddInParameters(objparm);
                objclsdbwraper.ExecuteNonQuerySP("Help_dbo.st_ADMIN_UPD_SitePages");
                return true;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "ADMIN_Update_SitePage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;

        }
        public override System.Collections.Generic.List<Publicfaq> GetQuestions(Publicfaq objFaq)
        {
             try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                SqlDataReader sqlread = default(SqlDataReader);
            IDataParameter[] InParam = {
			new SqlParameter("@In_Status", (!string.IsNullOrEmpty(objFaq.Status) ? objFaq.Status : null)),
			new SqlParameter("@in_Category_id", (!string.IsNullOrEmpty(objFaq.Category_Id) ? objFaq.Category_Id : null)),
			new SqlParameter("@in_Question", (!string.IsNullOrEmpty(objFaq.Question) ? objFaq.Question : null)),
			new SqlParameter("@In_OrdderBY", (!string.IsNullOrEmpty(objFaq.OrderBy) ? objFaq.OrderBy : null)),
			new SqlParameter("@in_OrderByItem", (!string.IsNullOrEmpty(objFaq.OrderByItem) ? objFaq.OrderByItem : null)),
			new SqlParameter("@in_PageNo", (!string.IsNullOrEmpty(objFaq.PageNo) ? objFaq.PageNo : null)),
			new SqlParameter("@in_NoofRecords", (!string.IsNullOrEmpty(objFaq.NoofRecords) ? objFaq.NoofRecords : null)),
			new SqlParameter("@in_PublicInd", (!string.IsNullOrEmpty(objFaq.public_ind) ? objFaq.public_ind : null)),
			new SqlParameter("@in_ProviderInd", (!string.IsNullOrEmpty(objFaq.Provider_Ind) ? objFaq.Provider_Ind : null)),
			new SqlParameter("@in_fromdate", (!string.IsNullOrEmpty(objFaq.strfromdate) ? objFaq.strfromdate : null)),
			new SqlParameter("@in_todate", (!string.IsNullOrEmpty(objFaq.strtodate) ? objFaq.strtodate : null)),
            	new SqlParameter("@in_verificationuser_ind", (!string.IsNullOrEmpty(objFaq.Verfication_User_ind) ? objFaq.Verfication_User_ind : null))
		};
                objCommon.AddInParameters(InParam);
                sqlread = objCommon.GetDataReader("Help_dbo.St_FAQ_List_Questions_Rdpaging");
                List<Publicfaq> objques = new List<Publicfaq>();
                while (sqlread.Read())
                {
                    Publicfaq objq = new Publicfaq();

                    objq.Sino =Convert.ToInt32(sqlread["R_No"]);
                    if (sqlread["Category_id"]!=DBNull.Value)
                    {
                        objq.Category_Id =(string) sqlread["Category_id"];
                    }
                    if (sqlread["Categoryname"] != DBNull.Value)
                    {
                        objq.Category_Name =(string) sqlread["Categoryname"];
                        if (objq.Category_Name != null)
                        {
                            objq.Category_Name = objq.Category_Name.Replace("$", ", ");
                        }
                    }

                    if (sqlread["Question_Id"] != DBNull.Value)
                    {
                        objq.Question_id =Convert.ToString(sqlread["Question_Id"]);
                    }

                    if (sqlread["Question"] != DBNull.Value)
                    {
                        objq.Question =(string) sqlread["Question"];
                    }
                    if (sqlread["CreatedOn"] != DBNull.Value)
                    {
                        objq.CreatedOn = Convert.ToString(sqlread["CreatedOn"]);
                    }

                    if (sqlread["UpdatedOn"]!=DBNull.Value)
                    {
                        objq.UpdatedOn = Convert.ToString(sqlread["UpdatedOn"]);
                    }

                    if (sqlread["FromRole_Id"] != DBNull.Value)
                    {
                        objq.From_Roleid = (int)sqlread["FromRole_Id"];
                    }
                    if (sqlread["Answertext"] != DBNull.Value)
                    {
                        objq.Answer = (string)sqlread["Answertext"];
                    }
                    if (sqlread["Status"] != DBNull.Value)
                    {
                        objq.Status =(string) sqlread["Status"];
                    }
                    if (sqlread["Availableto"] != DBNull.Value)
                    {
                        objq.AvailableTo = (string)sqlread["Availableto"];
                    }
                    if (sqlread["Rating"] != DBNull.Value)
                    {
                        objq.rating = Convert.ToString(sqlread["Rating"]);
                    }
                    if (sqlread["countstring"] != DBNull.Value)
                    {
                        objq.countstring =(string)sqlread["countstring"];
                    }
                    if (sqlread["Tages"] != DBNull.Value)
                    {
                        objq.Keywords =(string) sqlread["Tages"];
                    }
                    if (sqlread["LinkNames"] != DBNull.Value)
                    {
                        objq.relatedlinks =(string) sqlread["LinkNames"];
                    }
                    if (sqlread["RelatedFaqs"] != DBNull.Value)
                    {
                        objq.relatedfaqs1 =(string) sqlread["RelatedFaqs"];
                    }
                    objques.Add(objq);
                }
                sqlread.NextResult();
                if (sqlread.HasRows == true)
                {
                    if (sqlread.Read())
                    {
                        Publicfaq.TotalRecords =(int) sqlread[0];
                    }
                }

                return objques;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetQuestions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override List<Publicfaq> GetQuesCategories()
        {

            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                DataSet dset = new DataSet();
                List<Publicfaq> objdatalist = new List<Publicfaq>();
                dset = objcommon.GetDataSet("Help_dbo.St_FAQ_List_Categories");
                if (dset.Tables.Count > 0)
                {
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dset.Tables[0].Rows)
                        {
                            var obj = new Publicfaq();
                            if (dr["Category_Name"] != null)
                            {
                                obj.Category_Name = dr["Category_Name"].ToString();
                            }
                            else
                            {
                                obj.Category_Name = null;
                            }
                            if (dr["Category_Id"] != null)
                            {
                                obj.Category_Id = dr["Category_Id"].ToString();
                            }
                            else {
                                obj.Category_Id = null;
                            }
                            objdatalist.Add(obj);
                            obj = null;
                        }
                    }
                }
                return objdatalist;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetQuesCategories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override System.Collections.Generic.List<Publicfaq> GetRelatedQuestions(Publicfaq objFaq)
        {

            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                SqlDataReader sqlread = default(SqlDataReader);

                IDataParameter[] InParam = {
			new SqlParameter("@In_Status", (!string.IsNullOrEmpty(objFaq.Status) ? objFaq.Status : null)),
			new SqlParameter("@in_Category_id", (!string.IsNullOrEmpty(objFaq.Category_Id) ? objFaq.Category_Id : null)),
			new SqlParameter("@in_Question", (!string.IsNullOrEmpty(objFaq.Question) ? objFaq.Question : null)),
			new SqlParameter("@In_OrdderBY", (!string.IsNullOrEmpty(objFaq.OrderBy) ? objFaq.OrderBy : null)),
			new SqlParameter("@in_OrderByItem", (!string.IsNullOrEmpty(objFaq.OrderByItem) ? objFaq.OrderByItem : null)),
			new SqlParameter("@in_PageNo", (!string.IsNullOrEmpty(objFaq.PageNo) ? objFaq.PageNo : null)),
			new SqlParameter("@in_NoofRecords", (!string.IsNullOrEmpty(objFaq.NoofRecords) ? objFaq.NoofRecords : null)),
			new SqlParameter("@in_PublicInd", (!string.IsNullOrEmpty(objFaq.public_ind) ? objFaq.public_ind : null)),
			new SqlParameter("@in_ProviderInd", (!string.IsNullOrEmpty(objFaq.Provider_Ind) ? objFaq.Provider_Ind : null)),
			new SqlParameter("@in_exceptids", (!string.IsNullOrEmpty(objFaq.strExcept) ? objFaq.strExcept : null)),
			new SqlParameter("@in_fromdate", (!string.IsNullOrEmpty(objFaq.strfromdate) ? objFaq.strfromdate : null)),
			new SqlParameter("@in_todate", (!string.IsNullOrEmpty(objFaq.strtodate) ? objFaq.strtodate : null)),
            new SqlParameter("@in_question_id", (!string.IsNullOrEmpty(objFaq.Question_id) ? objFaq.Question_id : null)),
			new SqlParameter("@in_verificationuser_ind", (!string.IsNullOrEmpty(objFaq.Verfication_User_ind) ? objFaq.Verfication_User_ind : null))
		};
                objCommon.AddInParameters(InParam);
                sqlread = objCommon.GetDataReader("Help_dbo.St_FAQ_List_related_Questions_Rdpaging");
                List<Publicfaq> objques = new List<Publicfaq>();
                while (sqlread.Read())
                {
                    Publicfaq objq = new Publicfaq();

                    objq.Sino = Convert.ToInt32(sqlread["R_No"]);
                    if (sqlread["Category_id"]!=DBNull.Value)
                    {
                        objq.Category_Id = sqlread["Category_id"].ToString();
                    }
                    if (sqlread["Categoryname"]!=DBNull.Value)
                    {
                        objq.Category_Name = sqlread["Categoryname"].ToString();
                        if (objq.Category_Name != null)
                        {
                            objq.Category_Name = objq.Category_Name.Replace("$", ", ");
                        }
                    }

                    if (sqlread["Question_Id"]!=DBNull.Value)
                    {
                        objq.Question_id = sqlread["Question_Id"].ToString();
                    }

                    if (sqlread["Question"]!=DBNull.Value)
                    {
                        objq.Question = sqlread["Question"].ToString();
                    }
                   
                    objques.Add(objq);
                }
                sqlread.NextResult();
                if (sqlread.HasRows == true)
                {
                    if (sqlread.Read())
                    {
                        Publicfaq.TotalRecords = Convert.ToInt32(sqlread[0]);
                    }
                }

                return objques;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetRelatedQuestions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override int insertAdminQuestions(Publicfaq objitems)
        {

            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                objclsdbwraper = new clsDBWrapper();
                int questionID = 0;
                IDataParameter[] inpara = {
			new SqlParameter("@In_FromRole_Id", (objitems.From_Roleid == 0 ? null : objitems.From_Roleid)),
			new SqlParameter("@In_Category_id", (string.IsNullOrEmpty(objitems.Category_Id) ? null : objitems.Category_Id)),
			new SqlParameter("@In_Question", (string.IsNullOrEmpty(objitems.Question) ? null : objitems.Question)),
			new SqlParameter("@In_Answertext", (string.IsNullOrEmpty(objitems.Answer) ? null : objitems.Answer)),
			new SqlParameter("@In_Tages", (string.IsNullOrEmpty(objitems.Keyword) ? null : objitems.Keyword)),
			new SqlParameter("@In_Status", (string.IsNullOrEmpty(objitems.Status) ? null : objitems.Status)),
			new SqlParameter("@In_CreatedBy", (string.IsNullOrEmpty(objitems.CreatedBy) ? null : objitems.CreatedBy)),
			new SqlParameter("@In_public_ind", (string.IsNullOrEmpty(objitems.public_ind) ? null : objitems.public_ind)),
			new SqlParameter("@In_provider_Ind", (string.IsNullOrEmpty(objitems.Provider_Ind) ? null : objitems.Provider_Ind)),
			new SqlParameter("@in_verificationuser_ind", (!string.IsNullOrEmpty(objitems.Verfication_User_ind) ? objitems.Verfication_User_ind : null)),
			new SqlParameter("@in_Loginhistory_ID", (!string.IsNullOrEmpty(objitems.Loginhistory_ID) ? objitems.Loginhistory_ID : null))
		};
                IDataParameter[] outparam = { new SqlParameter("@Out_Question_Id", SqlDbType.BigInt) };
                objcommon.AddInParameters(inpara);
                objcommon.AddOutParameters(outparam);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_INS_Admin_Questions");
                if (objcommon.objdbCommandWrapper.Parameters["@Out_Question_Id"].Value!=DBNull.Value)
                {
                    questionID = Convert.ToInt32(objcommon.objdbCommandWrapper.Parameters["@Out_Question_Id"].Value);
                }
                return questionID;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "insertAdminQuestions", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return 0;
        }
        public override bool AddRelatedLink(Publicfaq objlink)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Question_id", (objlink.Question_id == "0" ? null : objlink.Question_id)),
			new SqlParameter("@In_LinkUrl", (string.IsNullOrEmpty(objlink.LinkUrl) ? null : objlink.LinkUrl)),
			new SqlParameter("@In_LinkName", (string.IsNullOrEmpty(objlink.LinkName) ? null : objlink.LinkName)),
			new SqlParameter("@in_Description", (string.IsNullOrEmpty(objlink.LinkUrlDescription) ? null : objlink.LinkUrlDescription)),
			new SqlParameter("@In_CreatedBy", (string.IsNullOrEmpty(objlink.CreatedBy) ? null : objlink.CreatedBy)),
			new SqlParameter("@in_Status_ind", (string.IsNullOrEmpty(objlink.Status_Ind) ? null : objlink.Status_Ind)),
			new SqlParameter("@in_DelRelLink_Ids", (string.IsNullOrEmpty(objlink.strDelLinkIds) ? null : objlink.strDelLinkIds)),
			new SqlParameter("@in_Video_id", (objlink.strVideoid == 0 ? null : objlink.strVideoid))
		};
                objcommon.AddInParameters(inpara);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_DML_RelatedLinks");
                return true;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "AddRelatedLink", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool DML_Relatedfaqs(Publicfaq objFaq)
        {
            try
            {
                IDataParameter[] InParam = {
			new SqlParameter("@in_ParentFaq_ID", (objFaq.Question_id != "0" ? objFaq.Question_id : null)),
			new SqlParameter("@in_ChildFaq_id", (objFaq.Relatedfaq_Id != 0 ? objFaq.Relatedfaq_Id : null)),
			new SqlParameter("@In_CreatedBy", (objFaq.CreatedBy != "0" ? objFaq.CreatedBy : null)),
			new SqlParameter("@in_DelFaq_Id", (!string.IsNullOrEmpty(objFaq.strDelfaqIds) ? objFaq.strDelfaqIds : null)),
			new SqlParameter("@in_Status_ind", (!string.IsNullOrEmpty(objFaq.Status_Ind) ? objFaq.Status_Ind : null))
		};

                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_DML_RelatedFaqs");
                return true;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "DML_Relatedfaqs", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool DeleteQuestion(string Qid, int Loginhistory_ID)
        {

            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] inpara = {
			new SqlParameter("@in_Question_ID", Qid),
			new SqlParameter("@in_Loginhistory_ID", Loginhistory_ID)
		};

                objcommon.AddInParameters(inpara);
                objcommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_DEL_Questions");
                return true;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "DeleteQuestion", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool UpdateQuestion(Publicfaq objqid)
        {

            try
            {
                clsCommonFunctions objCommon = new clsCommonFunctions();
                IDataParameter[] inparam = {
			new SqlParameter("@In_Question_Id", (objqid.Question_id == "0" ? null : objqid.Question_id)),
			new SqlParameter("@In_Category_id", (string.IsNullOrEmpty(objqid.Category_Id) ? null : objqid.Category_Id)),
			new SqlParameter("@In_Question", (string.IsNullOrEmpty(objqid.Question) ? null : objqid.Question)),
			new SqlParameter("@In_Answertext", (string.IsNullOrEmpty(objqid.Answer) ? null : objqid.Answer)),
			new SqlParameter("@In_Tages", (string.IsNullOrEmpty(objqid.Keyword) ? null : objqid.Keyword)),
			new SqlParameter("@In_Status", (string.IsNullOrEmpty(objqid.Status) ? null : objqid.Status)),
			new SqlParameter("@In_public_ind", (string.IsNullOrEmpty(objqid.public_ind) ? null : objqid.public_ind)),
			new SqlParameter("@In_provider_Ind", (string.IsNullOrEmpty(objqid.Provider_Ind) ? null : objqid.Provider_Ind)),
			new SqlParameter("@In_UpdatedBy", (objqid.UpdatedBy == 0 ? null : objqid.UpdatedBy)),
			new SqlParameter("@in_verificationuser_ind", (!string.IsNullOrEmpty(objqid.Verfication_User_ind) ? objqid.Verfication_User_ind : null)),
			new SqlParameter("@in_Loginhistory_ID", (!string.IsNullOrEmpty(objqid.Loginhistory_ID) ? objqid.Loginhistory_ID : null))
		};
                objCommon.AddInParameters(inparam);
                objCommon.ExecuteNonQuerySP("Help_dbo.St_FAQ_INS_Questions_Answer");
                return true;

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "UpdateQuestion", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override bool Resetratings(Publicfaq objFaq)
        {
            try
            {
                IDataParameter[] InParam = { new SqlParameter("@in_Question_ID", (objFaq.Question_id != "0" ? objFaq.Question_id : null)) };

                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParam);
                objcommon.ExecuteNonQuerySP("Help_dbo.sp_Faq_Question_resetrating");
                return true;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Resetratings", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return false;
        }
        public override List<Publicfaq> Get_FaqsComments(Publicfaq objFaq)
        {
            try
            {
                IDataParameter[] InParam = { new SqlParameter("@in_Question_ID", (objFaq.Question_id != "0" ? objFaq.Question_id : null)) };

                clsCommonFunctions objcommon = new clsCommonFunctions();
                objcommon.AddInParameters(InParam);
                DataSet dslink = new DataSet();
                List<Publicfaq> objdatalist = new List<Publicfaq>();
                dslink = objcommon.GetDataSet("Help_dbo.sp_Faq_Question_Getcomments");
                if (dslink.Tables.Count > 0)
                {
                    if (dslink.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dslink.Tables[0].Rows)
                        {
                            var obj = new Publicfaq();
                            if (dr["Comment"] != null)
                            {
                                obj.Comment = dr["Comment"].ToString();
                            }
                            else
                            {
                                obj.Comment = null;
                            }
                            if (dr["PostedBy"] != null)
                            {
                                obj.Postedby = dr["PostedBy"].ToString();
                            }
                            else
                            {
                                obj.Postedby = null;
                            }
                            if (dr["Rate_value"] != null)
                            {
                                obj.Rate_value = dr["Rate_value"].ToString();
                            }
                            else
                            {
                                obj.Rate_value = null;
                            }
                            if (dr["CreatedOn"] != null)
                            {
                                obj.CreatedOn = dr["CreatedOn"].ToString();
                            }
                            else
                            {
                                obj.CreatedOn = null;
                            }
                            objdatalist.Add(obj);
                            obj = null;
                        }
                    }
                }
                return objdatalist;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "Get_FaqsComments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public override string GetObjectname(string ObjectID)
        {
            try
            {
                IDataParameter[] objparam = { new SqlParameter("@In_ObjectID", ObjectID) };
                IDataParameter objRet = new SqlParameter("@Loc_Object_name", SqlDbType.VarChar, 1000);
                clsCommonFunctions obj = new clsCommonFunctions();
                obj.AddInParameters(objparam);
                obj.AddReturnParameters(objRet);
                obj.ExecuteStringFunction("Help_dbo.FXn_Admin_Get_ObjectName");
                if (obj.objdbCommandWrapper.Parameters["@Loc_Object_name"].Value != null)
                {
                    return Convert.ToString(obj.objdbCommandWrapper.Parameters["@Loc_Object_name"].Value);
                }
                return null;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, ClassName, "GetObjectname", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }




    }
   
}
