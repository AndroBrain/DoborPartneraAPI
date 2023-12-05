using API.Models;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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
            var parameters = new Dictionary<string, object> { { "@UserId", userId }, { "@DeclinedUserId", declinedUserId } };
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

            sql = "SELECT * FROM tests " +
                "WHERE NOT user_id = @UserId " +
                "AND user_id IN (SELECT declined_user_id FROM declined_matches WHERE user_id = @UserId)";
            parameters = new Dictionary<string, object> { { "@UserId", id } };

            var tests = await _db.LoadData<Test>(sql, parameters);

            var eyesPrefferability = new Dictionary<int, int>();
            var hairPrefferability = new Dictionary<int, int>();
            var tattooPrefferability = new Dictionary<int, int>();
            var sportPrefferability = new Dictionary<int, int>();
            var educationPrefferability = new Dictionary<int, int>();
            var recreationPrefferability = new Dictionary<int, int>();
            var familyPrefferability = new Dictionary<int, int>();
            var charityPrefferability = new Dictionary<int, int>();
            var peoplePrefferability = new Dictionary<int, int>();
            var weddingPrefferability = new Dictionary<int, int>();
            var beliefPrefferability = new Dictionary<int, int>();
            var moneyPrefferability = new Dictionary<int, int>();
            var religiousPrefferability = new Dictionary<int, int>();
            var mindPrefferability = new Dictionary<int, int>();
            var humourPrefferability = new Dictionary<int, int>();
            tests.ForEach(test =>
            {
                IncrementOrSet(eyesPrefferability, test.Eyes);
                IncrementOrSet(hairPrefferability, test.Hair);
                IncrementOrSet(tattooPrefferability, test.Tattoo);
                IncrementOrSet(sportPrefferability, test.Sport);
                IncrementOrSet(educationPrefferability, test.Education);
                IncrementOrSet(recreationPrefferability, test.Recreation);
                IncrementOrSet(familyPrefferability, test.Family);
                IncrementOrSet(charityPrefferability, test.Charity);
                IncrementOrSet(peoplePrefferability, test.People);
                IncrementOrSet(weddingPrefferability, test.Wedding);
                IncrementOrSet(beliefPrefferability, test.Belief);
                IncrementOrSet(moneyPrefferability, test.Money);
                IncrementOrSet(religiousPrefferability, test.Religious);
                IncrementOrSet(mindPrefferability, test.Mind);
                IncrementOrSet(humourPrefferability, test.Humour);
            }
            );

            sql = "SELECT * FROM tests WHERE user_id = ANY (@Accounts)";
            parameters = new Dictionary<string, object> { { "@Accounts", accounts.Select(x => x.UserId).ToList() } };
            var accountTests = await _db.LoadData<Test>(sql, parameters);

            accountTests.Sort((t, nextT) =>
                (
                GetOrZero(eyesPrefferability, t.Eyes) + GetOrZero(hairPrefferability, t.Hair) +
                GetOrZero(tattooPrefferability, t.Tattoo) + GetOrZero(sportPrefferability, t.Sport) +
                GetOrZero(educationPrefferability, t.Education) + GetOrZero(recreationPrefferability, t.Recreation) +
                GetOrZero(familyPrefferability, t.Family) + GetOrZero(charityPrefferability, t.Charity) +
                GetOrZero(peoplePrefferability, t.People) + GetOrZero(weddingPrefferability, t.Wedding) +
                GetOrZero(beliefPrefferability, t.Belief) + GetOrZero(moneyPrefferability, t.Money) +
                GetOrZero(religiousPrefferability, t.Religious) + GetOrZero(mindPrefferability, t.Mind) +
                GetOrZero(humourPrefferability, t.Humour)
                ) -
                (
                GetOrZero(eyesPrefferability, nextT.Eyes) + GetOrZero(hairPrefferability, nextT.Hair) +
                GetOrZero(tattooPrefferability, nextT.Tattoo) + GetOrZero(sportPrefferability, nextT.Sport) +
                GetOrZero(educationPrefferability, nextT.Education) + GetOrZero(recreationPrefferability, nextT.Recreation) +
                GetOrZero(familyPrefferability, nextT.Family) + GetOrZero(charityPrefferability, nextT.Charity) +
                GetOrZero(peoplePrefferability, nextT.People) + GetOrZero(weddingPrefferability, nextT.Wedding) +
                GetOrZero(beliefPrefferability, nextT.Belief) + GetOrZero(moneyPrefferability, nextT.Money) +
                GetOrZero(religiousPrefferability, nextT.Religious) + GetOrZero(mindPrefferability, nextT.Mind) +
                GetOrZero(humourPrefferability, nextT.Humour)
                )
            );

            accounts.Sort((a, nextA) => accountTests.FindIndex(t => t.UserId == a.UserId) - accountTests.FindIndex(t => t.UserId == nextA.UserId));

            return accounts.ToList();
        }

        private static Dictionary<int, int> IncrementOrSet(Dictionary<int, int> dictionary, int key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = dictionary[key] + 1;
            }
            else
            {
                dictionary[key] = 1;
            }
            return dictionary;
        }

        private static int GetOrZero(Dictionary<int, int> dictionary, int key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return 0;
            }
        }
    }
}
