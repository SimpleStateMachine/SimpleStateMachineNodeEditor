using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas : BaseView<NodesCanvasViewModel>
    {
        public static NodesCanvas Current { get; private set; }
        public NodesCanvas()
        {
            InitializeComponent();
            Current = this;
        }
    }
}
