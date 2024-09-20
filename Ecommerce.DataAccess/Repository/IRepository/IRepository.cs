using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(Guid id);

        Task<IEnumerable<T>> GetAll(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string propertiesInclude = null,
                bool isTracking = true
                );

        Task<T> GetFirstOrDefault(
                Expression<Func<T, bool>> filter = null,
                string propertiesInclude = null,
                bool isTracking = true
            );

        Task Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
