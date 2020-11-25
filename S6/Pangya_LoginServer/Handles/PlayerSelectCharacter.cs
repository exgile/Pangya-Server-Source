using Pangya_LoginServer.LoginPlayer;
using PangyaAPI.PangyaPacket;
using PangyaAPI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangya_LoginServer.Handles
{
    public static class PlayerSelectCharacter
    {
        public static void CharacaterCreate(this LPlayer session, Packet ClientPacket)
        {
            if (!ClientPacket.ReadUInt32(out uint CHAR_TYPEID))
            {
                return;
            }
            if (!ClientPacket.ReadUInt16(out ushort HAIR_COLOR))
            {
                return;
            }

            try
            {
                WriteConsole.WriteLine($"[PLAYER_CREATE_CHARACTER]: {Program.IFF.GetName(CHAR_TYPEID).ToUpper()}");
                if (string.IsNullOrEmpty(session.GetNickname) == false)
                {
                    session.LoginSucess();
                }
                else
                {
                    session.Disconnect();
                    return;
                }
            }
            catch
            {
                session.Disconnect();
            }
        }
    }
}
