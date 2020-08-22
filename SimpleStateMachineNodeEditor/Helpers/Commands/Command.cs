using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public class Command<TParameter, TResult> : ICommandWithUndoRedo, ICommand, ICloneable
    {
        private readonly Func<TParameter, TResult, TResult> _execute;
        private readonly Func<TParameter, TResult, TResult> _unExecute;
        public Action OnExecute { get; set; }
        public TParameter Parameters { get; set; }
        public TResult Result { get; set; }


        public object Clone()
        {
            return new Command<TParameter, TResult>(_execute, _unExecute, OnExecute)
            {
                Parameters = this.Parameters,
                Result = this.Result
            };
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = default)
        {
            Parameters = parameter.Cast<TParameter>();

            Result = this._execute(Parameters, Result).Cast<TResult>();

            ICommandWithUndoRedo.AddInUndo(this.Clone() as ICommandWithUndoRedo);

            ICommandWithUndoRedo.StackRedo.Clear();

            Result = default(TResult);

            Parameters = default(TParameter);

            OnExecute?.Invoke();
        }

        void ICommandWithUndoRedo.ExecuteWithSubscribe()
        {
            this.Result = this._execute(this.Parameters, this.Result);

            ICommandWithUndoRedo.AddInUndo(this.Clone() as ICommandWithUndoRedo);
        }

        void ICommandWithUndoRedo.UnExecute()
        {
            this._unExecute(Parameters, Result);

            ICommandWithUndoRedo.AddInRedo(this.Clone() as ICommandWithUndoRedo);
        }

        public Command(Func<TParameter, TResult, TResult> ExecuteWithSubscribe, Func<TParameter, TResult, TResult> unExecute, Action onExecute = null)
        {
            _execute = ExecuteWithSubscribe;

            _unExecute = unExecute;

            OnExecute += onExecute;
        }
    }
}
