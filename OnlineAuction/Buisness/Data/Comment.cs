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
    
    public partial class Comment
    {
        public int ID { get; set; }
        public int LotID { get; set; }
        public string CommentText { get; set; }
        public System.DateTime CommentDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public bool IsApproved { get; set; }
    
        public virtual Lot Lot { get; set; }
        public virtual User User { get; set; }
    }
}
