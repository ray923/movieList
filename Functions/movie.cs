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
  public static class movie
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

    [FunctionName("movie")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      string id = req.Query["id"];
      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      id = id ?? data?.id;

      cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
      // Create a new database
      database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
      container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

      var sqlQueryText = String.Format("SELECT * FROM c where c.id = '{0}'", id);
      QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
      FeedIterator<AinunuMovieDTO> queryResultSetIterator = container.GetItemQueryIterator<AinunuMovieDTO>(queryDefinition);

      List<AinunuMovieDTO> movies = new List<AinunuMovieDTO>();

      while (queryResultSetIterator.HasMoreResults)
      {
        FeedResponse<AinunuMovieDTO> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        foreach (var movie in currentResultSet)
        {
          movies.Add(movie);
        }
      }
      log.LogInformation(String.Format("C# HTTP trigger function processed a request. Id: {0}; Name: {1}", id, movies.FirstOrDefault().Name));

      return new OkObjectResult(movies.FirstOrDefault());
    }
  }
}
