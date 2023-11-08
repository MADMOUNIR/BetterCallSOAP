namespace BetterCallSOAP
{
    partial class ListURL
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
            this.lbURL = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbURL
            // 
            this.lbURL.FormattingEnabled = true;
            this.lbURL.Location = new System.Drawing.Point(44, 57);
            this.lbURL.Name = "lbURL";
            this.lbURL.Size = new System.Drawing.Size(672, 342);
            this.lbURL.TabIndex = 0;
            this.lbURL.SelectedIndexChanged += new System.EventHandler(this.lbURL_SelectedIndexChanged);
            this.lbURL.DoubleClick += new System.EventHandler(this.lbURL_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(315, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ListURL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbURL);
            this.Name = "ListURL";
            this.Text = "ListURL";
            this.Load += new System.EventHandler(this.ListURL_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbURL;
        private System.Windows.Forms.Button button1;
    }
}