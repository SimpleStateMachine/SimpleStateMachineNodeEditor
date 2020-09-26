using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectorViewModel : BaseViewModel
    {
        [Reactive] public NodeViewModel Node { get; set; }
        [Reactive] public StringWithEnable Name { get; set; } 
        [Reactive] public Point Position { get; set; }
        public ConnectorViewModel(NodeViewModel node, string name="", bool isEnable = true )
        {
            Node = node;
            Name = new StringWithEnable(name, isEnable);   
        }
    }
}
