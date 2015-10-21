using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;

using Site.Data.API;

namespace Site.ViewModels
{
    public class MobileViewModel : BaseViewModel
    {
        public MobileViewModel(IKernel kernel, HttpContextBase context) : base(kernel, context) { }

        public IEnumerable<IContentPage> SectionPages { get; set; }
    }
}