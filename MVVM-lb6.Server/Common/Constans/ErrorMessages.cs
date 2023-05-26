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
        public const string ThereIsUsers = "There is users in lobby";
        public const string UserAlreadyInLobby = "User already in lobby";
        public const string UserIsNotInLobby = "Lobby does not contain user with given id";
        public const string UsersNotReady = "Not all lobby members are ready";
        public const string NoLobbies = "No active lobbies found. You can start a new game";
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