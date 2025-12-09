using Microsoft.AspNetCore.Mvc;

namespace QuanLyBanHang.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
