using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<Lot> Lots { get; set; }

    }
}