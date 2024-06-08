using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using ITService.UI.ViewModels.MainViewModels;
using System.Windows;

namespace ITService.UI.ViewModels
{
    public class EmployeeDetailViewModel : ViewModel
    {
        private IEmployeeDataProvider _employeeDataProvider;
        private IDepartmentDataProvider _departmentDataProvider;
        private INavigationService _navigationService;
        private IUserRoleDataProvider _userRoleDataProvider;
        private IUserDataProvider _userDataProvider;
        private UserWrapper _user;
        private EmployeeWrapper _employee;

        public EmployeeDetailViewModel(
            IEmployeeDataProvider employeeDataProvider,
            IDepartmentDataProvider departmentDataProvider,
            INavigationService navigationService,
            IUserRoleDataProvider userRoleDataProvider,
            IUserDataProvider userDataProvider)
        {
            _employeeDataProvider = employeeDataProvider;
            _departmentDataProvider = departmentDataProvider;
            _navigationService = navigationService;
            _userRoleDataProvider = userRoleDataProvider;
            _userDataProvider = userDataProvider;

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<EmployeesViewModel>(0); }, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<DepartmentWrapper> Departments { get; } = new();
        public ObservableCollection<UserRoleWrapper> UserRoles { get; } = new();
        public EmployeeWrapper Employee
        {
            get => _employee;
            set => Set(ref _employee, value);
        }
        public UserWrapper User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        public override async Task LoadAsync(int employeId)
        {
            var employee = new Employee();
            var user = new User();
            if (employeId > 0)
            {
                employee = await _employeeDataProvider.GetById(employeId);
                user = await _userDataProvider.GetById((int)employee.UserId);
            }
            else
            {
                user = CreateNewUser();
                employee = CreateNewEmployee();
            }
            InitializeUser(user);
            InitializeEmployee(employee);
            await LoadDepartmentsAsync();
            await LoadUserRolesAsync();
            Employee.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Employee.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeEmployee(Employee employee)
        {
            Employee = new EmployeeWrapper(employee);
            if (Employee.Id == 0)
            {
                //работа валидации
                Employee.FirstName = "";
                Employee.LastName = "";
                Employee.Patronymic = "";
                Employee.Email = "";
                Employee.Position = "";
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
                User.UserRoleId = 2;
            }
        }
        private Employee? CreateNewEmployee()
        {
            var employee = new Employee();

            employee.FirstName = "Имя";
            employee.LastName = "Фамилия";
            employee.Patronymic = "Отчество";
            employee.Email = "Почта";
            employee.Position = "Должность";
            return employee;
        }
        private User? CreateNewUser()
        {
            var user = new User();

            user.Login = "логин";
            user.Password = "пароль";
            user.UserRoleId = 2;
            return user;
        }
        private async Task LoadDepartmentsAsync()
        {
            var departments = await _departmentDataProvider.Get();
            Departments.Clear();
            foreach (var department in departments)
            {
                Departments.Add(new DepartmentWrapper(department));
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
            return Employee != null && !Employee.HasErrors;
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
            var employee = new Employee()
            {
                Id = Employee.Id,
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                Patronymic = Employee.Patronymic,
                Email = Employee.Email,
                Position = Employee.Position,
                DepartmentId = Employee.DepartmentId,
                UserId = user.Id
            };
            if (employee.Id > 0)
            {
                _employeeDataProvider.Update(employee);
            }
            else
            {
                _employeeDataProvider.Add(employee);
            }
            MessageBox.Show("Сотрудник успешно сохранен", "Сохранение");
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
