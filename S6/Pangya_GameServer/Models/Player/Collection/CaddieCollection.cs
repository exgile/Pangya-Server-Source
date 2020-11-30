using Pangya_GameServer.Models;
using PangyaAPI.BinaryModels;
using System;
using System.Collections.Generic;
using System.Text;
using Pangya_GameServer.Models.Player.Data;
namespace Pangya_GameServer.Models.Player.Collection
{
    public class CaddieCollection : List<CaddieData>
    {
        public CaddieCollection(int UID)
        {
            Build(UID);
        }
        // SerialCaddieData
        public int CadieAdd(CaddieData Value)
        {
            Value.CaddieNeedUpdate = false;
            Add(Value);
            return Count;
        }


        void Build(int UID)
        {
        }


        public byte[] Build()
        {
            PangyaBinaryWriter Reply;

            using (Reply = new PangyaBinaryWriter())
            {
                Reply.Write(new byte[] { 0x71, 0x00 });
                Reply.WriteUInt16((ushort)Count);
                Reply.WriteUInt16((ushort)Count);
                foreach (CaddieData CaddieInfo in this)
                {
                    Reply.Write(CaddieInfo.GetCaddieInfo());
                }
                return Reply.GetBytes();
            }
        }

        public byte[] BuildExpiration()
        {
            PangyaBinaryWriter Reply;

            using (Reply = new PangyaBinaryWriter())
            {
                Reply.Write(new byte[] { 0xD4, 0x00 });
                foreach (CaddieData CaddieInfo in this)
                {
                    Reply.Write(CaddieInfo.GetExpirationNotice());
                }
                return Reply.GetBytes();
            }
        }
        public byte[] GetCaddie()
        {
            PangyaBinaryWriter Result;
            Result = new PangyaBinaryWriter();
            try
            {
                foreach (CaddieData CaddieInfo in this)
                {
                    Result.Write(CaddieInfo.GetCaddieInfo());
                }
                return Result.GetBytes();
            }
            finally
            {
                Result.Dispose();
            }
        }

        public bool IsExist(UInt32 TypeId)
        {
            foreach (CaddieData CaddieInfo in this)
            {
                if ((CaddieInfo.CaddieTypeId == TypeId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanHaveSkin(UInt32 SkinTypeId)
        {
            foreach (CaddieData CaddieInfo in this)
            {
                if (CaddieInfo.Exist(SkinTypeId))
                {
                    return true;
                }
            }
            return false;
        }

        public CaddieData GetCaddieByIndex(UInt32 Index)
        {
            foreach (CaddieData CaddieInfo in this)
            {
                if (CaddieInfo.CaddieIdx == Index)
                {
                    return CaddieInfo;
                }
            }
            return null;
        }

        public CaddieData GetCaddieBySkinId(UInt32 SkinTypeId)
        {
            foreach (CaddieData CaddieInfo in this)
            {
                if (CaddieInfo.Exist(SkinTypeId))
                {
                    return CaddieInfo;
                }
            }
            return null;
        }


        public string GetSqlUpdateCaddie()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                foreach (CaddieData CaddieInfo in this)
                {
                    if (CaddieInfo.CaddieNeedUpdate)
                    {
                        SQLString.Append(CaddieInfo.GetSQLUpdateString());
                        // update to false when get string
                        CaddieInfo.CaddieNeedUpdate = false;
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
