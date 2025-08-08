using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
//-----------------------------------------------------------------------------------------
namespace Kenel32Pack
{
    public unsafe class UDog
    {
        //---------------------------------------------------------
        private static ushort DogBytes, DogAddr;
        private static uint DogPassword;
        private static uint NewPassword;
        private static uint DogResult, CurrentNu;
        private static ushort DogCascade, NewCascade;
        private static byte[] DogData;
        private static uint Retocde;
        //---------------------------------------------------------
        [DllImport("htbdog.dll", CharSet = CharSet.Ansi)]
        private static unsafe extern void Check_Dog(ushort Cascade, uint Password, uint* lpRet);
        [DllImport("htbdog.dll", CharSet = CharSet.Ansi)]
        private static unsafe extern void Read_Dog(ushort Cascade, uint Password, ushort addr, ushort bytes, void* pdata, uint* lpRet);
        [DllImport("htbdog.dll", CharSet = CharSet.Ansi)]
        private static unsafe extern void GetCurrentNo_Dog(ushort Cascade, uint* lpCurrentNo, uint* lpRet);
        //-----------------------------------------------------------------------------------------
        public static void Initialize()
        {
            DogBytes = 200;
            DogData = new Byte[DogBytes];
            DogCascade = 0;
            DogPassword = Int32.MaxValue / 2;
            DogThread = new Thread(new ThreadStart(RunDog));
            DogThread.Start();
        }
        //-----------------------------------------------------------------------------------------
        private  static Boolean CheckValidity()
        {
            //
            CheckDog();
            if (Retocde != 0) return false;
            //
            ReadDog();
            if (DogData[0] != 0xAD) return false;
            if (DogData[1] != 0x39) return false;
            if (DogData[2] != 0x99) return false;
            if (DogData[3] != 0x01) return false;
            return true;
        }
        //-----------------------------------------------------------------------------------------
        private static unsafe void CheckDog()
        {
            fixed (uint* pRetcode = &Retocde)
            {
                Check_Dog(DogCascade, DogPassword, pRetcode);
            }
        }
        //-----------------------------------------------------------------------------------------
        private static unsafe void ReadDog()
        {
            DogAddr = 64;
            DogBytes = 4;

            fixed (byte* pDogData = &DogData[0])
            fixed (uint* pRetcode = &Retocde)
            {
                Read_Dog(DogCascade, DogPassword, DogAddr, DogBytes, pDogData, pRetcode);
            }
        }
        //-----------------------------------------------------------------------------------------
        private static Thread DogThread = null;
        public static Boolean IsValidity = true;
        //-----------------------------------------------------------------------------------------
        private static void RunDog()
        {
            while (true)
            {
                Thread.Sleep(10 * 1000);
                if (!UDog.CheckValidity())
                {
                    IsValidity = false;
                }
            }
        }
        //-----------------------------------------------------------------------------------------

    }

}
