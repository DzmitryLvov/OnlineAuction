using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAuction.Buisness.Models.Item
{
    public class CreateLotModel
    {
        [Required]
        [StringLength(1000,ErrorMessage = "Name must have more then 2 letters",MinimumLength = 2) ]
        [DataType(DataType.Text)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "description")]
        [StringLength(1000, ErrorMessage = "Name must have more then 10 letters", MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "actual date")]
        public DateTime ActualDate { get; set; }


        [Required]
        [Display(Name = "currency")]
        public Int64 Currency { get; set; }
    }
}