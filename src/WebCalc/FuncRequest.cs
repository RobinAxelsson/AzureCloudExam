using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started#next-steps
namespace WebCalc
{
    public class FuncRequest
    {
        public FuncRequest(IConfiguration configuration, ILogger<FuncRequest> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        private IConfiguration _configuration;
        private readonly ILogger<FuncRequest> _logger;

        public async Task<String> Request(string a, string b, string op)
        {
            string endpoint = Operation.ADDITION == op ? _configuration["AdditionEndpoint"] : _configuration["SubtractionEndpoint"];
            var uriBuilder = new UriBuilder(endpoint);
            var query = uriBuilder.Query;

            var parameters = HttpUtility.ParseQueryString(endpoint.Split('?')[^1]);
            parameters["a"] = a;
            parameters["b"] = b;
            uriBuilder.Query = parameters.ToString();

            Console.WriteLine(uriBuilder.Uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriBuilder.ToString());
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Http request threw, tried to calculate", new object[] { a, b });
                return null;
            }
        }
    }
}