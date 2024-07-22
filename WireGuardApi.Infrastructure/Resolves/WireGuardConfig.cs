using TBotPlatform.Extension;
using WireGuardApi.Application.Abstractions;
using WireGuardApi.Domain.WGConfig;
using WireGuardApi.Domain.WGConfig.Enums;

namespace WireGuardApi.Infrastructure.Resolves;

internal class WireGuardConfig : IWireGuardConfig
{
    public async Task<Root> ReadConfigAsync(string wgConfigPath, CancellationToken cancellationToken)
    {
        var fileLines = await File.ReadAllLinesAsync(wgConfigPath, cancellationToken);

        var configList = new List<Configuration>();

        var config = new Configuration();

        foreach (var line in fileLines)
        {
            if (line.Equals(Environment.NewLine)
                || line.Equals(string.Empty)
               )
            {
                configList.Add(config);
                config = new();

                continue;
            }

            if (line.StartsWith('[')
                && line.EndsWith(']')
               )
            {
                config.ConfigurationType = Enum.Parse<EConfigurationType>(
                    line
                       .Replace("[", string.Empty)
                       .Replace("]", string.Empty)
                    );

                continue;
            }

            if (line.StartsWith('#'))
            {
                if (config.Comment.IsNull())
                {
                    config.Comment = line
                                    .Replace("# ", string.Empty)
                                    .Replace("#", string.Empty);
                }

                continue;
            }

            var lineSplit = line.Split(" = ");

            var newValue = new ConfigurationValues
            {
                Key = Enum.Parse<EConfigurationKeyType>(lineSplit[0]),
                Value = lineSplit[1],
            };

            config.ConfigurationValues.Add(newValue);
        }

        if (config.ConfigurationValues.Count > 0)
        {
            configList.Add(config);
        }

        return new()
        {
            Configs = configList,
        };
    }

    public Task SaveConfigAsync(string wgConfigPath, Root root, CancellationToken cancellationToken)
    {
        var strings = new List<string>();

        foreach (var config in root.Configs)
        {
            strings.Add($"[{config.ConfigurationType}]");
            strings.Add($"# {config.Comment}");

            strings.AddRange(config.ConfigurationValues.Select(value => $"{value.Key} = {value.Value}"));

            strings.Add(string.Empty);
        }

        return File.WriteAllLinesAsync(wgConfigPath, strings, cancellationToken);
    }
}