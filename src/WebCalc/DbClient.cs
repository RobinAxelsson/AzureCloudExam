using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started#next-steps
namespace WebCalc
{
    public class DbClient
    {
        private static int counter = 0;
        private Database _database;
        private Container _container;
        private CosmosClient _cosmosClient;
        private IConfiguration _configuration;
        private string _containerName;
        private string _dbName;
        public DbClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _cosmosClient = new CosmosClient(_configuration["accountEndpoint"], _configuration["accountKey"]);
            counter++;

            _dbName = _configuration["CalcDbName"];
            _containerName = _configuration["CalcContainer"];
        }
        private async Task InitDbEnvironment()
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_dbName);
            _container = await _database.CreateContainerIfNotExistsAsync(_containerName, "/" + nameof(Calculation.Operation));
        }

        /// <summary>
        /// Checks if item already exist with id and partition key.
        /// If it does not exist it creates the item in the container.
        /// </summary>
        /// <param name="calculation"></param>
        /// <returns>Returns true if it was added, false if it wasn't.</returns>

        public async Task AddCalculationAsync(Calculation calculation)
        {
            await InitDbEnvironment();
            await _container.CreateItemAsync<Calculation>(calculation, new PartitionKey(calculation.Operation));
        }

        /// <summary>
        /// Runs a query (using Azure Cosmos DB SQL syntax) against the container "all" and retrieves all calculations.
        /// </summary>
        public async Task<List<Calculation>> GetTop10Async()
        {
            await InitDbEnvironment();
            QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM all d ORDER BY d._ts DESC OFFSET 0 LIMIT 10");
            FeedIterator<Calculation> queryResultSetIterator = _container.GetItemQueryIterator<Calculation>(queryDefinition);

            var calculations = new List<Calculation>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Calculation> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Calculation calculation in currentResultSet)
                {
                    calculations.Add(calculation);
                }
            }
            return calculations;
        }
    }
}
