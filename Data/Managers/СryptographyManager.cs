using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class СryptographyManager : IСryptographyManager
    {
        public bool CheckSignature(string message, byte[] publicKey, byte[] signature)
        {
            byte[] hash = ComputeHash(message);
            
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(publicKey, out int bytesRead);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));

                if (rsaDeformatter.VerifySignature(hash, signature))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        public RSA GetNewKeyPair()
        {
            return RSA.Create();
        }

        public byte[] GetSignature(string message, byte[] privateKey)
        {
            byte[] signedHash;

            byte[] hash = ComputeHash(message);


            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKey, out int bytesRead);

                RSAPKCS1SignatureFormatter rsaFormatter = new(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));

                signedHash = rsaFormatter.CreateSignature(hash);
            }

            return signedHash;
        }

        protected byte[] ComputeHash(string message)
        {
            using (SHA256 alg = SHA256.Create())
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                byte[] hash = alg.ComputeHash(data);

                return hash;
            }
        }

        public void test()
        {
            using SHA256 alg = SHA256.Create();

            byte[] data = Encoding.ASCII.GetBytes("Hello, from the .NET Docs!");
            byte[] hash = alg.ComputeHash(data);

            RSAParameters sharedParameters;
            byte[] signedHash;

            // Generate signature
            using (RSA rsa = RSA.Create())
            {
                sharedParameters = rsa.ExportParameters(false);

                RSAPKCS1SignatureFormatter rsaFormatter = new(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));

                signedHash = rsaFormatter.CreateSignature(hash);
            }

            // Verify signature
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(sharedParameters);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new(rsa);
                rsaDeformatter.SetHashAlgorithm(nameof(SHA256));

                if (rsaDeformatter.VerifySignature(hash, signedHash))
                {
                    Console.WriteLine("The signature is valid.");
                }
                else
                {
                    Console.WriteLine("The signature is not valid.");
                }
            }
        }
    }
}
