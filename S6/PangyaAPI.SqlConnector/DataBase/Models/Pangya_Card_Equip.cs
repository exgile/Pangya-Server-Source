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
    
    public partial class Pangya_Card_Equip
    {
        public int ID { get; set; }
        public int UID { get; set; }
        public int CID { get; set; }
        public Nullable<int> CHAR_TYPEID { get; set; }
        public Nullable<int> CARD_TYPEID { get; set; }
        public Nullable<int> SLOT { get; set; }
        public Nullable<System.DateTime> REGDATE { get; set; }
        public Nullable<System.DateTime> ENDDATE { get; set; }
        public Nullable<byte> FLAG { get; set; }
        public Nullable<byte> VALID { get; set; }
    }
}
