using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API.Repository
{
    public interface ITextCommandRepository
    {
        bool Save(string command, string path, bool handled);
    }

    public interface ITextCommandRepositoryBackingStore : ITextCommandRepository
    {
    }
}
