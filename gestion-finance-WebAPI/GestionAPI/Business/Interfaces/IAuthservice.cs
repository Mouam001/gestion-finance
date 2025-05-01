using Common.Requests;

namespace Business.Interfaces
{
    public interface IAuthservice
    {
        Task<AuthenticateResponse> Login (LoginRequest request);
        Task Logout();

    }
}