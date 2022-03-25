using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace QRPass
{
    public partial class table : UserControl
    {
        private string[] objects = {"Хрячник+ПФ1", "ПФ-2", "ПФ-3", "ТР-1", "ТР-2", "ТР-3", "ТР-4", "ФФО-1", "ФФО-2", "ФФО-3", "ФФО-4", "Крематор", "Убойный цех"};

        public table()
        {
            InitializeComponent();

            FillRecordNo();


            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;


            _form1inc = this;
        }

        public static table _form1inc;

        private void FillRecordNo()
        {
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged_1;

            clearTable();
            try
            {
                Data[] data = MainAsync();

                for(int i = 0; i <objects.Length; i++)
                {
                    dataGridView1.Columns.Add(String.Empty, objects[i]);
                    //populating columns header
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                   
                }

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    string name = dataGridView1.Columns[i].HeaderText;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].HeaderCell.Value = name;

                    if (dataGridView1.Rows[i].Index == dataGridView1.Columns[i].Index)
                    {
                        dataGridView1.Rows[i].Cells[dataGridView1.Columns[i].Index].Style.BackColor = Color.Gray;//setting gray color if column=row
                        dataGridView1.Rows[i].Cells[dataGridView1.Columns[i].Index].ReadOnly = true;
                    }

                }


                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        for (int b = 0; b < data.Length; b++)
                            if (data[b].@object == dataGridView1.Rows[i].HeaderCell.Value + "to" + dataGridView1.Columns[j].HeaderText
                                && int.Parse(data[b].carantine) != 0) {
                                
                                int hours = int.Parse(data[b].carantine);
                                IntegerExtend integerExtend = new IntegerExtend();
                                string noun = integerExtend.Decline(hours, "час", "часа", "часов");
                                dataGridView1.Rows[i].Cells[j].Value = hours +" "+ noun;
                            }
                    }
                }


            }
            finally
            {
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged_1;
                dataGridView1.Height = dataGridView1.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + dataGridView1.ColumnHeadersHeight + 10;
            }

        }

        private void clearTable()
        {
            /*while (dataGridView1.Rows.Count > 1 && dataGridView1.Rows.Count < 13)
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        dataGridView1.Rows.Remove(dataGridView1.Rows[i]);*/
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
        }


        




        private static Data[] MainAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://qrpasscon.ru/");


                HttpResponseMessage response = client.GetAsync("/getobject.php").Result;
                response.EnsureSuccessStatusCode();
                string resultContent = response.Content.ReadAsStringAsync().Result;

                JavaScriptSerializer js = new JavaScriptSerializer();
                Data[] data = js.Deserialize<Data[]>(resultContent);
                return data;
            }
        }





        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            int hours = 0;
            try
            {
                string pattern = @"[^0-9]";
                if ((string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                     hours = int.Parse(Regex.Replace((string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, pattern, String.Empty));
                }
                
                string from = (string)dataGridView1.Rows[e.RowIndex].HeaderCell.Value;
                string to = dataGridView1.Columns[e.ColumnIndex].HeaderText;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://qrpasscon.ru/");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("from", from),
                new KeyValuePair<string, string>("to", to),
              
                new KeyValuePair<string, string>("hours", hours.ToString())
                });
                    HttpResponseMessage result = client.PostAsync("/changequarantine.php", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                }

                FillRecordNo();
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void table_Load(object sender, EventArgs e)
        {

        }

        public class Data
        {
            public string @object { get; set; }
            public string carantine { get; set; }
        }
        
    }
}
