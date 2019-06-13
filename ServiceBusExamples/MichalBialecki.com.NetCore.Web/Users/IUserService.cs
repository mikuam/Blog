namespace MichalBialecki.com.NetCore.Web.Users
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<bool> ExportUsersToExternalSystem();
    }
}