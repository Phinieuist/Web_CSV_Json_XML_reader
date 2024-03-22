using System.Security.Cryptography;
using Web_CSV_Json_XML_reader.Data.DB.Entities;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IСryptographyKeysDBManager
    {
        public Task<RSA> CreateNewKeyPair(Guid userId);

        public Task<bool> DeleteKeyPair(Guid userId);

        public Task<RSA> GetKeyPair(Guid userId);

        public Task<IReadOnlyCollection<СryptographyKey>> GetAllKeys();
    }
}
