using ReactiveUI;
using ReactiveUI.Fody.Helpers;


using System;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelCutter : ReactiveObject
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

        #region Setup Commands
        public ReactiveCommand<Point, Unit> CommandStartCut { get; set; }

        private void SetupCommands()
        {
            CommandStartCut = ReactiveCommand.Create<Point>(StartCut);
        }

        private void StartCut(Point point)
        {
            Visible = true;
            StartPoint = point;
        }

        #endregion Setup Commands
    }
}
