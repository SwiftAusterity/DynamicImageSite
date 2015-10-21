using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Site.Data.API;
using Site.Data.API.Repository;

using Ninject;

namespace Site.Data.DTO
{
    [Serializable]
    public class Contact : IContact
    {
        [Inject]
        public IUserRepository UserRepository { get; set; }

        public string Email { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public bool Subscribed { get; set; }
        public DateTime Created { get; set; }
    }
}
