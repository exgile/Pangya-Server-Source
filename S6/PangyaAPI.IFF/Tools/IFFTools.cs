using PangyaAPI.IFF.Flags;
using System;
using System.Runtime.InteropServices;

namespace PangyaAPI.IFF.Tools
{
    public static class IFFTools
    {
        public static uint GenerateNewTypeID(this uint iffType, int characterId, int int_0, int group, int type, int serial)
        {
            if (group - 1 < 0)
            {
                group = 0;
            }
            return Convert.ToUInt32((iffType * Math.Pow(2.0, 26.0)) + (characterId * Math.Pow(2.0, 18.0)) + (int_0 * Math.Pow(2.0, 13.0)) + (group * Math.Pow(2.0, 11.0)) + (type * Math.Pow(2.0, 9.0)) + serial);
        }

        public static uint TypeIdItem(this uint TypeID)
        {
            return (uint)((int)((TypeID & 0x3fc0000) / Math.Pow(2.0, 18.0)));
        }
     
        public static uint[] GetTypeIdValues(this uint TypeID)
        {
            uint[] _TypeIDValues = new uint[6];
            _TypeIDValues[0] = ((uint)((TypeID & 0x3fc0000) / Math.Pow(2.0, 18.0)));
            _TypeIDValues[1] = (ushort)((TypeID & 0x3fc0000) / Math.Pow(2.0, 18.0));
            _TypeIDValues[2] = (ushort)((TypeID & 0xfc000000) / Math.Pow(2.0, 26.0));
            _TypeIDValues[3] = (ushort)((TypeID & 0x1f0000) / Math.Pow(2.0, 16.0));
            _TypeIDValues[4] = (ushort)((TypeID & 0x3e003) / Math.Pow(2.0, 13.0));
            _TypeIDValues[5] = (ushort)(TypeID & 0xff);
            return _TypeIDValues;
        }

        public static uint SizeStruct(object obj)
        {
            return (uint)Marshal.SizeOf(obj);
        }

        public static IffGroupFlag GetItemGroup(this uint TypeId)
        {
            uint result;
            result = (uint)Math.Round((TypeId & 0xFC000000) / Math.Pow(2.0, 26.0));
            return (IffGroupFlag)result;
        }
    }
}
