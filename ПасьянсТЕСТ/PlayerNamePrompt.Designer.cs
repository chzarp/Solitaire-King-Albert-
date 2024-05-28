namespace ПасьянсТЕСТ
{
    partial class PlayerNamePrompt
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            playerNameTextBox = new TextBox();
            okButton = new Button();
            SuspendLayout();
            // 
            // playerNameTextBox
            // 
            playerNameTextBox.Location = new Point(12, 12);
            playerNameTextBox.Name = "playerNameTextBox";
            playerNameTextBox.Size = new Size(260, 27);
            playerNameTextBox.TabIndex = 0;
            // 
            // okButton
            // 
            okButton.Location = new Point(197, 38);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += requestNickname;
            // 
            // PlayerNamePrompt
            // 
            ClientSize = new Size(284, 61);
            Controls.Add(okButton);
            Controls.Add(playerNameTextBox);
            Name = "PlayerNamePrompt";
            Text = "Введите ваше имя";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox playerNameTextBox;
        private System.Windows.Forms.Button okButton;
    }
}
