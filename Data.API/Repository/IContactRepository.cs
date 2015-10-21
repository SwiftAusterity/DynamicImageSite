using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface IContactRepository
    {
        bool Save(String email, String body, String name, bool subscribed);
        bool Delete(IContact contact);

        IEnumerable<IContact> Get();
        IEnumerable<IContact> Get(String emailAddress);
    }

    public interface IContactRepositoryBackingStore : IContactRepository
    {
        IEnumerable<IContact> Get(bool first);
    }
}
