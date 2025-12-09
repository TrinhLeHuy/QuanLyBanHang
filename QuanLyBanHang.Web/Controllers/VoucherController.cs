using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Data.Repositories;

namespace QuanLyBanHang.Web.Controllers
{
    public class VoucherController : Controller
    {
        private readonly VoucherRepository _repository;

        public VoucherController(VoucherRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string? search, bool? active)
        {
            var vouchers = _repository.GetAll();

            if (!string.IsNullOrEmpty(search))
                vouchers = _repository.Search(search);

            if (active.HasValue)
                vouchers = _repository.Filter(active.Value);

            return View(vouchers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(voucher);
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        public IActionResult Edit(int id)
        {
            var voucher = _repository.GetById(id);
            if (voucher == null) return NotFound();
            return View(voucher);
        }

        [HttpPost]
        public IActionResult Edit(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(voucher);
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
