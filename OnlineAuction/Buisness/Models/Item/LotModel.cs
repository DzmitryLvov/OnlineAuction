using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.Buisness.Models.Item
{
    public class LotModel
    {
        

        public int ID { get; set; }

        [Required]
        [StringLength(1000,ErrorMessage = "Name must have more then 2 letters",MinimumLength = 0) ]
        [DataType(DataType.Text)]
        [Display(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "description")]

        [StringLength(1000, ErrorMessage = "Name must have more then 10 letters", MinimumLength = 0)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "actual date")]
        public DateTime ActualDate { get; set; }


        //[CustomValidation(, "Validate", "Message")]
        [DataType(DataType.Currency)]
        [Display(Name = "currency")]
        public Int64 Currency { get; set; }


    }

    public class ValAttr : ValidationAttribute
    {
        public ValAttr()
        {
        }

        public enum ValidationTypes
        {
            RangeValidator,
            Compare
        }

    }
}