using System;
using System.IO;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace freeRSS.Common
{
    public static class UpdateTile
    {
        public static void UpDateTile(Array articleArray)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText("tile.xml", Encoding.UTF8));
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            //循环次数系要显示噶文章数量，一定要小于等于下边数组噶size
            for (int j = 0; j < 3; j++)
            {
                Models.ArticleModel article = (Models.ArticleModel)articleArray.GetValue(j); /*哩度要将文章传入一个数组再传比article*/
                XmlNodeList texts = xmlDocument.GetElementsByTagName("text");

                texts[0].InnerText = article.Title;
                texts[1].InnerText = article.Title;
                texts[2].InnerText = article.Summary;
                texts[4].InnerText = article.PubDate.ToString();
                texts[5].InnerText = article.Title;
                texts[6].InnerText = article.Description;
                texts[8].InnerText = article.PubDate.ToString();
                texts[9].InnerText = article.Title;
                texts[10].InnerText = article.Summary;
                texts[18].InnerText = article.PubDate.ToString();
                var notification = new TileNotification(xmlDocument);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
        }
    }
}
