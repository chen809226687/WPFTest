﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();



            comboBox1.SelectedIndex = 0; // 设置第一个选项为默认
            button1.Select();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
             {


                 int i = 0;
                 while (true)
                 {
                     Thread.Sleep(100);
                     this.Invoke(new Action(() =>
                     {
                         label1.Text = i.ToString();
                     }));

                     i++;

                     if (i == 50)
                     {
                         break;
                     }
                 }

             });

            label2.Text = "sssssss";

        }
    }
}
