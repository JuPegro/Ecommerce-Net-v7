using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {

        private readonly IUnitWork _unitWork;

        public StoreController(IUnitWork unitWork)
        {
             _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(Guid? id)
        {
            Store store = new Store();

            if (id == null)
            {
                // CREATE NEW STORE
                store.Status = true;
                return View(store);
            }

            // UPDATE STORE

            store = await _unitWork.Store.Get(id.Value);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }
        #region API

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var storeDb = await _unitWork.Store.Get(id);

            if (storeDb == null)
            {
                return Json(new { success = false, message = "Store not found" });
            }

            _unitWork.Store.Remove(storeDb);
            await _unitWork.Save();
            return Json(new { success = true, message = "Successfully deleted store" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Store store)
        {
            if (ModelState.IsValid)
            {
                if (store.Id == Guid.Empty)
                {
                    await _unitWork.Store.Add(store);
                    TempData[DS.Successfully] = "Successfully created store";
                }
                else
                {
                    _unitWork.Store.Update(store);
                    TempData[DS.Successfully] = "Successfully updated store";
                }

                await _unitWork.Save();
                return RedirectToAction("Index");
            }
            TempData[DS.Error] = "Error into save store";
            return View(store);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _unitWork.Store.GetAll();
            return Json(new {data = all});
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, Guid? id)
        {
            bool value = false;

            var list = await _unitWork.Store.GetAll();

            if (id == Guid.Empty) 
            {
                value = list.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                value = list.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Id != id);

            }

            if (value) 
            {
                return Json(new {data = true});
            }

                return Json(new {data = false});

        }

        #endregion
    }
}
