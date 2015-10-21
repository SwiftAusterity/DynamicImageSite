using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class NavElement : INavElement
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
    }
}
