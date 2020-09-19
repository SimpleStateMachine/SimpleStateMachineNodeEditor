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
    public partial class LeftConnectorViewModel : ConnectorViewModel
    {
        public LeftConnectorViewModel(NodeViewModel node, string name = "", bool isEnable = true) : base(node, name, isEnable)
        {

        } 
    }
}
