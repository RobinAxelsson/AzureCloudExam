using System;

//https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started#next-steps
namespace WebCalc
{
    public class Operation
    {
        public const string ADDITION = "ADDITION";
        public const string SUBTRACTION = "SUBTRACTION";
        public static string Parse(string op) => op == "0" ? ADDITION : op == "1" ? SUBTRACTION : throw new ArgumentException("Could not parse argument: " + op);
        public static string ToPlusOrMinus(string op) => op == "0" ? "+" : op == "1" ? "-" : throw new ArgumentException("Could not parse argument: " + op);
    }
}
