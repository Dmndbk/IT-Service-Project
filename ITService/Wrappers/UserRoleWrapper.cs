using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class UserRoleWrapper
    {
        private readonly UserRole _model;

        public UserRoleWrapper(UserRole model)
        {
            _model = model;
        }
        public int Id => _model.Id;
        public string Name => _model.Name;
    }
}
