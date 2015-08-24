using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Models
{
    public class Variables
    {
        public Page Page { get; set; }
        public SiteVariables Site { get; set; }

        public Variables()
        {
            Site = new SiteVariables();
        }
    }
}
