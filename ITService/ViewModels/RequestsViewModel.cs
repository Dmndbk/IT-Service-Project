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
    public class RequestsViewModel : ViewModel
    {
        private IRequestDataProvider _requestDataProvider;
        private INavigationService _navigationService;
        private IEmployeeDataProvider _employeeDataProvider;
        private IClientDataProvider _clientDataProvider;
        private IUserDataProvider _userDataProvider;
        private IMessageBus _messageBus;
        private string _searchWord;
        private RequestWrapper _selectedRequest;
        private UserWrapper _user;

        public RequestsViewModel(
            IRequestDataProvider requestDataProvider, 
            INavigationService navigationService,
            IEmployeeDataProvider employeeDataProvider,
            IClientDataProvider clientDataProvider,
            IUserDataProvider userDataProvider, 
            IMessageBus messageBus)
        {
            _requestDataProvider = requestDataProvider;
            _navigationService = navigationService;
            _employeeDataProvider = employeeDataProvider;
            _clientDataProvider = clientDataProvider;
            _userDataProvider = userDataProvider;

            _messageBus = messageBus;

            NavigateToDetailCommand = new DelegateCommand(OnNavigateToDetail, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(OnNavigateToCreate, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
            
        }
        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<RequestWrapper> Requests { get; set; } = new();

        public override async Task LoadAsync(int id)
        {
            var user = await _userDataProvider.GetById(id);
            if (user != null)
            {
                IEnumerable<Request> requests = null;
                await InitializeRequests(user, requests);
            }
            User = new UserWrapper(user);
        }
        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public RequestWrapper SelectedRequest
        {
            get => _selectedRequest;
            set => Set(ref _selectedRequest, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        private void OnNavigateToDetail(object? parameter)
        {
            if (SelectedRequest != null)
            {
                Navigation.NavigateTo<RequestDetailViewModel>(SelectedRequest.Id);
                _messageBus.Send(User);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать заявку из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedRequest is null)
            {
                MessageBox.Show("Сначала необходимо выбрать заявку из списка", "Внимание", MessageBoxButton.OK);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить заявку { SelectedRequest.Theme}", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    _requestDataProvider.Remove(SelectedRequest.Id);
                    Requests.Remove(SelectedRequest);
                    break;
            }

        }
        private async void OnSearch(object? parameter)
        {
            var user = await _userDataProvider.GetById(User.Id);
            if (user != null)
            {
                IEnumerable<Request> requests = null;
                await InitializeRequests(user, requests);
            }
            
        }
        private void LoadRequests(IEnumerable<Request> requests)
        {
            Requests.Clear();
            foreach (var request in requests)
            {
                Requests.Add(new RequestWrapper(request));
            }
        }
        private async Task InitializeRequests(User? user, IEnumerable<Request>? requests)
        {
            switch (user.UserRoleId)
            {
                case 2: //Employee
                {
                    var employee = await _employeeDataProvider.GetByUserId(user.Id);
                    var query = await _requestDataProvider.GetByDepartmentId((int)employee.DepartmentId);
                    requests = Search(query);
                    break;
                }
                case 3: //Client
                {
                    var client = await _clientDataProvider.GetByUserId(user.Id);
                    var query = await _requestDataProvider.GetByClientId(client.Id);
                    requests = Search(query);
                    break;
                }
                case 1: //Admin
                {
                    var query = await _requestDataProvider.GetAll();
                    requests = Search(query);
                    break;
                }
            }
            LoadRequests(requests);
        }

        private IEnumerable<Request> Search(IEnumerable<Request> requests)
        {
            if (!string.IsNullOrEmpty(SearchWord))
            {
                return requests.Where(r => r is { RelatedAsset: not null, Service: not null, CurrentStatus: not null, RelatedClient: not null, RelatedEmployee: not null }
                                           && (r.RelatedAsset.Name.Contains(SearchWord)
                                               || r.Theme.Contains(SearchWord)
                                               || r.CurrentStatus.Name.Contains(SearchWord)
                                               || r.Service.Name.Contains(SearchWord)
                                               || r.RelatedClient.FirstName.Contains(SearchWord)
                                               || r.RelatedClient.LastName.Contains(SearchWord)
                                               || r.RelatedEmployee.FirstName.Contains(SearchWord)
                                               || r.RelatedEmployee.LastName.Contains(SearchWord)
                                           ));
            }
            return requests;
            
        }
        private void OnNavigateToCreate(object? parameter)
        {
            Navigation.NavigateTo<RequestDetailViewModel>(0);
            _messageBus.Send(User);
        }
    }
}
