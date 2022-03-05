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
using Movie.Functions.Data;
using Newtonsoft.Json;

namespace Movies.Function
{
  public static class search
  {
    private static readonly string EndpointUri = "";
    private static readonly string PrimaryKey = "";
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

    [FunctionName("search")]
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
      string keyword = req.Query["keyword"];
      var result = movies.FindAll(x => x.Name.ToLower().Contains(keyword.ToLower())
        || x.Introduction.ToLower().Contains(keyword.ToLower())
        || x.Overview.ToLower().Contains(keyword.ToLower()));
      string page = req.Query["page"];

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      page = page ?? data?.page;

      log.LogInformation(String.Format("C# HTTP trigger function processed a request. Page: {0}; Movies: {1}", page, result.Count));
      return new OkObjectResult(result.Skip(pageSize * (int.Parse(page) - 1)).Take(pageSize).ToList());
    }
  }
}
