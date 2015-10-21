using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class ClientsController : BaseContentController
    {
        protected override String ParentSection() { return "Clients"; }
        protected override String IndexTitle() { return "Clients"; }
        protected override String MainClass() { return "clients"; }
    }
}
