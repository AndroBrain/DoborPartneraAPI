using API.Models;
using System.Security.Principal;

namespace API.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(int senderId, int receiverId, string message, long timestamp);
        Task<List<Conversation>> GetConversations(int userId);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IUserRepository _userRepository;
        public MessageRepository(ISqlDataAccess db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public async Task AddMessage(int senderId, int receiverId, string message, long timestamp)
        {
            var sql = "INSERT INTO messages (from_user, to_user, text, timestamp) VALUES (@FromUser, @ToUser, @Text, @SentTimestamp)";

            var parameters = new Dictionary<string, object> {
                { "@FromUser", senderId },
                { "@ToUser", receiverId },
                { "@Text", message },
                { "@SentTimestamp", timestamp }
            };
            await _db.SaveData(sql, parameters);
        }

        public async Task<List<Conversation>> GetConversations(int userId)
        {
            var sql = "SELECT distinct(to_user) FROM messages where from_user = @UserId";
            var parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };
            var messagedPartners = await _db.LoadData<int>(sql, parameters);

            sql = "SELECT distinct(from_user) FROM messages where to_user = @UserId";
            parameters = new Dictionary<string, object> {
                { "@UserId", userId }
            };
            var partnersThatMessaged = await _db.LoadData<int>(sql, parameters);

            var partnerIds = messagedPartners.Select(x => x).ToList();

            foreach (var partner in partnersThatMessaged)
            {
                if (!partnerIds.Contains(partner))
                {
                    partnerIds.Add(partner);
                }
            }

            sql = "SELECT user_id, name, avatar FROM users_info WHERE user_id = ANY (@Ids)";
            parameters = new Dictionary<string, object> {
                { "@Ids", partnerIds}
            };
            return await _db.LoadData<Conversation>(sql, parameters);
        }
    }
}
