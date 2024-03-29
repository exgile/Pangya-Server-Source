﻿using PangyaAPI.BinaryModels;
namespace Pangya_GameServer.Models.Player.Data
{
   public class TrophySpecialData
    {
        public uint Index { get; set; }
        public uint TypeID { get; set; }
        public uint Quantity { get; set; }
        public byte[] GetInfo()
        {
            using (var result = new PangyaBinaryWriter())
            {
                result.Write(Index);
                result.Write(TypeID);
                result.Write(Quantity);
                return result.GetBytes();
            }
        }
    }
}
