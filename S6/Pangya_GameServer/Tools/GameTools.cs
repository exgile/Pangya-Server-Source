﻿using Pangya_GameServer.Flags;
using System;
namespace Pangya_GameServer.Tools
{
    public static class GameTools
    {
        public static int[] _THole18 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
        public static int[] _TMap19 = { 0x14, 0x12, 0x13, 0x10, 0x0F, 0x0E, 0x0D, 0x0B, 0x08, 0x0A, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x09 };

        public static ushort GetMap()
        {
            var Map = _TMap19;
            byte I;
            byte S;
            byte A;
            byte B;

            for (I = 0; I <= _TMap19.Length - 1; I++)
            {
                S = (byte)new Random().Next(_TMap19.Length);
                A = (byte)Map[S];
                B = (byte)Map[I];
                Map[I] = A;
                Map[S] = B;
            }
            return (ushort)(Map[new Random().Next(Map.Length)]);
        }

        public static int[] RandomHole()
        {
            int I;
            var Values = _THole18;
            for (I = 0; I <= _THole18.GetUpperBound(0); I++)
            {
                var Rnd = new Random();
                if (I != _THole18.Length)
                {
                    SwapX(ref Values[I], ref Values[I + Rnd.Next(Values.Length - I)]);
                }
            }
            return Values;
        }

        public static int[] RandomMap()
        {
            int I;
            var Values = _TMap19;
            for (I = 0; I <= _TMap19.GetUpperBound(0); I++)
                SwapX(ref Values[I], ref Values[I + new Random().Next(Values.Length - I)]);
            return Values;
        }

        public static void SwapX(ref int lhs, ref int rhs)
        {
            int tmp;
            tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

    }
   
    public static class GameExpTable
    {
        // ExpTable
        // $0 = Blue Lagoon 1 Star
        // $E = Ice Spa 1 Star
        // $F = Last Seaway 1 Star
        // $B = Pink Wind 1 Star
        // $5 = West Wiz 1 Star
        // $8 = Ice Cannon 2 Stars
        // $A = Shining Sand 2 Stars
        // $10 = Eastern Valley 2 Stars
        // $13 = Wiz City 2 Stars
        // $14 = Aboot Mine 2 Stars
        // $6 = Blue Moon 3 Stars
        // $1 = Blue Water 3 Stars
        // $2 = Sepia Wind 3 Stars
        // $9 = White Wiz 3 Stars
        // $7 = Silvia Canon 4 Stars
        // $4 = WizWiz 4 Stars
        // $12 = Ice Inferno 4 Stars
        // $D = Deep Inferno 5 Stars
        // $3 = Wind Hill 6 Stars
        static byte GetDifficulty(byte Map)
        {
            var type = new byte();
            switch (Map)
            {
                case 0x0:
                case 0xE:
                case 0xF:
                case 0xB:
                case 0x5:
                    {
                        type = 1;
                    }
                    break;
                case 0x8:
                case 0xA:
                case 0x10:
                case 0x13:
                case 0x14:
                    {
                        type = 2;
                    }
                    break;
                case 0x6:
                case 0x1:
                case 0x2:
                case 0x9:
                    {
                        type = 3;
                    }
                    break;
                case 0x7:
                case 0x4:
                case 0x12:
                    {
                        type = 4;
                    }
                    break;
                case 0xD:
                case 0x3:
                    {
                        type = 5;
                    }
                    break;
            }
            return type;
        }

