using Examplinvi.WPF.ViewModels;
using System.Windows;

namespace Examplinvi.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StreamViewModel StreamVM { get; set; }
        public string Plop { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            StreamVM = new StreamViewModel();
            StreamVM.Authenticate();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            StreamVM.RunSampleStream();
        }
    }
}
