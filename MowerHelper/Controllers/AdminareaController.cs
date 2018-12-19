using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class AdminareaController : Controller
    {

        Hashtable htScriptID = new Hashtable();


        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult Exceptionreports()
        //{
        //    if (Request["ddlItem"] == null)
        //    {
        //        ViewBag.itemid = "1";
        //    }
        //    else
        //    {
        //        ViewBag.itemid = Request["ddlItem"];
        //    }
        //    if (Request["itemid"] != null)
        //    {
        //        ViewBag.itemid = Request["itemid"];
        //    }

        //    return View();


        //}

        public ActionResult Uploaddetails()
        {
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.name = (Request["txtName"] != null ? Request["txtName"] : null);
            //ViewBag.name1 = (Request["txtName1"] != null ? Request["txtName1"] : null);
            ViewBag.cat = (Request["ddlcategory"] != null ? Request["ddlcategory"] : "Doc");
            return View();
        }
        public ActionResult Provideruploaddetails()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            string pagesize = null;
            if (Request["ddlrecords"] != null)
            {
                ViewBag.pagesize = Request["ddlrecords"];
                Session["Rowsperpage"] = ViewBag.pagesize;
                pagesize = ViewBag.pagesize;
            }
            else if (Session["Rowsperpage"] != null)
            {
                ViewBag.pagesize = Session["Rowsperpage"].ToString();
                pagesize = ViewBag.pagesize;
            }
            else
            {
                ViewBag.pagesize = "10";
                pagesize = ViewBag.pagesize;
            }
            Provider_DocumentInfo Provider_Document = new Provider_DocumentInfo();
            Provider_Document.ProvName = (Request["txtName"] != null ? Request["txtName"] : null);
            Provider_Document.OrderBy = "ASC";
            Provider_Document.OrderBYItem = null;
            Provider_Document.NoOfRecords = Convert.ToInt32(pagesize);
            Provider_Document.PageNo = Convert.ToInt32(Request["page"] == null ? "1" : Request["page"]);
            Provider_Document.Category = (Request["ddlcategory"] != null ? Request["ddlcategory"] : "Doc");
            DataSet ds = Provider_DocumentInfo.Provider_DocumentuploadDetails(Provider_Document);
            dynamic modelList = new List<Provider_DocumentInfo>();
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new Provider_DocumentInfo();
                model.R_No = Convert.ToInt32(ds.Tables[0].Rows[i]["R_No"].ToString());
                model.ProviderName = Convert.ToString(ds.Tables[0].Rows[i]["ProviderName"]);
                model.Counts = Convert.ToInt32(ds.Tables[0].Rows[i]["Counts"].ToString());
                model.Size_KB = Convert.ToString(ds.Tables[0].Rows[i]["Size_KB"]);
                modelList.Add(model);
            }
            ViewBag.totrec = ds.Tables[1].Rows[0]["TotalRec"].ToString();
            return View(modelList);

        }
        [HttpGet()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetProviderfullnames(string term)
        {
            List<string> objlist = new List<string>();
           
            clsCommonFunctions objcomman = new clsCommonFunctions();
            IDataParameter[] objparam = { new SqlParameter("@In_KeyWord", term) };
            objcomman.AddInParameters(objparam);
            SqlDataReader drlist = default(SqlDataReader);
            drlist = objcomman.GetDataReader("Help_dbo.st_Public_Typeahead_provider_fullname");
            while (drlist.Read())
            {
                objlist.Add(drlist[0].ToString());
            }
         
            return Json(objlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        //[HttpGet()]
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        //public ActionResult Mobilestats()
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    return View();
        //}
        //public ActionResult Addressinformation()
        //{
        //    //if (Session["UserID"] == null)
        //    //{
        //    //    return RedirectToAction("SessionExpire", "Home");
        //    //}
        //    return View();
        //}
        public ActionResult Dbscripts()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            ViewBag.Count = 3;
            Session["CurrentUrl2"] = Request.RawUrl;
            string startdate;
            startdate = DateTime.Now.ToString("MM/dd/yyyy");
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
            DataSet dsObjects = PTHome.ListDBobjects(FromDate, ToDate, Request["txtObjectname"], "N");
            ViewBag.Objectname = Request["txtObjectname"];
            dynamic modelList = new List<PTHome>();
            for (int i = 0; i <= dsObjects.Tables[0].Rows.Count - 1; i++)
            {
                dynamic model = new PTHome();
                model.Objectname = Convert.ToString(dsObjects.Tables[0].Rows[i]["Objectname"]);
                model.ObjectType = Convert.ToString(dsObjects.Tables[0].Rows[i]["ObjectType"]);
                model.Description = Convert.ToString(dsObjects.Tables[0].Rows[i]["Description"]);

                model.CreatedBy = Convert.ToString(dsObjects.Tables[0].Rows[i]["CreatedBy"]);
                model.CreatedOn = Convert.ToString(dsObjects.Tables[0].Rows[i]["CreatedOn"]);
                model.modify_date = Convert.ToString(dsObjects.Tables[0].Rows[i]["modify_date"]);
                model.IsScriptGen = Convert.ToString(dsObjects.Tables[0].Rows[i]["IsScriptGen"]);
                model.DBA_Script_ID = Convert.ToString(dsObjects.Tables[0].Rows[i]["DBA_Script_ID"]);
                modelList.Add(model);
            }
            return View(modelList);
        }
        public ActionResult AddDatabaseobjects()
        {

                DataSet ds = PTHome.Fillusers("Help_dbo.st_Admin_Get_users");
                IList<SelectListItem> _result1 = new List<SelectListItem>();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {


                            _result1.Add(new SelectListItem
                              {
                                  Text = dr["Username"].ToString(),
                                  Value = dr["VBVUser_ID"].ToString()
                              });
                        }
                    }
                }
                StateCity reg1 = new StateCity();
                reg1 = new StateCity
                {
                    StateList = _result1,
                };
                return View(reg1);
            
        }
        [HttpPost]
        public ActionResult AddDatabaseobjects(string obj)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
                PTHome.InsertObjectDetails(Request["txtObject"], Request["ddltype"], Request["txtDesc"], Request["ddlusers"], (Request["txtMod"] != null & Request["txtMod"] != "mm/dd/yyyy" ? Request["txtMod"] : null));
                return RedirectToAction("Dbscripts");
            
        }
        public ActionResult Generatescript(string hid)
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            StringBuilder strBuilder = new StringBuilder();
            string strScriptIDs = "";
            string strDatabase = "";
            string strServer = "";
            string strUserName = "";
            string strPwd = "";
            //clsWebConfigsettings objconfig = new clsWebConfigsettings();
            //string strConnectionstring = clsWebConfigsettings.GetConfigSettingsValue("ConnectionString");
            string[] strParameters = clsWebConfigsettings.GetConfigSettingsValue("ConnectionString").Split(';');
            for (int i = 0; i <= strParameters.Length - 2; i++)
            {
                string[] strValues = strParameters[i].Split('=');
                if (strValues[0].ToUpper() == "SERVER")
                {
                    strServer = strValues[1];
                }
                if (strValues[0].ToUpper() == "DATABASE")
                {
                    strDatabase = strValues[1];
                }
                if (strValues[0].ToUpper() == "USER ID" || strValues[0].ToUpper() == "UID")
                {
                    strUserName = strValues[1];
                }
                if (strValues[0].ToUpper() == "PASSWORD" || strValues[0].ToUpper() == "PWD")
                {
                    strPwd = strValues[1];
                }
            }

            clsCommonFunctions objcommon = new clsCommonFunctions();
            string strScript = "";
            strScript += "exec Help_dbo.st_Admin_INS_ScriptFilexecution @In_Filename" + "\n";
            strScript += ", @In_IsSQLObject" + "\n";
            strScript += ", @In_IsSuccess" + "\n";
            strScript += ", @In_CreatedBy" + "\n";
            strScript += ", @In_VerifiedBy" + "\n";
            strScript += "GO" + "\n";
            strBuilder.Append(strScript);
            if (Request["hid"] != null)
            {
                string ScrIds = Request["hid"] + "^";
                string[] scrval = ScrIds.Split(',');
                htScriptID.Add(Convert.ToInt32(scrval[0]), ScrIds);
            }
            string strhtScriptIDs = "";
            IDictionaryEnumerator myEnumerator = htScriptID.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                strhtScriptIDs = strhtScriptIDs + myEnumerator.Value;
            }
            string[] strSplitIDs = strhtScriptIDs.Split('^');
            for (int i = 0; i <= strSplitIDs.Length - 2; i++)
            {
                string[] strValues = strSplitIDs[i].Split(',');
                string scriptID = strValues[0];
                string objecttype = strValues[2];
                Microsoft.SqlServer.Management.Smo.Server server = default(Microsoft.SqlServer.Management.Smo.Server);

                server = new Server(strServer);

                server.ConnectionContext.LoginSecure = false;
                server.ConnectionContext.Login = strUserName;
                server.ConnectionContext.Password = strPwd;

                Database myDB = server.Databases[strDatabase];

                if (objecttype == "Stored Procedure")
                {
                    StoredProcedure spt = new StoredProcedure();
                    spt = myDB.StoredProcedures[strValues[1]];

                    if ((spt != null))
                    {
                        if (spt.IsSystemObject == false)
                        {
                            string strCheck = "";
                            strCheck += "IF (OBJECT_ID('Help_dbo." + strValues[1] + "')>0)" + "\n";
                            strCheck += "BEGIN" + "\n";
                            strCheck += "DROP PROC Help_dbo." + strValues[1] + "\n";
                            strCheck += "END" + "\n";
                            strCheck += "GO" + "\n";
                            strBuilder.Append(strCheck);

                            StringCollection sc = spt.Script();
                            string str = "";
                            string objname = GetObjectname(Convert.ToString(spt.ID));
                            string s1 = null;
                            foreach (string s in sc)
                            {
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);

                                s1 = s1.Replace(objname, "Help_dbo." + objname);

                                str = str + Convert.ToString("GO" + "\n" + s1 + "\n");
                                strBuilder.Append(str);
                            }

                            string strSp = "";
                            strSp += "GO" + "\n";
                            strSp += "exec Help_dbo.DBA_INS_FileObjects 'Help_dbo." + strValues[1] + "','P'" + "\n";
                            strSp += "GO" + "\n";
                            strBuilder.Append(strSp);
                            PTHome.udpateDbscript(str, scriptID);
                            strScriptIDs += scriptID + ",";
                        }
                    }
                }
                else if (objecttype == "View")
                {
                    View spt = new View();
                    spt = myDB.Views[strValues[1]];
                    if ((spt != null))
                    {
                        if (spt.IsSystemObject == false)
                        {
                            string strCheck = "";
                            strCheck += "IF (OBJECT_ID('Help_dbo." + strValues[1] + "')>0)" + "\n";
                            strCheck += "BEGIN" + "\n";
                            strCheck += "DROP View Help_dbo." + strValues[1] + "\n";
                            strCheck += "END" + "\n";
                            strCheck += "GO" + "\n";
                            strBuilder.Append(strCheck);

                            StringCollection sc = spt.Script();

                            string str = "";
                            string objname = GetObjectname(Convert.ToString(spt.ID));
                            string s1 = null;
                            foreach (string s in sc)
                            {
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s.Replace("Help_dbo." + objname, objname);

                                s1 = s1.Replace(objname, "Help_dbo." + objname);

                                str = str + Convert.ToString("GO" + "\n" + s1 + "\n");
                                strBuilder.Append(str);
                            }



                            string strVw = "";
                            strVw += "GO" + "\n";
                            strVw += "exec Help_dbo.DBA_INS_FileObjects 'Help_dbo." + strValues[1] + "','V'" + "\n";
                            strVw += "GO" + "\n";
                            strBuilder.Append(strVw);
                            PTHome.udpateDbscript(str, scriptID);
                            strScriptIDs += scriptID + ",";
                        }

                    }
                }
                else if (objecttype == "Function")
                {
                    UserDefinedFunction spt = new UserDefinedFunction();
                    spt = myDB.UserDefinedFunctions[strValues[1]];
                    if ((spt != null))
                    {
                        if (spt.IsSystemObject == false)
                        {
                            string strCheck = "";
                            strCheck += "IF (OBJECT_ID('Help_dbo." + strValues[1] + "')>0)" + "\n";
                            strCheck += "BEGIN" + "\n";
                            strCheck += "DROP Function Help_dbo." + strValues[1] + "\n";
                            strCheck += "END" + "\n";
                            strCheck += "GO" + "\n";
                            strBuilder.Append(strCheck);

                            StringCollection sc = spt.Script();
                            string str = "";
                            string objname = GetObjectname(Convert.ToString(spt.ID));
                            string s1 = null;
                            foreach (string s in sc)
                            {
                                s1 = s.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);
                                s1 = s1.Replace("Help_dbo." + objname, objname);

                                s1 = s1.Replace(objname, "Help_dbo." + objname);

                                str = str + Convert.ToString("GO" + "\n" + s1 + "\n");
                                strBuilder.Append(str);
                            }


                            string strFn = "";
                            strFn += "GO" + "\n";
                            strFn += "exec Help_dbo.DBA_INS_FileObjects 'Help_dbo." + strValues[1] + "','FN'" + "\n";
                            strFn += "GO" + "\n";
                            strBuilder.Append(strFn);
                            PTHome.udpateDbscript(str, scriptID);
                            strScriptIDs += scriptID + ",";
                        }
                    }
                }
            }
            PTHome.upDateScriptIDs(strScriptIDs);

            string strUpd = "";
            strUpd += "GO" + "\n";
            strUpd += "exec Help_dbo.st_Admin_UPD_ScriptFilexecution" + "\n";
            strBuilder.Append(strUpd);

            if ((strBuilder != null))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=DBScriptFile.txt");
                Response.ContentType = "application/vnd.text";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                Response.Write(strBuilder.ToString());
                Response.End();
            }
            return View();
        }

        public string GetObjectname(string ObjectID)
        {
            try
            {
            return AdminProfile.GetObjectname(ObjectID);
            }
            catch (Exception ex)
            {
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "AdminareaController", "GetObjectname", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
                return null;
            }
        }


    }
}
