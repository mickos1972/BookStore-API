using System;
using System.Threading.Tasks;
using BookStoreUI.Models;

namespace BookStoreUI.Contracts
{
    public interface IAuthenticationRepository
    {
        public Task<bool> Register(RegistrationModel user);
        public Task<bool> Login(LoginModel user);
        public Task Logout();
    }
}
