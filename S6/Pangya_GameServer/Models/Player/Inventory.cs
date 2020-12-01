using Pangya_GameServer.Common;
using Pangya_GameServer.Models.Player.Collection;
using System;
using System.Text;

namespace Pangya_GameServer.Models.Player
{
    public partial class Inventory
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
        public TrophyCollection ItemTrophies { get; set; }
        public TrophySpecialCollection ItemTrophySpecial { get; set; }
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

        public Inventory()
        {

        }

        public Inventory(uint ID)
        {
            UID = ID;
            ItemCardEquip = new CardEquipCollection((int)UID);
            ItemCharacter = new CharacterCollection((int)UID);
            ItemMascot = new MascotCollection((int)UID);
            ItemWarehouse = new WarehouseCollection((int)UID);
            ItemCaddie = new CaddieCollection((int)UID);
            ItemCard = new CardCollection((int)UID);
            ItemTransaction = new TransactionsCollection();
            ItemRoom = new FurnitureCollection((int)UID);
            ItemSlot = new ItemSlotData();
            ItemDecoration = new ItemDecorationData();
            ItemTrophies = new TrophyCollection();
            ItemTrophySpecial = new TrophySpecialCollection();
        }
        #endregion
    }
}
