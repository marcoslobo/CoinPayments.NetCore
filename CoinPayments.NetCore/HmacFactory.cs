using System;
using System.Text;

namespace CoinPayments.NetCore
{
    public static class HmacFactory
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static string GetHMAC(string secretKey, string parameters)
        {
            byte[] keyBytes = encoding.GetBytes(secretKey);
            byte[] postBytes = encoding.GetBytes(parameters);

            var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes);
            return BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);
        }
    }
}