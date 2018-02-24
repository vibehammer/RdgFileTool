using System;
using System.Linq;

namespace RdgFileTool
{
    public class Decryptor
    {
        public void Decrypt(string filename)
        {
            var document = new RdgDocumentHandler(filename);

            Console.WriteLine("Decrypting passwords...");
            document.EnumerateUsernames((userNameElement) =>
            {
                var domainNode = userNameElement.Parent?.Descendants("domain").First();
                var passwordNode = userNameElement.Parent?.Descendants("password").First();

                if (passwordNode == null) return;

                try
                {
                    if (TryDecryptStringUsingLocalUser(passwordNode.Value, out string password))
                    {
                        Console.WriteLine($"{domainNode?.Value ?? "."}\\{userNameElement.Value}: {password}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public void RemoveAllUsersThatCannotBeDecrypted(string filename, string outputFilename)
        {
            var document = new RdgDocumentHandler(filename);

            Console.WriteLine("Removing usernames and passwords that cannot be decrypted");
            document.EnumerateUsernames((userNameElement) =>
            {
                try
                {
                    var passwordNode = userNameElement.Parent?.Descendants("password").First();
                    var domainNode = userNameElement.Parent?.Descendants("domain").First();
                    if (passwordNode == null) return;

                    if (CanDecrypt(passwordNode.Value)) return;
                    
                    userNameElement.Value = string.Empty;
                    passwordNode.Value = string.Empty;
                    if (domainNode != null)
                    {
                        domainNode.Value = string.Empty;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });

            document.Save(outputFilename);
        }

        private static bool CanDecrypt(string encryptedString) => TryDecryptStringUsingLocalUser(encryptedString, out string result);

        private static unsafe bool TryDecryptStringUsingLocalUser(string encryptedString, out string result)
        {
            if (string.IsNullOrEmpty(encryptedString))
            {
                result = string.Empty;
                return false;
            }

            var optionalEntropy = new DataBlob
            {
                Size = 0
            };
            var promptStruct = new CryptProtectPromptStruct
            {
                Size = 0
            };
            var byteArray = Convert.FromBase64String(encryptedString);
            DataBlob dataIn;
            dataIn.Size = byteArray.Length;
            var description = string.Empty;
            DataBlob dataOut = new DataBlob();
            fixed (byte* bytePtr = byteArray)
            {
                dataIn.Data = (IntPtr) bytePtr;
                if (!Win32Crypto.CryptUnprotectData(ref dataIn, ref description, ref optionalEntropy, (IntPtr) null, ref promptStruct,
                    0, ref dataOut))
                {
                    result = string.Empty;
                    return false;
                }

                var data = (char*) (void*) dataOut.Data;
                var charArray = new char[dataOut.Size / 2];
                for (var i = 0; i < charArray.Length; ++i)
                {
                    charArray[i] = data[i];
                }
                result = new string(charArray);
                Win32Crypto.LocalFree(dataOut.Data);
                return true;
            }
        }
    }
}
