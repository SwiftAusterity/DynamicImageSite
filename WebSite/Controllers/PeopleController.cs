using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class PeopleController : BaseContentController
    {
        protected override string ParentSection() { return "People"; }
        protected override string IndexTitle() { return "People"; }
        protected override string MainClass() { return "people"; }
    }
}
