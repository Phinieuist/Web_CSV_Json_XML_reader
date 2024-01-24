using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Web_CSV_Json_XML_reader.DB;
using Web_CSV_Json_XML_reader.Entities;

namespace Web_CSV_Json_XML_reader.Managers
{
    public class UserManager : IUserManager
    {
        private readonly EFContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public async Task<IReadOnlyCollection<User>> GetUsers()
        {
            var s = _dbContext.Users.Select(q => q).AsNoTracking();

            return await s.ToListAsync();
        }

        public Task<User> GetUser(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefaultAsync(q => q.Email == email && q.Password == password);

            return user;
        }

        public async Task<bool> AddUser(string email, string password)
        {
            User user = new(Guid.NewGuid(), email, password);
            return await AddUser(user);
        }

        public async Task<bool> AddUser(User user)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == user.Email);

            if (existingUser is not null)
                return false;

            else
            {
                _dbContext.Users.Add(user);
                int res = await _dbContext.SaveChangesAsync();

                return res > 0;
            }
        }

        public UserManager(EFContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }
    }
}
