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
    public static class Room
    {
        public const string Extracted = "Rooms list has been successfully extracted";
        public const string Created = "Room has been successfully created";
        public const string Deleted = "Room has been successfully deleted";
        public const string Updated = "Room has been successfully updated";
    }
    
    public static class Session
    {
        public const string Found = "Session has been successfully found";
    }
}