using System.Collections.Generic;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<LotModel> Lots { get;set;}

        public IndexModel()
        {
            Lots =  Data.DataAccess.ConvertedActualLotCollection;
        }
    }
}