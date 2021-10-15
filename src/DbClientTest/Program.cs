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

            configuration["AdditionEndpoint"] = configuration["ADDITIONENDPOINT"];
            configuration["SubtractionEndpoint"] = configuration["SUBTRACTIONENDPOINT"];
            var funcRequest = new FuncRequest(configuration);

            var answer = await funcRequest.RequestAsync("10", "-5", Operation.SUBTRACTION);

            if (answer != "15") throw new Exception("The answer is 15 not: " + answer);
        }
    }
}