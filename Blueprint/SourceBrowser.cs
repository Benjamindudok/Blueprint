using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint
{
    class SourceBrowser
    {
        public SourceBrowser()
        {
            
        }

        public static void ProcessDirectory(string source, string destination)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(source);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, destination);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(source);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, destination);
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
