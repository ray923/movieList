using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler
{
  public class Ainunu
  {
    private string baseUrl = "http://video.ainunu.net";//邮 箱：khp876*gmail.com （把*替换成@）
    private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndpointUri"];
    private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
    // The Cosmos client instance
    private CosmosClient cosmosClient;

    // The database we will create
    private Database database;

    // The container we will create.
    private Container detailContainer;
    private Container downloadContainer;

    // The name of the database and container we will create
    private string databaseId = "Movies";
    private string detailContainerId = "Details";
    private string downloadContainerId = "Downloads";
    public async Task<string> GetAinunuContent(int switcher)
    {
      this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
      // Create a new database
      this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
      this.detailContainer = await this.database.CreateContainerIfNotExistsAsync(detailContainerId, "/partitionKey");
      this.downloadContainer = await this.database.CreateContainerIfNotExistsAsync(downloadContainerId, "/partitionKey");

      var JsonFileHelper = new JsonFileHelper();

      string exsistMoives = "";
      string exsistDownloads = "";
      var tempMoives = new List<AinunuMovieDTO>();
      var tempDownloads = new List<AinunuDownloadDTO>();
      //增量更新
      if (switcher == 2)
      {
        exsistMoives = JsonFileHelper.ReadJsonFile("Data.json");
        exsistDownloads = JsonFileHelper.ReadJsonFile("Download.json");
        tempMoives = JsonConvert.DeserializeObject<List<AinunuMovieDTO>>(exsistMoives);
        tempDownloads = JsonConvert.DeserializeObject<List<AinunuDownloadDTO>>(exsistDownloads);
      }

      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      var movieList = new List<AinunuMovieDTO>();
      var downloadList = new List<AinunuDownloadDTO>();

      var totalPageUrl = baseUrl + "/c/movie/";
      var helper = new CrawlerHelper();
      var html = helper.DownloadHtml(totalPageUrl, Encoding.GetEncoding("GB2312"));
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
      HtmlNode totalPage = document.DocumentNode.SelectSingleNode("/html/body/div[4]/div[1]/div[3]/div/ul/li[15]/span/strong[1]");
      var totalPageCount = int.Parse(totalPage.InnerText);
      HtmlNode totalMovies = document.DocumentNode.SelectSingleNode("/html/body/div[4]/div[1]/div[3]/div/ul/li[15]/span/strong[2]");
      //Start Id 12286 total -- 2021/1/2
      var Id = int.Parse(totalMovies.InnerText);
      Id = Id > 0 ? Id : 15000;//起始Id

      for (int i = 1; i <= totalPageCount; i++)//410 total -- 2022/1/2 TODO:动态读取总页数
      {
        var breakFlag = false;
        var url = baseUrl + "/c/movie/list_" + i.ToString() + ".html";
        html = helper.DownloadHtml(url, Encoding.GetEncoding("GB2312"));
        if (html == "") break;
        HtmlNodeCollection nodes = GetMovieList(html);

        for (int j = 0; j < nodes.Count; j++)
        {
          HtmlNode node = nodes[j].SelectSingleNode("child::a[2]");
          string movieCategory = nodes[j].SelectSingleNode("child::a[1]").InnerHtml;
          string movieUpdateTime = nodes[j].SelectSingleNode("child::span").InnerHtml;
          string detaiRelativelUrl = node.GetAttributeValue("href", "");
          string movieDetailPageUrl = baseUrl + detaiRelativelUrl;
          var urlName = node.InnerHtml;
          Console.WriteLine(urlName);
          if (tempMoives.Exists(x => x.UrlName == urlName) && switcher == 2)
          {
            breakFlag = true;
            break;
          }
          try
          {
            var detail = GetMovieDetail(movieDetailPageUrl, Id);
            if (detail.movie != null)
            {
              detail.movie.UpdateDate = movieUpdateTime;
              detail.movie.Category = movieCategory;
              detail.movie.partitionKey = movieCategory;
              detail.movie.UrlName = urlName;
              await InsertMovieIntoCosmos(detail.movie);
              movieList.Add(detail.movie);
              var download = GetMovieDownloadDetail(detail.movie.DownloadUrl, detail.downloadNodes, detail.movie.Name, Id);
              if (download != null)
              {
                downloadList.Add(download);
                await InsertDownloadIntoCosmos(download);
              }
            }
          }
          catch (Exception ex)
          {
            LogHelper.Error(string.Format(ex.Message + " " + node.InnerHtml), ex);
          }
          Id -= 1;
        }
        if (breakFlag) break;
      }
      var newMovieList = movieList.Concat(tempMoives).ToList();
      var newDownloadList = downloadList.Concat(tempDownloads).ToList();
      JsonFileHelper.WriteJsonFile("Data.json", JsonConvert.SerializeObject(newMovieList));
      JsonFileHelper.WriteJsonFile("Download.json", JsonConvert.SerializeObject(newDownloadList));
      // JsonFileHelper.WriteJsonFile("../FrontEnd/src/data/movie.json", JsonConvert.SerializeObject(newMovieList));
      // JsonFileHelper.WriteJsonFile("../FrontEnd/src/data/download.json", JsonConvert.SerializeObject(newDownloadList));
      return "grab done Total:" + movieList.Count.ToString();
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
        id = Id.ToString(),
        MovieId = Id,
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
        id = MovieId.ToString(),
        MovieId = MovieId,
        MovieName = movieName,
        Resources = Resources,
        UpdateDate = "",
        CreateDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        partitionKey = "download"
      };
      return download;
    }
    private async Task InsertMovieIntoCosmos(AinunuMovieDTO movie)
    {
      try
      {
        // Read the item to see if it exists.  
        ItemResponse<AinunuMovieDTO> movieResponse = await this.detailContainer.ReadItemAsync<AinunuMovieDTO>(movie.id, new PartitionKey(movie.partitionKey));
        Console.WriteLine("Item in database with id: {0} already exists\n", movieResponse.Resource.id);
      }
      catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
      {
        // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
        ItemResponse<AinunuMovieDTO> movieResponse = await this.detailContainer.CreateItemAsync<AinunuMovieDTO>(movie, new PartitionKey(movie.partitionKey));

        // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
        Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", movieResponse.Resource.id, movieResponse.RequestCharge);
      }
    }

    private async Task InsertDownloadIntoCosmos(AinunuDownloadDTO movie)
    {
      try
      {
        // Read the item to see if it exists.  
        ItemResponse<AinunuDownloadDTO> movieResponse = await this.downloadContainer.ReadItemAsync<AinunuDownloadDTO>(movie.id, new PartitionKey(movie.partitionKey));
        Console.WriteLine("Item in database with id: {0} already exists\n", movieResponse.Resource.id);
      }
      catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
      {
        // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
        ItemResponse<AinunuDownloadDTO> movieResponse = await this.downloadContainer.CreateItemAsync<AinunuDownloadDTO>(movie, new PartitionKey(movie.partitionKey));

        // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
        Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", movieResponse.Resource.id, movieResponse.RequestCharge);
      }
    }
  }

  public class AinunuMovieDTO
  {
    public string id { get; set; }
    public int MovieId { get; set; }
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
    public string partitionKey { get; set; }
    public string UrlName { get; set; }
  }

  public class AinunuDownloadDTO
  {
    public string id { get; set; }
    public int MovieId { get; set; }
    public string MovieName { get; set; }
    public List<ResourceDTO> Resources { get; set; }
    public string UpdateDate { get; set; }
    public string CreateDate { get; set; }
    public string partitionKey { get; set; }
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
