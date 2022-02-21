using System.Collections.Generic;

namespace Movie.Functions.Data
{
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
}