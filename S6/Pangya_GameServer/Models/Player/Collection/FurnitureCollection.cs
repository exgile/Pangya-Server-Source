using PangyaAPI.BinaryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pangya_GameServer.Models.Player.Data;
namespace Pangya_GameServer.Models.Player.Collection
{
    public class FurnitureCollection : List<FurnitureData>
    {
        public FurnitureCollection(int UID)
        {
            Build(UID);
        }
        public int FurnitureAdd(FurnitureData Value)
        {
            Value.Update = false;
            Add(Value);
            return Count;
        }

        void Build(int UID)
        {
        }

        public byte[] GetItemInfo()
        {
            var Packet = new PangyaBinaryWriter();
            Packet.Write(new byte[] { 0x2D, 0x01 });
            Packet.WriteUInt32(1);
            Packet.WriteUInt16((ushort)Count);
            foreach (var Furniture in this)
            {
                Packet.Write(Furniture.GetBytes());
            }
            return Packet.GetBytes();
        }
    }
}
