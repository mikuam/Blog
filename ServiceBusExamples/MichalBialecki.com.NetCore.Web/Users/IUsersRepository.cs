using System.Collections.Generic;
using System.Threading.Tasks;

namespace MichalBialecki.com.NetCore.Web.Users
{
    public interface IUsersRepository
    {
        Task<UserDto> GetUserById(int userId);

        Task<IEnumerable<UserDto>> GetUsersById(IEnumerable<int> userIds);

        Task AddUser(string name);

        Task<IEnumerable<UserDto>> GetCountByCountryCode(string countryCode);

        Task<IEnumerable<UserDto>> GetCountByCountryCodeAsAnsi(string countryCode);
    }
}
