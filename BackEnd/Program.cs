using System;
using Crawler;

namespace BackEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            var Ainunu = new Ainunu();
            var AinunuHtml = Ainunu.GetAinunuContent();
            //Ainunu.GetMovieList(AinunuHtml);
            Console.WriteLine(AinunuHtml);
        }
    }
}
