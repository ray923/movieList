using System;
using Crawler;
using System.Threading.Tasks;

namespace BackEnd
{
  class Program
  {
    public static int switcher = 1;//1:get Ainunu full movie list, 2:update Ainunu movie list
    static async Task Main(string[] args)
    {
      switch (switcher)
      {
        case 1:
          Console.WriteLine("Ainunu电影网站全量更新");
          break;
        case 2:
          Console.WriteLine("Ainunu电影网站增量更新");
          break;
        default:
          Console.WriteLine("Nothing Done.");
          break;
      }
      var Ainunu = new Ainunu();
      var result = await Ainunu.GetAinunuContent(switcher);
      Console.WriteLine(result);
    }
  }
}
