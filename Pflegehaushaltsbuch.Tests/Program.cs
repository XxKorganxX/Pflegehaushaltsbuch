using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Databases;
namespace Pflegehaushaltsbuch.Tests
{
    /// <summary>
    /// Contains tests that verify the password Hasher Tests behavior.
    /// </summary>
    [TestClass]
    public class PasswordHasherTests
    {
        /// <summary>
        /// Runs the hashes And Verifies Password operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void HashesAndVerifiesPassword()
        {
            string hash = PasswordHasher.Hash("CorrectHorseBatteryStaple#1");
            StringAssert.StartsWith(hash, "pbkdf2-sha256:v1:100000:");
            Assert.IsTrue(PasswordHasher.Verify("CorrectHorseBatteryStaple#1", hash));
        }
        /// <summary>
        /// Runs the rejects Wrong Password operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void RejectsWrongPassword()
        {
            string hash = PasswordHasher.Hash("CorrectHorseBatteryStaple#1");
            Assert.IsFalse(PasswordHasher.Verify("wrong-password", hash));
        }
        /// <summary>
        /// Checks support for the legacy Md5 behavior in the current implementation.
        /// </summary>
        [TestMethod]
        public void SupportsLegacyMd5()
        {
            string legacyHash = PasswordHasher.HashLegacyMd5("legacy-password");
            Assert.IsTrue(PasswordHasher.Verify("legacy-password", legacyHash));
            Assert.IsFalse(PasswordHasher.Verify("wrong-password", legacyHash));
        }
        /// <summary>
        /// Marks the legacy Md5 For Rehash state so the caller can react to it.
        /// </summary>
        [TestMethod]
        public void MarksLegacyMd5ForRehash()
        {
            string legacyHash = PasswordHasher.HashLegacyMd5("legacy-password");
            string currentHash = PasswordHasher.Hash("current-password");
            Assert.IsTrue(PasswordHasher.NeedsRehash(legacyHash));
            Assert.IsFalse(PasswordHasher.NeedsRehash(currentHash));
        }
    }
    /// <summary>
    /// Contains tests that verify the credential Protector Tests behavior.
    /// </summary>
    [TestClass]
    public class CredentialProtectorTests
    {
        /// <summary>
        /// Runs the protects And Restores Value operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void ProtectsAndRestoresValue()
        {
            string secret = "server-login:verwahrgeld";
            string protectedValue = CredentialProtector.Protect(secret);
            StringAssert.StartsWith(protectedValue, "dpapi:v1:");
            Assert.AreNotEqual(secret, protectedValue);
            Assert.AreEqual(secret, CredentialProtector.Unprotect(protectedValue));
        }
        /// <summary>
        /// Runs the rejects Invalid Format operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void RejectsInvalidFormat()
        {
            string plainValue;
            Assert.IsFalse(CredentialProtector.TryUnprotect("plain-text-password", out plainValue));
            Assert.AreEqual(string.Empty, plainValue);
            Assert.ThrowsExactly<CryptographicException>(() => CredentialProtector.Unprotect("plain-text-password"));
        }
    }
    /// <summary>
    /// Contains tests that verify the sql Tests behavior.
    /// </summary>
    [TestClass]
    public class SqlTests
    {
        /// <summary>
        /// Runs the quotes Identifier operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void QuotesIdentifier()
        {
            Assert.AreEqual("[Max]", SQL.QuoteSqlServerIdentifier("Max"));
            Assert.AreEqual("[Ma]]x]", SQL.QuoteSqlServerIdentifier("Ma]x"));
        }
        /// <summary>
        /// Runs the rejects Empty Identifier operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void RejectsEmptyIdentifier()
        {
            Assert.ThrowsExactly<ArgumentException>(() => SQL.QuoteSqlServerIdentifier(" "));
        }
        /// <summary>
        /// Runs the connection String Supports Sql Login operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void ConnectionStringSupportsSqlLogin()
        {
            SQL sql = new SQL();
            string connectionString = sql.GetConnectionString("SERVER\\SQLEXPRESS", "db_user", "pa;ss=word", "pflege");
            StringAssert.Contains(connectionString, "Data Source=SERVER\\SQLEXPRESS");
            StringAssert.Contains(connectionString, "Initial Catalog=pflege");
            StringAssert.Contains(connectionString, "User ID=db_user");
            StringAssert.Contains(connectionString, "Password=\"pa;ss=word\"");
            StringAssert.Contains(connectionString, "Trust Server Certificate=True");
        }
        /// <summary>
        /// Runs the connection String Supports Windows Login operation and updates the related application state.
        /// </summary>
        [TestMethod]
        public void ConnectionStringSupportsWindowsLogin()
        {
            SQL sql = new SQL();
            string connectionString = sql.GetConnectionString("SERVER\\SQLEXPRESS", "ignored", string.Empty);
            StringAssert.Contains(connectionString, "Data Source=SERVER\\SQLEXPRESS");
            StringAssert.Contains(connectionString, "Integrated Security=True");
            StringAssert.Contains(connectionString, "Trust Server Certificate=True");
        }
    }
}
