using Pangya_LoginServer.LoginPlayer;
using PangyaAPI.Auth.AuthPacket;
using PangyaAPI.Auth.Client;
using PangyaAPI.Auth.Enums;
using PangyaAPI.PangyaClient;
using PangyaAPI.TcpServer;
using PangyaAPI.Tools;
using System;
using System.Net;
using System.Net.Sockets;
namespace Pangya_LoginServer.LoginSocketServer
{
    public class Server : AsyncTcpServer
    {
        public Server(string localIPAddress, int listenPort) : base(localIPAddress, listenPort)
        {
            WriteConsole.WriteLine($"[LOGINSERVER]: START {localIPAddress}:{listenPort}", ConsoleColor.Green);
        }
        public override void DisconnectPlayer(Player session)
        {
            Close(session);
        }

        protected override Player OnConnectPlayer(TcpClient tcp, uint ConnectionID)
        {
            var player = new LPlayer(tcp) {Server = this, ConnectionID = ConnectionID };
            SendKey(player);

            var GetAdress = ((IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString();
            var GetPort = ((IPEndPoint)tcp.Client.RemoteEndPoint).Port.ToString();
            WriteConsole.WriteLine($"[NEW_PLAYER_CONNECT] => {GetAdress}:{GetPort}", ConsoleColor.Cyan);
            return player;
        }

        protected override void SendKey(Player player)
        {
            var US = new byte[] { 0x00, 0x0B, 0x00, 0x00, 0x00, 0x00,player.GetKey, 0x00, 0x00, 0x00, 0x75, 0x27, 0x00, 0x00 };
            if (player.Tcp.Connected)
                //Envia packet com a chave
                player.SendPacket(US);
        }


        protected override ClientAuth AuthServerConstructor()
        {
            return new ClientAuth("Unogames", AuthClientTypeEnum.LoginServer, 30303,
                "3493ef7ca4d69f54de682bee58be4f93");
        }

        protected override void OnAuthServerPacketReceive(ClientAuth client, AuthPacketInfo packet)
        {
            switch (packet.ID)
            {
                case AuthPacketEnum.SERVER_KEEPALIVE:
                    break;
                case AuthPacketEnum.SERVER_CONNECT:
                    break;
                case AuthPacketEnum.SERVER_COMMAND:
                    break;
                case AuthPacketEnum.SERVER_UPDATE:
                    break;
                case AuthPacketEnum.RECEIVES_USER_UID:
                    break;
                case AuthPacketEnum.DISCONNECT_PLAYER_ALL_ON_SERVERS:
                    break;
                case AuthPacketEnum.SERVER_RELEASE_CHAT:
                    break;
                case AuthPacketEnum.SERVER_RELEASE_NOTICE_GM:
                    break;
                case AuthPacketEnum.SERVER_RELEASE_TICKET:
                    break;
                case AuthPacketEnum.SERVER_RELEASE_BOXRANDOM:
                    break;
                case AuthPacketEnum.SERVER_RELEASE_NOTICE:
                    break;
                case AuthPacketEnum.CHECK_PLAYER_DUPLICATE:
                    break;
                case AuthPacketEnum.RESULT_PLAYER_DUPLICATE:
                    break;
                case AuthPacketEnum.SEND_DISCONNECT_PLAYER:
                    break;
                case AuthPacketEnum.LOGIN_RESULT:
                    break;
                case AuthPacketEnum.PLAYER_LOGIN_RESULT:
                    break;
                default:
                    break;
            }
        }
    }
}
