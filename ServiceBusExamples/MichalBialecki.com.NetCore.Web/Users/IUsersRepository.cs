namespace MichalBialecki.com.NetCore.Web.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersRepository
    {
        Task<UserDto> GetUserById(int userId);

        Task<IEnumerable<UserDto>> GetUsersById(IEnumerable<int> userIds);

        Task AddUser(string name);
        
        Task<IEnumerable<UserDto>> GetCountByCountryCode(string countryCode);

        Task<IEnumerable<UserDto>> GetCountByCountryCodeAsAnsi(string countryCode);

        Task InsertMany(IEnumerable<string> userNames);

        Task SafeInsertMany(IEnumerable<string> userNames);

        Task InsertInBulk(IList<string> userNames);

    }
}
