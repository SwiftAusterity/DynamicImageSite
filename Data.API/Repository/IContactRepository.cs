using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IContactRepository
    {
        bool Save(string email, string body, string name, bool subscribed);
        bool Delete(IContact contact);

        IEnumerable<IContact> Get();
        IEnumerable<IContact> Get(string emailAddress);
    }

    public interface IContactRepositoryBackingStore : IContactRepository
    {
        IEnumerable<IContact> Get(bool first);
    }
}
