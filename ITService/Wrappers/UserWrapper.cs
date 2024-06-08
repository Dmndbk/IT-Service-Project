using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class UserWrapper : ModelWrapper<User>
    {
        public UserWrapper(User model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string Login
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public UserRole? UserRole
        {
            get => GetValue<UserRole?>();
            set => SetValue(value);
        }
        public int? UserRoleId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
