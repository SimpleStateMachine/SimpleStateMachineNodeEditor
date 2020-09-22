using Avalonia;
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
    public partial class RightConnector
    {
        protected override void SetupBinding()
        {
            base.SetupBinding();
            this.WhenViewModelAnyValue(disposable =>
            {
                //this.TextBoxConnector.Events().PointerPressed.Subscribe(e => e.Handled = true).DisposeWith(disposable);
                //this.Events().PointerPressed.Subscribe(e =>
                //{

                //    if (Keyboard.IsKeyDown(Key.LeftAlt))
                //    {
                //        e.Handled = true;
                //    }

                //}).DisposeWith(disposable);

            });
         
        }
    }
}
