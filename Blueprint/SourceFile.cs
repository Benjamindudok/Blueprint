using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            using (var reader = new StreamReader(source))
            using (var writer = new StreamWriter(destination))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}
