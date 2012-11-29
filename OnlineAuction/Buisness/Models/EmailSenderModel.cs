using System.Net;
using System.Net.Mail;

namespace OnlineAuction.Buisness.Models
{
    public class EmailSenderModel
    {
        public static void SendEmail(string email, string title,string textMessage)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Credentials = new NetworkCredential("@gmail.com", "pass"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

            var message = new MailMessage {From = new MailAddress("@gmail.com")};
            message.To.Add(new MailAddress(email));
            message.Subject = title;
            message.Body = textMessage;
            smtp.Send(message);
        }
    }
}