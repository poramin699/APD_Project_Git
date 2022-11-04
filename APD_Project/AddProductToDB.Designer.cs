namespace APD_Project
{
    partial class AddProductToDB
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.button26 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.groupBox17.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.button26);
            this.groupBox17.Controls.Add(this.label2);
            this.groupBox17.Controls.Add(this.textBox15);
            this.groupBox17.Location = new System.Drawing.Point(0, 0);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(355, 98);
            this.groupBox17.TabIndex = 9;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "เพิ่มสินค้า";
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(262, 54);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(75, 25);
            this.button26.TabIndex = 2;
            this.button26.Text = "เพิ่ม";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "รหัสสินค้า";
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(9, 55);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(240, 20);
            this.textBox15.TabIndex = 0;
            // 
            // AddProductToDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox17);
            this.Name = "AddProductToDB";
            this.Size = new System.Drawing.Size(355, 101);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox15;
    }
}
