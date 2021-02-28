using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using SimpleStateMachineNodeEditorAvalonia.Helpers;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connector<TViewModel> : BaseView<TViewModel>
    where TViewModel: ConnectorViewModel
    {
        
        private Grid grid;
        private TextBox textBox;
        private Ellipse ellipse;

        protected void InitializeComponent()
        {
            grid = this.FindControlWithExeption<Grid>("GridConnector");
            textBox = this.FindControlWithExeption<TextBox>("TextBoxConnector");
            ellipse = this.FindControlWithExeption<Ellipse>("EllipseConnector");
        }

    }
}
