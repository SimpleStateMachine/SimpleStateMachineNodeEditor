using ReactiveUI;
using ReactiveUI.Fody.Helpers;


using System;
using System.Reactive.Linq;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial  class ViewModelCutter : ReactiveObject
    {
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public Point StartPoint { get; set;}
        [Reactive] public Point EndPoint { get; set; }      
        [Reactive] public double StrokeThickness { get; set; } = 1;

        public ViewModelCutter(ViewModelNodesCanvas nodesCanvas)
        {
            NodesCanvas = nodesCanvas;
            SetupCommands();
            SetupSubscriptions();
            
        }

        #region Setup Subscriptions
        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.NodesCanvas.Scale.Value).Subscribe(value => StrokeThickness = value);
        }

        #endregion Setup Subscriptions
    }
}
