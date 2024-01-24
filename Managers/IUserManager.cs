using System.Diagnostics.Metrics;
using Web_CSV_Json_XML_reader.Entities;

namespace Web_CSV_Json_XML_reader.Managers
{
    public interface IUserManager
    {
        public Task<IReadOnlyCollection<User>> GetUsers();

        public Task<User> GetUser(string email, string password);

        public Task<bool> AddUser(User user);

        public Task<bool> AddUser(string email, string password);
    }
}
