using System.Collections.Generic;
using static System.Windows.Forms.ListBox;

namespace PsiComponents
{
    partial class GUI
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
            this.languages = new string[] { "en", "de", "ru", "es" };

            this.speakButton = new System.Windows.Forms.Button();
            this.swapButton = new System.Windows.Forms.Button();
            this.inputLang = new System.Windows.Forms.ListBox();
            this.outputLang = new System.Windows.Forms.ListBox();
            this.TextDisplay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // speakButton
            // 
            this.speakButton.Location = new System.Drawing.Point(298, 361);
            this.speakButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.speakButton.Name = "speakButton";
            this.speakButton.Size = new System.Drawing.Size(244, 85);
            this.speakButton.TabIndex = 0;
            this.speakButton.Text = "Speak";
            this.speakButton.UseVisualStyleBackColor = true;
            this.speakButton.Click += new System.EventHandler(this.SpeakButton_Click);
            // 
            // swapButton
            // 
            this.swapButton.Location = new System.Drawing.Point(372, 145);
            this.swapButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.swapButton.Name = "swapButton";
            this.swapButton.Size = new System.Drawing.Size(112, 35);
            this.swapButton.TabIndex = 1;
            this.swapButton.Text = "Swap";
            this.swapButton.UseVisualStyleBackColor = true;
            this.swapButton.Click += new System.EventHandler(this.SwapButton_Click);
            // 
            // inputLang
            // 
            this.inputLang.FormattingEnabled = true;
            this.inputLang.ItemHeight = 20;
            this.inputLang.Items.AddRange(this.languages);
            this.inputLang.SetSelected(0, true);
            this.inputLang.Location = new System.Drawing.Point(111, 145);
            this.inputLang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inputLang.Name = "inputLang";
            this.inputLang.Size = new System.Drawing.Size(178, 144);
            this.inputLang.TabIndex = 2;
            this.inputLang.Click += new System.EventHandler(this.InputLang_Click);
            // 
            // outputLang
            // 
            this.outputLang.FormattingEnabled = true;
            this.outputLang.ItemHeight = 20;
            this.outputLang.Items.AddRange(this.languages);
            this.outputLang.SetSelected(1, true);
            this.outputLang.Location = new System.Drawing.Point(554, 145);
            this.outputLang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outputLang.Name = "outputLang";
            this.outputLang.Size = new System.Drawing.Size(178, 144);
            this.outputLang.TabIndex = 3;
            this.outputLang.Click += new System.EventHandler(this.OutputLang_Click);
            // 
            // TextDisplay
            // 
            this.TextDisplay.Location = new System.Drawing.Point(308, 209);
            this.TextDisplay.Multiline = true;
            this.TextDisplay.Name = "TextDisplay";
            this.TextDisplay.Size = new System.Drawing.Size(224, 130);
            this.TextDisplay.TabIndex = 4;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 543);
            this.Controls.Add(this.TextDisplay);
            this.Controls.Add(this.speakButton);
            this.Controls.Add(this.swapButton);
            this.Controls.Add(this.inputLang);
            this.Controls.Add(this.outputLang);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GUI";
            this.Text = "PSI Google Translate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private string[] languages;
        private System.Windows.Forms.Button speakButton;
        private System.Windows.Forms.Button swapButton;
        private System.Windows.Forms.ListBox inputLang;
        private System.Windows.Forms.ListBox outputLang;
        private System.Windows.Forms.TextBox TextDisplay;
    }
}