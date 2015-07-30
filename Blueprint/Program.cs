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
        public static string BlueprintAction = "manufacture";
        public static string SourceFolder = "O:\\_temp\\blueprint-test";
        public static string DestinationFolder = "O:\\_temp\\blueprint-test\\www\\";

        static void Main(string[] args)
        {

            // check which arguments are present
            foreach (string argument in args)
            {
                int index = Array.IndexOf(args, argument);

                // assign arguments to properties
                if (index == 0)
                {
                    BlueprintAction = argument;
                } 
                else if (index == 1 && index != (args.Length - 1))
                {
                    SourceFolder = argument;
                }
                else if (index == 1 && index == (args.Length - 1))
                {
                    DestinationFolder = argument;
                }
                else if (index == 2)
                {
                    DestinationFolder = argument;
                }
            }

            // check if destination folder exists
            if (DestinationFolder != "" && !Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(args[2]);
            }

            // check if source folder exists
            if (Directory.Exists(SourceFolder))
            {
                // start processing directies/files
                ProcessDirectory(SourceFolder, DestinationFolder);
            }

            Console.WriteLine("Process completed, press a key to exit");
            Console.ReadLine();
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
            SourceFile file = new SourceFile(path);

            // convert markdown file to HTML
            if (file.FileType == ".md")
            {
                file.ConvertFileToHTML(path, writeDirectory + file.FileName + ".html");
            }
        }
    }
}
