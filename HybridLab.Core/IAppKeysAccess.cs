namespace HybridLab.Data
{
    public interface IAppKeysAccess
    {
        /*
         * IOS
        public string DatabasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, UnsyncedDatabase.NAME);

        }
         */

        /*
         * Android
        public string DatabasePath()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), UnsyncedDatabase.NAME);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            return path;
        }
         */

        /*
         * Windows
        public string DatabasePath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, UnsyncedDatabase.NAME);
        }
         */

        // For maui check: https://youtu.be/m03IjT1b2Hs?t=341
        string KeysPath();
    }
}
