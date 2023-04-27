namespace HybridLab.Data
{
    public class DataBaseAccess : IDataBaseAccess
    {
        public string DatabasePath()
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, "HybridLabData.db");
        }
    }
}
