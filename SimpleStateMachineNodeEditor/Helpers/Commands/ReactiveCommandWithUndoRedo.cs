using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public class ReactiveCommandWithUndoRedo<TParam, TResult> : ReactiveCommand<TParam, TResult>
    {
        //private readonly Func<TParam, TResult, TResult> _unExecute;
        //private readonly Func<TParam, TResult> _unExecute2;
        //public void UnExecute()
        //{
        //    _unExecute = _unExecute2;
        //    _unExecute2 = _unExecute;
        //    //Выполняем отмену команду
        //    this._unExecute(Parameters, Result);

        //    //Добавляем копию команды в стек команд, которые можно выполнить повторно
        //    CommandWithUndoRedo.AddInRedo(this.Clone() as CommandWithUndoRedo);
        //}

        ///// <summary>
        ///// Повторное выполнения команды
        ///// </summary>
        //public void Execute()
        //{
        //    //Выполянем команду
        //    this.Result = this._execute(this.Parameters, this.Result);

        //    //Добавляем копию команды в стек команд, которые можно отменить
        //    CommandWithUndoRedo.AddInUndo(this.Clone() as CommandWithUndoRedo);
        //}

        protected internal ReactiveCommandWithUndoRedo(Func<TParam, IObservable<TResult>> execute, IObservable<bool> canExecute, IScheduler outputScheduler):base(execute, canExecute, outputScheduler)
        {

        }

    }
}
