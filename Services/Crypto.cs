using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace sapping.Services
{
    class Crypto
    {
        public static string Decrypt(string inputText)
        {
            var _key = Encoding.ASCII.GetBytes("12%R0!_|*;RE4#s56d()¡c'l4*9ssswf");
            var _iv = Encoding.ASCII.GetBytes("D3&p/¡!_].37h$%S");
            var inputBytes = Convert.FromBase64String(inputText);
            var textoLimpio = "";
            var cripto = new RijndaelManaged();
            using (var ms = new MemoryStream(inputBytes))
            {
                using (var objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(_key, _iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }

            return textoLimpio;
        }

        public static string Encrypt(string inputText)
        {
            var _key = Encoding.ASCII.GetBytes("12%R0!_|*;RE4#s56d()¡c'l4*9ssswf");
            var _iv = Encoding.ASCII.GetBytes("D3&p/¡!_].37h$%S");

            var inputBytes = Encoding.ASCII.GetBytes(inputText);
            byte[] encripted = null;

            var cripto = new RijndaelManaged();
            using (var ms = new MemoryStream(inputBytes.Length))
            {
                using (var objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(_key, _iv), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }

            return Convert.ToBase64String(encripted);
        }
    }
}
