namespace ПасьянсТЕСТ
{
    partial class Game
    {
        private System.ComponentModel.IContainer components = null;
        private Button startButton;
        private Label timerLabel;

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
            this.startButton = new Button();
            this.timerLabel = new Label();
            this.SuspendLayout();

            // startButton
            this.startButton.Location = new System.Drawing.Point(13, 13);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Начать";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);

            // timerLabel
            this.timerLabel.AutoSize = true;
            this.timerLabel.Location = new System.Drawing.Point(177, 18);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(49, 13);
            this.timerLabel.TabIndex = 2;
            this.timerLabel.Text = "Время: 0:0";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Пасьянс Король Альберт";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
