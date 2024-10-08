﻿using System.Net;
using TBotPlatform.Extension;
using WireGuardApi.Application.Abstractions;
using WireGuardApi.Application.Abstractions.Helpers;
using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.Helpers;
using WireGuardApi.Domain.Config;
using WireGuardApi.Domain.Contracts;
using WireGuardApi.Domain.Enums;
using WireGuardApi.Domain.Exceptions;
using WireGuardApi.Domain.WGConfig;
using WireGuardApi.Domain.WGConfig.Enums;

namespace WireGuardApi.Application;

internal class WireGuardСontrol(
    IWireGuardConfig wireGuardConfig,
    IWireGuardClient wireGuardClient,
    IConductorHelper conductorHelper,
    IConfigService configService,
    ITerminalCommandGeneratorHelper terminalCommandGeneratorHelper,
    ITerminalCommand terminalCommand,
    IMap map
    ) : IWireGuardСontrol
{
    private static readonly ValidationConfigException ConfigException = new("Дубликаты данных входящей модели конфигурации");

    public async Task<WireGuardConfig> GetConfigAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var settings = configService.GetValueOrNull<WireGuardSettings>(EConfigKey.WireGuard);

        var path = CheckAndGetPathFile(settings.WGConfigPath, NameToConfig(wgInterfaceName));

        var root = await wireGuardConfig.ReadConfigAsync(path, cancellationToken);

        return map.Map<WireGuardConfig>(root);
    }

    public async Task ReNewConfigAsync(string wgInterfaceName, WireGuardConfig config, CancellationToken cancellationToken)
    {
        var values = config
                    .Peers
                    .SelectMany(z => z.ConfigurationValues)
                    .Select(x => x.Value)
                    .ToList();

        if (values.Count != values.Distinct().Count()
            || values.Any(z => z == config.Interface.Address)
           )
        {
            throw ConfigException;
        }

        var settings = configService.GetValueOrNull<WireGuardSettings>(EConfigKey.WireGuard);

        var path = CheckAndGetPathFile(settings.WGConfigPath, NameToConfig(wgInterfaceName));

        var backupWgConf = conductorHelper.GetFullPath(settings.WGConfigPath, $"{NameToConfig(wgInterfaceName)}.{DateTime.UtcNow:yyyyMMddHHmmss}");

        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.CopyConfig, path, conductorHelper.GetFullPath(backupWgConf));
        await terminalCommand.ExecuteAsync(command, cancellationToken);

        var root = map.Map<Root>(config);

        await wireGuardConfig.SaveConfigAsync(
            path,
            root,
            cancellationToken
            );

        command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.Down, wgInterfaceName);
        await terminalCommand.ExecuteAsync(command, cancellationToken);

        command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.Up, wgInterfaceName);
        await terminalCommand.ExecuteAsync(command, cancellationToken);
    }

    public async Task<List<AddAutoPeerResponse>> AddAutoPeerAsync(string wgInterfaceName, string comment, CancellationToken cancellationToken)
    {
        var clientPrivateIp = await GenerateClientNewIpAsync(wgInterfaceName, cancellationToken);

        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.GenKey);
        var clientPrivateKey = await terminalCommand.ExecuteAsync(command, cancellationToken);

        clientPrivateKey = clientPrivateKey.RemoveNewLine();

        command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.GeneratePublicKey, clientPrivateKey);
        var clientPublicKey = await terminalCommand.ExecuteAsync(command, cancellationToken);

        clientPublicKey = clientPublicKey.RemoveNewLine();

        command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.GetServerPublicKey, wgInterfaceName);
        var serverPublicKey = await terminalCommand.ExecuteAsync(command, cancellationToken);

        serverPublicKey = serverPublicKey.RemoveText("public key:").RemoveNewLine();

        comment = comment.CheckAny()
            ? comment
            : $"Add Auto Peer {clientPrivateIp}";

        await AddPeerAsync(
            wgInterfaceName,
            clientPublicKey,
            clientPrivateIp,
            comment,
            cancellationToken
            );

        var newClientGuid = Guid.NewGuid();
        var fileName = NameToConfig(comment);
        var qrName = $"{newClientGuid}.png";

        var wireGuardSettings = configService.GetValueOrNull<WireGuardSettings>(EConfigKey.WireGuard);

        var wgInterfacePath = conductorHelper.GetFullPath(wireGuardSettings.WGClientsPath, wgInterfaceName);

        if (!conductorHelper.FolderExist(wgInterfacePath))
        {
            Directory.CreateDirectory(wgInterfacePath);
        }

        var filePath = conductorHelper.GetFullPath(wgInterfacePath, fileName);
        var qrPath = conductorHelper.GetFullPath(wgInterfacePath, qrName);

        await wireGuardClient.CreateClientAsync(
            clientPrivateKey,
            clientPrivateIp,
            serverPublicKey,
            filePath,
            cancellationToken
            );

        command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.GenQRCode, qrPath, filePath);
        await terminalCommand.ExecuteAsync(command, cancellationToken);

        var qrBytes = await File.ReadAllBytesAsync(qrPath, cancellationToken);
        var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

        return
        [
            new(qrBytes, qrName),
            new(fileBytes, fileName),
        ];
    }

    public async Task AddPeerAsync(string wgInterfaceName, string clientPeer, string clientPrivateIp, string comment, CancellationToken cancellationToken)
    {
        var config = await GetConfigAsync(wgInterfaceName, cancellationToken);

        var existPublicKey = config.Peers.FirstOrDefault(x => x.ConfigurationValues.Any(z => z.Key == EConfigurationKeyType.PublicKey && z.Value == clientPeer));
        var checkAllowedIPs = config.Peers.FirstOrDefault(x => x.ConfigurationValues.Any(z => z.Key == EConfigurationKeyType.AllowedIPs && z.Value == clientPrivateIp));

        if (checkAllowedIPs.IsNotNull()
            && existPublicKey.IsNotNull()
            && !checkAllowedIPs!.Equals(existPublicKey)
           )
        {
            throw ConfigException;
        }

        var checkPeer = config.Peers.Any(
            z => z.ConfigurationValues.Any(x => x.Key == EConfigurationKeyType.PublicKey && x.Value == clientPeer)
                 && z.ConfigurationValues.Any(x => x.Key == EConfigurationKeyType.AllowedIPs && x.Value == clientPrivateIp)
            );

        if (!checkPeer)
        {
            var newClient = new WireGuardConfigPeer
            {
                Comment =
                    comment.CheckAny()
                        ? comment
                        : $"Add Peer {clientPrivateIp}",
                ConfigurationValues =
                [
                    new()
                    {
                        Key = EConfigurationKeyType.PublicKey,
                        Value = clientPeer,
                    },
                    new()
                    {
                        Key = EConfigurationKeyType.AllowedIPs,
                        Value = clientPrivateIp,
                    },
                ],
            };

            config.Peers.Add(newClient);

            await ReNewConfigAsync(wgInterfaceName, config, cancellationToken);
        }

        await DownAsync(wgInterfaceName, cancellationToken);

        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.AddPeer, wgInterfaceName, clientPeer, clientPrivateIp);
        await terminalCommand.ExecuteAsync(command, cancellationToken);

        await UpAsync(wgInterfaceName, cancellationToken);
    }

    public async Task RemovePeerAsync(string wgInterfaceName, string clientPeer, CancellationToken cancellationToken)
    {
        var config = await GetConfigAsync(wgInterfaceName, cancellationToken);

        var peer = config.Peers.FirstOrDefault(z => z.ConfigurationValues.Any(x => x.Value == clientPeer && x.Key == EConfigurationKeyType.PublicKey));

        if (peer.IsNull())
        {
            return;
        }

        config.Peers.Remove(peer);

        await ReNewConfigAsync(wgInterfaceName, config, cancellationToken);

        await DownAsync(wgInterfaceName, cancellationToken);

        await UpAsync(wgInterfaceName, cancellationToken);
    }

    public Task<string> GetStatisticsAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.Statistics, wgInterfaceName);
        return terminalCommand.ExecuteAsync(command, cancellationToken);
    }

    public async Task UpAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.Up, wgInterfaceName);
        await terminalCommand.ExecuteAsync(command, cancellationToken);
    }

    public async Task DownAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = terminalCommandGeneratorHelper.CreateCommand(ETerminalCommandType.Down, wgInterfaceName);
        await terminalCommand.ExecuteAsync(command, cancellationToken);
    }

    private string CheckAndGetPathFile(string folderPath, string fileName = "")
    {
        if (!conductorHelper.FileExist(folderPath, fileName))
        {
            throw new FileNotFoundException($"Не смогли найти файл или папку по пути {folderPath}", fileName);
        }

        return conductorHelper.GetFullPath(folderPath, fileName);
    }

    internal async Task<string> GenerateClientNewIpAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var root = await GetConfigAsync(wgInterfaceName, cancellationToken);
        var peers = root.Peers;

        if (peers.IsNull())
        {
            var wgInterface = root.Interface;

            if (wgInterface.IsNull())
            {
                throw new NullReferenceException();
            }

            var allowedIpSplit = wgInterface.Address.Split("/");

            if (allowedIpSplit.IsNull())
            {
                throw new ArgumentNullException();
            }

            var nextIpAddress = Increment(IPAddress.Parse(allowedIpSplit[0]));

            return $"{nextIpAddress}/{allowedIpSplit[1]}";
        }

        var maxAllowedIp = peers
                          .SelectMany(
                               x => x.ConfigurationValues
                                     .Where(z => z.Key == EConfigurationKeyType.AllowedIPs)
                               )
                          .Select(y => y.Value)
                          .MaxBy(a => a);

        if (maxAllowedIp.IsNull())
        {
            throw new NullReferenceException();
        }

        var maxAllowedIPsSplit = maxAllowedIp.Split("/");

        if (maxAllowedIPsSplit.IsNull())
        {
            throw new ArgumentNullException();
        }

        var newIpAddress = Increment(IPAddress.Parse(maxAllowedIPsSplit[0]));

        return $"{newIpAddress}/{maxAllowedIPsSplit[1]}";
    }

    private static IPAddress Increment(IPAddress value)
    {
        var ip = BitConverter.ToInt32(value.GetAddressBytes().Reverse().ToArray(), startIndex: 0);
        ip++;
        return new(BitConverter.GetBytes(ip).Reverse().ToArray());
    }

    private static string NameToConfig(string wgInterfaceName)
        => $"{wgInterfaceName}.conf";
}