using CoinPayments.NetCore.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoinPayments.NetCore
{
    public class CoinPaymentsClient
    {
        private readonly string apiKey;
        private readonly string secretKey;

        public CoinPaymentsClient(string apiKey, string secretKey)
        {
            this.apiKey = apiKey ?? throw new System.ArgumentNullException(nameof(apiKey));
            this.secretKey = secretKey;
        }

        public CoinPaymentsTransactionResponseDto MakeOrder(string amount, string currency1, string currency2, string buyer_email, string item_name,
            string invoice, string ipn_url, string success_url, string cancel_url)
        {
            var parameters = $"cancel_url={cancel_url}&success_url={success_url}&cmd=create_transaction&amount={amount}&currency1={currency1}&currency2={currency2}&buyer_email={buyer_email}&item_name={item_name}&invoice={invoice}&ipn_url={ipn_url}&key={apiKey}&version=1&format=json";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.WebClient cl = new System.Net.WebClient();
            cl.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            cl.Headers.Add("HMAC", HmacFactory.GetHMAC(secretKey, parameters));
            cl.Encoding = Encoding.UTF8;

            var ret = new Dictionary<string, dynamic>();
            try
            {
                string resp = cl.UploadString("https://www.coinpayments.net/api.php", parameters);
                return JsonConvert.DeserializeObject<CoinPaymentsTransactionResponseDto>(resp);
            }
            catch (System.Net.WebException e)
            {
                return new CoinPaymentsTransactionResponseDto() { Error = "true", ErrorMessage = e.Message };
            }
            catch (Exception e)
            {
                return new CoinPaymentsTransactionResponseDto() { Error = "true", ErrorMessage = e.Message };
            }
        }
    }
}