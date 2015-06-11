using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Crypto4
{
    public partial class Form1 : Form
    {
        OpenFileDialog OpenFile;
        Literature literatureCreate = null;

        public Form1()
        {
            InitializeComponent();
        }

        //вибір функції шифрування чи розшифрування
        private void button1_Click(object sender, EventArgs e) 
        {
            if (radioEncrypt.Checked && textBox1.TextLength != 0 && literatureCreate != null)
            {
                textBox2.Text = literatureCreate.Encrypt(textBox1.Text);
            }
            if (radioDecrypt.Checked && textBox2.TextLength != 0 && literatureCreate != null)
            {
                textBox1.Text = literatureCreate.Decrypt(textBox2.Text);
            }
        }

        //зчитати з файлу
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Text Files|*.txt";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                literatureCreate = new Literature(OpenFile.FileName);
            }
            string[] allLines = File.ReadAllLines(OpenFile.FileName);
            foreach (string l in allLines)
            {
                textBox3.AppendText(l+"\r\n");
            }
            
        }

        //зчитати введений літературний фрагмент
        private void button3_Click(object sender, EventArgs e)
        {
            literatureCreate = new Literature(textBox3.Lines);
        }

        //Зчитати вхідне повідомлення
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Text Files|*.txt";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(OpenFile.FileName);
            }
        }

        public bool TestMetto(string ABC, TextBox textBox)
        {
            bool met = true;
            for (int j = 0; j < textBox.TextLength; j++)
            {
                for (int i = 0; i < ABC.Length; i++)
                {
                    if (textBox.Text[j] == ABC[i])
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioEncrypt.Checked)
            textBox2.ResetText();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioDecrypt.Checked)
            textBox1.ResetText();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
