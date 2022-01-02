using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler
{
  public class Ainunu
  {
    private int Id = 0;//Start Id
    private string baseUrl = "http://video.ainunu.net";
    public string GetAinunuContent()
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      var movieList = new List<AinunuMovieDTO>();
      //for (int i = 0; i >= 0; i++)
      for (int i = 1; i <= 410; i++)//410 total
      {
        var url = baseUrl + "/c/movie/list_" + i.ToString() + ".html";
        var helper = new CrawlerHelper();
        var html = helper.DownloadHtml(url, Encoding.GetEncoding("GB2312"));
        if (html == "") break;
        HtmlNodeCollection nodes = GetMovieList(html);

        for (int j = 0; j < nodes.Count; j++)
        {
          HtmlNode node = nodes[j].SelectSingleNode("child::a[2]");
          string detaiRelativelUrl = node.GetAttributeValue("href", "");
          string movieDetailPageUrl = baseUrl + detaiRelativelUrl;
          Id += 1;
          var movie = GetMovieDetail(movieDetailPageUrl, Id);
          if (movie != null)
          {
            movieList.Add(movie);
          }
        }
      }
      return JsonConvert.SerializeObject(movieList);
    }

    public HtmlNodeCollection GetMovieList(string htmlContent)
    {
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(htmlContent);
      HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[4]/div[1]/div[3]/ul/li");
      return nodes;
    }

    public AinunuMovieDTO GetMovieDetail(string movieDetailPageUrl, int Id)
    {
      Console.WriteLine(Id);
      var helper = new CrawlerHelper();
      var html = helper.DownloadHtml(movieDetailPageUrl, Encoding.GetEncoding("GB2312"));
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
      //HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[3]/div[0]");
      //if (nodes == null) { return null; }
      HtmlNode node = document.DocumentNode.SelectSingleNode("/html/body/div[4]/div[1]/div[3]");
      if (node == null) { return null; }
      var movieName = node.SelectSingleNode("child::div[1]");
      if (movieName == null) { return null; }
      string UpdateDate = node.SelectSingleNode("child::div[2]") != null ? node.SelectSingleNode("child::div[2]").InnerText : "";
      HtmlNode imgNode = node.SelectSingleNode("child::div[3]/p[1]/img");
      string ImgUrl = imgNode != null ? imgNode.GetAttributeValue("src", "") : "";
      string Overview = node.SelectSingleNode("child::div[3]/p[2]") != null ? node.SelectSingleNode("child::div[3]/p[2]").InnerHtml : "";
      string Introduction = node.SelectSingleNode("child::div[3]/p[3]") != null ? node.SelectSingleNode("child::div[3]/p[3]").InnerHtml : "";
      string downloadPageUrl = node.SelectSingleNode("child::div[3]/div[1]/a") != null ? node.SelectSingleNode("child::div[3]/div[1]/a").GetAttributeValue("href", "") : "";
      var movie = new AinunuMovieDTO()
      {
        Id = Id,
        Name = movieName.InnerText,
        ListImgUrl = ImgUrl,
        UpdateDate = UpdateDate,
        ImgUrl = ImgUrl,
        Overview = Overview,
        Introduction = Introduction,
        DownloadUrl = downloadPageUrl
      };
      return movie;
      //GetMovieDownloadDetail(downloadPageUrl, Id);
    }

    public void GetMovieDownloadDetail(string downloadPageUrl, int MovieId)
    {
      var helper = new CrawlerHelper();
      var html = helper.DownloadHtml(downloadPageUrl, Encoding.GetEncoding("GB2312"));
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
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
  }

  public class AinunuDownloadDTO
  {
    public string Id { get; set; }
    public string MoiveId { get; set; }
    public string MovieName { get; set; }
    public string OnlinePlay { get; set; }
    public string Resource { get; set; }
    public string UpdateDate { get; set; }
    public string CreateDate { get; set; }
  }
}
