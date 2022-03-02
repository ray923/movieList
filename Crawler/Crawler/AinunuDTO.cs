using System.Collections.Generic;
using HtmlAgilityPack;

namespace Crawler.Models
{
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