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
    
    public partial class Pangya_Guild_Info
    {
        public int GUILD_INDEX { get; set; }
        public string GUILD_NAME { get; set; }
        public string GUILD_INTRODUCING { get; set; }
        public string GUILD_NOTICE { get; set; }
        public int GUILD_LEADER_UID { get; set; }
        public Nullable<int> GUILD_POINT { get; set; }
        public Nullable<int> GUILD_PANG { get; set; }
        public string GUILD_IMAGE { get; set; }
        public Nullable<int> GUILD_IMAGE_KEY_UPLOAD { get; set; }
        public Nullable<System.DateTime> GUILD_CREATE_DATE { get; set; }
        public Nullable<byte> GUILD_VALID { get; set; }
    }
}
