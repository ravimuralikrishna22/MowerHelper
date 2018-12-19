using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class AdminprofileController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Admindetails()
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetInfo(Convert.ToString(Session["userId"]));
           // var reg1 = clsCommonCClist.GetStates();

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
            return View();
        }
        [HttpPost]
        public ActionResult Admindetails(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string strHPNumber = null;
            string StrMobile = null;
            string strWPNumber = null;
            string strFax = null;
            strHPNumber = Request["txtHPNumber1"] + Request["txtHPNumber2"] + Request["txtHPNumber3"];
            strWPNumber = Request["txtWPNumber1"] + Request["txtWPNumber2"] + Request["txtWPNumber3"];
            StrMobile = Request["txtMobile1"] + Request["txtMobile2"] + Request["txtMobile3"];

            strFax = Request["txtUsFax1"] + Request["txtUsFax2"] + Request["txtUsFax3"];

            AdminProfile objProfile = new AdminProfile();
            var _with1 = objProfile;
            _with1.Username = (Request["txtUserName"] != null ? Request["txtUserName"] : null);
            _with1.Password = (Request["txtpwd"] != null ? Request["txtpwd"] : null);
            _with1.Prefix = (Request["ddlPreffix"] != " " ? Request["ddlPreffix"] : null);
            _with1.FirstName = (Request["txtFirstName"] != null ? Request["txtFirstName"] : null);
            _with1.MI = (Request["txtMiddleName"] != null ? Request["txtMiddleName"] : null);
            _with1.LastName = (Request["txtLastName"] != null ? Request["txtLastName"] : null);
            _with1.Suffix = (Request["ddlSuffix"] != " " ? Request["ddlSuffix"] : null);
            _with1.NickName = (Request["txtNickName"] != null ? Request["txtNickName"] : null);
            _with1.Email = (Request["txtEmailId1"] != null ? Request["txtEmailId1"] : null);
            _with1.CompanyName = (Request["txtCompany"] != null ? Request["txtCompany"] : null);
            _with1.Address1 = (Request["txtAddress1"] != null ? Request["txtAddress1"] : null);
            _with1.Address2 = (Request["txtAddress2"] != null ? Request["txtAddress2"] : null);
            _with1.City_ID = (Request["DDCity"] != "--Select City--" ? Request["DDCity"] : null);
            _with1.State_ID = (Request["DDState"] != "--Select State--" ? Request["DDState"] : null);
            _with1.ZIP = (Request["txtZip"] != null ? Request["txtZip"] : null);
            _with1.HomePhone = (string.IsNullOrEmpty(strHPNumber) ? null : strHPNumber);
            _with1.WorkPhone = (string.IsNullOrEmpty(strWPNumber) ? null : strWPNumber);
            _with1.MobilePhone = (string.IsNullOrEmpty(StrMobile) ? null : StrMobile);
            _with1.Fax = (string.IsNullOrEmpty(strFax) ? null : strFax);
            _with1.Website = (Request["txtWebSite"] != null ? Request["txtWebSite"] : null);
            _with1.DateofBirth = Convert.ToString(Request["txtDob"] != null & Request["txtDob"] != "mm/dd/yyyy" ? Request["txtDob"] : null);

            string strMSg = AdminProfile.updateAdminInfo(objProfile);
            return RedirectToAction("../AdminHome/Tools");
        }

        public void GetInfo(string LoginID)
        {
            try { 
                DataSet dsInfo = new DataSet();

                dsInfo = AdminProfile.GetAdminInfo(LoginID);

                if ((dsInfo != null))
                {
                    if (dsInfo.Tables.Count > 0)
                    {
                        if (dsInfo.Tables[0].Rows.Count > 0)
                        {
                            ViewBag.Prefix = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Prefix"]) != null & Convert.ToString(dsInfo.Tables[0].Rows[0]["Prefix"]) !="" ? dsInfo.Tables[0].Rows[0]["Prefix"] : "0");
                            ViewBag.FirstName = (Convert.ToString(dsInfo.Tables[0].Rows[0]["FirstName"]) != null & Convert.ToString(dsInfo.Tables[0].Rows[0]["FirstName"]) != "" ? dsInfo.Tables[0].Rows[0]["FirstName"] : null);
                            ViewBag.MI = (Convert.ToString(dsInfo.Tables[0].Rows[0]["MI"]) != null & Convert.ToString(dsInfo.Tables[0].Rows[0]["MI"]) !="" ? dsInfo.Tables[0].Rows[0]["MI"] : null);
                            ViewBag.LastName = (Convert.ToString(dsInfo.Tables[0].Rows[0]["LastName"]) != null & Convert.ToString(dsInfo.Tables[0].Rows[0]["LastName"]) !="" ? dsInfo.Tables[0].Rows[0]["LastName"] : null);
                            ViewBag.Suffix = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Suffix"]) != null & Convert.ToString(dsInfo.Tables[0].Rows[0]["Suffix"]) !=""? dsInfo.Tables[0].Rows[0]["Suffix"] : "0");
                            ViewBag.NickName = (Convert.ToString(dsInfo.Tables[0].Rows[0]["NickName"]) !="" ? dsInfo.Tables[0].Rows[0]["NickName"] : null);
                            ViewBag.CompanyName = (Convert.ToString(dsInfo.Tables[0].Rows[0]["CompanyName"]) != "" ? dsInfo.Tables[0].Rows[0]["CompanyName"] : null);
                            ViewBag.Email = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Email"]) != "" ? dsInfo.Tables[0].Rows[0]["Email"] : null);
                            ViewBag.Address1 = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Address1"]) != "" ? dsInfo.Tables[0].Rows[0]["Address1"] : null);
                            ViewBag.Address2 = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Address2"]) !="" ? dsInfo.Tables[0].Rows[0]["Address2"] : null);
                           if (Convert.ToString(dsInfo.Tables[0].Rows[0]["State_ID"])!="")
                            {
                                ViewBag.State_ID = dsInfo.Tables[0].Rows[0]["State_ID"];
                                ViewBag.State = Getstatename(Convert.ToString(dsInfo.Tables[0].Rows[0]["State_ID"]));
                            }

                            if (Convert.ToString(dsInfo.Tables[0].Rows[0]["City_ID"])!="")
                            {
                                ViewBag.City_ID = dsInfo.Tables[0].Rows[0]["City_ID"];
                                ViewBag.City = clsCommonCClist.Getcityname(Convert.ToString(dsInfo.Tables[0].Rows[0]["City_ID"]));
                            }

                            ViewBag.ZIP = (Convert.ToString(dsInfo.Tables[0].Rows[0]["ZIP"]) != "" ? dsInfo.Tables[0].Rows[0]["ZIP"] : null);
                            string strWorkPhone = null;
                            strWorkPhone = Convert.ToString(Convert.ToString(dsInfo.Tables[0].Rows[0]["WorkPhone"]) != "" ? dsInfo.Tables[0].Rows[0]["WorkPhone"] : null);

                            if (strWorkPhone != null & strWorkPhone != "")
                            {

                                ViewBag.WPhone1 = strWorkPhone.Substring(0, 3);
                                ViewBag.WPhone2 = strWorkPhone.Substring(3, 3);
                                ViewBag.WPhone3 = strWorkPhone.Substring(6, 4);
                                ViewBag.phnind = "2";
                                ViewBag.myphn1 = strWorkPhone.Substring(0, 3);
                                ViewBag.myphn2 = strWorkPhone.Substring(3, 3);
                                ViewBag.myphn3 = strWorkPhone.Substring(6, 4);
                            }
                            string strHomePhone = null;
                            strHomePhone = Convert.ToString(Convert.ToString(dsInfo.Tables[0].Rows[0]["HomePhone"])!="" ? dsInfo.Tables[0].Rows[0]["HomePhone"] : null);
                            if (strHomePhone != null & strHomePhone != "")
                            {

                                ViewBag.HPhone1 = strHomePhone.Substring(0, 3);
                                ViewBag.HPhone2 = strHomePhone.Substring(3, 3);
                                ViewBag.HPhone3 = strHomePhone.Substring(6, 4);
                                ViewBag.phnind = "1";
                                ViewBag.myphn1 = strHomePhone.Substring(0, 3);
                                ViewBag.myphn2 = strHomePhone.Substring(3, 3);
                                ViewBag.myphn3 = strHomePhone.Substring(6, 4);
                            }

                            if (dsInfo.Tables[0].Rows[0]["cellphone"] != null & dsInfo.Tables[0].Rows[0]["cellphone"].ToString() != "")
                            {
                                string StrCellPhone = Convert.ToString(dsInfo.Tables[0].Rows[0]["cellphone"]);

                                ViewBag.mphone1 = StrCellPhone.Substring(0, 3);
                                ViewBag.mphone2 = StrCellPhone.Substring(3, 3);
                                ViewBag.mphone3 = StrCellPhone.Substring(6, 4);
                            }
                            string strFax = null;
                            strFax = Convert.ToString(Convert.ToString(dsInfo.Tables[0].Rows[0]["Fax"])!="" ? dsInfo.Tables[0].Rows[0]["Fax"] : null);
                                if (strFax != null)
                                {
                                    ViewBag.Fax1 = strFax.Substring(0, 3);
                                    ViewBag.Fax2 = strFax.Substring(3, 3);
                                    ViewBag.Fax3 = strFax.Substring(6, 4);
                                }
                            else
                            {
                                ViewBag.Fax1 = null;
                                ViewBag.Fax2 = null;
                                ViewBag.Fax3 = null;
                            }
                            ViewBag.Website = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Website"]) !="" ? dsInfo.Tables[0].Rows[0]["Website"] : null);
                            string dob = Convert.ToString(Convert.ToString(dsInfo.Tables[0].Rows[0]["DateofBirth"]) != "" ? dsInfo.Tables[0].Rows[0]["DateofBirth"] : null);
                            if (!string.IsNullOrEmpty(Convert.ToString(dob)))// != null & dob.ToString() != "")
                            {
                                string[] dob1=dob.Split(' ');
                                ViewBag.Dob=dob1[0];
                            }
                            else
                            {
                                ViewBag.Dob=null;
                            }
                            ViewBag.Username = (Convert.ToString(dsInfo.Tables[0].Rows[0]["Username"]) !="" ? dsInfo.Tables[0].Rows[0]["Username"] : null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminprofileController", "GetInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
      
        public string Getstatename(string stateid)
        {
            try{
            clsCommonFunctions clscommon = new clsCommonFunctions();
            SqlDataReader dr_GetName;
            IDataParameter[] InParmList = { new SqlParameter("@in_State_ID", stateid) };
            clscommon.AddInParameters(InParmList);
            dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_StateName");
            while (dr_GetName.Read())
            {
                return dr_GetName["State"] != null ? dr_GetName["State"].ToString() : null;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminprofileController", "Getstatename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        //public string Getcityname(string cityid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_City_ID", cityid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
        //    while (dr_GetName.Read())
        //    {
        //        return dr_GetName["City"] != null ? dr_GetName["City"].ToString() : null;
        //    }
        //    return null;
        //}

    }
}
