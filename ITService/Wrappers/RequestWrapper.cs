using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class RequestWrapper : ModelWrapper<Request>
    {
        public RequestWrapper(Request model) : base(model)
        {

        }
        public int Id => Model.Id;
        public string? ClientName => Model.RelatedClient?.FirstName + " " + Model.RelatedClient?.LastName;
        public string? EmployeeName => Model.RelatedEmployee?.FirstName + " " + Model.RelatedEmployee?.LastName;
        public string? AssetName => Model.RelatedAsset?.Name;
        public string? Status => Model.CurrentStatus?.Name;
        public string? Service => Model.Service?.Name;


        public string? Theme
        {
            get => GetValue<string?>();
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
        public DateTime CreationDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }
        public DateTime? CloseDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }
        public int? RelatedClientId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? RelatedEmployeeId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? RelatedAssetId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? CurrentStatusId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? ServiceId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public int? AssessmentId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public string? Comment
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
    }
}
