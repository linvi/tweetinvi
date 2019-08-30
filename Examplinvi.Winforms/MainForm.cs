using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

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
            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            Auth.SetCredentials(creds);

            var client = new TwitterClient(creds);
            var user = client.Users.GetAuthenticatedUser().Result;

            textBox.Text = $"You are now authenticated as {user}!";
        }
    }
}
