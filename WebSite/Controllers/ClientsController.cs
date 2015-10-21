using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class ClientsController : BaseContentController
    {
        protected override string ParentSection() { return "Clients"; }
        protected override string IndexTitle() { return "Clients"; }
        protected override string MainClass() { return "clients"; }
    }
}
