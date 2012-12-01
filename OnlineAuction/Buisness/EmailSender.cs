using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness.Models
{
    public class EmailSender
    {
        private const string CURRENT_PATH = @"localhost:50441/";

        public static void SendResetEmail(MembershipUser currentUser)
        {
            /*var email = new MailMessage {From = new MailAddress("noreply@example.com")};

            email.To.Add(new MailAddress(currentUser.Email));

            email.Subject = "Password Reset";
            email.IsBodyHtml = true;
            var link = CURRENT_PATH + "Account/ResetPassword/?username=" + currentUser.UserName + "&reset=" + HashResetParams(currentUser.UserName, currentUser.ProviderUserKey.ToString());
            email.Body = "<p>" + currentUser.UserName + " please click the following link to reset your password: <a href='" + link + "'>" + link + "</a></p>";
            email.Body += "<p>If you did not request a password reset you do not need to take any action.</p>";

            var smtpClient = new SmtpClient();

            smtpClient.Send(email);*/
        }

       /* private static string HashResetParams(string userName, string toString)
        {

            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var hashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));

            return hashParams;
        }*/

        public static void SendEmailToLeader(ViewLotModel model)
        {
            throw new NotImplementedException();
        }
    }
}