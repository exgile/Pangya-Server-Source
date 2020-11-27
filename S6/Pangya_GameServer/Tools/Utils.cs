using System;
using System.Text;
using System.Reflection;
namespace Pangya_GameServer.Tools
{
    public static class Utils
    {
     
        public static string IsUCCNull(string UNIQUE, string IfNull = "0")
        {
            if (UNIQUE == null || UNIQUE.Length <= 0)
            {
                return IfNull;
            }
            return UNIQUE;
        }

        public static string StringFormat(string Format, object[] Args)
        {
            return string.Format(Format, Args);
        }


        public static string GetMethodName(MethodBase methodBase)
        {
            string str = methodBase.Name + "(";
            foreach (ParameterInfo info in methodBase.GetParameters())
            {
                string[] textArray1 = new string[] { str, info.ParameterType.Name, " ", info.Name, ", " };
                str = string.Concat(textArray1);
            }
            return str.Remove(str.Length - 2) + ")";
        }

        public static void PrintError(MethodBase methodBase, string msg)
        {
            string[] textArray1 = new string[] { "[", methodBase.DeclaringType.ToString(), "::", GetMethodName(methodBase), "]" };
            Console.WriteLine(string.Concat(textArray1));
            Console.WriteLine("Error : " + msg);
        }
        public static int Checksum(string dataToCalculate)
        {
            byte[] byteToCalculate = Encoding.ASCII.GetBytes(dataToCalculate);
            int checksum = 0;
            foreach (byte chData in byteToCalculate)
            {
                checksum += chData;
            }
            checksum &= 0xff;
            return checksum;
        }

    }
    public class TCompare
    {
        public static T IfCompare<T>(bool expression, T trueValue, T falseValue)
        {
            if (expression)
            {
                return trueValue;
            }
            else
            {
                return falseValue;
            }
        }
    }
}
