using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Shagu.Utils.Utils
{
    public class Md5Helper
    {
        /// <summary>
        /// 返回的值为大写
        /// </summary>
        public static string ComputeHash(string input)
        {
            var hash = GetHash(input);

            Array.Resize(ref hash, 16);

            var builder = new StringBuilder();
            foreach (var b in hash)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        public static byte[] GetHash(string input)
        {
            var md5 = HashAlgorithmProvider.OpenAlgorithm("MD5");
            var buff = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var hashed = md5.HashData(buff);
            var hash = hashed.ToArray();
            return hash;
        }

        public static byte[] ComputeHash(byte[] buffer)
        {
            var md5 = HashAlgorithmProvider.OpenAlgorithm("MD5");
            var buff = CryptographicBuffer.CreateFromByteArray(buffer);
            var hashed = md5.HashData(buff);
            var hash = hashed.ToArray();
            return hash;
        }

    }
}
