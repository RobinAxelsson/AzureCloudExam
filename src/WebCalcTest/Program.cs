using System;
using System.Threading.Tasks;
using WebCalc;
using Microsoft.Extensions.Configuration;

namespace WebCalcTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddCommandLine(args) //adds accountEnpoint, accountKey, SubtractionEndpoint and AdditionEndpoint
            .Build();

            configuration["CalcDbName"] = "TestDB-DbClient";
            configuration["CalcContainer"] = "TestContainer";

            var dbClient = new DbClient(configuration);

            var addition = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = "1+1=2",
                Operation = Operation.ADDITION
            };

            await dbClient.AddCalculationAsync(addition);

            var subtraction = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = "1+1=2",
                Operation = Operation.SUBTRACTION
            };

            await dbClient.AddCalculationAsync(subtraction);

            var calcs = await dbClient.GetTop10Async();

            var funcRequest = new FuncRequest(configuration);

            await funcRequest.RequestAsync("10", "-5", Operation.SUBTRACTION);

            await funcRequest.RequestAsync("10", "-5", Operation.ADDITION);

            Console.WriteLine("End of WebCalcTest, No exception was thrown!");
        }
    }
}