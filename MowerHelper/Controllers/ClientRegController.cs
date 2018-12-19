using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class ClientRegController : Controller
    {

        System.Collections.Hashtable obj = new System.Collections.Hashtable();
      
        [AllowAnonymous]
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ClientInformation(string id, string practiceid)
        {
            //if (Request.IsAjaxRequest() && Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
              Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
              objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
              //Reg_ProvidersDetailInfo objPass = new Reg_ProvidersDetailInfo();
            //if ((objInfo != null))
            //{
              
            //    if (!string.IsNullOrEmpty(objInfo.Zip))
            //    {
            //        //ViewBag.Zip = objInfo.Zip;
            //        objPass.Zip = objInfo.Zip;
            //    }
            //    //else
            //    //{
            //    //    ViewBag.Zip = null;
            //    //}


            //    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.State_ID)))
            //    {
            //        //ViewBag.State_ID = objInfo.State_ID;
            //        objPass.State_ID = objInfo.State_ID;
            //    }
            //    //else
            //    //{
            //    //    ViewBag.State_ID = null;
            //    //}
            //    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.City_ID)))
            //    {
            //        //ViewBag.City_ID = objInfo.City_ID;
            //        objPass.City_ID = objInfo.City_ID;
            //    }
            //    //else
            //    //{
            //    //    ViewBag.City_ID = null;
            //    //}
            //    if (!string.IsNullOrEmpty(objInfo.Statename))
            //    {
            //        //ViewBag.Statename = objInfo.Statename;
            //        objPass.Statename = objInfo.Statename;
            //    }
            //    //else
            //    //{
            //    //    ViewBag.Statename = null;
            //    //}
            //    if (!string.IsNullOrEmpty(objInfo.Cityname))
            //    {
            //        //ViewBag.Cityname = objInfo.Cityname;
            //        objPass.Cityname = objInfo.Cityname;
            //    }
            //    //else
            //    //{
            //    //    ViewBag.Cityname = null;
            //    //}
            //}
            Session.Add("TopIndex", 0);
            if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
            {

                return RedirectToAction("Schedulespecs", "Schedule");

            }
            else if (Convert.ToString(Session["Msgitem"]) != "5")
            {
                return RedirectToAction("Schedulespecs", "Schedule");
            }
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

            return View("ClientInformation", objInfo);
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public JsonResult ClientInformation(PatientRegistration obj)
        {
            
          //  string strpwd = null;
          //  string username = null;
           // string outmesg = null;
            clsCommonFunctions objCommon = new clsCommonFunctions();
            //if (!string.IsNullOrEmpty(Request["txtEmailID"]))
            //{
            //    outmesg = objCommon.EmailExistsFunction(Request["txtEmailID"]);
            //}
            //if (outmesg != null & outmesg != "")
            //{
            //    return Json(JsonResponseFactory.ErrorResponse("Enter email is already exists"), JsonRequestBehavior.DenyGet);
            //}
            //else
            //{
            PatientRegistration PatInfo = new PatientRegistration();
                try
                {
                    
                    if (string.IsNullOrEmpty(Request["txtHomePhone1"]) && string.IsNullOrEmpty(Request["txtHomePhone2"]) && string.IsNullOrEmpty(Request["txtHomePhone3"]))
                    {
                        PatInfo.HomePhone = null;
                    }
                    else
                    {
                        PatInfo.HomePhone = Request["txtHomePhone1"].Trim() + Request["txtHomePhone2"].Trim() + Request["txtHomePhone3"].Trim();
                    }

                    if (string.IsNullOrEmpty(Request["txtWorkPhone1"]) && string.IsNullOrEmpty(Request["txtWorkPhone2"]) && string.IsNullOrEmpty(Request["txtWorkPhone3"]))
                    {
                        PatInfo.WPhone = null;
                    }
                    else
                    {
                        PatInfo.WPhone = Request["txtWorkPhone1"].Trim() + Request["txtWorkPhone2"].Trim() + Request["txtWorkPhone3"].Trim();
                    }

                    if (string.IsNullOrEmpty(Request["txtMobile1"]) && string.IsNullOrEmpty(Request["txtMobile2"]) && string.IsNullOrEmpty(Request["txtMobile3"]))
                    {
                        PatInfo.MPhone = null;
                    }
                    else
                    {
                        PatInfo.MPhone = Request["txtMobile1"] + Request["txtMobile2"] + Request["txtMobile3"];
                    }
                    if (Request["txtMiddleName"] != null && Request["txtMiddleName"] != "" && Request["txtMiddleName"] != "MI")
                    {
                        PatInfo.MI = Request["txtMiddleName"];
                    }
                    else
                    {
                        PatInfo.MI = null;
                    }
                    //PatInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                    PatInfo.Provider_ID = Convert.ToInt32(Session["Prov_ID"]);
                    PatInfo.Prefix = (Request["ddlPrefix"].Trim() == "" ? null : Request["ddlPrefix"]);
                    PatInfo.FirstName = (!string.IsNullOrEmpty(Convert.ToString(Request["txtFirstName"]).Trim())  ? Request["txtFirstName"].Trim() : null);
                    PatInfo.LastName = (!string.IsNullOrEmpty(Request["txtLastName"].Trim()) ? Request["txtLastName"].Trim() : null);
                    PatInfo.Suffix = (Request["ddlSuffix"].Trim() == "" ? null : Request["ddlSuffix"]);
                    PatInfo.PatientEmail = (Request["txtEmailID"] != "" ? Request["txtEmailID"] : null);
                    PatInfo.DrivingLicenceNo = (!string.IsNullOrEmpty(Request["txtLicense"]) ? Request["txtLicense"] : null);
                    PatInfo.Address1 = (!string.IsNullOrEmpty(Request["txtAddress1"]) ? Request["txtAddress1"] : null);
                    PatInfo.Address2 = (!string.IsNullOrEmpty(Request["txtAddress2"]) ? Request["txtAddress2"] : null);
                    PatInfo.City_ID = (Convert.ToInt32(Request["DDCity"]) != 0 ? Convert.ToInt32(Request["DDCity"]) : 0);
                    PatInfo.State_ID = (Convert.ToInt32(Request["DDState"]) != 0 ? Convert.ToInt32(Request["DDState"]) : 0);
                    PatInfo.Country_ID = 1;
                    PatInfo.ZIP = (!string.IsNullOrEmpty(Request["txtZip"]) ? Request["txtZip"] : null);
                    PatInfo.UserId = Convert.ToInt32(Session["UserID"]);
                    string gMapAddress = PatInfo.Address1 + " " + Request["hdnSelectState"] + " " + Request["hdnSelectCity"] + " " + Request["txtZip"];
                   
                    try
                    {
                        string url = "https://maps.google.com/maps/api/geocode/xml?address=" + gMapAddress + "&key=AIzaSyCgKM4MCuYrILJ8CFPR-a5xXelNuobdK50";
                        WebRequest request = WebRequest.Create(url);
                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            DataSet dsResult = new DataSet();
                            dsResult.ReadXml(reader);
                            DataRow location=null;                       
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + Convert.ToString(row["result_id"]))[0]["geometry_id"].ToString();
                                 location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                //dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);                             
                            }
                            PatInfo.GmapLatitude = Convert.ToString(location["lat"]);
                            PatInfo.GmapLongitude = Convert.ToString(location["lng"]);
                        }
                    }
                    }
                    catch (Exception e)
                    {                      
                         PatInfo.GmapLatitude = null;
                         PatInfo.GmapLongitude = null;
                    }
                    string Out_Msg = null;
                    string Out_login_id = null;
                    PatInfo.Ind = !string.IsNullOrEmpty(Request["HdnInd"])?Request["HdnInd"]:"N";
                    MowerHelper.Models.BLL.Patients.PatientRegistration.Insert_Patinet_FT(PatInfo, ref Out_Msg, ref Out_login_id);
                    if (!string.IsNullOrEmpty(Out_Msg) && Out_Msg.Contains("Do you want to create"))
                    {
                        return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    //PatInfo.GmapLatitude = null;
                   // PatInfo.GmapLongitude = null;
                    clsExceptionLog clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "ClientRegController", "ClientInformation", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    clsCustomEx = null;
                }


            //}


            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);

        }

        public ActionResult Conformation()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            return RedirectToAction("ClientsHome", "Clients", null);
        }
  
    }
}
