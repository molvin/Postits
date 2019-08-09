namespace PostIts
{
    partial class PostItForm 
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
            this.TextBox = new System.Windows.Forms.RichTextBox();
            this.ExitButton = new System.Windows.Forms.Button();
            this.NewWindowButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox.ForeColor = System.Drawing.Color.Ivory;
            this.TextBox.Location = new System.Drawing.Point(12, 36);
            this.TextBox.MinimumSize = new System.Drawing.Size(10, 10);
            this.TextBox.Name = "TextBox";
            this.TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.TextBox.Size = new System.Drawing.Size(260, 213);
            this.TextBox.TabIndex = 2;
            this.TextBox.Text = "dgsdfgsdfg";
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.DarkGray;
            this.ExitButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ExitButton.Location = new System.Drawing.Point(250, 5);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(25, 25);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.UseVisualStyleBackColor = false;
            // 
            // NewWindowButton
            // 
            this.NewWindowButton.BackColor = System.Drawing.Color.DarkGray;
            this.NewWindowButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.NewWindowButton.Location = new System.Drawing.Point(10, 5);
            this.NewWindowButton.Name = "NewWindowButton";
            this.NewWindowButton.Size = new System.Drawing.Size(25, 25);
            this.NewWindowButton.TabIndex = 3;
            this.NewWindowButton.UseVisualStyleBackColor = false;
            // 
            // PostItForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.NewWindowButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.TextBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(80, 50);
            this.Name = "PostItForm";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.ShowInTaskbar = false;

        }

        #endregion
        private System.Windows.Forms.RichTextBox TextBox;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button NewWindowButton;
    }
}

