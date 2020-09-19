using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Nodes
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
            });
        }
   
    }
}
