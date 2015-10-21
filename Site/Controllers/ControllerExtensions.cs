using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

using Site.ViewModels;

namespace Site.Controllers
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Render a view into a string. It's a hack, it may fail badly.
        /// </summary>
        /// <param name="name">Name of the view, that is, its path.</param>
        /// <param name="data">Data to pass to the view, a model or something like that.</param>
        /// <returns>A string with the (HTML of) view.</returns>
        public static string RenderViewToString(this Controller controller,
                                                string viewPath,
                                                ViewDataDictionary viewData,
                                                TempDataDictionary tempData)
        {
            ViewPage viewPage = new ViewPage();

            using (var writer = new StringWriter())
            {
                viewPage.ViewContext = new ViewContext(controller.ControllerContext, new WebFormView(viewPath), viewData, tempData, writer);
                //Now render the view into the string writer and flush the response
                viewPage.ViewContext.View.Render(viewPage.ViewContext, writer);
                writer.Flush();
                return writer.ToString();
            }

        }

        public static string RenderPartialToString(this Controller controller,
                                                   string controlName, object viewData)
        {
            var request = new HttpRequest("/", controller.Request.Url.ToString(), "");
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            var httpContextBase = new HttpContextWrapper(httpContext);
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    var viewContext = new ViewContext(controller.ControllerContext, new WebFormView("FAKEVIEW"), new ViewDataDictionary(viewData), new TempDataDictionary(), tw);
                    viewContext.HttpContext = httpContextBase;
                    //new ViewContext() {HttpContext = httpContextBase};
                    // Have to set the View and TempData properties to something or
                    // else nested partials blow up
                    //viewContext.View = new WebFormView("FAKEVIEW");
                    //viewContext.TempData = new TempDataDictionary();
                    ViewPage viewPage = new ViewPage() { ViewContext = viewContext };
                    viewPage.ViewData = viewContext.ViewData;
                    viewPage.Url = GetBogusUrlHelper(httpContextBase);
                    // We need to set the Html helper to an instance with access to the app's route table
                    // In order to use the form helpers.
                    viewPage.Html = new HtmlHelper<object>(viewContext, viewPage, RouteTable.Routes);

                    viewPage.Controls.Add(viewPage.LoadControl(controlName));


                    // Have to set the writer on the ViewContext or using(Html.BeginRouteForm...) 
                    // blows up
                    viewContext.Writer = tw;
                    viewPage.RenderControl(tw);
                }
            }

            return sb.ToString();
        }

        public static UrlHelper GetBogusUrlHelper(HttpContextBase contextBase)
        {

            var routeData = new RouteData();
            var requestContext = new RequestContext(contextBase, routeData);

            return new UrlHelper(requestContext, RouteTable.Routes);
        }
    }
}
