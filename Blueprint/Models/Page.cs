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
        public string Content { get; set; }

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
            sb.Replace(".html", "");

            return sb.ToString();
        }
    }

    public class Post : Page
    {
        public DateTime Date { get; set; }
        public string Excerpt { get; set; }

        public Post(string filepath) : base(filepath)
        {
            string[] file = filepath.Split('\\');
            string[] fileName = file[file.Length - 1].Split('.')[0].Split('-');

            // populate class attributes
            Title = Regex.Replace(getFileName(string.Join("-", fileName), fileName), @"^\s+", "");
            Date = new DateTime(int.Parse(fileName[0]), int.Parse(fileName[1]), int.Parse(fileName[2]));
            Url = "/" + fileName[0] + "/" + fileName[1] + "/" + fileName[2] + "/" + Title.Replace(" ", "-").ToLower() + ".html";
        }

        private string getFileName(string file, string[] fileName)
        {
            StringBuilder sb = new StringBuilder(file);

            sb.Replace("-", " ");
            sb.Replace(fileName[0], "");
            sb.Replace(fileName[1], "");
            sb.Replace(fileName[2], "");

            return sb.ToString();
        }
    }
}
