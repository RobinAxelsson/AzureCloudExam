using System;
using System.Threading.Tasks;
using WebCalc;
using Microsoft.Extensions.Configuration;

namespace DbClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddCommandLine(args) //adds accountEnpoint and accountKey
            .Build();

            configuration["CalcDbName"] = "TestDB-DbClient";
            configuration["CalcContainer"] = "TestContainer";

            var dbClient = new DbClient(configuration);

            var calculation = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = "1+1=2",
                Operation = Operation.ADDITION
            };

            await dbClient.AddCalculationAsync(calculation);
        }
    }
}