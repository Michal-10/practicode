using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public DateTime? LastCommitDate { get; set; }
        public int StarsCount { get; set; }
        public int PullRequestCount { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public List<string> Languages { get; set; }
    }
}
