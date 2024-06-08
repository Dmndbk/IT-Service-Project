using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Services.Dialogs;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITService.UI.ViewModels
{
    public class DepartmentsServicesViewModel : ViewModel
    {
        private IDepartmentDataProvider _departmentDataProvider;
        private IServiceDataProvider _serviceDataProvider;
        private IDialogService _dialogService;
        private IMessageBus _messageBus;
        private DepartmentWrapper _selectedDepartment;
        private ServiceWrapper _selectedService;
        private INavigationService _navigationService;

        public DepartmentsServicesViewModel(
            INavigationService navigationService,
            IDepartmentDataProvider departmentDataProvider,
            IServiceDataProvider serviceDataProvider,
            IDialogService dialogService,
            IMessageBus messageBus)
        {
            _navigationService = navigationService;
            _departmentDataProvider = departmentDataProvider;
            _serviceDataProvider = serviceDataProvider;
            _dialogService = dialogService;
            _messageBus = messageBus;

            DeleteDepartmentCommand = new LambdaCommand(OnDeleteDepartment);
            ChangeDepartmentCommand = new LambdaCommand(OnChangeDepartment);
            CreateDepartmentCommand = new LambdaCommand(OnCreateDepartment);
            DeleteServiceCommand = new LambdaCommand(OnDeleteService);
            ChangeServiceCommand = new LambdaCommand(OnChangeService);
            CreateServiceCommand = new LambdaCommand(OnCreateService);
        }
        public LambdaCommand DeleteDepartmentCommand { get; set; }
        public LambdaCommand ChangeDepartmentCommand { get; set; }
        public LambdaCommand CreateDepartmentCommand { get; set; }
        public LambdaCommand DeleteServiceCommand { get; set; }
        public LambdaCommand ChangeServiceCommand { get; set; }
        public LambdaCommand CreateServiceCommand { get; set; }
        public ObservableCollection<DepartmentWrapper> Departments { get; } = new();
        public ObservableCollection<ServiceWrapper> Services { get; } = new();
        public DepartmentWrapper SelectedDepartment
        {
            get => _selectedDepartment;
            set => Set(ref _selectedDepartment, value);
        }
        public ServiceWrapper SelectedService
        {
            get => _selectedService;
            set => Set(ref _selectedService, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
        public override async Task LoadAsync(int Id)
        {
            var departments = await _departmentDataProvider.Get();
            var services = await _serviceDataProvider.Get();
            InitializeDepartments(departments);
            InitializeServices(services);
        }
        private void InitializeDepartments(IEnumerable<Department> departments)
        {
            Departments.Clear();
            foreach (var department in departments)
            {
                Departments.Add(new DepartmentWrapper(department));
            }
        }
        private void InitializeServices(IEnumerable<Service> services)
        {
            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(new ServiceWrapper(service));
            }
        }
        private void OnDeleteService(object? parameter)
        {
            if (SelectedService is null)
            {
                MessageBox.Show("Сначала необходимо выбрать услугу из списка", "Внимание", MessageBoxButton.OK);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить услугу {SelectedService.Name}", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    _serviceDataProvider.Remove(SelectedService.Id);
                    Services.Remove(SelectedService);
                    break;
            }
            
        }
        private void OnDeleteDepartment(object? parameter)
        {
            if (SelectedDepartment is null)
            {
                MessageBox.Show("Сначала необходимо выбрать отдел из списка", "Внимание", MessageBoxButton.OK);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить отдел {SelectedDepartment.Name}", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    _departmentDataProvider.Remove(SelectedDepartment.Id);
                    Departments.Remove(SelectedDepartment);
                    break;
            }
        }
        private void OnCreateService(object? parameter)
        {
            var service = new Service();
            var dialog = new ServiceDialogViewModel("Услуга", "сообщение (изменить при небоходимости)", new ServiceWrapper(service), Departments);
            ServiceWrapper result = _dialogService.OpenDialog(dialog);

            if (result is null) return;

            service = new Service()
            {
                Id = result.Id,
                Name = result.Name,
                DepartmentId = result.DepartmentId
            };
            _serviceDataProvider.Add(service);
            Navigation.NavigateTo<DepartmentsServicesViewModel>(0);
        }
        private void OnCreateDepartment(object? parameter)
        {
            var department = new Department();
            var dialog = new DepartmentDialogViewModel("Отдел", "сообщение (изменить при небоходимости)", new DepartmentWrapper(department));
            DepartmentWrapper result = _dialogService.OpenDialog(dialog);

            if (result is null) return;

            department = new Department()
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description
            };
            _departmentDataProvider.Add(department);
            Navigation.NavigateTo<DepartmentsServicesViewModel>(0);
        }
        
        private void OnChangeService(object? parameter)
        {
            if (SelectedService is null)
            {
                MessageBox.Show("Сначала необходимо выбрать услугу из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                var dialog = new ServiceDialogViewModel("Услуга", "сообщение", SelectedService, Departments);
                ServiceWrapper result = _dialogService.OpenDialog(dialog);
                if (result is null) return;
                var service = new Service()
                {
                    Id = result.Id,
                    Name = result.Name,
                    DepartmentId = result.DepartmentId
                };
                _serviceDataProvider.Update(service);
            }
            
        }

        private void OnChangeDepartment(object? parameter)
        {
            if (SelectedDepartment is null)
            {
                MessageBox.Show("Сначала необходимо выбрать отдел из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                var dialog = new DepartmentDialogViewModel("Отдел", "сообщение", SelectedDepartment);
                DepartmentWrapper result = _dialogService.OpenDialog(dialog);
                if (result is null) return;
                var department = new Department()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description
                };
                _departmentDataProvider.Update(department);
            }
        }
    }
}
