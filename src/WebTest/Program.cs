using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace CosmosTest
{
    public class Calculation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CalculationString { get; set; }
        public string Operation { get; set; } //Partition
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class Operation
    {
        public const string ADDITION = "ADDITION";
        public const string SUBTRACTION = "SUBTRACTION";
        public static string Parse(string op) => op == "0" ? ADDITION : op == "1" ? SUBTRACTION : throw new ArgumentException("Could not parse argument: " + op);
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            args.ToList().ForEach(a => Console.WriteLine(a));
            var accountEndpoint = Environment.GetEnvironmentVariable("accountEndpoint");
            var accountKey = Environment.GetEnvironmentVariable("accountKey");
            var cosmosClient = new CosmosClient(accountEndpoint, accountKey);

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("CalcDB-PROD");

            Container container = await database.CreateContainerIfNotExistsAsync("CalcCont-PROD", "/" + nameof(Calculation.Operation));

            var calculation = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = args[0],
                Operation = args[1] == Operation.ADDITION ? Operation.ADDITION : args[1] == Operation.SUBTRACTION ? Operation.SUBTRACTION : throw new ArgumentException("Need to input Operation as argument.")
            };

            await container.CreateItemAsync<Calculation>(calculation, new PartitionKey(calculation.Operation));
        }
    }
}