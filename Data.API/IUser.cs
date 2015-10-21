using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface IUser
    {
        Guid ID { get; set; }
        String Name { get; set; }
        String Permissions { get; set; }
    }
}
