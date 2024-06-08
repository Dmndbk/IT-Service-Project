using ITService.UI.Commands;

namespace ITService.UI.Services.Dialogs
{
    class ResolveDialogViewModel : DialogViewModelBase<string>
    {
        private string _resolveDescription;

        public ResolveDialogViewModel(string title, string message) : base(title, message)
        {
            SaveCommand = new DelegateCommand(OnSave);
        }
        public string ResolveDescription
        {
            get => _resolveDescription;
            set
            {
                _resolveDescription = value; 
                OnPropertyChanged();
                if (string.IsNullOrEmpty(_resolveDescription))
                {
                    AddError(_resolveDescription, "нельзя пустое значение");
                }
            }
        }
        public DelegateCommand SaveCommand { get; set; }
        private void OnSave(object? parameter)
        {
            string text = ResolveDescription;
            CloseDialogWithResult(parameter as IDialogWindow, text);
        }

    }
}
