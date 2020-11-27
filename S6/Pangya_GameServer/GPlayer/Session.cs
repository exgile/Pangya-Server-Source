using Pangya_GameServer.Channel;
using Pangya_GameServer.Common;
using Pangya_GameServer.Game;
using Pangya_GameServer.Models;
using PangyaAPI.PangyaClient;
using System;
using System.Net.Sockets;
namespace Pangya_GameServer.GPlayer
{
    public class Session : Player
    {
        #region Fields 

        public ushort GameID { get; set; }

        public Lobby Lobby { get; set; }

        public bool InLobby { get; set; }

        public GameBase Game { get; set; }

        public Inventory Inventory { get; set; }

        public GameData GameInfo;
        #endregion
        public Session(TcpClient pTcpClient) : base(pTcpClient)
        {
        }

        internal bool GetGameInfomations(int v)
        {
            throw new NotImplementedException();
        }

        internal byte[] GetLobbyInfo()
        {
            throw new NotImplementedException();
        }

        public void SetGameID(ushort ID)
        {
            GameID = ID;
        }
    }
}
