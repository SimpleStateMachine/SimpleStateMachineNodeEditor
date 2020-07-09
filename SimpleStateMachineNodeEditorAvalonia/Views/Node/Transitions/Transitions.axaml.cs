using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesElements
{
    public class Transitions : UserControl
    {
        public Transitions()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
