namespace ITService.UI.Wrappers
{
    public class AssessmetsWrapper : ModelWrapper<AssessmetsWrapper>
    {
        public AssessmetsWrapper(AssessmetsWrapper model) : base(model)
        {
        }

        public int Id => Model.Id;
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public int Mark
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}
