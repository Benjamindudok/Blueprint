using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Newtonsoft.Json;

namespace Blueprint
{
    public class Config
    {
        public bool Debug { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string[] Include { get; set; }

        public List<Post> Posts { get; set; }

        public DateTime Time = DateTime.Now;

        public Config()
        {
            Posts = new List<Post>();
        }
    }
}
