using System.Threading.Tasks;
using BlazorSPA.Client.DTOs;

namespace BlazorSPA.Client.Services
{
    public interface IAuthService
    {
        Task<(bool, string)> Login(LoginDTO loginDTO);

        void Logout();
    }
}