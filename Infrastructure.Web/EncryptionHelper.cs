using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Web
{
    public class EncryptionHelper
    {
        /// <summary>
        /// Tính toán chuỗi byte hash (băm) từ xâu gốc.
        /// </summary>
        /// <param name="inputString">Xâu gốc</param>
        /// <returns>Chuỗi byte sau khi đã tính hash.</returns>
        private static byte[] Hash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Tính toán xâu hash từ xâu gốc.
        /// </summary>
        /// <param name="inputString">Xâu gốc</param>
        /// <returns>Xâu sâu khi đã hash</returns>
        public static string GetHash(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Hash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Lấy xâu ngẫu nhiên có độ dài 10.
        /// </summary>
        /// <returns>Xâu ngẫu nhiên có độ dài 10.</returns>
        public static string GetSalt()
        {
            int saltLength = 10;
            const string baseString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];
                while (saltLength-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(baseString[(int)(num % (uint)baseString.Length)]);
                }
            }
            return res.ToString();
        }
    }
}
