using PangyaAPI.BinaryModels;
using PangyaAPI.PangyaClient;
using PangyaAPI.PangyaPacket;
using PangyaAPI.TcpServer;
using System;
using System.IO;
using System.Net.Sockets;

namespace Pangya_LoginServer.LoginPlayer
{
    public class LPlayer : Player
    {
        public LPlayer(TcpClient pTcpClient) : base(pTcpClient)
        {
        }

    }
}
