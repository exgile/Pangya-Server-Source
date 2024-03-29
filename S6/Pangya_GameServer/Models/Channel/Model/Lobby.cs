﻿using System;
using PangyaAPI.Dispose.Collection;
using Pangya_GameServer.GPlayer;
using Pangya_GameServer.Models.Channel.Interface;
using Pangya_GameServer.Models.Game.Collections;
using Pangya_GameServer.Models.Game.Model;
using System.Linq;
using PangyaAPI.BinaryModels;
using Pangya_GameServer.Flags;
using Pangya_GameServer.PacketCreator;
using Pangya_GameServer.Common;
using PangyaAPI.Tools;
using PangyaAPI.PangyaPacket;
namespace Pangya_GameServer.Models.Channel.Model
{
    public class Lobby : ILobby
    {
        public DisposableCollection<Session> Players { get; set; }
        public GameCollection Games { get; set; }
        public Lobby this[Session player] => Program.GameServer.LobbyList.GetLobby(player.Lobby);
        public GameBase this[ushort GameID] => GetGameHandle(GameID);
        public bool IsFull { get { return Players.Count >= MaxPlayers; } }
        public byte Count { get { return (byte)PlayersInLobby(); } }
        public byte Id { get; set; }

        public string Name { get; set; }

        public ushort MaxPlayers { get; set; }

        public uint Flag { get; set; }

        /// <summary>
        /// Create Channel/Lobby
        /// </summary>
        /// <param name="name">lobby name</param>
        /// <param name="maxPlayers">lobby max players</param>
        /// <param name="id">lobby id</param>
        /// <param name="flag">Lobby Type/Flag</param>
        public Lobby(string name, ushort maxPlayers, byte id, uint flag)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            Id = id;
            Flag = flag;
            Games = new GameCollection(this);
            Players = new DisposableCollection<Session>(10);
        }

        public bool AddPlayer(Session player)
        {
            if (Players.Model.Any(c => c.UserInfo.GetUID == player.UserInfo.GetUID) == false)
            {
                Players.Add(player);
                player.Lobby = this; //Define no Player qual lobby ele está
                return true;
            }
            return false;
        }


        public byte[] Build()
        {
            return Creator.LobbyInfo(Name, MaxPlayers, Convert.ToUInt16(Players.Count), Id, Flag);
        }

        public byte[] BuildGameLists()
        {
          return  Games.GameAction();
        }

        public byte[] BuildPlayerLists()
        {
            var result = new PangyaBinaryWriter();
            result.Write(new byte[] { 0x46, 0x00 });
            result.WriteByte(4);
            result.WriteByte(Count);
            foreach (Session player in Players.Model)
            {
                if (player.InLobby)
                {
                    result.Write(player.GetLobbyInfo());
                }
            }
            return result.GetBytes();
        }


        public void DestroyGame(GameBase GameHandle)
        {
            DestroyGameEvent(GameHandle);

            GameHandle.Terminating = true;
            GameHandle.TerminateTime = DateTime.Now;
            //{ remove from game list }
            Games.GameRemove(GameHandle);
        }


        public GameBase GetGameHandle(Session player)
        {
            return Games.GetByID(player.GameID);
        }

        public GameBase GetGameHandle(ushort GameID)
        {
            return Games.GetByID(GameID);
        }

        public void PlayerLeaveGame(Session player)
        {
            var GameHandle = player.Game;

            if (GameHandle == null)
            {
                return;
            }
            GameHandle.RemovePlayer(player);
        }

        public void PlayerLeaveGP(Session player)
        {
            var GameHandle = player.Game;

            if (GameHandle == null)
            {
                return;
            }

            GameHandle.RemovePlayer(player);

            PlayerGetTime(player);
            player.SendResponse(new byte[] { 0x54, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF });
        }

        public void PlayerJoinGame(Session player, Packet packet)
        {
            packet.ReadUInt16(out ushort GameID);
            packet.ReadPStr(out string Pass);

            GameBase GameHandle = this[GameID];

            if (GameHandle == null)
            {
                player.SendResponse(Creator.ShowRoomError(GameCreateResultFlag.CREATE_GAME_ROOM_DONT_EXISTS));
                return;
            }

            if (GameHandle.Password.Length > 0 && player.UserInfo.GetCapability < 4)
            {
                if (GameHandle.Password != Pass)
                {
                    player.SendResponse(Creator.ShowRoomError(GameCreateResultFlag.CREATE_GAME_INCORRECT_PASSWORD));

                    return;
                }
            }

            if (GameHandle.Count == 0 && GameHandle.GameType == GameTypeFlag.GM_EVENT)
            {
                GameHandle.AddPlayerInEvent(player);
            }
            else
            {
                GameHandle.AddPlayer(player);
            }

        }

