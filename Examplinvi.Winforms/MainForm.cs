using System.Diagnostics;
using System.Windows.Forms;
using Tweetinvi;

namespace Examplinvi.Winforms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, System.EventArgs e)
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            var user = User.GetAuthenticatedUser();

            textBox.Text = string.Format("You are now authenticated as {0}!", user);
        }
    }
}
