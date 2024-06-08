using ITService.Model;
using ITService.UI.Commands;
using ITService.UI.Wrappers;
using System.Collections.ObjectModel;
using ITService.UI.Data;

namespace ITService.UI.Services.Dialogs
{
    public class ServiceDialogViewModel : DialogViewModelBase<ServiceWrapper>
    {
        private ServiceWrapper _service;
        public ServiceDialogViewModel(string title, string message,
            ServiceWrapper service,
            ObservableCollection<DepartmentWrapper> departments) : base(title, message)
        {
            Service = service;
            Departments = departments;
            if (Service.Id == 0)
            {
                Service.Name = "";
                Service.DepartmentId = 1;
            }
            
            SaveCommand = new DelegateCommand(OnSave);
        }
        public ObservableCollection<DepartmentWrapper> Departments { get; } = new();

        public ServiceWrapper Service
        {
            get => _service;
            set => Set(ref _service, value);
        }
        public DelegateCommand SaveCommand { get; set; }
        private void OnSave(object? parameter)
        {
            CloseDialogWithResult(parameter as IDialogWindow, Service);
        }
    }
}
