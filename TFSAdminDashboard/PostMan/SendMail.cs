using System;
using System.Net.Mail;

namespace PostMan
{
    public static class SendMail
    {
        public static void SendException(Exception e)
        {
            using (SmtpClient client = new SmtpClient())
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("vincent.tollu@orange.com");

                mail.Subject = string.Format("Error in TFS Admin Console: {0}", e.Message);

                mail.Body = e.ToString();

                client.Send(mail);
            }
        }
    }
}
