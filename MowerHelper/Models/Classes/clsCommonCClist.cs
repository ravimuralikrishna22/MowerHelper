using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.BLL.ProviderRegistration;

namespace MowerHelper.Models.Classes
{
    public static class clsCommonCClist
    {
        public static IList<SelectListItem> GetCCList()
        {
            var cardtype = new Reg_CreditCardProcess();
            List<Reg_CreditCardProcess> cardlist = cardtype.Get_Creditcardtype();
            IList<SelectListItem> cardstype = new List<SelectListItem>();
            if (cardlist.Count > 0)
            {
                for (int i = 0; i <= cardlist.Count - 1; i++)
                {
                    if (i == 0)
                    {

                        cardstype.Add(new SelectListItem
                        {
                            Value = Convert.ToString(cardlist[i].CreditCardType_ID),//cardlist[i].CreditCardType_ID .ToString(),
                            Text = "American Express",
                            Selected = true
                        });
                    }
                    else if (i == 3)
                    {

                        cardstype.Add(new SelectListItem
                        {
                            Value = Convert.ToString(cardlist[i].CreditCardType_ID),//cardlist[i].CreditCardType_ID.ToString(),
                            Text = "Visa"
                        });
                    }
                    else
                    {

                        cardstype.Add(new SelectListItem
                        {
                            Value = Convert.ToString(cardlist[i].CreditCardType_ID),//cardlist[i].CreditCardType_ID.ToString(),
                            Text = Convert.ToString(cardlist[i].code)//cardlist[i].code.ToString()
                        });
                    }
                }
            }
            return cardstype;
        }
        public static IList<SelectListItem> GetCCMonth()
        {
            int[] monthArray = null;
            monthArray = new int[12];
            for (int intVal = 0; intVal <= 11; intVal++)
            {
                monthArray[intVal] = intVal + 1;
            }
            IList<SelectListItem> month = new List<SelectListItem>();
            if (monthArray.Length > 0)
            {
                for (int i = 0; i <= monthArray.Length - 1; i++)
                {
                    if (monthArray[i] == 1)
                    {
                        month.Add(new SelectListItem
                        {
                            Value = Convert.ToString(monthArray[i]),
                            Text = Convert.ToString(monthArray[i]),
                            Selected = true
                        });
                    }
                    else
                    {
                        month.Add(new SelectListItem
                        {
                            Value = Convert.ToString(monthArray[i]),
                            Text = Convert.ToString(monthArray[i]),
                            Selected = false
                        });
                    }

                }
            }
            return month;
        }
        public static IList<SelectListItem> GetCCYear()
        {
            int intCounter;
            IList<SelectListItem> year = new List<SelectListItem>();
            for (intCounter = DateTime.Now.Year; intCounter <= DateTime.Now.Year + 12; intCounter++)
            {
                year.Add(new SelectListItem
                {
                    Value = intCounter.ToString(),
                    Text = intCounter.ToString(),
                    Selected = true
                });
            }
            return year;
        }
        public static List<clsCountry> GetStates()
        {
            List<clsCountry> objstates = clsCountry.GetStatesByCountryId(1);
            //IList<SelectListItem> result1 = new List<SelectListItem>();
            //if (objstates.Count > 0)
            //{
            //    if (objstates.Count > 100)
            //    {
            //        Parallel.ForEach(objstates, State =>
            //        {
            //            //Your stuff
            //            result1.Add(new SelectListItem
            //            {
            //                Text = State.StateFullName,
            //                Value = Convert.ToString(State.StateId)
            //            });
            //        });
            //    }
            //    else
            //    {
            //        for (int i = 0; i <= objstates.Count - 1; i++)
            //        {
            //            result1.Add(new SelectListItem
            //            {
            //                Text = objstates[i].StateFullName,
            //                Value = Convert.ToString(objstates[i].StateId)//objstates[i].StateId.ToString()
            //            });
            //        }
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
            //    StateList = objstates,
            //    //CityList = result2
            //};
            return objstates;
        }
        //public static List<clsCountry> GetC()
        //{
        //}
        public static string Getcityname(string cityid)
        {
            var clscommon = new clsCommonFunctions();
            IDataParameter[] inParmList = { new SqlParameter("@in_City_ID", cityid) };
            clscommon.AddInParameters(inParmList);
            SqlDataReader dr_GetName = clscommon.GetDataReader("Help_dbo.st_Get_CityName");
            while (dr_GetName.Read())
            {

                return Convert.ToString(dr_GetName["City"]) != null ? Convert.ToString(dr_GetName["City"]) : null;//drGetName["City"].ToString();
            }
            return null;
        }
        public static string Getstatename(string stateid)
        {
            var clscommon = new clsCommonFunctions();
            IDataParameter[] inParmList = { new SqlParameter("@in_state_id", stateid) };
            clscommon.AddInParameters(inParmList);
            SqlDataReader drGetName = clscommon.GetDataReader("Help_dbo.St_get_stateabbrevation");
            while (drGetName.Read())
            {
                return Convert.ToString(drGetName["state"]);//drGetName["state"].ToString();
            }
            return null;
        }
        public static string RandomPassword()
        {
            return Convert.ToString(Guid.NewGuid());//Guid.NewGuid().ToString();

        }
        public static string TransactionSms(int smsBodyId)
        {
            var Subject = string.Empty;
            var Content = string.Empty;
            //var objconfig = new clsWebConfigsettings();
            try
               {
                var EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(smsBodyId, 0);
                if ((EMO != null))
                {
                    Subject = EMO.Msg_Subject ?? null;
                    Content = EMO.Msg_Body ?? null;
                }
                             }
            catch (Exception ex)
            {
                var clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "clsCommonCClist", "TransactionSms", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
            return Content;
        }
        public static void TwilioSms(string strbussinessname, string strproviderphone, string strclientphone, string appatdate, string ApptTime, string PayAmount, int ddlPayment)
        {// string clientphone,
            //string appatdate,ApptTime;
            // Do stuff
            //string strbussinessname = string.Empty;
            //string strproviderphone = string.Empty;
            //string strclientphone = string.Empty;
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["PracticeName"])))
            //{
            //    strbussinessname = Convert.ToString(Session["PracticeName"]);
            //}
            //if (!string.IsNullOrEmpty(Convert.ToString(Session["Providerphone"])))
            //{
            //    strproviderphone = Convert.ToString(Session["Providerphone"]);
            //}
            //if (!string.IsNullOrEmpty(clientphone))
            //{
            //    strclientphone = clientphone;
            //    strclientphone = strclientphone.Replace("-", "");
            //}
           
