using System;
namespace Pangya_GameServer.Common
{
    public struct GuildData
    {
        public UInt32 ID { get; set; }
        public string Name { get; set; }
        public byte Position { get; set; }
        public string Image { get; set; }
        public uint TotalMember { get; set; }
        public string Notice { get; set; }
        public string Introducing { get; set; }
        public uint LeaderUID { get; set; }
        public string LeaderNickname { get; set; }
        public DateTime? Create_Date { get; set; }
    }
}
