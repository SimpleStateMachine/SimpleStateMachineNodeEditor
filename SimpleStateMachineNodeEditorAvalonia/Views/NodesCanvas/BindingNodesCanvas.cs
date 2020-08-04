using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas
    {
        private void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Nodes, x => x.NodesNodesCanvas.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Connects, x => x.ConnectsNodesCanvas.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Selector, x => x.SelectorNodesCanvas.ViewModel).DisposeWith(disposable);
     
            });
        }

    }
}

//this.OneWayBind(this.ViewModel, x => x.Cutter, x => x.Cut).DisposeWith(disposable);
