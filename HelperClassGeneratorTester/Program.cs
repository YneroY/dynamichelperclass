using System;
using AutoHelperClassGenerator;

namespace HelperClassGeneratorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CInternalHelper.IWantMethodName());
            Console.WriteLine(CInternalHelper.GetCrc16("kkkkkkkk"));
        }
    }
}
