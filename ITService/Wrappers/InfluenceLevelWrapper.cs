using ITService.Model;

namespace ITService.UI.Wrappers
{
    public class InfluenceLevelWrapper
    {
        private readonly InfluenceLevel _model;

        public InfluenceLevelWrapper(InfluenceLevel model)
        {
            _model = model;
        }
        public int Id => _model.Id;
        public string Name => _model.Name;
    }
}
