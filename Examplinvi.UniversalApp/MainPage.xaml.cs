using Windows.UI.Xaml;
using Examplinvi.UniversalApp.ViewModels;

namespace Examplinvi.UniversalApp
{
    /// <summary>
    /// The main view of the application. 
    /// </summary>
    /// <remarks>Can be used on its own or navigated to within a Frame.</remarks>
    public sealed partial class MainPage
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