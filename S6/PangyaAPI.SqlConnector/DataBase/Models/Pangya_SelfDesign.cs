//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PangyaAPI.SqlConnector.DataBase.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pangya_SelfDesign
    {
        public int UID { get; set; }
        public int ITEM_ID { get; set; }
        public string UCC_UNIQE { get; set; }
        public int TYPEID { get; set; }
        public Nullable<byte> UCC_STATUS { get; set; }
        public string UCC_KEY { get; set; }
        public string UCC_NAME { get; set; }
        public Nullable<int> UCC_DRAWER { get; set; }
        public Nullable<int> UCC_COPY_COUNT { get; set; }
        public Nullable<System.DateTime> IN_DATE { get; set; }
    }
}
