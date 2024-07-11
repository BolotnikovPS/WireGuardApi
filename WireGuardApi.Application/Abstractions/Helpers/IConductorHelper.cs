namespace WireGuardApi.Application.Abstractions.Helpers;

public interface IConductorHelper
{
    public string GetFullPath(string folderPath, string fileName = "");

    public bool FileExist(string folderPath, string fileName);

    public bool FolderExist(string folderPath);
}