using PangyaAPI.Json;
using PangyaAPI.Auth.AuthPacket;
using PangyaAPI.Auth.Common;
using PangyaAPI.Auth.Flags;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
namespace PangyaAPI.Auth.Client
{
    public class ClientAuth : IDisposable
    {
        #region Delegates
        public delegate void DisconnectedEvent();
        public delegate void PacketReceivedEvent(ClientAuth authClient, AuthPacketInfo packet);
        #endregion

        #region Events

        /// <summary>
        /// Este evento ocorre quando o ProjectG se conecta ao Servidor
        /// </summary>
        public event DisconnectedEvent OnDisconnect;

        /// <summary>
        /// Este evento ocorre quando o client recebe um Packet do AuthServer
        /// </summary>
        public event PacketReceivedEvent OnPacketReceived;
        #endregion

        #region Public Fields
        /// <summary>
        /// Servers Information
        /// </summary>
        public ServerInfo Info { get; set; } = new ServerInfo();
        //Conexão
        public TcpClient Tcp;

        public AuthPacketInfo CurrentPacket { get; set; }

        public NetworkStream NetworkStream { get { return this.Tcp.GetStream(); } }

        public string Key { get; set; }

        public string Name { get; set; }

        public AuthClientTypeEnum Type { get; set; }

        public int Port { get; set; }

        #endregion

        #region Constructor's
        public ClientAuth(ServerInfo _client)
        {
            Tcp = new TcpClient();
            Info = _client;
        }

        public ClientAuth(string name, AuthClientTypeEnum type, int port, string key)
        {
            Tcp = new TcpClient();
            Name = name;
            Type = type;
            Port = port;
            Key = key;
        }

        public ClientAuth(TcpClient client, AuthPacketInfo packet)
        {
            Tcp = client;
            ServerInfo ClientData;

            ClientData = new ServerInfo()
            {
                UID = packet.Message._data.UID,
                Type = packet.Message._data.Type,
                AuthServer_Ip = packet.Message._data.AuthServer_Ip,
                AuthServer_Port = packet.Message._data.AuthServer_Port,
                Port = packet.Message._data.Port,
                MaxPlayers = packet.Message._data.MaxPlayers,
                IP = packet.Message._data.IP,
                Key = packet.Message._data.Key,
                Name = packet.Message._data.Name,
                BlockFunc = packet.Message._data.BlockFunc,
                EventFlag = packet.Message._data.EventFlag,
                GameVersion = packet.Message._data.GameVersion,
                ImgNo = packet.Message._data.ImgNo,
                Property = packet.Message._data.Property,
                Version = packet.Message._data.Version,
            };
            Info = ClientData;
        }
        #endregion

        #region Methods 
        
        #region Private
        /// <summary>
        /// Verifica de tempo em tempo se o AuthServer ainda está conectado.
        /// </summary>
        private void KeepAlive()
        {
            while (true)
            {
                //Aguarda tempo
                Thread.Sleep(TimeSpan.FromSeconds(5));

                try
                {
                    //Send KeepAlive
                    Send(new AuthPacketInfo()
                    {
                        ID = AuthPacketEnum.SERVER_KEEPALIVE
                    });
                }
                catch
                {
                    //Dispara evento quando não há conexão
                    OnDisconnect?.Invoke();
                }
            }
        }

        private void HandleAuthClient()
        {
            //Inicia Thread KeepAlive
            var keepAliveThread = new Thread(new ThreadStart(KeepAlive));
            keepAliveThread.Start();

            while (Tcp.Connected)
            {
                try
                {
                    var messageBufferRead = new byte[500000]; //Tamanho do BUFFER á ler

                    //Lê mensagem do cliente
                    var bytesRead = this.NetworkStream.Read(messageBufferRead, 0, 500000);

                    //variável para armazenar a mensagem recebida
                    var message = new byte[bytesRead];

                    //Copia mensagem recebida
                    Buffer.BlockCopy(messageBufferRead, 0, message, 0, bytesRead);

                    var json = System.Text.Encoding.Default.GetString(message);

                    var response = JsonConvert.Deserialize<AuthPacketInfo>(json);

                    //Dispara evento OnPacketReceived
                    OnPacketReceived?.Invoke(this, response);
                }
                catch
                {
                    OnDisconnect?.Invoke();
                }
            }
        }

        #endregion

        #region Public


        /// <summary>
        /// Conecta-se ao AuthServer
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                Tcp.Connect("127.0.0.1", Port);

                var packet = new AuthPacketInfo()
                {
                    ID = AuthPacketEnum.SERVER_CONNECT,
                    Message = new
                    {
                        ServerName = Name,
                        ServerType = Type,
                        Key
                    }
                };

                var response = SendAndReceive(packet);

                if (response.Message.Success == true)
                {
                    //Inicia Thread KeepAlive
                    var authClienthread = new Thread(new ThreadStart(HandleAuthClient));
                    authClienthread.Start();

                    return true;
                }
                else
                {
                    Console.WriteLine(response.Message.Exception);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Envia Packet sem aguardar uma Resposta
        /// </summary>
        public void Send(AuthPacketInfo packet)
        {
            var _stream = Tcp.GetStream();

            var json = JsonConvert.Serialize(packet);


            var result = json.Select(Convert.ToByte).ToArray();
            NetworkStream.Write(result, 0, result.Length);
        }

        /// <summary>
        /// Envia Packet aguardando uma resposta
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public AuthPacketInfo SendAndReceive(AuthPacketInfo packet)
        {
            Send(packet);

            var messageBufferRead = new byte[500000]; //Tamanho do BUFFER á ler

            //Lê mensagem do cliente
            var bytesRead = this.NetworkStream.Read(messageBufferRead, 0, 500000);

            //variável para armazenar a mensagem recebida
            var message = new byte[bytesRead];

            //Copia mensagem recebida
            Buffer.BlockCopy(messageBufferRead, 0, message, 0, bytesRead);

            var json = System.Text.Encoding.Default.GetString(message);

            var response = JsonConvert.Deserialize<AuthPacketInfo>(json);

            return response;
        }

        internal void Close()
        {
            if (Tcp.Connected)
            {
                Tcp.Close();
            }
        }

        #endregion

        #region IDisposable
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
                    Tcp.Dispose();
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

        // C#
        ~ClientAuth()
        {
            Dispose(false);
        }


        #endregion

        #endregion
    }
}
