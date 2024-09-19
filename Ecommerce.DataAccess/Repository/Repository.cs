using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // REQUIRED DB CONTEXT
        private readonly ApplicationDbContext _db;

        // ENTITY 
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity); // INSERT INTO 
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id); // SELECT * FROM [ID]
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string propertiesInclude = null, 
            bool isTracking = true)
        {
            IQueryable<T> query = dbSet; 

            // SELECT * FROM [T] WHERE [FILTER]
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // ORDERBY IN FIND
            if(orderBy != null)
            {
                query = orderBy(query);
            }

            if (propertiesInclude != null) 
            {
                // SEPARATE THE STRING FOR EACH ONE [,]
                foreach (var property in propertiesInclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                { 
                    query = query.Include(property); // EXAMPLE: [TYPE], [CATEGORY];
                }            
            }

            if (!isTracking) 
            { 
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string propertiesInclude = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            // SELECT * FROM [T] WHERE [FILTER]
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (propertiesInclude != null)
            {
                // SEPARATE THE STRING FOR EACH ONE [,]
                foreach (var property in propertiesInclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property); // EXAMPLE: [TYPE], [CATEGORY];
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity); // DELETE FROM
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity); // DELETE COLLECTION
        }
    }
}
