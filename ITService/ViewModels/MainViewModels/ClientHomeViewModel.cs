using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels.MainViewModels
{
    public class ClientHomeViewModel : ViewModel
    {
        private IUserDataProvider _userDataProvider;
        private IClientDataProvider _clientDataProvider;
        private INavigationService _navigationService;
        private IMessageBus _messageBus;
        private ClientWrapper _client;
        private UserWrapper _user;

        public ClientHomeViewModel(
            IUserDataProvider userDataProvider,
            IClientDataProvider clientDataProvider,
            INavigationService navigationService,
            IMessageBus messageBus)
        {
            _userDataProvider = userDataProvider;
            _clientDataProvider = clientDataProvider;
            _navigationService = navigationService;
            _messageBus = messageBus;

            NavigateToRequestsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<RequestsViewModel>(User.Id); }, canExecute: o => true);
            NavigateToCreate = new DelegateCommand(execute: o =>
            {
                Navigation.NavigateTo<RequestDetailViewModel>(0);
                _messageBus.Send(User);
            }, canExecute: o => true);
            NavigateToAccount = new DelegateCommand(execute: o => { Navigation.NavigateTo<ClientDetailViewModel>(Client.Id); }, canExecute: o => true);
        }
        public DelegateCommand NavigateToRequestsCommand { get; set; }
        public DelegateCommand NavigateToCreate { get; set; }
        public DelegateCommand NavigateToAccount { get; set; }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        public UserWrapper User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        public ClientWrapper Client
        {
            get => _client;
            set => Set(ref _client, value);
        }
        public override async Task LoadAsync(int userId)
        {
            var user = await _userDataProvider.GetById(userId);
            User = new UserWrapper(user);
            var client = await _clientDataProvider.GetByUserId(userId);
            Client = new ClientWrapper(client);
        }
    }
}
