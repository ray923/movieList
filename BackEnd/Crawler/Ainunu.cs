using System;
using System.Text;
using HtmlAgilityPack;

namespace Crawler
{
    public class Ainunu
    {
        private string baseUrl = "http://video.ainunu.net";
        public string GetAinunuContent()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //for (int i = 0; i >= 0; i++)
            for (int i = 1; i <= 1; i++)
            {
                var url = baseUrl + "/c/movie/list_" + i.ToString() + ".html";
                var helper = new CrawlerHelper();
                var html = helper.DownloadHtml(url, Encoding.GetEncoding("GB2312"));
                if (html == "") break;
                HtmlNodeCollection nodes = GetMovieList(html);
                HtmlNode node = nodes[0].SelectSingleNode("child::a[2]");
                string detaiRelativelUrl = node.GetAttributeValue("href","");
                string movieDetailUrl = baseUrl + detaiRelativelUrl;
                GetMovieDetail(movieDetailUrl);
            }
            return "end";
        }

        public HtmlNodeCollection GetMovieList(string htmlContent)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);
            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[4]/div[1]/div[3]/ul/li");
            return nodes;
        }

        public void GetMovieDetail(string htmlContent)
        {
            var helper = new CrawlerHelper();
            var html = helper.DownloadHtml(htmlContent, Encoding.GetEncoding("GB2312"));
        }
    }
}
