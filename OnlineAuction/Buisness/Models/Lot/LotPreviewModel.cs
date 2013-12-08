using System;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class LotPreviewModel 
    {
       

        public  int ID { get; set; }

        public string LotName { get; set; }

        public string PhotoLink{ get; set;}

        public int Currency { get; set; }

        public DateTime ActualDate { get; set; }
    }
}