using System.Collections.Generic;
using System.Dynamic;
using Avalonia;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas : BaseView<NodesCanvasViewModel>
    {
        public NodesCanvas()
        {
            InitializeComponent();
        } 
        public void StartDrag(PointerEventArgs e, Point startPosition)
        {
            DraggableConnect.DoDragDrop(e, ("StartPosition", startPosition));
        }
    }
}
