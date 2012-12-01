using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace OnlineAuction.Buisness.Models.Account
{
    public class RestorePasswordModel
    {
        [Required]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        public void ResetPassword(string username)
        {
            var currentUser = Membership.GetUser(username);
            if (currentUser != null)
            {
                var password = currentUser.ResetPassword();
                EmailSender.SendResetEmail(currentUser);
            }
            else
            {
                
            }
            
        }

    }
}