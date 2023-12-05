using API.Models;

namespace API.DataAccess.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccount?> GetUser(string email);
        Task AddNewAccount(UserAccount newAccount, AccountBaseInfo newAccountBaseInfo);
        Task UpdateAccount(int id, UserUpdateInfo info);
        Task SetTest(Test test);
        Task<AccountInfo?> GetAccount(int id);
        Task<List<string>> GetImages(int id);
        Task<List<string>> GetInterests(int id);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ISqlDataAccess _db;
        public AccountRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<UserAccount?> GetUser(string email)
        {
            var sql = "SELECT id, email, password_hash, password_salt FROM users WHERE email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };

            var users = await _db.LoadData<UserAccount>(sql, parameters);
            var user = users.FirstOrDefault();
            if (user == null || user.Id == 0)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public async Task AddNewAccount(UserAccount newAccount, AccountBaseInfo newAccountBaseInfo)
        {
            var sql = "INSERT INTO users (email, password_hash, password_salt) VALUES (@Email, @PasswordHash, @PasswordSalt)";
            var parameters = new Dictionary<string, object> {
                { "@Email", newAccount.Email },
                { "@PasswordHash", newAccount.PasswordHash },
                { "@PasswordSalt", newAccount.PasswordSalt }
            };
            await _db.SaveData(sql, parameters);

            var userInfo = await GetUser(newAccount.Email);

            sql = "INSERT INTO users_info (user_id, name, surname, gender, birthdate, description, avatar) VALUES (@UserId, @Name, @Surname, @Gender, @Birthdate, null, null)";
            parameters = new Dictionary<string, object> {
                { "@UserId", userInfo.Id },
                { "@Name", newAccountBaseInfo.Name },
                { "@Surname", newAccountBaseInfo.Surname },
                { "@Gender", newAccountBaseInfo.Gender },
                { "@Birthdate", newAccountBaseInfo.Birthdate }
            };
            await _db.SaveData(sql, parameters);
        }

        public async Task UpdateAccount(int id, UserUpdateInfo info)
        {
            var sql = "UPDATE users_info SET description = @Description, avatar = @Avatar WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", id }, { "@Description", info.Description }, { "@Avatar", info.Avatar } };
            await _db.UpdateData(sql, parameters);

            sql = "DELETE FROM user_images WHERE user_id = @UserId";
            parameters = new Dictionary<string, object> { { "@UserId", id } };
            await _db.DeleteData(sql, parameters);

            sql = "INSERT INTO user_images (user_id, url) VALUES (@UserId, @Url)";
            foreach (string image in info.Images)
            {
                parameters = new Dictionary<string, object> { { "@UserId", id }, { "@Url", image } };
                await _db.SaveData(sql, parameters);
            }

            sql = "DELETE FROM user_interests WHERE user_id = @UserId";
            parameters = new Dictionary<string, object> { { "@UserId", id } };
            await _db.DeleteData(sql, parameters);

            sql = "INSERT INTO user_interests (user_id, interest) VALUES (@UserId, @Interest)";
            foreach (string interest in info.Interests)
            {
                parameters = new Dictionary<string, object> { { "@UserId", id }, { "@Interest", interest } };
                await _db.SaveData(sql, parameters);
            }
        }

        public async Task SetTest(Test test)
        {
            var sql = "INSERT INTO tests (user_id, eyes, hair, tattoo, sport, education, recreation, family, charity, people, wedding ,belief, money, religious, mind, humour)" +
                "VALUES (@UserId, @Eyes, @Hair, @Tattoo, @Sport, @Education, @Recreation, @Family, @Charity, @People, @Wedding, @Belief, @Money, @Religious, @Mind, @Humour)";
            var parameters = new Dictionary<string, object> {
                { "@UserId", test.UserId }, { "@Eyes", test.Eyes }, { "@Hair", test.Hair },
                { "@Tattoo", test.Tattoo }, { "@Sport", test.Sport }, { "@Education", test.Education },
                { "@Recreation", test.Recreation }, { "@Family", test.Family }, { "Charity", test.Charity},
                { "@People", test.People }, { "@Wedding", test.Wedding }, { "@Belief", test.Belief },
                { "@Money", test.Money },{ "@Religious", test.Religious },{ "@Mind", test.Mind }, { "@Humour", test.Humour },
            };
            await _db.SaveData(sql, parameters);
        }

        public async Task<AccountInfo?> GetAccount(int id)
        {
            var sql = "SELECT name, surname, description, avatar FROM users_info WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", id } };

            var accounts = await _db.LoadData<AccountInfo>(sql, parameters);
            return accounts.SingleOrDefault();
        }

        public async Task<List<string>> GetImages(int id)
        {
            var sql = "SELECT url FROM user_images WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", id } };

            return await _db.LoadData<string>(sql, parameters);
        }

        public async Task<List<string>> GetInterests(int id)
        {
            var sql = "SELECT interest FROM user_interests WHERE user_id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", id } };

            return await _db.LoadData<string>(sql, parameters);
        }
    }
}
