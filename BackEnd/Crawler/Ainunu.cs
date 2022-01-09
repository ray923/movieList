using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler
{
  public class Ainunu
  {
    private int Id = 12286;//Start Id 12286 total -- 2021/1/2
    private string baseUrl = "http://video.ainunu.net";//邮 箱：khp876*gmail.com （把*替换成@）
    public string GetAinunuContent()
    {
      var JsonFileHelper = new JsonFileHelper();
      string exsistMoives = JsonFileHelper.ReadJsonFile("Data.json");
      var tempMoives = JsonConvert.DeserializeObject<List<AinunuMovieDTO>>(exsistMoives);
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      var movieList = new List<AinunuMovieDTO>();
      var downloadList = new List<AinunuDownloadDTO>();
      //for (int i = 0; i >= 0; i++)
      for (int i = 1; i <= 410; i++)//410 total -- 2021/1/2
      {
        var url = baseUrl + "/c/movie/list_" + i.ToString() + ".html";
        var helper = new CrawlerHelper();
        var html = helper.DownloadHtml(url, Encoding.GetEncoding("GB2312"));
        if (html == "") break;
        HtmlNodeCollection nodes = GetMovieList(html);

        for (int j = 0; j < nodes.Count; j++)
        {
          HtmlNode node = nodes[j].SelectSingleNode("child::a[2]");
          string movieCategory = nodes[j].SelectSingleNode("child::a[1]").InnerHtml;
          string movieUpdateTime = nodes[j].SelectSingleNode("child::span").InnerHtml;
          string detaiRelativelUrl = node.GetAttributeValue("href", "");
          string movieDetailPageUrl = baseUrl + detaiRelativelUrl;
          Id -= 1;
          var urlName = node.InnerHtml;
          Console.WriteLine(urlName);
          try
          {
            var detail = GetMovieDetail(movieDetailPageUrl, Id);
            if (detail.movie != null)
            {
              detail.movie.UpdateDate = movieUpdateTime;
              detail.movie.Category = movieCategory;
              movieList.Add(detail.movie);
              var download = GetMovieDownloadDetail(detail.movie.DownloadUrl, detail.downloadNodes, detail.movie.Name, Id);
              if (download != null)
              {

                downloadList.Add(download);
              }
            }
          }
          catch (Exception ex)
          {
            LogHelper.Error(string.Format(ex.Message + " " + node.InnerHtml), ex);
          }
        }
      }
      JsonFileHelper.WriteJsonFile("Data.json", JsonConvert.SerializeObject(movieList));
      JsonFileHelper.WriteJsonFile("Download.json", JsonConvert.SerializeObject(downloadList));
      return "done";
    }

    public HtmlNodeCollection GetMovieList(string htmlContent)
    {
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(htmlContent);
      HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[4]/div[1]/div[3]/ul/li");
      return nodes;
    }

    public DetailResult GetMovieDetail(string movieDetailPageUrl, int Id)
    {
      Console.WriteLine(Id);
      var helper = new CrawlerHelper();
      var html = helper.DownloadHtml(movieDetailPageUrl, Encoding.GetEncoding("GB2312"));
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
      HtmlNode node = document.DocumentNode.SelectSingleNode("/html/body/div[4]/div[1]/div[3]");
      if (node == null) { return null; }
      var movieName = node.SelectSingleNode("child::div[1]");
      if (movieName == null) { return null; }
      HtmlNode imgNode = node.SelectSingleNode("child::div[3]/p[1]/img");
      string ImgUrl = imgNode != null ? imgNode.GetAttributeValue("src", "") : "";
      string Overview = node.SelectSingleNode("child::div[3]/p[2]") != null ? node.SelectSingleNode("child::div[3]/p[2]").InnerHtml : "";
      string Introduction = node.SelectSingleNode("child::div[3]/p[3]") != null ? node.SelectSingleNode("child::div[3]/p[3]").InnerHtml : "";
      string downloadPageUrl = node.SelectSingleNode("child::div[3]/div[1]/a") != null ? node.SelectSingleNode("child::div[3]/div[1]/a").GetAttributeValue("href", "") : "";
      HtmlNodeCollection downloadNodes = null;
      if (downloadPageUrl == "")
      {
        downloadNodes = node.SelectNodes("child::div[3]/div");
      }
      var movie = new AinunuMovieDTO()
      {
        Id = Id,
        Name = movieName.InnerText,
        ListImgUrl = ImgUrl,
        ImgUrl = ImgUrl,
        Overview = Overview,
        Introduction = Introduction,
        DownloadUrl = downloadPageUrl,
        CreateDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
      };
      return new DetailResult()
      {
        movie = movie,
        downloadNodes = downloadNodes
      };
    }

    public AinunuDownloadDTO GetMovieDownloadDetail(string downloadPageUrl, HtmlNodeCollection downloadNodes, string movieName, int MovieId)
    {
      HtmlNodeCollection nodes = null;
      if (downloadPageUrl != null && downloadPageUrl != "")
      {
        var helper = new CrawlerHelper();
        var html = helper.DownloadHtml(downloadPageUrl, Encoding.GetEncoding("UTF-8"));
        HtmlDocument document = new HtmlDocument();
        if (html == null || html == "") return null;
        document.LoadHtml(html);
        nodes = document.DocumentNode.SelectNodes("/html/body/div[1]/div[1]/div[1]/div[1]/article[1]/div[1]/div");
      }
      else
      {
        nodes = downloadNodes;
      }
      if (nodes == null || nodes.Count < 2) return null;
      var Resources = new List<ResourceDTO>();
      for (int i = 0; i < nodes.Count; i = i + 2)//第一个是说明内容的div
      {
        HtmlNode node = nodes[i];
        if (i < nodes.Count)
        {
          HtmlNode node2 = null;
          if (i + 1 < nodes.Count)
          {
            node2 = nodes[i + 1];
          }
          string formatName = node.InnerHtml;
          var resourceLinks = new List<ResourceLinkDTO>();
          HtmlNodeCollection resourceNodes = node2 == null ? null : node2.SelectNodes("child::a");
          HtmlNodeCollection resourceOthersNodes = node2 == null ? null : node2.SelectNodes("child::text()");
          if (resourceNodes != null)
          {
            for (int j = 0; j < resourceNodes.Count; j++)
            {
              string resourceLinkName = resourceNodes[j].InnerHtml;
              string resourceLinkUrl = resourceNodes[j].GetAttributeValue("href", "");
              string resourceLinkOther = resourceOthersNodes != null ? resourceOthersNodes[j].InnerHtml : "";
              if (resourceLinkUrl != null)
              {
                var entity = new ResourceLinkDTO()
                {
                  Name = resourceLinkName,
                  Type = "",
                  Url = resourceLinkUrl,
                  Others = resourceLinkOther
                };
                resourceLinks.Add(entity);
              }
            }
          }
          var resource = new ResourceDTO()
          {
            Id = i.ToString(),
            FormatName = formatName,
            ResourceLinks = resourceLinks
          };
          Resources.Add(resource);
        }
      }
      var download = new AinunuDownloadDTO()
      {
        Id = MovieId.ToString(),
        MovieId = MovieId.ToString(),
        MovieName = movieName,
        Resources = Resources,
        UpdateDate = "",
        CreateDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
      };
      return download;
    }
  }

  public class AinunuMovieDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string ListImgUrl { get; set; }
    public string SubTitle { get; set; }
    public string ContentTitle { get; set; }
    public string ImgUrl { get; set; }
    public string Introduction { get; set; }
    public string DownloadUrl { get; set; }
    public string Overview { get; set; }
    public string Caption { get; set; }
    public string Format { get; set; }
    public string UpdateDate { get; set; }
    public string CreateDate { get; set; }
    public string Category { get; set; }
  }

  public class AinunuDownloadDTO
  {
    public string Id { get; set; }
    public string MovieId { get; set; }
    public string MovieName { get; set; }
    public List<ResourceDTO> Resources { get; set; }
    public string UpdateDate { get; set; }
    public string CreateDate { get; set; }
  }

  public class ResourceDTO
  {
    public string Id { get; set; }
    public string FormatName { get; set; }
    public List<ResourceLinkDTO> ResourceLinks { get; set; }
  }

  public class ResourceLinkDTO
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public string Others { get; set; }
  }

  public class DetailResult
  {
    public AinunuMovieDTO movie { get; set; }
    public HtmlNodeCollection downloadNodes { get; set; }
  }
}
