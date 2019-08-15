using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace CoinPayments.NetCore.Dtos
{
    public static class Serialize
    {
        public static string ToJson(this CoinPaymentsTransactionResponseDto self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public partial class CoinPaymentsTransactionResponseDto
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        public string ErrorMessage { get; set; }

        [JsonProperty("result", Required = Required.AllowNull)]
        public Result Result { get; set; }
    }

    public partial class CoinPaymentsTransactionResponseDto
    {
        public static CoinPaymentsTransactionResponseDto FromJson(string json) => JsonConvert.DeserializeObject<CoinPaymentsTransactionResponseDto>(json, Converter.Settings);
    }

    public partial class Result
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("checkout_url")]
        public Uri CheckoutUrl { get; set; }

        [JsonProperty("confirms_needed")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ConfirmsNeeded { get; set; }

        [JsonProperty("dest_tag")]
        public string DestTag { get; set; }

        [JsonProperty("qrcode_url")]
        public Uri QrcodeUrl { get; set; }

        [JsonProperty("status_url")]
        public Uri StatusUrl { get; set; }

        [JsonProperty("timeout")]
        public long Timeout { get; set; }

        [JsonProperty("txn_id")]
        public string TxnId { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public static readonly ParseStringConverter Singleton = new ParseStringConverter();

        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }
    }
}