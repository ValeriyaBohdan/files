using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Crypto1
{
    public partial class Cezar : Form
    {
        private string ABCEng = " abcdefghijklmnopqrstuvwxyz";
        private string ABCUkr = " абвгдеєжзиіїйклмнопрстуфхцчшщьюя";
        private string numb = "1234567890,.()!@#$%^&*_-+=~`";
        private int step = 0;

        public Cezar()
        {
            InitializeComponent();
        }

        public void CezarMethodFindKey(string ABC, int size)  //дешифровка
        {
            step = 0;
            StringBuilder NewString = new StringBuilder(textBox1.TextLength);
            while ( step < ABC.Length)
            {
                for (int i = 0; i < textBox1.TextLength; i++)
                {
                    for (int j = 0; j < ABC.Length; j++)
                    {
                        if (textBox1.Text[i] == ABC[j])
                        {
                            int position = (j - step) % size;
                            if (position < 0) { position = size + position; }
                            NewString.Append(ABC[position]);
                            break;
                        }
                    }
                }
                NewString.Append("\r\n");
                NewString.Append(" [key=" + (1+step).ToString()+"]  ");
                step++;
            }
            textBox2.Text = NewString.ToString();
        }

        public void CezarMethodEncrypt(string ABC,int size)  //шифрування
        {
            step = Convert.ToInt32(numericUpDown1.Value);
            StringBuilder NewString = new StringBuilder(textBox1.TextLength);
            for (int i = 0; i < textBox1.TextLength; i++)
            {
                for (int j = 0; j < ABC.Length; j++)
                {
                    if (textBox1.Text[i] == ABC[j])
                    {
                        int position = (j + step) % size;
                        NewString.Append(ABC[position]);
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

        public void CezarMethodDecrypt(string ABC, int size) //розшифрування
        {
            step = Convert.ToInt32(numericUpDown1.Value);
            StringBuilder NewString = new StringBuilder(textBox1.TextLength);
            for (int i = 0; i < textBox1.TextLength; i++)
            {
                for (int j = 0; j < ABC.Length; j++)
                {
                    if (textBox1.Text[i] == ABC[j])
                    {
                        int position = (j - step) % size;
                        if (position < 0) { position = size + position; }
                        NewString.Append(ABC[position]);
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

        private void button_Start_Click(object sender, EventArgs e) //вибір мови
        {
            string ABC =null;
            int size = 0;
            if (comboBox1.Text == "Українська") { ABC = ABCUkr; size = 33; numericUpDown1.Maximum = 33; }
            else
            {
                ABC = ABCEng;
                size = 27;
                numericUpDown1.Maximum = 27;
            }
            if (radioButton1.Checked)
            { 
                CezarMethodEncrypt(ABC,size); 
            }
            if (radioButton2.Checked)
            {
                CezarMethodDecrypt(ABC,size);
            }
        }

        private void button_OpenFile_Click(object sender, EventArgs e) //зчитати з файлу
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Text Files|*.txt";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(OpenFile.FileName);
            }
        }

        private void button_FileSave_Click(object sender, EventArgs e) //записати у файл
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

        private void button_FindKey_Click(object sender, EventArgs e)
        {
            CezarMethodFindKey(ABCEng,27);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
