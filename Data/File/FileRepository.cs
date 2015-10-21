using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Xml;

namespace Site.Data.File
{
    public abstract class FileRepository
    {
        public IList<TElement> UntilDovesCry<TElement>(String fileLocation
            , String rootNode
            , String nodeName
            , Action<XmlNode, IList<TElement>> appendAction)
        {
            var resolvedPath = HostingEnvironment.MapPath(fileLocation);

            // do our own mapping, because we could be called without a Context-in-flight
            var getFile = new XmlDocument();
            getFile.Load(resolvedPath);

            var result = new List<TElement>();
            foreach (XmlNode currentNode in getFile.SelectSingleNode(rootNode).SelectNodes(nodeName))
                appendAction(currentNode, result);

            return result;
        }
    }
}
