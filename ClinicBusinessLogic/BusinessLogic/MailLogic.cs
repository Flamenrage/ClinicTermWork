using ClinicBusinessLogic.HelperModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBusinessLogic.BusinessLogic
{
    public static class MailLogic
    {
        private static string smtpClientHost;
        private static int smtpClientPort;
        private static string mailLogin;
        private static string mailPassword;
        public static void MailConfig(MailConfig config)
        {
            smtpClientHost = config.SmtpClientHost;
            smtpClientPort = config.SmtpClientPort;
            mailLogin = config.MailLogin;
            mailPassword = config.MailPassword;
        }
        public static async void SendMail(MailSendInfo info)
        {
            using (var objMailMessage = new MailMessage())
            {
                using (var objSmtpClient = new SmtpClient(smtpClientHost, smtpClientPort))
                {
                    try
                    {
                        objMailMessage.From = new MailAddress(mailLogin);
                        objMailMessage.To.Add(new MailAddress(info.Email));
                        objMailMessage.Subject = info.Subject;
                        objMailMessage.Body = info.Body;
                        objMailMessage.SubjectEncoding = Encoding.UTF8;
                        objMailMessage.BodyEncoding = Encoding.UTF8;
                        objSmtpClient.UseDefaultCredentials = false;
                        objSmtpClient.EnableSsl = true;
                        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        objSmtpClient.Credentials = new NetworkCredential(mailLogin, mailPassword);
                        objMailMessage.Attachments.Add(new Attachment(info.AttachmentPath));
                        await Task.Run(() => objSmtpClient.Send(objMailMessage));
                        Debug.WriteLine("Письмо отправлено клиенту");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
