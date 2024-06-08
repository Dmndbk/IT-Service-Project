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
    class ObjectivesViewModel : ViewModel
    {
        private IObjectiveDataProvider _objectiveDataProvider;
        private INavigationService _navigationService;
        private ObjectiveWrapper _selectedObjective;
        private string _searchWord;

        public ObjectivesViewModel(IObjectiveDataProvider objectiveDataProvider, INavigationService navigationService)
        {
            _objectiveDataProvider = objectiveDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ObjectiveDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
        }

        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<ObjectiveWrapper> Objectives { get; } = new();
        public override async Task LoadAsync(int Id)
        {
            var objectives = await _objectiveDataProvider.GetAll();
            InitializeObjectives(objectives);
        }

        private void InitializeObjectives(IEnumerable<Objective> objectives)
        {
            Objectives.Clear();
            foreach (var objective in objectives)
            {
                Objectives.Add(new ObjectiveWrapper(objective));
            }
        }
        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public ObjectiveWrapper SelectedObjective
        {
            get => _selectedObjective;
            set => Set(ref _selectedObjective, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedObjective != null)
            {
                Navigation.NavigateTo<ObjectiveDetailViewModel>(SelectedObjective.Id);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать задачу из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedObjective is null)
            {
                MessageBox.Show("Сначала необходимо выбрать задачу из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                _objectiveDataProvider.Remove(SelectedObjective.Id);
                Objectives.Remove(SelectedObjective);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _objectiveDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {
                
                var objectives = query.Where(p => p is { ResponsibleEmployee: not null, CurrentStatus: not null }
                                                && (p.ResponsibleEmployee.FirstName.Contains(SearchWord) 
                                                    || p.ResponsibleEmployee.LastName.Contains(SearchWord)
                                                    || p.Theme.Contains(SearchWord) 
                                                    || p.CurrentStatus.Name.Contains(SearchWord)
                                                ));
                InitializeObjectives(objectives);
            }
            else
            {
                InitializeObjectives(query);
            }
        }
    }
}
