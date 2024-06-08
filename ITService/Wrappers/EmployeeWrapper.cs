using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class EmployeeWrapper : ModelWrapper<Employee>
    {
        public EmployeeWrapper(Employee model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string? DepartmentName => Model.Department?.Name;
        public string? FullName => Model.FirstName + " " + Model.LastName;

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? LastName
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }

        public string? Patronymic
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }

        public string? Email
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }

        public string? Position
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }

        public int? DepartmentId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
    }
}
