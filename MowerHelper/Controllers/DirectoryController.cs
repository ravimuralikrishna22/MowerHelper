using Obout.Mvc.ComboBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.SERVICESMENU;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class DirectoryController : Controller
    {
        public string Requesturl;
        DataSet objds = new DataSet();
        int StartRec = 0;
        int PreviousRec;
        int NextRec;
        string aPublicURL = "";
        clsCommonFunctions ObjCommFun = new clsCommonFunctions();
        static string CustomizeFileName;
        static string HomePageFileName;
        //clsWebConfigsettings objconfig = new clsWebConfigsettings();
        public ActionResult ViewIdentifyingInfo()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Show_license").ToUpper() == "YES")
            {
                ViewBag.Show_license = "Y";
            }
            else
            {
                ViewBag.Show_license = "N";
            } Session.Add("TopIndex", "1");
            if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
            {

                return RedirectToAction("Schedulespecs", "Schedule");

            }
            else if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("Schedulespecs", "Schedule");
            }

            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
            if (Convert.ToString(Session["Roleid"]) == "1" & Request["Prov_ID"] != null)
            {
                Session.Add("Prov_ID", Request["Prov_ID"]);
                // Session.Add("Practice_ID", Request["Practice_ID"]);
                //List<Provider_TimaTable> ObjDataList = new List<Provider_TimaTable>();
                //ObjDataList = Provider_TimaTable.GetFacilitiesName(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(Session["Practice_ID"]));
                //if (ObjDataList.Count > 0)
                //{
                //    Session.Add("PrimaryPOSID", ObjDataList[0].PlaceOfService_ID);
                //}

            }
     // var objPCInfo =      GetProviderContactInfo();
      return View(GetProviderContactInfo());
        }
        private Provider_ContactInfo GetProviderContactInfo()
        {
            Provider_ContactInfo objContactInfo = new Provider_ContactInfo();
            //ViewBag.proid = Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]);
            //var description = new Referrals { FieldIDString = "17,19,20,21" };
            //var ds = Referrals.List_field_description(description.FieldIDString);
            var ds = Referrals.List_field_description("17,19,20,21");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.fbcommentsmsg = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.licensemsg = Convert.ToString(ds.Tables[0].Rows[1][3]);
                ViewBag.Electmsg = Convert.ToString(ds.Tables[0].Rows[2][3]);
                ViewBag.Drivermsg = Convert.ToString(ds.Tables[0].Rows[3][3]);
            }
            try
            {

                 objContactInfo = Provider_ContactInfo.Get_ContactInfo(Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]), 2);
                //ViewBag.proid = Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]);
                if ((objContactInfo != null))
                {
                    //ViewBag.PracticeName = !string.IsNullOrEmpty(objContactInfo.PracticeName) ? objContactInfo.PracticeName : null;
                    if (!string.IsNullOrEmpty(objContactInfo.PracticeName))
                    {
                        Session["Elebusenessnm"] = "Electrician Info(" + objContactInfo.PracticeName + ")";
                    }
                    else
                    {
                        Session["Elebusenessnm"] = "Electrician Info(" + null + ")";
                    }
                    //ViewBag.Fullname = objContactInfo.Fullname ?? null;

                    //ViewBag.Firstname = !string.IsNullOrEmpty(objContactInfo.Firstname) ? objContactInfo.Firstname : null;
                    //ViewBag.MiddleInitial = !string.IsNullOrEmpty(objContactInfo.MiddleInitial) ? objContactInfo.MiddleInitial : null;
                    //ViewBag.LastName = !string.IsNullOrEmpty(objContactInfo.LastName) ? objContactInfo.LastName : null;
                    //ViewBag.Address1 = !string.IsNullOrEmpty(objContactInfo.Address1) ? objContactInfo.Address1 : null;
                    //ViewBag.Address2 = !string.IsNullOrEmpty(objContactInfo.Address2) ? objContactInfo.Address2 : null;
                    //ViewBag.Zip = !string.IsNullOrEmpty(objContactInfo.Zip) ? objContactInfo.Zip : null;
                    //ViewBag.Referralcode = !string.IsNullOrEmpty(objContactInfo.Referralcode) ? objContactInfo.Referralcode : null;
                    //ViewBag.Refferal_Indicator = !string.IsNullOrEmpty(objContactInfo.Refferal_Indicator) ? objContactInfo.Refferal_Indicator : null;
                    //if (!string.IsNullOrEmpty(objContactInfo.StateName))
                    //{
                    //    ViewBag.Statename = objContactInfo.StateName;
                    //    ViewBag.State_ID = objContactInfo.State_ID;

                    //}
                    //else
                    //{
                    //    ViewBag.Statename = null;
                    //}
                    //if (!string.IsNullOrEmpty(objContactInfo.CityName))
                    //{
                    //    ViewBag.Cityname = objContactInfo.CityName;
                    //    ViewBag.City_ID = objContactInfo.City_ID;
                    //}
                    //else
                    //{
                    //    ViewBag.Cityname = null;
                    //}
                    //if (!string.IsNullOrEmpty(objContactInfo.CellPhone))
                    //{
                    //    ViewBag.CellPhone = clsCommonFunctions.UsPhoneFormat(objContactInfo.CellPhone);
                    //    if (objContactInfo.CellPhone.Length >= 10)
                    //    {
                    //        ViewBag.txtContCPhone1 = objContactInfo.CellPhone.Substring(0, 3);
                    //        ViewBag.txtContCPhone2 = objContactInfo.CellPhone.Substring(3, 3);
                    //        ViewBag.txtContCPhone3 = objContactInfo.CellPhone.Substring(6, 4);
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(objContactInfo.WorkPhone))
                    //{
                    //    ViewBag.WPhone = Convert.ToString(objContactInfo.WorkPhone);
                    //    if (objContactInfo.WorkPhone.Length >= 10)
                    //    {
                    //        string[] cpn1 = objContactInfo.WorkPhone.Split('-');
                    //        ViewBag.txtContWPhone1 = cpn1[0];
                    //        ViewBag.txtContWPhone2 = cpn1[1];
                    //        ViewBag.txtContWPhone3 = cpn1[2];
                    //    }
                    //}
                    //ViewBag.Fax = !string.IsNullOrEmpty(objContactInfo.Fax) ? objContactInfo.Fax : null;
                    //if (!string.IsNullOrEmpty(objContactInfo.Fax))
                    //{
                        //if (objContactInfo.Fax.Length >= 10)
                        //{
                        //    string[] cfax = objContactInfo.Fax.Split('-');
                        //    ViewBag.txtContFax1 = cfax[0];
                        //    ViewBag.txtContFax2 = cfax[1];
                        //    ViewBag.txtContFax3 = cfax[2];
                        //}
                    //}

                    //ViewBag.Email = !string.IsNullOrEmpty(objContactInfo.Email) ? objContactInfo.Email : null;

                    //ViewBag.Website = !string.IsNullOrEmpty(objContactInfo.Website) ? objContactInfo.Website : null;
                    //ViewBag.facebookurl = !string.IsNullOrEmpty(objContactInfo.facebookurl) ? objContactInfo.facebookurl : null;
                    //ViewBag.twitterurl = !string.IsNullOrEmpty(objContactInfo.twitterurl) ? objContactInfo.twitterurl : null;
                    ViewBag.fbcomments = objContactInfo.fbcomments == "Y" ? "Yes" : "No";
                    //ViewBag.LicenseNo = !string.IsNullOrEmpty(objContactInfo.LicenseNo) ? objContactInfo.LicenseNo : null;
                    //ViewBag.Random_number = !string.IsNullOrEmpty(objContactInfo.Random_number) ? objContactInfo.Random_number : null;
                    //if (!string.IsNullOrEmpty(objContactInfo.IsLicenseVerified))
                    //{
                    //    if (objContactInfo.IsLicenseVerified == "Y")
                    //    {
                    //        ViewBag.IsLicenseVerified = "Y";
                    //        ViewBag.islicense = "License verified";
                    //    }
                    //    else if (objContactInfo.IsLicenseVerified == "N")
                    //    {
                    //        ViewBag.IsLicenseVerified = "N";
                    //        ViewBag.islicense = "License not verified";
                    //    }

                    //}
                    //else
                    //{
                    //    ViewBag.islicense = "License not verified";
                    //}


                    //if (objContactInfo.licenseexpirydate != null & objContactInfo.licenseexpirydate != "")
                    //{
                    //    ViewBag.licenseexpirydate = DateTime.Parse(objContactInfo.licenseexpirydate).ToString("MM/dd/yyyy");

                    //}
                    //else
                    //{
                    //    ViewBag.licenseexpirydate = null;


                    //}
                    //ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
                    string Requesturl = null;
                    if (Request.Url != null) Requesturl = Request.Url.ToString();
                    var strurl = Requesturl.Split('/');
                    string domain = null;
                    if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                    {
                        if (Requesturl.Contains("localhost:"))
                        {
                            domain = strurl[0] + "//" + strurl[2] + "/";

                        }
                        else if (Requesturl.Contains("vbv"))
                        {
                            domain = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/";

                        }
                    }
                    else
                    {
                        domain = clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal");
                    }
                    if (!string.IsNullOrEmpty(objContactInfo.ProviderImage))
                    {
                        string str = objContactInfo.ProviderImage;
                        ViewData["str"] = str;
                        ViewBag.strimage = str;
                        if (str.Contains(".") == true)
                        {
                            string[] str1 = str.Split('.');
                            ViewData["ExistedLogo"] = str1[0];
                            ViewBag.ExistedLogo = str1[0];
                            ViewData["ExistedExtn"] = str1[1];
                            ViewBag.ExistedExtn = str1[1];
                        }
                        string strings = Path.Combine(Server.MapPath("~/Attachments/Providers"), str);
                        if (System.IO.File.Exists(strings))
                        {

                            ViewBag.imageavail = "Y";
                            ViewBag.image = "Y";
                            ViewBag.providerimage = domain + "Attachments/Providers/" + str;

                        }


                    }
                    if (!string.IsNullOrEmpty(objContactInfo.ElectricianImage))
                    {
                        string strEle = objContactInfo.ElctricianCustmiseImage;
                        ViewData["str"] = strEle;
                        //ViewBag.strEleimage = strEle;
                        if (strEle.Contains(".") == true)
                        {
                            string[] str11 = strEle.Split('.');
                            ViewData["ExistedEleLogo"] = str11[0];
                            ViewBag.ExistedEleLogo = str11[0];
                            ViewData["ExistedEleExtn"] = str11[1];
                            ViewBag.ExistedEleExtn = str11[1];
                        }
                        string strings = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), strEle);
                        if (System.IO.File.Exists(strings))
                        {

                            //ViewBag.imageavail = "Y";
                            ViewBag.Eleimage = "Y";
                            if (strEle.Contains(".pdf"))
                            {
                                ViewBag.providerEleimagepdf = objContactInfo.ElectricianImage;
                            }
                            else
                            {
                                ViewBag.providerEleimage = domain + "Attachments/VerifiedDocuments/" + strEle;
                            }


                        }
                        //ViewBag.electricianImage = objContactInfo.ElectricianImage;
                    }
                    //if (!string.IsNullOrEmpty(objContactInfo.ElctricianCustmiseImage))
                    //{
                    //    ViewBag.ElctricianCustmiseImage = objContactInfo.ElctricianCustmiseImage;
                    //}
                    //if (!string.IsNullOrEmpty(objContactInfo.DriverCustomiseImage))
                    //{
                    //    ViewBag.DriverCustomiseImage = objContactInfo.DriverCustomiseImage;
                    //}
                    if (!string.IsNullOrEmpty(objContactInfo.DriverImage))
                    {
                        string strdriver = objContactInfo.DriverCustomiseImage;
                        ViewData["str"] = strdriver;
                        ViewBag.strDriverimage = strdriver;
                        if (strdriver.Contains(".") == true)
                        {
                            string[] strD = strdriver.Split('.');
                            ViewData["ExistedDriverLogo"] = strD[0];
                            ViewBag.ExistedDriverLogo = strD[0];
                            ViewData["ExistedDriverExtn"] = strD[1];
                            ViewBag.ExistedDriverExtn = strD[1];
                        }
                        string stringsdriver = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), strdriver);
                        if (System.IO.File.Exists(stringsdriver))
                        {

                            ViewBag.DriverimageInd = "Y";
                            //ViewBag.image = "Y";
                            if (strdriver.Contains(".pdf"))
                            {
                                ViewBag.providerDriverimagepdf = objContactInfo.DriverImage;
                            }
                            else
                            {
                                ViewBag.providerDriverimage = domain + "Attachments/VerifiedDocuments/" + strdriver;
                            }

                        }
                        //ViewBag.driverImage = objContactInfo.DriverImage;

                    }
                   
                }
                return objContactInfo;
            }
            catch (Exception ex)
            {
               
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "DirectoryController", "GetProviderContactInfo", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        public ActionResult ReferToFriend(string code)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            
                ViewBag.code = code;
                return View();
           
        }
        [HttpPost]
        public JsonResult ReferToFriend(string code, string txtEmailID)
        {
           
            //clsWebConfigsettings cs = new clsWebConfigsettings();
            string strsubject = null;
            string strbody = null;
            string mailfrom = null;
            string mailto = null;
           // string Providername = null;
            if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
            {
                var objcommon = new clsCommonFunctions();
                IDataParameter[] insparam ={ new SqlParameter("@In_ReferenceLogin_ID",Session["UserID"]),
                                                   new SqlParameter("@In_Practice_ID",null),
                                                   new SqlParameter("@in_ToMailId",txtEmailID!=""?txtEmailID:null)
                     };
                objcommon.AddInParameters(insparam);
                objcommon.GetDataReader("Help_dbo.st_EmailMessage_INS_refferalcodeemail");
                IDataParameter[] insmail ={
                                              new SqlParameter("@in_provider_id",Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]) )
      
                                              };
                objcommon.AddInParameters(insmail);
                SqlDataReader drEmail = objcommon.GetDataReader("Help_dbo.st_get_emailmessage_referalcode");
                string strEmailMessage_Transaction_ID = string.Empty;
                if (drEmail.Read())
                {
                    if (!DBNull.Value.Equals(drEmail["Subject"]))
                    {
                        strsubject = Convert.ToString(drEmail["Subject"]);
                    }
                    if (!DBNull.Value.Equals(drEmail["Message_Body"]))
                    {
                        strbody = Convert.ToString(drEmail["Message_Body"]);
                    }
                    if (!DBNull.Value.Equals(drEmail["Mail_From"]))
                    {
                        mailfrom = Convert.ToString(drEmail["Mail_From"]);
                    }
                    if (!DBNull.Value.Equals(drEmail["Mail_To"]))
                    {
                        mailto = Convert.ToString(drEmail["Mail_To"]);
                    }
                    if (!string.IsNullOrEmpty(strbody))
                    {
                        strbody = strbody.Replace("$RefferalCode$", code);
                    }
                }
                ClsMailMessage objmessage = new ClsMailMessage();
                bool strvalid = objmessage.SendMail(mailto, mailfrom, strsubject, strbody, EMailFormats.MailFormatText, EMailPriorities.PriorityNormal);
                strEmailMessage_Transaction_ID = Convert.ToString(drEmail["EmailMessage_Transaction_ID"]);
                if (strvalid == true)
                {
                    IDataParameter[] strmail ={
                                                   new SqlParameter("@In_EmailMessage_Transaction_IDs",strEmailMessage_Transaction_ID)
                                              };
                    objcommon.AddInParameters(strmail);
                    objcommon.GetDataReader("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                    return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                }
            }
            return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
            // return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
        }
        public ActionResult EditIdentifyingInfo()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Show_license").ToUpper() == "YES")
            {
                ViewBag.Show_license = "Y";
            }
            else
            {
                ViewBag.Show_license = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
        //var objProviderContactInfo =     GetProviderContactInfo();
            //var reg1 = clsCommonCClist.GetStates();

            //List<clsCountry> objstates = clsCountry.GetStatesByCountryId(1);
            //IList<SelectListItem> result1 = new List<SelectListItem>();
            //if (objstates.Count > 0)
            //{
            //    for (int i = 0; i <= objstates.Count - 1; i++)
            //    {
            //        result1.Add(new SelectListItem
            //        {
            //            Text = objstates[i].StateFullName,
            //            Value = Convert.ToString(objstates[i].StateId)
            //        });
            //    }
            //}
            //IList<SelectListItem> result2 = new List<SelectListItem>();
            //result2.Add(new SelectListItem
            //{
            //    Text = "--Select City--",
            //    Value = "0",
            //    Selected = true
            //});
            //var reg1 = new StateCity
            //{
            //    StateList = result1,
            //    CityList = result2
            //};
            if (Request["lbloutmsg"] == "Y")
            {
                ViewBag.outmsg = "Y";
            }
            if (Request["Imagetype"] == "I")
            {
                ViewBag.Imagetype = "I";
            }

            return View("EditIdentifyingInfo", GetProviderContactInfo());
        }
        private void Providerdetails(string randomnumber)
        {

            //var objconfig = new clsWebConfigsettings();

            var obj = new ProviderAdvertising();
            var objData = obj.GetTheProviderProfile(randomnumber);
            TempData["Providerid1"] = objData.ProviderID;
            if (Convert.ToString(objData.ProviderID) != "" & objData.ProviderID != 0)
            {
                ViewBag.ProviderID = objData.ProviderID;
                int providerid = Convert.ToInt32(objData.ProviderID);
                ViewBag.videocnt = objData.VideoCount;
                ViewBag.ProviderName = objData.ProviderFullName;
                ViewBag.picture = objData.Picture;
                string Requesturl = null;
                if (Request.Url != null) Requesturl = Request.Url.ToString();
                var strurl = Requesturl.Split('/');
                string domain = null;
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    if (Requesturl.Contains("localhost:"))
                    {
                        domain = strurl[0] + "//" + strurl[2] + "/";

                    }
                    else if (Requesturl.Contains("vbv"))
                    {
                        domain = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/";

                    }
                }
                else
                {
                    domain = clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal");
                }
                if (!string.IsNullOrEmpty(objData.Picture))
                {
                    string str = objData.Picture;
                    ViewData["str"] = str;
                    ViewBag.strimage = str;
                    if (str.Contains(".") == true)
                    {
                        string[] str1 = str.Split('.');
                        ViewData["ExistedLogo"] = str1[0];
                        ViewBag.ExistedLogo = str1[0];
                        ViewData["ExistedExtn"] = str1[1];
                        ViewBag.ExistedExtn = str1[1];
                    }

                    string strings = null;
                    strings = Path.Combine(Server.MapPath("~/Attachments/Providers"), str);
                    if (System.IO.File.Exists(strings))
                    {

                        ViewBag.imageavail = "Y";
                        ViewBag.image = "Y";
                        ViewBag.providerimage = domain + "Attachments/Providers/" + str;

                    }

                    else
                    {
                        ViewBag.providerimage = domain + "images/Coach1.JPG";

                    }

                }
                else
                {

                    ViewBag.providerimage = domain + "images/Coach1.JPG";



                }

                ViewBag.BusinessName = objData.Businessname;
                ViewBag.provideremail = objData.Email;
                ViewBag.businessnm1 = objData.PublicURL.Trim().Replace(" ", "-");
                ViewBag.licenseverify = objData.IsLicenseVerified;
                ViewBag.fbcomments = objData.fbcomments;
                TempData["proname"] = ViewBag.businessnm1;
                ViewBag.showschedule = objData.show_schedule ?? null;
                string strcookiestate = null;
                string strcookiecity = null;
                if ((Request.Cookies["State"] != null))
                {
                    if (Request.Cookies["State"].Value != null)
                    {
                        strcookiestate = Request.Cookies["State"].Value.Replace("%20", " ");
                    }
                }
                if ((Request.Cookies["City"] != null))
                {
                    if (Request.Cookies["City"].Value != null)
                    {
                        strcookiecity = Request.Cookies["City"].Value.Replace("%20", " ");
                    }
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
                {
                    ViewBag.Showschedulecog = "Y";
                }
                else
                {
                    ViewBag.Showschedulecog = "N";
                }
                //var obj1 = new ProviderAdvertising
                //{
                //    Provider_ID = Convert.ToString(providerId),
                //    SiteStatic_ID = "4",
                //    State_Name = strcookiestate,
                //    CityNm = strcookiecity
                //};
                //obj.CountingProviderVisitToHisProfile(obj1);
                providerid = objData.ProviderID;
                if (objData.Is_Contact_Info == "Y")
                {
                    ViewBag.contactinfo = "Y";
                }
                if (objData.Country != null)
                {
                    ViewBag.country = objData.Country;
                    ViewBag.Title = "Electricians in " + objData.Country;
                    ViewBag.country = objData.Country;
                    ViewBag.countryid = objData.Country_ID;

                }
                else
                {
                    ViewBag.country = "United States";
                    ViewBag.Title = "Electricians in United States";
                    ViewBag.countryid = "1";
                }
                if (objData.State_Name != null)
                {
                    ViewBag.statenm = objData.State_Name;
                    string statenm = objData.State_Name;
                    ViewBag.statetitle = "Electricians in " + objData.State_Name;
                    if (statenm.Contains("-"))
                    {
                        statenm = statenm.Replace("-", " ");
                        ViewBag.statenm = statenm;

                    }
                    string stateab = Convert.ToString(obj.GetProviderStateab(statenm));
                    ViewBag.stateab = stateab;

                }
                else
                {
                    ViewBag.statenm = null;
                }


                ViewBag.stateid = objData.State_ID ?? null;
                if (objData.CityNm != null)
                {
                    ViewBag.citynm = objData.CityNm;
                    ViewBag.citytitle = "Electricians in " + objData.CityNm;
                    ViewBag.city = objData.CityNm + " (" + ViewBag.stateab + ")";
                }
                else
                {
                    ViewBag.citynm = null;
                }

                if (objData.ZIPValue != null)
                {
                    string zipCode = objData.ZIPValue;
                    DataListCities_DataListZIPCodes_Bind(zipCode);
                }
                if (objData.Address != null)
                {
                    ViewBag.address = objData.Address;
                }
                else
                {
                    ViewBag.address = null;
                    ViewBag.mapaddress = null;
                }
                ViewBag.fax = objData.Fax ?? null;
                ViewBag.Vmoffice = objData.Vmoffice ?? null;
                ViewBag.cellphone = objData.cellphone ?? null;
                ViewBag.Availability = objData.Availability ?? null;
                ViewBag.Personalwebsite = "1";
                if (objData.Webaddress != null)
                {
                    if (objData.Webaddress.StartsWith("http"))
                    {
                        objData.Webaddress = objData.Webaddress;
                        ViewBag.Webaddress = objData.Webaddress;
                    }
                    else if (objData.Webaddress.StartsWith("https"))
                    {
                        objData.Webaddress = objData.Webaddress;
                        ViewBag.Webaddress = objData.Webaddress;
                    }
                    else
                    {
                        objData.Webaddress = "http://" + objData.Webaddress;
                        ViewBag.Webaddress = "http://" + objData.Webaddress;
                    }
                }
                else
                {
                    ViewBag.Webaddress = null;
                }
                ViewBag.lnkPersonalNavigateUrl = objData.Webaddress;

                if (objData.Description != null)
                {
                    ViewBag.desc = objData.Description;
                }
                if (objData.Payments != null)
                {
                    ViewBag.paymentsandconditions = objData.Payments.Trim();
                }

                if (objData.Avgcostepersessionfrom != null)
                {
                    string s = objData.Avgcosepersessionto;
                    if (Convert.ToDecimal(objData.Avgcostepersessionfrom) >= 0 & Convert.ToDecimal(objData.Avgcosepersessionto) >= 0)
                    {
                        if (Convert.ToDouble(objData.Avgcosepersessionto) > 0)
                        {
                            ViewBag.avcostpersession = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(objData.Avgcostepersessionfrom))) + " - " + "$" + string.Format("{0:0.00}", (Convert.ToDecimal(objData.Avgcosepersessionto)));
                        }
                        else
                        {
                            ViewBag.avcostpersession = string.Format(objData.Avgcostepersessionfrom);
                        }

                    }
                }
                if (objData.License != null)
                {
                    string str = objData.License.ToUpper();
                    if (str.Contains("XXXX") | str.Contains("XXX") | str.Contains("XX"))
                    {
                        ViewBag.license = null;

                    }
                    else
                    {
                        ViewBag.license = objData.License;
                    }
                }
                else
                {
                    ViewBag.license = null;
                }

                ViewBag.Yearsinpractice = objData.Yearsinpractice != 0 ? (dynamic)objData.Yearsinpractice : null;
                if (objData.twitterurl != null)
                {

                    if (objData.twitterurl.StartsWith("http"))
                    {
                        ViewBag.twitterurl = objData.twitterurl;
                    }
                    else if (objData.twitterurl.StartsWith("https"))
                    {
                        ViewBag.twitterurl = objData.twitterurl;
                    }
                    else
                    {
                        ViewBag.twitterurl = "http://" + objData.twitterurl;
                    }

                }
                else
                {
                    ViewBag.twitterurl = null;
                }
                if (objData.facebookurl != null)
                {

                    if (objData.facebookurl.StartsWith("http"))
                    {
                        ViewBag.facebookurl = objData.facebookurl;
                    }
                    else if (objData.facebookurl.StartsWith("https"))
                    {
                        ViewBag.facebookurl = objData.facebookurl;
                    }
                    else
                    {
                        ViewBag.facebookurl = "http://" + objData.facebookurl;
                    }



                }
                else
                {
                    ViewBag.facebookurl = null;
                }
                ViewBag.PublicURL = objData.PublicURL;
                ViewBag.publicurl = objData.PublicURL;
                objds = obj.GetPreviousNextRecords(objData.State_ID, objData.City_ID);

                {
                    if (objds.Tables[0].Rows.Count > 1)
                    {

                        DataTable dt = objds.Tables[0];
                        for (int row = 0; row <= dt.Rows.Count - 1; row++)
                        {
                            if (Convert.ToString(objds.Tables[0].Rows[row]["Random_number"]) == randomnumber)
                            {
                                StartRec = row;
                            }
                        }
                        if (StartRec == 0)
                        {
                            PreviousRec = objds.Tables[0].Rows.Count - 1;
                        }
                        else
                        {
                            PreviousRec = StartRec - 1;
                        }
                        if (StartRec == objds.Tables[0].Rows.Count - 1)
                        {
                            NextRec = 0;
                        }
                        else
                        {
                            NextRec = StartRec + 1;
                        }
                        for (int row = 0; row <= dt.Rows.Count - 1; row++)
                        {
                            aPublicURL = Convert.ToString(objds.Tables[0].Rows[row]["PublicURL"]);
                            string[] prevNextUrl = aPublicURL.Split('.');
                            string randomnumber1 = Convert.ToString(objds.Tables[0].Rows[row]["Random_number"]);
                            string nextprezip = Convert.ToString(objds.Tables[0].Rows[row]["l_zipcode"]);
                            if (row == PreviousRec)
                            {
                                ViewBag.previousrecord = randomnumber1;
                                ViewBag.preproname = prevNextUrl[0].Trim().Replace(" ", "-");
                                ViewBag.nextprezip = nextprezip;
                            }
                            if (row == NextRec)
                            {
                                ViewBag.nextrecord = randomnumber1;
                                ViewBag.nextprovnme = prevNextUrl[0].Trim().Replace(" ", "-");
                                ViewBag.nextprezip = nextprezip;

                            }
                        }
                    }
                    else
                    {
                        ViewBag.previousrecord = null;
                        ViewBag.nextrecord = null;

                    }
                }
            }

        }
        public void DataListCities_DataListZIPCodes_Bind(string zipCode)
        {
            var objCitiesList = new List<clsNearestCities>();
            var objZipCodesList = new List<clsNearestZIPCodes>();
            var obj = new clsNearestCities();

            try
            {
                obj.GetNearestZIPCodesandNearestCities(zipCode, ref objCitiesList, ref objZipCodesList);
                if (objCitiesList.Count > 0)
                {
                    ViewBag.DtlistCities = objCitiesList;
                    ViewBag.Dtlistcitescount = objCitiesList.Count;
                    int rowcount = objCitiesList.Count / 2;
                    ViewBag.rows1 = rowcount;

                }
                else
                {
                    ViewBag.Dtlistcitescount = 0;
                }
                if (objZipCodesList.Count > 0)
                {
                    ViewBag.DtlistZIPCodes = objZipCodesList;
                    ViewBag.DtlistZIPCodescount = objZipCodesList.Count;
                    int rowcount = objZipCodesList.Count / 2;
                    ViewBag.rows2 = rowcount;

                }
                else
                {
                    ViewBag.DtlistZIPCodescount = 0;

                }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "PublicController", "DataListCities_DataListZIPCodes_Bind", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }

        }
    
        private bool Getfilename(HttpPostedFileBase flLogo)
        {
            string FileExtn1 = null;
            string strFilePath = null;
            FileExtn1 = System.IO.Path.GetExtension(flLogo.FileName);
            ViewData["EncryptLogo"] = clsCommonCClist.RandomPassword();
            string filename = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
            strFilePath = Path.Combine(Server.MapPath("~/Attachments/Providers"), filename);
            ViewData["LogoPath"] = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
            flLogo.SaveAs(strFilePath);
            return true;
        }
        private bool GetElectricianfilename(HttpPostedFileBase myElectricianFile1)
        {
            string FileElecExtn1 = null;
            string strFileElecPath = null;
            FileElecExtn1 = System.IO.Path.GetExtension(myElectricianFile1.FileName);
            ViewData["ElecFilename"] = System.IO.Path.GetFileName(myElectricianFile1.FileName);
            ViewData["EncryptElecFile"] = clsCommonCClist.RandomPassword();
            string fileElecname = Convert.ToString(ViewData["EncryptElecFile"]) + FileElecExtn1;
            strFileElecPath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), fileElecname);
            ViewData["ElecFilePath"] = Convert.ToString(ViewData["EncryptElecFile"]) + FileElecExtn1;
            myElectricianFile1.SaveAs(strFileElecPath);
            return true;
        }
        private bool GetDriverfilename(HttpPostedFileBase myDriverFile)
        {
            string FileDriverExtn1 = null;
            string strFileDriverPath = null;
            FileDriverExtn1 = System.IO.Path.GetExtension(myDriverFile.FileName);
            ViewData["DrlverFilename"] = System.IO.Path.GetFileName(myDriverFile.FileName);
            ViewData["EncryptDriverFile"] = clsCommonCClist.RandomPassword();
            string filedrivername = Convert.ToString(ViewData["EncryptDriverFile"]) + FileDriverExtn1;
            strFileDriverPath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), filedrivername);
            ViewData["ElecDrlverPath"] =Convert.ToString( ViewData["EncryptDriverFile"]) + FileDriverExtn1;
            myDriverFile.SaveAs(strFileDriverPath);
            return true;
        }
        //private string RandomPassword()
        //{
        //    return Convert.ToString(Guid.NewGuid());

        //}
        [HttpPost()]
        public ActionResult EditIdentifyingInfo(SimpleProvider_DetailsInfo obj, HttpPostedFileBase flLogo, HttpPostedFileBase myElectricianFile1, HttpPostedFileBase myDriverFile)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (flLogo != null)
            {
                if (flLogo.ContentLength > 0)
                {
                    Getfilename(flLogo);
                    if (!string.IsNullOrEmpty(Request["strimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/Providers"), Request["strimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Request["ExistedLogo"]))
                {
                    ViewData["LogoPath"] = Request["strimage"];
                }
                else
                {
                    ViewData["LogoPath"] = null;
                    if (!string.IsNullOrEmpty(Request["strimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/Providers"), Request["strimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            if (myElectricianFile1 != null)
            {
                if (myElectricianFile1.ContentLength > 0)
                {
                    GetElectricianfilename(myElectricianFile1);
                    if (!string.IsNullOrEmpty(Request["strEleimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), Request["strEleimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Request["ExistedEleLogo"]))
                {
                    ViewData["ElecFilePath"] = Request["strEleimage"];
                    ViewData["ElecFilename"] = Request["strElename"];
                }
                else
                {
                    ViewData["ElecFilePath"] = null;
                    ViewData["ElecFilename"] = null;
                    if (!string.IsNullOrEmpty(Request["strEleimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), Request["strEleimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            if (myDriverFile != null)
            {
                if (myDriverFile.ContentLength > 0)
                {
                    GetDriverfilename(myDriverFile);
                    if (!string.IsNullOrEmpty(Request["strDriverimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), Request["strDriverimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Request["ExistedDriverLogo"]))
                {
                    ViewData["ElecDrlverPath"] = Request["strDriverimage"];
                    ViewData["DrlverFilename"] = Request["strDrivername"];
                }
                else
                {
                    ViewData["ElecDrlverPath"] = null;
                    ViewData["DrlverFilename"] = null;
                    if (!string.IsNullOrEmpty(Request["strDriverimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), Request["strDriverimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            if (Request["txtPracticeName"] != null & Request["txtPracticeName"] != "")
            {
                obj.PracticeName = Request["txtPracticeName"];
            }
            else
            {
                obj.PracticeName = null;
            }
            if (Request["txtFName"] != null & Request["txtFName"].Trim() != "")
            {
                obj.FirstName = Request["txtFName"];
            }
            else
            {
                obj.FirstName = null;
            }
            if (Request["txtMName"] != null & Request["txtMName"].Trim() != "")
            {
                obj.MI = Request["txtMName"];
            }
            else
            {
                obj.MI = null;
            }
            if (Request["txtLName"] != null & Request["txtLName"].Trim() != "")
            {
                obj.LastName = Request["txtLName"];
            }
            else
            {
                obj.LastName = null;
            }
            if (Request["txtLicense"] != null & Request["txtLicense"] != "")
            {
                obj.LicenseNo = Request["txtLicense"];
            }
            else
            {
                obj.LicenseNo = null;
            }
            if (Request["txtContEmail"] != null & Request["txtContEmail"] != "")
            {
                obj.Email = Request["txtContEmail"];
            }
            else
            {
                obj.Email = null;
            }
            if (Request["txtContAddress1"] != null & Request["txtContAddress1"] != "")
            {
                obj.Address1 = Request["txtContAddress1"];
            }
            else
            {
                obj.Address1 = null;
            }
            if (Request["txtContAddress2"] != null & Request["txtContAddress2"] != "")
            {
                obj.Address2 = Request["txtContAddress2"];
            }
            else
            {
                obj.Address2 = null;
            }
            if (Request["DDCity"] != null & Request["DDCity"] != "")
            {
                obj.City_ID = Convert.ToInt32(Request["DDCity"]);
            }
            else
            {
                obj.City_ID = null;
            }
            if (Request["DDState"] != null & Request["DDState"] != "")
            {
                obj.State_ID = Convert.ToInt32(Request["DDState"]);
            }
            else
            {
                obj.State_ID = null;
            }
            if (Request["txtZip"] != null & Request["txtZip"] != "")
            {
                obj.Zip = Request["txtZip"];
            }
            else
            {
                obj.Zip = null;
            }

            if ((Request["txtContCPhone1"] != null & Request["txtContCPhone1"] != "") & (Request["txtContCPhone2"] != null & Request["txtContCPhone2"] != "") & (Request["txtContCPhone3"] != null & Request["txtContCPhone3"] != ""))
            {
                obj.CellPhone = Request["txtContCPhone1"] + Request["txtContCPhone2"] + Request["txtContCPhone3"];
            }
            else
            {
                obj.CellPhone = null;
            }
            if ((Request["txtContWPhone1"] != null & Request["txtContWPhone1"] != "") & (Request["txtContWPhone2"] != null & Request["txtContWPhone2"] != "") & (Request["txtContWPhone3"] != null & Request["txtContWPhone3"] != ""))
            {
                obj.WorkPhone = Request["txtContWPhone1"] + Request["txtContWPhone2"] + Request["txtContWPhone3"];
            }
            else
            {
                obj.WorkPhone = null;
            }
            if ((Request["txtContFax1"] != null & Request["txtContFax1"] != "") & (Request["txtContFax2"] != null & Request["txtContFax2"] != "") & (Request["txtContFax3"] != null & Request["txtContFax3"] != ""))
            {
                obj.FAX = Request["txtContFax1"] + Request["txtContFax2"] + Request["txtContFax3"];
            }
            else
            {
                obj.FAX = null;
            }
            if (Request["txtWebsite"] != null & Request["txtWebsite"] != "")
            {
                obj.Website = Request["txtWebsite"];
            }
            else
            {
                obj.Website = null;
            }
            if (Request["txtfacebookurl"] != null & Request["txtfacebookurl"] != "")
            {
                obj.facebookurl = Request["txtfacebookurl"];
            }
            else
            {
                obj.facebookurl = null;
            }
            if (Request["rdfbcommentsYes"] == "True" | Request["rdfbcommentsYes"] == "False" & Request["rdfbcommentsNo"] == null)
            {
                obj.Fbcomments = "Y";

            }
            if (Request["rdfbcommentsNo"] == "True" | Request["rdfbcommentsNo"] == "False" & Request["rdfbcommentsYes"] == null)
            {
                obj.Fbcomments = "N";

            }
            if (Request["txttwitterurl"] != null & Request["txttwitterurl"] != "")
            {
                obj.twitterurl = Request["txttwitterurl"];
            }
            else
            {
                obj.twitterurl = null;
            }

            if (Convert.ToString(Session["Roleid"]) == "1")
            {
                if (Request["chkIsLicense"] != null)
                {
                    string[] WorkPhoneLeaveMsg = null;
                    if (Request["chkIsLicense"].Contains(","))
                    {
                        WorkPhoneLeaveMsg = Request["chkIsLicense"].Split(',');
                        if (WorkPhoneLeaveMsg.Length == 2)
                        {
                            if (WorkPhoneLeaveMsg[0] == "true")
                            {
                                obj.IsLicenseVerified = "Y";
                            }
                            else if (WorkPhoneLeaveMsg[0] == "false")
                            {
                                obj.IsLicenseVerified = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkIsLicense"] == "false")
                        {
                            obj.IsLicenseVerified = "N";
                        }
                        else if (Request["chkIsLicense"] == "true")
                        {
                            obj.IsLicenseVerified = "Y";
                        }
                    }

                }
                else
                {
                    obj.IsLicenseVerified = "N";
                }
                if (Request["txtexpirydate"] != "mm/dd/yyyy" & Convert.ToString(Request["txtexpirydate"]) != "")
                {
                    obj.licenseexpirydate = Request["txtexpirydate"];
                }
                else
                {
                    obj.licenseexpirydate = null;
                }

            }
            if (Convert.ToString(Session["Roleid"]) == "4")
            {
                if (Request["Hdndate"] != null & Request["Hdndate"] != "")
                {
                    obj.licenseexpirydate = Request["Hdndate"];
                }
                else
                {
                    obj.licenseexpirydate = null;
                }
            }


            obj.UpdatedBy = Convert.ToInt32(Session["userid"]);
            obj.ProviderImage = ViewData["LogoPath"] == null ? null : ViewData["LogoPath"].ToString();
            obj.ElctricianLicenseImage = ViewData["ElecFilename"] == null ? null : ViewData["ElecFilename"].ToString();
            obj.DriverLicsenseImage = ViewData["DrlverFilename"] == null ? null : ViewData["DrlverFilename"].ToString();
            obj.ElctricianCustmiseImage = ViewData["ElecFilePath"] == null ? null : ViewData["ElecFilePath"].ToString();
            obj.DriverCustomiseImage = ViewData["ElecDrlverPath"] == null ? null : ViewData["ElecDrlverPath"].ToString();
            string strOutMsgSimple = null;
            obj.Provider_ID = ((Session["Provider_ID"] == null) ? Convert.ToInt32(Session["Prov_ID"]) : Convert.ToInt32(Session["Provider_ID"]));
            SimpleProvider_DetailsInfo.Upd_SimpleProviderDetailsinDirectory(obj, ref strOutMsgSimple);
            if (!string.IsNullOrEmpty(strOutMsgSimple))
            {
                return RedirectToAction("EditIdentifyingInfo", new { lbloutmsg = "Y" });
            }
            else
            {
                return RedirectToAction("ViewIdentifyingInfo");
            }

        }
        public ActionResult ViewElectricianAdvancedSearch()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //var description = new Referrals { FieldIDString = "2,8,16,17,18" };
            //var ds = Referrals.List_field_description(description.FieldIDString);
            var ds = Referrals.List_field_description("2,8,16,17,18");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.firstfeesession = Convert.ToString(ds.Tables[0].Rows[1][3]);
                ViewBag.showcontact = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.hfeerange = Convert.ToString(ds.Tables[0].Rows[2][3]);
                ViewBag.fbcommentsmsg = Convert.ToString(ds.Tables[0].Rows[3][3]);
                ViewBag.scheduleicon = Convert.ToString(ds.Tables[0].Rows[4][3]);
            }

            GetProfilesInfo();
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("Freeestimate").ToUpper() == "YES")
            {
                ViewBag.Freeestimate = "Y";
            }
            else
            {
                ViewBag.Freeestimate = "N";
            }
            return View();
        }
        private void GetProfilesInfo()
        {
            ViewBag.proid = Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"]));
            var objProfilesInfo = Provider_ProfilesInfo.Get_PrividerProfilesInfo(Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])));
            if ((objProfilesInfo != null))
            {
                if (objProfilesInfo.Yearinpractice != null & objProfilesInfo.Yearinpractice != "" & objProfilesInfo.Yearinpractice != "0.00")
                {
                    string[] yp = objProfilesInfo.Yearinpractice.Split('.');
                    ViewBag.Yearinpractice = yp[0];
                }
                ViewBag.freesession = objProfilesInfo.freesession == "Y" ? "Yes" : "No";
                ViewBag.schedule = objProfilesInfo.showschedule == "Y" ? "Yes" : "No";
                ViewBag.Random_number = objProfilesInfo.Random_number ?? null;
                ViewBag.state = objProfilesInfo.State ?? null;
                ViewBag.city = objProfilesInfo.City ?? null;
                ViewBag.zip = objProfilesInfo.Zip ?? null;
                ViewBag.Website = !string.IsNullOrEmpty(objProfilesInfo.Website) ? objProfilesInfo.Website : null;
                ViewBag.facebookurl = !string.IsNullOrEmpty(objProfilesInfo.facebookurl) ? objProfilesInfo.facebookurl : null;
                ViewBag.twitterurl = !string.IsNullOrEmpty(objProfilesInfo.twitterurl) ? objProfilesInfo.twitterurl : null;
                //ViewBag.fbcomments = objProfilesInfo.fbcomments == "y" ? "yes" : "no";
                ViewBag.fbcomments = objProfilesInfo.fbcomments == "Y" ? "Yes" : "No";
                //ViewBag.Website = "Hi";
                //ViewBag.facebookurl = "hi";
                //ViewBag.twitterurl = "hi";
                //ViewBag.fbcomments = "Yes";
                if (objProfilesInfo.Is_Contact_Info != null & objProfilesInfo.Is_Contact_Info != "")
                {
                    ViewBag.Is_Contact_Info = objProfilesInfo.Is_Contact_Info;
                }
                else
                {
                    ViewBag.Is_Contact_Info = null;
                }

                if (objProfilesInfo.FeeRange_From != null & objProfilesInfo.FeeRange_From != "")
                {
                    ViewBag.FeeRange_From = objProfilesInfo.FeeRange_From;
                }
                else
                {
                    if (objProfilesInfo.FeeRange_From == "0.00" & objProfilesInfo.FeeRange_From != "")
                    {
                        ViewBag.FeeRange_From = objProfilesInfo.FeeRange_From;
                    }
                    else
                    {
                        ViewBag.FeeRange_From = null;
                    }
                }
                if (objProfilesInfo.FeeRange_to != null & objProfilesInfo.FeeRange_to != "")
                {
                    ViewBag.FeeRange_to = objProfilesInfo.FeeRange_to;
                }
                else
                {
                    if (objProfilesInfo.FeeRange_to == "0.00" & objProfilesInfo.FeeRange_to != "")
                    {
                        ViewBag.FeeRange_to = objProfilesInfo.FeeRange_to;
                    }
                    else
                    {
                        ViewBag.FeeRange_to = null;
                    }

                }

                if (objProfilesInfo.paragraph != null & objProfilesInfo.paragraph != "")
                {

                    ViewBag.paragraph = objProfilesInfo.paragraph;

                }
                else
                {
                    ViewBag.paragraph = null;

                }

                if (objProfilesInfo.URL != null & objProfilesInfo.URL != "")
                {
                    ViewBag.URL = objProfilesInfo.URL;

                }
                else
                {
                    ViewBag.URL = null;

                }
                if (objProfilesInfo.Payments != null & objProfilesInfo.Payments != "")
                {
                    ViewBag.Payments = objProfilesInfo.Payments;

                }
                else
                {
                    ViewBag.Payments = null;

                }
            }

        }
        public ActionResult EditElectricianAdvancedSearch()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            //var description = new Referrals { FieldIDString = "2,8,16,17,18" };
            //var ds = Referrals.List_field_description(description.FieldIDString);
            var ds = Referrals.List_field_description("2,8,16,17,18");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.firstfeesession = Convert.ToString(ds.Tables[0].Rows[1][3]);
                ViewBag.showcontact = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.hfeerange = Convert.ToString(ds.Tables[0].Rows[2][3]);
                ViewBag.fbcommentsmsg = Convert.ToString(ds.Tables[0].Rows[3][3]);
                ViewBag.scheduleicon = Convert.ToString(ds.Tables[0].Rows[4][3]);
            }
            GetProfilesInfo();
            FillYearsInPractice();
            if (ViewBag.paragraph != null & ViewBag.paragraph != "")
            {
                ViewData["txtDescription"] = System.Web.HttpUtility.HtmlDecode(ViewBag.paragraph);
            }
            else
            {
                ViewData["txtDescription"] = string.Empty;
            }
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("Freeestimate").ToUpper() == "YES")
            {
                ViewBag.Freeestimate = "Y";
            }
            else
            {
                ViewBag.Freeestimate = "N";
            }
            return View();
        }
        public ActionResult ProfilePreview(string randomnumber)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Providerdetails(randomnumber);
            string Therapistname = ViewBag.ProviderName;
            int prov_id = (int)ViewBag.ProviderID;
            string date = null;
            DataSet dsAppointmentsList = new DataSet();
            string Fromdate = null;
            string Todate = null;

            if (date != null)
            {
                Fromdate = date.Replace('-', '/');
                Todate = Convert.ToDateTime(Fromdate).AddDays(+4).ToShortDateString();
            }
            else
            {
                Fromdate = DateTime.Now.ToShortDateString();
                Todate = DateTime.Now.AddDays(+4).ToShortDateString();
            }
            ViewBag.nextdate = Convert.ToDateTime(Todate).AddDays(+1).ToShortDateString().Replace('/', '-');
            ViewBag.prevdate = Convert.ToDateTime(Fromdate).AddDays(-5).ToShortDateString().Replace('/', '-');
            ViewData["Previoustooltip"] = DateTime.Parse(Fromdate).AddDays(-5).ToShortDateString() + " To " + DateTime.Parse(Fromdate).AddDays(-1).ToShortDateString();
            ViewData["Nexttooltip"] = DateTime.Parse(Todate).AddDays(+1).ToShortDateString() + " To " + DateTime.Parse(Todate).AddDays(+5).ToShortDateString();


            //ViewBag.businessname = Therapistname;
            ViewBag.id = prov_id;
            int duration = 60;

            dsAppointmentsList = GetAppointment.GetProviderOpenSlots(prov_id, Fromdate, Todate, "07:00 AM", "07:00 PM", duration);
            int day1 = 0;
            int day2 = 0;
            int day3 = 0;
            int day4 = 0;
            int day5 = 0;


            if (date != null)
            {
                day1 = (int)Convert.ToDateTime(Fromdate).DayOfWeek + 1;
                day2 = (int)Convert.ToDateTime(Fromdate).AddDays(+1).DayOfWeek + 1;
                day3 = (int)Convert.ToDateTime(Fromdate).AddDays(+2).DayOfWeek + 1;
                day4 = (int)Convert.ToDateTime(Fromdate).AddDays(+3).DayOfWeek + 1;
                day5 = (int)Convert.ToDateTime(Fromdate).AddDays(+4).DayOfWeek + 1;
            }
            else
            {
                day1 = (int)DateTime.Now.DayOfWeek + 1;
                day2 = (int)DateTime.Now.AddDays(+1).DayOfWeek + 1;
                day3 = (int)DateTime.Now.AddDays(+2).DayOfWeek + 1;
                day4 = (int)DateTime.Now.AddDays(+3).DayOfWeek + 1;
                day5 = (int)DateTime.Now.AddDays(+4).DayOfWeek + 1;
            }
            ViewBag.practicename = "Schedules for  " + Therapistname.Replace("-", " ") + "  from  " + Fromdate + "  -  " + Todate;

            string backcolorTr = string.Empty;
            string colorfont = string.Empty;

            string strQuery2 = "wday = '" + day1 + "'";
            DataRow[] drFilterRows2 = dsAppointmentsList.Tables[0].Select(strQuery2);
            DataSet dsAppointmentsListResults2 = dsAppointmentsList.Clone();
            foreach (DataRow dr in drFilterRows2)
                dsAppointmentsListResults2.Tables[0].ImportRow(dr);

            StringBuilder strdayappt = new StringBuilder();
            strdayappt =
                strdayappt.Append(
                    "<table id='tblAppointments' cellspacing='1' cellpadding='1' width='100%' style='background-color:#d0e4f4;' >");
            strdayappt = strdayappt.Append(" <tr>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).AddDays(+1).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'> " + Convert.ToDateTime(Fromdate).AddDays(+2).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'>" + Convert.ToDateTime(Fromdate).AddDays(+3).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("<td align='center' style='background-color:#1d70ab; font-weight:bold;width:12.5%; padding:5px;'><strong style='font-size:small; color:White;'>" + Convert.ToDateTime(Fromdate).AddDays(+4).ToString("ddd - MM/dd") + "</strong></td>");
            strdayappt = strdayappt.Append("</tr>");
            strdayappt = strdayappt.Append("</table>");
            strdayappt =
               strdayappt.Append(
                   "<table id='tblAppointments1' cellspacing='0' cellpadding='0' width='100%' >");

            strdayappt = strdayappt.Append("<tr id='trAppointments' style='background: white'>");
            for (int i = 1; i <= 5; i++)
            {
                strdayappt = strdayappt.Append("<td valign='top' style='background: white'><table cellspacing='1' cellpadding='0' width='100%' style='background-color:#d0e4f4'>");
                string j = null;
                if (i == 1)
                {
                    j = Convert.ToString(day1);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                else if (i == 2)
                {
                    j = Convert.ToString(day2);
                    backcolorTr = "#FFFF99";
                    colorfont = "#044F96";
                }
                else if (i == 3)
                {
                    j = Convert.ToString(day3);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                else if (i == 4)
                {
                    j = Convert.ToString(day4);
                    backcolorTr = "#FFFF99";
                    colorfont = "#044F96";
                }
                else if (i == 5)
                {
                    j = Convert.ToString(day5);
                    backcolorTr = null;
                    colorfont = "#044F96";
                }
                string strQuery = "wday = '" + j + "'";
                DataRow[] drFilterRows = dsAppointmentsList.Tables[0].Select(strQuery);
                DataSet dsAppointmentsListResults = dsAppointmentsList.Clone();
                foreach (DataRow dr in drFilterRows)
                    dsAppointmentsListResults.Tables[0].ImportRow(dr);

                foreach (DataRow drappt in dsAppointmentsListResults.Tables[0].Rows)
                {
                    string timperiod = string.Empty;
                    timperiod = Convert.ToString(drappt["AppointmentTime"]);
                    string strdate = Convert.ToDateTime(drappt["Appointmentdate"]).ToShortDateString();

                    strdayappt =
                   strdayappt.Append("<tr ><td id='tcIntervel' style='background-color: " + backcolorTr + ";width: 13%; padding:10px; " +
                                     "' align='center' valign='top'> <a class='Mytestadd' style='color:" + colorfont + ";text-decoration:none;font-weight:bold; font-size:small;' href='#'  title='Click here to add the Appointment'>" + drappt["AppointmentTime"] +
                                     "</a></br></td></tr>");
                }
                strdayappt = strdayappt.Append("</table></td>");

            }


            strdayappt = strdayappt.Append("</tr>");

            strdayappt = strdayappt.Append("</table>");
            ViewBag.daysch = Convert.ToString(strdayappt);
            return View();
        }
        public void FillYearsInPractice()
        {
            string[] years = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50" };
            var yearlist = new ComboBoxItemList(years);
            var yearitems = new List<ComboBoxItem>();
            if (ViewBag.Yearinpractice != null & ViewBag.Yearinpractice != "")
            {
                string years1 = ViewBag.Yearinpractice;


                foreach (ComboBoxItem item in yearlist)
                {
                    if (item.Text == years1.Trim())
                    {
                        item.Selected = true;

                    }
                    yearitems.Add(item);
                }
                ViewData["cboyears"] = yearitems;
            }
            else
            {
                ViewData["cboyears"] = yearlist;
            }
        }
        public bool Insert_AdvSearchSetting()
        {
            
            Provider_AdvancedSearch ObjIns = new Provider_AdvancedSearch();
            ObjIns.Provider_ID = Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"]));

            if (Request["txtDescription"] != null & Request["txtDescription"] != "")
            {
                ObjIns.Paragraph = Request["txtDescription"];
            }
            else
            {
                ObjIns.Paragraph = null;
            }
            if (Request["txtPayment"] != null & Request["txtPayment"] != "")
            {
                ObjIns.Payments = Request["txtPayment"].Trim();
            }
            else
            {
                ObjIns.Payments = null;
            }


            if (!string.IsNullOrEmpty(Request["txtFromFee"].Trim()) | !string.IsNullOrEmpty(Request["txtToFee"].Trim()))
            {
                ObjIns.DisplayFee_ind = "Y";
            }
            else
            {
                ObjIns.DisplayFee_ind = "N";
            }

            ObjIns.FeeRange_From = Request["txtFromFee"].Trim() ?? null;
            ObjIns.FeeRange_To = Request["txtToFee"].Trim() ?? null;

            if (Request["rdfreesessionYes"] == "True" | Request["rdfreesessionYes"] == "False" & Request["rdfreesessionNo"] == null)
            {
                ObjIns.freesession = "Y";

            }
            if (Request["rdfreesessionNo"] == "True" | Request["rdfreesessionNo"] == "False" & Request["rdfreesessionYes"] == null)
            {
                ObjIns.freesession = "N";

            }
            ObjIns.Yearinpractice = !string.IsNullOrEmpty(Request["cboyears_SelectedText"]) ? Request["cboyears_SelectedText"] : null;

            if (Request["rdocontactyes"] == "True" | Request["rdocontactyes"] == "False" & Request["rdocontactno"] == null)
            {
                ObjIns.Is_Contact_Info = "Y";

            }
            if (Request["rdocontactno"] == "True" | Request["rdocontactno"] == "False" & Request["rdocontactyes"] == null)
            {
                ObjIns.Is_Contact_Info = "N";

            }
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                if (Request["rdtscheduleYes"] == "True" | Request["rdtscheduleYes"] == "False" & Request["rdtscheduleNo"] == null)
                {
                    ObjIns.showschedule = "Y";

                }
                if (Request["rdtscheduleNo"] == "True" | Request["rdtscheduleNo"] == "False" & Request["rdtscheduleYes"] == null)
                {
                    ObjIns.showschedule = "N";

                }
            }
            else
            {
                ObjIns.showschedule = "N";
            }
            if (Request["txtWebsite"] != null & Request["txtWebsite"] != "")
            {
                ObjIns.Website = Request["txtWebsite"].Trim();
            }
            else
            {
                ObjIns.Website = null;
            }
            if (Request["txtfacebookurl"] != null & Request["txtfacebookurl"] != "")
            {
                ObjIns.facebookurl = Request["txtfacebookurl"].Trim();
            }
            else
            {
                ObjIns.facebookurl = null;
            }
            if (Request["txttwitterurl"] != null & Request["txttwitterurl"] != "")
            {
                ObjIns.twitterurl = Request["txttwitterurl"].Trim();
            }
            else
            {
                ObjIns.twitterurl = null;
            }
            if (Request["rdfbcommentsYes"] == "True" | Request["rdfbcommentsYes"] == "False" & Request["rdfbcommentsNo"] == null)
            {
                ObjIns.fbcomments = "Y";

            }
            if (Request["rdfbcommentsNo"] == "True" | Request["rdfbcommentsNo"] == "False" & Request["rdfbcommentsYes"] == null)
            {
                ObjIns.fbcomments = "N";

            }
            ObjIns.UpdatedBy = Convert.ToString(Session["UserID"]);
            Provider_AdvancedSearch.Insert_AdvancedSearchInfo(ObjIns);

            return true;

        }
        [HttpPost()]
        public ActionResult EditElectricianAdvancedSearch(Provider_AdvancedSearch obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Insert_AdvSearchSetting();
            return RedirectToAction("ViewElectricianAdvancedSearch");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Addvideo()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        public JsonResult Addvideo(FeaturesList obj)
        {
            
            if (ModelState.IsValid)
            {
                var objData = new FeaturesList
                {
                    Provider_ID =
                        Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"]))
                };

                if (Convert.ToString(Session["RoleId"]) == "1" & Convert.ToString(Session["RoleId"]) == "13" & Convert.ToString(Session["RoleId"]) == "14" & Convert.ToString(Session["RoleId"]) == "15" & Convert.ToString(Session["RoleId"]) == "16")
                {
                    if (Session["Prov_Loginid"] != null)
                    {
                        objData.ProviderLogin_ID = Convert.ToInt32(Session["Prov_Loginid"]);
                    }
                    else
                    {

                        objData.ProviderLogin_ID = null;
                    }
                }
                if (Convert.ToString(Session["RoleId"]) == "4")
                {
                    if (Session["userid"] != null)
                    {
                        objData.ProviderLogin_ID = Convert.ToInt32(Session["userid"]);
                    }
                    else
                    {

                        objData.ProviderLogin_ID = null;
                    }
                }
                if (Session["PracticeID"] != null)
                {
                    objData.Practice_ID = Convert.ToInt32(Session["Prov_ID"]);
                }
                else
                {
                    objData.Practice_ID = null;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(Request["txtYTitle"])))// != null & Request["txtYTitle"] != "")
                {
                    objData.Title = Request["txtYTitle"].ToString();
                }
                else
                {
                    objData.Title = null;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDecription"])))// != null & Request["txtDecription"] != "")
                {
                    objData.Description = Request["txtDecription"].ToString();
                }
                else
                {
                    objData.Description = null;
                }
               if (!string.IsNullOrEmpty(Convert.ToString(Request["txtYouTubetxt"])))// != null & Request["txtYouTubetxt"] != "")
                {
                    objData.Embed_Text = Request["txtYouTubetxt"].ToString();
                }
                else
                {
                    objData.Embed_Text = null;
                }
                if (obj.Insert_Provider_Greetings(objData) == true)
                {
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(JsonResponseFactory.ErrorResponse(null), JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
            }

        }
        public ActionResult Delete(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            FeaturesList obj = new FeaturesList();
            obj.ApplyStatus(id, Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])), "D");
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        public ActionResult StatusChange(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            FeaturesList obj = new FeaturesList();
            if (Convert.ToString(Request["status"]) == "Active")
            {
                obj.ApplyStatus(id, Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])), "I");
            }
            else if (Convert.ToString(Request["status"]) == "InActive")
            {
                obj.ApplyStatus(id, Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])), "A");
            }
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        public ActionResult Playvideo(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string youtubetext = GetProviderYouTubeEmbeddedText(id);
            ViewBag.youtube = youtubetext;
            return PartialView("Playvideo", youtubetext);
        }
        public string GetProviderYouTubeEmbeddedText(int videoid)
        {
            IDataParameter[] ObjInParam = { new SqlParameter("@In_Provider_Greeting_ID", videoid) };
            ObjCommFun.AddInParameters(ObjInParam);
            SqlDataReader DRinfo = ObjCommFun.GetDataReader("Help_dbo.st_Provider_Youtube_EmbedText");
            if (DRinfo.Read())
            {
                if (DRinfo["Embed_Text"] != null)
                {

                    return DRinfo["Embed_Text"].ToString();

                }
            }
            return null;
        }

        public ActionResult ViewElectricianDocumentInfo()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Session.Add("TopIndex", "1");
            if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
            {

                return RedirectToAction("Schedulespecs", "Schedule");

            }
            else if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("Schedulespecs", "Schedule");
            }
            var objData = new FeaturesList();
            var objdatalist = objData.GetProviderBasedGreetings(Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])), 10, Convert.ToInt32(Request["page"] ?? "1"));
            if (Convert.ToInt32(FeaturesList.TotalNoRecords) > 0)
            {
                ViewBag.videototrec = FeaturesList.TotalNoRecords;
            }
            return View("ViewElectricianDocumentInfo", objdatalist);
        }

        public ActionResult ProviderDocuments()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objDocInfo = new Provider_DocumentInfo
            {
                Practice_ID = Convert.ToInt32(Session["Practice_ID"]),
                Provider_ID =
                    Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])),
                OrderBYItem = Request["sort"] ?? null,
                OrderBy = Request["sortdir"] ?? null,
                PageNo = Convert.ToInt32(Request["S1P1"] ?? "1"),
                NoOfRecords = 10,
                verifieddocument = "N"
            };
            var docList = Provider_DocumentInfo.Get_DocumentInfo(objDocInfo);
            ViewBag.totrec = Provider_DocumentInfo.TotalRecords;
            return View("ProviderDocuments", docList);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddProviderDocument()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddProviderCertificate()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        [HttpPost()]
        public ActionResult AddProviderCertificate(Provider_DocumentInfo obj, HttpPostedFileBase myFile1)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                if (myFile1.ContentLength > 0)
                {
                    string StrTitle = null;
                    string strImgSuffix = null;
                    string StrDesc = null;
                    string FileExtn2 = null;
                    double filesize = 0;
                    double filelength = myFile1.ContentLength;
                    HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(myFile1.FileName);
                    FileExtn2 = System.IO.Path.GetExtension(myFile1.FileName);
                    HomePageFileName = HomePageFileName + FileExtn2;
                    CustomizeFileName = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + FileExtn2;
                    if (Request["rbtdriverlicense"] != "" && Request["rbtdriverlicense"] != null)
                    {
                        StrTitle = "Driver license";
                    }
                    else
                    {
                        StrTitle = "Mower helper license";
                    }
                    //if (Request["txtTitle"] != null & Request["txtTitle"] != "")
                    //{
                    //    StrTitle = Request["txtTitle"].ToString();
                    //}
                   if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription"])))// != null & Request["txtDescription"] != "")
                    {
                        StrDesc = Request["txtDescription"].ToString();
                    }
                    if (filelength != 0)
                    {

                        filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                    }
                    Provider_DocumentInfo.Insert_Attachment(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), StrTitle, StrDesc,
                        CustomizeFileName, null, HomePageFileName, "Y", Convert.ToInt32(Session["userid"]), "Y", ref strImgSuffix, filesize);

                    //flLogo.SaveAs(strFilePath);

                    string filename = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + strImgSuffix + FileExtn2;
                    string strFilePath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), filename);
                    myFile1.SaveAs(strFilePath);
                }

            }
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditDocument(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetAttachmentList(id);
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult EditCertificate(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            GetAttachmentList(id);
            return View();
        }
        [HttpPost()]
        public ActionResult EditCertificate(Provider_DocumentInfo obj, HttpPostedFileBase CertFileUpLoad)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                string StrTitle = null;
                string StrDesc = null;
                string isdisplaypublic = "Y";
                double filesize = 0;
                string FileExtn2 = null;
                int? docid = null;
                string StrImagePath = null;
                if (Request["hdncertid"] != null)
                {
                    docid = Convert.ToInt32(Request["hdncertid"]);
                }
                if (CertFileUpLoad == null)
                {
                    if (Request["rbtdriverlicense"] != "" && Request["rbtdriverlicense"] != null)
                    {
                        StrTitle = "Driver license";
                    }
                    else
                    {
                        StrTitle = "Mower helper license";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription_cert"])))// != null & Request["txtDescription_cert"] != "")
                    {
                        StrDesc = Request["txtDescription_cert"].ToString();
                    }

                    Provider_DocumentInfo.Update_Attachment(docid, StrTitle, StrDesc, CustomizeFileName, null, HomePageFileName, Convert.ToInt32(Session["userid"]), "Y", filesize, isdisplaypublic);
                }
                else
                {
                    if (CertFileUpLoad.ContentLength > 0)
                    {

                        double filelength = CertFileUpLoad.ContentLength;

                        HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(CertFileUpLoad.FileName);
                        FileExtn2 = System.IO.Path.GetExtension(CertFileUpLoad.FileName);
                        HomePageFileName = HomePageFileName + FileExtn2;
                        CustomizeFileName = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + FileExtn2;
                        if (Request["rbtdriverlicense"] != "" && Request["rbtdriverlicense"] != null)
                        {
                            StrTitle = "Driver license";
                        }
                        else
                        {
                            StrTitle = "Mower helper license";
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription_cert"])))// != null & Request["txtDescription_cert"] != "")
                        {
                            StrDesc = Request["txtDescription_cert"].ToString();
                        }
                        if (filelength != 0)
                        {

                            filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                        }
                        Provider_DocumentInfo.Update_Attachment(docid, StrTitle, StrDesc, CustomizeFileName, null, HomePageFileName, Convert.ToInt32(Session["userid"]), "Y", filesize, isdisplaypublic);

                        if (!string.IsNullOrEmpty(Request["hdnstrpath"]))
                        {
                            string[] strPathup = Request["hdnstrpath"].Split('.');
                            string strFinalPath = strPathup[0] + Request["hdnPathSuffix"] + "." + strPathup[1];
                            StrImagePath = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), strFinalPath);
                            if (!string.IsNullOrEmpty(StrImagePath))
                            {
                                System.IO.FileInfo File = new System.IO.FileInfo(StrImagePath);
                                if (File.Exists)
                                {
                                    File.Attributes = System.IO.FileAttributes.Normal;
                                    File.Delete();
                                }
                            }
                        }

                        string filename = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + Request["hdnPathSuffix"] + FileExtn2;
                        string strFilePath1 = Path.Combine(Server.MapPath("~/Attachments/VerifiedDocuments"), filename);
                        CertFileUpLoad.SaveAs(strFilePath1);
                    }

                }

            }
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        private void GetAttachmentList(Int32 IntProvDocID)
        {
            try
            {
                Provider_DocumentInfo obj = new Provider_DocumentInfo();
                obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), IntProvDocID);
                if ((obj != null))
                {
                    ViewBag.docid = IntProvDocID;
                    if (!string.IsNullOrEmpty(obj.Title))
                    {
                        ViewBag.txtTitle = obj.Title;
                    }
                    else
                    {
                        ViewBag.txtTitle = null;
                    }

                    if (!string.IsNullOrEmpty(obj.DocDescription))
                    {
                        ViewBag.txtDescription = obj.DocDescription;
                    }
                    else
                    {
                        ViewBag.txtDescription = null;
                    }
                    string StrTempFilePath = "";
                    if (!string.IsNullOrEmpty(obj.FileName))
                    {
                        ViewBag.hdnPaTH = obj.FileName;
                        StrTempFilePath = obj.FileName;
                        ViewBag.FileName = StrTempFilePath;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(obj.Path))
                        {
                            ViewBag.hdnPaTH = obj.Path;
                            StrTempFilePath = obj.Path;
                            ViewBag.FileName = StrTempFilePath;
                        }

                        ViewBag.hypLnkAttachedFileName = "";
                    }
                    if (obj.Displayinpublic != null)
                    {
                        ViewBag.Displayinpublic = obj.Displayinpublic;
                    }
                    if (obj.Path_Suffix != null)
                    {
                        ViewBag.Path_Suffix = obj.Path_Suffix;
                    }
                    if (!string.IsNullOrEmpty(obj.Path))
                    {
                        string strPath = obj.Path;
                        ViewData["strpath"] = strPath;
                        ViewBag.strpath = strPath;
                        string[] strExtension = strPath.Split('.');
                        ViewData["Extn"] = strExtension[1];
                    }

                    CustomizeFileName = "PROVDoc" + (Session["Provider_ID"] ?? Session["Prov_ID"]) + "." + ViewData["Extn"];
                    HomePageFileName = ViewBag.FileName;
                }
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "DirectoryController", "GetAttachmentList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                clsCustomEx = null;
            }
        }
        [HttpPost()]
        public ActionResult EditDocument(Provider_DocumentInfo obj, HttpPostedFileBase FileUpLoad)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                string StrTitle = null;
                string StrDesc = null;
                string isdisplaypublic = null;
                double filesize = 0;
                string FileExtn2 = null;
                int? docid = null;
                string StrImagePath = null;
                if (Request["hdndocid"] != null)
                {
                    docid = Convert.ToInt32(Request["hdndocid"]);
                }
                if (FileUpLoad == null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["txtTitle1"])))// != null & Request["txtTitle1"] != "")
                    {
                        StrTitle = Request["txtTitle1"].ToString();
                    }
                    if (Request["rbtyes"] != null & Request["rbtyes"] == "Yes")
                    {
                        isdisplaypublic = "Y";
                    }
                    if (Request["rbtno"] != null & Request["rbtno"] == "No")
                    {
                        isdisplaypublic = "N";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription1"])))// != null & Request["txtDescription1"] != "")
                    {
                        StrDesc = Request["txtDescription1"].ToString();
                    }

                    Provider_DocumentInfo.Update_Attachment(docid, StrTitle, StrDesc, CustomizeFileName, null, HomePageFileName, Convert.ToInt32(Session["userid"]), "N", filesize, isdisplaypublic);
                }
                else
                {
                    if (FileUpLoad.ContentLength > 0)
                    {

                        double filelength = FileUpLoad.ContentLength;

                        HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(FileUpLoad.FileName);
                        FileExtn2 = System.IO.Path.GetExtension(FileUpLoad.FileName);
                        HomePageFileName = HomePageFileName + FileExtn2;
                        CustomizeFileName = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + FileExtn2;
                        if (!string.IsNullOrEmpty(Convert.ToString(Request["txtTitle1"])))// != null & Request["txtTitle1"] != "")
                        {
                            StrTitle = Request["txtTitle1"].ToString();
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(Request["rbtyes"])))// != null & Request["rbtyes"] == "Yes")
                        {
                            isdisplaypublic = "Y";
                        }
                        if (Request["rbtno"] != null & Request["rbtno"] == "No")
                        {
                            isdisplaypublic = "N";
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription1"])))// != null & Request["txtDescription1"] != "")
                        {
                            StrDesc = Request["txtDescription1"].ToString();
                        }
                        if (filelength != 0)
                        {

                            filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                        }
                        Provider_DocumentInfo.Update_Attachment(docid, StrTitle, StrDesc, CustomizeFileName, null, HomePageFileName, Convert.ToInt32(Session["userid"]), "N", filesize, isdisplaypublic);

                        if (!string.IsNullOrEmpty(Request["hdnstrpath"]))
                        {
                            string[] strPathup = Request["hdnstrpath"].Split('.');
                            string strFinalPath = strPathup[0] + Request["hdnPathSuffix"] + "." + strPathup[1];
                            StrImagePath = Path.Combine(Server.MapPath("~/Attachments/Documents"), strFinalPath);
                            if (!string.IsNullOrEmpty(StrImagePath))
                            {
                                System.IO.FileInfo File = new System.IO.FileInfo(StrImagePath);
                                if (File.Exists)
                                {
                                    File.Attributes = System.IO.FileAttributes.Normal;
                                    File.Delete();
                                }
                            }
                        }
                        string filename = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + Request["hdnPathSuffix"] + FileExtn2;
                        string strFilePath1 = Path.Combine(Server.MapPath("~/Attachments/Documents"), filename);
                        FileUpLoad.SaveAs(strFilePath1);
                    }
                }
            }
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        [HttpPost()]
        public ActionResult AddProviderDocument(Provider_DocumentInfo obj, HttpPostedFileBase myFile1)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (ModelState.IsValid)
            {
                if (myFile1.ContentLength > 0)
                {
                    string StrTitle = null;
                    string strImgSuffix = null;
                    string StrDesc = null;
                    string FileExtn2 = null;
                    double filesize = 0;
                    double filelength = myFile1.ContentLength;
                    string isdisplaypublic = null;
                    HomePageFileName = System.IO.Path.GetFileNameWithoutExtension(myFile1.FileName);
                    FileExtn2 = System.IO.Path.GetExtension(myFile1.FileName);
                    HomePageFileName = HomePageFileName + FileExtn2;
                    CustomizeFileName = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + FileExtn2;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["txtTitle_AddDoc"])))// != null & Request["txtTitle_AddDoc"] != "")
                    {
                        StrTitle = Request["txtTitle_AddDoc"].ToString();
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["rbtyes_AddDoc"])))// != null & Request["rbtyes_AddDoc"] == "Yes")
                    {
                        isdisplaypublic = "Y";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["rbtno_AddDoc"])))// != null & Request["rbtno_AddDoc"] == "No")
                    {
                        isdisplaypublic = "N";
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(Request["txtDescription_AddDoc"])))// != null & Request["txtDescription_AddDoc"] != "")
                    {
                        StrDesc = Request["txtDescription_AddDoc"].ToString();
                    }
                    if (filelength != 0)
                    {

                        filesize = Convert.ToDouble(string.Format("{0:0.00}", (Convert.ToDecimal(filelength / 1024))));
                    }
                    Provider_DocumentInfo.Insert_Attachment(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), StrTitle, StrDesc,
                        CustomizeFileName, null, HomePageFileName, isdisplaypublic, Convert.ToInt32(Session["userid"]), "N", ref strImgSuffix, filesize);

                    string filename = "PROVDoc" + Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])) + strImgSuffix + FileExtn2;
                    string strFilePath = Path.Combine(Server.MapPath("~/Attachments/Documents"), filename);
                    myFile1.SaveAs(strFilePath);
                }

            }
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        public ActionResult ProviderCertificates()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Provider_DocumentInfo ObjDocInfo = new Provider_DocumentInfo();
            //ObjDocInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            ObjDocInfo.Provider_ID = Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"]));
            ObjDocInfo.OrderBYItem = Request["sort"] ?? null;
            ObjDocInfo.OrderBy = Request["sortdir"] ?? null;
            ObjDocInfo.PageNo = Convert.ToInt32(Request["S2P2"] ?? "1");
            ObjDocInfo.NoOfRecords = 10;
            ObjDocInfo.verifieddocument = "Y";
            List<Provider_DocumentInfo> DocList = new List<Provider_DocumentInfo>();
            DocList = Provider_DocumentInfo.Get_DocumentInfo(ObjDocInfo);
            ViewBag.totrec = Provider_DocumentInfo.TotalRecords;
            return View("ProviderCertificates", DocList);

        }
        public ActionResult DeleteDocument(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string filename = null;
            string extn = null;
            string patencrypt = null;
            string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\Documents" + "\\";
            Provider_DocumentInfo obj = new Provider_DocumentInfo();
            obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), id);
            if ((obj != null))
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + obj.Path);
                extn = System.IO.Path.GetExtension(strFilePath + obj.Path);
                patencrypt = obj.Path_Suffix;
            }
            string fullfilepath = filename + patencrypt + extn;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                File.Attributes = System.IO.FileAttributes.Normal;
                File.Delete();
            }
            Provider_DocumentInfo.Del_DocumentInfo(id, id);
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        public ActionResult DeleteCertificate(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string filename = null;
            string extn = null;
            string patencrypt = null;
            string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\VerifiedDocuments" + "\\";
            Provider_DocumentInfo obj = new Provider_DocumentInfo();
            obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), id);
            if ((obj != null))
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + obj.Path);
                extn = System.IO.Path.GetExtension(strFilePath + obj.Path);
                patencrypt = obj.Path_Suffix;
            }
            string fullfilepath = filename + patencrypt + extn;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                File.Attributes = System.IO.FileAttributes.Normal;
                File.Delete();
            }
            Provider_DocumentInfo.Del_DocumentInfo(id, id);
            return RedirectToAction("ViewElectricianDocumentInfo");
        }
        public ContentResult Download(int id)
        {
            
            string filename = null;
            string extn = null;
            string patencrypt = null;
            string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\Documents" + "\\";
            Provider_DocumentInfo obj = new Provider_DocumentInfo();
            obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), id);
            if ((obj != null))
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + obj.Path);
                extn = System.IO.Path.GetExtension(strFilePath + obj.Path);
                patencrypt = obj.Path_Suffix;
            }
            string fullfilepath = filename + patencrypt + extn;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                Response.ContentType = extn;
                Response.WriteFile(File.FullName);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                Response.AddHeader("Content-Length", "0");
                Response.ContentType = extn;
                Response.Write("file not found.");
                Response.End();
            }
            return null;
        }
        public ActionResult CertificateDownload(int id)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string filename = null;
            string extn = null;
            string patencrypt = null;
            string strFilePath = HttpContext.Server.MapPath("../../") + "Attachments\\VerifiedDocuments" + "\\";
            Provider_DocumentInfo obj = new Provider_DocumentInfo();
            obj = Provider_DocumentInfo.GetAttachmentList(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])), id);
            if ((obj != null))
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + obj.Path);
                extn = System.IO.Path.GetExtension(strFilePath + obj.Path);
                patencrypt = obj.Path_Suffix;
            }
            string fullfilepath = filename + patencrypt + extn;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                Response.ContentType = extn;
                Response.WriteFile(File.FullName);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "Attachment;filename=" + obj.FileName);
                Response.AddHeader("Content-Length", "0");
                Response.ContentType = extn;
                Response.Write("file not found.");
                Response.End();
            }
            return null;
        }
        public ActionResult Sitestatistics(string year)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            FillYears();
            Provider_Sitestastics objStatastics = new Provider_Sitestastics();

            List<Provider_Sitestastics> objGetStatastics = new List<Provider_Sitestastics>();
            objStatastics.Provider_ID = Convert.ToInt32(Session["Provider_ID"] == null ? Session["Prov_ID"] : Session["Provider_ID"]);
            if (year == null & Request["Years"] == null)
            {
                objStatastics.Year = DateTime.Now.Year.ToString();
            }
            else
            {
                objStatastics.Year = Request["Years"];
                ViewBag.year = Request["Years"];
            }
            objGetStatastics = Provider_Sitestastics.GET_Statistics(objStatastics);

            return View("Sitestatistics", objGetStatastics);
        }
        public void FillYears(string year = null)
        {

            try
            {
                Provider_Sitestastics objData = new Provider_Sitestastics();
                IList<SelectListItem> objlist = new List<SelectListItem>();
                objData = Provider_Sitestastics.Get_StartYearOfProvider(Convert.ToInt32((Session["Provider_ID"] ?? Session["Prov_ID"])));

                if ((objData != null))
                {
                    DateTime strYear = default(DateTime);
                    if (objData.CreatedOn != null)
                    {
                        if (!string.IsNullOrEmpty(objData.CreatedOn))
                        {
                            strYear = Convert.ToDateTime(objData.CreatedOn);
                        }
                    }
                    int startintYear = 0;
                    if (strYear == null)
                    {
                        startintYear = DateTime.Now.Year;
                        int NowYear = DateTime.Now.Year;
                        while ((startintYear <= NowYear))
                        {

                            objlist.Add(new SelectListItem { Text = NowYear.ToString(), Value = NowYear.ToString() });

                            NowYear = NowYear - 1;
                        }
                    }
                    else
                    {
                        startintYear = strYear.Year;
                        int NowYear = DateTime.Now.Year;
                        while ((startintYear <= NowYear))
                        {
                            objlist.Add(new SelectListItem { Text = NowYear.ToString(), Value = NowYear.ToString() });
                            NowYear = NowYear - 1;
                        }
                    }
                    Provider_Sitestastics.Years = objlist;
                }

            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "DirectoryController", "FillYears", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                clsCustomEx = null;
            }
        }
        public ActionResult Showfbhelp()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return View();
        }
        public ContentResult electricianslicDownload(string certname, string certname1)
        {
            string filename = null;
            string extn = null;
            string strFilePath = HttpContext.Server.MapPath("~/") + "Attachments\\VerifiedDocuments" + "\\";
            filename = System.IO.Path.GetFileNameWithoutExtension(strFilePath + certname);
            extn = System.IO.Path.GetExtension(strFilePath + certname);
            string fullfilepath = certname;
            strFilePath = strFilePath + "/" + fullfilepath;
            System.IO.FileInfo File = new System.IO.FileInfo(strFilePath);
            if (File.Exists)
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + certname1);
                Response.AddHeader("Content-Length", Convert.ToString(File.Length));
                Response.ContentType = extn;
                Response.WriteFile(File.FullName);
                Response.End();
            }
            else
            {

            }
            return null;
        }
        public ActionResult ProviderVideos()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            var objData = new FeaturesList();
            var objdatalist = objData.GetProviderBasedGreetings(Convert.ToInt32(((Session["Provider_ID"] == null) ? Session["Prov_Id"] : Session["Provider_Id"])), 10, Convert.ToInt32(Request["page"] ?? "1"));
            if (Convert.ToInt32(FeaturesList.TotalNoRecords) > 0)
            {
                ViewBag.videototrec = FeaturesList.TotalNoRecords;
            }
            return View("ProviderVideos", objdatalist);
        }
    }
}
