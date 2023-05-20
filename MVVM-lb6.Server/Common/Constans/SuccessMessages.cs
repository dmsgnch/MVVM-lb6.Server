namespace Server.Common.Constants;

public static class SuccessMessages
{
    public static class User
    {
        public const string Created = "New user has been successfully created";
    }

    public static class Authentication
    {
        public const string SuccessfulLogin = "Login is successful";
    }
    public static class Lobby
    {
        public const string Created = "Lobby has been successfully created";
        public const string Deleted = "Lobby has been successfully deleted";
        public const string Exited = "You have successfully exited the lobby";
        public const string Found = "Lobbies has been successfully found";
    }
    
    public static class Session
    {
        public const string Found = "Session has been successfully found";
    }
}