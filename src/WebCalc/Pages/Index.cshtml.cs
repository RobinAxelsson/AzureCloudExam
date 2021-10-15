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
        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("Page was requested.");
            var calcs = await _dbClient.GetLast10Async();
            Answers = calcs.Count == 0 ? new List<string>() { "No Calculations Made" } : calcs.Select(c => c.CalculationString).ToList();
            return Page();
        }
        public List<string> Answers { get; set; }
        [BindProperty]
        public string a { get; set; }
        [BindProperty]
        public string b { get; set; }
        [BindProperty]
        public string op { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"POSTED:a={a} b={b} op={Operation.Parse(op)}");
            var response = await _functionClient.Request(a, b, Operation.Parse(op));
            if (response == null) return Page();

            string calcString = $"{a}+{b}={response}";

            var calculation = new Calculation()
            {
                Id = Guid.NewGuid().ToString("N"),
                CalculationString = calcString,
                Operation = Operation.Parse(op)
            };

            await _dbClient.TryAddCalculation(calculation);
            var calcs = await _dbClient.GetLast10Async();
            Answers = calcs.Select(c => c.CalculationString).ToList();
            return Page();
        }
    }
}
