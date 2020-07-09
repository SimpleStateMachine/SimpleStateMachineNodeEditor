using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public class Selector : BaseView
    {
        public Selector()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
