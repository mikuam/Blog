namespace MichalBialecki.com.NetCore.Web.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    public class UsersRepository : IUsersRepository
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=Blog;Integrated Security=True";
        private readonly Random random = new Random();
        
        public async Task<UserDto> GetUserById(int userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<UserDto>(
                    "SELECT [Id], [Name], [LastUpdatedAt] FROM [Users] WHERE Id = @Id",
                    new { Id = userId }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersById(IEnumerable<int> userIds)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT [Id], [Name], [LastUpdatedAt] FROM [Users] WHERE Id IN @Ids",
                    new { Ids = userIds }).ConfigureAwait(false);
            }
        }

        public async Task AddUser(string name)
        {
            var countryCode = random.NextDouble() > 0.5 ? "US" : "PL";
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO [Users] (Name, CountryCode, LastUpdatedAt) VALUES (@Name, @countryCode, getdate())",
                    new { name, countryCode }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetCountByCountryCode(string countryCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT count(*) FROM [Users] WHERE CountryCode = @CountryCode",
                    new { CountryCode = countryCode }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<UserDto>> GetCountByCountryCodeAsAnsi(string countryCode)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<UserDto>(
                    "SELECT count(*) FROM [Users] WHERE CountryCode = @CountryCode",
                    new { CountryCode = new DbString() { Value = countryCode, IsAnsi = true, Length = 2 } })
                    .ConfigureAwait(false);
            }
        }

        public async Task InsertMany(IEnumerable<string> userNames)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO [Users] (Name, LastUpdatedAt) VALUES (@Name, getdate())",
                    userNames.Select(u => new { Name = u })).ConfigureAwait(false);
            }
        }

        public async Task SafeInsertMany(IEnumerable<string> userNames)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var parameters = userNames.Select(u =>
                    {
                        var tempParams = new DynamicParameters();
                        tempParams.Add("@Name", u, DbType.String, ParameterDirection.Input);
                        return tempParams;
                    });

                await connection.ExecuteAsync(
                    "INSERT INTO [Users] (Name, LastUpdatedAt) VALUES (@Name, getdate())",
                    parameters).ConfigureAwait(false);
            }
        }

        public async Task InsertInBulk(IList<string> userNames)
        {
            var sqls = GetSqlsInBatches(userNames);
            using (var connection = new SqlConnection(ConnectionString))
            {
                foreach (var sql in sqls)
                {
                    await connection.ExecuteAsync(sql);
                }
            }
        }

        private IList<string> GetSqlsInBatches(IList<string> userNames)
        {
            var insertSql = "INSERT INTO [Users] (Name, LastUpdatedAt) VALUES ";
            var valuesSql = "('{0}', getdate())";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)userNames.Count / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var userToInsert = userNames.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = userToInsert.Select(u => string.Format(valuesSql, u));
                sqlsToExecute.Add(insertSql + string.Join(',', valuesToInsert));
            }

            return sqlsToExecute;
        }
    }
}
