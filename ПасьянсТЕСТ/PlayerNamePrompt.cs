using System;
using System.Windows.Forms;

namespace ПасьянсТЕСТ
{
    public partial class PlayerNamePrompt : Form
    {
        public string PlayerName { get; private set; }

        public PlayerNamePrompt()
        {
            InitializeComponent();
        }

        private void requestNickname(object sender, EventArgs e)
        {
            PlayerName = playerNameTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }
}
