using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class CompanyWrapper
    {
        private readonly Company _model;

        public CompanyWrapper(Company model)
        {
            _model = model;
        }
        public int Id => _model.Id;
        public string Name => _model.Name;
    }
}
