using ITService.UI.Views.MainViews;
using ITService.UI.Wrappers;

namespace ITService.UI.Services
{
    public interface IDialogService
    {
        T OpenDialog<T>(DialogViewModelBase<T> viewModel);
    }
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> viewModel)
        {
            IDialogWindow window = new DialogWindow();
            window.DataContext = viewModel;
            window.ShowDialog();
            return viewModel.DialogResultData;
        }
    }   
    public interface IDialogWindow
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }
        bool? ShowDialog();
    }
    public abstract class DialogViewModelBase<T> : NotifyDataErrorInfoBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResultData { get; set; }

        public DialogViewModelBase() : this(string.Empty, string.Empty) { }
        public DialogViewModelBase(string title) : this(title, string.Empty) { }

        public DialogViewModelBase(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public void CloseDialogWithResult(IDialogWindow dialogWindow, T result)
        {
            DialogResultData = result;
            if (dialogWindow != null)
                dialogWindow.DialogResult = true;
        }
    }
}
