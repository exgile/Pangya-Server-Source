using System;
using System.Linq;
using Pangya_GameServer.Game;
using Pangya_GameServer.GPlayer;
using PangyaAPI.Dispose.Collection;
using Pangya_GameServer.Game.Collections;
using Pangya_GameServer.Channel.Interface;
namespace Pangya_GameServer.Channel
{
    public class Lobby : ILobby
    {
        public DisposableCollection<Session> Players { get; set; }

        public GameCollection Games { get; set; }
        public Lobby this[Session player] => Program.LobbyList.GetLobby(player.Lobby);
        public GameBase this[ushort GameID] => GetGameHandle(GameID);

        public bool IsFull { get { return Players.Count >= MaxPlayers; } }
        public byte PlayersCount { get { return (byte)Players.Model.Where(c => c.InLobby == true).ToList().Count(); } }
        public byte Id { get; set; }

        public string Name { get; set; }

        public ushort MaxPlayers { get; set; }

        public uint Flag { get; set; }

        public byte PlayersInLobby => throw new System.NotImplementedException();

        public Lobby(string name, ushort maxPlayers, byte id, uint flag)
        {
            Name = name;
            MaxPlayers = maxPlayers;
            Id = id;
            Flag = flag;
            Players = new DisposableCollection<Session>();
        }
        public bool AddPlayer(Session player)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Build()
        {
            throw new System.NotImplementedException();
        }

        public byte[] BuildGameLists()
        {
            throw new System.NotImplementedException();
        }

        public byte[] BuildPlayerLists()
        {
            throw new System.NotImplementedException();
        }

        public void CreateGameEvent(GameBase GameHandle)
        {
            throw new System.NotImplementedException();
        }

        public void DestroyGame(GameBase GameHandle)
        {
            throw new System.NotImplementedException();
        }

        public void DestroyGameEvent(GameBase GameHandle)
        {
            throw new System.NotImplementedException();
        }

        public void PlayerJoinGameEvent(GameBase GameHandle, Session player)
        {
            throw new System.NotImplementedException();
        }

        public void PlayerLeaveGameEvent(GameBase GameHandle, Session player)
        {
            throw new System.NotImplementedException();
        }

        public void Send(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateGameEvent(GameBase GameHandle)
        {
            throw new System.NotImplementedException();
        }


        private GameBase GetGameHandle(ushort gameID)
        {
            throw new NotImplementedException();
        }
    }
}
