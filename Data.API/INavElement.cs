using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface INavElement
    {
        string Name { get; set; }
        string Url { get; set; }
        string Thumbnail { get; set; }
    }
}
