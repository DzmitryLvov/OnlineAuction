using System;
using System.ComponentModel.DataAnnotations;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    
    public class LotModel
    {
        private string _imgUrl;
        
        public int ID { get; set; }

        public int OwnerId { get; set; }
      
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ActualDate { get; set; }

        [BetRangeValidator("MinValue", ErrorMessage = "your bet can not be less than current currency")]
        [Display(Name = "currency")]
        public Int64 Currency { get; set; }

        public string ImageUrl {
            get
            {
                if (String.IsNullOrEmpty(_imgUrl))
                {
                    _imgUrl = new DataAccess().IndexImageExits(ID, "Lots")
                            ? @"Content/Image/Lots/" + ID.ToString() + @"/index.jpg"
                            : @"Content/Image/Lots/Default/index.jpg";
                }
                return _imgUrl;
            }
            set { _imgUrl = value; }
        }


    }

    
}