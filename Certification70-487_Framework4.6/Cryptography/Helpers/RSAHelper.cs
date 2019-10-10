using System.Diagnostics;
using System.Security.Cryptography;

namespace Certification70_487_Framework4._6.Cryptography
{
    public class RSAHelper
    {
        const bool OAEPPADDING = false;

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, string publicKey)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This only needs toinclude the public key information.
                    RSA.FromXmlString(publicKey);

                    //Encrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.  
                    return RSA.Encrypt(DataToEncrypt, OAEPPADDING);
                }
            }
            //Catch and display a CryptographicException to the console.
            catch (CryptographicException e)
            {
                Debug.WriteLine(e.Message);
            }
         
            return null;
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, string privateKey)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs to include the private key information.
                    RSA.FromXmlString(privateKey);

                    //Decrypt the passed byte array and specify OAEP padding. OAEP padding is only available on Microsoft Windows XP or later.  
                    return RSA.Decrypt(DataToDecrypt, OAEPPADDING);
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine(e.ToString());
            }
            
            return null;
        }
    }
}
