using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel
    {
        public ReactiveCommand<Point, Unit> MoveCommand { get; set; }

        protected override void SetupCommands()
        {
            MoveCommand = ReactiveCommand.Create<Point>(Move);
        }

        private void Move(Point point)
        {
            foreach (var node in SelectedNodes.Items)
            {
                node.Point1 = node.Point1 + point;
            }
        }
    }
}
