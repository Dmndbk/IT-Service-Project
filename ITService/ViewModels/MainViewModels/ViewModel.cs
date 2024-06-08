
using ITService.UI.Wrappers;

namespace ITService.UI.ViewModels.MainViewModels
{
    public abstract class ViewModel : ObservableObject
    {
        private bool isEmployee;
        private bool isClient;
        public bool IsEmployee
        {
            get => isEmployee;
            set => Set(ref isEmployee, value);
        }
        public bool IsClient
        {
            get => isClient;
            set => Set(ref isClient, value);
        }

        private UserWrapper _user;
        public UserWrapper User
        {
            get => _user;
            set => Set(ref _user, value);
        }
    }
}
