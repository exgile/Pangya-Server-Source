using Newtonsoft.Json;
using PangyaAPI.Auth.AuthPacket;
using PangyaAPI.Auth.Client;
using PangyaAPI.Auth.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace PangyaAPI.Auth.TcpServerAuth
{
    public abstract partial class AsyncAuthServer
    {
        #region Public Delegates
        public delegate void ConnectedEvent(ClientAuth player);
        public delegate void DisconnectEvent(ClientAuth player);
        public delegate void PacketReceivedEvent(ClientAuth player, AuthPacketInfo packet);
        #endregion

        #region Public Events
        /// <summary>
        /// Client Connect
        /// </summary>
        public event ConnectedEvent ClientConnected;
        /// <summary>
        /// Client Disconnect
        /// </summary>
        public event DisconnectEvent ClientDisconnected;
        /// <summary>
        /// Receives Client Data 
        /// </summary>
        public event PacketReceivedEvent OnPacketReceived;
        #endregion

        #region Fields

        public byte[] MsgBufferRead { get; private set; }

        /// <summary>
        /// Lista de Clientes conectados
        /// </summary>
        /// 
        private TcpListener _server;

        public ServerInfo Data { get; set; }
        public IPAddress Address { get; }
        public int Port { get; }
        public bool IsRunning { get; private set; }
        public int ClientCount { get; private set; }

        private Dictionary<long, ClientAuth> m_Clients;
        private int s_AllReceBytes;
        #endregion

        #region Constructor

        public AsyncAuthServer(string localIPAddress, int listenPort)
        {
            try
            {
                Address = IPAddress.Parse(localIPAddress);
                Port = listenPort;

                m_Clients = new Dictionary<long, ClientAuth>(32);
                MsgBufferRead = new byte[500000];
                _server = new TcpListener(Address, Port);
                _server.Server.SendBufferSize = 1024 * 8;
                _server.Server.ReceiveBufferSize = 1024 * 8;
                _server.Server.NoDelay = true;
                _server.AllowNatTraversal(true);
            }
            catch (Exception erro)
            {
                Console.WriteLine($"ERRO_START: {erro.Message}");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        #endregion

        #region Methods
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _server.Start();
                _server.BeginAcceptTcpClient(HandleTcpClientAccepted, _server);
            }
        }

        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _server.Start(backlog);
                _server.BeginAcceptTcpClient(HandleTcpClientAccepted, _server);
            }
        }


        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _server.Stop();
                lock (m_Clients)
                {
                    CloseAllClient();
                }
            }
        }

        private void HandleTcpClientAccepted(IAsyncResult ar)
        {
            if (IsRunning)
            {
                var _Client = _server.EndAcceptTcpClient(ar);
                if (ClientCount > 10)
                {
                    //TODO
                    _Client.Close();
                    return;
                }


                var _Session = new ClientAuth(_Client, new AuthPacketInfo() { });

                lock (m_Clients)
                {
                    ClientCount++;
                    m_Clients.Add(_Session.Info.UID, _Session);
                    OnClientConnected(_Session);
                }

                var _Stream = _Session.NetworkStream;

                _Stream.BeginRead(MsgBufferRead, 0, MsgBufferRead.Length, HandleDataReceived, _Session);

                _server.BeginAcceptTcpClient(HandleTcpClientAccepted, ar.AsyncState);
            }
        }

        private void HandleDataReceived(IAsyncResult ar)
        {
            int bytesRead = 0;
            byte[] message = new byte[0];
            if (IsRunning)
            {
                var _Session = (ClientAuth)ar.AsyncState;
                var _Stream = _Session.NetworkStream;

                try
                {
                    bytesRead = _Stream.EndRead(ar);
                    //verifica se o tamanho do pacote é zero
                    if (bytesRead == 0)
                        lock (m_Clients)
                        {
                            m_Clients.Remove(_Session.Info.UID);

                            OnClientDisconnected(_Session);
                            return;
                        }

                    //variável para armazenar a mensagem recebida
                    message = new byte[bytesRead];

                    //Copia mensagem recebida
                    Buffer.BlockCopy(MsgBufferRead, 0, message, 0, bytesRead);

                    s_AllReceBytes += bytesRead;
                    //ler o proximo pacote(se houver)
                    _Stream.BeginRead(MsgBufferRead, 0, MsgBufferRead.Length, HandleDataReceived, _Session);
                }
                catch
                {
                    lock (m_Clients)
                    {
                        m_Clients.Remove(_Session.Info.UID);

                        OnClientDisconnected(_Session);
                        return;
                    }
                }

                //checa se o tamanho da mensagem é zerada
                if (message.Length > 0)
                {
                    var json = System.Text.Encoding.Default.GetString(message);

                    var packet = JsonConvert.DeserializeObject<AuthPacketInfo>(json);

                    //Dispara evento OnPacketReceived
                    ClientRequestPacket(_Session, packet);
                }
            }
        }

        private void OnClientConnected(ClientAuth session)
        {
            ClientConnected?.Invoke(session);
        }

        protected void ClientRequestPacket(ClientAuth session, AuthPacketInfo packet)
        {
            OnPacketReceived?.Invoke(session, packet);
        }

        private void OnClientDisconnected(ClientAuth session)
        {
            ClientCount--;
            ClientDisconnected?.Invoke(session);
        }

        public void Close(ClientAuth pSession, bool pRemove = true)
        {
            if (pSession != null)
            {
                pSession.Close();
                if (pRemove)
                    m_Clients.Remove(pSession.Info.UID);
                OnClientDisconnected(pSession);
            }
        }

        public void CloseAllClient()
        {
            foreach (var _Session in m_Clients)
                Close(_Session.Value, false);
            ClientCount = 0;
            m_Clients.Clear();
        }
        #endregion
    }
}
