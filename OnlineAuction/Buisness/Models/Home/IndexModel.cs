using System.Collections.Generic;
using System.Web.Mvc;
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
        [AllowHtml]
        public string SearchQuery { get; set; }
    }
}