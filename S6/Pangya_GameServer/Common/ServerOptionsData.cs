using Pangya_GameServer.Flags;
using PangyaAPI.BinaryModels;
using System.Runtime.InteropServices;
namespace Pangya_GameServer.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServerOptionsData
    {
        public uint Unknown0;
        public uint Unknown1;
        public ushort Unknown2;
        public uint Unknown3;
        public ServerOptionFlag MaintenanceFlags;
        public uint Unknown4;
        public uint Property;

        public void Set(ServerOptionFlag FuncFlags, uint property)
        {
            Unknown0 = 2;
            Unknown1 = uint.MaxValue;
            Unknown2 = ushort.MaxValue;
            Unknown3 = 0;
            Unknown4 = 0;
            MaintenanceFlags = FuncFlags;
            Property = property;
        }

        public byte[] GetInfo()
        {
            using (var resp = new PangyaBinaryWriter())
            {
                resp.WriteStruct(this);
                return resp.GetBytes();
            }
        }
    }
}
