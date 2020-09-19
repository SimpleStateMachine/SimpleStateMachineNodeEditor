using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Connects
    {
        protected override void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {

            });
        }
    }
}
