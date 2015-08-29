using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Nustache.Core;

namespace Blueprint.Components
{
    public class SourceFile
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        public string Content { get; set; }
        public string PageType { get; set; }

        public SourceFile(string path)
        {
            string[] file = path.Split('\\');

            FileName = file[file.Length - 1].Split('.')[0];
            FileType = "." + file[file.Length - 1].Split('.')[1];
            SourcePath = path;
            DestinationPath = Program.DestinationFolder + path.Replace(Program.SourceFolder + "\\", "").Replace(FileName + FileType, "");

            // get content and convert markdown if needed
            Content = File.ReadAllText(path);
            if (FileType == ".md") Content = CommonMark.CommonMarkConverter.Convert(Content);
        }

        public void GenerateHtmlFile(bool renderLayout, string layoutName)
        {
            // locally hold file contents
            string content = Content;

            // if layout is needed, replace file contents with layout file content
            if (renderLayout) { 
                SourceFile layoutFile = Program.Config.Files.Where(f => f.PageType == "layout").First(l => l.FileName == layoutName);
                content = layoutFile.Content;
            }

            // set page variables
            Program.Config.Variables.Page = new Page(SourcePath);
            Program.Config.Variables.Page.Content = Render.StringToString(Content, Program.Config.Variables);

            // add partials to content
            content = Program.Config.Files
                .Where(f => f.PageType == "partial")
                .Aggregate(content, (current, partial) => current + partial.Content);

            // render file with Nustache replacements
            Render.StringToFile(content, Program.Config.Variables, DestinationPath + FileName + ".html");
        }

        public string GenerateDirectoryStructureForPosts(string filename)
        {
            string path = Program.DestinationFolder;

            // by creation date 
            string[] date = filename.Split('-');
            path += date[0] + "\\" + date[1] + "\\" + date[2] + "\\";


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // return path for reference
            return path;
        }

        //public string GenerateFileNameForPosts(string filename)
        //{
        //    return "";
        //}
    }
}
