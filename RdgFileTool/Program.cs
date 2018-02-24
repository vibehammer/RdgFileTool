using System;
using System.Diagnostics;

namespace RdgFileTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0].ToLower() == "-d")
            {
                var decryptor = new Decryptor();
                decryptor.Decrypt(args[1]);
            }
            else if (args.Length == 3 && args[0].ToLower() == "-r")
            {
                var decryptor = new Decryptor();
                decryptor.RemoveAllUsersThatCannotBeDecrypted(args[1], args[2]);
            }
            else if (args.Length == 3 && args[0].ToLower() == "-c")
            {
                var cleaner = new Cleaner();
                cleaner.Clean(args[1], args[2]);
            }
            else
            {
                Console.WriteLine("RdgTool.exe by Jacob Vibe Hammer");
                Console.WriteLine("Use: rdgtool.exe [-d|-r|-c] file.rdg [output file.rgd]");
                Console.WriteLine("Examples:");
                Console.WriteLine("rdgtool.exe -d filename.rdg attempts to decrypt passwords and displays those that succeed");
                Console.WriteLine("rdgtool.exe -r filename.rdg newFilename.rdg removes all passwords and users that can't be decrypted");
                Console.WriteLine("rdgtool.exe -c filename.rdg newFilename.rdg removes all username and passwords from the file");
            }

            if (Debugger.IsAttached)
            {
                Console.ReadKey();
            }
        }
    }
}
