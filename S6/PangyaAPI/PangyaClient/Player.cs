using PangyaAPI.BinaryModels;
using PangyaAPI.Crypt;
using PangyaAPI.TcpServer;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace PangyaAPI.PangyaClient
{
    public partial class Player
    {
        /// <summary>
        /// Chave de criptografia e decriptografia
        /// </summary>
        byte Key { get; set; }

        public PangyaBinaryWriter Response { get; set; }
        /// <summary>
        /// Identificador da conexão
        /// </summary>
        public uint ConnectionID { get; set; }
        public uint GetUID { get; set; }
        public byte GetKey { get { return Key; } set { Key = value; } }
        public byte GetFirstLogin { get; set; }
        public string GetLogin = string.Empty;
        public string GetPass = string.Empty;
        public string GetNickname = string.Empty;
        public byte GetSex { get; set; }
        public byte GetCapability { get; set; } = 0;
        public byte GetLevel { get; set; } = 0;
        public string GetAuth1 { get; set; } = String.Empty;
        public string GetAuth2 { get; set; } = String.Empty;

        public TcpClient Tcp { get; set; }

        public NetworkStream NetworkStream => Tcp.GetStream();

        public AsyncTcpServer Server { get; set; }

        /// </summary>
        public string GetAdress
        {
            get { return ((IPEndPoint)Tcp.Client.RemoteEndPoint).Address.ToString(); }
        }

        public string GetPort
        {
            get { return ((IPEndPoint)Tcp.Client.RemoteEndPoint).Port.ToString(); }
        }
        public Player(TcpClient pTcpClient)
        {
            Tcp = pTcpClient ?? throw new ArgumentNullException(nameof(pTcpClient));

            //Max Value hexadecimal value: FF (255)
            Key = Convert.ToByte(new Random().Next(1, 15));
            Response = new PangyaBinaryWriter(new MemoryStream());
        }


        public void Close()
        {
            if (Tcp.Connected)
            {
                Tcp.Close();
            }
        }

        /// <summary>
        /// not compression
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        public void SendPacket(byte[] data)
        {
            Send(data);
        }

        public void SendResponse()
        {
            var DataCompression = Crypt.ServerCipher.Encrypt(Response.GetBytes(), GetKey, 0);
            Response.Clear();
            Send(DataCompression);
        }

        public void SendResponse(byte[] data)
        {
            var DataCompression = Crypt.ServerCipher.Encrypt(data, GetKey, 0);
            Send(DataCompression);
        }


        public void Send(byte[] data)
        {
            Tcp.GetStream().BeginWrite(data, 0, data.Length, SendDataEnd, Tcp);
        }

        void SendDataEnd(IAsyncResult ar)
        {
            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);           
        }

        public void SavePacket(string name)
        {
            File.WriteAllBytes($"Packets/{name}.hex", Response.GetBytes());
        }
        public void SavePacket(byte[] data, string name)
        {
            File.WriteAllBytes($"Packets/{name}.hex", data);
        }

        public void Disconnect()
        {
            Server.DisconnectPlayer(this);
        }
    }
}
