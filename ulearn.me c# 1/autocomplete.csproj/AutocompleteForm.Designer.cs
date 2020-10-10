﻿namespace Autocomplete
{
	partial class AutocompleteForm
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
			this.inputBox = new System.Windows.Forms.TextBox();
			this.autocompleteList = new System.Windows.Forms.ListBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputBox
			// 
			this.inputBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.inputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.inputBox.Location = new System.Drawing.Point(0, 0);
			this.inputBox.Margin = new System.Windows.Forms.Padding(0);
			this.inputBox.Name = "inputBox";
			this.inputBox.Size = new System.Drawing.Size(596, 34);
			this.inputBox.TabIndex = 0;
			this.inputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
			// 
			// autocompleteList
			// 
			this.autocompleteList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.autocompleteList.FormattingEnabled = true;
			this.autocompleteList.ItemHeight = 17;
			this.autocompleteList.Location = new System.Drawing.Point(0, 34);
			this.autocompleteList.Margin = new System.Windows.Forms.Padding(0);
			this.autocompleteList.Name = "autocompleteList";
			this.autocompleteList.Size = new System.Drawing.Size(596, 309);
			this.autocompleteList.TabIndex = 1;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 318);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
			this.statusStrip1.Size = new System.Drawing.Size(596, 25);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(139, 20);
			this.statusLabel.Text = "please start typing";
			// 
			// AutocompleteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(596, 343);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.autocompleteList);
			this.Controls.Add(this.inputBox);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "AutocompleteForm";
			this.Text = "Start typing";
			this.Load += new System.EventHandler(this.AutocompleteForm_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.ListBox autocompleteList;
		private System.Windows.Forms.TextBox inputBox;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.StatusStrip statusStrip1;

		#endregion
	}
}

