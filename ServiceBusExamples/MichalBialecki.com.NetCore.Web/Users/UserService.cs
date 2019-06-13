namespace MichalBialecki.com.NetCore.Web.Users
{
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        public async Task<bool> ExportUsersToExternalSystem()
        {
            return await Task.FromResult(true);
        }
    }
}
