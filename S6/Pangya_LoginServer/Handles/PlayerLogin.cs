using Pangya_LoginServer.LoginPlayer;
using PangyaAPI.BinaryModels;
using PangyaAPI.PangyaPacket;
using System;
using System.IO;
using System.Linq;

namespace Pangya_LoginServer.Handles
{
    public static class PlayerLogin
    {
        public static bool LoginResult(this LPlayer session, Packet packet)
        {
            if (!packet.ReadPStr(out session.GetLogin))
            {
                return false;
            }

            if (!packet.ReadPStr(out session.GetPass))
            {
                return false;
            }

            Console.WriteLine($"Login packet: {session.GetLogin}:{session.GetPass}");
            var count = Directory.GetFiles(@"Packets", "*.*", SearchOption.TopDirectoryOnly).Count();
            if (count >= 7)
            {
                if (File.Exists("Packets\\Login.hex"))
                {
                    var reader = new PangyaBinaryReader(new MemoryStream(File.ReadAllBytes("Packets\\Login.hex")));
                    reader.Skip(3);
                    var GetLogin = reader.ReadPStr();
                    session.GetUID = reader.ReadUInt32();
                    session.GetCapability = (byte)reader.ReadUInt32();
                    session.GetLevel = (byte)reader.ReadUInt32();
                    reader.Skip(6);
                    var GetNickname = reader.ReadPStr();
                    if (string.IsNullOrEmpty(GetNickname))
                    {
                        session.Response.Write(new byte[] { 0x01, 0x00 });
                        session.Response.WriteByte((byte)0xD8);//Call Create NickName
                        session.Response.WriteInt32(0);
                        session.SavePacket("PlayerCallCreateNickName");
                        session.SendResponse();
                        return false;
                    }
                    else
                    {
                        session.GetNickname = GetNickname;
                        return true;
                    }

                }
            }

            if (string.IsNullOrEmpty(session.GetNickname))
            {
                session.Response.Write(new byte[] { 0x01, 0x00 });
                session.Response.WriteByte((byte)0xD8);//Call Create NickName
                session.Response.WriteInt32(0);
                session.SavePacket("PlayerCallCreateNickName");
                session.SendResponse();
                return false;
            }
            return true;
        }
    }
}
