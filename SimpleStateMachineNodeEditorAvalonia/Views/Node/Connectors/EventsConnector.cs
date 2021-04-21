using SimpleStateMachineNodeEditorAvalonia.Helpers;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
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
                ListBoxConnectors.AddHandler(PointerPressedEvent, OnListBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
        }
        
        public void OnListBoxPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //If is not textbox - can be moved
            if (e.Source is not TextPresenter)
            {
                e.Handled = false;
            }
        }
    }
}
