using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IUserRepository 
    {
        Task<int?> GetUserId(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;
        public UserRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<int?> GetUserId(string email)
        {
            var sql = "SELECT id FROM users WHERE email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };

            var users = await _db.LoadData<int>(sql, parameters);

            return users.SingleOrDefault();
        }
    }
}
