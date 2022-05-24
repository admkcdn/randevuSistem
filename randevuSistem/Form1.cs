using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace randevuSistem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Form2 form2 = new Form2();
        List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> rows2 = new List<Dictionary<string, string>>();
        Dictionary<string, string> column;
        Dictionary<string, string> randevular;
        static string constring = "Data Source=localhost;Initial Catalog=Kisiler;Integrated Security=True"; 
        SqlConnection connect = new SqlConnection(constring);
        string sqlQuery = "SELECT id,adi,soyadi,telefon,randevuTarih FROM Bilgi \n order by randevuTarih";
        private void button1_Click(object sender, EventArgs e)
        {
            try {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                    string tarihsql = "select randevuTarih FROM Bilgi";
                    SqlCommand command = new SqlCommand(tarihsql, connect);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        randevular = new Dictionary<string, string>();
                        randevular["randevuTarih"] = reader["randevuTarih"].ToString();
                        rows2.Add(randevular);
                    }
                    reader.Close();

                    int counter = 0;

                    foreach(Dictionary<string,string> tarihler in rows2)
                    {
                        if (tarihler["randevuTarih"].Remove(10) == dateTimePicker1.Value.Date.ToString().Remove(10))
                            counter++;
                    }

                    if (counter == 0)
                        { 
                        string kayit = "insert into Bilgi (adi,soyadi,telefon,randevuTarih) values (@ad,@soyad,@telefon,@tarih)";
                        SqlCommand cmd = new SqlCommand(kayit, connect);
                        cmd.Parameters.AddWithValue("@ad", textBox1.Text);
                        cmd.Parameters.AddWithValue("@soyad", textBox2.Text);
                        cmd.Parameters.AddWithValue("@telefon", textBox3.Text);
                        cmd.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);

                        cmd.ExecuteNonQuery();

                        connect.Close();
                        MessageBox.Show("Başarıyla Randevu alındı!");
                        }
                    else
                    {
                        MessageBox.Show("Aynı güne birden fazla randevu verilemez!");
                        connect.Close();
                    }
                    }
                }
            catch(Exception err) {
                MessageBox.Show("Hata: "+err);
                connect.Close();
                }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(sqlQuery, connect);
            try
            {
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {  
                    column = new Dictionary<string, string>();

                    column["id"] = reader["id"].ToString();
                    column["adi"] = reader["adi"].ToString();
                    column["soyadi"] = reader["soyadi"].ToString();
                    column["telefon"] = reader["telefon"].ToString();
                    column["randevuTarih"] = reader["randevuTarih"].ToString();

                    rows.Add(column);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hata: "+ ex);
            }
            finally
            {
                connect.Close();
            }


            foreach (Dictionary<string, string> column in rows)
            {
                form2.listBox1.Items.Add(column["id"] + "\t" + column["adi"] + "\t" + column["soyadi"] + "\t" + column["telefon"] + "\t" + column["randevuTarih"].Remove(10));
            }
            form2.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ademkocdogan.wordpress.com/");
        }
    }
    }

