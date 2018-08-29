namespace CameraViewer
{
    partial class Add_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add_Form));
            this.Template_Input = new System.Windows.Forms.TextBox();
            this.Quantity_Input = new System.Windows.Forms.TextBox();
            this.templateLabel = new System.Windows.Forms.Label();
            this.Quantity = new System.Windows.Forms.Label();
            this.Save_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Label_Input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Template_Input
            // 
            this.Template_Input.BackColor = System.Drawing.SystemColors.Window;
            this.Template_Input.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Template_Input.Location = new System.Drawing.Point(87, 11);
            this.Template_Input.Name = "Template_Input";
            this.Template_Input.Size = new System.Drawing.Size(232, 21);
            this.Template_Input.TabIndex = 0;
            // 
            // Quantity_Input
            // 
            this.Quantity_Input.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Quantity_Input.Location = new System.Drawing.Point(87, 65);
            this.Quantity_Input.Name = "Quantity_Input";
            this.Quantity_Input.Size = new System.Drawing.Size(83, 21);
            this.Quantity_Input.TabIndex = 2;
            this.Quantity_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Quantity_Input_KeyDown);
            // 
            // templateLabel
            // 
            this.templateLabel.AutoSize = true;
            this.templateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.templateLabel.Location = new System.Drawing.Point(15, 14);
            this.templateLabel.Name = "templateLabel";
            this.templateLabel.Size = new System.Drawing.Size(32, 15);
            this.templateLabel.TabIndex = 2;
            this.templateLabel.Text = "URL";
            // 
            // Quantity
            // 
            this.Quantity.AutoSize = true;
            this.Quantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Quantity.Location = new System.Drawing.Point(15, 68);
            this.Quantity.Name = "Quantity";
            this.Quantity.Size = new System.Drawing.Size(51, 15);
            this.Quantity.TabIndex = 3;
            this.Quantity.Text = "Quantity";
            // 
            // Save_Button
            // 
            this.Save_Button.Location = new System.Drawing.Point(244, 92);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(75, 23);
            this.Save_Button.TabIndex = 4;
            this.Save_Button.Text = "Save";
            this.Save_Button.UseVisualStyleBackColor = true;
            this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Location = new System.Drawing.Point(142, 92);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 5;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(15, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Label";
            // 
            // Label_Input
            // 
            this.Label_Input.BackColor = System.Drawing.SystemColors.Window;
            this.Label_Input.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Label_Input.Location = new System.Drawing.Point(87, 38);
            this.Label_Input.Name = "Label_Input";
            this.Label_Input.Size = new System.Drawing.Size(232, 21);
            this.Label_Input.TabIndex = 1;
            // 
            // Add_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(354, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label_Input);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.Save_Button);
            this.Controls.Add(this.Quantity);
            this.Controls.Add(this.templateLabel);
            this.Controls.Add(this.Quantity_Input);
            this.Controls.Add(this.Template_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Add_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Template_Input;
        private System.Windows.Forms.TextBox Quantity_Input;
        private System.Windows.Forms.Label templateLabel;
        private System.Windows.Forms.Label Quantity;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Label_Input;
    }
}