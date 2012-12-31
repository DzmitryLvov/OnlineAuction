using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class CatalogModel
    {
        public List<LotTypePreviewModel> TypePreviewCollections { get; set; }

        public CatalogModel()
        {
            TypePreviewCollections = new List<LotTypePreviewModel>();
            foreach (var typeName in new DataAccess().GetLotTypeList())
            {
                TypePreviewCollections.Add(new LotTypePreviewModel(typeName));
            }
        }
    }
}