using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

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
            Name = new StringWithEnable(name);

            node.WhenAnyValue(n => n.Point1).Buffer(2, 1).Select(value => value[1] - value[0]).Subscribe(x => Position = Position + x);
        }
    }
}
