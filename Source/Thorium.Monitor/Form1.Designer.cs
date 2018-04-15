namespace Thorium.Monitor
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
            if(disposing && (components != null))
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
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numFramesPerTask = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numEndFrame = new System.Windows.Forms.NumericUpDown();
            this.numStartFrame = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBlendFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearchDataPackage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataPackagePath = new System.Windows.Forms.TextBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnStartJob = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numServerPort = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServerHost = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFramesPerTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartFrame)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.numFramesPerTask);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtJobName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numEndFrame);
            this.groupBox1.Controls.Add(this.numStartFrame);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBlendFileName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSearchDataPackage);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDataPackagePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 328);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blender Job";
            // 
            // numFramesPerTask
            // 
            this.numFramesPerTask.Location = new System.Drawing.Point(6, 229);
            this.numFramesPerTask.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numFramesPerTask.Name = "numFramesPerTask";
            this.numFramesPerTask.Size = new System.Drawing.Size(120, 20);
            this.numFramesPerTask.TabIndex = 13;
            this.numFramesPerTask.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Frames per Task:";
            // 
            // txtJobName
            // 
            this.txtJobName.Location = new System.Drawing.Point(6, 111);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(211, 20);
            this.txtJobName.TabIndex = 11;
            this.txtJobName.Text = "asdasdsa";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Job Name:";
            // 
            // numEndFrame
            // 
            this.numEndFrame.Location = new System.Drawing.Point(6, 190);
            this.numEndFrame.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numEndFrame.Name = "numEndFrame";
            this.numEndFrame.Size = new System.Drawing.Size(120, 20);
            this.numEndFrame.TabIndex = 9;
            this.numEndFrame.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // numStartFrame
            // 
            this.numStartFrame.Location = new System.Drawing.Point(6, 151);
            this.numStartFrame.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numStartFrame.Name = "numStartFrame";
            this.numStartFrame.Size = new System.Drawing.Size(120, 20);
            this.numStartFrame.TabIndex = 8;
            this.numStartFrame.Value = new decimal(new int[] {
            34,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "End Frame:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Start Frame:";
            // 
            // txtBlendFileName
            // 
            this.txtBlendFileName.Location = new System.Drawing.Point(6, 71);
            this.txtBlendFileName.Name = "txtBlendFileName";
            this.txtBlendFileName.Size = new System.Drawing.Size(211, 20);
            this.txtBlendFileName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Blend File Name:";
            // 
            // btnSearchDataPackage
            // 
            this.btnSearchDataPackage.Location = new System.Drawing.Point(223, 28);
            this.btnSearchDataPackage.Name = "btnSearchDataPackage";
            this.btnSearchDataPackage.Size = new System.Drawing.Size(57, 26);
            this.btnSearchDataPackage.TabIndex = 2;
            this.btnSearchDataPackage.Text = "Search";
            this.btnSearchDataPackage.UseVisualStyleBackColor = true;
            this.btnSearchDataPackage.Click += new System.EventHandler(this.BtnSearchDataPackage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data Package Zip:";
            // 
            // txtDataPackagePath
            // 
            this.txtDataPackagePath.Location = new System.Drawing.Point(6, 32);
            this.txtDataPackagePath.Name = "txtDataPackagePath";
            this.txtDataPackagePath.Size = new System.Drawing.Size(211, 20);
            this.txtDataPackagePath.TabIndex = 0;
            // 
            // btnStartJob
            // 
            this.btnStartJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartJob.Location = new System.Drawing.Point(12, 346);
            this.btnStartJob.Name = "btnStartJob";
            this.btnStartJob.Size = new System.Drawing.Size(57, 26);
            this.btnStartJob.TabIndex = 12;
            this.btnStartJob.Text = "Start Job";
            this.btnStartJob.UseVisualStyleBackColor = true;
            this.btnStartJob.Click += new System.EventHandler(this.BtnStartJob_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.numServerPort);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtServerHost);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(331, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 328);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thorium Server";
            // 
            // numServerPort
            // 
            this.numServerPort.Location = new System.Drawing.Point(6, 72);
            this.numServerPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numServerPort.Name = "numServerPort";
            this.numServerPort.Size = new System.Drawing.Size(120, 20);
            this.numServerPort.TabIndex = 12;
            this.numServerPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Server Port:";
            // 
            // txtServerHost
            // 
            this.txtServerHost.Location = new System.Drawing.Point(6, 32);
            this.txtServerHost.Name = "txtServerHost";
            this.txtServerHost.Size = new System.Drawing.Size(211, 20);
            this.txtServerHost.TabIndex = 12;
            this.txtServerHost.Text = "35.184.234.72";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Server Host:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 384);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStartJob);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFramesPerTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartFrame)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServerPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearchDataPackage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDataPackagePath;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numEndFrame;
        private System.Windows.Forms.NumericUpDown numStartFrame;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBlendFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartJob;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numServerPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtServerHost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numFramesPerTask;
        private System.Windows.Forms.Label label8;
    }
}

