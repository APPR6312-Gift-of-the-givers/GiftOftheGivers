using System.Net.Http.Json;

namespace GiftOfTheGivers.Web.Services
{
    public class TaxCertificateService
    {
        private readonly HttpClient _http;
        public TaxCertificateService(HttpClient http) => _http = http;

        public async Task<string> GenerateAsync(string donorName, decimal amount)

        {
            var payload = new { donorName, amount, currency = "ZAR" };
            var res = await _http.PostAsJsonAsync("http://localhost:7271/api/GenerateDonationCertificate", payload);
            
            res.EnsureSuccessStatusCode();
            var result = await res.Content.ReadFromJsonAsync<CertificateResponse>();
            return result.CertificateNumber;

        }

        private class CertificateResponse
        {
            public bool Success { get; set; }
            public string CertificateNumber { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
        }

    }
}
