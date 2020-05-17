using System;
using System.Windows;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Logging;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMessage : ReactiveObject
    {
        public TypeMessage TypeMessage { get; set; }
        [Reactive] public string Text { get; set; }
        public ViewModelMessage(TypeMessage typeMessage, string text)
        {
            TypeMessage = typeMessage;
            Text = text;
         
        }
    }
}
