using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blueprint.Models;
using Blueprint.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blueprint
{
    public class Config
    {
        public bool Debug { get; set; }
        public string[] Include { get; set; }
        public string[] Exclude { get; set; }

        public List<SourceFile> Files { get; set; }
        public Defaults Defaults { get; set; }

        public Variables Variables { get; set; }

        public Config()
        {
            Files = new List<SourceFile>();
            Variables = new Variables();
        }
    }

    public class Defaults
    {
        public SourceFile Page { get; set; }
        public SourceFile Post { get; set; }
    }
}
