using System;
namespace PangyaAPI.Tools
{
    public static class WriteConsole
    {
        public static void WriteLine()
        {
            Console.WriteLine();
        }
        public static void WriteLine(string format)
        {
            Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + format);
        }
        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(string.Format(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + format, args));
        }
        public static void WriteLine(string format, ConsoleColor Fore = ConsoleColor.White, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = Fore;
            Console.WriteLine(string.Format(format, args));
            Console.ResetColor();
        }
        /// <summary>
        /// Format = [2020/07/08 13:06:35] [SERVER_START]: PORT 10103
        /// </summary>
        /// <param name="msg">Exemple: [SERVER_START]</param>
        /// <param name="Fore">Exemple: ConsoleColor.White</param>
        public static void WriteLine(string msg, ConsoleColor Fore = ConsoleColor.White)
        {
            Console.ResetColor();
            Console.ForegroundColor = Fore;
            Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + msg);
            Console.ResetColor();
        }
        public static void Write(string msg = "", ConsoleColor Fore = ConsoleColor.White)
        {
            Console.ResetColor();
            Console.ForegroundColor = Fore;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void WriteColoredMessage(ConsoleColor color, string message, params object[] args)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.Write(string.Format(message, args));
            Console.ResetColor();
        }
    }
}
