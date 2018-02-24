using System;
using System.Linq;

namespace RdgFileTool
{
    public class Cleaner
    {
        public void Clean(string filename, string outputFilename)
        {
            var document = new RdgDocumentHandler(filename);

            Console.WriteLine("Removing all username and passwords from file");
            document.EnumerateUsernames((userNameElement) =>
            {
                var passwordNode = userNameElement.Parent?.Descendants("password").First();

                userNameElement.Value = string.Empty;
                if (passwordNode != null)
                {
                    passwordNode.Value = string.Empty;
                }
            });
            document.Save(outputFilename);
        }
    }
}
