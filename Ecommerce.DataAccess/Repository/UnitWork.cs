using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _db;

        public IStoreRepository Store {  get; private set; }

        public UnitWork(ApplicationDbContext db)
        {
            _db = db;
            Store = new StoreRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose(); // RELEASE RESOURCES THAT ARE NOT USED
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync(); // SAVE ALL CHANGES
        }
    }
}
