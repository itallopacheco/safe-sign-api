namespace Safe_Sign.DAL.DAO.Base
{
    public interface IDAO<T> where T : class
    {
        T GetById(ulong id);

        IEnumerable<T> GetAll();

        T Create(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Delete(ulong id);
    }
}
