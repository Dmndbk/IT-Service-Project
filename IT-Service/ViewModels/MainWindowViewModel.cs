using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_Service.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _Title = "IT-Service";
        public string Title
        {
            get => _Title;
            //set
            //{
            //    //if(Equals(_Title, value)) return;
            //    //_Title = value;
            //    //OnPropertyChanged();

            //    Set(ref _Title, value)
            //}
            set => Set(ref _Title, value);
        }
    }
}
