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
        private ILogger _logger;
        private Database _database;
        private Container _container;
        private CosmosClient _cosmosClient;
        private IConfiguration _configuration;
        private string _containerName;
        private string _dbName;
        public DbClient(ILogger<DbClient> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _cosmosClient = new CosmosClient(_configuration["accountEndpoint"], _configuration["accountKey"]);
            counter++;
            _logger = logger;
            logger.LogInformation($"DbClient number {counter} was instantiated");

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

        public async Task<bool> TryAddCalculation(Calculation calculation)
        {
            await InitDbEnvironment();
            try
            {
                await _container.ReadItemAsync<Calculation>(calculation.Id, new PartitionKey(calculation.Operation));
                return false;
            }
            catch (CosmosException ex)
            {
                await _container.CreateItemAsync<Calculation>(calculation, new PartitionKey(calculation.Operation));
                return true;
            }
        }
        /// <summary>
        /// Checks if item already exist with id and partition key.
        /// If it does exist it creates the item in the container. 
        /// </summary>
        /// <param name="id">The id string of the calculation-object</param>
        /// <param name="partitionKeyValue">The tag value of the calculation-object</param>
        /// <returns></returns>
        public async Task<bool> TryDeleteCalculation(string id, string partitionKeyValue)
        {
            await InitDbEnvironment();
            try
            {
                await _container.ReadItemAsync<Calculation>(id, new PartitionKey(partitionKeyValue));
                await _container.DeleteItemAsync<Calculation>(id, new PartitionKey(partitionKeyValue));
                return true;
            }
            catch (CosmosException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Runs a query (using Azure Cosmos DB SQL syntax) against the container "all" and retrieves all calculations.
        /// </summary>
        public async Task<List<Calculation>> GetAllCalculationsAsync()
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
        /// <summary>
        /// Replace an item in the container
        /// </summary>
        public async Task ReplaceCalculationItemAsync(Calculation calculation)
        {
            await InitDbEnvironment();
            ItemResponse<Calculation> calculationResponse = await _container.ReadItemAsync<Calculation>(calculation.Id, new PartitionKey(calculation.Operation));
            var oldCalculation = calculationResponse.Resource;
            calculationResponse = await _container.ReplaceItemAsync<Calculation>(calculation, oldCalculation.Id, new PartitionKey(oldCalculation.Operation));
        }
        /// <summary>
        /// Delete the database and dispose of the Cosmos Client instance
        /// </summary>
        private async Task DeleteDatabaseAndCleanupAsync()
        {
            await InitDbEnvironment();

            DatabaseResponse databaseResourceResponse = await _database.DeleteAsync();
            // Also valid: await this.cosmosClient.Databases["FamilyDatabase"].DeleteAsync();

            Console.WriteLine("Deleted Database: {0}\n", _containerName);

            //Dispose of CosmosClient
            _cosmosClient.Dispose();
        }
    }
}
