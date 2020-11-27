using Pangya_GameServer.Common;
using Pangya_GameServer.Flags;
using Pangya_GameServer.GameTcpServer;
using Pangya_GameServer.Session;
using Pangya_GameServer.Handle.Channel.Collection;
using PangyaAPI.IFF.Manager;
using PangyaAPI.PangyaClient;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
using System.Threading;
namespace Pangya_GameServer
{
    class Program
    {
        static Server GameServer;
        public static IFFFile IFF;

        public static LobbyCollection LobbyList { get; internal set; }

        static void Main(string[] args)
        {
            IFF = new IFFFile();
            GameServer = new Server("127.0.0.1", 20201);
            GameServer.Start();
            GameServer.ClientConnected += ClientConnected;
            GameServer.ClientDisconnected += ClientDisconnected;
            GameServer.OnPacketReceived += OnPacketReceived;
            while (true)
            {
               Thread.Sleep(200);
            }
        }


        private static void ClientDisconnected(Player client)
        {
            var session = (Session)client;
            WriteConsole.WriteLine($"[PLAYER_DISCONNETED]: {session.GetAdress}:{session.GetPort}", ConsoleColor.Red);
        }

        private static void ClientConnected(Player client)
        {
            var session = (Session)client;
            WriteConsole.WriteLine($"[PLAYER_CONNETED]: {session.GetAdress}:{session.GetPort}", ConsoleColor.Green);
        }


