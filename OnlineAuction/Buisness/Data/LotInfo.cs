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
    
    public partial class LotInfo
    {
        public int ID { get; set; }
        public string LotName { get; set; }
        public string Description { get; set; }
        public long StartCurrency { get; set; }
        public System.DateTime ActualDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ViewCount { get; set; }
        public Nullable<int> MaxBet { get; set; }
        public string Username { get; set; }
        public string PhotoLink { get; set; }
    }
}
