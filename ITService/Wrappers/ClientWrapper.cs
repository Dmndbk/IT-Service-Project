using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class ClientWrapper : ModelWrapper<Client>
    {
        public ClientWrapper(Client model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string? CompanyName => Model.Company?.Name;
        public string? FullName => Model.FirstName + " " + Model.LastName;
        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Patronymic
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string? Email
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public int? CompanyId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
    }
}
