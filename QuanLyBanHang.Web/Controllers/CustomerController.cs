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
        // chức năng thêm khách hàng
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // 🚀 Nếu là AJAX, trả lại partial form (chỉ form thôi)
                    return PartialView("Create", customer);
                }
                // Nếu dữ liệu chưa hợp lệ (trống, sai định dạng) → hiển thị lại form với lỗi
                return View(customer);
            }
            // ✅ Kiểm tra trùng email
            var existingEmail = _customerRepository.GetAll().FirstOrDefault(c => c.Email == customer.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email này đã được đăng ký");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Create", customer);
                }
                return View(customer);
            }
            // ✅ Kiểm tra trùng số điện thoại
            var existingPhone = _customerRepository.GetAll().FirstOrDefault(c => c.Phone == customer.Phone);
            if (existingPhone != null)
            {
                ModelState.AddModelError("Phone", "Số điện thoại này đã tồn tại");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Create", customer);
                }
                return View(customer);
            }
            // ✅ Nếu hợp lệ → thêm mới
            _customerRepository.Add(customer);
            return RedirectToAction("Index");
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
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu chưa hợp lệ (trống, sai định dạng)
                return View(customer);
            }

            // ✅ Kiểm tra trùng Email (ngoại trừ chính khách hàng đang sửa)
            var existingEmail = _customerRepository.GetAll()
                .FirstOrDefault(c => c.Email == customer.Email && c.CustomerId != customer.CustomerId);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng bởi khách hàng khác");
                return View(customer);
            }

            // ✅ Kiểm tra trùng Số điện thoại (ngoại trừ chính khách hàng đang sửa)
            var existingPhone = _customerRepository.GetAll()
                .FirstOrDefault(c => c.Phone == customer.Phone && c.CustomerId != customer.CustomerId);
            if (existingPhone != null)
            {
                ModelState.AddModelError("Phone", "Số điện thoại này đã tồn tại");
                return View(customer);
            }

            // ✅ Nếu hợp lệ → cập nhật dữ liệu
            _customerRepository.Update(customer);
            return RedirectToAction("Index");
        }

        // Xóa khách hàng
        public IActionResult Delete(int id)
        {
            _customerRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
