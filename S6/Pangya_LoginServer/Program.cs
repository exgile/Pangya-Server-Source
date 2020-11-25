using Pangya_LoginServer.Handles;
using Pangya_LoginServer.LoginPlayer;
using Pangya_LoginServer.LoginSocketServer;
using PangyaAPI.IFF.Manager;
using PangyaAPI.PangyaClient;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
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
                System.Threading.Thread.Sleep(200);
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
            switch (packet.Id)
            {
                /// <summary>
                /// Player digita o usuário e senha e clica em login
                /// </summary>
                case 0x01:
                    if (player.LoginResult(packet))
                    {
                        player.LoginSucess();
                    }
                    break;
                /// <summary>
                /// Player Seleciona um Servidor para entrar
                /// </summary>
                case 0x03:
                    player.SelectServer(packet);
                    break;

                /// <summary>
                /// login com duplicidade 
                /// </summary>
                case 0x04:
                    break;

                /// <summary>
                /// Seta primeiro nickname do usuário
                /// </summary>
                case 0x06:
                    player.SetNickName(packet);
                    break;

                /// <summary>
                /// Ocorre quando o cliente clica em Confirmar (se o nickname está disponível), 
                /// </summary>
                case 0x07:
                    player.ConfirmNickName(packet);
                    break;
                /// <summary>
                /// Player selecionou seu primeiro personagem
                /// </summary>
                case 0x08:
                    player.CharacaterCreate(packet);
                    break;
                /// <summary>
                /// envia chave de autenficação do login e lista novamente os servers
                /// </summary>
                case 0x0B:
                    break;
                /// <summary>
                /// ?????????
                /// </summary>
                case 0xFF:
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
