using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Web_CSV_Json_XML_reader.Data.DB;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class UserManager : IUserManager
    {
        private readonly EFContext _dbContext;

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
            User? user = await _dbContext.Users.FindAsync(oldUser.UserId);

            if (user is null) throw new ArgumentNullException("Пользователь не найден в базе данных");
            
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    user.Email = newUser.Email;
                    user.Password = newUser.Password;

                    int res = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return res > 0;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            User? user = await _dbContext.Users.FindAsync(userId);

            if (user is null) throw new ArgumentNullException("Пользователь не найден в базе данных");

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Users.Remove(user);

                    int res = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return res > 0;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IReadOnlyCollection<User>> GetUsers()
        {
            var s = _dbContext.Users.Select(q => q).AsNoTracking();

            return await s.ToListAsync();
        }

        public async Task<User?> GetUser(string email, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == email && q.Password == password);
        }

        public async Task<User?> GetUser(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User?> GetUser(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task<bool> AddUser(string email, string password)
        {
            try
            {
                User user = new(Guid.NewGuid(), email, password);
                return await AddUser(user);
            }
            catch { throw; }
        }

        public async Task<bool> AddUser(User user)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(q => q.Email == user.Email);

            if (existingUser is not null)
                throw new ArgumentException("Такой пользователь уже существует в базе данных");

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Users.Add(user);

                    int res = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return res > 0;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public UserManager(EFContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
