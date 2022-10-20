using Microsoft.EntityFrameworkCore;

namespace Safe_Sign.DAL.DAO.Base
{
    public class BaseDAO<T> : IDAO<T> where T : class
    {
        public SafeSignContext _context { get; set; }

        public BaseDAO()
        {
            _context = new SafeSignContext();
        }

        public virtual T Create(T entity)
        {
            _context.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public virtual T GetById(ulong id)
        {
            T? entity = _context.Set<T>().Find(id);

            if (entity is null) throw new NullReferenceException();

            else return entity;
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T>? table = _context.Set<T>();

            return table;
        }

        public virtual T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            
            // Context.Set<T>().Update(entity);

            _context.SaveChanges();

            return entity; 
        }

        public virtual void Delete(ulong id)
        {
            T entity = GetById(id);

            if (entity is null) throw new NullReferenceException();

            _context.Remove(entity);

            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity is null) throw new ArgumentNullException();

            _context.Remove(entity);

            _context.SaveChanges();
        }
    }
}
