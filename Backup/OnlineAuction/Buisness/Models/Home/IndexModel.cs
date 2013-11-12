using System.Collections.Generic;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<LotModel> Lots { get;set;}

        public IndexModel()
        {
            Lots =  new DataAccess().ConvertedActualLotCollection;
        }
    }
}