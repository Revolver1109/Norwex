namespace EDITranslator
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
            this.lblFileToProcess = new System.Windows.Forms.Label();
            this.lstbxFileToProcess = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chbHPS = new System.Windows.Forms.CheckBox();
            this.chbHealthSprings = new System.Windows.Forms.CheckBox();
            this.grpbxFileType = new System.Windows.Forms.GroupBox();
            this.chbPekin = new System.Windows.Forms.CheckBox();
            this.chbBoon = new System.Windows.Forms.CheckBox();
            this.grpbxFileType.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFileToProcess
            // 
            this.lblFileToProcess.AutoSize = true;
            this.lblFileToProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileToProcess.Location = new System.Drawing.Point(52, 71);
            this.lblFileToProcess.Name = "lblFileToProcess";
            this.lblFileToProcess.Size = new System.Drawing.Size(198, 15);
            this.lblFileToProcess.TabIndex = 4;
            this.lblFileToProcess.Text = "Drop File To Process In Box Below.";
            // 
            // lstbxFileToProcess
            // 
            this.lstbxFileToProcess.AllowDrop = true;
            this.lstbxFileToProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbxFileToProcess.FormattingEnabled = true;
            this.lstbxFileToProcess.ItemHeight = 15;
            this.lstbxFileToProcess.Location = new System.Drawing.Point(55, 89);
            this.lstbxFileToProcess.Name = "lstbxFileToProcess";
            this.lstbxFileToProcess.Size = new System.Drawing.Size(287, 94);
            this.lstbxFileToProcess.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(52, 204);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 5;
            // 
            // chbHPS
            // 
            this.chbHPS.AutoSize = true;
            this.chbHPS.Location = new System.Drawing.Point(3, 26);
            this.chbHPS.Name = "chbHPS";
            this.chbHPS.Size = new System.Drawing.Size(48, 17);
            this.chbHPS.TabIndex = 6;
            this.chbHPS.Text = "HPS";
            this.chbHPS.UseVisualStyleBackColor = true;
            // 
            // chbHealthSprings
            // 
            this.chbHealthSprings.AutoSize = true;
            this.chbHealthSprings.Location = new System.Drawing.Point(54, 26);
            this.chbHealthSprings.Name = "chbHealthSprings";
            this.chbHealthSprings.Size = new System.Drawing.Size(92, 17);
            this.chbHealthSprings.TabIndex = 7;
            this.chbHealthSprings.Text = "HealthSprings";
            this.chbHealthSprings.UseVisualStyleBackColor = true;
            // 
            // grpbxFileType
            // 
            this.grpbxFileType.Controls.Add(this.chbBoon);
            this.grpbxFileType.Controls.Add(this.chbPekin);
            this.grpbxFileType.Controls.Add(this.chbHPS);
            this.grpbxFileType.Controls.Add(this.chbHealthSprings);
            this.grpbxFileType.Location = new System.Drawing.Point(55, 8);
            this.grpbxFileType.Name = "grpbxFileType";
            this.grpbxFileType.Size = new System.Drawing.Size(287, 49);
            this.grpbxFileType.TabIndex = 8;
            this.grpbxFileType.TabStop = false;
            this.grpbxFileType.Text = "File Type";
            // 
            // chbPekin
            // 
            this.chbPekin.AutoSize = true;
            this.chbPekin.Location = new System.Drawing.Point(154, 26);
            this.chbPekin.Name = "chbPekin";
            this.chbPekin.Size = new System.Drawing.Size(53, 17);
            this.chbPekin.TabIndex = 8;
            this.chbPekin.Text = "Pekin";
            this.chbPekin.UseVisualStyleBackColor = true;
            // 
            // chbBoon
            // 
            this.chbBoon.AutoSize = true;
            this.chbBoon.Location = new System.Drawing.Point(210, 26);
            this.chbBoon.Name = "chbBoon";
            this.chbBoon.Size = new System.Drawing.Size(80, 17);
            this.chbBoon.TabIndex = 9;
            this.chbBoon.Text = "BoonGroup";
            this.chbBoon.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(413, 262);
            this.Controls.Add(this.grpbxFileType);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lstbxFileToProcess);
            this.Controls.Add(this.lblFileToProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Careington EDI File Reader";
            this.grpbxFileType.ResumeLayout(false);
            this.grpbxFileType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileToProcess;
        private System.Windows.Forms.ListBox lstbxFileToProcess;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chbHPS;
        private System.Windows.Forms.CheckBox chbHealthSprings;
        private System.Windows.Forms.GroupBox grpbxFileType;
        private System.Windows.Forms.CheckBox chbPekin;
        private System.Windows.Forms.CheckBox chbBoon;
    }
}

