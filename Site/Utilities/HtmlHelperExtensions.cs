using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Site.Utilities
{
    public static class HtmlHelperExtensions
    {
        public static string DatePicker(this HtmlHelper helper, string name, object date)
        {
            return DatePicker(helper, name, date, String.Empty);
        }

        public static string DatePicker(this HtmlHelper helper, string name, object date, string dateFormat)
        {
            StringBuilder html = new StringBuilder();

            // Build our base input element
            html.Append("<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\"");

            // Model Binding Support
            if (date != null)
            {
                string dateValue = String.Empty;

                if (date is DateTime? && ((DateTime)date) != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToString();
                else if (date is DateTime && (DateTime)date != DateTime.MinValue)
                    dateValue = ((DateTime)date).ToString();
                else if (date is string)
                    dateValue = (string)date;

                html.Append(" value=\"" + dateValue + "\" />");
            }

            //default dateFormat
            if (String.IsNullOrEmpty(dateFormat))
                dateFormat = "%m/%d/%z %H:%i";

            // Now we call the datepicker function, passing in our options.  Again, a future enhancement would be to
            // pass in date options as a list of attributes ( min dates, day/month/year formats, etc. )
            html.Append("<script type=\"text/javascript\">");

            // html.Append("$().ready(");
            // html.Append("function() { ");

            html.Append("AnyTime.picker(");
            html.AppendFormat("'{0}'", name);
            //Add format and options
            html.Append(", { format: '" + dateFormat + "', firstDOW: 1 }");
            html.Append(");");

            // html.Append("});");

            html.Append("</script>");

            return html.ToString();
        }
    }
}
