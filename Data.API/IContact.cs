using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface IContact
    {
        String Email { get; set; }
        String Body { get; set; }
        String Name { get; set; }
        bool Subscribed { get; set; }
        DateTime Created { get; set; }
    }
}
