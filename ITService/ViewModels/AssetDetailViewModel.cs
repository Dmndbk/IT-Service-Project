using ITService.Model;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using ITService.UI.Commands;
using ITService.UI.ViewModels.MainViewModels;

namespace ITService.UI.ViewModels
{
    class AssetDetailViewModel : ViewModel
    {
        private IAssetDataProvider _assetDataProvider;
        private ICompanyDataProvider _companyDataProvider;
        private IAssetTypeDataProvider _assetTypeDataProvider;
        private INavigationService _navigationService;
        private AssetWrapper _asset;
        public AssetDetailViewModel(
            IAssetDataProvider assetDataProvider, 
            ICompanyDataProvider companyDataProvider,
            IAssetTypeDataProvider assetTypeDataProvider,
            INavigationService navigationService)
        {
            _assetDataProvider = assetDataProvider;
            _companyDataProvider = companyDataProvider;
            _assetTypeDataProvider = assetTypeDataProvider;
            _navigationService = navigationService;

            NavigateToBackCommand = new DelegateCommand(execute: o => { Navigation.NavigateTo<AssetsViewModel>(0); }, canExecute: o => true);
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCanExecute);
        }
        public DelegateCommand NavigateToBackCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public ObservableCollection<CompanyWrapper> Companies { get; } = new(); 
        public ObservableCollection<AssetTypeWrapper> AssetTypes { get; } = new();

        public AssetWrapper Asset
        {
            get => _asset;
            set
            {
                _asset = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int assetId)
        {
            //id = assetId;

            var asset = assetId > 0
                ? await _assetDataProvider.GetById(assetId)
                : CreateNewAsset();
            InitializeAsset(asset);
            await LoadCompaniesAsync();
            await LoadTypesAsync();

            Asset.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Asset.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
        
        private void InitializeAsset(Asset asset)
        {
            Asset = new AssetWrapper(asset);
            if (Asset.Id == 0)
            {
                Asset.Name = "Имя";
            }
        }
        private Asset? CreateNewAsset()
        {
            var asset = new Asset();
            asset.Name = "имя";
            return asset;
        }

        private async Task LoadCompaniesAsync()
        {
            var companies = await _companyDataProvider.Get();
            Companies.Clear();
            foreach (var company in companies)
            {
                Companies.Add(new CompanyWrapper(company));
            }
        }
        private async Task LoadTypesAsync()
        {
            var types = await _assetTypeDataProvider.Get();
            AssetTypes.Clear();
            foreach (var type in types)
            {
                AssetTypes.Add(new AssetTypeWrapper(type));
            }
        }

        private bool OnSaveCanExecute(object? parameter)
        {
            return Asset != null && !Asset.HasErrors;
        }
        private void OnSaveCommand(object? parameter)
        {
            var asset = new Asset()
            {
                Id = Asset.Id,
                Name = Asset.Name,
                AssetTypeId = Asset.AssetTypeId,
                CompanyId = Asset.CompanyId,
                Description = Asset.Description,
                StartDate = Asset.StartDate,
                EndDate = Asset.EndDate,
                Price = Asset.Price,
                Location = Asset.Location,
                SerialNumber = Asset.SerialNumber
            };
            if (asset.Id > 0)
            {
                _assetDataProvider.Update(asset);
            }
            else
            {
                _assetDataProvider.Add(asset);
            }
            
        }
        public INavigationService Navigation
        {
            get => _navigationService;
            set => Set(ref _navigationService, value);
        }
    }
}
