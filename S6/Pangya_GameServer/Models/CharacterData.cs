using Pangya_GameServer.Common;
using PangyaAPI.BinaryModels;
using System;
using System.Text;
namespace Pangya_GameServer.Models
{
    public class CharacterData
    {
        #region Public Fields
        public UInt32 TypeID { get; set; }
        public UInt32 Index { get; set; }
        public UInt32[] EquipTypeID { get; set; } = new UInt32[24];
        public UInt16 HairColour { get; set; }
        public Byte Power { get; set; }
        public Byte Control { get; set; }
        public Byte Impact { get; set; }
        public Byte Spin { get; set; }
        public Byte Curve { get; set; }
        public byte GiftFlag { get; set; }
        /// <summary>
        /// Anel da Direita
        /// </summary>
        public UInt32 AuxPartRight { get; set; }
        /// <summary>
        /// Anel da Esquerda
        /// </summary>
        public UInt32 AuxPartLeft { get; set; }
        public UInt32 CutinIndex { get; set; }

        public Byte MasteryPoint { get; set; }
        public UInt32[] EquipIndex { get; set; } = new UInt32[24];

        public bool NEEDUPDATE { get; set; }
        #endregion

        public bool UpgradeSlot(Byte Slot)
        {
            switch (Slot)
            {
                case 0:
                    this.Power += 1;
                    break;
                case 1:
                    this.Control += 1;
                    break;
                case 2:
                    this.Impact += 1;
                    break;
                case 3:
                    this.Spin += 1;
                    break;
                case 4:
                    this.Curve += 1;
                    break;
                default:
                    return false;
            }
            this.NEEDUPDATE = true;
            return true;
        }

        public bool DowngradeSlot(Byte Slot)
        {
            switch (Slot)
            {
                case 0:
                    if ((this.Power <= 0))
                    {
                        return false;
                    }
                    this.Power -= 1;
                    break;
                case 1:
                    if ((this.Control <= 0))
                    {
                        return false;
                    }
                    this.Control -= 1;
                    break;
                case 2:
                    if ((this.Impact <= 0))
                    {
                        return false;
                    }
                    this.Impact -= 1;
                    break;
                case 3:
                    if ((this.Spin <= 0))
                    {
                        return false;
                    }
                    this.Spin -= 1;
                    break;
                case 4:
                    if ((this.Curve <= 0))
                    {
                        return false;
                    }
                    this.Curve -= 1;
                    break;
            }
            this.NEEDUPDATE = true;
            return true;
        }

        public ItemUpgradeData GetPangUpgrade(byte Slot)
        {
            const uint POWPANG = 2100, CONPANG = 1700, IMPPANG = 2400, SPINPANG = 1900, CURVPANG = 1900;
            switch (Slot)
            {
                case 0:
                    return new ItemUpgradeData() { Able = true, Pang = ((this.Power * POWPANG) + POWPANG) };
                case 1:
                    return new ItemUpgradeData() { Able = true, Pang = ((this.Control * CONPANG) + CONPANG) };
                case 2:
                    return new ItemUpgradeData() { Able = true, Pang = ((this.Impact * IMPPANG) + IMPPANG) };
                case 3:
                    return new ItemUpgradeData() { Able = true, Pang = ((this.Spin * SPINPANG) + SPINPANG) };
                case 4:
                    return new ItemUpgradeData() { Able = true, Pang = ((this.Curve * CURVPANG) + CURVPANG) };

            }
            return new ItemUpgradeData() { Able = false, Pang = 0 };
        }

        public void Update(CharacterData info)
        {
            HairColour = info.HairColour;
            Power = info.Power;
            Control = info.Control;
            Impact = info.Impact;
            Spin = info.Spin;
            Curve = info.Curve;
            CutinIndex = info.CutinIndex;
            EquipTypeID = info.EquipTypeID;
            EquipIndex = info.EquipIndex;
            AuxPartRight = info.AuxPartRight;
            AuxPartLeft = info.AuxPartLeft;
            NEEDUPDATE = true;
        }

        public void SaveChar(uint UID)
        {
            StringBuilder SQLString;
            if (NEEDUPDATE)
            {
                SQLString = new StringBuilder();

                SQLString.Append('^');
                SQLString.Append(Index);
                SQLString.Append('^');
                SQLString.Append(Power);
                SQLString.Append('^');
                SQLString.Append(Control);
                SQLString.Append('^');
                SQLString.Append(Impact);
                SQLString.Append('^');
                SQLString.Append(Spin);
                SQLString.Append('^');
                SQLString.Append(Curve);
                SQLString.Append('^');
                SQLString.Append(CutinIndex);
                SQLString.Append('^');
                SQLString.Append(HairColour);
                SQLString.Append('^');
                SQLString.Append(AuxPartRight);
                SQLString.Append('^');
                SQLString.Append(AuxPartLeft);
                var Table4 = $"Exec dbo.USP_SAVE_CHARACTER_STATS  @UID = '{(int)UID}', @ITEMSTR = '{SQLString}'";
                //var _db = new PangyaEntities();

                //_db.Database.SqlQuery<PangyaEntities>(Table4).FirstOrDefault();

                //SQLString.Clear();
            }
        }

        internal string GetStringCharInfo()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                SQLString.Append('^');
                SQLString.Append(Index);
                for (int i = 0; i < 23; i++)
                {
                    SQLString.Append('^');
                    SQLString.Append(EquipTypeID[i]);
                }
                for (int i = 0; i < 23; i++)
                {
                    SQLString.Append('^');
                    SQLString.Append(EquipIndex[i]);
                }
                SQLString.Append(',');

                return SQLString.ToString();
            }
            finally
            {
                SQLString.Clear();
            }
        }

        public byte[] GetData(byte[] CardMap)
        {
            PangyaBinaryWriter Packet;

            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(TypeID);
                Packet.Write(Index);
                for (var Index = 0; Index < 24; Index++)
                {
                    Packet.Write(EquipTypeID[Index]);
                }
                Packet.WriteUInt16(HairColour);//is hair color
                Packet.WriteByte(Power);
                Packet.WriteByte(Control);
                Packet.WriteByte(Impact);
                Packet.WriteByte(Spin);
                Packet.WriteByte(Curve);
                Packet.WriteByte(15);//Mastery?
                Packet.WriteUInt32(AuxPartRight);//ring direita
                Packet.WriteUInt32(AuxPartLeft);//ring esquerda
                Packet.Write(new byte[236]);
                for (var Index = 0; Index < 24; Index++)
                {
                    Packet.Write(EquipIndex[Index]);
                }
                Packet.WriteUInt32(CutinIndex);
                Packet.WriteUInt32(0);
                Packet.WriteUInt32(0);
                return Packet.GetBytes();
            }
            finally
            {
                Packet.Dispose();
            }
        }
    }
}
