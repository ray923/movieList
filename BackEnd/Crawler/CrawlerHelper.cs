using System;
using System.IO;
using System.Net;
using System.Text;

namespace Crawler
{
  public class CrawlerHelper
  {
    public string DownloadHtml(string url, Encoding encode)
    {
      string html = string.Empty;
      try
      {
        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
        request.Timeout = 30 * 1000; // request timeout 30s
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
        request.ContentType = "text/html; charset=utf-8";
        //request.Host = "";
        request.Method = "GET";
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
          if (response.StatusCode != HttpStatusCode.OK)
          {
            //To do Log4net;
            LogHelper.Info(string.Format("catch{0} failure", url));
          }
          else
          {
            try
            {
              StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
              html = sr.ReadToEnd();
              sr.Close();
            }
            catch (Exception ex)
            {
              LogHelper.Error(string.Format($"catch{url} failure"), ex);
              html = null;
            }
          }
        }
      }
      catch (System.Net.WebException ex)
      {
        if (ex.Message.Equals("306"))
        {
          LogHelper.Error("306", ex);
          html = null;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(string.Format("Downlaod {0} error", url), ex);
        html = null;
      }
      return html;
    }
  }
}
