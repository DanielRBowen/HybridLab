using Newtonsoft.Json;

namespace HybridLab.Core
{
    public class DeviceAppKeys
    {
        private readonly string _filePath;
        private Dictionary<string, string> _dictionary;
        private object _lock = new object();

        /// <summary>
        /// Created with Bing Chat
        /// </summary>
        /// <param name="filePath"></param>
        public DeviceAppKeys(string filePath)
        {
            _filePath = filePath;

            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    _dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (_dictionary == null)
                    {
                        _dictionary = new Dictionary<string, string>();
                    }
                }
                else
                {
                    _dictionary = new Dictionary<string, string>();
                }
            }
            catch (Exception)
            {
                _dictionary = new Dictionary<string, string>();
            }
        }

        public async Task Set(string key, string value)
        {
            _dictionary[key] = value;
            await SaveAsync();
        }

        public bool Contains(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public string Get(string key)
        {
            _dictionary.TryGetValue(key, out string value);
            return value;
        }

        public async Task Remove(string key)
        {
            _dictionary.Remove(key);
            await SaveAsync();
        }

        public async Task RemoveAll()
        {
            _dictionary.Clear();
            await SaveAsync();
        }

        private async Task SaveAsync()
        {
            string json = JsonConvert.SerializeObject(_dictionary);

            if (File.Exists(_filePath) == false) 
            {
                lock (_lock)
                {
                    File.WriteAllText(_filePath, json);
                    return;
                }
            }

            while (IsFileLocked(new FileInfo(_filePath)))
            {
                await Task.Delay(200);
            }

            lock (_lock)
            {
                File.WriteAllText(_filePath, json);
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
