using System;
using Crawler;

namespace BackEnd
{
  class Program
  {
    static void Main(string[] args)
    {
      var Ainunu = new Ainunu();
      var result = Ainunu.GetAinunuContent();//Ainunu电影网站
      Console.WriteLine(result);
    }
  }
}
