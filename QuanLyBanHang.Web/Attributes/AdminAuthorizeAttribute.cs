using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuanLyBanHang.Web.Attributes
{
    // Attribute này giúp chỉ cho phép người có quyền Admin truy cập vào Controller / Action
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("EmployeeRole");

            // Nếu chưa đăng nhập hoặc không phải Admin → quay lại trang Login
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
