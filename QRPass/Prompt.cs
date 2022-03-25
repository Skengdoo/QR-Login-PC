using System;
using System.Windows.Forms;

namespace QRPass
{
    public partial class TextPrompt : Form
    {
        private Label lblPromptText;
        private Button button1;
        private TextBox fullnameTextBox;
        private TextBox positionTextBox;
        private Label label1;
        private Label label2;
        private Button button2;
        private TextBox idText;
        string[] worker1 = new string[3];
        bool isClosed = false;

        public string[] Value
        {
            
            get { worker1[0] = idText.Text;
                worker1[1] = fullnameTextBox.Text;
                worker1[2] = positionTextBox.Text;
                return worker1;
            }
        }

        public bool wasClosed {
            get { return isClosed; }
        }

        /// <param name="isAdding">True if we adding new user, false if we editing existing user</param>
        public TextPrompt(bool isAdding)
        {
            InitializeComponent();
            if (isAdding) button1.Text = "Добавить";
            else button1.Text = "Изменить";
        }

        public TextPrompt setInfo(string id, string fullName, string position) {
            idText.Text = id;
            fullnameTextBox.Text = fullName;
            positionTextBox.Text = position;

            return this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fullnameTextBox.Text) && !string.IsNullOrEmpty(idText.Text) && !string.IsNullOrEmpty(positionTextBox.Text))
            {
                Close();
                isClosed = false;
            }
            else MessageBox.Show("Введите все значения!");
           
        }

        private void TextPrompt_Load_1(object sender, EventArgs e)
        {
            CenterToParent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Array.Clear(worker1, 0, worker1.Length);
            isClosed = true;
        }

        private void InitializeComponent()
        {
            this.idText = new System.Windows.Forms.TextBox();
            this.lblPromptText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.fullnameTextBox = new System.Windows.Forms.TextBox();
            this.positionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // idText
            // 
            this.idText.Location = new System.Drawing.Point(40, 99);
            this.idText.Name = "idText";
            this.idText.Size = new System.Drawing.Size(100, 20);
            this.idText.TabIndex = 0;
            // 
            // lblPromptText
            // 
            this.lblPromptText.AutoSize = true;
            this.lblPromptText.Font = new System.Drawing.Font("SF Pro Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPromptText.Location = new System.Drawing.Point(37, 83);
            this.lblPromptText.Name = "lblPromptText";
            this.lblPromptText.Size = new System.Drawing.Size(21, 14);
            this.lblPromptText.TabIndex = 1;
            this.lblPromptText.Text = "ID:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("SF Pro Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(146, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fullnameTextBox
            // 
            this.fullnameTextBox.Location = new System.Drawing.Point(146, 99);
            this.fullnameTextBox.Name = "fullnameTextBox";
            this.fullnameTextBox.Size = new System.Drawing.Size(144, 20);
            this.fullnameTextBox.TabIndex = 3;
            // 
            // positionTextBox
            // 
            this.positionTextBox.Location = new System.Drawing.Point(296, 99);
            this.positionTextBox.Name = "positionTextBox";
            this.positionTextBox.Size = new System.Drawing.Size(165, 20);
            this.positionTextBox.TabIndex = 4;
            this.positionTextBox.TextChanged += new System.EventHandler(this.positionTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SF Pro Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(293, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Должность:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SF Pro Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(143, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "ФИО:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("SF Pro Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(248, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TextPrompt
            // 
            this.ClientSize = new System.Drawing.Size(505, 256);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.positionTextBox);
            this.Controls.Add(this.fullnameTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblPromptText);
            this.Controls.Add(this.idText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TextPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.TextPrompt_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void positionTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
