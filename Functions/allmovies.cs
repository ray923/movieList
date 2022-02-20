using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Movies.Function
{
  public static class allmovies
  {
    private static readonly string EndpointUri = "https://ray-movie.documents.azure.com:443/";
    private static readonly string PrimaryKey = "qDEJv6fymsPZpMro2iAOKvEZm0vb22RfqbOpVMDK1Y57AI1O8dpTL46H5jtGIZXJTE8r3pPPb28gQ64fzwBYcw==";
    // The Cosmos client instance
    private static CosmosClient cosmosClient;
    // The database we will create
    private static Database database;
    // The container we will create.
    private static Container container;
    // The name of the database and container we will create
    private static string databaseId = "Movies";
    private static string containerId = "Details";
    private static int pageSize = 48;

    [FunctionName("allmovies")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
      // Create a new database
      database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
      container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
      var sqlQueryText = "SELECT * FROM c";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
      FeedIterator<AinunuMovieDTO> queryResultSetIterator = container.GetItemQueryIterator<AinunuMovieDTO>(queryDefinition);

      List<AinunuMovieDTO> movies = new List<AinunuMovieDTO>();

      while (queryResultSetIterator.HasMoreResults)
      {
        FeedResponse<AinunuMovieDTO> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        foreach (AinunuMovieDTO movie in currentResultSet)
        {
          movies.Add(movie);
        }
      }
      string page = req.Query["page"];

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      page = page ?? data?.page;

      log.LogInformation(String.Format("C# HTTP trigger function processed a request. Page: {0}; Movies: {1}", page, movies.Count));
      return new OkObjectResult(movies.Skip(pageSize * (int.Parse(page) - 1)).Take(pageSize).ToList());
    }
  }

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
}
