public class UserSessionService
{
    public string? EmployeeName { get; set; }
    public string? EmployeeRole { get; set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(EmployeeRole);

    public void Logout()
    {
        EmployeeName = null;
        EmployeeRole = null;
    }
}
