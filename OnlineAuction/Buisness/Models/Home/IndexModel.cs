using System.Collections.Generic;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<ViewLotModel> Lots { get; set; }

    }
}