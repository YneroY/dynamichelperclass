using System.Collections.Generic;

namespace HelperClassGenerator
{
    /// <summary>
    /// Constant variables.
    /// </summary>
    public static class Constants
    {
        public const string GETASYNCMETHODNAMEID = "GETASYNCMETHODNAME";
        public const string GETCRC16ID = "GETCRC16";

        public static readonly MethodInfo GETASYNCMETHODNAME = new MethodInfo
        {
            MethodName = GETASYNCMETHODNAMEID,
            MethodImplementation = "public static string {0}([CallerMemberName]string name = null) => name;",
            ArgumentCount = 0,
            ArgumentList = new List<string>()
        };

        public static readonly MethodInfo GETCRC16 = new MethodInfo
        {
            MethodName = GETCRC16ID,
            MethodImplementation = @"public static ushort {0}(string str)
                                     {{
                                         ushort temp, a;
                                         byte[] inputArr = Encoding.UTF8.GetBytes(str);
                                         ushort[] table = new ushort[256];
                                         ushort crc = 0xffff;
                                     
                                         for (int i = 0; i < table.Length; ++i)
                                         {{
                                             temp = 0;
                                             a = (ushort)(i << 8);
                                             for (int j = 0; j < 8; ++j)
                                             {{
                                                 if (((temp ^ a) & 0x8000) != 0)
                                                     temp = (ushort)((temp << 1) ^ 0x1021);
                                                 else
                                                     temp <<= 1;
                                                 a <<= 1;
                                             }}
                                             table[i] = temp;
                                         }}
                                     
                                         for (int i = 0; i < inputArr.Length; ++i)
                                         {{
                                             crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & inputArr[i]))]);
                                         }}
                                     
                                         return crc;
                                     }}",
            ArgumentCount = 1,
            ArgumentList = new List<string>() { "string" }
        };
    }
}
