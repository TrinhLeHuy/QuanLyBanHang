namespace QuanLyBanHang.BlazorServer.Services
{
    public class UserSessionService
    {
        public string? EmployeeName { get; set; }
        public string? EmployeeRole { get; set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(EmployeeName);
    }
}
