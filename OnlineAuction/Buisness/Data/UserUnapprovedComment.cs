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
    
    public partial class UserUnapprovedComment
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public System.DateTime CommentDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
