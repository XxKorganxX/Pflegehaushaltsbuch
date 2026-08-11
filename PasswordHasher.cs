using System;
using System.Security.Cryptography;
using System.Text;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Provides helper methods for password Hasher operations used by the application.
    /// </summary>
    internal static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private const string Prefix = "pbkdf2-sha256:v1";
        /// <summary>
        /// Creates a hash for the hash value using the configured algorithm.
        /// </summary>
        internal static string Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            byte[] hash = HashPassword(password, salt, Iterations, HashSize);
            return string.Format(
                "{0}:{1}:{2}:{3}",
                Prefix,
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }
        /// <summary>
        /// Verifies the verify value against the stored application data.
        /// </summary>
        internal static bool Verify(string password, string storedHash)
        {
            if (password == null || string.IsNullOrWhiteSpace(storedHash))
                return false;
            if (IsLegacyMd5Hash(storedHash))
                return FixedTimeEquals(HashLegacyMd5(password), storedHash);
            string[] parts = storedHash.Split(':');
            if (parts.Length != 5 || parts[0] != "pbkdf2-sha256" || parts[1] != "v1")
                return false;
            int iterations;
            if (!int.TryParse(parts[2], out iterations))
                return false;
            byte[] salt;
            byte[] expectedHash;
            try
            {
                salt = Convert.FromBase64String(parts[3]);
                expectedHash = Convert.FromBase64String(parts[4]);
            }
            catch (FormatException)
            {
                return false;
            }
            byte[] actualHash = HashPassword(password, salt, iterations, expectedHash.Length);
            return FixedTimeEquals(actualHash, expectedHash);
        }
        /// <summary>
        /// Runs the needs Rehash operation and updates the related application state.
        /// </summary>
        internal static bool NeedsRehash(string storedHash)
        {
            return IsLegacyMd5Hash(storedHash) || !storedHash.StartsWith(Prefix + ":" + Iterations + ":", StringComparison.Ordinal);
        }
        /// <summary>
        /// Creates a hash for the legacy Md5 value using the configured algorithm.
        /// </summary>
        internal static string HashLegacyMd5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    builder.Append(data[i].ToString("x2"));
                return builder.ToString();
            }
        }
        /// <summary>
        /// Checks whether the legacy Md5 Hash condition is true for the current value.
        /// </summary>
        private static bool IsLegacyMd5Hash(string storedHash)
        {
            if (storedHash == null || storedHash.Length != 32)
                return false;
            for (int i = 0; i < storedHash.Length; i++)
            {
                char c = storedHash[i];
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Creates a hash for the password value using the configured algorithm.
        /// </summary>
        private static byte[] HashPassword(string password, byte[] salt, int iterations, int hashSize)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                return pbkdf2.GetBytes(hashSize);
        }
        /// <summary>
        /// Runs the fixed Time Equals operation and updates the related application state.
        /// </summary>
        private static bool FixedTimeEquals(string left, string right)
        {
            if (left == null || right == null)
                return false;
            return FixedTimeEquals(Encoding.UTF8.GetBytes(left), Encoding.UTF8.GetBytes(right));
        }
        /// <summary>
        /// Runs the fixed Time Equals operation and updates the related application state.
        /// </summary>
        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
                return false;
            int difference = 0;
            for (int i = 0; i < left.Length; i++)
                difference |= left[i] ^ right[i];
            return difference == 0;
        }
    }
}
