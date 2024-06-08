using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ITService.DataAccess.Context;
using ITService.UI.Data;
using ITService.UI.Services;
using ITService.UI.Services.Dialogs;
using ITService.UI.ViewModels;
using ITService.UI.ViewModels.MainViewModels;
using ITService.UI.Views.MainViews;
using Microsoft.Extensions.DependencyInjection;

namespace ITService.UI
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(
                    CultureInfo.CurrentUICulture.IetfLanguageTag)));
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<ResolveViewModel>();  
            services.AddSingleton<StatisticsViewModel>();
            services.AddTransient<ResolveDialogViewModel>();
            services.AddTransient<ServiceDialogViewModel>();
            services.AddTransient<DepartmentDialogViewModel>();
            services.AddDbContext<ItServiceDb>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ClientViewModel>();
            services.AddTransient<ClientHomeViewModel>();
            services.AddTransient<AdminViewModel>();
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<AssetsViewModel>();
            services.AddTransient<ChangesViewModel>();
            services.AddTransient<ClientsViewModel>();
            services.AddTransient<EmployeesViewModel>();
            services.AddTransient<ObjectivesViewModel>();
            services.AddTransient<ProblemsViewModel>();
            services.AddTransient<RequestsViewModel>();
            services.AddTransient<AssetDetailViewModel>();
            services.AddTransient<ObjectiveDetailViewModel>();
            services.AddTransient<ProblemDetailViewModel>();
            services.AddScoped<RequestDetailViewModel>();
            services.AddTransient<ChangeDetailViewModel>();
            services.AddTransient<ClientDetailViewModel>();
            services.AddTransient<EmployeeDetailViewModel>();
            services.AddTransient<DepartmentsServicesViewModel>();
            services.AddTransient<IAssetDataProvider, AssetDataProvider>();
            services.AddTransient<IChangeDataProvider, ChangeDataProvider>();
            services.AddTransient<IClientDataProvider, ClientDataProvider>();
            services.AddTransient<IEmployeeDataProvider, EmployeeDataProvider>();
            services.AddTransient<IObjectiveDataProvider, ObjectiveDataProvider>();
            services.AddTransient<IProblemDataProvider, ProblemDataProvider>();
            services.AddTransient<IRequestDataProvider, RequestDataProvider>();
            services.AddTransient<ICompanyDataProvider, CompanyDataProvider>();
            services.AddTransient<IAssetTypeDataProvider, AssetTypeDataProvider>();
            services.AddTransient<IServiceDataProvider, ServiceDataProvider>();
            services.AddTransient<IDepartmentDataProvider, DepartmentDataProvider>();
            services.AddTransient<IInfluenceLevelDataProvider, InfluenceLevelDataProvider>();
            services.AddTransient<IStatusDataProvider, StatusDataProvider>();
            services.AddTransient<IUserDataProvider, UserDataProvider>();
            services.AddTransient<IUserRoleDataProvider, UserRoleDataProvider>();
            services.AddSingleton<IMessageBus, MessageBusService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var testWindow = _serviceProvider.GetRequiredService<AdminWindow>();
            //testWindow?.Show();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            //var empWindow = _serviceProvider.GetRequiredService<ExampleWindow>();
            //empWindow?.Show();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "Произошла непредвиденная ошибка. Пожалуйста сообщите администратору." + Environment.NewLine +
                e.Exception.Message, "Unexpected error");
            e.Handled = true;
        }
    }
}
