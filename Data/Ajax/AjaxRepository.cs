using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;

namespace Site.Data.Ajax
{
    public class AjaxRepository
    {
        //I dont want to explode on individual values
        internal static T GetJsonProperty<T>(JToken thing, String name, T defaultValue)
        {
            if (thing != null)
            {
                try
                {
                    if (thing[name] != null)
                        return thing[name].Value<T>();
                }
                catch
                {
                    //dont explode
                }
            }

            return defaultValue;
        }

        public IList<TElement> UntilDovesCry<TElement>(bool initialLoad
            , Uri targetUri
            , String rootNodeName
            , String elementName
            , Action<JToken, IList<TElement>> appendAction)
        {
            var maxChances = initialLoad ? 10 : 3;
            var jsonString = String.Empty;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    HttpWebRequest getRequest = (HttpWebRequest)HttpWebRequest.Create(targetUri);
                    getRequest.Credentials = CredentialCache.DefaultCredentials;
                    getRequest.ContentType = "application/x-www-form-urlencoded";
                    getRequest.Method = "POST";
                    getRequest.Timeout = -1;

                    getRequest.ContentLength = 0;

                    using (HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse())
                    using (Stream dataStream = getResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                        jsonString = reader.ReadToEnd();

                    var jO = JObject.Parse(jsonString);
                    var jT = jO[rootNodeName];

                    var result = new List<TElement>();

                    if (jT != null)
                    {
                        var nodeList = jT.Children().Where(child => ((JProperty)child).Name.Equals(elementName, StringComparison.InvariantCultureIgnoreCase));

                        //if there's only one and it's not a sequence
                        if (nodeList.First().Children().Count() == 0 || nodeList.First().Children().First().Type != JTokenType.Array)
                        {
                            foreach (JToken currentRow in nodeList)
                                appendAction(currentRow, result);
                        }
                        else if (nodeList.First().Children().First().Type == JTokenType.Array)
                        {
                            foreach (JToken currentNode in nodeList)
                                foreach (JToken currentRow in currentNode.Children())
                                    appendAction(currentRow, result);
                        }
                    }

                    return result;

                }
                catch (Exception ex)
                {
                    //TODO: looking into ex to see if it is worth retrying
                    var wex = ex as WebException;

                    if (wex != null && wex.Status == WebExceptionStatus.Timeout)
                        Thread.Sleep(5000);
                    else
                        return new List<TElement>();
                }
            }

            return new List<TElement>();
        }

        public IList<TElement> UntilDovesCry<TElement>(bool initialLoad
            , Uri targetUri
            , String rootNode
            , String nodeName
            , Action<XmlNode, IList<TElement>> appendAction)
        {
            var maxChances = initialLoad ? 10 : 3;
            var xmlString = String.Empty;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    HttpWebRequest getRequest = (HttpWebRequest)HttpWebRequest.Create(targetUri);
                    getRequest.Credentials = CredentialCache.DefaultCredentials;
                    getRequest.ContentType = "application/x-www-form-urlencoded";
                    getRequest.Method = "POST";
                    getRequest.Timeout = -1;

                    getRequest.ContentLength = 0;

                    using (HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse())
                    using (Stream dataStream = getResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                        xmlString = reader.ReadToEnd();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlString);

                    var result = new List<TElement>();
                    foreach (XmlNode currentNode in doc.SelectSingleNode(rootNode).SelectNodes(nodeName))
                        appendAction(currentNode, result);

                    return result;
                }
                catch (Exception ex)
                {
                    //TODO: looking into ex to see if it is worth retrying
                    var wex = ex as WebException;

                    if (wex != null && wex.Status == WebExceptionStatus.Timeout)
                        Thread.Sleep(5000);
                    else
                        return new List<TElement>();
                }
            }

            return new List<TElement>();
        }
    }
}
