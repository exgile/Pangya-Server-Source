using PangyaAPI.IFF.Common;
using System.Runtime.InteropServices;
namespace PangyaAPI.IFF.Models
{
    /// <summary>
    /// Is Struct file Course.iff
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Course
    {
        [field: MarshalAs(UnmanagedType.Struct)]
        public IFFCommon Base;//iff base data
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Mpet;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Mpet2;
        public byte Unknown1;
        [field: MarshalAs(UnmanagedType.ByValTStr, SizeConst = 43)]
        public string XML;
        public float PangMap;
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] Unknown2;
    }
}
