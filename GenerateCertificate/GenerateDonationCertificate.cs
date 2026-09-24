using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GenerateCertificate;

public class GenerateDonationCertificate
{
    private readonly ILogger<GenerateDonationCertificate> _logger;

    public GenerateDonationCertificate(ILogger<GenerateDonationCertificate> logger)
    {
        _logger = logger;
    }

    [Function("GenerateDonationCertificate")]
    public async Task<IActionResult> Run(
     [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("Generating donation certificate.");

        string donorName;
        decimal donationAmount;

        if (req.Method == "POST")
        {
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<DonationRequest>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            donorName = data.DonorName ?? "Unknown Donor";
            donationAmount = data?.DonationAmount ?? 0;
        }
        else
        {
            // Allow GET testing
            donorName = req.Query["donorName"];
            decimal.TryParse(req.Query["Amount"], out donationAmount);
        }

        string certificateNumber = $"CERT-2026-{new Random().Next(1, 99999):D1}";

        var response = new
        {
            success = true,
            certificateNumber,
            donorName,
            donationAmount,
            message = " Tax certificate generated successfully."
        };

        return new OkObjectResult(response);
    }
}

public class DonationRequest
{
    public string DonorName { get; set; }
    public decimal DonationAmount { get; set; }
}
    
