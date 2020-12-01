﻿using Pangya_GameServer.Models;
using Pangya_GameServer.Models.Player;
using PangyaAPI.BinaryModels;
using PangyaAPI.SqlConnector.DataBase;
using PangyaAPI.Tools;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pangya_GameServer.GPlayer
{
    public partial class Session
    {
        #region Methods


        public void SetGameID(ushort ID)
        {
            GameID = ID;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LoadStatistic()
        {
            UserInfo.UserStatistic.Score = new byte[5];
            //foreach (var Data in _db.ProcGetStatistic((int)UserInfo.GetUID))
            //{
            //    UserInfo.UserStatistic.Drive = (uint)Data.Drive;
            //    UserInfo.UserStatistic.Putt = (uint)Data.Putt;
            //    UserInfo.UserStatistic.PlayTime = (uint)Data.Playtime;
            //    UserInfo.UserStatistic.ShotTime = (uint)Data.ShotTime;
            //    UserInfo.UserStatistic.LongestDistance = Data.Longest;
            //    UserInfo.UserStatistic.Pangya = (uint)Data.Pangya;
            //    UserInfo.UserStatistic.TimeOut = (uint)Data.Timeout;
            //    UserInfo.UserStatistic.OB = (uint)Data.OB;
            //    UserInfo.UserStatistic.DistanceTotal = (uint)Data.Distance;
            //    UserInfo.UserStatistic.Hole = (uint)Data.Hole;
            //    UserInfo.UserStatistic.TeamHole = (uint)Data.TeamHole;
            //    UserInfo.UserStatistic.HIO = (uint)Data.Holeinone;
            //    UserInfo.UserStatistic.Bunker = (ushort)Data.Bunker;
            //    UserInfo.UserStatistic.Fairway = (uint)Data.Fairway;
            //    UserInfo.UserStatistic.Albratoss = (uint)Data.Albatross;
            //    UserInfo.UserStatistic.Holein = (uint)Data.Holein;
            //    UserInfo.UserStatistic.Puttin = (uint)Data.PuttIn;
            //    UserInfo.UserStatistic.LongestPutt = Data.LongestPuttin;
            //    UserInfo.UserStatistic.LongestChip = Data.LongestChipIn;
            //    UserInfo.UserStatistic.EXP = (uint)Data.Game_Point;
            //    UserInfo.UserStatistic.Level = (byte)Data.Game_Level;
            //    UserInfo.UserStatistic.Pang = (ulong)Data.Pang;
            //    UserInfo.UserStatistic.TotalScore = (uint)Data.TotalScore;
            //    UserInfo.UserStatistic.Score = new byte[5] { (byte)Data.BestScore0, (byte)Data.BestScore1, (byte)Data.BestScore2, (byte)Data.BestScore3, (byte)Data.BESTSCORE4 };
            //    UserInfo.UserStatistic.Unknown = 0;
            //    UserInfo.UserStatistic.MaxPang0 = (ulong)Data.MaxPang0;
            //    UserInfo.UserStatistic.MaxPang1 = (ulong)Data.MaxPang1;
            //    UserInfo.UserStatistic.MaxPang2 = (ulong)Data.MaxPang2;
            //    UserInfo.UserStatistic.MaxPang3 = (ulong)Data.MaxPang3;
            //    UserInfo.UserStatistic.MaxPang4 = (ulong)Data.MaxPang4;
            //    UserInfo.UserStatistic.SumPang = (ulong)Data.SumPang;
            //    UserInfo.UserStatistic.GamePlayed = (uint)Data.GameCount;
            //    UserInfo.UserStatistic.Disconnected = (uint)Data.DisconnectGames;
            //    UserInfo.UserStatistic.TeamWin = (uint)Data.wTeamWin;
            //    UserInfo.UserStatistic.TeamGame = (uint)Data.wTeamGames;
            //    UserInfo.UserStatistic.LadderPoint = (uint)Data.LadderPoint;
            //    UserInfo.UserStatistic.LadderWin = (uint)Data.LadderWin;
            //    UserInfo.UserStatistic.LadderLose = (uint)Data.LadderLose;
            //    UserInfo.UserStatistic.LadderDraw = (uint)Data.LadderDraw;
            //    UserInfo.UserStatistic.LadderHole = (uint)Data.LadderHole;
            //    UserInfo.UserStatistic.ComboCount = (uint)Data.ComboCount;
            //    UserInfo.UserStatistic.MaxCombo = (uint)Data.MaxComboCount;
            //    UserInfo.UserStatistic.NoMannerGameCount = (uint)Data.NoMannerGameCount;
            //    UserInfo.UserStatistic.SkinsPang = (ulong)Data.SkinsPang;
            //    UserInfo.UserStatistic.SkinsWin = (uint)Data.SkinsWin;
            //    UserInfo.UserStatistic.SkinsLose = (uint)Data.SkinsLose;
            //    UserInfo.UserStatistic.SkinsRunHole = (uint)Data.SkinsRunHoles;
            //    UserInfo.UserStatistic.SkinsStrikePoint = (uint)Data.SkinsStrikePoint;
            //    UserInfo.UserStatistic.SKinsAllinCount = (uint)Data.SkinsAllinCount;
               UserInfo.UserStatistic.Unknown1 = new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //    UserInfo.UserStatistic.GameCountSeason = (uint)Data.GameCountSeason;
               UserInfo.UserStatistic.Unknown2 = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, };
            //}
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LoadGuildData()
        {
            foreach (var Data in _db.ProcGuildGetPlayerData((int)UserInfo.GetUID, 0))
            {
                GuildInfo = new Common.GuildData
                {
                    Name = Data.GUILD_NAME,
                    ID = (uint)Data.GUILD_INDEX,
                    Position = Data.GUILD_POSITION,
                    Image = Data.GUILD_IMAGE,
                    Introducing = Data.GUILD_INTRODUCING,
                    LeaderNickname = Data.GUILD_LEADER_NICKNAME,
                    Notice = Data.GUILD_NOTICE,
                    LeaderUID = (uint)Data.GUILD_LEADER_UID,
                    TotalMember = (uint)Data.GUILD_TOTAL_MEMBER,
                };
            }

            if (GuildInfo.LeaderUID == 0)
            {
                GuildInfo.LeaderUID = uint.MaxValue;
            }
        }

        public bool RemoveCookie(uint Amount)
        {
            if (UserInfo.GetCookie < Amount)
            {
                return (false);
            }
            UserInfo.GetCookie -= Amount;
            var table1 = $"UPDATE Pangya_Personal SET CookieAmt = '{UserInfo.GetCookie}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return (true);
        }

        public bool RemoveLockerPang(uint Amount)
        {
            if ((LockerPang < Amount)) return (false);
            LockerPang -= Amount;
            var table1 = $"UPDATE Pangya_Personal SET PangLockerAmt = '{LockerPang}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return (true);
        }

        public bool RemovePang(uint Amount)
        {
            if (UserInfo.UserStatistic.Pang < Amount)
            {
                return (false);
            }
            UserInfo.UserStatistic.Pang -= Amount;
            var table1 = $"UPDATE Pangya_User_Statistics SET Pang = '{UserInfo.UserStatistic.Pang}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return (true);
        }

        public bool AddLockerPang(uint Amount)
        {
            LockerPang += Amount;

            var table1 = $"UPDATE Pangya_Personal SET PangLockerAmt = '{LockerPang}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return (true);
        }

        public bool AddPang(uint Amount)
        {
            if (UserInfo.UserStatistic.Pang >= uint.MaxValue)
            {
                return false;
            }
            UserInfo.UserStatistic.Pang += Amount;
            var table1 = $"UPDATE Pangya_User_Statistics SET Pang = '{UserInfo.UserStatistic.Pang}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return true;
        }

        public bool AddCookie(uint Amount)
        {
            if (UserInfo.GetCookie >= uint.MaxValue)
            {
                return false;
            }
            UserInfo.GetCookie += Amount;
            var table1 = $"UPDATE Pangya_Personal SET CookieAmt = '{UserInfo.GetCookie}' WHERE UID = '{UserInfo.GetUID}'";
            _db.Database.SqlQuery<PangyaEntities>(table1).FirstOrDefault();
            return true;
        }

        public bool InventoryLoad()
        {
            if (UserInfo.GetUID != 0)
            {
                Inventory = new Inventory(UserInfo.GetUID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] GetGameInfomations(int level)
        {
            using (var result = new PangyaBinaryWriter())
            {
                result.WriteUInt32(ConnectionID);
                if (level >= 1)
                {
                    result.Write(GetGameInfo());
                }
                if (level >= 2)
                {
                    result.Write(Inventory.GetCharData());
                }

                return result.GetBytes();
            }
        }

        /// <summary>
        /// return 239 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] Statistic()
        {
            PangyaBinaryWriter result;
            result = new PangyaBinaryWriter();

            result.WriteUInt32(UserInfo.UserStatistic.Drive);
            result.WriteUInt32(UserInfo.UserStatistic.Putt);
            result.WriteUInt32(UserInfo.UserStatistic.PlayTime);
            result.WriteUInt32(UserInfo.UserStatistic.ShotTime);
            result.WriteSingle(UserInfo.UserStatistic.LongestDistance);
            result.WriteUInt32(UserInfo.UserStatistic.Pangya);
            result.WriteUInt32(UserInfo.UserStatistic.TimeOut);
            result.WriteUInt32(UserInfo.UserStatistic.OB);
            result.WriteUInt32(UserInfo.UserStatistic.DistanceTotal);
            result.WriteUInt32(UserInfo.UserStatistic.Hole);
            result.Write(new byte[] {
                0x00, 0x00, 0x00, 0x00, //team hole
                0x00, 0x00, 0x00, 0x00,//HIO
                0x1F, 0x00, //bunker?
                0x00, 0x00, 0x00, 0x00, //fairway
                0x00, 0x00, 0x00, 0x00, //albatoss
                0x3E, 0x00, 0x00, 0x00, //hole in
                0x50, 0xC2, 0x70, 0x40, //puttin
                0x13, 0xC4, 0x4E, 0x41,//longestputt
            });
            //result.WriteUInt32(UserInfo.UserStatistic.TeamHole);
            //result.WriteUInt32(UserInfo.UserStatistic.HIO);
            //result.WriteUInt16(UserInfo.UserStatistic.Bunker);
            //result.WriteUInt32(UserInfo.UserStatistic.Fairway);
            //result.WriteUInt32(UserInfo.UserStatistic.Albratoss);
            //result.WriteUInt32(UserInfo.UserStatistic.Holein);
            //result.WriteUInt32(UserInfo.UserStatistic.Puttin);
            //result.WriteSingle(UserInfo.UserStatistic.LongestPutt);
            //result.WriteSingle(UserInfo.UserStatistic.LongestChip);
            result.WriteUInt32(UserInfo.UserStatistic.EXP);
            UserInfo.UserStatistic.Level = 70;
            result.WriteByte(UserInfo.UserStatistic.Level);
            result.WriteUInt64(UserInfo.UserStatistic.Pang);//pangs inicias
            result.WriteUInt32(UserInfo.UserStatistic.TotalScore);
            result.WriteByte(UserInfo.UserStatistic.Score[0]);
            result.WriteByte(UserInfo.UserStatistic.Score[1]);
            result.WriteByte(UserInfo.UserStatistic.Score[2]);
            result.WriteByte(UserInfo.UserStatistic.Score[3]);
            result.WriteByte(UserInfo.UserStatistic.Score[4]);
            result.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x1D, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE8, 0x03, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00,
                0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,

                0x00, 0x00, 0x00, 0x00,

                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00 });

            result.WriteByte(UserInfo.UserStatistic.Unknown1[0]);
            result.WriteByte(UserInfo.UserStatistic.Unknown1[1]);
            result.WriteByte(UserInfo.UserStatistic.Unknown1[2]);
            result.WriteByte(UserInfo.UserStatistic.Unknown1[3]);
            result.WriteByte(UserInfo.UserStatistic.Unknown1[4]);
            result.WriteByte(UserInfo.UserStatistic.Unknown); //Unknow3[5]
            result.WriteUInt32(UserInfo.UserStatistic.GameCountSeason);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[0]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[1]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[2]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[3]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[4]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[5]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[6]);
            result.WriteByte(UserInfo.UserStatistic.Unknown2[7]);
            var response = result.GetBytes();
            return response;
        }

        /// <summary>
        /// Size = 267 bytes
        /// </summary>
        /// <returns>login information</returns>
        public byte[] GetLoginInfo()
        {
            PangyaBinaryWriter Reply;
            Reply = new PangyaBinaryWriter();
            try
            {
                Reply.WriteUInt16(GameID);
                Reply.WriteStr(UserInfo.GetLogin, 22);
                Reply.WriteStr(UserInfo.GetNickname, 22);
                Reply.WriteStr(GuildInfo.Name, 17);
                Reply.WriteStr(GuildInfo.Image, 16);
                Reply.WriteUInt64(UserInfo.GetCapability);
                Reply.WriteUInt32(ConnectionID);
                Reply.WriteZero(12);
                Reply.WriteUInt64(GuildInfo.ID);
                Reply.WriteUInt16(UserInfo.GetSex);
                Reply.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                Reply.WriteZero(16);
                Reply.WriteZero(128);
                Reply.WriteUInt32(UserInfo.GetUID);
                return Reply.GetBytes();
            }
            finally
            {
                Reply.Dispose();
            }
        }

        public byte[] GetGameInfo()
        {
            using (var Reply = new PangyaBinaryWriter())
            {
                Reply.WriteStr(UserInfo.GetNickname, 22);
                Reply.WriteStr(GuildInfo.Name, 17);//bytes 39 
                Reply.WriteByte(GameInfo.GameSlot);//40 bytes
                Reply.WriteUInt32(UserInfo.Visible);//44 bytes
                Reply.WriteUInt32(Inventory.GetTitleTypeID());
                Reply.WriteUInt32(Inventory.GetCharTypeID());//52 bytes                
                Reply.WriteStruct(Inventory.ItemDecoration);//72 bytes              
                Reply.WriteByte(GameInfo.Role);//77 bytes
                Reply.WriteByte(Compare.IfCompare<byte>(GameInfo.GameReady, 0x02, 0x00));//78 bytes
                Reply.WriteByte(UserInfo.GetLevel); // 79 bytes == GetLevel
                Reply.WriteByte(0); // 80 bytes
                Reply.WriteByte(0x0A);// 81 bytes
                Reply.WriteUInt32(GuildInfo.ID); // 85 bytes
                Reply.WriteStr(Compare.IfCompare(GuildInfo.Image.Length <= 0, "guildmark", GuildInfo.Image), 12);//94 bytes
                Reply.WriteUInt32(UserInfo.GetUID);//101 bytes
                Reply.Write(GameInfo.Action.ToArray());//123 bytes
                Reply.WriteUInt32(0);//ID Shop//127 bytes
                Reply.WriteStr("Shop Name", 35);//158 bytes
                Reply.WriteZero(33);//191 bytes
                Reply.WriteUInt32(Inventory.GetMascotTypeID());//195 bytes
                Reply.WriteByte(Compare.IfCompare<byte>(Inventory.IsExist(436207618), 0x1, 0x0)); // Pang Mastery// 196 bytes
                Reply.WriteByte(Compare.IfCompare<byte>(Inventory.IsExist(436207621), 0x1, 0x0)); // Nitro Pang Mastery//197 bytes
                Reply.WriteStr(GetSubLogin, 18);
                Reply.WriteZero(103);//329 bytes
                Reply.Write(0);//333 bytes = 0x1400006C
                Reply.Write(0);//337 bytes = 0x42000000
                var Getbytes = Reply.GetBytes();
                return Getbytes;
            }
        }
        /// <summary>
        /// Size Packet = 277 
        /// </summary>
        /// <returns></returns>
        public byte[] GetGuildInfo()
        {
            var Reply = new PangyaBinaryWriter();
            Reply.WriteStruct(GuildInfo);
            return Reply.GetBytes();
        }

        public byte[] GetMapStatistic()
        {
            return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x2E, 0x01, 0x38, 0xB0, 0xAA, 0x0C, 0xC9, 0x9D, 0x82, 0x7C, 0xC8, 0x7F, 0x81, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xE2, 0x00, 0x78, 0xAD, 0xAA, 0x0C, 0xB4, 0xAF, 0xAA, 0x0C, 0xF8, 0xE6, 0x79, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xC0, 0xAF, 0xAA, 0x0C, 0xAB, 0x85, 0x40, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x2E, 0x8F, 0xAD, 0x2B, 0x50, 0x00, 0x00, 0x00, 0xD8, 0x6B, 0x02, 0x15, 0x00, 0x00, 0x00, 0x00, 0x20, 0xC4, 0xE7, 0x1D, 0xD8, 0x6B, 0xED, 0x15, 0x98, 0xAF, 0xAA, 0x0C, 0xD8, 0x6B, 0xED, 0x15, 0x50, 0x00, 0x00, 0x00, 0xDC, 0xAF, 0xAA, 0x7F, 0x7D, 0xA6, 0x40, 0x00, 0x20, 0xC4, 0xE7, 0x1D, 0x60, 0xC7, 0xE7, 0x1D, 0x50, 0x03, 0x00, 0x00, 0xC8, 0x6B, 0xED, 0x15, 0xA8, 0xD2, 0x8F, 0x00, 0x90, 0xD2, 0x8F, 0x00, 0x0C, 0xB0, 0xAA, 0x0C, 0xE5, 0x4E, 0x5F, 0x00, 0x8C, 0xB0, 0xAA, 0x0C, 0x00, 0x00, 0x7F, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xE2, 0x90, 0xAD, 0x2B, 0xD8, 0x6B, 0xED, 0x15, 0x04, 0x6B, 0xED, 0x15, 0xC8, 0x6B, 0xED, 0x15, 0x40, 0xB0, 0xAA, 0x0C, 0xA8, 0xD2, 0x8F, 0x00, 0x00, 0x00, 0xE2, 0x00, 0x48, 0x00, 0x2E, 0x01, 0x4C, 0xB0, 0xAA, 0x0C, 0x92, 0x7F, 0x5F, 0x00, 0x90, 0xD2, 0x8F, 0x00, 0xA2, 0x90, 0x01, 0x00, 0x68, 0xAF, 0xAA, 0x05, 0x0F, 0x00, 0x00, 0x00, 0xB4, 0xB8, 0xAA, 0x0C, 0xE0, 0x80, 0x82, 0x7C, 0xD0, 0x9D, 0x82, 0x7C, 0xFF, 0xFF, 0xFF, 0xFF, 0xC9, 0x9D, 0x82, 0x7C, 0xBD, 0x19, 0x75, 0x00, 0x7F, 0x00, 0xE2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0xC7, 0xE7, 0x1D, 0xC0, 0xB8, 0x06, 0x0C, 0xA1, 0x64, 0x5F, 0x00, 0x60, 0xC7, 0xE7, 0x1D, 0x2E, 0x98, 0xAD, 0x2B, 0xDC, 0xD2, 0x8F, 0x00, 0x04, 0x00, 0x00, 0x00, 0xD0, 0x64, 0x5F, 0x00, 0x30, 0xD2, 0x8F, 0x7F, 0x91, 0x82, 0xF2, 0x52, 0x00, 0x00, 0x00, 0x00, 0x84, 0xB0, 0xAA, 0x0C, 0x8C, 0x07, 0xAA, 0x0C, 0x04, 0x00, 0x00, 0x00, 0x48, 0xEB, 0x7D, 0x00, 0x91, 0x82, 0xF2, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC7, 0xE7, 0x1D, 0x8A, 0x91, 0x6E, 0x00, 0x5E, 0x90, 0x7F, 0x2B, 0x04, 0xB9, 0xAA, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x08, 0xB8, 0xAA, 0x0C, 0xA8, 0xEB, 0x7D, 0x00, 0x31, 0x09, 0x44, 0x6F, 0x6C, 0x66, 0x69, 0x6E, 0x69, 0x09, 0x30, 0x09, 0x5B, 0x4E, 0x5F, 0x4C, 0x4F, 0x47, 0x49, 0x4E, 0x5D, 0x7F, 0x4C, 0x6F, 0x67, 0x49, 0x6E, 0x50, 0x72, 0x6F, 0x63, 0x65, 0x73, 0x73, 0x20, 0x09, 0x20, 0x43, 0x4F, 0x4D, 0x50, 0x4C, 0x41, 0x54, 0x45, 0x20, 0x3A, 0x20, 0x47, 0x55, 0x49, 0x44, 0x28, 0x31, 0x30, 0x37, 0x29, 0x2C, 0x20, 0x43, 0x6F, 0x75, 0x6E, 0x74, 0x7F, 0x33, 0x32, 0x29, 0x2C, 0x20, 0x43, 0x6D, 0x64, 0x49, 0x44, 0x28, 0x32, 0x31, 0x0A, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
        }

        public byte[] GetLobbyInfo()
        {
            PangyaBinaryWriter Reply;

            Reply = new PangyaBinaryWriter();
            try
            {
                Reply.WriteUInt32(UserInfo.GetUID);
                Reply.WriteUInt32(ConnectionID);
                Reply.WriteUInt16(GameID); // Room Number
                Reply.WriteStr(UserInfo.GetNickname, 22);
                Reply.WriteByte(UserInfo.UserStatistic.Level); // Level
                Reply.WriteUInt32(UserInfo.Visible); // GM
                Reply.WriteUInt32(Inventory.GetTitleTypeID());//964689929); // Title TypeID
                Reply.Write(1000); // Unknown
                Reply.WriteByte(UserInfo.GetSex); // Add $10 for wings  + $10 + $20
                Reply.WriteUInt32(GuildInfo.ID); // Guild ID
                Reply.WriteStr(GuildInfo.Image, 12); // Guild Images 9 Length
                Reply.WriteByte(0); // IS VIP ?
                Reply.WriteZero(6);
                Reply.WriteStr(GetSubLogin, 18);
                Reply.WriteZero(109);
                return Reply.GetBytes();
            }
            finally
            {
                Reply.Dispose();
            }
        }

        public void SendExp()
        {
            PangyaBinaryWriter Packet;

            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0xD9, 0x01 });
                Packet.WriteUInt32(UserInfo.UserStatistic.Level);
                Packet.WriteUInt32(UserInfo.UserStatistic.EXP);
                Send(Packet.GetBytes());
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public void SendLevelUp()
        {
            PangyaBinaryWriter Packet;

            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0x0F, 0x00 });
                Packet.Write(0);
                Packet.Write(this.UserInfo.GetLevel);
                Packet.Write(this.UserInfo.GetLevel);
                SendResponse(Packet);
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public void SendCookies()
        {
            PangyaBinaryWriter Reply;
            Reply = new PangyaBinaryWriter();
            try
            {
                Reply.Write(new byte[] { 0x96, 0x00 });
                Reply.WriteUInt64(UserInfo.GetCookie);
                Reply.WriteUInt64(0);
                Send(Reply.GetBytes());
            }
            finally
            {
                Reply.Dispose();
            }
        }

        public void SendGuildData()
        {
            PangyaBinaryWriter Packet;


            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0xBF, 0x01 });
                Packet.WriteUInt32(1);
                Packet.WriteUInt32(GuildInfo.ID); // Guild ID
                Packet.WriteStr(GuildInfo.Name, 17);
                Packet.WriteZero(9);
                Packet.WriteUInt32(GuildInfo.TotalMember);
                Packet.WriteStr(GuildInfo.Image, 9);
                Packet.WriteZero(3);
                Packet.WriteStr(GuildInfo.Notice, 0x65);
                Packet.WriteStr(GuildInfo.Introducing, 101);
                Packet.WriteUInt32(GuildInfo.Position);
                Packet.WriteUInt32(GuildInfo.LeaderUID);
                Packet.WriteStr(GuildInfo.LeaderNickname, 22);
                Send(Packet.GetBytes());
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public void SendLockerPang()
        {
            PangyaBinaryWriter Packet;
            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0x72, 0x01 });
                Packet.WriteUInt64(LockerPang);
                Send(Packet.GetBytes());
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public void SendPang()
        {
            PangyaBinaryWriter Packet;
            Packet = new PangyaBinaryWriter();
            try
            {
                Packet.Write(new byte[] { 0xC8, 0x00 });
                Packet.WriteUInt64(UserInfo.GetPang);
                Packet.WriteUInt32(0);
                Send(Packet.GetBytes());
            }
            finally
            {
                Packet.Dispose();
            }
        }

        public void SendLoginMainData(string version, ServerOptions serverOptions)
        {
            Response.Write(new byte[] { 0x44, 0x00, 0x00 });
            Response.WritePStr(version);
            Response.WritePStr("Pangya_GameServer_V727.00");
            Response.Write(GetLoginInfo());
            Response.Write(Statistic());
            Response.Write(Inventory.GetTrophyInfo());
            Response.Write(Inventory.GetEquipData());
            Response.Write(GetMapStatistic());
            Response.Write(Inventory.GetEquipInfo());
            Response.WriteTime();
            Response.WriteStruct(serverOptions.OptionsData());
            Response.Write(GetGuildInfo());
            SendResponse();
        }


        public void SendCharacterData()
        {
            SendResponse(Inventory.ItemCharacter.Build());
        }

        public void SendItemsData()
        {
            SendResponse(Inventory.ItemWarehouse.Build());
        }

        public void SendCaddiesData()
        {
            SendResponse(Inventory.ItemCaddie.Build());
        }

        public void SendMascotsData()
        {
            SendResponse(Inventory.ItemCaddie.Build());
        }

        public void SendEquipmentData()
        {
            SendResponse(Inventory.GetEquipData());
        }
        #endregion
    }
}