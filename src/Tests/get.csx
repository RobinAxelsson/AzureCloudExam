using System;
using System.Net;
using System.Web;

var args = Environment.GetCommandLineArgs();
var uriBuilder = new UriBuilder(args[2]);
var parameters = HttpUtility.ParseQueryString(string.Empty);
parameters["a"] = args[3];
parameters["b"] = args[4];
uriBuilder.Query = parameters.ToString();

HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
try
{

    using (var response = (HttpWebResponse)await request.GetResponseAsync())
    using (var stream = response.GetResponseStream())
    using (var reader = new StreamReader(stream))
    {
        Console.WriteLine(response.StatusCode);
        if (response.StatusCode == HttpStatusCode.OK)
            Console.WriteLine(await reader.ReadToEndAsync());
    }
}
catch (System.Net.WebException ex)
{
    Console.WriteLine(ex);
}