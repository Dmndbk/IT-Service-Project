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
    class EmployeesViewModel : ViewModel
    {
        private IEmployeeDataProvider _employeeDataProvider;
        private INavigationService _navigationService;
        private EmployeeWrapper _selectedEmployee;
        private string _searchWord;

        public EmployeesViewModel(IEmployeeDataProvider employeeDataProvider, INavigationService navigationService)
        {
            _employeeDataProvider = employeeDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<EmployeeDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
        }
        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<EmployeeWrapper> Employees { get; } = new();
        public override async Task LoadAsync(int Id)
        {
            var employees = await _employeeDataProvider.GetAll();
            InitializeEmployee(employees);
        }

        private void InitializeEmployee(IEnumerable<Employee>? employees)
        {
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(new EmployeeWrapper(employee));
            }
        }
        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public EmployeeWrapper SelectedEmployee
        {
            get => _selectedEmployee;
            set => Set(ref _selectedEmployee, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedEmployee != null)
            {
                Navigation.NavigateTo<EmployeeDetailViewModel>(SelectedEmployee.Id);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать сотрудника из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedEmployee is null)
            {
                MessageBox.Show("Сначала необходимо выбрать сотрудника из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                _employeeDataProvider.Remove(SelectedEmployee.Id);
                Employees.Remove(SelectedEmployee);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _employeeDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {

                var employees = query.Where(p => p is { Department: not null }
                                               && (p.FirstName.Contains(SearchWord)
                                                   || p.LastName.Contains(SearchWord)
                                                   || p.Patronymic.Contains(SearchWord)
                                                   || p.Email.Contains(SearchWord)
                                                   || p.Position.Contains(SearchWord)
                                                   || p.Department.Name.Contains(SearchWord)
                                               ));
                InitializeEmployee(employees);
            }
            else
            {
                InitializeEmployee(query);
            }
        }
    }
}
