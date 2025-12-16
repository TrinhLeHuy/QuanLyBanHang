public static class Session
{
    public static int? CurrentUserId { get; set; }
    public static string? CurrentUserRole { get; set; }

    public static void Logout()
    {
        CurrentUserId = null;
        CurrentUserRole = null;
    }
}