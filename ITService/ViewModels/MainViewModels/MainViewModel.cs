using System.Collections.ObjectModel;
using System.Windows;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels.MainViewModels
{
    public class MainViewModel : ViewModel
    {
        private IUserDataProvider _userDataProvider;
        private ViewModel? _selectedViewModel;
        private string _login;
        private string _password;
        private bool _isLoggingIn;
        public MainViewModel(
            AdminViewModel adminViewModel,
            ClientViewModel clientViewModel,
            EmployeeViewModel employeeViewModel,
            IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;

            AdminViewModel = adminViewModel;
            ClientViewModel = clientViewModel;
            EmployeeViewModel = employeeViewModel;

            IsLoggingIn = true;

            SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }
        public AdminViewModel AdminViewModel { get; }
        public ClientViewModel ClientViewModel { get; }
        public EmployeeViewModel EmployeeViewModel { get; }
        public DelegateCommand SelectViewModelCommand { get; }
        public ObservableCollection<UserWrapper> Users = new();

        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set => Set(ref _isLoggingIn, value);
        }
        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public ViewModel? SelectedViewModel
        {
            get => _selectedViewModel;
            set => Set(ref _selectedViewModel, value);
        }
        public override async Task LoadAsync(int id)
        {
            await LoadUsersAsync();
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync(id);
            }
        }
        private async Task LoadUsersAsync()
        {
            var users = await _userDataProvider.GetAll();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(new UserWrapper(user));
            }
        }
        private async void SelectViewModel(object? parameter)
        {
            var user = Users.FirstOrDefault(u => u.Login == Login && u.Password == Password);
            if (user != null)
            {
                switch (user.UserRole.Name.Trim())
                {
                    case "Client":
                        SelectedViewModel = ClientViewModel;
                        await LoadAsync(user.Id);
                        IsEmployee = false;
                        IsClient = true;
                        break;
                    case "Employee":
                        SelectedViewModel = EmployeeViewModel;
                        await LoadAsync(user.Id);
                        IsEmployee = true;
                        IsClient = false;
                        break;
                    case "Admin":
                        SelectedViewModel = AdminViewModel;
                        await LoadAsync(user.Id);
                        IsEmployee = true;
                        IsClient = false;
                        break;
                }
                IsLoggingIn = false;
            }
            else
            {
                MessageBox.Show("Ошибка авторизации", "Внимание", MessageBoxButton.OK);
            }
        }
    }
}
