using Newtonsoft.Json;
//using System.Xml;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using PayPal.Tenniscoach;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.ProviderRegistration;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.Technicians;
using MowerHelper.Models.Classes;
//using PayPal.Api;



namespace MowerHelper.Controllers
{
    public class TechniciansController : Controller
    {
        //
        // GET: /Technicians/
        clsCommonFunctions objCommon = new clsCommonFunctions();
      //  int intCounter;
        //int StartingYear = 2014;
        //string objresponse;
        string objstorecreditcardresponse;
        TechniciansInfo objinfo = new TechniciansInfo();
    
        public ActionResult Index(string alert, string Tid)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/SessionExpire");
            }
            //clsWebConfigsettings clsweb = new clsWebConfigsettings();
            if (clsWebConfigsettings.GetConfigSettingsValue("ShowForumsInElectricianLogin").ToUpper() == "YES")
            {
                ViewBag.Showforums = "Y";
            }
            else
            {
                ViewBag.Showforums = "N";
            }
            if (clsWebConfigsettings.GetConfigSettingsValue("Showtask").ToUpper() == "YES")
            {
                ViewBag.Showtask = "Y";
            }
            else
            {
                ViewBag.Showtask = "N";
            }
            ViewBag.alert = alert;
            ViewBag.Tid = Tid;
            if (Session["Prov_ID"] != null)
            {

                objinfo.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            }
            else
            {

                objinfo.Provider_id = null;
            }
            if (Request["lastname"] != null)
            {
                objinfo.Last_Name = Request["lastname"].ToString();
                ViewBag.lastnm = Request["lastname"].ToString();
            }
            else
            {
                objinfo.Last_Name = null;
                ViewBag.lastnm = null;
            }
            if (Request["sortdir"] != "" && Request["sortdir"] != null)
            {
                objinfo.OrderBy = Request["sortdir"].ToString();
            }
            else
            {
                objinfo.OrderBy = "ASC";
            }
            objinfo.OrderByItem = "Lastname";
            objinfo.noofrowsperpage = 10;
            objinfo.PageNO = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            var objcollection = TechniciansInfo.GetTechniciansInfo(objinfo);
            if (objcollection.Count > 0)
            {
                ViewBag.totrec = objcollection.Count;
            }
            else
            {
                ViewBag.totrec = null;
            }
            List<TechniciansInfo> objlist = new List<TechniciansInfo>();
            TechniciansInfo gettechniciancount = new TechniciansInfo();
            objlist = gettechniciancount.GetTechniciancount(Convert.ToInt32(Session["UserID"]));
            ViewBag.billingchange_ind = objlist[0].billingchange_ind;

