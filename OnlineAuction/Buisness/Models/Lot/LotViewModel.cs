using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Models.Lot
{
    
    public class LotViewModel
    {
        private string _imgUrl;
        
        public LotInfo Model { get; set; }

        public Int32 StartCurrency { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } //TODO:перенести на ajax

        [DataType(DataType.Currency)]
        public int BetValue { get; set; }

        public string CommentText { get; set; } 

        public string ImageUrl {
            get
            {
                if(Model != null)
                    if (String.IsNullOrEmpty(_imgUrl))
                    {
                        _imgUrl = new DataAccess().IndexImageExits(Model.ID, "Lots")
                            ? @"Content/Image/Lots/" + Model.ID.ToString() + @"/index.jpg"
                            : @"Content/Image/Lots/Default/index.jpg";
                    }
                    return _imgUrl;
            }
            set { _imgUrl = value; }
        }


    }

    
}