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
            // test data
            IDictionary<string, string> exampleData = new Dictionary<string, string>();
            exampleData.Add("test", "Dit is een test variabel geplaatst door Nustache");
       
            // convert markdown to html
            using (var reader = new StreamReader(source))
            using (var writer = new StreamWriter(destination))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }

            // add layout to html file
            string header   = File.ReadAllText("O:\\_temp\\blueprint-test\\_layout\\header.html");
            string content = Render.FileToString(destination, exampleData);
            string footer   = File.ReadAllText("O:\\_temp\\blueprint-test\\_layout\\footer.html");

            // delete 'old' generated html file
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            // add new html file with layout
            File.WriteAllText(destination, header + content + footer);
        }

        public string CreateDirectoryStructure(string filename)
        {
            string path = "O:\\_temp\\blueprint-test\\_www\\";

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
