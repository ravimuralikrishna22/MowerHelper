using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Obout.Mvc.ComboBox;
using PayPal.Api;
using PayPal.Tenniscoach;
using ServiceStack.Stripe;
using ServiceStack.Stripe.Types;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.ProviderRegistration;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;


namespace MowerHelper.Controllers
{
     public class AdminController : Controller
    {
       // string totalrecords = "0";
       // int intCounter;
        //private const int StartingYear = 2014;
    
     
             #region "Service management"
        public ActionResult FT_servicesTransaction()
        {
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
            var ftService = new RPBilling
            {
                //Practice_ID = Convert.ToInt32(Session["Practice_ID"]),
                Provider_id = Convert.ToInt32(Session["Prov_ID"]),
                FromDate = null,
                ToDate = null,
                ToReference_ID = null,
                ToReferenceType_ID = null
            };
            DataSet dsFtService = ftService.PracticeAdminSummary(ftService);
            var sbFtSummary = new StringBuilder();
            sbFtSummary = sbFtSummary.Append("<table width='100%' cellpadding='8' cellspacing='1' border='0' class='border_style' id='tblsummary' runat='server'><tr class='tr_bgcolor'>");
            sbFtSummary = sbFtSummary.Append("<td align='center'><strong>Total charges</strong></td><td align='center'><strong>Total payments</strong></td><td align='center'><strong>Total credits</strong></td><td align='center'><strong>Balance</strong></td></tr>");
            if (dsFtService.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSummary in dsFtService.Tables[0].Rows)
                {
                    string StrTCharge = string.Format("{0:c}", drSummary["TotalCharges"]);
                    string strTPay = string.Format("{0:c}", drSummary["TotalPayments"]);
                    string strBal = string.Format("{0:c}", drSummary["Balance"]);
                    string strCredit = string.Format("{0:c}", drSummary["TotalCredits"]);
                    sbFtSummary = sbFtSummary.Append("<tr><td align='center'>" + StrTCharge + " </td><td align='center'>" + strTPay + "</td><td align='center'>" + strCredit + "</td><td align='center'>" + strBal + "</td></tr>");
                }
            }
            sbFtSummary = sbFtSummary.Append("</table>");
            ViewBag.FtSummary = Convert.ToString(sbFtSummary);
            dynamic chgListPr = getPrChgList();
            return View("FT_servicesTransaction", chgListPr);
        }
       [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult FTservices_addnewpayment()
        {

            return View();
        }
       [HttpGet()]
       [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
       public ActionResult FTservicesAddnewpayment()
       {
           if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
           {
               var obj = new Paymentmethods();
               List<Paymentmethods> objcardlist = Paymentmethods.CreditCard_list_paymentinfo(obj);
               if (objcardlist.Count != 0)
               {
                   ViewBag.totrec = objcardlist.Count;
               }

               return PartialView("_FTservicesAddnewpayment", objcardlist);
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
        [AllowAnonymous]
        public ActionResult Addnewpaymentmethod()
       {
           if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
           {
               ViewData["month"] = clsCommonCClist.GetCCMonth();
               ViewData["year"] = clsCommonCClist.GetCCYear();
               ViewData["CardType"] = clsCommonCClist.GetCCList();
               //ViewBag.Practice_ID = Convert.ToString(Session["Practice_ID"]);
               //ViewBag.Provider_ID = Convert.ToString(Session["Prov_ID"]);
               var ds = Referrals.List_field_description("26");
               ViewBag.IssuingNotes = Convert.ToString(ds.Tables[0].Rows[0][3]);                  
               Reg_ProvidersDetailInfo objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
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
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [AllowAnonymous]
        public ActionResult EditPaymentMethod(string id)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                ViewData["Emonth"] = clsCommonCClist.GetCCMonth();
                ViewData["Eyear"] = clsCommonCClist.GetCCYear();
                ViewData["ECardType"] = clsCommonCClist.GetCCList();
                ViewBag.Practice_ID = Convert.ToString(Session["Practice_ID"]);
                ViewBag.Provider_ID = Convert.ToString(Session["Prov_ID"]);
                //string vaultid = CCProcess.GetVaultID(id);
                ViewBag.vaultid = CCProcess.GetVaultID(id);
                ViewBag.customerid = Request["customerid"];
                // Filldata(id, vaultid);
                var ds = Referrals.List_field_description("26");
                ViewBag.IssuingNotesEdit = Convert.ToString(ds.Tables[0].Rows[0][3]); 
                FillProviderAddress();
                return View("EditPaymentMethod", Filldata(id, ViewBag.vaultid));
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
        private List<CCProcess> Filldata(string id, string valutid)
        {
            try
            {
                ViewBag.CardID = id;
                               //clsWebConfigsettings clsweb = new clsWebConfigsettings();
                               //if (clsweb.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                                   if (string.IsNullOrEmpty(Request["customerid"]))// == "" || Request["customerid"] == null)
                               {
                                   APIContext apiContext = Configuration.GetAPIContext();
                                   //try
                                   //{
                                       if (!string.IsNullOrEmpty(valutid))
                                       {
                                           CreditCard getcardinfo = CreditCard.Get(apiContext, valutid);
                                           if (getcardinfo != null)
                                           {
                                               ViewBag.cardnumber = getcardinfo.number;
                                               ViewBag.cvv2 = getcardinfo.cvv2;
                                               ViewBag.FirstName = getcardinfo.first_name;
                                               ViewBag.LastName = getcardinfo.last_name;
                                           }
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
                                   ViewBag.FirstName = Request["Firstname"];
                                   ViewBag.LastName = Request["lastname"];
                               }

                var obj = new CCProcess {CardID = id, Provider_ID = Convert.ToString(Session["Prov_ID"])};
                List<CCProcess> objlist = CCProcess.CreditCard_Get_paymentinfo(obj);
                if (objlist.Count > 0)
                {
                    ViewBag.CardType = objlist[0].strx_card_code ?? null;
                    if (ViewBag.CardType == "VisaCard")
                    {
                        ViewBag.CardType = "Visa";
                    }
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
                return objlist;
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
                return null;
            }
        }
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaymentMethod(Reg_CreditCardProcess obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                int? Out_CCID = 0;
                string firstName = null, lastName = null;
                var id = Request["vaultid"];
                
                var crdtCard = new CreditCard();

                if (!string.IsNullOrEmpty(obj.EFullName))
                {
                    if (obj.EFullName.Contains(' '))
                    {
                        var names = obj.EFullName.Split(' ');
                        if (names.Length >= 2)
                        {
                            firstName = names[0] ?? null;
                            lastName = names[1] ?? null;
                        }
                    }
                    else
                    {
                        firstName = obj.EFullName;
                    }
                }

                if (!string.IsNullOrEmpty(Request["ECardType"]))// != "")
                {
                    switch (Request["ECardType"])
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
                try
                {
                    if (clsWebConfigsettings.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                    {
                        APIContext apiContext = Configuration.GetAPIContext();
                        if (!string.IsNullOrEmpty(Request["ECardNumber"]))
                        {
                            crdtCard.number = Request["ECardNumber"];
                        }
                        crdtCard.cvv2 = !string.IsNullOrEmpty(Request["EtxtCVV2"]) ? Request["EtxtCVV2"] : null;
                        crdtCard.first_name = !string.IsNullOrEmpty(firstName) ? firstName : null;
                        crdtCard.last_name = !string.IsNullOrEmpty(lastName) ? lastName : null;
                        //crdtCard.first_name = !string.IsNullOrEmpty(Request["Etxtfirstname"]) ? Request["Etxtfirstname"] : null;
                        //crdtCard.last_name = !string.IsNullOrEmpty(Request["Etxtlastname"]) ? Request["Etxtlastname"] : null;
                        if (!string.IsNullOrEmpty(Request["Emonth"]))
                        {
                            crdtCard.expire_month = Convert.ToInt32(Request["Emonth"]);
                        }

                        if (!string.IsNullOrEmpty(Request["Eyear"]))
                        {
                            crdtCard.expire_year = Convert.ToInt32(Request["Eyear"]);
                        }
                        if (!string.IsNullOrEmpty(Request["EHdnProvider_ID"]))
                        {
                            crdtCard.payer_id = Request["EHdnProvider_ID"];
                        }

                        var billingAddress = new Address { country_code = "US" };
                        billingAddress.line1 = Request["Ehdnadd1"] != "" ? Request["Ehdnadd1"] : null;
                        billingAddress.line2 = Request["Ehdnadd2"] != "" ? Request["Ehdnadd2"] : null;
                        billingAddress.postal_code = Request["Ehdnzip"] != "" ? Request["Ehdnzip"].ToString() : null;
                        billingAddress.state = Request["Ehdnstate"] != "" ? clsCommonCClist.Getstatename(Request["Ehdnstate"]) : null;
                        billingAddress.city = Request["Ehdncity"] != "" ? clsCommonCClist.Getcityname(Request["Ehdncity"]) : null;
                        crdtCard.billing_address = billingAddress;
                        CreditCard newCreditCard = new CreditCard();
                        newCreditCard = crdtCard.Create(apiContext);
                        var objinsert = new CCProcess
                        {
                            StrPaidRefID = "1",
                            StrPaidRefTypeID = "1",
                            StrChargebleRefID = Request["EHdnProvider_ID"],
                            StrChargebleRefTypeID = "2",
                            strpost = null,
                            RefLoginID = 0,
                            strPracticeID = Request["EHdnPractice_ID"],
                            CVV2 = null,
                            strx_card_code = crdtCard.type,
                            strStrCardType = null,
                            strStateID = Request["Ehdnstate"],
                            strZipCode = Request["Ehdnzip"],
                            strCityID = Request["Ehdncity"],
                            strBillAddress1 = billingAddress.line1,
                            strBillAddress2 = billingAddress.line2,
                            FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                            LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                            //FirstName = Request["Etxtfirstname"],
                            //LastName = Request["Etxtlastname"],
                            strx_invoice_num = null,
                            strx_card_num = null,
                            IsPaypalInd = "Y",
                            StrExpMon = crdtCard.expire_month,
                            StrExpYear = crdtCard.expire_year,
                            CreatedBy = Convert.ToInt32(Session["userid"]),
                            ALLowCCCharges = "Y"
                        };
                        objinsert.LastFourdigitCCNo = Request["ECardNumber"].Substring(Request["ECardNumber"].Length - Math.Min(4, Request["ECardNumber"].Length));
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["EIssuingBank"]) ? Request["EIssuingBank"] : null;

                        if (!string.IsNullOrEmpty(Request["Echkaddress"]))
                        {
                            objinsert.practice_ind = Request["Echkaddress"] == "true,false" ? "Y" : "N";
                        }
                        //if (newCreditCard.state != null)
                        //{
                        //    if (newCreditCard.state.ToLower() == "ok")
                        //    {
                                if (!string.IsNullOrEmpty(Request["EhdnCardid"]))// != null)
                                {
                                    CCProcess.DeleteCreditCard(Convert.ToString(Request["EhdnCardid"]), Convert.ToString(Session["userid"]));
                                }
                                if (!string.IsNullOrEmpty(id))
                                {
                                    var card = CreditCard.Get(apiContext, id);
                                    card.Delete(apiContext);
                                }
                                if (Session["Prov_ID"] != null)
                                {
                                    CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, null);
                                    CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), newCreditCard.id, Out_CCID);
                                }
                                return Json(JsonResponseFactory.SuccessResponse(true), JsonRequestBehavior.AllowGet);
                            //}
                            //else
                            //{
                            //    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
                            //}
                        //}
                        //else
                        //{
                        //    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
                        //}
                       
                    }
                    else
                    {
                        string StripeID,CustomerID = null;
                        if (!string.IsNullOrEmpty(Request["customerid"]))
                        {

                            //clsWebConfigsettings cls_obj = new clsWebConfigsettings();
                            var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                            var deletedRef = gateway.Delete(new DeleteStripeCard
                            {
                                CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                                CardId = Request["Vaultid"],
                            });
                            var card = gateway.Post(new CreateStripeCard
                            {
                                CustomerId = Convert.ToString(Session["Stripe_customerid"]),
                                Card = new StripeCard
                                {
                                    //Name = Request["Etxtfirstname"] + Request["Etxtlastname"],
                                    Name = Request["EFullName"],
                                    Number = Request["ECardNumber"],
                                    Cvc = Request["EtxtCVV2"] != "" ? Request["txtCVV2"] : null,
                                    ExpMonth = Convert.ToInt32(Request["Emonth"]),
                                    ExpYear = Convert.ToInt32(Request["Eyear"]),
                                    AddressLine1 = Request["Ehdnadd1"] != "" ? Request["Ehdnadd1"] : null,
                                    AddressLine2 = Request["Ehdnadd2"] != "" ? Request["Ehdnadd2"] : null,
                                    AddressZip = Request["Ehdnzip"] != "" ? Request["Ehdnzip"] : null,
                                    AddressState = Request["Ehdnstate"] != "" ? clsCommonCClist.Getstatename(Request["Ehdnstate"]) : null,
                                    AddressCity = Request["Ehdncity"] != "" ? clsCommonCClist.Getcityname(Request["Ehdncity"]) : null,
                                    AddressCountry = "US",
                                },
                            });
                            StripeID = card.Id;
                            CustomerID = Convert.ToString(Session["Stripe_customerid"]);
                        }
                        else
                        {
                            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["Prov_ID"]));
                            string customer_mail = null;
                            if ((obgCredn != null))
                            {
                                customer_mail = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                            }
                           var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                            var customer = gateway.Post(new CreateStripeCustomer
                            {
                                Description = "Description",
                                Email = customer_mail,
                            });
                            var card = gateway.Post(new CreateStripeCard
                            {
                                CustomerId = customer.Id,
                                Card = new StripeCard
                                {
                                    //Name = Request["Etxtfirstname"] + Request["Etxtlastname"],
                                    Name = Request["EFullName"],
                                    Number = Request["ECardNumber"],
                                    Cvc = Request["EtxtCVV2"] != "" ? Request["txtCVV2"] : null,
                                    ExpMonth = Convert.ToInt32(Request["Emonth"]),
                                    ExpYear = Convert.ToInt32(Request["Eyear"]),
                                    AddressLine1 = Request["Ehdnadd1"] != "" ? Request["Ehdnadd1"] : null,
                                    AddressLine2 = Request["Ehdnadd2"] != "" ? Request["Ehdnadd2"] : null,
                                    AddressZip = Request["Ehdnzip"] != "" ? Request["Ehdnzip"] : null,
                                    AddressState = Request["Ehdnstate"] != "" ? clsCommonCClist.Getstatename(Request["Ehdnstate"]) : null,
                                    AddressCity = Request["Ehdncity"] != "" ? clsCommonCClist.Getcityname(Request["Ehdncity"]) : null,
                                    AddressCountry = "US",
                                },
                            });
                            StripeID = card.Id;
                            CustomerID =customer.Id;
                        }
                        var objinsert = new CCProcess
                        {
                            StrPaidRefID = "1",
                            StrPaidRefTypeID = "1",
                            StrChargebleRefID = Request["EHdnProvider_ID"],
                            StrChargebleRefTypeID = "2",
                            strpost = null,
                            RefLoginID = 0,
                            strPracticeID = Request["EHdnPractice_ID"],
                            CVV2 = null,
                            strx_card_code = crdtCard.type,
                            strStrCardType = null,
                            strStateID = Request["Ehdnstate"],
                            strZipCode = Request["Ehdnzip"],
                            strCityID = Request["Ehdncity"],
                            strBillAddress1 = Request["Ehdnadd1"] != "" ? Request["Ehdnadd1"] : null,
                            strBillAddress2 = Request["Ehdnadd2"] != "" ? Request["Ehdnadd2"] : null,
                            FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                            LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                            //FirstName = Request["Etxtfirstname"],
                            //LastName = Request["Etxtlastname"],
                            strx_invoice_num = null,
                            strx_card_num = null,
                            IsPaypalInd = "N",
                            StrExpMon = Convert.ToInt32(Request["Emonth"]),
                            StrExpYear = Convert.ToInt32(Request["Eyear"]),
                            CreatedBy = Convert.ToInt32(Session["userid"]),
                            ALLowCCCharges = "Y"
                        };
                        objinsert.LastFourdigitCCNo = Request["ECardNumber"].Substring(Request["ECardNumber"].Length - Math.Min(4, Request["ECardNumber"].Length));
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["EIssuingBank"]) ? Request["EIssuingBank"] : null;
                        if (!string.IsNullOrEmpty(Request["Echkaddress"]))// != null)
                        {
                            objinsert.practice_ind = Request["Echkaddress"] == "true,false" ? "Y" : "N";
                        }
                        if (Session["Prov_ID"] != null)
                        {
                            CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, null);
                            CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]), StripeID, Out_CCID, CustomerID);
                        }
                        return Json(JsonResponseFactory.SuccessResponse(true), JsonRequestBehavior.AllowGet);

                    }
                }
                catch (PayPal.PayPalException ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminController", "EditPaymentMethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                catch (StripeException ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminController", "EditPaymentMethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    return Json(JsonResponseFactory.ErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                   // ViewBag.vaultid = null;
                    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
                }
               // ViewBag.vaultid = null;
                return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
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
        [ValidateAntiForgeryToken]
        public ActionResult Addnewpaymentmethod(Reg_CreditCardProcess obj)
        {
            if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                string firstName = null, lastName = null;    
                int? Out_CCID = 0;
                var crdtCard = new CreditCard();
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

                if (!string.IsNullOrEmpty(Request["CardType"]))// != "")
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
                
                if (!string.IsNullOrEmpty(Request["CardNumber"]))
                {
                    crdtCard.number = Request["CardNumber"];
                }
                crdtCard.cvv2 = !string.IsNullOrEmpty(Request["txtCVV2"]) ? Request["txtCVV2"] : null;
                //crdtCard.first_name = !string.IsNullOrEmpty(Request["txtfirstname"]) ? Request["txtfirstname"] : null;
                //crdtCard.last_name = !string.IsNullOrEmpty(Request["txtlastname"]) ? Request["txtlastname"] : null;   
                crdtCard.first_name = !string.IsNullOrEmpty(firstName) ? firstName : null;
                crdtCard.last_name = !string.IsNullOrEmpty(lastName) ? lastName : null;
                if (!string.IsNullOrEmpty(Request["month"]))
                {
                    crdtCard.expire_month = Convert.ToInt32(Request["month"]);
                }

                if (!string.IsNullOrEmpty(Request["year"]))
                {
                    crdtCard.expire_year = Convert.ToInt32(Request["year"]);
                }
                if (!string.IsNullOrEmpty(Request["HdnProvider_ID"]))
                {
                    crdtCard.payer_id = Request["HdnProvider_ID"];
                }

                var billingAddress = new Address
                {
                    country_code = "US",
                    line1 = Request["hdnadd1"] != "" ? Request["hdnadd1"] : null,
                    line2 = Request["hdnadd2"] != "" ? Request["hdnadd2"] : null,
                    postal_code = Request["hdnzip"] != "" ? Request["hdnzip"] : null,
                    state = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                    city = Request["hdncity"] != "" ? clsCommonCClist.Getcityname(Request["hdncity"]) : null
                };
                crdtCard.billing_address = billingAddress;
                try
                {
                    if (clsWebConfigsettings.GetConfigSettingsValue("UsePaypal").ToUpper() == "YES")
                    {
                        APIContext apiContext = Configuration.GetAPIContext();
                        CreditCard newCreditCard = new CreditCard();
                        newCreditCard = crdtCard.Create(apiContext);
                        var objinsert = new CCProcess
                        {
                            StrPaidRefID = "1",
                            StrPaidRefTypeID = "1",
                            StrChargebleRefID = Request["HdnProvider_ID"],
                            StrChargebleRefTypeID = "2",
                            strpost = null,
                            RefLoginID = 0,
                            strPracticeID = Request["HdnPractice_ID"],
                            CVV2 = null,
                            strx_card_code = crdtCard.type,
                            strStrCardType = null,
                            strStateID = Request["hdnstate"],
                            strZipCode = Request["hdnzip"],
                            strCityID = Request["hdncity"],
                            strBillAddress1 = billingAddress.line1,
                            strBillAddress2 = billingAddress.line2,
                            //FirstName = Request["txtfirstname"],
                            //LastName = Request["txtlastname"],
                            FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                            LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                            strx_invoice_num = null,
                            strx_card_num = null,
                            IsPaypalInd = "Y",
                            StrExpMon = crdtCard.expire_month,
                            StrExpYear = crdtCard.expire_year,
                            CreatedBy = Convert.ToInt32(Session["userid"]),
                            ALLowCCCharges = "Y"
                        };
                        objinsert.LastFourdigitCCNo = Request["CardNumber"].Substring(Request["CardNumber"].Length - Math.Min(4, Request["CardNumber"].Length));
                  
                    objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                
                        if (!string.IsNullOrEmpty(Request["chkaddress"]))// != null)
                        {
                            objinsert.practice_ind = Request["chkaddress"] == "true,false" ? "Y" : "N";
                        }

                        if (Session["Prov_ID"] != null)
                        {
                            CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, null);
                            //objinsert.ccinfo_id = Convert.ToString(Out_CCID);

                            CCProcess.UpdateCreditCardVaultID(Session["Prov_ID"].ToString(), newCreditCard.id, Out_CCID);
                        }
                        return Json(JsonResponseFactory.SuccessResponse(true), JsonRequestBehavior.AllowGet);
                        // }
                    }
                    else
                    {
                        string StripeID, CustomerID = null;
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Stripe_customerid"])))
                        {
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
                                    AddressLine1 = Request["hdnadd1"] != "" ? Request["hdnadd1"] : null,
                                    AddressLine2 = Request["hdnadd2"] != "" ? Request["hdnadd2"] : null,
                                    AddressZip = Request["hdnzip"] != "" ? Request["hdnzip"] : null,
                                    AddressState = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                                    AddressCountry = "US",
                                },
                            });
                            StripeID = card.Id;
                            CustomerID = Convert.ToString(Session["Stripe_customerid"]);
                        }
                        else
                        {
                            var obgCredn = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["Prov_ID"]));
                            string customer_mail = null;
                            if ((obgCredn != null))
                            {
                                customer_mail = (obgCredn.UserName) != null ? obgCredn.UserName : null;
                            }
                            var gateway = new StripeGateway(clsWebConfigsettings.GetConfigSettingsValue("StripeTestSecretkey"));
                            var customer = gateway.Post(new CreateStripeCustomer
                            {                                
                                Description = "Description",
                                Email = customer_mail,
                            });
                            var card = gateway.Post(new CreateStripeCard
                            {
                                CustomerId = customer.Id,
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
                                    AddressZip = Request["hdnzip"] != "" ? Request["hdnzip"] : null,
                                    AddressState = Request["hdnstate"] != "" ? clsCommonCClist.Getstatename(Request["hdnstate"]) : null,
                                    AddressCountry = "US",
                                },
                            });
                            StripeID = card.Id;
                            CustomerID = customer.Id;
                        }
                        var objinsert = new CCProcess
                        {
                            StrPaidRefID = "1",
                            StrPaidRefTypeID = "1",
                            StrChargebleRefID = Request["HdnProvider_ID"],
                            StrChargebleRefTypeID = "2",
                            strpost = null,
                            RefLoginID = 0,
                            strPracticeID = Request["HdnPractice_ID"],
                            CVV2 = null,
                            strx_card_code = crdtCard.type,
                            strStrCardType = null,
                            strStateID = Request["hdnstate"],
                            strZipCode = Request["hdnzip"],
                            strCityID = Request["hdncity"],
                            strBillAddress1 = billingAddress.line1,
                            strBillAddress2 = billingAddress.line2,
                            //FirstName = Request["txtfirstname"],
                            //LastName = Request["txtlastname"],
                            FirstName = !string.IsNullOrEmpty(firstName) ? firstName : null,
                            LastName = !string.IsNullOrEmpty(lastName) ? lastName : null,
                            strx_invoice_num = null,
                            strx_card_num = null,
                            IsPaypalInd = "N",
                            StrExpMon = crdtCard.expire_month,
                            StrExpYear = crdtCard.expire_year,
                            CreatedBy = Convert.ToInt32(Session["userid"]),
                            ALLowCCCharges = "Y"
                        };
                        objinsert.LastFourdigitCCNo = Request["CardNumber"].Substring(Request["CardNumber"].Length - Math.Min(4, Request["CardNumber"].Length));
                        objinsert.IssuingBank = !string.IsNullOrEmpty(Request["IssuingBank"]) ? Request["IssuingBank"] : null;
                        if (!string.IsNullOrEmpty(Request["chkaddress"]))// != null)
                        {
                            objinsert.practice_ind = Request["chkaddress"] == "true,false" ? "Y" : "N";
                        }
                        if (Session["Prov_ID"] != null)
                        {
                            CCProcess.Insert_NewRegProviderCreditcard(objinsert, ref Out_CCID, null);
                            CCProcess.UpdateCreditCardVaultID(Convert.ToString(Session["Prov_ID"]),StripeID, Out_CCID,CustomerID);
                        }
                        return Json(JsonResponseFactory.SuccessResponse(true), JsonRequestBehavior.AllowGet);

                    }

                }
                catch (PayPal.PayPalException ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminController", "Addnewpaymentmethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                }
                catch (StripeException ex)
                {
                    var clsCustomEx = new clsExceptionLog();
                    clsCustomEx.LogException(ex, "AdminController", "Addnewpaymentmethod", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                    return Json(JsonResponseFactory.ErrorResponse(ex.Message), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
                }
                return Json(JsonResponseFactory.ErrorResponse("Please review your form"), JsonRequestBehavior.AllowGet);
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
        public ActionResult DeletePaymentMethod(string id, string Vaultid)
         {
             if (Request.IsAjaxRequest() & !string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
             {                
                 APIContext apiContext = Configuration.GetAPIContext();
                 var card = CreditCard.Get(apiContext, Vaultid);
                 card.Delete(apiContext);

                 CCProcess.DeleteCreditCard(id, Convert.ToString(Session["userid"]));
                 return RedirectToAction("FTservicesAddnewpayment");
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
        //public string Getstatename(string stateid)
        //{
        //    var clscommon = new clsCommonFunctions();
        //    IDataParameter[] inParmList = { new SqlParameter("@in_state_id", stateid) };
        //    clscommon.AddInParameters(inParmList);
        //    SqlDataReader drGetName = clscommon.GetDataReader("Help_dbo.St_get_stateabbrevation");
        //    while (drGetName.Read())
        //    {
        //        return Convert.ToString(drGetName["state"]);
        //    }
        //    return null;
        //}
        //public string Getcityname(string cityid)
        //{
        //    var clscommon = new clsCommonFunctions();
        //    IDataParameter[] inParmList = { new SqlParameter("@in_City_ID", cityid) };
        //    clscommon.AddInParameters(inParmList);
        //    SqlDataReader drGetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
        //    while (drGetName.Read())
        //    {
        //        return Convert.ToString(drGetName["City"]);
        //    }
        //    return null;
        //}
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
                ViewBag.PState_ID = !string.IsNullOrEmpty(Convert.ToString(objInfo.State_ID)) ? Convert.ToString(objInfo.State_ID) : null;
                ViewBag.PCity_ID = !string.IsNullOrEmpty(Convert.ToString(objInfo.City_ID)) ? Convert.ToString(objInfo.City_ID) : null;
                ViewBag.PStatename = !string.IsNullOrEmpty(objInfo.Statename) ? objInfo.Statename : null;
                ViewBag.PCityname = !string.IsNullOrEmpty(objInfo.Cityname) ? objInfo.Cityname : null;
            }
              }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "FillProviderAddress", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        //public Reg_ProvidersDetailInfo GetProviderDetails()
        //{
            //Reg_ProvidersDetailInfo objInfo = Reg_ProvidersDetailInfo.Get_ProviderAddressDetails(Convert.ToInt32(Session["Prov_ID"]));
            //if ((objInfo != null))
            //{
                //ViewBag.FirstName = !string.IsNullOrEmpty(objInfo.FirstName) ? objInfo.FirstName : null;
                //ViewBag.LastName = !string.IsNullOrEmpty(objInfo.LastName) ? objInfo.LastName : null;
                //ViewBag.PAddress1 = !string.IsNullOrEmpty(objInfo.Address1) ? objInfo.Address1 : null;
                //ViewBag.PAddress2 = !string.IsNullOrEmpty(objInfo.Address2) ? objInfo.Address2 : null;
                //ViewBag.PZip = !string.IsNullOrEmpty(objInfo.Zip) ? objInfo.Zip : null;


                //if (!string.IsNullOrEmpty(Convert.ToString(objInfo.State_ID)))
                //{
                //    ViewBag.State_ID = objInfo.State_ID;
                //}
                //else
                //{
                //    ViewBag.State_ID = null;
                //}
                //if (!string.IsNullOrEmpty(Convert.ToString(objInfo.City_ID)))
                //{
                //    ViewBag.City_ID = objInfo.City_ID;
                //}
                //else
                //{
                //    ViewBag.City_ID = null;
                //}
                //ViewBag.Statename = !string.IsNullOrEmpty(objInfo.Statename) ? objInfo.Statename : null;
                //ViewBag.Cityname = !string.IsNullOrEmpty(objInfo.Cityname) ? objInfo.Cityname : null;

            //}
        //    return objInfo;
        //}
        public ActionResult Prpmtlist()
        {
            dynamic paygrid = getPrPmtList();
            return View(paygrid);
        }
        public ActionResult Pradjlist()
        {
            dynamic Adjgrid = getPrAdjList();
            return View(Adjgrid);
        }
        public List<RPBilling> getPrChgList()
        {
            List<RPBilling> chgInfo = null;
            try
            {
            var objChgInfo = new RPBilling
            {
                TransactionTypeIDs = "1",
                ReferenceType_ID = "1",
                fromReferenceId = 1,
                //Practice_ID = Convert.ToInt32(Session["Prov_ID"]),
                Provider_id = Convert.ToInt32(Session["Prov_ID"]),
                ToReference_ID = null,
                ToReferenceType_ID = null,
                FromDate = null,
                ToDate = null,
                //ServiceID = "1"
            };
            objChgInfo.OrderbyItem = Request["sort"] == null ? "Transaction_Date" : Request["sort"];
            objChgInfo.orderBy = Request["sortdir"] == null ? "DESC" : Request["sortdir"];

            objChgInfo.NoOfRecords = "10";
            objChgInfo.PageNo = Request["page"] != null ? Request["page"] : "1";
            chgInfo = objChgInfo.Get_PracticeUserTrasnactions(objChgInfo);
            if (chgInfo.Count > 0)
            {
                ViewBag.totrec = RPBilling.Totalnoofrecords;
              
            }
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getPrChgList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return chgInfo;
        }
        public List<RPBilling> getPrPmtList()
        {
            List<RPBilling> pmtInfo = null;
            try{
           
            var objPmtInfo = new RPBilling
            {
                TransactionTypeIDs = "2",
                ReferenceType_ID = "1",
                fromReferenceId = 1,
                //Practice_ID = Convert.ToInt32(Session["Prov_ID"]),
                Provider_id = Convert.ToInt32(Session["Prov_ID"]),
                ToReference_ID = null,
                ToReferenceType_ID = null,
                FromDate = null,
                ToDate = null,
                //ServiceID = "1"
            };
            objPmtInfo.OrderbyItem = Request["g2sort"] == null ? "Transaction_Date" : Request["g2sort"];
            objPmtInfo.orderBy = Request["g2sortdir"] == null ? "DESC" : Request["g2sortdir"];

            objPmtInfo.NoOfRecords = "10";
            objPmtInfo.PageNo = Request["page"] != null ? Request["page"] : "1";
             pmtInfo = objPmtInfo.Get_PracticeUserTrasnactions(objPmtInfo);
            if (pmtInfo.Count > 0)
            {
                ViewBag.Paytotrec = RPBilling.Totalnoofrecords;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getPrPmtList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return pmtInfo;
        }
        public List<RPBilling> getPrAdjList()
        {
            List<RPBilling> adjInfo = null;
            try
            {
            var objAdjInfo = new RPBilling
            {
                TransactionTypeIDs = "3,4",
                ReferenceType_ID = "1",
                fromReferenceId = 1,
                //Practice_ID = Convert.ToInt32(Session["Prov_ID"]),
                Provider_id = Convert.ToInt32(Session["Prov_ID"]),
                ToReference_ID = null,
                ToReferenceType_ID = null,
                FromDate = null,
                ToDate = null,
                //ServiceID = "1",
                OrderbyItem = Request["g3sort"] ?? "Transaction_Date",
                orderBy = Request["g3sortdir"] ?? "DESC",
                NoOfRecords = "10",
                PageNo = Request["g3p3"] != null ? Request["g3p3"] : "1"
            };

            adjInfo = objAdjInfo.Get_PracticeUserTrasnactions(objAdjInfo);
            if (adjInfo.Count > 0)
            {
                ViewBag.Adjtotrec = RPBilling.Totalnoofrecords;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getPrAdjList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return adjInfo;
        }
        public ActionResult PracticeServicesInvoice(string ddlInvoiceYear, string monthName, string monthFull)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
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
            if (Convert.ToString(Session["roleid"]) == "1")
            {
                if (Convert.ToString(TempData["Practiceid"]) != "" && TempData["Practiceid"] != null)
                {
                    ViewBag.proid = TempData["Practiceid"];
                }
                if (Convert.ToString(TempData["pracname"]) != "" && TempData["pracname"] != null)
                {
                    ViewBag.proname = TempData["pracname"];
                }
                TempData.Keep("pracname");
                TempData.Keep("Practiceid");
            }
            
            IList<SelectListItem> ddlYear =new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year + 10; i++)
            {
                ddlYear.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            ViewBag.ddlInvoiceYear = ddlYear;
            dynamic invoiceInfoList = getprInvoiceList(ddlInvoiceYear, monthName, monthFull);
            return View(invoiceInfoList);
        }
        public ActionResult ReceiptItemList(string ddlRecieptYear, string monthName, string monthFull)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            //if (TempData["Practiceid"] != null)
            //{
            //    if (Convert.ToString(TempData["Practiceid"]) != proid || Convert.ToString(TempData["pracname"]) != proname)
            //    {
            //        return RedirectToAction("PageNotFound", "Error");
            //    }
            //}
            //TempData.Keep("Practiceid");
            //TempData.Keep("pracname");
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
            if (Convert.ToString(Session["roleid"]) == "1")
            {
                if (Convert.ToString(TempData["Practiceid"]) != "" && TempData["Practiceid"] != null)
                {
                    ViewBag.proid = TempData["Practiceid"];
                }
                if (Convert.ToString(TempData["pracname"]) != "" && TempData["pracname"] != null)
                {
                    ViewBag.proname = TempData["pracname"];
                }
                TempData.Keep("pracname");
                TempData.Keep("Practiceid");
            }
            IList<SelectListItem> ddlYear = new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year + 10; i++)
            {
                ddlYear.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            ViewBag.ddlRecieptYear = ddlYear;
            dynamic recieptInfoList = getprRecieptList(ddlRecieptYear, monthName, monthFull, Convert.ToString(TempData["Practiceid"]));
            return View(recieptInfoList);
        }
        public List<RPBilling> getprRecieptList(string ddlInvoiceYear, string monthName, string monthFull,string proid)
        {
            List<RPBilling> recieptListInfo = null;
            try
            {
            string pracid = string.Empty;
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                pracid = Convert.ToString(Session["Prov_ID"]);
            }
            else
            {
                //pracid = null;
                pracid = proid;
            }
            var objRecieptInfo = new RPBilling
            {
                fromReferenceId = 1,
                ReferenceType_ID = "1",
                ToReference_ID =pracid,
                ToReferenceType_ID = "2"
            };
            if (ddlInvoiceYear != null && monthFull != null && monthName != null)
            {
                ViewBag.monthName = "ThMonth" + monthName;
                ViewBag.monthFull = monthFull;
                ViewBag.ddlyear = ddlInvoiceYear;
                objRecieptInfo.Invoice_Date = monthName + "/" + "01/" + ddlInvoiceYear;
            }
            else if (ddlInvoiceYear != null && monthName == null && monthFull == null)
            {
                ViewBag.monthName = "ThMonth" + monthName;
                ViewBag.monthFull = monthFull;
                ViewBag.ddlyear = ddlInvoiceYear;
                string monthse = DateTime.Now.Month.ToString();
                string monName = DateTime.Now.ToString("MMMM");
                ViewBag.monthName = "ThMonth" + monthse;
                ViewBag.monthFull = monName;
                objRecieptInfo.Invoice_Date = monthse + "/" + "01/" + ddlInvoiceYear;
            }
            else
            {
                objRecieptInfo.Invoice_Date = DateTime.Now.ToShortDateString();
                ViewBag.monthName = "ThMonth" + DateTime.Now.Month.ToString();
                ViewBag.monthFull = DateTime.Now.ToString("MMMM");
                ViewBag.ddlyear = DateTime.Now.Year;
            }

            objRecieptInfo.FromDate = null;
            objRecieptInfo.ToDate = null;
            objRecieptInfo.NoOfRecords = "5";
            //if (Session["roleid"].ToString() != "1")
            //{
            //    objRecieptInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //}
            //else
            //{
            //    objRecieptInfo.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            //}
            TempData.Keep("Practiceid");
            objRecieptInfo.OrderbyItem = Request["sort"] == null ? "Invoice_Date" : Request["sort"];
            objRecieptInfo.orderBy = Request["sortdir"] == null ? "DESC" : Request["sortdir"];
            objRecieptInfo.PageNo = Request["page"] != null ? Request["page"] : "1";
            recieptListInfo = objRecieptInfo.Get_AdmintopracticeInvoiceReciept(objRecieptInfo);
            if (recieptListInfo.Count > 0)
            {
                ViewBag.Reciepttotrec = RPBilling.Totalnoofrecords;

            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getprRecieptList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return recieptListInfo;
        }
        public List<RPBilling> getprInvoiceList(string ddlInvoiceYear, string monthName, string monthFull)
        {
            
            List<RPBilling> invInfo  = null;
            try
            {
            var objInvoiceInfo = new RPBilling();
            
               objInvoiceInfo.fromReferenceId = 1;
               objInvoiceInfo.ReferenceType_ID = "1";
               if (Convert.ToString(Session["roleid"]) != "1")
               {
                   objInvoiceInfo.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
               }
               else
               {
                   objInvoiceInfo.ToReference_ID = Convert.ToString(TempData["Practiceid"]);
                   Session["Prov_ID"] = Convert.ToString(TempData["Practiceid"]);
               }
               TempData.Keep("Practiceid");
              objInvoiceInfo.ToReferenceType_ID = "2";
           
            if (ddlInvoiceYear != null && monthFull != null && monthName != null)
            {
                ViewBag.monthName = "ThMonth" + monthName;
                ViewBag.monthFull = monthFull;
                ViewBag.ddlyear = ddlInvoiceYear;
                objInvoiceInfo.Invoice_Date = monthName + "/" + "01/" + ddlInvoiceYear;
            }
            else if (ddlInvoiceYear != null && monthName == null && monthFull == null)
            {
                ViewBag.monthName = "ThMonth" + monthName;
                ViewBag.monthFull = monthFull;
                ViewBag.ddlyear = ddlInvoiceYear;
                string monthse = DateTime.Now.Month.ToString();
                string monName = DateTime.Now.ToString("MMMM");
                ViewBag.monthName = "ThMonth" + monthse;
                ViewBag.monthFull = monName;
                objInvoiceInfo.Invoice_Date = monthse + "/" + "01/" + ddlInvoiceYear;
            }
            else
            {
                objInvoiceInfo.Invoice_Date = DateTime.Now.ToShortDateString();
                ViewBag.monthName = "ThMonth" + DateTime.Now.Month.ToString();
                ViewBag.monthFull = DateTime.Now.ToString("MMMM");
                ViewBag.ddlyear = DateTime.Now.Year;
            }
           
            objInvoiceInfo.FromDate = null;
            objInvoiceInfo.ToDate = null;
            objInvoiceInfo.NoOfRecords = "5";
            //if (Session["roleid"].ToString() != "1")
            //{
            //    objInvoiceInfo.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
            //}
            //else
            //{
            //    objInvoiceInfo.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
            //}
            TempData.Keep("Practiceid");
            objInvoiceInfo.OrderbyItem = Request["sort"] == null ? "Invoice_Date" : Request["sort"];
            objInvoiceInfo.orderBy = Request["sortdir"] == null ? "DESC" : Request["sortdir"];

            objInvoiceInfo.PageNo = Request["page"] != null ? Request["page"] : "1";
             invInfo = objInvoiceInfo.GET_Invoice(objInvoiceInfo);
            if (invInfo.Count > 0)
            {
                ViewBag.Instotrec = RPBilling.Totalnoofrecords;

            }
             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getprInvoiceList", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return invInfo;
        }
        public ActionResult AddPracticePayment()
        {
            return View();
        }
        public ActionResult PrintReceipt(int rid)//, string proid, string proname)
        {
            //if (TempData["Practiceid"] != null)
            //{
            //    if (Convert.ToString(TempData["Practiceid"]) != proid || Convert.ToString(TempData["pracname"]) != proname)
            //    {
            //        return RedirectToAction("PageNotFound", "Error");
            //    }
            //}
            TempData.Keep("pracname");
                RPBilling objRecipt = new RPBilling();
                objRecipt.fromReferenceId = 1;
                objRecipt.ReferenceType_ID = "1";
                if (Convert.ToString(Session["roleid"]) != "1")
                {
                    //objRecipt.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                    objRecipt.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
                }
                else
                {
                    //objRecipt.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                    objRecipt.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
                }
                objRecipt.ToReferenceType_ID = "2";
                TempData.Keep("Practiceid");
                //ViewBag.proid = proid;
                //ViewBag.proname = proname;
                if (rid != 0)
                {
                    objRecipt.Invoice_ID = rid;
                    DataSet dsInvoice = new DataSet();
                    dsInvoice = objRecipt.practiceInvoicelist(objRecipt);
                    StringBuilder strPreview = new StringBuilder();

                    for (int i = 0; i <= dsInvoice.Tables[0].Rows.Count - 1; i++)
                    {
                        string prAddress = Convert.ToString(dsInvoice.Tables[0].Rows[0]["FullAddress"]);
                        ViewBag.recid = dsInvoice.Tables[0].Rows[0]["recieptno"];
                        ViewBag.recDate = DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Invoice_Date"])).ToShortDateString();
                        string[] arrayAddress = prAddress.Split(',');
                        strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' width='80%'><tr class='background_color'><td width='50%'>");
                        strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='0' height='100%' width='100%'><tr class='background_color'><td style='PADDING-LEFT: 10px'><b>Bill To:&nbsp; </b></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color' height='52'><td><span id='lblBillable'>" + dsInvoice.Tables[0].Rows[0]["PracticeName"] + "</span><br><span id='lblAddress'>" + arrayAddress[0] + ",<br>" + arrayAddress[1] + ",<br>" + arrayAddress[2] + "</span></td></tr></table></td>");
                        strPreview = strPreview.Append("<td><table cellpadding='1' cellspacing='1' border='0' height='100%' width='100%'><tr class='background_color' height='22'><td colspan='2' align='center'><b>Summary</b></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Previous Balance&nbsp;:&nbsp;</b></td><td width='50%'><span id='lblBalenceforwarded'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["PrevBalance"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td style='HEIGHT: 14px' align='right'><b>Current&nbsp;Charges&nbsp;:&nbsp;</b></td><td style='HEIGHT: 14px'><span id='lblCInvoice'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceCharges"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Payments&nbsp;:&nbsp;</b></td><td><span id='lblPayments'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoicePayments"]) + "</span></td></tr>");
                        strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Balance&nbsp;:&nbsp;</b></td><td><span id='lblBalence'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceBalance"]) + "</span></td></tr></table></td></tr></table>");

                    }
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'><font size='3'><strong>Transactions</strong></font> </td></tr></table>");
                    strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'>");
                    strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'><b>Transaction date</b></td><td style='width:100px;'><b>Transaction Type</b></td><td style='width:250px;'><b>Notes</b></td><td style='width:100px;' align='right'><b>Amount</b></td><td style='width:100px;' align='right'><b>Total amount</b></td></tr>");
                    for (int j = 0; j <= dsInvoice.Tables[1].Rows.Count - 1; j++)
                    {
                        strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'>" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[1].Rows[j]["Transaction_Date"])).ToShortDateString() + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["TransactionType"] + "</td><td style='width:250px;'>" + dsInvoice.Tables[1].Rows[j]["Notes"] + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td></tr>");
                    }
                    strPreview = strPreview.Append("</table>");
                    ViewBag.invoiceprview = Convert.ToString(strPreview);
                }
            return View();
        }
        public ActionResult InvoiceWizardpreview( int ivid)
        {
            try
            {           
            RPBilling objInvoice = new RPBilling();
            objInvoice.fromReferenceId = 1;
            objInvoice.ReferenceType_ID = "1";
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                //objInvoice.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                objInvoice.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
            }
            else
            {
                //objInvoice.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                objInvoice.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
            }
            TempData.Keep("Practiceid");
            TempData.Keep("pracname");
            objInvoice.ToReferenceType_ID = "2";
            //objInvoice.Messagetouser = "Thankyou";
            //objInvoice.TermsAndConditions = "Thankyou";
            //objInvoice.UserId = Convert.ToString(Session["UserID"]);
            objInvoice.Invoice_ID = ivid;
            DataSet dsInvoice = new DataSet();
            dsInvoice = objInvoice.practiceInvoicelist(objInvoice);
            StringBuilder strPreview = new StringBuilder();

            for (int i = 0; i <= dsInvoice.Tables[0].Rows.Count - 1; i++)
            {
                string prAddress = Convert.ToString(dsInvoice.Tables[0].Rows[0]["FullAddress"]);
                ViewBag.invoiceno = dsInvoice.Tables[0].Rows[0]["InvoiceNo"];
                ViewBag.invoiceDate = DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Invoice_Date"])).ToShortDateString();
                string[] arrayAddress = prAddress.Split(',');
                strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' width='80%'><tr class='background_color'><td width='50%'>");
                strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='0' height='100%' width='100%'><tr class='background_color'><td style='PADDING-LEFT: 10px'><b>Bill To:&nbsp; </b></td></tr>");
                strPreview = strPreview.Append("<tr class='background_color' height='52'><td><span id='lblBillable'>" + dsInvoice.Tables[0].Rows[0]["PracticeName"] + "</span><br><span id='lblAddress'>" + arrayAddress[0] + ",<br>" + arrayAddress[1] + ",<br>" + arrayAddress[2] + "</span></td></tr></table></td>");
                strPreview = strPreview.Append("<td><table cellpadding='1' cellspacing='1' border='0' height='100%' width='100%'><tr class='background_color' height='22'><td colspan='2' align='center'><b>Summary</b></td></tr>");
                strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Previous Balance&nbsp;:&nbsp;</b></td><td width='50%'><span id='lblBalenceforwarded'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["PrevBalance"]) + "</span></td></tr>");
                strPreview = strPreview.Append("<tr class='background_color'><td style='HEIGHT: 14px' align='right'><b>Current&nbsp;Charges&nbsp;:&nbsp;</b></td><td style='HEIGHT: 14px'><span id='lblCInvoice'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceCharges"]) + "</span></td></tr>");
                strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Payments&nbsp;:&nbsp;</b></td><td><span id='lblPayments'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoicePayments"]) + "</span></td></tr>");
                strPreview = strPreview.Append("<tr class='background_color'><td align='right'><b>Balance&nbsp;:&nbsp;</b></td><td><span id='lblBalence'>" + string.Format("{0:c}", dsInvoice.Tables[0].Rows[0]["InvoiceBalance"]) + "</span></td></tr></table></td></tr></table>");
                strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'>Invoice period &nbsp;&nbsp;From&nbsp;: &nbsp;" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["Begin_Date"])).ToShortDateString() + " &nbsp;&nbsp;To&nbsp;:&nbsp;" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[0].Rows[0]["End_Date"])).ToShortDateString() + "  </td></tr></table>");
            }
            strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'><tr class='background_color'><td align='center'><font size='3'><strong>Transactions</strong></font> </td></tr></table>");
            strPreview = strPreview.Append("<table cellpadding='1' cellspacing='1' align='center' border='1' height='100%' width='80%'>");
            strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'><b>Date</b></td><td style='width:100px;'><b>Transaction Type</b></td><td style='width:100px;'><b>Items</b></td><td style='width:250px;'><b>Notes</b></td><td style='width:100px;' align='right'><b>Amount</b></td></tr>");
            for (int j = 0; j <= dsInvoice.Tables[1].Rows.Count - 1; j++)
            {
                strPreview = strPreview.Append("<tr class='background_color'><td style='width:100px;'>" + DateTime.Parse(Convert.ToString(dsInvoice.Tables[1].Rows[j]["Transaction_Date"])).ToShortDateString() + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["TransactionType"] + "</td><td style='width:100px;'>" + dsInvoice.Tables[1].Rows[j]["BillingChargeType"] + "</td><td style='width:250px;'>" + dsInvoice.Tables[1].Rows[j]["Notes"] + "</td><td style='width:100px;' align='right'>" + string.Format("{0:c}", dsInvoice.Tables[1].Rows[j]["AmountCharged"]) + "</td></tr>");
            }
            strPreview = strPreview.Append("</table>");
            ViewBag.invoiceprview = Convert.ToString(strPreview);
            }
            catch (Exception ex)
            {

                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "InvoiceWizardpreview", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return View();
        }
        public ActionResult RegenerateInvoice(int ivid)
        {
            try
            {
                RPBilling objInvoice = new RPBilling();
            objInvoice.fromReferenceId = 1;
            objInvoice.ReferenceType_ID = "1";
            if (Convert.ToString(Session["roleid"]) != "1")
            {
                //objInvoice.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                objInvoice.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
            }
            else
            {
                //objInvoice.Practice_ID = Convert.ToInt32(TempData["Practiceid"]);
                objInvoice.ToReference_ID = Convert.ToString(Session["Prov_ID"]);
            }
            TempData.Keep("Practiceid");
            objInvoice.ToReferenceType_ID = "2";
            objInvoice.UserId = Convert.ToString(Session["roleid"]);
            objInvoice.Invoice_ID = ivid;
            objInvoice.invoiceRegenerate(objInvoice);
            return RedirectToAction("PracticeServicesInvoice");
            }
            catch (Exception ex)
            {

                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "RegenerateInvoice", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }
#endregion
        #region "MailStatus"
        public ActionResult MailStatus(string[] chkStatusbox)
        {
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string FromDate = null;
            string ToDate = null;
            ViewBag.txtFromDate = string.IsNullOrEmpty(Request["txt_FromDate"]) ? null : Request["txt_FromDate"];
            ViewBag.txtToDate = string.IsNullOrEmpty(Request["txt_ToDate"]) ? null : Request["txt_ToDate"];
            ViewBag.Daterange = string.IsNullOrEmpty(Request["dt_filter"]) ? "30" : Request["dt_filter"];
            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            {
                FromDate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"]);
                ToDate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? startdate : Request["txt_ToDate"]);
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
            if (Request["btnSend"] != "Send")
            {

                var objcls = new messageStatus();
                var objlist = objcls.FillProviderids();
                IList<SelectListItem> listItems = new List<SelectListItem>();
                int providerid = 0; int patId = 0;
                if (Request["PracticeList"] != null && Request["PracticeList"] != "")
                {
                    providerid = Convert.ToInt32(Request["PracticeList"]);
                    ViewBag.pract = providerid;
                }
                if (Request["PatList"] != null && Request["PatList"] != "")
                {
                    patId = Convert.ToInt32(Request["PatList"]);
                    ViewBag.pat = patId;
                }
                if (objlist.Count > 0)
                {
                    for (int i = 0; i < objlist.Count; i++)
                    {
                        if (providerid != 0)
                        {
                            if (providerid == objlist[i].practiceId)
                            {
                                listItems.Add(new SelectListItem
                                {
                                    Text = objlist[i].practiceName,
                                    Value = Convert.ToString(objlist[i].providerId),
                                    Selected = true
                                });
                            }
                            else
                            {

                                listItems.Add(new SelectListItem
                                {
                                    Text = objlist[i].practiceName,
                                    Value = Convert.ToString(objlist[i].providerId)
                                });
                            }
                        }
                        else
                        {

                            listItems.Add(new SelectListItem
                            {
                                Text = objlist[i].practiceName,
                                Value = Convert.ToString(objlist[i].providerId)
                            });
                        }
                    }
                }
                ViewData["PracticeList"] = listItems;
                var objPatlist = objcls.fillPatientName(providerid);
                IList<SelectListItem> PatlistItems = new List<SelectListItem>();
                if (objPatlist.Count > 0)
                {
                    for (int i = 0; i < objPatlist.Count; i++)
                    {
                        if (patId != 0)
                        {
                            if (patId == objPatlist[i].patientId)
                            {
                                PatlistItems.Add(new SelectListItem
                                {
                                    Text = objPatlist[i].patientName,
                                    Value = Convert.ToString(objPatlist[i].patientId),
                                    Selected = true
                                });
                            }
                            else
                            {

                                PatlistItems.Add(new SelectListItem
                                {
                                    Text = objPatlist[i].patientName,
                                    Value = Convert.ToString(objPatlist[i].patientId)
                                });
                            }
                        }
                        else
                        {

                            PatlistItems.Add(new SelectListItem
                            {
                                Text = objPatlist[i].patientName,
                                Value = Convert.ToString(objPatlist[i].patientId)
                            });
                        }
                    }
                }
                ViewData["PatList"] = PatlistItems;
                messageStatus objgrid = new messageStatus();
                if (providerid != 0)
                {
                    objgrid.providerId = providerid;
                }
                if (patId != 0)
                {
                    objgrid.patientId = patId;
                }
                if (Request["statusddl"] != null)
                {
                    if (Convert.ToInt32(Request["statusddl"]) == 3)
                    {
                        objgrid.statusId =null;
                        ViewBag.statusind = Request["statusddl"];
                    }
                    else
                    {
                        objgrid.statusId = Convert.ToInt32(Request["statusddl"]);
                        ViewBag.statusind = Request["statusddl"];
                    }
                }
                else
                {
                    objgrid.statusId = 1;
                    ViewBag.statusind = "1";
                }
              
                    objgrid.fromdate = FromDate;
                   
              
              
                    objgrid.todate = ToDate;
                  
                if (!string.IsNullOrEmpty(Request["mailType"]))// != null && Request["mailType"] != "")
                {
                    objgrid.isRec = Request["mailType"];
                    ViewBag.isrec = Request["mailType"];
                }
                else
                {
                    objgrid.isRec = "No";
                    ViewBag.isrec = "No";
                }
                objgrid.orderByItem = "Mail_Generateddate";
                objgrid.orderBy = "DESC";
                string pgesize = null;
                if (Request["ddlrecords"] != null)
                {
                    ViewBag.Summarypagesize = Request["ddlrecords"];
                    TempData["Summarypagesize"] = Request["ddlrecords"];
                    pgesize = Request["ddlrecords"];
                }
                else if (Session["Rowsperpage"] != null)
                {
                    ViewBag.Summarypagesize = Session["Rowsperpage"].ToString();
                    TempData["Summarypagesize"] = Session["Rowsperpage"].ToString();
                    pgesize = Session["Rowsperpage"].ToString();
                }
                else
                {
                    ViewBag.Summarypagesize = "10";
                    TempData["Summarypagesize"] = "10";
                    pgesize = "10";
                }

                if (pgesize != null)
                {
                    objgrid.noofrecords = pgesize;
                }
                else
                {
                    objgrid.noofrecords = "10";
                }
                if (Request["page"] == null)
                {
                    objgrid.pageno = "1";
                }
                else
                {
                    objgrid.pageno = Request["page"];
                }
                dynamic statugrid = getMailstatusGrid(objgrid);
                return View(statugrid);
            }
            else
            {
               
                string tranids = string.Empty;
                foreach (var ids in chkStatusbox)
                {
                    messageStatus objclass=new messageStatus();
                    DataSet mailInfo = new DataSet();
                    mailInfo = objclass.getmailInfo(Convert.ToInt32(ids));
                    if (mailInfo.Tables[0].Rows.Count > 0)
                    {
                        if(!DBNull.Value.Equals(mailInfo.Tables[0].Rows[0]["Mail_From"]))
                        {
                            objclass.mailFrom = Convert.ToString(mailInfo.Tables[0].Rows[0]["Mail_From"]);
                        }
                        if (!DBNull.Value.Equals(mailInfo.Tables[0].Rows[0]["Mail_To"]))
                        {
                            objclass.mailTo = Convert.ToString(mailInfo.Tables[0].Rows[0]["Mail_To"]);
                        }
                        if (!DBNull.Value.Equals(mailInfo.Tables[0].Rows[0]["Subject"]))
                        {
                            objclass.mailstatus = Convert.ToString(mailInfo.Tables[0].Rows[0]["Subject"]);
                        }
                        if (!DBNull.Value.Equals(mailInfo.Tables[0].Rows[0]["Message_Body"]))
                        {
                            objclass.mailbody = Convert.ToString(mailInfo.Tables[0].Rows[0]["Message_Body"]);
                        }
                        var objMailMessage = new ClsMailMessage();
                        var objWrapper = new clsDBWrapper();
                        bool res = objMailMessage.SendMail(objclass.mailTo, objclass.mailFrom, objclass.mailstatus, objclass.mailbody, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                        if (res == true)
                        {
                            IDataParameter[] strMailParam = { new SqlParameter("@in_Emailtrn_ID", ids) };
                            var with2 = objWrapper;
                            with2.AddInParameters(strMailParam);
                            with2.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_upd_Expiryservices");

                        }
                        else
                        {
                            TempData["Result"] = res;
                            //return View();
                        }
                    }
                }
                return RedirectToAction("MailStatus");
               
            }
        }
        public List<messageStatus> getMailstatusGrid(messageStatus objmail)
        {
            List<messageStatus> objstatus = new List<messageStatus>();
            try
            {
            objstatus = objmail.mailStatusGrid(objmail);
            if (objstatus.Count > 0)
            {
                //totalrecords =Convert.ToString(messageStatus.Totalnoofrecords);
                ViewBag.totrec = messageStatus.Totalnoofrecords;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getMailstatusGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objstatus;
        }
        #endregion
        #region "AddressInformation"
        public ActionResult AddressInformation()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            AdminAddress objaddress = new AdminAddress();
            //if (string.IsNullOrEmpty( Request["sort"] == null)
            //{
            //    objaddress.orderByItem = null;               
            //}
            //else
            //{
            //    objaddress.orderByItem = "Full_Name";
                
            //}
            objaddress.orderByItem = string.IsNullOrEmpty(Request["sort"]) ? null : "Full_Name";
            //if (Request["sortdir"] == null)
            //{
            //    objaddress.orderBy = null;
              
            //}
            //else
            //{
            //    objaddress.orderBy = Request["sortdir"].ToString();
              
            //}
            objaddress.orderBy = string.IsNullOrEmpty(Request["sortdir"]) ? null : Request["sortdir"];
            //if (Request["page"] == null)
            //{
            //    objaddress.pageno = "1";
            //}
            //else
            //{
            //    objaddress.pageno = Convert.ToString(Request["page"]);
            //}
            objaddress.pageno = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
            string pgesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.Addresspagesize = Request["ddlrecords"];
                pgesize = Request["ddlrecords"];
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.Addresspagesize = Session["Rowsperpage"].ToString();
                pgesize = Session["Rowsperpage"].ToString();
            }
            else
            {
                ViewBag.Addresspagesize = "10";
                pgesize = "10";
            }

            //if (pgesize != null)
            //{
            //    objaddress.noofrecords = pgesize;
            //}
            //else
            //{
            //    objaddress.noofrecords = "10";
            //}
            objaddress.noofrecords = !string.IsNullOrEmpty(pgesize) ? pgesize : "10";
            //if (Request["ddlUser"] != null && Request["ddlUser"]!="")
            //{
            //if (!string.IsNullOrEmpty(Convert.ToString(Request["ddlUser"])))
            //{

            //    objaddress.roleId = Convert.ToInt32(Request["ddlUser"]);
            //    ViewBag.ddlUser=Request["ddlUser"];
            //}
            //else
            //{
            //    objaddress.roleId=5;
            //    ViewBag.ddlUser="5";
            //}
            if (Request["ddlUser"] == "4")
            {

                objaddress.roleId = Convert.ToInt32(Request["ddlUser"]);
                ViewBag.ddlUser = Request["ddlUser"];
            }
            else if (Request["ddlUser"] == "5")
            {

                objaddress.roleId = Convert.ToInt32(Request["ddlUser"]);
                ViewBag.ddlUser = Request["ddlUser"];
            }
            else if (Request["ddlUser"] != "5" && Request["ddlUser"] != "4")
            {
                objaddress.roleId = 5;
                ViewBag.ddlUser = "5";
            }




            if (ViewBag.ddlUser == "5" )
            {
                if (!string.IsNullOrEmpty(Request["txtPatient"]))
                {
                    objaddress.Full_Name = Request["txtPatient"];
                    ViewBag.txtPatient = Request["txtPatient"];
                }                               
            }
            else if (ViewBag.ddlUser == "4")
            {
                if (!string.IsNullOrEmpty(Request["txtProvider"]))
                {
                    objaddress.Full_Name = Request["txtProvider"];
                    ViewBag.txtProvider = Request["txtProvider"];
                }   
            }
            dynamic addressGrid=fillAddressGrid(objaddress);
            return View(addressGrid);
        }
        public List<AdminAddress> fillAddressGrid(AdminAddress objadmin)
        {
            List<AdminAddress> objlist = new List<AdminAddress>();
            try{
            objlist = objadmin.getAddressGrid(objadmin);
            if (objlist.Count > 0)
            {
                //totalrecords = Convert.ToString(AdminAddress.Totalnoofrecords);
                ViewBag.totrec = AdminAddress.Totalnoofrecords;
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "fillAddressGrid", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return objlist;
        }
        public ActionResult ContactsInfo(int rid,int lid)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            AdminAddress objaddress = new AdminAddress();
            DataSet dsContactinfo = new DataSet();
            dsContactinfo = objaddress.getcontactinfo(rid, lid);
            if (dsContactinfo.Tables[0].Rows.Count > 0)
            {
                ViewBag.username = dsContactinfo.Tables[0].Rows[0]["NAME"];
                ViewBag.address1 = dsContactinfo.Tables[0].Rows[0]["Address1"];
                ViewBag.address2 = dsContactinfo.Tables[0].Rows[0]["Address2"];
                ViewBag.Czipcode = dsContactinfo.Tables[0].Rows[0]["ZIP"];
                ViewBag.Ccountry = dsContactinfo.Tables[0].Rows[0]["Country"];
                ViewBag.CState = dsContactinfo.Tables[0].Rows[0]["State"];
                ViewBag.CCity = dsContactinfo.Tables[0].Rows[0]["City"];
                if (!DBNull.Value.Equals(dsContactinfo.Tables[0].Rows[0]["HomePhone"]))
                {
                    ViewBag.CHomePhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsContactinfo.Tables[0].Rows[0]["HomePhone"]));
                }
                else
                {
                    ViewBag.CHomePhone = null;
                }
                if (!DBNull.Value.Equals(dsContactinfo.Tables[0].Rows[0]["WorkPhone"]))
                {
                    ViewBag.CWorkPhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsContactinfo.Tables[0].Rows[0]["WorkPhone"]));
                }
                else
                {
                    ViewBag.CWorkPhone = null;
                }
                if (!DBNull.Value.Equals(dsContactinfo.Tables[0].Rows[0]["CellPhone"]))
                {
                    ViewBag.CCellPhone = clsCommonFunctions.UsPhoneFormat(Convert.ToString(dsContactinfo.Tables[0].Rows[0]["CellPhone"]));
                }
                else
                {
                    ViewBag.CCellPhone = null;
                }
                ViewBag.CEmail = dsContactinfo.Tables[0].Rows[0]["Email"];
            }
            return View();
        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetPatientfullname(string term)
        {
            List<string> objlist = new List<string>();
          
            clsCommonFunctions objcomman = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@In_practice_id",null),
		new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.St_practice_Typeahead_Patient_Fullname");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString(drlist[0]));
            }
           
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProviderNames(string term)
        {
            var objlist = new List<Autocomplete>();
            var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
				new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = objcomman.GetDataReader("Help_dbo.st_Typeahead_provider_practicename");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[2]) + "$" + Convert.ToString(drlist[3]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_Article_Titles(string term)
        {
            var objlist = new List<Autocomplete>();
            var objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {
           new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = objcomman.GetDataReader("Help_dbo.St_PublicArticles_Typeheader_Titles");
            while (drlist.Read())
            {
                objlist.Add(new Autocomplete { Name = Convert.ToString(drlist[0]), value = Convert.ToString(drlist[0]) });
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }


        #endregion
        public ActionResult Articles()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            ViewBag.title = !string.IsNullOrEmpty(Request["txtart_title"]) ? Request["txtart_title"] : null;
            ViewBag.Fromdate = string.IsNullOrEmpty(Request["txt_FromDate"]) ? null : Request["txt_FromDate"];
            ViewBag.Todate = string.IsNullOrEmpty(Request["txt_ToDate"]) ? null : Request["txt_ToDate"];
            ViewBag.Daterange = string.IsNullOrEmpty(Request["dt_filter"]) ? "30" : Request["dt_filter"];
            ViewBag.cat = !string.IsNullOrEmpty(Request["ddllistCategory"]) ? Request["ddllistCategory"] : "0";
            ViewBag.subcat = !string.IsNullOrEmpty(Request["ddllistSubCategory"]) ? Request["ddllistSubCategory"] : "0";

            //List<EmailMessageOption> catlist = new List<EmailMessageOption>();
            //EmailMessageOption EMO = new EmailMessageOption();
            //catlist = EMO.ADMIN_List_Main_Email_OptionTypes();

            List<Referrals> catlist = new List<Referrals>();
            Referrals objcat = new Referrals();
            catlist = objcat.BindCaregories();

            IList<SelectListItem> _result1 = new List<SelectListItem>();
            if (catlist.Count > 0)
            {
                for (int i = 0; i <= catlist.Count - 1; i++)
                {
                    _result1.Add(new SelectListItem
                    {
                        Text = catlist[i].Category_Title,
                        Value = Convert.ToString(catlist[i].ArticleCategory_ID)
                    });
                }
            }
            IList<SelectListItem> _result2 = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(Request["ddllistCategory"]))
            {
                //List<EmailMessageOption> Subcatlist = new List<EmailMessageOption>();
                //EmailMessageOption EMO1 = new EmailMessageOption();
                //EMO1.Email_OptionType_ID = Convert.ToInt32(Request["ddllistCategory"]);
                //Subcatlist = EMO1.ADMIN_List_ExistingEmail_OptionTypes(EMO1);

                List<Referrals> Subcatlist = new List<Referrals>();
                Referrals objindex = new Referrals();
                objindex.ParentCategory_ID = Convert.ToInt32(Request["ddllistCategory"]);
                Subcatlist = objindex.BindArticalIndex(objindex);


                if (Subcatlist.Count > 0)
                {

                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Article Index--",
                        Value = "0",
                        Selected = true
                    });

                    for (int i = 0; i <= Subcatlist.Count - 1; i++)
                    {
                        _result2.Add(new SelectListItem
                        {

                            Text = Subcatlist[i].Category_Title,
                            Value = Convert.ToString(Subcatlist[i].ArticleCategory_ID)
                        });
                    }
                }
                else
                {
                    _result2.Add(new SelectListItem
                    {
                        Text = "--Select Article Index--",
                        Value = "0",
                        Selected = true
                    });
                }
            }
            else
            {
                _result2.Add(new SelectListItem
                {
                    Text = "--Select Article Index--",
                    Value = "0",
                    Selected = true
                });
            }
            //StateCity reg1 = new StateCity();
            StateCity reg1 = new StateCity
            {
                StateList = _result1,
                CityList = _result2
            };
            return View(reg1);
        }
        public ActionResult ArticlesGrid()
        {

           List<Referrals> list = new List<Referrals>();

           string startdate;
           startdate = DateTime.Now.ToString("MM/dd/yyyy");
           //ViewBag.Fromdate = (Request["txt_FromDate"] == null || Request["txt_FromDate"] == "" ? null : Request["txt_FromDate"]);
           //ViewBag.Todate = (Request["txt_ToDate"] == null || Request["txt_ToDate"] == "" ? null : Request["txt_ToDate"]);
           //ViewBag.Daterange = (Request["dt_filter"] == null || Request["dt_filter"] == "" ? "7" : Request["dt_filter"]);
           string FromDate = null;
           string ToDate = null;

           if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
           {
               FromDate = string.IsNullOrEmpty(Request["txt_FromDate"]) ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"];
               ToDate = string.IsNullOrEmpty(Request["txt_ToDate"]) ? startdate : Request["txt_ToDate"];
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

            
            Referrals video = new Referrals();
            if (Request["ddlrecords"] != "undefined")
            {
                video.NoofRecords = Convert.ToInt32(!string.IsNullOrEmpty(Request["ddlrecords"]) ? Request["ddlrecords"] : "10");
                ViewBag.pagesize = Request["ddlrecords"];
            }
            else {
                ViewBag.pagesize = "10";
            }
            //int NoofRecords = Convert.ToInt32(10);
            //if (NoofRecords > 0)
            //{
            //    video.NoofRecords = 10;
            //}
            //else
            //{
            //    NoofRecords = 0;
            //}
                video.PageNo = Request["page"]!=null ? Request["page"]: "1" ;
                video.OrderByItem = "Title";
                video.OrderBy = "DESC";
                video.Category_id = string.IsNullOrEmpty(Request["ddllistCategory"]) ? 0 : Convert.ToInt32(Request["ddllistCategory"]);
                video.ArticalIndex = Convert.ToInt32(Request["ddllistSubCategory"]) > 0 ? Convert.ToInt32(Request["ddllistSubCategory"]) : 0;
            video.FromDate = FromDate;
            video.Todate = ToDate;
            video.Title = Request["txtart_title"];

            video.Online_ind = "Y";

            list = Referrals.Admin_List_Articles(video);
            ViewBag.totrec = Referrals.TotalRecords;
            return View(list);

        }
           [HttpGet()]
           public ActionResult AddNewArticles()
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;

            Referrals objcat = new Referrals();
            //List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
            List<Referrals> objList = new List<Referrals>();
            objList = objcat.BindCaregories();
            IList<SelectListItem> _result1 = new List<SelectListItem>();
            if (objList.Count > 0)
            {

                for (int i = 0; i <= objList.Count - 1; i++)
                {
                    _result1.Add(new SelectListItem
                    {
                        Text = objList[i].Category_Title,
                        Value = Convert.ToString(objList[i].ArticleCategory_ID)
                    });
                }
            }
            StateCity reg1 = new StateCity
            {
                StateList = _result1
            };
            ViewBag.ddlcat = !string.IsNullOrEmpty(Request["ddllistCategory"]) ? Request["ddllistCategory"] : "";
            //Bind_catgory();
            Bind_Index(Request["ddllistCategory"]);
            ViewBag.Todaydate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewData["txtArticleDesc"] = "";
            ViewData["txtautherdes"] = "";
            return View(reg1);
        }
           [HttpPost()]
             public JsonResult AddNewArticles(string obj)
           {
               
                  string strartindex = Request["ComboIndexlist"];
               //string Article_Desc = System.Web.HttpUtility.HtmlDecode(Request["txtArticleDesc"]);

               Referrals Articles = new Referrals();
             //  int createdby = 1;
               //if (Request["ddlCategory"] != "" && Request["ddlCategory"]!=null)
               //{
               // Articles.Article_ID = Convert.ToInt32(Request["ddlCategory"]);
               //}
               //else
               //{
               //    Articles.Article_ID = null;
               //}
               // ViewState("Article_ID")
               Articles.Title = !string.IsNullOrEmpty(Request["txtArticleName"]) ? Request["txtArticleName"] : null;
               Articles.Article_Desc = System.Web.HttpUtility.HtmlDecode(Request["txtArticleDesc"] != null ? Request["txtArticleDesc"] : null);
               Articles.CreatedOn = !string.IsNullOrEmpty(Request["txt_Date"]) ? Request["txt_Date"] : DateTime.Now.ToString("MM/dd/yyyy");
               Articles.CreatedBy = "1";
               Articles.Online_ind = "Y";
               if (!string.IsNullOrEmpty(Request["hdnfaqid"]))// != "" && Request["hdnfaqid"] != null)
               {
                   Articles.Related_Article_ID = Request["hdnfaqid"] + ",";
               }
               else
               {
                   Articles.Related_Article_ID = null;
               }
               if (!string.IsNullOrEmpty(Request["hndmediaid"]))// != "" && Request["hndmediaid"] != null)
               {
                   Articles.Related_video_id = Request["hndmediaid"] + ",";
               }
               else
               {
                   Articles.Related_video_id = null;
               }
               //string strout = null;
               string strout = Convert.ToString(Referrals.Ins_Article(Articles));
               if (strout != null & strout.Length > 12)
               {
                   return Json(JsonResponseFactory.ErrorResponse(strout), JsonRequestBehavior.DenyGet);
               }
               else
               {
                   if (strout != null)
                   {
                       Referrals ArticlesAuthor = new Referrals();
                       ArticlesAuthor.Article_ID = Convert.ToInt32(strout);
                       if (!string.IsNullOrEmpty(Request["hdnproviderid"]))// != null && Request["hdnproviderid"] != "")
                       {
                           ArticlesAuthor.Provider_ID = Convert.ToInt32(Request["hdnproviderid"]);
                       }
                       else
                       {
                           ArticlesAuthor.Provider_ID = null;
                       }
                       if (!string.IsNullOrEmpty(Request["txtautherdes"]))// != null && Request["txtautherdes"] != "")
                       {
                           ArticlesAuthor.Description = System.Web.HttpUtility.HtmlDecode(Request["txtautherdes"]);
                       }
                       else
                       {
                           ArticlesAuthor.Description = null;
                       }
                       ArticlesAuthor.CreatedBy = Convert.ToString(Session["userid"]);

                       Referrals.Ins_ArticleAuthor(ArticlesAuthor);
                       if (!string.IsNullOrEmpty(strartindex))// != "" && strartindex != null)
                       {
                       InsArCategories(strartindex, Convert.ToInt32(strout));
                       }
                   }
                   return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
               }
           }
          [HttpGet()]
           public ActionResult EditArticle()
           {
               ViewBag.Count = 4;
               Session["CurrentUrl3"] = Request.RawUrl;
               if (Request["lbloutmsg"] == "Y")
               {
                   ViewBag.outmsg = "Y";
               }

               Referrals objcat = new Referrals();
               //List<ExpenseLedgerList> objList = new List<ExpenseLedgerList>();
               List<Referrals> objList = new List<Referrals>();
               objList = objcat.BindCaregories();
               IList<SelectListItem> _result1 = new List<SelectListItem>();
               if (objList.Count > 0)
               {
                   _result1.Add(new SelectListItem
                   {
                       Text = "-- Select Category --",
                       Value = "0"
                   });

                   for (int i = 0; i <= objList.Count - 1; i++)
                   {
                       _result1.Add(new SelectListItem
                       {
                           Text = objList[i].Category_Title,
                           Value = Convert.ToString(objList[i].ArticleCategory_ID)
                       });
                   }
               }
               //StateCity reg1 = new StateCity();
               StateCity reg1 = new StateCity
               {
                   StateList = _result1
               };
               //if (Request["ddllistCategory"] != null)
               //{
               //    ViewBag.ddlcat = Request["ddllistCategory"];
               //}
               //else
               //{
               //    ViewBag.ddlcat = "";
               //}
               //Bind_catgory();
               //Bind_Index(Request["ddllistCategory"]);
               //ViewBag.Todaydate = DateTime.Now.ToString("MM/dd/yyyy");
               //Referrals art = new Referrals();
               var ds = new DataSet();
               ds = Referrals.Get_Articles(Convert.ToInt32(Request["Article_ID"]));


              if (ds.Tables[0].Rows[0]["Title"] != DBNull.Value)
              {
                  ViewBag.txtArticleName = ds.Tables[0].Rows[0]["Title"];
              }
              else
              {
                  ViewBag.txtArticleName = null;
              }
              if (ds.Tables[0].Rows[0]["Article_Desc"] != DBNull.Value)
              {
                  //ViewBag.txtArticleDesc = ds.Tables[0].Rows[0]["Article_Desc"];
                  ViewData["txtArticleDesc"] = ds.Tables[0].Rows[0]["Article_Desc"];
              }
              else
              {
                  ViewData["txtArticleDesc"] = null;
              }
              //if (ds.Tables[0].Rows[0]["Online_Ind"] != DBNull.Value)
              //{
              //}
              //else
              //{
              //}
              //if (ds.Tables[0].Rows[0]["Imagepath"] != DBNull.Value)
              //{
              //}
              //else
              //{
              //}
              //clsWebConfigsettings clsWebConfigsettings = new clsWebConfigsettings();
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
              if (ds.Tables[0].Rows[0]["Imagepath"] != DBNull.Value)
              {
                  string str = ds.Tables[0].Rows[0]["Imagepath"].ToString();
                  ViewData["str"] = str;
                  ViewBag.strimage = str;
                  if (str.Contains(".") == true)
                  {
                      string[] str1 = str.Split('.');
                      //ViewData["ExistedLogo"] = str1[0];
                      ViewBag.ExistedLogo = str1[0];
                      //ViewData["ExistedExtn"] = str1[1];
                      ViewBag.ExistedExtn = str1[1];
                  }
                  //string strings = Path.Combine(Server.MapPath("~/Attachments/Providers"), str);
                  if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Attachments/Providers"), str)))
                  {

                      //ViewBag.imageavail = "Y";
                      ViewBag.image = "Y";
                      ViewBag.providerimage = domain + "Attachments/Providers/" + str;

                  }


              }


              if (ds.Tables[0].Rows[0]["Createdon"] != DBNull.Value)
              {
                  string[] regdate = ds.Tables[0].Rows[0]["Createdon"].ToString().Split(' ');
                  ViewBag.txt_Date = regdate[0];
              }
              else
              {
                  ds.Tables[0].Rows[0]["Createdon"] = null;
              }
              if (ds.Tables[0].Rows[0]["Provider_ID"] != DBNull.Value)
              {
                  ViewBag.Provider_ID = ds.Tables[0].Rows[0]["Provider_ID"];
              }
              else
              {
                  ViewBag.Provider_ID = null;
              }
              //if (ds.Tables[0].Rows[0]["PublicURL"] != DBNull.Value)
              //{
              //}
              //else
              //{
              //}
              if (ds.Tables[0].Rows[0]["Provider_Name"] != DBNull.Value)
              {
                  ViewBag.txtauthor = ds.Tables[0].Rows[0]["Provider_Name"];
              }
              else
              {
                  ViewBag.txtauthor = null;
              }
              if (ds.Tables[0].Rows[0]["Author_Desc"] != DBNull.Value)
              {
                  //ViewBag.txtautherdes=ds.Tables[0].Rows[0]["Author_Desc"];
                  ViewData["txtautherdes"] = ds.Tables[0].Rows[0]["Author_Desc"];
              }
              else
              {
                  //ViewBag.txtautherdes = null;
                  ViewData["txtautherdes"] = "";
              }

              //if (art.Provider_ID != null)
              //{
              //    ViewState("ProviderID") = art.Provider_ID;
              //}
              //if (art.ProviderName != null)
              //{
              //    txtauthor.Text = art.ProviderName;
              //    btnclrAuthor.Style.Add("display", "");
              //    //ViewState("AuthorDesc") = art.ProviderName
              //}
              //else
              //{
              //    btnclrAuthor.Style.Add("display", "none");
              //}
              //if (art.Author_Desc != null)
              //{
              //    txtautherdes.Content = art.Author_Desc;
              //    ViewState("AuthorDesc") = art.Author_Desc;
              //}
              //if (art.Title != null)
              //{
              //    txtArticleName.Text = art.Title;
              //}
              //else
              //{
              //    txtArticleName.Text = null;
              //}
              //if (art.Article_Desc != null)
              //{
              //    txtArticleDesc.Content = art.Article_Desc;
              //    ViewState("ArticleDesc") = art.Article_Desc;
              //}
              //else
              //{
              //    txtArticleDesc.Content = null;
              //}
              //Calendar3.SelectedDate = new DateTime(0);
              //txtDate.Value = "";
              //if (art.CreatedOn != null)
              //{
              //    txtDate.Value = Convert.ToDateTime(art.CreatedOn).ToShortDateString;
              //    Calendar3.DateToday = txtDate.Value;
              //}
              //else
              //{
              //    txtDate.Value = System.DateTime.Now.ToShortDateString;
              //}
              //if (art.Online_ind != "Y")
              //{
              //    rdonlineyes.Checked = false;
              //    rdonlineNo.Checked = true;
              //}
              //else
              //{
              //    rdonlineyes.Checked = true;
              //    rdonlineNo.Checked = false;
              //}

              DataSet ArDataSet = new DataSet();
              ArDataSet = Referrals.Get_ArticleIndexing(Convert.ToInt32(Request["Article_ID"]));
              string strcatvalue = null;
              if (ArDataSet.Tables[0].Rows.Count > 0)
              {
                  if (ArDataSet.Tables[0].Rows[0]["ParentCategory_ID"] != DBNull.Value)
                  {
                      ViewBag.ParentCategory_ID = ArDataSet.Tables[0].Rows[0]["ParentCategory_ID"];
                      strcatvalue = ArDataSet.Tables[0].Rows[0]["ParentCategory_ID"].ToString();
                  }
              }
              else
              {
                  ViewBag.ParentCategory_ID = "";
              }
              if (ArDataSet.Tables[1].Rows.Count > 0)
              {
                  //if (ArDataSet.Tables[1].Rows[0]["ArticleCategory_ID"] != DBNull.Value)
                  //{
                  //    ViewBag.articleindexvalue = ArDataSet.Tables[1].Rows[0]["ArticleCategory_ID"];
                  //}
                  string strindexvalue = null;
                  for (int i = 0; i <= ArDataSet.Tables[1].Rows.Count - 1; i++)
                  {
                   strindexvalue += ArDataSet.Tables[1].Rows[i]["ArticleCategory_ID"] + ",";
                  }
                  ViewBag.articleindexvalue = strindexvalue;

              }
              if (Request["ddllistCategory"] == null)
              {
                  Bind_Index(strcatvalue);
                  ViewBag.ddlcat = strcatvalue;
              }
              else
              {
                  Bind_Index(Request["ddllistCategory"]);
                  ViewBag.ddlcat = Request["ddllistCategory"];
              }
              GetRelatedArticles(Convert.ToInt32(Request["Article_ID"]));
              GetRelatedvideos(Convert.ToInt32(Request["Article_ID"]));
              ViewBag.Article_ID = Request["Article_ID"];
               return View(reg1);
           }
          [HttpPost()]
          //public JsonResult EditArticle(string obj)
          public ActionResult EditArticle(HttpPostedFileBase flLogo)
          {

              Referrals Articles = new Referrals();
              if (flLogo != null)
              {
                  if (flLogo.ContentLength > 0)
                  {
                      Getfilename(flLogo);
                  }
              }
              else
              {

                  if (!string.IsNullOrEmpty(Request["ExistedLogo"]))
                  {
                      Articles.Imagepath = Request["strimage"];
                  }
                  else
                  {
                      Articles.Imagepath = null;
                  }
              }

              //string strartindex = Request["ComboIndexlist"];
              string strartindex = string.Join(",", Request["ComboIndexlist"].Split(',').Distinct().ToArray());

              
            //  int createdby = 1;
              //if (Request["ddlCategory"] != "" && Request["ddlCategory"] != null)
              //{
              //    Articles.Article_ID = Convert.ToInt32(Request["ddlCategory"]);
              //}
              //else
              //{
              //    Articles.Article_ID = null;
              //}
              Articles.Article_ID = Convert.ToInt32(Request["hdnartid"]);
              Articles.Title = !string.IsNullOrEmpty(Request["txtArticleName"])? Request["txtArticleName"] : null;
              Articles.Article_Desc = System.Web.HttpUtility.HtmlDecode(Request["txtArticleDesc"] != null ? Request["txtArticleDesc"] : null);
              Articles.CreatedOn = !string.IsNullOrEmpty(Request["txt_Date"]) ? Request["txt_Date"] : DateTime.Now.ToString("MM/dd/yyyy");
              Articles.CreatedBy = "1";
              Articles.Online_ind = "Y";
              string strrelartid = !string.IsNullOrEmpty(Request["hdnfaqid"]) ? (Request["hdnfaqid"] + ",") : null;
              Articles.Related_Article_ID = !string.IsNullOrEmpty(strrelartid) ? (strrelartid.EndsWith(",,") ? strrelartid.Replace(",,", ",") : strrelartid) : null;
              string strrelvideoid = !string.IsNullOrEmpty(Request["hndmediaid"]) ? (Request["hndmediaid"] + ",") : null;
              
              Articles.Related_video_id = !string.IsNullOrEmpty(strrelvideoid) ? (strrelvideoid.EndsWith(",,") ? strrelvideoid.Replace(",,", ",") : strrelvideoid) : null;

              if (!string.IsNullOrEmpty(Request["hdnproviderid"]))
              {
                  Articles.Provider_ID = Convert.ToInt32(Request["hdnproviderid"]);
              }
              else
              {
                  Articles.Provider_ID = null;
              }


              if (!string.IsNullOrEmpty(Request["txtautherdes"]))
              {
                  Articles.Author_Desc = System.Web.HttpUtility.HtmlDecode(Request["txtautherdes"]);
              }
              else
              {
                  Articles.Author_Desc = null;
              }
              //Articles.Imagepath = ViewData["LogoPath"] == null ? null : ViewData["LogoPath"].ToString();

              //string strout = null;
              string strout = Convert.ToString(Referrals.upd_Articles(Articles));
              if (strout != null & strout.Length > 12)
              {
                  //return Json(JsonResponseFactory.ErrorResponse(strout), JsonRequestBehavior.DenyGet);
                  return RedirectToAction("EditArticle", new { Article_ID = Request["hdnartid"], lbloutmsg = "Y" });
              }
              else
              {
                  if (!string.IsNullOrEmpty(strartindex))
                  {
                  updArCategories(strartindex, Convert.ToInt32(Request["hdnartid"]));
                  }
                  
                  //return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                  return RedirectToAction("Articles");
              }
          }
          public void GetRelatedArticles(int Article_ID)
          {
              try
              {
              DataSet ArDataSet = new DataSet();
              //int i = 0;
              //int icount = 0;
              //int autoMediaID = 0;

              ArDataSet = Referrals.Get_RelatedArticleID(Article_ID);
              ViewBag.relatedfaqs = ArDataSet.Tables[0].AsEnumerable();
              ViewBag.relatedfaqcount = ArDataSet.Tables[0].Rows.Count;


              if (ArDataSet.Tables[0].Rows.Count > 0)
              {
                  string strrelatedArticle = null;
                  for (int i = 0; i <= ArDataSet.Tables[0].Rows.Count - 1; i++)
                  {
                      strrelatedArticle += ArDataSet.Tables[0].Rows[i]["RelatedArticle_Id"] + ",";
                  }
                  ViewBag.Faqid = strrelatedArticle;

              }

    //          ViewState("autoArticleID") = 0;
    //          DataColumn clmSino = new DataColumn();
    //          DataColumn clmMediaID = new DataColumn();
    //          DataColumn clmTitle = new DataColumn();
    //          DataColumn clmRelatedlinks = new DataColumn();
    //          var _with1 = clmSino;
    //          _with1.ColumnName = "autoArticleID";
    //          var _with2 = clmMediaID;
    //          _with2.ColumnName = "Article_ID";
    //          var _with3 = clmTitle;
    //          _with3.ColumnName = "Title";
    //          var _with4 = clmRelatedlinks;
    //          _with4.ColumnName = "RelatedArticle_Id";
    //          object[] colName = {
    //    clmSino,
    //    clmMediaID,
    //    clmTitle,
    //    clmRelatedlinks
    //};
    //          dsrelart = new DataTable();
    //          for (icount = 0; icount <= 3; icount++)
    //          {
    //              dsrelart.Columns.Add(colName(icount));
    //          }
    //          if ((ViewState("dsrelart") != null))
    //          {
    //              dsrelart = ViewState("dsrelart");
    //          }
    //          if ((dsmedia != null))
    //          {
    //              for (i = 0; i <= dsmedia.Tables(0).Rows.Count - 1; i++)
    //              {
    //                  autoMediaID = autoMediaID + 1;
    //                  DataRow drrow = dsrelart.NewRow;
    //                  drrow(0) = autoMediaID;
    //                  drrow(1) = dsmedia.Tables(0).Rows(i)("Article_ID");
    //                  drrow(2) = dsmedia.Tables(0).Rows(i)("Title");
    //                  drrow(3) = dsmedia.Tables(0).Rows(i)("RelatedArticle_Id");
    //                  dsrelart.Rows.Add(drrow);
    //              }
    //              ViewState("dsrelart") = dsrelart;
    //              ViewState("autoArticleID") = autoMediaID;
    //          }
    //          if (dsrelart.Rows.Count > 0)
    //          {
    //              dlrelart.DataSource = dsrelart;
    //              dlrelart.DataBind();
    //              dlrelart.Visible = true;
    //          }
    //          else
    //          {
    //              dlrelart.DataSource = null;
    //              dlrelart.DataBind();
    //              dlrelart.Visible = false;
    //          }
              //  objwcfclient.Close()
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "GetRelatedArticles", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }
          }
          public void GetRelatedvideos(int Article_ID)
          {
              try
              {
              DataSet Videodataset = new DataSet();
              //int i = 0;
              //int icount = 0;
              //int autoMediaID1 = 0;

              Videodataset = Referrals.Get_RelatedvideoID(Article_ID);
              ViewBag.relatedlinks = Videodataset.Tables[0].AsEnumerable();
              ViewBag.relatedlinkcount = Videodataset.Tables[0].Rows.Count;
              if (Videodataset.Tables[0].Rows.Count > 0)
              {
                  string strrelatedVideos = null;
                  for (int i = 0; i <= Videodataset.Tables[0].Rows.Count - 1; i++)
                  {
                      strrelatedVideos += Videodataset.Tables[0].Rows[i]["Publicvideo_id"] + ",";
                  }
                  ViewBag.mediaid = strrelatedVideos;

              }
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "GetRelatedvideos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
              }

    //          ViewState("autovideoID") = 0;
    //          DataColumn clmSino = new DataColumn();
    //          DataColumn clmMediaID = new DataColumn();
    //          DataColumn clmTitle = new DataColumn();
    //          DataColumn clmRelatedlinks = new DataColumn();
    //          var _with1 = clmSino;
    //          _with1.ColumnName = "autovideoID";
    //          var _with2 = clmMediaID;
    //          _with2.ColumnName = "publicvideo_id";
    //          var _with3 = clmTitle;
    //          _with3.ColumnName = "Title";
    //          var _with4 = clmRelatedlinks;
    //          _with4.ColumnName = "Relatedvideo_Id";
    //          object[] colName = {
    //    clmSino,
    //    clmMediaID,
    //    clmTitle,
    //    clmRelatedlinks
    //};
    //          dsrelvideo = new DataTable();
    //          for (icount = 0; icount <= 3; icount++)
    //          {
    //              dsrelvideo.Columns.Add(colName(icount));
    //          }
    //          if ((ViewState("dsrelvideo") != null))
    //          {
    //              dsrelvideo = ViewState("dsrelvideo");
    //          }
    //          if ((dsmedia1 != null))
    //          {
    //              for (i = 0; i <= dsmedia1.Tables(0).Rows.Count - 1; i++)
    //              {
    //                  autoMediaID1 = autoMediaID1 + 1;
    //                  DataRow drrow1 = dsrelvideo.NewRow;
    //                  drrow1(0) = autoMediaID1;
    //                  drrow1(1) = dsmedia1.Tables(0).Rows(i)("Publicvideo_id");
    //                  drrow1(2) = dsmedia1.Tables(0).Rows(i)("Title");
    //                  drrow1(3) = dsmedia1.Tables(0).Rows(i)("Relatedvideo_Id");
    //                  dsrelvideo.Rows.Add(drrow1);
    //              }
    //              ViewState("dsrelvideo") = dsrelvideo;
    //              ViewState("autovideoID") = autoMediaID1;
    //          }
    //          if (dsrelvideo.Rows.Count > 0)
    //          {
    //              DataList1.DataSource = dsrelvideo;
    //              DataList1.DataBind();
    //              DataList1.Visible = true;
    //          }
    //          else
    //          {
    //              DataList1.DataSource = null;
    //              DataList1.DataBind();
    //              DataList1.Visible = false;
    //          }
          }

          private bool Getfilename(HttpPostedFileBase flLogo)
          {
              try{
              string FileExtn1 = null;
              string strFilePath = null;
              FileExtn1 = System.IO.Path.GetExtension(flLogo.FileName);
              ViewData["EncryptLogo"] = clsCommonCClist.RandomPassword();
              string filename = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
              strFilePath = Path.Combine(Server.MapPath("~/Attachments/Providers"), filename);
              //ViewData["LogoPath"] = Convert.ToString(ViewData["EncryptLogo"]) + FileExtn1;
              flLogo.SaveAs(strFilePath);
              return true;
              }
              catch (Exception ex)
              {
                  var clsCustomEx = new clsExceptionLog();
                  clsCustomEx.LogException(ex, "AdminController", "Getfilename", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                  return false;
              }
              
          }
          //private string RandomPassword()
          //{
          //    return Convert.ToString(Guid.NewGuid());

          //}
           public ActionResult RelatedArticles(string faq)
           {
               ViewBag.faq = faq;
               Referrals video = new Referrals();
               video.NoofRecords = 10;
               video.PageNo = !string.IsNullOrEmpty(Request["Page"]) ? Request["Page"] : "1";
               video.OrderByItem = "Title";
               video.OrderBy = "DESC";
               //video.Category_id = 0;
               //video.ArticalIndex = 0;
               //video.FromDate = "";
               //video.Todate = "";
               video.Title = Request["txtrelarticle"];
               //video.Online_ind = "Y";
               //if (hdnarticleid.Value != null)
               //{
                   video.Related_Article_ID = null;
               //}
               //else
               //{
               //    video.Related_Article_ID = null;
               //}
                   if (faq != null & faq != "")
                   {
                       if (faq.EndsWith(","))
                       {
                           video.strexplictids = faq.Trim();
                       }
                       else
                       {
                           video.strexplictids = faq.Trim() + ",";
                       }
                   }
                   else
                   {
                       video.strexplictids = null;
                   }
                   var objlist = new List<Referrals>();
                   objlist = Referrals.Getarticles1(video);
                   ViewBag.totalrec = Referrals.TotalRecords;


               return View("RelatedArticles", objlist);
           }
           public PartialViewResult SubmitArticles(string faq)
           {
               ViewBag.faq = faq;
               Referrals video = new Referrals();
               video.NoofRecords = 10;
               video.PageNo = !string.IsNullOrEmpty(Request["Page"]) ? Request["Page"] : "1";
               video.OrderByItem = "Title";
               video.OrderBy = "DESC";
               //video.Category_id = 0;
               //video.ArticalIndex = 0;
               //video.FromDate = "";
               //video.Todate = "";
               video.Title = Request["txtrelarticle"];
               //video.Online_ind = "Y";
               //if (hdnarticleid.Value != null)
               //{
               video.Related_Article_ID = null;
               //}
               //else
               //{
               //    video.Related_Article_ID = null;
               //}
               if (faq != null & faq != "")
               {
                   if (faq.EndsWith(","))
                   {
                       video.strexplictids = faq.Trim();
                   }
                   else
                   {
                       video.strexplictids = faq.Trim() + ",";
                   }
               }
               else
               {
                   video.strexplictids = null;
               }
               var objlist = new List<Referrals>();
               objlist = Referrals.Getarticles1(video);
               ViewBag.totalrec = Referrals.TotalRecords;

               return PartialView("_SubmitArticles", objlist);
           }
           public void InsArCategories(string hdnCatIDs, int Article_ID)
           {
               try
               {
               Referrals Articlescat = new Referrals();

               
               string[] str = hdnCatIDs.Split(',');
               for (int i = 0; i <= str.Length - 1; i++)
               {

                   if (str[i] != "null")
                   {

                       Articlescat.ArticleCategory_ID = Convert.ToInt32(str[i]);
                   }
                   else
                   {
                       Articlescat.ArticleCategory_ID = 0;
                   }
                   
                   Articlescat.Article_ID = Article_ID;


                   Referrals.Ins_ArticleIndexing(Articlescat);

               }
               }
               catch (Exception ex)
               {
                   var clsCustomEx = new clsExceptionLog();
                   clsCustomEx.LogException(ex, "AdminController", "InsArCategories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               }
           }

           public void updArCategories(string hdnCatIDs, int Article_ID)
           {
               try
               {
               Referrals Articles = new Referrals();
               
               Articles.ArticleCategory_IDs = !string.IsNullOrEmpty(hdnCatIDs) ? hdnCatIDs : null;
               Articles.Article_ID = Article_ID;

               Referrals.Upd_ArticleIndexing(Articles);
               }
               catch (Exception ex)
               {
                   var clsCustomEx = new clsExceptionLog();
                   clsCustomEx.LogException(ex, "AdminController", "updArCategories", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               }
           }
           [HttpGet()]
           [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
           public ActionResult Deletearticle(int Article_ID)
           {
               //Referrals objData = new Referrals();

               Referrals.Del_Article(Article_ID);
               return RedirectToAction("Articles");
           }
        //public void Bind_catgory()
        //{

        //    List<Referrals> catlist = new List<Referrals>();
        //    Referrals objcat = new Referrals();
        //    catlist = objcat.BindCaregories();

        //    ComboBoxItemList objlistCategory = new ComboBoxItemList(catlist, "ArticleCategory_ID", "Category_Title");
        //    ViewData["ComboCategorylist"] = objlistCategory;

        //}
        public void Bind_Index(string catid)
        {
            try
            {
            List<Referrals> Indexlist = new List<Referrals>();
            Referrals objindex = new Referrals();
            objindex.ParentCategory_ID = Convert.ToInt32(catid);
            Indexlist = objindex.BindArticalIndex(objindex);

            ComboBoxItemList objlistCategory = new ComboBoxItemList(Indexlist, "ArticleCategory_ID", "Category_Title");
            ViewData["ComboIndexlist"] = objlistCategory;
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "Bind_Index", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        [HttpGet]
        public ActionResult AddNewCategory()
        {
            
                return View();
           
        }

         [HttpPost()]
          public JsonResult AddNewCategory(string obj)
          {
              
              clsCommonFunctions objcommon = new clsCommonFunctions();
            //  string ParentCatID = null;

              string strurl = Request["txtCategoryName"];
              Regex reg_exp = new Regex("[^a-zA-Z0-9_]");
              strurl = reg_exp.Replace(strurl, "-");
              if (strurl.EndsWith("-"))
              {
                  strurl = strurl.Substring(0, strurl.Length - 1);
              }

              IDataParameter[] objparam = {
	new SqlParameter("@in_Category_Title", Request["txtCategoryName"]),
	new SqlParameter("@in_ArticleUrl", strurl),
    	new SqlParameter("@ParentCategory_ID", null)
};
              IDataParameter objretparam = new SqlParameter("@in_Returnparam", SqlDbType.Int);
              objcommon.AddInParameters(objparam);
              objcommon.AddReturnParameters(objretparam);
              //string str = "";
              //str = objcommon.ExecuteStringFunction("Help_dbo.st_LOOKUP_INS_um_Article_Category");
              if (objcommon.ExecuteStringFunction("Help_dbo.st_LOOKUP_INS_um_Article_Category") == "0")
              {
                  return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
              }
              else
              {
                  return Json(JsonResponseFactory.ErrorResponse("Enter category already exist"), JsonRequestBehavior.DenyGet);
              }

          }
          public ActionResult AddNewIndex()
          {
              
                  ViewBag.Parentcatid = Request["Parentcatid"];
                  return View();
              
           }
          [HttpPost()]
          public JsonResult AddNewIndex(Publicfaq obj)
          {
              
              clsCommonFunctions objcommon = new clsCommonFunctions();
              //string ParentCatID = null;
              //ParentCatID = (Request["hdnPatcatId"]!= null && Request["hdnPatcatId"]!= null ? Request["hdnPatcatId"] : null);

              string strurl = Request["txtIndexName"];
              Regex reg_exp = new Regex("[^a-zA-Z0-9_]");
              strurl = reg_exp.Replace(strurl, "-");
              if (strurl.EndsWith("-"))
              {
                  strurl = strurl.Substring(0, strurl.Length - 1);
              }

              IDataParameter[] objparam = {
	new SqlParameter("@in_Category_Title", Request["txtIndexName"]),
	new SqlParameter("@in_ArticleUrl", strurl),
    new SqlParameter("@ParentCategory_ID", !string.IsNullOrEmpty(Request["hdnPatcatId"]) ? Request["hdnPatcatId"] : null)
};
              IDataParameter objretparam = new SqlParameter("@in_Returnparam", SqlDbType.Int);
              objcommon.AddInParameters(objparam);
              objcommon.AddReturnParameters(objretparam);
              //string str = "";
              //str = objcommon.ExecuteStringFunction("Help_dbo.st_LOOKUP_INS_um_Article_Category");
              if (objcommon.ExecuteStringFunction("Help_dbo.st_LOOKUP_INS_um_Article_Category") == "0")
              {
                  obj.parentdropdownid = !string.IsNullOrEmpty(Request["hdnPatcatId"]) ? Request["hdnPatcatId"] : null;
                  return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
              }
              else
              {
                  return Json(JsonResponseFactory.ErrorResponse("Enter index already exist"), JsonRequestBehavior.DenyGet);
              }

          }

        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult publicvideos(Articlevideos obj)
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //string startdate;
            string startdate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.Fromdate = string.IsNullOrEmpty(Request["txt_FromDate"]) ? null : Request["txt_FromDate"];
            ViewBag.Todate = string.IsNullOrEmpty(Request["txt_ToDate"]) ? null : Request["txt_ToDate"];
            ViewBag.Daterange = string.IsNullOrEmpty(Request["dt_filter"]) ? "30" : Request["dt_filter"];
            string FromDate = null;
            string ToDate = null;

            if (Request["dt_filter"] == "0" || Request["dt_filter"] == null || Request["dt_filter"] == "" || Request["dt_filter"] == "Custom")
            {
                FromDate = string.IsNullOrEmpty(Request["txt_FromDate"]) ? DateTime.Parse(startdate).AddDays(-29).ToString("MM/dd/yyyy") : Request["txt_FromDate"];
                ToDate = string.IsNullOrEmpty(Request["txt_ToDate"]) ? startdate : Request["txt_ToDate"];
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
            obj.Online_ind = null;
            obj.OrderByItem = null;
            
            obj.OrderBy = !string.IsNullOrEmpty(Request["sortdir"]) ? Request["sortdir"] : null;
            obj.NoofRecords = 10;
            
            obj.PageNo = !string.IsNullOrEmpty(Request["page"]) ? Request["page"] : "1";
            obj.Video_Description = null;
            if (!string.IsNullOrEmpty(Request["txtTitle1"]))
            {
                ViewBag.title = Request["txtTitle1"] ;
                obj.Title = Request["txtTitle1"];
            }
            else
            {
                ViewBag.title = null;
                obj.Title = null;
            }
            obj.Startdate = FromDate;
            obj.Enddate = ToDate;
            List<Articlevideos> videolist = new List<Articlevideos>();
            videolist = Articlevideos.List_videos(obj, null, null);

            ViewBag.totrec = Articlevideos.TotalRecords;
            return View(videolist);
        }
         [HttpGet()]
        public ActionResult AddNewVideo(string msg = null)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            ViewBag.outmsg = msg;
            return View();
        }
         [HttpPost()]
         [ValidateInput(false)]
         public ActionResult AddNewVideo(Articlevideos obj)
         {
             
             obj.Title = !string.IsNullOrEmpty(Request["txtTitle"]) ? Request["txtTitle"] : null;
             obj.Video_Description = !string.IsNullOrEmpty(Request["txtDescription"]) ? Request["txtDescription"] : null;
            
             string hrs = !string.IsNullOrEmpty(Request["txtHours"]) ? Request["txtHours"] : "00";
             string mins = !string.IsNullOrEmpty(Request["txtmins"]) ? Request["txtmins"] : "00";
             obj.Duration=hrs+":" +mins;
             
             obj.Online_ind = Request["chkactive"] == "Yes" ? "Y" : "N";
             
             obj.Youtube_embededtext = !string.IsNullOrEmpty(Request["txtembedtext"]) ? HttpUtility.HtmlDecode(Request["txtembedtext"]) : null;
             obj.File_Path = null;
             obj.Video_path = null;
             
             obj.ExceptVideo_Ids = !string.IsNullOrEmpty(Request["hndmediaid"]) ? Request["hndmediaid"] : null;
             obj.EncryptedFile_Path = null;
             obj.ImagePath = null;
             obj.EncryptedImagepath = null;
             obj.Createdby = Convert.ToInt32(Session["userid"]);
        var txt=     Articlevideos.Insert_videos(obj);
        if (Convert.ToString(txt) != null)
        {
            return RedirectToAction("publicvideos");
        }
        else
        {
            //string msg=null;
            string msg = Convert.ToString(Articlevideos.Out_msg);
            return RedirectToAction("AddNewVideo", new { msg = msg });
        }
            
         }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
         public ActionResult Relatedvideos(string media, string delmeid, int? parvideoid=null)
        {
            
                Articlevideos objvideo = new Articlevideos();
                if (!string.IsNullOrEmpty(media))
                {
                    if (media.EndsWith(","))
                    {
                        objvideo.ExceptVideo_Ids = media.Trim();
                    }
                    else
                    {
                        objvideo.ExceptVideo_Ids = media.Trim() + ",";
                    }
                }
                else
                {
                    objvideo.ExceptVideo_Ids = null;
                }

                objvideo.Title = null;
                objvideo.Description = null;
                if (parvideoid != null && parvideoid != 0)
                {
                    objvideo.ParentVideo_ID = parvideoid;
                }
                else
                {
                    objvideo.ParentVideo_ID = null;
                }
                objvideo.NoofRecords = 10;
                //if (Request["page"] != null)
                //{
                //    objvideo.PageNo = Request["page"];
                //}
                //else
                //{
                //    objvideo.PageNo = "1";
                //}
                objvideo.PageNo = !string.IsNullOrEmpty(Request["page"]) ? Request["page"] : "1";
                objvideo.OrderBy = null;
                objvideo.OrderByItem = null;

                if (objvideo.ParentVideo_ID != null & objvideo.ExceptVideo_Ids != null)
                {
                    if (Convert.ToString(objvideo.ExceptVideo_Ids).EndsWith(","))
                    {
                        objvideo.ExceptVideo_Ids = objvideo.ExceptVideo_Ids + objvideo.ParentVideo_ID + ",";
                    }
                    else
                    {
                        objvideo.ExceptVideo_Ids = objvideo.ExceptVideo_Ids + "," + objvideo.ParentVideo_ID + ",";
                    }
                }
                else if (objvideo.ParentVideo_ID != null & objvideo.ExceptVideo_Ids == null)
                {
                    objvideo.ExceptVideo_Ids = objvideo.ParentVideo_ID + ",";
                }
                else if (objvideo.ParentVideo_ID == null & objvideo.ExceptVideo_Ids != null)
                {
                    objvideo.ExceptVideo_Ids = objvideo.ExceptVideo_Ids;
                }
                else
                {
                    objvideo.ExceptVideo_Ids = null;
                }
                var objvideolist = Articlevideos.List_Relatedvideos(objvideo);
                ViewBag.totalrec = Articlevideos.TotalRecords;
                ViewBag.medialist = media;
                return View("Relatedvideos", objvideolist);
            
        }
        public PartialViewResult SubmitPublicVideolist(string media, string delmeid)
        {
            if (!string.IsNullOrEmpty(Request["hdnmedia"]))
            {
                media = Request["hdnmedia"];
            }
            if (!string.IsNullOrEmpty(Request["hdndelmeid"]))
            {
                delmeid = Request["hdndelmeid"];
            }
            Articlevideos objvideo = new Articlevideos();
            objvideo.NoofRecords = 10;
            
            objvideo.PageNo = !string.IsNullOrEmpty(Request["page"]) ? Request["page"] : "1";

           if (!string.IsNullOrEmpty(Request["txtTitle1"]))
            {
                ViewBag.title = Request["txtTitle1"];
                objvideo.Title = Request["txtTitle1"];
            }
            else
            {
                ViewBag.title = null;
                objvideo.Title = null;
            }
            objvideo.OrderBy = "DESC";
            objvideo.OrderByItem = "Title";
            if (media != null & media != "")
            {
                if (media.EndsWith(","))
                {
                    objvideo.ExceptVideo_Ids = media.Trim();
                }
                else
                {
                    objvideo.ExceptVideo_Ids = media.Trim() + ",";
                }
            }
            else
            {
                objvideo.ExceptVideo_Ids = null;
            }
            var objvideolist = Articlevideos.List_Relatedvideos(objvideo);
            ViewBag.totalrec1 = Articlevideos.TotalRecords;
            ViewBag.medialist = media;
            return PartialView("_SubmitPublicVideolist", objvideolist);
        }
        public ActionResult Playvideo(int id)
        {
            string youtubetext = GetProviderYouTubeEmbeddedText(id);
            ViewBag.youtube = HttpUtility.HtmlDecode(youtubetext);
            return PartialView("Playvideo", youtubetext);
        }
        public string GetProviderYouTubeEmbeddedText(int videoid)
        {
            try
            {
            clsCommonFunctions ObjCommFun = new clsCommonFunctions();
            IDataParameter[] ObjInParam = { new SqlParameter("@in_PublicVideo_ID", videoid) };
            ObjCommFun.AddInParameters(ObjInParam);
            SqlDataReader DRinfo = ObjCommFun.GetDataReader("Help_dbo.ST_get_youtubevideopath_publicvideos");
            if (DRinfo.Read())
            {
                if (DRinfo["Youtube_embededtext"].ToString() != null)
                {

                    return DRinfo["Youtube_embededtext"].ToString();

                }
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "GetProviderYouTubeEmbeddedText", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return null;
        }
        public ActionResult DeletePublicvideo(int id)
        {
              Articlevideos.Del_videos(id);
              return RedirectToAction("publicvideos");
        }
        public ActionResult EditPublicVideo(int videoid)
        {
            ViewBag.Count = 4;
            Session["CurrentUrl3"] = Request.RawUrl;
            var ds = new DataSet();
            ds = Articlevideos.Get_videos(videoid);
            ViewBag.videioid = videoid;

                ViewBag.Title = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Title"])) ? ds.Tables[0].Rows[0]["Title"].ToString() : null;
            
                ViewBag.Video_Description = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Video_Description"])) ? ds.Tables[0].Rows[0]["Video_Description"].ToString() : null;
           
                ViewBag.Online_ind = !string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Online_ind"])) ? ds.Tables[0].Rows[0]["Online_ind"].ToString() : null;
            
            if (ds.Tables[0].Rows[0]["Duration"] != null)
            {

                string dur = ds.Tables[0].Rows[0]["Duration"].ToString();
                string[] duration = dur.Split(':');
                ViewBag.hh = duration[0];
                ViewBag.mm = duration[1];
            }
            else
            {
               // ViewBag.Duration = null;
                ViewBag.hh = null;
                ViewBag.mm = null;
            }
         
            if (ds.Tables[0].Rows[0]["Youtube_embededtext"] != null)
            {
                ViewData["txtembedtext"] = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["Youtube_embededtext"].ToString());

            }
            else
            {
                ViewData["txtembedtext"] = null;
            }
            getrelatedvideos(videoid);
            //ViewBag.outmsg = msg;
             return View();
         }
        [HttpPost()]
        [ValidateInput(false)]
        public ActionResult EditPublicVideo(Articlevideos obj)
        {
            obj.PublicVideo_ID = Request["hdnvidoid"];
            
            obj.Title = !string.IsNullOrEmpty(Request["txtTitle"]) ? Request["txtTitle"] : null;
            
            obj.Video_Description = !string.IsNullOrEmpty(Request["txtDescription"]) ? Request["txtDescription"] : null;
           
            string hrs = !string.IsNullOrEmpty(Request["txtHours"]) ? Request["txtHours"] : "00";
            string mins = !string.IsNullOrEmpty(Request["txtmins"]) ? Request["txtmins"] : "00";
            obj.Duration = hrs + ":" + mins;
            
            obj.Online_ind = Request["chkactive"] == "Yes" ? "Y" : "N";
            
            obj.Youtube_embededtext = !string.IsNullOrEmpty(Request["txtembedtext"]) ? HttpUtility.HtmlDecode(Request["txtembedtext"]) : null;
            obj.File_Path = null;
            obj.Video_path = null;

            string editmedia = Request["hdneditmid"];
            string addmedia = Request["hndmediaid"];
           // string[] editmedia2 = null;
            //if (editmedia != null && addmedia != null)
            //{
            //    editmedia2 = editmedia.Split(',');
            //    for (int i = 0; i < editmedia2.Count(); i++)
            //    {
            //        if (!addmedia.EndsWith(","))
            //        {
            //            addmedia = addmedia + ",";
            //        }
            //        if (editmedia2[i] != "" && editmedia2[i] != null)
            //        {
            //            if (addmedia.Contains(editmedia2[i]))
            //            {
            //                string media123 = editmedia2[i].ToString() + ",";
            //                addmedia = addmedia.Replace(media123, null);
            //            }
            //            else
            //            {
            //            }
            //        }
            //    }
            //}   if (Request["hndmediaid"] != null && Request["hndmediaid"] != "")
            
            obj.ExceptVideo_Ids = !string.IsNullOrEmpty(Request["hndmediaid"]) ? Request["hndmediaid"] : null;
            
            obj.EncryptedFile_Path = null;
            obj.ImagePath = null;
            obj.Createdby = Convert.ToInt32(Session["userid"]);
            Articlevideos.upd_videos(obj);
            return RedirectToAction("publicvideos");
        }
        public void getrelatedvideos(int id)
        {
            try
            {
            var returnset = Articlevideos.Get_Relatedvideos(id);
            ViewBag.relatedlinks = returnset.Tables[0].AsEnumerable();
            ViewBag.relatedlinkcount = returnset.Tables[0].Rows.Count;
            if (returnset.Tables.Count > 0)
            {
                if (returnset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow objDR in returnset.Tables[0].Rows)
                    {
                        if (ViewBag.mediaid == null)
                        {
                            ViewBag.mediaid = Convert.ToString(objDR["PublicVideo_ID"]);
                        }
                        else
                        {
                            ViewBag.mediaid = ViewBag.mediaid + "," + Convert.ToString(objDR["PublicVideo_ID"]);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminController", "getrelatedvideos", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
        public JsonResult TitleList(string term)
        {
            List<string> objlist = new List<string>();
            clsCommonFunctions objcomman = new clsCommonFunctions();

            IDataParameter[] objparam = {       
		new SqlParameter("@in_Keyword", term)
	};
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.St_Publicvideos_Typeahead_Titles");
            while (drlist.Read())
            {
                objlist.Add(Convert.ToString(drlist[0]));
            }
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
