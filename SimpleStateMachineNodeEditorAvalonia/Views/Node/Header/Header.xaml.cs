using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header : ReactiveUserControl<HeaderNodeViewModel>
    {
        public Header()
        {
            this.InitializeComponent();
        }
    }
}
