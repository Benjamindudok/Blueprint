using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blueprint.Models
{
    public class Page
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public Page(string filepath)
        {
            string[] file = filepath.Split('\\');
            string fileName = file[file.Length - 1];

            // populate class attributes
            Title = Regex.Replace(getFileName(fileName), @"^\s+", "");
            Url = "/" + fileName[0] + "/" + fileName[1] + "/" + fileName[2] + "/" + Title.Replace(" ", "-").ToLower() + ".html";
        }

        private string getFileName(string file)
        {
            StringBuilder sb = new StringBuilder(file);

            sb.Replace("-", " ");
            sb.Replace(".md", "");

            return sb.ToString();
        }
    }
}