        private static void OnPacketReceived(Player client, Packet packet)
        {
            var player = (Session)client;
            WriteConsole.WriteLine($"[PLAYER_PACKET_LOG]: {player.GetAdress}:{player.GetPort}", ConsoleColor.Green);
            packet.Log();
            switch ((GamePacketFlag)packet.Id)
            {
                case GamePacketFlag.PLAYER_LOGIN:
                    {
                        player.Disconnect();
                    }
                    break;
                case GamePacketFlag.PLAYER_CHAT:
                    break;
                case GamePacketFlag.PLAYER_SELECT_LOBBY:
                    break;
                case GamePacketFlag.PLAYER_CREATE_GAME:
                    break;
                case GamePacketFlag.PLAYER_JOIN_GAME:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_NICKNAME:
                    break;
                case GamePacketFlag.PLAYER_EXCEPTION:
                    break;
                case GamePacketFlag.PLAYER_JOIN_MULTIGAME_LIST:
                    break;
                case GamePacketFlag.PLAYER_LEAVE_MULTIGAME_LIST:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_MESSENGER_LIST:
                    break;
                case GamePacketFlag.PLAYER_JOIN_MULTIGAME_GRANDPRIX:
                    break;
                case GamePacketFlag.PLAYER_LEAVE_MULTIGAME_GRANDPRIX:
                    break;
                case GamePacketFlag.PLAYER_ENTER_GRANDPRIX:
                    break;
                case GamePacketFlag.PLAYER_OPEN_PAPEL:
                    break;
                case GamePacketFlag.PLAYER_PLAY_BONGDARI:
                    break;
                case GamePacketFlag.PLAYER_SAVE_MACRO:
                    break;
                case GamePacketFlag.PLAYER_OPEN_MAILBOX:
                    break;
                case GamePacketFlag.PLAYER_READ_MAIL:
                    break;
                case GamePacketFlag.PLAYER_RELEASE_MAILITEM:
                    break;
                case GamePacketFlag.PLAYER_DELETE_MAIL:
                    break;
                case GamePacketFlag.PLAYER_GM_COMMAND:
                    break;
                case GamePacketFlag.PLAYER_USE_ITEM:
                    break;
                case GamePacketFlag.PLAYER_SEND_INVITE_CONFIRM:
                    break;
                case GamePacketFlag.PLAYER_SEND_INVITE:
                    break;
                case GamePacketFlag.PLAYER_PRESS_READY:
                    break;
                case GamePacketFlag.PLAYER_START_GAME:
                    break;
                case GamePacketFlag.PLAYER_LEAVE_GAME:
                    break;
                case GamePacketFlag.PLAYER_KEEPLIVE:
                    break;
                case GamePacketFlag.PLAYER_LOAD_OK:
                    break;
                case GamePacketFlag.PLAYER_SHOT_DATA:
                    break;
                case GamePacketFlag.PLAYER_ACTION:
                    break;
                case GamePacketFlag.PLAYER_ENTER_TO_ROOM:
                    break;
                case GamePacketFlag.PLAYER_CLOSE_SHOP:
                    break;
                case GamePacketFlag.PLAYER_OPEN_SHOP:
                    break;
                case GamePacketFlag.PLAYER_ENTER_SHOP:
                    break;
                case GamePacketFlag.PLAYER_SHOP_CREATE_VISITORS_COUNT:
                    break;
                case GamePacketFlag.PLAYER_EDIT_SHOP_NAME:
                    break;
                case GamePacketFlag.PLAYER_SHOP_VISITORS_COUNT:
                    break;
                case GamePacketFlag.PLAYER_SHOP_ITEMS:
                    break;
                case GamePacketFlag.PLAYER_BUY_SHOP_ITEM:
                    break;
                case GamePacketFlag.PLAYER_SHOP_PANGS:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_CHAT_OFFLINE:
                    break;
                case GamePacketFlag.PLAYER_MASTER_KICK_PLAYER:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_GAME_OPTION:
                    break;
                case GamePacketFlag.PLAYER_LEAVE_GRANDPRIX:
                    break;
                case GamePacketFlag.PLAYER_AFTER_UPLOAD_UCC:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_UPLOAD_KEY:
                    break;
                case GamePacketFlag.PLAYER_1ST_SHOT_READY:
                    break;
                case GamePacketFlag.PLAYER_LOADING_INFO:
                    break;
                case GamePacketFlag.PLAYER_GAME_ROTATE:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_CLUB:
                    break;
                case GamePacketFlag.PLAYER_GAME_MARK:
                    break;
                case GamePacketFlag.PLAYER_ACTION_SHOT:
                    break;
                case GamePacketFlag.PLAYER_SHOT_SYNC:
                    break;
                case GamePacketFlag.PLAYER_HOLE_INFORMATIONS:
                    break;
                case GamePacketFlag.PLAYER_TUTORIAL_MISSION:
                    break;
                case GamePacketFlag.PLAYER_MY_TURN:
                    break;
                case GamePacketFlag.PLAYER_HOLE_COMPLETE:
                    break;
                case GamePacketFlag.PLAYER_CHAT_ICON:
                    break;
                case GamePacketFlag.PLAYER_SLEEP_ICON:
                    break;
                case GamePacketFlag.PLAYER_MATCH_DATA:
                    break;
                case GamePacketFlag.PLAYER_MOVE_BAR:
                    break;
                case GamePacketFlag.PLAYER_PAUSE_GAME:
                    break;
                case GamePacketFlag.PLAYER_QUIT_SINGLE_PLAYER:
                    break;
                case GamePacketFlag.PLAYER_CALL_ASSIST_PUTTING:
                    break;
                case GamePacketFlag.PLAYER_USE_TIMEBOOSTER:
                    break;
                case GamePacketFlag.PLAYER_DROP_BALL:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_TEAM:
                    break;
                case GamePacketFlag.PLAYER_VERSUS_TEAM_SCORE:
                    break;
                case GamePacketFlag.PLAYER_POWER_SHOT:
                    break;
                case GamePacketFlag.PLAYER_WIND_CHANGE:
                    break;
                case GamePacketFlag.PLAYER_SEND_GAMERESULT:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_ANIMALHAND_EFFECT:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_RING_EFFECTS:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_COOKIES_COUNT:
                    break;
                case GamePacketFlag.PLAYER_BUY_ITEM_GAME:
                    break;
                case GamePacketFlag.PLAYER_ENTER_TO_SHOP:
                    break;
                case GamePacketFlag.PLAYER_CHECK_USER_FOR_GIFT:
                    break;
                case GamePacketFlag.PLAYER_SAVE_BAR:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_EQUIPMENT:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_EQUIPMENTS:
                    break;
                case GamePacketFlag.PLAYER_WHISPER:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_TIME:
                    break;
                case GamePacketFlag.PLAYER_GM_DESTROY_ROOM:
                    break;
                case GamePacketFlag.PLAYER_GM_KICK_USER:
                    break;
                case GamePacketFlag.PLAYER_GM_ENTER_ROOM:
                    break;
                case GamePacketFlag.PLAYER_GM_IDENTITY:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_LOBBY_INFO:
                    break;
                case GamePacketFlag.PLAYER_REMOVE_ITEM:
                    break;
                case GamePacketFlag.PLAYER_PLAY_AZTEC_BOX:
                    break;
                case GamePacketFlag.PLAYER_OPEN_BOX:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_SERVER:
                    break;
                case GamePacketFlag.PLAYER_ASSIST_CONTROL:
                    break;
                case GamePacketFlag.PLAYER_OPEN_TIKIPOINT:
                    break;
                case GamePacketFlag.PLAYER_SELECT_LOBBY_WITH_ENTER_TLobby:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_GAMEINFO:
                    break;
                case GamePacketFlag.PLAYER_GM_SEND_NOTICE:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_PLAYERINFO:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_MASCOT_MESSAGE:
                    break;
                case GamePacketFlag.PLAYER_ENTER_ROOM:
                    break;
                case GamePacketFlag.PLAYER_ENTER_ROOM_GETINFO:
                    break;
                case GamePacketFlag.PLAYER_OPENUP_SCRATCHCARD:
                    break;
                case GamePacketFlag.PLAYER_PLAY_SCRATCHCARD:
                    break;
                case GamePacketFlag.PLAYER_ENTER_SCRATCHY_SERIAL:
                    break;
                case GamePacketFlag.PLAYER_FIRST_SET_LOCKER:
                    break;
                case GamePacketFlag.PLAYER_ENTER_TO_LOCKER:
                    break;
                case GamePacketFlag.PLAYER_OPEN_LOCKER:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_LOCKERPWD:
                    break;
                case GamePacketFlag.PLAYER_GET_LOCKERPANG:
                    break;
                case GamePacketFlag.PLAYER_LOCKERPANG_CONTROL:
                    break;
                case GamePacketFlag.PLAYER_CALL_LOCKERITEMLIST:
                    break;
                case GamePacketFlag.PLAYER_PUT_ITEMLOCKER:
                    break;
                case GamePacketFlag.PLAYER_TAKE_ITEMLOCKER:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE_CLUB:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE_ACCEPT:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE_CALCEL:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE_RANK:
                    break;
                case GamePacketFlag.PLAYER_TRASAFER_CLUBPOINT:
                    break;
                case GamePacketFlag.PLAYER_CLUBSET_ABBOT:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_INTRO:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_NOTICE:
                    break;
                case GamePacketFlag.PLAYER_CHANGE_SELFINTRO:
                    break;
                case GamePacketFlag.PLAYER_LEAVE_GUILD:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE:
                    break;
                case GamePacketFlag.PLAYER_CALL_GUILD_LIST:
                    break;
                case GamePacketFlag.PLAYER_SEARCH_GUILD:
                    break;
                case GamePacketFlag.PLAYER_GUILD_AVAIABLE:
                    break;
                case GamePacketFlag.PLAYER_CREATE_GUILD:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_GUILDDATA:
                    break;
                case GamePacketFlag.PLAYER_GUILD_GET_PLAYER:
                    break;
                case GamePacketFlag.PLAYER_GUILD_LOG:
                    break;
                case GamePacketFlag.PLAYER_JOIN_GUILD:
                    break;
                case GamePacketFlag.PLAYER_CANCEL_JOIN_GUILD:
                    break;
                case GamePacketFlag.PLAYER_GUILD_ACCEPT:
                    break;
                case GamePacketFlag.PLAYER_GUILD_KICK:
                    break;
                case GamePacketFlag.PLAYER_GUILD_PROMOTE:
                    break;
                case GamePacketFlag.PLAYER_GUILD_DESTROY:
                    break;
                case GamePacketFlag.PLAYER_GUILD_CALL_UPLOAD:
                    break;
                case GamePacketFlag.PLAYER_GUILD_CALL_AFTER_UPLOAD:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_CHECK_DAILY_ITEM:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_DAILY_REWARD:
                    break;
                case GamePacketFlag.PLAYER_CALL_ACHIEVEMENT:
                    break;
                case GamePacketFlag.PLAYER_OPEN_TIKIREPORT:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_UNKNOWN:
                    break;
                case GamePacketFlag.PLAYER_OPEN_CARD:
                    break;
                case GamePacketFlag.PLAYER_CARD_SPECIAL:
                    break;
                case GamePacketFlag.PLAYER_PUT_CARD:
                    break;
                case GamePacketFlag.PLAYER_PUT_BONUS_CARD:
                    break;
                case GamePacketFlag.PLAYER_REMOVE_CARD:
                    break;
                case GamePacketFlag.PLAYER_LOLO_CARD_DECK:
                    break;
                case GamePacketFlag.PLAYER_CALL_CUTIN:
                    break;
                case GamePacketFlag.PLAYER_DO_MAGICBOX:
                    break;
                case GamePacketFlag.PLAYER_RENEW_RENT:
                    break;
                case GamePacketFlag.PLAYER_DELETE_RENT:
                    break;
                case GamePacketFlag.PLAYER_LOAD_QUEST:
                    break;
                case GamePacketFlag.PLAYER_ACCEPT_QUEST:
                    break;
                case GamePacketFlag.PLAYER_MATCH_HISTORY:
                    break;
                case GamePacketFlag.PLAYER_SEND_TOP_NOTICE:
                    break;
                case GamePacketFlag.PLAYER_CHECK_NOTICE_COOKIE:
                    break;
                case GamePacketFlag.PLAYER_REQUEST_CADDIE_RENEW:
                    break;
                case GamePacketFlag.PLAYER_UPGRADE_STATUS:
                    break;
                case GamePacketFlag.PLAYER_DOWNGRADE_STATUS:
                    break;
                default:
                    break;
            }
        }
    }
}
