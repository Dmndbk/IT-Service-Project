using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;

namespace ITService.UI.ViewModels
{
    class ProblemDetailViewModel : ViewModel
    {
        private IProblemDataProvider _problemDataProvider;
        private IInfluenceLevelDataProvider _influenceLevelDataProvider;
        private IStatusDataProvider _statusDataProvider;
        private INavigationService _navigationService;
        private ProblemWrapper _problem;
        private int id;
        private string _status;

        public ProblemDetailViewModel(
            IProblemDataProvider problemDataProvider,
            IInfluenceLevelDataProvider influenceLevelDataProvider,
            IStatusDataProvider statusDataProvider,
            INavigationService navigationService)
        {
            _problemDataProvider = problemDataProvider;
            _influenceLevelDataProvider = influenceLevelDataProvider;
            _statusDataProvider = statusDataProvider;
            _navigationService = navigationService;

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ProblemsViewModel>(0); }, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<InfluenceLevelWrapper> InfluenceLevels { get; } = new();
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
        public ProblemWrapper Problem
        {
            get => _problem;
            set
            {
                _problem = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int problemId)
        {
            id = problemId;

            var problem = problemId > 0
                ? await _problemDataProvider.GetById(problemId)
                : CreateNewProblem();
            InitializeProblem(problem);
            await LoadInfluenceLevelsAsync();
            await LoadStatusesAsync();

            Problem.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Problem.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitializeProblem(Problem problem)
        {
            Problem = new ProblemWrapper(problem);
            if (Problem.Id == 0)
            {
                //работа валидации
                Problem.Theme = "";
                Problem.Description = "";
                Problem.EndDate = DateTime.Now;
                Problem.CurrentInfluenceLevelId = 0;
            }
        }
        private Problem? CreateNewProblem()
        {
            var problem = new Problem();

            problem.Theme = "Тема";
            problem.Description = "Описание";
            problem.EndDate = DateTime.Now;
            problem.CurrentInfluenceLevelId = 1;
            return problem;
        }

        private async Task LoadInfluenceLevelsAsync()
        {
            var levels = await _influenceLevelDataProvider.Get();
            InfluenceLevels.Clear();
            foreach (var level in levels)
            {
                InfluenceLevels.Add(new InfluenceLevelWrapper(level));
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
            return Problem != null && !Problem.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var problem = new Problem()
            {
                Id = Problem.Id,
                Theme = Problem.Theme,
                Description = Problem.Description,
                EndDate = Problem.EndDate,
                CurrentInfluenceLevelId = Problem.CurrentInfluenceLevelId
            };
            if (problem.Id > 0)
            {
                _problemDataProvider.Update(problem);
            }
            else
            {
                _problemDataProvider.Add(problem);
            }

        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
