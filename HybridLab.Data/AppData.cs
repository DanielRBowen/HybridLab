using LiteDB;

namespace HybridLab.Data
{
    public class AppData : IAppData
    {
        private readonly LiteDatabase _liteDatabase;

        public AppData(IDataBaseAccess dataBaseAccess)
        {
            _liteDatabase = new LiteDatabase(dataBaseAccess.DatabasePath());
        }

        public T Get<T>(int id)
        {
            var collection = _liteDatabase.GetCollection<T>();
            return collection.FindById(id);
        }

        public bool Add<T>(T item)
        {
            var collection = _liteDatabase.GetCollection<T>();
            return collection.Insert(item) != null;
        }

        public bool Update<T>(T item)
        {
            var collection = _liteDatabase.GetCollection<T>();
            return collection.Update(item);
        }

        public bool Delete<T>(T item)
        {
            var collection = _liteDatabase.GetCollection<T>();
            return collection.Delete(new BsonValue(item));
        }

        public T FirstOrDefault<T>()
        {
            var collection = _liteDatabase.GetCollection<T>();
            return collection.FindAll().FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var collection = _liteDatabase.GetCollection<T>();
            return new List<T>(collection.FindAll());
        }
    }
}
