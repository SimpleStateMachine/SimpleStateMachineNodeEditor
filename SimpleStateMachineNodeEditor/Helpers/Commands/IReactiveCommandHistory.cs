using ReactiveUI;
using System;
using System.Reactive;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public interface IReactiveCommandHistory
    {
        IObservable<bool> CanUndo { get; }
        IObservable<bool> IsUndoing { get; }

        IObservable<bool> CanRedo { get; }
        IObservable<bool> IsRedoing { get; }

        IObservable<bool> CanClear { get; }
        IObservable<bool> IsClearing { get; }

        ReactiveCommand<Unit, Unit> Undo { get; }
        ReactiveCommand<Unit, Unit> Redo { get; }
        ReactiveCommand<Unit, Unit> Clear { get; }

        IReactiveCommandWithUndoRedo AddInRedo(IReactiveCommandWithUndoRedo command);
        IReactiveCommandWithUndoRedo AddInUndo(IReactiveCommandWithUndoRedo command);
    }
}
