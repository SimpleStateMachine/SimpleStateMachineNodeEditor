using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class CutterViewModel
    {
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
    }
}
