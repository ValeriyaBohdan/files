using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Cryptosystem
{
    interface GetResult
    {
        string ShowResult();
    };
    class BaseCryptClass : GetResult
    {
        protected string alphabet { get; set;}
        protected string inputText { get; set;}
        protected string resultString { get; set; }
        protected bool cryptFlag { get; set; }
        public BaseCryptClass(string alph, string text)
        {
            alphabet = alph;
            inputText = text;
        }
        public string CryptMaker(string input, bool flag)
        {
            return resultString;
        }
        public string ShowResult()
        {
            return CryptMaker(inputText, cryptFlag);
        }
    }

    class DES : BaseCryptClass, GetResult
    {
       protected DESCryptoServiceProvider cryptic;
       protected CryptoStream crStream;
       protected FileStream stream_in, stream_out;
       protected StreamReader reader;
       protected byte[] data;
       protected string desKey, desVect;
       public DES(string alph, string text, bool flag, string key, string in_vect)
           : base(alph, text)
       {
           alphabet = alph;
           inputText = text;
           cryptFlag = flag;
           desKey = key;
           desVect = in_vect;
           System.Text.Encoding ASCIIEncoding = Encoding.GetEncoding("UTF-8");
           data = ASCIIEncoding.GetBytes(inputText);

           //створюється криптопровайдер, задаються ключ та вектор
           cryptic = new DESCryptoServiceProvider();
           cryptic.Key = ASCIIEncoding.GetBytes(desKey);
           cryptic.IV = ASCIIEncoding.GetBytes(desVect);

       }
       
       private string DesMaker()
       {
           string path = @"d:\test.txt";
           resultString = "";
           //шифрування
           if (cryptFlag==true)
           {
               stream_in = new FileStream(path, FileMode.Create, FileAccess.Write);
               crStream = new CryptoStream(stream_in, cryptic.CreateEncryptor(), CryptoStreamMode.Write);
               crStream.Write(data, 0, data.Length);
               crStream.Close();
               stream_in.Close();
               resultString = File.ReadAllText(path);
           }

           //розшифрування
           if (cryptFlag==false)
           {
               stream_out = new FileStream(path, FileMode.Open, FileAccess.Read);
               crStream = new CryptoStream(stream_out, cryptic.CreateDecryptor(), CryptoStreamMode.Read);
               reader = new StreamReader(crStream);
               string uncrypt = reader.ReadToEnd();
               resultString = uncrypt;
               crStream.Close();
               reader.Close();
               stream_out.Close();
           }
           return resultString;
       }
       public string ShowResult()
       {
           return DesMaker();
       }
    }
}
