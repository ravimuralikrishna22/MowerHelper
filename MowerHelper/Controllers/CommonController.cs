using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using MowerHelper.Models;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.Classes;
namespace MowerHelper.Controllers
{
    public class CommonController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegistrationkeyImage()
        {
            CreateKeyImage("ProviderProfile");
            return View();
        }
         [HttpGet]
        public ActionResult ProviderRegistrationkeyImage()
        {
            CreateKeyImage("NewReg_signup");
            return View();
        }
         [HttpGet]
        public ActionResult GenerateImage()
        {
               Double Width;
            Double Height;
            try {
	string str = Request["strImgPath"];
	if (Request["ind"] == "Y") {
		if (str.Contains("$") == true) {
			str = str.Replace("$", "#");
		}
		if (str.Contains("*") == true) {
			str = str.Replace("*", "&");
		}
	}
	if (!string.IsNullOrEmpty(str)) {
		if (str.Substring(1, 1) == ".") {
			str = str.Substring(3, str.Length - 3);
		}
	}

	Image imageFirst = System.Drawing.Image.FromFile(Server.MapPath(str));
	Width = Convert.ToDouble(Request["Width"]);
	Height = Convert.ToDouble(Request["Height"]);
	if (!(Width == 0) & Height == 0) {
		Height = (Width / Convert.ToDouble(imageFirst.Width)) * imageFirst.Height;
	} else if (!(Height == 0) & Width == 0) {
		Width = (Height / Convert.ToDouble(imageFirst.Height)) * imageFirst.Width;
	}
	Bitmap bitmapCanvas = new Bitmap(Convert.ToInt32(Width / 1), Convert.ToInt32(Height / 1));
	Graphics g = Graphics.FromImage(bitmapCanvas);


	g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
	g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
	g.DrawImage(imageFirst, 0, 0, bitmapCanvas.Width, bitmapCanvas.Height);
	Response.Clear();
	Response.ContentType = "image/jpeg";
	bitmapCanvas.Save(Response.OutputStream, ImageFormat.Jpeg);
} catch (Exception ex) {
    clsExceptionLog clsex = new clsExceptionLog();
    clsex.LogException(ex, "CommonController", "GenerateImage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
}

            return View();
        }
        private string GenerateKey(int length, int numberOfNonAlphanumericCharacters)
        {
            //Make sure length and numberOfNonAlphanumericCharacters are valid....
            //... checks omitted for brevity ... see live demo for full code ...
            string strReturn = null;
         try{
            while (true)
            {
                int i = 0;
                int nonANcount = 0;
                byte[] buffer1 = new byte[length];

                //chPassword contains the password's characters as it's built up
                char[] chPassword = new char[length];

                //chPunctionations contains the list of legal non-alphanumeric characters
                char[] chPunctuations = "ABCDEFGHJKLMNOPQRSTUVWXYZ".ToCharArray();

                //Get a cryptographically strong series of bytes
                System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
                rng.GetBytes(buffer1);
                for (i = 0; i <= length - 1; i++)
                {
                    //Convert each byte into its representative character
                    int rndChr = (buffer1[i] % 87);
                    if ((rndChr < 10))
                    {
                        chPassword[i] = Convert.ToChar(Convert.ToUInt16(48 + rndChr));
                    }
                    else
                    {
                        if ((rndChr < 36))
                        {
                            chPassword[i] = Convert.ToChar(Convert.ToUInt16((65 + rndChr) - 10));
                        }
                        else
                        {
                            if ((rndChr < 62))
                            {
                                chPassword[i] = Convert.ToChar(Convert.ToUInt16((97 + rndChr) - 36));
                            }
                            else
                            {
                                chPassword[i] = chPunctuations[rndChr - 62];
                                nonANcount += 1;
                            }
                        }
                    }
                }

                if (nonANcount < numberOfNonAlphanumericCharacters)
                {
                    Random rndNumber = new Random();
                    for (i = 0; i <= (numberOfNonAlphanumericCharacters - nonANcount) - 1; i++)
                    {
                        int passwordPos = 0;
                        do
                        {
                            passwordPos = rndNumber.Next(0, length);
                        } while (!char.IsLetterOrDigit(chPassword[passwordPos]));
                        chPassword[passwordPos] = chPunctuations[rndNumber.Next(0, chPunctuations.Length)];
                    }
                }
                strReturn = new string(chPassword).Replace("O", "X");
                strReturn = strReturn.Replace("l", "L");
                strReturn = strReturn.Replace("i", "J");
                strReturn = strReturn.Replace("1", "2");
                strReturn = strReturn.Replace("o", "x");
                strReturn = strReturn.Replace("0", "3");
                RandomKey = strReturn;

                return strReturn;
            }
            //return null;
         }
         catch (Exception ex)
         {
             var clsexp = new clsExceptionLog();
             clsexp.LogException(ex, "CommonController", "GenerateKey", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
             return strReturn;
         }
        }

        public string RandomKey
        {
            
            get
            {
                if (Session["Value"] != null)
                {
                    return Session["Value"].ToString();
                }
                else
                {
                    return null;
                }

            }
            set
            {
                Session.Add("Value", value);
            }
        }

        private string CreateKeyImage(string PageName = "")
        {
            try{
            Image imageFirst;
            if (PageName == "ProviderProfile")
            {
                imageFirst = System.Drawing.Image.FromFile(Server.MapPath("../images/verification.gif"));
            }
            else
            {
                imageFirst = System.Drawing.Image.FromFile(Server.MapPath("../images/regkey.jpg"));
            }
            string str = null;
            if (PageName == "ProviderProfile")
            {
                str = GenerateKey(4, 0);
            }
            else
            {
                str = GenerateKey(6, 0);
            }


            System.Drawing.StringFormat salign = new System.Drawing.StringFormat();
            string ImgTxt = RandomKey;

            Bitmap bitmapCanvas = new Bitmap(Convert.ToInt32(imageFirst.Width / 1), Convert.ToInt32(imageFirst.Height / 1));
            Graphics g = Graphics.FromImage(bitmapCanvas);
            int intImgWidth = 0;
            int intImgHieght = 0;
            g.DrawImage(imageFirst, 0, 0);
            Response.Clear();
            Response.ContentType = "image/jpeg";
            if (PageName == "ProviderProfile")
            {
                intImgWidth =(int) g.MeasureString(ImgTxt, new Font("Comic Sans MS", 9, FontStyle.Bold)).Width;
                intImgHieght = (int)g.MeasureString(ImgTxt, new Font("Comic Sans MS", 9, FontStyle.Bold)).Height;
                g.DrawString(ImgTxt, new Font("Verdana", 9, FontStyle.Regular), new SolidBrush(Color.Blue), (bitmapCanvas.Width - intImgWidth) / 2, (bitmapCanvas.Height - intImgHieght) / 2, salign);
            }
            else
            {
                intImgWidth = (int)g.MeasureString(ImgTxt, new Font("Comic Sans MS", 30, FontStyle.Bold)).Width;
                intImgHieght = (int)g.MeasureString(ImgTxt, new Font("Comic Sans MS", 30, FontStyle.Bold)).Height;
                g.DrawString(ImgTxt, new Font("Times New Roman", 30, FontStyle.Regular), new SolidBrush(Color.Blue), (bitmapCanvas.Width - intImgWidth) / 2, (bitmapCanvas.Height - intImgHieght) / 2, salign);
            }
            bitmapCanvas.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CommonController", "CreateKeyImage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
               
            }
            return null;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordManagement()
        {
            //if (Session["UserID"] == null)
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
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
            //if (string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            //{
            //    return RedirectToAction("SessionExpire", "Home");
            //}
            Reg_ProviderConfirmation objcred = new Reg_ProviderConfirmation();
            if (Convert.ToString(Session["RoleID"]) == "31" || Convert.ToString(Session["RoleID"]) == "38")
            {
                objcred = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                objcred = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]));
            }
            if (objcred != null)
            {
                if (objcred.ProviderLogin_ID != null)
                {
                    TempData["ProviderLogin_ID"]=objcred.ProviderLogin_ID;
                }
                if (objcred.UserName != null)
                {
                    TempData["UserName"]=objcred.UserName;
                }
                 if (objcred.Password != null)
                {
                    TempData["Password"] = objcred.Password;
                }
                if(objcred.Email!=null)
                {
                    ViewBag.Email=objcred.Email.ToString();
                    TempData["Email"] = objcred.Email.ToString();
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult PasswordManagement(Provider_Password obj)
        {
            if (string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["UserID"])))
            {
                return Json(JsonResponseFactory.ErrorResponse("Y"), JsonRequestBehavior.DenyGet);
            }
            VBVMD5CryptoServiceProvider objmd5 = new VBVMD5CryptoServiceProvider();
             Reg_ProviderConfirmation objcred = new Reg_ProviderConfirmation();
             if (Convert.ToString(Session["RoleID"]) == "31" || Convert.ToString(Session["RoleID"]) == "38")
             {
                 objcred = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Convert.ToInt32(Session["UserID"]));
             }
             else
             {
                 objcred = Reg_ProviderConfirmation.Reg_GetUserNameAndPassword(Session["Provider_ID"] == null ? Convert.ToInt32(Session["Prov_Id"]) : Convert.ToInt32(Session["Provider_Id"]));

             }
            if (objcred != null)
            {
                if (objcred.ProviderLogin_ID != null)
                {
                    TempData["ProviderLogin_ID"] = objcred.ProviderLogin_ID;
                }
                if (objcred.UserName != null)
                {
                    TempData["UserName"] = objcred.UserName;
                }
                if (objcred.Password != null)
                {
                    TempData["Password"] = objcred.Password;
                }
                if (objcred.Email != null)
                {
                    ViewBag.Email = objcred.Email.ToString();
                    TempData["Email"] = objcred.Email.ToString();
                }
            }
            if (Convert.ToString(TempData["Password"]).Trim() != objmd5.getMd5Hash(Convert.ToString(Request["txtOldpwd"]).Trim()))
            {
                
                return Json(JsonResponseFactory.ErrorResponse("Invalid Old Password Entry."), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string hash = null;
                hash = objmd5.getMd5Hash(Convert.ToString(Request["txtpwd"]).Trim());
                Provider_Password.InsertPassword(hash, Convert.ToInt32(Session["userid"]), Convert.ToInt32(Session["userid"]));
                    //if(Request["ChkEmail"]!=null)
                    //{
                    //    clsWebConfigsettings objconfig = new clsWebConfigsettings();
                    //    if (objconfig.GetConfigSettingsValue("SendMailFlag").ToUpper() == "YES")
                    //    {
                    //        string categorycount = Convert.ToString(Category.EmailMsgcount(14));
                    //        var objcategory = new Category
                    //        {
                    //            EmailCategoryCount = Convert.ToInt32(categorycount),
                    //            EmailCategoryID = "14",
                    //            FromReference_id = Session["Prov_Id"].ToString(),
                    //            Toreference_id = Session["Prov_Id"].ToString(),
                    //            ISWebService_IND = "N",
                    //            Application_Id = null,
                    //            FromReferenceType_id="2",
                    //            Toreferencetype_id="2"
                    //        };
                    //        Category.SentEmailLog(objcategory);
                    //        GetProviderEmailMessage();
                    //        string strUserName = TempData["UserName"].ToString();
                    //        string strPassword = null;
                    //        string strContent = null;
                    //        if (Request["txtpwd"] != "")
                    //        {
                    //            strPassword = Request["txtpwd"].ToString();
                    //        }
                    //        strContent=TempData["Content"].ToString();
                    //       if(strContent!=null)
                    //       {
                    //    if(strUserName!=null)
                    //    {
                    //        strContent = strContent.Replace("$UserName$", strUserName);
                    //    }
                    //    if(strPassword!=null)
                    //    {
                    //        strContent = strContent.Replace("$Password$", strPassword);
                    //       }

                    //       }
                            
                    //   Reg_ProviderConfirmation ObjEmail = new Reg_ProviderConfirmation();
                    //   ObjEmail.ProviderLogin_ID = Convert.ToInt32(TempData["ProviderLogin_ID"]);
                    //      //ObjEmail.Practice_ID = Convert.ToInt32(Session["Practice_ID"]);
                    //      ObjEmail.Email = Convert.ToString(TempData["Email"]);
                    //      string emailtranid = Reg_ProviderConfirmation.PasswordManage_ins_EmailMessage(ObjEmail);
                    //  string strOutMailId = objconfig.GetConfigSettingsValue("Out_Email_ID");
                    //                ClsMailMessage ObjSendMessage = new ClsMailMessage();
                    //                bool strvalid = ObjSendMessage.SendMail(Convert.ToString(TempData["Email"]), strOutMailId, TempData["Subject"].ToString(), strContent, EMailFormats.MailFormatHtml, EMailPriorities.PriorityNormal);
                    //                if (strvalid == true)
                    //                {
                    //                    clsCommonFunctions objclscommon = new clsCommonFunctions();
                    //                    IDataParameter[] strMailParam = { new SqlParameter("@In_EmailMessage_Transaction_IDs", emailtranid) };
                    //                    objclscommon.AddInParameters(strMailParam);
                    //                    objclscommon.ExecuteNonQuerySP("Help_dbo.st_EmailMessage_UPD_MessageStatus");
                    //                }
                    //        return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.DenyGet);
                    //    }
                    //}
            }
            return Json(JsonResponseFactory.SuccessResponse(obj), JsonRequestBehavior.AllowGet);
        }
        private void GetProviderEmailMessage()
        {
          try
          {
            EmailMessageOption EMO = new EmailMessageOption();
            EMO = EmailMessageOption.GET_EmailMessage_OptionBasedonID(Convert.ToInt32(13));

            if ((EMO != null))
            {
                if (EMO.Msg_Subject != null)
                {
                    TempData["Subject"] = EMO.Msg_Subject;
                }
                else
                {
                    TempData["Subject"] = null;
                }
                if (EMO.Msg_Body != null)
                {
                    TempData["Content"] = EMO.Msg_Body;
                }
                else
                {
                    TempData["Content"] = null;
                }
            }
              }
            catch (Exception ex)
            {
                var clsexp = new clsExceptionLog();
                clsexp.LogException(ex, "CommonController", "GetProviderEmailMessage", System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Session);
            }
        }
    }
}
