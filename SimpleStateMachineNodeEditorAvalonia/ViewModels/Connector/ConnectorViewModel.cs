using Avalonia;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class ConnectorViewModel : BaseViewModel
    {
        [Reactive] public StringEnabled Name { get; set; } 
        [Reactive] public Point Position { get; set; }
        [Reactive] public ConnectViewModel Connect { get; set; }

        public ConnectorViewModel(string name="")
        {
            Name = new StringEnabled(name);
        }
    }
}
