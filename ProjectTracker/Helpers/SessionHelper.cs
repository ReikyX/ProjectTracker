namespace ProjectTracker.Helpers
{

    public static class SessionHelper
    {
        private const string UserIdKey = "UserId";
        //private const string UsernameKey = "Username";
        private const string IsAdminKey = "IsAdmin";

        // User einloggen
        public static void SetUserSession(ISession session, int userId, bool isAdmin)
        {
            session.SetInt32(UserIdKey, userId);
            session.SetString(IsAdminKey, isAdmin.ToString());
        }

        // User ausloggen
        public static void ClearUserSession(ISession session)
        {
            session.Clear();
        }

        // Ist User eingeloggt?
        public static bool IsLoggedIn(ISession session)
        {
            return session.GetInt32(UserIdKey).HasValue;
        }

        // User-ID abrufen
        public static int? GetUserId(ISession session)
        {
            return session.GetInt32(UserIdKey);
        }

        // Username abrufen
        public static string? GetUsername(ISession session)
        {
            return session.GetString(UserIdKey);
        }

        // Ist User Admin?
        public static bool IsAdmin(ISession session)
        {
            var isAdminValue = session.GetString(IsAdminKey);
            return bool.TryParse(isAdminValue, out bool isAdmin) && isAdmin;
        }
    }
}
