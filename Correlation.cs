using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Net;
using System.Threading;
using excel = Microsoft.Office.Interop.Excel;

namespace DemoExcel
{
    public partial class Form1 : Form
    {
        private Microsoft.Office.Interop.Excel.Application ObjExcel;
        private Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;
        private Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;

        public Form1()
        {
            InitializeComponent();
        }
      
        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS";
            openDialog.ShowDialog();

            try
            {
                ObjExcel = new Microsoft.Office.Interop.Excel.Application();                //книга
                ObjWorkBook = ObjExcel.Workbooks.Open(openDialog.FileName);                 //таблиця
                ObjWorkSheet = ObjExcel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Range rg = null;

                Int32 row = 1;
                dataGridViewMain.Rows.Clear();
                List<String> arr = new List<string>();

                while (ObjWorkSheet.get_Range("a" + row, "a" + row).Value != null)          // Зчитування даних з ячейок
                {
                    rg = ObjWorkSheet.get_Range("a" + row, "f" + row);
                    foreach (Microsoft.Office.Interop.Excel.Range item in rg)
                    {
                        try
                        {
                            arr.Add(item.Value.ToString().Trim());
                        }
                      catch { arr.Add(""); }
                    }

                    dataGridViewMain.Rows.Add(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                    arr.Clear();
                    row++;
                }

                MessageBox.Show("Файл вдало зчитаний !", "Зчитуваня excel файлу", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка при зчитувані excel файлу", MessageBoxButtons.OK, MessageBoxIcon.Error); }


            this.Text = this.Text + " - " + openDialog.SafeFileName;
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            //Середнє значення стопчиків
            excel.Application App = new excel.Application();
            excel.WorksheetFunction Fun = App.WorksheetFunction;
            double[] sersum = new double[6] ;
            double[] ser = new double[6];
            for (int y = 0, x = 0; y < 6 && x<6; y++,x++)
            {
                for (int j = 0; j < 26; j++)
                    sersum[y] += Convert.ToDouble(dataGridViewMain[y, j].Value);
                
                ser[y] = sersum[y] / 26;
                ser[x] = ser[y];
    
            }

            //Корреляція           
            for (int x = 0; x < 6; x++)
            {
                
                for (int y = 0; y < 6; y++)
                {
                    double chiselcorel = 0;
                    double sumakvx = 0;
                    double sumakvy = 0;
              
                    for (int i= 0; i < 26; i++)
                    {
                        chiselcorel += ((Convert.ToDouble(dataGridViewMain[x, i].Value) - ser[x]) * (Convert.ToDouble(dataGridViewMain[y, i].Value)-ser[y]));
                        sumakvx += Math.Pow((Convert.ToDouble(dataGridViewMain[x, i].Value)-ser[x]),2);
                        sumakvy += Math.Pow((Convert.ToDouble(dataGridViewMain[y, i].Value) - ser[y]), 2); 
                    }

                    Console.WriteLine("{0}",sumakvx,sumakvy); 

                    corel[x, y] = chiselcorel / Math.Sqrt(sumakvx * sumakvy);
                    dataGridView1.Rows.Add();
                    dataGridView1[x,y].Value = corel[x, y];
                }
                
            }
            this.dataGridView1.Rows[0].HeaderCell.Value = "Безробітне населення";
            this.dataGridView1.Rows[1].HeaderCell.Value = "Кількість активних підприємств";
            this.dataGridView1.Rows[2].HeaderCell.Value = "Доходи населення";
            this.dataGridView1.Rows[3].HeaderCell.Value = "Зайняте населення";
            this.dataGridView1.Rows[4].HeaderCell.Value = "Чисельність населення";
        }

        private void dataGridViewMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
