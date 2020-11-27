using Pangya_GameServer.Channel;
using Pangya_GameServer.Common;
using Pangya_GameServer.Flags;
using Pangya_GameServer.Game.Collections;
using Pangya_GameServer.GPlayer;
using Pangya_GameServer.PacketCreator;
using PangyaAPI.BinaryModels;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Pangya_GameServer.Game
{
    public abstract partial class GameBase
    {
        #region Fields
        public uint ID { get; set; }

        public string Password { get { return fGameData.Password; } }

        public GameInformation GameData { get { return fGameData; } set { fGameData = value; } }

        public GameTypeFlag GameType { get { return fGameData.GameType; } }

        public DateTime GameStart { get; set; }
        public DateTime GameEnd { get; set; }

        public uint GetTrophy { get { { return Trophy; } } }
        public byte Count { get { { return (byte)Players.Count; } } }
        public bool HoleComplete { get; set; }
        // Terminating
        public bool Terminating { get; set; } = false;
        public DateTime TerminateTime { get; set; }
        public bool GameStarted { get { return Started; } }


        protected GameInformation fGameData;
        public Point3D CurrentHole;
        protected uint Owner { get; set; }
        protected bool Started { get; set; }
        protected bool Await { get; set; }
        protected uint Trophy { get; set; }
        protected byte Idle;
        // Trophy Showing
        protected uint Gold { get; set; }
        protected uint Silver1 { get; set; }
        protected uint Silver2 { get; set; }
        protected uint Bronze1 { get; set; }
        protected uint Bronze2 { get; set; }
        protected uint Bronze3 { get; set; }
        // Medal Showing
        protected uint BestRecovery { get; set; }
        protected uint BestChipIn { get; set; }
        protected uint BestDrive { get; set; }
        protected uint BestSpeeder { get; set; }
        protected uint LongestPutt { get; set; }
        protected uint LuckyAward { get; set; }
        protected bool FirstShot { get; set; }
        // Map
        protected byte Map { get; set; }
        public List<Session> Players { get; set; }

        protected List<GameData> PlayerData { get; set; }
        // UID AND GAMEScoreDATA
        protected Dictionary<uint, GameScoreData> Scores { get; set; }
        protected GameHolesCollection Holes { get; set; }


        protected byte[] GameKey { get; set; }
        // Event
        protected GameEvent Create { get; set; }//cria sala
        protected GameEvent Update { get; set; }//update sala
        protected GameEvent Destroy { get; set; }//destroy sala
        // Player Event
        protected PlayerEvent PlayerJoin { get; set; }
        protected PlayerEvent PlayerLeave { get; set; }

        #endregion

        #region Delegate
        public delegate void LobbyEvent(Lobby Lobby, GameBase Game);

        public delegate void GameEvent(GameBase Game);

        public delegate void PlayerEvent(GameBase Game, Session Player);

        #endregion

        #region Abstract Method's
        //Disconnect Game
        public abstract void PlayerGameDisconnect(Session player);
        //create player 
        public abstract void SendPlayerOnCreate(Session player);
        //join player
        public abstract void SendPlayerOnJoin(Session player);
        public abstract void SendHoleData(Session player);
        public abstract void OnPlayerLeave();
        //check room
        public abstract bool Validate();
        //Gera experiencia(XP)
        public abstract void GenerateExperience();
        public abstract void PlayerLoading(Session player, Packet CP);
        public abstract void PlayerShotInfo(Session player, Packet CP);
        public abstract void PlayerShotData(Session player, Packet CP);
        public abstract void PlayerLoadSuccess(Session player);
        public abstract void PlayerLeavePractice();
        public abstract void PlayerStartGame();
        public abstract void PlayerSyncShot(Session player, Packet CP);
        // Final Result       
        public abstract void PlayerSendFinalResult(Session player, Packet CP);
        public abstract byte[] GameInformation();
        public abstract byte[] GetGameHeadData();
        public abstract void DestroyRoom();
        public abstract void AcquireData(Session player);

        #endregion

        #region Construtor

        public GameBase(Session player, GameInformation GameInfo, GameEvent CreateEvent, GameEvent UpdateEvent, GameEvent DestroyEvent, PlayerEvent OnJoin, PlayerEvent OnLeave, ushort GameID)
        { 
        
        }

        #endregion

        #region Handle Reader Packet_Game

        #endregion

        #region Handle Response Packet_Game

        #endregion


        #region Methods

        /// <summary>
        /// Add Player in List
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected bool Add(Session player)
        {
            if (null == player)
            {
                if (GameType == GameTypeFlag.GM_EVENT)
                {
                    SetOwner(uint.MaxValue);
                    return false;
                }
                return false;
            }
            if (Players.Any(c => c.UserInfo.GetLogin == player.UserInfo.GetLogin) == false)
            {

                player.Game = this;
                Players.Add(player);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// set the id of the player who just created the room
        /// </summary>
        /// <param name="UID">Player.GetUID</param>
        protected void SetOwner(uint UID)
        {
            Owner = UID;
        }

        /// <summary>
        /// Arrow the type of player is in the room(byte 8 == adm/gm || byte 1 == normal player)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="IsAdmin"></param>
        protected void SetRole(Session player, bool IsAdmin)
        {
            switch (IsAdmin)
            {
                case true: { player.GameInfo.Role = 0x08; } break;
                case false: { player.GameInfo.Role = 0x01; } break;
            }
            if (GameType == GameTypeFlag.CHAT_ROOM)
            {
                player.GameInfo.GameReady = true;
            }
            if (Players.Count > 1 && player.UserInfo.GetCapability == 4 || player.UserInfo.GetCapability == 15)
            {
                foreach (var client in Players.Where(c => c.GameInfo.Role == 0x08))
                {
                    client.GameInfo.Role = 0x01;
                }
                player.GameInfo.Role = 0x08;
            }
        }

        protected void ComposePlayer()
        {
            byte i = 0;
            foreach (var P in Players)
            {
                i += 1;
                P.GameInfo.GameSlot = i;
            }
        }

        /// <summary>
        /// Creates a key with 16 bytes
        /// I realized that the key is made up of 17 bytes but the last byte is zero, while the other 16 bytes are random
        /// </summary>
        protected void CreateKey()
        {
            var result = new byte[16];

            new Random().NextBytes(result);

            GameKey = result;
        }

        /// <summary>
        /// Update the game head
        /// </summary>
        protected void GameUpdate()
        {
            Send(this.GetGameHeadData());
        }


        /// <summary>
        /// decrypt the shot using the room key
        /// </summary>
        /// <param name="data">bytes packet ShotData</param>
        /// <returns></returns>
        protected byte[] DecryptShot(byte[] data)
        {
            var decrypted = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = (byte)(data[i] ^ GameKey[i % 16]);
            }
            return decrypted;
        }

        /// <summary>
        /// Generates a Type ID of the trophy for each player's level
        /// </summary>
        protected void GenerateTrophy()
        {
            uint SumLevel, AvgLevel;
            SumLevel = 0;

            foreach (var P in Players)
            {
                SumLevel += P.UserInfo.GetLevel;
            }
            if (SumLevel <= 0)
            {
                AvgLevel = 0;
            }
            else
            {
                AvgLevel = (uint)(SumLevel / Players.Count);
            }
            switch (AvgLevel)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    {
                        Trophy = 738197504;
                    }
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    {
                        Trophy = 738263040;
                    }
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    {
                        Trophy = 738328576;
                    }
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    {
                        Trophy = 738394112;
                    }
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    {
                        Trophy = 738459648;
                    }
                    break;
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                    {
                        Trophy = 738525184;
                    }
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                    {
                        Trophy = 738590720;
                    }
                    break;
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                    {
                        Trophy = 738656256;
                    }
                    break;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                    {
                        Trophy = 738721792;
                    }
                    break;
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                    {
                        Trophy = 738787328;
                    }
                    break;
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                    {
                        Trophy = 738852864;
                    }
                    break;
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                    {
                        Trophy = 738918400;
                    }
                    break;
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                    {
                        Trophy = 738983936;
                    }
                    break;
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                    {
                        Trophy = 738983936;
                    }
                    break;
            }
            switch (GameType)
            {
                case GameTypeFlag.CHAT_ROOM:
                case GameTypeFlag.VERSUS_MATCH:
                case GameTypeFlag.VERSUS_STROKE:
                    Trophy = 0;
                    break;
            }
        }


        /// <summary>
        /// Create Holes data
        /// </summary>
        protected void BuildHole()
        {
            Holes.Init(GameData.Mode, GameData.GameType, fGameData.HoleNumber > 0, fGameData.Map, fGameData.HoleTotal);
        }

        /// <summary>
        /// Gets the data from the Holes created
        /// </summary>
        /// <returns></returns>
        protected byte[] GetHoleBuild()
        {
           return Holes.GetData();
        }

        /// <summary>
        /// Checks if there is an item to play in Practice Mode GP
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        protected bool CheckItemPractice(uint TypeID)
        {
            var player = Players.First();
            if (player.Inventory.IsExist(TypeID))
            {
                player.Inventory.Remove(TypeID, (uint)1, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Send Room information to all players in the room
        /// </summary>
        protected void SendGameInfo()
        {
            PangyaBinaryWriter Response;
            Response = new PangyaBinaryWriter();
            try
            {
                Response.Write(new byte[] { 0x49, 0x00, 0x00, 0x00 }); // TODO
                Response.Write(GameInformation());
                Send(Response.GetBytes());
            }
            finally
            {
                Response.Dispose();
            }
        }

        /// <summary>
        /// Send Packet Response
        /// </summary>
        /// <param name="Data">bytes for respose</param>
        public void Send(byte[] Data)
        {
            foreach (var p in Players)
            {
                p.SendResponse(Data);
            }
        }

        public void Send(PangyaBinaryWriter resp)
        {
            foreach (var p in Players)
            {
                p.SendResponse(resp.GetBytes());
            }
            resp.Clear();
        }

        public void Write(byte[] Data)
        {
            foreach (var p in Players)
            {
                p.SendResponse(Data);
            }
        }

        public void ClearPlayerData()
        {
            PlayerData.Clear();
        }

        public void ClearScoresData()
        {
            Scores.Clear();
        }

        public void SendUnfinishedData()
        {
            foreach (var P in PlayerData)
                if (!P.GameCompleted)
                    Send(Creator.ShowHoleData(P.ConnectionID, P.HolePos, (byte)P.ScoreData.TotalShot, (uint)P.ScoreData.Score, P.ScoreData.Pang, P.ScoreData.BonusPang, false));
        }

        public bool _allFinished()
        {
            foreach (var P in Players)
            {
                if (!P.GameInfo.GameCompleted)
                    return false;
            }
            return true;
        }

        public void CopyScore()
        {
            GameData S;

            ClearPlayerData();
            ClearScoresData();
            foreach (var P in Players)
            {
                S = new GameData();

                S = P.GameInfo;
                PlayerData.Add(S);
                Scores.Add(P.UserInfo.GetUID, S.ScoreData);
            }
        }

        public void ClearHole()
        {
            Holes.Dispose();
        }

        public void AddPlayerInEvent(Session player)
        {
            Add(player);

            player.SetGameID((ushort)ID);
            player.GameInfo.SetDefault();
            SetRole(player, true);

            GameUpdate();
            SendGameInfo();
            ComposePlayer();
            SendPlayerOnJoin(player);
        }

        #endregion

    }
}
