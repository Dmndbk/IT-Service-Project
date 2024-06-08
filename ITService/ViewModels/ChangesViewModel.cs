using ITService.UI.Data;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using System.Windows;
using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Services;

namespace ITService.UI.ViewModels
{
    class ChangesViewModel : ViewModel
    {
        private IChangeDataProvider _changeDataProvider;
        private INavigationService _navigationService;
        private ChangeWrapper _selectedChange;
        private string _searchWord;

        public ChangesViewModel(IChangeDataProvider changeDataProvider, INavigationService navigationService)
        {
            _changeDataProvider = changeDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<ChangeDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
        }
        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<ChangeWrapper> Changes { get; } = new();

        public override async Task LoadAsync(int Id)
        {
            var changes = await _changeDataProvider.GetAll();
            InitializeChanges(changes);
        }

        private void InitializeChanges(IEnumerable<Change>? changes)
        {
            Changes.Clear();
            foreach (var change in changes)
            {
                Changes.Add(new ChangeWrapper(change));
            }
        }

        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public ChangeWrapper SelectedChange
        {
            get => _selectedChange;
            set => Set(ref _selectedChange, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedChange != null)
            {
                Navigation.NavigateTo<ChangeDetailViewModel>(SelectedChange.Id);
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать изменение из списка", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedChange is null)
            {
                MessageBox.Show("Сначала необходимо выбрать изменение из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                _changeDataProvider.Remove(SelectedChange.Id);
                Changes.Remove(SelectedChange);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _changeDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {

                var changes = query.Where(p => p is { RelatedEmployee: not null, RelatedService: not null, CurrentStatus: not null }
                                               && (p.Theme.Contains(SearchWord)
                                                   || p.RelatedEmployee.FirstName.Contains(SearchWord)
                                                   || p.RelatedEmployee.LastName.Contains(SearchWord)
                                                   || p.RelatedService.Name.Contains(SearchWord)
                                                   || p.CurrentStatus.Name.Contains(SearchWord)
                                               ));
                InitializeChanges(changes);
            }
            else
            {
                InitializeChanges(query);
            }
        }
    }
}
