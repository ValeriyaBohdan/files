using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Crypto2
{
    public partial class Tritemius : Form
    {
        private string ABCEng = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.()!@#$%^&*_-+=~`";
        private string ABCUkr = " абвгдеєжзиіїйклмнопрстуфхцчшщьюяАБВГДЕЄЖЗИІЇКЛМНОПРСТУФХЦЧШЩЬЮЯ1234567890,.()!@#$%^&*_-+=~`";
        private static int num = 0;
        private int a;
        private int b;
        private int c;
        private bool tr = true;

        public Tritemius()
        {
            InitializeComponent();
        }

        public void TritemiumEncrypt(string ABC) //Шифрування
        {
            int CharCode = 0;
            int key = 0;
            StringBuilder NewString = new StringBuilder(textBox1.TextLength);

            for (int i = 0; i < textBox1.TextLength; i++)
            {
                for (int j = 0; j < ABC.Length; j++)
                {
                    if (textBox1.Text[i] == ABC[j]) 
                    {
                        key = Calckey(i,ABC) % ABC.Length;
                        CharCode = Math.Abs((ABC.Length + j + key) % ABC.Length);
                        NewString.Append(ABC[CharCode]);
                        break;
                    }
                    else
                    {
                        if (j == ABC.Length - 1) { NewString.Append(textBox1.Text[i]); break; }
                    }
                }
            }
            textBox2.ResetText();
            textBox2.Text = NewString.ToString();
            NewString.Clear(); 
        }

        public void TritemiumDecrypt(string ABC) //Розшифрування
        {
            int CharCode = 0;
            int key = 0;
            StringBuilder NewString = new StringBuilder(textBox1.TextLength);

            for (int i = 0; i < textBox1.TextLength; i++)
            {
                for (int j = 0; j < ABC.Length; j++)
                {
                    if (textBox1.Text[i] == ABC[j])
                    {
                        key = Calckey(i,ABC) % ABC.Length;
                        CharCode = Math.Abs((ABC.Length + j - key) % ABC.Length);
                        NewString.Append(ABC[CharCode]);
                        break;
                    }
                    else
                    {
                        if (j == ABC.Length - 1) { NewString.Append(textBox1.Text[i]); break; }
                    }
                }
            }
            textBox2.ResetText();
            textBox2.Text = NewString.ToString();
            NewString.Clear(); 
        }

        public int Calckey(int i,string ABC) //обчислення кроку зміщення ключа
        {
                if (radioButton1.Checked) //лінійне рівняння
                {
                    return Convert.ToInt32((a * i + b));
                }
                if (radioButton2.Checked)
                {
                    return Convert.ToInt32((a * Math.Pow(i, 2) + b * i + c));  //нелінійне рівняння
                }
                if (radioButton3.Checked) //використання гасла(текстового рядка)
                {
                    int[] CodeChars = new int[textBox6.TextLength];
                    int count = 0;
                    for (int j = 0; j < ABC.Length; j++)
                    {
                        if (textBox6.Text[count] == ABC[j] && count < textBox6.TextLength - 1)
                        {
                            CodeChars[count] = j;
                            count++;
                        }
                    }
                    if (num > CodeChars.Length) { num = 0; }
                    return (i + CodeChars[num]);
                    num++;
                }
                else
                {
                    return Int32.MaxValue;
                }
        }

        public bool TestMetto(string ABC) //текстовий рядок записується декілька разів, довжина гасла має відповідати довжині повідомлення
        {
            bool met = true;
            for (int j = 0; j < textBox6.TextLength; j++)
            {
                for (int i = 0; i < ABC.Length; i++)
                {
                    if (textBox6.Text[j] == ABC[i]) 
                    { 
                        met = true; 
                    }
                    else 
                    {
                        if (i == ABC.Length)
                        {
                            met = false;
                            return met;
                        }
                    }
                }
            }
            return met;
        }

        private void button1_Click_Start(object sender, EventArgs e) //початок виконання операцій обраних користувачем
        {
            try
            {
                a = Convert.ToInt32(textBox3.Text);
                b = Convert.ToInt32(textBox4.Text);
                c = Convert.ToInt32(textBox5.Text);
            }
            catch (FormatException f) 
            {
                MessageBox.Show(f.ToString());
                tr = false;
                textBox2.Text = "ПОМИЛКА ВВОДУ ПОЧАТКОВИХ УМОВ (некоректний формат)!!!";
            }
            catch (OverflowException  our)
            {
                MessageBox.Show(our.ToString());
                tr = false;
                textBox2.Text = "ПОМИЛКА ВВОДУ ДАНИХ КОРИСТУВАЧА (Аргумент за межами допустимого)!!!";
            }

            if (comboBox2.Text == "Англійська" && comboBox1.Text == "Зашифрувати" && tr && TestMetto(ABCEng)) 
            {
                TritemiumEncrypt(ABCEng);
            }
            if (comboBox2.Text == "Англійська" && comboBox1.Text == "Розшифрувати" && tr && TestMetto(ABCEng))
            {
                TritemiumDecrypt(ABCEng);
            }
            if (comboBox2.Text == "Українська" && comboBox1.Text == "Зашифрувати" && tr && TestMetto(ABCUkr))
            {
                TritemiumEncrypt(ABCUkr);
            }
            if (comboBox2.Text == "Українська" && comboBox1.Text == "Розшифрувати" && tr && TestMetto(ABCUkr))
            {
                TritemiumDecrypt(ABCUkr);
            }
            tr = true;
        }

        private void button3_Click_OpenFile(object sender, EventArgs e) //зчитати вхідне повідомлення з файлу
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Text Files|*.txt";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(OpenFile.FileName);
            }
        }

        private void button2_Click_SaveFile(object sender, EventArgs e) //зберегти результат в файл
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Filter = "Text File|*.txt";
            SaveFile.ShowDialog();
            if (SaveFile.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveFile.OpenFile();
                StreamWriter st = new StreamWriter(fs, Encoding.UTF8);
                st.WriteLine(textBox2.Text);
                st.Dispose();
                fs.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        

    }
}
