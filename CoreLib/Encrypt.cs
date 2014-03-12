using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace CoreLib
{
    public static class Encrypt
    {
        // Methods
        private static void Decrypt(XmlDocument doc, SymmetricAlgorithm alg)
        {
            XmlElement element = doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;
            EncryptedData encryptedData = new EncryptedData();
            encryptedData.LoadXml(element);
            EncryptedXml xml = new EncryptedXml();
            byte[] decryptedData = xml.DecryptData(encryptedData, alg);
            xml.ReplaceData(element, decryptedData);
        }

        public static XmlDocument DecryptInMemory(string srcPath, string privateKey)
        {
            RijndaelManaged alg = new RijndaelManaged();
            byte[] bytes = Encoding.Unicode.GetBytes(privateKey);
            if (bytes.Count<byte>() == 0x10)
            {
                alg.Key = bytes;
            }
            else
            {
                alg.Key = Encoding.Unicode.GetBytes(privateKey);
            }
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(srcPath);
            Decrypt(doc, alg);
            if (alg != null)
            {
                alg.Clear();
            }
            return doc;
        }

        public static XmlDocument EncryptInMemory(string srcPath, string privateKey)
        {
            RijndaelManaged key = new RijndaelManaged();
            byte[] bytes = Encoding.Unicode.GetBytes(privateKey);
            if (bytes.Count<byte>() == 0x10)
            {
                key.Key = bytes;
            }
            else
            {
                key.Key = Encoding.Unicode.GetBytes(privateKey);
            }
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(srcPath);
            Encryptwsmd(doc, key);
            if (key != null)
            {
                key.Clear();
            }
            doc.Save(srcPath);
            return doc;
        }

        public static void Encryptwsmd(XmlDocument Doc, SymmetricAlgorithm Key)
        {
            if (Doc == null)
            {
                throw new ArgumentNullException("Doc");
            }
            string name = "WSMD";
            if (Key == null)
            {
                throw new ArgumentNullException("Alg");
            }
            XmlElement inputElement = Doc.GetElementsByTagName(name)[0] as XmlElement;
            if (inputElement == null)
            {
                throw new XmlException("The specified element was not found");
            }
            byte[] buffer = new EncryptedXml().EncryptData(inputElement, Key, false);
            EncryptedData encryptedData = new EncryptedData();
            encryptedData.Type = "http://www.w3.org/2001/04/xmlenc#Element";
            string algorithm = null;
            if (Key is TripleDES)
            {
                algorithm = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
            }
            else if (Key is DES)
            {
                algorithm = "http://www.w3.org/2001/04/xmlenc#des-cbc";
            }
            if (!(Key is Rijndael))
            {
                throw new CryptographicException("The specified algorithm is notsupported for XML Encryption.");
            }
            switch (Key.KeySize)
            {
                case 0x80:
                    algorithm = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
                    break;

                case 0xc0:
                    algorithm = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";
                    break;

                case 0x100:
                    algorithm = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
                    break;
            }
            encryptedData.EncryptionMethod = new EncryptionMethod(algorithm);
            encryptedData.CipherData.CipherValue = buffer;
            EncryptedXml.ReplaceElement(inputElement, encryptedData, false);
        }
    }

}
