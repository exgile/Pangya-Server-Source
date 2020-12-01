using Pangya_GameServer.Common;
using Pangya_GameServer.GPlayer;
using PangyaAPI.PangyaPacket;
using PangyaAPI.SqlConnector.DataBase;
using PangyaAPI.Tools;
using System;
using System.IO;
using System.Linq;
namespace Pangya_GameServer.Handle.PlayerLogin
{
    public static class SystemLogin
    {
        public static void ProcessLogin(this Session session, Packet packet)
        {
            try
            {
                var loginresult = (LoginData)packet.ReadObject(new LoginData());

                if (!Program.CheckVersion(loginresult.Version))
                {
                    session.Send(new byte[] { 0x44, 0x00, 0x0B });
                    session.Disconnect();
                    return;
                }

                session.SetLogin(loginresult.UserName);
                session.SetNickname("MK(e7)");
                session.SetSex((byte)0);
                session.SetCapabilities((byte)4);
                session.SetUID(1);//loginresult.UID
                session.SetCookie((uint)10000000);
                session.LockerPang = (uint)100000;
                session.LockerPWD = "1234";
                session.SetAuthKey1(loginresult.AuthKey_Login);
                session.SetAuthKey2(loginresult.AuthKey_Game);
                if (true)
                {
                    session.InventoryLoad();

                    session.LoadStatistic();

                    //session.LoadGuildData();

                  //  SendJunkPackets(session);

                    PlayerRequestInfo(session, loginresult.Version);
                }
            }
            catch
            {
                session.Disconnect();
            }
        }

        private static void SendJunkPackets(Session session)
        {
            throw new NotImplementedException();
        }

        static void PlayerRequestInfo(Session session, string version)
        {
            ///SEND PLAYER_MAIN_DATA == 0x0044
            session.SendLoginMainData(version, Program.GameServer.Options);

            ///SEND PLAYER_CHARACTERS_DATA == 0x0070
            session.SendCharacterData();

            ///SEND PLAYER_ITEMS_DATA == 0x0073
            session.SendItemsData();

            ///SEND PLAYER_CADDIES_DATA == 0x0071
            session.SendCaddiesData();

            ///SEND PLAYER_EQUIP_DATA == 0x0072
            session.SendEquipmentData();

            ///SEND PLAYER_MASCOTS_DATA == 0x00E1
            session.SendMascotsData();

            ///SEND PLAYER_COOKIES_DATA == 0x0096
            session.SendCookies();

            ///SEND SERVER_LOBBIES_LIST == 0x004D
            session.SendResponse(Program.GameServer.LobbiesList());
        }
    }
}
