using System;
namespace BookStore_API.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Content { get; set; }
    }
}