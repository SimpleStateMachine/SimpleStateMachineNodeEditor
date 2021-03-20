using System.Reactive.Disposables;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Linq;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Remote.Protocol.Input;
using Avalonia.VisualTree;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class DraggableConnector
    {
        private Node NodeTo = null;
        string magnetClassName = "MagnetDragOver";
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                _canvas = this.FindAncestorOfType<NodesCanvas>();
                this.Events().PointerMoved.Subscribe(OnPointerMovedEvent).DisposeWith(disposable);
                this.Events().PointerReleased.Subscribe(OnPointerReleasedEvent).DisposeWith(disposable);
                AddHandler(DragDrop.DropEvent, OnNodesCanvasDragOver);
            });
        }
        void OnNodesCanvasDragOver(object handler, DragEventArgs e)
        {
            var t = 5;
            // e.GetDraggable().DragOver(e.GetPosition(this));
        }
        void OnPointerMovedEvent(PointerEventArgs e)
        {
            var point = e.GetPosition(_canvas);
            ViewModel.EndPoint = point - new Point(2,2);
            
            var node = FindNodeByInput(point);
            
            if (node is not null)
            {
                MagnetToNodeInput(node);
                AddClassToNodeInput(node, magnetClassName);
            }
            
            if (NodeTo != node)
            {
                RemoveClassFromNodeInput(NodeTo, magnetClassName);
                NodeTo = node;
            }
            
        }
        void OnPointerReleasedEvent(PointerEventArgs e)
        {
            var point = e.GetPosition(_canvas);
            // var node = FindNodeByInput(point);
        }

        Node FindNodeByInput(Point point)
        {
            var all = _canvas.GetInputElementsAt(point);
            var connector = all.Where(x => x is StyledElement styledElement &&
                                           styledElement.Name.Contains(nameof(Node.InputConnector)))
                .Select(x => x.FindAncestorOfType<Node>())
                .FirstOrDefault(x => x is not null);
            
            return connector;
        }

        void MagnetToNodeInput(Node node)
        {
            var connector = node.InputConnector;
            var ellipseConnector = connector.EllipseConnector;
            var positionConnectPoint = ellipseConnector.
                TranslatePoint(new Point(ellipseConnector.Width / 2, ellipseConnector.Height / 2), connector);
            var startPosition = connector.TranslatePoint(positionConnectPoint.Value, _canvas);
            ViewModel.EndPoint = startPosition.Value;
        }

        void AddClassToNodeInput(Node node, string className)
        { 
            node?.InputConnector.EllipseConnector.Classes.Add(className);
        }
        
        
        void RemoveClassFromNodeInput(Node node, string className)
        {
            node?.InputConnector.EllipseConnector.Classes.Remove(className);
        }
    }
}
