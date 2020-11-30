using PangyaAPI.IFF.Manager;
using System;

namespace PangyaAPI.IFF
{
    /// <summary>
    /// static class for load iff
    /// </summary>
    public class IFFEntry
    {
        public static IFFFile GetIff;

        static IFFEntry()
        {
            GetIff = new IFFFile();
        }

        public static void Load()
        {
            GetIff.LoadIff();
        }

        public static void Load(string Filename)
        {
            GetIff.SetFileName(Filename);
            GetIff.LoadIff();
        }
    }
}
