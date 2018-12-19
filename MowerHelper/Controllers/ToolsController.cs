using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using PayPal.Tenniscoach;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.ProviderRegistration;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;

namespace MowerHelper.Controllers
{
    public class ToolsController : Controller
    {
       // int intCounter;
        string objstorecreditcardresponse;
       // private const int StartingYear = 2014;
       
        public DataSet DsTopSection;
        public ActionResult Topsection()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            if (Session["TopMenu"] == null)
            {
                GetHeaderSectionDataSetDetails();
            }
            else
            {
                ViewBag.topsection = Session["TopMenu"];
            }
            if (Convert.ToString(Session["Roleid"])!="1"){
                  GetPatientAlphabets();
            }
            else if (Convert.ToString(Session["Roleid"])=="1")
            {
                GetProviderAlphabets();
            }
            string browser = Convert.ToString(Request.UserAgent);
            if (browser.Contains("iPad"))
            {
                //   ViewBag.browser = browser;
                string name = Convert.ToString(Session["FullName"]);
                if (name.Length >= 10)
                {
                    name = name.Substring(0, 10);
                    name = name + "..";
                }
                ViewBag.fullname = name;
            }
            else
            {
                ViewBag.fullname = Session["FullName"];
            }
            ViewBag.lastlogin = "(Last web login : " + Session["BeforeLoginDate"] + ")";
            ViewBag.mobilelogin = "(Last mobile login : " + Session["MobileLoginDate"] + ")";
            if (Convert.ToString(Session["roleid"]) == "31")
            {
                ViewBag.HomeUrl = "../VerificationUser/ProviderProfile";
            }
            else if (Convert.ToString(Session["roleid"]) == "38")
            {
                ViewBag.HomeUrl = "../VerificationUser/Userprofile";
            }
            else
            {
                if (Convert.ToString(Session["Licenseverified"]) == "Y")
                {
                    ViewBag.HomeUrl = "../Schedule/WeeklyAppts";
                }
                else if (Convert.ToString(Session["Licenseverified"]) != "Y")
                {
                    ViewBag.HomeUrl = "../Directory/ViewIdentifyingInfo";
                }
                else
                {
                    ViewBag.HomeUrl = "../Tools/ToolsHome";

                }
            }
            ViewBag.Roleid = Convert.ToString(Session["Roleid"]);
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("Showschedule").ToUpper() == "YES")
            {
                ViewBag.Showschedule = "Y";
                if (Convert.ToString(Session["Roleid"]) == "4")
                {
                    DataSet ds_Categories = Category.GetMs_GetCategoriesDS(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]));
                    if (ds_Categories.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds_Categories.Tables[0].Rows[0]["MessageCategory_ID"]) == "10")
                        {
                            ViewBag.aptTotcount = "Requests (" + Convert.ToInt32(ds_Categories.Tables[0].Rows[0]["NewMsgsCount"]) + ")";
                        }
                    }
                    else
                    {
                        ViewBag.aptTotcount = "Requests (0)";
                    }
                }
            }
            else
            {
                ViewBag.Showschedule = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowElectricianInPublic").ToUpper() == "YES")
            {
                ViewBag.ShowElectricians = "Y";
            }
            else
            {
                ViewBag.ShowElectricians = "N";
            }
            //if (Session["Roleid"].ToString() == "4")
            //{
            //    var obj = new Paymentmethods();
            //    List<Paymentmethods> objcardexpirylist = Paymentmethods.CreditCard_Expirylist_info(obj);
            //    ViewBag.ccinfoid = objcardexpirylist[0].CreditCardInfo_ID;
            //    ViewBag.daysremining = objcardexpirylist[0].Daysremining;
            //}
            //else
            //{
            //    ViewBag.ccinfoid = null;
            //}
            string strDate = DateTime.Now.ToShortDateString();
            ViewBag.frmdate = DateTime.Parse(strDate).AddDays(-6).ToShortDateString();
            ViewBag.todate = strDate;
            ViewBag.lblHeaderweb = "Web login history details from " + ViewBag.frmdate + " to " + strDate;
            ViewBag.lblHeadermobile = "Mobile login history details from " + ViewBag.frmdate + " to " + strDate;
            
            
            return View();
        }


        public JsonResult GetAptreqcount()
        {
            if (Session["UserID"] == null)
            {
                return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.AllowGet);
            }
            Category userResultModel = new Category();
            DataSet ds_Categories = Category.GetMs_GetCategoriesDS(Convert.ToInt32(Session["Userid"]), Convert.ToInt32(Session["Roleid"]));
            if (ds_Categories.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds_Categories.Tables[0].Rows[0]["MessageCategory_ID"]) == "10")
                {
                    userResultModel.NewMsgsCount = Convert.ToInt32(ds_Categories.Tables[0].Rows[0]["NewMsgsCount"]);
                }
            }
                        else
            {
                ViewBag.NewMsgsCount = "0";
            }

            return Json(userResultModel, JsonRequestBehavior.AllowGet);
        }

        private void GetHeaderSectionDataSetDetails()
{
            try
{
	var strDisplay = new System.Text.StringBuilder();
    string strselect = null;
    if (Convert.ToString(Session["RoleID"]) == "4" || Convert.ToString(Session["RoleID"]) == "39")
        {           
            //string[] MenuNames = { "My"+"&nbsp;"+"Info", "Schedule", "Billing", "Messages", "Tools" };
            //string[] MenuUrls = { "Directory/ViewIdentifyingInfo", "Schedule/WeeklyAppts", "Billing/TransactionDisplay", "Messages/MessageFromHelp", "OfficeExpenses/ExpenseLedger" };            
            string[] MenuNames = { "My" + "&nbsp;" + "Info", "Schedule", "Billing", "Notes", "Tools" };
            string[] MenuUrls = { "Directory/ViewIdentifyingInfo", "Schedule/WeeklyAppts", "Billing/TransactionDisplay", "Notes/ProfileNotes", "OfficeExpenses/ExpenseLedger" };
            for (int i = 0; i <= 4; i++)
            {
                strDisplay.Append("<a  class=' " + strselect +"' style='TEXT-DECORATION: none' href='../" +MenuUrls[i] + "' id='Menu" + i + "' >");
                strDisplay.Append("" + MenuNames[i] + "");
                strDisplay.Append("</a>");
                strselect = null;
            }
        }
    else if (Session["UserID"] == null || Session.IsNewSession)
    {
        string[] MenuNames = { "Home", "Messages", "Tools" };
        string[] MenuUrls = { "VerificationUser/ProviderProfile", "Messages/MessageFromHelp", "VerificationUser/Licenseexpirelist" };
        for (int i = 0; i <= 2; i++)
        {
            strDisplay.Append("<a  class=' " + strselect + "' style='TEXT-DECORATION: none' href='../" + MenuUrls[i] + "' id='../" + MenuUrls[i] + "' >");
            strDisplay.Append("" + MenuNames[i] + "");
            strDisplay.Append("</a>");
            strselect = null;
        }
      
    }
        else if (Convert.ToString(Session["RoleID"]) == "1")
        {
            string[] MenuNames = { "Home", "Schedule", "Messages", "Tools" };
            string[] MenuUrls = { "Providers/Providerprofile", "Schedule/ScheduleSpecs", "Messages/Contactusentries", "AdminHome/Tools" };
            for (int i = 0; i <= 3; i++)
            {
                strDisplay.Append("<a  class=' " + strselect + "' style='TEXT-DECORATION: none' href='../" + MenuUrls[i] + "' id='Menu" + i + "' >");
                strDisplay.Append("" + MenuNames[i] + "");
                strDisplay.Append("</a>");
                strselect = null;
            }
        }
        else if (Convert.ToString(Session["RoleID"]) == "31")
        {
            string[] MenuNames = { "Home",  "Messages", "Tools" };
            string[] MenuUrls = { "VerificationUser/ProviderProfile", "Messages/MessageFromHelp", "VerificationUser/Licenseexpirelist" };
            for (int i = 0; i <= 2; i++)
            {
                strDisplay.Append("<a  class=' " + strselect + "' style='TEXT-DECORATION: none' href='../" + MenuUrls[i] + "' id='../" + MenuUrls[i] + "' >");
                strDisplay.Append("" + MenuNames[i] + "");
                strDisplay.Append("</a>");
                strselect = null;
            }
        }
        else if (Convert.ToString(Session["RoleID"]) == "38")
        {
            string[] MenuNames = { "Home", "Messages", "Tools" };
            string[] MenuUrls = { "VerificationUser/Userprofile", "Messages/MessageFromHelp", "VerificationUser/Licenseexpirelist" };
            for (int i = 0; i <= 2; i++)
            {
                strDisplay.Append("<a  class=' " + strselect + "' style='TEXT-DECORATION: none' href='../" + MenuUrls[i] + "' id='../" + MenuUrls[i] + "' >");
                strDisplay.Append("" + MenuNames[i] + "");
                strDisplay.Append("</a>");
                strselect = null;
            }
        }
    if (Convert.ToString(Session["RoleID"]) == "8")
        {
            string[] MenuNames = { "Schedule" };
            string[] MenuUrls = { "Schedule/WeeklyAppts" };
            for (int i = 0; i <= 0; i++)
            {
                strDisplay.Append("<a  class=' " + strselect + "' style='TEXT-DECORATION: none' href='../" + MenuUrls[i] + "' id='../" + MenuUrls[i] + "' >");
                strDisplay.Append(""+ MenuNames[i] + "");
                strDisplay.Append("</a>");
                strselect = null;
            }
        }

        ViewBag.topsection =  Convert.ToString(strDisplay) ;
        Session.Add("TopMenu", ViewBag.topsection);
}
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ToolsController", "GetHeaderSectionDataSetDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
}

        public void GetPatientAlphabets()
        {
            try
            {
            if (Request["register"] != null)
            {
                Session["dsalphabets"] = null;
            }
            //if (Session["dsalphabets"] == null)
            //{
                var dsalphabets = PTHome.getPatientAlphabets();
                if (dsalphabets.Tables.Count > 0)
                {
                    if (dsalphabets.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.patalphabets = dsalphabets.Tables[0].AsEnumerable().GetEnumerator();
                        Session.Add("dsalphabets", dsalphabets.Tables[0].AsEnumerable().GetEnumerator());
                    }
                }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ToolsController", "GetPatientAlphabets", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            //}
            //else
            //{
            //    ViewBag.patalphabets = Session["dsalphabets"];
            //}
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
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ToolsController", "GetProviderAlphabets", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }

        public ActionResult Bottomsections()
        {
            return View();
        }
        public ActionResult BottomsectionsNew()
        {
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult ExpiryPaymentMethod(string id, string expdate)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.expdate ="Your card has been expired in " + expdate + "days ";

            var cardstype = clsCommonCClist.GetCCList();

            var month = clsCommonCClist.GetCCMonth();

            var year = clsCommonCClist.GetCCYear();

            var reg1 = clsCommonCClist.GetStates();

            //var cardtype = new Reg_CreditCardProcess();
            //List<Reg_CreditCardProcess> cardlist = cardtype.Get_Creditcardtype();
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
            //int[] monthArray = null;
            //monthArray = new int[12];
            //for (int intVal = 0; intVal <= 11; intVal++)
            //{
            //    monthArray[intVal] = intVal + 1;
            //}
            //IList<SelectListItem> month = new List<SelectListItem>();
            //if (monthArray.Length > 0)
            //{
            //    for (int i = 0; i <= monthArray.Length - 1; i++)
            //    {
            //        if (monthArray[i] == 1)
            //        {
            //            month.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(monthArray[i]),
            //                Text = Convert.ToString(monthArray[i]),
            //                Selected = true
            //            });
            //        }
            //        else
            //        {
            //            month.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString(monthArray[i]),
            //                Text = Convert.ToString(monthArray[i]),
            //                Selected = false
            //            });
            //        }

            //    }
            //}
            //IList<SelectListItem> year = new List<SelectListItem>();
            //for (this.intCounter = DateTime.Now.Year; this.intCounter <= DateTime.Now.Year + 12; this.intCounter++)
            //{
            //    year.Add(new SelectListItem
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
            ////        year.Add(new SelectListItem
            ////        {
            ////            Value = this.intCounter.ToString(),
            ////            Text = this.intCounter.ToString(),
            ////            Selected = true
            ////        });
            ////    }
            ////    else
            ////    {
            ////        year.Add(new SelectListItem
            ////        {
            ////            Value = this.intCounter.ToString(),
            ////            Text = this.intCounter.ToString(),
            ////            Selected = false
            ////        });
            ////    }

            ////}
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
            ViewData["Exmonth"] = month;
            ViewData["Exyear"] = year;
            ViewData["ExCardType"] = cardstype;
            ViewBag.Practice_ID = Convert.ToString(Session["Practice_ID"]);
            ViewBag.Provider_ID = Convert.ToString(Session["Prov_ID"]);
            string vaultid = CCProcess.GetVaultID(id);
            Filldata(id, vaultid);
            FillProviderAddress();
            return View("ExpiryPaymentMethod", reg1);
        }

        private void FillProviderAddress()
        {
            try
            {
            var objcomm = new clsCommonFunctions();
            Reg_ProvidersDetailInfo objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
            if ((objInfo != null))
            {

                ViewBag.PAddress1 = !string.IsNullOrEmpty(objInfo.Address1) ? objInfo.Address1 : null;
                ViewBag.PAddress2 = !string.IsNullOrEmpty(objInfo.Address2) ? objInfo.Address2 : null;
                ViewBag.PZip = !string.IsNullOrEmpty(objInfo.Zip) ? objInfo.Zip : null;

            }
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ToolsController", "FillProviderAddress", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public void GetProviderDetails()
        {
            try
            {
            Reg_ProvidersDetailInfo objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
            if ((objInfo != null))
            {
                ViewBag.FirstName = !string.IsNullOrEmpty(objInfo.FirstName) ? objInfo.FirstName : null;
                ViewBag.LastName = !string.IsNullOrEmpty(objInfo.LastName) ? objInfo.LastName : null;
                ViewBag.Address1 = !string.IsNullOrEmpty(objInfo.Address1) ? objInfo.Address1 : null;
                ViewBag.Address2 = !string.IsNullOrEmpty(objInfo.Address2) ? objInfo.Address2 : null;
                ViewBag.Zip = !string.IsNullOrEmpty(objInfo.Zip) ? objInfo.Zip : null;


                if (!string.IsNullOrEmpty(objInfo.State_ID.ToString()))
                {
                    ViewBag.State_ID = objInfo.State_ID;
                }
                else
                {
                    ViewBag.State_ID = null;
                }
                if (!string.IsNullOrEmpty(objInfo.City_ID.ToString()))
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
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();

                clsexp.LogException(ex, "ToolsController", "GetProviderDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        private void Filldata(string id, string valutid)
        {
            try
            {
                ViewBag.CardID = id;
                APIContext apiContext = Configuration.GetAPIContext();
                CreditCard getcardinfo = CreditCard.Get(apiContext, valutid);

                var obj = new CCProcess { CardID = id, Provider_ID = Convert.ToString(Session["Prov_ID"]) };
                List<CCProcess> objlist = CCProcess.CreditCard_Get_paymentinfo(obj);
                if (objlist.Count > 0 & getcardinfo != null)
                {
                    ViewBag.CardType = objlist[0].strx_card_code ?? null;
                    ViewBag.cardnumber = getcardinfo.number;
                    ViewBag.cvv2 = getcardinfo.cvv2;
                    ViewBag.FirstName = getcardinfo.first_name;
                    ViewBag.LastName = getcardinfo.last_name;

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
                }
            }
            catch (Exception ex)
            {
                var log = new clsExceptionLog();
                log.LogException(ex, "AdminController", "Filldata", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                Session.Add("IsPopUp", "true");
                ViewBag.ExipryMonth = null;
                ViewBag.ExipryYear = null;
                ViewBag.Address1 = null;
                ViewBag.Address2 = null;
                ViewBag.Zip = null;
                ViewBag.State_ID = null;
                ViewBag.City_ID = null;
                ViewBag.Statename = null;
                ViewBag.Cityname = null;
                ViewBag.practice_ind = null;
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public JsonResult ExpiryPaymentMethod(Reg_CreditCardProcess obj)
        {
            int? Out_CCID = 0;
            CreditCard crdtCard = new CreditCard();
          
            if (Request["ExCardType"] != "")
            {
                switch (Request["ExCardType"])
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
            if (Request["ExCardNumber"] != "")
            {
                crdtCard.number = Request["ExCardNumber"].ToString();
            }
            if (Request["ExtxtCVV2"] != "")
            {
                crdtCard.cvv2 = Request["ExtxtCVV2"].ToString();
            }
            else
            {
                crdtCard.cvv2 = null;
            }
            if (Request["Extxtfirstname"] != "")
            {
                crdtCard.first_name = Request["Extxtfirstname"].ToString();
            }
            else
            {
                crdtCard.first_name = null;
            }
            if (Request["Extxtlastname"] != "")
            {
                crdtCard.last_name = Request["Extxtlastname"].ToString();
            }
            else
            {
                crdtCard.last_name = null;
            }
            if (Request["Exmonth"] != "")
            {
                crdtCard.expire_month = Convert.ToInt32(Request["Exmonth"]);
            }

            if (Request["Exyear"] != "")
            {
                crdtCard.expire_year = Convert.ToInt32(Request["Exyear"]);
            }
            if (Request["ExHdnProvider_ID"] != null)
            {
                crdtCard.payer_id = Request["ExHdnProvider_ID"];
            }

            Address billingAddress = new Address();
            billingAddress.country_code = "US";
            if (Request["Exhdnadd1"] != "")
            {
                billingAddress.line1 = Request["Exhdnadd1"];
            }
            else
            {
                billingAddress.line1 = null;
            }
            if (Request["Exhdnadd2"] != "")
            {
                billingAddress.line2 = Request["Exhdnadd2"];
            }
            else
            {
                billingAddress.line2 = null;
            }
            if (Request["Exhdnzip"] != "")
            {
                billingAddress.postal_code = Request["Exhdnzip"].ToString();
            }
            else
            {
                billingAddress.postal_code = null;
            }
            if (Request["Exhdnstate"] != "")
            {
                billingAddress.state = clsCommonCClist.Getstatename(Request["Exhdnstate"]);
            }
            else
            {
                billingAddress.state = null;
            }
            if (Request["Exhdncity"] != "")
            {
                billingAddress.city = clsCommonCClist.Getcityname(Request["Exhdncity"]);
            }
            else
            {
                billingAddress.city = null;
            }
            crdtCard.billing_address = billingAddress;
            Amount amnt = new Amount();
            amnt.currency = "USD";
            if (Request["Exhdmamount"] != "")
            {
                amnt.total = Request["Exhdmamount"].ToString();
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
                APIContext apiContext = Configuration.GetAPIContext();
                Payment createdPayment = pymnt.Create(apiContext);
                JsonResult objser = new JsonResult();
                string objrequest = JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented);
                if (createdPayment != null & objrequest != "" & createdPayment.state == "approved")
                {
                  
                    // if (createdPayment.state == "approved")
                    //{
                    CCProcess objinsert = new CCProcess();
                    objinsert.StrPaidRefID = "1";
                    objinsert.StrPaidRefTypeID = "1";
                    objinsert.StrChargebleRefID = Request["ExHdnProvider_ID"];
                    objinsert.StrChargebleRefTypeID = "2";
                    objinsert.strpost = objrequest;
                    objinsert.RefLoginID = 0;
                    objinsert.strPracticeID = Request["ExHdnPractice_ID"];
                    objinsert.CVV2 = null;
                    objinsert.strx_card_code = crdtCard.type;
                    objinsert.strStrCardType = null;
                    objinsert.strStateID = Request["Exhdnstate"];
                    objinsert.strZipCode = Convert.ToString(Request["Exhdnzip"]);
                    objinsert.strCityID = Request["Exhdncity"];
                    objinsert.strBillAddress1 = billingAddress.line1;
                    objinsert.strBillAddress2 = billingAddress.line2;
                    objinsert.FirstName = Request["Extxtfirstname"];
                    objinsert.LastName = Request["Extxtlastname"];
                    objinsert.strx_invoice_num = null;
                    objinsert.strx_card_num = null;
                    objinsert.IsPaypalInd = "Y";
                    objinsert.Provider_ID = Request["ExHdnProvider_ID"];
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
                    clsCommonFunctions objcommon = new clsCommonFunctions();
                    objinsert.CreatedBy = objcommon.GetProviderLoginID(Convert.ToString(Request["ExHdnProvider_ID"]));
                    objinsert.ALLowCCCharges = "Y";
                    string Mycardno = Convert.ToString(Request["ExCardNumber"]);
                    objinsert.LastFourdigitCCNo = Mycardno.Substring(Mycardno.Length - Math.Min(4, Mycardno.Length));
                    if (Request["Exchkaddress"] != null)
                    {
                        if (Request["Exchkaddress"] == "true,false")
                        {
                            objinsert.practice_ind = "Y";
                        }
                        else
                        {
                            objinsert.practice_ind = "N";
                        }
                    }
                    //if (Request["ExhdnCardid"] != null)
                    //{
                    //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                    //}
                    CreditCard newCreditCard = crdtCard.Create(apiContext);
                    if (newCreditCard.state == "ok")
                    {
                        MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,!string.IsNullOrEmpty(Request["ExhdnCardid"])?Request["ExhdnCardid"]:null);
                        //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                        CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), newCreditCard.id, Out_CCID);
                    }

                    Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);
                    //CreditCard strcard = crdtCard.Create(apiContext);
                    objstorecreditcardresponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);
                    int LoginId = PTHome.GetProviderLoginID(Convert.ToInt32(Request["ExHdnProvider_ID"]));

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
                        //Reg_ProviderConfirmation.Reg_Upd_Status(Convert.ToInt32(Request["ExHdnProvider_ID"]), "A", null, null, strcard.id);
                        obj.Provider_ID = Convert.ToInt32(Request["ExHdnProvider_ID"]);
                        //Message newMessage = new Message(LoginId, "Registration Alert", "New Electrician Registration", 4, "0", null, null, 1, "1");
                        //newMessage.Save();
                        Session["CCexists"] = "Y";
                        return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on Process"), JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                }
            }
            catch (PayPal.PayPalException ex)
            {
                var objex = new clsExceptionLog();
                objex.LogException(ex, "Tools", "ExpiryPaymentMethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on save"), JsonRequestBehavior.DenyGet);
        }
        //public string Getstatename(string stateid)
        //{
        //    clsCommonFunctions clscommon = new clsCommonFunctions();
        //    SqlDataReader dr_GetName;
        //    IDataParameter[] InParmList = { new SqlParameter("@in_state_id", stateid) };
        //    clscommon.AddInParameters(InParmList);
        //    dr_GetName = clscommon.GetDataReader("Help_dbo.St_get_stateabbrevation");
        //    while (dr_GetName.Read())
        //    {
        //        return dr_GetName["state"]!= null ? dr_GetName["state"].ToString() : null;
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
        //        return dr_GetName["City"] != null ? dr_GetName["City"].ToString() : null;
        //    }
        //    return null;
        //}
        //private string Encrypt(string clearText)
        //{
        //    try{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        var clsexp = new clsExceptionLog();

        //        clsexp.LogException(ex, "ToolsController", "Encrypt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
        //    }
        //    return clearText;
        //}
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult WebLoginHistoryDetails()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.pagesize = "10";
            List<Login_history> Login_historycollection = default(List<Login_history>);
            Login_history objloginhistory = new Login_history();
            objloginhistory.Login_ID = Convert.ToInt32(Session["UserID"]);
            objloginhistory.Mobile_type = "W";
            // objloginhistory.OrderByItem = !string.IsNullOrEmpty(Request["sort"]) ? Request["sort"] : null;
            // objloginhistory.OrderBy = !string.IsNullOrEmpty(Request["sortdir"]) ? Request["sortdir"] : null;// Request["sortdir"] == null ? null : Request["sortdir"];
            objloginhistory.NoofRecords = 10;
            objloginhistory.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            int totcount = 0;
            Login_historycollection = Login_history.WebLoginHistoryDetails(objloginhistory, ref totcount);
            if (Login_historycollection.Count > 0)
            {
                ViewBag.totrec = totcount;
            }
            else
            {
                ViewBag.totrec = 0;
            }
            return View(Login_historycollection);
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MobileLoginHistoryDetails()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.pagesize = "10";
            List<Login_history> Login_historycollection = default(List<Login_history>);
            Login_history objloginhistory = new Login_history();
            objloginhistory.Login_ID = Convert.ToInt32(Session["UserID"]);
            objloginhistory.Mobile_type = "M";
            //  objloginhistory.OrderByItem = !string.IsNullOrEmpty(Request["sort"]) ? Request["sort"] : null;
            // objloginhistory.OrderBy = !string.IsNullOrEmpty(Request["sortdir"]) ? Request["sortdir"] : null;// Request["sortdir"] == null ? null : Request["sortdir"];
            objloginhistory.NoofRecords = 10;
            objloginhistory.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            int totcount = 0;
            Login_historycollection = Login_history.WebLoginHistoryDetails(objloginhistory, ref totcount);
            if (Login_historycollection.Count > 0)
            {
                ViewBag.totrec = totcount;
            }
            else
            {
                ViewBag.totrec = 0;
            }
            return View(Login_historycollection);
        }
    }
}
