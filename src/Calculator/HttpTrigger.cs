using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
namespace Function
{
    //dotnet add package Azure.Security.KeyVault.Secrets --version 4.2.0
    //https://devkimchi.com/2020/04/30/3-ways-referencing-azure-key-vault-from-azure-functions/
    //https://zimmergren.net/azure-functions-key-vault-reference-azurewebjobsstorage/
    //https://docs.microsoft.com/en-us/azure/azure-functions/functions-host-json
    //https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-output#hostjson-settings
    public static class HttpTrigger
    {
        public static readonly string DefaultResponse = "Add query parameters ?a=1&b=3 to use the calculator." + Environment.GetEnvironmentVariable("SuperSecret");
        public static string responseCalculation(string decimalA, string decimalB, bool isAddition) =>
            isAddition ? $"{Convert.ToDecimal(decimalA) + Convert.ToDecimal(decimalB)}" :
            $"{Convert.ToDecimal(decimalA) - Convert.ToDecimal(decimalB)}";

        public static string ValidateInput(string input)
        {
            if (input == null) return null;                                                                         //Null check
            if (input.Length > 20) return null;                                                                     //Check for decimal type overflow
            if (input.ToList().Aggregate(0, (acc, c) => char.IsDigit(c) ? acc + 1 : acc + 0) == 0) return null;     //At least one digit
            if (input.ToList().TrueForAll(c => !char.IsDigit(c) && c != '.' && c != '-')) return null;              // Only digits and "." or "-"
            if (input.ToList().Aggregate(0, (acc, x) => x == '.' ? acc + 1 : acc + 0) > 1) return null;             //Only one "."
            if (input.ToList().Aggregate(0, (acc, x) => x == '.' ? acc + 1 : acc + 0) == 1 && input[0] == '.') return null;     //dot is not first
            if (input.ToList().Aggregate(0, (acc, x) => x == '-' ? acc + 1 : acc + 0) > 1) return null;                         //Only one "-"
            if (input.ToList().Aggregate(0, (acc, x) => x == '-' ? acc + 1 : acc + 0) == 1 && input[0] != '-') return null;     //"-" is first
            return input;
        }
        [FunctionName("HttpTrigger")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string Operation = Environment.GetEnvironmentVariable("Operation");
            bool isAdd = Operation == "ADDITION" ? true :
            Operation == "SUBTRACTION" ? false : throw new ArgumentException("Input variables are incorrect", Operation);

            log.LogInformation($"-----------NEW REQUEST TO {Operation}-----------------");
            log.LogInformation("C# HTTP trigger function processed a request.");

            string a = req.Query["a"];
            string b = req.Query["b"];

            string response = ValidateInput(a) != null && ValidateInput(b) != null ? responseCalculation(a, b, isAdd) : null;

            //If no value is null, calculation is returned with status code 200.
            if (response != null) return new OkObjectResult(response);

            //else response is the default message with status code 400.
            return new BadRequestObjectResult(DefaultResponse);
        }
    }
}
