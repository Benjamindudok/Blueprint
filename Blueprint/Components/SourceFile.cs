using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Nustache.Core;

namespace Blueprint
{
    class SourceFile
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string SourcePath { get; set; }

        public SourceFile(string path)
        {
            string[] file = path.Split('\\');

            FileName = file[file.Length - 1].Split('.')[0];
            FileType = "." + file[file.Length - 1].Split('.')[1];
            SourcePath = path;
        }

        public void ConvertFileToHTML(string source, string destination)
        {
       
            // convert markdown to html
            using (var reader = new StreamReader(source))
            using (var writer = new StreamWriter(destination))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }

            // add layout to html file and let Nustache do it's work
            string header   = Render.FileToString(Program.SourceFolder + "\\_layout\\header.html", Program.Config.Variables);
            string content  = Render.FileToString(destination, Program.Config.Variables);
            string footer   = Render.FileToString(Program.SourceFolder + "\\_layout\\footer.html", Program.Config.Variables);

            // delete 'old' generated html file
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            // add new html file with layout
            File.WriteAllText(destination, header + content + footer);
        }

        public void CopyFileToDestination(string source, string destination)
        {
            string sourceFile = Render.FileToString(source, Program.Config.Variables);
            File.WriteAllText(destination, sourceFile);
        }

        public string CreateDirectoryStructure(string filename)
        {
            string path = Program.DestinationFolder;

            // by creation date
            string[] date = filename.Split('-');
            path += date[0] + "\\" + date[1] + "\\" + date[2] + "\\";


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // return path so file has reference
            return path;
        }
    }
}
