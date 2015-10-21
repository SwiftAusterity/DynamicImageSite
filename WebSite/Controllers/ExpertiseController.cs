using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class ExpertiseController : BaseContentController
    {
        protected override string ParentSection() { return "Expertise"; }
        protected override string IndexTitle() { return "Expertise"; }
        protected override string MainClass() { return "expertise"; }
    }
}
