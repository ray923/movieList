using System;
using Crawler;

namespace BackEnd
{
  class Program
  {
    static void Main(string[] args)
    {
      var Ainunu = new Ainunu();
      var AinunuMoiveJson = Ainunu.GetAinunuContent();
      var JsonFileHelper = new JsonFileHelper();
      JsonFileHelper.WriteJsonFile("Data.json", AinunuMoiveJson);
      //Console.WriteLine(AinunuHtml);
    }
  }
}
