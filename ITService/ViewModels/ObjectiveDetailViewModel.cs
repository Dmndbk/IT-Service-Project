using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITService.UI.ViewModels
{
    class ObjectiveDetailViewModel : ViewModel
    {
        private IEmployeeDataProvider _employeeDataProvider;
        private IObjectiveDataProvider _objectiveDataProvider;
        private IStatusDataProvider _statusDataProvider;
        private INavigationService _navigationService;
        private ObjectiveWrapper _objective;
        private int id;
        private string _status;

        public ObjectiveDetailViewModel(
            IEmployeeDataProvider employeeDataProvider,
            IObjectiveDataProvider objectiveDataProvider,
            IStatusDataProvider statusDataProvider,
            INavigationService navigationService)
        {
            _employeeDataProvider = employeeDataProvider;
            _objectiveDataProvider = objectiveDataProvider;
            _statusDataProvider = statusDataProvider;
            _navigationService = navigationService;

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ObjectivesViewModel>(0); }, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<EmployeeWrapper> Employees { get; } = new();
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
        public ObjectiveWrapper Objective
        {
            get => _objective;
            set
            {
                _objective = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int objectiveId)
        {
            id = objectiveId;

            var objective = objectiveId > 0
                ? await _objectiveDataProvider.GetById(objectiveId)
                : CreateNewObjective();
            InitializeObjective(objective);
            await LoadEmployeesAsync();
            await LoadStatusesAsync();
            Objective.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Objective.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeObjective(Objective objective)
        {
            Objective = new ObjectiveWrapper(objective);
            if (Objective.Id == 0)
            {
                //работа валидации
                Objective.Theme = "";
                Objective.Description = "";
                Objective.StartDate = DateTime.Now;
                Objective.EndDate = DateTime.Now.AddDays(1);
                Objective.CurrentStatusId = 1;
            }
        }
        private Objective? CreateNewObjective()
        {
            var objective = new Objective();

            objective.Theme = "Имя";
            objective.Description = "Фамилия";
            return objective;
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
            return Objective != null && !Objective.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var objective = new Objective()
            {
                Id = Objective.Id,
                Theme = Objective.Theme,
                Description = Objective.Description,
                StartDate = Objective.StartDate,
                EndDate = Objective.EndDate,
                ResponsibleEmployeeId = Objective.ResponsibleEmployeeId,
                CurrentStatusId = Objective.CurrentStatusId
            };
            if (objective.Id > 0)
            {
                _objectiveDataProvider.Update(objective);
            }
            else
            {
                _objectiveDataProvider.Add(objective);
            }
            MessageBox.Show("Задача успешно сохранена", "Сохранение");
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
