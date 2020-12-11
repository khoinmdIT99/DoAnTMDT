using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Web.PaypalHelpers
{
    public class PaypalAPI
    {
        private readonly IConfiguration configuration;

        public PaypalAPI(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> GetRedirectUrlToPaypal(double total, string currency)
        {
            try
            {
                HttpClient http = GetPaypalHttpClient();

                PaypalAccessToken accessToken = await GetPaypalAccessTokenAsync(http);

                PaypalPaymentCreateResponce paypalPaymentCreateResponce = await CreatePaypalPaymentAsync(http, accessToken, total, currency);

                return paypalPaymentCreateResponce.links.First(x => x.rel == "approval_url").href;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PalPal");
                return null;
            }
        }

        public async Task<PayPalPaymentExecutedResponse> ExecutePayment(string paymentId, string payerId)
        {
            try
            {
                HttpClient http = GetPaypalHttpClient();
                PaypalAccessToken accessToken = await GetPaypalAccessTokenAsync(http);
                return await ExecutePaypalPaymentAsync(http, accessToken, paymentId, payerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PalPal");
                return null;
            }
        }
        private HttpClient GetPaypalHttpClient()
        {
            string sandbox = configuration["Paypal:urlAPI"];
            var http = new HttpClient
            {
                BaseAddress = new Uri(sandbox),
                Timeout = TimeSpan.FromSeconds(100)
            };
            return http;
        }

        public async Task<PaypalAccessToken> GetPaypalAccessTokenAsync(HttpClient http)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{configuration["Paypal:ClientID"]}:{configuration["Paypal:Secret"]}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PaypalAccessToken accessToken = JsonConvert.DeserializeObject<PaypalAccessToken>(content);
            return accessToken;
        }

        public async Task<PaypalPaymentCreateResponce> CreatePaypalPaymentAsync(HttpClient httpclient, PaypalAccessToken accessToken, double total, string currency)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/payments/payment/");//--/v1/checkout/orders/ /v2/checkout/orders/


            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = configuration["Paypal:returnURL"],
                    cancel_url = configuration["Paypal:cancelURL"]
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = total,
                            currency = currency
                        }
                    }
                })
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpclient.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PaypalPaymentCreateResponce payPalPaymentExecutedResponse = JsonConvert.DeserializeObject<PaypalPaymentCreateResponce>(content);
            return payPalPaymentExecutedResponse;
        }

        public async Task<PayPalPaymentExecutedResponse> ExecutePaypalPaymentAsync(HttpClient httpClient, PaypalAccessToken paypalAccessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", paypalAccessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalPaymentExecutedResponse payPalPaymentExecutedResponse = JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);
            return payPalPaymentExecutedResponse;
        }

    }
}