        public static byte GetEXP(GameTypeFlag GameType, byte Map, byte Rank, byte SumPlayer, byte SumHole)
        {
            byte[] exp_3h;
            byte[] exp_6h;
            byte[] exp_9h;
            byte[] exp_18h;
            switch (GameType)
            {
                case GameTypeFlag.VERSUS_STROKE:
                    // Default
                    exp_3h = new byte[] { 0, 0, 0, 0 };
                    exp_6h = new byte[] { 0, 0, 0, 0 };
                    exp_9h = new byte[] { 0, 0, 0, 0 };
                    exp_18h = new byte[] { 0, 0, 0, 0 };
                    switch (GetDifficulty(Map))
                    {
                        case 1:
                            // Declare Versus Mode Exp Table
                            switch (SumPlayer)
                            {
                                case 2:
                                    exp_3h = new byte[] { 7, 6, 0, 0 };
                                    exp_6h = new byte[] { 14, 10, 0, 0 };
                                    exp_9h = new byte[] { 20, 15, 0, 0 };
                                    exp_18h = new byte[] { 40, 29, 0, 0 };
                                    break;
                                case 3:
                                    exp_3h = new byte[] { 9, 8, 7, 0 };
                                    exp_6h = new byte[] { 17, 15, 13, 0 };
                                    exp_9h = new byte[] { 25, 22, 18, 0 };
                                    exp_18h = new byte[] { 50, 43, 36, 0 };
                                    break;
                                case 4:
                                    exp_3h = new byte[] { 11, 10, 9, 8 };
                                    exp_6h = new byte[] { 21, 19, 17, 15 };
                                    exp_9h = new byte[] { 31, 29, 25, 22 };
                                    exp_18h = new byte[] { 62, 56, 49, 42 };
                                    break;
                            }
                            break;
                        case 2:
                            switch (SumPlayer)
                            {
                                case 2:
                                    exp_3h = new byte[] { 8, 6, 0, 0 };
                                    exp_6h = new byte[] { 14, 11, 0, 0 };
                                    exp_9h = new byte[] { 21, 16, 0, 0 };
                                    exp_18h = new byte[] { 41, 30, 0, 0 };
                                    break;
                                case 3:
                                    exp_3h = new byte[] { 9, 8, 7, 0 };
                                    exp_6h = new byte[] { 18, 16, 13, 0 };
                                    exp_9h = new byte[] { 26, 23, 19, 0 };
                                    exp_18h = new byte[] { 52, 45, 38, 0 };
                                    break;
                                case 4:
                                    exp_3h = new byte[] { 12, 11, 9, 8 };
                                    exp_6h = new byte[] { 22, 20, 18, 16 };
                                    exp_9h = new byte[] { 33, 30, 26, 23 };
                                    exp_18h = new byte[] { 65, 59, 52, 45 };
                                    break;
                            }
                            break;
                        case 3:
                            switch (SumPlayer)
                            {
                                case 2:
                                    exp_3h = new byte[] { 8, 6, 0, 0 };
                                    exp_6h = new byte[] { 15, 11, 0, 0 };
                                    exp_9h = new byte[] { 22, 16, 0, 0 };
                                    exp_18h = new byte[] { 43, 32, 0, 0 };
                                    break;
                                case 3:
                                    exp_3h = new byte[] { 10, 9, 7, 0 };
                                    exp_6h = new byte[] { 19, 16, 14, 0 };
                                    exp_9h = new byte[] { 27, 24, 20, 0 };
                                    exp_18h = new byte[] { 54, 47, 40, 0 };
                                    break;
                                case 4:
                                    exp_3h = new byte[] { 12, 11, 10, 9 };
                                    exp_6h = new byte[] { 23, 21, 19, 16 };
                                    exp_9h = new byte[] { 34, 31, 28, 24 };
                                    exp_18h = new byte[] { 67, 62, 55, 47 };
                                    break;
                            }
                            break;
                        case 4:
                            switch (SumPlayer)
                            {
                                case 2:
                                    exp_3h = new byte[] { 8, 7, 0, 0 };
                                    exp_6h = new byte[] { 16, 12, 0, 0 };
                                    exp_9h = new byte[] { 23, 18, 0, 0 };
                                    exp_18h = new byte[] { 45, 35, 0, 0 };
                                    break;
                                case 3:
                                    exp_3h = new byte[] { 11, 9, 8, 0 };
                                    exp_6h = new byte[] { 20, 18, 15, 0 };
                                    exp_9h = new byte[] { 30, 26, 22, 0 };
                                    exp_18h = new byte[] { 58, 51, 44, 0 };
                                    break;
                                case 4:
                                    exp_3h = new byte[] { 13, 12, 11, 10 };
                                    exp_6h = new byte[] { 25, 23, 21, 18 };
                                    exp_9h = new byte[] { 37, 34, 30, 27 };
                                    exp_18h = new byte[] { 72, 67, 60, 53 };
                                    break;
                            }
                            break;
                        case 5:
                            switch (SumPlayer)
                            {
                                case 2:
                                    exp_3h = new byte[] { 9, 7, 0, 0 };
                                    exp_6h = new byte[] { 17, 14, 0, 0 };
                                    exp_9h = new byte[] { 25, 20, 0, 0 };
                                    exp_18h = new byte[] { 49, 39, 0, 0 };
                                    break;
                                case 3:
                                    exp_3h = new byte[] { 12, 10, 9, 0 };
                                    exp_6h = new byte[] { 22, 20, 17, 0 };
                                    exp_9h = new byte[] { 33, 29, 25, 0 };
                                    exp_18h = new byte[] { 64, 57, 50, 0 };
                                    break;
                                case 4:
                                    exp_3h = new byte[] { 14, 13, 12, 11 };
                                    exp_6h = new byte[] { 27, 26, 23, 21 };
                                    exp_9h = new byte[] { 41, 38, 34, 31 };
                                    exp_18h = new byte[] { 80, 75, 68, 61 };
                                    break;
                            }
                            break;
                    }
                    switch (SumHole)
                    {
                        case 3:
                            {
                                return exp_3h[Rank - 1];
                            }
                        case 6:
                            {
                                return exp_6h[Rank - 1];
                            }
                        case 9:
                            {
                                return exp_9h[Rank - 1];
                            }
                        case 18:
                            {
                                return exp_18h[Rank - 1];
                            }
                        default:
                            {
                                return 0;
                            }
                    }

                case GameTypeFlag.VERSUS_MATCH:
                    break;
                case GameTypeFlag.CHAT_ROOM:
                    break;
                case GameTypeFlag.TOURNEY:
                    break;
                case GameTypeFlag.TOURNEY_TEAM:
                    break;
                case GameTypeFlag.TOURNEY_GUILD:
                    break;
                case GameTypeFlag.PANG_BATTLE:
                    break;
                case GameTypeFlag.CHIP_IN_PRACTICE:
                    break;
                case GameTypeFlag.SSC:
                    break;
                case GameTypeFlag.HOLE_REPEAT:
                    {
                        if (SumHole > 18)
                        {
                            return 0;
                        }
                        return SumHole;
                    }
                case GameTypeFlag.GRANDPRIX:
                    break;
            }

            return 0;
        }
    }
}
