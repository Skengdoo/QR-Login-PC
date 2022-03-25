using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRPass
{
    public partial class Form2 : Form
    {
        private Form1 form1;

        public Form2()
        {
            InitializeComponent();
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.Start("https://qrpasscon.ru/update.xml");

           //  MessageBox.Show(GetBoardProductId());
        }


        private static string GetBoardProductId()
        {
            ManagementObjectCollection instances = new ManagementClass("win32_processor").GetInstances();
            string result = string.Empty;
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    result = ((ManagementObject)enumerator.Current).Properties["processorID"].Value.ToString();
                }
            }
            return result;
        }



        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            Hide();
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;
                        dialogResult =
                            MessageBox.Show(
                                $@"Новая версия {args.CurrentVersion} доступна. Вы используете версию {
                                        args.InstalledVersion
                                    }. Хотите ли вы установить новую версию?", @"Доступно обновление!",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    


                    if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                Application.Exit();
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else Application.Exit();
                }
                else
                {
                    Animate();
                }
            }
            else Animate();
        }


        private async void Animate()
        {
            Visible = true;
            backgroundWorker1.RunWorkerAsync();
            while (pictureBox1.Location.X <= pictureBox1.Width)
            { 
                pictureBox1.Location = new Point(pictureBox1.Location.X + 10, pictureBox1.Location.Y);
                await Task.Delay(1);
            }
        }

     

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                form1 = new Form1();
            }
            catch (Exception ex) {
                if (ex.HResult == -2146233088)
                    MessageBox.Show( "Невозможно подключиться к серверу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Hide();
                form1.Show();
            }
            catch (Exception ex) { MessageBox.Show("Произошла непредвиденная ошибка \n" + ex); }
        }




        private void Form2_Load(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
