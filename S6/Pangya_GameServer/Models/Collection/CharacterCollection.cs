﻿using PangyaAPI.BinaryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PangyaAPI.IFF.Flags;
namespace Pangya_GameServer.Models.Collection
{
    public class CharacterCollection : List<CharacterData>
    {
        public uint UID;
        CardEquipCollection fCard = null;
        public CardEquipCollection Card
        {
            get
            {
                return fCard;
            }
            set
            {
                fCard = value;
            }
        }

        public CharacterCollection(int PlayerUID)
        {
            UID = (uint)PlayerUID;
            Build(PlayerUID);
        }


        void Build(int UID)
        {
          
        }

        public int CharacterAdd(CharacterData Value)
        {
            Value.NEEDUPDATE = true;
            foreach (var chars in this)
            {
                if (chars.AuxPartRight > 0 && chars.AuxPartLeft > 0)
                {
                    Value.AuxPartRight = chars.AuxPartRight;
                    Value.AuxPartLeft = chars.AuxPartLeft;
                    break;
                }
            }
            Value = CharacterPartDefault(Value);
            Add(Value);
            return Count;
        }

        private CharacterData CharacterPartDefault(CharacterData character)
        {
            return character;
        }


        public void UpdateCharacter(CharacterData character)
        {
            foreach (var Char in this)
            {
                if (Char.Index == character.Index && Char.TypeID == character.TypeID)
                {
                    Char.Update(character);
                }
            }
        }

        /// <summary>
        /// By LuisMK = 460 bytes for Character Data
        /// </summary>
        /// <param name="CharData">Character Information</param>
        /// <param name="CardMap">Card Information</param>
        /// <returns></returns>
        public byte[] CreateChar(CharacterData Character, byte[] CardMap)
        {
            PangyaBinaryWriter Packet;

            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(Character.GetData(CardMap));
                return Packet.GetBytes();
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public CharacterData GetCharByType(byte charType)
        {
            switch ((HairColorFlag)charType)
            {
                case HairColorFlag.Nuri:
                    return GetChar(67108864, 1);
                case HairColorFlag.Hana:
                    return GetChar(67108865, 1);
                case HairColorFlag.Azer:
                    return GetChar(67108866, 1);
                case HairColorFlag.Cecilia:
                    return GetChar(67108867, 1);
                case HairColorFlag.Max:
                    return GetChar(67108868, 1);
                case HairColorFlag.Kooh:
                    return GetChar(67108869, 1);
                case HairColorFlag.Arin:
                    return GetChar(67108870, 1);
                case HairColorFlag.Kaz:
                    return GetChar(67108871, 1);
                case HairColorFlag.Lucia:
                    return GetChar(67108872, 1);
                case HairColorFlag.Nell:
                    return GetChar(67108873, 1);
                case HairColorFlag.Spika:
                    return GetChar(67108874, 1);
                case HairColorFlag.NR:
                    return GetChar(67108875, 1);
                case HairColorFlag.HR:
                    return GetChar(67108876, 1);
                case HairColorFlag.CR:
                    return GetChar(67108878, 1);
            }
            return null;
        }

        public CharacterData GetChar(UInt32 ID, uint GetType)
        {
            switch (GetType)
            {
                case 1:
                    foreach (CharacterData Char in this)
                    {
                        if (Char.TypeID == ID)
                        {
                            return Char;
                        }
                    }
                    return null;
                case 0:
                    foreach (CharacterData Char in this)
                    {
                        if (Char.Index == ID)
                        {
                            return Char;
                        }
                    }
                    return null;
            }
            return null;
        }


        /// <summary>
        /// Get Size 460 bytes
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public byte[] GetCharData(UInt32 CID)
        {
            foreach (CharacterData Char in this)
            {
                if (Char.Index == CID)
                {
                    return CreateChar(Char, Card.MapCard(CID));
                }
            }
            return new byte[460];
        }

        public byte[] Build()
        {
            PangyaBinaryWriter Packet;
            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0x70, 0x00 });
                Packet.WriteUInt16((ushort)this.Count);
                Packet.WriteUInt16((ushort)this.Count);
                foreach (CharacterData Char in this)
                {
                    Packet.Write(CreateChar(Char, Card.MapCard(Char.Index)));
                }
                return Packet.GetBytes();
            }
            finally
            {
                Packet.Dispose();
            }
        }
        /// <summary>
        /// String usada para salvar dados do Character, Status + Equipamentos
        /// </summary>
        /// <returns>string for Transation SQL SERVER</returns>
        public string GetSqlUpdateCharacter()
        {
            StringBuilder SQLString;
            SQLString = new StringBuilder();
            try
            {
                foreach (CharacterData Char in this)
                {
                    if (Char.NEEDUPDATE)
                    {
                        SQLString.Append(Char.GetStringCharInfo());//string com informações do equipmento do char  
                    }
                    Char.SaveChar(UID);
                    Char.NEEDUPDATE = false;//seta como falso, para não causa erros ao salvar item
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
