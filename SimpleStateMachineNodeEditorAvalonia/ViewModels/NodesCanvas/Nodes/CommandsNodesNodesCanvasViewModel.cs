using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel
    {
        public ReactiveCommand<Point, Unit> MoveCommand { get; set; }
        public ReactiveCommand<(bool value, List<NodeViewModel> exludedNodes), Unit> SetIsSelectAllNodesCommand { get; set; }

        protected override void SetupCommands()
        {
            MoveCommand = ReactiveCommand.Create<Point>(Move);
            SetIsSelectAllNodesCommand = ReactiveCommand.Create<(bool value, List<NodeViewModel> exludedNodes)>(SelectAllNodes);
        }

        private void Move(Point point)
        {
            foreach (var node in SelectedNodes.Items)
            {
                node.Point1 = node.Point1 + point;
            }
        }

        private void SelectAllNodes((bool value, List<NodeViewModel> exludedNodes) parameter)
        {
            List<NodeViewModel> exludedNodes = parameter.exludedNodes ?? new List<NodeViewModel>();
            foreach (var node in SelectedNodes.Items.Except(exludedNodes))
            {
                node.IsSelect = parameter.value;
            }
        }
    }
}
