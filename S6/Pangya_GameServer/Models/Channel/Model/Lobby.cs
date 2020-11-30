using System;
using PangyaAPI.Dispose.Collection;
using Pangya_GameServer.GPlayer;
using Pangya_GameServer.Models.Channel.Interface;
using Pangya_GameServer.Models.Game.Collections;
using Pangya_GameServer.Models.Game.Model;
using System.Linq;

namespace Pangya_GameServer.Models.Channel.Model
{
    public class Lobby : ILobby
    {
        public DisposableCollection<Session> s { get; set; }

        public GameCollection Games { get; set; }
        public Lobby this[Session player] => Program.LobbyList.GetLobby(player.Lobby);
        public GameBase this[ushort GameID] => GetGameHandle(GameID);

        public bool IsFull { get { return s.Count >= Maxs; } }
        public byte sCount { get { return (byte)s.Model.Where(c => c.InLobby == true).ToList().Count(); } }
        public byte Id { get; set; }

        public string Name { get; set; }

        public ushort Maxs { get; set; }

        public uint Flag { get; set; }

        public byte sInLobby => throw new System.NotImplementedException();

        public Lobby(string name, ushort maxs, byte id, uint flag)
        {
            Name = name;
            Maxs = maxs;
            Id = id;
            Flag = flag;
            s = new DisposableCollection<Session>();
        }
        public bool Add(Session player)
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

        public byte[] BuildLists()
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

        public void JoinGameEvent(GameBase GameHandle, Session player)
        {
            throw new System.NotImplementedException();
        }

        public void LeaveGameEvent(GameBase GameHandle, Session player)
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
