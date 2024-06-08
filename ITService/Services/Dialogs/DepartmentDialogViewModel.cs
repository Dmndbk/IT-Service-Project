using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Wrappers;

namespace ITService.UI.Services.Dialogs
{
    public class DepartmentDialogViewModel : DialogViewModelBase<DepartmentWrapper>
    {
        private DepartmentWrapper _department;
        public DepartmentDialogViewModel(string title, string message, DepartmentWrapper department) : base(title, message)
        {
            DepartmentWrapper = department;
            SaveCommand = new DelegateCommand(OnSave);
            if (DepartmentWrapper.Id == 0)
            {
                DepartmentWrapper.Name = "";
                DepartmentWrapper.Description = "";
            }
        }

        public DepartmentWrapper DepartmentWrapper
        {
            get => _department;
            set => Set(ref _department, value);
        }
        public DelegateCommand SaveCommand { get; set; }
        private void OnSave(object? parameter)
        {
            CloseDialogWithResult(parameter as IDialogWindow, DepartmentWrapper);
        }
    }
}
