using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Selector : BaseView<SelectorViewModel>
    {
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public Selector()
        {

            InitializeComponent();
        }
    }
}
