using ITService.UI.ViewModels.MainViewModels;
namespace ITService.UI.Services
{
    public interface INavigationService
    {
        ViewModel CurrentView { get; set; }
        Task NavigateTo<T>(int Id) where T : ViewModel;
    }
    class NavigationService : ObservableObject, INavigationService
    {
        private readonly Func<Type, ViewModel> _viewModelFactory;
        private ViewModel _currentView;
        public ViewModel CurrentView
        {
            get => _currentView;
            set => Set(ref _currentView, value);
        }
        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        public async Task NavigateTo<TViewModel>(int Id) where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            await viewModel.LoadAsync(Id);
            CurrentView = viewModel;
        }
    }
}