            return View(objcollection);
        }

        public ActionResult AddNewTechnician(string id)
        {


            return View();
        }
        [HttpPost()]
        public JsonResult AddNewTechnician()
        {

            if (Session["Prov_ID"] != null)
            {

                objinfo.Provider_id = Convert.ToInt32(Session["Prov_ID"]);
            }
            else
            {

                objinfo.Provider_id = null;
            }
            if (Request["txtFirstName"] != "")
            {
                objinfo.First_Name = Request["txtFirstName"].ToString();
            }
            else
            {
                objinfo.First_Name = null;
            }
            if (Request["txtLastName"] != "")
            {
                objinfo.Last_Name = Request["txtLastName"].ToString();
            }
            else
            {
                objinfo.Last_Name = null;
            }
            if (Request["txtLastName"] != "")
            {
                objinfo.Last_Name = Request["txtLastName"].ToString();
            }
            else
            {
                objinfo.Last_Name = null;
            }
            if (string.IsNullOrEmpty(Request["txtMobile1"]) & string.IsNullOrEmpty(Request["txtMobile2"]) & string.IsNullOrEmpty(Request["txtMobile3"]))
            {
                objinfo.Phonenumber = null;
            }
            else
            {
                objinfo.Phonenumber = Request["txtMobile1"] + Request["txtMobile2"] + Request["txtMobile3"];
            }
            if (Request["txtEmail"] != "")
            {
                objinfo.Email = Request["txtEmail"].ToString();
            }
            else
            {
                objinfo.Email = null;
            }
            if (Request["rbtself"] == "8")
            {
                objinfo.Roleid = "8";
            }
            else
            {
                objinfo.Roleid = "39";
            }
            var objmd5 = new VBVMD5CryptoServiceProvider();
            objinfo.Password = objmd5.getMd5Hash(Request["txtpwd"]);
            string Out_Msg = null;
            TechniciansInfo.InsTechniciansInfo(objinfo, ref Out_Msg);
            if (Out_Msg != "" & Out_Msg != null )
            {
                return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
            }
            return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
        }
        public ActionResult EditTechnician(int Technician_id)
        {
            if (Request["lbloutmsg"] == "Y")
            {
                ViewBag.outmsg = "Y";
            }
            objinfo = TechniciansInfo.GetupdTechnicianInfo(Technician_id);
            if (objinfo.First_Name != "")
            {
                ViewBag.firestnm = objinfo.First_Name;
            }
            else
            {
                ViewBag.firestnm = null;
            }
            if (objinfo.Last_Name != "")
            {
                ViewBag.lastnm = objinfo.Last_Name;
            }
            else
            {
                ViewBag.lastnm = null;
            }
            if (!string.IsNullOrEmpty(objinfo.Phonenumber))
            {
                ViewBag.phone = clsCommonFunctions.UsPhoneFormat(objinfo.Phonenumber);
                if (objinfo.Phonenumber.Length >= 10)
                {
                    ViewBag.ph1 = objinfo.Phonenumber.Substring(0, 3);
                    ViewBag.ph2 = objinfo.Phonenumber.Substring(3, 3);
                    ViewBag.ph3 = objinfo.Phonenumber.Substring(6, 4);
                }
            }
            if (objinfo.Email != "")
            {
                ViewBag.Email = objinfo.Email;
            }
            else
            {
                ViewBag.Email = null;
            }
            //clsWebConfigsettings objconfig = new clsWebConfigsettings();
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
            if (objinfo.Tech_Image != null)
            {
                string str = objinfo.Tech_Image.ToString();
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
                string strings = Path.Combine(Server.MapPath("~/Attachments/Technicians"), str);
                if (System.IO.File.Exists(strings))
                {

                    ViewBag.imageavail = "Y";
                    ViewBag.image = "Y";
                    ViewBag.providerimage = domain + "Attachments/Technicians/" + str;

                }


            }
            ViewBag.techid = Technician_id;
            return View();
        }
        [HttpPost()]
        public ActionResult EditTechnician(HttpPostedFileBase flLogo, string hdntechidedit)
        {
            if (flLogo != null)
            {
                if (flLogo.ContentLength > 0)
                {
                    Getfilename(flLogo);
                    if (!string.IsNullOrEmpty(Request["strimage"]))
                    {
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/Technicians"), Request["strimage"]);
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
                        string filepath = Path.Combine(Server.MapPath("~/Attachments/Technicians"), Request["strimage"]);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                    }
                }
            }
            if (Request["txtFirstNameedit"] != "")
            {
                objinfo.First_Name = Request["txtFirstNameedit"].ToString();
            }
            else
            {
                objinfo.First_Name = null;
            }
            if (Request["txtLastNameedit"] != "")
            {
                objinfo.Last_Name = Request["txtLastNameedit"].ToString();
            }
            else
            {
                objinfo.Last_Name = null;
            }
            if (string.IsNullOrEmpty(Request["txtMobile1edit"]) & string.IsNullOrEmpty(Request["txtMobile2edit"]) & string.IsNullOrEmpty(Request["txtMobile3edit"]))
            {
                objinfo.Phonenumber = null;
            }
            else
            {
                objinfo.Phonenumber = Request["txtMobile1edit"] + Request["txtMobile2edit"] + Request["txtMobile3edit"];
            }
            if (Request["hdntechidedit"] != null)
            {
                objinfo.Technician_id = Convert.ToInt32(Request["hdntechidedit"]);
            }
            else
            {
                objinfo.Technician_id = null;
            }
            int tech_id = Convert.ToInt32(Request["hdntechidedit"]);
            objinfo.Email= Request["txt_Email"];
            objinfo.status_ind = null;
            string Out_Msg = null;
            TechniciansInfo.UpdTechnicianInfo(objinfo, ref Out_Msg);
            if (Out_Msg != "" & Out_Msg != null)
            {
                //return Json(JsonResponseFactory.ErrorResponse(Out_Msg), JsonRequestBehavior.DenyGet);
                return RedirectToAction("EditTechnician", new { Technician_id = tech_id, lbloutmsg = "Y" });
            }
            else
            {
                string img_path = ViewData["LogoPath"] == null ? null : ViewData["LogoPath"].ToString();
                TechniciansInfo.upd_technicianimage(tech_id, img_path);
            }
            //return Json(JsonResponseFactory.SuccessResponse(), JsonRequestBehavior.DenyGet);
            return RedirectToAction("Index");
        }

        private bool Getfilename(HttpPostedFileBase flLogo)
        {
            try
            {
            string FileExtn1 = null;
            string strFilePath = null;
            FileExtn1 = System.IO.Path.GetExtension(flLogo.FileName);
            ViewData["EncryptLogo"] = clsCommonCClist.RandomPassword();
            string filename = Convert.ToString( ViewData["EncryptLogo"]) + FileExtn1;
            strFilePath = Path.Combine(Server.MapPath("~/Attachments/Technicians"), filename);
            ViewData["LogoPath"] = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
            flLogo.SaveAs(strFilePath);
            return true;
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "TechniciansController", "Getfilename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return false;
            }
        }
        //private string RandomPassword()
        //{
        //    return Convert.ToString(Guid.NewGuid());

        //}
        public ActionResult DeleteTechInfo(int Technician_id, string Reassigned)
        {
            
                objinfo.First_Name = null;
                objinfo.Last_Name = null;
                objinfo.Phonenumber = null;     
                objinfo.Technician_id = Technician_id;
            objinfo.status_ind = 'D';
            objinfo.Reassigned = Reassigned;
            string Out_Msg = null;
            TechniciansInfo.UpdTechnicianInfo(objinfo, ref Out_Msg);
            return RedirectToAction("Index", new { alert = Convert.ToString(Out_Msg), Tid = Convert.ToString(Technician_id) });
        }
        public JsonResult GetClientlastname(string term)
        {
            //if (Session["UserID"] == null)
            //{
            //    return Json(JsonResponseFactory.ErrorResponse("false"), JsonRequestBehavior.DenyGet);
            //}

            var objlist = new List<Autocomplete>();
            var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
		new SqlParameter("@in_Keyword", term),
        new SqlParameter("@in_provider_id",Convert.ToInt32(Session["Prov_ID"]))
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = objcomman.GetDataReader("Help_dbo.st_typeahead_technician_lastname");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
          [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult Addnewpaymentmethod(string cardexpiry, string ccexists)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("../Home/SessionExpire");
            }
          if (Convert.ToString( Session["CCexists"]) =="Y")
                {
                    CCProcess objcc = new CCProcess();
                    objcc.ReferenceTypeID = "2";
                    objcc.ReferenceID = Convert.ToString( Session["Prov_ID"]);
                    string ccinfoid = CCProcess.LoadCreditCardInfo(objcc);
                    string vaultid = CCProcess.GetVaultID(ccinfoid);

                    Filldata(Convert.ToString( ccinfoid), vaultid);
                    }
                else
                {
                    GetProviderDetails();
                    ViewBag.expdate = "Your card information ";
                }


            //Referrals description = new Referrals();
            //description.FieldIDString = "9";
            DataSet ds = new DataSet();
            ds = Referrals.List_field_description("9");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string str = Convert.ToString(ds.Tables[0].Rows[0][3]);
                ViewBag.regFee = str;
            }
            var cardstype = clsCommonCClist.GetCCList();

            var _month = clsCommonCClist.GetCCMonth();

            var _year = clsCommonCClist.GetCCYear();

            var reg1 = clsCommonCClist.GetStates();

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
            //                Value = Convert.ToString( cardlist[i].CreditCardType_ID),
            //                Text = "American Express",
            //                Selected = true
            //            });
            //        }
            //        else if (i == 3)
            //        {

            //            cardstype.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString( cardlist[i].CreditCardType_ID),
            //                Text = "Visa"
            //            });
            //        }
            //        else
            //        {

            //            cardstype.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString( cardlist[i].CreditCardType_ID),
            //                Text = Convert.ToString( cardlist[i].code)
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
            //                Value = Convert.ToString( MonthArray[i]),
            //                Text = Convert.ToString( MonthArray[i]),
            //                Selected = true
            //            });
            //        }
            //        else
            //        {
            //            _month.Add(new SelectListItem
            //            {
            //                Value = Convert.ToString( MonthArray[i]),
            //                Text = Convert.ToString( MonthArray[i]),
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
            //            Value = Convert.ToString( objstates[i].StateId)
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
            if (Convert.ToString( Session["roleid"]) != "1")
            {
                ViewBag.Practice_ID = Convert.ToString( Session["Practice_ID"]);
                ViewBag.Provider_ID = Convert.ToString( Session["Prov_ID"]);
            }
            else
            {
                ViewBag.Practice_ID = Convert.ToString( Session["ComboPractice_ID"]);
                ViewBag.Provider_ID = Convert.ToString(Session["ComboProv_ID"]);
            }


            DataSet dsrenewdetails = new DataSet();
            dsrenewdetails = Referrals.GetBillingChargeDetails(Convert.ToString( Session["userid"]));
            if (dsrenewdetails.Tables[0].Rows.Count > 0)
            {
                string CurrentmonthStart = Convert.ToDateTime(Convert.ToString( dsrenewdetails.Tables[0].Rows[0]["Currentmonthstartdate"])).ToShortDateString();
                string CurrentmonthEnd = Convert.ToDateTime(Convert.ToString( dsrenewdetails.Tables[0].Rows[0]["currentmonthsEnddate"])).ToShortDateString();
                ViewBag.currentmonthdates = CurrentmonthStart + " To " + CurrentmonthEnd;
                ViewBag.Remaingdaysfee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["remaingdays_servicefee"]) + " (Prorated)";
                ViewBag.Totalservicefee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["Totalservicefee"]);

                string NextmonthStart = Convert.ToDateTime(Convert.ToString( dsrenewdetails.Tables[0].Rows[0]["nextmonthstartdate"])).ToShortDateString();
                string NextmonthEnd = Convert.ToDateTime(Convert.ToString( dsrenewdetails.Tables[0].Rows[0]["nextmonthEnddate"])).ToShortDateString();
                ViewBag.NextmonthEnd = NextmonthEnd;
                ViewBag.nextmonthdates = NextmonthStart + " To " + NextmonthEnd;
                ViewBag.servicefee = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["servicefee"]);
                ViewBag.ctempid1 = Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["chargetempleate_id"]);
                ViewBag.billingservice_id = Convert.ToString(dsrenewdetails.Tables[0].Rows[0]["billingservice_id"]);
                ViewBag.Totalservicefee3 = dsrenewdetails.Tables[0].Rows[0]["Totalservicefee"];
                ViewBag.servicetype = dsrenewdetails.Tables[0].Rows[0]["name"];
                ViewBag.Billingservicename = dsrenewdetails.Tables[0].Rows[0]["Billingservicename"];

                string needtopay = String.Format("{0:0.00}", (Convert.ToDecimal(dsrenewdetails.Tables[0].Rows[0]["needtopay"])));
                ViewBag.needtopay = needtopay;
                ViewBag.needtopayamount = string.Format("{0:c}", dsrenewdetails.Tables[0].Rows[0]["needtopay"]) + " (Prorated)";
            }

            return View("Addnewpaymentmethod", reg1);
        }

          [HttpPost()]
          [ValidateAntiForgeryToken]
          public JsonResult Addnewpaymentmethod(Reg_CreditCardProcess obj)
          {
              if (Request.IsAjaxRequest() && Session["UserID"] == null)
              {
                  return Json(JsonResponseFactory.ErrorResponse("expire"), JsonRequestBehavior.DenyGet);
              }
              int? Out_CCID = 0;
              string stramount = null;
              string strchargetempleid = null;
              string strenddate = null;
              CreditCard crdtCard = new CreditCard();
              stramount = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnneedtopay"])));

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
                  crdtCard.number = Convert.ToString( Request["CardNumber"]);
              }
              if (Request["txtCVV2"] != "")
              {
                  crdtCard.cvv2 = Convert.ToString( Request["txtCVV2"]);
              }
              else
              {
                  crdtCard.cvv2 = null;
              }
              if (Request["txtfirstname"] != "")
              {
                  crdtCard.first_name = Request["txtfirstname"].ToString();
              }
              else
              {
                  crdtCard.first_name = null;
              }
              if (Request["txtlastname"] != "")
              {
                  crdtCard.last_name = Request["txtlastname"].ToString();
              }
              else
              {
                  crdtCard.last_name = null;
              }
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
                  billingAddress.state = clsCommonCClist.Getstatename(Request["hdnstate"]);
              }
              else
              {
                  billingAddress.state = null;
              }
              if (Request["hdncity"] != "")
              {
                  billingAddress.city = clsCommonCClist.Getcityname(Request["hdncity"]);
              }
              else
              {
                  billingAddress.city = null;
              }
              crdtCard.billing_address = billingAddress;
              Amount amnt = new Amount();
              amnt.currency = "USD";
              //if (Request["hdmamount"] != "")
              //{
              //    amnt.total = Request["hdmamount"].ToString();
              //}
              amnt.total = String.Format("{0:0.00}", (Convert.ToDecimal(Request["hdnneedtopay"])));
                  strchargetempleid = Request["hdnctempid1"];
                  strenddate = Request["hdnenddate1"];

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
                                                          objinsert.strStateID = Request["hdnstate"];
                                                          objinsert.strZipCode = Convert.ToString( Request["hdnzip"]);
                                                          objinsert.strCityID = Request["hdncity"];
                                                          objinsert.strBillAddress1 = billingAddress.line1;
                                                          objinsert.strBillAddress2 = billingAddress.line2;
                                                          objinsert.FirstName = Request["txtfirstname"];
                                                          objinsert.LastName = Request["txtlastname"];
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
                                                          clsCommonFunctions objcommon = new clsCommonFunctions();
                                                          int LoginId = objcommon.GetProviderLoginID(Convert.ToString( Request["HdnProvider_ID"]));
                                                          objinsert.CreatedBy = LoginId;
                                                          objinsert.ALLowCCCharges = "Y";
                                                          string Mycardno = Convert.ToString( Request["CardNumber"]);
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
                                                          //if (Request["ExhdnCardid"] != null)
                                                          //{
                                                          //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                                                          //}
                                                          CreditCard newCreditCard = new CreditCard();
                                                          //try
                                                          //{
                                                              newCreditCard = crdtCard.Create(apiContext);
                                                          //}
                                                          //catch (Exception)
                                                          //{
                                                              
                                                              
                                                          //}
                                                         
                                                          //if (newCreditCard.state == "ok")
                                                          //{
                                                          MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,!string.IsNullOrEmpty(Request["ExhdnCardid"])?Request["ExhdnCardid"]:null);
                                                          //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                                                          CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), newCreditCard.id, Out_CCID);
                                                         // }
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
                                                              CCProcess objcc = new CCProcess();
                                                              objcc.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                                              objcc.CreatedBy = Convert.ToInt32( Session["userid"]);
                                                              objcc.transactionAmount = Request["hdnmonth"];
                                                              CCProcess.upgrade_ServiceCharge(objcc);
                                                              CCProcess objcc1 = new CCProcess();
                                                              objcc1.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                                              objcc1.CCTransaction_ID = Loc_transid;
                                                              objcc1.CreatedBy = Convert.ToInt32( Session["userid"]);
                                                              objcc1.NextRenewDate = strenddate;

                                                              CCProcess.Insert_ServicePaymentInfo(objcc1);

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

                                                                  SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total, servicename);
                                                              }

                                                              return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                                                          }
                                                          else
                                                          {
                                                              return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on save"), JsonRequestBehavior.DenyGet);
                                                          }
                                                      }
                                                      else
                                                      {
                                                          return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.DenyGet);
                                                      }
                                                  }





                                                  else
                                                  {
                                                      double strip_amount = Convert.ToDouble(amnt.total) * 100;

                                                      //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                                                      var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                                                      var card = gateway.Post(new CreateStripeCard
                                                      {
                                                          CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                                                          Card = new StripeCard
                                                          {
                                                              Name = Request["txtfirstname"] + Request["txtlastname"],
                                                              Number = Request["CardNumber"],
                                                              Cvc = Request["txtCVV2"] != "" ? Request["txtCVV2"] : null,
                                                              ExpMonth = Convert.ToInt32(crdtCard.expire_month),
                                                              ExpYear = Convert.ToInt32(crdtCard.expire_year),
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
                                                          Customer =Convert.ToString(  Session["Stripe_customerid"]),
                                                          Currency = "usd",
                                                          Description = "Test Charge Client",
                                                          Card=card.Id
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
                                                          objinsert.strZipCode = Convert.ToString( Request["hdnzip"]);
                                                          objinsert.strCityID = Request["hdncity"];
                                                          objinsert.strBillAddress1 = billingAddress.line1;
                                                          objinsert.strBillAddress2 = billingAddress.line2;
                                                          objinsert.FirstName = Request["txtfirstname"];
                                                          objinsert.LastName = Request["txtlastname"];
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
                                                          clsCommonFunctions objcommon = new clsCommonFunctions();
                                                      int LoginId = objcommon.GetProviderLoginID(Convert.ToString( Request["HdnProvider_ID"]));
                                                      objinsert.CreatedBy = LoginId;
                                                          objinsert.ALLowCCCharges = "Y";
                                                          string Mycardno = Convert.ToString( Request["CardNumber"]);
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
                                                          //if (Request["ExhdnCardid"] != null)
                                                          //{
                                                          //    CCProcess.DeleteCreditCard(Request["ExhdnCardid"].ToString(), Session["userid"].ToString());
                                                          //}
                                                          MowerHelper.Models.BLL.Billing.CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID,!string.IsNullOrEmpty(Request["ExhdnCardid"])?Request["ExhdnCardid"]:null);
                                                          //objinsert.ccinfo_id = Convert.ToString(Out_CCID);
                                                          CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), card.Id, Out_CCID, Convert.ToString(Session["Stripe_customerid"]));
                                                              Loc_transid = MowerHelper.Models.BLL.Billing.CCProcess.PatientInsertCCTransactionDetails(objinsert, null);

                                                          CCProcess objccins = new CCProcess();
                                                          objccins.strTransactionID = Loc_transid;
                                                          objccins.StrRespStatusCode = "approved";
                                                          objccins.strRetval = null;
                                                          objccins.ResponseCode = null;
                                                          objccins.strUserID = LoginId;
                                                          objccins.GatewayDetail_ID = null;
                                                          objccins.PaypalProcessInd = "N";
                                                          Models.BLL.Billing.CCProcess.InsertCreditCardResponse(objccins);
                                                              CCProcess objcc = new CCProcess();
                                                              objcc.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                                              objcc.CreatedBy = Convert.ToInt32( Session["userid"]);
                                                              objcc.transactionAmount = Request["hdnmonth"];
                                                              CCProcess.upgrade_ServiceCharge(objcc);
                                                              CCProcess objcc1 = new CCProcess();
                                                              objcc1.ServiceSt = Session["Prov_ID"] + "," + Request["hdnbillserid"] + ",2," + strchargetempleid + "," + stramount + ",$";
                                                              objcc1.CCTransaction_ID = Loc_transid;
                                                              objcc1.CreatedBy = Convert.ToInt32( Session["userid"]);
                                                              objcc1.NextRenewDate = strenddate;

                                                              CCProcess.Insert_ServicePaymentInfo(objcc1);

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

                                                                  SendEmailtoElectrician(strtoMailid, Convert.ToString(Loc_transid), strprovidername, strdate, amnt.total, servicename);
                                                              }

                                                              return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                                                  }
              }
              //catch (PayPal.Exception.PayPalException ex)
              //{
              //    var objex = new clsExceptionLog();
              //    objex.LogException(ex, "ScheduleController", "Addnewpaymentmethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              //}
              catch (PayPal.PayPalException ex)
              {
                  if (ex.InnerException is PayPal.ConnectionException)
                  {
                      string txt = (((PayPal.ConnectionException)ex.InnerException).Response);
                  }
                  else
                  {
                      //context.Response.Write(ex.Message);
                  }
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
                      objinsert.strZipCode = Convert.ToString( Request["hdnzip"]);
                      objinsert.strCityID = Request["hdncity"];
                      objinsert.strBillAddress1 = billingAddress.line1;
                      objinsert.strBillAddress2 = billingAddress.line2;
                      objinsert.FirstName = Request["txtfirstname"];
                      objinsert.LastName = Request["txtlastname"];
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
                      clsCommonFunctions objcommon = new clsCommonFunctions();
                      int LoginId = objcommon.GetProviderLoginID(Convert.ToString( Request["HdnProvider_ID"]));
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
              catch(Exception ex)
              {
                  return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on save"), JsonRequestBehavior.DenyGet);
              }
              return Json(JsonResponseFactory.ErrorResponse("Some error occured.Please give payment information and click on save"), JsonRequestBehavior.DenyGet);
          }
          public void SendEmailtoElectrician(string strTomailid, string strTransaction_ID, string strProvidername, string strtransactiondate, string stramoumt,string servicename)
          {
             
              try
              {
              if ((clsWebConfigsettings.GetConfigSettingsValue("SendMailFlag").ToUpper()) == "YES")
              {
                  var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(61, 0);
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
                      str_Content = str_Content.Replace("$CCtransactionnumber$", strTransaction_ID);
                      if (strtransactiondate != null)
                      {
                          str_Content = str_Content.Replace("$transationdate$", strtransactiondate);
                      }
                      str_Content = str_Content.Replace("$ServiceContent$",servicename);
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
                      clsCommonFunctions objclscommon = new clsCommonFunctions();
                      IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", strTransaction_ID) };
                      objclscommon.AddInParameters(strMailParam);
                      objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                  }

              }
                }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "TechniciansController", "SendEmailtoElectrician", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
          }
          private void Filldata(string id, string valutid)
          {
              try
              {
                  ViewBag.CardID = id;
                  ViewBag.vaultid = valutid;

                  var obj1 = new Paymentmethods();
                  List<Paymentmethods> objcardlist = Paymentmethods.CreditCard_list_paymentinfo(obj1);
                  if (!string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"]) ))
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

                  var obj = new CCProcess { CardID = id, Provider_ID = Convert.ToString( Session["Prov_ID"]) };
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
                      ViewBag.FirstName = objlist[0].FirstName ?? null;
                      ViewBag.LastName = objlist[0].LastName ?? null;
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
          public void GetProviderDetails()
          {
              try
              {
              clsCommonFunctions objcomm = new clsCommonFunctions();
              Reg_ProvidersDetailInfo objInfo = new Reg_ProvidersDetailInfo();
              objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
              if ((objInfo != null))
              {
                  if (!string.IsNullOrEmpty(objInfo.FirstName))
                  {
                      ViewBag.FirstName = objInfo.FirstName;
                  }
                  else
                  {
                      ViewBag.FirstName = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.LastName))
                  {
                      ViewBag.LastName = objInfo.LastName;
                  }
                  else
                  {
                      ViewBag.LastName = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.Address1))
                  {
                      ViewBag.Address1 = objInfo.Address1;
                  }
                  else
                  {
                      ViewBag.Address1 = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.Address2))
                  {
                      ViewBag.Address2 = objInfo.Address2;
                  }
                  else
                  {
                      ViewBag.Address2 = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.Zip))
                  {
                      ViewBag.Zip = objInfo.Zip;
                  }
                  else
                  {
                      ViewBag.Zip = null;
                  }


                  if (!string.IsNullOrEmpty(Convert.ToString( objInfo.State_ID)))
                  {
                      ViewBag.State_ID = objInfo.State_ID;
                  }
                  else
                  {
                      ViewBag.State_ID = null;
                  }
                  if (!string.IsNullOrEmpty(Convert.ToString( objInfo.City_ID)))
                  {
                      ViewBag.City_ID = objInfo.City_ID;
                  }
                  else
                  {
                      ViewBag.City_ID = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.Statename))
                  {
                      ViewBag.Statename = objInfo.Statename;
                  }
                  else
                  {
                      ViewBag.Statename = null;
                  }
                  if (!string.IsNullOrEmpty(objInfo.Cityname))
                  {
                      ViewBag.Cityname = objInfo.Cityname;
                  }
                  else
                  {
                      ViewBag.Cityname = null;
                  }

              }
              }
              catch (Exception ex)
              {
                  var clsexp = new clsExceptionLog();
                  clsexp.LogException(ex, "TechniciansController", "GetProviderDetails", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }
          }

          //private string Encrypt(string clearText)
          //{
          //    try
          //    {
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
          //      }
          //    catch (Exception ex)
          //    {
          //        var clsexp = new clsExceptionLog();
          //        clsexp.LogException(ex, "TechniciansController", "Encrypt", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
          //    }
          //    return clearText;
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
          //        return Convert.ToString(dr_GetName["City"]) != null ? Convert.ToString(dr_GetName["City"]) : null;
          //    }
          //    return null;
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
          //        return Convert.ToString(dr_GetName["state"]) != null ? Convert.ToString(dr_GetName["state"]) : null;
          //    }
          //    return null;
          //}
    }
}
