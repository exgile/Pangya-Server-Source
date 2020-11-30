using Pangya_GameServer.Models.Player.Data;
using PangyaAPI.IFF.Tools;
using System;
using System.Text;
namespace Pangya_GameServer.Models.Player
{
    /// <summary>
    /// Class Extension 
    /// </summary>
    public partial class Inventory
    {
        #region Methods"


        public uint GetTitleTypeID()
        {
            return ItemDecoration.TitleTypeID;
        }
        public uint GetCharTypeID()
        {
            CharacterData CharInfo;
            CharInfo = ItemCharacter.GetChar(CharacterIndex, 0);
            if (!(CharInfo == null))
            {
                return CharInfo.TypeID;
            }
            return 0;
        }

        public uint GetCutinIndex()
        {
            CharacterData CharInfo;
            CharInfo = ItemCharacter.GetChar(CharacterIndex, 0);
            if (!(CharInfo == null))
            {
                return CharInfo.CutinIndex;
            }
            return 0;
        }


        public uint GetMascotTypeID()
        {
            MascotData MascotInfo;
            MascotInfo = ItemMascot.GetMascotByIndex(MascotIndex);
            if (!(MascotInfo == null))
            {
                return MascotInfo.MascotTypeID;
            }
            return 0;
        }

        public uint GetQuantity(uint TypeId)
        {
            switch ((byte)IFFTools.GetItemGroup(TypeId))
            {
                case 5:
                case 6:
                    // Ball And Normal
                    return ItemWarehouse.GetQuantity(TypeId);
                default:
                    return 0;
            }
        }

        public bool SetFrameIndex(uint typeID)
        {
            var Get = ItemWarehouse.GetItem(typeID, 1);
            if (Get == null)
            {
                return false;
            }
            ItemDecoration.FrameTypeID = typeID;
            FrameIndex = Get.ItemIndex;
            return true;
        }


        internal void Remove(uint typeID, uint v1, bool v2)
        {
            throw new NotImplementedException();
        }

        internal bool IsExist(uint typeID)
        {
            throw new NotImplementedException();
        }


        public WarehouseData GetUCC(uint ItemIdx)
        {
            foreach (WarehouseData ItemUCC in ItemWarehouse)
            {
                if ((ItemUCC.ItemIndex == ItemIdx) && (ItemUCC.ItemUCCUnique.Length >= 8))
                {
                    return ItemUCC;
                }
            }
            return null;
        }

        //THIS IS USE OR UCC THAT ALREADY PAINTED
        public WarehouseData GetUCC(uint TypeId, string UCC_UNIQUE, byte Status = 1)
        {
            foreach (WarehouseData ItemUCC in ItemWarehouse)
            {
                if ((ItemUCC.ItemTypeID == TypeId) && (ItemUCC.ItemUCCUnique == UCC_UNIQUE) && (ItemUCC.ItemUCCStatus == Status))
                {
                    return ItemUCC;
                }
            }
            return null;
        }

        //THIS IS USE OR UCC THAT ALREADY {NOT PAINTED}
        public WarehouseData GetUCC(uint TypeID, string UCC_UNIQUE)
        {
            foreach (WarehouseData ItemUCC in ItemWarehouse)
            {
                if ((ItemUCC.ItemTypeID == TypeID) && (ItemUCC.ItemUCCUnique == UCC_UNIQUE) && !(ItemUCC.ItemUCCStatus == 1))
                {
                    return ItemUCC;
                }
            }
            return null;
        }

        public CharacterData GetCharacter(uint TypeID)
        {
            CharacterData Character;
            Character = ItemCharacter.GetChar(TypeID, 1);
            if (!(Character == null))
            {
                return Character;
            }
            return null;
        }

        #endregion

        public string GetSqlUpdateToolbar()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();

            SQLString.Append('^');
            SQLString.Append(CharacterIndex);
            SQLString.Append('^');
            SQLString.Append(CaddieIndex);
            SQLString.Append('^');
            SQLString.Append(MascotIndex);
            SQLString.Append('^');
            SQLString.Append(BallTypeID);
            SQLString.Append('^');
            SQLString.Append(ClubSetIndex);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot1);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot2);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot3);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot4);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot5);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot6);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot7);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot8);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot9);
            SQLString.Append('^');
            SQLString.Append(ItemSlot.Slot10);
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.BackGroundTypeID);
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.FrameTypeID);
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.StickerTypeID);
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.SlotTypeID);
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.UnknownTypeID);//is zero, for typeID unknown
            SQLString.Append('^');
            SQLString.Append(ItemDecoration.TitleTypeID);
            SQLString.Append(',');
            // close for next player
            return SQLString.ToString();
        }
    }
}
