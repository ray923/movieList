using System;
using Crawler;

namespace BackEnd
{
  class Program
  {
    public static int switcher = 2;//1:get Ainunu full movie list, 2:update Ainunu movie list
    static void Main(string[] args)
    {
      var Ainunu = new Ainunu();
      switch (switcher)
      {
        case 1:
          var resultFull = Ainunu.GetAinunuContent();//Ainunu电影网站全量更新
          Console.WriteLine(resultFull);
          break;
        case 2:
          var resultUpdate = Ainunu.UpdateAinunuContent();//Ainunu电影网站增量更新
          Console.WriteLine(resultUpdate);
          break;
        default:
          Console.WriteLine("Nothing Done.");
          break;
      }
    }
  }
}
