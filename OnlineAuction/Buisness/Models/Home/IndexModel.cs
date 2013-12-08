using System.Collections.Generic;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Models.Home
{
    public class IndexModel
    {
        public IEnumerable<LotPreviewModel> Lots { get;set;}

        public IEnumerable<Category> Categories
        {
            get
            {
                return new DataAccess().GetCategoriesCollection();
            } 
        }
    }
}