        public void PlayerRequestGameInfo(Session player, Packet packet)
        {
            packet.ReadUInt16(out ushort GameID);

            GameBase GameHandle = this[GameID];
            if (GameHandle == null)
                return;


            player.SendResponse(GameHandle.GetGameInfo()); // : TODO ---------------------
        }

        public void JoinMultiplayerGamesList(Session player)
        {
            WriteConsole.WriteLine($"[PLAYER_LOBBY_LIST]: TOTAL GAMES IN LOBBY => {Games.Count}", ConsoleColor.Green);

            if (player.InLobby)
                return;
            // Set Current User To Lobby
            player.InLobby = true;
            //Send List Players in Lobby
            Send(BuildPlayerLists());

            player.SendResponse(BuildGameLists());

            if (Players.Count > 1)
                Send(Creator.ShowPlayerAction(player, LobbyActionFlag.CREATE));
        }

        public void LeaveMultiplayerGamesList(Session player)
        {
            if (player.InLobby)
            {
                player.InLobby = false; // Set Current User To Lobby
                // Send to All player
                Send(Creator.ShowPlayerAction(player, LobbyActionFlag.DESTROY));
            }
        }

        internal GameBase CreateGame(Session player, GameInformation GameData)
        {
            if (Games.Limit)
            {
                player.SendResponse(Creator.ShowRoomError(GameCreateResultFlag.CREATE_GAME_CREATE_FAILED2));
                return null;
            }

            GameBase result = null;
            switch (GameData.GameType)
            {
                case GameTypeFlag.VERSUS_STROKE:
                case GameTypeFlag.VERSUS_MATCH:
                    {
                      //  result = new ModeVersus(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.TOURNEY_TEAM:
                case GameTypeFlag.TOURNEY:
                    {
                        //result = new ModeTorney(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.CHAT_ROOM:
                    {
                       // result = new ModeChatRoom(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.TOURNEY_GUILD:
                    {
                      //  result = new ModeGuildBattle(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.GM_EVENT:
                    {
                      //  result = new ModeGMEvent(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.CHIP_IN_PRACTICE:
                    {
                      //  result = new ModeChipInPractice(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.SSC:
                    {
                      //  result = new ModeSSC(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                case GameTypeFlag.HOLE_REPEAT:
                    {
                       // result = new ModePractice(player, GameData, CreateGameEvent, UpdateGameEvent, DestroyGame, PlayerJoinGameEvent, PlayerLeaveGameEvent, Games.GetID);
                    }
                    break;
                default:
                    {
                        player.SendResponse(Creator.ShowRoomError(GameCreateResultFlag.CREATE_GAME_CANT_CREATE));
                        return null;
                    }

            }
            Games.Create(result);
            return result;
        }

        public void CreateGameEvent(GameBase GameHandle)
        {
            if (GameHandle == null || GameHandle.GameType == GameTypeFlag.HOLE_REPEAT)
            {
                return;
            }
            Send(Creator.ShowGameAction(GameHandle.GameInformation(), GameActionFlag.CREATE));
        }

        public void DestroyGameEvent(GameBase GameHandle)
        {
            if (GameHandle == null || GameHandle.GameType == GameTypeFlag.HOLE_REPEAT)
            {
                return;
            }
            Send(Creator.ShowGameAction(GameHandle.GameInformation(), GameActionFlag.DESTROY));
        }


        public void PlayerJoinGameEvent(GameBase GameHandle, Session player)
        {
            if (GameHandle == null || GameHandle.GameType == GameTypeFlag.HOLE_REPEAT)
            {
                return;
            }
            Send(Creator.ShowPlayerAction(player, LobbyActionFlag.UPDATE));
        }

        public void PlayerLeaveGameEvent(GameBase GameHandle, Session player)
        {
            if (GameHandle == null || GameHandle.GameType == GameTypeFlag.HOLE_REPEAT)
            {
                return;
            }
            Send(Creator.ShowPlayerAction(player, LobbyActionFlag.UPDATE));
        }

        public void UpdateGameEvent(GameBase GameHandle)
        {
            if (GameHandle == null || GameHandle.GameType == GameTypeFlag.HOLE_REPEAT)
            {
                return;
            }
            Send(Creator.ShowGameAction(GameHandle.GameInformation(), GameActionFlag.UPDATE));
        }

        public void PlayerSendChat(Session player, string Messages)
        {
            if (player.Game != null)
            {
                var GameHandle = player.Game;
                GameHandle.Write(Creator.ChatText(player.UserInfo.GetNickname, Messages, player.UserInfo.GetCapability == 4 || player.UserInfo.GetCapability == 15));
                DebugCommand(player, Messages, player.UserInfo.GetCapability == 4 || player.UserInfo.GetCapability == 15);

            }
            else
            {
                foreach (Session Client in this.Players.Model)
                {
                    if (Client.InLobby && Client.GameID == 0xFFFF)
                    {
                        Client.SendResponse(Creator.ChatText(player.UserInfo.GetNickname, Messages, player.UserInfo.GetCapability == 4 || player.UserInfo.GetCapability == 15));
                    }
                }
                DebugCommand(player, Messages, player.UserInfo.GetCapability == 4 || player.UserInfo.GetCapability == 15);
            }
        }


        public void PlayerSendWhisper(Session player, string Nickname, string Messages)
        {
            PangyaBinaryWriter Response;
            Session Client;


            Response = new PangyaBinaryWriter();

            try
            {
                Client = (Session)player;


                if (Client == null)
                {
                    Response.Write(new byte[] { 0x40, 0x00 });
                    Response.WriteByte(5); //Status          
                    Response.WritePStr(Nickname);
                    player.SendResponse(Response);
                    return;
                }
                if (!Client.InLobby)
                {
                    Response.Write(new byte[] { 0x40, 0x00 });
                    Response.WriteByte(4); //Status          
                    Response.WritePStr(Nickname);
                    player.SendResponse(Response);
                    return;
                }

                Response = new PangyaBinaryWriter();
                Response.Write(new byte[] { 0x84, 0x00 });
                Response.WriteByte(0); //atingir player       
                Response.WritePStr(player.UserInfo.GetNickname);
                Response.WritePStr(Messages);
                Client.SendResponse(Response);

                Response = new PangyaBinaryWriter();
                Response.Write(new byte[] { 0x84, 0x00 });
                Response.WriteByte(1);//atingir player             
                Response.WritePStr(Nickname);
                Response.WritePStr(Messages);
                player.SendResponse(Response);
            }
            finally
            {
                if (Response != null)
                {
                    Response.Dispose();
                }
            }
        }
        public void RemovePlayer(Session player)
        {
            Program.GameServer.LobbyList.HandleRemoveLobbyPlayer(player);

            GameBase GameHandle = player.Game;

            if (GameHandle != null)
            {
                GameHandle.RemovePlayer(player);
            }

            if (player.InLobby)
            {
                LeaveMultiplayerGamesList(player);
            }
        }
        public void PlayerGetTime(Session player)
        {
            player.Response.Write(new byte[] { 0xBA, 0x00 });
            player.Response.WriteTime();
            player.SendResponse();
        }
        public void UpdatePlayerLobbyInfo(Session player)
        {
            if (player.InLobby)
            {
                Send(Creator.ShowPlayerAction(player, LobbyActionFlag.UPDATE));
                player.SendResponse(new byte[] { 0x0F, 0x00 });
            }
        }
        public int PlayersInLobby()
        {
            return Players.Model.Where(c => c.InLobby == true).ToList().Count;
        }

        public void Send(byte[] data)
        {
            foreach (Session client in Players.Model)
                client.SendResponse(data);
        }

        /// <summary>
        /// Command Chat Debug, Using in chat : command @sendpangs 1000
        /// </summary>
        protected void DebugCommand(Session client, string message, bool IsAdmin)
        {
            string[] Command;
            string ReadCommand = "";

            Command = message.Split(new char[] { ' ' });

            
            if (Command.Length > 1)
            {
                ReadCommand = Command[1].ToUpper();
            }
            if (Command[0].ToUpper() == "COMMAND" && IsAdmin)
            {
                switch (ReadCommand)
                {
                    case "@SENDPANGS":
                        {
                            if (Command.Length >= 3)
                            {
                                var pang = uint.Parse(Command[2]);

                                if (pang <= 0) { return; }

                                foreach (Session player in Players.Model)
                                {
                                    if (player.InLobby)
                                    {
                                        player.AddPang(pang);
                                    }
                                    player.SendPang();
                                }
                            }
                        }
                        break;
                    case "@SENDCOOKIES":
                        {
                            if (Command.Length >= 3)
                            {
                                var cookies = uint.Parse(Command[2]);

                                if (cookies <= 0) { return; }

                                foreach (Session player in Players.Model)
                                {
                                    if (player.InLobby)
                                    {
                                        player.AddCookie(cookies);
                                    }
                                    player.SendCookies();
                                }
                            }
                        }
                        break;
                    case "@WAREHOUSE":
                        {
                            if (Command.Length >= 3)
                            {
                                var typeID = uint.Parse(Command[2]);

                                if (typeID <= 0) { return; }

                            }
                        }
                        break;
                    default:
                        {
                            WriteConsole.WriteLine("[COMMAND GM] Unknown !");
                            return;
                        }
                }
                WriteConsole.WriteLine("[COMMAND GM] Comando de GM recebido de: " + client.UserInfo.GetLogin);
            }
        }
    }
}
