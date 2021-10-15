using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebCalc.Pages
{
    public class IndexModel : PageModel
    {
        public FuncRequest _functionClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly DbClient _dbClient;

        public IndexModel(FuncRequest functionClient, ILogger<IndexModel> logger, DbClient dbClient)
        {
            _functionClient = functionClient;
            _logger = logger;
            _dbClient = dbClient;
        }

        public List<string> Answers { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("Page was requested.");
            await TryGetAnswersAsync();
            return Page();
        }
        [BindProperty]
        public string a { get; set; }
        [BindProperty]
        public string b { get; set; }
        [BindProperty]
        public string op { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"POSTED:a={a} b={b} op={Operation.Parse(op)}");

            var response = await _functionClient.RequestAsync(a, b, Operation.Parse(op));

            if (response == null) return Page();

            string calcString = $"{a}{Operation.ToPlusOrMinus(op)}{b}={response}";

            var calculation = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = calcString,
                Operation = Operation.Parse(op)
            };
            await TryAddCalculationAsync(calculation);
            await TryGetAnswersAsync();
            return Page();
        }
        public async Task TryGetAnswersAsync()
        {

            List<Calculation> calcs = null;
            try
            {
                calcs = await _dbClient.GetTop10Async();
                Answers = calcs.Count == 0 ? new List<string>() { "No Calculations Made" } : calcs.Select(c => c.CalculationString).ToList();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed getting top 10 from database.");
                Answers = new List<string>() { "Error retrieving calculations from database" };
            }
        }
        public async Task TryAddCalculationAsync(Calculation calculation)
        {
            try
            {
                await _dbClient.AddCalculationAsync(calculation);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Calculation couldn't be added to database.");
                throw ex;
            }
        }
        public async Task<string> TryRequestFunctionAsync(string a, string b, string op)
        {
            try
            {
                return await _functionClient.RequestAsync(a, b, op);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Could not get function response with", new object[] { a, b, op });
            }
            return null;
        }
    }
}
