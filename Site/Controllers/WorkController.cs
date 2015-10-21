using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class WorkController : BaseContentController
    {
        protected override String ParentSection() { return "Work"; }
        protected override String IndexTitle() { return "Interaction By Design"; }
        protected override String MainClass() { return "work"; }
    }
}
