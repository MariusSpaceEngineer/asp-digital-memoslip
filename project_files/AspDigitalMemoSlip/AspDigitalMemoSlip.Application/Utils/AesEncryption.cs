using System.Security.Cryptography;

public static class AesEncryption
{
    /// <summary>
    /// Encrypts the specified data using AES encryption.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <param name="key">The encryption key.</param>
    /// <param name="iv">The initialization vector.</param>
    /// <returns>The encrypted data.</returns>
    public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                return PerformCryptography(data, encryptor);
            }
        }
    }

    /// <summary>
    /// Decrypts the specified data using AES decryption.
    /// </summary>
    /// <param name="data">The data to decrypt.</param>
    /// <param name="key">The decryption key.</param>
    /// <param name="iv">The initialization vector.</param>
    /// <returns>The decrypted data.</returns>
    public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                return PerformCryptography(data, decryptor);
            }
        }
    }

    /// <summary>
    /// Performs the cryptographic transformation on the specified data.
    /// </summary>
    /// <param name="data">The data to transform.</param>
    /// <param name="cryptoTransform">The cryptographic transform to use.</param>
    /// <returns>The transformed data.</returns>
    private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        using (var ms = new MemoryStream())
        using (var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}
