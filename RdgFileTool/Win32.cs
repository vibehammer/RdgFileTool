using System;

namespace RdgFileTool
{
    public struct DataBlob
    {
        public int Size;
        public IntPtr Data;
    }

    public struct CryptProtectPromptStruct
    {
        public int Size;
        public int Flags;
        public IntPtr Window;
        public string Message;
    }
}
