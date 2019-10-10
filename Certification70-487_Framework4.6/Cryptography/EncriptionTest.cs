using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Certification70_487_Framework4._6.Cryptography
{
    [TestClass]
    public class EncriptionTest
    {
        [TestMethod]
        [DataRow("Geiser")]
        public void AesEncryption(string text)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    byte[] encrypted = AESHelper.Encrypt(text, aes.Key, aes.IV);

                    // Print encrypted string    
                    Debug.WriteLine($"Encrypted data: {Encoding.UTF8.GetString(encrypted)}");

                    // Decrypt the bytes to a string.    
                    string decrypted = AESHelper.Decrypt(encrypted, aes.Key, aes.IV);

                    // Print decrypted string. It should be same as raw data    
                    Debug.WriteLine($"Decrypted data: {decrypted}");

                    Assert.AreEqual(text, decrypted);
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
            }
        }

        [TestMethod]
        [DataRow("Geiser")]
        public void RSAEncryption(string text)
        {
            //Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);

            // Create a new instance of RSACryptoServiceProvider to generate public and private key data.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                #region Generate public and Private key

                var publicKey = RSA.ToXmlString(false);
                var privateKey = RSA.ToXmlString(true);

                #endregion

                #region Encrypt and decrypt

                // Pass the data to ENCRYPT, the public key information (using RSACryptoServiceProvider.ExportParameters(false), and a boolean flag specifying no OAEP padding.
                byte[] encryptedData = RSAHelper.RSAEncrypt(dataToEncrypt, publicKey);

                // Pass the data to DECRYPT, the private key information  (using RSACryptoServiceProvider.ExportParameters(true), and a boolean flag specifying no OAEP padding.
                byte[] decryptedData = RSAHelper.RSADecrypt(encryptedData, privateKey);

                #endregion

                // Display the decrypted plaintext to the console. 
                Debug.WriteLine($"Decrypted plaintext: {Encoding.UTF8.GetString(decryptedData)}");

                Assert.AreEqual(text, Encoding.UTF8.GetString(decryptedData));
            }
        }
    }
}
