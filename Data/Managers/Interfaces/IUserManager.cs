using System.Diagnostics.Metrics;
using Web_CSV_Json_XML_reader.Data.DB.Entities;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IUserManager
    {
        public Task<IReadOnlyCollection<User>> GetUsers();

        public Task<User?> GetUser(Guid userId);

        public Task<User?> GetUser(string email);

        public Task<User?> GetUser(string email, string password);

        public Task<bool> AddUser(User user);

        public Task<bool> AddUser(string email, string password);

        public Task<bool> UpdateUser(User oldUser, User newUser);

        public Task<bool> DeleteUser(Guid userId);

        public Task<string> DB();
    }
}
