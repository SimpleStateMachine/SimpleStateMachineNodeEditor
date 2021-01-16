using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Connectors
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.ListBoxConnectors.AddHandler(ListBox.PointerPressedEvent, OnListBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
        }
        
        public void OnListBoxPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //for select connector on ListBox click
            if (e.Source is RightConnector)
            {
                e.Handled = false;
            }
            else if (e.Source is TextBox)
            {
                e.Handled = true;
            }
            

        }
    }
}
