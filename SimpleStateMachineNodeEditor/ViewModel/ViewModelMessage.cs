using System;
using System.Windows;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Transformations;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMessage : ReactiveObject
    {
        [Reactive] public string Text { get; set; }
        public ViewModelMessage(string text)
        {
            Text = text;
        }
    }
}
