using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IUserRepository
    {
        bool Save(string source, string url);
        bool Delete(IUser user);

        IUser Get(Guid userId);
        IEnumerable<IUser> Get();
    }

    public interface IUserRepositoryBackingStore : IUserRepository
    {
        IEnumerable<IUser> Get(bool first);
    }
}
