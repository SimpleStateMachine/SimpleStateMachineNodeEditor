using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

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

            Connects.Add(new ConnectViewModel() {StartPoint= new Point(10,10), EndPoint = new Point(500,500) });
        }
    }

}
