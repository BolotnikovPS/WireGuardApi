using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using WireGuardApi.Domain.WGConfig.Enums;

namespace WireGuardApi.Domain.WGConfig;

public class ConfigurationValues
{
    [JsonConverter(typeof(StringEnumConverter))]
    public EConfigurationKeyType? Key { get; set; }

    public string Value { get; set; }
}