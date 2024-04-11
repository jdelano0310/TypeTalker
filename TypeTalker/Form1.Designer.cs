namespace TypeTalker
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
            this.lblDisplayPressedKey = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDisplayPressedKey
            // 
            this.lblDisplayPressedKey.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplayPressedKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDisplayPressedKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 52F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayPressedKey.Location = new System.Drawing.Point(0, 0);
            this.lblDisplayPressedKey.Name = "lblDisplayPressedKey";
            this.lblDisplayPressedKey.Size = new System.Drawing.Size(749, 577);
            this.lblDisplayPressedKey.TabIndex = 0;
            this.lblDisplayPressedKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisplayPressedKey.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblDisplayPressedKey_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 577);
            this.ControlBox = false;
            this.Controls.Add(this.lblDisplayPressedKey);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Type Talker";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDisplayPressedKey;
    }
}

