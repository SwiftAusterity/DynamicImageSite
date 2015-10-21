using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class PeopleController : BaseContentController
    {
        protected override String ParentSection() { return "People"; }
        protected override String IndexTitle() { return "People"; }
        protected override String MainClass() { return "people"; }
    }
}
