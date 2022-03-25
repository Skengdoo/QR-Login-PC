using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRPass
{
    public partial class programLogView : UserControl
    {
        DataTable dt = new DataTable();
        string[] headers = { "№", "Действие", "Дата"};
        HTTP request = new HTTP();
        public programLogView()
        {
            InitializeComponent();
            fillTable();
            backgroundWorker1.WorkerSupportsCancellation = true;
            circularProgressBar1.Visible = false;
            circularProgressBar1.BackColor = Color.White;
        }

        public void fillTable()
        {

            string resultContent = request.setRequestUri("/getprogramlogs.php").get();

            List<Root> users = JsonConvert.DeserializeObject<List<Root>>(resultContent);
            dt = users.ToTable();

            dataGridView1.DataSource = dt;
            dataGridView1.ScrollBars = ScrollBars.Both;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = headers[i]; //setting column header
            }

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (backgroundWorker1.IsBusy) backgroundWorker1.CancelAsync();
        }

        public class Root
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("action")]
            public string action { get; set; }
            [JsonProperty("date")]
            public string date { get; set; }
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            circularProgressBar1.Value = 0;
            while (!backgroundWorker1.CancellationPending)
            {
                if (circularProgressBar1.Value != 100)
                {
                    circularProgressBar1.Increment(2);
                    System.Threading.Thread.Sleep(1);
                }
                else circularProgressBar1.Value = 0;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            circularProgressBar1.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            circularProgressBar1.Value = e.ProgressPercentage;
        }
    }
}
