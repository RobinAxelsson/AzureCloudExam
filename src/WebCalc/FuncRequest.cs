using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

//https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started#next-steps
namespace WebCalc
{
    public class FuncRequest
    {
        public FuncRequest(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IConfiguration _configuration;
        public async Task<String> RequestAsync(string a, string b, string op)
        {
            string endpoint = Operation.ADDITION == op ? _configuration["AdditionEndpoint"] : _configuration["SubtractionEndpoint"];
            var uriBuilder = new UriBuilder(endpoint);
            var query = uriBuilder.Query;

            var parameters = HttpUtility.ParseQueryString(endpoint.Split('?')[^1]);
            parameters["a"] = a;
            parameters["b"] = b;
            uriBuilder.Query = parameters.ToString();

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
                throw new System.Exception($"Error FuncRequest class: requesting function calculations with endpoint: {uriBuilder.ToString()}, a: {a}, b: {b}", ex);
            }
        }
    }
}