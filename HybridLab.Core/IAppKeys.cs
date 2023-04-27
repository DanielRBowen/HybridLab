namespace HybridLab.Core
{
    // It is just Microsoft.Maui.Storage.ISecureStorage
    public interface IAppKeys
    {
        Task<string> GetAsync(string key);
        Task<bool> RemoveAsync(string key);
        Task RemoveAllAsync();
        Task SetAsync(string key, string value);
        Task<bool> Contains(string key);
    }
}
