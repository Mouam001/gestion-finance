using Common.DTO;
using Common.Requests;

namespace Business.Interfaces
{

    public interface IUserService
    {
       Task<UserDto> RegisterUser (RegisterRequest request);
    }
}