using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels.MainViewModels
{
    public class AdminViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IUserDataProvider _userDataProvider;
        public AdminViewModel(
            INavigationService navigationService,
            IUserDataProvider userDataProvider)
        {
            _navigationService = navigationService;
            _userDataProvider = userDataProvider;
            
            NavigateToAssetsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<AssetsViewModel>(0); }, canExecute: o => true);
            NavigateToChangesCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ChangesViewModel>(0); }, canExecute: o => true);
            NavigateToClientsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ClientsViewModel>(0); }, canExecute: o => true);
            NavigateToEmployeesCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<EmployeesViewModel>(0); }, canExecute: o => true);
            NavigateToObjectivesCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ObjectivesViewModel>(0); }, canExecute: o => true);
            NavigateToProblemsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ProblemsViewModel>(0); }, canExecute: o => true);
            NavigateToRequestsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<RequestsViewModel>(User.Id); }, canExecute: o => true);
            NavigateToStatisticsCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<StatisticsViewModel>(User.Id); }, canExecute: o => true);
            NavigateToDepartmentsServicesCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<DepartmentsServicesViewModel>(0); }, canExecute: o => true);
        }
        public override async Task LoadAsync(int id)
        {
            
            var user = await _userDataProvider.GetById(id);
            if (user != null)
            {
                User = new UserWrapper(user);
            }
            Navigation.NavigateTo<StatisticsViewModel>(User.Id);
        }
        public DelegateCommand NavigateToAssetsCommand { get; set; }
        public DelegateCommand NavigateToChangesCommand { get; set; }
        public DelegateCommand NavigateToClientsCommand { get; set; }
        public DelegateCommand NavigateToEmployeesCommand { get; set; }
        public DelegateCommand NavigateToObjectivesCommand { get; set; }
        public DelegateCommand NavigateToProblemsCommand { get; set; }
        public DelegateCommand NavigateToRequestsCommand { get; set; }
        public DelegateCommand NavigateToStatisticsCommand { get; set; }
        public DelegateCommand NavigateToDepartmentsServicesCommand { get; set; }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
