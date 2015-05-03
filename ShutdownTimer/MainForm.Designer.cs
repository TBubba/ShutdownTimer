namespace ShutdownTimer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._bStart = new System.Windows.Forms.Button();
            this._cbShutdownMode = new System.Windows.Forms.ComboBox();
            this._lTimeMode = new System.Windows.Forms.Label();
            this._lShutdownMode = new System.Windows.Forms.Label();
            this._rtbTime = new System.Windows.Forms.RichTextBox();
            this._cbTimeMode = new ShutdownTimer.FakeComboBox();
            this.SuspendLayout();
            // 
            // _bStart
            // 
            this._bStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._bStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._bStart.Location = new System.Drawing.Point(2, 102);
            this._bStart.Name = "_bStart";
            this._bStart.Size = new System.Drawing.Size(201, 44);
            this._bStart.TabIndex = 3;
            this._bStart.Text = "Start";
            this._bStart.UseVisualStyleBackColor = true;
            this._bStart.Click += new System.EventHandler(this._bStart_Click);
            // 
            // _cbShutdownMode
            // 
            this._cbShutdownMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbShutdownMode.FormattingEnabled = true;
            this._cbShutdownMode.Location = new System.Drawing.Point(3, 75);
            this._cbShutdownMode.Name = "_cbShutdownMode";
            this._cbShutdownMode.Size = new System.Drawing.Size(98, 21);
            this._cbShutdownMode.TabIndex = 1;
            this._cbShutdownMode.SelectedIndexChanged += new System.EventHandler(this._cbShutdownMode_SelectedIndexChanged);
            // 
            // _lTimeMode
            // 
            this._lTimeMode.Location = new System.Drawing.Point(105, 60);
            this._lTimeMode.Name = "_lTimeMode";
            this._lTimeMode.Size = new System.Drawing.Size(98, 13);
            this._lTimeMode.TabIndex = 9;
            this._lTimeMode.Text = "Time mode:";
            // 
            // _lShutdownMode
            // 
            this._lShutdownMode.Location = new System.Drawing.Point(3, 59);
            this._lShutdownMode.Name = "_lShutdownMode";
            this._lShutdownMode.Size = new System.Drawing.Size(98, 13);
            this._lShutdownMode.TabIndex = 10;
            this._lShutdownMode.Text = "Shutdown mode:";
            // 
            // _rtbTime
            // 
            this._rtbTime.BackColor = System.Drawing.SystemColors.Menu;
            this._rtbTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._rtbTime.DetectUrls = false;
            this._rtbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._rtbTime.Location = new System.Drawing.Point(3, 1);
            this._rtbTime.MaxLength = 8;
            this._rtbTime.Multiline = false;
            this._rtbTime.Name = "_rtbTime";
            this._rtbTime.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this._rtbTime.Size = new System.Drawing.Size(200, 55);
            this._rtbTime.TabIndex = 0;
            this._rtbTime.Text = "00:05:00";
            // 
            // _cbTimeMode
            // 
            this._cbTimeMode.BackColor = System.Drawing.SystemColors.Window;
            this._cbTimeMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbTimeMode.ForeColor = System.Drawing.SystemColors.WindowText;
            this._cbTimeMode.FormattingEnabled = true;
            this._cbTimeMode.Location = new System.Drawing.Point(105, 76);
            this._cbTimeMode.Name = "_cbTimeMode";
            this._cbTimeMode.Size = new System.Drawing.Size(98, 21);
            this._cbTimeMode.TabIndex = 2;
            this._cbTimeMode.SelectedIndexChanged += new System.EventHandler(this._cbTimeMode_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 147);
            this.Controls.Add(this._cbTimeMode);
            this.Controls.Add(this._rtbTime);
            this.Controls.Add(this._lShutdownMode);
            this.Controls.Add(this._lTimeMode);
            this.Controls.Add(this._cbShutdownMode);
            this.Controls.Add(this._bStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Shutdown Timer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _bStart;
        private System.Windows.Forms.ComboBox _cbShutdownMode;
        private System.Windows.Forms.Label _lTimeMode;
        private System.Windows.Forms.Label _lShutdownMode;
        private System.Windows.Forms.RichTextBox _rtbTime;
        private FakeComboBox _cbTimeMode;
    }
}

