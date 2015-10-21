using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Infuz.Utilities;

namespace Site.Data.API
{
    public enum PermissionsRole
    {
        None = 0,
        Admin
    }

    public static partial class EnumHelper
    {
        public static bool TryParse(this string value, out PermissionsRole roleType)
        {
            return value.TryParse(out roleType, default(PermissionsRole));
        }
    }
}
