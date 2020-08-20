﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Nodes
    {
        public ItemsControl ItemsControlNodes;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ItemsControlNodes = this.FindControlWithExeption<ItemsControl>("ItemsControlNodes");
        }
    }
}