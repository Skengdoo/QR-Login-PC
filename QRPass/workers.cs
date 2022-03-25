using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace QRPass
{
    public partial class workers : UserControl
    {
        DataTable dt = new DataTable();
        private readonly string[] headers = {"ID", "ФИО", "Должность"};
        HTTP connection = new HTTP();

        public workers()
        {
            InitializeComponent();
            fillTable();
        }

        public void fillTable()
        {

            string resultContent = connection.setRequestUri("/getworkers.php").get();

            List<Workers> users = JsonConvert.DeserializeObject<List<Workers>>(resultContent);
            dt = users.ToTable();

            dataGridView1.DataSource = dt;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = headers[i];//setting column header
            }
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending); //setting first id column auto sorting

            dataGridView1.ScrollBars = ScrollBars.Vertical;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public class Workers
        {
            [JsonProperty ("id")]
            public string id { get; set; }

            [JsonProperty("fullname")]
            public string fullname { get; set; }

            [JsonProperty("position")]
            public string position { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)//adding worker to db
        {
            var t = new TextPrompt(true);
            t.ShowDialog();
            string[] info = t.Value;

            

            if (!t.wasClosed)
            {
                if (!info.All(y => y == "")) //checking if array isn't empty 
                {

                    KeyValuePair<string, string>[] bodyRequest = new[] {
                     new KeyValuePair<string, string>("id", info[0]),
                new KeyValuePair<string, string>("fullname", info[1]),
                new KeyValuePair<string, string>("position", info[2])
                };

                    connection.setRequestUri("/addworker.php").post();

                    fillTable();
                }
                else MessageBox.Show("Пожалуйста, заполните все поля");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            if(dataGridView1.SelectedRows != null) {
                int rowindex = dataGridView1.CurrentCell.RowIndex;//selected user row
                DialogResult dialogResult = MessageBox.Show("Вы уверены, что вы хотите удалить этого пользователя?", "Удаление пользователя", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                { 
                    KeyValuePair<string, string>[] bodyRequest = new[] {
                     new KeyValuePair<string,string>("id", dataGridView1.Rows[rowindex].Cells[0].Value.ToString())//cell[0] - column with worker id
                    };

                    connection.setBodyRequest(bodyRequest).setHeaders("authorization", "YXNkYXNkOmFzZGFzZA==").setRequestUri("/deleteworker.php").post();

                    fillTable();//refresh table
                }
            }
            else MessageBox.Show("Выберите работника из таблицы");
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            if (dataGridView1.SelectedRows != null)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;//selected user row
                string id = (string)dataGridView1.Rows[rowindex].Cells[0].Value;
                string fullname = (string)dataGridView1.Rows[rowindex].Cells[1].Value;
                string position = (string)dataGridView1.Rows[rowindex].Cells[2].Value;

                TextPrompt t = new TextPrompt(false);
                t.setInfo(id, fullname, position);
                t.ShowDialog();
                string[] info = t.Value;
                if (!t.wasClosed)//if form wasn't closed by user
                {
                    if (!info.All(y => y == "")) //checking if array isn't empty 
                    {
                        KeyValuePair<string, string>[] bodyRequest = new[] {
                        new KeyValuePair<string, string>("id", id.ToString()),
                        new KeyValuePair<string, string>("fullname", info[1]),
                        new KeyValuePair<string, string>("position", info[2]),
                        new KeyValuePair<string, string>("newid", info[0])
                };

                        string s = connection.setRequestUri("/updateworker.php").post();

                        fillTable();
                    }
                    else MessageBox.Show("Пожалуйста, заполните все поля");
                }
            }
            else MessageBox.Show("Выберите работника из таблицы");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void changeFilter()
        {
            string filter = "";
            // Check if text fields are not null before adding to filter. 
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (filter.Length > 0) filter += "AND ";
                filter += "id = '" + textBox1.Text + "' ";
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (filter.Length > 0) filter += "AND ";
                filter += "fullname LIKE '%" + textBox2.Text + "%'";
            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                if (filter.Length > 0) filter += "AND ";
                filter += "position LIKE '%" + textBox3.Text + "%'";
            }
            

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }
    }
}
