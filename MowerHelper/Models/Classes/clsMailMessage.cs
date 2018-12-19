using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace MowerHelper.Models.Classes
{
    
    public enum EMailFormats
    {
        MailFormatText = 1,
        MailFormatHtml = 2
    }

    public enum EMailPriorities
    {
        PriorityNormal = 1,
        PriorityHigh = 2,
        PriorityLow = 3
    }
        public class ClsMailMessage
    {
          public string Outmsg { get; set; }

           
            readonly string _sSmtpServer;
            readonly int _sSmtpPort;
            readonly string Fromaddress;
        public ClsMailMessage()
        {
            //var objconfig = new clsWebConfigsettings();
            _sSmtpServer = clsWebConfigsettings.GetConfigSettingsValue("SMTPHOST");
            if (clsWebConfigsettings.GetConfigSettingsValue("PORT") != "")
            {
                _sSmtpPort = Convert.ToInt32(clsWebConfigsettings.GetConfigSettingsValue("PORT"));
            }
            if (_sSmtpPort == 0)
            {
                _sSmtpPort = 25;
            }
            if (!string.IsNullOrEmpty(clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID")))
            {
                Fromaddress = clsWebConfigsettings.GetConfigSettingsValue("Out_Email_ID");
            }
        }
        public bool SendMail(string sTo, string sFrom, string sSubject, string sMessage, EMailFormats iFormat = EMailFormats.MailFormatText, EMailPriorities iPriority = EMailPriorities.PriorityNormal, string sCc = "", string sBcc = "", string sAttachmentPAth = "")
        {
            bool res;
            try
            {
                if (!string.IsNullOrEmpty(sTo) && !string.IsNullOrEmpty(sFrom) && !string.IsNullOrEmpty(sSubject) && !string.IsNullOrEmpty(sMessage))
                {
                var mail = new MailMessage();
                var with1 = mail;
                if (sAttachmentPAth.Length > 0)
                {
                    foreach (string sSubstrLoopVariable in Regex.Split(sAttachmentPAth, ","))
                    {
                        string sSubstr = sSubstrLoopVariable;
                        var myAttachment = new Attachment(sSubstr);
                        with1.Attachments.Add(myAttachment);
                    }
                }
                with1.Subject = sSubject;
                with1.Body = HttpUtility.HtmlDecode(sMessage);
                mail.From = new MailAddress(Fromaddress, "Mower Helper");
                mail.ReplyTo = new MailAddress(sFrom);
                with1.To.Add(sTo);
                with1.IsBodyHtml = true;
                if (sCc.Length > 0)
                    with1.CC.Add(sCc);
                if (sBcc.Length > 0)
                    with1.Bcc.Add(sBcc);
                var smtp = new SmtpClient(_sSmtpServer, _sSmtpPort);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //var cs = new clsWebConfigsettings();
                smtp.Credentials = new System.Net.NetworkCredential(clsWebConfigsettings.GetConfigSettingsValue("SMTPUserID"), clsWebConfigsettings.GetConfigSettingsValue("SMTPPassword"));
                smtp.Send(mail);
                res = true;
            }
                else { res = false; }
            }
            catch (Exception ex)
            {
                res = false;
                clsExceptionLog clsCustomEx = new clsExceptionLog();
                clsCustomEx.LogException(ex, "EMailFormats", "SendMail", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            return res;
        }

    }
}