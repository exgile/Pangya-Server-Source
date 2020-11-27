using System;
using System.Runtime.InteropServices;
namespace PangyaAPI.PangyaClient.Data
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StatisticInfo
    {
        public UInt32 Drive { get; set; }
        public UInt32 Putt { get; set; }
        public UInt32 PlayTime { get; set; }
        // Second
        public UInt32 ShotTime { get; set; }
        public float LongestDistance { get; set; }
        public UInt32 Pangya { get; set; }
        public UInt32 TimeOut { get; set; }
        public UInt32 OB { get; set; }
        public UInt32 DistanceTotal { get; set; }
        public UInt32 Hole { get; set; }
        public UInt32 TeamHole { get; set; }
        public UInt32 HIO { get; set; }
        public ushort Bunker { get; set; }
        public UInt32 Fairway { get; set; }
        public UInt32 Albratoss { get; set; }
        public UInt32 Holein { get; set; }
        public UInt32 Puttin { get; set; }
        public float LongestPutt { get; set; }
        public float LongestChip { get; set; }
        public UInt32 EXP { get; set; }
        public byte Level { get; set; }
        public UInt64 Pang { get; set; }
        public UInt32 TotalScore { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x05)]
        public byte[] Score;
        public byte Unknown { get; set; }
        public UInt64 MaxPang0 { get; set; }
        public UInt64 MaxPang1 { get; set; }
        public UInt64 MaxPang2 { get; set; }
        public UInt64 MaxPang3 { get; set; }
        public UInt64 MaxPang4 { get; set; }
        public UInt64 SumPang { get; set; }
        public UInt32 GamePlayed { get; set; }
        public UInt32 Disconnected { get; set; }
        public UInt32 TeamWin { get; set; }
        public UInt32 TeamGame { get; set; }
        public UInt32 LadderPoint { get; set; }
        public UInt32 LadderWin { get; set; }
        public UInt32 LadderLose { get; set; }
        public UInt32 LadderDraw { get; set; }
        public UInt32 LadderHole { get; set; }
        public UInt32 ComboCount { get; set; }
        public UInt32 MaxCombo { get; set; }
        public UInt32 NoMannerGameCount { get; set; }
        public UInt64 SkinsPang { get; set; }
        public UInt32 SkinsWin { get; set; }
        public UInt32 SkinsLose { get; set; }
        public UInt32 SkinsRunHole { get; set; }
        public UInt32 SkinsStrikePoint { get; set; }
        public UInt32 SKinsAllinCount { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x06)]
        public byte[] Unknown1;
        public UInt32 GameCountSeason { get; set; }
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x08)]
        public byte[] Unknown2;

        public static StatisticInfo operator +(StatisticInfo Left, StatisticInfo Right)
        {
            var Result = new StatisticInfo()
            {

                //{ Drive }
                Drive = Left.Drive + Right.Drive,
                //{ Putt}
                Putt = Left.Putt + Right.Putt,
                //{ Player Time Do Nothing }
                PlayTime = Left.PlayTime,
                //{ Shot Time }
                ShotTime = Left.ShotTime + Right.ShotTime
            };
            //{ Longest }
            if (Right.LongestDistance > Left.LongestDistance)
            {
                Result.LongestDistance = Right.LongestDistance;
            }
            else
            {
                Result.LongestDistance = Left.LongestDistance;
            }
            //{ Hit Pangya }
            Result.Pangya = Left.Pangya + Right.Pangya;
            //{ Timeout }
            Result.TimeOut = Left.TimeOut;
            //{ OB }
            Result.OB = Left.OB + Right.OB;
            //{ Total Distance }
            Result.DistanceTotal = Left.DistanceTotal + Right.DistanceTotal;
            //{ Hole Total }
            Result.Hole = Left.Hole + Right.Hole;
            //{ Team Hole }
            Result.TeamHole = Left.TeamHole;
            //{ Hole In One }
            Result.HIO = Left.HIO;
            //{ Bunker }
            Result.Bunker = (ushort)(Left.Bunker + Right.Bunker);
            //{ Fairway }
            Result.Fairway = Left.Fairway + Right.Fairway;
            //{ Albratoss }
            Result.Albratoss = Left.Albratoss + Right.Albratoss;
            //{ Holein ? }
            Result.Holein = Left.Holein + (Result.Hole - Right.Holein);
            //{ Puttin }
            Result.Puttin = Left.Puttin + Right.Puttin;
            //{ Longest Putt }
            if (Right.LongestPutt > Left.LongestPutt)
            {
                Result.LongestPutt = Right.LongestPutt;
            }
            else
            {
                Result.LongestPutt = Left.LongestPutt;
            }
            //{ Longest Chip }
            if (Right.LongestChip > Left.LongestChip)
            {
                Result.LongestChip = Right.LongestChip;
            }
            else
            {
                Result.LongestChip = Left.LongestChip;
            }
            return Result;
        }
    }
}
