using HybridLab.Core;

namespace HybridLab.Hybrid
{
    public class SecureStorageAppKeys : IAppKeys
    {
        public async Task<bool> Contains(string key)
        {
            return await SecureStorage.Default.GetAsync(key) != null;
        }

        public async Task<string> GetAsync(string key)
        {
            return await SecureStorage.Default.GetAsync(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await Task.Delay(0);
            return SecureStorage.Default.Remove(key);
        }

        public async Task RemoveAllAsync()
        {
            await Task.Delay(0);
            SecureStorage.Default.RemoveAll();
        }

        public async Task SetAsync(string key, string value)
        {
            await SecureStorage.Default.SetAsync(key, value);
        }
    }
}
