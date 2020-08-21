using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public abstract class ReactiveCommandWithUndoRedoBase<TParam, TResult> : ReactiveCommandBase<TParam, TResult>
    {
        private EventHandler? _canUnExecuteChanged;
        private bool _canUnExecuteValue;

        event EventHandler CanUnExecuteChanged
        {
            add => _canUnExecuteChanged += value;
            remove => _canUnExecuteChanged -= value;
        }

        public abstract IObservable<bool> CanUnExecute
        {
            get;
        }

        public abstract IObservable<bool> IsUnExecuting
        {
            get;
        }

        protected void OnCanUnExecuteChanged(bool newValue)
        {
            _canUnExecuteValue = newValue;
            _canUnExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
