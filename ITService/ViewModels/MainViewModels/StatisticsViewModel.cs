using ITService.UI.Commands;
using ITService.UI.Data;
using ITService.UI.Wrappers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ITService.UI.ViewModels.MainViewModels
{
    public class StatisticsViewModel : ViewModel
    {
        private IRequestDataProvider _requestDataProvider;
        private IObjectiveDataProvider _objectiveDataProvider;
        private IEmployeeDataProvider _employeeDataProvider;

        private int _allRequestsCount;
        private int _activeRequestsCount;
        private int _lateRequestsCount;

        private int _allObjectivesCount;
        private int _activeObjectivesCount;
        private int _lateObjectivesCount;
        private EmployeeWrapper _employee;
        private PlotModel _plotModel;
        public StatisticsViewModel(
            IRequestDataProvider requestDataProvider,
            IObjectiveDataProvider objectiveDataProvider,
            IEmployeeDataProvider employeeDataProvider)
        {
            _requestDataProvider = requestDataProvider;
            _objectiveDataProvider = objectiveDataProvider;
            _employeeDataProvider = employeeDataProvider;
        }
        public override async Task LoadAsync(int userId)
        {
            var employee = await _employeeDataProvider.GetByUserId(userId);
            Employee = new EmployeeWrapper(employee);

            var requests = await _requestDataProvider.GetAll();
            AllRequestsCount = requests.Count();
            ActiveRequestsCount = requests.Count(r => r.CurrentStatus.Name != "Закрыта");
            LateRequestsCount = requests.Count(r => r.CloseDate < DateTime.Now && r.CurrentStatus.Name != "Закрыта");

            var objectives = await _objectiveDataProvider.GetAll();
            AllObjectivesCount = objectives.Count();
            ActiveObjectivesCount = objectives.Count(r => r.CurrentStatus.Name != "Закрыта");
            LateObjectivesCount = objectives.Count(r => r.EndDate < DateTime.Now && r.CurrentStatus.Name != "Закрыта");

            await LoadChart();
        }

        private async Task LoadChart()
        {
            var requests = await _requestDataProvider.GetAll();
            double allRequests = requests.Count(r => r.AssessmentId != null);
            double requests_1 = requests.Count(r => r.AssessmentId == 1);
            double requests_2 = requests.Count(r => r.AssessmentId == 2);
            double requests_3 = requests.Count(r => r.AssessmentId == 3);
            double requests_4 = requests.Count(r => r.AssessmentId == 4);
            double requests_5 = requests.Count(r => r.AssessmentId == 5);

            _plotModel = new PlotModel { Title = "Оценки клиентов", TextColor = OxyColors.White};

            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>(new[]
                {
                    new BarItem{ Value = (requests_1 / allRequests * 100), Color = OxyColor.FromRgb(186, 24, 24)},
                    new BarItem{ Value = (requests_2 / allRequests * 100), Color = OxyColor.FromRgb(186, 56, 24)},
                    new BarItem{ Value = (requests_3 / allRequests * 100), Color = OxyColor.FromRgb(186, 124, 24)},
                    new BarItem{ Value = (requests_4 / allRequests * 100), Color = OxyColor.FromRgb(173, 186, 24)},
                    new BarItem{ Value = (requests_5 / allRequests * 100), Color = OxyColor.FromRgb(110, 186, 24)}
                }),
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:.00}%"
            };
            _plotModel.Series.Add(barSeries);
            _plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = new[]
                {
                    "Крайне плохо",
                    "Плохо",
                    "Удовлетворительно",
                    "Хорошо",
                    "Отлично"
                }
            });
        }
        public PlotModel PieChart
        {
            get => _plotModel;
            set => Set(ref _plotModel, value);
        }
        public DelegateCommand PrintCommand { get; set; }
        public EmployeeWrapper Employee
        {
            get => _employee;
            set => Set(ref _employee, value);
        }
        public int AllRequestsCount
        {
            get => _allRequestsCount;
            set => Set(ref _allRequestsCount, value);
        }
        public int ActiveRequestsCount
        {
            get => _activeRequestsCount;
            set => Set(ref _activeRequestsCount, value);
        }
        public int LateRequestsCount
        {
            get => _lateRequestsCount;
            set => Set(ref _lateRequestsCount, value);
        }
        public int AllObjectivesCount
        {
            get => _allObjectivesCount;
            set => Set(ref _allObjectivesCount, value);
        }
        public int ActiveObjectivesCount
        {
            get => _activeObjectivesCount;
            set => Set(ref _activeObjectivesCount, value);
        }
        public int LateObjectivesCount
        {
            get => _lateObjectivesCount;
            set => Set(ref _lateObjectivesCount, value);
        }
    }
}
