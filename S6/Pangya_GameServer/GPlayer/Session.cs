using Pangya_GameServer.Common;
using Pangya_GameServer.Models.Channel.Model;
using Pangya_GameServer.Models.Game;
using Pangya_GameServer.Models.Game.Model;
using Pangya_GameServer.Models.Player;
using PangyaAPI.PangyaClient;
using System;
using System.Net.Sockets;
namespace Pangya_GameServer.GPlayer
{
    public partial class Session : Player
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
            InventoryLoad();
        }
    }
}
