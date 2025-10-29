using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Data.Repositories;

namespace QuanLyBanHang.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Danh sách khách hàng
        public IActionResult Index(string keyword = "")
        {
            var customers = _customerRepository.GetAll(keyword);
            ViewBag.Keyword = keyword;
            return View(customers);
        }

        // Tạo khách hàng mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Add(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // Chỉnh sửa khách hàng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.Update(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // Xóa khách hàng
        public IActionResult Delete(int id)
        {
            _customerRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
