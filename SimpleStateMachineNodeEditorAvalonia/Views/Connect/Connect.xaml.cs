using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect : BaseView<ConnectViewModel>
    {
        public Connect()
        {
            SetupSubscriptions();
        }
    }
}
