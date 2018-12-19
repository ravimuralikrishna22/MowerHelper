using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obout.Mvc.ComboBox;
using PayPal.Api;
using PayPal.Tenniscoach;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.ProviderRegistration;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{

    public class ScheduleController : Controller
    {

        string[] timeapplist = { "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" };
        string stroutmsg = null;
        DataSet dsAppointmentsList = new DataSet();
        DataSet drAppoiontments = new DataSet();
        DataSet dsTodayTask = new DataSet();
        clsCommonFunctions objCommon = new clsCommonFunctions();
        // int intCounter;
        //int StartingYear = 2014;
        //string objresponse;
        string objstorecreditcardresponse;
        private void GetDayAppointments(string ShowAll, string apptdate = null)
        {
            try
            {
                string provideid = null;
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    if (Session["Prov_ID"] != null)
                    {
                        provideid = Convert.ToString(Session["Prov_ID"]);

                    }
                    else
                    {
                        provideid = Convert.ToString(Session["ProviderID"]);
                    }
                }
                else
                {
                    provideid = Convert.ToString(Session["ComboProv_ID"]);
                }
                int weekdays = (int)DateTime.Parse(apptdate).DayOfWeek + 1;
                dsAppointmentsList = GetAppointment.GetDayTemplate(provideid, apptdate, ShowAll, Convert.ToString(weekdays));
                //ViewBag.AppointmentsList = null;
                //ViewBag.AppointmentsList = dsAppointmentsList.Tables[0].AsEnumerable().GetEnumerator();


            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetDayAppointments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void Fillallappointments(string apptdate = null, string week = null, string tech_id = null)
        {
            try
            {
                //if (tech_id != null && tech_id != "")
                //{
                //    tech_id = "," + tech_id + ",";
                //}
                int ProviderID;
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    ProviderID = Convert.ToInt32(Session["ComboProv_ID"]);
                }
                else
                {
                    if (Session["Prov_ID"] != null)
                    {
                        ProviderID = Convert.ToInt32(Session["Prov_ID"]);

                    }
                    else
                    {
                        ProviderID = Convert.ToInt32(Session["ProviderID"]);
                    }
                }
                DateTime startdate = Convert.ToDateTime(apptdate);
                startdate = new DateTime(startdate.Year, startdate.Month, 1);
                DataSet dsset = null;
                IDataParameter[] objparam = {
            new SqlParameter("@in_CurrentDate", startdate),
			new SqlParameter("@In_Provider_ID", ProviderID),
            new SqlParameter("@in_week", week),
             new SqlParameter("@in_technician_id", null)    

		};
                objCommon.AddInParameters(objparam);
                dsset = objCommon.GetDataSet("Help_dbo.st_Schedule_LIST_FlyoutAppointments");
                if (dsset.Tables.Count > 0)
                {
                    if (dsset.Tables[0].Rows.Count > 0)
                    {
                        ViewData["highlightapptdate"] = Convert.ToString(dsset.Tables[0].Rows[0]["appointments"]);
                    }
                    if (dsset.Tables[1].Rows.Count > 0)
                    {
                        ViewData["highlightdates"] = Convert.ToString(dsset.Tables[1].Rows[0]["appointment_dates"]);
                    }
                }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "Fillallappointments", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void GetView(string ShowAll, string apptdate = null, string tech_id = null)
        {
            try
            {
                //if (tech_id != null && tech_id != "")
                //{
                //    tech_id = "," + tech_id + ",";
                //}

                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    drAppoiontments = GetAppointment.GetDayAppointments(Convert.ToString(Session["Prov_ID"]), apptdate, ShowAll, tech_id);
                }
                else
                {
                    drAppoiontments = GetAppointment.GetDayAppointments(Convert.ToString(Session["ComboProv_ID"]), apptdate, ShowAll, tech_id);
                }

                string firstappttime = null;
                if (drAppoiontments.Tables[0].Rows.Count > 0)
                {
                    firstappttime = Convert.ToString(drAppoiontments.Tables[0].Rows[0]["AppointmentTime"]);
                    if (firstappttime.Contains("AM"))
                    {
                        string[] apptime = firstappttime.Split(':');
                        int appt1 = Convert.ToInt32(apptime[0]);
                        if (appt1 < 7)
                        {
                            ViewBag.scrolind = "1";
                        }
                    }
                    if (firstappttime.Contains("PM"))
                    {
                        string[] apptime = firstappttime.Split(':');
                        int appt1 = Convert.ToInt32(apptime[0]);
                        if (appt1 > 7)
                        {
                            ViewBag.scrolind = "2";
                        }
                    }
                }
                //ViewBag.drAppoiontments = null;
                //ViewBag.drAppoiontments = drAppoiontments.Tables[0].AsEnumerable().GetEnumerator();
                //ViewData["apptcount"] = null;
                //ViewData["apptcount"] = drAppoiontments.Tables[0].Rows.Count;
                //for (int i = 0; i <= drAppoiontments.Tables[0].Rows.Count - 1; i++)
                //{
                //    if (ViewData["Status"] != null)
                //    {
                //        ViewData["Status"] = ViewData["Status"] + "," + Convert.ToString(drAppoiontments.Tables[0].Rows[i]["AppointmentStatus_ID"]);
                //    }
                //    else
                //    {
                //        ViewData["Status"] = Convert.ToString(drAppoiontments.Tables[0].Rows[i]["AppointmentStatus_ID"]);
                //    }
                //}
                //if (ViewData["Status"] != null)
                //{
                //    string str = Convert.ToString(ViewData["Status"]);
                //    string[] str1 = str.Split(',');
                //    for (int z = 0; z <= str1.Length - 1; z++)
                //    {
                //        if (str1[z] == "3")
                //        {
                //            ViewData["count"] = Convert.ToUInt32(ViewData["count"]) + 1;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetView", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //public void FillPlaceofServices(int ProvID, int PracticeID)
        //{
        //    try
        //    {
        //        List<clsClaimSessionInfo> obllist = new List<clsClaimSessionInfo>();
        //        clsClaimSessionInfo objInfo = new clsClaimSessionInfo();
        //        if (Session["roleid"].ToString() != "1")
        //        {
        //            obllist = objInfo.GetAllProvidersBasedForClaim(Convert.ToInt32(Session["Prov_ID"]), Convert.ToInt32(Session["Practice_ID"]));
        //        }
        //        else
        //        {
        //            obllist = objInfo.GetAllProvidersBasedForClaim(ProvID, PracticeID);
        //        }
        //        if (obllist.Count > 0)
        //        {
        //            Session["PlaceOfService_ID"] = obllist[0].PlaceOfService_ID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, "ScheduleController", "FillPlaceofServices", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult AddNewAppointment(string id, string apptdate, string frompage = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {

                FillPatients();
                //  FillTechnicians();

                if (clsWebConfigsettings.GetConfigSettingsValue("Display_placeofservice").ToUpper() == "YES")
                {
                    ViewBag.hdnplaceind = "Y";
                }
                else
                {
                    ViewBag.hdnplaceind = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowRecurrence").ToUpper() == "YES")
                {
                    ViewBag.ShowRecurrence = "Y";
                }
                else
                {
                    ViewBag.ShowRecurrence = "N";
                }
                //IList<SelectListItem> OtherAddressItemList = new List<SelectListItem>();
                //IDataParameter[] objparam = {new SqlParameter("@In_Provider_id",Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"]))};
                //objCommon.AddInParameters(objparam);
                //SqlDataReader objread = default(SqlDataReader);
                //objread = objCommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
                //while (objread.Read())
                //{
                //    //if (Convert.ToString(objread["courtlocation"]) != "")
                //    //{
                //        OtherAddressItemList.Add(new SelectListItem
                //        {
                //            Text = Convert.ToString(objread["courtlocation"]),
                //            Value = Convert.ToString(objread["Appointment_address_id"])
                //        });
                //  //  }
                //}
                //OtherAddressItemList.Add(new SelectListItem
                //{
                //    Text = "------------------------",
                //    Value = ""
                //});
                //OtherAddressItemList.Add(new SelectListItem
                //{

                //    Text = "New Location",
                //    Value = "0"
                //});
                //ViewBag.OtherAddressItemList = OtherAddressItemList;

                //IList<SelectListItem> _ddlTimeMer = new List<SelectListItem>();

                //_ddlTimeMer.Add(new SelectListItem
                //{
                //    Text = "AM",
                //    Value = "0"
                //});
                //_ddlTimeMer.Add(new SelectListItem()
                //{
                //    Text = "PM",
                //    Value = "1"
                //});
                //IList<SelectListItem> _ddlDuration = new List<SelectListItem>();
                //for (int i = 30; i <= 120; i = i + 30)
                //{
                //    _ddlDuration.Add(new SelectListItem
                //    {
                //        Text = i.ToString(),
                //        Value = i.ToString()
                //    });
                //}
                AddAppointmentModel objAddAppt = new AddAppointmentModel();
                //string str = id;
                string[] str1 = id.Split('-');
               // str = str1[0];
                //string strmin = str1[1];
                //ViewBag.ddlTimeMer = _ddlTimeMer;
                //ViewBag.ddlDuration = _ddlDuration;
                objAddAppt.AppointmentDate = apptdate;

                ViewBag.timemin = (str1[1] == "00AM" || str1[1] == "00PM") ? "00" : "30";
                ViewBag.timemer = (str1[1] == "00AM" || str1[1] == "30AM") ? 0 : 1;
                ViewBag.time = str1[0] + ":" + ViewBag.timemin;
               // string[] timeapp = { "01:00", "01:30", "02:00", "02:30", "03:00", "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30" };
                var timelist = new ComboBoxItemList(timeapplist);
                ViewBag.index = Array.IndexOf(timeapplist, ViewBag.time);
                //var timeitems = new List<ComboBoxItem>();
               // if (ViewBag.cbotime != null & ViewBag.cbotime != "")
               // {
                    //string timesapp = ViewBag.cbotime;

                   // timeitems.AddRange(timelist);
              //  timeitems.Where(Func<ComboBoxItem,t)
                   // timeitems.Where(w => w.Text == timesapp.Trim()).Select(s => s.Selected == true);
                    //foreach (ComboBoxItem item in timelist)
                    //{
                    //    if (item.Text == timesapp.Trim())
                    //    {
                    //        item.Selected = true;

                    //    }
                    //    timeitems.Add(item);
                    //}
                    ViewData["cbotime"] = timelist;
               // }
               // else
               // {
                  //  ViewData["cbotime"] = timelist;
                    //ViewData["cboyears"] = timeapplist;
               // }
                //ViewData["cbotime"] = timelist;
                //TempData["frompage"] = frompage;
                ViewBag.frompage = frompage;
                Dictionary<int, string> weeklyitems = new Dictionary<int, string>();
                weeklyitems.Add(1, "Sun");
                weeklyitems.Add(2, "Mon");
                weeklyitems.Add(3, "Tue");
                weeklyitems.Add(4, "Wed");
                weeklyitems.Add(5, "Thu");
                weeklyitems.Add(6, "Fri");
                weeklyitems.Add(7, "Sat");
                ViewBag.weeklylist = weeklyitems;
                return View("AddNewAppointment", objAddAppt);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {

                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddNewAppointment(string txtEmailID, Models.AddAppointmentModel addAppointment)
        {
             if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
              {
                    string strMessage = null;
                    if (string.IsNullOrEmpty(stroutmsg))
                    {
                        if (Convert.ToString(Session["roleid"]) != "1")
                        {
                            addAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
                        }
                        else
                        {
                            addAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                        }
                        addAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
                        addAppointment.FromReferenceType_ID = 2;
                        addAppointment.GroupInd = "N";
                        addAppointment.NumofOccurrences = null;
                        if (addAppointment.ApptType != null)
                        {
                            if (addAppointment.ApptType == "Patient")
                            {
                                addAppointment.AppointmentType_ID = 1;
                                addAppointment.AppointmentStatus_ID = 1;
                            }
                            else
                            {
                                addAppointment.AppointmentType_ID = 2;
                                addAppointment.AppointmentStatus_ID = 7;
                            }
                        }
                        if (addAppointment.ApptType == "Patient")
                        {
                            if (addAppointment.AddToReference_ID != null && addAppointment.AddToReference_ID != 0)
                            {
                                addAppointment.Patient_ID = addAppointment.AddToReference_ID;
                                addAppointment.ToReference_ID = addAppointment.AddToReference_ID;
                                addAppointment.ToReferenceLogin_ID = !string.IsNullOrEmpty(Request["AddToReferenceLogin_ID"]) ? Convert.ToInt32(Request["AddToReferenceLogin_ID"]) : 0;
                            }
                            else
                            {
                                if (Request["combobox1"] != null)
                                {
                                    addAppointment.Patient_ID = Convert.ToInt32(Request["combobox1"]);
                                    addAppointment.ToReference_ID = Convert.ToInt32(Request["combobox1"]);
                                }
                              //  addAppointment.Technician_ids = null;
                               // addAppointment.TechnicianPositions = null;
                                //if (Request["combobox2"] != null & Request["combobox2"] != "")
                                //{
                                //    string techids = string.Join(",", Request["combobox2"].Split(',').Distinct().ToArray());
                                //    addAppointment.Technician_ids = "," + techids + ",";

                                //}
                                //else
                                //{
                                //    addAppointment.Technician_ids = "," + Request["hdntecid"].ToString() + ",";
                                //}

                                //if (Request["hdntechpos"] != null & Request["hdntechpos"] != "")
                                //{
                                //    string techpos = string.Join(",", Request["hdntechpos"].Split(',').Distinct().ToArray());
                                //    addAppointment.TechnicianPositions = "," + techpos + ",";
                                //}
                                //else
                                //{
                                //    addAppointment.TechnicianPositions ="," +"0"+",";
                                //}   
                                addAppointment.ToReferenceLogin_ID = !string.IsNullOrEmpty(Request["AddToReferenceLogin_ID"]) ? Convert.ToInt32(Request["AddToReferenceLogin_ID"]) : 0;
                                    //addAppointment.ToReferenceLogin_ID != null ? addAppointment.ToReferenceLogin_ID : 0;
                            }
                        }
                        addAppointment.ToReferenceType_ID = 3;
                     //   addAppointment.ToReference_IDs = null;
                        addAppointment.StartDate = !string.IsNullOrEmpty(Request["txtApptDate"]) ? Request["txtApptDate"] : null;             
                        //addAppointment.PlaceOfService_ID = Convert.ToInt32(Session["PlaceOfService_ID"]);
                        addAppointment.Notes = !string.IsNullOrEmpty(addAppointment.Notes) ? Sanitizer.GetSafeHtmlFragment(addAppointment.Notes) : null;
                        addAppointment.AppointmentTime = Request["cbotime_SelectedText"] + (Request["ddlTimeMer"] == "0" ? "AM" : "PM");
                        //string validInd = "N";
                        if (Request["hdnValidInd"] != string.Empty)
                        {
                            addAppointment.IsValidate_Ind = Request["hdnValidInd"];
                        }
                        addAppointment.IsValidate_Ind = !string.IsNullOrEmpty(addAppointment.IsValidate_Ind) ? addAppointment.IsValidate_Ind : "N";
                        if (Request["ddlDuration"] != null)
                        {
                            addAppointment.Duration = Convert.ToInt32(Request["ddlDuration"]);
                        }
                        if (Convert.ToString(Session["roleid"]) != "39")
                        {
                            addAppointment.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        }
                        else
                        {
                            addAppointment.CreatedBy = Convert.ToInt32(Session["Techlog_id"]);
                        }
                        GetAppointment onjcheck = new GetAppointment();
                        string appatdate = addAppointment.StartDate;
                        string prId;
                        if (Convert.ToString(Session["roleid"]) != "1")
                        {
                            prId = Convert.ToString(Session["Prov_ID"]);
                        }
                        else
                        {
                            prId = Convert.ToString(Session["ComboProv_ID"]);
                        }
                        //if (Request["rdoApptPlace"] != null)
                        //{
                        //    if (Request["rdoApptPlace"] == "Patientname")
                        //    {
                        //        addAppointment.Aptplaceofservice = "C";
                        //    }
                        //    else
                        //    {
                        //        addAppointment.Aptplaceofservice = "P";
                        //    }
                        //}
                        if (addAppointment.ApptType == "Patient")
                        {
                            if (Request["hdnselectother"] != null)
                            {
                                //if (Request["hdnselectother"] == "My Location")
                                //{
                                //    addAppointment.Aptplaceofservice = "P";
                                //}
                                //else if (Request["hdnselectother"] == "Client Location")
                                //{
                                //    addAppointment.Aptplaceofservice = "C";
                                //}
                                //else if (Request["hdnselectother"] == "Other")
                                if (Request["hdnselectother"] == "New Location")
                                {
                                    addAppointment.Aptplaceofservice = "O";
                                    addAppointment.defaultCoachAddress = addAppointment.ChkDefaultCourt ? "Y" : "N";
                                    addAppointment.CourtLocationName = !string.IsNullOrEmpty(addAppointment.CourtLocationName) ? addAppointment.CourtLocationName : null;
                                }
                                else
                                {
                                    addAppointment.defaultCoachAddress = addAppointment.ChkDefaultCourt ? "Y" : "N";
                                    addAppointment.Aptplaceofservice = "O";
                                    addAppointment.otherAddressid = Convert.ToInt32(Request["OtherAddressItemList"]);
                                    addAppointment.CourtLocationName = !string.IsNullOrEmpty(addAppointment.CourtLocationName) ? addAppointment.CourtLocationName : null;
                                }
                            }
                        }

                        if (addAppointment.Recurring == "Daily")
                        {
                            addAppointment.IntervalType_ID = "1";
                            addAppointment.IntervalValue = addAppointment.AddDailyReccur;
                            addAppointment.EndDate = addAppointment.EndDateForDaily;
                            addAppointment.SelectedWeekdays = null;
                        }
                        else if (addAppointment.Recurring == "Weekly")
                        {
                            addAppointment.IntervalType_ID = "2";
                            addAppointment.IntervalValue = addAppointment.AddWeeklyReccur;
                            addAppointment.EndDate = addAppointment.EndDateForWeek;
                            addAppointment.SelectedWeekdays = Request["AddWselection"].Replace(",", "");
                        }
                        else
                        {
                            addAppointment.EndDate = null;
                            addAppointment.SelectedWeekdays = null;
                            addAppointment.IntervalValue = null;
                            addAppointment.IntervalType_ID = null;
                        }
                        string strDates = "";

                        if (addAppointment.Recurring == "No Recurring")
                        {
                            strDates = DateTime.Parse(appatdate).ToShortDateString();
                        }
                        else if (addAppointment.Recurring == "Daily")
                        {
                            while (DateTime.Parse(appatdate) <= DateTime.Parse(addAppointment.EndDate))
                            {
                                if (string.IsNullOrEmpty(strDates))
                                {
                                    strDates = DateTime.Parse(appatdate).ToShortDateString();
                                }
                                else
                                {
                                    strDates += "," + DateTime.Parse(appatdate).ToShortDateString();
                                }
                                appatdate = DateTime.Parse(appatdate).AddDays(Convert.ToInt32(addAppointment.IntervalValue)).ToShortDateString();
                            }
                        }
                        else
                        {
                            strDates = onjcheck.weeklyApptDates(appatdate, addAppointment.IntervalValue, addAppointment.EndDate, addAppointment.SelectedWeekdays);
                        }
                        strMessage = onjcheck.checkApptExist(strDates, Convert.ToString(addAppointment.ToReference_ID), addAppointment.AppointmentTime, prId, string.Empty);
                        if (strMessage == "Y")
                        {
                            if (addAppointment.ApptType == "Patient")
                            {
                                strMessage = "1";
                            }
                            else
                            {
                                strMessage = "2";
                            }
                            strMessage = strMessage.Replace("<br>", "\r\n");
                            return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            strMessage = string.Empty;
                            strMessage = addAppointment.InsertAppointment(addAppointment, 0);
                        }
                        if (string.IsNullOrEmpty(strMessage) & addAppointment.ApptType == "Patient")
                        {
                            string strbussinessname = string.Empty;
                            string strproviderphone = string.Empty;
                            string strclientphone = string.Empty;
                            if (!string.IsNullOrEmpty(Request["Bussinessname"]))
                            {
                                strbussinessname = Request["Bussinessname"];
                            }
                            if (!string.IsNullOrEmpty(Request["Provider_cellphoneno"]))
                            {
                                strproviderphone = Request["Provider_cellphoneno"];
                            }
                            if (!string.IsNullOrEmpty(addAppointment.cellphone))
                            {
                                strclientphone = addAppointment.cellphone.Replace("-", "");
                                //strclientphone = strclientphone.Replace("-", "");
                            }
                            clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, strclientphone, strDates, addAppointment.AppointmentTime, null, 0);
                        }
                        //int emailTran=0;
                        //if ((AppointmentID != 0) )
                        //{

                        //    if (Convert.ToString(Request["hdncstmemail"]) !="" && Request["hdncstmid"].ToString() != null)
                        //    {
                        //        emailTran = addAppointment.getEmailTranId(addAppointment, AppointmentID);
                        //    }
                        //    else if (Request["combobox1"] != null && (!string.IsNullOrEmpty(Request["hdnEmailID"])))
                        //    {
                        //        emailTran = addAppointment.getEmailTranId(addAppointment, AppointmentID);
                        //    }
                        //}
                    }
                    else
                    {
                        strMessage = stroutmsg.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(strMessage))
                    {
                        DateTime dt = DateTime.Parse(Request["txtApptDate"]);
                        addAppointment.monthname = new DateTime(dt.Year, dt.Month, 1).ToShortDateString();
                        if (Request["hdnfrompage"] != "month")
                        {
                            switch ((int)dt.DayOfWeek)
                            {
                                case 0:
                                    addAppointment.monthname = dt.ToShortDateString();
                                    break;
                                case 1:
                                    addAppointment.monthname = dt.AddDays(-1).ToShortDateString();
                                    break;
                                case 2:
                                    addAppointment.monthname = dt.AddDays(-2).ToShortDateString();
                                    break;
                                case 3:
                                    addAppointment.monthname = dt.AddDays(-3).ToShortDateString();
                                    break;
                                case 4:
                                    addAppointment.monthname = dt.AddDays(-4).ToShortDateString();
                                    break;
                                case 5:
                                    addAppointment.monthname = dt.AddDays(-5).ToShortDateString();
                                    break;
                                case 6:
                                    addAppointment.monthname = dt.AddDays(-6).ToShortDateString();
                                    break;
                            }
                        }
                        return Json(JsonResponseFactory.SuccessResponse(addAppointment), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                    }
              }
             else
             {
                 if (Request.IsAjaxRequest())
                 {

                     return RedirectToRoute("SessionExpirepopup");
                 }
                 else
                 {
                     return RedirectToRoute("SessionExpire");
                 }
             }

        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult AddNewCustomer()
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
                if (Convert.ToString(Session["RoleID"]) != "1")
                {
                    objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
                }
                else
                {
                    objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["ComboProv_ID"]));
                }
                if ((objInfo != null))
                {
                    ViewBag.Zip = !string.IsNullOrEmpty(objInfo.Zip) ? objInfo.Zip : null;

                    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.State_ID)))
                    {
                        ViewBag.State_ID = objInfo.State_ID;
                    }
                    else
                    {
                        ViewBag.State_ID = null;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(objInfo.City_ID)))
                    {
                        ViewBag.City_ID = objInfo.City_ID;
                    }
                    else
                    {
                        ViewBag.City_ID = null;
                    }
                    ViewBag.Statename = !string.IsNullOrEmpty(objInfo.Statename) ? objInfo.Statename : null;
                    ViewBag.Cityname = !string.IsNullOrEmpty(objInfo.Cityname) ? objInfo.Cityname : null;

                }
                //Session.Add("TopIndex", 0);
                //if (Convert.ToString(Session["CCexists"]) == "N" && Convert.ToString(Session["Registeredin"]) != "M")
                //{
                //    return RedirectToAction("Schedulespecs", "Schedule");
                //}
                //else if (Convert.ToString(Session["Msgitem"]) != "5")
                //{
                //    return RedirectToAction("Schedulespecs", "Schedule");
                //}
                return View("AddNewCustomer", new PatientRegistration());
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost()]
        public ActionResult AddNewCustomer(PatientRegistration obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //string outmesg = null;
                //if (!string.IsNullOrEmpty(obj.PatientEmail))
                //{
                //    outmesg = objCommon.EmailExistsFunction(obj.PatientEmail);
                //}
                //if (!string.IsNullOrEmpty(outmesg))
                //{
                //    return Json(JsonResponseFactory.ErrorResponse("Enter email is already exists"), JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                    //try
                    //{
                        if (!string.IsNullOrEmpty(obj.HomePhone1) & !string.IsNullOrEmpty(obj.HomePhone2) & !string.IsNullOrEmpty(obj.HomePhone3))
                        {
                            obj.HomePhone = obj.HomePhone1.Trim() + obj.HomePhone2.Trim() + obj.HomePhone3.Trim();
                        }
                        if (!string.IsNullOrEmpty(obj.WorkPhone1) & !string.IsNullOrEmpty(obj.WorkPhone2) & !string.IsNullOrEmpty(obj.WorkPhone3))
                        {
                            obj.WPhone = obj.WorkPhone1.Trim() + obj.WorkPhone2.Trim() + obj.WorkPhone3.Trim();
                        }
                        if (!string.IsNullOrEmpty(obj.MobilePhone1) & !string.IsNullOrEmpty(obj.MobilePhone2) & !string.IsNullOrEmpty(obj.MobilePhone3))
                        {
                            obj.MPhone = obj.MobilePhone1.Trim() + obj.MobilePhone2.Trim() + obj.MobilePhone3.Trim();
                        }
                        if (Convert.ToString(Session["RoleID"]) != "1")
                        {
                            obj.Provider_ID = Convert.ToInt32(Session["Prov_ID"]);
                        }
                        else
                        {
                            obj.Provider_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                        }
                        obj.City_ID = (Convert.ToInt32(Request["DDCity"]) != 0 ? Convert.ToInt32(Request["DDCity"]) : 0);
                        obj.State_ID = (Convert.ToInt32(Request["DDState"]) != 0 ? Convert.ToInt32(Request["DDState"]) : 0);
                        obj.Country_ID = 1;
                        obj.ZIP = (!string.IsNullOrEmpty(Request["txtZip"]) ? Request["txtZip"] : null);
                        obj.UserId = Convert.ToInt32(Session["UserID"]);
                        string Out_Msg = null;
                        string Out_login_id = null;
                        obj.Ind = !string.IsNullOrEmpty(Request["HdnInd"]) ? Request["HdnInd"] : "N";
                        MowerHelper.Models.BLL.Patients.PatientRegistration.Insert_Patinet_FT(obj, ref Out_Msg, ref Out_login_id);
                        obj.Customer_Name = obj.FirstName + " " + obj.LastName;
                        obj.Customer_Id = Out_Msg;
                        obj.Customer_LoginId = Out_login_id;
                       
                        if (!string.IsNullOrEmpty(Out_Msg) && Out_Msg.Contains("Do you want to create"))
                        {
                            return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                        }
                    //}
                    //catch (Exception ex)
                    //{
                    //    clsExceptionLog clsCustomEx = new clsExceptionLog();
                    //    clsCustomEx.LogException(ex, "ClientRegController", "ClientInformation", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    //    clsCustomEx = null;
                    //}
               // }
                //return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        public void FillPatients()
        {
            try
            {
                ListofPatients getPatient = new ListofPatients();
                List<ListofPatients> objlist = new List<ListofPatients>();
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    objlist = getPatient.getPatientList(Convert.ToInt32(Session["Prov_ID"]));
                }
                else
                {
                    objlist = getPatient.getPatientList(Convert.ToInt32(Session["ComboProv_ID"]));
                }
                ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Patient_ID", "Patientname", "PatientLogin_ID");
                ViewData["combobox1"] = custlist;
                ViewData["combobox1_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "FillPatients", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //public void FillTechnicians()
        //{
        //    try
        //    {
        //        TechniciansInfo gettechnician = new TechniciansInfo();
        //        List<TechniciansInfo> objlist = new List<TechniciansInfo>();
        //        if (Convert.ToString(Session["roleid"]) != "1")
        //        {
        //            objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["Prov_ID"]), null);
        //            ViewBag.protechid = objlist[0].Technician_id;
        //            // ViewBag.Phoneno = objlist[0].Phonenumber;
        //        }
        //        else
        //        {
        //            objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["ComboProv_ID"]), null);
        //            ViewBag.protechid = objlist[0].Technician_id;
        //        }
        //        ViewBag.techcnt = objlist.Count;
        //        if (objlist.Count == 1)
        //        {
        //            ViewBag.tecid = objlist[0].Technician_id;
        //        }
        //        ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Technician_id", "Technician_Name");
        //        ViewData["combobox2"] = custlist;
        //        ViewData["combobox2_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, "ScheduleController", "FillTechnicians", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //public void FillTechnicians123(string apptEid)
        //{
        //    try
        //    {
        //        TechniciansInfo gettechnician = new TechniciansInfo();
        //        List<TechniciansInfo> objlist = new List<TechniciansInfo>();
        //        if (Convert.ToString(Session["roleid"]) != "1")
        //        {
        //            objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["Prov_ID"]), apptEid);
        //        }
        //        else
        //        {
        //            objlist = gettechnician.GetTechnicianList(Convert.ToInt32(Session["ComboProv_ID"]), apptEid);
        //        }
        //        ViewBag.techcnt1 = objlist.Count;
        //        if (objlist.Count == 1)
        //        {
        //            ViewBag.tecid1 = objlist[0].Technician_id;
        //        }
        //        ComboBoxItemList custlist = new ComboBoxItemList(objlist, "Technician_id", "Technician_Name");
        //        ViewData["combobox5"] = custlist;
        //        ViewData["combobox5_Folderstyle"] = "../Content/Obout/ComboBox/styles/black_glass";
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, "ScheduleController", "FillTechnicians123", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPatientInfo(string patientid)
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
           // List<string> objpatientinfolist = new List<string>();

            SqlDataReader ObjReader;
            IDataParameter[] InParamList = {
            new SqlParameter("@In_Patient_Ids", patientid),
           // new SqlParameter("@in_Practice_ID", Session["PracticeID"]),
            new SqlParameter("@iN_pROVIDER_ID", null)
        };
            objCommon.AddInParameters(InParamList);
            ObjReader = objCommon.GetDataReader("Help_dbo.st_Scheduling_Get_PatientDetails");
            List<GetPatientDetails> objGetPatientDetailslist = new List<GetPatientDetails>();
            while (ObjReader.Read())
            {
                GetPatientDetails objGetPatientDetails = new GetPatientDetails(Convert.ToInt32(ObjReader["Patient_ID"]), Convert.ToInt32(ObjReader["PatientLogin_ID"]), Convert.ToString(ObjReader["PatientName"]), Convert.ToInt32(ObjReader["Duration"]), Convert.ToString(ObjReader["PatientEmail"]), Convert.ToString(ObjReader["DefaultCourtLocation_ID"]));
                objGetPatientDetails.Bussinessname = Convert.ToString(ObjReader["Bussinessname"]);
                objGetPatientDetails.Provider_cellphoneno = Convert.ToString(ObjReader["Provider_cellphoneno"]);
                objGetPatientDetails.cellphone = Convert.ToString(ObjReader["patient_cellphoneno"]);
                objGetPatientDetails.DefaultCourtId = Convert.ToString(ObjReader["DefaultCourtLocation_ID"]);
                objGetPatientDetailslist.Add(objGetPatientDetails);
            }
            return Json(objGetPatientDetailslist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult EditAppointment(string apptEid, string apptdate, string recId, string frompage = null, string cphone = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (clsWebConfigsettings.GetConfigSettingsValue("Display_placeofservice").ToUpper() == "YES")
                {
                    ViewBag.hdnEplaceind = "Y";
                }
                else
                {
                    ViewBag.hdnEplaceind = "N";
                }
                if (clsWebConfigsettings.GetConfigSettingsValue("ShowRecurrence").ToUpper() == "YES")
                {
                    ViewBag.ShowRecurrence = "Y";
                }
                else
                {
                    ViewBag.ShowRecurrence = "N";
                }
                AddAppointmentModel objGetAppt = GetAppointmentDetails(apptEid, apptdate);
                if (Convert.ToString(Session["Roleid"]) != "1")
                {
                    objGetAppt.ProvAddrInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
                }
                else
                {
                    objGetAppt.ProvAddrInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["ComboProv_ID"]));
                }
    //            IList<SelectListItem> OtherAddressItemList = new List<SelectListItem>();
    //            IDataParameter[] objparam = {
    //    new SqlParameter("@In_Provider_id",Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"]))       
    //};
    //            objCommon.AddInParameters(objparam);
    //            SqlDataReader objread = default(SqlDataReader);
    //            objread = objCommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
    //            while (objread.Read())
    //            {
    //               // if (Convert.ToString(objread["courtlocation"]) != "")
    //              //  {
    //                    //if (Convert.ToString(ViewBag.SelectedAddressid) == Convert.ToString(objread["Appointment_address_id"]))
    //                    //{
    //                    //    ViewBag.SelectedAddressD = Convert.ToString(objread["courtlocation"]);
    //                    //    ViewBag.SelectedAddress_id = Convert.ToString(objread["Appointment_address_id"]);
    //                    //}
    //                    OtherAddressItemList.Add(new SelectListItem
    //                    {
    //                        Text = Convert.ToString(objread["courtlocation"]),
    //                        Value = Convert.ToString(objread["Appointment_address_id"])
    //                    });
    //                //}
    //            }
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "------------------------",
    //                Value = "",
    //                Selected = false
    //            });
    //            OtherAddressItemList.Add(new SelectListItem
    //            {

    //                Text = "New Location",
    //                Value = "0"
    //            });
    //            ViewBag.EOtherAddressItemList = OtherAddressItemList;
                // FillTechnicians123(apptEid);
                Dictionary<int, string> weeklyitems = new Dictionary<int, string>();
                weeklyitems.Add(1, "Sun");
                weeklyitems.Add(2, "Mon");
                weeklyitems.Add(3, "Tue");
                weeklyitems.Add(4, "Wed");
                weeklyitems.Add(5, "Thu");
                weeklyitems.Add(6, "Fri");
                weeklyitems.Add(7, "Sat");
                objGetAppt.cellphone = cphone;
                ViewBag.weeklylist = weeklyitems;
               // TempData["frompage"] = frompage;
                objGetAppt.FromPage = frompage;
                return View("EditAppointment", objGetAppt);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        private AddAppointmentModel GetAppointmentDetails(string patId, string ApptDate)
        {
            AddAppointmentModel objAddAppt = new AddAppointmentModel();
            try
            {
                List<GetAppointment> objGetAppointment = GetAppointment.GetPatientAppointment(patId, null, ApptDate);
                if (objGetAppointment.Count > 0)
                {
                    objAddAppt.cellphone = objGetAppointment[0].phoneReminderHR;
                    ViewBag.SelectedAddress = objGetAppointment[0].Defaultaddress_Ind;
                    ViewBag.SelectedAddressid = objGetAppointment[0].Appointment_address_ID;
                    objAddAppt.ProviderName = objGetAppointment[0].ProviderName;
                    objAddAppt.PatientName = objGetAppointment[0].PatientName;
                  //  ViewBag.Patnote = objGetAppointment[0].PTNotes;
                    objAddAppt.Notes = System.Web.HttpUtility.HtmlDecode(objGetAppointment[0].Notes.Trim());
                    objAddAppt.AppointmentID = objGetAppointment[0].Appointment_ID;
                    if (!string.IsNullOrEmpty(objGetAppointment[0].VoiceFileName))
                    {
                        objAddAppt.VoiceFileName = objGetAppointment[0].VoiceFileName;
                    }
                    TempData["ApptType"] = objGetAppointment[0].AppointmentType_ID;
                    if (!string.IsNullOrEmpty(objGetAppointment[0].Startdate))
                    {
                        ViewBag.Startdate = DateTime.Parse(objGetAppointment[0].Startdate).ToShortDateString();
                    }
                    //ViewBag.techpositions = objGetAppointment[0].TechnicianPositions;
                    objAddAppt.Aptplaceofservice = objGetAppointment[0].Aptplaceofservice;
                    objAddAppt.Aptplaceofservicename = objGetAppointment[0].Aptplaceofservicename;

                    if (objGetAppointment[0].AppointmentType_ID == 1)
                    {
                        objAddAppt.ApptType = "Client";
                    }
                    else
                    {
                        objAddAppt.ApptType = "Blocked";
                    }
                    if ((objGetAppointment[0].AppointmentDate != null))
                    {
                        objAddAppt.AppointmentDate = objGetAppointment[0].AppointmentDate;
                    }
                    if (objGetAppointment[0].IntervalType_ID != 0)
                    {
                        if (objGetAppointment[0].IntervalType_ID == 1)
                        {
                            objAddAppt.RepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "day(s) at " + " " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.WeekDay = "DayShow";
                            objAddAppt.dayEndDate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.interdayvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
                        }
                        else if (objGetAppointment[0].IntervalType_ID == 2)
                        {
                            objAddAppt.WeekDay = "Weekshow";
                            string showweek = string.Empty;
                            string showweekind = string.Empty;
                            if (objGetAppointment[0].SelectedWeekdays != null)
                            {
                                string selectedweeks = objGetAppointment[0].SelectedWeekdays;
                                //selectedweeks.ToCharArray();

                                foreach (char i in selectedweeks)
                                {
                                    switch (i)
                                    {
                                        case '1':
                                            showweek += "Sun" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '2':
                                            showweek += "Mon" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '3':
                                            showweek += "Tue" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '4':
                                            showweek += "Wed" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '5':
                                            showweek += "Thu" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '6':
                                            showweek += "Fri" + ",";
                                            showweekind += i + ",";
                                            break;
                                        case '7':
                                            showweek += "Sat" + ",";
                                            showweekind += i + ",";
                                            break;
                                    }


                                }
                                showweek = showweek.Remove(showweek.LastIndexOf(","));
                                showweekind = showweekind.Remove(showweekind.LastIndexOf(","));
                            }
                            objAddAppt.RepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "week(s) on " + " " + showweek + " " + "at  " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.weekEndDate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
                            objAddAppt.interweekvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
                            ViewBag.chkweek = showweekind;
                        }
                    }
                    else
                    {
                        objAddAppt.RepeatType = "No Repeat";
                    }
                    objAddAppt.EndDate = !string.IsNullOrEmpty(objGetAppointment[0].Enddate) ? DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString() : null;
                    
                    //IList<SelectListItem> _ddlTimeMer = new List<SelectListItem>();

                    //_ddlTimeMer.Add(new SelectListItem
                    //{
                    //    Text = "AM",
                    //    Value = "0"
                    //});
                    //_ddlTimeMer.Add(new SelectListItem()
                    //{
                    //    Text = "PM",
                    //    Value = "1"
                    //});
                    //IList<SelectListItem> _ddlDuration = new List<SelectListItem>();
                    //for (int i = 30; i <= 120; i = i + 30)
                    //{
                    //    _ddlDuration.Add(new SelectListItem
                    //    {
                    //        Text = i.ToString(),
                    //        Value = i.ToString()
                    //    });
                    //}
                    //ViewBag.ddlETimeMer = _ddlTimeMer;
                    //ViewBag.ddlEDuration = _ddlDuration;
                    if (objGetAppointment[0].AppointmentRecurrence_ID != 0)
                    {
                        objAddAppt.AppointmentRecurrence_ID = objGetAppointment[0].AppointmentRecurrence_ID;
                    }
                    if (objGetAppointment[0].CurrentAppointmentTracking_ID != null)
                    {
                        objAddAppt.AppointmentTracking_ID = objGetAppointment[0].CurrentAppointmentTracking_ID;
                    }
                    if (objGetAppointment[0].FacilityReference_ID != null)
                    {
                        objAddAppt.FacilityReference_ID = objGetAppointment[0].FacilityReference_ID;
                    }
                    if ((objGetAppointment[0].AppointmentTime != null))
                    {
                       // string strTime = objGetAppointment[0].AppointmentTime;
                        ViewBag.strTime = objGetAppointment[0].AppointmentTime;
                        string[] strsplit = objGetAppointment[0].AppointmentTime.Split(':');
                        //string strhr = strsplit[0];
                        //ViewBag.gethour = strhr;
                        if (strsplit[1] == "00AM" | strsplit[1] == "00PM")
                        {
                            ViewBag.getmin = "00";
                        }
                        else
                        {
                            ViewBag.getmin = "30";
                        }
                        if (strsplit[1] == "00AM" | strsplit[1] == "30AM")
                        {
                            ViewBag.getAMPM = "0";
                        }
                        else
                        {
                            ViewBag.getAMPM = "1";
                        }
                        ViewBag.comboEtime = strsplit[0] + ":" + ViewBag.getmin;
                       
                        var timelist = new ComboBoxItemList(timeapplist);
                        ViewBag.index = Array.IndexOf(timeapplist, ViewBag.comboEtime);
                        ViewData["cboEtime"] = timelist;
                        //var timeitems = new List<ComboBoxItem>();
                        //if (ViewBag.comboEtime != null & ViewBag.comboEtime != "")
                        //{
                        //    string timesapp = ViewBag.comboEtime;


                        //    foreach (ComboBoxItem item in timelist)
                        //    {
                        //        if (item.Text == timesapp.Trim())
                        //        {
                        //            item.Selected = true;

                        //        }
                        //        timeitems.Add(item);
                        //    }
                        //    ViewData["cboEtime"] = timeitems;
                        //}
                        //else
                        //{
                        //    ViewData["cboEtime"] = timelist;
                        //    //ViewData["cboyears"] = timeapplist;
                        //}
                    }
                    if (objGetAppointment[0].Duration != 0)
                    {
                        objAddAppt.Duration = objGetAppointment[0].Duration;
                    }
                    if (objGetAppointment[0].AppointmentStatus_ID != null)
                    {
                        objAddAppt.AppointmentStatus_ID = objGetAppointment[0].AppointmentStatus_ID;
                    }
                    objAddAppt.ToReference_ID = objGetAppointment[0].ToReference_ID;
                    objAddAppt.ToReferenceLogin_ID = objGetAppointment[0].ToReferenceLogin_ID;
                    // ViewBag.technician_ids = objGetAppointment[0].Technician_ids;
                    // string tech1 = null;
                    // string tech2 = null;
                    //if (techid.StartsWith(","))
                    //{
                    //    tech1 = techid.Remove(0, 1);
                    //}
                    //else
                    //{
                    //    tech1 = techid;
                    //}
                    //if (tech1 != null)
                    //{
                    //    if (tech1.EndsWith(","))
                    //    {
                    //        tech2 = tech1.Remove(tech1.Length - 1);
                    //    }
                    //    else
                    //    {
                    //        tech2 = tech1;
                    //    }
                    //}
                    //ViewBag.technician_ids = tech2;
                }

            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetAppointmentDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objAddAppt;
        }
        [HttpPost()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAppointment(Models.AddAppointmentModel objUpdateAppointment)
        {
            //if (!string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            //{
            //    if (Request.IsAjaxRequest())
            //    {
                    if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
                    {
                    string strFuturemessage = null;
                    string strChangeRecur_Ind = "";
                    //string strappttime1 = null;
                    //string strTimeDifference = "";
                    string ApptTime = "";
                    //    string intEveryNo = "";
                    string strStartDate = "";
                    string dtEndOn = "";
                    string SelectedDays = "";
                    string strRecurenceValue = "";
                    string strMessage = null;
                    DateTime originalDate;
                    DateTime modifDate;
                    string originalval = string.Empty;
                    string modifyval = string.Empty;
                    string modify = string.Empty;
                    string modify1 = string.Empty;
                    string StrDateChange = "N";
                    objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(objUpdateAppointment.AppointmentStatus_ID);
                    if (objUpdateAppointment.AppointmentStatus_ID != 7 )
                    {
                        if (Request["hdnselectEother"] != null)
                        {
                            if (Request["hdnselectEother"] == "New Location")
                            {
                                objUpdateAppointment.Aptplaceofservice = "O";
                                objUpdateAppointment.otherAddress = objUpdateAppointment.ProvAddrInfo.Address1;
                                objUpdateAppointment.zip = objUpdateAppointment.ProvAddrInfo.Zip;
                                objUpdateAppointment.CourtLocationName = !string.IsNullOrEmpty(objUpdateAppointment.CourtLocationName) ? objUpdateAppointment.CourtLocationName : null;
                                objUpdateAppointment.cityid = objUpdateAppointment.ProvAddrInfo.City_ID;
                                objUpdateAppointment.stateid = objUpdateAppointment.ProvAddrInfo.State_ID;
                                objUpdateAppointment.defaultCoachAddress = objUpdateAppointment.ChkDefaultCourt == true ? "Y" : "N";
                            }
                            else
                            {
                                objUpdateAppointment.defaultCoachAddress = objUpdateAppointment.ChkDefaultCourt == true ? "Y" : "N";
                                objUpdateAppointment.Aptplaceofservice = "O";
                                objUpdateAppointment.otherAddressid = Convert.ToInt32(Request["EOtherAddressItemList"]);
                                objUpdateAppointment.CourtLocationName = !string.IsNullOrEmpty(objUpdateAppointment.CourtLocationName) ? objUpdateAppointment.CourtLocationName : null;
                            }
                        }
                    }
                    if (objUpdateAppointment.AppointmentStatus_ID == 1 || objUpdateAppointment.AppointmentStatus_ID == 3 || objUpdateAppointment.AppointmentStatus_ID == 5 || objUpdateAppointment.AppointmentStatus_ID == 7)
                    {
                        ApptTime = Request["cboEtime_SelectedText"] + (Request["ddlETimeMer"] == "0" ? "AM" : "PM");
                        strStartDate = Request["txtEApptDate"];
                        
                        if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) || !string.IsNullOrEmpty(objUpdateAppointment.weekEndDate))
                        {
                            if (Request["Hdnapt_date"] != strStartDate)
                            {
                                modify = "Y";
                                StrDateChange = "Y";
                            }
                        }
                        //if (!string.IsNullOrEmpty(Request["strTime"]))
                        //{
                        //    if (ApptTime != Request["strTime"])
                        //    {
                        //        modify = "N";
                        //        modify1 = "Y";
                        //    }
                        //}
                        //if (!string.IsNullOrEmpty(Request["dayenddate"]) & Request["Recurring"] != "Weekly")
                        if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) & objUpdateAppointment.ERecurring != "Weekly")
                        {
                            objUpdateAppointment.IntervalType_ID = "1";
                            if (!string.IsNullOrEmpty(objUpdateAppointment.DailyReccur))
                            {
                                strRecurenceValue = objUpdateAppointment.DailyReccur;
                                originalval = objUpdateAppointment.interdayvalue;
                                modifyval = objUpdateAppointment.DailyReccur;
                                if (originalval != modifyval)
                                {
                                    modify = "N";
                                    modify1 = "Y";
                                }
                            }
                            else
                            {
                                strRecurenceValue = objUpdateAppointment.interdayvalue;
                                //originalval = Request["interdayvalue"];
                                //modifyval = Request["interdayvalue"];
                                //if (originalval != modifyval)
                                //{
                                //    modify = "Y";
                                //}

                            }

                            if (objUpdateAppointment.ERecurring == "Daily")
                            {
                                dtEndOn = Request["EtxtEndDateforDaily"];
                                originalDate = DateTime.Parse(objUpdateAppointment.dayEndDate);
                                modifDate = DateTime.Parse(Request["EtxtEndDateforDaily"]);
                                if (originalDate != modifDate)
                                {
                                    modify = "N";
                                    modify1 = "Y";
                                    //StrDateChange = "Y";
                                }
                            }
                            else
                            {
                                dtEndOn = objUpdateAppointment.dayEndDate;
                            }
                            SelectedDays = null;
                            //intEveryNo = Request["txtDailyRecur"];
                        }
                        //else if (!string.IsNullOrEmpty(Request["dayenddate"]) & Request["Recurring"] == "Weekly")
                        //{

                        //    StrRecurrencetype_ID = "2";

                        //    if (Request["Recurring"] == "Weekly")
                        //    {
                        //        dtEndOn = Request["EtxtEndDateforWeek"];
                        //        strRecurenceValue = Request["EtxtWeeklyRecur"];
                        //        modify = "Y";
                        //        modify1 = "Y";
                        //        //originalDate = DateTime.Parse(Request["weekenddate"]);
                        //        //modifDate = DateTime.Parse(dtEndOn);
                        //        //if (originalDate != modifDate)
                        //        //{
                        //        //    modify = "Y";
                        //        //}
                        //        //originalval = Request["interweekvalue"];
                        //        //modifyval = Request["txtWeeklyRecur"];
                        //        //if (originalval != modifyval)
                        //        //{
                        //        //    modify = "Y";
                        //        //}
                        //    }
                        //    else
                        //    {
                        //        dtEndOn = Request["weekenddate"];
                        //        strRecurenceValue = Request["interweekvalue"];

                        //    }
                        //    SelectedDays = Request["Wselection"];
                        //    if (Request["chkweek"] != Request["Wselection"])
                        //    {
                        //        modify = "N";
                        //        modify1 = "Y";
                        //    }
                        //    SelectedDays = SelectedDays.Replace(",", "");
                        //}
                        //else if (!string.IsNullOrEmpty(Request["weekenddate"]) & Request["Recurring"] != "Daily")
                        else if (!string.IsNullOrEmpty(objUpdateAppointment.dayEndDate) & objUpdateAppointment.ERecurring == "Weekly")
                        {

                            objUpdateAppointment.IntervalType_ID = "2";

                            if (objUpdateAppointment.ERecurring == "Weekly")
                            {
                                dtEndOn = Request["EtxtEndDateforWeek"];
                                strRecurenceValue = objUpdateAppointment.WeeklyReccur;
                                modify = "Y";
                                modify1 = "Y";
                                //originalDate = DateTime.Parse(Request["weekenddate"]);
                                //modifDate = DateTime.Parse(dtEndOn);
                                //if (originalDate != modifDate)
                                //{
                                //    modify = "Y";
                                //}
                                //originalval = Request["interweekvalue"];
                                //modifyval = Request["txtWeeklyRecur"];
                                //if (originalval != modifyval)
                                //{
                                //    modify = "Y";
                                //}
                            }
                            else
                            {
                                dtEndOn = objUpdateAppointment.weekEndDate;
                                strRecurenceValue = objUpdateAppointment.interweekvalue;

                            }
                            SelectedDays = Request["Wselection"];
                            if (Request["chkweek"] != Request["Wselection"])
                            {
                                modify = "N";
                                modify1 = "Y";
                            }
                            SelectedDays = SelectedDays.Replace(",", "");
                        }
                        else if (!string.IsNullOrEmpty(objUpdateAppointment.weekEndDate) & objUpdateAppointment.ERecurring != "Daily")
                        {
                            objUpdateAppointment.IntervalType_ID = "2";
                            if (objUpdateAppointment.ERecurring == "Weekly")
                            {
                                dtEndOn = Request["EtxtEndDateforWeek"];
                                strRecurenceValue = objUpdateAppointment.WeeklyReccur;
                                originalDate = DateTime.Parse(objUpdateAppointment.weekEndDate);
                                modifDate = DateTime.Parse(dtEndOn);
                                if (originalDate != modifDate)
                                {
                                    modify = "N";
                                    modify1 = "Y";
                                    //StrDateChange = "Y";
                                }
                                originalval = objUpdateAppointment.interweekvalue;
                                modifyval = objUpdateAppointment.WeeklyReccur;
                                if (originalval != modifyval)
                                {
                                    modify = "N";
                                    modify1 = "Y";
                                }
                            }
                            else
                            {
                                dtEndOn = objUpdateAppointment.weekEndDate;
                                strRecurenceValue = objUpdateAppointment.interweekvalue;

                            }
                            SelectedDays = Request["Wselection"];
                            if (Request["chkweek"] != Request["Wselection"])
                            {
                                modify = "N";
                                modify1 = "Y";
                            }
                            SelectedDays = SelectedDays.Replace(",", "");
                        }
                        //else if (!string.IsNullOrEmpty(Request["weekenddate"]) & Request["Recurring"] == "Daily")
                        else if (!string.IsNullOrEmpty(objUpdateAppointment.weekEndDate) & objUpdateAppointment.ERecurring == "Daily")
                        {
                            objUpdateAppointment.IntervalType_ID = "1";
                            if (!string.IsNullOrEmpty(objUpdateAppointment.DailyReccur))
                            {
                                strRecurenceValue = objUpdateAppointment.DailyReccur;
                                modify = "N";
                                modify1 = "Y";
                                //originalval = Request["interdayvalue"];
                                //modifyval = Request["txtEditDailyRecur"];
                                //if (originalval != modifyval)
                                //{
                                //    modify = "Y";
                                //}
                            }
                            else
                            {
                                strRecurenceValue = objUpdateAppointment.interdayvalue;
                                //originalval = Request["interdayvalue"];
                                //modifyval = Request["interdayvalue"];
                                //if (originalval != modifyval)
                                //{
                                //    modify = "Y";
                                //}

                            }

                            if (objUpdateAppointment.ERecurring == "Daily")
                            {
                                dtEndOn = Request["EtxtEndDateforDaily"];
                                //originalDate = DateTime.Parse(Request["dayenddate"]);
                                //modifDate = DateTime.Parse(Request["txtEndDateforDaily"]);
                                //if (originalDate != modifDate)
                                //{
                                //    modify = "Y";
                                //}
                            }
                            else
                            {
                                dtEndOn = objUpdateAppointment.dayEndDate;
                            }
                            SelectedDays = null;
                        }
                        //not recurrence
                        else
                        {
                            objUpdateAppointment.IntervalType_ID = null;
                            strRecurenceValue = null;
                            //  intEveryNo = null;
                            SelectedDays = null;
                            dtEndOn = null;
                        }


                    }
                    else if (objUpdateAppointment.AppointmentStatus_ID == 4)
                    {
                        strStartDate = Request["txtEApptDate"];
                    }
                    if (objUpdateAppointment.ToReferenceLogin_ID != 0)
                    {
                        objUpdateAppointment.ToReferenceLogin_ID = objUpdateAppointment.ToReferenceLogin_ID;
                    }
                    else
                    {
                        objUpdateAppointment.ToReferenceLogin_ID = null;
                    }
                    if (objUpdateAppointment.ToReference_ID != 0)
                    {
                        objUpdateAppointment.ToReference_ID = objUpdateAppointment.ToReference_ID;
                    }
                    else
                    {
                        objUpdateAppointment.ToReference_ID = null;
                    }
                    if (objUpdateAppointment.AppointmentID != null)
                    {
                        objUpdateAppointment.Appointment_ID = objUpdateAppointment.AppointmentID;
                    }
                    else
                    {
                        objUpdateAppointment.Appointment_ID = null;
                    }
                    if (objUpdateAppointment.AppointmentTracking_ID != 0)
                    {
                        objUpdateAppointment.AppointmentTracking_ID = objUpdateAppointment.AppointmentTracking_ID;
                    }
                    else
                    {
                        objUpdateAppointment.AppointmentTracking_ID = null;
                    }
                    objUpdateAppointment.FromReferenceType_ID = 2;
                    if (Convert.ToString(Session["roleid"]) != "1")
                    {
                        objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["Prov_ID"]);
                    }
                    else
                    {
                        objUpdateAppointment.FromReference_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                    }
                    objUpdateAppointment.FromReferenceLogin_ID = Convert.ToInt32(Session["UserID"]);
                    objUpdateAppointment.ToReferenceType_ID = 3;
                    if (objUpdateAppointment.FacilityReference_ID != 0)
                    {
                        objUpdateAppointment.FacilityReference_ID = objUpdateAppointment.FacilityReference_ID;
                    }
                    else
                    {
                        objUpdateAppointment.FacilityReference_ID = null;
                    }
                    if (!string.IsNullOrEmpty(objUpdateAppointment.Editfutreapnt_Ind))
                    {
                        if (objUpdateAppointment.Editfutreapnt_Ind == "Y")
                        {
                            objUpdateAppointment.Editfutreapnt_Ind = "Y";
                            objUpdateAppointment.IsValidate_Ind = "Y";
                        }
                        else if (objUpdateAppointment.Editfutreapnt_Ind == "N")
                        {
                            objUpdateAppointment.Editfutreapnt_Ind = "N";
                            objUpdateAppointment.IsValidate_Ind = "Y";
                        }
                        else if (objUpdateAppointment.Editfutreapnt_Ind == "Null")
                        {
                            objUpdateAppointment.Editfutreapnt_Ind = null;
                            objUpdateAppointment.IsValidate_Ind = "Y";
                        }
                        else
                        {
                            objUpdateAppointment.Editfutreapnt_Ind = null;
                            objUpdateAppointment.IsValidate_Ind = "Y";
                        }
                    }
                    else
                    {
                        objUpdateAppointment.Editfutreapnt_Ind = "N";
                        objUpdateAppointment.IsValidate_Ind = "N";
                    }
                    
                        objUpdateAppointment.VoiceFileName =!string.IsNullOrEmpty(Request["VoiceFileName"])?Request["VoiceFileName"]:null;
                 
                    objUpdateAppointment.GroupNo = null;
                    objUpdateAppointment.GroupInd = "N";
                    objUpdateAppointment.IntervalValue = strRecurenceValue;
                    objUpdateAppointment.NumofOccurrences = null;
                    if (objUpdateAppointment.AppointmentRecurrence_ID != 0)
                    {
                        objUpdateAppointment.AppointmentRecurrence_ID = objUpdateAppointment.AppointmentRecurrence_ID;
                    }
                    else
                    {
                        objUpdateAppointment.AppointmentRecurrence_ID = null;
                    }
                    if (string.IsNullOrEmpty(modify))
                    {
                        objUpdateAppointment.StartDate = !string.IsNullOrEmpty(objUpdateAppointment.StartDate) ? objUpdateAppointment.StartDate : strStartDate;
                    }
                    else if (modify1 == "Y")
                    {
                        //if (!string.IsNullOrEmpty(Request["Hdnapt_date"]))
                        //{
                           // if (Request["Hdnapt_date"] != strStartDate)
                           // {
                        if (StrDateChange == "Y")
                        {
                            objUpdateAppointment.StartDate = strStartDate;
                        }
                        else
                        {
                            objUpdateAppointment.StartDate = !string.IsNullOrEmpty(objUpdateAppointment.StartDate) ? objUpdateAppointment.StartDate : strStartDate;
                        }
                            //}
                       // }
                    }
                    else
                    {
                        objUpdateAppointment.StartDate =  strStartDate;
                    }
                    objUpdateAppointment.EndDate = dtEndOn;
                    objUpdateAppointment.SelectedWeekdays = SelectedDays;
                        objUpdateAppointment.Notes =!string.IsNullOrEmpty(objUpdateAppointment.Notes)? Sanitizer.GetSafeHtmlFragment(objUpdateAppointment.Notes):null;
                    objUpdateAppointment.AppointmentTime = ApptTime;
                    objUpdateAppointment.Duration = Convert.ToInt32(Request["ddlEDuration"]);
                    if (objUpdateAppointment.ApptType == "Client")
                    {
                        objUpdateAppointment.AppointmentType_ID = 1;
                    }
                    else
                    {
                        objUpdateAppointment.AppointmentType_ID = 2;
                    }
                    if (Convert.ToString(Session["roleid"]) != "39")
                    {
                        objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                    }
                    else
                    {
                        objUpdateAppointment.UpdatedBy = Convert.ToInt32(Session["Techlog_id"]);
                    }
                    objUpdateAppointment.Edit_Ind = "Y";
                    string reflogin = string.Empty;
                    string refeId = string.Empty;
                    //if (Request["combobox5"] != null & Request["combobox5"] != "")
                    //{
                    //    string techids = string.Join(",", Request["combobox5"].Split(',').Distinct().ToArray());
                    //    if (techids.EndsWith(","))
                    //    {
                    //        objUpdateAppointment.Technician_ids = "," + techids;
                    //    }
                    //    else
                    //    {
                    //        objUpdateAppointment.Technician_ids = "," + techids + ",";
                    //    }
                    //}
                    //else
                    //{
                    //    objUpdateAppointment.Technician_ids = "," + Request["hdntecid1"].ToString() + ",";
                    //}
                    //if (Request["hdntechpos"] != null & Request["hdntechpos"] != "")
                    //{
                    //    string techpos = string.Join(",", Request["hdntechpos"].Split(',').Distinct().ToArray());
                    //    if (techpos.EndsWith(","))
                    //    {
                    //        objUpdateAppointment.TechnicianPositions = "," + techpos;
                    //    }
                    //    else
                    //    {
                    //        objUpdateAppointment.TechnicianPositions = "," + techpos + ",";
                    //    }
                    //    //objUpdateAppointment.TechnicianPositions = Request["hdntechpos"];
                    //}
                    //else
                    //{
                    //    objUpdateAppointment.TechnicianPositions ="," +"0"+",";
                    //}
                    GetAppointment onjcheck = new GetAppointment();
                    string strDates = "";

                    if (objUpdateAppointment.ERecurring == "No Recurring")
                    {
                        strDates = DateTime.Parse(strStartDate).ToShortDateString();
                    }
                    else if (objUpdateAppointment.ERecurring == "Daily")
                    {
                        while (DateTime.Parse(strStartDate) <= DateTime.Parse(dtEndOn))
                        {
                            if (string.IsNullOrEmpty(strDates))
                            {
                                strDates = DateTime.Parse(strStartDate).ToShortDateString();
                            }
                            else
                            {
                                strDates += "," + DateTime.Parse(strStartDate).ToShortDateString();
                            }
                            //StartDate = DateAndTime.DateAdd(DateInterval.Day, Convert.ToInt32(txtDailyRecur.Text), appatdate);
                            strStartDate = DateTime.Parse(strStartDate).AddDays(Convert.ToInt32(strRecurenceValue)).ToShortDateString();
                        }
                    }
                    else if (objUpdateAppointment.ERecurring == "Weekly")
                    {
                        strDates = onjcheck.weeklyApptDates(strStartDate, strRecurenceValue, dtEndOn, SelectedDays);

                    }
                    string torefid = Convert.ToString(objUpdateAppointment.ToReference_ID);
                    if (modify == "Y")
                    {
                        strMessage = onjcheck.checkApptExist(strDates, torefid, ApptTime, Convert.ToString(Session["roleid"] != (object)1 ? Session["Prov_ID"] : Session["ComboProv_ID"]), Convert.ToString(objUpdateAppointment.AppointmentRecurrence_ID));
                    }
                    if (strMessage == "Y")
                    {
                            if (objUpdateAppointment.ApptType == "Client")
                            {
                                strMessage = "1";
                            }
                            else
                            {
                                strMessage = "2";
                            }
                            strMessage = strMessage.Replace("<br>", "\r\n");
                            return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                    }
                    if (ApptTime != Request["strTime"])
                    {

                        strMessage = onjcheck.checkApptExist(strDates, torefid, ApptTime, Convert.ToString(Session["roleid"] != (object)1 ? Session["Prov_ID"] : Session["ComboProv_ID"]), Convert.ToString(objUpdateAppointment.AppointmentRecurrence_ID));
                        if (strMessage == "Y")
                        {
                                if (objUpdateAppointment.ApptType == "Client")
                                {
                                    strMessage = "1";
                                }
                                else
                                {
                                    strMessage = "2";
                                }
                                strMessage = strMessage.Replace("<br>", "\r\n");
                                return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (modify != "Y" & StrDateChange=="N")
                    {
                        strMessage = objUpdateAppointment.EditAppointment(objUpdateAppointment, ref strFuturemessage, ref strChangeRecur_Ind);
                    }
                    else
                    {
                        if (objUpdateAppointment.ToReferenceLogin_ID == null)
                        {
                            reflogin = "0";
                        }
                        else
                        {
                            reflogin = Convert.ToString(objUpdateAppointment.ToReferenceLogin_ID);
                        }
                        if (objUpdateAppointment.ToReference_ID == null)
                        {
                            refeId = "0";
                        }
                        else
                        {
                            refeId = Convert.ToString(objUpdateAppointment.ToReference_ID);
                        }
                     //   objUpdateAppointment.ToReference_IDs = reflogin + "," + refeId + "," + Convert.ToString(objUpdateAppointment.ToReferenceType_ID) + ",$";
                        objUpdateAppointment.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        objUpdateAppointment.PlaceOfService_ID = Convert.ToInt32(Session["PlaceOfService_ID"]);
                       // objUpdateAppointment.Autoreminder_Ind = "N";
                       // objUpdateAppointment.phoneAutoreminder_Ind = "N";
                        if (objUpdateAppointment.Editfutreapnt_Ind == "N")
                        {
                            objUpdateAppointment.IsValidate_Ind = "N";
                            objUpdateAppointment.Edit_Ind = "N";
                            objUpdateAppointment.Editfutreapnt_Ind = "N";
                        }
                        if (objUpdateAppointment.ApptType == "Client")
                        {
                           // objUpdateAppointment.Autoreminder_Ind = "Y";
                            objUpdateAppointment.AppointmentType_ID = 1;
                            objUpdateAppointment.AppointmentStatus_ID = 1;
                        }
                        else
                        {
                            objUpdateAppointment.AppointmentType_ID = 2;
                            objUpdateAppointment.AppointmentStatus_ID = 2;
                        }
                        //if (Convert.ToInt32(TempData["ApptType"]) == 1)
                        //{
                        //    objUpdateAppointment.Autoreminder_Ind = "Y";

                        //}
                        //else
                        //{
                        //    objUpdateAppointment.AppointmentStatus_ID = Convert.ToInt32(TempData["ApptType"]);
                        //}
                        //GetAppointment onjcheck1 = new GetAppointment();
                        //string appatdate1 = strStartDate;
                        //if (Request["Recurring"] != null)
                        //{
                        //    appatdate1 = strDates;
                        //}
                        //string torefid1 = Request["hidPatientID"];
                        //string apptime1 = ApptTime;
                        //string prId1;
                        //if (Session["roleid"].ToString() != "1")
                        //{
                        //    prId1 = Session["Prov_ID"].ToString();
                        //}
                        //else
                        //{
                        //    prId1 = Session["ComboProv_ID"].ToString();
                        //}

                        //string apptrecid = string.Empty;

                        //if (recId != 0)
                        //{
                        //    apptrecid = Convert.ToString(recId);
                        //}
                        //strMessage = onjcheck1.checkApptExist(appatdate1, torefid1, apptime1, prId1, apptrecid);
                        //if (strMessage == "Y")
                        //{
                        //    if (strMessage == "Y")
                        //    {
                        //        if (Request["ApptType"] == "Client")
                        //        {
                        //            strMessage = "1";
                        //        }
                        //        else
                        //        {
                        //            strMessage = "2";
                        //        }
                        //        strMessage = strMessage.Replace("<br>", "\r\n");
                        //        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.DenyGet);
                        //    }
                        //}
                        // else
                        // {
                        if (Request["cancelappt"] != "Y")
                        {
                            strMessage = objUpdateAppointment.InsertAppointment(objUpdateAppointment, null);
                        }
                        if (strMessage == "")
                        {
                            if (objUpdateAppointment.ApptType == "Client")
                            {
                                DeleteRecurrenceappts(objUpdateAppointment.AppointmentRecurrence_ID); //recId);
                            }
                        }
                        // }
                    }
                    if (!string.IsNullOrEmpty(strMessage))
                    {
                        strMessage = strMessage.Replace("<br>", "\r\n");
                        return Json(JsonResponseFactory.ErrorResponse(strMessage), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strMessage) & (!string.IsNullOrEmpty(Request["cphone"])))
                        {
                            if (ApptTime != Request["strTime"] || strStartDate != Request["Hdnapt_date"])
                            {
                                if (objUpdateAppointment.ApptType == "Client")
                                {
                                    string strbussinessname = string.Empty;
                                    string strproviderphone = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
                                    {
                                        strbussinessname = Convert.ToString(Session["PracticeName"]);
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
                                    {
                                        strproviderphone = Convert.ToString(Session["Providerphone"]);
                                    }
                                    if (!string.IsNullOrEmpty(objUpdateAppointment.cellphone))
                                    {
                                        objUpdateAppointment.cellphone = objUpdateAppointment.cellphone.Replace("-", "");
                                    }
                                    clsCommonCClist.TwilioSms(strbussinessname, strproviderphone, objUpdateAppointment.cellphone, strStartDate, ApptTime, null, 0);
                                }
                            }
                        }
                        return Json(JsonResponseFactory.SuccessResponse(objUpdateAppointment), JsonRequestBehavior.AllowGet);
                    }
                 }
                    else
                    {
                        if (Request.IsAjaxRequest())
                        {

                            return RedirectToRoute("SessionExpirepopup");
                        }
                        else
                        {
                            return RedirectToRoute("SessionExpire");
                        }
                    }
            //    }
            //    else
            //    {
            //        return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //{
            //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            //}
        }
        private void DeleteRecurrenceappts(int? ApptRecurrID)
        {
            try
            {
                IDataParameter[] insparam ={
                                           new SqlParameter("@in_AppointmentRecurrence_ID",ApptRecurrID),
                                           new SqlParameter("@in_UpdatedBy",Convert.ToInt32(Session["UserID"]))
                                      };
                clsDBWrapper obj = new clsDBWrapper();
                obj.AddInParameters(insparam);
                obj.ExecuteNonQuerySP("help_dbo.st_Appointment_DEL_RecurrenceSchedule");
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "DeleteRecurrenceappts", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult ApptDelete(String apptDelid, String apptdate, String ToRefId, string enddate, string recId, string fromPage = null)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                DeleteAppointment objdelappt = new DeleteAppointment();
                objdelappt.apptDelid = apptDelid;
                objdelappt.apptdate = apptdate;
                objdelappt.ToRefId = ToRefId;
                objdelappt.enddate = enddate;
                objdelappt.recId = recId;
                objdelappt.fromPage = fromPage;
                return View(objdelappt);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpPost]
        public ActionResult ApptDelete(DeleteAppointment delappt)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    delappt.providerid = Convert.ToString(Session["Prov_ID"]);
                }
                else
                {
                    delappt.providerid = Convert.ToString(Session["ComboProv_ID"]);
                }
                delappt.futureInd = !string.IsNullOrEmpty(delappt.futureInd) ? delappt.futureInd : "N";
                delappt.recId = !string.IsNullOrEmpty(delappt.recId) ? delappt.recId : null;
                DateTime stdate = DateTime.Parse(delappt.apptdate);
                DateTime endate = new DateTime();
                if (delappt.enddate != " ")
                {
                    if (!string.IsNullOrEmpty(delappt.enddate))
                    {
                        endate = DateTime.Parse(delappt.enddate);
                    }
                    else
                    {
                        endate = stdate;
                    }
                }
                else
                {
                    endate = stdate;
                }
                delappt.getPagedate = DateTime.Parse(delappt.apptdate).ToShortDateString();
                if (delappt.futureInd != "Y" && delappt.futureInd != "C")
                {
                    if (stdate < endate)
                    {
                        return Json(JsonResponseFactory.ErrorResponse("Do you want to delete the Future Appointments"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        delappt.DeleteAppt(delappt, null, null);
                    }
                }
                else if (delappt.futureInd == "C")
                {
                    delappt.futureInd = "N";
                    delappt.DeleteAppt(delappt, null, null);
                }
                else
                {
                    delappt.DeleteAppt(delappt, null, null);
                }
                return Json(JsonResponseFactory.SuccessResponse(delappt), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult Printableview(string apptdate, string beginDate, string endDate, string ApptType, string techid = null, string technm = null)
        {
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                ViewBag.fullname = Session["FullName"];
            }
            else
            {
                ViewBag.fullname = Session["ComboProvFullName"];
            }
            if (techid != null && techid != "")
            {
                ViewBag.fullname = technm;
            }
            ViewBag.appttype = ApptType;
            GetDayViewList(apptdate, beginDate, endDate, ApptType, techid);
            return View();
        }
        private void GetDayViewList(string getdate, string begindate, string enddate, string ApptType, string techid)
        {
            try
            {
                //if (techid == "")
                //{
                //    techid = null;
                //}
                clsDBWrapper objWrapper = new clsDBWrapper();
                if (begindate == "null" || enddate == "null")
                {
                    begindate = null;
                    enddate = null;
                }
                if (getdate == "null")
                {
                    getdate = null;
                }
                if (begindate != null || enddate != null)
                {
                    begindate = DateTime.Parse(begindate).ToString("MM/dd/yyyy");
                    enddate = DateTime.Parse(enddate).ToString("MM/dd/yyyy");

                }
                if (ApptType == "Month")
                {
                    //DateTime FirstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    //DateTime LastDate = FirstDate.AddMonths(1).AddDays(-1);
                    //begindate = FirstDate.ToString("MM/dd/yyyy");
                    //enddate = LastDate.ToString("MM/dd/yyyy");
                    begindate = DateTime.Parse(begindate).AddMonths(1).ToString("MM/dd/yyyy");

                    enddate = DateTime.Parse(begindate).Month + "/" + DateTime.DaysInMonth(DateTime.Parse(begindate).Year, DateTime.Parse(begindate).Month) + "/" + DateTime.Parse(begindate).Year;
                }
                ViewBag.begindate = begindate;
                ViewBag.enddate = enddate;
                if (getdate != null)
                {
                    ViewBag.apptdate = getdate;
                }
                else
                {
                    ViewBag.apptdate =  begindate + " to " + enddate;
                }
                string providerid;
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    providerid = Convert.ToString(Session["Prov_ID"]);
                }
                else
                {
                    providerid = Convert.ToString(Session["ComboProv_ID"]);
                }
                IDataParameter[] DayViewInParamList ={new SqlParameter("@in_Provider_ID",providerid ),
                                                          new SqlParameter("@in_PlaceOfService_ID", null),
                                                          new SqlParameter("@in_ApptDate", getdate), 
                                                          new SqlParameter("@in_BeginDate", begindate),
                                                          new SqlParameter("@in_EndDate", enddate), 
                                                          new SqlParameter("@In_Technician_ids",!string.IsNullOrEmpty(techid)?techid:null),
                                                          new SqlParameter("@In_ShowAll", "Y")};
                objWrapper.AddInParameters(DayViewInParamList);
                DataSet dsAppointments = new DataSet();
                dsAppointments = objWrapper.GetDataSet("Help_dbo.st_Scheduling_List_PrintviewAppointments");
                StringBuilder strprnt = new StringBuilder("<table style='border-color:#111111; border-collapse:collapse;' cellspacing='1' cellpadding='4' width='90%' border='1' align='center'><tr><td width='10%' align='center'><strong>Appointment date</strong> </td><td width='10%' align='center'><strong>Appointment time</strong> </td>");
                //if (techid == null || techid == "")
                //{
                //    strprnt = strprnt.Append("<td width='10%' align='center'><strong>Coach name</strong> </td>");
                //}
                strprnt = strprnt.Append("<td width='5%' align='center'><strong> Duration</strong> </td><td width='20%' align='center'><strong>Client name</strong> </td><td width='15%' align='center'><strong>Court location name</strong> </td><td align='center'><strong>Address</strong> </td><td width='9%' align='center'><strong> Cell phone</strong> </td></tr>");
                foreach (DataRow dr in dsAppointments.Tables[0].Rows)
                {
                    string strphone = string.Empty;

                    if (!DBNull.Value.Equals(dr["phonenumber"]))
                    {
                        if (!string.IsNullOrEmpty(dr["phonenumber"].ToString()))
                        {
                            strphone = dr["phonenumber"].ToString();
                        }
                    }
                    string strdate = Convert.ToString(dr["AppointmentDate"]);
                    string[] strdate1 = strdate.Split(' ');
                    strprnt = strprnt.Append("<tr><td width='10%' align='center'>" + strdate1[0] + "</td><td width='10%' align='center'>" + dr["AppointmentTime"] + "</td>");
                    //if (techid == null || techid == "")
                    //{
                    //    if (Session["roleid"].ToString() != "1")
                    //    {
                    //        ViewBag.fullname = Session["FullName"];
                    //    }
                    //    else
                    //    {
                    //        ViewBag.fullname = Session["ComboProvFullName"];
                    //    }
                    //    strprnt = strprnt.Append("<td width='10%' align='center'><span>" + ViewBag.fullname + "</span> </td>");
                    //}
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Court_location_name"])))
                    {
                        strprnt = strprnt.Append("<td width='5%' align='center'>" + dr["Duration"] + "" + " Mins" + "</td><td width='20%' align='center'>" + dr["PatientName"] + "</td><td width='15%' align='center'>" + dr["Court_location_name"] + "</td><td align='center'>" + Convert.ToString(dr["Address"]).Replace("+", ",") + "</td><td width='9%' align='center'>" + strphone + "</td></tr>");
                    }
                    else
                    {
                        strprnt = strprnt.Append("<td width='5%' align='center'>" + dr["Duration"] + "" + " Mins" + "</td><td width='20%' align='center'>" + dr["PatientName"] + "</td><td width='15%' align='center'>-</td><td align='center'>" + Convert.ToString(dr["Address"]).Replace("+", ",") + "</td><td width='9%' align='center'>" + strphone + "</td></tr>");
                    }

                }

                strprnt = strprnt.Append("</table>");
                ViewBag.prntview = Convert.ToString(strprnt);
                ViewBag.GetDayViewList = dsAppointments.Tables[0].AsEnumerable().GetEnumerator();
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetDayViewList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult WeeklyAppts(string Nextclick, string previousClick, string weekstart, string timeinterval = null, string redirectind = null)
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            {
                ViewBag.Showtask = "Y";
            }
            else
            {
                ViewBag.Showtask = "N";
            }
            //string ddlschind = null;
            //clsWebConfigsettings cs = new clsWebConfigsettings();
            //ViewBag.src = cs.GetConfigSettingsValue("Islocal") == "Y" ? "https://maps.google.com/maps?file=api&v=3&key=ABQIAAAAi81Rjwr1-u7oEXnMu8ZAshSFiB3mIwTcvLSCB8Nf9sXhY0PL7hT-ysUkeccfB7NnkLj50hUFucSl7w" : "http://maps.google.com/maps?file=api&v=3&key=ABQIAAAAeznoROiGjKNjJuBg-sGaAhSj_VfuZaUqmXjgQ-jc6fA3gvU1eRScLYfjNHKJDPSP-99WON_NMwdG8A";
            if (Session["RoleID"] != null)
            {
                if (Session["RoleID"].ToString() == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            if (!string.IsNullOrEmpty(Request["hdnname"]))
            {

                TempData["combotext"] = Request["hdnname"];
            }
            else
            {
                TempData["combotext"] = "";
            }
            //Session.Add("TopIndex", "2");
            if (Convert.ToString(Session["Roleid"]) != "1" || Request["hdnPrid"] != null || TempData["hdnPrid"] != null)
            {

                ViewBag.noexist = Session["CCexists"];
                ViewBag.ccexpiry = Session["ccexpiry"];
                if (Convert.ToString(Session["Roleid"]) == "1")
                {
                    if (!string.IsNullOrEmpty(Request["hdnPrid"]) || !string.IsNullOrEmpty(Convert.ToString(TempData["hdnPrid"])))
                    {
                        ViewBag.Prselect = "Y";
                        string strPracid;
                        if (!string.IsNullOrEmpty(Request["hdnPrid"]))// || !string.IsNullOrEmpty(Convert.ToString(TempData["hdnPrid"])))
                        {
                            strPracid = Request["hdnPrid"];
                            TempData["hdnPrid"] = Request["hdnPrid"];
                            TempData["hdnpname"] = Request["hdnname"];
                            TempData.Keep("hdnPrid");
                        }
                        else
                        {
                            strPracid = Convert.ToString(TempData["hdnPrid"]);
                            TempData.Keep("hdnPrid");
                        }
                        //string[] strSplitPracid = strPracid.Split('$');
                        Session["ComboProv_ID"] = strPracid;
                        TempData["hdnPrid"] = Session["ComboProv_ID"];
                        //Session["ComboProv_ID"] =strSplitPracid[1] ;
                        //Session["ComboPractice_ID"] = strSplitPracid[0];
                        if (!string.IsNullOrEmpty(Request["hdnname"]))
                        {
                            Session["ComboProvFullName"] = Request["hdnname"];

                        }
                        else
                        {
                            Session["ComboProvFullName"] = TempData["hdnpname"];
                        }
                        TempData.Keep("hdnpname");
                        FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strPracid);
                    }
                    else
                    {
                        ViewBag.Prselect = null;
                    }
                }
            }
            //String Strtechid = null;
            //Strtechid = Request["Ddltech"];
            //if (Strtechid != null)
            //{
            //    ViewBag.techid = Strtechid;
            //}
            string fromDate = string.Empty;
            string endday = string.Empty;

            string startdate = string.Empty;
            int day;
            if (Nextclick != null)
            {
                startdate = DateTime.Parse(Nextclick).ToString("dd MMMM yyyy");
                day = (int)DateTime.Parse(Nextclick).DayOfWeek;
                ViewData["hdndate"] = Nextclick;
            }
            else if (previousClick != null)
            {
                startdate = DateTime.Parse(previousClick).AddDays(-6).ToString("dd MMMM yyyy");
                day = (int)DateTime.Parse(previousClick).AddDays(-6).DayOfWeek;
                ViewData["hdndate"] = previousClick;
            }
            else if (weekstart != null)
            {
                startdate = DateTime.Parse(weekstart).ToString("dd MMMM yyyy");
                day = (int)DateTime.Parse(weekstart).DayOfWeek;
                ViewData["hdndate"] = weekstart;
            }
            else
            {
                startdate = DateTime.Now.ToString("dd MMMM yyyy");
                day = (int)DateTime.Now.DayOfWeek;
            }

            Fillallappointments(startdate, "Y", null);
            switch (day)
            {
                case 0:
                    ViewBag.BeginDate = startdate;
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+6).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+6).ToShortDateString();
                    break;
                case 1:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-1).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+5).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-1).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+5).ToShortDateString();
                    break;
                case 2:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-2).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+4).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-2).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+4).ToShortDateString();
                    break;
                case 3:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-3).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+3).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-3).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+3).ToShortDateString();
                    break;
                case 4:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-4).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+2).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-4).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+2).ToShortDateString();
                    break;
                case 5:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-5).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).AddDays(+1).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-5).ToShortDateString();
                    endday = DateTime.Parse(startdate).AddDays(+1).ToShortDateString();
                    break;
                case 6:
                    ViewBag.BeginDate = DateTime.Parse(startdate).AddDays(-6).ToString("dd MMMM yyyy");
                    ViewBag.EndDate = DateTime.Parse(startdate).ToString("dd MMMM yyyy");
                    fromDate = DateTime.Parse(startdate).AddDays(-6).ToShortDateString();
                    endday = DateTime.Parse(startdate).ToShortDateString();
                    break;
            }
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                //if (Request["techname"] != null && Request["techname"] != "" && Request["techname"] != "All")
                //{
                //    ViewBag.practicename = Request["techname"];
                //}
                //else
                //{
                    ViewBag.practicename = Session["PracticeName"];
                //}

                ViewBag.practicerangedates = "  from  " + ViewBag.BeginDate + "  -  " + ViewBag.EndDate;
            }
            else
            {
                ViewBag.practicename = Session["ComboProvFullName"];
                ViewBag.practicerangedates = "  from  " + ViewBag.BeginDate + "  -  " + ViewBag.EndDate;
            }
            ViewData["Next"] = DateTime.Parse(endday).AddDays(+1).ToShortDateString();
            ViewData["Previous"] = DateTime.Parse(fromDate).AddDays(-1).ToShortDateString();

            ViewData["Previoustooltip"] = DateTime.Parse(fromDate).AddDays(-7).ToShortDateString() + " To " + DateTime.Parse(fromDate).AddDays(-1).ToShortDateString();
            ViewData["Nexttooltip"] = DateTime.Parse(endday).AddDays(+1).ToShortDateString() + " To " + DateTime.Parse(endday).AddDays(+7).ToShortDateString();

            string[] splitstart = fromDate.Split('/');
            int loopstart = Convert.ToInt32(splitstart[1]);
            int loopMonth = Convert.ToInt32(splitstart[0]);
            string[] splitend = endday.Split('/');
            int loopend;
            loopend = loopstart + 6;
            string provideid = null;
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                if (Session["Prov_ID"] != null)
                {
                    provideid = Convert.ToString(Session["Prov_ID"]);

                }
                else
                {
                    provideid = Convert.ToString(Session["ProviderID"]);
                }
            }
            else
            {
                provideid = Convert.ToString(Session["ComboProv_ID"]);
            }
            DataSet dset = null;
            IDataParameter[] objparm = {
			new SqlParameter("@In_Provider_ID", provideid),
            new SqlParameter("@in_Appointmentfromdate", fromDate),
            new SqlParameter("@in_Appointmenttodate", endday)
		};
            objCommon.AddInParameters(objparm);
            dset = objCommon.GetDataSet("Help_dbo.ST_GET_Appointment_EXIST");
            if (dset.Tables.Count > 0)
            {
                ViewBag.ddlschind =  Convert.ToString(dset.Tables[0].Rows[0]["AppointmentExists"]);
               // ViewBag.ddlschind = ddlschind;
            }
            if (timeinterval == null || redirectind != null)
            {
                if (ViewBag.ddlschind == "Y")
                {
                    timeinterval = "12:00-12:00";
                }
                else
                {
                    timeinterval = "07:00-07:00";
                }
            }
            //if (Session["roleid"].ToString() != "1")
            //{
            //    Session["PlaceOfService_ID"] = Session["PrimaryPOSID"];
            //}
            //else
            //{
            //    FillPlaceofServices(Convert.ToInt32(Session["ComboProv_ID"]), Convert.ToInt32(Session["ComboPractice_ID"]));
            //}
            //string placeofservice_id = Session["PlaceOfService_ID"].ToString();

            if (timeinterval == "12:00-12:00")
            {
                ViewBag.timeinterval = timeinterval;
                GetDayAppointments("Y", startdate);
            }
            else if (timeinterval == "07:00-07:00")
            {
                ViewBag.timeinterval = timeinterval;
                GetDayAppointments("N", startdate);
            }
            else
            {
                GetDayAppointments("N", startdate);
            }
            if (ViewBag.Showtask == "Y")
            {
                GetTodayTask(startdate);
            }
            //if (weekstart != null)
            //{
            //    Fillallappointments(weekstart, "Y");
            //    ViewData["hdndate"] = weekstart;
            //}
            //else
            //{
            //    Fillallappointments(startdate, "Y");

            //}


        //    DataTable dt = new DataTable();
            DataSet dsWeekApptList = new DataSet();
            //string Tech_Id = null;
            //if (Strtechid != null && Strtechid != "")
            //{
            //    Tech_Id = "," + Convert.ToString(Strtechid) + ",";
            //}
            dsWeekApptList = GetAppointment.GetWeekAppointments(provideid, ViewBag.BeginDate, ViewBag.EndDate, null);
            string firstappttime = null;
            if (dsWeekApptList.Tables[0].Rows.Count > 0)
            {

                firstappttime = Convert.ToString(dsWeekApptList.Tables[0].Rows[0]["AppointmentTime"]);

                if (firstappttime.Contains("AM"))
                {
                    string[] apptime = firstappttime.Split(':');
                    int appt1 = Convert.ToInt32(apptime[0]);
                    if (appt1 < 7)
                    {
                        ViewBag.scrolind = "1";
                    }
                }
                if (firstappttime.Contains("PM"))
                {
                    string[] apptime = firstappttime.Split(':');
                    int appt1 = Convert.ToInt32(apptime[0]);
                    if (appt1 > 7)
                    {
                        ViewBag.scrolind = "2";
                    }
                }
            }
            string applistdate = string.Empty;
            System.Text.StringBuilder StrDisplay = new System.Text.StringBuilder();
            string backcolorTr = string.Empty;


            StringBuilder strdayappt = new StringBuilder();
            strdayappt =
                strdayappt.Append(
                    "<table id='tblAppointments' cellspacing='1' cellpadding='4' width='100%'  style='background-color:#d0e4f4'>");
            int j = 0;
            string strformat = string.Empty;
            string strhigh = string.Empty;
            strdayappt = strdayappt.Append(" <tr>");
            strdayappt = strdayappt.Append("<td style='width:11%; text-align:center'><select id='lnkInterPlus' style='width: 120px; height: 25px; font-size: 13px;'><option value='07:00-07:00'>7AM/7PM</option><option value='12:00-12:00'>12AM/12PM</option></select></td>");
            for (int i = loopstart; i <= loopend; i++)
            {

                if (i == loopstart)
                {
                    applistdate = fromDate;

                    strformat = DateTime.Parse(applistdate).ToString("dddd - MM/dd");
                }
                else
                {
                    applistdate = DateTime.Parse(fromDate).AddDays(j).ToShortDateString();
                    strformat = DateTime.Parse(applistdate).ToString("dddd - MM/dd");
                }
                strhigh += applistdate;
                strhigh += ",";
                j++;
                if (applistdate == DateTime.Now.ToShortDateString())
                {
                    backcolorTr = "#C4D23A";
                }
                else
                {
                    backcolorTr = null;
                }
                strdayappt = strdayappt.Append("<td align='center' style='background-color: " + backcolorTr + ";font-weight:bold;width:12%;'><a href='../Schedule/Schedulespecs?apptdate=" + applistdate + "&timeinterval=" + "07:00-07:00" + "'  title='Click here to add the Appointment'> " + strformat + "</a></td>");
            }
            strdayappt.Append(" </tr>");

            ViewBag.weekhigh = strhigh;
            foreach (DataRow drappt in dsAppointmentsList.Tables[0].Rows)
            {
                string timperiod = string.Empty;
                string rowspantr = string.Empty;
                string backcolorTd = string.Empty;
                if (drappt["Period"] != null)
                {
                    timperiod = Convert.ToString(drappt["Period"]).Replace(":", "-");
                    //timperiod = timperiod.Replace(":", "-");
                }
                strdayappt =
                    strdayappt.Append("<tr id='trAppointments' style='height:20px;'   >");
                strdayappt =
                    strdayappt.Append("<td id='tcIntervel' style='width: 10%;font-weight:bold;' align='center' valign='middle' ><label>" + timperiod + "</label></td>");
                string chkdatetime = null;
                if (dsWeekApptList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsWeekApptList.Tables[0].Rows)
                    {
                        string chkdate = Convert.ToString(dr["AppointmentDate"]);
                        chkdate = DateTime.Parse(chkdate).ToShortDateString();
                        string chktime = Convert.ToString(dr["AppointmentTime"]).Replace(":", "-");
                       // string chkdatetime2 = chkdate + chktime;
                        chkdatetime = chkdatetime + "," + chkdate + chktime;

                    }
                }

                for (int i = 0; i <= 6; i++)
                {
                    if (i == 0)
                    {
                        applistdate = fromDate;
                    }
                    else
                    {
                        applistdate = DateTime.Parse(fromDate).AddDays(i).ToShortDateString();
                    }
                    if (applistdate == DateTime.Now.ToShortDateString())
                    {
                        backcolorTr = "#C4D23A";
                    }
                    else
                    {
                        backcolorTr = null;
                    }
                    strdayappt = strdayappt.Append("<td style='width: 10%;height:20px; background-color: " + backcolorTr + "'>");
                    if (dsWeekApptList.Tables[0].Rows.Count > 0)
                    {
                        strdayappt = strdayappt.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                        string chkdatetime1 = applistdate + timperiod;
                        string aptid = null;
                       // string apttime = null;
                       // string upto = null;
                        //string strname = null;
                       // string toref = null;
                        //string strdate = null;
                        if (chkdatetime.Contains(chkdatetime1))
                        {
                            strdayappt = strdayappt.Append("<tr style='height:20px; background-color: " + backcolorTr + "'>");
                            strdayappt = strdayappt.Append("<td width='10%' align='center' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return addAppt(\"" + applistdate + "\",\"" + timperiod + "\")' title='Click here to add the Appointment'>+</a></td>");
                            strdayappt = strdayappt.Append("</tr>");
                            if (dsWeekApptList.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dsWeekApptList.Tables[0].Rows)
                                {
                                    string aptdate = Convert.ToString(dr["AppointmentDate"]);
                                    aptdate = DateTime.Parse(aptdate).ToShortDateString();
                                    if (aptdate == applistdate & Convert.ToString(dr["AppointmentTime"]).Replace(":", "-") == timperiod)
                                    {
                                        aptid = Convert.ToString(dr["Appointment_ID"]);
                                       // apttime = Convert.ToString(dr["Appointment_ID"]);
                                        //upto = Convert.ToString(dr["UptoAppointmentTime"]);
                                        ViewBag.custname = Convert.ToString(dr["FullName"]);
                                        //toref = Convert.ToString(dr["ToReference_ID"]);
                                      //  ViewBag.custname = Convert.ToString(dr["FullName"]);
                                       // strdate = aptdate;
                                       // string technames = null;
                                       // if (!string.IsNullOrEmpty(Convert.ToString(dr["Technicianname"])))//!= null & Convert.ToString(dr["Technicianname"]) != "")
                                       // {
                                           // technames = "Technicians : " + Convert.ToString(dr["Technicianname"]);
                                       // }
                                        //else
                                        ///{
                                          //  technames = "Click here to Edit appointment";
                                       // }
                                        strdayappt = strdayappt.Append("<tr style='height:20px; background-color: " + backcolorTr + "'>");
                                        if (ViewBag.custname != "Block Appointment")
                                        {
                                            strdayappt = strdayappt.Append("<td width='100%' align='left'><a class='EditApptclass'  href='#' onclick='return EditAppt(\"" + aptid + "\",\"" + dr["phonenumber"] + "\")'   title='Click here to Edit appointment'>" + ViewBag.custname + "</a><br/>&nbsp;&nbsp;<a class='showmap' onclick='javascript:return fnmaplanglat(" + aptid + ");' style='color:white;font-size:13px;' href='#'    title='Click here to show appointment location' ><img alt='searchPage' style = 'background:  no-repeat ; vertical-align:middle; vertical-align:middle; width:24px; height:24px;' src='../Images/Map1.gif'  /></a>" + "&nbsp;<a class='showcustmtransactions' onclick='javascript:return fncustmtransactions(" + dr["ToReference_ID"] + "," + dr["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to add transactions.' >" + "<img src='../Images/currency_dollar_green.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" + "</td>");
                                        }
                                        else
                                        {
                                            strdayappt = strdayappt.Append("<td width='100%' align='left'><a class='EditApptclass'  href='#" + "' onclick='return EditAppt(\"" + aptid + "\",\"" + dr["phonenumber"] + "\")'  title='Click here to Edit appointment'>" + ViewBag.custname + "</a></td>");
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            strdayappt = strdayappt.Append("<tr style='height:20px; background-color: " + backcolorTr + "'>");
                            strdayappt = strdayappt.Append("<td width='10%' align='center' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return addAppt(\"" + applistdate + "\",\"" + timperiod + "\")' title='Click here to add the Appointment'>+</a></td>");
                        }
                        strdayappt = strdayappt.Append("</tr></table>");
                    }
                    else
                    {

                        strdayappt = strdayappt.Append("<table width='100%' cellspacing='0' cellpadding='0' style='background-color:#d0e4f4'><tr style='height:20px; background-color: " +
                                      backcolorTr + "'>");
                        strdayappt = strdayappt.Append("<td width='10%' align='center' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return addAppt(\"" + applistdate + "\",\"" + timperiod + "\")' title='Click here to add the Appointment'>+</a></td>");
                        strdayappt = strdayappt.Append("</tr></table>");
                    }
                    strdayappt = strdayappt.Append("</td>");

                }
            }
            strdayappt = strdayappt.Append("</td></tr>");
            strdayappt = strdayappt.Append("</table>");
            ViewBag.daysch = Convert.ToString(strdayappt);


    //        List<TechniciansInfo> TechnicianList = new List<TechniciansInfo>();          
    //        IDataParameter[] objparam = {
    //    new SqlParameter("@in_provider_id",Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"])),
    //    new SqlParameter("@in_appointment_id", null )
    //};
    //        objCommon.AddInParameters(objparam);
    //        SqlDataReader objread = default(SqlDataReader);
    //        objread = objCommon.GetDataReader("Help_dbo.st_get_technician_names_list");
    //        IList<SelectListItem> _result1 = new List<SelectListItem>();
    //        int counter = 0;
    //        while (objread.Read())
    //        {

    //            _result1.Add(new SelectListItem
    //            {
    //                Text = Convert.ToString(objread["name"]),
    //                Value = Convert.ToString(objread["Technician_id"])
    //            });
    //            counter++;
    //        }
    //        ViewBag.tech_count = Convert.ToString(counter);

    //        StateCity reg1 = new StateCity();
    //        reg1 = new StateCity
    //        {
    //            Techlist = _result1
    //        };
            return View();
          //return View(reg1);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public JsonResult Getaddress(int patid)
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            Patient_Info obj = new Patient_Info();

            if (Convert.ToString(Session["roleid"]) != "1")
            {
                obj.PracticeID = Convert.ToString(Session["Practice_ID"]);
                obj.ProviderID = Convert.ToString(Session["Prov_ID"]);
            }
            else
            {
                obj.PracticeID = Convert.ToString(Session["ComboProv_ID"]);
                obj.ProviderID = Convert.ToString(Session["ComboProv_ID"]);
            }
            //obj.PracticeID = Session["Practice_ID"].ToString();
            obj.PatientID = Convert.ToString(patid);
            obj = Patient_Info.ViewPatineInfo(obj);
            obj.l_GoogleMapPath = obj.l_GoogleMapPath.Replace("+", ",");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public JsonResult GetOtherAddressDetails(int? AppAddressId, int? custId)
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            Patient_Info otherdetails = new Patient_Info();
            //clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@In_Provider_id",Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"])),
        new SqlParameter("@In_customer_id",custId!=0?custId:null),
        new SqlParameter("In_Appointment_address_id",AppAddressId!=0?AppAddressId:null)
	};
            objCommon.AddInParameters(objparam);
            SqlDataReader objread = default(SqlDataReader);
            objread = objCommon.GetDataReader("Help_dbo.St_get_appointment_adress");
            while (objread.Read())
            {

                //if (Convert.ToString(objread["Address1"]).Contains(','))
                //{
                //    if (objread["Address1"] != null)
                //    {
                //        string[] strsplitaddress1 = Convert.ToString(objread["Address1"]).Split(',');
                //        if (strsplitaddress1.Length >= 3)
                //        {
                //            ViewBag.changedAddress1 = strsplitaddress1[0];
                //            otherdetails.Address1 = strsplitaddress1[0];
                //            otherdetails.CourtLocationName = Convert.ToString(strsplitaddress1[2]);
                //            if (objread["Default_address"] != null)
                //            {
                //                otherdetails.Default_addr = Convert.ToString(objread["Default_address"]);
                //            }
                //            else
                //            {
                //                otherdetails.Default_addr = null;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    otherdetails.Address1 = Convert.ToString(objread["Address1"]);
                //    otherdetails.CourtLocationName = "";
                //}
                if (objread["Address1"] != null)
                {
                    otherdetails.Address1 = Convert.ToString(objread["Address1"]);
                }
                if (objread["CourtLocationName"] != null)
                {
                    otherdetails.CourtLocationName = Convert.ToString(objread["CourtLocationName"]);
                }
                if (objread["Default_address"] != null)
                {
                    otherdetails.Default_addr = Convert.ToString(objread["Default_address"]);
                }
                else
                {
                    otherdetails.Default_addr = null;
                }
                otherdetails.cityzip = Convert.ToString(objread["cityzip"]);
                otherdetails.State = Convert.ToString(objread["statename"]);
            }
            return Json(otherdetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult GetMapAddressDetails(string appointid)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                //if (Session["UserID"] == null)
                //{
                //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
                //}
                Patient_Info otherdetails = new Patient_Info();
                //clsCommonFunctions objcommon = new clsCommonFunctions();
                IDataParameter[] objparam = {
		new SqlParameter("@In_appointment_id",appointid)        
	};
                objCommon.AddInParameters(objparam);
                SqlDataReader objread = default(SqlDataReader);
                objread = objCommon.GetDataReader("Help_dbo.ST_schedule_appointments_googlepath");
                while (objread.Read())
                {
                    otherdetails.l_GoogleMapPath = Convert.ToString(objread["l_GoogleMapPath"]).Replace("+", ",");
                    otherdetails.CourtLocationName = Convert.ToString(objread["Court_location_name"]);
                }
                return Json(otherdetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {

                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult ViewProvMonthlySchedule(string nextMonth, string PrevMonth)
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            {
                ViewBag.Showtask = "Y";
            }
            else
            {
                ViewBag.Showtask = "N";
            }
            if (Session["RoleID"] != null)
            {
                if (Session["RoleID"].ToString() == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                }
            }
            if (!string.IsNullOrEmpty(Request["hdnname"]))
            {

                TempData["combotext"] = Request["hdnname"];
            }
            else
            {
                TempData["combotext"] = "";
            }
            //Session.Add("TopIndex", "2");
            if (Convert.ToString(Session["Roleid"]) != "1" || Request["hdnPrid"] != null || TempData["hdnPrid"] != null)
            {

                ViewBag.noexist = Session["CCexists"];
                ViewBag.ccexpiry = Session["ccexpiry"];
                if (Convert.ToString(Session["Roleid"]) == "1")
                {
                    if (!string.IsNullOrEmpty(Request["hdnPrid"]) || !string.IsNullOrEmpty(Convert.ToString(TempData["hdnPrid"])))
                    {
                        ViewBag.Prselect = "Y";
                        string strPracid;
                        if (!string.IsNullOrEmpty(Request["hdnPrid"]))// || !string.IsNullOrEmpty(Convert.ToString(TempData["hdnPrid"])))
                        {
                            strPracid = Request["hdnPrid"];
                            TempData["hdnPrid"] = Request["hdnPrid"];
                            TempData["hdnpname"] = Request["hdnname"];
                            TempData.Keep("hdnPrid");
                        }
                        else
                        {
                            strPracid = Convert.ToString(TempData["hdnPrid"]);
                            TempData.Keep("hdnPrid");
                        }
                        //string[] strSplitPracid = strPracid.Split('$');
                        Session["ComboProv_ID"] = strPracid;
                        TempData["hdnPrid"] = Session["ComboProv_ID"];
                        //Session["ComboProv_ID"] =strSplitPracid[1] ;
                        //Session["ComboPractice_ID"] = strSplitPracid[0];
                        if (!string.IsNullOrEmpty(Request["hdnname"]))
                        {
                            Session["ComboProvFullName"] = Request["hdnname"];

                        }
                        else
                        {
                            Session["ComboProvFullName"] = TempData["hdnpname"];
                        }
                        TempData.Keep("hdnpname");
                        FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strPracid);
                    }
                    else
                    {
                        ViewBag.Prselect = null;
                    }
                }
            }
            //String Strtechid = null;
            //Strtechid = Request["Ddltech"];
            //if (Strtechid != null)
            //{
            //    ViewBag.techid = Strtechid;
            //}

            DateTime firstdate;
            DateTime lastdate;
            DateTime dt;
            if (nextMonth != null)
            {
                dt = Convert.ToDateTime(nextMonth);

            }
            else if (PrevMonth != null)
            {
                dt = Convert.ToDateTime(PrevMonth);
            }
            else
            {
                dt = DateTime.Now;
            }
            string presMonth = dt.ToString("MMMM  yyyy");
            string selcMonth = dt.Month.ToString();
            string selcYear = dt.Year.ToString();
            firstdate = new DateTime(dt.Year, dt.Month, 1);
            int firstloop = firstdate.Day;
            int firstday = (int)firstdate.DayOfWeek;
            firstday = firstday + 1;

            dt = firstdate.AddMonths(1);

            lastdate = dt.AddDays(-1);
            int lastloop = lastdate.Day;
            if (firstday > 1)
            {
                lastloop += (firstday);
            }
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                //if (Request["techname"] != null && Request["techname"] != "" && Request["techname"] != "All")
                //{
                //    ViewBag.practicename = Request["techname"];
                //}
                //else
                //{
                    ViewBag.practicename = Session["PracticeName"];
               // }
                ViewBag.practicerangedates = "  for the Month of  " + presMonth;
            }
            else
            {
                ViewBag.practicename = "Appointments of" + " " + Session["ComboProvFullName"] + "  for the Month of  " + presMonth;
            }
            StringBuilder strMonth = new StringBuilder();
            strMonth = strMonth.Append("<table id='monthTable' border='0' cellpadding='0' cellspacing='1' width='100%' class='border_style'>");
            strMonth = strMonth.Append("<tr style='height:40px'><td style='width:14.28%; text-align:center'><strong>Sunday</strong></td><td style='width:14.28%; text-align:center'><strong>Monday</strong></td>");
            strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><strong>Tuesday</strong></td><td style='width:14.28%; text-align:center'><strong>Wednesday</strong></td>");
            strMonth = strMonth.Append(" <td style='width:14.28%; text-align:center'><strong>Thursday</strong></td>");
            strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><strong>Friday</strong></td>");
            strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><strong>Saturday</strong></td></tr>");
            strMonth = strMonth.Append("<tr>");
            string provideid = null;
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                if (Session["Prov_ID"] != null)
                {
                    provideid = Session["Prov_ID"].ToString();

                }
                else
                {
                    provideid = Convert.ToString(Session["ProviderID"]);
                }
            }
            else
            {
                provideid = Convert.ToString(Session["ComboProv_ID"]);
            }
            //if (Session["roleid"].ToString() != "1")
            //{
            //    Session["PlaceOfService_ID"] = Session["PrimaryPOSID"];
            //}
            //else
            //{
            //    FillPlaceofServices(Convert.ToInt32(Session["ComboProv_ID"]), Convert.ToInt32(Session["ComboPractice_ID"]));
            //}
            //string placeofservice_id = Session["PlaceOfService_ID"].ToString();

            DataSet dsMonthApptList = new DataSet();
            //string str_techid = null;
            //if (Strtechid != null && Strtechid != "")
            //{
            //    str_techid = "," + Strtechid + ",";
            //}
            dsMonthApptList = GetAppointment.GetMonthAppointments(provideid, selcMonth, selcYear, null, null);

            int intcount;
            int intDate = 1;
            string strDateFormat = string.Empty;
            string strDate = string.Empty;
            for (intcount = firstloop; intcount < lastloop; intcount++)
            {
                if (intcount >= firstday)
                {
                    if (intDate < 10)
                    {
                        strDate = "0" + Convert.ToString(intDate);
                    }
                    else
                    {
                        strDate = Convert.ToString(intDate);
                    }
                    if (intcount == firstday)
                    {
                        strDateFormat = firstdate.ToShortDateString();
                    }
                    else
                    {
                        strDateFormat = firstdate.AddDays(intDate - 1).ToShortDateString();
                    }

                    if (dsMonthApptList.Tables[0].Select("AppointmentDate = '" + strDateFormat + "'").Length > 0)
                    {
                        if ((intcount % 7) == 0)
                        {
                            if (DateTime.Now.ToShortDateString() == strDateFormat)
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;overflow-y: scroll;border: 1px solid Thistle;z-index:1; background:#C4D23A;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");' title='Click here to add the Appointment'>+</a></td></tr>");
                            }
                            else
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;overflow-y: scroll;border: 1px solid Thistle;z-index:1;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr>");
                            }

                        }
                        else
                        {
                            if (DateTime.Now.ToShortDateString() == strDateFormat)
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;overflow-y: scroll;border: 1px solid Thistle;z-index:1; background:#C4D23A;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr>");
                            }
                            else
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;overflow-y: scroll;border: 1px solid Thistle;z-index:1;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr>");
                            }
                        }
                        foreach (DataRow dr in dsMonthApptList.Tables[0].Select("AppointmentDate = '" + strDateFormat + "'"))
                        {
                            //string technames = null;
                            //if (!string.IsNullOrEmpty(Convert.ToString(dr["Technicianname"])))// != null & dr["Technicianname"]) != "")
                            //{
                            //    technames = "Technicians : " + dr["Technicianname"].ToString();
                            //}
                            //else
                            //{
                               // technames = "Click here to Edit appointment";
                            //}
                            if ((intcount % 7) == 0)
                            {

                                strMonth = strMonth.Append("<tr><td class='bluemenu' valign='top' style='text-align:left'><a class='EditApptclass' href='#' onclick='return EditAppt(\"" + dr["Appointment_ID"] + "\",\"" + dr["phonenumber"] + "\");'   title='Click here to Edit appointment' >" + dr["AppointmentTime"] + " </a> <a class='EditApptclass'  href='#' onclick='return EditAppt(\"" + dr["Appointment_ID"] + "\",\"" + null + "\");'  title='Click here to Edit appointment' >" + dr["Patientname"] + "&nbsp;&nbsp;" + dr["phonenumber"] + "</a></td></tr>");
                            }
                            else
                            {
                                strMonth = strMonth.Append("<tr><td class='bluemenu' valign='top' style='text-align:left'><a class='EditApptclass' href='#' onclick='return EditAppt(\"" + dr["Appointment_ID"] + "\",\"" + dr["phonenumber"] + "\");'   title='Click here to Edit appointment' >" + dr["AppointmentTime"] + " </a> <a class='EditApptclass'  href='#' onclick='return EditAppt(\"" + dr["Appointment_ID"] + "\",\"" + null + "\");'  title='Click here to Edit appointment' >" + dr["Patientname"] + "&nbsp;&nbsp;" + dr["phonenumber"] + "</a></td></tr>");
                            }
                        }
                        if ((intcount % 7) == 0)
                        {
                            strMonth = strMonth.Append("</TABLE></div> </td></tr><tr>");
                        }
                        else
                        {
                            strMonth = strMonth.Append("</TABLE></div> </td>");
                        }
                    }
                    else
                    {
                        if ((intcount % 7) == 0)
                        {
                            if (DateTime.Now.ToShortDateString() == strDateFormat)
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;border: 1px solid Thistle;z-index:1; background:#C4D23A;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr></TABLE></div> </td></tr><tr>");
                            }
                            else
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;border: 1px solid Thistle;z-index:1;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr></TABLE></div> </td></tr><tr>");
                            }
                        }
                        else
                        {
                            if (DateTime.Now.ToShortDateString() == strDateFormat)
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;border: 1px solid Thistle;z-index:1; background:#C4D23A;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr></TABLE></div> </td>");
                            }
                            else
                            {
                                strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;border: 1px solid Thistle;z-index:1;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign='top' style='text-align:left'>" + strDate + "</td></tr><tr><td class='bluemenu' valign='top' style='font-size:x-large;'><a class='schedulemenu' href='#' onclick='return AddAppt(\"" + strDateFormat + "\");'  title='Click here to add the Appointment'>+</a></td></tr></TABLE></div> </td>");
                            }
                        }
                    }
                    intDate++;
                }
                else
                {
                    strMonth = strMonth.Append("<td style='width:14.28%; text-align:center'><div name='divTD' id=divTD style='position:relative;height:140px;width:100%;border: 1px solid Thistle;z-index:1;' runat='server'><TABLE id='tbl' width='100%'><tr><td class='bluemenu' valign=top></td></tr><tr><td class='bluemenu' valign='top'></td></tr></TABLE></div></td>");
                }

            }
            strMonth = strMonth.Append("</tr></table>");


            ViewBag.monthtab = Convert.ToString(strMonth);
            ViewData["nextMonth"] = DateTime.Parse(lastdate.ToShortDateString()).AddDays(+1).ToShortDateString();
            ViewData["PrevMonth"] = DateTime.Parse(firstdate.ToShortDateString()).AddMonths(-1).ToShortDateString();
            ViewBag.NMtext = DateTime.Parse(lastdate.ToShortDateString()).AddDays(+1).ToString("MMMM");
            ViewBag.PMtext = DateTime.Parse(firstdate.ToShortDateString()).AddMonths(-1).ToString("MMMM");
            if (ViewBag.Showtask == "Y")
            {
                if (nextMonth != null)
                {
                    GetTodayTask(nextMonth);
                }
                else
                {
                    GetTodayTask(DateTime.Now.ToShortDateString());
                }
            }
            return View();
    //        List<TechniciansInfo> TechnicianList = new List<TechniciansInfo>();
    //        IDataParameter[] objparam = {
    //    new SqlParameter("@in_provider_id", Convert.ToString(Session["roleid"]) != "1"? Session["Prov_id"]:Convert.ToString(Session["ComboProv_ID"])),
    //    new SqlParameter("@in_appointment_id", null )
    //};
    //        objCommon.AddInParameters(objparam);
    //        SqlDataReader objread = default(SqlDataReader);
    //        objread = objCommon.GetDataReader("Help_dbo.st_get_technician_names_list");
    //        IList<SelectListItem> _result1 = new List<SelectListItem>();
    //        int counter = 0;
    //        while (objread.Read())
    //        {

    //            _result1.Add(new SelectListItem
    //            {
    //                Text = Convert.ToString(objread["name"]),
    //                Value = Convert.ToString(objread["Technician_id"])
    //            });
    //            counter++;
    //        }
    //        ViewBag.tech_count = Convert.ToString(counter);

    //        StateCity reg1 = new StateCity();
    //        reg1 = new StateCity
    //        {
    //            Techlist = _result1
    //        };
    //        return View(reg1);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult ViewYearlySchedule()
        {


            ViewBag.weekhigh = DateTime.Now.ToShortDateString();
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult Schedulespecs(string timeinterval = null, string apptdate = null, string redirectind = null)
        {
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            {
                ViewBag.Showtask = "Y";
            }
            else
            {
                ViewBag.Showtask = "N";
            }
          //  string ddlschind = null;
            //string appointmentdt1 = null;
            //if (apptdate != null)
            //{
            //    appointmentdt1 = apptdate;
            //}
            //else
            //{
            //    appointmentdt1 = DateTime.Now.ToShortDateString();
            //}
            if (Convert.ToString(Session["roleid"]) != "1")
            {
               // if (Request["techname"] != null && Request["techname"] != "" && Request["techname"] != "All")
                //{
                   // ViewBag.practicename = Request["techname"];
               // }
               // else
               // {
                    ViewBag.practicename = Session["PracticeName"];
               // }

                ViewBag.practicerangedates = "  from  " + ViewBag.BeginDate + "  -  " + ViewBag.EndDate;
            }
            else
            {
                ViewBag.practicename = (Request["hdnname"] != null ? Request["hdnname"] : Session["ComboProvFullName"]);
                ViewBag.practicerangedates = "  from  " + ViewBag.BeginDate + "  -  " + ViewBag.EndDate;
            }
            //clsWebConfigsettings cs = new clsWebConfigsettings();
            //ViewBag.src = cs.GetConfigSettingsValue("Islocal") == "Y" ? "https://maps.google.com/maps?file=api&v=3&key=ABQIAAAAi81Rjwr1-u7oEXnMu8ZAshSFiB3mIwTcvLSCB8Nf9sXhY0PL7hT-ysUkeccfB7NnkLj50hUFucSl7w" : "http://maps.google.com/maps?file=api&v=3&key=ABQIAAAAeznoROiGjKNjJuBg-sGaAhSj_VfuZaUqmXjgQ-jc6fA3gvU1eRScLYfjNHKJDPSP-99WON_NMwdG8A";


            //String Strtechid = null;
            //if (!string.IsNullOrEmpty(Request["Ddltech"]))
            //{
            //    Strtechid = Request["Ddltech"];
            //}
            //if (Strtechid != null)
            //{
            //    ViewBag.techid = Strtechid;
            //}
            ViewBag.Msgitem = Convert.ToString(Session["Msgitem"]);
            ViewBag.Serviceactive = Convert.ToString(Session["Serviceactive"]);
            if (Session["RoleID"] != null)
            {
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    Session["Prov_ID"] = null;
                    Session["Practice_ID"] = null;
                    Session["ComboProv_ID"] = null;
                    Session["ComboPractice_ID"] = null;
                    //if (TempData["hdnPrid"] == null)
                    //{
                    //    FillCoachesList();
                    //}
                }
            }
            if (!string.IsNullOrEmpty(Request["hdnname"]))
            {

                TempData["combotext"] = Request["hdnname"];
            }
            else
            {
                TempData["combotext"] = "";
            }
            Session.Add("TopIndex", "2");
            if (Request["hid"] == null)
            {
                if (Convert.ToString(Session["roleid"]) != "1" || Request["hdnPrid"] != null || TempData["hdnPrid"] != null)// & (Request["ComboBoxPr_selectedIndex"] != "-1" & Request["ComboBoxPr_selectedIndex"] != null))
                {

                    ViewBag.noexist = Session["CCexists"];
                    ViewBag.ccexpiry = Session["ccexpiry"];
                    if (Convert.ToString(Session["roleid"]) == "1")
                    {
                        if (!string.IsNullOrEmpty(Request["hdnPrid"]) || !string.IsNullOrEmpty(TempData["hdnPrid"].ToString()))
                        {
                            ViewBag.Prselect = "Y";
                            string strPracid;
                            if (!string.IsNullOrEmpty(Request["hdnPrid"]))// || !string.IsNullOrEmpty(TempData["hdnPrid"].ToString()))
                            {
                                strPracid = Request["hdnPrid"];
                                TempData["hdnPrid"] = Request["hdnPrid"];
                                TempData["hdnpname"] = Request["hdnname"];
                                TempData.Keep("hdnPrid");
                            }
                            else
                            {
                                strPracid = Convert.ToString(TempData["hdnPrid"]);
                                TempData.Keep("hdnPrid");
                            }
                            TempData.Keep("hdnpname");
                            //string[] strSplitPracid = strPracid.Split('$');
                            Session["ComboProv_ID"] = strPracid;
                            TempData["hdnPrid"] = Session["ComboProv_ID"];
                            //Session["ComboProv_ID"] =strSplitPracid[1] ;
                            //Session["ComboPractice_ID"] = strSplitPracid[0];
                            if (!string.IsNullOrEmpty(Request["hdnname"]))
                            {
                                Session["ComboProvFullName"] = Request["hdnname"];

                            }
                            else
                            {
                                Session["ComboProvFullName"] = TempData["hdnpname"];
                            }

                            FillProvider(Convert.ToString(Session["ComboProvFullName"]), 0, strPracid);
                        }
                        else
                        {
                            TempData.Remove("hdnPrid");
                            ViewBag.Prselect = null;

                        }
                    }
                }
                int Proide_ID;
                if (Convert.ToString(Session["RoleID"]) == "1")
                {
                    Proide_ID = Convert.ToInt32(Session["ComboProv_ID"]);
                }
                else
                {
                    if (Session["Prov_ID"] != null)
                    {
                        Proide_ID = Convert.ToInt32(Session["Prov_ID"]);

                    }
                    else
                    {
                        Proide_ID = Convert.ToInt32(Session["ProviderID"]);
                    }
                }
                DataSet dset = null;
                IDataParameter[] objparm = {
			new SqlParameter("@In_Provider_ID", Proide_ID),
            new SqlParameter("@in_Appointmentfromdate", apptdate!=null?apptdate:DateTime.Now.ToShortDateString()),
            new SqlParameter("@in_Appointmenttodate", apptdate!=null?apptdate:DateTime.Now.ToShortDateString())
		};
                objCommon.AddInParameters(objparm);
                dset = objCommon.GetDataSet("Help_dbo.ST_GET_Appointment_EXIST");
                if (dset.Tables.Count > 0)
                {
                    ViewBag.ddlschind = Convert.ToString(dset.Tables[0].Rows[0]["AppointmentExists"]);
                   // ViewBag.ddlschind = ddlschind;
                }
                if (timeinterval == null || redirectind != null)
                {
                    if (ViewBag.ddlschind == "Y")
                    {
                        timeinterval = "12:00-12:00";
                    }
                    else
                    {
                        timeinterval = "07:00-07:00";
                    }
                }
                //if (Session["roleid"].ToString() != "1")
                //{
                //    Session["PlaceOfService_ID"] = Session["PrimaryPOSID"];
                //}
                //else
                //{
                //    FillPlaceofServices(Convert.ToInt32(Session["ComboProv_ID"]), Convert.ToInt32(Session["ComboPractice_ID"]));
                //}
                string startdate = string.Empty;
                if (timeinterval == "12:00-12:00")
                {
                    ViewBag.timeinterval = timeinterval;
                    if (!string.IsNullOrEmpty(apptdate))
                    {
                        TempData["msg"] = DateTime.Parse(apptdate).DayOfWeek + " " +
                                          DateTime.Parse(apptdate).ToString("MMMM dd yyyy");
                        ViewBag.prvmsg = "Day view on " + DateTime.Parse(apptdate).DayOfWeek + " " +
                                         DateTime.Parse(apptdate).ToString("MMMM dd yyyy");
                        GetDayAppointments("Y", apptdate);
                        ViewData["hdndate"] = apptdate;
                        GetView("Y", apptdate, null);

                        startdate = apptdate;
                    }
                    else
                    {
                        TempData["msg"] = DateTime.Now.DayOfWeek + " " +
                                                              DateTime.Now.ToString("MMMM dd yyyy");
                        ViewBag.prvmsg = "Day view on " + DateTime.Now.DayOfWeek + " " +
                                         DateTime.Now.ToString("MMMM dd yyyy");
                        GetDayAppointments("Y", DateTime.Now.ToShortDateString());
                        ViewData["hdndate"] = DateTime.Now.ToShortDateString();
                        GetView("Y", DateTime.Now.ToShortDateString(), null);
                        startdate = DateTime.Now.ToShortDateString();
                    }
                    Fillallappointments(startdate, "N", null);

                    ViewData["Pre_date"] = DateTime.Parse(startdate).AddDays(-1).ToShortDateString();
                    ViewData["Next_date"] = DateTime.Parse(startdate).AddDays(+1).ToShortDateString();
                    TempData["Ind"] = "Y";
                }
                else if (timeinterval == "07:00-07:00")
                {
                    if (!string.IsNullOrEmpty(apptdate))
                    {
                        ViewBag.timeinterval = timeinterval;
                        TempData["msg"] = DateTime.Parse(apptdate).DayOfWeek + " " +
                                          DateTime.Parse(apptdate).ToString("MMMM dd yyyy");
                        ViewBag.prvmsg = "Day view on " + DateTime.Parse(apptdate).DayOfWeek + " " +
                                         DateTime.Parse(apptdate).ToString("MMMM dd yyyy");
                        GetDayAppointments("N", apptdate);
                        ViewData["hdndate"] = apptdate;
                        GetView("Y", apptdate, null);

                        startdate = apptdate;
                    }
                    else
                    {
                        TempData["msg"] = DateTime.Now.DayOfWeek + " " +
                                                              DateTime.Now.ToString("MMMM dd yyyy");
                        ViewBag.prvmsg = "Day view on " + DateTime.Now.DayOfWeek + " " +
                                         DateTime.Now.ToString("MMMM dd yyyy");
                        GetDayAppointments("N", DateTime.Now.ToShortDateString());
                        ViewData["hdndate"] = DateTime.Now.ToShortDateString();
                        GetView("Y", DateTime.Now.ToShortDateString(), null);
                        startdate = DateTime.Now.ToShortDateString();
                    }
                    Fillallappointments(startdate, "N", null);
                    ViewData["Pre_date"] = DateTime.Parse(startdate).AddDays(-1).ToShortDateString();
                    ViewData["Next_date"] = DateTime.Parse(startdate).AddDays(+1).ToShortDateString();
                    ViewData["Pre_date1"] = DateTime.Parse(startdate).AddDays(-1).DayOfWeek + " " +
                                       DateTime.Parse(startdate).AddDays(-1).ToString("MMMM dd yyyy");
                    ViewData["Next_date1"] = DateTime.Parse(startdate).AddDays(+1).DayOfWeek + " " +
                   DateTime.Parse(startdate).AddDays(+1).ToString("MMMM dd yyyy");
                    TempData["Ind"] = "N";
                }
                else
                {
                    startdate = DateTime.Now.ToShortDateString();
                    TempData["msg"] = DateTime.Now.DayOfWeek + " " +
                                      DateTime.Now.ToString("MMMM dd yyyy");
                    ViewBag.prvmsg = "Day view on " + DateTime.Now.DayOfWeek + " " +
                                     DateTime.Now.ToString("MMMM dd yyyy");
                    Fillallappointments(startdate, "N", null);
                    ViewData["Pre_date"] = DateTime.Parse(startdate).AddDays(-1).ToShortDateString();
                    ViewData["Next_date"] = DateTime.Parse(startdate).AddDays(+1).ToShortDateString();
                    ViewData["Pre_date1"] = DateTime.Parse(startdate).AddDays(-1).DayOfWeek + " " +
                                     DateTime.Parse(startdate).AddDays(-1).ToString("MMMM dd yyyy");
                    ViewData["Next_date1"] = DateTime.Parse(startdate).AddDays(+1).DayOfWeek + " " +
DateTime.Parse(startdate).AddDays(+1).ToString("MMMM dd yyyy");

                    GetDayAppointments("N", DateTime.Now.ToShortDateString());
                    ViewData["hdndate"] = DateTime.Now.ToShortDateString();
                    GetView("Y", DateTime.Now.ToShortDateString(), null);

                    TempData["Ind"] = null;
                }
                ViewBag.selectadate = apptdate;
                if (ViewBag.Showtask == "Y")
                {
                    GetTodayTask(startdate);
                }
                StringBuilder strdayappt = new StringBuilder();
                strdayappt =
                    strdayappt.Append(
                        "<table id='tblAppointments' cellspacing='1' cellpadding='8' width='98%'  style='background-color:#d0e4f4'>");

                foreach (DataRow drappt in dsAppointmentsList.Tables[0].Rows)
                {
                    string backcolorTr = string.Empty;
                    string timperiod = string.Empty;
                    string rowspantr = string.Empty;
                    string backcolorTd = string.Empty;
                    int columnspanTd;
                    if (drappt["Period"] != null)
                    {
                        timperiod = drappt["Period"].ToString().Replace(":", "-");
                        //timperiod = timperiod.Replace(":", "-");
                    }
                    if (Convert.ToString(drappt["ProviderInd"]) == "N")
                    {
                        backcolorTr = "#FDEEDB";
                        strdayappt =
                            strdayappt.Append("<tr id='trAppointments' style='height:20px; background-color: " +
                                              backcolorTr + "'   >");
                        strdayappt =
                            strdayappt.Append("<td id='tcIntervel' style='width: 13%; background-color: " + backcolorTr +
                                              "' align='center' valign='middle' > <a class='schedulemenu' href='#' onclick='return AddAppt(\"" + timperiod + "\",\"" + apptdate + "\")'  title='Click here to add the Appointment'>" + drappt["Period"] +
                                              "</a></td>");
                    }
                    else
                    {
                        strdayappt = strdayappt.Append("<tr id='trAppointments' style='height:20px; ' >");
                        strdayappt =
                            strdayappt.Append(
                                "<td id='tcIntervel' style='width: 13%;' align='center' valign='middle' class='white_color'> <a class='schedulemenu' href='#' onclick='return AddAppt(\"" + timperiod + "\",\"" + apptdate + "\")'  title='Click here to add the Appointment'>" + drappt["Period"] + "</a></td>");
                    }
                    if (Convert.ToString(drappt["ProviderInd"]) == "N")
                    {
                        if (drAppoiontments.Tables[0].Select("AppointmentTime = '" + drappt["Period"] + "'").Length < 0)
                        {
                            backcolorTr = "#FDEEDB";
                            strdayappt =
                                strdayappt.Append(
                                    "<td id='tcAppointments' width='87%' rowspan='1' colspan='0' style='height:20px; background-color: " +
                                    backcolorTr + "' >");
                        }
                    }
                    int aptcount = 0;
                    if (drAppoiontments.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drSche in drAppoiontments.Tables[0].Rows)
                        {
                            if (Convert.ToString(drSche["AppointmentTime"]) == Convert.ToString(drappt["Period"]))
                            {
                                aptcount++;
                                if (!DBNull.Value.Equals(drSche["Duration"]))
                                {
                                    int duration = Convert.ToInt32(drSche["Duration"]);
                                    switch (duration)
                                    {
                                        case 30:
                                            {
                                                rowspantr = "1";
                                                break;
                                            }
                                        case 60:
                                            {
                                                rowspantr = "2";
                                                break;
                                            }
                                        case 90:
                                            {
                                                rowspantr = "3";
                                                break;
                                            }
                                        case 120:
                                            {
                                                rowspantr = "4";
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    rowspantr = "1";
                                }
                                if (Convert.ToString(drSche["IsConflict"]) == "Y")
                                {
                                    if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "3")
                                    {
                                        backcolorTd = "#E19571";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "7")
                                    {
                                        backcolorTd = "#996699";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "4")
                                    {
                                        backcolorTd = "#F2B75E";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "5")
                                    {
                                        backcolorTd = "#53898C";
                                    }
                                    else
                                    {
                                        backcolorTd = "#c4d23a";
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "3")
                                    {
                                        backcolorTd = "#E19571";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "7")
                                    {
                                        backcolorTd = "#996699";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "4")
                                    {
                                        backcolorTd = "#F2B75E";
                                    }
                                    else if (Convert.ToString(drSche["AppointmentStatus_ID"]) == "5")
                                    {
                                        backcolorTd = "#53898C";
                                    }
                                    else
                                    {
                                        backcolorTd = "#c4d23a";
                                    }
                                }
                                string colspant = string.Empty;
                                if (Convert.ToInt32(drSche["Countas"]) != 0)
                                {
                                    columnspanTd = Convert.ToInt32(drSche["MaxCount"]) /
                                                   Convert.ToInt32(drSche["Countas"]);
                                    colspant = Convert.ToString(columnspanTd);
                                }

                                strdayappt =
                                    strdayappt.Append("<td id='tcAppointments' width='87%' rowspan=" + rowspantr +
                                                      " colspan=" + colspant + " style='background-color: " +
                                                      backcolorTd + "'>");
                                strdayappt = strdayappt.Append("<table id='tbl' width='100%'><tr>");
                                if (!DBNull.Value.Equals(drSche["Appointment_ID"]))
                                {
                                    if (!DBNull.Value.Equals(drSche["PatientName"]))
                                    {

                                        //ViewBag.custname = drSche["PatientName"].ToString();

                                        //string[] strgmap = null;
                                        ////if (drSche["ZipLatitue"] != null)
                                        ////{
                                        //    string strcuname = drSche["PatientName"].ToString().Trim();
                                        //    strgmap = strcuname.Split("");
                                        //}
                                        string pastdues = string.Empty;
                                        if (!DBNull.Value.Equals(drSche["Pastdues"]))
                                        {
                                            pastdues = "Past dues : " + string.Format("{0:c}", drSche["Pastdues"]);

                                        }
                                        else
                                        {
                                            pastdues = "Past dues : " + "$0.00";
                                        }
                                      //  string techtitle = null;
                                       // if (!string.IsNullOrEmpty(Convert.ToString(drSche["Technicianname"])))// != null & drSche["Technicianname"])!="")
                                       // {

                                           // techtitle = "Technicians : " + Convert.ToString(drSche["Technicianname"]);
                                       // }
                                        //else
                                        //{
                                          //  techtitle = "Click here to Edit appointment";
                                        //}
                                        if (Convert.ToString(drSche["notes_voice_filename"]) != null & Convert.ToString(drSche["notes_voice_filename"]) != "")
                                        {
                                            strdayappt =
                                                     strdayappt.Append("<td align='left'  style='background-color: " +
                                                                       backcolorTd +
                                                                       "'><a class='EditDialog' style='color:white;font-size:13px;' href='#' onclick='return EditAppt(\"" + drSche["Appointment_ID"] + "\",\"" + drSche["phonenumber"] + "\")'    title='Click here to Edit appointment' ></a>&nbsp;&nbsp;<strong><a class='EditDialog' style='color:white;font-size:13px;' href='#' onclick='return EditAppt(\"" + drSche["Appointment_ID"] + "\",\"" + drSche["phonenumber"] + "\")'    title='Click here to Edit appointment' >" +
                                                                       drSche["PatientName"] +
                                                                       "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<a class='showmap' onclick='javascript:return fnmaplanglat(" + drSche["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to show appointment location' >" +
                                                                       drSche["Address1"] + ',' + drSche["cityzip"] + "&nbsp;&nbsp;<img src='../Images/Map1.gif' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'>" + "&nbsp;&nbsp;<a class='showcustmtransactions' onclick='javascript:return fncustmtransactions(" + drSche["ToReference_ID"] + "," + drSche["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to add transactions.' >" + "<img src='../Images/currency_dollar_green.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" + "&nbsp;&nbsp;<a class='showPlayaudio'   style='color:white;font-size:13px;' href='../Schedule/audioPlay?GetApptfilename=" + drSche["notes_voice_filename"] + "'    title='Click here to play audio.' >" + "<img src='../Images/play.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" +
                                                                       "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + drSche["phonenumber"] + "</label>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + pastdues + "</label> </strong>");
                                        }
                                        else
                                        {
                                            strdayappt =
                                                     strdayappt.Append("<td align='left'  style='background-color: " +
                                                                       backcolorTd +
                                                                       "'><a class='EditDialog' style='color:white;font-size:13px;' href='#' onclick='return EditAppt(\"" + drSche["Appointment_ID"] + "\",\"" + drSche["phonenumber"] + "\")'    title='Click here to Edit appointment' ></a>&nbsp;&nbsp;<strong><a class='EditDialog' style='color:white;font-size:13px;' href='#' onclick='return EditAppt(\"" + drSche["Appointment_ID"] + "\",\"" + drSche["phonenumber"] + "\")'    title='Click here to Edit appointment' >" +
                                                                       drSche["PatientName"] +
                                                                       "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<a class='showmap' onclick='javascript:return fnmaplanglat(" + drSche["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to show appointment location' >" +
                                                                       drSche["Address1"] + ',' + drSche["cityzip"] + "&nbsp;&nbsp;<img src='../Images/Map1.gif' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'>" + "&nbsp;&nbsp;<a class='showcustmtransactions' onclick='javascript:return fncustmtransactions(" + drSche["ToReference_ID"] + "," + drSche["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to add transactions.' >" + "<img src='../Images/currency_dollar_green.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" +
                                                                       "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + drSche["phonenumber"] + "</label>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + pastdues + "</label> </strong>");
                                        }
                                        //strdayappt =
                                        //    strdayappt.Append("<td align='left'  style='background-color: " +
                                        //                      backcolorTd +
                                        //                      "'><a class='EditDialog' style='color:white;font-size:13px;' href='../Schedule/EditAppointment?apptEid=" +
                                        //                      drSche["Appointment_ID"] + "" +
                                        //                      "&frompage=dayview" + "'    title='" + techtitle + "' ></a>&nbsp;&nbsp;<strong><a class='EditDialog' style='color:white;font-size:13px;' href='../Schedule/EditAppointment?apptEid=" +
                                        //                      drSche["Appointment_ID"] + "" +
                                        //                      "&frompage=dayview" + "'    title='" + techtitle + "' >" +
                                        //                      drSche["PatientName"] +
                                        //                      "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<a class='showmap' onclick='javascript:return fnlanglat(" + drSche["ToReference_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to show customer address directions' >" +
                                        //                      drSche["Address"] + "&nbsp;&nbsp;<img src='../Images/Map1.gif' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'>" + "&nbsp;&nbsp;<a class='showcustmtransactions' onclick='javascript:return fncustmtransactions(" + drSche["ToReference_ID"] + "," + drSche["Appointment_ID"] + ");'  style='color:white;font-size:13px;' href='#'    title='Click here to customer transactions.' >" + "<img src='../Images/currency_dollar_green.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" + "&nbsp;&nbsp;<a class='showPlayaudio'   style='color:white;font-size:13px;' href='../Schedule/audioPlay'    title='Click here to customer transactions.' >" + "<img src='../Images/play.png' style='background:  no-repeat ; vertical-align:middle; width:24px; height:24px;' alt='searchPage'></a>" +
                                        //                      "</a>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + drSche["phonenumber"] + "</label>&nbsp;&nbsp;||&nbsp;&nbsp;<label style='color:white;font-size:13px;'>" + pastdues + "</label> </strong>");                                     
                                    }
                                    else
                                    {
                                        strdayappt =
                                            strdayappt.Append("<td align='left'  style='background-color: " +
                                                              backcolorTd +
                                                              "'><strong><a class='EditDialog' style='color:white;font-size:13px;' href='#' onclick='return Displaydialog(\"EditlDialog\",\"../Schedule/EditAppointment?apptEid=" + drSche["Appointment_ID"] + "" + "&frompage=dayview" + "\",\"auto\",\"930\",\"dvLoading\",\"null\",\"null\");'  data-dialog-title='EditApptclass' data-dialog-id='EditlDialog' title='Click here to Edit appointment' >" +
                                                              drSche["AppointmentTime"] + " </a></strong>");
                                    }

                                }

                                strdayappt = strdayappt.Append("</td></tr></table>");
                            }
                        }
                        if (aptcount == 0)
                        {
                            backcolorTr = "#d0e4f4";
                            strdayappt =
                                strdayappt.Append(
                                    "<td id='tcAppointments' width='100%' rowspan='1' colspan='0'  style='height:20px; background-color: " +
                                    backcolorTr + "' >");
                        }
                    }
                    else
                    {
                        if (Convert.ToString(drappt["ProviderInd"]) == "N")
                        {
                            strdayappt =
                                strdayappt.Append(
                                    "<td id='tcAppointments' width='100%' rowspan='1' colspan='0' style='height:20px; background-color: " +
                                    backcolorTr + "' >");
                        }
                        else
                        {
                            backcolorTr = "#d0e4f4";
                            strdayappt =
                                strdayappt.Append(
                                    "<td id='tcAppointments' width='100%' rowspan='1' colspan='0'  style='height:20px; background-color: " +
                                    backcolorTr + "' >");
                        }
                    }
                }
                strdayappt = strdayappt.Append("</td></tr>");
                strdayappt = strdayappt.Append("</table>");
                ViewBag.daysch = Convert.ToString(strdayappt);
            }
            else
            {
                ViewBag.prvmsg = "Day view on " + DateTime.Now.DayOfWeek + " " + DateTime.Now.ToString("MMMM dd yyyy");
            }
            return View();
            //string pid = null;
            //if (Convert.ToString(Session["roleid"]) == "1")
            //{
            //    if (Request["hdnPrid"] != null || TempData["hdnPrid"] != null)
            //    {
            //        pid = Request["hdnPrid"];
            //    }
            //}
            //else
            //{
            //    pid = Convert.ToString(Session["Prov_id"]);
            //}

    //        List<TechniciansInfo> TechnicianList = new List<TechniciansInfo>();
    //        IDataParameter[] objparam = {
    //    new SqlParameter("@in_provider_id", pid),
    //    new SqlParameter("@in_appointment_id", null )
    //};
    //        objCommon.AddInParameters(objparam);
    //        SqlDataReader objread = default(SqlDataReader);
    //        objread = objCommon.GetDataReader("Help_dbo.st_get_technician_names_list");
    //        IList<SelectListItem> _result1 = new List<SelectListItem>();
    //        int counter = 0;
    //        while (objread.Read())
    //        {

    //            _result1.Add(new SelectListItem
    //            {
    //                Text = Convert.ToString(objread["name"]),
    //                Value = Convert.ToString(objread["Technician_id"])
    //            });
    //            counter++;
    //        }
    //        ViewBag.tech_count = Convert.ToString(counter);

    //        StateCity reg1 = new StateCity();
    //        reg1 = new StateCity
    //        {
    //            Techlist = _result1
    //        };
    //        return View(reg1);
        }
        private void GetTodayTask(string apptdate)
        {
            try
            {
                ViewBag.tasktitle = "Task List for  " + apptdate;
                int refTypeId;
                if (Convert.ToString(Session["RoleID"]) == "15")
                {
                    refTypeId = 13;
                }
                else
                {
                    refTypeId = 2;
                }
                IDataParameter[] inputparamlist ={
                                                 new SqlParameter("@In_ProviderLogin_ID",Convert.ToInt32(Session["UserID"])),
                                                 new SqlParameter("@In_referencetype_id",refTypeId),
                                                 new SqlParameter("@in_Startdate",apptdate)
                                            };
                clsDBWrapper obj = new clsDBWrapper();
                obj.AddInParameters(inputparamlist);
                dsTodayTask = obj.GetDataSet("Help_dbo.st_Schedule_LIST_ProviderTasks");
                StringBuilder sbTask = new StringBuilder();
                sbTask = sbTask.Append("<table width='100%' cellspacing='1' cellpadding='6' border='0' class='border_style'>");
                int i = 1;
                if (dsTodayTask.Tables[0].Rows.Count > 0)
                {
                    sbTask = sbTask.Append("<tr class='tr_bgcolor'><td width='10%' align='center'>#</td><td align='center'><strong>Title</strong></td></tr>");

                    foreach (DataRow dr in dsTodayTask.Tables[0].Rows)
                    {
                        string trcolor = string.Empty;
                        if (i % 2 == 0)
                        {
                            trcolor = "nav_blue_color";
                        }
                        else
                        {
                            trcolor = "white_color";
                        }

                        sbTask = sbTask.Append("<tr class=" + trcolor + "><td width='5%' align='center'>" + i + "</td><td align='center'>" + dr["Tasktitle"] + " </td></tr>");
                        i += 1;
                    }


                }
                else
                {
                    sbTask = sbTask.Append("<tr class='white_color'><td colspan='2' align='center'>No tasks found.</td></tr>");
                }
                sbTask = sbTask.Append("</table>");
                ViewBag.todaysTask = Convert.ToString(sbTask);
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetTodayTask", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }


        //public JsonResult GetProviderDetail()
        //{
        //    clsCommonFunctions objcomm = new clsCommonFunctions();
        //    Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
        //    objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
        //    return Json(objInfo, JsonRequestBehavior.DenyGet);
        //    //called when provider service is expired
        //}

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult Addnewpaymentmethod(string cardexpiry, string ccexists)
        {//called when provider service is expired
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
                Session["Stripe_customerid"] = null;

                if (Convert.ToString(Session["Msgitem"]) == "1")
                {
                    ViewBag.message = Convert.ToString(Session["Msgtext1"]);
                    ViewBag.Msgitem = Convert.ToString(Session["Msgitem"]);
                }
                else if (Convert.ToString(Session["Msgitem"]) == "2")
                {
                    ViewBag.message = Convert.ToString(Session["Msgtext2"]);
                    ViewBag.Msgitem = Convert.ToString(Session["Msgitem"]);
                }
                else if (Convert.ToString(Session["Msgitem"]) == "3")
                {
                    ViewBag.message = Convert.ToString(Session["Msgtext3"]);
                    ViewBag.Msgitem = Convert.ToString(Session["Msgitem"]);
                }
                else if (Convert.ToString(Session["Msgitem"]) == "4")
                {
                    ViewBag.message = Convert.ToString(Session["Msgtext4"]);
                    ViewBag.Msgitem = Convert.ToString(Session["Msgitem"]);
                }
                else if (Convert.ToString(Session["Msgitem"]) == "5")
                {
                    ViewBag.message = null;
                    ViewBag.Msgitem = "5";
                }
                if (Convert.ToString(Session["Serviceactive"]) == "N")
                {
                    ViewBag.message = "Your service is not active.Please activate your service";
                }




                ViewBag.ccexists = ccexists;
                ViewBag.cardexpiry = cardexpiry;
                if (cardexpiry == "Y")
                {
                    int? ccinfoid = null;
                    if (Convert.ToString(Session["Roleid"]) == "4")
                    {
                        var obj = new Paymentmethods();
                        List<Paymentmethods> objcardexpirylist = Paymentmethods.CreditCard_Expirylist_info(obj);
                        ccinfoid = objcardexpirylist[0].CreditCardInfo_ID;
                        ViewBag.daysremining = objcardexpirylist[0].Daysremining;
                    }
                    else
                    {
                        ccinfoid = null;
                    }
                    ViewBag.expdate = "Your card has been expired in " + ViewBag.daysremining + "days ";
                    if (ccinfoid != null)
                    {
                        string vaultid = CCProcess.GetVaultID(Convert.ToString(ccinfoid));
                        Filldata(Convert.ToString(ccinfoid), vaultid);//*
                        // FillProviderAddress();
                        //}
                        //else
                        //{
                        objInfo = GetProviderDetails();//*
                    }
                }
                else
                {
                    if (Convert.ToString(Session["Msgitem"]) == "3" || Convert.ToString(Session["Msgitem"]) == "4")
                    {
                        CCProcess objcc = new CCProcess();
                        objcc.ReferenceTypeID = "2";
                        objcc.ReferenceID = Convert.ToString(Session["Prov_ID"]);
                        string ccinfoid = CCProcess.LoadCreditCardInfo(objcc);
                        string vaultid = CCProcess.GetVaultID(ccinfoid);

                        Filldata(Convert.ToString(ccinfoid), vaultid);//*
                        objInfo = GetProviderDetails();
                    }
                    else if (Convert.ToString(Session["Serviceactive"]) == "N" && Convert.ToString(Session["CCexists"]) == "Y")
                    {
                        CCProcess objcc = new CCProcess();
                        objcc.ReferenceTypeID = "2";
                        objcc.ReferenceID = Convert.ToString(Session["Prov_ID"]);
                        string ccinfoid = CCProcess.LoadCreditCardInfo(objcc);
                        string vaultid = CCProcess.GetVaultID(ccinfoid);

                        Filldata(Convert.ToString(ccinfoid), vaultid);//*
                        //}
                        //else
                        //{
                        objInfo = GetProviderDetails();//*****
                        ViewBag.expdate = "Your card information ";
                    }
                    else
                    {
                        ViewBag.practice_ind = "Y";
                        objInfo = GetProviderDetails();
                    }
                }
                //Referrals description = new Referrals();
                //description.FieldIDString = "9";
                DataSet ds = new DataSet();
                ds = Referrals.List_field_description("9,26");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //string str = Convert.ToString(ds.Tables[0].Rows[0][3]);
                    ViewBag.regFee = Convert.ToString(ds.Tables[0].Rows[0][3]);
                    ViewBag.IssuingNotes = Convert.ToString(ds.Tables[0].Rows[1][3]);
                }
                var cardstype = clsCommonCClist.GetCCList();

                var _month = clsCommonCClist.GetCCMonth();

                var _year = clsCommonCClist.GetCCYear();

                //var reg1 = clsCommonCClist.GetStates();

                //Reg_CreditCardProcess cardtype = new Reg_CreditCardProcess();
                //List<Reg_CreditCardProcess> cardlist = new List<Reg_CreditCardProcess>();
                //cardlist = cardtype.Get_Creditcardtype();
                //IList<SelectListItem> cardstype = new List<SelectListItem>();
                //if (cardlist.Count > 0)
                //{
                //    for (int i = 0; i <= cardlist.Count - 1; i++)
                //    {
                //        if (i == 0)
                //        {

                //            cardstype.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
                //                Text = "American Express",
                //                Selected = true
                //            });
                //        }
                //        else if (i == 3)
                //        {

                //            cardstype.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
                //                Text = "Visa"
                //            });
                //        }
                //        else
                //        {

                //            cardstype.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(cardlist[i].CreditCardType_ID),
                //                Text = Convert.ToString(cardlist[i].code)
                //            });
                //        }
                //    }
                //}
                //int[] MonthArray = null;
                //MonthArray = new int[12];
                //for (int intVal = 0; intVal <= 11; intVal++)
                //{
                //    MonthArray[intVal] = intVal + 1;
                //}
                //IList<SelectListItem> _month = new List<SelectListItem>();
                //if (MonthArray.Length > 0)
                //{
                //    for (int i = 0; i <= MonthArray.Length - 1; i++)
                //    {
                //        if (MonthArray[i] == 1)
                //        {
                //            _month.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(MonthArray[i]),
                //                Text = Convert.ToString(MonthArray[i]),
                //                Selected = true
                //            });
                //        }
                //        else
                //        {
                //            _month.Add(new SelectListItem
                //            {
                //                Value = Convert.ToString(MonthArray[i]),
                //                Text = Convert.ToString(MonthArray[i]),
                //                Selected = false
                //            });
                //        }

                //    }
                //}
                //IList<SelectListItem> _year = new List<SelectListItem>();
                //for (this.intCounter = DateTime.Now.Year; this.intCounter <= DateTime.Now.Year + 12; this.intCounter++)
                //{
                //    _year.Add(new SelectListItem
                //    {
                //        Value = this.intCounter.ToString(),
                //        Text = this.intCounter.ToString(),
                //        Selected = true
                //    });
                //}
                ////for (this.intCounter = StartingYear; this.intCounter <= DateTime.Now.Year + 12; this.intCounter++)
                ////{
                ////    if (this.intCounter == 2014)
                ////    {
                ////        _year.Add(new SelectListItem
                ////        {
                ////            Value = this.intCounter.ToString(),
                ////            Text = this.intCounter.ToString(),
                ////            Selected = true
                ////        });
                ////    }
                ////    else
                ////    {
                ////        _year.Add(new SelectListItem
                ////        {
                ////            Value = this.intCounter.ToString(),
                ////            Text = this.intCounter.ToString(),
                ////            Selected = false
                ////        });
                ////    }

                ////}
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
                ViewData["month"] = _month;
                ViewData["year"] = _year;
                ViewData["CardType"] = cardstype;
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    ViewBag.Practice_ID = Convert.ToString(Session["Practice_ID"]);
                    ViewBag.Provider_ID = Convert.ToString(Session["Prov_ID"]);
                }
                else
                {
                    ViewBag.Practice_ID = Convert.ToString(Session["ComboPractice_ID"]);
                    ViewBag.Provider_ID = Convert.ToString(Session["ComboProv_ID"]);
                }

                if (Convert.ToString(Session["Msgitem"]) != "5" || Convert.ToString(Session["Serviceactive"]) == "N")
                {
                    DataSet dsrenewdetails = new DataSet();
                    dsrenewdetails = Referrals.GetRenewChargeDetails(Convert.ToString(Session["userid"]));
                    if (dsrenewdetails.Tables[0].Rows.Count > 0)
                    {
                        string CurrentmonthStart = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["Currentmonthstartdate"])))// != null && dsrenewdetails.Tables[0].Rows[0]["Currentmonthstartdate"].ToString() != "")
                        {
                            CurrentmonthStart = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[0]["Currentmonthstartdate"].ToString()).ToShortDateString();
                        }
                        string CurrentmonthEnd = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["currentmonthsEnddate"])))//!= null && dsrenewdetails.Tables[0].Rows[0]["currentmonthsEnddate"].ToString() != "")
                        {
                            CurrentmonthEnd = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[0]["currentmonthsEnddate"].ToString()).ToShortDateString();
                        }
                        if (CurrentmonthStart != null && CurrentmonthEnd != null)
                        {
                            ViewBag.currentmonthdates = CurrentmonthStart + " To " + CurrentmonthEnd;
                        }
                        else
                        {
                            ViewBag.currentmonthdates = null;
                        }

                        string totservice = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["remaingdays_servicefee"]);
                        if (totservice != "0.00")
                        {
                            ViewBag.Remaingdaysfee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["remaingdays_servicefee"]) + " (Prorated)";
                        }
                        else
                        {
                            ViewBag.Remaingdaysfee = null;
                        }
                        ViewBag.Totalservicefee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["Totalservicefee"]);
                        string NextmonthStart = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["nextmonthstartdate"])))// != null && dsrenewdetails.Tables[0].Rows[0]["nextmonthstartdate"].ToString() != "")
                        {
                            NextmonthStart = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[0]["nextmonthstartdate"].ToString()).ToShortDateString();
                        }
                        string NextmonthEnd = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["nextmonthEnddate"])))//!= "" && dsrenewdetails.Tables[0].Rows[0]["nextmonthEnddate"].ToString() != null)
                        {
                            NextmonthEnd = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[0]["nextmonthEnddate"].ToString()).ToShortDateString();
                        }

                        ViewBag.NextmonthEnd = NextmonthEnd;
                        if (NextmonthStart != null && NextmonthEnd != null)
                        {
                            ViewBag.nextmonthdates = NextmonthStart + " To " + NextmonthEnd;
                        }
                        else
                        {
                            ViewBag.nextmonthdates = null;
                        }
                        ViewBag.servicefee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["servicefee"]);
                        ViewBag.ctempid1 = Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["chargetempleate_id"]);
                        ViewBag.billingservice_id = Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["billingservice_id"]);
                        ViewBag.Totalservicefee3 = dsrenewdetails.Tables[0].Rows[0]["Totalservicefee"];

                        ViewBag.Billingservicename = dsrenewdetails.Tables[0].Rows[0]["Billingservicename"];





                        if (dsrenewdetails.Tables[0].Rows.Count > 1)
                        {
                            string CurrentmonthStart1 = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["Currentmonthstartdate"])))//!= null && dsrenewdetails.Tables[0].Rows[1]["Currentmonthstartdate"].ToString() != "")
                            {
                                CurrentmonthStart1 = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[1]["Currentmonthstartdate"].ToString()).ToShortDateString();
                            }
                            string CurrentmonthEnd1 = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["currentmonthsEnddate"])))// != null && dsrenewdetails.Tables[0].Rows[1]["currentmonthsEnddate"].ToString() != "")
                            {
                                CurrentmonthEnd1 = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[1]["currentmonthsEnddate"].ToString()).ToShortDateString();
                            }
                            if (CurrentmonthStart1 != null && CurrentmonthEnd1 != null)
                            {
                                ViewBag.currentmonthdates1 = CurrentmonthStart1 + " To " + CurrentmonthEnd1;
                            }
                            else
                            {
                                ViewBag.currentmonthdates1 = null;
                            }
                            string totservice1 = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[1]["remaingdays_servicefee"]);
                            if (totservice1 != "0.00")
                            {
                                ViewBag.Remaingdaysfee1 = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[1]["remaingdays_servicefee"]) + " (Prorated)";
                            }
                            else
                            {
                                ViewBag.Remaingdaysfee1 = null;
                            }
                            ViewBag.Totalservicefee1 = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[1]["Totalservicefee"]);
                            string NextmonthStart1 = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["nextmonthstartdate"])))//!= null && dsrenewdetails.Tables[0].Rows[1]["nextmonthstartdate"].ToString() != "")
                            {
                                NextmonthStart1 = Convert.ToDateTime(dsrenewdetails.Tables[0].Rows[1]["nextmonthstartdate"].ToString()).ToShortDateString();
                            }
                            string NextmonthEnd1 = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["nextmonthEnddate"])))// != null && dsrenewdetails.Tables[0].Rows[1]["nextmonthEnddate"].ToString() != "")
                            {
                                NextmonthEnd1 = Convert.ToDateTime(Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["nextmonthEnddate"])).ToShortDateString();
                            }
                            ViewBag.NextmonthEnd1 = NextmonthEnd1;
                            if (NextmonthStart1 != null && NextmonthEnd1 != "")
                            {
                                ViewBag.nextmonthdates1 = NextmonthStart1 + " To " + NextmonthEnd1;
                            }
                            else
                            {
                                ViewBag.nextmonthdates1 = null;
                            }
                            ViewBag.servicefee1 = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[1]["servicefee"]);
                            ViewBag.ctempid2 = Convert.ToString(dsrenewdetails.Tables[0].Rows[1]["chargetempleate_id"]);
                            ViewBag.Totalservicefee4 = dsrenewdetails.Tables[0].Rows[1]["Totalservicefee"];
                        }
                        else
                        {
                            ViewBag.currentmonthdates1 = null;
                            ViewBag.Remaingdaysfee1 = null;
                            ViewBag.Totalservicefee1 = null;
                            ViewBag.NextmonthEnd1 = null;
                            ViewBag.nextmonthdates1 = null;
                            ViewBag.servicefee1 = null;
                            ViewBag.ctempid2 = null;
                            ViewBag.Totalservicefee4 = null;
                        }
                    }
                    ViewBag.onedolor = "N";
                }
                else
                {
                    ViewBag.Totalservicefee = "$1.00";
                    ViewBag.Totalservicefee3 = "1.00";
                    ViewBag.onedolor = "Y";
                }

                return View("Addnewpaymentmethod", objInfo);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }

        public JsonResult Updatesessionvalue()
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            Paymentmethods userResultModel = new Paymentmethods();
            Session["Msgitem"] = "5";
            Session["CCexists"] = "Y";

            return Json(userResultModel, JsonRequestBehavior.AllowGet);
        }
        private void Filldata(string id, string valutid)
        {
            try
            {
                ViewBag.CardID = id;
                ViewBag.vaultid = valutid;

                var obj1 = new Paymentmethods();
                List<Paymentmethods> objcardlist = Paymentmethods.CreditCard_list_paymentinfo(obj1);
                if (string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"])) == true)
                {
                    APIContext apiContext = Configuration.GetAPIContext();
                    //try
                    //{
                    CreditCard getcardinfo = CreditCard.Get(apiContext, valutid);
                    if (getcardinfo != null)
                    {
                        ViewBag.cardnumber = getcardinfo.number;
                        ViewBag.cvv2 = getcardinfo.cvv2;
                        ViewBag.FirstName = getcardinfo.first_name;
                        ViewBag.LastName = getcardinfo.last_name;
                    }
                    //}
                    //catch (Exception)
                    //{


                    //}


                }
                else
                {
                    //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                    var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                    var card = gateway.Get(new GetStripeCard
                    {
                        CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                        CardId = valutid,
                    });
                    ViewBag.cardnumber = "************" + card.Last4;
                    ViewBag.cvv2 = card.Cvc;
                    ViewBag.FirstName = Session["Stripe_Firstname"];
                    ViewBag.LastName = Session["Stripe_lastname"];
                }

                var obj = new CCProcess { CardID = id, Provider_ID = Convert.ToString(Session["Prov_ID"]) };
                List<CCProcess> objlist = CCProcess.CreditCard_Get_paymentinfo(obj);
                if (objlist.Count > 0)
                {
                    //ViewBag.CardType = objlist[0].strx_card_code ?? null;
                    if (objlist[0].strx_card_code != null)
                    {
                        if (objlist[0].strx_card_code == "MasterCard")
                        {
                            ViewBag.ctype = 1;
                        }
                        else if (objlist[0].strx_card_code == "VisaCard")
                        {
                            ViewBag.ctype = 2;
                        }
                        else if (objlist[0].strx_card_code == "American Express")
                        {
                            ViewBag.ctype = 3;
                        }
                        else if (objlist[0].strx_card_code == "DiscoverCard")
                        {
                            ViewBag.ctype = 4;
                        }
                    }
                    else
                    {
                        ViewBag.ctype = 3;
                    }
                    //ViewBag.ctype = 2;


                    ViewBag.ExipryMonth = objlist[0].StrExpMon ?? null;
                    ViewBag.ExipryYear = objlist[0].StrExpYear ?? null;
                    ViewBag.Address1 = objlist[0].strBillAddress1 ?? null;
                    ViewBag.Address2 = objlist[0].strBillAddress2 ?? null;
                    ViewBag.Zip = objlist[0].strZipCode ?? null;
                    ViewBag.State_ID = objlist[0].strStateID ?? null;
                    ViewBag.City_ID = objlist[0].strCityID ?? null;
                    ViewBag.Statename = objlist[0].state ?? null;
                    ViewBag.Cityname = objlist[0].city ?? null;
                    ViewBag.practice_ind = objlist[0].practice_ind ?? null;
                    ViewBag.CFirstName = objlist[0].FirstName ?? null;
                    ViewBag.CLastName = objlist[0].LastName ?? null;
                }
            }
            catch (Exception ex)
            {
                var log = new clsExceptionLog();
                log.LogException(ex, "AdminController", "Filldata", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                Session.Add("IsPopUp", "true");
                ViewBag.ExipryMonth = null;
                ViewBag.ExipryYear = null;
                ViewBag.PAddress1 = null;
                ViewBag.PAddress2 = null;
                ViewBag.PZip = null;
                ViewBag.State_ID = null;
                ViewBag.City_ID = null;
                ViewBag.Statename = null;
                ViewBag.Cityname = null;
                ViewBag.practice_ind = null;
            }
        }
        private Reg_ProvidersDetailInfo GetProviderDetails()
        {
            try
            {
                //clsCommonFunctions objcomm = new clsCommonFunctions();
                Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
                objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));

                return objInfo;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetProviderDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult Addnewpaymentmethod(Reg_CreditCardProcess obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string firstName = null, lastName = null;
                int? Out_CCID = 0;
                string stramount = null;
                string strchargetempleid = null;
                string strenddate = null;
                CreditCard crdtCard = new CreditCard();

                if (!string.IsNullOrEmpty(obj.FullName))
                {
                    if (obj.FullName.Contains(' '))
                    {
                        var names = obj.FullName.Split(' ');
                        if (names.Length >= 2)
                        {
                            firstName = names[0] ?? null;
                            lastName = names[1] ?? null;
                        }
                    }
                    else
                    {
                        firstName = obj.FullName;
                    }
                }

                if (Request["CardType"] != "")
                {
                    switch (Request["CardType"])
                    {
                        case "1": crdtCard.type = "mastercard";
                            break;
                        case "2": crdtCard.type = "visa";
                            break;
                        case "3": crdtCard.type = "amex";
                            break;
                        case "4": crdtCard.type = "discover";
                            break;
                    }
                }
                if (Request["CardNumber"] != "")
                {
                    crdtCard.number = Request["CardNumber"].ToString();
                }
                if (Request["txtCVV2"] != "")
                {
                    crdtCard.cvv2 = Request["txtCVV2"].ToString();
                }
                else
                {
                    crdtCard.cvv2 = null;
                }
                crdtCard.first_name =!string.IsNullOrEmpty(firstName)?firstName:null;
                crdtCard.last_name = !string.IsNullOrEmpty(lastName)?lastName:null;
                //if (Request["txtfirstname"] != "")
                //{
                //    crdtCard.first_name = Request["txtfirstname"].ToString();
                //}
                //else
                //{
                //    crdtCard.first_name = null;
                //}
                //if (Request["txtlastname"] != "")
                //{
                //    crdtCard.last_name = Request["txtlastname"].ToString();
                //}
                //else
                //{
                //    crdtCard.last_name = null;
                //}
                if (Request["month"] != "")
                {
                    crdtCard.expire_month = Convert.ToInt32(Request["month"]);
                }

                if (Request["year"] != "")
                {
                    crdtCard.expire_year = Convert.ToInt32(Request["year"]);
                }
                if (Request["HdnProvider_ID"] != null)
                {
                    crdtCard.payer_id = Request["HdnProvider_ID"];
                }

                Address billingAddress = new Address();
                billingAddress.country_code = "US";
                string StateID = null, CityID = null;
                if (Request["hdnadd1"] != "")
                {
                    billingAddress.line1 = Request["hdnadd1"];
                }
                else
                {
                    billingAddress.line1 = null;
                }
                if (Request["hdnadd2"] != "")
                {
                    billingAddress.line2 = Request["hdnadd2"];
                }
                else
                {
                    billingAddress.line2 = null;
                }
                if (Request["hdnzip"] != "")
                {
                    billingAddress.postal_code = Request["hdnzip"].ToString();
                }
                else
                {
                    billingAddress.postal_code = null;
                }
                if (Request["hdnstate"] != "")
                {
                    StateID = Request["hdnstate"];
                    billingAddress.state = clsCommonCClist.Getstatename(Request["hdnstate"]);
                }
                else
                {
                    StateID = null;
                    billingAddress.state = null;
                }
                if (Request["hdncity"] != "")
                {
                    CityID = Request["hdncity"];
                    billingAddress.city = clsCommonCClist.Getcityname(Request["hdncity"]);
                }
                else
                {
                    CityID = null;
                    billingAddress.city = null;
                }              
                crdtCard.billing_address = billingAddress;
                Amount amnt = new Amount();
                amnt.currency = "USD";
                if (Request["hdnonedolor"] == "N")
                {
                    if (Request["rbtmonth"] == "1")
                    {
                        stramount = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnmonth"])));
                        amnt.total = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnmonth"])));
                        strchargetempleid = Request["hdnctempid1"];
                        strenddate = Request["hdnenddate1"];
                    }
                    else
                    {
                        stramount = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnyear"])));
                        amnt.total = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnyear"])));
                        strchargetempleid = Request["hdnctempid2"];
                        strenddate = Request["hdnenddate2"];
                    }
                }
                else
                {
                    stramount = "1.00";
                    amnt.total = "1.00";
                    strchargetempleid = Request["hdnctempid1"];
                    strenddate = Request["hdnenddate1"];
                }

                Transaction tran = new Transaction();
                tran.amount = amnt;
                tran.description = "SmbHelpGroup.";
                List<Transaction> transactions = new List<Transaction>();
                transactions.Add(tran);
                FundingInstrument fundInstrument = new FundingInstrument();
                fundInstrument.credit_card = crdtCard;
                List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
                fundingInstrumentList.Add(fundInstrument);
                Payer payr = new Payer();
                payr.funding_instruments = fundingInstrumentList;
                payr.payment_method = "credit_card";
                Payment pymnt = new Payment();
                pymnt.intent = "sale";
                pymnt.payer = payr;
                pymnt.transactions = transactions;
                try
                {

                    //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                    if (clsWebConfigsettings.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                    {
                        APIContext apiContext = Configuration.GetAPIContext();
                        Payment createdPayment = pymnt.Create(apiContext);
                        JsonResult objser = new JsonResult();
                        string objrequest = JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented);
                        if (createdPayment != null & objrequest != "" & createdPayment.state == "approved")
                        {
                            CCProcess objinsert = new CCProcess();
                            objinsert.StrPaidRefID = "1";
                            objinsert.StrPaidRefTypeID = "1";
                            objinsert.StrChargebleRefID = Request["HdnProvider_ID"];
                            objinsert.StrChargebleRefTypeID = "2";
                            objinsert.strpost = objrequest;
                            objinsert.RefLoginID = 0;
                            objinsert.strPracticeID = Request["HdnPractice_ID"];
                            objinsert.CVV2 = null;
                            objinsert.strx_card_code = crdtCard.type;
                            objinsert.strStrCardType = null;
                            objinsert.strStateID = StateID;// Request["hdnstate"];
                            objinsert.strZipCode = billingAddress.postal_code;// Convert.ToString(Request["hdnzip"]);
                            objinsert.strCityID = CityID;// Request["hdncity"];
                            objinsert.strBillAddress1 = billingAddress.line1;
                            objinsert.strBillAddress2 = billingAddress.line2;
                            objinsert.FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null;
                            objinsert.LastName = !string.IsNullOrEmpty(lastName) ? lastName : null;
                            //objinsert.FirstName = Request["txtfirstname"];
                            //objinsert.LastName = Request["txtlastname"];
                            objinsert.strx_invoice_num = null;
                            objinsert.strx_card_num = null;
                            objinsert.IsPaypalInd = "Y";
                            objinsert.Provider_ID = Request["HdnProvider_ID"];
                            objinsert.StrExpMon = crdtCard.expire_month;
                            objinsert.StrExpYear = crdtCard.expire_year;
                            objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                            objinsert.paypaltransactionid = createdPayment.id;
                            List<Transaction> objlist = new List<Transaction>();
                            List<RelatedResources> objfund = new List<RelatedResources>();
                            Sale objsale = new Sale();
                            objlist = createdPayment.transactions;
                            objfund = objlist[0].related_resources;
                            objsale = objfund[0].sale;
                            objinsert.paypalsaletransactionid = objsale.id;
                            int Loc_transid;
                            //clsCommonFunctions objcommon = new clsCommonFunctions();
                            int LoginId = objCommon.GetProviderLoginID(Convert.ToString(Request["HdnProvider_ID"]));
                            objinsert.CreatedBy = LoginId;
                            objinsert.ALLowCCCharges = "Y";
                            string Mycardno = Convert.ToString(Request["CardNumber"]);
                            objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                            if (Request["chkaddress"] != null)
                            {
                                if (Request["chkaddress"] == "true,false")
                                {
                                    objinsert.practice_ind = "Y";
                                }
                                else
                                {
                                    objinsert.practice_ind = "N";
                                }
                            }
                            CreditCard newCreditCard = new CreditCard();                          
                            newCreditCard = crdtCard.Create(apiContext);                          
                            objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                            //if (newCreditCard.state != null)
                            //{
                            //    if (newCreditCard.state.ToLower() == "ok")
                            //    {
                                    MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                                    CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), newCreditCard.id, Out_CCID);
                            //    }
                            //}

                                           
                            Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                            objstorecreditcardresponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);

                            CCProcess objccins = new CCProcess();
                            objccins.strTransactionID = Loc_transid;
                            objccins.StrRespStatusCode = createdPayment.state;
                            objccins.strRetval = objstorecreditcardresponse;// Encrypt(objstorecreditcardresponse);
                            objccins.ResponseCode = null;
                            objccins.strUserID = LoginId;
                            objccins.GatewayDetail_ID = null;
                            objccins.PaypalProcessInd = "Y";
                            Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);

                            if (createdPayment.state == "approved")
                            {
                                if (Request["hdnonedolor"] != "N")
                                {
                                    //if (newCreditCard.state != null)
                                    //{
                                    //    if (newCreditCard.state.ToLower() == "ok")
                                    //    {
                                            Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, null, newCreditCard.id);
                                    //    }
                                    //}
                                }
                                obj.Provider_ID = Convert.ToInt32(Request["HdnProvider_ID"]);
                                Session["CCexists"] = "Y";
                                Session["Msgitem"] = "5";
                                Session["Serviceactive"] = "Y";
                                if (Request["hdnonedolor"] == "N")
                                {
                                    CCProcess objcc = new CCProcess();
                                    objcc.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                    objcc.CreatedBy = Convert.ToInt32(Session["userid"]);
                                    CCProcess.Insert_ServiceChargeInfo(objcc);
                                    CCProcess objcc1 = new CCProcess();
                                    objcc1.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                    objcc1.CCTransaction_ID = Loc_transid;
                                    objcc1.CreatedBy = Convert.ToInt32(Session["userid"]);
                                    objcc1.NextRenewDate = strenddate;

                                    CCProcess.Insert_ServicePaymentInfo(objcc1);
                                }
                                var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["Prov_ID"]));

                                if ((obgCredn != null))
                                {
                                    ViewBag.Login_ID = (obgCredn.ProviderLogin_ID) != null ? obgCredn.ProviderLogin_ID : null;
                                    string strtoMailid = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                                    string strprovidername = (obgCredn.providername) != null ? obgCredn.providername : null;
                                    string strdate = DateTime.Now.ToShortDateString();
                                    string servicename = Request["hdnservicename"];
                                    if (servicename == null || servicename == "")
                                    {
                                        servicename = "credit card verification process";
                                    }
                                    if (Request["hdnonedolor"] != "N")
                                    {
                                        obj.RegInd = "Y";
                                        //var objcng = new clsWebConfigsettings();
                                        if (clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
                                        {
                                            SendActivationEmail(strtoMailid);
                                        }
                                    }
                                    SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total, servicename);
                                }

                                return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on process"), JsonRequestBehavior.DenyGet);
                            }
                        }
                        else
                        {
                            return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (!string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"])))
                    {
                        double strip_amount = Convert.ToDouble(amnt.total) * 100;

                        //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                        var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                        var card = gateway.Post(new CreateStripeCard
                        {
                            CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                            Card = new StripeCard
                            {
                                //Name = Request["txtfirstname"] + Request["txtlastname"],
                                Name = Request["FullName"],
                                Number = Request["CardNumber"],
                                Cvc = Request["txtCVV2"] != "" ? Request["txtCVV2"] : null,
                                ExpMonth = Convert.ToInt32(Request["month"]),
                                ExpYear = Convert.ToInt32(Request["year"]),
                                AddressLine1 = billingAddress.line1,
                                AddressLine2 = billingAddress.line2,
                                AddressZip = Request["hdnzip"],
                                AddressState = Request["hdnstate"],
                                AddressCountry = "US",
                            },
                        });

                        var charge = gateway.Post(new ChargeStripeCustomer
                        {
                            Amount = Convert.ToInt32(strip_amount),
                            Customer = Convert.ToString(Session["Stripe_customerid"]),
                            Currency = "usd",
                            Description = "Test Charge Client",
                            Card = card.Id
                        });
                        CCProcess objinsert = new CCProcess();
                        objinsert.StrPaidRefID = "1";
                        objinsert.StrPaidRefTypeID = "1";
                        objinsert.StrChargebleRefID = Request["HdnProvider_ID"];
                        objinsert.StrChargebleRefTypeID = "2";
                        objinsert.strpost = null;
                        objinsert.RefLoginID = 0;
                        objinsert.strPracticeID = Request["HdnPractice_ID"];
                        objinsert.CVV2 = null;
                        objinsert.strx_card_code = crdtCard.type;
                        objinsert.strStrCardType = null;
                        objinsert.strStateID = Request["hdnstate"];
                        objinsert.strZipCode = Convert.ToString(Request["hdnzip"]);
                        objinsert.strCityID = Request["hdncity"];
                        objinsert.strBillAddress1 = billingAddress.line1;
                        objinsert.strBillAddress2 = billingAddress.line2;
                        objinsert.FirstName=!string.IsNullOrEmpty(firstName) ? firstName : null;
                        objinsert.LastName = !string.IsNullOrEmpty(lastName) ? lastName : null;
                        //objinsert.FirstName = Request["txtfirstname"];
                        //objinsert.LastName = Request["txtlastname"];
                        objinsert.strx_invoice_num = null;
                        objinsert.strx_card_num = null;
                        objinsert.IsPaypalInd = "N";
                        objinsert.Provider_ID = Request["HdnProvider_ID"];
                        objinsert.StrExpMon = crdtCard.expire_month;
                        objinsert.StrExpYear = crdtCard.expire_year;
                        objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                        objinsert.paypaltransactionid = charge.Id;
                        objinsert.paypalsaletransactionid = null;
                        int Loc_transid;
                        //clsCommonFunctions objcommon = new clsCommonFunctions();
                        objinsert.CreatedBy = objCommon.GetProviderLoginID(Request["HdnProvider_ID"]);
                        objinsert.ALLowCCCharges = "Y";
                        string Mycardno = Convert.ToString(Request["CardNumber"]);
                        objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                        if (Request["chkaddress"] != null)
                        {
                            if (Request["chkaddress"] == "true,false")
                            {
                                objinsert.practice_ind = "Y";
                            }
                            else
                            {
                                objinsert.practice_ind = "N";
                            }
                        }
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                        MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                        //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                        CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), card.Id, Out_CCID, Convert.ToString(Session["Stripe_customerid"]));
                        //}

                        Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                        int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));

                        CCProcess objccins = new CCProcess();
                        objccins.strTransactionID = Loc_transid;
                        objccins.StrRespStatusCode = "approved";
                        objccins.ResponseCode = null;
                        objccins.ResponseCode = null;
                        objccins.strUserID = LoginId;
                        objccins.GatewayDetail_ID = null;
                        objccins.PaypalProcessInd = "N";
                        Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);
                        if (charge.FailureMessage == null)
                        {
                            if (Request["hdnonedolor"] != "N")
                            {
                                Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, null, card.Id, Convert.ToString(Session["Stripe_customerid"]));
                            }
                            obj.Provider_ID = Convert.ToInt32(Request["HdnProvider_ID"]);
                            //}
                            Session["CCexists"] = "Y";
                            Session["Msgitem"] = "5";
                            Session["Serviceactive"] = "Y";
                            if (Request["hdnonedolor"] == "N")
                            {
                                CCProcess objcc = new CCProcess();
                                objcc.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                objcc.CreatedBy = Convert.ToInt32(Session["userid"]);
                                CCProcess.Insert_ServiceChargeInfo(objcc);
                                CCProcess objcc1 = new CCProcess();
                                objcc1.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                objcc1.CCTransaction_ID = Loc_transid;
                                objcc1.CreatedBy = Convert.ToInt32(Session["userid"]);
                                objcc1.NextRenewDate = strenddate;

                                CCProcess.Insert_ServicePaymentInfo(objcc1);
                            }
                            
                            var obgCredn1 = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["Prov_ID"]));

                            if ((obgCredn1 != null))
                            {
                                ViewBag.Login_ID = (obgCredn1.ProviderLogin_ID) != null ? obgCredn1.ProviderLogin_ID : null;
                                string strtoMailid = (obgCredn1.UserName) != null ? obgCredn1.UserName : null;
                                string strprovidername = (obgCredn1.providername) != null ? obgCredn1.providername : null;
                                string strdate = DateTime.Now.ToShortDateString();
                                string servicename = Request["hdnservicename"];
                                if (servicename == null || servicename == "")
                                {
                                    servicename = "credit card verification process";
                                }

                                SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total, servicename);
                            }

                            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on process"), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        double strip_amount = Convert.ToDouble(amnt.total) * 100;

                        //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                        var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                        // StripeCard Card;
                        var cardToken = gateway.Post(new CreateStripeToken
                        {
                            Card = new StripeCard
                            {
                                //Name = Request["txtfirstname"] + Request["txtlastname"],
                                Name = Request["FullName"],
                                Number = Request["CardNumber"],
                                Cvc = Request["txtCVV2"] != "" ? Request["txtCVV2"] : null,
                                ExpMonth = Convert.ToInt32(Request["month"]),
                                ExpYear = Convert.ToInt32(Request["year"]),
                                AddressLine1 = Request["hdnadd1"] != "" ? Request["hdnadd1"] : null,
                                AddressLine2 = Request["hdnadd2"] != "" ? Request["hdnadd2"] : null,
                                AddressZip = Request["hdnzip"] != "" ? Request["hdnzip"].ToString() : null,
                                AddressState = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                                AddressCountry = "US",
                            },
                        });
                        var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Request["HdnProvider_ID"]));
                        string strcustomermail = null;
                        if ((obgCredn != null))
                        {
                            strcustomermail = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                        }
                        var customer = gateway.Post(new CreateStripeCustomerWithToken
                        {

                            Card = cardToken.Id,
                            Description = "Description",
                            Email = strcustomermail,
                        });

                        var charge = gateway.Post(new ChargeStripeCustomer
                        {
                            Amount = Convert.ToInt32(strip_amount),
                            Customer = customer.Id,
                            Currency = "usd",
                            Description = "Test Charge Client",
                        });
                        var card = gateway.Get(new GetStripeCard
                        {
                            CustomerId = customer.Id,
                            CardId = cardToken.Card.Id,
                        });
                        JsonResult objser = new JsonResult();
                        CCProcess objinsert = new CCProcess();
                        objinsert.StrPaidRefID = "1";
                        objinsert.StrPaidRefTypeID = "1";
                        objinsert.StrChargebleRefID = Request["HdnProvider_ID"];
                        objinsert.StrChargebleRefTypeID = "2";
                        objinsert.strpost = null;
                        objinsert.RefLoginID = 0;
                        objinsert.strPracticeID = Request["HdnPractice_ID"];
                        objinsert.CVV2 = null;
                        objinsert.strx_card_code = crdtCard.type;
                        objinsert.strStrCardType = null;
                        objinsert.strStateID = Request["hdnstate"];
                        objinsert.strZipCode = Convert.ToString(Request["hdnzip"]);
                        objinsert.strCityID = Request["hdncity"];
                        objinsert.strBillAddress1 = billingAddress.line1;
                        objinsert.strBillAddress2 = billingAddress.line2;
                        objinsert.FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null;
                        objinsert.LastName = !string.IsNullOrEmpty(lastName) ? lastName : null;
                        //objinsert.FirstName = Request["txtfirstname"];
                        //objinsert.LastName = Request["txtlastname"];
                        objinsert.strx_invoice_num = null;
                        objinsert.strx_card_num = null;
                        objinsert.IsPaypalInd = "N";
                        objinsert.Provider_ID = Request["HdnProvider_ID"];
                        objinsert.StrExpMon = crdtCard.expire_month;
                        objinsert.StrExpYear = crdtCard.expire_year;
                        objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                        objinsert.paypaltransactionid = charge.Id;
                        objinsert.paypalsaletransactionid = null;
                        int Loc_transid;
                        //clsCommonFunctions objcommon = new clsCommonFunctions();
                        objinsert.CreatedBy = objCommon.GetProviderLoginID(Convert.ToString(Request["HdnProvider_ID"]));
                        objinsert.ALLowCCCharges = "Y";
                        string Mycardno = Convert.ToString(Request["CardNumber"]);
                        objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                        if (Request["chkaddress"] != null)
                        {
                            if (Request["chkaddress"] == "true,false")
                            {
                                objinsert.practice_ind = "Y";
                            }
                            else
                            {
                                objinsert.practice_ind = "N";
                            }
                        }
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                        MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, !string.IsNullOrEmpty(Request["ExhdnCardid"]) ? Request["ExhdnCardid"] : null);
                        //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                        CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), card.Id, Out_CCID, cardToken.Id);
                        //}

                        Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                        int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["HdnProvider_ID"]));

                        CCProcess objccins = new CCProcess();
                        objccins.strTransactionID = Loc_transid;
                        objccins.StrRespStatusCode = "approved";
                        objccins.ResponseCode = null;
                        objccins.ResponseCode = null;
                        objccins.strUserID = LoginId;
                        objccins.GatewayDetail_ID = null;
                        objccins.PaypalProcessInd = "N";
                        Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);
                        if (charge.FailureMessage == null)
                        {
                            if (Request["hdnonedolor"] != "N")
                            {
                                Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["HdnProvider_ID"]), "A", null, null, card.Id, customer.Id);
                            }
                            obj.Provider_ID = Convert.ToInt32(Request["HdnProvider_ID"]);
                            //}
                            Session["CCexists"] = "Y";
                            Session["Msgitem"] = "5";
                            Session["Serviceactive"] = "Y";
                            if (Request["hdnonedolor"] == "N")
                            {
                                CCProcess objcc = new CCProcess();
                                objcc.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                objcc.CreatedBy = Convert.ToInt32(Session["userid"]);
                                CCProcess.Insert_ServiceChargeInfo(objcc);
                                CCProcess objcc1 = new CCProcess();
                                objcc1.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                objcc1.CCTransaction_ID = Loc_transid;
                                objcc1.CreatedBy = Convert.ToInt32(Session["userid"]);
                                objcc1.NextRenewDate = strenddate;

                                CCProcess.Insert_ServicePaymentInfo(objcc1);
                            }
                           
                            var obgCredn1 = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["Prov_ID"]));

                            if ((obgCredn1 != null))
                            {
                                ViewBag.Login_ID = (obgCredn1.ProviderLogin_ID) != null ? obgCredn1.ProviderLogin_ID : null;
                                string strtoMailid = (obgCredn1.UserName) != null ? obgCredn1.UserName : null;
                                string strprovidername = (obgCredn1.providername) != null ? obgCredn1.providername : null;
                                string strdate = DateTime.Now.ToShortDateString();
                                string servicename = Request["hdnservicename"];
                                if (servicename == null || servicename == "")
                                {
                                    servicename = "credit card verification process";
                                }

                                SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total, servicename);
                            }

                            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on process"), JsonRequestBehavior.AllowGet);
                        }
                    }




                }
                //catch (PayPal.Exception.PayPalException ex)
                //{
                //    var objex = new clsExceptionLog();
                //    objex.LogException(ex, "ScheduleController", "Addnewpaymentmethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                //}
                catch (PayPal.PayPalException ex)
                {
                    //if (ex.InnerException is PayPal.ConnectionException)
                    //{
                    //    string txt = (((PayPal.ConnectionException)ex.InnerException).Response);
                    //}
                    //else
                    //{
                    //    //context.Response.Write(ex.Message);
                    //}
                }
                catch (StripeException ex)
                {
                    string errormessage = ex.Message;
                    if (errormessage != null)
                    {
                        CCProcess objinsert = new CCProcess();
                        objinsert.StrPaidRefID = "1";
                        objinsert.StrPaidRefTypeID = "1";
                        objinsert.StrChargebleRefID = Request["HdnProvider_ID"];
                        objinsert.StrChargebleRefTypeID = "2";
                        //objinsert.strpost = objrequest;
                        objinsert.strpost = null;
                        objinsert.RefLoginID = 0;
                        objinsert.strPracticeID = Request["HdnPractice_ID"];
                        objinsert.CVV2 = null;
                        objinsert.strx_card_code = crdtCard.type;
                        objinsert.strStrCardType = null;
                        objinsert.strStateID = Request["hdnstate"];
                        objinsert.strZipCode = Convert.ToString(Request["hdnzip"]);
                        objinsert.strCityID = Request["hdncity"];
                        objinsert.strBillAddress1 = billingAddress.line1;
                        objinsert.strBillAddress2 = billingAddress.line2;
                        objinsert.FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null;
                        objinsert.LastName = !string.IsNullOrEmpty(lastName) ? lastName : null;
                        //objinsert.FirstName = Request["txtfirstname"];
                        //objinsert.LastName = Request["txtlastname"];
                        objinsert.strx_invoice_num = null;
                        objinsert.strx_card_num = null;
                        objinsert.IsPaypalInd = "N";
                        objinsert.Provider_ID = Request["HdnProvider_ID"];
                        objinsert.StrExpMon = crdtCard.expire_month;
                        objinsert.StrExpYear = crdtCard.expire_year;
                        objinsert.dblx_amount = Convert.ToDouble(amnt.total);
                        objinsert.paypaltransactionid = null;
                        objinsert.paypalsaletransactionid = null;
                        int Loc_transid;
                        //clsCommonFunctions objcommon = new clsCommonFunctions();
                        int LoginId = objCommon.GetProviderLoginID(Convert.ToString(Request["HdnProvider_ID"]));
                        objinsert.CreatedBy = LoginId;
                        objinsert.ALLowCCCharges = "Y";
                        string Mycardno = Convert.ToString(Request["CardNumber"]);
                        objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                        if (Request["chkaddress"] != null)
                        {
                            if (Request["chkaddress"] == "true,false")
                            {
                                objinsert.practice_ind = "Y";
                            }
                            else
                            {
                                objinsert.practice_ind = "N";
                            }
                        }
                        Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                        CCProcess objccins = new CCProcess();
                        objccins.strTransactionID = Loc_transid;
                        objccins.StrRespStatusCode = Convert.ToString(ex.Code);
                        objccins.strRetval = ex.Message;
                        objccins.ResponseCode = null;
                        objccins.ResponseCode = null;
                        objccins.strUserID = LoginId;
                        objccins.GatewayDetail_ID = null;
                        objccins.PaypalProcessInd = "N";
                        Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);
                    }
                }
                return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on process"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
       private void SendActivationEmail(string UserName)
        {
            try
            {
                //var objclscommon = new clsCommonFunctions();
                var objMailMessage = new ClsMailMessage();
                string strsubject = null;
                string strbody = null;
                string mailfrom = null;
                string mailto = null;
                string providername = null;
                //  string uname = null;
                string stractivationcode = null;
                IDataParameter[] strParam = { new SqlParameter("@in_email", UserName) };
                objCommon.AddInParameters(strParam);
                SqlDataReader drReader = objCommon.GetDataReader("Help_dbo.st_get_emailmessage_Activationcodeemail");
                if (drReader.Read())
                {
                    if (!string.IsNullOrEmpty(drReader["Subject"].ToString()))
                    {
                        strsubject = drReader["Subject"].ToString();
                    }
                    if (!string.IsNullOrEmpty(drReader["Message_Body"].ToString()))
                    {
                        strbody = drReader["Message_Body"].ToString();
                    }
                    if (!string.IsNullOrEmpty(drReader["Mail_From"].ToString()))
                    {
                        mailfrom = drReader["Mail_From"].ToString();
                    }
                    if (!string.IsNullOrEmpty(drReader["Mail_To"].ToString()))
                    {
                        mailto = drReader["Mail_To"].ToString();
                    }
                    if (!string.IsNullOrEmpty(drReader["Full_name"].ToString()))
                    {
                        providername = drReader["Full_name"].ToString();
                    }
                    if (!string.IsNullOrEmpty(drReader["Activationcode"].ToString()))
                    {
                        stractivationcode = drReader["Activationcode"].ToString();
                    }
                    if (strbody != null)
                    {
                        strbody = strbody.Replace("$ActivationCode$", stractivationcode);
                        strbody = strbody.Replace("$ProviderName$", providername);
                    }
                }

                bool strvalid = objMailMessage.SendMail(mailto, mailfrom, strsubject, strbody, EMailFormats.MailFormatText, EMailPriorities.PriorityNormal);
                string strEmailMessageTransactionId = Convert.ToString(drReader["EmailMessage_Transaction_ID"]);
                if (strvalid == true)
                {
                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strEmailMessageTransactionId) };
                    objCommon.AddInParameters(strMailParam);
                    objCommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "SendActivationEmail", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
       private void SendEmailtoElectrician(string strTomailid, string strTransaction_ID, string strProvidername, string strtransactiondate, string stramoumt, string servicename)
        {
            try
            {

                if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
                {
                    int Message_option_id = 0;
                    if (servicename == "credit card verification process")
                    {
                        Message_option_id = 56;
                    }
                    else
                    {
                        Message_option_id = 61;
                    }
                    var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(Message_option_id, 0);
                    if ((EMO != null))
                    {
                        ViewBag.Subject = EMO.Msg_Subject ?? null;
                        ViewBag.Content = EMO.Msg_Body ?? null;
                    }
                    string str_Content = ViewBag.Content;
                    if (str_Content != null)
                    {
                        if (strProvidername != null)
                        {
                            str_Content = str_Content.Replace("$ProviderName$", strProvidername);
                        }
                        str_Content = str_Content.Replace("$CSRName$", "Mower Helper");
                        if (Message_option_id == 56)
                        {
                            str_Content = str_Content.Replace("$transactionnumber$", strTransaction_ID);
                        }
                        else
                        {
                            str_Content = str_Content.Replace("$CCtransactionnumber$", strTransaction_ID);
                        }
                        if (strtransactiondate != null)
                        {
                            str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                        }
                        str_Content = str_Content.Replace("$ServiceContent$", servicename);
                        if (stramoumt != null)
                        {
                            double str_amoumt = Convert.ToDouble(stramoumt);
                            str_Content = str_Content.Replace("$Amount$", string.Format("{0:c}", str_amoumt));
                        }
                        if (strtransactiondate != null)
                        {
                            str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                        }
                    }
                    string strFrommailid = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
                    var objSendMessage = new ClsMailMessage();
                    bool strvalid = objSendMessage.SendMail(strTomailid, strFrommailid, ViewBag.Subject, str_Content, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                    if (strvalid == true)
                    {
                        //clsCommonFunctions objclscommon = new clsCommonFunctions();
                        IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strTransaction_ID) };
                        objCommon.AddInParameters(strMailParam);
                        objCommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                    }

                }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "SendEmailtoElectrician", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        //private string Encrypt(string clearText)
        //{
        //    try
        //    {
        //        string EncryptionKey = "MAKV2SPBNI99212";
        //        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //        using (Aes encryptor = Aes.Create())
        //        {
        //            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //            encryptor.Key = pdb.GetBytes(32);
        //            encryptor.IV = pdb.GetBytes(16);
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(clearBytes, 0, clearBytes.Length);
        //                    cs.Close();
        //                }
        //                clearText = Convert.ToBase64String(ms.ToArray());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, "ScheduleController", "Encrypt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

        //    }
        //    return clearText;
        //}

        //public string Getstatename(string stateid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_state_id", stateid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.St_get_stateabbrevation");
        //    while (dr_GetName.Read())
        //    {
        //        return dr_GetName["state"].ToString() != null ? dr_GetName["state"].ToString() : null;
        //    }
        //    return null;
        //}
        //public string Getcityname(string cityid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_City_ID", cityid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
        //    while (dr_GetName.Read())
        //    {
        //        return Convert.ToString(dr_GetName["City"]) != null ? dr_GetName["City"].ToString() : null;
        //    }
        //    return null;
        //}
        //[HttpGet()]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //[AllowAnonymous()]
        //public ActionResult Viewappointment(string apptEid, string apptdate, string recId)
        //{

        //    ViewAppointmentDetails(apptEid, apptdate);
        //    return View();
        //}
        //private void ViewAppointmentDetails(string patId, string ApptDate)
        //{
        //    try
        //    {
        //        List<GetAppointment> objGetAppointment = GetAppointment.GetPatientAppointment(patId, null, ApptDate);
        //        if (objGetAppointment.Count > 0)
        //        {

        //            ViewBag.Epatname = objGetAppointment[0].PatientName;

        //            ViewBag.Enote = objGetAppointment[0].Notes;
        //            ViewBag.Eapptid = objGetAppointment[0].Appointment_ID;

        //            if (!DBNull.Value.Equals(objGetAppointment[0].AppointmentMode_ID))
        //            {
        //                if (objGetAppointment[0].AppointmentMode_ID == 1)
        //                {
        //                    ViewBag.EApptMode = "Face";
        //                }
        //                else if (objGetAppointment[0].AppointmentMode_ID == 2)
        //                {
        //                    ViewBag.EApptMode = "Online";
        //                }
        //                else if (objGetAppointment[0].AppointmentMode_ID == 3)
        //                {
        //                    ViewBag.EApptMode = "Phone";
        //                }
        //            }
        //            if (objGetAppointment[0].AppointmentType_ID == 1)
        //            {
        //                ViewBag.EApptType = "Client";
        //                ViewBag.Ecptcode = objGetAppointment[0].CPTCode;
        //            }
        //            else
        //            {
        //                ViewBag.EApptType = "Blocked";
        //            }
        //            if ((objGetAppointment[0].AppointmentDate != null))
        //            {
        //                ViewBag.Eaptdate = objGetAppointment[0].AppointmentDate;
        //            }
        //            if ((objGetAppointment[0].AppointmentTime != null))
        //            {
        //                ViewBag.EaptTime = objGetAppointment[0].AppointmentTime;
        //            }
        //            if ((objGetAppointment[0].Duration != 0))
        //            {
        //                ViewBag.EaptDuration = objGetAppointment[0].Duration + "Minutes";
        //            }
        //            if (objGetAppointment[0].Auto_Reminder_Allow_Ind != null)
        //            {
        //                if (objGetAppointment[0].Auto_Reminder_Allow_Ind == "Y")
        //                {
        //                    ViewBag.autorem = "Yes";
        //                    if (!DBNull.Value.Equals(objGetAppointment[0].ReminderHR))
        //                    {
        //                        ViewBag.VwReminderHR = objGetAppointment[0].ReminderHR;
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.autorem = "No";
        //                }
        //                if ((objGetAppointment[0].PatientEmail != null))
        //                {
        //                    ViewBag.GetEmail = objGetAppointment[0].PatientEmail;
        //                }

        //            }

        //            if (objGetAppointment[0].IntervalType_ID != 0)
        //            {
        //                if (objGetAppointment[0].IntervalType_ID == 1)
        //                {
        //                    ViewBag.ERepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "day(s) at " + " " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
        //                    ViewBag.EweekDay = "DayShow";
        //                    ViewBag.dayenddate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
        //                    ViewBag.interdayvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
        //                }
        //                else if (objGetAppointment[0].IntervalType_ID == 2)
        //                {
        //                    ViewBag.weekDay = "Weekshow";
        //                    string showweek = string.Empty;
        //                    string showweekind = string.Empty;
        //                    if (objGetAppointment[0].SelectedWeekdays != null)
        //                    {
        //                        string selectedweeks = objGetAppointment[0].SelectedWeekdays;
        //                        selectedweeks.ToCharArray();

        //                        foreach (char i in selectedweeks)
        //                        {
        //                            switch (i)
        //                            {
        //                                case '1':
        //                                    showweek += "Sun" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '2':
        //                                    showweek += "Mon" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '3':
        //                                    showweek += "Tue" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '4':
        //                                    showweek += "Wed" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '5':
        //                                    showweek += "Thu" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '6':
        //                                    showweek += "Fri" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                                case '7':
        //                                    showweek += "Sat" + ",";
        //                                    showweekind += i + ",";
        //                                    break;
        //                            }


        //                        }
        //                        showweek = showweek.Remove(showweek.LastIndexOf(","));
        //                        showweekind = showweekind.Remove(showweekind.LastIndexOf(","));
        //                    }
        //                    ViewBag.ERepeatType = "Occurs every" + " " + Convert.ToString(objGetAppointment[0].IntervalValue) + " " + "week(s) on " + " " + showweek + " " + "at  " + objGetAppointment[0].AppointmentTime + " until " + DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
        //                    ViewBag.weekenddate = DateTime.Parse(objGetAppointment[0].Enddate).ToShortDateString();
        //                    ViewBag.interweekvalue = Convert.ToString(objGetAppointment[0].IntervalValue);
        //                    ViewBag.chkweek = showweekind;
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.ERepeatType = "No Repeat";
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var obj = new clsExceptionLog();
        //        obj.LogException(ex, "ScheduleController", "ViewAppointmentDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //}
        //[HttpGet()]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //[AllowAnonymous()]
        public JsonResult LoadingItems1(ComboBoxLoadingItemsEventArgs args)
        {
            ComboBoxItemList items = GetFilteredinsurances1(args.Text, 0);

            JsonResult result = new JsonResult
            {
                Data = new
                {
                    Items = items
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return result;
        }
        void FillCoachesList()
        {
            try
            {
                PatientRegistration obj = new PatientRegistration();
                List<PatientRegistration> objDataList = new List<PatientRegistration>();
                objDataList = obj.BindComboPracticeProviderSearch(null);
                ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
                ViewData["ComboBoxPr"] = payerlist;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "FillCoachesList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        protected ComboBoxItemList GetFilteredinsurances1(string filter, int offset)
        {
            try
            {
                TempData.Keep();
                PatientRegistration obj = new PatientRegistration();
                List<PatientRegistration> objDataList = new List<PatientRegistration>();
                string PracticeName = null;
                if (filter != null)
                {
                    PracticeName = filter;
                    TempData["combotext"] = "";
                }
                else if (Convert.ToString(TempData["combotext"]) != "")
                {
                    PracticeName = Convert.ToString(TempData["combotext"]).Substring(0, 2);
                }
                else
                {
                    PracticeName = null;
                }
                objDataList = obj.BindComboPracticeProviderSearch(PracticeName);


                ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
                List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
                ViewData["ComboBoxPr"] = payerlist;

                return payerlist;
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "GetFilteredinsurances1", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
        private void FillProvider(string filter, int offset, string hdnprid)
        {
            try
            {
                PatientRegistration obj = new PatientRegistration();
                List<PatientRegistration> objDataList = new List<PatientRegistration>();
                string PracticeName = filter.Trim();
                objDataList = obj.BindComboPracticeProviderSearch(null);


                ComboBoxItemList payerlist = new ComboBoxItemList(objDataList, "Provider_ID", "ProviderName");
                List<Obout.Mvc.ComboBox.ComboBoxItem> items = new List<Obout.Mvc.ComboBox.ComboBoxItem>();
                if (!string.IsNullOrEmpty(hdnprid))
                {
                    foreach (ComboBoxItem item in payerlist)
                    {
                        if (item.Value == hdnprid)
                        {
                            item.Selected = true;
                        }
                        items.Add(item);
                    }
                    ViewData["ComboBoxPr"] = items;
                }
                else
                {
                    ViewData["ComboBoxPr"] = payerlist;
                }
            }
            catch (Exception ex)
            {
                var obj = new clsExceptionLog();
                obj.LogException(ex, "ScheduleController", "FillProvider", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);

            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult CustomerTransactions(string id, string proid, string appid, string schetype = null)
        {

            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                    //ViewBag.test = "1";
                    ViewBag.proid = proid;
                    ViewBag.custmid = id;
                    ViewBag.schetype = schetype;
                    RPBilling apointdate = new RPBilling();
                    apointdate.Appointment_ID = appid;
                    string adate = apointdate.Get_Appointmentdate(apointdate);
                    string[] patapdate = adate.Split(' ');
                    ViewBag.Appdate = Convert.ToString(patapdate[0]);
                    ViewBag.appid = appid;
                    //  List<string> objpatientinfolist = new List<string>();

                    SqlDataReader ObjReader;
                    //clsCommonFunctions objcommon = new clsCommonFunctions();
                    IDataParameter[] InParamList = {
            new SqlParameter("@In_Patient_Ids", id),
           // new SqlParameter("@in_Practice_ID", Session["PracticeID"]),
            new SqlParameter("@iN_pROVIDER_ID", null)
        };
                    objCommon.AddInParameters(InParamList);
                    ObjReader = objCommon.GetDataReader("Help_dbo.st_Scheduling_Get_PatientDetails");
                    List<GetPatientDetails> objGetPatientDetailslist = new List<GetPatientDetails>();
                    while (ObjReader.Read())
                    {
                        GetPatientDetails objGetPatientDetails = new GetPatientDetails(Convert.ToInt32(ObjReader["Patient_ID"]), Convert.ToInt32(ObjReader["PatientLogin_ID"]), Convert.ToString(ObjReader["PatientName"]), Convert.ToInt32(ObjReader["Duration"]), Convert.ToString(ObjReader["PatientEmail"]), Convert.ToString(ObjReader["DefaultCourtLocation_ID"]));
                        objGetPatientDetailslist.Add(objGetPatientDetails);
                    }
                    ViewBag.customernm = Convert.ToString(objGetPatientDetailslist[0].PatientName);
                    ViewBag.custmemail = Convert.ToString(objGetPatientDetailslist[0].PatientEmail);

                    RPBilling patinfo = new RPBilling();
                    List<RPBilling> patinfolist = new List<RPBilling>();
                    patinfo.Reference_id = Convert.ToInt32(id);
                    patinfo.ReferenceType_ID = "3";
                    if (Convert.ToString(Session["roleid"]) != "1")
                    {
                        patinfo.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
                    }
                    else
                    {
                        //patinfo.Provider_id = Convert.ToInt32(proid);
                        patinfo.Provider_id = Convert.ToInt32(Session["ComboProv_ID"]);
                    }
                    patinfo.PatientLogin_ID = null;
                    patinfolist = patinfo.getUnpaidBalance(patinfo);
                    ViewBag.clientphone = patinfolist[0].clientphone;

                    ViewBag.balanceamt = patinfolist[0].BalanceAmount;

                    RPBilling clspayMode = new RPBilling();
                    //List<RPBilling> listPaymode = new List<RPBilling>();
                    //listPaymode = clspayMode.GetPaymentModes();
                    //IList<SelectListItem> paymentModeList = new List<SelectListItem>();
                    //if (listPaymode.Count > 0)
                    //{
                    //    for (int i = 0; i <= listPaymode.Count - 1; i++)
                    //    {
                    //        if (i != 3)
                    //        {
                    //            paymentModeList.Add(new SelectListItem
                    //            {
                    //                Value = Convert.ToString(listPaymode[i].PaymentMode_ID),
                    //                Text = Convert.ToString(listPaymode[i].PaymentMode)
                    //            });
                    //        }
                    //    }
                    //}
                    ViewBag.ddlPaymentMode = clspayMode.GetPaymentModes();
                    //Referrals description = new Referrals();
                    //description.FieldIDString = "11";
                    DataSet dsnote = new DataSet();
                    dsnote = Referrals.List_field_description("11");
                    if (dsnote.Tables[0].Rows.Count > 0)
                    {
                        //string str = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                        ViewBag.pmtNote = Convert.ToString(dsnote.Tables[0].Rows[0][3]);
                    }
                    return View(objGetPatientDetailslist);
                }
                else
                {
                       if (Request.IsAjaxRequest())
                    {

                        return RedirectToRoute("SessionExpirepopup");
                    }
                    else
                    {
                        return RedirectToRoute("SessionExpire");
                    }
                }
            

        }
        [HttpPost()]
        public ActionResult CustomerTransactions(RPBilling obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                RPBilling objTran = new RPBilling();
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    objTran.fromReferenceId = Convert.ToInt32(Session["Prov_ID"]);
                }
                else
                {
                    //objTran.fromReferenceId = Convert.ToInt32(Request["hdnproid"]);
                    objTran.fromReferenceId = Convert.ToInt32(Session["ComboProv_ID"]);
                }
                objTran.FromReferenceType_ID = "2";
                objTran.ToReferenceType_ID = "3";
                objTran.ToReference_ID = Request["hdncustmid"];
                objTran.Transaction_Date = Request["txtPayApptDate"];
                objTran.Transaction_Amount = Request["txtPayAmount"];
                if (Request["hdnPaymentMode"] != "1")
                {
                    objTran.PaymentType_ID = Convert.ToInt32(Request["ddlPaymentMode"]);
                }
                else
                {
                    objTran.PaymentType_ID = null;
                }
                objTran.PcType_ID = Convert.ToInt32(Request["ddlPayment"]);
                if (Request["txtAuthNum"] != null & Request["txtAuthNum"] != "Last 4 digits of credit card" & Request["txtAuthNum"] != "")
                {
                    objTran.Authorizednumber = Request["txtAuthNum"];
                }
                else
                {
                    objTran.Authorizednumber = null;
                }
                if (Request["txtCheckNo"] != null & Request["txtCheckNo"] != "Check number" & Request["txtCheckNo"] != "")
                {
                    objTran.ChecksNo = Request["txtCheckNo"];
                }
                else
                {
                    objTran.ChecksNo = null;
                }

                if (Request["txtEmailID"] != null & Request["txtEmailID"] != "")
                {
                    objTran.Customer_Email = Request["txtEmailID"];
                }
                else
                {
                    objTran.Customer_Email = null;
                }
                if (Request["hdnappid"] != null & Request["hdnappid"] != "")
                {
                    objTran.Appointment_ID = Request["hdnappid"];
                }
                else
                {
                    objTran.Appointment_ID = null;
                }
                if (Request["hdnAppdate"] != null & Request["hdnAppdate"] != "")
                {
                    objTran.apptdate = Request["hdnAppdate"];
                }
                else
                {
                    objTran.apptdate = null;
                }
                objTran.Notes = Convert.ToString(Request["txtNotes"]);
                string chkStaus = null;
                if (Request["chkemailrecipt"] != null)
                {
                    string[] WorkphoneLeaveMsg = null;
                    if (Request["chkemailrecipt"].Contains(","))
                    {
                        WorkphoneLeaveMsg = Request["chkemailrecipt"].Split(',');
                        if (WorkphoneLeaveMsg.Length == 2)
                        {
                            if (WorkphoneLeaveMsg[0] == "true")
                            {
                                chkStaus = "Y";
                            }
                            else if (WorkphoneLeaveMsg[0] == "false")
                            {
                                chkStaus = "N";
                            }
                        }
                    }
                    else
                    {
                        if (Request["chkemailrecipt"] == "false")
                        {
                            chkStaus = "N";
                        }
                        else if (Request["chkemailrecipt"] == "true")
                        {
                            chkStaus = "Y";
                        }
                    }
                }


                objTran.emailcheck = chkStaus;
                string Out_Msg = null;
                objTran.Insert_Customertransaction(objTran, ref Out_Msg);
                if (!string.IsNullOrEmpty(Out_Msg))
                {
                    return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.AllowGet);
                }
                else
                {

                    TempData.Keep("hdnPrid");
                    if (TempData["hdnPrid"] != null)
                    {
                        //     objContactInfo = Provider_ContactInfo.Get_ContactInfo(Convert.ToInt16(TempData["hdnPrid"]), 2);
                        //   // objContactInfo = Provider_ContactInfo.Get_ContactInfo(Convert.ToInt32(Session["Prov_ID"]), 2);
                        //    clsCommonCClist.TwilioSms(!string.IsNullOrEmpty(objContactInfo.PracticeName) ? objContactInfo.PracticeName : "", objContactInfo.CellPhone, Request["hdnclientphone"], null, null, Request["txtPayAmount"], Convert.ToInt16(Request["ddlPayment"]));
                        //}
                        //else
                        //{
                        //    var cc = TempData["hdnPrid"];
                        var objContactInfo = Provider_ContactInfo.Get_ContactInfo(Convert.ToInt16(TempData["hdnPrid"]), 2);
                        clsCommonCClist.TwilioSms(!string.IsNullOrEmpty(objContactInfo.PracticeName) ? objContactInfo.PracticeName : "", objContactInfo.CellPhone, Request["hdnclientphone"], null, null, Request["txtPayAmount"], Convert.ToInt16(Request["ddlPayment"]));
                    }
                    else
                    {
                        var objContactInfo = Provider_ContactInfo.Get_ContactInfo(Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]), 2);
                        clsCommonCClist.TwilioSms(!string.IsNullOrEmpty(objContactInfo.PracticeName) ? objContactInfo.PracticeName : "", Convert.ToString(Session["Providerphone"]), Request["hdnclientphone"], null, null, Request["txtPayAmount"], Convert.ToInt16(Request["ddlPayment"]));
                    }
                    obj.schedule_type = Convert.ToString(Request["hdnschetype"]);
                    return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                {

                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
                //else
                //{
                //    return RedirectToRoute("SessionExpire");
                //    //return Json(JsonResponseFactory.ErrorResponse("Logon"), JsonRequestBehavior.AllowGet);
                //}
            //}
            //else
            //{
            //    return RedirectToRoute("SessionExpire");
            //   // return Json(JsonResponseFactory.ErrorResponse("Logon"), JsonRequestBehavior.AllowGet);
            //}
            //if (Request["hdnschetype"] == "day")
            //{
            //    return RedirectToAction("Schedulespecs");
            //}
            //else
            //{
            //    return RedirectToAction("WeeklyAppts");
            //}
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public ActionResult audioPlay(string GetApptfilename)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string Requesturl = null, url = null;
                if (Request.Url != null)  Requesturl = Request.Url.ToString();
                var strurl = Requesturl.Split('/');
               
                if (clsWebConfigsettings.GetConfigSettingsValue("Islocal") == "Y")
                {
                    if (Requesturl.Contains("localhost:"))
                    {

                        url = strurl[0] + "//" + strurl[2] + "/" + "Attachments/Audiofiles/" + GetApptfilename ;
                        
                    }
                    else if (Requesturl.Contains("vbv"))
                    {

                        url = strurl[0] + "//" + strurl[2] + "/" + strurl[3] + "/" + "Attachments/Audiofiles/" + GetApptfilename;
                       
                    }
                }
                else
                {
                    url = clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal") + "Attachments/Audiofiles/" + GetApptfilename;
                   
                }
                ViewBag.getApptfilename = url;
                //clsWebConfigsettings.GetConfigSettingsValue("imagechecklocal") + "Attachments/Audiofiles/" + GetApptfilename;
                return View();
            }
            else
            {
                if (Request.IsAjaxRequest())
                {

                    return RedirectToRoute("SessionExpirepopup");
                }
                else
                {
                    return RedirectToRoute("SessionExpire");
                }
            }
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous()]
        public JsonResult FillCourtLocations()
        {
            IList<SelectListItem> OtherAddressItemList = new List<SelectListItem>();
            IDataParameter[] objparam = { new SqlParameter("@In_Provider_id", Convert.ToString(Session["roleid"]) != "1" ? Session["Prov_id"] : Convert.ToString(Session["ComboProv_ID"])) };
            objCommon.AddInParameters(objparam);
            SqlDataReader objread = default(SqlDataReader);
            objread = objCommon.GetDataReader("Help_dbo.St_List_get_appointment_adress");
            while (objread.Read())
            {
                //if (Convert.ToString(objread["courtlocation"]) != "")
                //{
                OtherAddressItemList.Add(new SelectListItem
                {
                    Text = Convert.ToString(objread["courtlocation"]),
                    Value = Convert.ToString(objread["Appointment_address_id"])
                });
                //  }
            }
            OtherAddressItemList.Add(new SelectListItem
            {
                Text = "------------------------",
                Value = ""
            });
            OtherAddressItemList.Add(new SelectListItem
            {

                Text = "New Location",
                Value = "0"
            });
            return Json(OtherAddressItemList, JsonRequestBehavior.AllowGet);
        }
    }
}
