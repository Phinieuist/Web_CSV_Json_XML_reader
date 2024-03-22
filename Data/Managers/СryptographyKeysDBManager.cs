using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web_CSV_Json_XML_reader.Data.DB;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class СryptographyKeysDBManager : IСryptographyKeysDBManager
    {
        private readonly EFContext _dbContext;
        private readonly IСryptographyManager _сryptographyManager;

        public async Task<RSA> CreateNewKeyPair(Guid userId)
        {
            User? user = await _dbContext.Users.FindAsync(userId);
            if (user is null) throw new ArgumentNullException("Пользователь не найден в базе данных");

            СryptographyKey? сryptographyKeyUser = await _dbContext.сryptographyKeys.FindAsync(userId);
            if (сryptographyKeyUser is not null) throw new ArgumentNullException("Для данного пользователя уже существует пара ключей");

            RSA keys = _сryptographyManager.GetNewKeyPair();

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    СryptographyKey сryptographyKey = new();
                    
                    сryptographyKey.UserId = user.UserId;
                    сryptographyKey.PublicKey = Convert.ToBase64String(keys.ExportRSAPublicKey());
                    сryptographyKey.PrivateKey = Convert.ToBase64String(keys.ExportRSAPrivateKey());
                    сryptographyKey.Created = DateTime.Now;

                    _dbContext.сryptographyKeys.Add(сryptographyKey);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return keys;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteKeyPair(Guid userId)
        {
            User? user = await _dbContext.Users.FindAsync(userId);
            if (user is null) throw new ArgumentNullException("Пользователь не найден в базе данных");

            СryptographyKey? сryptographyKeyUser = await _dbContext.сryptographyKeys.FindAsync(userId);
            if (сryptographyKeyUser is null) throw new ArgumentNullException("Для данного пользователя не существует пары ключей");

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.сryptographyKeys.Remove(сryptographyKeyUser);

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

        public async Task<RSA> GetKeyPair(Guid userId)
        {
            User? user = await _dbContext.Users.FindAsync(userId);
            if (user is null) throw new ArgumentNullException("Пользователь не найден в базе данных");

            СryptographyKey? сryptographyKeyUser = await _dbContext.сryptographyKeys.FindAsync(userId);
            if (сryptographyKeyUser is null) return null;

            RSA keys = RSA.Create();

            keys.ImportRSAPublicKey(Convert.FromBase64String(сryptographyKeyUser.PublicKey), out int bytesRead1);
            keys.ImportRSAPrivateKey(Convert.FromBase64String(сryptographyKeyUser.PrivateKey), out int bytesRead2);

            return keys;
        }

        public async Task<IReadOnlyCollection<СryptographyKey>> GetAllKeys()
        {
            var s = _dbContext.сryptographyKeys.Select(q => q).AsNoTracking();

            return await s.ToListAsync();
        }

        public СryptographyKeysDBManager(EFContext dbContext, IСryptographyManager сryptographyManager)
        {
            _dbContext = dbContext;
            _сryptographyManager = сryptographyManager;
        }
    }
}
