using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelDialog : ReactiveValidationObject<ViewModelDialog>
    {
        [Reactive] public bool? Visibility { get; set; }
        [Reactive] public DialogType Type { get; set; }
        [Reactive] public DialogResult Result { get; set; }
        [Reactive] public string Title { get; set; }


        [Reactive] public string MessageBoxText { get; set; }
        [Reactive] public MessageBoxButton MessageBoxButtons{ get; set; }


        [Reactive] public string FileDialogFilter { get; set; }
        [Reactive] public string FileName { get; set; }

        private void Show()
        {
            Visibility = true;
        }
        private void Clear()
        {
            Type = DialogType.noCorrect;
            Result = DialogResult.noCorrect;
            Title = "";
            MessageBoxText = "";
            FileDialogFilter = "";
            FileName = "";
        }
        public void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button)
        {
            Clear();
            MessageBoxText = messageBoxText;
            MessageBoxButtons = button;
            Type = DialogType.MessageBox;
            Title = caption;
            Show();
        }

        public void ShowOpenFileDialog(string filter, string fileName, string title)
        {
            Clear();
            FileDialogFilter = filter;
            FileName = fileName;
            Title = title;
            Type = DialogType.OpenFileDialog;
            Show();
        }

        public void ShowSaveFileDialog(string filter, string fileName, string title)
        {
            Clear();
            FileDialogFilter = filter;
            FileName = fileName;
            Title = title;
            Type = DialogType.SaveFileDialog;
            Show();
        }

    }
}
