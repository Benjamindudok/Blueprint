﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Nustache.Core;

namespace Blueprint
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
            DestinationPath = Program.DestinationFolder + path.Replace(Program.SourceFolder, "").Replace(FileName + FileType, "");
        }

        public string ConvertMarkdown(string source)
        {
            // read file
            string file = File.ReadAllText(source);

            // convert markdown to html string
            return CommonMark.CommonMarkConverter.Convert(file);
        }

        public void Render(string content, string destination, bool renderLayout)
        {
            // add header / footer to content
            // TODO make layout files variable by config
            if (renderLayout) {
                string header   = File.ReadAllText(Program.SourceFolder + "\\_layout\\header.html");
                string footer = File.ReadAllText(Program.SourceFolder + "\\_layout\\footer.html");

                content = header + content + footer;
            }

            foreach (SourceFile partial in Program.Config.Files.Where(f => f.PageType == "partial"))
            {
                Console.WriteLine(partial.FileName);
                content += partial.Content;
            }

            // TODO add page variables for current page
            // render html file with replaced variables
            Nustache.Core.Render.StringToFile(content, Program.Config.Variables, destination);
        }

        public string GetDirectoryStructureForPosts(string filename)
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
    }
}
