using HybridLab.Data;

namespace HybridLab.Hybrid
{
    public class AppKeysAccess : IAppKeysAccess
    {
        public string KeysPath()
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, "Keys.json");
        }
    }
}
