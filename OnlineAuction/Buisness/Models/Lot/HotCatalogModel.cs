using System.Collections.Generic;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class HotCatalogModel
    {
        public IEnumerable<LotViewModel> Lots { get; set; }

        public HotCatalogModel()
        {
            Lots = new DataAccess().GetHotLots();
        }
    }
}