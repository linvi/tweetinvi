using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button_Click(object sender, System.EventArgs e)
        {
            var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var user = client.Users.GetAuthenticatedUser().Result;

            textBox.Text = $"You are now authenticated as {user}!";
        }
    }
}