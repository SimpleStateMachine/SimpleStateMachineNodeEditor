﻿using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodeViewModel
    {
        public ReactiveCommand<SelectMode, Unit> SelectCommand;
        protected override void SetupCommands()
        {
            SelectCommand = ReactiveCommand.Create<SelectMode>(Select);
        }

        private void Select(SelectMode selectMode)
        {
            if (selectMode == SelectMode.ClickWithCtrl)
            {
                this.IsSelect = !this.IsSelect;
            }
            else if((selectMode == SelectMode.Click)&&(!IsSelect))
            {
                NodesCanvas.Nodes.SetIsSelectAllNodesCommand.ExecuteWithSubscribe((false, null));
                this.IsSelect = true;
            }

        }
    }
}
