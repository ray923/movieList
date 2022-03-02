using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Crawler;
using Crawler.Models;
using X.Web.Sitemap;
using Newtonsoft.Json;
namespace BackEnd
{
  class Program
  {
    public static int switcher = 3;//1:get Ainunu full movie list, 2:update Ainunu movie list
    private static string baseUrl = "https://www.dianyingbox.com/";
    static async Task Main(string[] args)
    {
      var Ainunu = new Ainunu();
      var result = "";
      switch (switcher)
      {
        case 1:
          Console.WriteLine("Ainunu电影网站全量更新");
          result = await Ainunu.GetAinunuContent(switcher);
          Console.WriteLine(result);
          break;
        case 2:
          Console.WriteLine("Ainunu电影网站增量更新");
          result = await Ainunu.GetAinunuContent(switcher);
          Console.WriteLine(result);
          break;
        case 3:
          Console.WriteLine("生成SiteMap");
          BuildSiteMap();
          Console.WriteLine("done!");
          break;
        default:
          Console.WriteLine("Nothing Done.");
          break;
      }
    }

    private static void BuildSiteMap()
    {
      var sitemap = new Sitemap();
      var JsonFileHelper = new JsonFileHelper();
      var allMovies = JsonFileHelper.ReadJsonFile("Data.json");
      var movieList = JsonConvert.DeserializeObject<List<AinunuMovieDTO>>(allMovies);
      var totalpages = movieList.Count / 48;
      if (movieList.Count % 48 != 0)
      {
        totalpages++;
      }

      for (int i = 1; i <= totalpages; i++)
      {
        sitemap.Add(new Url
        {
          Location = baseUrl + i.ToString(),
          ChangeFrequency = ChangeFrequency.Daily,
          Priority = 0.5,
          TimeStamp = DateTime.Now
        });
      }

      foreach (var movie in movieList)
      {
        sitemap.Add(new Url
        {
          Location = baseUrl + "content/" + movie.id,
          ChangeFrequency = ChangeFrequency.Daily,
          Priority = 0.3,
          TimeStamp = DateTime.Now
        });
      }
      JsonFileHelper.WriteJsonFile("sitemap.xml", sitemap.ToXml());
    }
  }
}
