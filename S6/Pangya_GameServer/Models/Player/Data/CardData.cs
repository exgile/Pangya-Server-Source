using PangyaAPI.BinaryModels;
using System;
using System.Text;
namespace Pangya_GameServer.Models.Player.Data
{
   public class CardData
    {
        public uint CardIndex { get; set; }
        public uint CardTypeID { get; set; }
        public uint CardQuantity { get; set; }
        public byte CardIsValid { get; set; }
        public bool CardNeedUpdate { get; set; }

        // Card
        public void AddQuantity(UInt32 Qty)
        {
            this.CardQuantity += Qty;
            this.CardNeedUpdate = true;
        }

        public bool RemoveQuantity(UInt32 Count)
        {
            this.CardQuantity -= Count;
            if (this.CardQuantity <= 0)
            {
                this.CardIsValid = 0;
            }
            this.CardNeedUpdate = true;
            return true;
        }

        public bool Exist(UInt32 TypeID, UInt32 Index, UInt32 Quantity)
        {
            return (CardTypeID == TypeID) && (CardIndex == Index) && (CardQuantity >= Quantity) && (CardIsValid == 1);
        }

        public byte[] GetCardInfo()
        {
            PangyaBinaryWriter Reply;

            Reply = new PangyaBinaryWriter();
            Reply.WriteUInt32(CardIndex);
            Reply.WriteUInt32(CardTypeID);
            Reply.WriteZero(12);
            Reply.WriteUInt32(CardQuantity);
            Reply.WriteZero(32);
            Reply.WriteUInt16(1);
            return Reply.GetBytes();
        }

        public string GetSqlUpdateString()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                SQLString.Append('^');
                SQLString.Append(CardIndex);
                SQLString.Append('^');
                SQLString.Append(CardQuantity);
                SQLString.Append('^');
                SQLString.Append(CardIsValid);
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
