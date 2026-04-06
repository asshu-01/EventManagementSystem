using System;
using System.Security.Cryptography;

namespace EventManagementSystem.Security
{
    public static class PasswordHasher
    {
        // 🔐 CONFIGURATION
        private const int SaltSize = 16;      // 128-bit
        private const int HashSize = 32;      // 256-bit
        private const int Iterations = 100000;

        // 🔥 HASH PASSWORD
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            // 🔐 GENERATE SALT
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 🔐 CREATE HASH (PBKDF2 + SHA256)
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            // FORMAT: iterations.salt.hash
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        // 🔁 VERIFY PASSWORD
        public static bool VerifyPassword(string enteredPassword, string storedValue)
        {
            if (string.IsNullOrWhiteSpace(enteredPassword) || string.IsNullOrWhiteSpace(storedValue))
                return false;

            // ❌ NO PLAIN TEXT ALLOWED
            if (!storedValue.Contains("."))
                return false;

            string[] parts = storedValue.Split('.');
            if (parts.Length != 3)
                return false;

            int iterations;
            if (!int.TryParse(parts[0], out iterations))
                return false;

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedHash = Convert.FromBase64String(parts[2]);

            // 🔐 RE-COMPUTE HASH
            byte[] computedHash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                enteredPassword,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                computedHash = pbkdf2.GetBytes(storedHash.Length);
            }

            // 🔒 CONSTANT TIME COMPARISON
            return FixedTimeEquals(storedHash, computedHash);
        }

        // ⏱️ PREVENT TIMING ATTACK
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }

        // 🔁 OPTIONAL SHORT METHODS (FOR CLEAN CODE)
        public static string Hash(string password)
        {
            return HashPassword(password);
        }

        public static bool Verify(string enteredPassword, string storedValue)
        {
            return VerifyPassword(enteredPassword, storedValue);
        }
    }
}