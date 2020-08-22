using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface IReactiveCommandWithUndoRedo:IReactiveCommand
    {
        IObservable<bool> IsUndoing { get; }
        IObservable<bool> IsRedoing { get; }

        IObservable<bool> CanUndo { get; }
        IObservable<bool> CanRedo { get; }

        void Undo();
        void Redo();
    }
}
