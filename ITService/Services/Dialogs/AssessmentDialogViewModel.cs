using ITService.UI.Commands;

namespace ITService.UI.Services.Dialogs
{
    class AssessmentDialogViewModel : DialogViewModelBase<DialogItem>
    {
        private string _comment;
        private int _id;

        public AssessmentDialogViewModel(string title, string message) : base(title, message)
        {
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            Id = 4;
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand SaveCommand { get; set; }
        private bool CanSave(object? parameter) => Comment != "";
        private void OnSave(object? parameter)
        {
            DialogItem item = new DialogItem()
            {
                Id = Id,
                Comment = Comment
            };
            CloseDialogWithResult(parameter as IDialogWindow, item);
        }
    }
}
