using Pangya_GameServer.Common;
using Pangya_GameServer.Models.Collection;
using System;

namespace Pangya_GameServer.Models
{
    public class Inventory
    {
        #region Field

        /// <summary>
        /// Size Packet 24 bytes
        /// </summary>

        public WarehouseCollection ItemWarehouse { get; set; }
        public CaddieCollection ItemCaddie { get; set; }
        public CharacterCollection ItemCharacter { get; set; }
        public MascotCollection ItemMascot { get; set; }
        public CardCollection ItemCard { get; set; }
        public CardEquipCollection ItemCardEquip { get; set; }
        public FurnitureCollection ItemRoom { get; set; }
        //public TrophyCollection ItemTrophies { get; set; }
        //public TrophySpecialCollection ItemTrophySpecial { get; set; }
        public TransactionsCollection ItemTransaction { get; set; }
        public ItemDecorationData ItemDecoration;
        public ItemSlotData ItemSlot;
        readonly uint UID;

        public uint CharacterIndex { get; set; }
        public uint CaddieIndex { get; set; }
        public uint MascotIndex { get; set; }
        public uint BallTypeID { get; set; }
        public uint ClubSetIndex { get; set; }
        public uint CutinIndex { get; set; }
        public uint TitleIndex { get; set; }
        public uint BackGroundIndex { get; set; }
        public uint FrameIndex { get; set; }
        public uint StickerIndex { get; set; }
        public uint SlotIndex { get; set; }
        public uint Poster1 { get; set; }
        public uint Poster2 { get; set; }
        public uint TranCount
        {
            get { return ((uint)ItemTransaction.Count); }
        }


        #endregion

        #region Constructor

        Inventory()
        {

        }

        Inventory(uint ID)
        {
            UID = ID;
        }

        internal void Remove(uint typeID, uint v1, bool v2)
        {
            throw new NotImplementedException();
        }

        internal bool IsExist(uint typeID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
