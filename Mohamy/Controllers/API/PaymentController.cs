using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : BaseController
    {
        private readonly HttpClient _httpClient;

        public PaymentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] double amount)
        {
            var apiKey = "sk_test_ZZvkpUn8AcK3pzeA3hmqVHz4KWxyDiFbtzwFKwjN";
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:"));

            var paymentData = new
            {
                amount = amount * 100,  // Amount in smallest currency unit
                currency = "SAR",
                description = "Lawyer Payment",
                callback_url = "https://your-backend.com/api/moyasar/payment-callback",
                source = new { type = "creditcard" }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(paymentData), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");

            var response = await _httpClient.PostAsync("https://api.moyasar.com/v1/invoices", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return Ok(JsonSerializer.Deserialize<object>(responseString)); // Return the response
        }
    }

}