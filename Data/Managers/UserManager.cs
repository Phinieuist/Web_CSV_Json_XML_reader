using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text;
using Web_CSV_Json_XML_reader.Data.DB;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class UserManager : IUserManager
    {
        private readonly EFContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public async Task<string> DB()
        {
            var DB = await _dbContext.Users.Join(_dbContext.Files, user => user.UserId, file => file.UserId, (user, file) => new
            {
                user.UserId,
                user.Email,
                user.Password,
                file.FileId,
                file.FileName,
                file.LastChanged
            }).OrderBy(q => q.Email).ToListAsync();

            StringBuilder sb = new StringBuilder();
            foreach (var item in DB)
            {
                sb.AppendLine(string.Join(" | ", new string[] { item.UserId.ToString(),
                item.Email,
                item.Password,
                item.FileId.ToString(),
                item.FileName,
                item.LastChanged.ToString()}));
            }

            return sb.ToString();
        }

        public async Task<bool> UpdateUser(User oldUser, User newUser)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserId == oldUser.UserId);

            if (user is null) return false;

            user.Email = newUser.Email;
            user.Password = newUser.Password;

            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user is null) return false;

            _dbContext.Users.Remove(user);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<IReadOnlyCollection<User>> GetUsers()
        {
            var s = _dbContext.Users.Select(q => q).AsNoTracking();

            return await s.ToListAsync();
        }

        public async Task<User> GetUser(string email, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == email && q.Password == password);
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(q => q.UserId == userId);
        }

        public async Task<User> GetUser(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == email);
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
