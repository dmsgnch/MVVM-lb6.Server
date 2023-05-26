using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_lb6.Domain.Routes
{
    public static class ApiRoutes 
    {
        public static class Authentication
        {
            public const string Register = "register";
            public const string Login = "login";
        }
        
        public static class Room
        {
            public const string GetAllRooms = "GetAllRooms";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "delete/{id}";
            public const string GetAvailable = "GetAvailable";
            public const string Book = "book";
            public const string Stay = "stay";
        }
        
        public static class Session
        {
            public const string GetById = "{id}";
            public const string ResearchColonizePlanet = "researchorcolonize/{sessionId}";
        }
        
        public static class Map
        {
            public const string GetMap = "{id}";
        }
    }
}
