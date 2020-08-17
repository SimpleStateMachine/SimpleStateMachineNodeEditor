using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Disposables;
using System.Reactive.Linq;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connector: BaseView<ConnectorViewModel>
    {
        public Connector()
        {
            SetupEvents();
        }
        protected virtual void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                //this.TextBoxConnector.Events().

                //this.EllipseElement.Events().MouseLeftButtonDown.Subscribe(e => ConnectDrag(e)).DisposeWith(disposable);
                //this.TextBoxElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                //this.GridElement.Events().PreviewMouseLeftButtonDown.Subscribe(e => ConnectorDrag(e)).DisposeWith(disposable);
                //this.GridElement.Events().PreviewDragEnter.Subscribe(e => ConnectorDragEnter(e)).DisposeWith(disposable);
                //this.GridElement.Events().PreviewDrop.Subscribe(e => ConnectorDrop(e)).DisposeWith(disposable);
            });
        }


    }
}
