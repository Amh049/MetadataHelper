namespace Main
{
    partial class MainForm
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
            this.txtTemplatePath = new System.Windows.Forms.TextBox();
            this.ofdTemplatePath = new System.Windows.Forms.OpenFileDialog();
            this.btnTemplatePath = new System.Windows.Forms.Button();
            this.btnDataSource = new System.Windows.Forms.Button();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ofdDataSource = new System.Windows.Forms.OpenFileDialog();
            this.btnSelectOutput = new System.Windows.Forms.Button();
            this.txtSelectOutput = new System.Windows.Forms.TextBox();
            this.fbdOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTemplatePath
            // 
            this.txtTemplatePath.Location = new System.Drawing.Point(12, 68);
            this.txtTemplatePath.Name = "txtTemplatePath";
            this.txtTemplatePath.Size = new System.Drawing.Size(343, 23);
            this.txtTemplatePath.TabIndex = 0;
            // 
            // btnTemplatePath
            // 
            this.btnTemplatePath.Location = new System.Drawing.Point(12, 12);
            this.btnTemplatePath.Name = "btnTemplatePath";
            this.btnTemplatePath.Size = new System.Drawing.Size(343, 50);
            this.btnTemplatePath.TabIndex = 1;
            this.btnTemplatePath.Text = "Select XML Template";
            this.btnTemplatePath.UseVisualStyleBackColor = true;
            this.btnTemplatePath.Click += new System.EventHandler(this.btnTemplatePath_Click);
            // 
            // btnDataSource
            // 
            this.btnDataSource.Location = new System.Drawing.Point(12, 110);
            this.btnDataSource.Name = "btnDataSource";
            this.btnDataSource.Size = new System.Drawing.Size(343, 50);
            this.btnDataSource.TabIndex = 3;
            this.btnDataSource.Text = "Select Metadata Source";
            this.btnDataSource.UseVisualStyleBackColor = true;
            this.btnDataSource.Click += new System.EventHandler(this.btnDataSource_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(12, 166);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(343, 23);
            this.txtDataSource.TabIndex = 2;
            // 
            // btnSelectOutput
            // 
            this.btnSelectOutput.Location = new System.Drawing.Point(12, 208);
            this.btnSelectOutput.Name = "btnSelectOutput";
            this.btnSelectOutput.Size = new System.Drawing.Size(343, 50);
            this.btnSelectOutput.TabIndex = 5;
            this.btnSelectOutput.Text = "Select Output Folder";
            this.btnSelectOutput.UseVisualStyleBackColor = true;
            this.btnSelectOutput.Click += new System.EventHandler(this.btnSelectOutput_Click);
            // 
            // txtSelectOutput
            // 
            this.txtSelectOutput.Location = new System.Drawing.Point(12, 264);
            this.txtSelectOutput.Name = "txtSelectOutput";
            this.txtSelectOutput.Size = new System.Drawing.Size(343, 23);
            this.txtSelectOutput.TabIndex = 4;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 298);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(343, 58);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 368);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnSelectOutput);
            this.Controls.Add(this.txtSelectOutput);
            this.Controls.Add(this.btnDataSource);
            this.Controls.Add(this.txtDataSource);
            this.Controls.Add(this.btnTemplatePath);
            this.Controls.Add(this.txtTemplatePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "NV5 Excel to XML Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtTemplatePath;
        private OpenFileDialog ofdTemplatePath;
        private Button btnTemplatePath;
        private Button btnDataSource;
        private TextBox txtDataSource;
        private OpenFileDialog openFileDialog1;
        private OpenFileDialog ofdDataSource;
        private Button btnSelectOutput;
        private TextBox txtSelectOutput;
        private FolderBrowserDialog fbdOutput;
        private Button btnRun;
    }
}