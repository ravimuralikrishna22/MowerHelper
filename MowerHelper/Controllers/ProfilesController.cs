using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class ProfilesController : Controller
    {
        clsDBWrapper objDBWrapper = new clsDBWrapper();
        DataSet dsSections = new DataSet();
        [ActionName("Users")]
        public ActionResult Index()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
           // Session.Add("TopIndex", "4");
            char? AcPrv;
            if (Session["strOutIsPaid"] != null)
            {
                AcPrv = Convert.ToChar(Session["strOutIsPaid"]);
            }
            else
            {
                AcPrv = 'Y';
            }
            dsSections = ChildSections(Convert.ToString(Session["userid"]), "121", Convert.ToString(Session["Roleid"]), AcPrv);
            StringBuilder strAdmin = new StringBuilder();
            strAdmin = strAdmin.Append("<table id='Toolstable' border='0' cellpadding='20' cellspacing='0' width='100%' >");
            if (dsSections.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                strAdmin = strAdmin.Append("<tr class='background_color'>");
                string RedirectPath = string.Empty;
                foreach (DataRow drs in dsSections.Tables[0].Rows)
                {
                    if (Convert.ToString(drs["SectionsInRole_ID"]) != "13710" & Convert.ToString(drs["SectionsInRole_ID"]) != "13328" & Convert.ToString(drs["SectionsInRole_ID"]) != "2826")
                    {
                        if (Convert.ToString(drs["SectionPath"]) == "Admin/ApproveProviders.aspx")
                        {
                            RedirectPath = "../Profiles/ApproveProviders";
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Providers/ProviderProfile.aspx")
                        {
                            RedirectPath = "../VerificationUser/ProviderProfile";
                        }
                        if (Convert.ToString(drs["SectionPath"]) == "Payers/Payer_List.aspx")
                        {
                            RedirectPath = "../Profiles/Payer_List";
                        }
                        if (i % 4 == 0)
                        {
                            strAdmin = strAdmin.Append("</tr><tr><td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'> <a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a> <br /> <span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span></td>  ");
                        }
                        else
                        {
                            strAdmin = strAdmin.Append("<td align='center' valign='top' style='font-family:arial;font-size:9pt;width:21%;'><a href='" + RedirectPath + "' style='font-family: Arial; font-size: 9pt;'><span style='font-family: Arial; font-size: 9pt; color:blue;'> " + drs["SectionName"] + " </span></a><br /><span style='font-family:arial;font-size:9pt;'> " + drs["SectionDescription"] + " </span> </td> ");
                        }
                        i++;
                    }
                }
                strAdmin = strAdmin.Append("</tr>");
            }
            strAdmin = strAdmin.Append("</table>");
            ViewBag.Profiles = Convert.ToString(strAdmin);
            return View("Index");
        }
        public DataSet ChildSections(string login_id, string section_id, string Role_id, char? IsBasicAccProv)
        {

            try
            {
                string IS_Billing_Ind = null;
                string IS_receptionist_ind = null;
                DataSet dsSections = new DataSet();
                int? strProviderID = 0;
                int? strPracticeID = 0;
                if (Convert.ToString(Session["RoleID"]) == "15" | (int)Session["RoleID"] == 4)
                {
                    strProviderID =Convert.ToInt32(Session["Prov_ID"] == null ? Session["Provider_ID"] : Session["Prov_ID"]);
                    strPracticeID = (int)Session["Prov_ID"];
                }
                IDataParameter[] objInparameters = {
			new SqlParameter("@in_Login_ID", Convert.ToInt32(login_id)),
			new SqlParameter("@in_Section_ID", Convert.ToInt32(section_id)),
			new SqlParameter("@in_RoleId", Convert.ToInt32(Role_id)),
			new SqlParameter("@in_Provider_ID", (strProviderID == 0 ? null : strProviderID)),
			new SqlParameter("@in_Practice_ID", (strPracticeID == 0 ? null : strPracticeID)),
			new SqlParameter("@In_IsBasicACPrv", (IsBasicAccProv == null ? 'Y' : IsBasicAccProv)),
			new SqlParameter("@In_IsClerk_ind", IS_Billing_Ind),
			new SqlParameter("@In_IsRecept_Ind", IS_receptionist_ind)
		};


                objDBWrapper.AddInParameters(objInparameters);
                if (section_id == "2291")
                {
                    dsSections = objDBWrapper.GetDataSet("Help_dbo.st_Security_Get_HomeChildSections");
                }
                else
                {
                    dsSections = objDBWrapper.GetDataSet("Help_dbo.St_Security_Get_ChildSections");
                }
                return dsSections;
               
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ProfilesController", "ChildSections", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        [HttpGet()]
        public ActionResult ApproveProviders()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
         List<Admin_postajob> objuserslist = new List<Admin_postajob>();
            Admin_postajob objusers = new Admin_postajob();
            if (Request["sortdir"] != null & Request["sortdir"] != "")
            {
                objusers.OrderBy = Convert.ToString(Request["sortdir"]);
            }
            else
            {
                objusers.OrderBy = "ASC";
            }
            if (Request["sort"] != null & Request["sort"] != "")
            {
                objusers.OrderByItem = Convert.ToString(Request["sort"]);
            }
            else
            {
                objusers.OrderByItem = "Full_Name";
            }
            if (Request["page"] != null & Request["page"] != "")
            {
                objusers.PageNo = Convert.ToInt32(Request["page"]);
            }
            else
            {
                objusers.PageNo = 1;
            }
            int NoofRecords;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Convert.ToInt32(Request["ddlrecords"]);
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Convert.ToInt32(Session["Rowsperpage"]);
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = 10;
            }
            objusers.NoOfRecords = NoofRecords;
            int totalrec = 0;
                objusers.LastName = null;
               objuserslist = Admin_postajob.verificationusersList(objusers, ref totalrec);
               if (objuserslist.Count > 0)
               {
                   ViewBag.totalrec = totalrec;
               }
               else
               {
                   ViewBag.totalrec = 0;
               }
               return View("ApproveProviders", objuserslist);
        }

        public ActionResult AddnewVerificationUser()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //var reg9 = clsCommonCClist.GetStates();

            //List<clsCountry> objstates1 = new List<clsCountry>();
            //objstates1 = clsCountry.GetStatesByCountryId(1);
            //IList<SelectListItem> _result3 = new List<SelectListItem>();
            //if (objstates1.Count > 0)
            //{
            //    for (int i = 0; i <= objstates1.Count - 1; i++)
            //    {
            //        _result3.Add(new SelectListItem
            //        {
            //            Text = objstates1[i].StateFullName,
            //            Value = Convert.ToString(objstates1[i].StateId)
            //        });
            //    }
            //}
            //IList<SelectListItem> _result4 = new List<SelectListItem>();
            //_result4.Add(new SelectListItem
            //{
            //    Text = "--Select City--",
            //    Value = "0",
            //    Selected = true
            //});

            //StateCity reg9 = new StateCity();
            //reg9 = new StateCity
            //{
            //    StateList = _result3,
            //    CityList = _result4
            //};
            return View("AddnewVerificationUser");
        }
        VBVMD5CryptoServiceProvider objmd5 = new VBVMD5CryptoServiceProvider();
        [HttpPost()]
        public JsonResult AddnewVerificationUser( string obj)
        {

            try
            {
                int Out_Login_ID = 0;
                int Out_verification_User_ID = 0;
                string strFirstName = "";
                string strFormatedFirstName = "";
                string strLastName = "";
                string strFormatedLastName = "";
                if (!string.IsNullOrEmpty(Request["txtFirstName"]))
                {
                    strFirstName = Request["txtFirstName"].ToString();
                    if (strFirstName.Contains(" "))
                    {
                        strFormatedFirstName = Regex.Replace(strFirstName, "\\s+", " ");
                    }
                    else
                    {
                        strFormatedFirstName = Request["txtFirstName"].ToString();
                    }
                }
                else
                {
                    strFormatedFirstName = null;
                }
                if (!string.IsNullOrEmpty(Request["txtLastName"]))
                {
                    strLastName = Request["txtLastName"].ToString();
                    if (strLastName.Contains(" "))
                    {
                        strFormatedLastName = Regex.Replace(strLastName, "\\s+", " ");
                    }
                    else
                    {
                        strFormatedLastName = Request["txtLastName"].ToString();
                    }
                }
                else
                {
                    strFormatedLastName = null;
                }
                string _strTitle = null;

                _strTitle = Request["DDUserPrefix"].Trim() == "" ? null : Request["DDUserPrefix"];
               
                string _strTemp1 = "";
                string _strTemp = "";
                string _strWorkPhone = "";
                string _strWorkPhoneExtn = "";
                string _strCellPhone = "";
                string _strPager = "";
                string _Fax = "";
                string _strStateID = "";
                string _strCityID = "";
                string _chrHPLeaveMsg_Ind = "";
                string _chrWPLeaveMsg_Ind = "";
                string ChkFLeaveMsg_Ind = null;
                 if (Request["ChkWPLeaveMsg_Ind"] != null)
                {
                    string[] workphnlmsg = null;
                    if (Request["ChkWPLeaveMsg_Ind"].Contains(","))
                    {
                        workphnlmsg = Request["ChkWPLeaveMsg_Ind"].Split(',');
                        if (workphnlmsg.Length == 2)
                        {
                            if (workphnlmsg[0] == "true")
                            {
                                ChkFLeaveMsg_Ind = "Y";
                            }
                            else if (workphnlmsg[0] == "false")
                            {
                                ChkFLeaveMsg_Ind = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["ChkWPLeaveMsg_Ind"] == "false")
                        {
                            ChkFLeaveMsg_Ind = "N";
                        }
                        else if (Request["ChkWPLeaveMsg_Ind"] == "true")
                        {
                            ChkFLeaveMsg_Ind = "Y";
                        }
                    }
                }
                string _chrFLeaveMsg_Ind = "";
                string strrole = null;
                if (Request["hdnrole"] == "0")
                {
                    strrole = "31";
                }
                else
                {
                    strrole = "38";
                }
                _strTemp = (Request["txtWorkPhone1"] + Request["txtWorkPhone2"] + Request["txtWorkPhone3"]);
                if (!string.IsNullOrEmpty(_strTemp))
                {
                    _strWorkPhone = _strTemp;
                    _chrWPLeaveMsg_Ind = ChkFLeaveMsg_Ind; 
                }
                else
                {
                    _chrWPLeaveMsg_Ind = null;
                    _strWorkPhone = null;
                }
                if (!string.IsNullOrEmpty(Request["txtWorkPhoneExt"]))
                {
                    _strWorkPhoneExtn = Request["txtWorkPhoneExt"].ToString();
                }
                else
                {
                    _strWorkPhoneExtn = null;
                }
               
                    _strPager = null;

                    _strTemp1 = (Convert.ToString(Request["txtCellPhone1"]) + Convert.ToString(Request["txtCellPhone2"]) + Convert.ToString(Request["txtCellPhone3"]));
                if (!string.IsNullOrEmpty(_strTemp1))
                {
                    _strCellPhone = _strTemp1;
                }
                else
                {
                    _strCellPhone = null;
                }
                _strTemp = Request["txtFax1"] + Request["txtFax2"] + Request["txtFax3"];
                if (!string.IsNullOrEmpty(_strTemp))
                {
                    _Fax = _strTemp;
                    _chrFLeaveMsg_Ind = ChkFLeaveMsg_Ind;
                }
                else
                {
                    _Fax = null;
                    _chrFLeaveMsg_Ind = null;
                }

                
                    _strStateID =Request["DDState"].ToString() != null ? Request["DDState"].ToString() : null;
               

            
                    _strCityID = Request["DDCity"].ToString() != null ? Request["DDCity"].ToString() : null;
              
                string strhashcode = null;
                if (!string.IsNullOrEmpty(Request["txtPassword"]))
                {
                    strhashcode = objmd5.getMd5Hash(Request["txtPassword"]);
                }
                else
                {
                    strhashcode = null;
                }
                string out_msg = null;
                Admin_postajob ObjInsert = new Admin_postajob();
                ObjInsert.UserName = Convert.ToString(Request["txtEmail"]);
                ObjInsert.Password = strhashcode;
                ObjInsert.RoleType = strrole;
                ObjInsert.CreatedBy = Convert.ToInt32(Session["userid"]);
                ObjInsert.Title = _strTitle;
                ObjInsert.FirstName = strFormatedFirstName;
                ObjInsert.MI = (!string.IsNullOrEmpty(Request["txtMiddle"]) ? Request["txtMiddle"].ToString() : null);
                ObjInsert.LastName = strFormatedLastName;
                ObjInsert.Gender = (Request["hdngender"] == "" ? null : Request["hdngender"]);
                ObjInsert.Address1 = (!string.IsNullOrEmpty(Request["txtAdr1"]) ? Request["txtAdr1"].ToString() : null);
                ObjInsert.Address2 = (!string.IsNullOrEmpty(Request["txtAdr2"]) ? Request["txtAdr2"].ToString() : null);
                ObjInsert.Email = (!string.IsNullOrEmpty(Request["txtEmail"]) ? Request["txtEmail"].ToString() : null);
                ObjInsert.City_ID = Convert.ToInt32(_strCityID);
                ObjInsert.State_ID = Convert.ToInt32(_strStateID);
                ObjInsert.Country_ID = 1;
                ObjInsert.zip = (!string.IsNullOrEmpty(Request["txtZip"]) ? Request["txtZip"].ToString() : null);
                ObjInsert.WorkPhone = _strWorkPhone;
                ObjInsert.WPExtension = _strWorkPhoneExtn;
                ObjInsert.WPLeaveMsg_Ind = _chrWPLeaveMsg_Ind;
                ObjInsert.WPIsUSFormat_Ind = "Y";
                ObjInsert.HomePhone = null;
                ObjInsert.HPExtension = null;
                ObjInsert.HPLeaveMsg_Ind = _chrHPLeaveMsg_Ind;
                ObjInsert.HPIsUSFormat_Ind = "Y";
                ObjInsert.CellPhone = _strCellPhone;
                ObjInsert.MobilePhone = _strCellPhone;
                ObjInsert.MPLeaveMsg_Ind = null;
                ObjInsert.Pager = _strPager;
                ObjInsert.PLeaveMsg_Ind = null;
                ObjInsert.Fax = _Fax;
                ObjInsert.FLeaveMsg_Ind = _chrFLeaveMsg_Ind;
                ObjInsert.FIsUSFormat_Ind = "Y";
                ObjInsert.PrimaryAddress_Ind = "Y";
                ObjInsert.CreatedBy = 1;
                 string outmesg = null;
                 string insmesg = "";
            clsCommonFunctions objCommon = new clsCommonFunctions();
            outmesg = objCommon.EmailExistsFunction((!string.IsNullOrEmpty(Request["txtEmail"].ToString()) ? Request["txtEmail"].ToString() : null));
            if (outmesg != null & outmesg != "")
            {
                insmesg = outmesg;
                return Json(JsonResponseFactory.ErrorResponse("Enter email is already exists"), JsonRequestBehavior.DenyGet);
            }
                else
                {
                    int? loginhistory_id = Convert.ToInt32(Session["Loginhistory_id"]);
                    insmesg = (string)Admin_postajob.Insert_verificationusers(ObjInsert, ref out_msg, ref Out_verification_User_ID, ref Out_Login_ID, loginhistory_id);

                }
                if (out_msg != null)
                {
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                 
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ProfilesController", "AddnewVerificationUser", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
           
        }

        public ActionResult Verifyuserview(string verification_User_Id, string Login_ID)
        {
           
                var objinfo = Admin_postajob.verificationusersView(Convert.ToInt32(Login_ID), Convert.ToInt32(verification_User_Id));
                ViewBag.objinfo = objinfo;
                foreach (var item in objinfo)
                {
                    string StrContCellPhone = null;
                    StrContCellPhone = !string.IsNullOrEmpty(item.CellPhone) ? item.CellPhone : null;
                    if (!string.IsNullOrEmpty(StrContCellPhone))
                    {
                        if (StrContCellPhone.Length >= 10)
                        {
                            ViewBag.cellphn = clsCommonFunctions.UsPhoneFormat(StrContCellPhone);

                        }
                    }

                    ViewBag.Fax = !string.IsNullOrEmpty(item.Fax) ? item.Fax : null;
                    string StrWorkPhone = item.WorkPhone;
                    if (!string.IsNullOrEmpty(StrWorkPhone))
                    {
                        ViewBag.WPhone = clsCommonFunctions.UsPhoneFormat(StrWorkPhone);
                        ViewBag.wphnext = item.WorkPhoneExtn;
                        ViewBag.oktoleave = item.WPLeaveMsg_Ind;
                    }
                }
                return View();
           
        }
        public ActionResult VerificationusersEdit(string verification_User_Id, string Login_ID)
        {
           
                ViewBag.veriuserid = verification_User_Id;
                ViewBag.loginid = Login_ID;
                //var reg1 = clsCommonCClist.GetStates();

                //List<clsCountry> objstates = new List<clsCountry>();
                //objstates = clsCountry.GetStatesByCountryId(1);
                //IList<SelectListItem> _result1 = new List<SelectListItem>();
                //if (objstates.Count > 0)
                //{
                //    for (int i = 0; i <= objstates.Count - 1; i++)
                //    {
                //        _result1.Add(new SelectListItem
                //        {
                //            Text = objstates[i].StateFullName,
                //            Value = Convert.ToString(objstates[i].StateId)
                //        });
                //    }
                //}
                //IList<SelectListItem> _result2 = new List<SelectListItem>();
                //_result2.Add(new SelectListItem
                //{
                //    Text = "--Select City--",
                //    Value = "0",
                //    Selected = true
                //});
                //StateCity reg1 = new StateCity();
                //reg1 = new StateCity
                //{
                //    StateList = _result1,
                //    CityList = _result2
                //};
                List<Admin_postajob> objinfo = new List<Admin_postajob>();

                objinfo = Admin_postajob.verificationusersView(Convert.ToInt32(Login_ID), Convert.ToInt32(verification_User_Id));
                //ViewBag.objinfo = objinfo;
                //foreach (var item in objinfo)
                //{
                    //ViewBag.firstname = item.FirstName;
                    //ViewBag.lastname = item.LastName;
                    //ViewBag.gender = item.Gender;
                    //ViewBag.mi = item.MI;
                    //ViewBag.prefix = item.Prefix;
                    //ViewBag.role = item.Rolename;
                    //string StrContCellPhone = "";
                    //string StrContWorkPhone = "";
                    //StrContCellPhone = !string.IsNullOrEmpty(item.CellPhone) ? item.CellPhone : null;
                    //StrContWorkPhone = !string.IsNullOrEmpty(item.WorkPhone) ? item.WorkPhone : null;
                    //if (!string.IsNullOrEmpty(StrContCellPhone))
                    //{
                    //    if (StrContCellPhone.Length >= 10)
                    //    {
                    //        ViewBag.txtcellphn1 = StrContCellPhone.Substring(0, 3);

                    //        ViewBag.txtcellphn2 = StrContCellPhone.Substring(3, 3);

                    //        ViewBag.txtcellphn3 = StrContCellPhone.Substring(6, 4);

                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(StrContWorkPhone))
                    //{
                        //if (StrContWorkPhone.Length >= 10)
                        //{
                        //    ViewBag.txtWPhone1 = StrContWorkPhone.Substring(0, 3);
                        //    ViewBag.txtWPhone2 = StrContWorkPhone.Substring(3, 3);
                        //    ViewBag.txtPhone3 = StrContWorkPhone.Substring(6, 4);
                        //}
                        //if (item.WorkPhoneExtn != null & item.WorkPhoneExtn != "")
                        //{
                        //    ViewBag.workextension = item.WorkPhoneExtn;
                        //}
                        //ViewBag.okmsg = item.WPLeaveMsg_Ind;
                    //}
                    //string StrContFax = null;
                    //StrContFax = item.Fax;
                    //if (!string.IsNullOrEmpty(StrContFax))
                    //{
                    //    if (StrContFax.Length >= 10)
                    //    {
                    //        ViewBag.txtFax1 = StrContFax.Substring(0, 3);
                    //        ViewBag.txtFax2 = StrContFax.Substring(3, 3);
                    //        ViewBag.txtFax3 = StrContFax.Substring(6, 4);
                    //    }
                    //}
                    //ViewBag.Email = !string.IsNullOrEmpty(item.Email) ? item.Email : null;
                    //ViewBag.Address1 = !string.IsNullOrEmpty(item.Address1) ? item.Address1 : null;
                    //ViewBag.Address2 = !string.IsNullOrEmpty(item.Address2) ? item.Address2 : null;
                    //ViewBag.Zip = !string.IsNullOrEmpty(item.zip) ? item.zip : null;
                    //if (!string.IsNullOrEmpty(item.statename))
                    //{
                    //    ViewBag.Statename = item.statename;
                    //    ViewBag.State_ID = item.State_ID;

                    //}
                    //if (!string.IsNullOrEmpty(item.city))
                    //{
                    //    ViewBag.Cityname = item.city;
                    //    ViewBag.City_ID = item.City_ID;
                    //}
                    //return View("VerificationusersEdit", objinfo);
                //}
                return View("VerificationusersEdit", objinfo);
           
        }
        [HttpPost()]
        public JsonResult VerificationusersEdit(string obj)
        {
            
            Admin_postajob objusers = new Admin_postajob();
            if (!string.IsNullOrEmpty(Request["hdnloginid"]))
            {
                objusers.Login_ID = Convert.ToInt32(Request["hdnloginid"]);
            }
            else
            {
                objusers.Login_ID = null;
            }
            if (Convert.ToInt32(Request["hdnveriuserid"]) != 0)
            {
                objusers.verification_User_Id = Convert.ToInt32(Request["hdnveriuserid"]);
            }
            else
            {
                objusers.verification_User_Id = null;
            }
            if (Request["ddl_prefix"] != null & Request["ddl_prefix"]!="")
            {
                objusers.Prefix = Request["ddl_prefix"].ToString();
            }
            else
            {
                objusers.Prefix = null;
            }
            if (!string.IsNullOrEmpty(Request["txt_FirstName"]))
            {
                objusers.FirstName = Request["txt_FirstName"].ToString();
            }
            else
            {
                objusers.FirstName = null;
            }

            if (!string.IsNullOrEmpty(Request["txt_MI"]))
            {
                objusers.MI = Request["txt_MI"].ToString();
            }
            else
            {
                objusers.MI = null;
            }
            if (!string.IsNullOrEmpty(Request["txt_LastName"]))
            {
                objusers.LastName = Request["txt_LastName"].ToString();
            }
            else
            {
                objusers.LastName = null;
            }
          
                objusers.company_name = null;

                if (!string.IsNullOrEmpty(Request["txtAddress1"]))
            {
                objusers.Address1 = Request["txtAddress1"].ToString();
            }
            else
            {
                objusers.Address1 = null;
            }
                if (!string.IsNullOrEmpty(Request["txtAddress2"]))
            {
                objusers.Address2 = Request["txtAddress2"].ToString();
            }
            else
            {
                objusers.Address2 = null;
            }

                if (!string.IsNullOrEmpty(Request["txtWorkPhone1"]) & !string.IsNullOrEmpty(Request["txtWorkPhone2"]) & !string.IsNullOrEmpty(Request["txtWorkPhone3"]))
            {
                objusers.WorkPhone = Request["txtWorkPhone1"].ToString() + Request["txtWorkPhone2"].ToString() + Request["txtWorkPhone3"].ToString();
            }
            else
            {
                objusers.WorkPhone = null;
            }
                if (!string.IsNullOrEmpty(Request["txtWorkPhoneExtn"]))
            {
                objusers.WorkPhoneExtn = Request["txtWorkPhoneExtn"].ToString();
            }
            else
            {
                objusers.WorkPhoneExtn = null;
            }

                string ChkFLeaveMsg_Ind = null;
                if (Request["chkMsg"] != null)
                {
                    string[] workphnlmsg = null;
                    if (Request["chkMsg"].Contains(","))
                    {
                        workphnlmsg = Request["chkMsg"].Split(',');
                        if (workphnlmsg.Length == 2)
                        {
                            if (workphnlmsg[0] == "true")
                            {
                                ChkFLeaveMsg_Ind = "Y";
                            }
                            else if (workphnlmsg[0] == "false")
                            {
                                ChkFLeaveMsg_Ind = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkMsg"] == "false")
                        {
                            ChkFLeaveMsg_Ind = "N";
                        }
                        else if (Request["chkMsg"] == "true")
                        {
                            ChkFLeaveMsg_Ind = "Y";
                        }
                    }
                }

                objusers.WPLeaveMsg_Ind = Convert.ToString(ChkFLeaveMsg_Ind);
           
            if (!string.IsNullOrEmpty(Request["txtMobile1"]) & !string.IsNullOrEmpty(Request["txtMobile2"]) & !string.IsNullOrEmpty(Request["txtMobile3"]))
            {
                objusers.CellPhone = Request["txtMobile1"].ToString() + Request["txtMobile2"].ToString() + Request["txtMobile3"].ToString();
            }
            else
            {
                objusers.CellPhone = null;
            }
            if (!string.IsNullOrEmpty(Request["txtFax1"]) & !string.IsNullOrEmpty(Request["txtFax2"]) & !string.IsNullOrEmpty(Request["txtFax3"]))
            {
                objusers.Fax = Request["txtFax1"].ToString() + Request["txtFax2"].ToString() + Request["txtFax3"].ToString();
            }
            else
            {
                objusers.Fax = null;
            }
            if (!string.IsNullOrEmpty(Request["txtEmail"]))
            {
                objusers.Email = Request["txtEmail"].ToString();
            }
            else
            {
                objusers.Email = null;
            }
            
                objusers.Country_ID = 1;

                if (Request["DDCity"] != null)
            {
                objusers.City_ID = Convert.ToInt32(Request["DDCity"]);
            }
            else
            {
                objusers.City_ID = null;
            }
                if (Request["DDState"] != null)
            {
                objusers.State_ID = Convert.ToInt32(Request["DDState"]);
            }
            else
            {
                objusers.State_ID = null;
            }
                if (!string.IsNullOrEmpty(Request["txtZip"]))
            {
                objusers.zip = Request["txtZip"];
            }
            else
            {
                objusers.zip = null;
            }

            string out_msg = null;

            objusers.Gender = (Request["hdngender"] == "" ? null : Request["hdngender"]);
            out_msg = Admin_postajob.verificationuserUpdate(objusers);


            if (out_msg != null & out_msg!="")
            {

                return Json(JsonResponseFactory.ErrorResponse("Enter email is already exists"), JsonRequestBehavior.DenyGet);

            }
            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
        }
        public ActionResult Changestatus(string verification_User_Id, string Login_ID,string status)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Admin_postajob objvusers = new Admin_postajob();
            if (status == "I")
            {
                objvusers.Status_Ind = 'I';
            }
            else if (status == "A")
            {
                objvusers.Status_Ind = 'A';
            }
            objvusers.JobPosting_User_ID = Convert.ToInt32(verification_User_Id);
            objvusers.Login_ID = Convert.ToInt32(Login_ID);

            Admin_postajob.InActive_verificationusers(objvusers, Convert.ToInt32(Session["Loginhistory_id"]));

            return RedirectToAction("ApproveProviders");
        }
        private string SetPhoneTypes(Int16 int_ID)
        {
            try
            {
            if (int_ID == 1)
            {
                return "Main:";
            }
            if (int_ID == 2)
            {
                return "Certification:";
            }
            if (int_ID == 3)
            {
                return "Prv Rltns:";
            }
            if (int_ID == 4)
            {
                return "Benefits:";
            }
            if (int_ID == 5)
            {
                return "Claims:";
            }
            if (int_ID == 6)
            {
                return "Other:";
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ProfilesController", "SetPhoneTypes", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             
            }
            return null;
        }
        public JsonResult GetProviderfullnames(string term)
        {
            //if (Session["UserID"] == null)
            //{
            //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
            //}
            var objlist = new List<string>();
            var objcom = new clsCommonFunctions();
            IDataParameter[] objparam = {
                                        new SqlParameter("@in_Payer_ID", Session["Payer_ID"]),
			new SqlParameter("@in_LastName", term),
			new SqlParameter("@in_PhoneNo", null),
			new SqlParameter("@in_City", null),
			new SqlParameter("@in_State", null),
			new SqlParameter("@in_Zip", null),
			new SqlParameter("@in_OrderbyItem", "Name"),
			new SqlParameter("@in_Order", "ASC")
                                        };
            objcom.AddInParameters(objparam);

            SqlDataReader drlist = objcom.GetDataReader("Help_dbo.st_Payer_List_Providers");
            while (drlist.Read())
            {
                objlist.Add(drlist[2].ToString());
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public void GetProviderAlphabets()
        {
            try
            {
            var dsProvideralpha = PTHome.getProviderAlphabet();
            if (dsProvideralpha.Tables.Count > 0)
            {
                if (dsProvideralpha.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Provideralphabets = dsProvideralpha.Tables[0].AsEnumerable().GetEnumerator();
                }
            }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "ProfilesController", "GetProviderAlphabets", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
        }
        public ActionResult Rowsperpage()
        {
            var cnt = Request["ddlrecords"];
            if (cnt != null)
            {
                Session["Rowsperpage"] = cnt;
            }

            return View();

        }

    }
}
