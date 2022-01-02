using System;
using System.IO;

namespace Crawler
{
  public class JsonFileHelper
  {
    public void WriteJsonFile(string filePath, string json)
    {
      using (StreamWriter sw = new StreamWriter(filePath))
      {
        sw.Write(json);
      }
    }
  }
}