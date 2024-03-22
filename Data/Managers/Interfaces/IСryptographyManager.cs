using System.Security.Cryptography;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IСryptographyManager
    {
        public RSA GetNewKeyPair();

        public byte[] GetSignature(string message, byte[] privateKey);

        public bool CheckSignature(string message, byte[] publicKey, byte[] signature);
    }
}
