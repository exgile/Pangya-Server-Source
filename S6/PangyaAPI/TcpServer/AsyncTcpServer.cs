using PangyaAPI.Auth.AuthPacket;
using PangyaAPI.Auth.Client;
using PangyaAPI.PangyaClient;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace PangyaAPI.TcpServer
{
    public abstract partial class AsyncTcpServer : IDisposable
    {
        #region Fields
        public readonly Dictionary<long, Player> m_Clients;

        private bool disposed;

        private long m_ClientID = 1;

        //tcpListener
        private TcpListener m_Listener;

        public static long s_AllSendBytes = 0;
        public static long s_AllReceBytes = 0;
        private byte[] MsgBufferRead { get; set; }


        public int ClientCount { get; set; }

        public bool IsRunning { get; set; }


        public IPAddress Address { get; }

        public int Port { get; }

        public ClientAuth AuthServer;

        #endregion

        #region Public Delegates
        public delegate void ConnectedEvent(Player player);
        public delegate void DisconnectEvent(Player player);
        public delegate void PacketReceivedEvent(Player player, Packet packet);
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

        #region Abstract Methods
        /// <summary>
        /// Envia chave para o player
        /// </summary>
        protected abstract void SendKey(Player player);

     // protected abstract void SendAuthKey(ClientAuth client);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tcp"></param>
        protected abstract Player OnConnectPlayer(TcpClient tcp, uint ConnectionID);

        public abstract void DisconnectPlayer(Player Player);

        #region AuthServer 

        protected abstract ClientAuth AuthServerConstructor();

        protected abstract void OnAuthServerPacketReceive(ClientAuth client, AuthPacketInfo packet);

        #endregion

        #endregion

        #region Constructor
        public AsyncTcpServer(string localIPAddress, int listenPort)
        {

            Address = IPAddress.Parse(localIPAddress);
            Port = listenPort;

            m_Clients = new Dictionary<long, Player>(32);
            MsgBufferRead = new byte[500000];
            m_Listener = new TcpListener(Address, Port);
            m_Listener.Server.SendBufferSize = 1024 * 8;
            m_Listener.Server.ReceiveBufferSize = 1024 * 8;
            m_Listener.Server.NoDelay = true;
            m_Listener.AllowNatTraversal(true);

            //if (ConnectToAuthServer(AuthServerConstructor()) == false)
            //{
            //    WriteConsole.WriteLine("[ERROR_START_AUTH]: Não foi possível se conectar ao AuthServer");
            //    Console.ReadKey();
            //    Environment.Exit(1);
            //}
        }
        #endregion

        #region Methods
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_Listener.Start();
                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, m_Listener);

            }
        }

        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                m_Listener.Start(backlog);
                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, m_Listener);
            }
        }


        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                m_Listener.Stop();
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
                var _Client = m_Listener.EndAcceptTcpClient(ar);
                if (ClientCount > 10)
                {
                    //TODO
                    _Client.Close();
                    return;
                }


                var _Session = OnConnectPlayer(_Client, (uint)m_ClientID++);

                lock (m_Clients)
                {
                    ClientCount++;
                    m_Clients.Add(_Session.ConnectionID, _Session);
                    OnClientConnected(_Session);
                }

                var _Stream = _Session.NetworkStream;

                _Stream.BeginRead(MsgBufferRead, 0, MsgBufferRead.Length, HandleDataReceived, _Session);

                m_Listener.BeginAcceptTcpClient(HandleTcpClientAccepted, ar.AsyncState);
            }
        }

        private void HandleDataReceived(IAsyncResult ar)
        {
            int bytesRead = 0;
            byte[] message = new byte[0];
            if (IsRunning)
            {
                var _Session = (Player)ar.AsyncState;
                var _Stream = _Session.NetworkStream;

                try
                {
                    bytesRead = _Stream.EndRead(ar);
                    //verifica se o tamanho do pacote é zero
                    if (bytesRead == 0)
                        lock (m_Clients)
                        {
                            m_Clients.Remove(_Session.ConnectionID);

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
                        m_Clients.Remove(_Session.ConnectionID);

                        OnClientDisconnected(_Session);
                        return;
                    }
                }

                //checa se o tamanho da mensagem é zerada
                if (message.Length > 0)
                {
                    var packet = new Packet(message, _Session.GetKey);
                    //Dispara evento OnPacketReceived
                    PlayerRequestPacket(_Session, packet);
                }
            }
        }

        private void OnClientConnected(Player session)
        {
            ClientConnected?.Invoke(session);
        }

        protected void PlayerRequestPacket(Player session, Packet packet)
        {
            OnPacketReceived?.Invoke(session, packet);
        }

        private void OnClientDisconnected(Player session)
        {
            ClientCount--;
            ClientDisconnected?.Invoke(session);
        }

        public void Close(Player pSession, bool pRemove = true)
        {
            if (pSession != null)
            {
                pSession.Close();
                if (pRemove)
                    m_Clients.Remove(pSession.ConnectionID);
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

        /// <summary>
        /// Conecta-se ao AuthServer
        /// </summary>
        public bool ConnectToAuthServer(ClientAuth client)
        {
            AuthServer = client;
            AuthServer.OnDisconnect += OnAuthServerDisconnected;
            AuthServer.OnPacketReceived += AuthServer_OnPacketReceived;
            return AuthServer.Connect();
        }

        /// <summary>
        /// É Disparado quando um packet é recebido do AuthServer
        /// </summary>
        private void AuthServer_OnPacketReceived(ClientAuth authClient, AuthPacketInfo packet)
        {
            OnAuthServerPacketReceive(authClient, packet);
        }

        /// <summary>
        /// É disparado quando não há conexão com o AuthServer
        /// </summary>
        private void OnAuthServerDisconnected()
        {
            Console.WriteLine("Servidor parado.");
            Console.WriteLine("Não foi possível estabelecer conexão com o authServer!");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool pDisposing)
        {
            if (!disposed)
            {
                if (pDisposing)
                    try
                    {
                        Stop();
                        m_Listener = null;
                    }
                    catch (SocketException)
                    {
                    }

                disposed = true;
            }
        }
        #endregion  
    }
}
