using Avalonia;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class CutterViewModel : BaseViewModel
    {
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        [Reactive] public double StrokeThickness { get; set; } = 1;
    }
}
