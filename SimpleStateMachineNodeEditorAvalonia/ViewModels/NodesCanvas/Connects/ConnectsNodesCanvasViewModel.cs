using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectsNodesCanvasViewModel : BaseViewModel
    {
        public SourceList<ConnectViewModel> Connects = new SourceList<ConnectViewModel>();
        public ObservableCollectionExtended<ConnectViewModel> ConnectsForView = new ObservableCollectionExtended<ConnectViewModel>();
        [Reactive] public ConnectViewModel DraggedConnect { get; set; }
      
        public ConnectsNodesCanvasViewModel()
        {
            Connects.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(ConnectsForView).Subscribe();

            //Connects.Add(new ConnectViewModel() { StartPoint = new Point(10, 10), EndPoint = new Point(500, 500) });
        }

        public ConnectViewModel GetNewConnect(RightConnectorViewModel connectorFrom, Point point = default)
        {
            var newConnect = new ConnectViewModel(connectorFrom, point);
            Connects.Add(newConnect);

            return newConnect;
        }
    }

}
