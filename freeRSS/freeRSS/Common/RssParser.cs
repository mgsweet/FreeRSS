using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Web.Syndication;

namespace freeRSS.Common
{
    public static class RssParser
    {
        public static void SaveValueAsXml(this SyndicationFeed feed)
        {

            var a = feed.Links.ToArray();

            var temp = feed.GetXmlDocument(SyndicationFormat.Rss20);

        }
    }
}
