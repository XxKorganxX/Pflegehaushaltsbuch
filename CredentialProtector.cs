using System;
using System.Security.Cryptography;
using System.Text;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Provides helper methods for credential Protector operations used by the application.
    /// </summary>
    internal static class CredentialProtector
    {
        private const string ProtectedDataPrefix = "dpapi:v1:";
        private static readonly byte[] AdditionalEntropy = Encoding.UTF8.GetBytes("Pflegehaushaltsbuch.Credentials.v1");
        /// <summary>
        /// Protects the protect value before it is stored or reused.
        /// </summary>
        internal static string Protect(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            byte[] plainData = Encoding.UTF8.GetBytes(value);
            byte[] protectedData = ProtectedData.Protect(plainData, AdditionalEntropy, DataProtectionScope.LocalMachine);
            return ProtectedDataPrefix + Convert.ToBase64String(protectedData);
        }
        /// <summary>
        /// Restores the protected unprotect value for use by the application.
        /// </summary>
        internal static string Unprotect(string value)
        {
            if (TryUnprotect(value, out string plainValue))
                return plainValue;
            throw new CryptographicException("The credential was not protected with the current DPAPI format.");
        }
        /// <summary>
        /// Tries to run the unprotect operation and reports whether it succeeded.
        /// </summary>
        internal static bool TryUnprotect(string value, out string plainValue)
        {
            plainValue = string.Empty;
            if (string.IsNullOrEmpty(value) || !value.StartsWith(ProtectedDataPrefix, StringComparison.Ordinal))
                return false;
            try
            {
                string protectedValue = value.Substring(ProtectedDataPrefix.Length);
                byte[] protectedData = Convert.FromBase64String(protectedValue);
                byte[] plainData = ProtectedData.Unprotect(protectedData, AdditionalEntropy, DataProtectionScope.LocalMachine);
                plainValue = Encoding.UTF8.GetString(plainData);
                return true;
            }
            catch (CryptographicException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
