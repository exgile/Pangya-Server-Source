using System.Runtime.InteropServices;
namespace Pangya_GameServer.Common
{
    public class LoginData
    {
        public string UserName { get; set; }
        public uint UID { get; set; }
        public uint Blank1 { get; set; }
        public ushort CheckUnknow1 { get; set; }
        public string AuthKey_Login { get; set; }
        public string Version { get; set; }
        public uint CheckUnknow2 { get; set; }
        public uint Blank2 { get; set; }
        public string AuthKey_Game { get; set; }
    }
}
