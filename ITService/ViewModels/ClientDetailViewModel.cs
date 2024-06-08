using System.Collections.ObjectModel;
using System.Windows;
using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels
{
    public class ClientDetailViewModel : ViewModel
    {
        private IClientDataProvider _clientDataProvider;
        private ICompanyDataProvider _companyDataProvider;
        private INavigationService _navigationService;
        private IUserRoleDataProvider _userRoleDataProvider;
        private IUserDataProvider _userDataProvider;
        private ClientWrapper _client;

        public ClientDetailViewModel(
            IClientDataProvider clientDataProvider,
            ICompanyDataProvider companyDataProvider,
            INavigationService navigationService,
            IUserRoleDataProvider userRoleDataProvider,
            IUserDataProvider userDataProvider)
        {
            _clientDataProvider = clientDataProvider;
            _companyDataProvider = companyDataProvider;
            _navigationService = navigationService;
            _userRoleDataProvider = userRoleDataProvider;
            _userDataProvider = userDataProvider;

            NavigateToBackCommand = new DelegateCommand(OnNavigateBack, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }

        private void OnNavigateBack(object? parameter)
        {
            if (IsEmployee)
            {
                Navigation.NavigateTo<ClientsViewModel>(User.Id);
            }
            else
            {
                Navigation.NavigateTo<ClientHomeViewModel>(User.Id);
            }
                
        }

        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<CompanyWrapper> Companies { get; } = new();
        public ObservableCollection<UserRoleWrapper> UserRoles { get; } = new();

        public ClientWrapper Client
        {
            get => _client;
            set => Set(ref _client, value);
        }
        public override async Task LoadAsync(int clientId)
        {
            var client = new Client();
            var user = new User();
            if (clientId > 0)
            {
                client = await _clientDataProvider.GetById(clientId);
                user = await _userDataProvider.GetById((int)client.UserId);
            }
            else
            {
                user = CreateNewUser();
                client = CreateNewClient();
            }
            
            //var client = clientId > 0 ? await _clientDataProvider.GetById(clientId) : CreateNewClient();
            //var user = await _userDataProvider.GetById((int)client.UserId);

            InitializeClient(client);
            InitializeUser(user);

            await LoadCompaniesAsync();
            await LoadUserRolesAsync();

            Client.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Client.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeClient(Client client)
        {
            Client = new ClientWrapper(client);
            if (Client.Id == 0)
            {
                //работа валидации
                Client.FirstName = "";
                Client.LastName = "";
                Client.Patronymic = "";
                Client.Email = "";
            }
        }
        private void InitializeUser(User user)
        {
            User = new UserWrapper(user);
            if (User.Id == 0)
            {
                //работа валидации
                User.Login = "";
                User.Password = "";
                User.UserRoleId = 3;
            }
        }
        private Client? CreateNewClient()
        {
            var client = new Client();

            client.FirstName = "Имя";
            client.LastName = "Фамилия";
            client.Patronymic = "Отчество";
            client.Email = "Почта";
            return client;
        }
        private User? CreateNewUser()
        {
            var user = new User();

            user.Login = "логин";
            user.Password = "пароль";
            user.UserRoleId = 3;
            return user;
        }

        private async Task LoadCompaniesAsync()
        {
            var companies = await _companyDataProvider.Get();
            Companies.Clear();
            foreach (var company in companies)
            {
                Companies.Add(new CompanyWrapper(company));
            }
        }
        private async Task LoadUserRolesAsync()
        {
            var userRoles = await _userRoleDataProvider.Get();
            UserRoles.Clear();
            foreach (var userRole in userRoles)
            {
                UserRoles.Add(new UserRoleWrapper(userRole));
            }
        }
        private bool OnSaveCanExecute(object? parameter)
        {
            return Client != null && !Client.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var user = new User()
            {
                Id = User.Id,
                Login = User.Login,
                Password = User.Password,
                UserRoleId = User.UserRoleId
            };
            
            if (user.Id > 0)
            {
                _userDataProvider.Update(user);
            }
            else
            {
                _userDataProvider.Add(user);
            }

            var client = new Client()
            {
                Id = Client.Id,
                FirstName = Client.FirstName,
                LastName = Client.LastName,
                Patronymic = Client.Patronymic,
                Email = Client.Email,
                CompanyId = Client.CompanyId,
                UserId = user.Id
            };
            if (client.Id > 0)
            {
                _clientDataProvider.Update(client);
            }
            else
            {
                _clientDataProvider.Add(client);
            }
            MessageBox.Show("Клиент успешно сохранен", "Сохранение");
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
