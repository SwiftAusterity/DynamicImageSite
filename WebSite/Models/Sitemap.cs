using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml;
using System.Text;

namespace Site.Models
{
    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }

    public interface ISitemapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        ChangeFrequency? ChangeFrequency { get; }
        float? Priority { get; }
    }

    public class SitemapItem : ISitemapItem
    {
        public SitemapItem(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public float? Priority { get; set; }
    }

    public class XmlSitemapResult : ActionResult
    {
        private IEnumerable<ISitemapItem> _items;

        public XmlSitemapResult(IEnumerable<ISitemapItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var sb = new StringBuilder();
            string encoding = context.HttpContext.Response.ContentEncoding.WebName;

            var siteMapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", siteMapNamespace);

                foreach(ISitemapItem item in _items)
                {
                    writer.WriteStartElement("url", siteMapNamespace);
                    writer.WriteElementString("loc", item.Url.ToLower());

                    if (item.LastModified.HasValue)
                        writer.WriteElementString("lastmod", item.LastModified.Value.ToString("yyyy-MM-dd"));

                    if (item.ChangeFrequency.HasValue)
                        writer.WriteElementString("changefreq", item.ChangeFrequency.Value.ToString().ToLower());

                    if (item.Priority.HasValue)
                        writer.WriteElementString("priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture));

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            var dec = new XDeclaration("1.0", encoding, "yes");

            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(dec + sb.ToString());
        }
    }
}
