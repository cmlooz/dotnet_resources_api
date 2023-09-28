using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesAPI.Wrappers
{
    public class ResourcesRequest
    {
        public string process { get; set; }
        public string action { get; set; }
        public string data { get; set; }
        public dynamic parameters { get; set; }
    }
}