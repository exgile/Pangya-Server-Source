using Pangya_GameServer.Flags;
using System.Runtime.InteropServices;
namespace Pangya_GameServer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GameInformation
    {
        public byte Unknown1;
        public uint VSTime;
        public uint GameTime;
        public byte MaxPlayer;
        public GameTypeFlag GameType;
        public byte HoleTotal;
        public GameMapFlag Map;
        public GameModeFlag Mode;
        public bool GMEvent;
        // Hole Repeater
        public byte HoleNumber;
        public uint LockHole;
        // Game Data
        public string Name;
        public string Password;
        public uint Artifact;
        public byte Time30S;
        public bool RoomClosed { get { return Password.Length == 0; } }
    }
}
