using API.Models;
using System.Security.Principal;

namespace API.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> AddMessage(Message message);
        Task<List<Conversation>> GetConversations(int userId);
        Task<List<Message>> GetMessages(int ownerId, int receiverId, long timestamp);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly ISqlDataAccess _db;
        public MessageRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Message> AddMessage(Message message)
        {
            var sql = "INSERT INTO messages (from_user, to_user, text, timestamp) VALUES (@FromUser, @ToUser, @Text, @SentTimestamp) RETURNING id";

            var parameters = new Dictionary<string, object> {
                { "@FromUser", message.FromUser },
                { "@ToUser", message.ToUser },
                { "@Text", message.MessageText },
                { "@SentTimestamp", message.SentTimestamp }
            };
            var id = await _db.SaveDataWithId(sql, parameters);
            message.Id = id;
            return message;
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
         
            var partners = await _db.LoadData<Conversation>(sql, parameters);

            sql = "SELECT * FROM messages " +
                "WHERE (from_user = @FromId AND to_user = @ToId) " +
                "OR (from_user = @ToId AND to_user = @FromId) " +
                "ORDER BY timestamp DESC " +
                "LIMIT 10";

            for (int i = 0; i < partners.Count; i++)
            {
                parameters = new Dictionary<string, object> {
                    { "@FromId", userId }, {"@ToId", partners[i].UserId}
                };
                partners[i].Messages = await _db.LoadData<Message>(sql, parameters);
            }

            return partners;
        }

        public async Task<List<Message>> GetMessages(int ownerId, int receiverId, long timestamp)
        {
            var sql = "SELECT * FROM messages " +
                "WHERE (" +
                "(from_user = @FromId AND to_user = @ToId) " +
               "OR (from_user = @ToId AND to_user = @FromId)" +
                ") " +
                "AND timestamp < @Timestamp " +
                "ORDER BY timestamp DESC " +
                "LIMIT 10";
            var parameters = new Dictionary<string, object>
            {
                {"@FromId", ownerId }, {"@ToId", receiverId }, {"@Timestamp", timestamp}
            };
            return await _db.LoadData<Message>(sql, parameters);
        }
    }
}
