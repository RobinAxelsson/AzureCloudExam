using Newtonsoft.Json;

//https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started#next-steps
namespace WebCalc
{
    public class Calculation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CalculationString { get; set; }
        public string Operation { get; set; } //Partition
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
