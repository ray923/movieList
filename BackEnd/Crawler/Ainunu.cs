using System;
using System.Text;

namespace Crawler
{
    public class Ainunu
    {
        public string GetAinunuContent()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var url = "http://video.ainunu.net/";
            var helper = new CrawlerHelper();
            var html = helper.DownloadHtml(url, Encoding.GetEncoding("GB2312"));
            return html;
        }
    }
}
