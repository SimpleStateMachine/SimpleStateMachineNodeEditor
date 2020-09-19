using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class LeftConnector
    {
        protected override void SetupEvents()
        {
            base.SetupEvents();
            this.WhenViewModelAnyValue(disposable =>
            {
                AddHandler(DragDrop.DropEvent, OnConnectDrop);
            });

          
        }

        public void OnConnectDrop(object sender, DragEventArgs e)
        {

        }
    }
}
