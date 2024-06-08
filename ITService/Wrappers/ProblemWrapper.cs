using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class ProblemWrapper : ModelWrapper<Problem>
    {
        public ProblemWrapper(Problem model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string? InfluenceLevelName => Model.CurrentInfluenceLevel?.Name;
        public string? Status => Model.CurrentStatus?.Name;
        public string Theme
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? SolutionDescription
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public DateTime EndDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int? CurrentInfluenceLevelId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public int? CurrentStatusId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
    }
}
