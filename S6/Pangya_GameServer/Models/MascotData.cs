using PangyaAPI.BinaryModels;
using System;
using System.Text;
namespace Pangya_GameServer.Models
{
    public class MascotData
    {
        public uint MascotIndex { get; set; }
        public uint MascotTypeID { get; set; }
        public string MascotMessage { get; set; }
        public DateTime MascotEndDate { get; set; }
        public ushort MascotDayToEnd { get; set; }
        public byte MascotIsValid { get; set; }
        public bool MascotNeedUpdate { get; set; }

        // Mascots
        public void AddDay(uint DayTotal)
        {
            this.MascotNeedUpdate = true;
            if ((this.MascotEndDate == DateTime.MinValue) || (this.MascotEndDate < DateTime.Now))
            {
                this.MascotEndDate = DateTime.Now.AddDays(Convert.ToDouble(DayTotal));
                return;
            }
            this.MascotEndDate = this.MascotEndDate.AddDays(Convert.ToDouble(DayTotal));

            Update();
        }

        public byte[] GetMascotInfo()
        {
            using (var Packet = new PangyaBinaryWriter())
            {
                Packet.WriteUInt32(MascotIndex);
                Packet.WriteUInt32(MascotTypeID);
                Packet.WriteZero(5);
                Packet.WriteStr(MascotMessage, 16);
                Packet.WriteZero(14);
                Packet.WriteUInt16(MascotIsValid);
                Packet.WriteTime((MascotEndDate));
                Packet.WriteByte(0);
                var result = Packet.GetBytes();
                return result;
            }
        }

        public bool Update()
        {
            this.MascotNeedUpdate = true;
            return true;
        }
        public void SetText(string Text)
        {
            this.MascotMessage = Text;
            Update();
        }

        internal string GetSqlUpdateString()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                SQLString.Append('^');
                SQLString.Append(MascotIndex);
                SQLString.Append('^');
                SQLString.Append(MascotTypeID);
                SQLString.Append('^');
                SQLString.Append(MascotMessage);
                SQLString.Append('^');
                SQLString.Append(MascotIsValid);
                SQLString.Append(',');
                // close for next player
                return SQLString.ToString();
            }
            finally
            {
                SQLString.Clear();
            }
        }
    }
}
