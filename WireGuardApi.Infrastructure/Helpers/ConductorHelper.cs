using WireGuardApi.Application.Abstractions.Helpers;

namespace WireGuardApi.Infrastructure.Helpers;

internal class ConductorHelper : IConductorHelper
{
    public string GetFullPath(string folderPath, string fileName = "")
    {
        return Path.Combine(folderPath, fileName);
    }

    public bool FileExist(string folderPath, string fileName)
    {
        var path = GetFullPath(folderPath, fileName);

        return File.Exists(path);
    }

    public bool FolderExist(string folderPath)
    {
        var path = GetFullPath(folderPath);

        return Directory.Exists(path);
    }
}