using Examplinvi.UniversalApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Examplinvi.UAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public StreamViewModel StreamVM;

        public MainPage()
        {
            InitializeComponent();
            StreamVM = new StreamViewModel();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            StreamVM.RunSampleStream();
            //StreamVM.PublishTweet();
        }
    }
}
