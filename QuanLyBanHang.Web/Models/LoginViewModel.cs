using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; }
    }
}
