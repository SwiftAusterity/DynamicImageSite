using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class LabsController : BaseContentController
    {
        protected override string ParentSection() { return "Labs"; }
        protected override string IndexTitle() { return "Labs"; }
        protected override string MainClass() { return "Labs"; }
    }
}
