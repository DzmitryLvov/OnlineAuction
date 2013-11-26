using System;
using System.Collections.Generic;
using System.Drawing;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    public class LotTypePreviewModel
    {
        public string GetImageUrl(int id)
        {
            
              var  imgUrl = new DataAccess().IndexImageExits(id, "Lots")
                    ? @"Content/Image/Lots/" + id.ToString() + @"/index.jpg"
                    : @"Content/Image/Lots/Default/index.jpg";
            
            return imgUrl;
        }
       

        public string TypeName { get; set; }

        public IEnumerable<LotViewModel> LotTypePreviewCollection { get; set; }

        public LotTypePreviewModel(string typeName)
        {
            TypeName = typeName;
            LotTypePreviewCollection = new DataAccess().GetLotTypePreviewCollection(typeName);

        }
    }
}