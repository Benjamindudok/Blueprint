﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blueprint
{
    public class Config
    {
        public bool Debug { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string[] Exclude { get; set; }
        public string[] Include { get; set; }
    }
}
