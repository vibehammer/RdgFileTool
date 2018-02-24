using System;
using System.Runtime.InteropServices;

namespace RdgFileTool
{
    public class Win32Crypto
    {
        [DllImport("crypt32.dll",SetLastError = true,CharSet = CharSet.Auto)]
        public static extern
            bool CryptUnprotectData(ref DataBlob pCipherText,
                ref string pszDescription,
                ref DataBlob pEntropy,
                IntPtr pReserved,
                ref CryptProtectPromptStruct pPrompt,
                int dwFlags,
                ref DataBlob pPlainText);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern void LocalFree(IntPtr ptr);
    }
}