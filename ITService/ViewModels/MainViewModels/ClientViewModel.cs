using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels.MainViewModels
{
    public class ClientViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IUserDataProvider _userDataProvider;
        private IMessageBus _messageBus;
        public ClientViewModel(
            INavigationService navigationService,
            IUserDataProvider userDataProvider,
            IMessageBus messageBus)
        {
            _navigationService = navigationService;
            _userDataProvider = userDataProvider;

            _messageBus = messageBus;

            NavigateToAssetsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<AssetsViewModel>(0); }, canExecute: o => true);
            NavigateToRequestsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<RequestsViewModel>(User.Id); }, canExecute: o => true);
            NavigateToRequestDetailCommand = new DelegateCommand(OnNavigate, canExecute: o => true);
            NavigateToHomeCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ClientHomeViewModel>(User.Id); }, canExecute: o => true);
        }
        public override async Task LoadAsync(int id)
        {
            var user = await _userDataProvider.GetById(id);
            if (user != null)
            {
                User = new UserWrapper(user);
            }
        }
        public DelegateCommand NavigateToAssetsCommand { get; set; }
        public DelegateCommand NavigateToRequestsCommand { get; set; }
        public DelegateCommand NavigateToRequestDetailCommand { get; set; }
        public DelegateCommand NavigateToHomeCommand { get; set; }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigate(object? parameter)
        {
            Navigation.NavigateTo<RequestDetailViewModel>(0);
            _messageBus.Send(User);
        }
    }
}
