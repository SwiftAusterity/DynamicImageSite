using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;

using Site.Data.API;

namespace Site.ViewModels
{
    public interface IContactListViewModel
    {
        IEnumerable<IContact> Contacts { get; set; }
    }
}