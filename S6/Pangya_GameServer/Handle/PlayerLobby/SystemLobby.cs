using Pangya_GameServer.GPlayer;
using Pangya_GameServer.PacketCreator;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;

namespace Pangya_GameServer.Handle.PlayerLobby
{
    public static class SystemLobby
    {
       public static void ProcessLobby(this Session player, Packet packet, bool RequestJoinGameList = false)
        {
            var lp = player.Lobby;

            //Lê Id do Lobby
            if (!packet.ReadByte(out byte lobbyId))
            {
                return;
            }

            var lobby = Program.GameServer.GetLobby(lobbyId);

            if (lp != null)
            {
                lobby.RemovePlayer(player);
            }

            //Caso o lobby não existir
            if (lobby == null)
            {
                player.SendResponse(new byte[] { 0x95, 0x00, 0x02, 0x01, 0x00 });
                WriteConsole.WriteLine("Player Select Invalid Lobby");
                return;
            }
            //Se estiver lotado
            if (lobby.IsFull)
            {
                player.SendResponse(new byte[] { 0x4E, 0x00, 0x02 });
                WriteConsole.WriteLine("Player Selected Lobby Full");
                return;
            }

            // ## add player
            lobby.AddPlayer(player);

            PlayerSelectLobby(player,  RequestJoinGameList);
        }
        static void PlayerSelectLobby(Session player, bool RequestJoinGameList = false)
        {

            try
            {
                player.SendResponse(new byte[] { 0x95, 0x00, 0x02, 0x00, 0x00 });

                player.SendResponse(Creator.ShowEnterLobby(1));

                player.SendResponse(new byte[] { 0xF6, 0x01, 0x00, 0x00, 0x00, 0x00 });
                
                if (RequestJoinGameList == false)
                {
                }
                // ## if request join lobby
                if (RequestJoinGameList)
                {

                }
            }
            finally
            {

            }
        }
    }
}
