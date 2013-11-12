using System;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness
{
    public class EmailSender
    {
        private const string EMAIL_ADDRESS = @"epamauction@gmail.com";
        private const string LOCAL = @"http://auction.com";

        static bool SendEamil(string email,string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.ru", 25)
                {
                    Credentials = new NetworkCredential(EMAIL_ADDRESS, "onlineauction123")
                };
            var message = new MailMessage {From = new MailAddress(EMAIL_ADDRESS)};
            message.To.Add(new MailAddress(email));
            message.Subject = subject;
            message.Body = body;
            try
            {
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendResetEmail(string email, string username, string newpass)
        {
            return SendEamil(email, "Password restoring",
                             String.Format("Hello, {0}! \r\n Here is your confirmation URL: \r\n {1}/Account/RestorePasswordConfirmation?username={0}&hash={2} ", username,LOCAL, newpass));
        }

        public static void ToLeaderOnDelete(string leaderName)
        {
            return;
        }
        public static bool ToLeaderOnChangedRate(LotModel model, string newLeader)
        {
            var membershipUser = Membership.GetUser(model.LeaderName);
            return membershipUser != null && SendEamil(membershipUser.Email, model.Name,
                                                                             String.Format(
                                                                                 "Hello, {0} \r\n Your bid on lot <a href=\"{1}/Lot/Index/{2}\"> {3} </a>, was broken by <a href=\"{1}/localhost/OnlineAuction/Account/Profile?name={4}\"> {4} </a>",
                                                                                 model.LeaderName, LOCAL, model.ID, model.Name, newLeader));
        }

        public static void ToOwnerOnDelete(string ownerName)
        {
            return;
        }
        public static void ToLeaderOnComplete()
        {
        }



        public static bool  ToOwnerOnComplete(LotModel model, string leadername)
        {
            var membershipUser = Membership.GetUser(model.OwnerName);
            return membershipUser != null && SendEamil(membershipUser.Email, model.Name,
                                                                             String.Format(
                                                                                 "Hello, {0} \r\n Your  lot {1}, was won by the user {2} by $ {4}. \r\n " +
                                                                                 "Contact them for contacts listed in the user's profile:" +
                                                                                 " {3}/OnlineAuction/Account/Profile?name={2}",
                                                                                 model.OwnerName, model.Name,leadername,LOCAL,model.Currency));
        
        }

        public static bool ToLeaderOnComplete(LotModel model)
        {
            var membershipUser = Membership.GetUser(model.LeaderName);
            return membershipUser != null && SendEamil(membershipUser.Email, model.Name,
                                                                             String.Format(
                                                                                 "Hello, {0} \r\n Congratulations? you won lot {1} by $ {2}! \r\n " +
                                                                                 "Wait until you contact the seller",
                                                                                 model.LeaderName, model.Name, model.Currency));
        }

        public static bool ToOwnerOnComplete(LotModel model) 
        {
            var membershipUser = Membership.GetUser(model.OwnerName);
            return membershipUser != null && SendEamil(membershipUser.Email, model.Name,
                                                                             String.Format(
                                                                                 "Hello, {0} \r\n Sorry, but your  lot {1}, was removed from the auction. \r\n " ,
                                                                                 model.OwnerName, model.Name));
        
        }
    }
}