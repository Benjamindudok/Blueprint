using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arguments = {"manufacture", "O:\\_temp\\blueprint-test", "O:\\_temp\\blueprint-test\\www\\"};

            if (Directory.Exists(arguments[1]))
            {
                Console.WriteLine("checking " + arguments[1]);

                // check if www folder exists
                if (!Directory.Exists(arguments[2]))
                {
                    // create www folder
                    Directory.CreateDirectory(arguments[2]);
                }

                // This path is a directory
                ProcessDirectory(arguments[1], arguments[2]);
                Console.ReadLine();
            }

        }

        public static void ProcessDirectory(string readDirectory, string writeDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(readDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, writeDirectory);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(readDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, writeDirectory);
        }

        public static void ProcessFile(string path, string writeDirectory)
        {
            string[] file = path.Split('\\');
            string fileName = file[file.Length - 1].Split('.')[0];
            string fileExtension = "." + file[file.Length - 1].Split('.')[1];

            // convert markdown file to HTML
            if (fileExtension == ".md")
            {
                using (var reader = new StreamReader(path))
                using (var writer = new StreamWriter(writeDirectory + fileName + ".html"))
                {
                    CommonMark.CommonMarkConverter.Convert(reader, writer);
                }
            }
        }
    }
}
