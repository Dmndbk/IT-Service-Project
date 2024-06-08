using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class StatusWrapper
    {
        private readonly Status _model;

        public StatusWrapper(Status model)
        {
            _model = model;
        }
        public int Id => _model.Id;
        public string Name => _model.Name;
    }
}
