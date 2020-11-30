using Pangya_GameServer.GPlayer;
using Pangya_GameServer.Models.Game.Model;
namespace Pangya_GameServer.Models.Channel.Interface
{
    interface ILobby
    {
        byte sCount { get; }

        byte Id { get; set; }

        string Name { get; set; }

        ushort Maxs { get; set; }

        uint Flag { get; set; }
        /// <summary>
        /// Counter
        /// </summary>
        byte sInLobby { get; }
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
        byte[] BuildLists();

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

        void JoinGameEvent(GameBase GameHandle, Session player);

        void LeaveGameEvent(GameBase GameHandle, Session player);

        void Send(byte[] data);

        bool Add(Session player);
    }
}
