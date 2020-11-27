using Pangya_GameServer.Game;
using Pangya_GameServer.GPlayer;
namespace Pangya_GameServer.Channel.Interface
{
    interface ILobby
    {
        byte PlayersCount { get; }

        byte Id { get; set; }

        string Name { get; set; }

        ushort MaxPlayers { get; set; }

        uint Flag { get; set; }
        /// <summary>
        /// Counter
        /// </summary>
        byte PlayersInLobby { get; }
        bool IsFull { get; }

        
        /// <summary>
        /// Construi as informações do lobby
        /// </summary>
        /// <returns></returns>
        byte[] Build();
        /// <summary>
        /// Obtem a lista de jogos no lobby
        /// </summary>
        /// <returns></returns>
        byte[] BuildGameLists();

        /// <summary>
        /// Obtem a lista de jogadores no lobby
        /// </summary>
        /// <returns></returns>
        byte[] BuildPlayerLists();

        /// <summary>
        /// cria a sala
        /// </summary>
        /// <param name="GameHandle"></param>
        void CreateGameEvent(GameBase GameHandle);
        /// <summary>
        /// Destruir a sala
        /// </summary>
        /// <param name="GameHandle"></param>
        void DestroyGame(GameBase GameHandle);

        void UpdateGameEvent(GameBase GameHandle);


        void DestroyGameEvent(GameBase GameHandle);

        void PlayerJoinGameEvent(GameBase GameHandle, Session player);

        void PlayerLeaveGameEvent(GameBase GameHandle, Session player);

        void Send(byte[] data);

        bool AddPlayer(Session player);
    }
}
