using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface IContact
    {
        string Email { get; set; }
        string Body { get; set; }
        string Name { get; set; }
        bool Subscribed { get; set; }
        DateTime Created { get; set; }
    }
}
