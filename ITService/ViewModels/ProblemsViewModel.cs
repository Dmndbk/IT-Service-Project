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
    class ProblemsViewModel : ViewModel
    {
        private IProblemDataProvider _problemDataProvider;
        private INavigationService _navigationService;
        private ProblemWrapper _selectedProblem;
        private string _searchWord;

        public ProblemsViewModel(IProblemDataProvider problemDataProvider, INavigationService navigationService)
        {
            _problemDataProvider = problemDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ProblemDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete); 
            SearchCommand = new LambdaCommand(OnSearch);
        }
        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<ProblemWrapper> Problems { get; } = new();
        public override async Task LoadAsync(int Id)
        {
            var problems = await _problemDataProvider.GetAll();
            InitializeProblems(problems);
        }

        private void InitializeProblems(IEnumerable<Problem> problems)
        {
            Problems.Clear();
            foreach (var problem in problems)
            {
                Problems.Add(new ProblemWrapper(problem));
            }
        }
        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public ProblemWrapper SelectedProblem
        {
            get => _selectedProblem;
            set => Set(ref _selectedProblem, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedProblem != null)
            {
                Navigation.NavigateTo<ProblemDetailViewModel>(SelectedProblem.Id);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать проблему из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedProblem is null)
            {
                MessageBox.Show("Сначала необходимо выбрать проблему из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                _problemDataProvider.Remove(SelectedProblem.Id);
                Problems.Remove(SelectedProblem);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _problemDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {
                var problems = query.Where(p => p is { CurrentInfluenceLevel: not null, CurrentStatus: not null }
                                                && (p.CurrentInfluenceLevel.Name.Contains(SearchWord)
                                                    || p.Theme.Contains(SearchWord)
                                                    || p.CurrentStatus.Name.Contains(SearchWord)
                                                ));
                InitializeProblems(problems);
            }
            else
            {
                InitializeProblems(query);
            }
        }
    }
}
