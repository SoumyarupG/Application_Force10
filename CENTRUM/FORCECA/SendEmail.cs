using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;

namespace FORCECA
{
    public class SendEmail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMail"></param>
        /// <param name="pBody"></param>
        /// <param name="pSubject"></param>
        public void SendToMailgmail(string pMail, string pBody, string pSubject, string vSendAddress, string vPass)
        {
            string vMTo = "", vBody = "";
            vMTo = pMail;
            if (vMTo != "")
            {
                vBody = pBody;
                MailMessage oM = new MailMessage();
                oM.To.Add(vMTo);            
                oM.From = new System.Net.Mail.MailAddress(vSendAddress);
                oM.Subject = pSubject;
                oM.Body = vBody;
                //oM.CC.Add("dayitmitra@gmail.com");
                //oM.Attachments.Add(new Attachment(vFileNm));
                //oM.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);

                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(vSendAddress, vPass);
                smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = true;

                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 360000;
                smtp.Send(oM);
                smtp.Dispose();
                oM.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMail"></param>
        /// <param name="pBody"></param>
        /// <param name="pSubject"></param>
        public  void SendToMail(string pMail, string pBody, string pSubject)
        {            
            try
            {
                if (emailIsValid(pMail) == true)
                {
                    string emailid = "jobs.uttrayan@gmail.com";
                    string pwd = "*#hrjob!";
                    MailMessage oM = new MailMessage();
                    oM.To.Add(pMail);
                    oM.From = new MailAddress(emailid);                  
                    oM.Subject = pSubject;
                    oM.Body = pBody;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential(emailid, pwd);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 360000;
                    smtp.Send(oM);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void SendToMailHtml(string pMail, string pBody, string pSubject)
        {
            try
            {
                if (emailIsValid(pMail) == true)
                {
                    string emailid = "jobs.uttrayan@gmail.com";
                    string pwd = "*#hrjob!";
                    MailMessage oM = new MailMessage();
                    oM.To.Add(pMail);
                    oM.From = new MailAddress(emailid);
                    oM.IsBodyHtml = true;
                    oM.Subject = pSubject;
                    oM.Body = pBody;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential(emailid, pwd);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 360000;
                    smtp.Send(oM);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //checked valid mail

        public static bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMail"></param>
        /// <param name="pBody"></param>
        /// <param name="pSubject"></param>
        public void SendToMailOffice360(string pMail, string pBody, string pSubject, string vSendAddress, string vPass)
        {
            string vMTo = "", vBody = "";
            vMTo = pMail;
            if (vMTo != "")
            {
                vBody = pBody;
                //MailMessage oM = new MailMessage();
                //oM.To.Add(vMTo);
                //oM.From = new System.Net.Mail.MailAddress(vSendAddress);
                //oM.Subject = pSubject;
                //oM.Body = vBody;

                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.office365.com", 587);
                //smtp.Host = "smtp.office365.com";
                //smtp.Credentials = new System.Net.NetworkCredential(vSendAddress, vPass);
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = Credentials;  
                //smtp.Timeout = 360000;
                //smtp.Send(oM);
                //smtp.Dispose();
                //oM.Dispose();
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                MailMessage message = new MailMessage(vSendAddress, vMTo);
                string mailbody = vBody;
                message.Subject = pSubject;
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(vSendAddress, vPass);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;              
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    throw ex;
                } 
            }
        }
    }
}
