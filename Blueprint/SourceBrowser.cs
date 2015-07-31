﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blueprint
{
    class SourceBrowser
    {

        public static void ProcessDirectory(string source, string destination)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(source);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, destination);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(source);
            foreach (string subdirectory in subdirectoryEntries)
            {
                // only check certain directories
                Regex regex = new Regex(@"_posts");
                Match match = regex.Match(subdirectory);

                if (match.Success)
                    ProcessDirectory(subdirectory, destination);
            }
               
        }

        public static void ProcessFile(string path, string writeDirectory)
        {
            SourceFile file = new SourceFile(path);
            string destination = "";

            // convert markdown file to HTML
            if (file.FileType == ".md")
            {
                Regex regex = new Regex(@"_posts");
                Match match = regex.Match(file.SourcePath);

                if (match.Success)
                    destination = file.CreateDirectoryStructure(file.FileName);

                file.ConvertFileToHTML(path, destination + file.FileName + ".html");
            }

        }
    }
}
