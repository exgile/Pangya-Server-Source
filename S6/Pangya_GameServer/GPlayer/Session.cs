using Pangya_GameServer.Common;
using Pangya_GameServer.Models.Channel.Model;
using Pangya_GameServer.Models.Game;
using Pangya_GameServer.Models.Game.Model;
using Pangya_GameServer.Models.Player;
using PangyaAPI.PangyaClient;
using PangyaAPI.SqlConnector.DataBase;
using System;
using System.Net.Sockets;
namespace Pangya_GameServer.GPlayer
{
    public partial class Session : Player
    {
        #region Fields 

        public string GetSubLogin { get { return "@" + UserInfo.GetLogin; } }

        public ushort GameID { get; set; }

        public ulong LockerPang { get; set; }

        public Lobby Lobby { get; set; }

        public bool InLobby { get; set; }

        public GameBase Game { get; set; }

        public Inventory Inventory { get; set; }
        public string LockerPWD { get; internal set; }

        public GameData GameInfo;

        public GuildData GuildInfo;

        public PangyaEntities _db;

        #endregion
        public Session(TcpClient pTcpClient) : base(pTcpClient)
        {
            _db = new PangyaEntities();
        }
    }
}
