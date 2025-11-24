using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StudyThread4._0
{
    public partial class FormWork : Form
    {
        private Timer timer = new Timer();
        public int closeTime { get; set; }
        public int taskID { get; set; }

        public FormWork()
        {
            InitializeComponent();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.closeTime <= 0)
            {
                timer.Stop();
                this.Close();
            }

            this.closeTime -= 100;
            //TextBoxTimer.Text = this.closeTime.ToString();

            TextBoxTimer.BeginInvoke(new Action<TextBox, string>(UpdateUI), new object[] { TextBoxTimer,  this.closeTime.ToString() });
        }

        private void FormWork_Load(object sender, EventArgs e)
        {
            TextBoxTimer.BeginInvoke(new Action<TextBox, string>(UpdateUI), new object[] { TextBoxTimer, this.closeTime.ToString() });
            TextBoxTaskID.BeginInvoke(new Action<TextBox, string>(UpdateUI), new object[] { TextBoxTaskID, this.taskID.ToString() });


            ////1000 ~3000 random
            //TextBoxTimer.Text = this.closeTime.ToString();

            //timer.Interval = 100;
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void UpdateUI(TextBox target, string text)
        {
            target.Text = text;
        }
    }
}
