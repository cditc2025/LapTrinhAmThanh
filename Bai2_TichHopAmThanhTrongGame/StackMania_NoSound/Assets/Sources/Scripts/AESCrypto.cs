using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KienChi
{
	public static class AESCrypto
	{
		// Khóa và IV (nên lưu an toàn, không hard-code nếu dùng thực tế)
		private static readonly string key = "1234567890123456"; // 16 ký tự = 128 bit
		private static readonly string iv = "abcdefghijklmnop"; // 16 ký tự

		public static string Encrypt(string plainText)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = Encoding.UTF8.GetBytes(iv);

				ICryptoTransform encryptor = aes.CreateEncryptor();
				byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
				byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
				return Convert.ToBase64String(encrypted);
			}
		}

		public static string Decrypt(string cipherText)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = Encoding.UTF8.GetBytes(iv);

				ICryptoTransform decryptor = aes.CreateDecryptor();
				byte[] inputBytes = Convert.FromBase64String(cipherText);
				byte[] decrypted = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
				return Encoding.UTF8.GetString(decrypted);
			}
		}
	}
}
