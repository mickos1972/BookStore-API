using System;
namespace BookStoreUI.Static
{
    public static class Endpoints
    {
        public static string BaseUrl { get; private set; } = "https://localhost:5001/"; 
        public static string AuthorsEndpoint { get; private set; } = $"{BaseUrl}api/authors/";
        public static string BooksEndpoint { get; private set; } = $"{BaseUrl}api/books/";
        public static string RegisterEndpoint { get; private set; } = $"{BaseUrl}api/users/register/";
        public static string LoginEndpoint { get; private set; } = $"{BaseUrl}api/users/login/";

    }
}
