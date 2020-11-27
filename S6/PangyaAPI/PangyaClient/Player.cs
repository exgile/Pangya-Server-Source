using PangyaAPI.BinaryModels;
using PangyaAPI.Dispose;
using PangyaAPI.PangyaClient.Data;
using PangyaAPI.TcpServer;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace PangyaAPI.PangyaClient
{
    public partial class Player : IDisposeable
    {
        #region Field
       
        /// <summary>
        /// Chave de criptografia e decriptografia
        /// </summary>
        public byte Key { get; private set; }

        /// <summary>
        /// Identificador da conexão
        /// </summary>
        public uint ConnectionID { get; set; }

        /// <summary>
        /// Informações do usuario
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// Escreve os pacotes de resposta
        /// </summary>
        public PangyaBinaryWriter Response { get; set; }

        /// <summary>
        /// Conexao TCP
        /// </summary>
        public TcpClient Tcp { get; set; }

        public NetworkStream NetworkStream
        {
            get
            {
                return Tcp.GetStream();
            }
        }

        public AsyncTcpServer Server { get; set; }

        /// </summary>
        public string GetAdress
        {
            get
            {
                if (Tcp.Connected)
                {
                    return ((IPEndPoint)Tcp.Client.RemoteEndPoint).Address.ToString();
                }
                else
                {
                    return "IP NULL";
                }
            }
        }

        public string GetPort
        {
            get
            {
                if (Tcp.Connected)
                {
                    return ((IPEndPoint)Tcp.Client.RemoteEndPoint).Port.ToString();
                }
                else
                {
                    return "PORT NULL";
                }
            }
        }

        #endregion

        #region Constructor

        public Player(TcpClient pTcpClient)
        {
            Tcp = pTcpClient ?? throw new ArgumentNullException(nameof(pTcpClient));

            UserInfo = new UserInfo();

            //Max Value hexadecimal value: FF (255)
            Key = Convert.ToByte(new Random().Next(1, 15));

            Response = new PangyaBinaryWriter(new MemoryStream());

            if (Directory.Exists("Packets") == false)
            {
                Directory.CreateDirectory("Packets");
            }
        }

        #endregion

        #region Methods

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
            var DataCompression = Crypt.ServerCipher.Encrypt(Response.GetBytes(), Key, 0);
            Response.Clear();
            Send(DataCompression);
        }

        public void SendResponse(byte[] data)
        {
            var DataCompression = Crypt.ServerCipher.Encrypt(data, Key, 0);
            Send(DataCompression);
        }


        public void Send(byte[] data)
        {
            if (Tcp.Connected)
            {
                Tcp.GetStream().BeginWrite(data, 0, data.Length, SendDataEnd, Tcp);
            }
        }

        void SendDataEnd(IAsyncResult ar)
        {
            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
        }

        #endregion

        #region SavePackets

        public void SavePacket(string name)
        {
            File.WriteAllBytes($"Packets/{name}.hex", Response.GetBytes());
        }
        public void SavePacket(byte[] data, string name)
        {
            File.WriteAllBytes($"Packets/{name}.hex", data);
        }

        #endregion

        #region Disconnect Player

        public void Disconnect()
        {
            Server.DisconnectPlayer(this);
        }

        #endregion

        #region Set
        public void SetCookie(uint Cookie)
        {
            UserInfo.GetCookie = Cookie;
        }

        public bool SetAuthKey1(string TAUTH_KEY_1)
        {
            UserInfo.GetAuth1 = TAUTH_KEY_1;
            return true;
        }

        public bool SetAuthKey2(string TAUTH_KEY_2)
        {
            UserInfo.GetAuth2 = TAUTH_KEY_2;
            return true;
        }

        public bool SetCapabilities(byte TCapa)
        {
            UserInfo.GetCapability = TCapa;
            if (TCapa == 4)
            {
                UserInfo.Visible = 0;
            }
            return true;
        }

        public void SetExp(uint Amount)
        {
            UserInfo.UserStatistic.EXP = Amount;
        }
     
        public void SetLevel(byte Amount)
        {
            UserInfo.UserStatistic.Level = Amount;
        }

        public bool SetLogin(string TLogin)
        {
            UserInfo.GetLogin = TLogin;
            return true;
        }

        public bool SetNickname(string TNickname)
        {
            UserInfo.GetNickname = TNickname;
            return true;
        }

        public bool SetSex(Byte TSex)
        {
            UserInfo.GetSex = TSex;
            return true;
        }

        public bool SetUID(uint TUID)
        {
            UserInfo.GetUID = TUID;
            return true;
        }

        #endregion

        #region Dispose

        // booleano para controlar se
        // o método Dispose já foi chamado
        public bool Disposed { get; set; }
        // método privado para controle
        // da liberação dos recursos
        private void Dispose(bool disposing)
        {
            // Verifique se Dispose já foi chamado.
            if (!this.Disposed)
            {
                if (disposing)
                {
                    // Liberando recursos gerenciados
                    Tcp.Client.Dispose();
                }

                // Seta a variável booleana para true,
                // indicando que os recursos já foram liberados
                Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor
        /// </summary>
        ~Player()
        {
            Dispose(false);
        }

        #endregion
    }
}
