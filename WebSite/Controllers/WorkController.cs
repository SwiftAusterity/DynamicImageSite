using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class WorkController : BaseContentController
    {
        protected override string ParentSection() { return "Work"; }
        protected override string IndexTitle() { return "Interaction By Design"; }
        protected override string MainClass() { return "work"; }
    }
}
