using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blueprint.Models
{
    public class SiteVariables
    {
        public DateTime Time = DateTime.Now;
        public List<Page> Pages { get; set; }
        public List<Post> Posts { get; set; }
        public List<Post> RecentPosts { get; set; }

        public SiteVariables()
        {
            Posts = new List<Post>();
            Pages = new List<Page>();
        }
    }
}
