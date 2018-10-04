using System;
using System.Windows.Forms;
using Library;

namespace WinForms
{
    public partial class WelcomeWindow : Form
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            var text = textBoxName.Text;
            MessageBox.Show(Helper.GenerateMessage(text));
        }
    }
}
