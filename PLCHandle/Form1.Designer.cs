namespace PLCHandle
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            button2 = new Button();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            button3 = new Button();
            comboBox1 = new ComboBox();
            label1 = new Label();
            textBox4 = new TextBox();
            button4 = new Button();
            listBox1 = new ListBox();
            button5 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(43, 150);
            button1.Name = "button1";
            button1.Size = new Size(88, 34);
            button1.TabIndex = 0;
            button1.Text = "订阅";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(43, 36);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(283, 23);
            textBox1.TabIndex = 1;
            textBox1.Text = "Application.GVL_PARAM";
            // 
            // button2
            // 
            button2.Location = new Point(43, 210);
            button2.Name = "button2";
            button2.Size = new Size(88, 34);
            button2.TabIndex = 2;
            button2.Text = "读";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(152, 216);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(174, 23);
            textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(152, 275);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(174, 23);
            textBox3.TabIndex = 6;
            // 
            // button3
            // 
            button3.Location = new Point(43, 269);
            button3.Name = "button3";
            button3.Size = new Size(88, 34);
            button3.TabIndex = 5;
            button3.Text = "写";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Bool", "Int", "Byte", "Float", "String" });
            comboBox1.Location = new Point(152, 99);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(174, 25);
            comboBox1.TabIndex = 7;
            comboBox1.SelectedIndexChanged += CheckChange;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(43, 103);
            label1.Name = "label1";
            label1.Size = new Size(78, 21);
            label1.TabIndex = 8;
            label1.Text = "选择类型:";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(152, 156);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(174, 23);
            textBox4.TabIndex = 9;
            // 
            // button4
            // 
            button4.Location = new Point(404, 36);
            button4.Name = "button4";
            button4.Size = new Size(100, 44);
            button4.TabIndex = 10;
            button4.Text = "获取所有";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(539, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(372, 327);
            listBox1.TabIndex = 11;
            // 
            // button5
            // 
            button5.Location = new Point(404, 195);
            button5.Name = "button5";
            button5.Size = new Size(100, 44);
            button5.TabIndex = 12;
            button5.Text = "选择";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 363);
            Controls.Add(button5);
            Controls.Add(listBox1);
            Controls.Add(button4);
            Controls.Add(textBox4);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(textBox3);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Button button2;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button3;
        private ComboBox comboBox1;
        private Label label1;
        private TextBox textBox4;
        private Button button4;
        private ListBox listBox1;
        private Button button5;
    }
}
