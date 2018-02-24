using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RdgFileTool
{
    public class RdgDocumentHandler
    {
        private readonly XDocument document = null;

        public RdgDocumentHandler(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("Filename cannot be null or empty.");
                return;
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found");
                return;
            }

            document = XDocument.Load(filename);
        }

        public void EnumerateUsernames(Action<XElement> handleUserName)
        {
            if (document == null)
            {
                return;
            }

            var userElements = from nodes in document.Descendants("logonCredentials").Descendants("userName")
                select nodes;
            foreach (var username in userElements)
            {
                handleUserName(username);
            }
        }

        public void Save(string filename)
        {
            if (File.Exists(filename))
            {
                Console.WriteLine("Output file already exists");
                return;
            }
            document?.Save(filename);
        }
    }
}
