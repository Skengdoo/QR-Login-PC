
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace QRPass
{
    public partial class logView : UserControl
    {

        DataTable dt = new DataTable();
        string[] headers = { "№", "ID", "ФИО", "Действие", "Предыдущий объект", "Текущий объект", "Дата" };
        HTTP request = new HTTP();
        System.Timers.Timer timer1 = new System.Timers.Timer();
        private int interval = 300 * 1000;


        public logView()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            fillTable();
            dateTimePicker1.Checked = false; //so filteres won't be enable at the start
            dateTimePicker2.Checked = false;
            circularProgressBar1.Visible = false;
            circularProgressBar1.BackColor = Color.White;
            label8.Text = "Автообновления каждые " + interval/60000 + " минут";
        }

        private void timer() {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    timer();
                });
            }
            else
            {
                timer1.Interval = interval;
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
                timer1.Start();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            timer();
            base.OnLoad(e);
        }

        private void fillTable()
        {
            string resultContent = request.setRequestUri("/getlogs.php").get();

            List<Root> users = JsonConvert.DeserializeObject<List<Root>>(resultContent);
            dt = users.ToTable();

            dataGridView1.DataSource = dt;
            dataGridView1.ScrollBars = ScrollBars.Both;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = headers[i]; //setting column header
            }

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);

                backgroundWorker1.CancelAsync();
        }

        public class Root
        {

            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("user_id")]
            public string user_id { get; set; }
            [JsonProperty("fullname")]
            public string fullname { get; set; }

            [JsonProperty("action")]
            public string action { get; set; }

            [JsonProperty("prevojb")]
            public string prevojb { get; set; }

            [JsonProperty("currentobj")]
            public string currentobj { get; set; }

            [JsonProperty("date")]
            public string date { get; set; }
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
                filter += "user_id = '" + textBox2.Text + "' ";
            }
            if (comboBox1.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
                {
                    if (filter.Length > 0) filter += "AND ";
                    filter += "action = '" + comboBox1.SelectedItem.ToString() + "' ";
                }
            }
            if (comboBox2.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(comboBox2.SelectedItem.ToString()))
                {
                    if (filter.Length > 0) filter += "AND ";
                    filter += "prevojb = '" + comboBox2.SelectedItem.ToString() + "' ";
                }
            }
            if (comboBox3.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(comboBox3.SelectedItem.ToString()))
                {
                    if (filter.Length > 0) filter += "AND ";
                    filter += "currentobj = '" + comboBox3.SelectedItem.ToString() + "' ";
                }
            }
            if (dateTimePicker2.Checked)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy-MM-dd";
                if (dateTimePicker1.Checked)//if both of datetime picked
                {
                    if (filter.Length > 0) filter += "AND ";
                    filter += "date >= #" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "#  AND date <= #"
                        + dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd") + "#";
                }
            }

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }


        /// <summary>
        /// Filtering by number of log
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            changeFilter();
        }


        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeFilter();
        }



        private void button1_Click(object sender, EventArgs e)//reseting datetimepickers
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.Checked = false;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker2.Checked = false;
            changeFilter();

        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            changeFilter();
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.Cells[2].Value.ToString() == "Нарушение сроков карантина")
                    row.DefaultCellStyle.ForeColor = Color.Red;
                else if (row.Cells[2].Value.ToString() == "Выход не зарегестрирован")
                    row.DefaultCellStyle.ForeColor = Color.OrangeRed;

        }

        #region Exporting to excel
        private void button2_Click(object sender, EventArgs e)//Exporting to excel
        {
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = excelapp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

            for (int i = 1; i < dataGridView1.RowCount + 1; i++)
            {
                for (int j = 1; j < dataGridView1.ColumnCount + 1; j++)
                {
                    worksheet.Rows[i + 1].Columns[j] = dataGridView1.Rows[i - 1].Cells[j - 1].Value;
                }
            }
            worksheet.Cells[1, 1] = "reg no";
            worksheet.Cells[1, 2] = "br no";
            worksheet.Cells[1, 3] = "pr no";
            worksheet.Cells[1, 4] = "curency";

            excelapp.AlertBeforeOverwriting = true;
            workbook.SaveAs(@"C:\Users\1\source\repos\QRPass\QRPass\bin\Debug\test.xlsx");

            excelapp.Quit();

        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            circularProgressBar1.Value = 0;
            while (!backgroundWorker1.CancellationPending)
            {
                if (circularProgressBar1.Value != 100)
                {
                    circularProgressBar1.Increment(2);
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    circularProgressBar1.Value = 0;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            circularProgressBar1.Value = e.ProgressPercentage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                circularProgressBar1.Visible = true;
                fillTable();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            circularProgressBar1.Visible = false;
        }
        private  void timer1_Tick(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                fillTable();
            }));
        }

    }
}
