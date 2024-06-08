using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using System.Windows;

namespace ITService.UI.Views.MainViews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_OnLoaded;
        }
        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync(0);
        }
    }
}