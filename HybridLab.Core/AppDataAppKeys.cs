using HybridLab.Data;

namespace HybridLab.Core
{
    public class AppDataAppKeys : IAppKeys
    {
        private readonly DeviceAppKeys _deviceAppKeys;

        public AppDataAppKeys(IAppKeysAccess appKeysAccess)
        {
            _deviceAppKeys = new DeviceAppKeys(appKeysAccess.KeysPath());
        }

        public async Task<bool> Contains(string key)
        {
            await Task.Delay(0);
            return _deviceAppKeys.Contains(key);
        }

        public async Task<string> GetAsync(string key)
        {
            await Task.Delay(0);
            return _deviceAppKeys.Get(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await _deviceAppKeys.Remove(key);
            return true;
        }

        public async Task RemoveAllAsync()
        {
            await _deviceAppKeys.RemoveAll();
        }

        public async Task SetAsync(string key, string value)
        {
            await _deviceAppKeys.Set(key, value);
        }
    }
}
