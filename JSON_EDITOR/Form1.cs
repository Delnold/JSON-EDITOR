using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Laba4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public List<Champion> champions_list = new List<Champion>();
        public List<Champion> data = new List<Champion>();
        private double timer = 0;

        public void populateDataGrid(List<Champion> champions_list)
        {
            dataGridView1.Rows.Clear();
                
            foreach (var champion in champions_list)
            {
                dataGridView1.Rows.Add(champion.Name, champion.Gender, champion.Passive, champion.Age);
            }
        }
        //Search
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string by_name = textBox1.Text;
                string by_gender = textBox2.Text;
                string by_passive = textBox3.Text;
                dynamic by_age;
                if (textBox4.Text == "")
                {
                    by_age = textBox4.Text;
                }
                else
                { 
                    by_age = Convert.ToInt32(textBox4.Text);
                }

                data = champions_list;
                
                if (by_name != "")
                {
                    data = data.Where(champion => champion.Name.Contains(by_name)).ToList();
                }

                if (by_gender != "")
                {
                    data = data.Where(champion => champion.Gender.Contains(by_gender)).ToList();
                }

                if (by_passive != "")
                {
                    data = data.Where(champion => champion.Passive.Contains(by_passive)).ToList();
                }

                if (by_age is Int32)
                {
                    data = data.Where(champion => champion.Age.Equals(by_age)).ToList();
                }

                if (by_age is "" && by_passive == "" && by_gender == "" && by_name == "")
                {
                    data = champions_list;
                }

                if (data.Count == 0)
                {
                    MessageBox.Show("0 records found!");
                    return;
                }
                populateDataGrid(data);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Invalid input for Age textbox!{exception}");
            }
        }
        //Create
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var champ = new Champion();
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Textbox missing input!");
                    return;
                }
                champ.Name = textBox1.Text;
                champ.Gender = textBox2.Text;
                champ.Passive = textBox3.Text;
                champ.Age = Convert.ToInt32(textBox4.Text);
            
                champions_list.Add(champ);
                data = champions_list;
                populateDataGrid(champions_list);
            }
            catch
            {
                MessageBox.Show("Invalid input for Age textbox!");
            }
        }
        // Delete 
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var cell_index = dataGridView1.CurrentCell.RowIndex;
                champions_list.RemoveAt(cell_index);
                data = champions_list;
                populateDataGrid(champions_list);
            }
            catch
            {
                MessageBox.Show("Can`t delete, there are no rows left! ");
            }
        }
        //Edit
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                var cell_index = dataGridView1.CurrentCell.RowIndex;

                if (textBox1.Text != "")
                {
                    champions_list[cell_index].Name = textBox1.Text;
                }

                if (textBox2.Text != "")
                {
                    champions_list[cell_index].Gender = textBox2.Text;
                }

                if (textBox3.Text != "")
                {
                    champions_list[cell_index].Passive = textBox3.Text;
                }

                if (textBox4.Text != "")
                {
                    champions_list[cell_index].Age = Convert.ToInt32(textBox4.Text);
                }

                data = champions_list;
                populateDataGrid(champions_list);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"There are no rows to edit!");
            }
            catch
            {
                MessageBox.Show("Invalid input for 'Age' textbox!");
            }
            
        }
        //JSON Upload
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "json files (*json)|*.json|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    champions_list = JsonConvert.DeserializeObject<List<Champion>>(File.ReadAllText(ofd.FileName));
                    data = champions_list;
                    populateDataGrid(champions_list);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Invalid JSON file!");
                }
            }
            // dataGridView1.DataSource = championsTable;
        }
        //JSON Download
        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@"E:\University\RiderProjects\JSON_EDITOR\saved.json", JsonConvert.SerializeObject(data));
        }
        // Input 1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        // Input 2
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        // Input 3
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }
        // Input 4
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        // Show current cell
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox5.Text = $"Selected Row: {dataGridView1.CurrentCell.RowIndex + 1}";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Selected not a Row!");
            }
        }
        // Timer
        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer = timer + 0.1;
            int a = Convert.ToInt32(timer);
            textBox6.Text = $"Seconds Elapsed: {a.ToString()}";
        }
        // Help
        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lab4 made by Dmytro Palienko K-28 2022 \n" +
                            "Specifically to type textbox1,2,3 - string, textbox4 - int \n" +
                            "JSON Download: Donwload current table to JSON \n" +
                            "JSON Upload: Convert JSON file to table \n" +
                            "Create: To create a row you have to inset info in all textboxes\n" +
                            "Edit: To edit a row you have to select a row by clicking on one of the Cells, then you insert info in all textboxes, then press Edit \n" +
                            "Delete: Select a row you want to delete, press Delete \n" +
                            "Search: Enter in desirable textboxes values and press Search \n" +
                            "Order by Age: Press it, and it will order by age in in ascending order");
        }
        // Order By Age
        private void button8_Click(object sender, EventArgs e)
        {
            data = data.OrderBy(champion => champion.Age).ToList();
            populateDataGrid(data);
        }
    }
}