using System.Collections.Generic;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class LotTypePreviewModel
    {
        public string TypeName { get; set; }

        public IEnumerable<LotModel> LotTypePreviewCollection { get; set; }

        public LotTypePreviewModel(string typeName)
        {
            TypeName = typeName;
            LotTypePreviewCollection = new DataAccess().GetLotTypePreviewCollection(typeName);
        }
    }
}