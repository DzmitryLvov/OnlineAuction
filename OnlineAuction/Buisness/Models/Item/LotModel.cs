using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAuction.Buisness.Models.Item
{
    public class LotModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "description")]
        public string Description { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        //[Display(Name = "actual date")]
        public DateTime ActualDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "currency")]
        public string Currency { get; set; }
    }
}