namespace AudioAnalyzer
{
    partial class Form1
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.drawPan = new System.Windows.Forms.PictureBox();
            this.edit_rate = new System.Windows.Forms.TextBox();
            this.edit_duration = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAnalyze = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.drawPan)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(0, 3);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(76, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // drawPan
            // 
            this.drawPan.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.drawPan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.drawPan.Location = new System.Drawing.Point(0, 29);
            this.drawPan.Name = "drawPan";
            this.drawPan.Size = new System.Drawing.Size(724, 490);
            this.drawPan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.drawPan.TabIndex = 1;
            this.drawPan.TabStop = false;
            // 
            // edit_rate
            // 
            this.edit_rate.Location = new System.Drawing.Point(200, 3);
            this.edit_rate.Name = "edit_rate";
            this.edit_rate.Size = new System.Drawing.Size(100, 20);
            this.edit_rate.TabIndex = 2;
            this.edit_rate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // edit_duration
            // 
            this.edit_duration.Location = new System.Drawing.Point(374, 4);
            this.edit_duration.Name = "edit_duration";
            this.edit_duration.Size = new System.Drawing.Size(100, 20);
            this.edit_duration.TabIndex = 3;
            this.edit_duration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(307, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Hz";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(480, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sec";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(640, 2);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 6;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(724, 519);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edit_duration);
            this.Controls.Add(this.edit_rate);
            this.Controls.Add(this.drawPan);
            this.Controls.Add(this.btnOpen);
            this.Name = "Form1";
            this.Text = "Audio Spectrum Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.drawPan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PictureBox drawPan;
        private System.Windows.Forms.TextBox edit_rate;
        private System.Windows.Forms.TextBox edit_duration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAnalyze;
    }
}

