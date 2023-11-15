using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IPartnerRepository
    {
        Task DeclineUser(int userId, int declinedUserId);
        Task<Match?> GetMatch(int id);
        Task<List<Match>> GetMatches(int id);
    }

    public class PartnerRepository : IPartnerRepository
    {
        private readonly ISqlDataAccess _db;

        public PartnerRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task DeclineUser(int userId, int declinedUserId)
        {
            var sql = "INSERT INTO declined_matches (user_id, declined_user_id) VALUES (@UserId, @DeclinedUserId)";
            var parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@DeclinedUserId", declinedUserId} };
            // TODO delete all messages
            await _db.SaveData(sql, parameters);

            sql = "DELETE FROM messages WHERE (from_user = @UserId AND to_user = @DeclinedUserId) OR (from_user = @DeclinedUserId AND to_user = @UserId)";
            parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@DeclinedUserId", declinedUserId } };
            await _db.DeleteData(sql, parameters);
        }

        public async Task<Match?> GetMatch(int id)
        {
            var sql = "SELECT user_id, name, birthdate, description, avatar FROM users_info WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", id } };

            return (await _db.LoadData<Match>(sql, parameters)).FirstOrDefault();
        }

        public async Task<List<Match>> GetMatches(int id)
        {
            var sql = "SELECT user_id, name, birthdate, description, avatar FROM users_info " +
                "WHERE NOT user_id = @UserId " +
                "AND user_id NOT IN (SELECT declined_user_id FROM declined_matches WHERE user_id = @UserId) " +
                "AND description IS NOT NULL " +
                "AND avatar IS NOT NULL";
            var parameters = new Dictionary<string, object> { { "@UserId", id } };

            var accounts = await _db.LoadData<Match>(sql, parameters);

            return accounts.ToList();
        }
    }
}
