using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get the plaintext
            string plainText = "Hello, World!";

            Console.WriteLine("Plain text");
            Console.WriteLine(plainText);
            Console.WriteLine();

            // Encode the plaintext
            Encoding encoding= Encoding.UTF8;
            

            // obtain the plainText as byte[]
            byte[] plainTextAsByte = encoding.GetBytes(plainText);

            Console.WriteLine("Plaintext converted to byte[]");
            Console.WriteLine(String.Join(",", plainTextAsByte));
            Console.WriteLine();

            /*
            for (int i= 0; i < plainTextAsByte.Length; i++)
            {
                Console.WriteLine(plainTextAsByte[i]);
            }
            */

            string plainTextAsBase64String = Convert.ToBase64String(plainTextAsByte);
            Console.WriteLine("Base 64 String:");
            Console.WriteLine(plainTextAsBase64String);
            Console.WriteLine();

            // byte[] byteData = Convert.FromBase64String(plainTextAsBase64String);

            Aes aes = Aes.Create();
            
            // In the assignment I might give you the key to use
            aes.GenerateKey();
            // aes.Key = ...
            aes.GenerateIV();
            // aes.IV = ... 

            byte[] key = aes.Key;
            Console.WriteLine("Key:");
            Console.WriteLine(String.Join(",", key));
            Console.WriteLine();

            byte[] iv = aes.IV;
            Console.WriteLine("IV:");
            Console.WriteLine(String.Join(",", iv));
            Console.WriteLine();

            aes.Mode = CipherMode.CBC; // usually CBC is OK - however in the assignment I might change this requirement
            aes.Padding = PaddingMode.ISO10126;

            // var encryptor = aes.CreateEncryptor();

            // String used to store the encrypted result
            String encryptedString = String.Empty;

            // The memory stream will be used to receive the encrypted byte[]
            using (MemoryStream ms = new MemoryStream())
            {
                // The cryptostream will receive the encryptor
                // and it will be used to transform the input and output the resulting encryption to the memory stream
                using (CryptoStream cs = new CryptoStream(
                    ms,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write
                    ))
                {
                    // This will take the plain text
                    // It will write the encrypted information to ms
                    // Add padding if necessary
                    cs.Write(plainTextAsByte, 0, plainTextAsByte.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0; // go back to start
                    byte[] buffer = ms.GetBuffer(); // the buffer is the byte[] that memory stream is built upon
                    
                    // careful! Common mistake! This will not work!
                    // encryptedString = Convert.ToBase64String(buffer, 0, buffer.Length);
                    encryptedString = Convert.ToBase64String(buffer, 0, (int)ms.Length);
                }
            }

            Console.WriteLine("Encrypted String:");
            Console.WriteLine(encryptedString);
            Console.WriteLine();

            string decryptedString = String.Empty;
            byte[] encryptedDataAsByte =  Convert.FromBase64String(encryptedString);

            using (MemoryStream ms = new MemoryStream(encryptedDataAsByte))
            {
                using (CryptoStream cs = new CryptoStream(
                    ms,
                    aes.CreateDecryptor(),
                    CryptoStreamMode.Read
                    ))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        decryptedString = sr.ReadToEnd();
                    }
                }
            }

            Console.WriteLine("Decrypted String:");
            Console.WriteLine(decryptedString);
            Console.WriteLine();

        }
    }
}