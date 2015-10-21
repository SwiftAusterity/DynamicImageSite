using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class LabsController : BaseContentController
    {
        protected override String ParentSection() { return "Labs"; }
        protected override String IndexTitle() { return "Labs"; }
        protected override String MainClass() { return "Labs"; }
    }
}
