using Pangya_GameServer.Common;
using Pangya_GameServer.Models.Player.Data;
using PangyaAPI.BinaryModels;
using PangyaAPI.IFF;
using PangyaAPI.IFF.Flags;
using PangyaAPI.IFF.Tools;
using PangyaAPI.SqlConnector.DataBase;
using PangyaAPI.SqlConnector.Tools;
using PangyaAPI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Pangya_GameServer.Models.Player
{
    /// <summary>
    /// Class Extension 
    /// </summary>
    public partial class Inventory
    {
        #region Methods Array of Byte
        public byte[] GetToolbar()
        {
            PangyaBinaryWriter Reply;

            Reply = new PangyaBinaryWriter();

            Reply.Write(new byte[] { 0x72, 0x00 });
            Reply.Write(GetEquipData());
            return Reply.GetBytes();
        }
        public byte[] GetCharData()
        {
            return ItemCharacter.GetCharData(CharacterIndex);
        }
        public byte[] GetCharData(uint Index)
        {
            return ItemCharacter.GetCharData(Index);
        }
        /// <summary>
        /// Size Packet 62 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetMascotData()
        {
           var MascotInfo = ItemMascot.GetMascotByIndex(MascotIndex);
            if ((MascotInfo != null))
            {
                return MascotInfo.GetMascotInfo();
            }
            return new byte[0x3E];
        }
        /// <summary>
        /// return 78 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetTrophyInfo()
        {
            return ItemTrophies.GetTrophy();
        }
        /// <summary>
        /// GetSize 116 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetEquipData()
        {
            var result = new PangyaBinaryWriter();

            result.WriteUInt32(CaddieIndex);
            result.WriteUInt32(1); //CharacterIndex
            result.WriteUInt32(ClubSetIndex);
            result.WriteUInt32(BallTypeID);//16
            result.Write(ItemSlot.GetItemSlot());//56
            result.WriteUInt32(BackGroundIndex);
            result.WriteUInt32(FrameIndex);
            result.WriteUInt32(StickerIndex);
            result.WriteUInt32(SlotIndex);
            result.WriteUInt32(0);//UNKNOWN, value = 0
            result.WriteUInt32(TitleIndex);
            result.WriteStruct(ItemDecoration);//104
            result.WriteUInt32(MascotIndex);
            result.WriteUInt32(Poster1);
            result.WriteUInt32(Poster2);//116
            return result.GetBytes();
        }
        /// <summary>
        /// Size Packet 634 
        /// </summary>
        /// <returns>Select(634 array of byte)</returns>
        public byte[] GetEquipInfo()
        {
            var Response = new PangyaBinaryWriter();
            Response.Write(GetCharData());
            Response.Write(GetCaddieData());
            Response.Write(GetClubData());
            Response.Write(GetMascotData());
            return Response.GetBytes();
        }
        /// <summary>
        /// Size Packet 28 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetClubData()
        {
            var ClubInfo = ItemWarehouse.GetItem(this.ClubSetIndex);
            if ((ClubInfo == null))
            {
                return new byte[28];
            }
            return ClubInfo.GetClubInfo();
        }
        /// <summary>
        /// Size Packet 25 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetCaddieData()
        {
           var CaddieInfo = ItemCaddie.GetCaddieByIndex(CaddieIndex);
            if (!(CaddieInfo == null))
            {
                return CaddieInfo.GetCaddieInfo();
            }
            return new byte[0x19];
        }
        // transaction
        public byte[] GetTransaction()
        {
            return ItemTransaction.GetTran();
        }

        public byte[] GetDecorationData()
        {
            using (var result = new PangyaBinaryWriter())
            {
                result.WriteStruct(ItemDecoration);
                return result.GetBytes();
            }
        }
        public byte[] GetGolfEQP()
        {
            using (var Packet = new PangyaBinaryWriter())
            {
                Packet.WriteUInt32(BallTypeID);
                Packet.WriteUInt32(ClubSetIndex);
                return Packet.GetBytes();
            }
        }

        #endregion

        #region Methods Bool
        // poster


        public bool SetCutInIndex(uint CharIndex, uint CutinIndex)
        {
            if (CutinIndex == 0)
            {
                return true;
            }
            var Item = ItemWarehouse.GetItem(CutinIndex, 0);
            var CharType = ItemCharacter.GetChar(CharIndex,0);
            if (Item == null)
            {
                return false;
            }
            if (CharType == null)
            {
                return false;
            }
            CharType.CutinIndex = Item.ItemIndex;
            ItemCharacter.UpdateCharacter(CharType);
            return true;
        }
        public bool SetPoster(uint Poster1, uint Poster2)
        {
            this.Poster1 = Poster1;
            this.Poster2 = Poster2;
            return true;
        }

        public bool IsExist(uint TypeID, uint Index, uint Quantity)
        {
            switch ((byte)TypeID.GetItemGroup())
            {
                case 5:
                case 6:
                    // ## normal and ball
                    return ItemWarehouse.IsNormalExist(TypeID, Index, Quantity);
                case 2:
                    // ## part
                    return ItemWarehouse.IsPartExist(TypeID, Index, Quantity);
                case 0x1:
                    // ## card
                    return ItemCard.IsExist(TypeID, Index, Quantity);
            }
            return false;
        }

        // item exists?
        public bool IsExist(uint TypeId)
        {
            List<Dictionary<uint, uint>> ListSet;
            switch ((byte)TypeId.GetItemGroup())
            {
                case 2:
                    return ItemWarehouse.IsPartExist(TypeId);
                case 5:
                case 6:
                    return ItemWarehouse.IsNormalExist(TypeId);
                case 9:
                    ListSet = IFFEntry.GetIff.SetItem.SetList(TypeId);
                    try
                    {
                        if (ListSet.Count <= 0)
                        {
                            return false;
                        }
                        foreach (var __Enum in ListSet)
                        {
                            if (this.IsExist(__Enum.First().Key))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    finally
                    {
                        ListSet.Clear();
                    }
                case 14:
                    return ItemWarehouse.IsSkinExist(TypeId);
            }
            return false;
        }

        public bool Available(uint TypeID, uint Quantity)
        {

            var ListSet = IFFEntry.GetIff.SetItem.SetList(TypeID);

            switch (TypeID.GetItemGroup())
            {
                case IffGroupFlag.ITEM_TYPE_SETITEM:
                    {
                        if (ListSet.Count <= 0)
                        { return false; }

                        else
                        {
                            foreach (var data in ListSet)
                            {
                                Available(data.Keys.FirstOrDefault(), data.Values.FirstOrDefault());
                            }
                            return true;
                        }
                    }
                case IffGroupFlag.ITEM_TYPE_CHARACTER:
                    {
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_HAIR_STYLE:
                    {
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_PART:
                    {
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_CLUB:
                    {
                        if (ItemWarehouse.IsClubExist(TypeID))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case IffGroupFlag.ITEM_TYPE_AUX:
                case IffGroupFlag.ITEM_TYPE_BALL:
                case IffGroupFlag.ITEM_TYPE_USE:
                    {
                        if (GetQuantity(TypeID) + Quantity > 32767)
                        {
                            return false;
                        }
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_CADDIE:
                    {
                        if (ItemCaddie.IsExist(TypeID))
                        {
                            return false;
                        }
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_CADDIE_ITEM:
                    {
                        if (ItemCaddie.CanHaveSkin(TypeID))
                        {
                            return true;
                        }
                        return false;
                    }
                case IffGroupFlag.ITEM_TYPE_SKIN:
                    {
                        if (ItemWarehouse.IsSkinExist(TypeID))
                        {
                        }
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_MASCOT:
                    {
                        return true;
                    }
                case IffGroupFlag.ITEM_TYPE_CARD:
                    {
                        return true;
                    }

            }
            return false;
        }

        public bool SetMascotText(uint MascotIdx, string MascotText)
        {
          var  Mascot = ItemMascot.GetMascotByIndex(MascotIdx);
            if (!(Mascot == null))
            {
                Mascot.SetText(MascotText);
                return true;
            }
            return false;
        }
        // caddie system
        public bool SetCaddieIndex(uint Index)
        {
            if (Index == 0)
            {
                CaddieIndex = 0;
                return true;
            }
           var Caddie = ItemCaddie.GetCaddieByIndex(Index);
            if (Caddie == null)
            {
                return false;
            }
            CaddieIndex = Caddie.CaddieIdx;
            return true;
        }

        // mascot system
        public bool SetMascotIndex(uint Index)
        {
            if (Index == 0)
            {
                MascotIndex = 0;
                return true;
            }
           var Mascot = ItemMascot.GetMascotByIndex(Index);
            if (Mascot == null)
            {
                return false;
            }
            MascotIndex = Mascot.MascotIndex;
            return true;
        }

        public bool SetCharIndex(uint CharID)
        {
            var Char = ItemCharacter.GetChar(CharID, 0);
            if (Char == null)
            {
                return false;
            }
            CharacterIndex = CharID;
            return true;
        }

        public bool SetBackgroudIndex(uint typeID)
        {
            var Get = ItemWarehouse.GetItem(typeID, 1);
            if (Get == null)
            {
                return false;
            }
            ItemDecoration.BackGroundTypeID = typeID;
            BackGroundIndex = Get.ItemIndex;
            return true;
        }



        public bool SetStickerIndex(uint typeID)
        {
            var Get = ItemWarehouse.GetItem(typeID, 1);
            if (Get == null)
            {
                return false;
            }
            ItemDecoration.StickerTypeID = typeID;
            StickerIndex = Get.ItemIndex;
            return true;
        }
        public bool SetSlotIndex(uint typeID)
        {
            var Get = ItemWarehouse.GetItem(typeID, 1);
            if (Get == null)
            {
                return false;
            }
            ItemDecoration.SlotTypeID = typeID;
            SlotIndex = Get.ItemIndex;
            return true;
        }

        public bool SetTitleIndex(uint ID)
        {
            var Get = ItemWarehouse.GetItem(ID);
            if (Get == null)
            {
                return false;
            }
            ItemDecoration.TitleTypeID = Get.ItemTypeID;
            TitleIndex = Get.ItemIndex;
            return true;
        }

        public bool SetDecoration(uint background, uint frame, uint sticker, uint slot, uint un, uint title)
        {
            if (SetBackgroudIndex(background) || SetFrameIndex(frame) || SetStickerIndex(sticker) || SetSlotIndex(slot) || SetTitleIndex(title))
            {
                ItemDecoration.UnknownTypeID = un;
                return true;
            }
            return false;
        }

        // club system
        public bool SetClubSetIndex(uint Index)
        {
           var Club = ItemWarehouse.GetItem(Index);
            if ((Club == null) || (!(Club.ItemTypeID.GetItemGroup() == IffGroupFlag.ITEM_TYPE_CLUB)))
            {
                return false;
            }
            this.ClubSetIndex = Index;
            return true;
        }

        public bool SetGolfEQP(uint BallTypeID, uint ClubSetIndex)
        {
            return (this.SetBallTypeID(BallTypeID) || this.SetClubSetIndex(ClubSetIndex));
        }

        public bool SetBallTypeID(uint TypeID)
        {
            var Ball = ItemWarehouse.GetItem(TypeID, 1);
            if ((Ball == null) || (!(Ball.ItemTypeID.GetItemGroup() == IffGroupFlag.ITEM_TYPE_BALL)))
            {
                return false;
            }
            this.BallTypeID = TypeID;
            return true;
        }

        #endregion

        #region Methods UInt32



        public uint GetTitleTypeID()
        {
            return ItemDecoration.TitleTypeID;
        }
        public uint GetCharTypeID()
        {
           var CharInfo = ItemCharacter.GetChar(CharacterIndex,0);
            if (!(CharInfo == null))
            {
                return CharInfo.TypeID;
            }
            return 0;
        }

        public uint GetCutinIndex()
        {
            var CharInfo = ItemCharacter.GetChar(CharacterIndex,0);
            if (!(CharInfo == null))
            {
                return CharInfo.CutinIndex;
            }
            return 0;
        }


        public uint GetMascotTypeID()
        {
            var MascotInfo = ItemMascot.GetMascotByIndex(MascotIndex);
            if (!(MascotInfo == null))
            {
                return MascotInfo.MascotTypeID;
            }
            return 0;
        }

        public uint GetQuantity(uint TypeId)
        {
            switch ((byte)TypeId.GetItemGroup())
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
        #endregion

        #region Methods GetItem
        public WarehouseData GetUCC(uint ItemIdx)
        {
            foreach (var ItemUCC in ItemWarehouse)
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
            foreach (var ItemUCC in ItemWarehouse)
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
            foreach (var ItemUCC in ItemWarehouse)
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
           var Character = ItemCharacter.GetChar(TypeID, 1);
            if (!(Character == null))
            {
                return Character;
            }
            return null;
        }
        #endregion

        #region Methods AddItems
        public AddItemData AddItem(AddItem ItemAddItemData)
        {
            Object WareHouseData;
            AddItemData Result;

            Result = new AddItemData() { Status = false };


            if (UID == 0)
            {
                return Result;
            }
            switch (ItemAddItemData.ItemIffId.GetItemGroup())
            {
                case IffGroupFlag.ITEM_TYPE_CHARACTER:
                    {
                        WareHouseData = ItemCharacter.GetChar(ItemAddItemData.ItemIffId, 1);

                        if (WareHouseData == null)
                        {
                            return AddItemToDB(ItemAddItemData);
                        }

                        else if (!(WareHouseData != null))
                        {
                            Result.Status = true;
                            Result.ItemIndex = ((CharacterData)(WareHouseData)).Index;
                            Result.ItemTypeID = ((CharacterData)(WareHouseData)).TypeID;
                            Result.ItemOldQty = 1;
                            Result.ItemNewQty = 1;
                            Result.ItemUCCKey = string.Empty;
                            Result.ItemFlag = 0;
                            Result.ItemEndDate = null;

                            if (ItemAddItemData.Transaction)
                                ItemTransaction.AddChar(2, (CharacterData)WareHouseData);
                        }
                    }
                    break;
                case IffGroupFlag.ITEM_TYPE_HAIR_STYLE:
                    {
                        var IffHair = IFFEntry.GetIff.GetByHairColor(ItemAddItemData.ItemIffId);
                        var character = ItemCharacter.GetCharByType((byte)IffHair.CharType);
                        if (character != null)
                        {
                            character.HairColour = IffHair.HairColor;
                            character.Update(character);
                            Result.Status = true;
                            Result.ItemIndex = character.Index;
                            Result.ItemTypeID = ItemAddItemData.ItemIffId;
                            Result.ItemOldQty = 0;
                            Result.ItemNewQty = 1;
                            Result.ItemUCCKey = null;
                            Result.ItemFlag = 0;
                            Result.ItemEndDate = null;
                        }
                    }
                    break;
                case IffGroupFlag.ITEM_TYPE_PART:
                    {
                        return AddItemToDB(ItemAddItemData);
                    }
                case IffGroupFlag.ITEM_TYPE_CLUB:
                    {
                        return AddItemToDB(ItemAddItemData);
                    }
                case IffGroupFlag.ITEM_TYPE_AUX:
                case IffGroupFlag.ITEM_TYPE_BALL:
                case IffGroupFlag.ITEM_TYPE_USE:
                    {
                        WareHouseData = ItemWarehouse.GetItem(ItemAddItemData.ItemIffId, 1);
                        if (WareHouseData != null)
                        {

                            Result.Status = true;
                            Result.ItemIndex = ((WarehouseData)(WareHouseData)).ItemIndex;
                            Result.ItemTypeID = ((WarehouseData)(WareHouseData)).ItemTypeID;
                            Result.ItemOldQty = ((WarehouseData)(WareHouseData)).ItemC0;
                            Result.ItemNewQty = ((WarehouseData)(WareHouseData)).ItemC0 + ItemAddItemData.Quantity;
                            Result.ItemUCCKey = ((WarehouseData)(WareHouseData)).ItemUCCUnique;
                            Result.ItemFlag = (byte)((WarehouseData)(WareHouseData)).ItemFlag;
                            Result.ItemEndDate = null;
                            //Add
                            ((WarehouseData)(WareHouseData)).AddQuantity(ItemAddItemData.Quantity);

                            if (ItemAddItemData.Transaction)
                            {
                                ItemTransaction.AddItem(0x02, (WarehouseData)WareHouseData, ItemAddItemData.Quantity);
                            }
                        }

                        else if (WareHouseData == null)
                        {
                            return AddItemToDB(ItemAddItemData);
                        }
                    }
                    break;
                case IffGroupFlag.ITEM_TYPE_CADDIE:
                    {
                        return AddItemToDB(ItemAddItemData);
                    }
                case IffGroupFlag.ITEM_TYPE_CADDIE_ITEM:
                    {
                        WareHouseData = ItemCaddie.GetCaddieBySkinId(ItemAddItemData.ItemIffId);

                        if (!(WareHouseData == null))
                        {
                            ((CaddieData)(WareHouseData)).Update();
                            ((CaddieData)(WareHouseData)).UpdateCaddieSkin(ItemAddItemData.ItemIffId, ItemAddItemData.Day);
                            Result.Status = true;
                            Result.ItemIndex = ((CaddieData)(WareHouseData)).CaddieIdx;
                            Result.ItemTypeID = ((CaddieData)(WareHouseData)).CaddieSkin;
                            Result.ItemOldQty = 1;
                            Result.ItemNewQty = 1;
                            Result.ItemUCCKey = string.Empty;
                            Result.ItemFlag = 0;
                            Result.ItemEndDate = DateTime.Now.AddDays(ItemAddItemData.Day);
                        }
                    }
                    break;
                case IffGroupFlag.ITEM_TYPE_SKIN:
                    {
                        return AddItemToDB(ItemAddItemData);
                    }
                case IffGroupFlag.ITEM_TYPE_MASCOT:
                    {
                        WareHouseData = ItemMascot.GetMascotByTypeId(ItemAddItemData.ItemIffId);

                        if (WareHouseData != null)
                        {
                            ((MascotData)(WareHouseData)).AddDay(ItemAddItemData.Day);
                            Result.Status = true;
                            Result.ItemIndex = ((MascotData)(WareHouseData)).MascotIndex;
                            Result.ItemTypeID = ((MascotData)(WareHouseData)).MascotTypeID;
                            Result.ItemOldQty = 1;
                            Result.ItemNewQty = 1;
                            Result.ItemUCCKey = "";
                            Result.ItemFlag = 0;
                            Result.ItemEndDate = ((MascotData)(WareHouseData)).MascotEndDate;
                        }
                        else if (WareHouseData == null)
                        {
                            return AddItemToDB(ItemAddItemData);
                        }
                    }
                    break;

                case IffGroupFlag.ITEM_TYPE_CARD:
                    {
                        WareHouseData = ItemCard.GetCard(ItemAddItemData.ItemIffId, 1);

                        if (WareHouseData == null)
                        {
                            AddItemToDB(ItemAddItemData);
                        }
                        else if (WareHouseData != null)
                        {
                            Result.Status = true;
                            Result.ItemIndex = ((CardData)(WareHouseData)).CardIndex;
                            Result.ItemTypeID = ((CardData)(WareHouseData)).CardTypeID;
                            Result.ItemOldQty = ((CardData)(WareHouseData)).CardQuantity;
                            Result.ItemNewQty = ((CardData)(WareHouseData)).CardQuantity + ItemAddItemData.Quantity;
                            Result.ItemUCCKey = string.Empty;
                            Result.ItemFlag = 0;
                            Result.ItemEndDate = null;

                            ((CardData)(WareHouseData)).AddQuantity(ItemAddItemData.Quantity);

                            if (ItemAddItemData.Transaction)
                                ItemTransaction.AddCard(0x02, (CardData)WareHouseData, ItemAddItemData.Quantity);
                        }
                    }
                    break;
            }
            return Result;
        }

        public AddItemData AddRent(uint TypeID, ushort Day = 7)
        {
            object PRent;
            AddItemData Result;

            Result = new AddItemData() { Status = false };

            if (!(TypeID.GetItemGroup() == IffGroupFlag.ITEM_TYPE_PART))
            {
                return Result;
            }
            var _db = new PangyaEntities();
            var Add = _db.ProcAddRent((int)UID, (int)TypeID, Day).ToList();
            if (Add.Count <= 0)
                return Result;
            foreach (var data in Add)
            {
                PRent = new WarehouseData();

                ((WarehouseData)(PRent)).ItemIndex = (uint)data.ITEM_INDEX;
                ((WarehouseData)(PRent)).ItemTypeID = (uint)data.ITEM_TYPEID;
                ((WarehouseData)(PRent)).ItemC0 = 0;
                ((WarehouseData)(PRent)).ItemUCCUnique = string.Empty;
                ((WarehouseData)(PRent)).CreateNewItem();
                ((WarehouseData)(PRent)).ItemFlag = (byte)data.ITEM_FLAG;
                ((WarehouseData)(PRent)).ItemEndDate = data.ITEM_DATE_END;
                ItemWarehouse.ItemAdd((WarehouseData)(PRent));

                Result.Status = true;
                Result.ItemIndex = ((WarehouseData)(PRent)).ItemIndex;
                Result.ItemTypeID = ((WarehouseData)(PRent)).ItemTypeID;
                Result.ItemOldQty = 0;
                Result.ItemNewQty = 1;
                Result.ItemUCCKey = ((WarehouseData)(PRent)).ItemUCCUnique;
                Result.ItemFlag = (byte)((WarehouseData)(PRent)).ItemFlag;
                Result.ItemEndDate = ((WarehouseData)(PRent)).ItemEndDate;
            }

            return Result;
        }

        public AddItemData AddItemToDB(AddItem ItemAddItemData)
        {
            Object PlayerObjectData;
            TransactionData Tran;
            AddItemData Result;

            Result = new AddItemData() { Status = false };
            var _db = new PangyaEntities();
            var additem = _db.ProcAddItem((int)UID, (int)ItemAddItemData.ItemIffId, (int)ItemAddItemData.Quantity, Compare.IfCompare<byte>(IFFEntry.GetIff.IsSelfDesign(ItemAddItemData.ItemIffId), 1, 0), IFFEntry.GetIff.GetItemTimeFlag(ItemAddItemData.ItemIffId, ItemAddItemData.Day), (int)ItemAddItemData.Day).ToList();
            if (additem.Count > 0)
            {
                var dbdata = additem.FirstOrDefault();

                Tran = new TransactionData() { Types = 2, Index = (uint)dbdata.IDX, TypeID = (uint)dbdata.iffTypeId, PreviousQuan = 0, NewQuan = (uint)dbdata.Quantity, UCC = dbdata.UCC_KEY };

                ItemTransaction.Add(Tran);
                try
                {
                    switch (ItemAddItemData.ItemIffId.GetItemGroup())
                    {
                        case IffGroupFlag.ITEM_TYPE_CHARACTER:
                            {
                                PlayerObjectData = new CharacterData();

                                ((CharacterData)(PlayerObjectData)).Index = (uint)dbdata.IDX;
                                ((CharacterData)(PlayerObjectData)).TypeID = (uint)dbdata.iffTypeId;
                                ((CharacterData)(PlayerObjectData)).HairColour = 0;
                                ((CharacterData)(PlayerObjectData)).GiftFlag = 0;
                                ItemCharacter.CharacterAdd((CharacterData)(PlayerObjectData));

                                CharacterIndex = (uint)dbdata.IDX;
                                Result = new AddItemData()
                                {
                                    Status = true,
                                    ItemIndex = ((CharacterData)(PlayerObjectData)).Index,
                                    ItemTypeID = ((CharacterData)(PlayerObjectData)).TypeID,
                                    ItemOldQty = 1,
                                    ItemNewQty = 1,
                                    ItemUCCKey = string.Empty,
                                    ItemFlag = 0,
                                    ItemEndDate = DateTime.MinValue,
                                };
                            }
                            break;

                        case IffGroupFlag.ITEM_TYPE_AUX:
                        case IffGroupFlag.ITEM_TYPE_PART:
                        case IffGroupFlag.ITEM_TYPE_CLUB:
                        case IffGroupFlag.ITEM_TYPE_BALL:
                        case IffGroupFlag.ITEM_TYPE_USE:
                        case IffGroupFlag.ITEM_TYPE_SKIN:
                            {
                                PlayerObjectData = new WarehouseData();
                                ((WarehouseData)(PlayerObjectData)).ItemIndex = (uint)dbdata.IDX;
                                ((WarehouseData)(PlayerObjectData)).ItemTypeID = (uint)dbdata.iffTypeId;
                                ((WarehouseData)(PlayerObjectData)).ItemC0 = (ushort)dbdata.Quantity;
                                ((WarehouseData)(PlayerObjectData)).ItemUCCUnique = dbdata.UCC_KEY;
                                ((WarehouseData)(PlayerObjectData)).CreateNewItem();
                                // Add to inventory list
                                ItemWarehouse.ItemAdd((WarehouseData)(PlayerObjectData));
                                // Set the result data
                                Result = new AddItemData()
                                {
                                    Status = true,
                                    ItemIndex = ((WarehouseData)(PlayerObjectData)).ItemIndex,
                                    ItemTypeID = ((WarehouseData)(PlayerObjectData)).ItemTypeID,
                                    ItemOldQty = 0,
                                    ItemNewQty = ItemAddItemData.Quantity,
                                    ItemUCCKey = ((WarehouseData)(PlayerObjectData)).ItemUCCUnique,
                                    ItemFlag = 0,
                                    ItemEndDate = null,
                                };
                            }
                            break;
                        case IffGroupFlag.ITEM_TYPE_CADDIE:
                            {
                                PlayerObjectData = new CaddieData();
                                ((CaddieData)(PlayerObjectData)).CaddieIdx = (uint)dbdata.IDX;
                                ((CaddieData)(PlayerObjectData)).CaddieTypeId = (uint)dbdata.iffTypeId;
                                ((CaddieData)(PlayerObjectData)).CaddieDateEnd = (DateTime)dbdata.END_DATE;
                                ((CaddieData)(PlayerObjectData)).CaddieAutoPay = 0;
                                ((CaddieData)(PlayerObjectData)).CaddieType = (byte)dbdata.Flag;
                                // Add caddie to inventory list
                                ItemCaddie.CadieAdd((CaddieData)(PlayerObjectData));
                                // set the result data
                                Result = new AddItemData()
                                {
                                    Status = true,
                                    ItemIndex = ((CaddieData)(PlayerObjectData)).CaddieIdx,
                                    ItemTypeID = ((CaddieData)(PlayerObjectData)).CaddieTypeId,
                                    ItemOldQty = 0,
                                    ItemNewQty = 1,
                                    ItemUCCKey = string.Empty,
                                    ItemFlag = ((CaddieData)(PlayerObjectData)).CaddieType,
                                    ItemEndDate = null,
                                };
                            }
                            break;
                        case IffGroupFlag.ITEM_TYPE_CARD:
                            {
                                PlayerObjectData = new CardData();
                                ((CardData)(PlayerObjectData)).CardIndex = (uint)dbdata.IDX;
                                ((CardData)(PlayerObjectData)).CardTypeID = (uint)dbdata.iffTypeId;
                                ((CardData)(PlayerObjectData)).CardQuantity = ItemAddItemData.Quantity;
                                ((CardData)(PlayerObjectData)).CardIsValid = 1;
                                ((CardData)(PlayerObjectData)).CardNeedUpdate = false;
                                // ## add to card
                                ItemCard.CardAdd((CardData)(PlayerObjectData));
                                // set the result data
                                Result = new AddItemData()
                                {
                                    Status = true,
                                    ItemIndex = ((CardData)(PlayerObjectData)).CardIndex,
                                    ItemTypeID = ((CardData)(PlayerObjectData)).CardTypeID,
                                    ItemOldQty = 0,
                                    ItemNewQty = ((CardData)(PlayerObjectData)).CardQuantity,
                                    ItemUCCKey = string.Empty,
                                    ItemFlag = 0,
                                    ItemEndDate = null,
                                };
                            }
                            break;
                        case IffGroupFlag.ITEM_TYPE_MASCOT:
                            {
                                PlayerObjectData = new MascotData();
                                ((MascotData)(PlayerObjectData)).MascotIndex = (uint)dbdata.IDX;
                                ((MascotData)(PlayerObjectData)).MascotTypeID = (uint)dbdata.iffTypeId;
                                ((MascotData)(PlayerObjectData)).MascotMessage = "Pangya !";
                                ((MascotData)(PlayerObjectData)).MascotIsValid = 1;
                                ((MascotData)(PlayerObjectData)).MascotNeedUpdate = false;
                                ((MascotData)(PlayerObjectData)).MascotEndDate = (DateTime)dbdata.END_DATE;

                                ((MascotData)(PlayerObjectData)).MascotDayToEnd = (ushort)dbdata.END_DATE.DaysBetween(DateTime.Now);
                                // ## add to card
                                ItemMascot.MascotAdd((MascotData)(PlayerObjectData));
                                // set the result data
                                Result = new AddItemData()
                                {
                                    Status = true,
                                    ItemIndex = ((MascotData)(PlayerObjectData)).MascotIndex,
                                    ItemTypeID = ((MascotData)(PlayerObjectData)).MascotTypeID,
                                    ItemOldQty = 0,
                                    ItemNewQty = 1,
                                    ItemUCCKey = string.Empty,
                                    ItemFlag = 4,
                                    ItemEndDate = DateTime.Now.AddDays(ItemAddItemData.Day + 1),
                                };
                            }
                            break;
                    }
                }
                catch
                {
                  
                }
            }
            // ## resulted
            return Result;
        }
        #endregion

        #region RemoveItems
        public AddItemData Remove(uint ItemIffId, uint Quantity, bool Transaction = true)
        {
            AddItemData ItemDeletedData;
            WarehouseData Items;
            CardData Cards;
            TransactionData Tran;
            ItemDeletedData = new AddItemData() { Status = false };
            if (UID <= 0)
            { return ItemDeletedData; }

            if (ItemIffId <= 0 && Quantity <= 0)
            { return ItemDeletedData; }


            switch (ItemIffId.GetItemGroup())
            {
                case IffGroupFlag.ITEM_TYPE_CLUB:
                case IffGroupFlag.ITEM_TYPE_USE:
                    {
                        Items = ItemWarehouse.GetItem(ItemIffId, Quantity);

                        if (!(Items == null))
                        {
                            ItemDeletedData.Status = true;
                            ItemDeletedData.ItemIndex = Items.ItemIndex;
                            ItemDeletedData.ItemTypeID = Items.ItemTypeID;
                            ItemDeletedData.ItemOldQty = Items.ItemC0;
                            ItemDeletedData.ItemNewQty = Items.ItemC0 - Quantity;
                            ItemDeletedData.ItemUCCKey = Items.ItemUCCUnique;
                            ItemDeletedData.ItemFlag = 0;
                            ItemDeletedData.ItemEndDate = null;
                            if (Transaction)
                            {
                                Tran = new TransactionData() { UCC = "", Types = 2, TypeID = Items.ItemTypeID, Index = Items.ItemIndex, PreviousQuan = Items.ItemC0, NewQuan = Items.ItemC0 - Quantity };
                                ItemTransaction.Add(Tran);
                            }

                            // update item info
                            Items.RemoveQuantity(Quantity);
                        }
                        return ItemDeletedData;
                    }
                case IffGroupFlag.ITEM_TYPE_CARD:
                    {
                        Cards = ItemCard.GetCard(ItemIffId, Quantity);

                        if (!(Cards == null))
                        {
                            ItemDeletedData.Status = true;
                            ItemDeletedData.ItemIndex = Cards.CardIndex;
                            ItemDeletedData.ItemTypeID = Cards.CardTypeID;
                            ItemDeletedData.ItemOldQty = Cards.CardQuantity;
                            ItemDeletedData.ItemNewQty = Cards.CardQuantity - Quantity;
                            ItemDeletedData.ItemUCCKey = string.Empty;
                            ItemDeletedData.ItemFlag = 0;
                            ItemDeletedData.ItemEndDate = null;
                            if (Transaction)
                            {
                                Tran = new TransactionData() { UCC = "", Types = 2, TypeID = Cards.CardTypeID, Index = Cards.CardIndex, PreviousQuan = Cards.CardQuantity, NewQuan = Cards.CardQuantity - Quantity };
                                ItemTransaction.Add(Tran);
                            }
                        }
                        // update item info
                        Cards.RemoveQuantity(Quantity);
                        return ItemDeletedData;
                    }
            }
            ItemDeletedData.SetData(false, 0, 0, 0, 0, string.Empty, 0, DateTime.Now);
            return (ItemDeletedData);
        }

        public AddItemData Remove(uint ItemIffId, uint Index, uint Quantity, bool Transaction = true)
        {
            AddItemData ItemDeletedData;
            WarehouseData Items;
            CardData Cards;
            TransactionData Tran;
            ItemDeletedData = new AddItemData() { Status = false };
            if (UID <= 0)
            { return ItemDeletedData; }

            if (ItemIffId <= 0 && Quantity <= 0)
            { return ItemDeletedData; }


            switch (ItemIffId.GetItemGroup())
            {
                case IffGroupFlag.ITEM_TYPE_CLUB:
                case IffGroupFlag.ITEM_TYPE_USE:
                    {
                        Items = ItemWarehouse.GetItem(ItemIffId, Index, Quantity);

                        if (!(Items == null))
                        {
                            ItemDeletedData.Status = true;
                            ItemDeletedData.ItemIndex = Items.ItemIndex;
                            ItemDeletedData.ItemTypeID = Items.ItemTypeID;
                            ItemDeletedData.ItemOldQty = Items.ItemC0;
                            ItemDeletedData.ItemNewQty = Items.ItemC0 - Quantity;
                            ItemDeletedData.ItemUCCKey = Items.ItemUCCUnique;
                            ItemDeletedData.ItemFlag = 0;
                            ItemDeletedData.ItemEndDate = null;
                            if (Transaction)
                            {
                                Tran = new TransactionData() { UCC = "", Types = 2, TypeID = Items.ItemTypeID, Index = Items.ItemIndex, PreviousQuan = Items.ItemC0, NewQuan = Items.ItemC0 - Quantity };
                                ItemTransaction.Add(Tran);
                            }

                        }
                        // update item info
                        Items.RemoveQuantity(Quantity);
                        return ItemDeletedData;
                    }
                case IffGroupFlag.ITEM_TYPE_PART:
                    {
                        Items = ItemWarehouse.GetItem(ItemIffId, Index, 0); // ## part should be zero

                        if (!(Items == null))
                        {
                            ItemDeletedData.Status = true;
                            ItemDeletedData.ItemIndex = Items.ItemIndex;
                            ItemDeletedData.ItemTypeID = Items.ItemTypeID;
                            ItemDeletedData.ItemOldQty = 1;
                            ItemDeletedData.ItemNewQty = 0;
                            ItemDeletedData.ItemUCCKey = Items.ItemUCCUnique;
                            ItemDeletedData.ItemFlag = 0;
                            ItemDeletedData.ItemEndDate = null;
                            if (Transaction)
                            {
                                Tran = new TransactionData() { UCC = "", Types = 2, TypeID = Items.ItemTypeID, Index = Items.ItemIndex, PreviousQuan = 1, NewQuan = 0 };
                                ItemTransaction.Add(Tran);
                            }
                        }
                        // update item info
                        Items.RemoveQuantity(Quantity);
                        return ItemDeletedData;
                    }
                case IffGroupFlag.ITEM_TYPE_CARD:
                    {
                        Cards = ItemCard.GetCard(ItemIffId, Index, Quantity);

                        if (!(Cards == null))
                        {
                            ItemDeletedData.Status = true;
                            ItemDeletedData.ItemIndex = Cards.CardIndex;
                            ItemDeletedData.ItemTypeID = Cards.CardTypeID;
                            ItemDeletedData.ItemOldQty = Cards.CardQuantity;
                            ItemDeletedData.ItemNewQty = Cards.CardQuantity - Quantity;
                            ItemDeletedData.ItemUCCKey = string.Empty;
                            ItemDeletedData.ItemFlag = 0;
                            ItemDeletedData.ItemEndDate = null;
                            if (Transaction)
                            {
                                Tran = new TransactionData() { UCC = "", Types = 2, TypeID = Cards.CardTypeID, Index = Cards.CardIndex, PreviousQuan = Cards.CardQuantity, NewQuan = Cards.CardQuantity - Quantity };
                                ItemTransaction.Add(Tran);
                            }
                        }
                        // update item info
                        Cards.RemoveQuantity(Quantity);
                        return ItemDeletedData;
                    }
            }
            ItemDeletedData.SetData(false, 0, 0, 0, 0, string.Empty, 0, DateTime.Now);
            return (ItemDeletedData);
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
            // close for next 
            return SQLString.ToString();
        }
    }
}
