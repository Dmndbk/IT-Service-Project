using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class ServiceWrapper : ModelWrapper<Service>
    {
        public ServiceWrapper(Service model) : base(model)
        {

        }
        public int Id => Model.Id;
        public string? DepartmentName => Model.Department?.Name;
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int DepartmentId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}
