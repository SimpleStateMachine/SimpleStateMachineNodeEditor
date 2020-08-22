using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Media.TextFormatting;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    //public abstract class ReactiveCommandWithUndoRedoBase<TParam, TResult> : ReactiveCommandBase<TParam, TResult>, IReactiveCommandWithUndoRedo
    //{
    //    private EventHandler? _canUnExecuteChanged;
    //    private bool _canUnExecuteValue;

    //    event EventHandler CanUnExecuteChanged
    //    {
    //        add => _canUnExecuteChanged += value;
    //        remove => _canUnExecuteChanged -= value;
    //    }

    //    public abstract IObservable<bool> CanUnExecute
    //    {
    //        get;
    //    }

    //    public abstract IObservable<bool> IsUnExecuting
    //    {
    //        get;
    //    }

    //    protected void OnCanUnExecuteChanged(bool newValue)
    //    {
    //        _canUnExecuteValue = newValue;
    //        _canUnExecuteChanged?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    public abstract class ReactiveCommandWithUndoRedoBase<TParam, TResult> : ReactiveCommandBase<TParam, TResult>, IReactiveCommandWithUndoRedo
    {
        public ReactiveCommandWithUndoRedoBase(IReactiveCommandHistory history=null)
        {
            _history = history;
        }

        private IReactiveCommandHistory _history;
        public IObservable<bool> IsUndoing => throw new NotImplementedException();

        public IObservable<bool> IsRedoing => throw new NotImplementedException();

        public IObservable<bool> CanUndo => throw new NotImplementedException();

        public IObservable<bool> CanRedo => throw new NotImplementedException();

        public override IObservable<TResult> Execute(TParam parameter = default(TParam))
        {
            return null;
        }

        void IReactiveCommandWithUndoRedo.Undo()
        {
            IUndoExecute();
        }

        void IReactiveCommandWithUndoRedo.Redo()
        {
            IRedoExecute();
        }

        public abstract IObservable<TResult> Undo();
        public abstract IObservable<TResult> Redo();

        protected virtual void IUndoExecute()
        {
            Undo().Catch(Observable<TResult>.Empty)
                .Subscribe();
        }
        protected virtual void IRedoExecute()
        {
            Redo().Catch(Observable<TResult>.Empty)
                .Subscribe();
        }
    }
}
