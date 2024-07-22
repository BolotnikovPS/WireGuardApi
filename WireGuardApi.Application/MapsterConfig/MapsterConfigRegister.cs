using Mapster;
using TBotPlatform.Extension;
using WireGuardApi.Domain.WGConfig;
using WireGuardApi.Domain.WGConfig.Enums;

namespace WireGuardApi.Application.MapsterConfig;

internal class MapsterConfigRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
           .NewConfig<Root, WireGuardConfig>()
           .MapWith(src => MapRoot(src));

        config
           .NewConfig<WireGuardConfig, Root>()
           .MapWith(src => MapWireGuardConfig(src));
    }

    private static WireGuardConfig MapRoot(Root root)
    {
        var rootInterface = root.Configs.FirstOrDefault(z => z.ConfigurationType == EConfigurationType.Interface);

        var wgInterface = new WireGuardConfigInterface
        {
            Address = GetValue(EConfigurationKeyType.Address),
            ListenPort = GetValue(EConfigurationKeyType.ListenPort),
            PostDown = GetValue(EConfigurationKeyType.PostDown),
            PostUp = GetValue(EConfigurationKeyType.PostUp),
            PrivateKey = GetValue(EConfigurationKeyType.PrivateKey),
            SaveConfig = GetValue(EConfigurationKeyType.SaveConfig),
        };

        var peers = root
                   .Configs.Where(z => z.ConfigurationType == EConfigurationType.Peer)
                   .Select(
                        x => new WireGuardConfigPeer
                        {
                            Comment = x.Comment,
                            ConfigurationValues = x.ConfigurationValues,
                        })
                   .ToList();

        return new()
        {
            Interface = wgInterface,
            Peers = peers,
        };

        string GetValue(EConfigurationKeyType f)
        {
            if (rootInterface.IsNull())
            {
                return "";
            }

            var config = rootInterface!.ConfigurationValues.FirstOrDefault(z => z.Key == f);

            return config.IsNotNull()
                ? config!.Value
                : "";
        }
    }

    private static Root MapWireGuardConfig(WireGuardConfig wireGuardConfig)
    {
        var configurationValues = new List<ConfigurationValues>();

        if (wireGuardConfig.Interface.Address.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.Address,
                    Value = wireGuardConfig.Interface.Address,
                });
        }

        if (wireGuardConfig.Interface.ListenPort.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.ListenPort,
                    Value = wireGuardConfig.Interface.ListenPort,
                });
        }

        if (wireGuardConfig.Interface.PostDown.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.PostDown,
                    Value = wireGuardConfig.Interface.PostDown,
                });
        }

        if (wireGuardConfig.Interface.PostUp.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.PostUp,
                    Value = wireGuardConfig.Interface.PostUp,
                });
        }

        if (wireGuardConfig.Interface.PrivateKey.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.PrivateKey,
                    Value = wireGuardConfig.Interface.PrivateKey,
                });
        }

        if (wireGuardConfig.Interface.SaveConfig.CheckAny())
        {
            configurationValues.Add(
                new()
                {
                    Key = EConfigurationKeyType.SaveConfig,
                    Value = wireGuardConfig.Interface.SaveConfig,
                });
        }

        var interfaceConfig = new Configuration
        {
            ConfigurationType = EConfigurationType.Interface,
            Comment = "",
            ConfigurationValues = configurationValues,
        };

        var peersConfig = wireGuardConfig
                         .Peers.Select(
                              z => new Configuration
                              {
                                  ConfigurationType = EConfigurationType.Peer,
                                  Comment = z.Comment,
                                  ConfigurationValues = z.ConfigurationValues,
                              })
                         .ToList();

        var configs = new List<Configuration> { interfaceConfig, };
        configs.AddRange(peersConfig);

        return new()
        {
            Configs = configs,
        };
    }
}