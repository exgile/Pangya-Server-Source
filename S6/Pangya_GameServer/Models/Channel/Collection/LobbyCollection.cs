using Pangya_GameServer.Models.Channel.Model;
using System.Collections.Generic;
namespace Pangya_GameServer.Models.Channel.Collection
{
    public class LobbyCollection : List<Lobby>
    {
        public LobbyCollection()
        {

        }

        public Lobby GetLobby(Lobby lobby)
        {
            return lobby;
        }
    }
}
