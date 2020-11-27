using System;
using System.Collections.Generic;
namespace Pangya_GameServer.Channel.Collection
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
