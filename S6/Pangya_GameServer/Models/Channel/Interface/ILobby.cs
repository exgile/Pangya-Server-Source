namespace Pangya_GameServer.Models.Channel.Interface
{
    interface ILobby
    {
        /// <summary>
        /// Lobby ID
        /// </summary>
        byte Id { get; set; }

        /// <summary>
        /// Lobby Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Max Players in Lobby
        /// </summary>
        ushort MaxPlayers { get; set; }

        /// <summary>
        /// Lobby Type(Flag)
        /// </summary>
        uint Flag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        byte Count { get; }

        /// <summary>
        /// Lobby is full
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Build the lobby information
        /// </summary>
        /// <returns></returns>
        byte[] Build();
                
    }
}
