using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Crypto4
{
    class Literature
    {
        private string[] lines = null;
        private string[][] num = null;
        private readonly string path = null;
        
        public Literature(string path)
        {
            this.path = path;
            ReadFileKey(this.path);
        }

        public Literature(string[] text) //зчитування літературного фрагменту для використання в якості ключа
        {
            ReadTextBoxKey(text);
        }

        //Якщо ключ вибрано з файлу
        private void ReadFileKey(string path) //нумеруємо всі стовпчики і рядки обраного ключа присвоюємо їм номера
        {
            lines = System.IO.File.ReadAllLines(path);
            num = new string[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                num[i] = new string[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    num[i][j] = (i + 10).ToString() + (j + 10).ToString();
                }
            }
        }
        //Якщо використовується ключ введений вручну
        private void ReadTextBoxKey(string[] text)
        {
            lines = text;
            num = new string[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                num[i] = new string[lines[i].Length];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    num[i][j] = (i + 10).ToString() + (j + 10).ToString();
                }
            }
        }
        //кожному символу вихідного повідомлення ставимо у відповідність номер
        private List<string> FindAllCharacters(char ch)
        {
            List<string> listOfChar = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (ch == lines[i][j])
                    {
                        listOfChar.Add(num[i][j]);
                    }
                }
            }
            return listOfChar;
        }
        //рандомний вибір символу з ключа шифрування
        private string GetRandomNum(List<string> listOfChar)
        {
            try
            {
                Random rd = new Random(listOfChar.Count);
                int count = 0;
                for (int i = 0; i < listOfChar.Count; i++)
                {
                    count = rd.Next(0, listOfChar.Count);
                }
                return listOfChar[count];
            }
            catch(Exception)
            {
                MessageBox.Show("НЕВІДПОВІДНІСТЬ ЕЛЕМЕНТІВ КЛЮЧА З ВХІДНИМ ПОВІДОМЛЕННЯМ \r\n ПЕРЕВІРТЕ НАДІЙНІСТЬ КЛЮЧА","ПОМИЛКА!");
                return null;
            }
        }

        //номер заноситься до шифрограми
        private char ReturnNum(string st)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (num[i][j] == st)
                        return lines[i][j];
                }
            }
            return char.MaxValue;
        }

        public string Encrypt(string textBox1) //шифрування
        {
            StringBuilder code = new StringBuilder(textBox1.Length * 4);
            for (int i = 0; i < textBox1.Length; i++)
            {
                 if (GetRandomNum(FindAllCharacters(textBox1[i])) == null)
                {
                    code.Append(" ПОМИЛКА! ОПЕРАЦІЮ ПЕРЕРВАНО: символ " + textBox1[i] + " НЕ ВИЗНАЧЕНО");
                    break;
                }
                code.Append(GetRandomNum(FindAllCharacters(textBox1[i])) + " ");
            }
            return code.ToString();
        }

        public string Decrypt(string textBox2) //розшифрування
        {
            StringBuilder message = new StringBuilder();

            for (int i = 0; i < textBox2.Length; i += 5)
            {
                if (ReturnNum(textBox2.Substring(i, 4)) == char.MaxValue) 
                {
                    message.Append(" ПОМИЛКА! НЕ ЗНАЙДЕНО КОДУ CCSS");
                    break;
                }
                message.Append(ReturnNum(textBox2.Substring(i, 4)));
            }
            return message.ToString();
        }
    }
}
