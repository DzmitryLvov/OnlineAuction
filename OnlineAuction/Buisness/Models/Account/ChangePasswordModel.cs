
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineAuction.Buisness.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}