            try
            {
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TwilioSms"]))
            {
                if (System.Configuration.ConfigurationManager.AppSettings["TwilioSms"].ToUpper() == "Y")
                {
                    if ((!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AccountSid"])) & (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AuthToken"])))
                    {
                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TwilosmsPhone"]))
                        {

                                //var Subject = string.Empty;
                                var Content = string.Empty;                         
                                var twilio = new Twilio.TwilioRestClient(System.Configuration.ConfigurationManager.AppSettings["AccountSid"], System.Configuration.ConfigurationManager.AppSettings["AuthToken"]);
                                
                                // string str_Content = string.Empty;
                                //if (!(string.IsNullOrEmpty(Content)))
                                //{
                                //    Content = clsCommonCClist.TransactionSms(88);
                                  
                                //    if (!(string.IsNullOrEmpty(appatdate)))
                                //    {
                                //        Content = Content.Replace("$BussinessName$", strbussinessname);
                                //        Content = Content.Replace("$AppatDate$", appatdate);
                                //        Content = Content.Replace("$ApptTime$", ApptTime);
                                //        Content = Content.Replace("$ProviderPhone$", strproviderphone);
                                //    }
                                //}
                               
                                if ((ddlPayment == 2) || (ddlPayment == 3))
                                {
                                   
                                        Content = clsCommonCClist.TransactionSms(89);
                                        if (!(string.IsNullOrEmpty(Content)))
                                        {
                                        Content = Content.Replace("$BussinessName$", strbussinessname);
                                        Content = Content.Replace("$PayAmount$", PayAmount);
                                        }
                                    //msg = "Hello from " + strbussinessname + "! Thank you for your payment of $" + PayAmount;
                                }
                                else if (ddlPayment == 1)
                                {
                                    // msg = "Hello from " + strbussinessname + "! Thank you for your business. Our service fee is $ " + PayAmount;
                                    Content = clsCommonCClist.TransactionSms(90);
                                    if (!(string.IsNullOrEmpty(Content)))
                                    {
                                        Content = Content.Replace("$BussinessName$", strbussinessname);
                                        Content = Content.Replace("$PayAmount$", PayAmount);
                                    }
                                }
                                else if (!(string.IsNullOrEmpty(appatdate)))
                                {
                                    if (!string.IsNullOrEmpty(strproviderphone))
                                    {

                                        strproviderphone = strproviderphone.Replace("-", "");
                                    }
                                    Content = clsCommonCClist.TransactionSms(88);

                                    Content = Content.Replace("$BussinessName$", strbussinessname);
                                    Content = Content.Replace("$AppatDate$", appatdate);
                                    Content = Content.Replace("$ApptTime$", ApptTime);
                                    Content = Content.Replace("$ProviderPhone$", strproviderphone);
                                }

                                //var msg = "Hello from " + strbussinessname + " This is to confirm your appointment, on " + strDates + " at " + strappointmentTime + ". If needed please call us at  " + strproviderphone + ".";
                                if (!string.IsNullOrEmpty(strclientphone))
                                {
                                    if (strclientphone == "8106095419")
                                    {
                                        strclientphone = "+91" + strclientphone;
                                    }
                                    else if (strclientphone == "8885552546")
                                    {
                                        strclientphone = "+91" + strclientphone;
                                    }
                                    else
                                    {
                                        strclientphone = "+1" + strclientphone;
                                    }

                                    // msg = !(string.IsNullOrEmpty(Convert.ToString(msg))) ? msg : Content;
                                    var message = twilio.SendMessage(System.Configuration.ConfigurationManager.AppSettings["TwilosmsPhone"], strclientphone, Content);
                                }
                           
                            
                        }
                    }
                }
            }
         }
                            catch (Exception ex)
                            {
                                var clsCustomEx = new clsExceptionLog();
                                clsCustomEx.LogException(ex, "clsCommonCClist", "TwilioSms", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                            }
        }
    }
}