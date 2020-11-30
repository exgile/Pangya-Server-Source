using Pangya_GameServer.Models.Player.Data;
using PangyaAPI.BinaryModels;
using PangyaAPI.IFF.Flags;
using PangyaAPI.IFF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Pangya_GameServer.Models.Player.Collection
{
    public class WarehouseCollection : List<WarehouseData>
    {
        public WarehouseCollection(int UID)
        {
            Build(UID);
        }

        void Build(int UID)
        {
           
        }

        public byte[] Build()
        {
            using (var result = new PangyaBinaryWriter())
            {
                result.Write(new byte[] { 0x73, 0x00 });
                result.WriteUInt16(Convert.ToUInt16(Count));
                result.WriteUInt16(Convert.ToUInt16(Count));
                foreach (var item in this)
                {
                    result.Write(item.GetItems());
                }
                return result.GetBytes();
            }
        }
        public int ItemAdd(WarehouseData Value)
        {
            Value.ItemNeedUpdate = false;
            Add(Value);
            return Count;
        }


        public WarehouseData GetUCC(uint ItemIdx)
        {
            var item = this.Where(c => c.ItemIndex == ItemIdx && c.ItemUCCUnique.Length >= 8);
            if (item.Any()) { return item.FirstOrDefault(); }

            return null;
        }

        public WarehouseData GetUCC(uint TypeID, string UCCUnique, bool Status)
        {
            var item = this.Where(c => c.ItemTypeID == TypeID && c.ItemUCCUnique == UCCUnique && c.ItemUCCStatus == (byte?)Convert.ToByte(Status));
            if (item.Any()) { return item.FirstOrDefault(); }

            return null;
        }
        public WarehouseData GetUCC(uint TypeID, string UCCUnique)
        {
            var item = this.Where(c => c.ItemTypeID == TypeID && c.ItemUCCUnique == UCCUnique && c.ItemUCCStatus >= 0);
            if (item.Any()) { return item.FirstOrDefault(); }

            return null;
        }
        public WarehouseData GetItem(uint Index)
        {
            foreach (var Items in this)
            {
                if ((Items.ItemIndex == Index) && (Items.ItemIsValid == 1))
                {
                    return Items;
                }
            }
            return null;
        }

        public WarehouseData GetItem(uint ID, uint type)
        {
            switch (type)
            {
                case 1:
                    {
                        var Item = this.Where(c => c.ItemTypeID == ID);

                        if (Item.Any())
                        {
                            return Item.FirstOrDefault();
                        }
                        else
                        {
                            return null;
                        }
                    }
                case 0:
                    {
                        var Item = this.Where(c => c.ItemIndex == ID);

                        if (Item.Any())
                        {
                            return Item.FirstOrDefault();
                        }
                        else
                        {
                            return null;
                        }
                    }
            }

            return null;
        }

        public WarehouseData GetItemData(uint TypeID, uint Quantity)
        {
            foreach (var Items in this)
            {
                if (Items.ItemTypeID == TypeID)
                {
                    if (Items.ItemC0 >= Quantity && Items.ItemIsValid == 1)
                    {
                        return Items;
                    }
                }
            }
            return null;
        }

        public WarehouseData GetItem(uint TypeID, uint Index, uint Quantity)
        {
            foreach (var Items in this)
            {
                if (Items.ItemTypeID == TypeID && (Items.ItemIndex == Index) && Items.ItemC0 >= Quantity && Items.ItemIsValid == 1)
                {
                    return Items;
                }
            }
            return null;
        }


        public WarehouseData GetClub(uint ID, uint type)
        {
            switch (type)
            {
                case 1:
                    {
                        var ClubInfo = this.Where(c => c.ItemTypeID == ID && c.ItemGroup == 4);

                        if (ClubInfo.Any())
                        {
                            return ClubInfo.FirstOrDefault();
                        }
                        else
                        {
                            return null;
                        }
                    }
                case 0:
                    {
                        var ClubInfo = this.Where(c => c.ItemIndex == ID && c.ItemGroup == 4);

                        if (ClubInfo.Any())
                        {
                            return ClubInfo.FirstOrDefault();
                        }
                        else
                        {
                            return null;
                        }
                    }
            }

            return null;
        }


        public WarehouseData GetDataItem(uint ID, uint type, uint Quantity)
        {
            switch (type)
            {
                case 1:
                    {
                        var Item = this.Where(c => c.ItemTypeID == ID && c.ItemC0 >= Quantity && c.ItemIsValid == 1);

                        if (Item.Any())
                        {
                            return Item.FirstOrDefault();
                        }
                    }
                    break;
                case 0:
                    {
                        var Item = this.Where(c => c.ItemIndex == ID && c.ItemC0 >= Quantity && c.ItemIsValid == 1);

                        if (Item.Any())
                        {
                            return Item.FirstOrDefault();
                        }
                        break;
                    }
            }

            return null;
        }


        public bool IsSkinExist(uint typeID)
        {
            var Items = this.FirstOrDefault(c => c.ItemTypeID == typeID && c.ItemIsValid == 1);


            if (Items != null && (byte)IFFTools.GetItemGroup(Items.ItemTypeID) == 14)
            {
                return true;
            }
            return false;
        }

        public bool IsClubExist(uint typeID)
        {
            var Items = this.FirstOrDefault(c => c.ItemTypeID == typeID && c.ItemIsValid == 1);
            if (Items != null && (byte)IFFTools.GetItemGroup(Items.ItemTypeID) == 4)
            {
                return true;
            }
            return false;
        }

        public bool IsNormalExist(uint typeID)
        {
            var Items = this.Where(c => c.ItemTypeID == typeID && c.ItemC0 > 0 && c.ItemIsValid == 1);
            return Items.Any();
        }
        public bool IsNormalExist(uint typeID, uint index, uint Quantity)
        {
            var Items = this.Where(c => c.ItemTypeID == typeID && c.ItemIndex == index && c.ItemC0 >= Quantity && c.ItemIsValid == 1);
            return Items.Any();
        }

        public bool IsPartExist(uint typeID)
        {
            var Items = this.Where(c => c.ItemTypeID == typeID && c.ItemIsValid == 1);

            return Items.Any();
        }

        public bool IsHairStyleExist(int typeID)
        {
            var Items = this.Where(c => c.ItemTypeID == typeID && c.ItemIsValid == 1);

            return Items.Any();
        }

        public bool IsPartExist(uint typeID, uint index, uint Quantity = 1)
        {
            var Items = this.Where(c => c.ItemTypeID == typeID && c.ItemIndex == index && c.ItemIsValid == 1);

            return Items.Any();
        }

        public uint GetQuantity(uint TypeID)
        {
            var Items = this.FirstOrDefault(c => c.ItemTypeID == TypeID);

            if (Items != null)
            {
                return Items.ItemC0;
            }
            else
            {
                return 0;
            }
        }

        public string GetSqlUpdateItems()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();

            foreach (var Items in this)
            {
                if (Items.ItemNeedUpdate)
                {
                    SQLString.Append(Items.GetSqlUpdateString());
                    Items.ItemNeedUpdate = false;
                }
            }
            return SQLString.ToString();
        }


        public List<WarehouseData> GetClubData()
        {
            return this.Where(c => c.ItemGroup == (byte)IffGroupFlag.ITEM_TYPE_CLUB && c.ItemNeedUpdate == true).ToList();
        }

        internal bool RemoveItem(uint TypeId, uint Count)
        {
            switch ((byte)IFFTools.GetItemGroup(TypeId))
            {
                case 5:
                case 6:
                    {
                        foreach (var Items in this)
                        {
                            if (Items.ItemTypeID == TypeId && Items.ItemC0 >= Count && Items.ItemIsValid == 1)
                            {
                                Items.ItemC0 -= (ushort)Count;

                                if (Items.ItemC0 == 0)
                                {
                                    Items.ItemIsValid = 0;
                                }
                            }
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        public void Update(WarehouseData Item)
        {
            foreach (var upgrade in this)
            {
                if (upgrade.ItemIndex == Item.ItemIndex && upgrade.ItemTypeID == Item.ItemTypeID)
                {
                    upgrade.Update(Item);
                }
            }
        }
        internal bool RemoveItem(WarehouseData Item)
        {
            return this.Remove(Item);
        }
    }
}
