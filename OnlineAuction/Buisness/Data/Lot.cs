//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineAuction.Buisness.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lot
    {
        public Lot()
        {
            this.Bets = new HashSet<Bet>();
            this.Bookmarks = new HashSet<Bookmark>();
            this.Comments = new HashSet<Comment>();
            this.LotSubCategories = new HashSet<LotSubCategory>();
        }
    
        public int ID { get; set; }
        public string LotName { get; set; }
        public string Description { get; set; }
        public long StartCurrency { get; set; }
        public System.DateTime ActualDate { get; set; }
        public bool IsDeleted { get; set; }
        public int OwnerId { get; set; }
        public string ViewCount { get; set; }
        public int LotTypeID { get; set; }
    
        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LotSubCategory> LotSubCategories { get; set; }
        public virtual LotType LotType { get; set; }
        public virtual User User { get; set; }
    }
}
