using PangyaAPI.BinaryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangya_GameServer.Models.Collection
{
    public class MascotCollection : List<MascotData>
    {
        public MascotCollection(int PlayerUID)
        {
            Build(PlayerUID);
        }
        // SerialMascotData
        public int MascotAdd(MascotData Value)
        {
            Value.MascotNeedUpdate = false;
            Add(Value);
            return Count;
        }

        void Build(int UID)
        {
           
        }
        public byte[] Build()
        {
            PangyaBinaryWriter Packet;

            using (Packet = new PangyaBinaryWriter())
            {
                Packet.Write(new byte[] { 0xE1, 0x00 });
                Packet.WriteByte((byte)Count);
                foreach (var Mascot in this)
                {
                    Packet.Write(Mascot.GetMascotInfo());
                }
                return Packet.GetBytes();
            }

        }
        public MascotData GetMascotByIndex(UInt32 MascotIndex)
        {
            foreach (MascotData Mascot in this)
            {
                if ((Mascot.MascotIndex == MascotIndex) && (Mascot.MascotEndDate > DateTime.MinValue))
                {
                    return Mascot;
                }
            }
            return null;
        }

        public MascotData GetMascotByTypeId(UInt32 MascotTypeId)
        {
            foreach (MascotData Mascot in this)
            {
                if ((Mascot.MascotTypeID == MascotTypeId) && (Mascot.MascotEndDate > DateTime.Now))
                {
                    return Mascot;
                }
            }
            return null;
        }

        public bool MascotExist(UInt32 TypeId)
        {
            foreach (MascotData Mascot in this)
            {
                if ((Mascot.MascotTypeID == TypeId) && (Mascot.MascotEndDate > DateTime.Now))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetSqlUpdateMascots()
        {

            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                foreach (var mascot in this)
                {
                    if (mascot.MascotNeedUpdate)
                    {
                        SQLString.Append(mascot.GetSqlUpdateString());
                        // ## set update to false when request string
                        mascot.MascotNeedUpdate = false;
                    }
                }
                return SQLString.ToString();
            }
            finally
            {

                SQLString.Clear();
            }
        }
    }
}
