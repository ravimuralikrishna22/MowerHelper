using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Providers;

namespace MowerHelper.Controllers
{
    public class signupsController : Controller
    {
        int intCounter;
       // private const int StartingYear = 2015;
        [ActionName("NewSignups")]
        public ActionResult Index(string LastName, string year, string Month_ID)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("SessionExpire", "Home");
            }
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            IList<SelectListItem> year1 = new List<SelectListItem>();
            //for (this.intCounter = StartingYear; this.intCounter >= DateTime.Now.Year - 5; -- this.intCounter)
            //{
            //    if (this.intCounter == 2015)
            //    {
            //        year1.Add(new SelectListItem
            //        {
            //            Value = this.intCounter.ToString(),
            //            Text = this.intCounter.ToString(),
            //            Selected = true
            //        });
            //    }
            //    else
            //    {
            //        year1.Add(new SelectListItem
            //        {
            //            Value = this.intCounter.ToString(),
            //            Text = this.intCounter.ToString(),
            //            Selected = false
            //        });
            //    }
            //}
            for (this.intCounter = DateTime.Now.Year; this.intCounter >= DateTime.Now.Year - 5; --this.intCounter)
            {
                year1.Add(new SelectListItem
                {
                    Value = this.intCounter.ToString(),
                    Text = this.intCounter.ToString(),
                    Selected = true
                });
            }
            ViewData["year"] = year1;
            List<Provider_ProviderList> Providercount = new List<Provider_ProviderList>();
            if (year == null)
            {
                year = "2015";
                ViewBag.yeardropdown = "2015";
            }
            else
            {
                ViewBag.yeardropdown = year;
            }
            Providercount = Provider_ProviderList.signups_monthWisecount(year);
            if (Providercount.Count > 0)
            {
                ViewBag.January = "(" + Providercount[0].January + ")";
                ViewBag.February = "(" + Providercount[0].February + ")";
                ViewBag.March = "(" + Providercount[0].March + ")";
                ViewBag.April = "(" + Providercount[0].April + ")";
                ViewBag.May = "(" + Providercount[0].May + ")";
                ViewBag.June = "(" + Providercount[0].June + ")";
                ViewBag.July = "(" + Providercount[0].July + ")";
                ViewBag.August = "(" + Providercount[0].August + ")";
                ViewBag.September = "(" + Providercount[0].September + ")";
                ViewBag.October = "(" + Providercount[0].October + ")";
                ViewBag.November = "(" + Providercount[0].November + ")";
                ViewBag.December = "(" + Providercount[0].December + ")";
            }
            else
            {
                ViewBag.January = "(0)";
                ViewBag.February = "(0)";
                ViewBag.March = "(0)";
                ViewBag.April = "(0)";
                ViewBag.May = "(0)";
                ViewBag.June = "(0)";
                ViewBag.July = "(0)";
                ViewBag.August = "(0)";
                ViewBag.September = "(0)";
                ViewBag.October = "(0)";
                ViewBag.November = "(0)";
                ViewBag.December = "(0)";
            }

                 
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
            ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
            ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "30" : Request["dt_filter"]);
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            {
                FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"]);
                ToDate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? startdate : Request["txt_ToDate"]);
            }
            else if (Request["dt_filter"] == "All")
            {
                FromDate = null;
                ToDate = null;
}
            else
            {

                if (Request["dt_filter"] == "Today")
                {
                    FromDate = startdate;
                }
                else if (Request["dt_filter"] == "7")
                {
                    FromDate = DateTime.Parse(startdate).AddDays(-6).ToString("MM/dd/yyyy");
                }
                else if (Request["dt_filter"] == "30")
                {
                    FromDate = DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy");
                }
                ToDate = startdate;
            }
            if (Month_ID != null)
            {
                FromDate = Month_ID + "/" + "01/" + year;
                var lastDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(Month_ID), DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(Month_ID)));
                ToDate = lastDate.ToShortDateString();
                ViewBag.Fromdate = FromDate;
                ViewBag.Todate = ToDate;
                ViewBag.Daterange = "Custom";
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
            string PageNo = null;
            string NoofRecords = null;

            PageNo = (Request["page"] != null ? Request["page"] : "1");
          //  Nullable<int> _NPracID = null;
           // string PlaceOfService_ID = null;
            int totrec = 0;
            int ProvID = 0;
            string stremail = null;
            if (Request["ComboBox1"] != null & Request["ComboBox1"] != "")
            {
                ProvID = Convert.ToInt32(Request["ComboBox1"]);
                ViewBag.ProvID = Convert.ToInt32(Request["ComboBox1"]);
                TempData["combotext"] = Convert.ToString( Request["ComboBox1_SelectedText"]);
            }
            else
            {
                ViewBag.ProvID = null;
                TempData["combotext"] = "";
            }
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"].ToString();
                NoofRecords = Request["ddlrecords"].ToString();
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                NoofRecords = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.pagesize = "10";
                NoofRecords = "10";
            }
            if (Request["txtEmail"] != null & Request["txtEmail"] != "")
            {
                ViewBag.email = Request["txtEmail"];
                stremail = Request["txtEmail"];
            }
            else
            {
                ViewBag.email = null;
                stremail = null;
            }
            List<Provider_ProviderList> ElProviderList = new List<Provider_ProviderList>();
            ElProviderList = Provider_ProviderList.Get_NewElesignups("ProviderName", "ASC", Convert.ToInt32(ViewBag.pagesize), FromDate, ToDate,
Convert.ToInt32(PageNo), ref totrec, stremail, ProvID);
            ViewBag.totrec = totrec;
            return View("Index",ElProviderList);
        }

    }
}
