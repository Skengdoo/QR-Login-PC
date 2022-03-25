using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace QRPass
{
    public partial class Form1 : Form
    {

        private bool mouseDown;
        private Point lastLocation;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "Таблица сан.разрывов";

            label2.Text = Application.ProductVersion;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            workers1.Hide();
            logView1.Hide();
            programLogView1.Hide();
            table1.Show();
            label1.Text = "Таблица сан. разрывов";
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            table1.Hide();
            logView1.Hide();
            programLogView1.Hide();
            workers1.Show();
            label1.Text = "Работники";

            workers1.Location = new Point(-300, workers1.Location.Y);

            while (workers1.Location.X <= panel1.Width - 206)
            {
                workers1.Location = new Point(workers1.Location.X - (workers1.Location.X-40) / 8, workers1.Location.Y);
                await Task.Delay(5);
            }
        }


        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            table1.Hide();
            workers1.Hide();
            programLogView1.Hide();
            logView1.Show();

            logView1.Location = new Point(-300, logView1.Location.Y);

            label1.Text = "История";

            while (logView1.Location.X <= panel1.Width - 206)
            {
                logView1.Location = new Point(logView1.Location.X - (logView1.Location.X - 40) / 8, logView1.Location.Y);
                await Task.Delay(5);
            }
        } 
       


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point(
                    Location.X - lastLocation.X + e.X, Location.Y - lastLocation.Y + e.Y);

                Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void button5_ClickAsync(object sender, EventArgs e)
        {
            workers1.Hide();
            logView1.Hide();
            table1.Hide();
            programLogView1.Show();
            label1.Text = "Логирование";

            programLogView1.Location = new Point(-300, programLogView1.Location.Y);

            while (programLogView1.Location.X <= panel1.Width - 206)
            {
                programLogView1.Location = new Point(programLogView1.Location.X
                    - (programLogView1.Location.X - 40) / 8, programLogView1.Location.Y);
                await Task.Delay(5);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
