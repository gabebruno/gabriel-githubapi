using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gabriel_githubapi.Model
{
    public class ResultUserList
    {

        public List<User> Users { get; set; }
        public string NextPage { get; set; }
    }
}
