namespace Server.Common.Constants;

public static class ErrorMessages
{
    public static class User
    {
        public const string AlreadyExists = "User with this employee number already exists";
    }
    public static class Authentication
    {
        public const string NotRecordsFound = "Email or password is incorrect";
    }
    public static class Room
    {
        public const string NotFound = "There is no room with given id";
        public const string ThereIsNumber = "There is room with the same real number";
        public const string DatesOverlap = "The selected date is already booked";
    }
    public static class Session
    {
        public const string NotFound = "There is no session with given id";
    }

    public static class Planet
    {
        public const string NotFound = "There is no planet with given id";
    }
}