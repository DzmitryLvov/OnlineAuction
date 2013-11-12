using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace OnlineAuction.Buisness.Models.Account
{
    public class RestorePasswordModel
    {
        [Required]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid email")]
        public string Email { get; set; }
    }
}