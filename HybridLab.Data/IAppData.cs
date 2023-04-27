namespace HybridLab.Data
{
    public interface IAppData
    {
        bool Add<T>(T item);
        bool Delete<T>(T item);
        T Get<T>(int id);
        IEnumerable<T> GetAll<T>();
        T FirstOrDefault<T>();
        bool Update<T>(T item);
    }
}