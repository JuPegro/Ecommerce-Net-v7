using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        private readonly ApplicationDbContext _db;

        public StoreRepository( ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Store store)
        {
            // GET CURRENT DATA
            var storeDb = _db.Stores.FirstOrDefault(x => x.Id == store.Id);

            // IF NOT NULL
            if (storeDb != null) 
            {
                storeDb.Name = store.Name;
                storeDb.Description = store.Description;
                storeDb.Status = store.Status;
                _db.SaveChanges();
            }
        }
    }
}
