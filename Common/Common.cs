using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json.Linq;
using Project.Donna;

namespace Project.Donna.Common
{
    public class Common : System.Web.UI.Page
    {
        // 비밀번호 암호화 SHA256
        protected string EncryptPassword(string pPassword)
        {
            StringBuilder sbPlain = new StringBuilder();
            try
            {
                System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256Managed();
                byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(pPassword));
                
                foreach (byte b in hash)
                {
                    sbPlain.AppendFormat("{0:x2}", b);
                }
            }
            catch (Exception e)
            {
                Donna.Common.Error.WebHandler(e);
            }
            return sbPlain.ToString();
        }

        // 암호화 AES256
        public string EncryptString(string pPlain, string pKey)
        {
            if (pKey.Length != 32)
            {
                Donna.Common.Error.WebHandler(new Exception("Key 사이즈가 올바르지 않습니다."));
            }

            string strEncrypt = string.Empty;

            try
            {
                RijndaelManaged aes = new RijndaelManaged
                {
                    KeySize = 256,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    Key = Encoding.UTF8.GetBytes(pKey),
                    IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(pPlain);
                        cs.Write(bytes, 0, bytes.Length);
                    }

                    buffer = ms.ToArray();
                }

                strEncrypt = Convert.ToBase64String(buffer);
            }
            catch(Exception e)
            {
                Donna.Common.Error.WebHandler(e);
            }

            return strEncrypt;
        }

        // 복호화 AES256
        public string DecryptString(string pEncrypt, string pKey)
        {
            if (pKey.Length != 32)
            {
                Donna.Common.Error.WebHandler(new Exception("Key 사이즈가 올바르지 않습니다."));
            }
            //암호문에 공백이 들어간다면 + 기호로 추가
            pEncrypt = pEncrypt.Replace(" ", "+");

            string strDecrypt = string.Empty;

            try
            {
                RijndaelManaged aes = new RijndaelManaged
                {
                    KeySize = 256,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    Key = Encoding.UTF8.GetBytes(pKey),
                    IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                ICryptoTransform decrypt = aes.CreateDecryptor();
                byte[] buffer = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] bytes = Convert.FromBase64String(pEncrypt);
                        cs.Write(bytes, 0, bytes.Length);
                    }

                    buffer = ms.ToArray();
                }

                strDecrypt = Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Donna.Common.Error.WebHandler(e);
            }

            return strDecrypt;
        }


        //스크립트 문자열 변환
        protected string ReplaceString(string pText)
        {
            string strReturn = pText;

            //strReturn = pText.Replace("&lt;", "<").Replace("&gt;", ">");
            strReturn = strReturn.Replace("&lt;", "<");
            strReturn = strReturn.Replace("&gt;", ">");
            strReturn = strReturn.Replace("&quot;", "\"");
            strReturn = strReturn.Replace("&#39;", "\'");

            return strReturn;
        }

        protected bool IsGuid(string pID)
        {
            return Check.IsGuid(pID);
        }
    }
}