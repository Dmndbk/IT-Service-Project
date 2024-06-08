using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using System.Windows;
using ITService.UI.Services.Dialogs;
using Microsoft.VisualBasic.ApplicationServices;

namespace ITService.UI.ViewModels
{
    public class RequestDetailViewModel : ViewModel, IDisposable
    {
        private IDialogService _dialogService;
        private IRequestDataProvider _requestDataProvider;
        private IAssetDataProvider _assetDataProvider;
        private IEmployeeDataProvider _employeeDataProvider;
        private IClientDataProvider _clientDataProvider;
        private IStatusDataProvider _statusDataProvider;
        private IServiceDataProvider _serviceDataProvider;
        private INavigationService _navigationService;
        private IDisposable _subscription;
        private RequestWrapper _request;
        private UserWrapper _user; 
        private EmployeeWrapper _employee;
        private bool _isNoRelatedEmployee;
        private bool _isOpen;
        private bool _isResolved;
        private string _status;
        private DialogItem _dItem;
        private bool _isClosed;

        private IUserDataProvider _userDataProvider;
        public RequestDetailViewModel(
            IDialogService dialogService,
            IRequestDataProvider requestDataProvider,
            IAssetDataProvider assetDataProvider,
            IEmployeeDataProvider employeeDataProvider,
            IClientDataProvider clientDataProvider,
            IStatusDataProvider statusDataProvider,
            IServiceDataProvider serviceDataProvider,
            INavigationService navigationService,
            IUserDataProvider userDataProvider,
            IMessageBus messageBus)
        {
            _dialogService = dialogService;
            _requestDataProvider = requestDataProvider;
            _assetDataProvider = assetDataProvider;
            _employeeDataProvider = employeeDataProvider;
            _clientDataProvider = clientDataProvider;
            _statusDataProvider = statusDataProvider;
            _serviceDataProvider = serviceDataProvider;
            _navigationService = navigationService;
            _userDataProvider = userDataProvider;
            _subscription = messageBus.RegisterHandler<UserWrapper>(OnReceiveMessage);

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<RequestsViewModel>(User.Id);}, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
            TakeCommand = new DelegateCommand(OnTakeCommand);
            ChangeStatusCommand = new DelegateCommand(OnChangeStatus);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand TakeCommand { get; set; }
        public DelegateCommand ChangeStatusCommand { get; set; }
        public ObservableCollection<AssetWrapper> Assets { get; } = new();
        public ObservableCollection<EmployeeWrapper> Employees { get; } = new();
        public ObservableCollection<ClientWrapper> Clients { get; } = new();
        public ObservableCollection<ServiceWrapper> Services { get; } = new();
        public ObservableCollection<StatusWrapper> Statuses { get; } = new();
        public void Dispose() => _subscription.Dispose();

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        public bool IsNoRelatedEmployee
        {
            get => _isNoRelatedEmployee;
            set => Set(ref _isNoRelatedEmployee, value);
        }
        public bool IsOpen
        {
            get => _isOpen;
            set => Set(ref _isOpen, value);
        }
        public bool IsResolved
        {
            get => _isResolved;
            set => Set(ref _isResolved, value);
        }
        public bool IsClosed
        {
            get => _isClosed;
            set => Set(ref _isClosed, value);
        }
        public DialogItem Item
        {
            get => _dItem;
            set => Set(ref _dItem, value);
        }
        public RequestWrapper Request
        {
            get => _request;
            set => Set(ref _request, value);
        }
        public UserWrapper User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        public EmployeeWrapper Employee
        {
            get => _employee;
            set => Set(ref _employee, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        public override async Task LoadAsync(int requestId)
        {
            var request = requestId > 0
                ? await _requestDataProvider.GetById(requestId)
                : CreateNewRequest();
            
            if (request.RelatedEmployeeId is null)
                IsNoRelatedEmployee = true;
            if (request.Id <= 0)
                IsNoRelatedEmployee = false;

            InitializeStatus(request);
            InitializeRequest(request);
            await LoadAssetsAsync();
            await LoadEmployeesAsync();
            await LoadClientsAsync();
            await LoadServicesAsync();
            await LoadStatusesAsync();

            Request.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Request.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeStatus(Request request)
        {
            switch (request.CurrentStatusId)
            {
                case 1: /*Новая*/
                    Status = "В работу";
                    IsResolved = false;
                    IsClosed = false;
                    IsOpen = true;
                    break;
                case 2: /*В работе*/
                    Status = "Решить";
                    IsResolved = false;
                    IsClosed = false;
                    IsOpen = true;
                    break;
                case 3: /*Решена*/
                    Status = "Закрыть";
                    IsResolved = true;
                    IsClosed = false;
                    IsOpen = true;
                    break;
                case 4: /*Закрыта*/
                    IsResolved = true;
                    IsClosed = true;
                    IsOpen = false;
                    break;
            }
            //if (User.UserRoleId == 3)
            //{
            //    IsOpen = false;
            //    IsNoRelatedEmployee = false;
            //}
        }
        private void InitializeRequest(Request request)
        {
            Request = new RequestWrapper(request);

            if (Request.Id == 0)
            {
                Request.Theme = "";
                Request.Description = "";
                Request.CurrentStatusId = 1;
                Request.CreationDate = DateTime.Now;
                Request.CloseDate = DateTime.Now.AddDays(1);
            }
        }
        private Request? CreateNewRequest()
        {
            IsResolved = false;
            IsOpen = false;
            IsClosed = false;
            IsNoRelatedEmployee = true;
            var request = new Request();
            request.Theme = "Тема";
            request.Description = "Описание";
            request.CreationDate = DateTime.Now;
            request.CloseDate = DateTime.Now.AddDays(1);
            return request;
        }
        private async Task LoadAssetsAsync()
        {
            var assets = await _assetDataProvider.Get();
            Assets.Clear();
            foreach (var asset in assets)
            {
                Assets.Add(new AssetWrapper(asset));
            }
        }
        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeeDataProvider.Get();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(new EmployeeWrapper(employee));
            }
        }
        private async Task LoadClientsAsync()
        {
            var clients = await _clientDataProvider.Get();
            Clients.Clear();
            foreach (var client in clients)
            {
                Clients.Add(new ClientWrapper(client));
            }
        }
        private async Task LoadServicesAsync()
        {
            var services = await _serviceDataProvider.Get();
            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(new ServiceWrapper(service));
            }
        }
        private async Task LoadStatusesAsync()
        {
            var statuses = await _statusDataProvider.Get();
            Statuses.Clear();
            foreach (var status in statuses)
            {
                Statuses.Add(new StatusWrapper(status));
            }
        }
        private void OnReceiveMessage(UserWrapper user)
        {
            User = user;
            
        }
        private bool OnSaveCanExecute(object? parameter)
        {
            return Request != null && !Request.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var request = new Request()
            {
                Id = Request.Id,
                Theme = Request.Theme,
                Description = Request.Description,
                SolutionDescription = Request.SolutionDescription,
                CreationDate = Request.CreationDate,
                CloseDate = Request.CloseDate,
                CurrentStatusId = Request.CurrentStatusId,
                RelatedAssetId = Request.RelatedAssetId,
                RelatedEmployeeId = Request.RelatedEmployeeId,
                RelatedClientId = Request.RelatedClientId,
                ServiceId = Request.ServiceId,
                AssessmentId = Request.AssessmentId
            };
            if (request.Id > 0)
            {
                _requestDataProvider.Update(request);
            }
            else
            {
                _requestDataProvider.Add(request);
            }

            MessageBox.Show("Заявка успешно сохранена", "Сохранение");
        }
        private async void OnTakeCommand(object? paraneter)
        {
            var employee = await _employeeDataProvider.GetByUserId(User.Id);

            MessageBoxResult result = MessageBox.Show("Вы точно хотите взять заявку в ответственность?", "Изменение", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var request = new Request()
                    {
                        Id = Request.Id,
                        Theme = Request.Theme,
                        Description = Request.Description,
                        SolutionDescription = Request.SolutionDescription,
                        CreationDate = Request.CreationDate,
                        CloseDate = Request.CloseDate,
                        CurrentStatusId = Request.CurrentStatusId,
                        RelatedAssetId = Request.RelatedAssetId,
                        RelatedEmployeeId = employee.Id,
                        RelatedClientId = Request.RelatedClientId,
                        ServiceId = Request.ServiceId,
                        AssessmentId = Request.AssessmentId
                    };
                    _requestDataProvider.Update(request);
                    Navigation.NavigateTo<RequestsViewModel>(User.Id);
                    break;
            }
        }
        private void OnChangeStatus(object? obj)
        {
            var comment = Request.Comment;
            var assessmentId = Request.AssessmentId;
            int newStatusId = (int)Request.CurrentStatusId;
            string newSolutionDescription = Request.SolutionDescription;
            switch (Status)
            {
                case "В работу":
                    newStatusId = 2;
                    break;

                case "Решить":
                    var resolveDialog = new ResolveDialogViewModel("Отчет о решении", "сообщение (изменить при небоходимости)");
                    var resolveResult = _dialogService.OpenDialog(resolveDialog);
                    if (resolveResult == null) return;
                    newStatusId = 3;
                    newSolutionDescription = resolveResult;
                    break;

                case "Закрыть":
                    var assessmentDialog = new AssessmentDialogViewModel("Оценка решения", "сообщение (изменить при небоходимости)");
                    var result = _dialogService.OpenDialog(assessmentDialog);
                    if (result == null) return;
                    newStatusId = 4;
                    Item = result;
                    comment = Item.Comment;
                    assessmentId = Item.Id + 1;
                    break;
                    
            }
            
            var request = new Request()
            {
                Id = Request.Id,
                Theme = Request.Theme,
                Description = Request.Description,
                SolutionDescription = newSolutionDescription,
                CreationDate = Request.CreationDate,
                CloseDate = Request.CloseDate,
                CurrentStatusId = newStatusId,
                RelatedAssetId = Request.RelatedAssetId,
                RelatedEmployeeId = Request.RelatedEmployeeId,
                RelatedClientId = Request.RelatedClientId,
                ServiceId = Request.ServiceId,
                AssessmentId = assessmentId,
                Comment = comment
            };
            _requestDataProvider.Update(request);
            
            Navigation.NavigateTo<RequestsViewModel>(User.Id);
        }
    }
}
