using ITService.UI.Data;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using ITService.UI.Commands;
using ITService.UI.Services;
using System.Windows;
using ITService.Model;

namespace ITService.UI.ViewModels
{
    class ClientsViewModel : ViewModel
    {
        private IClientDataProvider _clientDataProvider;
        private INavigationService _navigationService;
        private ClientWrapper _selectedClient;
        private string _searchWord;

        public ClientsViewModel(IClientDataProvider clientDataProvider, INavigationService navigationService)
        {
            _clientDataProvider = clientDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ClientDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
        }
        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<ClientWrapper> Clients { get; } = new();

        public override async Task LoadAsync(int Id)
        {
            var clients = await _clientDataProvider.GetAll();
            InitializeClients(clients);
        }

        private void InitializeClients(IEnumerable<Client>? clients)
        {
            Clients.Clear();
            foreach (var client in clients)
            {
                Clients.Add(new ClientWrapper(client));
            }
        }

        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public ClientWrapper SelectedClient
        {
            get => _selectedClient;
            set => Set(ref _selectedClient, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedClient != null)
            {
                Navigation.NavigateTo<ClientDetailViewModel>(SelectedClient.Id);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать клиента из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedClient is null)
            {
                MessageBox.Show("Сначала необходимо выбрать клиента из списка", "Внимание", MessageBoxButton.OK);
            }
            {
                _clientDataProvider.Remove(SelectedClient.Id);
                Clients.Remove(SelectedClient);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _clientDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {

                var clients = query.Where(p => p is { Company: not null }
                                               && (p.FirstName.Contains(SearchWord)
                                                   || p.LastName.Contains(SearchWord)
                                                   || p.Patronymic.Contains(SearchWord)
                                                   || p.Email.Contains(SearchWord)
                                                   || p.Company.Name.Contains(SearchWord)
                                               ));
                InitializeClients(clients);
            }
            else
            {
                InitializeClients(query);
            }
        }
    }
}
