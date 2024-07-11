using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WireGuardApi.Domain.WGConfig.Enums;

namespace WireGuardApi.Domain.WGConfig;

public class Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public EConfigurationType ConfigurationType { get; set; }

    public string Comment { get; set; }

    public List<ConfigurationValues> ConfigurationValues { get; set; } = [];
}