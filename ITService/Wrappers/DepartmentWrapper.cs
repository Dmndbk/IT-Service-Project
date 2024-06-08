using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class DepartmentWrapper : ModelWrapper<Department>
    {
        public DepartmentWrapper(Department model) : base(model)
        {

        }
        public int Id => Model.Id;
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Description
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
    }
}
