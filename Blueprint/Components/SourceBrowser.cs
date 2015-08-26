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
    abstract class SourceBrowser
    {
        public static string SourceFolders = "_posts";

        protected SourceBrowser()
        {
            // get list of folders to browse
            foreach (string include in Program.Config.Include)
                SourceFolders += "|" + include;
        }

        public void ProcessDirectory(string source)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(source);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            ProcessSubDirectory(source);
        }

        public void ProcessSubDirectory(string source)
        {
            //Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(source);
            foreach (string subdirectory in subdirectoryEntries)
            {
                string subdirectoryName = subdirectory.Split('\\').Last();

                // only check right directories
                Regex regex = new Regex(@"(" + SourceFolders + ")");
                Match match = regex.Match(subdirectory);

                if (match.Success)
                    ProcessDirectory(subdirectory);
                else if (subdirectoryName[0].ToString() != "_")
                    CopyDirectory(subdirectory, Program.DestinationFolder + subdirectoryName);
            }
        }

        public abstract void ProcessFile(string path);

        public void CopyDirectory(string SourcePath, string DestinationPath)
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

    class AnalyzeBrowser : SourceBrowser
    {
        public override void ProcessFile(string path)
        {
            SourceFile file = new SourceFile(path);

            // convert markdown file to HTML
            if (file.FileType == ".md" || file.FileType == ".html")
            {
                // if file is in '_posts' folder, alter writeDirectory
                Regex regex = new Regex(@"_posts");
                Match postMatch = regex.Match(file.SourcePath);

                // if file is a post
                if (postMatch.Success)
                {
                    // store post in variable
                    Post post = new Post(path);
                    Program.Config.Variables.Site.Posts.Add(post);
                }
                else
                {
                    // store page in variable
                    Page page = new Page(path);
                    Program.Config.Variables.Site.Pages.Add(page);
                }
            }
        }
    }

    class ProcessBrowser : SourceBrowser
    {
        public override void ProcessFile(string path)
        {
            SourceFile file = new SourceFile(path);
            string destination = Program.DestinationFolder;

            Console.WriteLine(file.FileName + file.FileType);

            // convert markdown file to HTML
            if (file.FileType == ".md")
            {
                // if file is in '_posts' folder, alter writeDirectory
                Regex regex = new Regex(@"_posts");
                Match postMatch = regex.Match(file.SourcePath);

                // if file is a post
                if (postMatch.Success)
                {
                    destination = file.CreateDirectoryStructure(file.FileName);
                }

                file.ConvertFileToHTML(
                    path,
                    destination + file.FileName + ".html"
                );
            } 
            else if (file.FileType == ".html")
            {
                Console.WriteLine("encountered HTML file: " + file.FileName);
            }
        }
    }
}
