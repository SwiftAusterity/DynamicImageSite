using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Site.Data.API;

namespace Site.Data.DTO
{
    [Serializable]
    public class User : IUser
    {
        #region IUser Members

        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }

        #endregion
    }
}
