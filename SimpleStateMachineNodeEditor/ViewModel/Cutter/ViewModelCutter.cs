using ReactiveUI;
using ReactiveUI.Fody.Helpers;


using System;
using System.Reactive.Linq;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System.Reactive;
using SimpleStateMachineNodeEditor.ViewModel.NodesCanvas;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelCutter : ReactiveObject
    {
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public MyPoint StartPoint { get; set; } = new MyPoint();
        [Reactive] public MyPoint EndPoint { get; set; } = new MyPoint();      
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
        public ReactiveCommand<MyPoint, Unit> CommandStartCut { get; set; }

        private void SetupCommands()
        {
            CommandStartCut = ReactiveCommand.Create<MyPoint>(StartCut);
        }

        private void StartCut(MyPoint point)
        {
            Visible = true;
            StartPoint.Set(point);
        }

        #endregion Setup Commands
    }
}
