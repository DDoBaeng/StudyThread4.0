
namespace StudyThread4._0
{
    partial class FormWork
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBoxTaskID = new System.Windows.Forms.TextBox();
            this.TextBoxTimer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBoxTaskID
            // 
            this.TextBoxTaskID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxTaskID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxTaskID.Location = new System.Drawing.Point(69, 12);
            this.TextBoxTaskID.Multiline = true;
            this.TextBoxTaskID.Name = "TextBoxTaskID";
            this.TextBoxTaskID.Size = new System.Drawing.Size(147, 51);
            this.TextBoxTaskID.TabIndex = 1;
            this.TextBoxTaskID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxTimer
            // 
            this.TextBoxTimer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxTimer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxTimer.Location = new System.Drawing.Point(12, 88);
            this.TextBoxTimer.Multiline = true;
            this.TextBoxTimer.Name = "TextBoxTimer";
            this.TextBoxTimer.Size = new System.Drawing.Size(260, 127);
            this.TextBoxTimer.TabIndex = 1;
            this.TextBoxTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TextBoxTimer);
            this.Controls.Add(this.TextBoxTaskID);
            this.Name = "FormWork";
            this.Text = "FormWork";
            this.Load += new System.EventHandler(this.FormWork_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxTaskID;
        private System.Windows.Forms.TextBox TextBoxTimer;
    }
}