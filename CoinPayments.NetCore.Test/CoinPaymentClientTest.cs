using System;
using Xunit;

namespace CoinPayments.NetCore.Test
{
    public class CoinPaymentClientTest
    {
        [Fact]
        public void Test_Hash()
        {
            var HMAC = HmacFactory.GetHMAC("test", "currency=BTC&version=1&cmd=get_callback_address&key=your_api_public_key&format=json");
            Assert.Equal("5590EAC015E7692902E1A9CD5464F1D305A4B593D2F1343D826AC5AFFC5AC6F960A5167284F9BF31295CBA0E04DF9D8F7087935B5344C468CCF2DD036E159102", HMAC);
        }

        [Fact]
        public void Test_Payment()
        {
            var coinPaymentsClient = new CoinPaymentsClient(Environment.GetEnvironmentVariable("coinpayments__apikey"), Environment.GetEnvironmentVariable("coinpayments__secretkey"));
            var response = coinPaymentsClient.MakeOrder("1", "BTC", "BTC", "zedobar@msn.com", "TOKEN PROMO I", "5841", "http://google.com.br", "http://google.com.br", "http://google.com.br");
            Assert.True(response.Error == "ok", response.ErrorMessage);
        }
    }
}