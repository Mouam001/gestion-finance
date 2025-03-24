using Common.DAO;
using Common.DTO;

namespace Common.Requests{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}