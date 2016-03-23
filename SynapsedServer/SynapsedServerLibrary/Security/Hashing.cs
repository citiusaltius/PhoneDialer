using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Security
{
    public class Hashing
    {
        System.Security.Cryptography.KeyedHashAlgorithm KeyedAlgorithm = new System.Security.Cryptography.HMACSHA512();

        public string Hash(string Salt, string Message)
        {
            KeyedAlgorithm.Key = Encoding.UTF8.GetBytes(Salt);
            byte[] HashBytes = KeyedAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(Message));
            return Utilities.Debug.ByteArrayToHexString(HashBytes);
        }

        public string PasswordHash(string Salt, string Password)
        {
            string Hash1 = Hash(Salt, Password + Salt);
            string Hash2 = Hash(Salt, Hash1 + Salt);
            return Hash2;
        }

        public static string Sha256(string Message)
        {
            System.Security.Cryptography.SHA256 Hasher = System.Security.Cryptography.SHA256.Create();
            return Utilities.Debug.ByteArrayToHexString(Hasher.ComputeHash(Encoding.UTF8.GetBytes(Message)));
        }
    }
}
