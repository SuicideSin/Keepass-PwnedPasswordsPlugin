namespace PwnedPasswordsPlugin
{
    partial class PreferencesForm
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
            this.expireOnPwn = new System.Windows.Forms.CheckBox();
            this.addNoteOnPwn = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.checkAllNowButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // expireOnPwn
            // 
            this.expireOnPwn.AutoSize = true;
            this.expireOnPwn.Location = new System.Drawing.Point(12, 12);
            this.expireOnPwn.Name = "expireOnPwn";
            this.expireOnPwn.Size = new System.Drawing.Size(209, 17);
            this.expireOnPwn.TabIndex = 1;
            this.expireOnPwn.Text = "Expire password entry when breached.";
            this.expireOnPwn.UseVisualStyleBackColor = true;
            // 
            // addNoteOnPwn
            // 
            this.addNoteOnPwn.AutoSize = true;
            this.addNoteOnPwn.Location = new System.Drawing.Point(12, 36);
            this.addNoteOnPwn.Name = "addNoteOnPwn";
            this.addNoteOnPwn.Size = new System.Drawing.Size(235, 17);
            this.addNoteOnPwn.TabIndex = 2;
            this.addNoteOnPwn.Text = "Add note to password entry when breached.";
            this.addNoteOnPwn.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(197, 74);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(116, 74);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // checkAllNowButton
            // 
            this.checkAllNowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAllNowButton.Location = new System.Drawing.Point(21, 74);
            this.checkAllNowButton.Name = "checkAllNowButton";
            this.checkAllNowButton.Size = new System.Drawing.Size(89, 23);
            this.checkAllNowButton.TabIndex = 5;
            this.checkAllNowButton.Text = "Check All Now";
            this.checkAllNowButton.UseVisualStyleBackColor = true;
            this.checkAllNowButton.Click += new System.EventHandler(this.checkAllNowButton_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 109);
            this.Controls.Add(this.checkAllNowButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addNoteOnPwn);
            this.Controls.Add(this.expireOnPwn);
            this.Name = "PreferencesForm";
            this.Text = "Pwned Passwords Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox expireOnPwn;
        private System.Windows.Forms.CheckBox addNoteOnPwn;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button checkAllNowButton;
    }
}