using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blueprint.Models;

namespace Blueprint
{
    class SourceBrowser
    {

        public static void ProcessDirectory(string source, string destination)
        {
            string folderName = source.Split('\\').Last();
            
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(source);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, destination);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(source);
            foreach (string subdirectory in subdirectoryEntries)
            {
                string subdirectoryName = subdirectory.Split('\\').Last();

                // get list of folders to browse
                string sourceFolders = "_posts";
                foreach (string include in Program.Variable.Include)
                    sourceFolders += "|" + include;

                // only check certain directories
                Regex regex = new Regex(@"(" + sourceFolders + ")");
                Match match = regex.Match(subdirectory);


                if (match.Success)
                    ProcessDirectory(subdirectory, destination);
                else if (subdirectoryName[0].ToString() != "_")
                    CopyDirectory(subdirectory, destination + "scripts");
            }   
        }

        public static void ProcessFile(string path, string writeDirectory)
        {
            SourceFile file = new SourceFile(path);
            string destination = writeDirectory;

            // convert markdown file to HTML
            if (file.FileType == ".md")
            {
                // if file is in '_posts' folder, alter writeDirectory
                Regex regex = new Regex(@"_posts");
                Match match = regex.Match(file.SourcePath);

                // if file is a post
                if (match.Success) {
                    destination = file.CreateDirectoryStructure(file.FileName);

                    // store post in variable
                    Post post = new Post(path);
                    Program.Variable.Posts.Add(post);
                }

                file.ConvertFileToHTML(
                    path, 
                    destination + file.FileName + ".html"
                );
            }
        }

        public static void CopyDirectory(string SourcePath, string DestinationPath)
        {
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", 
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", 
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
    }
}
