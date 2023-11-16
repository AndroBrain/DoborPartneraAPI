using Dapper;
using Npgsql;
using System.Data;

namespace API.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T>(string sql, Dictionary<string, object> parameters);
        Task SaveData(string sql, Dictionary<string, object> parameters);
        Task<int> SaveDataWithId(string sql, Dictionary<string, object> parameters);
        Task UpdateData(string sql, Dictionary<string, object> parameters);
        Task DeleteData(string sql, Dictionary<string, object> parameters);
    }
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly string connectionString = "Host=postgresql_database;Username=admin;Password=admin;Database=ApplicationDatabase;";
        public async Task<List<T>> LoadData<T>(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, new DynamicParameters(parameters));
                return data.ToList();
            }
        }

        public async Task SaveData(string sql, Dictionary<string, object> parameters)
        {
            await ExecuteSql(sql, parameters);
        }

        public async Task<int> SaveDataWithId(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                return (int)connection.ExecuteScalar(sql, new DynamicParameters(parameters));
            }
        }

        public async Task UpdateData(string sql, Dictionary<string, object> parameters)
        {
            await ExecuteSql(sql, parameters);
        }

        public async Task DeleteData(string sql, Dictionary<string, object> parameters)
        {
            await ExecuteSql(sql, parameters);
        }

        private async Task ExecuteSql(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, new DynamicParameters(parameters));
            }
        }
    }
}
