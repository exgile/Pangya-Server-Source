using Pangya_GameServer.Models.Player.Data;
using PangyaAPI.BinaryModels;
using System;
using System.Collections.Generic;

namespace Pangya_GameServer.Models.Player.Collection
{
    public class TrophyCollection : List<TrophyData>
    {
        public TrophyCollection()
        {
            Add(new TrophyData());
        }

        public byte[] GetTrophy()
        {
            var result = new PangyaBinaryWriter();

            if (Count > 0)
            {
                foreach (var trophies in this)
                {
                    result.Write(trophies.GetTrophiesInfo());
                }
            }
            else
            {
                result.Write(78);
            }
            return result.GetBytes();
        }

        public byte[] Build(byte Code)
        {
            var result = new PangyaBinaryWriter();
            result.Write(new byte[] { 0x69, 0x01, Code });
            if (Count > 0)
            {
                foreach (var trophies in this)
                {
                    result.Write(trophies.GetTrophiesInfo());
                }
            }
            else
            {
                result.Write(78);
            }
            return result.GetBytes();
        }
    }
}
