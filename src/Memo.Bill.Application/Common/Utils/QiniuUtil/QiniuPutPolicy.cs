using System.Text.Json;
using System.Text.Json.Serialization;

namespace Memo.Bill.Application.Common.Utils.QiniuUtil.QiniuUtil
{
    public class QiniuPutPolicy
    {
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("isPrefixalScope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? isPrefixalScope { get; set; }

        [JsonPropertyName("deadline")]
        public long Deadline { get; private set; }

        [JsonPropertyName("insertOnly")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? InsertOnly { get; set; }

        [JsonPropertyName("forceSaveKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ForceSaveKey { get; set; }

        [JsonPropertyName("saveKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SaveKey { get; set; }

        [JsonPropertyName("endUser")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EndUser { get; set; }

        [JsonPropertyName("returnUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ReturnUrl { get; set; }

        [JsonPropertyName("returnBody")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ReturnBody { get; set; }

        [JsonPropertyName("callbackUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CallbackUrl { get; set; }

        [JsonPropertyName("callbackBody")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CallbackBody { get; set; }

        [JsonPropertyName("callbackBodyType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CallbackBodyType { get; set; }

        [JsonPropertyName("callbackHost")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CallbackHost { get; set; }

        [JsonPropertyName("callbackFetchKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CallbackFetchKey { get; set; }

        [JsonPropertyName("persistentOps")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PersistentOps { get; set; }

        [JsonPropertyName("persistentNotifyUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PersistentNotifyUrl { get; set; }

        [JsonPropertyName("persistentPipeline")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PersistentPipeline { get; set; }

        [JsonPropertyName("fsizeMin")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? FsizeMin { get; set; }

        [JsonPropertyName("fsizeLimit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? FsizeLimit { get; set; }

        [JsonPropertyName("detectMime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DetectMime { get; set; }

        [JsonPropertyName("mimeLimit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MimeLimit { get; set; }

        [JsonPropertyName("deleteAfterDays")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DeleteAfterDays { get; set; }

        [JsonPropertyName("fileType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FileType { get; set; }

        public void SetExpires(int expireInSeconds)
        {
            Deadline = GetUnixTimestamp(expireInSeconds);
        }

        public void SetExpires(long expireInSeconds)
        {
            Deadline = GetUnixTimestamp(expireInSeconds);
        }

        public string ToJsonString()
        {
            if (Deadline == 0L)
            {
                SetExpires(3600);
            }
            return JsonSerializer.Serialize(this);
        }

        private long GetUnixTimestamp(long secondsAfterNow)
        {
            var dtBase = new DateTime(1970, 1, 1).ToLocalTime();
            return DateTime.Now.AddSeconds(secondsAfterNow).ToLocalTime().Subtract(dtBase)
                .Ticks / 10000000;
        }
    }
}
