using Pangya_LoginServer.Flags;
using Pangya_LoginServer.Handles;
using Pangya_LoginServer.LoginPlayer;
using Pangya_LoginServer.LoginTcpServer;
using PangyaAPI.IFF.Manager;
using PangyaAPI.PangyaClient;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
using System.Threading;
namespace Pangya_LoginServer
{
    class Program
    {
        static Server _server;
        public static IFFFile IFF;
        static void Main()
        {
            IFF = new IFFFile();
            _server = new Server("127.0.0.1", 10103);
            _server.Start();
            _server.ClientConnected += ClientConnected;
            _server.ClientDisconnected += ClientDisconnected;
            _server.OnPacketReceived += OnPacketReceived;
            while (true)
            {
                Thread.Sleep(200);
            }
        }

        private static void ClientDisconnected(Player client)
        {
            var session = (LPlayer)client;
            WriteConsole.WriteLine($"[PLAYER_DISCONNETED]: {session.GetAdress}:{session.GetPort}", ConsoleColor.Red);
        }

        private static void ClientConnected(Player client)
        {
            var session = (LPlayer)client;
            WriteConsole.WriteLine($"[PLAYER_CONNETED]: {session.GetAdress}:{session.GetPort}", ConsoleColor.Green);
        }

        private static void OnPacketReceived(Player client, Packet packet)
        {
            var player = (LPlayer)client;
            WriteConsole.WriteLine($"[PLAYER_PACKET_LOG]: {player.GetAdress}:{player.GetPort}", ConsoleColor.Green);
            packet.Log();
            switch ((LoginPacketFlag)packet.Id)
            {
                case LoginPacketFlag.PLAYER_LOGIN:
                    if (player.LoginResult(packet))
                    {
                        player.LoginSucess();
                    }
                    break;
                case LoginPacketFlag.PLAYER_SELECT_SERVER:
                    player.SelectServer(packet);
                    break;
                case (LoginPacketFlag.PLAYER_DUPLCATE_LOGIN):
                    break;
                case (LoginPacketFlag.PLAYER_SET_NICKNAME):
                    player.SetNickName(packet);
                    break;
                case LoginPacketFlag.PLAYER_CONFIRM_NICKNAME:
                    player.ConfirmNickName(packet);
                    break;
                case LoginPacketFlag.PLAYER_SELECT_CHARACTER:
                    player.CharacaterCreate(packet);
                    break;
                case LoginPacketFlag.PLAYER_RECONNECT:
                    break;
                case LoginPacketFlag.NOTHING:
                default:
                    {
                        Console.WriteLine(BitConverter.ToString(packet.Message));
                        player.SendResponse(new byte[0]);
                        player.Disconnect();
                    }
                    break;
            }
        }
    }
}
