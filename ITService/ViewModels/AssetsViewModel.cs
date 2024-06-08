using System.Collections.ObjectModel;
using System.Windows;
using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels
{
    public sealed class AssetsViewModel : ViewModel
    {
        private IAssetDataProvider _assetDataProvider;
        private INavigationService _navigationService;
        private AssetWrapper _selectedAsset;
        private string _searchWord;

        public AssetsViewModel(IAssetDataProvider assetDataProvider, INavigationService navigationService)
        {
            _assetDataProvider = assetDataProvider;
            _navigationService = navigationService;
            NavigateToDetailCommand = new DelegateCommand(OnNavigation, canExecute: o => true);
            NavigateToCreateCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<AssetDetailViewModel>(0); }, canExecute: o => true);
            DeleteCommand = new LambdaCommand(OnDelete);
            SearchCommand = new LambdaCommand(OnSearch);
        }

        public LambdaCommand DeleteCommand { get; set; }
        public LambdaCommand SearchCommand { get; set; }
        public DelegateCommand NavigateToDetailCommand { get; set; }
        public DelegateCommand NavigateToCreateCommand { get; set; }
        public ObservableCollection<AssetWrapper> Assets { get; } = new();

        public override async Task LoadAsync(int Id)
        {
            var assets = await _assetDataProvider.GetAll();
            InitializeAssets(assets);
        }

        private void InitializeAssets(IEnumerable<Asset> assets)
        {
            Assets.Clear();
            foreach (var asset in assets)
            {
                Assets.Add(new AssetWrapper(asset));
            }
        }

        public string SearchWord
        {
            get => _searchWord;
            set => Set(ref _searchWord, value);
        }
        public AssetWrapper SelectedAsset
        {
            get => _selectedAsset;
            set => Set(ref _selectedAsset, value);
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }

        private void OnNavigation(object? parameter)
        {
            if (SelectedAsset != null)
            {
                Navigation.NavigateTo<AssetDetailViewModel>(SelectedAsset.Id);
            }
            else
            {
                MessageBox.Show("Выберите актив для изменения", "Внимание", MessageBoxButton.OK);
            }
        }
        private void OnDelete(object? parameter)
        {
            if (SelectedAsset is null)
            {
                MessageBox.Show("Сначала необходимо выбрать актив из списка", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                _assetDataProvider.Remove(SelectedAsset.Id);
                Assets.Remove(SelectedAsset);
            }
            
        }
        private async void OnSearch(object? parameter)
        {
            var query = await _assetDataProvider.GetAll();
            if (!string.IsNullOrEmpty(SearchWord))
            {

                var assets = query.Where(p => p is { Company: not null, AssetType: not null }
                                                  && (p.Company.Name.Contains(SearchWord)
                                                      || p.AssetType.Name.Contains(SearchWord)
                                                      || p.Name.Contains(SearchWord)
                                                      || p.Location.Contains(SearchWord)
                                                      || p.SerialNumber.Contains(SearchWord)
                                                      || p.Price.ToString().Contains(SearchWord)
                                                  ));
                InitializeAssets(assets);
            }
            else
            {
                InitializeAssets(query);
            }
        }
    }
}
