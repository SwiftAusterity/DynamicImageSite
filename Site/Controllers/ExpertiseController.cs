using System;
using System.Web.Mvc;

namespace Site.Controllers
{
    [HandleError]
    public class ExpertiseController : BaseContentController
    {
        protected override String ParentSection() { return "Expertise"; }
        protected override String IndexTitle() { return "Expertise"; }
        protected override String MainClass() { return "expertise"; }
    }
}
