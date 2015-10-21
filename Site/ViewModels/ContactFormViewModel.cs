using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;

using Site.Data.API;

namespace Site.ViewModels
{
    public class ContactFormViewModel : BaseViewModel
    {
        public ContactFormViewModel(IKernel kernel, HttpContextBase context) : base(kernel, context) { }

        public String FullName { get; set; }
        public String Email { get; set; }
        public String Body { get; set; }
        public bool eNewsletter { get; set; }
    }

    public class ContactFormMobileViewModel : MobileViewModel
    {
        public ContactFormMobileViewModel(IKernel kernel, HttpContextBase context) : base(kernel, context) { }

        public String FullName { get; set; }
        public String Email { get; set; }
        public String Body { get; set; }
        public bool eNewsletter { get; set; }
    }
}