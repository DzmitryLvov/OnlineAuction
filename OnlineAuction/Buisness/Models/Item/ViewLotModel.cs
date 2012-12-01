using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Item
{
    
    public class ViewLotModel
    {
        public int ID { get; set; }

        public string OwnerName { get; set; }
      
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ActualDate { get; set; }

        public string LeaderName { get; set; }

        [BetRangeValidator("MinValue",ErrorMessage = "your bet can not be less than current currency")]
        [Display(Name = "currency")]
        public Int64 Currency { get; set; }

    }

    
}