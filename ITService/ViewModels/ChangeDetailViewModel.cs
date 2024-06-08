using System.Collections.ObjectModel;
using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels
{
    public class ChangeDetailViewModel : ViewModel
    {
        private IChangeDataProvider _changeDataProvider;
        private IEmployeeDataProvider _employeeDataProvider;
        private IServiceDataProvider _serviceDataProvider;
        private IStatusDataProvider _statusDataProvider;
        private INavigationService _navigationService;
        private ChangeWrapper _change;
        private int id;
        private string _status;

        public ChangeDetailViewModel(
            IChangeDataProvider changeDataProvider,
            IEmployeeDataProvider employeeDataProvider,
            IServiceDataProvider serviceDataProvider,
            IStatusDataProvider statusDataProvider,
            INavigationService navigationService)
        {
            _changeDataProvider = changeDataProvider;
            _employeeDataProvider = employeeDataProvider;
            _serviceDataProvider = serviceDataProvider;
            _statusDataProvider = statusDataProvider;
            _navigationService = navigationService;

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ChangesViewModel>(0); }, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<EmployeeWrapper> Employees { get; } = new();
        public ObservableCollection<ServiceWrapper> Services { get; } = new();
        public ObservableCollection<StatusWrapper> Statuses { get; } = new();
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
        public int Id
        {
            get => id;
            set => Set(ref id, value);
        }
        public ChangeWrapper Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int changeId)
        {
            id = changeId;

            var change = changeId > 0
                ? await _changeDataProvider.GetById(changeId)
                : CreateNewChange();
            InitializeChange(change);
            await LoadEmployeesAsync();
            await LoadServicesAsync();
            await LoadStatusesAsync();

            Change.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Change.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeChange(Change change)
        {
            Change = new ChangeWrapper(change);
            if (Change.Id == 0)
            {
                //работа валидации
                Change.Theme = "";
                Change.Description = "";
                Change.EndDate = DateTime.Now;
            }
        }
        private Change? CreateNewChange()
        {
            var change = new Change();

            change.Theme = "Тема";
            change.Description = "Описание";
            return change;
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
        private bool OnSaveCanExecute(object? parameter)
        {
            return Change != null && !Change.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var change = new Change()
            {
                Id = Change.Id,
                Theme = Change.Theme,
                Description = Change.Description,
                EndDate = Change.EndDate,
                RelatedEmployeeId = Change.RelatedEmployeeId,
                RelatedServiceId = Change.RelatedServiceId
            };
            if (change.Id > 0)
            {
                _changeDataProvider.Update(change);
            }
            else
            {
                _changeDataProvider.Add(change);
            }

        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
