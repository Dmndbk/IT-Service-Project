using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class AssetWrapper : ModelWrapper<Asset>
    {
        public AssetWrapper(Asset model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string? Type => Model.AssetType?.Name;
        public string? CompanyName => Model.Company?.Name;
        public string? Name
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public string? Location
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public DateTime? StartDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }
        public DateTime? EndDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }
        public string? SerialNumber
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public decimal? Price
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }
        public string? Description
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }

        public int? CompanyId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public int? AssetTypeId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
    }
}
