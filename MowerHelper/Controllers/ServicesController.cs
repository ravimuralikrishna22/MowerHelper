using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using MowerHelper.Models.BLL.ElectricianServices;

namespace MowerHelper.Controllers
{
    public class ServicesController : Controller
    {
        //
        // GET: /Services/
        [ActionName("AdminServices")]
        public ActionResult Index()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Count = 2;
            Session["CurrentUrl1"] = Request.RawUrl;
            List<Electrician_Services> servicelist = null;
            Electrician_Services objservice = new Electrician_Services();
            objservice.NoOfRecords = 10;
            if (Request["page"] != null)
            {
                objservice.PageNo = Convert.ToInt32(Request["page"]);
            }
            else
            {
                objservice.PageNo = 1;
            }
            if (Request["sort"] != null)
            {
                objservice.OrderByItem = Request["sort"];
            }
            else
            {
                objservice.OrderByItem = null;
            }
            if (Request["sortdir"] != null)
            {
                objservice.OrderBy = Request["sortdir"];
            }
            else
            {
                objservice.OrderBy = "ASC";
            }
            int Totalnoofrecords = 0;
            servicelist = Electrician_Services.GET_ElectricianServices(objservice, ref Totalnoofrecords);
            ViewBag.totrec = Totalnoofrecords;
            ViewBag.pagesize = "10";
            return View("Index",servicelist);
        }
        [HttpGet()]
        public ActionResult AddNewService()
        {
           
                ViewBag.quantity = "1";
                return View();
           
        }
        [HttpPost()]
        public ActionResult AddNewService(string id = null)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                Electrician_Services obj = new Electrician_Services();
                obj.BillingService_ID = null;
                obj.BillingServiceTitle =Convert.ToString( Request["txtservinm"]);
                obj.BillingServiceDescription = Convert.ToString( Request["txtNotes"]);
                obj.Quantity = Convert.ToInt32(Request["txtquantity"]);
                obj.CreatedBy = 1;
                obj.StatusInd = "Y";
                obj.Chargevalues = "1," + Convert.ToString(Request["txtmnthamnt"]) + ",22,$1," + Convert.ToString(Request["txtyearlyamnt"]) + ",23,$";
                obj.IsRenewed = "N";
                obj.IsUpgradable = "N";
                // obj.Servicestring = "22," + Request["txtmnthamnt"] + ",$23," + Request["txtyearlyamnt"] + ",$";
                Electrician_Services.Insert_ElectricianServices(obj);
                return RedirectToAction("AdminServices");
            
        }

        public ActionResult Editservice(int bservid)
        {
            
                DataSet dsserdetail = new DataSet();
                DataSet dsserdetail1 = new DataSet();
               // List<Electrician_Services> servicelist = null;
                Electrician_Services objservice = new Electrician_Services();
                objservice.BillingService_ID = bservid;
                dsserdetail = objservice.showElectricianService(objservice);
                ViewBag.billserid = bservid;
                if (dsserdetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsserdetail.Tables[0].Rows)
                    {
                        if ((int)dr["BillingChargetype_ID"] == 22)
                        {
                            if (!string.IsNullOrEmpty(dr["ServiceFee"].ToString())) //!= null && dr["ServiceFee"]) != "")
                            {
                                ViewBag.mnthfee = Math.Round(Convert.ToDecimal(dr["ServiceFee"]), 2);
                            }
                            else
                            {
                                ViewBag.mnthfee = null;
                            }
                            if (!string.IsNullOrEmpty(dr["Billing_ServiceType_ID"].ToString()) )//!= null && dr["Billing_ServiceType_ID"]) != "")
                            {
                                ViewBag.servtypeid = (int)dr["Billing_ServiceType_ID"];
                            }
                            else
                            {
                                ViewBag.servtypeid = null;
                            }
                            if (!string.IsNullOrEmpty(dr["Quantity"].ToString()))// != "" && dr["Quantity"].ToString() != null)
                            {
                                ViewBag.eQuantity = dr["Quantity"].ToString();
                            }
                            else
                            {
                                ViewBag.eQuantity = null;
                            }
                        }
                        else if ((int)dr["BillingChargetype_ID"] == 23)
                        {
                            if (!string.IsNullOrEmpty(dr["ServiceFee"].ToString() ))//!= null && dr["ServiceFee"].ToString() != "")
                            {
                                ViewBag.yearlyfee = Math.Round(Convert.ToDecimal(dr["ServiceFee"]), 2);
                            }
                            else
                            {
                                ViewBag.yearlyfee = null;
                            }
                        }

                    }
                }
                dsserdetail1 = objservice.GetlectricianServiceDetails(objservice);
                if (dsserdetail1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dsserdetail1.Tables[0].Rows)
                    {

                        if (!string.IsNullOrEmpty(dr1["BillingServiceTitle"].ToString() ))//!= null && dr1["BillingServiceTitle"].ToString() != "")
                        {
                            ViewBag.servtitle = dr1["BillingServiceTitle"].ToString();
                        }
                        else
                        {
                            ViewBag.servtitle = null;
                        }
                        if (!string.IsNullOrEmpty(dr1["Servicedescription"].ToString() ))//!= null && dr1["Servicedescription"].ToString() != "")
                        {
                            ViewBag.serdescr = dr1["Servicedescription"].ToString();
                        }
                        else
                        {
                            ViewBag.serdescr = null;
                        }
                        if (!string.IsNullOrEmpty(dr1["IsRenewed"].ToString() ))//!= null && dr1["IsRenewed"].ToString() != "")
                        {
                            ViewBag.IsRenewed = dr1["IsRenewed"].ToString();
                        }
                        else
                        {
                            ViewBag.IsRenewed = null;
                        }
                        if (!string.IsNullOrEmpty(dr1["IsUpgradable"].ToString() ))//!= null && dr1["IsUpgradable"].ToString() != "")
                        {
                            ViewBag.IsUpgradable = dr1["IsUpgradable"].ToString();
                        }
                        else
                        {
                            ViewBag.IsUpgradable = null;
                        }
                    }
                }
                return View();
           
        }
        [HttpPost()]
        public ActionResult Editservice()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                Electrician_Services obj = new Electrician_Services();
                obj.BillingService_ID = Convert.ToInt32(Request["hdnbserid"]);
                obj.BillingServiceTitle = Convert.ToString( Request["etxtservinm"]);
                obj.BillingServiceDescription = Convert.ToString(Request["etxtNotes"]);
                obj.Quantity = Convert.ToInt32(Request["etxtquantity"]);
                obj.CreatedBy = 1;
                obj.StatusInd = "Y";
                obj.Chargevalues = "1," + Convert.ToString( Request["etxtmnthamnt"]) + ",22,$1," + Convert.ToString( Request["etxtyearlyamnt"]) + ",23,$";
                obj.IsRenewed = Convert.ToString( Request["hdnrenw"]);
                obj.IsUpgradable = Convert.ToString( Request["hdnupgrade"]);
                // obj.Servicestring = "22," + Request["txtmnthamnt"] + ",$23," + Request["txtyearlyamnt"] + ",$";
                Electrician_Services.Insert_ElectricianServices(obj);
                return RedirectToAction("AdminServices");
           
        }
        public ActionResult DeleteServiceInfo(int bservid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            int updaetdby = 1;
            Electrician_Services.Del_ServiceInfo(bservid, updaetdby);
            return RedirectToAction("AdminServices");
        }

    }
}
