using Newtonsoft.Json;

namespace NavinoShop.WebApplication.Utility
{
    public class ZarinPalRequestModel
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string callback_url { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
    public class ZarinPalDataResponseModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string authority { get; set; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }
    public class ZarinPalResponseModel
    {
        public ZarinPalDataResponseModel data { get; set; }
        public string[] errors { get; set; }
    }
    public class ZarinPalVerificationResponse
    {
        [JsonProperty("data")]
        public ZarinPalVerificationData Data { get; set; }

        [JsonProperty("errors")]
        public List<ZarinPalErrors> Errors { get; set; } 

        // خاصیت کمکی برای دسترسی آسان به Status
        [JsonIgnore]
        public int Status => Data?.Code ?? -1;

        [JsonIgnore]
        public long RefId => Data?.RefId ?? 0;
    }

    public class ZarinPalVerificationData
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("card_hash")]
        public string CardHash { get; set; }

        [JsonProperty("card_pan")]
        public string CardPan { get; set; }

        [JsonProperty("ref_id")]
        public long RefId { get; set; }

        [JsonProperty("fee_type")]
        public string FeeType { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }
    }

    public class ZarinPalErrors
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("validations")]
        public Dictionary<string, List<string>> Validations { get; set; }
    }



}
