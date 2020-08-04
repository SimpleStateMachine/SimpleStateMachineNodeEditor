using Avalonia;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class SelectorViewModel : BaseViewModel
    {
        [Reactive] public Size Size { get; set; }
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }
    }
}
