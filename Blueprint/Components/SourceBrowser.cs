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
        public string SourceFolders = "_partials|_posts";

        public SourceBrowser()
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
                Match match = regex.Match(subdirectoryName);

                if (match.Success)
                    ProcessDirectory(subdirectory);
                else if (subdirectoryName[0].ToString() != "_")
                    CopyDirectory(subdirectory, Program.DestinationFolder + subdirectoryName);
            }
        }

        public void ProcessFile(string path)
        {
            SourceFile file = new SourceFile(path);
            
            Regex regex = new Regex(@"_posts|_partials");
            Match postsMatch = regex.Match(file.SourcePath);

            // Check if file is a page or post
            if (postsMatch.Success)
            {
                string match = postsMatch.Captures.Cast<Match>().First().ToString();

                switch (match)
                {
                    case "_partials":
                        file.PageType = "partial";
                        break;

                    case "_posts":
                        // store post in variable
                        Post post = new Post(path);
                        Program.Config.Variables.Site.Posts.Add(post);

                        file.PageType = "post";
                        break;
                }
            } 
            else
            {
                // store page in variable
                Page page = new Page(path);
                Program.Config.Variables.Site.Pages.Add(page);

                file.PageType = "page";
            }

            switch (file.FileType)
            {
                case ".md":
                    file.Content = file.ConvertMarkdown(path);
                    break;

                case ".html":
                    file.Content = File.ReadAllText(path);
                    break;
            }

            // add file to memory
            Program.Config.Files.Add(file);
        }

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
   
}
